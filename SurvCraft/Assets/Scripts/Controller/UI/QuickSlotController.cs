using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSlotController : MonoBehaviour
{
    [SerializeField] private ItemSlot[] quickSlots; // �����Ե�
    [SerializeField] private Transform quickSlotsParent; // �������� �θ� ��ü

    private int selectedSlot; // ���õ� ������ ( 0~7 ) = 8��

    [SerializeField] private GameObject selectedImage; // ���õ� �������� �̹���
    [SerializeField] private WeaponManager weaponManager;

    [SerializeField] private Transform itemPos; // �������� ��ġ�� �� ��
    public static GameObject handItem;

    private void Start()
    {
        quickSlots = quickSlotsParent.GetComponentsInChildren<ItemSlot>();
        selectedSlot = 0;
    }

    private void Update()
    {
        TryInputNumber();
    }

    private void TryInputNumber()
    {
        if (!Inventory.inventoryActivated)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                ChageSelectedQickSlot(0);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                ChageSelectedQickSlot(1);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                ChageSelectedQickSlot(2);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                ChageSelectedQickSlot(3);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                ChageSelectedQickSlot(4);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                ChageSelectedQickSlot(5);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                ChageSelectedQickSlot(6);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha8))
            {
                ChageSelectedQickSlot(7);
            }
        }
        
    }

    private void ChageSelectedQickSlot(int num)
    {
        selectedSlot = num;
        // ���õ� �������� �̹��� �̵�
        selectedImage.transform.position = quickSlots[selectedSlot].transform.position;

        Execute();
    }

    /// <summary>
    /// �ش� ���Կ� �ƹ��͵� ������ �Ǽ��� �������� ����
    /// </summary>
    private void Execute()
    {
        Debug.Log(WeaponManager.currentWeapon.name);
        if (quickSlots[selectedSlot].item != null)
        {
            if(quickSlots[selectedSlot].item.itemType == Item.ItemType.Equipment)
            {
                StartCoroutine(weaponManager.ChangeWeaponCoroutine(quickSlots[selectedSlot].item.weaponType, quickSlots[selectedSlot].item.itemName));
            }else if (quickSlots[selectedSlot].item.itemType == Item.ItemType.Used)
            {
                ChangeHand(quickSlots[selectedSlot].item);
            }
            else
            {
                ChangeHand();
            }
        }
        else
        {
            if (WeaponManager.currentWeapon.name == ("Hand"))
            {
                return;
            }
            ChangeHand();
        }
    }

    /// <summary>
    /// �տ� ��� �ִ� ������ ����
    /// </summary>
    private void ChangeHand(Item item = null)
    {
        StartCoroutine(weaponManager.ChangeWeaponCoroutine("HAND", "�Ǽ�"));

        if(item != null)
        {
            StartCoroutine(HandItemCoroutine());
        }
    }

    /// <summary>
    /// ���� ��ġ�� ������ ����
    /// </summary>
    IEnumerator HandItemCoroutine()
    {
        HandController.isActivate = false;

        yield return new WaitUntil(() => HandController.isActivate);

        handItem = Instantiate(quickSlots[selectedSlot].item.itemPrefab, itemPos.position, itemPos.rotation);

        handItem.GetComponent<Rigidbody>().isKinematic = true; // �߷� ���� x
        handItem.GetComponent<BoxCollider>().enabled = false;
        handItem.tag = "Untagged";
        handItem.layer = 6; // Weapon
        handItem.transform.SetParent(itemPos);
    }

    public void Eatitem()
    {
        quickSlots[selectedSlot].SetSlotCount(-1);

        if (quickSlots[selectedSlot]. itemCount <= 0)
        {
            Destroy(handItem);
        }
    }

    public void isActivatedQuickSlot(int num)
    {
        if(selectedSlot == num)
        {
            Execute();
            return;
        }
        if (DragSlot.instance != null)
        {
            if(DragSlot.instance.itemSlot != null)
            {
                if (DragSlot.instance.itemSlot.GetQuickSlotNumber() == selectedSlot)
                {
                    Execute();
                    return;
                }
            }  
        }
        
    }

}
