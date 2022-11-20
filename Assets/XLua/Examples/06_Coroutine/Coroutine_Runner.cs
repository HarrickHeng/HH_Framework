using UnityEngine;
using XLua;
using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine.Networking;

namespace XLuaTest
{
    public class Coroutine_Runner : MonoBehaviour
    {
    }


    public static class CoroutineConfig
    {
        [LuaCallCSharp]
        public static List<Type> LuaCallCSharp =>
            new List<Type>()
            {
                typeof(WaitForSeconds),
                typeof(UnityWebRequest)
            };
    }
}
