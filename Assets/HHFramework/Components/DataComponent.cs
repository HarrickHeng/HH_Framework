namespace HHFramework
{
    /// <summary>
    /// 数据组件
    /// </summary>
    public class DataComponent : HHBaseComponent
    {
        /// <summary>
        /// 临时缓存数据
        /// </summary>
        public CacheData CacheData { get; private set; }

        /// <summary>
        /// 关卡地图数据
        /// </summary>
        public PveMapData PveMapData { get; private set; }

        /// <summary>
        /// 系统相关数据
        /// </summary>
        public SysData SysData { get; private set; }

        /// <summary>
        /// 用户相关数据
        /// </summary>
        public UserData UserData { get; private set; }

        protected override void OnAwake()
        {
            base.OnAwake();
            CacheData = new CacheData();
            PveMapData = new PveMapData();
            SysData = new SysData();
            UserData = new UserData();
        }

        public override void ShutDown()
        {
            CacheData.Dispose();
            PveMapData.Dispose();
            SysData.Dispose();
            UserData.Dispose();
        }
    }
}