using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool canPlayerMove = true; // 플레이어의 움직임 제어
    public static bool isOpenInventory = false; // 인벤토리 활성화
    public static bool isOpenCraftManual = false; // 건축 메뉴 활성화
    public static bool isOpenArchemyTable = false; // 연금 테이블 창 활성화
    public static bool isPowerOnComputer = false;

    private void Update()
    {
        if (isOpenInventory || isOpenCraftManual || isPowerOnComputer || isOpenArchemyTable)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            canPlayerMove = false;

            if(isPowerOnComputer)
            {
                canPlayerMove = true;
            }
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            canPlayerMove = true;
        }
    }
}
