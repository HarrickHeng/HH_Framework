using System.Text.RegularExpressions;
using DG.Tweening;
using UnityEngine.UI;

/// <summary>
/// 1.有字体自动打印功能
/// </summary>
namespace UI.Base
{
    public class MyText : Text
    {
        protected override void Start()
        {
            base.Start();
        }

        protected override void OnDestroy() { }

        //解析
        private char[] JIexiChar(string _text)
        {
            //将字体解析出来，去除<>
            string result = Regex.Replace(_text, @"<.*?>", "");
            result = Regex.Replace(result, @"\n", "");
            return result.ToCharArray();
        }

        public float AutoText(string text, float waitTime = 0.2f)
        {
            if (string.IsNullOrEmpty(text))
                return 0;
            float waitAllTime = JIexiChar(text).Length * waitTime;
            this.DOText(text, waitAllTime).SetEase(Ease.Linear);
            return waitAllTime;
        }
    }
}
