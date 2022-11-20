using UnityEngine;

namespace HHFramework
{
    /// <summary>
    /// Socket组件
    /// </summary>
    public class SocketComponent : HHBaseComponent, IUpdateComponent
    {
        public int MaxReceiveCount => mSocketManager.MaxReceiveCount;
        public int MaxSendCount => mSocketManager.MaxSendCount;
        public int MaxSendByteCount => mSocketManager.MaxSendByteCount;

        private SocketManager mSocketManager;

        protected override void OnAwake()
        {
            base.OnAwake();
            GameEntry.RegisterUpdateComponent(this);
            mSocketManager = new SocketManager();
        }

        protected override void OnStart()
        {
            base.OnStart();
            mMainSocket = CreateSocketTcpRoutine();
        }

        /// <summary>
        /// 注册SocketTcp访问器
        /// </summary>
        /// <param name="routine"></param>
        internal void RegisterSocketTcpRoutine(SocketTcpRoutine routine)
        {
            mSocketManager.RegisterSocketTcpRoutine(routine);
        }

        /// <summary>
        /// 移除SocketTcp访问器
        /// </summary>
        /// <param name="routine"></param>
        internal void RemoveSocketTcpRoutine(SocketTcpRoutine routine)
        {
            mSocketManager.RemoveSocketTcpRoutine(routine);
        }

        /// <summary>
        /// 创建SocketTcp访问器
        /// </summary>
        /// <returns></returns>
        public SocketTcpRoutine CreateSocketTcpRoutine()
        {
            return GameEntry.Pool.DequeueClassObject<SocketTcpRoutine>();
        }

        public override void ShutDown()
        {
            mSocketManager.Dispose();

            GameEntry.Pool.EnqueueClassObject(mMainSocket);
        }

        public void OnUpdate()
        {
            mSocketManager.OnUpdate();
        }

        /// <summary>
        /// 主Socket
        /// </summary>
        private SocketTcpRoutine mMainSocket;

        /// <summary>
        /// 连接主Socket
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        public void ConnectMainSocket(string ip, int port)
        {
            mMainSocket.Connect(ip, port, OnConnectComplete);
        }

        private static void OnConnectComplete(bool t1)
        {
            Debug.Log("连接成功！");
        }

        private void OnConnectComplete()
        {
            test();
        }

        private void test()
        {
            
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="proto"></param>
        public void SengMsg(IProto proto)
        {
            mMainSocket.SendMsg(proto);
        }
    }
}