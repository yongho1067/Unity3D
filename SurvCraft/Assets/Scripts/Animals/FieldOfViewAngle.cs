using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfViewAngle : MonoBehaviour
{
    [SerializeField] private float viewAngle; // �þ߰�
    [SerializeField] private float viewDistance; // �þ� �Ÿ�
    [SerializeField] private LayerMask targetMask; // Ÿ�� ����ũ(�÷��̾�)

    private void Update()
    {
        View();
    }

    private void View()
    {
        Vector3 leftBound = BoundaryAngle(-viewAngle * 0.5f);
        Vector3 rightBound = BoundaryAngle(viewAngle * 0.5f);

        Debug.DrawRay(transform.position + transform.up, leftBound, Color.red);
        Debug.DrawRay(transform.position + transform.up, rightBound, Color.red);

        Collider[] targets = Physics.OverlapSphere(transform.position, viewDistance, targetMask);

        for (int i = 0; i < targets.Length; i++)
        {
            Transform targetTransfrom = targets[i].transform;
            if(targetTransfrom.name == "Player")
            {

                Vector3 direction = (targetTransfrom.position - transform.position).normalized;
                float angle = Vector3.Angle(direction, transform.forward);

                if(angle < viewAngle * 0.5f)
                {
                    RaycastHit hit;

                    if(Physics.Raycast(transform.position + transform.up, direction, out hit, viewDistance))
                    {
                        Debug.Log("�÷��̾ ���� �þ� ���� �ֽ��ϴ�.");
                        Debug.DrawRay(transform.position + transform.up, direction, Color.blue);
                    }
                }
            }
        }
    }

    private Vector3 BoundaryAngle(float angle)
    {
        angle += transform.eulerAngles.y;
        return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0f, Mathf.Cos(angle * Mathf.Deg2Rad));
    }
}
