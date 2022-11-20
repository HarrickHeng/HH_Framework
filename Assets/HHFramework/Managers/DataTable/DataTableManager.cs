using Cysharp.Threading.Tasks;

namespace HHFramework
{
    /// <summary>
    /// 表格管理器
    /// </summary>
    public class DataTableManager : ManagerBase
    {
        public DataTableManager()
        {
            InitDBModel();
        }

        /// <summary>
        /// 异步加载表格
        /// </summary>
        public async UniTaskVoid LoadDataTableAsync()
        {
            await UniTask.RunOnThreadPool(LoadDataTable);
        }

        /// <summary>
        /// 章表
        /// </summary>
        public ChapterDBModel ChapterDBModel { get; private set; }

        /// <summary>
        /// 初始化DBModel
        /// </summary>
        public void InitDBModel()
        {
            // 实例化每张表
            ChapterDBModel = new ChapterDBModel();
        }

        /// <summary>
        /// 加载表格
        /// </summary>
        public void LoadDataTable()
        {
            // 每张表都加载
            ChapterDBModel.LoadData();

            // 所有表格加载完毕
            GameEntry.Event.CommonEvent.Dispatch(SysEventId.LoadDataTableComplete);
        }

        /// <summary>
        /// 清空表格
        /// </summary>
        public void Clear()
        {
            // 清空每张表
            ChapterDBModel.Clear();
        }
    }
}