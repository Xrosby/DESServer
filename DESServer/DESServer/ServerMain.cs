using System;
using DESServer;

namespace DESServer
{
    public class ServerMain
    {

        public static void Main()
        {

            Server server = new Server();
            server.Start();
        }
    }
}
