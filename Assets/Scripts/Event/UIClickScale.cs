using UnityEngine;

public class UIClickScale : MonoBehaviour
{
    RectTransform rect;
	public float downScale = 0.9f;

	private float toScale = 1;

	private bool zooming = false;
     void Awake()
    {
        rect = transform as RectTransform;
        UIEventListener.Get(transform.gameObject).AddListener(UIEventListener.UIClickEventType.Down, OnPointerDown);
        UIEventListener.Get(transform.gameObject).AddListener(UIEventListener.UIClickEventType.Up, OnPointerUp);
	}
    private void OnPointerDown(UnityEngine.EventSystems.PointerEventData eventData)
    {
		zooming = true;
		toScale = downScale;
	}

    private void OnPointerUp(UnityEngine.EventSystems.PointerEventData eventData)
    {
		zooming = true;
		toScale = 1;
    }

	private void Update() {
		if (zooming) {
			if (Mathf.Abs(toScale - rect.localScale.x) > 0.01f) {
				rect.localScale = Vector3.Lerp(rect.localScale, Vector3.one * toScale, Time.deltaTime * 20);
			} else {
				zooming = false;
			}
		}
	}
}
