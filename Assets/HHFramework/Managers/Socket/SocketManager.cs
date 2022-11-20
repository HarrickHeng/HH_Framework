using System;
using System.Collections.Generic;
using UnityEngine;

namespace HHFramework
{
    public class SocketManager : ManagerBase, IDisposable
    {
        [Header("每帧最大发送数量")]
        public int MaxSendCount = 5;

        [Header("每次发包最大的字节")]
        public int MaxSendByteCount = 1024;

        [Header("每帧最大处理包数量")]
        public int MaxReceiveCount = 5;

        [Header("心跳间隔")]
        public int HeartbeatInterval = 10;

        /// <summary>
        /// 上次心跳时间
        /// </summary>
        private float mPrevHeartbeatTime = 0;

        /// <summary>
        /// PING值(毫秒)
        /// </summary>
        public int PingValue;

        /// <summary>
        /// 游戏服务器的时间
        /// </summary>
        public long LastServerTime;


        /// <summary>
        /// 是否已经连接到主Socket
        /// </summary>
        private bool mIsConnectToMainSocket = false;

        /// <summary>
        /// 发送用的MS
        /// </summary>
        public MMO_MemoryStream SocketSendMS
        {
            get;
            private set;
        }

        /// <summary>
        /// 接收用的MS
        /// </summary>
        public MMO_MemoryStream SocketReceiveMS
        {
            get;
            private set;
        }
        
        /// <summary>
        /// SocketTcp访问器链表
        /// </summary>
        private readonly LinkedList<SocketTcpRoutine> mSocketTcpRoutineList;

        public SocketManager()
        {
            mSocketTcpRoutineList = new LinkedList<SocketTcpRoutine>();
        }

        /// <summary>
        /// 注册SocketTcp访问器
        /// </summary>
        /// <param name="routine"></param>
        internal void RegisterSocketTcpRoutine(SocketTcpRoutine routine)
        {
            mSocketTcpRoutineList.AddFirst(routine);
        }

        /// <summary>
        /// 移除SocketTcp访问器
        /// </summary>
        /// <param name="routine"></param>
        internal void RemoveSocketTcpRoutine(SocketTcpRoutine routine)
        {
            mSocketTcpRoutineList.Remove(routine);
        }

        internal void OnUpdate()
        {
            for (var curr = mSocketTcpRoutineList.First; curr != null; curr = curr.Next)
            {
                curr.Value.OnUpdate();
            }
        }

        public void Dispose()
        {
            mSocketTcpRoutineList.Clear();
        }
    }
}