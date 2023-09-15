using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    static public bool isActivated = true;
    // ���� ��ġ
    private Vector3 originPos;

    // ���� ��ġ
    private Vector3 currentPos;

    // sway �Ѱ�
    [SerializeField] private Vector3 limitPos;

    // ������ sway �Ѱ�
    [SerializeField] private Vector3 fineSightLimitPos;

    // �ε巯�� ������ ����
    [SerializeField] private Vector3 smoothSway;

    // �ʼ� ������Ʈ
    [SerializeField] private GunController gunController;

    void Start()
    {
        originPos = transform.localPosition;

    }

    void Update()
    {
        if(!Inventory.inventoryActivated && isActivated && !GameManager.isOpenCraftManual)
        {
            TrySway();
        }
    }

    /// <summary>
    /// �ڿ������� ������ ����
    /// </summary>
    private void TrySway()
    {
        // ���콺�� �����Ͻ�
        if(Input.GetAxisRaw("Mouse X") != 0 || Input.GetAxisRaw("Mouse Y") != 0)
        {
            Swaying();
        }
        else
        {
            BackToOriginPos();
        }
    }

    /// <summary>
    /// ���콺�� ������� ���� / �����ؽ� or ������ ���ҽ�
    /// </summary>
    private void Swaying()
    {
        float moveX = Input.GetAxisRaw("Mouse X");
        float moveY = Input.GetAxisRaw("Mouse Y");

        if (!gunController.isFineSightMode)
        {
            // �������� ���ҽ�
            // ȭ������� ������ �ʵ��� clamp�� ���ֳ���
            // �ڿ������� �����̵��� lerp
            currentPos.Set(Mathf.Clamp(Mathf.Lerp(currentPos.x, -moveX, smoothSway.x), -limitPos.x, limitPos.x),
                        Mathf.Clamp(Mathf.Lerp(currentPos.y, -moveY, smoothSway.x), -limitPos.y, limitPos.y),
                        originPos.z);
        }
        else
        {
            // �����ؽ�
            currentPos.Set(Mathf.Clamp(Mathf.Lerp(currentPos.x, -moveX, smoothSway.y), -fineSightLimitPos.x, fineSightLimitPos.x),
                        Mathf.Clamp(Mathf.Lerp(currentPos.y, -moveY, smoothSway.y), -fineSightLimitPos.y, fineSightLimitPos.y),
                        originPos.z);
        }
        transform.localPosition = currentPos;
    }

    private void BackToOriginPos()
    {
        currentPos = Vector3.Lerp(currentPos, originPos, smoothSway.x);
        transform.localPosition = currentPos;
    }
}
