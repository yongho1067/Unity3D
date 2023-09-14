using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    [SerializeField] private string fireName; // ���� �̸�, ����, ��ں�, ȭ�Ժ�
    [SerializeField] private int fireDamage; // ���� ������

    [SerializeField] private float damageTime; // �������� �� ������.
    private float currentDamageTime;

    [SerializeField] private float durationTime; // ���� ���ӽð�
    private float currentDurationTime;

    [SerializeField] private ParticleSystem ps_Fire; // ��ƼŬ �ý���

    // �ʿ��� ������Ʈ
    private StatusController statusController;

    // ���� ����
    private bool isFire = true;

    private void Start()
    {
        currentDurationTime = durationTime;
        statusController = FindObjectOfType<StatusController>();
    }

    private void Update()
    {
        if(isFire)
        {
            ElapseTime();
        }
    }

    private void ElapseTime()
    {
        currentDurationTime -= Time.deltaTime;

        if (currentDamageTime > 0)
        {
            currentDamageTime -= Time.deltaTime;
        }

        if (currentDurationTime <= 0)
        {
            // �Ҳ�
            Fire_Off();
        }

        
    }

    private void Fire_Off()
    {
        ps_Fire.Stop();
        isFire = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if(isFire && other.transform.tag == "Player")
        {
            if(currentDamageTime <= 0)
            {
                other.GetComponent<Burn>().StartBunring();
                statusController.DecreaseHP(fireDamage);
                currentDamageTime = damageTime;
            }
            
        }
    }

    public bool GetisFire()
    {
        return isFire;
    }
}
