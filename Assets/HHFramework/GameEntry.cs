using UnityEngine;

namespace HHFramework
{
    public partial class GameEntry : MonoBehaviour
    {
        private void Start()
        {
            InitBaseComponents();
        }

        private void Update()
        {
            // 循环更新组件
            for (var curr = m_UpdateComponents.First;
                 curr != null;
                 curr = curr.Next)
            {
                curr.Value.OnUpdate();
            }
        }

        /// <summary>
        /// 销毁
        /// </summary>
        private void OnDestroy()
        {
            // 关闭所有基础组件
            for (var curr = MBaseComponents.First;
                 curr != null;
                 curr = curr.Next)
            {
                curr.Value.ShutDown();
            }
        }
    }
}