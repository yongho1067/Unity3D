using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSlotController : MonoBehaviour
{
    [SerializeField] private ItemSlot[] quickSlots; // 퀵슬롯들
    [SerializeField] private Transform quickSlotsParent; // 퀵슬롯의 부모 객체

    private int selectedSlot; // 선택된 퀵슬롯 ( 0~7 ) = 8개

    [SerializeField] private GameObject selectedImage; // 선택된 퀵슬롯의 이미지
    [SerializeField] private WeaponManager weaponManager;

    [SerializeField] private Transform itemPos; // 아이템이 위치할 손 끝
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
        // 선택된 슬롯으로 이미지 이동
        selectedImage.transform.position = quickSlots[selectedSlot].transform.position;

        Execute();
    }

    /// <summary>
    /// 해당 슬롯에 아무것도 없으면 맨손이 나오도록 설정
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
    /// 손에 들고 있는 아이템 변경
    /// </summary>
    private void ChangeHand(Item item = null)
    {
        StartCoroutine(weaponManager.ChangeWeaponCoroutine("HAND", "맨손"));

        if(item != null)
        {
            StartCoroutine(HandItemCoroutine());
        }
    }

    /// <summary>
    /// 손의 위치로 아이템 생성
    /// </summary>
    IEnumerator HandItemCoroutine()
    {
        HandController.isActivate = false;

        yield return new WaitUntil(() => HandController.isActivate);

        handItem = Instantiate(quickSlots[selectedSlot].item.itemPrefab, itemPos.position, itemPos.rotation);

        handItem.GetComponent<Rigidbody>().isKinematic = true; // 중력 영향 x
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
