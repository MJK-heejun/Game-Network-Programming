using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace client1
{
    class Program
    {
        enum ChatState {
            HOST_TYPE_SELECT = 0,
            CHATTING,
            LEAVE,
            ERROR
        };
        static ChatState _currentState;
        static TransportTCP tcpLib = new TransportTCP();

        static void Main(string[] args)
        {

            

            bool ret = tcpLib.Connect("127.0.0.1", 122);


            string input;
            if (ret)
            {
                _currentState = ChatState.CHATTING;
                Console.Write("welcome to my chat");
                Thread chatThr = new Thread(ChatThread);
                chatThr.Start();

                while (true)
                {
                    input = Console.ReadLine();
                    byte[] buffer = System.Text.Encoding.UTF8.GetBytes(input);
                    tcpLib.Send(buffer, buffer.Length); //this fucking doesnt work
                    //tcpLib.m_socket.Send(buffer, buffer.Length, SocketFlags.None); //this does work
                }
            }
        }

        static void ChatThread()
        {
            while (true)
            {
                switch (_currentState)
                {
                    case ChatState.CHATTING:
                        UpdateChatting();
                        break;
                    case ChatState.ERROR:
                        break;
                    case ChatState.LEAVE:
                        break;
                }
                Thread.Sleep(1000);
            }
        }

        static void UpdateChatting() {
            byte[] buffer = new byte[1400];
            int rcvSize = tcpLib.Receive(ref buffer, buffer.Length);
            if(rcvSize > 0)
            {
                string message = System.Text.Encoding.UTF8.GetString(buffer);                
                Console.WriteLine(message.TrimEnd(new char[] { '\0' }));
            }
        }
    }
}
