using System.Collections.Specialized;
using System.Net;
using System.Web;

namespace tiny_key_value_server;

public static class DefaultEndpoint
{
    /* Endpoint for /.  */
    public static Res MyDefaultEndpoint(HttpListenerContext context, Dictionary<string, string>? cache = null)
    {
        Console.WriteLine("HELLO, WORLD! FROM /hello");
        string responseString = "{'code': 200, 'endpoint': '/'}";
        ContextHelper(context, responseString, "Accept", "application/json", 200);
        return new Res(0, 200, "200");
    }

    /* Endpoint for 404.  */
    public static Res NotFound(HttpListenerContext context, Dictionary<string, string>? cache = null)
    {
        Console.WriteLine("Not found");
        string responseString = "{'code': 404, 'endpoint': '/notfound'}";
        ContextHelper(context, responseString, "Accept", "application/json", 404);
        return new Res(0, 404, "404");
    }

    /* Endpoint for getting key:values from the internal cache.  */
    public static Res Get(HttpListenerContext context, Dictionary<string, string>? cache = null)
    {
        if (cache == null)
        {
            ContextHelper(context, "{'code': 500, 'endpoint': '/get'}", "Accept", "application/json", 200);
            return new Res(0, 500, "500");
        }

        NameValueCollection paramsCollection;
        if(context.Request.Url.Query != null)
        {
            paramsCollection = HttpUtility.ParseQueryString(context.Request.Url.Query);

            string keyName = paramsCollection["key"];

            string result = "";

            if (keyName != null)
            {
                lock (cache)
                {
                    if (cache.TryGetValue(keyName, out result))
                    {
#if DEBUG
                        Console.WriteLine(result);
#endif
                    }
                    else
                    {
                        result = "";
                    }
                }
            }

            Console.WriteLine("Getter");
            string responseString = "";
            if (result != "")
            {
                responseString = $"{{'code': 200, 'endpoint': '/get', 'result': '{result}'}}";
                ContextHelper(context, responseString, "Accept", "application/json", 200);
                return new Res(0, 200, "200");
            }
            else
            {
                responseString = $"{{'code': 400, 'endpoint': '/get', 'result': ''}}";
                ContextHelper(context, responseString, "Accept", "application/json", 400);
                return new Res(0, 400, "400");
            }
        } else
        {
            ContextHelper(context, $"{{'code': 400, 'endpoint': '/get', 'result': ''}}", "Accept", "application/json", 400);
            return new Res(0, 400, "400");
        }
    }

    /* Endpoint for setting key:values in the internal cache.  */
    public static Res Set(HttpListenerContext context, Dictionary<string, string>? cache = null)
    {
        Console.WriteLine("Setter");

        if(cache == null)
        {
            ContextHelper(context, "{'code': 500, 'endpoint': '/set'}", "Accept", "application/json", 200);
            return new Res(0, 500, "500");
        }

        lock(cache)
        {
            try
            {
                cache.Add("set-test", "setter test");
            }
            catch (ArgumentException)
            {
                cache["set-test"] = "setter test";
            }
        }
        
        string responseString = "{'code': 200, 'endpoint': 'set'}";

        ContextHelper(context, responseString, "Accept", "application/json", 200);
        return new Res(0, 200, "200");
    }

    /* Endpoint for tests, this will be deleted in the future.  */
    public static Res Hello(HttpListenerContext context, Dictionary<string, string>? cache = null)
    {
        Console.WriteLine("HELLO, WORLD! FROM /hello");
        ContextHelper(context, "{'code': 500, 'endpoint': '/hello'}", 200);
        return new Res(0, 200, "200");
    }

    public static Res NotImplemented(HttpListenerContext context, Dictionary<string, string>? cache = null)
    private static void ContextHelper(HttpListenerContext context, string message, int statusCode)
    {
        byte[] buffer = System.Text.Encoding.UTF8.GetBytes(message);
        context.Response.ContentLength64 = buffer.Length;
        context.Response.StatusCode = statusCode;
        context.Response.ContentLength64 = buffer.Length;
        System.IO.Stream output = context.Response.OutputStream;
        output.Write(buffer, 0, buffer.Length);
        output.Close();
    }

    private static void ContextHelper(HttpListenerContext context, string message, string headerName, string header, int statusCode)
    {
        byte[] buffer = System.Text.Encoding.UTF8.GetBytes(message);
        context.Response.ContentLength64 = buffer.Length;
        context.Response.StatusCode = statusCode;
        context.Response.AddHeader(headerName, header);
        context.Response.ContentLength64 = buffer.Length;
        System.IO.Stream output = context.Response.OutputStream;
        output.Write(buffer, 0, buffer.Length);
        output.Close();
    }

}
