using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server.Communication
{
    public class ClientHandler
    {

        Socket client;
        private byte[] buffer = new byte[2048];
        private Thread threadClient;
        const string endMessage = "@quit";

        public string Name { get; private set; }

        public ClientHandler(Socket sock)
        {
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
            

            while (true)
            {
                length = client.Receive(buffer);
                message += Encoding.UTF8.GetString(buffer, 0, length);
               
                Console.WriteLine(Encoding.UTF8.GetString(buffer, 0, length));

            }
        }

       public void Close()
        {
            //quit message senden
            Send(endMessage);
            this.client.Close();
            threadClient.Abort();
            
        }

      
    }
}