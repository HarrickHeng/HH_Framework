using System.IO;
using System;

public class IOUtil
{
    /// <summary>
    /// 读取文本文件
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public static string GetFileText(string filePath)
    {
        var content = string.Empty;

        if (!File.Exists(filePath))
        {
            return content;
        }

        using var sr = File.OpenText(filePath);
        content = sr.ReadToEnd();

        return content;
    }

    #region CreateTextFile 创建文本文件

    /// <summary>
    /// 创建文本文件
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="content"></param>
    public static void CreateTextFile(string filePath, string content)
    {
        DeleteFile(filePath);

        using (FileStream fs = File.Create(filePath))
        {
            using (StreamWriter sw = new StreamWriter(fs))
            {
                sw.Write(content.ToString());
            }
        }
    }

    #endregion

    #region DeleteFile 删除文件

    /// <summary>
    /// 删除文件
    /// </summary>
    /// <param name="filePath"></param>
    public static void DeleteFile(string filePath)
    {
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }

    #endregion

    #region CopyDirectory 拷贝文件夹

    /// <summary>
    /// 拷贝文件夹
    /// </summary>
    /// <param name="sourceDirName"></param>
    /// <param name="destDirName"></param>
    public static void CopyDirectory(string sourceDirName, string destDirName)
    {
        var dirs = Directory.GetDirectories(sourceDirName);
        try
        {
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
                File.SetAttributes(destDirName, File.GetAttributes(sourceDirName));
            }

            if (destDirName[^1] != Path.DirectorySeparatorChar)
                destDirName = destDirName + Path.DirectorySeparatorChar;

            var files = Directory.GetFiles(sourceDirName);
            foreach (var file in files)
            {
                if (File.Exists(destDirName + Path.GetFileName(file)))
                    continue;
                var fileInfo = new FileInfo(file);
                if (fileInfo.Extension.Equals(".meta", StringComparison.CurrentCultureIgnoreCase)
                    || fileInfo.Extension.Equals(".manifest", StringComparison.CurrentCultureIgnoreCase)
                   )
                    continue;

                File.Copy(file, destDirName + Path.GetFileName(file), true);
                File.SetAttributes(destDirName + Path.GetFileName(file), FileAttributes.Normal);
            }

            foreach (var dir in dirs)
            {
                CopyDirectory(dir, destDirName + Path.GetFileName(dir));
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    #endregion

    #region GetFileBuffer 读取本地文件到byte数组

    /// <summary>
    /// 读取本地文件到byte数组
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static byte[] GetFileBuffer(string path)
    {
        if (!File.Exists(path))
        {
            return null;
        }

        byte[] buffer = null;

        using var fs = new FileStream(path, FileMode.Open);
        buffer = new byte[fs.Length];
        fs.Read(buffer, 0, buffer.Length);

        return buffer;
    }

    #endregion
}