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

    // �κ��丮 ���Ե�
    private ItemSlot[] slots;
    private ItemSlot[] quickSlots;

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
        if (Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.Escape))
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

        // �κ��丮 ä���
        if (isNotPut)
        {
            PutSlot(slots, item, count);
        }

        if (isNotPut)
        {
            Debug.Log("�������� ����á���ϴ�");
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
}
