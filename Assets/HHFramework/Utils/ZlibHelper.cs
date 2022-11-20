using System.IO;
using System;
using zlib;

/// <summary>
/// 压缩帮助类
/// </summary>
public class ZlibHelper
{
    #region CompressBytes 对原始字节数组进行zlib压缩，得到处理结果字节数组

    /// <summary>
    /// 对原始字节数组进行zlib压缩，得到处理结果字节数组
    /// </summary>
    /// <param name="orgByte">需要被压缩的原始Byte数组数据</param>
    /// <param name="compressRate">压缩率：默认为zlibConst.Z_DEFAULT_COMPRESSION</param>
    /// <returns>压缩后的字节数组，如果出错则返回null</returns>
    public static byte[] CompressBytes(byte[] orgByte, int compressRate = zlibConst.Z_BEST_SPEED)
    {
        if (orgByte == null) return null;

        var orgStream = new MemoryStream(orgByte);
        var compressedStream = new MemoryStream();
        var outZStream = new ZOutputStream(compressedStream, compressRate);
        try
        {
            CopyStream(orgStream, outZStream);
            outZStream.finish(); // 重要！否则结果数据不完整！
            // 程序执行到这里，CompressedStream就是压缩后的数据
            return compressedStream.ToArray();
        }
        catch
        {
            return null;
        }
    }

    #endregion

    #region DeCompressBytes 对经过zlib压缩的数据，进行解密和zlib解压缩，得到原始字节数组

    /// <summary>
    /// 对经过zlib压缩的数据，进行解密和zlib解压缩，得到原始字节数组
    /// </summary>
    /// <param name="compressedBytes">被压缩的Byte数组数据</param>
    /// <returns>解压缩后的字节数组，如果出错则返回null</returns>
    public static byte[] DeCompressBytes(byte[] compressedBytes)
    {
        if (compressedBytes == null) return null;

        var compressedStream = new MemoryStream(compressedBytes);
        var orgStream = new MemoryStream();
        var outZStream = new ZOutputStream(orgStream);
        // 解压缩
        CopyStream(compressedStream, outZStream);
        outZStream.finish(); //重要！
        // 程序执行到这里，OrgStream就是解压缩后的数据
        return orgStream.ToArray();
    }

    #endregion

    #region CompressString 压缩字符串

    /// <summary>
    /// 压缩字符串
    /// </summary>
    /// <param name="sourceString">需要被压缩的字符串</param>
    /// <param name="compressRate"></param>
    /// <returns>压缩后的字符串，如果失败则返回null</returns>
    public static string CompressString(string sourceString, int compressRate = zlibConst.Z_DEFAULT_COMPRESSION)
    {
        var byteSource = System.Text.Encoding.UTF8.GetBytes(sourceString);
        var byteCompress = CompressBytes(byteSource, compressRate);
        return byteCompress != null ? Convert.ToBase64String(byteCompress) : null;
    }

    #endregion

    #region DecompressString 解压字符串

    /// <summary>
    /// 解压字符串
    /// </summary>
    /// <param name="sourceString">需要被解压的字符串</param>
    /// <returns>解压后的字符串，如果处所则返回null</returns>
    public static string DecompressString(string sourceString)
    {
        var byteSource = Convert.FromBase64String(sourceString);
        var byteDecompress = DeCompressBytes(byteSource);
        return byteDecompress != null ? System.Text.Encoding.UTF8.GetString(byteDecompress) : null;
    }

    #endregion

    #region CopyStream 拷贝流

    /// <summary>
    /// 拷贝流
    /// </summary>
    /// <param name="input"></param>
    /// <param name="output"></param>
    private static void CopyStream(Stream input, Stream output)
    {
        var buffer = new byte[2000];
        int len;
        while ((len = input.Read(buffer, 0, 2000)) > 0)
        {
            output.Write(buffer, 0, len);
        }

        output.Flush();
    }

    #endregion

    #region GetStringByGZIPData 将解压缩过的二进制数据转换回字符串

    /// <summary>
    /// 将解压缩过的二进制数据转换回字符串
    /// </summary>
    /// <param name="zipData"></param>
    /// <returns></returns>
    public static string GetStringByGZIPData(byte[] zipData)
    {
        return System.Text.Encoding.UTF8.GetString(zipData);
    }

    #endregion
}