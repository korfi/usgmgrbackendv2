using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;

namespace USG_backend_console
{
    class ServerRedirector
    {

        private int port;
        Byte[] bytes = new Byte[256];
        AutomationHandler ah = new AutomationHandler();

        public ServerRedirector(int p)
        {
            this.port = p;
        }

        public void start()
        {
            IPAddress[] ipTab = Array.FindAll(
                Dns.GetHostEntry(string.Empty).AddressList,
                a => a.AddressFamily == AddressFamily.InterNetwork);
            IPAddress ipAdd = ipTab[0];
            GlobalSettings.serverSocket = new TcpListener(ipAdd, this.port);
            GlobalSettings.clientSocket = default(TcpClient);
            GlobalSettings.serverSocket.Start();
            while (true)
            {
                Console.Write("Waiting for a connection at " + ipAdd.ToString() + "\r\n");
                GlobalSettings.clientSocket = GlobalSettings.serverSocket.AcceptTcpClient();
                Console.WriteLine("Connected!\r\n");

                NetworkStream stream = GlobalSettings.clientSocket.GetStream();
                StreamReader reader = new StreamReader(stream);
                while (GlobalSettings.clientSocket.Connected)
                {
                    try {
                       string currData = reader.ReadLine();
                       ah.HandleStringCommand(currData, GlobalSettings.clientSocket.Client.RemoteEndPoint.ToString());
                    }
                    catch (Exception ex) { 
                        Console.WriteLine("Couldn't process sending/receiving data: " + ex.Message);
                        stream.Close();
                        GlobalSettings.clientSocket.Close();
                    };
                }

                //stream.Close();
                //GlobalSettings.clientSocket.Close();
                //GlobalSettings.serverSocket.Stop();
            }
        }

    }
}
