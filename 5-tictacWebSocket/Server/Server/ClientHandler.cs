using System;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;


public class ClientHandler
{
    TcpClient clientSocket;
    string clNo;

    public void StartClient(TcpClient clientSocket, string clNo)
    {
        this.clientSocket = clientSocket;
        this.clNo = clNo;
        //start new thread for the client 'clineNo'
        Thread ctThread = new Thread(receive);
        ctThread.Start();
    }

    private void receive()
    {
        //while ((true))
        while (clientSocket.Connected)
        {
            NetworkStream stream = clientSocket.GetStream();

            while (!stream.DataAvailable) ; //loop until the stream data is available

            Byte[] bytes = new Byte[clientSocket.Available];

            stream.Read(bytes, 0, bytes.Length);

            //translate bytes of request to string
            String data = Encoding.UTF8.GetString(bytes);

            string msg = getDecodedData(bytes, bytes.Length);
            if (msg.Equals("\u0003?"))
                clientSocket.Close();
            else
                Console.WriteLine(msg);
        }

        Console.WriteLine("The client is disconnected");
    }


    private string getDecodedData(byte[] buffer, int length)
    {
        byte b = buffer[1];
        int dataLength = 0;
        int totalLength = 0;
        int keyIndex = 0;

        if (b - 128 <= 125)
        {
            dataLength = b - 128;
            keyIndex = 2;
            totalLength = dataLength + 6;
        }

        if (b - 128 == 126)
        {
            dataLength = BitConverter.ToInt16(new byte[] { buffer[3], buffer[2] }, 0);
            keyIndex = 4;
            totalLength = dataLength + 8;
        }

        if (b - 128 == 127)
        {
            dataLength = (int)BitConverter.ToInt64(new byte[] { buffer[9], buffer[8], buffer[7], buffer[6], buffer[5], buffer[4], buffer[3], buffer[2] }, 0);
            keyIndex = 10;
            totalLength = dataLength + 14;
        }

        if (totalLength > length)
            throw new Exception("The buffer length is small than the data length");

        byte[] key = new byte[] { buffer[keyIndex], buffer[keyIndex + 1], buffer[keyIndex + 2], buffer[keyIndex + 3] };

        int dataIndex = keyIndex + 4;
        int count = 0;
        for (int i = dataIndex; i < totalLength; i++)
        {
            buffer[i] = (byte)(buffer[i] ^ key[count % 4]);
            count++;
        }

        return Encoding.ASCII.GetString(buffer, dataIndex, dataLength);
    }


}
