using System;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace networkLibraryConsoleAppTest
{
    class Program
    {
        static void Main(string[] args)
        {
            WebSocketTCP test = new WebSocketTCP("127.0.0.1", 80);
            test.StartServer();
        }
    }




}
