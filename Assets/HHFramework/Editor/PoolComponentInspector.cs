using UnityEditor;
using UnityEngine;

namespace HHFramework
{
    [CustomEditor(typeof(PoolComponent), true)]
    public class PoolComponentInspector : Editor
    {
        /// <summary>
        /// 释放间隔 属性
        /// </summary>
        private SerializedProperty mClearInterval;

        /// <summary>
        /// 对象池分组 属性
        /// </summary>
        private SerializedProperty mGameObjectPoolGroups;

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            var component = target as PoolComponent;
            if (component == null) return;
            //=================绘制滑动条开始=================
            var clearInterval = (int)EditorGUILayout.Slider("清空对象池间隔(秒)", mClearInterval.intValue, 10, 1800);
            if (clearInterval != mClearInterval.intValue)
            {
                component.mClearInterval = clearInterval;
            }
            else
            {
                mClearInterval.intValue = component.mClearInterval;
            }
            //=================绘制滑动条结束=================
            GUILayout.Space(10);
            //=================类对象池开始===================
            GUILayout.BeginVertical("box");
            GUILayout.BeginHorizontal("box");
            GUILayout.Label("类名");
            GUILayout.Label("池中数量", GUILayout.Width(50));
            GUILayout.Label("常驻数量", GUILayout.Width(50));
            GUILayout.EndHorizontal();

            if (component.PoolManager != null)
            {
                var inspectorDic = component.PoolManager.ClassObjectPool.InspectorDic;
                var classObjectCount = component.PoolManager.ClassObjectPool.ClassObjectCount;
                foreach (var item in inspectorDic)
                {
                    GUILayout.BeginHorizontal("box");
                    GUILayout.Label(item.Key.Name);
                    GUILayout.Label(item.Value.ToString(), GUILayout.Width(50));

                    var key = item.Key.GetHashCode();
                    classObjectCount.TryGetValue(key, out var resideCount);

                    GUILayout.Label(resideCount.ToString(), GUILayout.Width(50));
                    GUILayout.EndHorizontal();
                }
            }
            GUILayout.EndVertical();
            //=================类对象池结束=================
            GUILayout.Space(10);
            //=================变量计数开始=================
            GUILayout.BeginVertical("box");
            GUILayout.BeginHorizontal("box");
            GUILayout.Label("变量");
            GUILayout.Label("数量", GUILayout.Width(50));
            GUILayout.EndHorizontal();

            var varObjectInspectorDic = component.VarObjectInspectorDic;
            if (varObjectInspectorDic != null)
            {
                foreach (var item in varObjectInspectorDic)
                {
                    GUILayout.BeginHorizontal("box");
                    GUILayout.Label(item.Key.Name);
                    GUILayout.Label(item.Value.ToString(), GUILayout.Width(50));
                    GUILayout.EndHorizontal();
                }
            }
            GUILayout.EndVertical();
            //=================变量计数结束=================
            GUILayout.Space(10);
            EditorGUILayout.PropertyField(mGameObjectPoolGroups, true);

            serializedObject.ApplyModifiedProperties();
            // 重绘
            Repaint();
        }

        private void OnEnable()
        {
            // 关联组件属性
            mClearInterval = serializedObject.FindProperty("mClearInterval");
            mGameObjectPoolGroups = serializedObject.FindProperty("mGameObjectPoolGroups");
            serializedObject.ApplyModifiedProperties();
        }
    }
}