using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ActionController : MonoBehaviour
{
    // ���� ������ �ִ� �Ÿ�
    [SerializeField] private float range;

    // ���� ������ �� true;
    private bool pickupActivated = false; // ���� ������ ��
    private bool dissolveActivated = false; // ��� ��ü ������ ��
    private bool isDissolving = false; // ��� ��ü �߿��� true

    // �浹ü ���� ����
    private RaycastHit hitinfo;

    // ��Ȯ�� ������ ������ ���� ������ġ
    [SerializeField] private LayerMask layerMask;

    [SerializeField] private TextMeshProUGUI actionText;
    [SerializeField] private Inventory inventory;
    [SerializeField] WeaponManager weaponManager;
    [SerializeField] private Transform tf_MeatDissolveTool; // ��� ��ü ��
    private int randomSound;

    [SerializeField] QuickSlotController quickSlotController;

    [SerializeField] private string sound_meat; // �Ҹ����


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
        actionText.text = hitinfo.transform.GetComponent<ItemPickUp>().item.itemName + " ȹ�� " + "<color=yellow>" + "(F)" + "</color>";
    }

    private void MeatinfoAppear()
    {
        if (hitinfo.transform.GetComponent<Pig>().isDead)
        {
            //Debug.Log("hitinfo.transform.GetComponent<Animal>().isDead");
            dissolveActivated = true;
            actionText.gameObject.SetActive(true);
            actionText.text = hitinfo.transform.GetComponent<Pig>().animalName + " ��ü�ϱ� " + "<color=yellow>" + "(F)" + "</color>";
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
                Debug.Log(hitinfo.transform.GetComponent<ItemPickUp>().item.itemName + " ȹ�� ");
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

                // ��� ��ü �ǽ�
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
