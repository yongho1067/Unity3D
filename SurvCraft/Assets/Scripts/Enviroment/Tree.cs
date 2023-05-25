using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class Tree : MonoBehaviour
{
    // 나무가 꺾인지 안꺾인지 확인
    private bool treeisAlive = true;

    // 깎일 나무 조각들
    [SerializeField] private GameObject[] treePieces;
    [SerializeField] private GameObject treeCenter;

    // 나무 타격 효과 (파편)
    [SerializeField] private GameObject hit_Effect;

    // 나무 파편 제거 시간
    [SerializeField] private float hit_Effect_DestroyTime;
    
    // 벌목시 나무 제거 시간
    [SerializeField] private float destroyTime;

    // 부모 트리 파괴되면 캡슐 콜라이더 제거
    [SerializeField] private CapsuleCollider parent_Collider;

    // 자식 트리 쓰러질 때 필요한 컴포넌트 활성화 및 중력 활성화
    [SerializeField] private CapsuleCollider child_Collider;
    [SerializeField] private Rigidbody child_Rigidbody;

    [SerializeField] private string tree_Hit_Sound;
    [SerializeField] private string tree_Falldown_Sound;
    [SerializeField] private string tree_Change_Sound;

    // 나무가 넘어갈때 힘의 세기
    [SerializeField] private float force;

    // 자식 트리
    [SerializeField] private GameObject childTree;
    
    // 통나무 프리팹
    [SerializeField] GameObject log;
    [SerializeField] int log_Count;

    [SerializeField] GameObject instance_TEMP;

    /// <summary>
    /// 트리와 도끼가 만나는 부분에 타격 이펙트 재생
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
    /// 플레이어의 위치에 따라 타격한 위치를 알기 위해 각도를 계산해야함
    /// </summary>
    private void AngleCalc(float angleY)
    {
        // 디버그를 통해 플레이어의 위치 각도에 따른 값에 위치한 TreePiece 배열(0 = 1)에 순서에 맞게 구현
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
    /// TreePiece와 플레이어의 위치에 따라 타격 되는 treepiece
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
    /// 플레이어가 타격하는 위치에 남아있는 나무 조각이 있는지 없는지
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
    /// 나무의 hp가 0이 되었을때 쓰러지게 하는 기능
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
    /// 나무가 falldown 한 이후 destroyTime 만큼의 시간이 지난후 Log 생성
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
