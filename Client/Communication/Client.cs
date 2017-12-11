using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Client.Communication
{
    public class Client
    {
        byte[] buffer = new byte[1024];
        Socket clientSocket;
        Action AbortInformer;
        Action<string> MessageInformer;

        public Client(string ip, int port, Action abort, Action<string> message)
        {
            try
            {
                this.AbortInformer = abort;
                this.MessageInformer = message;
                TcpClient client = new TcpClient();
                //client.ConnectAsync(IPAddress.Parse(ip), port);
                client.Connect(IPAddress.Parse(ip),port);
               // client.Connect(new IPEndPoint(IPAddress.Parse(ip), port));
                clientSocket = client.Client;
                StartReceiving();

            }
            catch (Exception)
            {
                // informer
                MessageInformer("Server is not ready");
                AbortInformer();
                
            }
        }


        public void StartReceiving ()
        {
            Task.Factory.StartNew(Receive);
        }

        private void Receive ()
        {
            string message = "";
            int length;
            while(!message.Contains("@quit"))
            {
                length = clientSocket.Receive(buffer);
                message = Encoding.UTF8.GetString(buffer, 0, length);
                MessageInformer(message);

            }
            Close();
        }

        public void Send(string message)
        {
            if (clientSocket != null)
            {
                clientSocket.Send(Encoding.UTF8.GetBytes(message));
            }
        }

        public void Close()
        {
            clientSocket.Close();
            AbortInformer();
        }
    }
}
