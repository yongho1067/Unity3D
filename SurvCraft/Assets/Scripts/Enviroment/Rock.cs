using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    // ������ ��� ����
    [SerializeField] private int dropItemcount;
    // ������ ü��
    [SerializeField] private int hp;

    // ���� ���� �ð�
    [SerializeField] private float destroyTime;

    //��ü �ö��̴� ��Ȱ��ȭ �뵵
    [SerializeField] private SphereCollider sphereCollider;

    // �Ϲ� ����
    [SerializeField] private GameObject normalRock;
    // ���� ����
    [SerializeField] private GameObject brokenRock;
    // ä�� ����Ʈ
    [SerializeField] private GameObject effect_Prefab;
    // ����� ������
    [SerializeField] private GameObject rock_item_Prefab;
    // ä�� ����Ʈ ���̶�Ű ������
    [SerializeField] private GameObject effect_Prefabs_Temp;

    // ���� ���ϸ�
    [SerializeField] private string strike_Sound;
    [SerializeField] private string destroy_Sound;

    /// <summary>
    /// ä��
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
    /// Rock�� hp�� 0�� �ɶ� ���� �ı�
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
