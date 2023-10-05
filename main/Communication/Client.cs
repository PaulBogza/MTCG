using System;
using System.Net.Sockets;
using System.Runtime.InteropServices;

class TcpClientClass{
    public static void Read(){
        
        var s = new TcpClient();
        s.Connect("127.0.0.1", 5555);
        var stream = s.GetStream();
        var sr = new StreamReader(stream);
        while (!sr.EndOfStream){
            var line = sr.ReadLine();
            System.Console.WriteLine(line);
        }
    }
}