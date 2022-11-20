namespace HHFramework
{
    /// <summary>
    /// 日志管理
    /// </summary>
    public partial class GameEntry
    {
        /// <summary>
        /// 打印日志
        /// </summary>
        /// <param name="message"></param>
        public static void Log(LogCategory catetory, string message, params object[] args)
        {
            switch (catetory)
            {
                default:
                case LogCategory.Normal:
#if DEBUG_LOG_NORMAL && DEBUG_MODEL
                    {
                        StringBuilder sbr = StringHelper.PoolNew();
                        Debug.Log("[youyou]" + (args.Length == 0 ? message : sbr.AppendFormatNoGC(message, args).ToString()));
                        StringHelper.PoolDel(ref sbr);
                    }

#endif
                    break;
                case LogCategory.Procedure:
#if DEBUG_LOG_PROCEDURE && DEBUG_MODEL
                    {
                        StringBuilder sbr = StringHelper.PoolNew();
                        Debug.Log("[youyou]" + string.Format("<color=#ffffff>{0}</color>", args.Length == 0 ? message : sbr.AppendFormatNoGC(message, args).ToString()));
                        StringHelper.PoolDel(ref sbr);
                    }
#endif
                    break;
                case LogCategory.Resource:
#if DEBUG_LOG_RESOURCE && DEBUG_MODEL
                    {
                        StringBuilder sbr = StringHelper.PoolNew();
                        Debug.Log("[youyou]" + string.Format("<color=#ace44a>{0}</color>", args.Length == 0 ? message : sbr.AppendFormatNoGC(message, args).ToString()));
                        StringHelper.PoolDel(ref sbr);
                    }
#endif
                    break;
                case LogCategory.Proto:
#if DEBUG_LOG_PROTO && DEBUG_MODEL
                    {
                        StringBuilder sbr = StringHelper.PoolNew();
                        Debug.Log("[youyou]" + string.Format("<color=#c5e1dc>{0}</color>", args.Length == 0 ? message : sbr.AppendFormatNoGC(message, args).ToString()));
                        StringHelper.PoolDel(ref sbr);
                    }
#endif
                    break;
            }
        }

        /// <summary>
        /// 打印错误日志
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        public static void LogError(string message, params object[] args)
        {
#if DEBUG_LOG_ERROR && DEBUG_MODEL
            StringBuilder sbr = StringHelper.PoolNew();
            Debug.LogError("[youyou]" + (args.Length == 0 ? message : sbr.AppendFormatNoGC(message, args).ToString()));
            StringHelper.PoolDel(ref sbr);
#endif
        }
    }
}