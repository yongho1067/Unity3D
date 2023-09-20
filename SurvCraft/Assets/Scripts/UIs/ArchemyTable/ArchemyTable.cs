using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ArchemyItem
{
    public string itemName;
    public string itemContent;
    public Sprite itemImage;

    public GameObject go_ItemPrefab;
}


public class ArchemyTable : MonoBehaviour
{
    private bool isOpen = false;

    [SerializeField] private ArchemyItem[] archemyItems; // ���� �� ���ִ� ���� ������ ����Ʈ.
    [SerializeField] private Transform tf_BaseUI; // ���̽� UI
    [SerializeField] private Transform tf_PotionDropPos; // ���� ���� ��ġ


    private void Update()
    {
        if(isOpen)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                CloseWindow();
            }
        }
    }


    public void Window()
    {
        isOpen = !isOpen;
        if(isOpen)
        {
            OpenWindow();
        }
        else
        {
            CloseWindow();
        }
    }


    private void OpenWindow()
    {
        isOpen = true;
        GameManager.isOpenArchemyTable = true;
        tf_BaseUI.localScale = new Vector3(1f, 1f, 1f);
    }

    // ���� ���۽ð��� �־ ��ü�� ��Ȱ��ȭ ���ѹ����� ������ ���۵��� ����
    private void CloseWindow()
    {
        isOpen = false;
        GameManager.isOpenArchemyTable = false;

        tf_BaseUI.localScale = new Vector3(0f, 0f, 0f);
    }

    public void ButtonClick(int _buttonNum)
    {
        ProductionComplete(_buttonNum);
    }

    private void ProductionComplete(int _buttonNum)
    {
        Instantiate(archemyItems[_buttonNum].go_ItemPrefab, tf_PotionDropPos.position, Quaternion.identity);
    }

}
