using SimpleTcp;
using System;
using System.Text;
using System.Threading;
using SimpleTcp;

namespace KiwiSocket
{
    class Program
    {
        static void Main(string[] args)
        {
            // instantiate
            TcpClient client = new TcpClient("127.0.0.1", 2012, false, null, null);

            // set callbacks
            client.Connected += Connected;
            client.Disconnected += Disconnected;
            client.DataReceived += DataReceived;

            // let's go!
            
            client.Connect();

            // once connected to the server...
            client.Send(Encoding.UTF8.GetBytes("Hello, world! \r\n"));
            Thread.Yield();
            Console.WriteLine("enter sent data ");
            Console.ReadLine();
            while (true)
            {
                for (int i = 0; i < 30; i++)
                {
                    if (i % 6 == 0) continue;
                    string path = $"Image/{i % 6}.png";
                    string base64 = Utils.ImageConverter.GetBase64FromImagePath(path);
                    string key = "V";
                    string data = base64;
                    if (!client.IsConnected) client.Connect();
                    client.Send(Encoding.UTF8.GetBytes($"{key} {data}\r\n"));
                    Thread.Sleep(100);
                }

                Console.Read();
            }



            Console.ReadKey();
        }

        static void Connected(object sender, EventArgs e)
        {
            Console.WriteLine("*** Server connected");
        }

        static void Disconnected(object sender, EventArgs e)
        {
            Console.WriteLine("*** Server disconnected");
        }

        static void DataReceived(object sender, DataReceivedFromServerEventArgs e)
        {
            Console.WriteLine("返回：" + Encoding.UTF8.GetString(e.Data));
        }
    }
}
