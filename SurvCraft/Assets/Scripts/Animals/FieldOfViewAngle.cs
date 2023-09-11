using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FieldOfViewAngle : MonoBehaviour
{
    [SerializeField] private float viewAngle; // �þ߰�
    [SerializeField] private float viewDistance; // �þ� �Ÿ�
    [SerializeField] private LayerMask targetMask; // Ÿ�� ����ũ(�÷��̾�)

    private PlayerController playerController;
    private NavMeshAgent nav;

    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        nav = GetComponent<NavMeshAgent>();
    }

    public Vector3 GetTargetPos()
    {
        return playerController.transform.position;
    }

    public bool View()
    {
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
                        if(hit.transform.name == "Player")
                        {
                            Debug.Log("�÷��̾ ���� �þ� ���� �ֽ��ϴ�.");
                            Debug.DrawRay(transform.position + transform.up, direction, Color.blue);
                            return true;
                        }
                        
                    }
                }
            }
            if (playerController.GetRun())
            {
                if(CalcPathLength(playerController.transform.position) <= viewDistance)
                {
                    Debug.Log("������ �ֺ����� �ٰ� �ִ� �÷��̾��� �������� �ľ��߽��ϴ�.");
                    return true;
                }
            }
        }
        return false;
    }

    private float CalcPathLength(Vector3 targetPos)
    {
        NavMeshPath path = new NavMeshPath();
        nav.CalculatePath(targetPos, path);

        Vector3[] wayPoint = new Vector3[path.corners.Length + 2];

        wayPoint[0] = transform.position;
        wayPoint[path.corners.Length + 1] = targetPos;

        float pathLength = 0;

        for (int i = 0; i < path.corners.Length; i++)
        {
            wayPoint[i + 1] = path.corners[i]; //��������Ʈ�� ��θ� ����
            pathLength += Vector3.Distance(wayPoint[i], wayPoint[i + 1]);
        }

        return pathLength;
    }
}
