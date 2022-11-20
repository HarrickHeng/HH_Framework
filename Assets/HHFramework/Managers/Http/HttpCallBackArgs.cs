using System;
namespace HHFramework
{
    /// <summary>
    /// Http请求回调数据
    /// </summary>
    public class HttpCallBackArgs : EventArgs
    {
        /// <summary>
        /// 是否有错误
        /// </summary>
        public bool HasError;

        /// <summary>
        /// 返回值
        /// </summary>
        public string Value;
    }
}