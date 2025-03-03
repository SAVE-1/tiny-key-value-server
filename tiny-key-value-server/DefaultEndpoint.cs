using System.Net;

namespace tiny_key_value_server;

public static class DefaultEndpoint
{
    public static Res MyDefaultEndpoint(HttpListenerContext context)
    {
        Console.WriteLine("HELLO, WORLD! FROM /hello");
        string responseString = "<HTML><BODY>ROOT</BODY></HTML>";
        byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
        context.Response.ContentLength64 = buffer.Length;
        System.IO.Stream output = context.Response.OutputStream;
        output.Write(buffer, 0, buffer.Length);
        output.Close();
        return new Res(0, "All good boss!!");
    }

    public static Res NotFound(HttpListenerContext context)
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

    public static Res Get(HttpListenerContext context)
    {
        Console.WriteLine("Getter");
        string responseString = "{'code': 200, 'endpoint': 'get'}";
        byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
        context.Response.ContentLength64 = buffer.Length;
        context.Response.AddHeader("Accept", "application/json");
        context.Response.StatusCode = 200;
        System.IO.Stream output = context.Response.OutputStream;
        output.Write(buffer, 0, buffer.Length);
        output.Close();
        return new Res(0, "All good boss!!");
    }

    public static Res Set(HttpListenerContext context)
    {
        Console.WriteLine("Setter");
        string responseString = "{'code': 200, 'endpoint': 'set'}";
        byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
        context.Response.ContentLength64 = buffer.Length;
        context.Response.AddHeader("Accept", "application/json");
        context.Response.StatusCode = 200;
        System.IO.Stream output = context.Response.OutputStream;
        output.Write(buffer, 0, buffer.Length);
        output.Close();
        return new Res(0, "All good boss!!");
    }

    public static Res Hello(HttpListenerContext context)
    {
        Console.WriteLine("HELLO, WORLD! FROM /hello");
        string responseString = "<HTML><BODY> Hello world!</BODY></HTML>";
        byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
        context.Response.ContentLength64 = buffer.Length;
        System.IO.Stream output = context.Response.OutputStream;
        output.Write(buffer, 0, buffer.Length);
        output.Close();
        return new Res(11, "All good boss!!");
    }



}
