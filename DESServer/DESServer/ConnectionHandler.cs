using System.Net;
using System.Net.Sockets;
using System;
using System.Text;

namespace Handler
{
    public class ConnectionHandler
    {
        private TcpListener listener;
        private IPAddress connectionIpAddress;
        private Socket socket;

        public ConnectionHandler(string ipAdress, int port)
        {
            // Servers IPaddress
            this.connectionIpAddress = IPAddress.Parse(ipAdress);
            // Initialize the Listener */
            this.listener = new TcpListener(connectionIpAddress, port);
        }

        public Socket AcceptedConnection()
        {
            // Wait until connectection is accepted
            this.socket = AcceptPendingConn();
            Console.WriteLine("Connection accepted from " + socket.RemoteEndPoint);
            return socket;
        }

        public Socket AcceptPendingConn()
        {
            Console.WriteLine("Waiting for a Connection.....");
            try
            {
                this.socket = this.listener.AcceptSocket(); // Waits until connection is accepted
            } catch(SocketException e) {
                Console.WriteLine(e);
            }
            return socket;
        }

        public void StartListener()
        {
            this.listener.Start();
            Console.WriteLine("Server's End point is :" + listener.LocalEndpoint);
        }

        public void StopListener()
        {
            this.listener.Stop();
        }

        public void CheckToDisconnect(StringBuilder message)
        {
            string strMessage = Convert.ToString(message);
            strMessage = strMessage.ToUpper();
            if (strMessage == "QUIT")
            {
                Console.WriteLine("\nServer shutting done...");
                this.socket.Close();
                StopListener();
                Console.WriteLine("\nGoodbye");
                Environment.Exit(1);
            }

        }
    }
}
