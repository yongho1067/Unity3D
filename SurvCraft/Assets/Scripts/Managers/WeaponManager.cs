using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponManager : MonoBehaviour
{
    // static�� ���� ���� ���� ������ �޸𸮰� ����ȴ�
    // ���� �ڿ� / Ŭ���� ���� / ���� ����

    // ���� �ߺ� ��ü ���� ����
    public static bool isChangeWeapon;

    // ���� ����� ���� ������ �ִϸ��̼�
    public static Transform currentWeapon;
    public static Animator currentWeaponAnim;

    // ���� ������ Ÿ��
    [SerializeField] private string currentWeaponType;

    // ���� ��ü ������
    [SerializeField] private float changeWeaponDelayTime;
    // ���� ��ü�� ���� ����
    [SerializeField] private float changeWeaponEndDelayTime;

    // ���� ������ ���� ����
    [SerializeField] private Gun[] guns;
    [SerializeField] private CloseWeapon[] hands;
    [SerializeField] private CloseWeapon[] axes;
    [SerializeField] private CloseWeapon[] pickaxes;

    // �迭 ���� �������� ���� ���� ������ �����ϵ��� ����
    private Dictionary<string, Gun> gunDictionary = new Dictionary<string, Gun>();
    private Dictionary<string, CloseWeapon> handDictionary = new Dictionary<string, CloseWeapon>();
    private Dictionary<string, CloseWeapon> axeDictionary = new Dictionary<string, CloseWeapon>();
    private Dictionary<string, CloseWeapon> pickaxeDictionary = new Dictionary<string, CloseWeapon>();

    // �ʿ��� ������Ʈ
    [SerializeField] private GunController gunController;
    [SerializeField] private HandController handController;
    [SerializeField] private AxeController axeController;
    [SerializeField] private PickaxeController pickaxeController;

    [SerializeField] private Image BulletUI;



    private void Awake()
    {

    }

    void Start()
    {
        for (int i = 0; i < guns.Length; i++)
        {
            gunDictionary.Add(guns[i].gunName, guns[i]);
        }
        for (int i = 0; i < hands.Length; i++)
        {
            handDictionary.Add(hands[i].closeWeaponName, hands[i]);
        }
        for (int i = 0; i < hands.Length; i++)
        {
            axeDictionary.Add(axes[i].closeWeaponName, axes[i]);
        }
        for (int i = 0; i < pickaxes.Length; i++)
        {
            pickaxeDictionary.Add(pickaxes[i].closeWeaponName, pickaxes[i]);
        }
    }

    /*void Update()
    {
        if (!isChangeWeapon)
        {
            
            // ���� 1 ������ ��� �Ǽ�����
            if (Input.GetKeyDown(KeyCode.Alpha1) && !(currentWeapon.name == "Hand"))
            {
                BulletUI.gameObject.SetActive(false);
                StartCoroutine(ChangeWeaponCoroutine("HAND","�Ǽ�"));
            }
            // ���� 2 ������ ��� ����ӽŰ�����
            else if (Input.GetKeyDown(KeyCode.Alpha2) && !(currentWeapon.name == "SubMachineGun1"))
            {
                BulletUI.gameObject.SetActive(true);
                StartCoroutine(ChangeWeaponCoroutine("GUN", "����ӽŰ�1"));
            }
            // ���� 3 ������ ��� ���� ��
            else if (Input.GetKeyDown(KeyCode.Alpha3) && !(currentWeapon.name == "Axe"))
            {
                BulletUI.gameObject.SetActive(false);
                StartCoroutine(ChangeWeaponCoroutine("AXE", "����"));
            }
            // ���� 4 ������ ��� ��̷�
            else if (Input.GetKeyDown(KeyCode.Alpha4) && !(currentWeapon.name == "Pickaxe"))
            {
                BulletUI.gameObject.SetActive(false);
                StartCoroutine(ChangeWeaponCoroutine("PICKAXE", "���"));
            }
        }
    }*/

    /// <summary>
    /// ���� ��ü �ڷ�ƾ
    /// </summary>
    public IEnumerator ChangeWeaponCoroutine(string type, string name)
    {
        isChangeWeapon = true;
        currentWeaponAnim.SetTrigger("Weapon_Out");

        yield return new WaitForSeconds(changeWeaponDelayTime);

        CancelPreWeaponAction();
        WeaponChange(type, name);

        yield return new WaitForSeconds(changeWeaponEndDelayTime);

        currentWeaponType = type;
        isChangeWeapon = false;
    }

    /// <summary>
    /// ���� ��ü�� �������̴� �ڷ�ƾ �� �ִϸ��̼� ����
    /// </summary>
    private void CancelPreWeaponAction()
    {
        switch (currentWeaponType)
        {
            case "GUN":
                gunController.CancelFineSight();
                gunController.CancelReload();
                GunController.isActivate = false;
                break;
            case "HAND":
                HandController.isActivate = false;
                if(QuickSlotController.handItem != null)
                {
                    Destroy(QuickSlotController.handItem);
                }
                break;
            case "AXE":
                AxeController.isActivate = false;
                break;
            case "PICKAXE":
                AxeController.isActivate = false;
                break;
        }
    }

    /// <summary>
    /// ���� ��ü
    /// </summary>
    private void WeaponChange(string type, string name)
    {
        if(type == "GUN")
        {
            gunController.GunChange(gunDictionary[name]);
        }
        else if(type == "HAND")
        {
            handController.CloseWeaponChange(handDictionary[name]);
        }
        else if(type == "AXE")
        {
            axeController.CloseWeaponChange(axeDictionary[name]);
        }
        else if(type == "PICKAXE")
        {
            pickaxeController.CloseWeaponChange(pickaxeDictionary[name]);
        }
    }
}
