using System.Collections.Generic;
using UnityEngine;

namespace HHFramework
{
    /// <summary>
    /// 启动流程
    /// </summary>
    public class ProcedureLaunch : ProcedureBase
    {
        public override void OnEnter()
        {
            base.OnEnter();
            // Debug.Log("ProcedureLaunch OnEnter");

            // 登录账号服务器
            var url = GameEntry.Http.RealWebAccountUrl;//+ "/api/init";
            var dic = GameEntry.Pool.DequeueClassObject<Dictionary<string, object>>();
            dic.Clear();
            dic["ChannelId"] = 0;
            dic["InnerVersion"] = 1001;

            GameEntry.Http.SendData(url, OnWebAccountInit, true, dic);
        }

        private void OnWebAccountInit(HttpCallBackArgs args)
        {
            // Debug.LogError("HasError=" + args.HasError);
            // Debug.LogError("Value=" + args.Value);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            // Debug.Log("ProcedureLaunch OnUpdate");
        }

        public override void OnLeave()
        {
            base.OnLeave();
            // Debug.Log("ProcedureLaunch OnLeave");
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            // Debug.Log("ProcedureLaunch OnDestroy");
        }
    }
}