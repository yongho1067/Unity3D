using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ArchemyItem
{
    public string itemName;
    public string itemContent;
    public Sprite itemImage;

    public float itemCraftingTime; // ���� ������ �ɸ��� �ð�(5�� 10�� 100��)

    public GameObject go_ItemPrefab;
}


public class ArchemyTable : MonoBehaviour
{

    private bool isOpen = false;
    private bool isCrafting = false; // �������� ���� ���� ����.

    [SerializeField] private ArchemyItem[] archemyItems; // ���� �� ���ִ� ���� ������ ����Ʈ.
    private Queue<ArchemyItem> archemyItemQueue = new Queue<ArchemyItem>(); // ���� ������ ���� ��⿭ (ť) ���Լ���
    private ArchemyItem currentCraftingItem; // ���� �������� ���� ������.

    private float craftingTime; // ���� ���� �ð�
    private float currentCraftingTime; // ���� ���

    [SerializeField] private Slider sliderGauge; //�����̴� ������
    [SerializeField] private Transform tf_BaseUI; // ���̽� UI
    [SerializeField] private Transform tf_PotionDropPos; // ���� ���� ��ġ

    [SerializeField] private GameObject go_Liquid; // ���۽�Ű�� ��ü ���� 
    [SerializeField] private Image[] image_CraftingItems; // ��⿭ ���Կ� �ִ� ������ �̹���

    private void Update()
    {
        if(!isFinish())
        {
            Crafting();
        }

        if(isOpen)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                CloseWindow();
            }
        }
    }

    private bool isFinish()
    {
        if(archemyItemQueue.Count == 0 && !isCrafting)
        {
            go_Liquid.SetActive(false);
            sliderGauge.gameObject.SetActive(false);
            return true;
        }
        else
        {
            go_Liquid.SetActive(true);
            sliderGauge.gameObject.SetActive(true);
            return false;
        }
    }

    private void Crafting()
    {
        if(!isCrafting && archemyItemQueue.Count != 0)
        {
            DequeueItem();
        }

        if(isCrafting)
        {
            currentCraftingTime += Time.deltaTime;
            sliderGauge.value = currentCraftingTime;

            if(currentCraftingTime >= craftingTime)
            {
                ProductionComplete();
            }
        }
    }

    private void DequeueItem()
    {
        isCrafting = true;
        currentCraftingItem = archemyItemQueue.Dequeue();

        // �ʱ�ȭ ����
        craftingTime = currentCraftingItem.itemCraftingTime;
        currentCraftingTime = 0;
        sliderGauge.maxValue = craftingTime;

        CraftingImageChange();
    }

    private void CraftingImageChange()
    {
        image_CraftingItems[0].gameObject.SetActive(true);

        // ������ Dequeue�� �����Ƿ� Count�� 1�� ����
        for (int i = 0; i < archemyItemQueue.Count + 1; i++)
        {
            image_CraftingItems[i].sprite = image_CraftingItems[i + 1].sprite;
            if(i+1 == archemyItemQueue.Count + 1)
            {
                image_CraftingItems[i + 1].gameObject.SetActive(false);
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
        if(archemyItemQueue.Count < 3)
        {
            archemyItemQueue.Enqueue(archemyItems[_buttonNum]);

            image_CraftingItems[archemyItemQueue.Count].gameObject.SetActive(true);
            image_CraftingItems[archemyItemQueue.Count].sprite = archemyItems[_buttonNum].itemImage;
        }
    }

    private void ProductionComplete()
    {
        isCrafting = false;
        image_CraftingItems[0].gameObject.SetActive(false);

        Instantiate(currentCraftingItem.go_ItemPrefab, tf_PotionDropPos.position, Quaternion.identity);
    }

    public bool GetisOpen()
    {
        return isOpen;
    }

}
