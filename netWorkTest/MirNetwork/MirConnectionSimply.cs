using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Sockets;
using C = ClientPackets;
using S = ServerPackets;
using System.Linq;
using System.Threading;
using System.Text;

namespace netWorkTest.MirNetwork
{
    public enum GameStage { None, Login, Select, Game, Disconnected }
    public class MirConnectionSimply
    {
        public readonly int SessionID;
        public readonly string IPAddress;
        public readonly SocketAsyncEventArgs SocketArgs;
        public SocketAsyncEventArgs SendSocketArgs;
        public GameStage Stage;

        public Socket _client;
        public  ConcurrentQueue<Packet> _receiveList;
        public  Queue<Packet> _sendList, _retryList;

        private bool _disconnecting;
        public bool Connected;
        public bool Disconnecting
        {
            get { return _disconnecting; }
            set
            {
                if (_disconnecting == value) return;
                _disconnecting = value;
                //TimeOutTime = SMain.Envir.Time + 500;
            }
        }
        public readonly long TimeConnected;
        public long TimeDisconnected, TimeOutTime;

        byte[] _rawData = new byte[0];

        public List<ItemInfo> SentItemInfo = new List<ItemInfo>();
        //public List<QuestInfo> SentQuestInfo = new List<QuestInfo>();
        public bool StorageSent;


        public MirConnectionSimply(int sessionID, Socket client, SocketAsyncEventArgs socketArgs )
        {
            SessionID = sessionID;
            IPAddress = client.RemoteEndPoint.ToString().Split(':')[0];
            SocketArgs = socketArgs;
            socketArgs.UserToken = this;
            int connCount = 0;
            for (int i = 0; i < Envir.Connections.Count; i++)
            {
                MirConnectionSimply conn = Envir.Connections[i];
                if (conn.IPAddress == IPAddress && conn.Connected)
                {
                    connCount++;

                    if (connCount >= Envir.MaxIP)
                    {
                        //SMain.EnqueueDebugging(IPAddress + ", Maximum connections reached.");
                        conn.SendDisconnect(5);
                    }
                }
            }

            //SMain.Enqueue(IPAddress + ", Connected.");

            _client = client;
            _client.NoDelay = true;

            TimeConnected = Envir.Time;
            TimeOutTime = TimeConnected + Envir.TimeOut;

            if (_receiveList == null)
            {
                _receiveList = new ConcurrentQueue<Packet>();
                _sendList = new Queue<Packet>();//new[] { new S.Connected() }
                _retryList = new Queue<Packet>();
            }

            Connected = true;
        }
        public void ReceiveData()
        {
            if (!Connected) return;

            if (SocketArgs.BytesTransferred == 0)
            {
                Disconnecting = true;
                return;
            }
            //byte[] rawBytes =SocketArgs.Buffer;
            //*pszNext++ = '#';

            //memmove(pszNext, pszPacket, nLen);

            //pszNext += nLen;

            //*pszNext++ = '!';
            //*pszNext++ = '$';
            //*pszNext++ = '\0';


            byte[] temp = _rawData;
            _rawData = new byte[SocketArgs.BytesTransferred+ temp.Length];
            Buffer.BlockCopy(temp, 0, _rawData, 0, temp.Length);
            //EnDecode.fnDecode6BitBufA(SocketArgs.Buffer, _rawData, temp.Length, _rawData.Length);

            Buffer.BlockCopy(SocketArgs.Buffer, 0, _rawData, temp.Length, SocketArgs.BytesTransferred);

            _sendList.Enqueue(new C.KeepAlive
            {
                Time = DateTime.Now.Ticks
            });

            Packet p;
            while ((p = Packet.ReceivePacketEx(_rawData, out _rawData)) != null)
                _receiveList.Enqueue(p);
            //Process();
            //BeginReceive();
        }

        public void Enqueue(Packet p)
        {
            if (_sendList != null && p != null)
                _sendList.Enqueue(p);
        }

        public void Process()
        {
            if (_client == null || !_client.Connected)
            {
                Disconnect(20);
                return;
            }

            while (!_receiveList.IsEmpty && !Disconnecting)
            {
                Packet p;
                if (!_receiveList.TryDequeue(out p)) continue;
                TimeOutTime = Envir.Time + Envir.TimeOut;
                ProcessPacket(p);
            }

            while (_retryList.Count > 0)
                _receiveList.Enqueue(_retryList.Dequeue());

            //if (Envir.Time > TimeOutTime)
            //{
            //    Disconnect(21);
            //    return;
            //}

            if (_sendList == null || _sendList.Count <= 0) return;
            if (Envir.BufferLock.WaitOne(200, true)&& Envir.SocketArgsPool.TryPop(out SendSocketArgs))
            {
                SendSocketArgs.UserToken = this;
                List<byte> data = new List<byte>();
                //byte[] outByte;
                while (_sendList.Count > 0)
                {
                    Packet p = _sendList.Dequeue();
                    data.AddRange(p.GetPacketBytesEx());
                }
                //outByte = new byte[data.Count + ((2 + data.Count) / 3)];

                lock (SendSocketArgs)
                {
                    //*pszNext++ = '#';

                    //memmove(pszNext, pszPacket, nLen);

                    //pszNext += nLen;

                    //*pszNext++ = '!';
                    //*pszNext++ = '$';
                    //*pszNext++ = '\0';
                    //EnDecode.fnEncode6BitBufA(data.ToArray(), SendSocketArgs.Buffer);
                    Buffer.BlockCopy(data.ToArray(), 0, SendSocketArgs.Buffer,0, data.Count);
                    SendSocketArgs.SetBuffer(SendSocketArgs.Offset,data.Count);
                }

                if (!_client.SendAsync(SendSocketArgs))
                    NetWork.ProcessSend(SendSocketArgs);
            }
            //SendData();
        }
        private void ProcessPacket(Packet p)
        {
            if (p == null || Disconnecting) return;

            switch (p.Index)
            {
                case (short)ClientPacketIds.ClientVersion:
                    ClientVersion((C.ClientVersion)p);
                    break;
                case (short)ClientPacketIds.Disconnect:
                    Disconnect(22);
                    break;
                case (short)ClientPacketIds.KeepAlive: // Keep Alive
                    ClientKeepAlive((C.KeepAlive)p);
                    break;
                case (short)ClientPacketIds.NewAccount:
                    //NewAccount((C.NewAccount)p);
                    break;
                case (short)ClientPacketIds.ChangePassword:
                    //ChangePassword((C.ChangePassword)p);
                    break;
                case (short)ClientPacketIds.Login:
                    //Login((C.Login)p);
                    break;
                case (short)ClientPacketIds.NewCharacter:
                    //NewCharacter((C.NewCharacter)p);
                    break;
                case (short)ClientPacketIds.DeleteCharacter:
                    //DeleteCharacter((C.DeleteCharacter)p);
                    break;
                case (short)ClientPacketIds.StartGame:
                    //StartGame((C.StartGame)p);
                    break;
                case (short)ClientPacketIds.LogOut:
                    //LogOut();
                    break;
                case (short)ClientPacketIds.Turn:
                    //Turn((C.Turn)p);
                    break;
                case (short)ClientPacketIds.Walk:
                    //Walk((C.Walk)p);
                    break;
                case (short)ClientPacketIds.Run:
                    //Run((C.Run)p);
                    break;
                case (short)ClientPacketIds.Chat:
                    //Chat((C.Chat)p);
                    break;
                case (short)ClientPacketIds.MoveItem:
                    //MoveItem((C.MoveItem)p);
                    break;
                case (short)ClientPacketIds.StoreItem:
                    //StoreItem((C.StoreItem)p);
                    break;
                case (short)ClientPacketIds.DepositRefineItem:
                    //DepositRefineItem((C.DepositRefineItem)p);
                    break;
                case (short)ClientPacketIds.RetrieveRefineItem:
                    //RetrieveRefineItem((C.RetrieveRefineItem)p);
                    break;
               
                default:
                    //SMain.Enqueue(string.Format("Invalid packet received. Index : {0}", p.Index));
                    break;
            }
        }

        public void SoftDisconnect(byte reason)
        {
            Stage = GameStage.Disconnected;
            TimeDisconnected = Envir.Time;

            lock (Envir.AccountLock)
            {
                //if (Player != null)
                //    Player.StopGame(reason);

                //if (Account != null && Account.Connection == this)
                //    Account.Connection = null;
            }

            //Account = null;
        }
        public void Disconnect(byte reason)
        {
            if (!Connected) return;

            Connected = false;
            Stage = GameStage.Disconnected;
            TimeDisconnected = Envir.Time;

            lock (Envir.Connections)
                Envir.Connections.Remove(this);

            lock (Envir.AccountLock)
            {
                //if (Player != null)
                //    Player.StopGame(reason);

                //if (Account != null && Account.Connection == this)
                //    Account.Connection = null;

            }
            if (_client != null)
            {
                _client.Close();
                _client.Dispose();
            }
            _client = null;
            Envir.SocketArgsPool.Push(SocketArgs);
            //Envir.SocketArgsPool.Push(SendSocketArgs);
            Envir.BufferLock.Release();
            //Account = null;

            _sendList = null;
            _receiveList = null;
            _retryList = null;
            _rawData = null;
        }
        public void SendDisconnect(byte reason)
        {
            if (!Connected)
            {
                Disconnecting = true;
                SoftDisconnect(reason);
                return;
            }

            Disconnecting = true;

            List<byte> data = new List<byte>();

            data.AddRange(new S.Disconnect { Reason = reason }.GetPacketBytes());
            _sendList.Enqueue(new S.Disconnect { Reason = reason });
            //BeginSend(data);
            SoftDisconnect(reason);
        }

        private void ClientVersion(C.ClientVersion p)
        {
            if (Stage != GameStage.None) return;

            if (Envir.CheckVersion)
                if (!Functions.CompareBytes(Envir.VersionHash, p.VersionHash))
                {
                    Disconnecting = true;

                    List<byte> data = new List<byte>();

                    data.AddRange(new S.ClientVersion { Result = 0 }.GetPacketBytes());

                    _sendList.Enqueue(new S.ClientVersion { Result = 0 });
                    //BeginSend(data);
                    SoftDisconnect(10);
                    //SMain.Enqueue(SessionID + ", Disconnnected - Wrong Client Version.");
                    return;
                }

            //SMain.Enqueue(SessionID + ", " + IPAddress + ", Client version matched.");
            Enqueue(new S.ClientVersion { Result = 1 });

            Stage = GameStage.Login;
        }
        private void ClientKeepAlive(C.KeepAlive p)
        {
            Enqueue(new S.KeepAlive
            {
                Time = p.Time
            });
        }
    }
}
