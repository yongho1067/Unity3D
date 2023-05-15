using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
    [SerializeField] private Hand[] hands;

    // �迭 ���� �������� ���� ���� ������ �����ϵ��� ����
    private Dictionary<string, Gun> gunDictionary = new Dictionary<string, Gun>();
    private Dictionary<string, Hand> handDictionary = new Dictionary<string, Hand>();

    // �ʿ��� ������Ʈ
    [SerializeField] private GunController gunController;
    [SerializeField] private HandController handController;


    void Start()
    {
        for (int i = 0; i < guns.Length; i++)
        {
            gunDictionary.Add(guns[i].gunName, guns[i]);

        }
        for (int i = 0; i < hands.Length; i++)
        {
            handDictionary.Add(hands[i].handName, hands[i]);
        }
    }

    void Update()
    {
        if (!isChangeWeapon)
        {
            // ���� 1 ������ ��� �Ǽ�����
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                StartCoroutine(ChangeWeaponCoroutine("HAND","�Ǽ�"));
            }
            // ���� 2 ������ ��� ����ӽŰ�����
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                StartCoroutine(ChangeWeaponCoroutine("GUN", "����ӽŰ�1"));

            }
        }
    }

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
            handController.HandChange(handDictionary[name]);
        }
    }
}
