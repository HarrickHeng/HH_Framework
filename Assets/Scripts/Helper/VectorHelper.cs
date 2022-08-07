using UnityEngine;

public static class VectorHelper
{
    public static void SetPosition(UnityEngine.Object obj, float x, float y, float z)
    {
        UIHelper.GetTransform(obj).position = new Vector3(x, y, z);
    }

    public static void AddPosition(UnityEngine.Object obj, float x, float y, float z)
    {
        UIHelper.GetTransform(obj).position += new Vector3(x, y, z);
    }

    public static void SetLocalPosition(UnityEngine.Object obj, float x, float y, float z)
    {
        UIHelper.GetTransform(obj).localPosition = new Vector3(x, y, z);
    }

    public static void AddLocalPosition(UnityEngine.Object obj, float x, float y, float z)
    {
        UIHelper.GetTransform(obj).localPosition += new Vector3(x, y, z);
    }

    public static void SetEulerAngles(UnityEngine.Object obj, float x, float y, float z)
    {
        UIHelper.GetTransform(obj).eulerAngles = new Vector3(x, y, z);
    }

    public static void AddEulerAngles(UnityEngine.Object obj, float x, float y, float z)
    {
        UIHelper.GetTransform(obj).eulerAngles += new Vector3(x, y, z);
    }

    public static void SetLocalEulerAngles(UnityEngine.Object obj, float x, float y, float z)
    {
        UIHelper.GetTransform(obj).localEulerAngles = new Vector3(x, y, z);
    }

    public static void AddLocalEulerAngles(UnityEngine.Object obj, float x, float y, float z)
    {
        UIHelper.GetTransform(obj).localEulerAngles += new Vector3(x, y, z);
    }

    public static void SetRotation(UnityEngine.Object obj, float x, float y, float z, float w)
    {
        Quaternion qu = new Quaternion(x, y, z, w);
        UIHelper.GetTransform(obj).rotation = qu;
    }

    public static void AddRotation(UnityEngine.Object obj, float x, float y, float z, float w)
    {
        Transform tf = UIHelper.GetTransform(obj);
        Quaternion a = tf.rotation;
        tf.rotation = new Quaternion(a.x + x, a.y + y, a.z + z, a.w + w);
    }

    public static void SetLocalScale(UnityEngine.Object obj, float v)
    {
        UIHelper.GetTransform(obj).localScale = Vector3.one * v;
    }

    public static void AddLocalScale(UnityEngine.Object obj, float v)
    {
        UIHelper.GetTransform(obj).localScale += Vector3.one * v;
    }

    public static void SetLocalScale(UnityEngine.Object obj, float x, float y, float z)
    {
        UIHelper.GetTransform(obj).localScale = new Vector3(x, y, z);
    }

    public static void AddLocalScale(UnityEngine.Object obj, float x, float y, float z)
    {
        UIHelper.GetTransform(obj).localScale += new Vector3(x, y, z);
    }

    public static void SetSpriteRenderColor(
        UnityEngine.SpriteRenderer sr,
        float r,
        float g,
        float b,
        float a
    )
    {
        sr.color = new Color(r, g, b, a);
    }

    public static void AddSpriteRenderColor(
        UnityEngine.SpriteRenderer sr,
        float r,
        float g,
        float b,
        float a
    )
    {
        sr.color = new Color(sr.color.r + r, sr.color.g + g, sr.color.b + b, sr.color.a + a);
    }

    public static void SetSpriteRenderAlpha(UnityEngine.SpriteRenderer sr, float a)
    {
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, a);
    }

    public static void AddSpriteRenderAlpha(UnityEngine.SpriteRenderer sr, float a)
    {
        sr.color += new Color(sr.color.r, sr.color.g, sr.color.b, a);
    }
}
