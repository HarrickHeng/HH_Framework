using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using PathologicalGames;
using UnityEngine;

namespace HHFramework.Managers.Pool
{
    /// <summary>
    /// 游戏物体对象池
    /// </summary>
    public class GameObjectPool : IDisposable
    {
        /// <summary>
        /// 游戏物体对象池字典
        /// </summary>
        private Dictionary<byte, GameObjectPoolEntity> mSpawnPoolDic;

        public GameObjectPool()
        {
            mSpawnPoolDic = new Dictionary<byte, GameObjectPoolEntity>();
        }

        public void Dispose()
        {
            mSpawnPoolDic.Clear();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="parent"></param>
        public async UniTaskVoid Init(GameObjectPoolEntity[] arr, Transform parent)
        {
            for (int i = 0, len = arr.Length; i < len; i++)
            {
                var entity = arr[i];

                if (entity.Pool != null)
                {
                    UnityEngine.Object.Destroy(entity.Pool.gameObject);
                    await UniTask.Yield();
                    entity.Pool = null;
                }

                // 创建对象池
                var pool = PathologicalGames.PoolManager.Pools.Create(entity.PoolName);
                pool.group.parent = parent;
                pool.group.localPosition = Vector3.zero;
                entity.Pool = pool;

                mSpawnPoolDic[entity.PoolId] = entity;
            }
        }

        /// <summary>
        /// 从对象池中获取对象
        /// </summary>
        /// <param name="poolId"></param>
        /// <param name="prefab"></param>
        /// <param name="onComplete"></param>
        public void Spawn(byte poolId, Transform prefab, Action<Transform> onComplete)
        {
            var entity = mSpawnPoolDic[poolId];

            // 拿到预设池
            var prefabPool = entity.Pool.GetPrefabPool(prefab);

            if (prefabPool == null)
            {
                prefabPool = new PrefabPool(prefab)
                {
                    cullDespawned = entity.CullDespawned,
                    cullAbove = entity.CullAbove,
                    cullDelay = entity.CullDelay,
                    cullMaxPerPass = entity.CullMaxPerPass
                };

                entity.Pool.CreatePrefabPool(prefabPool);
            }

            onComplete?.Invoke(entity.Pool.Spawn(prefab));
        }

        /// <summary>
        /// 对象回池
        /// </summary>
        /// <param name="poolId"></param>
        /// <param name="instance"></param>
        public void DeSpawn(byte poolId, Transform instance)
        {
            var entity = mSpawnPoolDic[poolId];
            entity.Pool.Despawn(instance);
        }
    }
}