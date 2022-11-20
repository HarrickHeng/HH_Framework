using System;
using System.Collections.Generic;
using HHFramework.Managers.Pool;
using UnityEngine;

namespace HHFramework
{
    /// <summary>
    /// 对象池组件
    /// </summary>
    public class PoolComponent : HHBaseComponent, IUpdateComponent
    {
        public PoolComponent(int mClearInterval, float mNextRunTime, GameObjectPoolEntity[] mGameObjectPoolGroups)
        {
            this.mClearInterval = mClearInterval;
            this.mNextRunTime = mNextRunTime;
            this.mGameObjectPoolGroups = mGameObjectPoolGroups;
        }

        /// <summary>
        /// 对象池管理器
        /// </summary>
        public PoolManager PoolManager { get; private set; }

        #region SetClassObjectResideCount 设置常驻数量

        /// <summary>
        /// 设置常驻数量
        /// </summary>
        /// <param name="count"></param>
        /// <typeparam name="T"></typeparam>
        public void SetClassObjectResideCount<T>(byte count) where T : class
        {
            PoolManager.ClassObjectPool.SetResideCount<T>(count);
        }

        #endregion

        #region DequeueClassObject 取出类对象

        /// <summary>
        /// 取出类对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public T DequeueClassObject<T>() where T : class, new()
        {
            return PoolManager.ClassObjectPool.DequeueClassObject<T>();
        }

        #endregion

        #region EnqueueClassObject 类对象回池

        /// <summary>
        /// 类对象回池
        /// </summary>
        /// <param name="obj"></param>
        public void EnqueueClassObject(object obj)
        {
            PoolManager.ClassObjectPool.EnqueueClassObject(obj);
        }

        #endregion

        #region 变量对象池

        /// <summary>
        /// 变量对象池锁
        /// </summary>
        private object mVarObjectLock;

#if UNITY_EDITOR
        /// <summary>
        /// 变量对象池监视字典
        /// </summary>
        public Dictionary<Type, int> VarObjectInspectorDic;
#endif

        #region DequeueVarObject 取出变量对象

        /// <summary>
        /// 取出变量对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T DequeueVarObject<T>() where T : VariableBase, new()
        {
            lock (mVarObjectLock)
            {
                var item = DequeueClassObject<T>();
#if UNITY_EDITOR
                var t = item.GetType();
                if (VarObjectInspectorDic.ContainsKey(t))
                {
                    VarObjectInspectorDic[t]++;
                }
                else
                {
                    VarObjectInspectorDic[t] = 1;
                }
#endif
                return item;
            }
        }

        #endregion

        #region EnqueueVarObject 变量对象回池

        /// <summary>
        /// 变量对象回池
        /// </summary>
        /// <param name="item"></param>
        public void EnqueueVarObject<T>(T item) where T : VariableBase
        {
            lock (mVarObjectLock)
            {
                EnqueueClassObject(item);
#if UNITY_EDITOR
                var t = item.GetType();
                if (!VarObjectInspectorDic.ContainsKey(t)) return;
                VarObjectInspectorDic[t]--;
                if (VarObjectInspectorDic[t] == 0)
                {
                    VarObjectInspectorDic.Remove(t);
                }
#endif
            }
        }

        #endregion

        #endregion

        protected override void OnAwake()
        {
            base.OnAwake();
            PoolManager = new PoolManager();
            GameEntry.RegisterUpdateComponent(this);
            mNextRunTime = Time.time;

            mVarObjectLock = new object();
#if UNITY_EDITOR
            lock (mVarObjectLock)
            {
                VarObjectInspectorDic = new Dictionary<Type, int>();
            }
#endif
            InitGameObjectPool();
        }

        protected override void OnStart()
        {
            base.OnStart();
            InitClassReside();
        }

        /// <summary>
        /// 初始化常用类常驻数量
        /// </summary>
        private void InitClassReside()
        {
            SetClassObjectResideCount<HttpRoutine>(3);
            SetClassObjectResideCount<Dictionary<string, object>>(3);
        }

        public override void ShutDown()
        {
            PoolManager.Dispose();
        }

        /// <summary>
        /// 释放间隔
        /// </summary>
        [SerializeField] public int mClearInterval;

        private float mNextRunTime;

        public void OnUpdate()
        {
            if (!(Time.time > mNextRunTime + mClearInterval)) return;
            mNextRunTime = Time.time;

            // 释放类对象池
            PoolManager.ClearClassObjectPool();
        }

        #region 游戏物体对象池

        /// <summary>
        /// 对象池分组
        /// </summary>
        [SerializeField] private GameObjectPoolEntity[] mGameObjectPoolGroups;

        /// <summary>
        /// 初始化游戏物体对象池
        /// </summary>
        public void InitGameObjectPool()
        {
            PoolManager.GameObjectPool.Init(mGameObjectPoolGroups, transform).Forget();
        }

        /// <summary>
        /// 从对象池中获取对象
        /// </summary>
        /// <param name="poolId"></param>
        /// <param name="prefab"></param>
        /// <param name="onComplete"></param>
        public void GameObjectSpawn(byte poolId, Transform prefab, Action<Transform> onComplete)
        {
            PoolManager.GameObjectPool.Spawn(poolId, prefab, onComplete);
        }

        /// <summary>
        /// 对象回池
        /// </summary>
        /// <param name="poolId"></param>
        /// <param name="instance"></param>
        public void GameObjectDeSpawn(byte poolId, Transform instance)
        {
            PoolManager.GameObjectPool.DeSpawn(poolId, instance);
        }

        #endregion
    }
}