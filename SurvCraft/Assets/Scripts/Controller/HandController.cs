using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    // 현재 장착된 Hand형 타입 무기.
    [SerializeField] private Hand currentHand;

    private bool isAttack = false;
    private bool isSwing = false; // 팔을 휘두르는지

    private RaycastHit hitinfo;

    void Update()
    {
        TryAttack();
    }

    /// <summary>
    /// 좌클릭시 공격
    /// </summary>
    private void TryAttack()
    {
        if (Input.GetButton("Fire1")) // 좌클릭
        {
            if (!isAttack)
            {
                StartCoroutine(AttackCoroutine());
            }
        }
    }

    /// <summary>
    /// 설정된 공격 딜레이에 따른 코드 제어
    /// </summary>
    IEnumerator AttackCoroutine()
    {
        isAttack = true;
        currentHand.anim.SetTrigger("Attack");

        yield return new WaitForSeconds(currentHand.attackAble); // 공격 후 팔이 펴진 상태
        isSwing = true;

        // 공격 활성화 시점
        StartCoroutine(HitCoroutine());

        yield return new WaitForSeconds(currentHand.attackUnable); // 공격 후 팔이 접히는 상태
        isSwing = false;

        yield return new WaitForSeconds(currentHand.attackDelay - currentHand.attackAble - currentHand.attackUnable); // 공격 후 팔이 완전히 접히고 난뒤 공격 딜레이
        isAttack = false;
    }

    /// <summary>
    /// 공격 활성화시 상대 오브젝트의 상태 제어
    /// </summary>
    IEnumerator HitCoroutine()
    {
        while (isSwing)
        {
            if (CheckObject())
            {
                // 오브젝트와 충돌함
                isSwing = false;
                Debug.Log(hitinfo.transform.name);
            }
            yield return null;
        }
    }

    /// <summary>
    /// 충돌된 상대 오브젝트가 있는지
    /// </summary>
    private bool CheckObject()
    {
        if(Physics.Raycast(transform.position, transform.forward, out hitinfo, currentHand.range))
        {
            return true;
        }
        return false;
    }
}
