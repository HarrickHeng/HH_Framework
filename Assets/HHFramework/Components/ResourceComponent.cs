using System.IO;
using UnityEngine;

namespace HHFramework
{
    /// <summary>
    /// 资源组件
    /// </summary>
    public class ResourceComponent : HHBaseComponent
    {
        /// <summary>
        /// 本地文件路径
        /// </summary>
        public string LocalFilePath;

        protected override void OnAwake()
        {
            base.OnAwake();

#if DISABLE_ASSETBUNDLE
            LocalFilePath = Application.dataPath;
#else
            LocalFilePath = Application.persistentDataPath;
#endif
        }

        /// <summary>
        /// 读取本地文件到byte数组
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static byte[] GetFileBuffer(string path)
        {
            using var fs = new FileStream(path, FileMode.Open);
            var buffer = new byte[fs.Length];
            fs.Read(buffer, 0, buffer.Length);

            return buffer;
        }

        public override void ShutDown()
        {
        }
    }
}