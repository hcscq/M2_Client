using System;
using System.Collections.Generic;
using S = ServerPackets;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using netWorkTest.MirNetwork;
using System.Threading;
using System.Collections.Concurrent;
using System.Text;
using C = ClientPackets;
using System.Drawing;

namespace netWorkTest
{
    public partial class NetWork : Form
    {
        private TcpListener _listener;
        private Socket ServSocket;
        private bool Running;
        private Thread _thread;
        private const string IP = "127.0.0.1";
        private const int port = 7000;
        public NetWork()
        {
            InitializeComponent();
            this.Disposed += new EventHandler(OnClose);
            Packet.IsServer = false;
            //InitialClientNetWork();

            //InitialServerNetwork();

            ContainerControl container = new ContainerControl() { BackColor=Color.Gray} ;
            container.Size = new Size(100,80);
            container.Location = new Point(30,60);
            container.Parent = this;

            Button btn = new Button() {Text="Text" };
            btn.Click += (o, e) =>
            {
                MessageBox.Show("There.");
            };
            btn.Parent = container;
            btn.Location = new Point(20,90);
            
            
            //packet test
            /*string str = "#=L>>><v!$";
            byte[] rawBytes = ASCIIEncoding.Default.GetBytes("#zt<>>><v");
            int tran = EnDecode.fnDecode6BitBufA(rawBytes, rawBytes, 1, 3, 1, 4);
            int length = (rawBytes[2] << 8) + rawBytes[1];
            length = 0;

            rawBytes = new byte[1024];
            rawBytes = (new S.Chat { Message="QQ拼音.",Type=ChatType.Guild,nRecog=1100}).GetPacketBytesEx() as byte[];
            Packet.ReceivePacketEx(rawBytes, out rawBytes);*/
            /*ButtonsLocation*/
            //ButtonsLocation();

        }
        private void ButtonsLocation()
        {

            int m_nSrvCount = 2, COUNT_BUTTON_PER_COLUME=2, POS_TOP_SERVER_BTN_Y=30, SERVER_BTN_HEIGHT=35, SERVER_BTN_GAP=15;
            Button[] buttons = new Button[m_nSrvCount];
            System.Drawing.Size bSize = new System.Drawing.Size(70, 35);
            for (int i = 0; i <= ((m_nSrvCount - 1) / COUNT_BUTTON_PER_COLUME); i++)
            {
                for (int j = i * COUNT_BUTTON_PER_COLUME; (j < m_nSrvCount && j < (COUNT_BUTTON_PER_COLUME * (i + 1))); j++)
                {

                    buttons[i] = new Button();
                    buttons[i].Size = bSize;
                    buttons[i].Text = i.ToString();
                    buttons[i].Location =
                        new System.Drawing.Point(100,
                        POS_TOP_SERVER_BTN_Y + (j - i * COUNT_BUTTON_PER_COLUME) * (SERVER_BTN_HEIGHT + SERVER_BTN_GAP));
                    this.Controls.Add(buttons[i]);
                }
            }
            //int len = 3;
            //System.Drawing.Size fSize = this.Size;
            //fSize.Height -= 30;
            //Button[] buttons = new Button[len];
            //System.Drawing.Size bSize = new System.Drawing.Size(70, 35);
            //for (int i=0;i<buttons.Length;i++)
            //{
            //    buttons[i] = new Button();
            //    buttons[i].Size = bSize;
            //    buttons[i].Text = i.ToString();
            //    buttons[i].Location = 
            //        new System.Drawing.Point(30,
            //        i*(fSize.Height/len)+fSize.Height/(2*len)-bSize.Height/2);
            //    this.Controls.Add(buttons[i]);
            //}

        }
        private void OnClose(object sender,EventArgs e)
        { Running = false; }
        private void InitialServerNetwork()
        {
            Envir.Connections.Clear();
            Envir.SocketArgsPool =new ConcurrentStack<SocketAsyncEventArgs> (new Stack<SocketAsyncEventArgs>(Envir.MaxIP*2));
            SocketAsyncEventArgs socketArgs;
            for (int i=0;i<Envir.MaxIP*2;i++)
            {
                socketArgs = new SocketAsyncEventArgs();
                socketArgs.SetBuffer(new byte[1024*8],0,1024*8);
                socketArgs.Completed += new EventHandler<SocketAsyncEventArgs>(OnIOCompleted);
                Envir.SocketArgsPool.Push(socketArgs);
            }
            if (Envir.AccSocketArgs == null)
                Envir.AccSocketArgs = new SocketAsyncEventArgs();
            Envir.AccSocketArgs.Completed += new EventHandler<SocketAsyncEventArgs>(OnIOCompleted);
            IPEndPoint servIp = new IPEndPoint( IPAddress.Parse(IP), port);
            ServSocket = new Socket(servIp.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            ServSocket.Bind(servIp);
            ServSocket.Listen(0); 
            //if (Envir.AccSocketArgs.Buffer == null)
            //   Envir.AccSocketArgs.SetBuffer(new byte[8 * 1024], 0, 8 * 1024);
            //ServSocket.BeginAccept(AccCallBack,0);
            if (!ServSocket.AcceptAsync(Envir.AccSocketArgs))
                IocpAcc(Envir.AccSocketArgs);
            //_listener = new TcpListener(IPAddress.Parse(IP), port);
            //_listener.Start();
            //_listener.BeginAcceptTcpClient(Connection, null);


            MessageBox.Show("Network Started.");
            Start();
            //FixGuilds();
        }
        public void InitialClientNetWork()
        {
            Envir.Connections.Clear();
            Envir.SocketArgsPool = new ConcurrentStack<SocketAsyncEventArgs>(new Stack<SocketAsyncEventArgs>(Envir.MaxIP * 2));
            SocketAsyncEventArgs socketArgs;
            for (int i = 0; i < Envir.MaxIP * 2; i++)
            {
                socketArgs = new SocketAsyncEventArgs();
                socketArgs.SetBuffer(new byte[1024 * 8], 0, 1024 * 8);
                socketArgs.Completed += new EventHandler<SocketAsyncEventArgs>(OnIOCompleted);
                Envir.SocketArgsPool.Push(socketArgs);
            }
            if (Envir.AccSocketArgs == null)
                Envir.AccSocketArgs = new SocketAsyncEventArgs();
            Envir.AccSocketArgs.Completed += new EventHandler<SocketAsyncEventArgs>(OnIOCompleted);
            IPEndPoint servIp = new IPEndPoint(IPAddress.Parse(IP), port);
            ServSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //ServSocket.Bind(servIp);
            if (Envir.AccSocketArgs.Buffer == null)
                Envir.AccSocketArgs.SetBuffer(new byte[8 * 1024], 0, 8 * 1024);
            Envir.AccSocketArgs.RemoteEndPoint = servIp;
            if (!ServSocket.ConnectAsync(Envir.AccSocketArgs))
                ProcessConnect(Envir.AccSocketArgs);
            S.Awakening p = new S.Awakening();

            MessageBox.Show("Network Started.");
            //Start();
        }
        public void Start()
        {
            if (Running || _thread != null) return;

            Running = true;

            _thread = new Thread(WorkLoop) { IsBackground = true };
            _thread.Start();

        }
        private void WorkLoop()
        {
            while (Running)
            {
                lock (Envir.Connections)
                {
                    for (int i = Envir.Connections.Count - 1; i >= 0; i--)
                    {
                        Envir.Connections[i].Process();
                        Thread.Sleep(10);
                        //if(Envir.Connections[i].SendSocketArgs.Count>0&&!Envir.Connections[i]._client.SendAsync(Envir.Connections[i].SendSocketArgs))
                        //   ProcessSend(Envir.Connections[i].SendSocketArgs);
                    }
                }
            }
        }
        /// <summary>
        /// 当Socket上的发送或接收请求被完成时，调用此函数
        /// </summary>
        /// <param name="sender">激发事件的对象</param>
        /// <param name="e">与发送或接收完成操作相关联的SocketAsyncEventArg对象</param>
        private void OnIOCompleted(object sender, SocketAsyncEventArgs e)
        {
            // Determine which type of operation just completed and call the associated handler.
            switch (e.LastOperation)
            {
                case SocketAsyncOperation.Receive:
                    this.ProcessReceive(e);
                    break;
                case SocketAsyncOperation.Send:
                    ProcessSend(e);
                    break;
                case SocketAsyncOperation.Accept:
                    IocpAcc(e);
                    break;
                case SocketAsyncOperation.Connect:
                    this.ProcessConnect(e);
                    break;
                default:
                    throw new ArgumentException("The last operation completed on the socket was not a receive or send");
            }
        }
        private void ProcessConnect(SocketAsyncEventArgs e)
        {
            SocketAsyncEventArgs socketArgs;
            if (e.SocketError==SocketError.Success&&Envir.SocketArgsPool != null && Envir.BufferLock.WaitOne(200, true) && Envir.SocketArgsPool.TryPop(out socketArgs))
            {
                MirConnectionSimply connectInfo = new MirConnectionSimply(++Envir._sessionID, e.ConnectSocket, socketArgs);
                lock (Envir.Connections)
                    Envir.Connections.Add(connectInfo);
                if (!e.ConnectSocket.ReceiveAsync(socketArgs))
                    ProcessReceive(socketArgs);
            }
            else
                e.AcceptSocket.Close();
        }
        public static void ProcessSend(SocketAsyncEventArgs e)
        {

            if (SocketError.Success != e.SocketError || e.BytesTransferred == 0)
                (e.UserToken as MirConnectionSimply).Disconnect(0);
            Envir.SocketArgsPool.Push(e);
            Envir.BufferLock.Release();


        }

        private void ProcessReceive(SocketAsyncEventArgs e)
        {
            //
            if (SocketError.Success == e.SocketError)
            {
                ((MirConnectionSimply)e.UserToken).ReceiveData();
                e.SetBuffer(0, e.Count);
                if (!(e.UserToken as MirConnectionSimply)._client.ReceiveAsync(e))
                    ProcessReceive(e);
            }
            else
                (e.UserToken as MirConnectionSimply).Disconnect(0);
        }

        private void IocpAcc(SocketAsyncEventArgs e)
        {
            SocketAsyncEventArgs socketArgs;
            if (Envir.SocketArgsPool != null && Envir.BufferLock.WaitOne(200, true)&& Envir.SocketArgsPool.TryPop(out socketArgs))
            {
                MirConnectionSimply connectInfo = new MirConnectionSimply(++Envir._sessionID, e.AcceptSocket, socketArgs);
                lock (Envir.Connections)
                    Envir.Connections.Add(connectInfo);
                if (!e.AcceptSocket.ReceiveAsync(socketArgs))
                    ProcessReceive(socketArgs);
            }
            else
                e.AcceptSocket.Close();
            e.AcceptSocket = null;
            if (!ServSocket.AcceptAsync(e))
                IocpAcc(e);
            //Envir.iocpList.Add(ioContext);
        }
    }
}
