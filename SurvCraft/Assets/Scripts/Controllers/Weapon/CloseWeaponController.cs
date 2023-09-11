using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 미완성 클래스 = 추상 클래스
public abstract class CloseWeaponController : MonoBehaviour
{
   

    // 현재 장착된 무기.
    [SerializeField] protected CloseWeapon currentCloseWeapon;

    protected bool isAttack = false;
    protected bool isSwing = false; // 팔을 휘두르는지

    protected RaycastHit hitinfo;

    [SerializeField] protected LayerMask layerMask;

    /// <summary>
    /// 좌클릭시 공격
    /// </summary>
    protected void TryAttack()
    {
        if (!Inventory.inventoryActivated)
        {
            if (Input.GetButton("Fire1")) // 좌클릭
            {
                if (!isAttack)
                {
                    if (CheckObject())
                    {
                        if (currentCloseWeapon.isAxe && hitinfo.transform.tag == "Tree")
                        {
                            StartCoroutine(AttackCoroutine("Hit_Tree", currentCloseWeapon.workAble, currentCloseWeapon.workUnable, currentCloseWeapon.workDelay));
                            return;
                        }
                    }
                    StartCoroutine(AttackCoroutine("Attack", currentCloseWeapon.attackAble, currentCloseWeapon.attackUnable, currentCloseWeapon.attackDelay));
                }
            }
        }
 
    }

    /// <summary>
    /// 설정된 공격 딜레이에 따른 코드 제어
    /// </summary>
    protected IEnumerator AttackCoroutine(string swingType, float attackAble, float attackUnable, float attackDelay)
    {
        isAttack = true;
        currentCloseWeapon.anim.SetTrigger(swingType);

        yield return new WaitForSeconds(attackAble); // 공격 후 팔이 펴진 상태
        isSwing = true;

        // 공격 활성화 시점
        StartCoroutine(HitCoroutine());

        yield return new WaitForSeconds(attackUnable); // 공격 후 팔이 접히는 상태
        isSwing = false;

        yield return new WaitForSeconds(attackDelay - attackAble - attackUnable); // 공격 후 팔이 완전히 접히고 난뒤 공격 딜레이
        isAttack = false;
    }

    /// <summary>
    /// 공격 활성화시 상대 오브젝트의 상태 제어
    /// </summary>
    /// abstract = 미완성 / 추상 코루틴
    protected abstract IEnumerator HitCoroutine();


    /// <summary>
    /// 충돌된 상대 오브젝트가 있는지
    /// </summary>
    protected bool CheckObject()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hitinfo, currentCloseWeapon.range, layerMask))
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 무기 교체
    /// </summary>
    /// 
    /// 완성 함수이지만, 추가 편집이 가능한 함수
    public virtual void CloseWeaponChange(CloseWeapon closeWeapon)
    {
        if (WeaponManager.currentWeapon != null)
        {
            WeaponManager.currentWeapon.gameObject.SetActive(false);
        }

        currentCloseWeapon = closeWeapon;
        WeaponManager.currentWeapon = currentCloseWeapon.GetComponent<Transform>();
        WeaponManager.currentWeaponAnim = currentCloseWeapon.anim;

        currentCloseWeapon.transform.localPosition = Vector3.zero;
        currentCloseWeapon.gameObject.SetActive(true);
    }
}
