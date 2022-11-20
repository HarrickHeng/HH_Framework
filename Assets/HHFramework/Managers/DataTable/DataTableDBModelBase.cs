using System.Collections.Generic;

namespace HHFramework
{
    /// <summary>
    /// 数据表管理基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="P"></typeparam>
    public abstract class DataTableDBModelBase<T, P> where T : class, new() where P : DataTableEntityBase
    {
        protected List<P> mList;
        protected Dictionary<int, P> mDic;

        public DataTableDBModelBase()
        {
            mList = new List<P>();
            mDic = new Dictionary<int, P>();
        }

        #region 需要子类实现的属性或方法

        /// <summary>
        /// 数据表名
        /// </summary>
        public abstract string DataTableName { get; }

        /// <summary>
        /// 加载数据列表
        /// </summary>
        protected abstract void LoadList(MMO_MemoryStream ms);

        #endregion

        #region LoadData 加载数据表数据

        /// <summary>
        /// 加载数据表数据
        /// </summary>
        public void LoadData()
        {
            var buffer =
                ResourceComponent.GetFileBuffer(
                    $"{GameEntry.Resource.LocalFilePath}/Download/DataTable/{DataTableName}.bytes");

            using var ms = new MMO_MemoryStream(buffer);
            LoadList(ms);

            GameEntry.Event.CommonEvent.Dispatch(SysEventId.LoadOneDataTableComplete, DataTableName);
        }

        #endregion

        #region GetList 获取集合

        /// <summary>
        /// 获取集合
        /// </summary>
        /// <returns></returns>
        public List<P> GetList()
        {
            return mList;
        }

        #endregion

        #region Get 根据编号获取实体

        /// <summary>
        /// 根据编号获取实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public P Get(int id)
        {
            return mDic.ContainsKey(id) ? mDic[id] : null;
        }

        #endregion

        public void Clear()
        {
            mList.Clear();
            mDic.Clear();
        }
    }
}