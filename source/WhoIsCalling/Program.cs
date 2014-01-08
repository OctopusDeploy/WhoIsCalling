using System;
using System.Net;
using System.Net.Sockets;

namespace WhoIsCalling
{
    class Program
    {
        static int Main(string[] args)
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Who is calling? (c) Octopus Deploy 2014");

                int port;
                if (args.Length != 1 || !int.TryParse(args[0], out port))
                {
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Error.WriteLine("  Usage: WhoIsCalling.exe [port]");
                    Console.ResetColor();
                    return -1;
                }

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine();
                Console.WriteLine("  Listening for inbound TCP connections on port {0}", port);
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("  Press CTRL+C to exit...");
                Console.WriteLine();

                var server = new TcpListener(IPAddress.Any, port);
                server.Start();

                while (true)
                {
                    var client = server.AcceptTcpClient();
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write("{0:R} [ACCEPT] ", DateTime.UtcNow);
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine(client.Client.RemoteEndPoint);
                    client.Close();

                    Console.ResetColor();
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error: {0}", ex);
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine();
                Console.WriteLine("Please ensure this app is run under an administrative account, and no " +
                    "other applications are listening on the same port. The Windows `netstat -o -n -a` command " +
                    "can help to find apps using the port.");
                Console.ResetColor();
                return -1;
            }
        }
    }
}
