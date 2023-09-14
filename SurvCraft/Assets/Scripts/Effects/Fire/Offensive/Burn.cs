using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burn : MonoBehaviour
{
    private bool isBurning = false;
    [SerializeField] private int burnDamage; // 화상 데미지

    [SerializeField] private float damageTime; // 화상 지속 시간
    private float currentDamageTime;

    [SerializeField] private float durationTime;
    private float currentDurationTime;

    [SerializeField] private GameObject fire_prefab; // 화상시 불 프리팹 생성
    private GameObject go_tempFire; // 임시 불 그릇

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
                // 데미지 입힘
                Damage();
            }

            if(currentDurationTime <= 0)
            {
                // 불을 끔
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
