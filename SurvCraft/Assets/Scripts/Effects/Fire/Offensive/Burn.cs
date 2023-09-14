using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burn : MonoBehaviour
{
    private bool isBurning = false;
    [SerializeField] private int burnDamage; // ȭ�� ������

    [SerializeField] private float damageTime; // ȭ�� ���� �ð�
    private float currentDamageTime;

    [SerializeField] private float durationTime;
    private float currentDurationTime;

    [SerializeField] private GameObject fire_prefab; // ȭ��� �� ������ ����
    private GameObject go_tempFire; // �ӽ� �� �׸�

    public void StartBunring()
    {
        if(!isBurning)
        {
            go_tempFire = Instantiate(fire_prefab, transform.position, Quaternion.Euler(new Vector3(-90f,0f,0f)));
            go_tempFire.transform.SetParent(transform);
        }
        isBurning = true;
        currentDurationTime = durationTime;
    }

    private void Update()
    {
        if(isBurning)
        {
            ElapseTime();
        }
    }

    private void ElapseTime()
    {
        if(isBurning)
        {
            currentDurationTime -= Time.deltaTime;

            if (currentDamageTime > 0)
                currentDamageTime -= Time.deltaTime;

            if(currentDamageTime <= 0)
            {
                // ������ ����
                Damage();
            }

            if(currentDurationTime <= 0)
            {
                // ���� ��
                Burn_Off();

            }
        }
    }

    private void Burn_Off()
    {
        isBurning = false;
        Destroy(go_tempFire);

    }

    private void Damage()
    {
        currentDamageTime = damageTime;
        GetComponent<StatusController>().DecreaseHP(burnDamage);
    }


}
