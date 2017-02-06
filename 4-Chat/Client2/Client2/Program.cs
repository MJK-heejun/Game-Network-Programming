using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace networkLibraryConsoleAppTest
{
    class Program
    {
        static void Main(string[] args)
        {

            TransportUDP udpLib = new TransportUDP();

            udpLib.StartServer(14674,2);


        }
    }
}
