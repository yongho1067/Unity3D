using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class ItemSlot : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    // 획득한 아이템
    public Item item;

    // 획득한 아이템의 갯수
    public int itemCount;

    // 아이템의 이미지
    public Image itemImage;

    [SerializeField] private TextMeshProUGUI text_Count;
    [SerializeField] private GameObject countImage;


    private ItemEffectDataBase itemEffectDataBase;
    private Rect baseRect;
    private InputNumber inputNumber;



    private void Start()    
    {
        itemEffectDataBase = FindObjectOfType<ItemEffectDataBase>();
                     // Inventory Base
        baseRect = transform.parent.parent.GetComponent<RectTransform>().rect;
        inputNumber = FindObjectOfType<InputNumber>();
            
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

        text_Count.text = "0";
        countImage.SetActive(false);

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
                itemEffectDataBase.UseItem(item);
                
                if(item.itemType == Item.ItemType.Used)
                {
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
        // 인벤토리의 x의 최소값과 최대값 y의 최소값과 최대값을 구해서 인벤토리 영역을 벗어나는지 확인
        if (DragSlot.instance.transform.localPosition.x < baseRect.xMin || DragSlot.instance.transform.localPosition.x > baseRect.xMax
            || DragSlot.instance.transform.localPosition.y < baseRect.yMin || DragSlot.instance.transform.localPosition.y > baseRect.yMax)
        {
            if(DragSlot.instance.itemSlot != null)
            {
                inputNumber.Call();
            }
        }
        else
        {
            DragSlot.instance.SetColor(0f);
            DragSlot.instance.itemSlot = null;
        }            
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

    /// <summary>
    /// 마우스가 슬롯에 들어갈때 발동
    /// </summary>
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(item != null)
        {
            itemEffectDataBase.ShowToolTip(item, itemCount);
        }
    }
    /// <summary>
    ///  슬롯에서 빠져 나올때 발동
    /// </summary>
    public void OnPointerExit(PointerEventData eventData)
    {
        itemEffectDataBase.HideToolTip();
    }

    public int GetItemCount()
    {
        return itemCount;
    }
}
