using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{

    [SerializeField] private GameObject go_BaseUi;
    [SerializeField] private SaveNLoad saveNLoad;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            if(!GameManager.isPause)
            {
                CallMenu();
            }
            else
            {
                CloseMenu();
            }
        }
    }

    private void CallMenu()
    {
        GameManager.isPause = true;
        go_BaseUi.SetActive(true);
        Time.timeScale = 0f;
    }

    private void CloseMenu()
    {
        GameManager.isPause = false;
        go_BaseUi.SetActive(false);
        Time.timeScale = 1f;
    }

    public void ClickSave()
    {
        Debug.Log("���̺�");
        saveNLoad.SaveData();
    }

    public void ClickLoad()
    {
        Debug.Log("�ε�");
        saveNLoad.LoadData();
    }

    public void ClickExit()
    {
        Debug.Log("��������");
        Application.Quit();
    }
}
