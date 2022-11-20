using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using LitJson;
using UnityEngine;
using UnityEngine.Networking;

namespace HHFramework
{
    /// <summary>
    /// Http发送数据的回调委托
    /// </summary>
    public delegate void HttpSendDataCallBack(HttpCallBackArgs args);

    /// <summary>
    /// Http访问器
    /// </summary>
    public class HttpRoutine
    {
        #region 属性

        /// <summary>
        /// Http请求回调
        /// </summary>
        private HttpSendDataCallBack mCallBack;

        /// <summary>
        /// Http请求回调数据
        /// </summary>
        private readonly HttpCallBackArgs mCallBackArgs;

        /// <summary>
        /// 是否繁忙
        /// </summary>
        public bool IsBusy { get; private set; }

        #endregion

        public HttpRoutine()
        {
            mCallBackArgs = new HttpCallBackArgs();
        }

        #region SendData 发送Http数据

        /// <summary>
        /// 发送Http数据
        /// </summary>
        /// <param name="url"></param>
        /// <param name="callBack"></param>
        /// <param name="isPost"></param>
        /// <param name="dic"></param>
        /// <param name="timeout">超时(毫秒)</param>
        public void SendData(string url, HttpSendDataCallBack callBack, bool isPost = false,
            Dictionary<string, object> dic = null, int timeout = 5000)
        {
            if (IsBusy) return;

            mCallBack = callBack;

            if (!isPost)
            {
                GetUrl(url, timeout).Forget();
            }
            else
            {
                // web加密
                if (dic != null)
                {
                    // 客户端标识符
                    dic["deviceIdentifier"] = DeviceUtil.DeviceIdentifier;

                    // 设备型号
                    dic["deviceModel"] = DeviceUtil.DeviceModel;

                    var t = GameEntry.Data.SysData.CurrServerTime;
                    // 签名
                    dic["sign"] = EncryptUtil.Md5($"{t}:{DeviceUtil.DeviceIdentifier}");

                    // 时间戳
                    dic["t"] = t;
                }

                var json = string.Empty;
                if (dic != null)
                {
                    json = JsonMapper.ToJson(dic);
                    GameEntry.Pool.EnqueueClassObject(dic);
                }

                PostUrl(url, json, timeout).Forget();
            }
        }

        #endregion

        #region GetUrl Get请求

        /// <summary>
        /// Get请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="timeout">超时(毫秒)</param>
        private async UniTaskVoid GetUrl(string url, int timeout)
        {
            var req = UnityWebRequest.Get(url);

            var cts = new CancellationTokenSource();
            cts.CancelAfterSlim(TimeSpan.FromMilliseconds(timeout));

            var (cancelOrFailed, data) =
                await req.SendWebRequest().WithCancellation(cts.Token).SuppressCancellationThrow();
            Request(cancelOrFailed, data);
        }

        #endregion

        #region PostUrl Post请求

        /// <summary>
        /// Post请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="json"></param>
        /// <param name="timeout">超时(毫秒)</param>
        private async UniTaskVoid PostUrl(string url, string json, int timeout)
        {
            var form = new WWWForm();
            form.AddField("", json);
            var req = UnityWebRequest.Post(url, form);

            var cts = new CancellationTokenSource();
            cts.CancelAfterSlim(TimeSpan.FromMilliseconds(timeout));

            var (cancelOrFailed, data) =
                await req.SendWebRequest().WithCancellation(cts.Token).SuppressCancellationThrow();
            Request(cancelOrFailed, data);
        }

        #endregion

        #region Request 请求服务器

        /// <summary>
        /// 请求服务器
        /// </summary>
        /// <param name="cancelOrFailed"></param>
        /// <param name="data"></param>
        private void Request(bool cancelOrFailed, UnityWebRequest data)
        {
            IsBusy = false;
            if (!cancelOrFailed ||
                data.result is UnityWebRequest.Result.ProtocolError or UnityWebRequest.Result.ConnectionError)
            {
                if (mCallBack != null)
                {
                    mCallBackArgs.HasError = true;
                    mCallBackArgs.Value = data.error;
                    mCallBack(mCallBackArgs);
                }
            }
            else
            {
                if (mCallBack != null)
                {
                    mCallBackArgs.HasError = false;
                    mCallBackArgs.Value = data.downloadHandler.text;
                    mCallBack(mCallBackArgs);
                }
            }

            data.Dispose();

            // 把Http访问器回池
            GameEntry.Pool.EnqueueClassObject(this);
        }

        #endregion
    }
}