using System.Net;
using System.Web;

namespace tiny_key_value_server;

public static class DefaultEndpoint
{
    public static Res MyDefaultEndpoint(HttpListenerContext context, Dictionary<string, string> cache = null)
    {
        Console.WriteLine("HELLO, WORLD! FROM /hello");
        string responseString = "<HTML><BODY>ROOT</BODY></HTML>";
        byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
        context.Response.ContentLength64 = buffer.Length;
        System.IO.Stream output = context.Response.OutputStream;
        output.Write(buffer, 0, buffer.Length);
        output.Close();
        return new Res(0, "200");
    }

    public static Res NotFound(HttpListenerContext context, Dictionary<string, string> cache = null)
    {
        Console.WriteLine("Not found");
        string responseString = "{'code': 404}";
        byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
        context.Response.ContentLength64 = buffer.Length;
        context.Response.AddHeader("Accept", "application/json");
        context.Response.StatusCode = 404;
        System.IO.Stream output = context.Response.OutputStream;
        output.Write(buffer, 0, buffer.Length);
        output.Close();
        return new Res(0, "All good boss!!");
    }

    public static Res Get(HttpListenerContext context, Dictionary<string, string> cache = null)
    {
        var paramsCollection = HttpUtility.ParseQueryString(context.Request.Url.Query);

        string result = "";
        lock (cache)
        {
            if (cache.TryGetValue("set-test", out result))
            {
                Console.WriteLine(result);
            }
            else
            {
                result = "can't find nada :((";
            }
        }

        Console.WriteLine("Getter");
        string responseString = $"{{'code': 200, 'endpoint': 'get', 'result': '{result}'}}";
        byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
        context.Response.ContentLength64 = buffer.Length;
        context.Response.AddHeader("Accept", "application/json");
        context.Response.StatusCode = 200;
        System.IO.Stream output = context.Response.OutputStream;
        output.Write(buffer, 0, buffer.Length);
        output.Close();
        return new Res(0, "200");
    }

    public static Res Set(HttpListenerContext context, Dictionary<string, string> cache = null)
    {
        Console.WriteLine("Setter");

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
        byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
        context.Response.ContentLength64 = buffer.Length;
        context.Response.AddHeader("Accept", "application/json");
        context.Response.StatusCode = 200;
        System.IO.Stream output = context.Response.OutputStream;
        output.Write(buffer, 0, buffer.Length);
        output.Close();
        return new Res(0, "200");
    }

    public static Res Hello(HttpListenerContext context, Dictionary<string, string> cache = null)
    {
        Console.WriteLine("HELLO, WORLD! FROM /hello");
        string responseString = "<HTML><BODY> Hello world!</BODY></HTML>";
        byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
        context.Response.ContentLength64 = buffer.Length;
        System.IO.Stream output = context.Response.OutputStream;
        output.Write(buffer, 0, buffer.Length);
        output.Close();
        return new Res(0, "200");
    }



}
