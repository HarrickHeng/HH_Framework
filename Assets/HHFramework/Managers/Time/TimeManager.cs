using System;
using System.Collections.Generic;

namespace HHFramework
{
    public class TimeManager : ManagerBase , IDisposable
    {
        /// <summary>
        /// 定时器链表
        /// </summary>
        private LinkedList<TimeAction> mTimeActionList;

        public TimeManager()
        {
            mTimeActionList = new LinkedList<TimeAction>();
        }

        /// <summary>
        /// 注册定时器
        /// </summary>
        /// <param name="timeAction"></param>
        internal void RegisterTimeAction(TimeAction timeAction)
        {
            mTimeActionList.AddLast(timeAction);
        }

        /// <summary>
        /// 移除定时器
        /// </summary>
        /// <param name="timeAction"></param>
        internal void RemoveTimeAction(TimeAction timeAction)
        {
            mTimeActionList.Remove(timeAction);
        }

        internal void OnUpdate()
        {
            for (var curr = mTimeActionList.First; curr != null; curr = curr.Next)
            {
                curr.Value.OnUpdate();
            }
        }

        public void Dispose()
        {
            mTimeActionList.Clear();
        }
    }
}