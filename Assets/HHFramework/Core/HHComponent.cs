using System;
using UnityEngine;

namespace HHFramework
{
    /// <summary>
    /// 组件基类
    /// </summary>
    public class HHComponent : MonoBehaviour
    {
        // 组件实例编号
        private int mInstanceId;

        private void Awake()
        {
            mInstanceId = GetInstanceID();

            OnAwake();
        }

        private void Start()
        {
            OnStart();
        }

        #region 组件实例编号

        public int InstanceId()
        {
            return mInstanceId;
        }

        #endregion

        protected virtual void OnAwake()
        {
        }

        protected virtual void OnStart()
        {
        }
    }
}