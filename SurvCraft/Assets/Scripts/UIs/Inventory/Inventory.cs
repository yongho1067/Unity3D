using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class Inventory : MonoBehaviour
{
    public static bool inventoryActivated = false;

    [SerializeField] private GameObject inventoryBase;
    [SerializeField] private GameObject slotParent;
    [SerializeField] private GameObject inputField;

    [SerializeField] private GameObject quickSlotParent;
    private bool isNotPut;

    [SerializeField] ItemEffectDataBase itemEffectDataBase;

    // 인벤토리 슬롯들
    private ItemSlot[] slots;
    private ItemSlot[] quickSlots;

    [SerializeField] private QuickSlotController quickSlotController;
    private int slotNum;

    void Start()
    {
        slots = slotParent.GetComponentsInChildren<ItemSlot>();
        quickSlots = quickSlotParent.GetComponentsInChildren<ItemSlot>();
    }

    
    void Update()
    {
        TryOpenInventory();
    }

    private void TryOpenInventory()
    {
        if (Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.Tab))
        {
            inventoryActivated = !inventoryActivated;

            if (inventoryActivated)
            {
                OpenInventory();
            }
            else
            {
                CloseInventory();
                itemEffectDataBase.HideToolTip();
            }
        }
    }

    private void OpenInventory()
    {
        inventoryBase.SetActive(true);
    }

    private void CloseInventory()
    {
        inventoryBase.SetActive(false);
    }

    public void AcquireItem(Item item, int count = 1)
    {
        PutSlot(quickSlots, item, count);
        if(!isNotPut)
        {
            quickSlotController.isActivatedQuickSlot(slotNum);
        }

        // 인벤토리 채우기
        if (isNotPut)
        {
            PutSlot(slots, item, count);
        }

        if (isNotPut)
        {
            Debug.Log("퀵슬롯이 가득찼습니다");
        }
    }

    private void PutSlot(ItemSlot[] slots, Item item, int count)
    {
        if (Item.ItemType.Equipment != item.itemType)
        {

            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].item != null)
                {
                    if (slots[i].item.itemName == item.itemName)
                    {
                        slotNum = i;
                        slots[i].SetSlotCount(count);
                        isNotPut = false;
                        return;
                    }

                }
            }
        }

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                Debug.Log("PICK UP");
                slots[i].AddItem(item, count);
                isNotPut = false;
                return;
            }
        }

        isNotPut = true;
    }

    public int GetItemCount(string _itemName)
    {
        int temp;
        temp = SearchSlotItem(slots, _itemName);

        return temp != 0 ? temp : SearchSlotItem(quickSlots, _itemName);
    }

    private int SearchSlotItem(ItemSlot[] _slots, string _itemName)
    {
        for (int i = 0; i < _slots.Length; i++)
        {
            if (_slots[i].item != null)
            {
                if (_itemName == _slots[i].item.itemName)
                {
                    return _slots[i].itemCount;
                }
            }
        }
    
        // 아이템을 찾지 못한 경우 0을 반환
        return 0;
    }

    public void SetItemCount(string _itemName, int _itemCount)
    {
        if(!ItemCountAdjust(slots, _itemName,_itemCount))
        {
            ItemCountAdjust(quickSlots, _itemName, _itemCount);
        }
    }

    private bool ItemCountAdjust(ItemSlot[] _slots, string _itemName, int _itemCount)
    {
        for (int i = 0; i < _slots.Length; i++)
        {
            if (_slots[i].item != null)
            {
                if (_itemName == _slots[i].item.itemName)
                {
                    _slots[i].SetSlotCount(-_itemCount);
                    return true;
                }
            }
        }

        return false;

    }
}
