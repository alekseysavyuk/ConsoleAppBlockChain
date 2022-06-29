using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ConsoleAppBlockChain
{
    public class ClientProgram
    {
        private const int port = 8888;
        private const string ip_server = "127.0.0.1";

        static void Main(string[] args)
        {
            Console.WriteLine("Client started");
            
            try
            {
                IPEndPoint point = new IPEndPoint(IPAddress.Parse(ip_server), port);
                Socket tcpClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                tcpClient.Connect(point);
                Console.WriteLine("Connected");

                byte[] data = new byte[256];
                StringBuilder builder = new StringBuilder();
                int bytes = 0;

                do
                {
                    bytes = tcpClient.Receive(data);
                    builder.Append(Encoding.UTF8.GetString(data, 0, bytes));

                } while (tcpClient.Available > 0);

                Console.Write("Enter your name: ");
                string name = Console.ReadLine();

                Console.Write("Enter your last name: ");
                string lastName = Console.ReadLine();

                Console.Write("Enter transaction details: ");
                string transaction = Console.ReadLine();

                Block block = new Block(builder.ToString(), name, lastName, transaction);

                string message = block.Serialize();

                data = Encoding.UTF8.GetBytes(message);
                tcpClient.Send(data);

                data = new byte[256];
                builder = new StringBuilder();
                bytes = 0;

                do
                {
                    bytes = tcpClient.Receive(data);
                    builder.Append(Encoding.UTF8.GetString(data, 0, bytes));

                } while (tcpClient.Available > 0);

                Console.WriteLine("Server: " + builder);

                tcpClient.Shutdown(SocketShutdown.Both);
                tcpClient.Close();
            }

            catch (Exception ex) { throw new Exception("\nCleint Error: " + ex.Message); }

            Console.ReadLine();
        }
    }
}