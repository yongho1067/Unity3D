using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponManager : MonoBehaviour
{
    // static을 많이 쓰면 많이 쓸수록 메모리가 낭비된다
    // 공유 자원 / 클래스 변수 / 정적 변수

    // 무기 중복 교체 실행 방지
    public static bool isChangeWeapon;

    // 현재 무기와 현재 무기의 애니메이션
    public static Transform currentWeapon;
    public static Animator currentWeaponAnim;

    // 현재 무기의 타입
    [SerializeField] private string currentWeaponType;

    // 무기 교체 딜레이
    [SerializeField] private float changeWeaponDelayTime;
    // 무기 교체가 끝난 시점
    [SerializeField] private float changeWeaponEndDelayTime;

    // 무기 종류들 전부 관리
    [SerializeField] private Gun[] guns;
    [SerializeField] private CloseWeapon[] hands;
    [SerializeField] private CloseWeapon[] axes;
    [SerializeField] private CloseWeapon[] pickaxes;

    // 배열 관리 차원에서 쉽게 무기 접근이 가능하도록 만듬
    private Dictionary<string, Gun> gunDictionary = new Dictionary<string, Gun>();
    private Dictionary<string, CloseWeapon> handDictionary = new Dictionary<string, CloseWeapon>();
    private Dictionary<string, CloseWeapon> axeDictionary = new Dictionary<string, CloseWeapon>();
    private Dictionary<string, CloseWeapon> pickaxeDictionary = new Dictionary<string, CloseWeapon>();

    // 필요한 컴포넌트
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
            
            // 숫자 1 눌렀을 경우 맨손으로
            if (Input.GetKeyDown(KeyCode.Alpha1) && !(currentWeapon.name == "Hand"))
            {
                BulletUI.gameObject.SetActive(false);
                StartCoroutine(ChangeWeaponCoroutine("HAND","맨손"));
            }
            // 숫자 2 눌렀을 경우 서브머신건으로
            else if (Input.GetKeyDown(KeyCode.Alpha2) && !(currentWeapon.name == "SubMachineGun1"))
            {
                BulletUI.gameObject.SetActive(true);
                StartCoroutine(ChangeWeaponCoroutine("GUN", "서브머신건1"));
            }
            // 숫자 3 눌렀을 경우 도끼 로
            else if (Input.GetKeyDown(KeyCode.Alpha3) && !(currentWeapon.name == "Axe"))
            {
                BulletUI.gameObject.SetActive(false);
                StartCoroutine(ChangeWeaponCoroutine("AXE", "도끼"));
            }
            // 숫자 4 눌렀을 경우 곡괭이로
            else if (Input.GetKeyDown(KeyCode.Alpha4) && !(currentWeapon.name == "Pickaxe"))
            {
                BulletUI.gameObject.SetActive(false);
                StartCoroutine(ChangeWeaponCoroutine("PICKAXE", "곡괭이"));
            }
        }
    }*/

    /// <summary>
    /// 무기 교체 코루틴
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
    /// 무기 교체시 실행중이던 코루틴 및 애니메이션 종료
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
    /// 무기 교체
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
