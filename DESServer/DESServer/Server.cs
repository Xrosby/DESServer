using System;
using System.Text;
using System.Net.Sockets;
using System.Net;

namespace Sever
{
	public class ServerMain
	{

		public static void Main()
		{

			Server server = new Server();
			server.Start();
		}
	}

	public class Server
	{

		public void Start()
		{
			//Initialize the MessageHandler
			MessageHandler messageHandler = new MessageHandler();
			//Initialize the ConnectionHandler
			ConnectionHandler connectionHandler = new ConnectionHandler("127.0.0.1", 8888);

			// string strMessage =null;


			while (true)
			{
				var message = new StringBuilder();
				
				// Start listening for a connection
				connectionHandler.StartListener();
				Socket socket = connectionHandler.AcceptedConnection();

				// Receive message from Client
				messageHandler.ReceiveClientMsge(socket);

				// Print Client's message
				messageHandler.PrintMsgeFromClient(message);

				// Send message to client to confirm receiving the message.
				messageHandler.MsgeToClient(message, socket);
				Console.WriteLine("Server has sent an Acknowledgement\n\n");

				// Check if Client has sent a 'quit' message. If so then disconnect and close socket.
				connectionHandler.CheckToDisconnect(message);

				// Stop listening on connection
				connectionHandler.StopListener();

			}
		}
	}


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
				socket.Send(aSCIIEncoding.GetBytes("Server has received the following: '" + message + "'"));
			}
			catch (SocketException e)
			{
				Console.WriteLine(e);
			}
		}

		public void PrintMsgeFromClient(StringBuilder message)
		{
			for (int i = 0; i < this.data_size; i++)
			{
				message.Append(Convert.ToChar(this.data[i]));
			}
			Console.WriteLine("Message: " + message);
		}

		public void ReceiveClientMsge(Socket socket)
		{
			this.data = new byte[100];
			try
			{
				this.data_size = socket.Receive(this.data);
			}
			catch (SocketException e)
			{
				Console.WriteLine(e);
			}
			Console.WriteLine("Recieved message..");
		}

	}


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
			}
			catch (SocketException e)
			{
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






