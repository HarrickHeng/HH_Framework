using System.Collections.Generic;

namespace HHFramework
{
    public class HttpManager : ManagerBase
    {
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
            var http = GameEntry.Pool.DequeueClassObject<HttpRoutine>();
            http.SendData(url, callBack, isPost, dic, timeout);
        }
    }
}