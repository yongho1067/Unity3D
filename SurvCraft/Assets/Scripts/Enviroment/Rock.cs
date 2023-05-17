using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    // 바위의 체력
    [SerializeField] private int hp;

    // 파편 제거 시간
    [SerializeField] private float destroyTime;

    //구체 컬라이더 비활성화 용도
    [SerializeField] private SphereCollider sphereCollider;

    // 일반 바위
    [SerializeField] private GameObject normalRock;
    // 깨진 바위
    [SerializeField] private GameObject brokenRock;
    // 채굴 이펙트
    [SerializeField] private GameObject effect_Prefab;
    // 채굴 이펙트 하이라키 보관함
    [SerializeField] private GameObject effect_Prefabs_Temp;

    // 사운드 이펙트 (임시 수정필요)
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip audioClip;
    [SerializeField] private AudioClip audioClip2;


    /// <summary>
    /// 채굴
    /// </summary>
    public void Mining()
    {
        audioSource.clip = audioClip;
        audioSource.Play();

        GameObject instance = Instantiate(effect_Prefab, sphereCollider.bounds.center, Quaternion.identity, effect_Prefabs_Temp.transform);
        Destroy(instance, destroyTime);

        hp--;
        if(hp <= 0)
        {
            hp = 0;
            DestroyRock();
        }
    }
    /// <summary>
    /// Rock의 hp가 0이 될때 바위 파괴
    /// </summary>
    private void DestroyRock()
    {
        audioSource.clip = audioClip2;
        audioSource.Play();

        sphereCollider.enabled = false;
        Destroy(normalRock);

        brokenRock.SetActive(true);
        Destroy(brokenRock, destroyTime);
    }
}
