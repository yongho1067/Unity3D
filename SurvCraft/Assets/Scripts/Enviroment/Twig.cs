using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Twig : MonoBehaviour
{
    // �����Ǵ� ����Ʈ ���̶�Ű ������
    [SerializeField] GameObject effectTemp; 

    // ü��
    [SerializeField] private int hp;

    // ����Ʈ ���� �ð�
    [SerializeField] private float destroyTime;

    // Ÿ�� ����Ʈ ������
    [SerializeField] private GameObject hit_Effect_prefab;

    // ���� �������� ������
    [SerializeField] private GameObject little_Twig;

    // Ÿ�ݽ� �ǰݴ��� �ݴ�������� �־������� �ϱ� ����
    private Vector3 originRotation;
    private Vector3 wantedRotation;
    private Vector3 currentRotation;

    // ���� �̸�
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

        // �ڽ� �ݶ��̴��� �߾ӿ��� ����
        GameObject instance = Instantiate(hit_Effect_prefab, gameObject.GetComponent<BoxCollider>().bounds.center + (Vector3.up * 0.5f), 
            Quaternion.identity, effectTemp.transform);

        Destroy(instance, destroyTime);
    }

    /// <summary>
    /// Ÿ�ݽ� �ڿ������� �÷��̾��� �ݴ�������� �־������� �ϴ� �ڷ�ƾ
    /// ���� ���ϰ� ���⿡ ���� ��� ������ �־����� ���ؼ� üũ
    /// </summary>
    IEnumerator HitSwayCoroutine(Transform target)
    {
        // �÷��̾�� ���������� �ٶ󺸴� ����
        Vector3 dir = (target.position - transform.position).normalized;

        // �ٶ󺸴� ������ ����
        Vector3 rotationDir = Quaternion.LookRotation(dir).eulerAngles;

        CheckDir(rotationDir);

        bool flag = true;

        while (flag)
        {
            currentRotation = Vector3.Lerp(currentRotation, wantedRotation, 0.15f);
            transform.rotation = Quaternion.Euler(currentRotation);

            // �ټ��� ���̸� ���� 0.5f
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

            // �ټ��� ���̸� ���� 0.5f
            if (Mathf.Abs(wantedRotation.x - currentRotation.x) <= 0.5f && Mathf.Abs(wantedRotation.z - currentRotation.z) <= 0.5f)
            {
                flag = false;
            }

            yield return null;
        }
    }

    /// <summary>
    /// �÷��̾��� ��ġ�� ���� �������� Ÿ�ݽ� �ݴ�� ���̰� �ϴ� ��ǥ
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
