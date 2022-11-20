using UnityEngine;

namespace HHFramework
{
    /// <summary>
    /// 检查更新流程
    /// </summary>
    public class ProcedureCheckVersion : ProcedureBase
    {
        public override void OnEnter()
        {
            base.OnEnter();
            Debug.Log("ProcedureCheckVersion OnEnter");
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            Debug.Log("ProcedureCheckVersion OnUpdate");
        }

        public override void OnLeave()
        {
            base.OnLeave();
            Debug.Log("ProcedureCheckVersion OnLeave");
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            Debug.Log("ProcedureCheckVersion OnDestroy");
        }
    }
}