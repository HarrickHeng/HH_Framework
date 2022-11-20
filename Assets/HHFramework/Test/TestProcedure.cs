using UnityEngine;
using HHFramework;
using Random = UnityEngine.Random;

public class TestProcedure : MonoBehaviour
{
    void Start()
    {
    }

    void Update()
    {
        GameEntry.Procedure.ChangeState(ProcedureState.Preload);

        if (Input.GetKeyDown(KeyCode.A))
        {
            GameEntry.Socket.ConnectMainSocket("169.254.93.147", 1038);
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            var lst = GameEntry.DataTable.DataTableManager.ChapterDBModel.GetList();
            var len = lst.Count;

            for (var i = 0; i < len; i++)
            {
                var entity = lst[i];
                Debug.Log($"Id = {entity.Id}");
                Debug.Log($"ChapterName = {entity.ChapterName}");
                Debug.Log($"Big_Pic = {entity.Big_Pic}");
            }
        }


        if (Input.GetKeyDown(KeyCode.C))
        {
            var vId = VarInt.Alloc(Random.Range(0, 50));
            var vName = VarString.Alloc("heng");
            GameEntry.Procedure.SetData("vId", vId);
            GameEntry.Procedure.SetData("name", vName);
            GameEntry.Procedure.ChangeState(ProcedureState.EnterGame);
            vId.Release();
            vName.Release();
        }
    }
}