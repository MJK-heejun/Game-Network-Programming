using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;


public class WebSocketTCP
{
    TcpListener serverSocket;


    // Use this for initialization
    public WebSocketTCP(string ipAddr = "127.0.0.1", int port = 8888)
    {
        serverSocket = new TcpListener(IPAddress.Parse(ipAddr), 80);
    }


    // start server socket|
    public void StartServer()
    {
        serverSocket.Start();
        acceptClient();
    }

    private void acceptClient() {

        TcpClient clientSocket;

        while ((true))
        {
            clientSocket = serverSocket.AcceptTcpClient();
            NetworkStream stream = clientSocket.GetStream();

            while (!stream.DataAvailable) ;

            Byte[] bytes = new Byte[clientSocket.Available];
            stream.Read(bytes, 0, bytes.Length);

            //translate bytes of request to string
            String data = Encoding.UTF8.GetString(bytes);

            if (new Regex("^GET").IsMatch(data))
            {
                handShake(stream, data);
                //start new thread
                ClientHandler client = new ClientHandler();
                client.StartClient(clientSocket, "11111");
            }
        }
    }

    // stop server socket
    public void StopServer()
    {
        serverSocket.Stop();
    }



    private void handShake(NetworkStream stream, String data)
    {
        Byte[] response = Encoding.UTF8.GetBytes("HTTP/1.1 101 Switching Protocols" + Environment.NewLine
            + "Connection: Upgrade" + Environment.NewLine
            + "Upgrade: websocket" + Environment.NewLine
            + "Sec-WebSocket-Accept: " + Convert.ToBase64String(
                SHA1.Create().ComputeHash(
                    Encoding.UTF8.GetBytes(
                        new Regex("Sec-WebSocket-Key: (.*)").Match(data).Groups[1].Value.Trim() + "258EAFA5-E914-47DA-95CA-C5AB0DC85B11"
                    )
                )
            ) + Environment.NewLine
            + Environment.NewLine);

        stream.Write(response, 0, response.Length);
    }
   

}
