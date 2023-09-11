using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ActionController : MonoBehaviour
{
    // 습득 가능한 최대 거리
    [SerializeField] private float range;

    // 습득 가능할 시 true;
    private bool pickupActivated = false; // 습득 가능할 시
    private bool dissolveActivated = false; // 고기 해체 가능할 시
    private bool isDissolving = false; // 고기 해체 중에는 true

    // 충돌체 정보 저장
    private RaycastHit hitinfo;

    // 정확한 아이템 습득을 위한 보조장치
    [SerializeField] private LayerMask layerMask;

    [SerializeField] private TextMeshProUGUI actionText;
    [SerializeField] private Inventory inventory;
    [SerializeField] WeaponManager weaponManager;
    [SerializeField] private Transform tf_MeatDissolveTool; // 고기 해체 툴
    private int randomSound;

    [SerializeField] QuickSlotController quickSlotController;

    [SerializeField] private string sound_meat; // 소리재생


    private void Update()
    {
        CheckAction();  
    }

    private void CheckAction()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hitinfo, range, layerMask))
        {
            if(hitinfo.transform.tag == "Item")
            {
                TryAction();
                IteminfoAppear();
            }
            else if(hitinfo.transform.tag == "WeakAnimal" || hitinfo.transform.tag == "StrongAnimal")
            {
                MeatinfoAppear();
                TryAction();
                //Debug.Log("hitinfo.transform.tag == \"WeakAnimal\" || hitinfo.transform.tag == \"StrongAnimal\"");
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

    private void IteminfoAppear()
    {
        pickupActivated = true;
        actionText.gameObject.SetActive(true);
        actionText.text = hitinfo.transform.GetComponent<ItemPickUp>().item.itemName + " 획득 " + "<color=yellow>" + "(F)" + "</color>";
    }

    private void MeatinfoAppear()
    {
        if (hitinfo.transform.GetComponent<Pig>().isDead)
        {
            //Debug.Log("hitinfo.transform.GetComponent<Animal>().isDead");
            dissolveActivated = true;
            actionText.gameObject.SetActive(true);
            actionText.text = hitinfo.transform.GetComponent<Pig>().animalName + " 해체하기 " + "<color=yellow>" + "(F)" + "</color>";
        }
    }

    private void InfoDisappear()
    {
        pickupActivated = false;
        dissolveActivated = false;
        actionText.gameObject.SetActive(false);
    }

    private void TryAction()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            PickUp();
            CanMeat();
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
}
