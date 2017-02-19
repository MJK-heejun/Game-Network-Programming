using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace networkLibraryConsoleAppTest
{
    class Program
    {
        static void Main(string[] args)
        {
            
            TransportTCP tcpLib = new TransportTCP();

            IPHostEntry hostEntry = Dns.GetHostEntry(Dns.GetHostName());
            System.Net.IPAddress hostAddress = hostEntry.AddressList[0];
            Console.WriteLine(hostAddress.ToString());
            Console.WriteLine(hostEntry.HostName);



            tcpLib.StartServer(122,3);

            //Thread newThread = new Thread(startNewThread);
            //newThread.Start();

            //tcpLib.Connect(hostAddress.ToString(), 14674);
        }


        static void startNewThread()
        {
            
        }

    }




}
