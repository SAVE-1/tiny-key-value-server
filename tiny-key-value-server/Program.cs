using tiny_key_value_server;
using System.Collections.Concurrent;
using System.Net;
using CommandLine;

namespace tiny_key_value_server;

internal class Program
{

    public class Options
    {
        [Option('p', "port", Required = true, HelpText = "Running port.")]
        public int? Port { get; set; }
    }

    static void Main(string[] args)
    {
        int port = -1;
        Parser.Default.ParseArguments<Options>(args)
               .WithParsed<Options>(o =>
               {
                   if (o.Port != null)
                   {
                       Console.WriteLine($"Port selected. Current Arguments: -port {o.Port}");
                       port = o.Port ?? default(int);
                   }
                   else
                   {
                       Console.WriteLine($"Port needs to be given: -port {o.Port}");
                   }
               });

        Console.WriteLine("port:" + port);
        if (0 >= port)
        {
            Console.WriteLine("Incorrect port, port must be greater than 0");
            return;
        }

        TinyKeyValueServer s = new();
        s.Start(port);

#if DEBUG
        Console.WriteLine("Starting at: http://localhost:8888/");
        Console.WriteLine("Starting at: http://localhost:8888/hello?key=hello");
        Console.WriteLine("Starting at: http://localhost:8888/asd404");
        Console.WriteLine("Starting at: http://localhost:8888/get");
        Console.WriteLine("Starting at: http://localhost:8888/set");
#endif

    }
}
