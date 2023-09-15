using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ActionController : MonoBehaviour
{
    // 습득 가능한 최대 거리
    [SerializeField] private float range;

    // 습득 가능할 시 true;
    private bool pickupActivated = false; // 아이템 습득 가능할 시
    private bool dissolveActivated = false; // 고기 해체 가능할 시
    private bool isDissolving = false; // 고기 해체 중에는 true
    private bool fireLookActivated = false; // 불을 근접해서 바라 볼 시 true
    private bool lookComputer = false; // 컴퓨터를 바라 볼 시 true

    // 충돌체 정보 저장
    private RaycastHit hitinfo;

    // 정확한 아이템 습득을 위한 보조장치
    [SerializeField] private LayerMask layerMask;

    [SerializeField] private string sound_meat; // 소리재생
    private int randomSound;

    // 필요한 컴포넌트
    [SerializeField] private TextMeshProUGUI actionText;
    [SerializeField] private Inventory inventory;
    [SerializeField] WeaponManager weaponManager;
    [SerializeField] QuickSlotController quickSlotController;
    [SerializeField] private Transform tf_MeatDissolveTool; // 고기 해체 툴
    [SerializeField] private Computer computerKit;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        CheckAction();  
    }

    private void CheckAction()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hitinfo, range, layerMask))
        {
            TryAction();

            if (hitinfo.transform.tag == "Item")
            {
                IteminfoAppear();
            }
            else if(hitinfo.transform.tag == "WeakAnimal" || hitinfo.transform.tag == "StrongAnimal")
            {
                MeatinfoAppear();
                //Debug.Log("hitinfo.transform.tag == \"WeakAnimal\" || hitinfo.transform.tag == \"StrongAnimal\"");
            }
            else if(hitinfo.transform.tag == "Fire")
            {  
                FireInfoAppear();
            }
            else if(hitinfo.transform.tag == "Computer")
            {
                ComputerInfoAppear();
            }
            else
            {
                InfoDisappear();
            }
            //Debug.Log(hitinfo.transform.tag);
        }
        else
        {
            InfoDisappear();
        }
    }

    private void Reset()
    {
        pickupActivated = false;
        dissolveActivated = false;
        fireLookActivated = false;
        lookComputer = false;
    }

    private void IteminfoAppear()
    {
        Reset();
        pickupActivated = true;
        actionText.gameObject.SetActive(true);
        actionText.text = hitinfo.transform.GetComponent<ItemPickUp>().item.itemName + " 획득 " + "<color=yellow>" + "(F)" + "</color>";
    }

    private void ComputerInfoAppear()
    {
        if(!hitinfo.transform.GetComponent<Computer>().isPowerOn)
        {
            Reset();
            lookComputer = true;
            actionText.gameObject.SetActive(true);
            actionText.text = " 컴퓨터 가동 " + "<color=yellow>" + "(F)" + "</color>";
        }
    }

    private void MeatinfoAppear()
    {
        if (hitinfo.transform.GetComponent<Pig>().isDead)
        {
            Reset();
            //Debug.Log("hitinfo.transform.GetComponent<Animal>().isDead");
            dissolveActivated = true;
            actionText.gameObject.SetActive(true);
            actionText.text = hitinfo.transform.GetComponent<Pig>().animalName + " 해체하기 " + "<color=yellow>" + "(F)" + "</color>";
        }
    }

    private void FireInfoAppear()
    {
        Reset();
        fireLookActivated = true;
        Debug.Log(hitinfo.transform.GetComponent<Fire>());

        if (hitinfo.transform.GetComponent<Fire>().GetisFire())
        {
            actionText.gameObject.SetActive(true);
            actionText.text = " 선택된 아이템 불에 넣기" + "<color=yellow>" + "(F)" + "</color>";
        }
    }

    private void InfoDisappear()
    {
        pickupActivated = false;
        dissolveActivated = false;
        fireLookActivated = false;
        lookComputer = false;
        actionText.gameObject.SetActive(false);
    }

    private void TryAction()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            PickUp();
            CanMeat();
            CanDropFire();
            ComputerPowerOn();
            //Debug.Log(fireLookActivated);
        }
    }

    private void PickUp()
    {
        if (pickupActivated)
        {
            if(hitinfo.transform != null)
            {
                randomSound = (int)Random.Range(1, 9);
                SoundManager.soundManager.PlaySE("PickUpSound" + randomSound);
                Debug.Log(hitinfo.transform.GetComponent<ItemPickUp>().item.itemName + " 획득 ");
                inventory.AcquireItem(hitinfo.transform.GetComponent<ItemPickUp>().item);
                Destroy(hitinfo.transform.gameObject);
                InfoDisappear();
                quickSlotController.ResetAppear();
            }
        }
    }

    private void CanMeat()
    {
        Debug.Log(dissolveActivated);
        if(dissolveActivated)
        {
            if((hitinfo.transform.tag == "WeakAnimal" || hitinfo.transform.tag == "StrongAnimal") && hitinfo.transform.GetComponent<Pig>().isDead && !isDissolving)
            {
                isDissolving = true;
                InfoDisappear();

                // 고기 해체 실시
                StartCoroutine(MeatCoroutine());
            }
                
        }
    }

    IEnumerator MeatCoroutine()
    {
        WeaponManager.isChangeWeapon = true;
        WeaponManager.currentWeaponAnim.SetTrigger("Weapon_Out");
        PlayerController.isActivated = false;
        WeaponSway.isActivated = false;

        yield return new WaitForSeconds(0.2f);

        WeaponManager.currentWeapon.gameObject.SetActive(false);
        tf_MeatDissolveTool.gameObject.SetActive(true);



        yield return new WaitForSeconds(0.2f);
        SoundManager.soundManager.PlaySE(sound_meat);
        yield return new WaitForSeconds(1.8f);

        inventory.AcquireItem(hitinfo.transform.GetComponent<Pig>().GetItem(), hitinfo.transform.GetComponent<Pig>().itemNumber);

        WeaponManager.currentWeapon.gameObject.SetActive(true);
        tf_MeatDissolveTool.gameObject.SetActive(false);

        PlayerController.isActivated = true;
        WeaponSway.isActivated = true;
        WeaponManager.isChangeWeapon = false;
        isDissolving = false;

    }

    private void CanDropFire()
    {
        if(fireLookActivated)
        {
            if (hitinfo.transform.tag == "Fire" && hitinfo.transform.GetComponent<Fire>().GetisFire())
            {
                // 손에 들고있는 아이템을 불에 넣음 == 선택된 퀵슬롯의 아이템 ( NULL )판별 필요
                ItemSlot _selectedSlot = quickSlotController.GetSelectedSlot();

                if(_selectedSlot.item != null)
                {
                    DropAnItem(_selectedSlot);
                }
            }
        }
    }

    private void DropAnItem(ItemSlot _selectedSlot)
    {
        switch (_selectedSlot.item.itemType)
        {
            case Item.ItemType.Used:
                if(_selectedSlot.item.itemName.Contains("고기")) // 문자열에 포함되있는 글자를 확인해줌
                {
                    Instantiate(_selectedSlot.item.itemPrefab, hitinfo.transform.position + Vector3.up, Quaternion.identity);
                    quickSlotController.DecreaseSelectedItem();
                }
                break;
            case Item.ItemType.Ingredient:
                break;

        }
    }

    private void ComputerPowerOn()
    {
        if(lookComputer)
        {
            if(hitinfo.transform != null)
            {
                if(!hitinfo.transform.GetComponent<Computer>().isPowerOn)
                {
                    hitinfo.transform.GetComponent<Computer>().PowerOn();
                    InfoDisappear();
                }
            }
        }
    }

}
