﻿using System.Net;

namespace tiny_key_value_server;

public record class Res
{
    public int code;
    public string message;

    public Res(int code, string message)
    {
        this.code = code;
        this.message = message;
    }

}

class TinyKeyValueServer : IDisposable
{
    private readonly HttpListener _listener;
    private readonly Thread _listenerThread;
    private readonly Thread _worker;
    private readonly ManualResetEvent _stop, _ready;
    private Queue<HttpListenerContext> _queue;

    private delegate TResult Func<in T, out TResult>(T arg);
    private Dictionary<string, Func<HttpListenerContext, Dictionary<string, string>, Res>> jobs;

    private Dictionary<string, string> cache;

    public TinyKeyValueServer(int maxThreads)
    {
        _queue = new Queue<HttpListenerContext>();
        _stop = new ManualResetEvent(false);
        _ready = new ManualResetEvent(false);
        _listener = new HttpListener();
        _listenerThread = new Thread(HandleRequests);
        _worker = new Thread(Worker);

        cache = new Dictionary<string, string>();

        jobs
        = new Dictionary<string, Func<HttpListenerContext, Dictionary<string, string>, Res>> {
            {
                "/hello", DefaultEndpoint.Hello
            },
            { 
                "/", DefaultEndpoint.MyDefaultEndpoint
            },
            {
                "/get", DefaultEndpoint.Get
            },
            {
                "/set", DefaultEndpoint.Set
            }

        };
    }

    private void ErrorEndpoint()
    {

    }

    public void Start()
    {
        _listener.Prefixes.Add(String.Format(@"http://localhost:8888/"));
        _listener.Start();
        _listenerThread.Start();
        _worker.Start();
    }

    public void Dispose()
    { Stop(); }

    public void Stop()
    {
        _stop.Set();
        _listenerThread.Join();
        _worker.Join();
        _listener.Stop();
    }

    private void HandleRequests()
    {
        while (_listener.IsListening)
        {
            var context = _listener.BeginGetContext(ContextReady, null);

            if (0 == WaitHandle.WaitAny(new[] { _stop, context.AsyncWaitHandle }))
                return;
        }
    }

    private void ContextReady(IAsyncResult ar)
    {
        try
        {
            lock (_queue)
            {
                _queue.Enqueue(_listener.EndGetContext(ar));
                _ready.Set();
            }
        }
        catch { return; }
    }

    private void Worker()
    {
        WaitHandle[] wait = new[] { _ready, _stop };
        while (0 == WaitHandle.WaitAny(wait))
        {
            HttpListenerContext context;
            lock (_queue)
            {
                if (_queue.Count > 0)
                    context = _queue.Dequeue();
                else
                {
                    _ready.Reset();
                    continue;
                }
            }

            try
            {
                Console.WriteLine(context.Request.Url.ToString());
                Console.WriteLine("URL: {0}", context.Request.Url.OriginalString);
                Console.WriteLine("Raw URL: {0}", context.Request.RawUrl);
                Console.WriteLine("Query: {0}", context.Request.QueryString);
                Console.WriteLine("Url fragment: {0}", context.Request.Url.Fragment);

                // https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Accept
                // Accept: application/json

                string url = context.Request.RawUrl ?? "";

                if (url != "")
                {
                    Func<HttpListenerContext, Dictionary<string, string>, Res > value = null;
                    //if (jobs.TryGetValue("/hello", out value))
                    if (jobs.TryGetValue(url, out value))
                    {
                        Res r = value(context, cache);
                        Console.WriteLine(r.message);
                    }
                    else
                    {
                        Res s = DefaultEndpoint.NotFound(context);
                        if(s.code != 0)
                        {
                            Console.WriteLine("Internal error");
                        } else
                        {
                            Console.WriteLine(s.message);
                        }
                    }
                }

            }
            catch (Exception e) { Console.Error.WriteLine(e); }
        }
    }
}
