using System;
using System.Collections.Generic;
using netWorkTest.MirNetwork;
using System.Net.Sockets;
using System.Threading;
using System.Collections.Concurrent;

namespace netWorkTest
{
    class Envir
    {
        public static int _sessionID = 0;
        public static List<MirConnectionSimply> Connections = new List<MirConnectionSimply>();
        public const int MaxIP = 5;
        public const int TimeOut = 5000;
        public static long Time { get { return DateTime.Now.Ticks; } }
        public static Object AccountLock = new Object();
        public static bool CheckVersion = false;
        public static byte[] VersionHash=null;

        public static int BuffSzie = 2048;
        public static ConcurrentStack<SocketAsyncEventArgs>  SocketArgsPool;
        
        public static SocketAsyncEventArgs AccSocketArgs;

        public static Semaphore BufferLock = new Semaphore(Envir.MaxIP * 2, Envir.MaxIP * 2);
        public enum STEP {UNLOGIN,LOGINING,LOGINED,SELSERVER }
        public static STEP curStep = STEP.UNLOGIN;
    }
}
