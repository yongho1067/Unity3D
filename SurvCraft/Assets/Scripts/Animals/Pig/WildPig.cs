using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WildPig : StrongAnimal
{

    protected override void Update()
    {
        base.Update();
        if (fieldOfViewAngle.View() && !isDead && !isAttacking)
        {
            StopAllCoroutines();
            StartCoroutine(ChaseTargetCoroutine());
        }
    }

    /*IEnumerator ChaseTargetCoroutine()
    {
        currentChaseTime = 0;

        while(currentChaseTime < chaseTime)
        {
            Chase(fieldOfViewAngle.GetTargetPos());

            // 충분히 가까이 있고
            if (Vector3.Distance(transform.position, fieldOfViewAngle.GetTargetPos()) <= 3f) // 3f는 매우 가까운 거리
            {
                if (fieldOfViewAngle.View()) // 눈앞에 있을 경우,
                {
                    Debug.Log("플레이어 공격 시도");
                    StartCoroutine(AttackCoroutine());
                }
            }
            yield return new WaitForSeconds(chaseDelayTime);
            currentChaseTime += chaseDelayTime;
        }

        isChasing = false;
        isRunning = false;
        anim.SetBool("Running", isRunning);
        nav.ResetPath();
    }

    IEnumerator AttackCoroutine()
    {
        isAttacking = true;
        nav.ResetPath();
        currentChaseTime = chaseTime; // 공격은 제자리에서 공격해야함

        yield return new WaitForSeconds(0.5f);
        transform.LookAt(fieldOfViewAngle.GetTargetPos());

        anim.SetTrigger("Attack");
        yield return new WaitForSeconds(0.5f);
        RaycastHit hit;

        if(Physics.Raycast(transform.position + Vector3.up, transform.forward, out hit, 3, targetMask))
        {
            Debug.Log("플레이어 적중!!");
        }
        else
        {
            Debug.Log("플레이어 빗나감");
        }

        yield return new WaitForSeconds(attackDelay);
        isAttacking = false;
        StartCoroutine(ChaseTargetCoroutine());

    }*/
}
