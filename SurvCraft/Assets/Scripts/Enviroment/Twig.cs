using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Twig : MonoBehaviour
{
    // 생성되는 이팩트 하이라키 보관함
    [SerializeField] GameObject effectTemp; 

    // 체력
    [SerializeField] private int hp;

    // 이펙트 삭제 시간
    [SerializeField] private float destroyTime;

    // 타격 이펙트 프리팹
    [SerializeField] private GameObject hit_Effect_prefab;

    // 작은 나뭇가지 조각들
    [SerializeField] private GameObject little_Twig;

    // 타격시 피격당한 반대방향으로 휘어지도록 하기 위해
    private Vector3 originRotation;
    private Vector3 wantedRotation;
    private Vector3 currentRotation;

    // 사운드 이름
    [SerializeField] private string hitSound;
    [SerializeField] private string brokenSound;


    void Start()
    {
        originRotation = transform.rotation.eulerAngles;
        currentRotation = originRotation;
    }
    
    public void Damage(Transform playerTransform)
    {
        hp--;

        Hit();

        StartCoroutine(HitSwayCoroutine(playerTransform));
        if(hp <= 0)
        {
            hp = 0;
            Destruction();
        }
    }

    private void Hit()
    {
        SoundManager.soundManager.PlaySE(hitSound);

        // 박스 콜라이더의 중앙에서 생성
        GameObject instance = Instantiate(hit_Effect_prefab, gameObject.GetComponent<BoxCollider>().bounds.center + (Vector3.up * 0.5f), 
            Quaternion.identity, effectTemp.transform);

        Destroy(instance, destroyTime);
    }

    /// <summary>
    /// 타격시 자연스럽게 플레이어의 반대방향으로 휘어지도록 하는 코루틴
    /// 방향 구하고 방향에 따른 어느 쪽으로 휘어질지 구해서 체크
    /// </summary>
    IEnumerator HitSwayCoroutine(Transform target)
    {
        // 플레이어와 나뭇가지의 바라보는 방향
        Vector3 dir = (target.position - transform.position).normalized;

        // 바라보는 방향의 각도
        Vector3 rotationDir = Quaternion.LookRotation(dir).eulerAngles;

        CheckDir(rotationDir);

        bool flag = true;

        while (flag)
        {
            currentRotation = Vector3.Lerp(currentRotation, wantedRotation, 0.15f);
            transform.rotation = Quaternion.Euler(currentRotation);

            // 근소한 차이를 위한 0.5f
            if(Mathf.Abs(wantedRotation.x - currentRotation.x) <= 0.5f &&Mathf.Abs(wantedRotation.z - currentRotation.z) <= 0.5f)
            {
                flag = false;
            }

            yield return null;
        }

        flag = true;
        wantedRotation = originRotation;

        while (flag)
        {
            currentRotation = Vector3.Lerp(currentRotation, wantedRotation, 0.05f);
            transform.rotation = Quaternion.Euler(currentRotation);

            // 근소한 차이를 위한 0.5f
            if (Mathf.Abs(wantedRotation.x - currentRotation.x) <= 0.5f && Mathf.Abs(wantedRotation.z - currentRotation.z) <= 0.5f)
            {
                flag = false;
            }

            yield return null;
        }
    }

    /// <summary>
    /// 플레이어의 위치에 따른 나뭇가지 타격시 반대로 꺾이게 하는 좌표
    /// </summary>
    private void CheckDir(Vector3 rotationDir)
    {

        if(rotationDir.y > 180)
        {
            if(rotationDir.y > 300)
            {
                wantedRotation = new Vector3(-50f, 0f, -50f);
            }
            else if(rotationDir.y > 240)
            {
                wantedRotation = new Vector3(0f, 0f, -50f);
            }
            else
            {
                wantedRotation = new Vector3(50f, 0f, -50f);
            }
        }
        else if (rotationDir.y <= 180)
        {
            if (rotationDir.y < 60)
            {
                wantedRotation = new Vector3(-50f, 0f, 50f);
            }
            else if (rotationDir.y < 120)
            {
                wantedRotation = new Vector3(0f, 0f, 50f);
            }
            else
            {
                wantedRotation = new Vector3(50f, 0f, 50f);
            }
        }
    }

    private void Destruction()
    {
        SoundManager.soundManager.PlaySE(brokenSound);

        GameObject instance = Instantiate(little_Twig, gameObject.GetComponent<BoxCollider>().bounds.center + (Vector3.up * 0.5f),
            Quaternion.identity, effectTemp.transform);

        GameObject instance2 = Instantiate(little_Twig, gameObject.GetComponent<BoxCollider>().bounds.center - (Vector3.up * 0.5f),
            Quaternion.identity, effectTemp.transform);

        Destroy(gameObject);

        Destroy(instance, destroyTime);
        Destroy(instance2, destroyTime);
    }
}
