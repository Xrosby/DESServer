using System;
using System.Text;
using System.Net.Sockets;

namespace Handler
{
    public class MessageHandler
    {
        private byte[] data;
        private int data_size;

        public MessageHandler()
        {
        }

        public void MsgeToClient(StringBuilder message, Socket socket)
        {
            ASCIIEncoding aSCIIEncoding = new ASCIIEncoding();
            try
            {
                socket.Send(aSCIIEncoding.GetBytes("Server has received the following: " + message + " ."));
            }catch(SocketException e) {
                Console.WriteLine(e);
            }
        }

        public void PrintMsgeFromClient(StringBuilder message)
        {
            for (int i = 0; i < this.data_size; i++)
            {
                message.Append(Convert.ToChar(this.data[i]));
            }
            Console.Write(message);
        }

        public void ReceiveClientMsge(Socket socket)
        {
            this.data = new byte[100];
            try
            {
                this.data_size = socket.Receive(this.data);
            } catch(SocketException e) {
                Console.WriteLine(e);
            }
            Console.WriteLine("Recieved...");
        }

    }
}
