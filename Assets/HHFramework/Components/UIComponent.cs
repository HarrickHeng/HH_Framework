namespace HHFramework
{
    /// <summary>
    /// UI组件
    /// </summary>
    public class UIComponent : HHBaseComponent, IUpdateComponent
    {
        protected override void OnAwake()
        {
            base.OnAwake();
            GameEntry.RegisterUpdateComponent(this);
        }

        public override void ShutDown()
        {
        }

        public void OnUpdate()
        {
        }
    }
}