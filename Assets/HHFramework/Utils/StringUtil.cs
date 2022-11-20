/// <summary>
/// 字符串工具类
/// </summary>
public static class StringUtil
{
    #region IsNullOrEmpty 验证值是否为null

    /// <summary>
    /// 判断对象是否为Null、DBNull、Empty或空白字符串
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool IsNullOrEmpty(string value)
    {
        var retVal = value == null || string.IsNullOrWhiteSpace(value);

        return retVal;
    }

    #endregion

    /// <summary>
    /// 把string类型转换成int
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static int ToInt(string str)
    {
        int.TryParse(str, out var temp);
        return temp;
    }

    /// <summary>
    /// 把string类型转换成long
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static long ToLong(string str)
    {
        long.TryParse(str, out var temp);
        return temp;
    }

    /// <summary>
    /// 把string类型转换成float
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static float ToFloat(string str)
    {
        float.TryParse(str, out var temp);
        return temp;
    }
}