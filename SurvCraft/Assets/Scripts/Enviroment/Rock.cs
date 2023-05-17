using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
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
    // ä�� ����Ʈ ���̶�Ű ������
    [SerializeField] private GameObject effect_Prefabs_Temp;

    // ���� ����Ʈ (�ӽ� �����ʿ�)
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip audioClip;
    [SerializeField] private AudioClip audioClip2;


    /// <summary>
    /// ä��
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
    /// Rock�� hp�� 0�� �ɶ� ���� �ı�
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
