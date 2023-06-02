using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusController : MonoBehaviour
{
    #region Status
    // 최대 체력
    [SerializeField] private int hp;
    private int currentHp;

    // 최대 스태미너
    [SerializeField] private int sp;
    private int currentSp;
    [SerializeField] private int increaseSpeedSp;

    // 스태미너 회복 딜레이
    [SerializeField] private int spRechargeDealy;
    private int currentSpRechargeTime;

    // 스태미너 감소 여부
    private bool spUsed;

    // 최대 방어력
    [SerializeField] private int dp;
    private int currentDp;

    // 최대 포만감
    [SerializeField] private int hungry;
    private int currentHungry;

    // 포만감이 줄어드는 속도
    [SerializeField] private int hungryDecreaseTime;
    private int currentHungryDecreaseTime;

    // 최대 목마름
    [SerializeField] private int thirsty;
    private int currentThirsty;

    // 목마름이 줄어드는 속도
    [SerializeField] private int thirstyDecreaseTime;
    private int currentThirstyDecreaseTime;

    // 최대 만족도
    [SerializeField] int satisfy;
    private int currentSatisfy;
    #endregion

    // 필요한 이미지
    [SerializeField] private Image[] status;

    private const int HP = 0, DP = 1, SP = 2, HUNGRY = 3, THIRSTY = 4, SATISFY = 5;

    void Start()
    {
        currentHp = hp;
        currentSp = sp;
        currentDp = dp;
        currentHungry = hungry;
        currentThirsty = thirsty;
        currentSatisfy = satisfy;
    }

    void Update()
    {
        Hungry();
        Thirsty();
        GaugeUpdate();
        RechargingStamina();
    }

    private void Hungry()
    {
        if(currentHungry > 0)
        {
            if(currentHungryDecreaseTime <= hungryDecreaseTime)
            {
                currentHungryDecreaseTime++;
            }
            else
            {
                currentHungryDecreaseTime = 0;
                currentHungry--;
            }
        }
        else
        {
            Debug.Log("배고픔 수치가 0이 되었습니다.");
        }
    }

    private void Thirsty()
    {
        if (currentThirsty > 0)
        {
            if (currentThirstyDecreaseTime <= thirstyDecreaseTime)
            {
                currentThirstyDecreaseTime++;
            }
            else
            {
                currentThirstyDecreaseTime = 0;
                currentThirsty--;
            }
        }
        else
        {
            Debug.Log("목마름 수치가 0이 되었습니다.");
        }
    }

    private void GaugeUpdate()
    {
        status[HP].fillAmount = (float)currentHp / hp;
        status[SP].fillAmount = (float)currentSp / sp;
        status[DP].fillAmount = (float)currentDp / dp;
        status[HUNGRY].fillAmount = (float)currentHungry / hungry;
        status[THIRSTY].fillAmount = (float)currentThirsty / thirsty;
        status[SATISFY].fillAmount = (float)currentSatisfy / satisfy;
    }

    public void DecreaseStamina(int count)
    {
        spUsed = true;
        currentSpRechargeTime = 0;

        if(currentSp - count > 0)
        {
            currentSp -= count;
        }
        else
        {
            currentSp = 0;
        }
    }

    private void RechargingStamina()
    {
        RecoverStaminaDelay();
        RecoverStamina();
    }

    private void RecoverStaminaDelay()
    {
        if (spUsed)
        {
            if(currentSpRechargeTime < spRechargeDealy)
            {
                currentSpRechargeTime++;
            }
            else
            {
                spUsed = false;
            }
        }
    }

    private void RecoverStamina()
    {
        if (!spUsed && currentSp < sp)
        {
            currentSp += increaseSpeedSp;
        }
    }
}
