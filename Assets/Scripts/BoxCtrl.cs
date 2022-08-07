using UnityEngine;

public class BoxCtrl : MonoBehaviour
{
    private Vector3 m_TargetPos = Vector3.zero;

    [SerializeField]
    private float m_Speed = 1.0f;

    void Start() { }

    void Update()
    {
        if (Input.GetMouseButtonUp(0) || Input.touchCount == 1)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo))
            {
                if (
                    hitInfo.collider.gameObject.name.Equals(
                        "Plane",
                        System.StringComparison.CurrentCultureIgnoreCase
                    )
                )
                {
                    m_TargetPos = hitInfo.point;
                }
            }
        }

        if (m_TargetPos != Vector3.zero)
        {
            Debug.DrawLine(Camera.main.transform.position, m_TargetPos, Color.blue);
            if (Vector3.Distance(m_TargetPos, transform.position) > 0.1f)
            {
                transform.LookAt(m_TargetPos);
                // transform.Translate(Vector3.forward * Time.deltaTime * m_Speed, Space.Self);
                transform.position = Vector3.Lerp(
                    transform.position,
                    m_TargetPos,
                    Time.deltaTime * m_Speed
                );
            }
        }
    }
}
