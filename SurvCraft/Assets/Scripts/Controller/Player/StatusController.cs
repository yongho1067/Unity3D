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

    #region 배고픔 관련
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
            currentHungry = 0;
            Debug.Log("배고픔 수치가 0이 되었습니다.");
        }
    }

    public void IncreaseHungry(int count)
    {
        if (currentHungry + count < hungry)
        {
            currentHungry += count;
        }
        else
        {
            currentHungry = hungry;
        }
    }

    public void DecreaseHungry(int count)
    {
        if(currentHungry - count < 0)
        {
            currentHungry = 0;
        }
        else
        { 
            currentHungry -= count;
        }
    }
    #endregion

    #region 목마름 관련
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
            currentThirsty = 0;
            Debug.Log("목마름 수치가 0이 되었습니다.");
        }
    }
    public void IncreaseThirsty(int count)
    {
        if (currentThirsty + count < thirsty)
        {
            currentThirsty += count;
        }
        else
        {
            currentThirsty = thirsty;
        }
    }

    public void DecreaseThirsty(int count)
    {
        if (currentThirsty - count < 0)
        {
            currentThirsty = 0;
        }
        else
        {
            currentThirsty -= count;
        }
    }

    #endregion

    private void GaugeUpdate()
    {
        status[HP].fillAmount = (float)currentHp / hp;
        status[SP].fillAmount = (float)currentSp / sp;
        status[DP].fillAmount = (float)currentDp / dp;
        status[HUNGRY].fillAmount = (float)currentHungry / hungry;
        status[THIRSTY].fillAmount = (float)currentThirsty / thirsty;
        status[SATISFY].fillAmount = (float)currentSatisfy / satisfy;
    }

    #region HP 관련
    public void IncreaseHP(int count)
    {
        if(currentHp + count < hp)
        {
            currentHp += count;
        }
        else
        {
            currentHp = hp;
        }
    }

    /// <summary>
    /// 방어력이 우선 적으로 차감되도록
    /// </summary>
    public void DecreaseHP(int count)
    {
        if(currentDp > 0)
        {
            DecreaseDP(count);
            return;
        }

        currentHp -= count;

        if(currentHp <= 0)
        {
            currentHp = 0;
            Debug.Log("캐릭터의 HP가 0이 되었습니다.");
        }
    }
    #endregion

    #region DP (방어력) 관련
    public void IncreaseDP(int count)
    {
        if(currentDp + count < dp)
        {
            currentDp += count;
        }
        else
        {
            currentDp = dp;
        }
    }

    public void DecreaseDP(int count)
    {
        currentDp -= count;

        if(currentDp <= 0)
        {
            currentDp = 0;
            Debug.Log("캐릭터의 DP가 0이 되었습니다.");
        }
    }
    #endregion

    #region 스태미너 관련
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

    public int GetCurrentSP() 
    {
        return currentSp;
    
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
    #endregion
}
