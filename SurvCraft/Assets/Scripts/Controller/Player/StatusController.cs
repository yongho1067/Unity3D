using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusController : MonoBehaviour
{
    #region Status
    // �ִ� ü��
    [SerializeField] private int hp;
    private int currentHp;

    // �ִ� ���¹̳�
    [SerializeField] private int sp;
    private int currentSp;
    [SerializeField] private int increaseSpeedSp;

    // ���¹̳� ȸ�� ������
    [SerializeField] private int spRechargeDealy;
    private int currentSpRechargeTime;

    // ���¹̳� ���� ����
    private bool spUsed;

    // �ִ� ����
    [SerializeField] private int dp;
    private int currentDp;

    // �ִ� ������
    [SerializeField] private int hungry;
    private int currentHungry;

    // �������� �پ��� �ӵ�
    [SerializeField] private int hungryDecreaseTime;
    private int currentHungryDecreaseTime;

    // �ִ� �񸶸�
    [SerializeField] private int thirsty;
    private int currentThirsty;

    // �񸶸��� �پ��� �ӵ�
    [SerializeField] private int thirstyDecreaseTime;
    private int currentThirstyDecreaseTime;

    // �ִ� ������
    [SerializeField] int satisfy;
    private int currentSatisfy;
    #endregion

    // �ʿ��� �̹���
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

    #region ����� ����
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
            Debug.Log("����� ��ġ�� 0�� �Ǿ����ϴ�.");
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

    #region �񸶸� ����
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
            Debug.Log("�񸶸� ��ġ�� 0�� �Ǿ����ϴ�.");
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

    #region HP ����
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
    /// ������ �켱 ������ �����ǵ���
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
            Debug.Log("ĳ������ HP�� 0�� �Ǿ����ϴ�.");
        }
    }
    #endregion

    #region DP (����) ����
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
            Debug.Log("ĳ������ DP�� 0�� �Ǿ����ϴ�.");
        }
    }
    #endregion

    #region ���¹̳� ����
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
