using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    // 획득한 아이템
    public Item item;

    // 획득한 아이템의 갯수
    public int itemCount;

    // 아이템의 이미지
    public Image itemImage;

    [SerializeField] private TextMeshProUGUI text_Count;
    [SerializeField] private GameObject countImage;

    private WeaponManager weaponManager;

    private void Start()    
    {
        weaponManager = FindObjectOfType<WeaponManager>();
    }

    /// <summary>
    /// 이미지 투명도 조절
    /// </summary>
    private void SetColor(float alpha)
    {
        Color color = itemImage.color;

        color.a = alpha;
        itemImage.color = color;
    }

    /// <summary>
    /// 아이템 획득
    /// </summary>
    public void AddItem(Item dropitem, int count = 1)
    {
        item = dropitem;
        itemCount = count;
        itemImage.sprite = item.itemImage;

        if (item.itemType != Item.ItemType.Equipment)
        {
            countImage.SetActive(true);
            text_Count.text = itemCount.ToString();
        }
        else
        {
            text_Count.text = "0";
            countImage.SetActive(false);
        }
        SetColor(1f);
    }

    /// <summary>
    /// 아이템 갯수 조절
    /// </summary>
    public void SetSlotCount(int count)
    {
        itemCount += count;
        text_Count.text = itemCount.ToString();

        if(itemCount <= 0)
        {
            ClearSlot();
        }
    }

    /// <summary>
    /// 아이템 슬롯 초기화
    /// </summary>
    private void ClearSlot()
    {
        item = null;
        itemCount = 0;
        itemImage.sprite = null;
        SetColor(0f);

        text_Count.text = "0";
        countImage.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right)
        {
            if(item != null)
            {
                if(item.itemType == Item.ItemType.Equipment)
                {

                    StartCoroutine(weaponManager.ChangeWeaponCoroutine(item.weaponType, item.itemName));
                }
                else
                {
                    Debug.Log(item.itemName + "을 사용했습니다.");
                    SetSlotCount(-1);
                }
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(item != null)
        {
            DragSlot.instance.itemSlot = this;
            DragSlot.instance.DragSetImage(itemImage);
            DragSlot.instance.transform.position = eventData.position;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (item != null)
        {
            DragSlot.instance.transform.position = eventData.position;
        }
    }

    // 드래그가 끝났을 모든 경우
    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("OnEndDrag");
        DragSlot.instance.SetColor(0f);
        DragSlot.instance.itemSlot = null;       
    }

    // 다른 슬롯 위에서 드래그가 끝났을 경우
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("OnDrag");
        if(DragSlot.instance.itemSlot != null)
        {
            ChangeSlot();
        }
    }

    private void ChangeSlot()
    {
        Item tempItem = item;
        int tempItemCount = itemCount;

        AddItem(DragSlot.instance.itemSlot.item, DragSlot.instance.itemSlot.itemCount);

        if(tempItem != null)
        {
            DragSlot.instance.itemSlot.AddItem(tempItem, tempItemCount);
        }
        else
        {
            DragSlot.instance.itemSlot.ClearSlot();
        }
    }
}
