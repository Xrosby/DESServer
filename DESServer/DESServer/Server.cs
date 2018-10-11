using System;
using System.Text;
using System.Net.Sockets;
using Handler;

public class Server
{

    public void Start() {
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
                Console.WriteLine("\n Server has sent an Acknowledgement");

                // Check if Client has sent a 'quit' message. If so then disconnect and close socket.
                connectionHandler.CheckToDisconnect(message);

                // Stop listening on connection
                connectionHandler.StopListener();

        

        }
    }

 





}

