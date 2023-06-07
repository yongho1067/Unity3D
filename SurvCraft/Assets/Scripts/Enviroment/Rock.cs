using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    // 아이템 드랍 갯수
    [SerializeField] private int dropItemcount;
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
    // 드랍될 아이템
    [SerializeField] private GameObject rock_item_Prefab;
    // 채굴 이펙트 하이라키 보관함
    [SerializeField] private GameObject effect_Prefabs_Temp;

    // 음원 파일명
    [SerializeField] private string strike_Sound;
    [SerializeField] private string destroy_Sound;

    /// <summary>
    /// 채굴
    /// </summary>
    public void Mining()
    {
        SoundManager.soundManager.PlaySE(strike_Sound);

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
        SoundManager.soundManager.PlaySE(destroy_Sound);

        sphereCollider.enabled = false;

        for(int i = 0; i < dropItemcount; i++)
        {
            Instantiate(rock_item_Prefab, normalRock.transform.position, Quaternion.identity, effect_Prefabs_Temp.transform);
        }

        Destroy(normalRock);

        brokenRock.SetActive(true);
        Destroy(brokenRock, destroyTime);
    }
}
