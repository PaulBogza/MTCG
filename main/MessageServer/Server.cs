/*
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

Socket serverSock = new Socket(
            AddressFamily.InterNetwork,
            SocketType.Stream, 
            ProtocolType.Tcp
        );

        serverSock.Bind(new IPEndPoint(IPAddress.Loopback, 5555));
        serverSock.Listen(5);

        Socket clientSock = serverSock.Accept();
        
        clientSock.Send(Encoding.ASCII.GetBytes("Hello World!"));

        byte[] buffer = {};

        int length = clientSock.Receive(buffer, 0 ,1024, SocketFlags.None);
        string input = Encoding.ASCII.GetString(buffer, 0, length);
*/

