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
            Debug.Log("����� ��ġ�� 0�� �Ǿ����ϴ�.");
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
            Debug.Log("�񸶸� ��ġ�� 0�� �Ǿ����ϴ�.");
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
