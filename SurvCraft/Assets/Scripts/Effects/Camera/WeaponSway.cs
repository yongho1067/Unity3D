using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    static public bool isActivated = true;
    // 기존 위치
    private Vector3 originPos;

    // 현재 위치
    private Vector3 currentPos;

    // sway 한계
    [SerializeField] private Vector3 limitPos;

    // 정조준 sway 한계
    [SerializeField] private Vector3 fineSightLimitPos;

    // 부드러운 움직임 정도
    [SerializeField] private Vector3 smoothSway;

    // 필수 컴포넌트
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
    /// 자연스러운 움직임 묘사
    /// </summary>
    private void TrySway()
    {
        // 마우스가 움직일시
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
    /// 마우스를 따라오는 묘사 / 정조준시 or 정조준 안할시
    /// </summary>
    private void Swaying()
    {
        float moveX = Input.GetAxisRaw("Mouse X");
        float moveY = Input.GetAxisRaw("Mouse Y");

        if (!gunController.isFineSightMode)
        {
            // 정조준을 안할시
            // 화면밖으로 나가지 않도록 clamp로 가둬놓고
            // 자연스럽게 움직이도록 lerp
            currentPos.Set(Mathf.Clamp(Mathf.Lerp(currentPos.x, -moveX, smoothSway.x), -limitPos.x, limitPos.x),
                        Mathf.Clamp(Mathf.Lerp(currentPos.y, -moveY, smoothSway.x), -limitPos.y, limitPos.y),
                        originPos.z);
        }
        else
        {
            // 정조준시
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
