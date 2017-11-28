using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server.Communication
{
    public class Server
    {
        Socket server;
        List<ClientHandler> clients = new List<ClientHandler>();
        Thread threadAccepting;
        

        public Server()
        {
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            server.Bind(new IPEndPoint(IPAddress.Parse("127.0.0.1"),8080));
            server.Listen(15);

            Task.Factory.StartNew(StartAccepting);
        }


        public void StartAccepting()
        {
            threadAccepting = new Thread(new ThreadStart(Accept));
            threadAccepting.IsBackground = true;
            threadAccepting.Start();

           
        }

        private void Accept()
        {
            while (threadAccepting.IsAlive)
            {

                try
                {
                    //constructor erweitern
                    clients.Add(new ClientHandler(server.Accept()));
                    Console.WriteLine("Neuer Client akzeptiert!");
                }
                catch (Exception e)
                {
                    throw e;
                }
               
            }
        }

        public void StopAccepting()
        {
            server.Close();
            threadAccepting.Abort();

            foreach(var item in clients)
            {
                item.Close();
            }

            clients.Clear();
        }

   
        public void DisconnectSpecificClient(string name)
        {
            foreach (var item in clients)
            {
                if (item.Name == name)
                {
                    item.Close();
                    clients.Remove(item);
                    break;
                }
            }
        }

        // new message received function

    }


}
