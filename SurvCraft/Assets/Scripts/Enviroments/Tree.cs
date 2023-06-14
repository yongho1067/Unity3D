using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class Tree : MonoBehaviour
{
    // ������ ������ �Ȳ����� Ȯ��
    private bool treeisAlive = true;

    // ���� ���� ������
    [SerializeField] private GameObject[] treePieces;
    [SerializeField] private GameObject treeCenter;

    // ���� Ÿ�� ȿ�� (����)
    [SerializeField] private GameObject hit_Effect;

    // ���� ���� ���� �ð�
    [SerializeField] private float hit_Effect_DestroyTime;
    
    // ����� ���� ���� �ð�
    [SerializeField] private float destroyTime;

    // �θ� Ʈ�� �ı��Ǹ� ĸ�� �ݶ��̴� ����
    [SerializeField] private CapsuleCollider parent_Collider;

    // �ڽ� Ʈ�� ������ �� �ʿ��� ������Ʈ Ȱ��ȭ �� �߷� Ȱ��ȭ
    [SerializeField] private CapsuleCollider child_Collider;
    [SerializeField] private Rigidbody child_Rigidbody;

    [SerializeField] private string tree_Hit_Sound;
    [SerializeField] private string tree_Falldown_Sound;
    [SerializeField] private string tree_Change_Sound;

    // ������ �Ѿ�� ���� ����
    [SerializeField] private float force;

    // �ڽ� Ʈ��
    [SerializeField] private GameObject childTree;
    
    // �볪�� ������
    [SerializeField] GameObject log;
    [SerializeField] int log_Count;

    [SerializeField] GameObject instance_TEMP;

    /// <summary>
    /// Ʈ���� ������ ������ �κп� Ÿ�� ����Ʈ ���
    /// </summary>
    public void Damage(Vector3 pos, float angleY)
    {

        AngleCalc(angleY);

        if (CheckTreePieces())
        {
            return;
        }

        FallDownTree();
    }

    /// <summary>
    /// �÷��̾��� ��ġ�� ���� Ÿ���� ��ġ�� �˱� ���� ������ ����ؾ���
    /// </summary>
    private void AngleCalc(float angleY)
    {
        // ����׸� ���� �÷��̾��� ��ġ ������ ���� ���� ��ġ�� TreePiece �迭(0 = 1)�� ������ �°� ����
        if(0 <= angleY && angleY <= 70)
        {
            DestroyPiece(2);
        }
        else if(70 <= angleY && angleY <= 140)
        {
            DestroyPiece(3);
        }
        else if(140 <= angleY && angleY <= 210)
        {
            DestroyPiece(4);
        }
        else if(210<= angleY && angleY <= 280)
        {
            DestroyPiece(0);
        }
        else if(280<= angleY && angleY <= 360)
        {
            DestroyPiece(1);
        }
    }

    /// <summary>
    /// TreePiece�� �÷��̾��� ��ġ�� ���� Ÿ�� �Ǵ� treepiece
    /// </summary>
    private void DestroyPiece(int num)
    {
        if (treePieces[num].gameObject != null)
        {
            SoundManager.soundManager.PlaySE(tree_Hit_Sound);
            
            GameObject instance = Instantiate(hit_Effect, treePieces[num].transform.position, Quaternion.identity, instance_TEMP.transform);
            Destroy(instance, hit_Effect_DestroyTime);
            Destroy(treePieces[num].gameObject);
        }
    }

    /// <summary>
    /// �÷��̾ Ÿ���ϴ� ��ġ�� �����ִ� ���� ������ �ִ��� ������
    /// </summary>
    private bool CheckTreePieces()
    {
        for(int i = 0; i < treePieces.Length; i++)
        {
            if (treePieces[i].gameObject != null)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// ������ hp�� 0�� �Ǿ����� �������� �ϴ� ���
    /// </summary>
    private void FallDownTree()
    {
        if(treeisAlive == true)
        {
            SoundManager.soundManager.PlaySE(tree_Hit_Sound);
            Destroy(treeCenter);
            SoundManager.soundManager.PlaySE(tree_Falldown_Sound);

            parent_Collider.enabled = false;
            child_Collider.enabled = true;
            child_Rigidbody.useGravity = true;

            child_Rigidbody.AddForce(Random.Range(-force, force), 0f, Random.Range(-force, force));

            StartCoroutine(LogCoroutine());


            treeisAlive = false;
        }
    }

    /// <summary>
    /// ������ falldown �� ���� destroyTime ��ŭ�� �ð��� ������ Log ����
    /// </summary>
    /// <returns></returns>
    IEnumerator LogCoroutine()
    {
        bool flag = true;
        int count = 0;
        yield return new WaitForSeconds(destroyTime);

        SoundManager.soundManager.PlaySE(tree_Change_Sound);

        while (flag)
        {
            float log_Instance_Transform = 2f;
            Instantiate(log, childTree.transform.position + (childTree.transform.up * ((float)(log_Instance_Transform + count++))), Quaternion.LookRotation(childTree.transform.up), instance_TEMP.transform);

            if(count >= log_Count)
            {
                flag = false;
            }
        }

        
        Destroy(childTree.gameObject);
    }
}
