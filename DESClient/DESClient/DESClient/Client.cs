using System;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace Client
{
	class Program
	{
		public static void Main(string[] args)
		{
			
			bool notdone = true; // variable used to check if the client needs to exit or not

			//Create a client
			Client client = new Client();
	

			//Loop until command 'quit' is entered
			while (notdone)
			{
				try
				{
					// Establish connection to Server
					client.Connect();

					// Get message to be sent to server
					String clientMsge = client.GetClientMsge();

					// Check if the client wants to disconnect message to be sent to server
					notdone = client.CheckToTerminate(clientMsge);

					// Send data to Server
					client.TransmitMsgeToServer(clientMsge);

					// Receive and print message from Server
					client.ReceiveMsgeFromServer();

					// Close Tcp connection
					client.Disconnect();
				}
				catch (Exception e)
				{
					Console.WriteLine("Error... " + e.StackTrace);
				}
			}
		}
	}

	class Client
	{
		private ConnectionManager connectionManager;

		public Client()
		{
			connectionManager = new ConnectionManager("127.0.0.1", 8888);
		}

		public void Connect()
		{
			connectionManager.ClientConnect();
		}

		public void Disconnect()
		{
			connectionManager.CloseConnection();
		}

		public void ReceiveMsgeFromServer()
		{
			Stream networkStream = connectionManager.getNetworkSteam();
			byte[] bytes_data = new byte[100];
			int size = networkStream.Read(bytes_data, 0, 100);
			Console.Write("Message from Server is: \n -> ");
			PrintServersMsge(bytes_data, size);
		}

		public void PrintServersMsge(byte[] data, int size)
		{
			for (int i = 0; i < size; i++)
			{
				Console.Write(Convert.ToChar(data[i]));
			}

			Console.WriteLine("");
		}

		public void TransmitMsgeToServer(string message)
		{
			ASCIIEncoding asen = new ASCIIEncoding();
			byte[] bytesData = asen.GetBytes(message);
			Console.Write("Sending message to Server .. ");
			Stream networkStream = connectionManager.getNetworkSteam();
			networkStream.Write(bytesData, 0, bytesData.Length);
			Console.WriteLine("..");
		}

		public string GetClientMsge()
		{
			Console.Write("Enter message to send to Server (or enter 'quit' to exit): ");
			string str = Console.ReadLine();
			return str;
		}

		public bool CheckToTerminate(string terminate_message)
		{
			return terminate_message.ToUpper() != "QUIT";
		}

	}


	class ConnectionManager
	{
		TcpClient tcpClient;
		String ipAddress;
		int port;

		public ConnectionManager(String ipAddress, int port)
		{
			this.ipAddress = ipAddress;
			this.port = port;
		}

		public void ClientConnect()
		{
			tcpClient = new TcpClient(); // create TCP client
			Console.WriteLine("\n****");
			Console.WriteLine("Connecting to Server \n***** \n");
			tcpClient.Connect(ipAddress, port); // try connecting to server IPaddress and open port
		}

		public void CloseConnection()
		{
			tcpClient.Close();
		}

		public Stream getNetworkSteam()
		{
			return tcpClient.GetStream();
		}
	}
}
