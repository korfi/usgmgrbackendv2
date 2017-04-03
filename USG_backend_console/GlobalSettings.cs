using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace USG_backend_console
{
    static class GlobalSettings
    {
        public static TcpListener serverSocket = null;
        public static TcpClient clientSocket = null;
    }
}
