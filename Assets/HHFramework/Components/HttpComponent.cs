using System.Collections.Generic;
using UnityEngine;

namespace HHFramework
{
    /// <summary>
    /// Http组件
    /// </summary>
    public class HttpComponent : HHBaseComponent
    {
        [SerializeField]
        [Header("账号正式服Url")]
        private string mWebAccountUrl;
        
        [SerializeField]
        [Header("账号测试服Url")]
        private string mTestWebAccountUrl;
        
        [SerializeField]
        [Header("是否测试环境")]
        private bool mIsTest;

        /// <summary>
        /// 实际账号服务器Url
        /// </summary>
        public string RealWebAccountUrl => mIsTest ? mTestWebAccountUrl : mWebAccountUrl;

        private HttpManager mHttpManager;

        protected override void OnAwake()
        {
            base.OnAwake();
            mHttpManager = new HttpManager();
        }

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
            mHttpManager.SendData(url, callBack, isPost, dic, timeout);
        }

        public override void ShutDown()
        {
        }
    }
}