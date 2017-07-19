using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using Client.MirControls;
using C = ClientPackets;


namespace Client.MirNetwork
{
    static class Network
    {
        private static TcpClient _client;
        public static int ConnectAttempt = 0;
        public static bool Connected;
        public static long TimeOutTime, TimeConnected;

        private static ConcurrentQueue<Packet> _receiveList;
        private static ConcurrentQueue<Packet> _sendList;

        private static ConcurrentQueue<Packet> _tempList=new ConcurrentQueue<Packet>();

        static byte[] _rawData = new byte[0];


        public static void Connect()
        {
            if (_client != null)
                Disconnect();

            ConnectAttempt++;

            _client = new TcpClient {NoDelay = true};
            //_client.Connect(Settings.IPAddress, Settings.Port);// Connection, null);
            _client.BeginConnect(Settings.IPAddress, Settings.Port, Connection, null);
            

        }
        public static void EnqueueTempList(Packet p)
        {
            if (_tempList != null && p != null)
                _tempList.Enqueue(p);
        }
        public static void ConnectionChangeTo(string ip,string port,Packet p=null)
        {
            if (p != null)
                _tempList.Enqueue(p);
            Disconnect();
            Settings.IPAddress = ip;
            Settings.Port =int.Parse(port);
            Connect();
        }
        private static void Connection(IAsyncResult result)
        {
            try
            {
                _client.EndConnect(result);

                if (!_client.Connected)
                {
                    Connect();
                    return;
                }

                _receiveList = new ConcurrentQueue<Packet>();
                if (_tempList.Count > 0)
                {
                    _sendList = _tempList;
                    _tempList = new ConcurrentQueue<Packet>();
                }
                else
                    _sendList = new ConcurrentQueue<Packet>();
                _rawData = new byte[0];

                TimeOutTime = CMain.Time + Settings.TimeOut;
                TimeConnected = CMain.Time;

                BeginReceive();
            }
            catch (SocketException e)
            {
                Connect();
            }
            catch (Exception ex)
            {
                if (Settings.LogErrors) CMain.SaveError(ex.ToString());
                Disconnect();
            }
        }

        private static void BeginReceive()
        {
            if (_client == null || !_client.Connected) return;

            byte[] rawBytes = new byte[8 * 1024];

            try
            {
                _client.Client.BeginReceive(rawBytes, 0, rawBytes.Length, SocketFlags.None, ReceiveData, rawBytes);
            }
            catch
            {
                Disconnect();
            }
        }
        private static void ReceiveData(IAsyncResult result)
        {
            if (_client == null || !_client.Connected) return;

            int dataRead;

            try
            {
                dataRead = _client.Client.EndReceive(result);
            }
            catch(Exception e)
            {
                Disconnect();
                return;
            }

            if (dataRead == 0)
            {
                Disconnect();
            }

            byte[] rawBytes = result.AsyncState as byte[];

            byte[] temp = _rawData;
            _rawData = new byte[dataRead + temp.Length];
            Buffer.BlockCopy(temp, 0, _rawData, 0, temp.Length);
            Buffer.BlockCopy(rawBytes, 0, _rawData, temp.Length, dataRead);

            Packet p;
            while ((p = Packet.ReceivePacketEx(_rawData, out _rawData)) != null)
                _receiveList.Enqueue(p);

            BeginReceive();
        }

        private static void BeginSend(List<byte> data)
        {
            if (_client == null || !_client.Connected || data.Count == 0) return;
            
            try
            {
                _client.Client.BeginSend(data.ToArray(), 0, data.Count, SocketFlags.None, SendData, null);
            }
            catch
            {
                Disconnect();
            }
        }
        private static void SendData(IAsyncResult result)
        {
            try
            {
                _client.Client.EndSend(result);
            }
            catch
            { }
        }


        public static void Disconnect()
        {
            if (_client == null) return;

            _client.Close();

            TimeConnected = 0;
            Connected = false;
            _sendList = null;
            _client = null;

            _receiveList = null;
        }

        public static void Process()
        {
            if (_client == null || !_client.Connected)
            {
                //return;//2017-01-24 test don't aways connect.
                if (Connected)
                {
                    while (_receiveList != null && !_receiveList.IsEmpty)
                    {
                        Packet p;

                        if (!_receiveList.TryDequeue(out p) || p == null) continue;
                        if (!(p is ServerPackets.Disconnect) && !(p is ServerPackets.ClientVersion)) continue;

                        MirScene.ActiveScene.ProcessPacket(p);
                        _receiveList = null;
                        return;
                    }

                    MirMessageBox.Show("Lost connection with the server.", true);
                    Disconnect();
                    return;
                }
                return;
            }

            if (!Connected && TimeConnected > 0 && CMain.Time > TimeConnected + 5000)
            {
                //return;//2017-01-24 test don't aways connect.
                //Disconnect();
                //Connect();
                //return;
            }



            while (_receiveList != null && !_receiveList.IsEmpty)
            {
                Packet p;
                if (!_receiveList.TryDequeue(out p) || p == null) continue;
                MirScene.ActiveScene.ProcessPacket(p);
            }


            //if (CMain.Time > TimeOutTime && _sendList != null && _sendList.IsEmpty)
            //    _sendList.Enqueue(new C.KeepAlive());2017-06-01 test don't aways send packet.

            if (_sendList == null || _sendList.IsEmpty) return;

            TimeOutTime = CMain.Time + Settings.TimeOut;

            List<byte> data = new List<byte>();
            while (!_sendList.IsEmpty)
            {
                Packet p;
                if (!_sendList.TryDequeue(out p)) continue;
                data.AddRange(p.GetPacketBytesEx());
            }




            BeginSend(data);
        }
        
        public static void Enqueue(Packet p)
        {
            if (_sendList != null && p != null)
                _sendList.Enqueue(p);
        }
    }
}
