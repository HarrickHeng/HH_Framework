using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMgr : MonoBehaviour
{
    private void Awake()
    {
        gameObject.AddComponent<LuaMgr>();
    }

    void Start()
    {
        LuaMgr.Instance.DoString("require 'Download/xLuaLogic/LuaTest'");
    }
}