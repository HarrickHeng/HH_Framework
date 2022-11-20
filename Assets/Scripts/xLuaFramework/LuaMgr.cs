using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

/// <summary>
/// Lua环境管理器
/// </summary>
public class LuaMgr : MonoSingleton<LuaMgr>
{
    // 全局Lua引擎
    public static LuaEnv luaEnv;

    private void Awake()
    {
        // 实例化xLua引擎
        luaEnv = new LuaEnv();
        
        // 设置xLua的脚本路径
        luaEnv.DoString(string.Format("package.path = '{0}/?.lua'", Application.dataPath));
    }

    /// <summary>
    /// 执行lua脚本
    /// </summary>
    /// <param name="str"></param>
    public void DoString(string str)
    {
        luaEnv.DoString(str);
    }

    void Start()
    {
        
    }
    
}
