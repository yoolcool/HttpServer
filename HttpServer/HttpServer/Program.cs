using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;


namespace HttpServer
{
    class Program
    {
        static void Main(string[] args)
        {
            newHttpRequest();
        }

        static void newHttpRequest()
        {
            String pageURL = "http://gall.dcinside.com/board/lists/?id=maplestory2&s_type=search_subject_memo&s_keyword=%EB%82%98%EA%B8%B0";
            HttpWebRequest req = WebRequest.Create(pageURL) as HttpWebRequest;

            HttpWebResponse resp = req.GetResponse() as HttpWebResponse;

            using (StreamReader sr = new StreamReader(resp.GetResponseStream()))
            {
                string responseText = sr.ReadToEnd();
                Console.WriteLine(responseText);
                File.WriteAllText("capturePage.html", responseText);
            }

            Console.ReadLine();
        }

        static void oldHttpRequest()
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress ipAddr = Dns.GetHostEntry("www.naver.com").AddressList[0];
            EndPoint serverEP = new IPEndPoint(ipAddr, 80);

            socket.Connect(serverEP);

            string request = "GET / HTTP/1.0\r\nHost: www.naver.com\r\n\r\n";
            byte[] sendBuffer = Encoding.UTF8.GetBytes(request);

            socket.Send(sendBuffer);

            MemoryStream ms = new MemoryStream();

            while (true)
            {
                byte[] rcvBuffer = new byte[4096];
                int nRecv = socket.Receive(rcvBuffer);
                if (nRecv == 0)
                {
                    break;
                }

                ms.Write(rcvBuffer, 0, nRecv);

            }

            socket.Close();

            string response = Encoding.UTF8.GetString(ms.GetBuffer(), 0, (int)ms.Length);
            Console.WriteLine(response);

            File.WriteAllText("naverpage.html", response);

        }
    }
}
