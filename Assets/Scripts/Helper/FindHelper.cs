using UnityEngine;

namespace Helper
{
    public static class FindHelper
    {
        public static T FindChild<T>(this GameObject go, string name) where T : Component
        {
            var child = go.transform.Find(name);
            if (child != null)
            {
                return child.GetComponent<T>();
            }

            return null;
        }

        public static GameObject FindGameObject(GameObject go, string name)
        {
            Transform tf = go.transform.Find(name);
            if (tf == null)
            {
                return null;
            }
            return tf.gameObject;
        }

        public static Transform FindTransform(GameObject go, string name)
        {
            return go.transform.Find(name);
        }

        public static Canvas FindCanvas(GameObject go, string name)
        {
            return FindChild<Canvas>(go, name);
        }

        public static Canvas GetCanvas(GameObject go)
        {
            return go.GetComponent<Canvas>();
        }

        public static CanvasGroup FindCanvasGroup(GameObject go, string name)
        {
            return FindChild<CanvasGroup>(go, name);
        }

        public static CanvasGroup GetCanvasGroup(GameObject go)
        {
            return go.GetComponent<CanvasGroup>();
        }

        public static RectTransform FindRectTransform(GameObject go, string name)
        {
            return FindChild<RectTransform>(go, name);
        }

        public static RectTransform GetRectTransform(GameObject go)
        {
            return go.GetComponent<RectTransform>();
        }

        public static UnityEngine.UI.Image FindImage(GameObject go, string name)
        {
            return FindChild<UnityEngine.UI.Image>(go, name);
        }

        public static UnityEngine.UI.Image GetImage(GameObject go)
        {
            return go.GetComponent<UnityEngine.UI.Image>();
        }

        public static UnityEngine.UI.RawImage FindRawImage(GameObject go, string name)
        {
            return FindChild<UnityEngine.UI.RawImage>(go, name);
        }

        public static UnityEngine.UI.RawImage GetRawImage(GameObject go)
        {
            return go.GetComponent<UnityEngine.UI.RawImage>();
        }

        public static UnityEngine.UI.Text FindText(GameObject go, string name)
        {
            return FindChild<UnityEngine.UI.Text>(go, name);
        }

        public static UnityEngine.UI.Text GetText(GameObject go)
        {
            return go.GetComponent<UnityEngine.UI.Text>();
        }

        public static UnityEngine.UI.InputField FindInputField(GameObject go, string name)
        {
            return FindChild<UnityEngine.UI.InputField>(go, name);
        }

        public static UnityEngine.UI.InputField GetInputField(GameObject go)
        {
            return go.GetComponent<UnityEngine.UI.InputField>();
        }

        public static UnityEngine.UI.Button FindButton(GameObject go, string name)
        {
            return FindChild<UnityEngine.UI.Button>(go, name);
        }

        public static UnityEngine.UI.Button GetButton(GameObject go)
        {
            return go.GetComponent<UnityEngine.UI.Button>();
        }

        public static UnityEngine.UI.Slider FindSlider(GameObject go, string name)
        {
            return FindChild<UnityEngine.UI.Slider>(go, name);
        }

        public static UnityEngine.UI.Slider GetSlider(GameObject go)
        {
            return go.GetComponent<UnityEngine.UI.Slider>();
        }

        public static UnityEngine.UI.Toggle FindToggle(GameObject go, string name)
        {
            return FindChild<UnityEngine.UI.Toggle>(go, name);
        }

        public static UnityEngine.UI.Toggle GetToggle(GameObject go)
        {
            return go.GetComponent<UnityEngine.UI.Toggle>();
        }

        public static UnityEngine.UI.GridLayoutGroup FindGridLayoutGroup(GameObject go, string name)
        {
            return FindChild<UnityEngine.UI.GridLayoutGroup>(go, name);
        }

        public static UnityEngine.UI.GridLayoutGroup GetGridLayoutGroup(GameObject go)
        {
            return go.GetComponent<UnityEngine.UI.GridLayoutGroup>();
        }

        public static UnityEngine.UI.HorizontalLayoutGroup FindHorizontalLayoutGroup(
            GameObject go,
            string name
        )
        {
            return FindChild<UnityEngine.UI.HorizontalLayoutGroup>(go, name);
        }

        public static UnityEngine.UI.HorizontalLayoutGroup GetHorizontalLayoutGroup(GameObject go)
        {
            return go.GetComponent<UnityEngine.UI.HorizontalLayoutGroup>();
        }

        public static UnityEngine.UI.VerticalLayoutGroup FindVerticalLayoutGroup(
            GameObject go,
            string name
        )
        {
            return FindChild<UnityEngine.UI.VerticalLayoutGroup>(go, name);
        }

        public static UnityEngine.UI.VerticalLayoutGroup GetVerticalLayoutGroup(GameObject go)
        {
            return go.GetComponent<UnityEngine.UI.VerticalLayoutGroup>();
        }

        public static UnityEngine.UI.ScrollRect FindScrollRect(GameObject go, string name)
        {
            return FindChild<UnityEngine.UI.ScrollRect>(go, name);
        }

        public static UnityEngine.UI.ScrollRect GetScrollRect(GameObject go)
        {
            return go.GetComponent<UnityEngine.UI.ScrollRect>();
        }

        public static UnityEngine.UI.ContentSizeFitter FindContentSizeFitter(GameObject go, string name)
        {
            return FindChild<UnityEngine.UI.ContentSizeFitter>(go, name);
        }

        public static UnityEngine.UI.ContentSizeFitter GetContentSizeFitter(GameObject go)
        {
            return go.GetComponent<UnityEngine.UI.ContentSizeFitter>();
        }

        public static UnityEngine.SpriteRenderer FindSpriteRenderer(GameObject go, string name)
        {
            return FindChild<UnityEngine.SpriteRenderer>(go, name);
        }

        public static UnityEngine.SpriteRenderer GetSpriteRenderer(GameObject go)
        {
            return go.GetComponent<UnityEngine.SpriteRenderer>();
        }
    }
}
