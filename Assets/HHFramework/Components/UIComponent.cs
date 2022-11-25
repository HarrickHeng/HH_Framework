using System;
using UnityEngine;
using UnityEngine.UI;

namespace HHFramework
{
    /// <summary>
    /// UI组件
    /// </summary>
    public class UIComponent : HHBaseComponent, IUpdateComponent
    {
        [Header("标准分辨率宽度")] [SerializeField] private int mStandardWidth = 1280;

        [Header("标准分辨率高度")] [SerializeField] private int mStandardHeight = 720;

        [Header("UI摄像机")] public Camera UICamera;

        [Header("跟画布的缩放")] [SerializeField] private CanvasScaler mUIRootCanvasScaler;

        protected override void OnAwake()
        {
            base.OnAwake();
            GameEntry.RegisterUpdateComponent(this);
        }

        #region UI适配

        /// <summary>
        /// 自动适配缩放
        /// </summary>
        public void AutoCanvasScaler()
        {
            var standardScreen = mStandardWidth / (float)mStandardHeight;
            var currScreen = Screen.width / (float)Screen.height;

            if (currScreen > standardScreen)
            {
                mUIRootCanvasScaler.matchWidthOrHeight = 0;
            }
            else
            {
                mUIRootCanvasScaler.matchWidthOrHeight = standardScreen - currScreen;
            }
        }

        /// <summary>
        /// 设置适配缩放为1
        /// </summary>
        public void SetCanvasScaler()
        {
            mUIRootCanvasScaler.matchWidthOrHeight = 1;
        }

        #endregion

        public override void ShutDown()
        {
        }

        public void OnUpdate()
        {
        }
    }
}