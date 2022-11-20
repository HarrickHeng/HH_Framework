using System;
using PathologicalGames;
using UnityEngine;

namespace HHFramework
{
    /// <summary>
    /// 对象池实体
    /// </summary>
    [Serializable]
    public class GameObjectPoolEntity
    {
        /// <summary>
        /// 对象池编号
        /// </summary>
        [Tooltip("对象池编号")] public byte PoolId;

        /// <summary>
        /// 对象池名称
        /// </summary>
        [Tooltip("对象池名称")] public string PoolName;

        /// <summary>
        /// 是否开启缓存池自动清理模式
        /// </summary>
        [Tooltip("是否开启缓存池自动清理模式")] public bool CullDespawned;

        /// <summary>
        /// 缓存池自动清理 但是始终保留几个对象不清理
        /// </summary>
        [Tooltip("缓存池自动清理 但是始终保留几个对象不清理")] public int CullAbove;

        /// <summary>
        /// 清理时间间隔(秒)
        /// </summary>
        [Tooltip("清理时间间隔(秒)")] public int CullDelay;

        /// <summary>
        /// 每次清理数目
        /// </summary>
        [Tooltip("每次清理数目")] public int CullMaxPerPass;

        /// <summary>
        /// 对应的游戏物体对象池
        /// </summary>
        [Tooltip("对应的游戏物体对象池")] public SpawnPool Pool;
    }
}