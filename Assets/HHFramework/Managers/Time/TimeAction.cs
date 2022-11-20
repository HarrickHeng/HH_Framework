using System;
using UnityEngine;

namespace HHFramework
{
    /// <summary>
    /// 定时器
    /// </summary>
    public class TimeAction
    {
        /// <summary>
        /// 是否运行中
        /// </summary>
        public bool IsRunning { get; private set; }

        /// <summary>
        /// 是否暂停
        /// </summary>
        private bool mIsPause = false;

        /// <summary>
        /// 当前运行时间
        /// </summary>
        private float mCurrRunTime;

        /// <summary>
        /// 循环次数(-1表示 无限循环)
        /// </summary>
        private int mLoop;

        /// <summary>
        /// 当前循环次数
        /// </summary>
        private int mCurrLoop;

        /// <summary>
        /// 延迟时间
        /// </summary>
        private float mDelayTime;

        /// <summary>
        /// 间隔(秒)
        /// </summary>
        private float mInterval;

        /// <summary>
        /// 最后暂停时间
        /// </summary>
        private float mLastPauseTime;

        /// <summary>
        /// 暂停了多久
        /// </summary>
        private float mPauseTime;

        /// <summary>
        /// 开始运行
        /// </summary>
        private Action mOnStart;

        /// <summary>
        /// 运行中 回调参数表示剩余次数
        /// </summary>
        private Action<int> mOnUpdate;

        /// <summary>
        /// 运行完成
        /// </summary>
        private Action mOnComplete;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="delayTime">延迟时间</param>
        /// <param name="interval">间隔(秒)</param>
        /// <param name="loop">循环次数(-1表示 无限循环)</param>
        /// <param name="onStart">开始运行</param>
        /// <param name="onUpdate">运行中 回调参数表示剩余次数</param>
        /// <param name="onComplete">运行完成</param>
        /// <returns></returns>
        public TimeAction Init(float delayTime, float interval, int loop, Action onStart, Action<int> onUpdate,
            Action onComplete)
        {
            mDelayTime = delayTime;
            mInterval = interval;
            mLoop = loop;
            mOnStart = onStart;
            mOnUpdate = onUpdate;
            mOnComplete = onComplete;
            return this;
        }

        /// <summary>
        /// 运行
        /// </summary>
        public void Run()
        {
            // 注册进定时器链表
            GameEntry.Time.RegisterTimeAction(this);
            // 设置当前时间
            mCurrRunTime = Time.time;

            mIsPause = false;
        }

        /// <summary>
        /// 暂停
        /// </summary>
        public void Pause()
        {
            mLastPauseTime = Time.time;
            mIsPause = true;
            IsRunning = false;
        }

        /// <summary>
        /// 恢复
        /// </summary>
        public void Resume()
        {
            mIsPause = false;

            mPauseTime = Time.time - mLastPauseTime;
        }

        /// <summary>
        /// 停止
        /// </summary>
        public void Stop()
        {
            mOnComplete?.Invoke();

            IsRunning = false;

            // 把定时器从链表移除
            GameEntry.Time.RemoveTimeAction(this);
        }

        /// <summary>
        /// 每帧执行
        /// </summary>
        public void OnUpdate()
        {
            if (mIsPause) return;

            if (Time.time > mCurrRunTime + mPauseTime + mDelayTime)
            {
                // 当程序执行与此 表示第一次过了延迟时间
                IsRunning = true;
                mCurrRunTime = Time.time;
                mPauseTime = 0;

                mOnStart?.Invoke();
            }

            if (!IsRunning) return;

            if (Time.time > mCurrRunTime + mPauseTime)
            {
                mCurrRunTime = Time.time + mInterval;

                // 以下代码 间隔mInterval时间执行一次
                mOnUpdate?.Invoke(mLoop - mCurrLoop);

                if (mLoop > -1)
                {
                    mCurrLoop++;
                    if (mCurrLoop >= mLoop)
                    {
                        Stop();
                    }
                }
            }
        }
    }
}