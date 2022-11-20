using Cysharp.Threading.Tasks;
using HHFramework;
using UnityEngine;

public class TestPool : MonoBehaviour
{
    public Transform trans1;
    public Transform trans2;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            CreateObj().Forget();
        }
        
        if (Input.GetKeyDown(KeyCode.C))
        {
            GameEntry.Pool.InitGameObjectPool();
        }
    }

    private async UniTask CreateObj()
    {
        for (var i = 0; i < 20; i++)
        {
            await UniTask.Delay(500);
            GameEntry.Pool.GameObjectSpawn(1, trans1, (instance) =>
            {
                Debug.Log("trans1" + instance.GetHashCode());
                instance.transform.localPosition += new Vector3(0, 0, i * 2);
                instance.gameObject.SetActive(true);
                DeSpawn(1, instance).Forget();
            });

            GameEntry.Pool.GameObjectSpawn(2, trans2, (instance) =>
            {
                Debug.Log("trans2" + instance.GetHashCode());
                instance.transform.localPosition += new Vector3(0, 5, i * 2);
                instance.gameObject.SetActive(true);
                DeSpawn(2, instance).Forget();
            });
        }
    }

    private async UniTask DeSpawn(byte poolId, Transform instance)
    {
        await UniTask.Delay(20000);
        GameEntry.Pool.GameObjectDeSpawn(poolId, instance);
    }
}