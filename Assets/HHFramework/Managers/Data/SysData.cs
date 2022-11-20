using System;

namespace HHFramework
{
    /// <summary>
    /// 系统相关数据
    /// 游戏周期内可以不清空
    /// </summary>
    public class SysData : IDisposable
    {
        public long CurrServerTime;
        
        public SysData()
        {
        }

        /// <summary>
        /// 清空数据
        /// </summary>
        public void Clear()
        {
        }

        public void Dispose()
        {
        }
    }
}