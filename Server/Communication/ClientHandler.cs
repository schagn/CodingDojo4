using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server.Communication
{
    public class ClientHandler
    {

        private byte[] buffer = new byte[2048];
        private Thread threadClient;
        const string endMessage = "@quit";
        private Action<string, Socket> ActionMessage;

        public string Name { get; private set; }

        public Socket Client { get; private set; }

        public ClientHandler(Socket sock, Action<string, Socket> action)
        {
            this.Client = sock;
            this.ActionMessage = action;
            threadClient = new Thread(Receive);
            threadClient.Start();
            //Task.Factory.StartNew(Receive);
            Send("Hello new Client!\r\n");
        }

        public void Send(string message)
        {
            Client.Send(Encoding.UTF8.GetBytes(message));

        }

        public void Receive() // noch ausuführlicher !! 
        {
            string message = "";
            

            //while (message != null && message != endMessage)
            while (!message.Equals(endMessage))
            {
                int length = Client.Receive(buffer);
                message = Encoding.UTF8.GetString(buffer, 0, length);
               
                Console.WriteLine(Encoding.UTF8.GetString(buffer, 0, length));

                if(Name == null && message.Contains(":"))
                {
                    Name = message.Split(':')[0];
                }
                ActionMessage(message, Client);
            }
            Close();
        }

       

            public void Close()
        {
            //quit message senden
            Send(endMessage);
            this.Client.Close(1);
            this.threadClient.Abort();
            
        }

      
    }
}