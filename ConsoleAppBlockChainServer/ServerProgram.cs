using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ConsoleAppBlockChain
{
    public class ServerProgram
    {
        private const int port = 8888;
        private const string ip_server = "127.0.0.1";

        static void Main(string[] args)
        {
            #region Deleted&CreatedDB
            /*
            using (ConsoleAppBlockChainContext db = new ConsoleAppBlockChainContext())
            {
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();
            }
            */
            #endregion

            try
            {
                Chain chain = new Chain();

                IPEndPoint point = new IPEndPoint(IPAddress.Parse(ip_server), port);
                Socket tcpServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                tcpServer.Bind(point);
                tcpServer.Listen();
                Console.WriteLine("Server started");

                while(true)
                {
                    Socket handler = tcpServer.Accept();

                    StringBuilder str = new StringBuilder();
                    int bytes = 0;
                    byte[] data = new byte[256];

                    string previousHashToClient = chain.GetLastHashFromDB();
                    data = Encoding.UTF8.GetBytes(previousHashToClient);
                    handler.Send(data);

                    do
                    {
                        bytes = handler.Receive(data);
                        str.Append(Encoding.UTF8.GetString(data, 0, bytes));

                    } while (handler.Available > 0);

                    Block block = Block.Deserialize(str.ToString());

                    chain.AddBlock(block);

                    Console.WriteLine($"Blockchain: {chain.Check()}");

                    string message = "Done";
                    data = Encoding.UTF8.GetBytes(message);
                    handler.Send(data);

                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }
            }

            catch (Exception ex) { throw new Exception("\nServer Error: " + ex.Message); }
        }
    }
}