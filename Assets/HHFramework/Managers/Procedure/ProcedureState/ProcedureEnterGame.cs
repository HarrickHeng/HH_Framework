using UnityEngine;

namespace HHFramework
{
    /// <summary>
    /// 检查更新流程
    /// </summary>
    public class ProcedureEnterGame : ProcedureBase
    {
        public override void OnEnter()
        {
            base.OnEnter();

            var vId = GameEntry.Procedure.GetData<VarInt>("vId");
            var name = GameEntry.Procedure.GetData<VarString>("name");
            Debug.Log("vId" + vId.Value);
            Debug.Log("name" + name);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
        }

        public override void OnLeave()
        {
            base.OnLeave();
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
        }
    }
}