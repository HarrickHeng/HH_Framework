using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

namespace HHFramework
{
    /// <summary>
    /// SocketTcp访问器
    /// </summary>
    public class SocketTcpRoutine
    {
        #region 发送消息所需变量

        //发送消息队列
        private Queue<byte[]> mSendQueue = new Queue<byte[]>();

        //压缩数组的长度界限
        private const int mCompressLen = 512;

        #endregion

        //是否连接成功
        private bool mIsDoConnected;
        private bool mIsConnectedSuccess;

        #region 发送接收消息所需变量

        //接收数据包的字节数组缓冲区
        private byte[] mReceiveBuffer = new byte[1024];

        //接收数据包的缓冲数据流
        private MMO_MemoryStream mReceiveMS = new MMO_MemoryStream();

        /// <summary>
        /// 发送用的MS
        /// </summary>
        private MMO_MemoryStream mSocketSendMS = new MMO_MemoryStream();

        /// <summary>
        /// 接收用的MS
        /// </summary>
        private MMO_MemoryStream mSocketReceiveMS = new MMO_MemoryStream();

        //接收消息的队列
        private Queue<byte[]> mReceiveQueue = new Queue<byte[]>();

        private int mReceiveCount = 0;

        /// <summary>
        /// 这一帧发送了多少
        /// </summary>
        private int mSendCount = 0;

        /// <summary>
        /// 是否有未处理的字节
        /// </summary>
        private bool mIsHasUnDealBytes = false;

        /// <summary>
        /// 未处理的字节
        /// </summary>
        private byte[] mUnDealBytes = null;

        #endregion

        /// <summary>
        /// 客户端socket
        /// </summary>
        private Socket mClient;

        public BaseAction<bool> mOnConnectComplete;

        internal void OnUpdate()
        {
            if (mIsDoConnected)
            {
                mIsDoConnected = false;
                //Debug.LogError("mIsConnectedSuccess=" + mIsConnectedSuccess);
                if (!mIsConnectedSuccess)
                {
                    GameEntry.Socket.RemoveSocketTcpRoutine(this);
                }

                mOnConnectComplete?.Invoke(mIsConnectedSuccess);
                //Debug.Log("连接成功");
            }

            if (!mIsConnectedSuccess)
            {
                return;
            }

            #region 从队列中获取数据

            while (true)
            {
                if (mReceiveCount <= GameEntry.Socket.MaxReceiveCount)
                {
                    mReceiveCount++;
                    lock (mReceiveQueue)
                    {
                        if (mReceiveQueue.Count > 0)
                        {
                            //得到队列中的数据包
                            var buffer = mReceiveQueue.Dequeue();

                            var ms = mSocketReceiveMS;
                            ms.SetLength(0);
                            ms.Write(buffer, 0, buffer.Length);
                            ms.Position = 0;

                            var header = (byte)ms.ReadByte();
                            var isCompress = BitUtil.GetBit(header, 0) == 1;
                            var isEncrypt = BitUtil.GetBit(header, 1) == 1;

                            //协议编号
                            var protoId = ms.ReadUShort();
                            var protoCategory = (ProtoCategory)ms.ReadByte();

                            //定义小包体 也就是真正的protobuf协议数据
                            var protoData = new byte[buffer.Length - 4];
                            Array.Copy(buffer, 4, protoData, 0, protoData.Length);
                            if (isEncrypt)
                            {
                                //解密
                                protoData = XXTEAUtil.Decrypt(protoData);
                            }

                            if (isCompress)
                            {
                                //解压
                                protoData = ZlibHelper.DeCompressBytes(protoData);
                            }

                            GameEntry.Event.SocketEvent.Dispatch(protoId, protoData);
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                else
                {
                    mReceiveCount = 0;
                    break;
                }
            }

            #endregion

            CheckSendQueue();
        }

        #region Connect 连接到socket服务器

        /// <summary>
        /// 连接到socket服务器
        /// </summary>
        /// <param name="ip">ip</param>
        /// <param name="port">端口号</param>
        /// <param name="onConnectComplete">连接成功回调</param>
        public void Connect(string ip, int port, BaseAction<bool> onConnectComplete)
        {
            Debug.LogError("Connect ip =" + ip + " port =" + port);
            mOnConnectComplete = onConnectComplete;
            // 如果socket已经存在 并且处于连接中状态 则直接返回
            if (mClient is { Connected: true })
            {
                mOnConnectComplete?.Invoke(false);
                return;
            }

            mIsConnectedSuccess = false;

            var newServerIp = ip;
            var addressFamily = AddressFamily.InterNetwork;

#if UNITY_IPHONE && !UNITY_EDITOR && SDKCHANNEL_APPLE_STORE
        AppleStoreInterface.GetIPv6Type(ip, port.ToString(), out newServerIp, out addressFamily);
#endif

            mClient = new Socket(addressFamily, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                GameEntry.Socket.RegisterSocketTcpRoutine(this);
                mClient.BeginConnect(new IPEndPoint(IPAddress.Parse(newServerIp), port), ConnectCallBack, mClient);
            }
            catch (Exception ex)
            {
                mIsDoConnected = true;
                GameEntry.LogError("连接失败=" + ex.Message);
            }
        }

        private void ConnectCallBack(IAsyncResult ar)
        {
            mIsConnectedSuccess = true;
            mIsDoConnected = true; //这里表示 进行了连接 不管是否成功
            if (mClient.Connected)
            {
                GameEntry.Log(LogCategory.Normal, "socket连接成功");
                ReceiveMsg();
            }
            else
            {
                GameEntry.LogError("socket连接失败");
            }

            mClient.EndConnect(ar);
        }

        #endregion

        /// <summary>
        /// 断开连接
        /// </summary>
        public void DisConnect()
        {
            if (mClient is not { Connected: true }) return;
            mClient.Shutdown(SocketShutdown.Both);
            mClient.Close();
            GameEntry.Socket.RemoveSocketTcpRoutine(this);
        }

        #region CheckSendQueue 检查发送队列

        /// <summary>
        /// 检查发送队列
        /// </summary>
        private void CheckSendQueue()
        {
            while (true)
            {
                if (mSendCount >= GameEntry.Socket.MaxSendCount)
                {
                    // 等待下一帧发送
                    mSendCount = 0;
                    break;
                }

                lock (mSendQueue)
                {
                    if (mSendQueue.Count > 0 || mIsHasUnDealBytes)
                    {
                        var ms = mSocketSendMS;
                        ms.SetLength(0);

                        // 先处理未处理的包
                        if (mIsHasUnDealBytes)
                        {
                            mIsHasUnDealBytes = false;
                            ms.Write(mUnDealBytes, 0, mUnDealBytes.Length);
                        }

                        while (true)
                        {
                            if (mSendQueue.Count == 0)
                            {
                                break;
                            }

                            // 取出一个字节数组
                            var buffer = mSendQueue.Dequeue();

                            if (buffer.Length + ms.Length <= GameEntry.Socket.MaxSendByteCount)
                            {
                                ms.Write(buffer, 0, buffer.Length);
                            }
                            else
                            {
                                // 已经取出了一个要发送的字节数组
                                mUnDealBytes = buffer;
                                mIsHasUnDealBytes = true;
                                break; // 非常重要
                            }
                        }

                        mSendCount++;
                        //Debug.LogError("拼凑了小包数量=" + smallCount);
                        Send(ms.ToArray());
                    }
                    else
                    {
                        //不加这个会死循环
                        break;
                    }
                }
            }
        }

        #endregion

        #region MakeData 封装数据包

        /// <summary>
        /// 封装数据包
        /// </summary>
        /// <param name="proto"></param>
        /// <returns></returns>
        private byte[] MakeData(IProto proto)
        {
            byte header = 0; //byte，1-2-3-4-5-6-7-8 |1-是否压缩|2-是否加密

            var buffer = proto.ToByteArray();
            var isCompress = buffer.Length > mCompressLen;
            if (isCompress)
            {
                // 1.压缩
                header = BitUtil.SetBit(header, 0);
                ZlibHelper.CompressBytes(buffer);
            }

            // 2.加密
            header = BitUtil.SetBit(header, 1);
            buffer = XXTEAUtil.Encrypt(buffer);

            var ms = mSocketSendMS;
            ms.SetLength(0);

            ms.WriteUShort((ushort)(buffer.Length + 4)); //4=header 1 + ProtoId 2 + Category 1

            ms.WriteByte(header);
            ms.WriteUShort(proto.ProtoId);
            ms.WriteByte((byte)proto.Category);

            ms.Write(buffer, 0, buffer.Length);

            var retBuffer = ms.ToArray();
            return retBuffer;
        }

        /// <summary>
        /// 封装数据包
        /// </summary>
        /// <param name="protoId"></param>
        /// <param name="category"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
        private byte[] MakeData(ushort protoId, byte category, byte[] buffer)
        {
            byte header = 0; //byte，1-2-3-4-5-6-7-8 |1-是否压缩|2-是否加密

            bool isCompress = buffer.Length > mCompressLen;
            if (isCompress)
            {
                header = BitUtil.SetBit(header, 0);
                ZlibHelper.CompressBytes(buffer);
            }

            //2.加密
            header = BitUtil.SetBit(header, 1);
            buffer = XXTEAUtil.Encrypt(buffer);

            var ms = this.mSocketSendMS;
            ms.SetLength(0);

            ms.WriteUShort((ushort)(buffer.Length + 4)); //4=header 1 + ProtoId 2 + Category 1

            ms.WriteByte(header);
            ms.WriteUShort(protoId);
            ms.WriteByte(category);

            ms.Write(buffer, 0, buffer.Length);

            var retBuffer = ms.ToArray();
            return retBuffer;
        }

        #endregion

        #region SendMsg 发送消息 把消息加入队列

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="proto"></param>
        public void SendMsg(IProto proto)
        {
            //得到封装后的数据包
            var sendBuffer = MakeData(proto);

            lock (mSendQueue)
            {
                //把数据包加入队列
                mSendQueue.Enqueue(sendBuffer);
            }
        }

        public void SendMsg(ushort protoId, byte category, byte[] buffer)
        {
            //得到封装后的数据包
            byte[] sendBuffer = MakeData(protoId, category, buffer);

            lock (mSendQueue)
            {
                //把数据包加入队列
                mSendQueue.Enqueue(sendBuffer);
            }
        }

        #endregion

        #region Send 真正发送数据包到服务器

        /// <summary>
        /// 真正发送数据包到服务器
        /// </summary>
        /// <param name="buffer"></param>
        private void Send(byte[] buffer)
        {
            mClient.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, SendCallBack, mClient);
        }

        #endregion

        #region SendCallBack 发送数据包的回调

        /// <summary>
        /// 发送数据包的回调
        /// </summary>
        /// <param name="ar"></param>
        private void SendCallBack(IAsyncResult ar)
        {
            if (!ar.CompletedSynchronously) return;
            mClient.EndSend(ar);
        }

        #endregion

        //====================================================

        #region ReceiveMsg 接收数据

        /// <summary>
        /// 接收数据
        /// </summary>
        private void ReceiveMsg()
        {
            //异步接收数据
            mClient.BeginReceive(mReceiveBuffer, 0, mReceiveBuffer.Length, SocketFlags.None, ReceiveCallBack, mClient);
        }

        #endregion

        #region IsSocketConnected 判断socket是否连接

        /// <summary>
        /// 判断socket是否连接
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        bool IsSocketConnected(Socket s)
        {
            return !((s.Poll(1000, SelectMode.SelectRead) && (s.Available == 0)) || !s.Connected);
        }

        #endregion

        #region ReceiveCallBack 接收数据回调

        /// <summary>
        /// 接收数据回调
        /// </summary>
        /// <param name="ar"></param>
        private void ReceiveCallBack(IAsyncResult ar)
        {
            //try
            //{
            if (IsSocketConnected(mClient))
            {
                int len = mClient.EndReceive(ar);

                if (len > 0)
                {
                    //已经接收到数据
                    //把接收到数据 写入缓冲数据流的尾部
                    mReceiveMS.Position = mReceiveMS.Length;

                    //把指定长度的字节 写入数据流
                    mReceiveMS.Write(mReceiveBuffer, 0, len);

                    //如果缓存数据流的长度>2 说明至少有个不完整的包过来了
                    //为什么这里是2 因为我们客户端封装数据包 用的ushort 长度就是2
                    if (mReceiveMS.Length > 2)
                    {
                        //进行循环 拆分数据包
                        while (true)
                        {
                            //把数据流指针位置放在0处
                            mReceiveMS.Position = 0;

                            //currMsgLen = 包体的长度
                            int currMsgLen = mReceiveMS.ReadUShort();

                            //currFullMsgLen 总包的长度=包头长度+包体长度
                            int currFullMsgLen = 2 + currMsgLen;

                            //如果数据流的长度>=整包的长度 说明至少收到了一个完整包
                            if (mReceiveMS.Length >= currFullMsgLen)
                            {
                                //至少收到一个完整包
                                //定义包体的byte[]数组
                                byte[] buffer = new byte[currMsgLen];

                                //把数据流指针放到2的位置 也就是包体的位置
                                mReceiveMS.Position = 2;

                                //把包体读到byte[]数组
                                mReceiveMS.Read(buffer, 0, currMsgLen);

                                lock (mReceiveQueue)
                                {
                                    mReceiveQueue.Enqueue(buffer);
                                }
                                //==============处理剩余字节数组===================

                                //剩余字节长度
                                int remainLen = (int)mReceiveMS.Length - currFullMsgLen;
                                if (remainLen > 0)
                                {
                                    //把指针放在第一个包的尾部
                                    mReceiveMS.Position = currFullMsgLen;

                                    //定义剩余字节数组
                                    byte[] remainBuffer = new byte[remainLen];

                                    //把数据流读到剩余字节数组
                                    mReceiveMS.Read(remainBuffer, 0, remainLen);

                                    //清空数据流
                                    mReceiveMS.Position = 0;
                                    mReceiveMS.SetLength(0);

                                    //把剩余字节数组重新写入数据流
                                    mReceiveMS.Write(remainBuffer, 0, remainBuffer.Length);

                                    remainBuffer = null;
                                }
                                else
                                {
                                    //没有剩余字节

                                    //清空数据流
                                    mReceiveMS.Position = 0;
                                    mReceiveMS.SetLength(0);

                                    break;
                                }
                            }
                            else
                            {
                                //还没有收到完整包
                                break;
                            }
                        }
                    }

                    //进行下一次接收数据包
                    ReceiveMsg();
                }
                else
                {
                    //客户端断开连接
                    GameEntry.Log(LogCategory.Normal, "服务器{0}断开连接", mClient.RemoteEndPoint.ToString());
                }
            }
            //}
            //catch (Exception ex)
            //{
            //    //客户端断开连接
            //    GameEntry.LogError("服务器{0}断开连接 {1}", mClient.RemoteEndPoint.ToString(), ex.Message);
            //}
        }

        #endregion
    }
}