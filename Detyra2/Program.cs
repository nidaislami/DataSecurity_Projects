using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server_TCP
{
    class Program
    {
        static void Main(string[] args)
        {
            int porti = 7000;
            Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint endpoint = new IPEndPoint(IPAddress.Any, porti);
            server.Bind(endpoint);

            server.Listen(10);
            Console.WriteLine("Duke pritur klientin ne portin " + porti);

            while (true)
            {
                try
                {
                    Socket klienti = server.Accept();
                    ConnectionHandler handler = new ConnectionHandler(klienti);

                    Thread thread = new Thread(new ThreadStart(handler.HandleConnection));
                    thread.Start();
                }
                catch (Exception)
                {
                    Console.WriteLine("Lidhja deshtoi ne portin " + porti);
                }
            }
        }
    }
}
