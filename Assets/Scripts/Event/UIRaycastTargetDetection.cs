using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRaycastTargetDetection : MonoBehaviour
{
	Vector3[] worldCorners = new Vector3[4];
	private void OnDrawGizmos() {
		foreach (MaskableGraphic maskableGraphic in FindObjectsOfType<MaskableGraphic>()) {
			if (maskableGraphic.raycastTarget) {
				RectTransform rectTransform = maskableGraphic.transform as RectTransform;
				rectTransform.GetWorldCorners(worldCorners);
				Gizmos.color = Color.blue;
				for (int i = 0; i < 4; i++)
					Gizmos.DrawLine(worldCorners[i], worldCorners[(i + 1) % 4]);
			}
		}
	}
}
