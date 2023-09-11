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

            // ����� ������ �ְ�
            if (Vector3.Distance(transform.position, fieldOfViewAngle.GetTargetPos()) <= 3f) // 3f�� �ſ� ����� �Ÿ�
            {
                if (fieldOfViewAngle.View()) // ���տ� ���� ���,
                {
                    Debug.Log("�÷��̾� ���� �õ�");
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
        currentChaseTime = chaseTime; // ������ ���ڸ����� �����ؾ���

        yield return new WaitForSeconds(0.5f);
        transform.LookAt(fieldOfViewAngle.GetTargetPos());

        anim.SetTrigger("Attack");
        yield return new WaitForSeconds(0.5f);
        RaycastHit hit;

        if(Physics.Raycast(transform.position + Vector3.up, transform.forward, out hit, 3, targetMask))
        {
            Debug.Log("�÷��̾� ����!!");
        }
        else
        {
            Debug.Log("�÷��̾� ������");
        }

        yield return new WaitForSeconds(attackDelay);
        isAttacking = false;
        StartCoroutine(ChaseTargetCoroutine());

    }*/
}
