using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool canPlayerMove = true; // �÷��̾��� ������ ����
    public static bool isOpenInventory = false; // �κ��丮 Ȱ��ȭ
    public static bool isOpenCraftManual = false; // ���� �޴� Ȱ��ȭ
    public static bool isOpenArchemyTable = false; // ���� ���̺� â Ȱ��ȭ
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
