using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ActionController : MonoBehaviour
{
    // ���� ������ �ִ� �Ÿ�
    [SerializeField] private float range;

    // ���� ������ �� true;
    private bool pickupActivated = false;

    // �浹ü ���� ����
    private RaycastHit hitinfo;

    // ��Ȯ�� ������ ������ ���� ������ġ
    [SerializeField] private LayerMask layerMask;

    [SerializeField] private TextMeshProUGUI actionText;
    [SerializeField] private Inventory inventory;
    private int randomSound;


    private void Update()
    {
        CheckItem();  
    }

    private void CheckItem()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hitinfo, range, layerMask))
        {
            if(hitinfo.transform.tag == "Item")
            {
                TryAction();
                IteminfoAppear();
            }
        }
        else
        {
            IteminfoDisappear();
        }
    }

    private void IteminfoAppear()
    {
        pickupActivated = true;
        actionText.gameObject.SetActive(true);
        actionText.text = hitinfo.transform.GetComponent<ItemPickUp>().item.itemName + " ȹ�� " + "<color=yellow>" + "(F)" + "</color>";
    }

    private void IteminfoDisappear()
    {
        pickupActivated = false;
        actionText.gameObject.SetActive(false);
    }

    private void TryAction()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            PickUp();
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
                IteminfoDisappear();
            }
        }
    }
}
