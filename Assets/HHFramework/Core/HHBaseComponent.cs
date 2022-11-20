namespace HHFramework
{
    public abstract class HHBaseComponent : HHComponent
    {
        protected override void OnAwake()
        {
            base.OnAwake();

            // 把自己加入基础组件列表
            GameEntry.RegisterBaseComponent(this);
        }

        public abstract void ShutDown();
    }
}