using UnityEngine;

namespace HHFramework
{
    /// <summary>
    /// 预加载流程
    /// </summary>
    public class ProcedurePreload : ProcedureBase
    {
        public override void OnEnter()
        {
            base.OnEnter();

            GameEntry.Event.CommonEvent.AddEventListener(SysEventId.LoadDataTableComplete, OnLoadDataTableComplete);
            GameEntry.Event.CommonEvent.AddEventListener(SysEventId.LoadOneDataTableComplete,
                OnLoadOneDataTableComplete);
            GameEntry.DataTable.LoadDataTableAsync();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
        }

        public override void OnLeave()
        {
            base.OnLeave();
            GameEntry.Event.CommonEvent.RemoveEventListener(SysEventId.LoadDataTableComplete, OnLoadDataTableComplete);
            GameEntry.Event.CommonEvent.RemoveEventListener(SysEventId.LoadOneDataTableComplete,
                OnLoadOneDataTableComplete);
        }

        /// <summary>
        /// 所有表格加载完毕
        /// </summary>
        private void OnLoadDataTableComplete(object userdata)
        {
            Debug.Log("所有表格加载完毕");
        }

        /// <summary>
        /// 加载单一表格完毕
        /// </summary>
        /// <param name="userdata">表名</param>
        private void OnLoadOneDataTableComplete(object userdata)
        {
            Debug.Log($"DataTableName = {userdata} 加载完毕");
        }
    }
}