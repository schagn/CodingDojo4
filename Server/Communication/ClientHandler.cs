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
        private Action<string, Socket> action;

        public string Name { get; private set; }

        public Socket client { get; private set; }

        public ClientHandler(Socket sock, Action<string, Socket> action)
        {
            this.client = sock;
            this.action = action;
            client = sock;
            Task.Factory.StartNew(Receive);
            Send("Hello new Client!\r\n");
        }

        public void Send(string message)
        {
            client.Send(Encoding.UTF8.GetBytes(message));

        }

        public void Receive() // noch ausuführlicher !! 
        {
            int length;
            string message = "";
            

            while (message != null && message != endMessage)
            {
                length = client.Receive(buffer);
                message = Encoding.UTF8.GetString(buffer, 0, length);
               
                Console.WriteLine(Encoding.UTF8.GetString(buffer, 0, length));

                if(Name == null && message.Contains(":"))
                {
                    Name = message.Split(':')[0];
                }
                action(message, client);
            }
            Close();
        }

       public void Close()
        {
            //quit message senden
            Send(endMessage);
            this.client.Close(1);
            threadClient.Abort();
            
        }

      
    }
}