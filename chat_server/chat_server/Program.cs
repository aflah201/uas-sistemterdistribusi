using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;

namespace chat_server
{
    class Program
    {
        const int portNo = 500;
        static void Main(string[] args)
        {
            String strHostName = "";
            if (args.Length == 0)
            {
                strHostName = Dns.GetHostName();
                Console.WriteLine("Nama Local Host: " + strHostName);
            }
            else
            {
                strHostName = args[0];
            }

            IPHostEntry ipEntry = Dns.GetHostByName(strHostName);
            IPAddress[] addr = ipEntry.AddressList;
            for (int i = 0; i < addr.Length; i++)
            {
                Console.WriteLine("IP Address {0}:{1}", i, addr[i].ToString());
            }

            System.Net.IPAddress localAdd = System.Net.IPAddress.Parse(addr[0].ToString());
            TcpListener listener = new TcpListener(localAdd, portNo);
            listener.Start();

            while (true)
            {
                ChatClient user = new
                ChatClient(listener.AcceptTcpClient());
            }
        }
    }
}
