using tiny_key_value_server;
using System.Collections.Concurrent;
using System.Net;

namespace tiny_key_value_server;

internal class Program
{
    static void Main(string[] args)
    {
        TinyKeyValueServer s = new();
        s.Start();

        Console.WriteLine("Starting at: http://localhost:8888/");
        Console.WriteLine("Starting at: http://localhost:8888/hello?key=hello");
        Console.WriteLine("Starting at: http://localhost:8888/asd404");
        Console.WriteLine("Starting at: http://localhost:8888/get");
        Console.WriteLine("Starting at: http://localhost:8888/set");
    }
}
