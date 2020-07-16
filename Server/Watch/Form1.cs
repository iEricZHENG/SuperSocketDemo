using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using SuperSocket.SocketBase.Config;

namespace Watch
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// 1. image转base64
        /// 2. base64转image
        /// 3. image展示
        /// 
        /// </summary>
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 1000; i++)
            {
                if (i % 6 == 0) continue;
                string path = $"Image/{i % 6}.png";
                string base64 = Utils.ImageConverter.GetBase64FromImagePath(path);
                Bitmap bitmap = Utils.ImageConverter.GetImageFromBase64(base64);
                Thread.Sleep(100);
                pictureBox1.Image = bitmap;
                pictureBox1.Refresh();
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            var appServer = new AppServer();
            ServerConfig config = new ServerConfig();
            config.Ip = "127.0.0.1";
            config.Port = 2012;
            config.MaxConnectionNumber = 100000;
            config.MaxRequestLength = Int32.MaxValue;

            appServer.NewSessionConnected += new SessionHandler<AppSession>(appServer_NewSessionConnected);
            appServer.NewRequestReceived += new RequestHandler<AppSession, StringRequestInfo>(appServer_NewRequestReceived);

            //Setup the appServer
            if (!appServer.Setup(config)) //Setup with listening port
            {
                MessageBox.Show("监听端口失败");
            }

            //Try to start the appServer
            if (!appServer.Start())
            {
                MessageBox.Show("启动失败");
            }



        }
        static void appServer_NewSessionConnected(AppSession session)
        {
            session.Send("Welcome to SuperSocket Telnet Server");
        }
        private delegate void DelegateSetCotnent(Bitmap bitmap);
        public void SetPrompInfo(Bitmap bitmap)
        {
            if (this.pictureBox1.InvokeRequired)
            {
                Invoke(new DelegateSetCotnent(SetPrompInfo),bitmap);
            }
            else
            {
                this.pictureBox1.Image = bitmap;
                this.pictureBox1.Refresh();
            }
        }
        void appServer_NewRequestReceived(AppSession session, StringRequestInfo requestInfo)
        {
            switch (requestInfo.Key.ToUpper())
            {
                case ("V"):

                    Bitmap bitmap = Utils.ImageConverter.GetImageFromBase64(requestInfo.Body);
                    
                    //pictureBox1.Image = bitmap;
                    //pictureBox1.Refresh();
                    SetPrompInfo(bitmap);
                    Thread.Sleep(100);

                    session.Send("Ok");
                    break;

                default:
                    session.Send(requestInfo.Body);
                    break;
            }
        }
    }
}
