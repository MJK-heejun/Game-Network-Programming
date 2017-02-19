using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace client1
{
    class Program
    {
        static void Main(string[] args)
        {

            TransportTCP tcpLib = new TransportTCP();

            bool ret = tcpLib.Connect("127.0.0.1", 122);
            Console.WriteLine(ret);
            Console.ReadLine();
        }
    }
}
