using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrongAnimal : Animal
{
    [SerializeField] protected int attackDamage; // ���� ������
    [SerializeField] protected float attackDelay; // ���� ������
    [SerializeField] protected LayerMask targetMask; // Ÿ�� ����ũ

    [SerializeField] protected float chaseTime; // �� �߰� �ð�
    protected float currentChaseTime; // �߰� �ð� ���
    [SerializeField] protected float chaseDelayTime; // �߰� ������
    public void Chase(Vector3 targetPos)
    {
        isChasing = true;
        isRunning = true;

        destination = targetPos;
        nav.speed = runSpeed;

        anim.SetBool("Running", isRunning);
        nav.SetDestination(destination);
    }

    public override void Damage(int damage, Vector3 targetPos)
    {
        base.Damage(damage, targetPos);
        if (!isDead)
        {
            Chase(targetPos);
        }
    }

    protected IEnumerator ChaseTargetCoroutine()
    {
        currentChaseTime = 0;

        while (currentChaseTime < chaseTime)
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

    protected IEnumerator AttackCoroutine()
    {
        isAttacking = true;
        nav.ResetPath();
        currentChaseTime = chaseTime; // ������ ���ڸ����� �����ؾ���

        yield return new WaitForSeconds(0.5f);
        transform.LookAt(fieldOfViewAngle.GetTargetPos());

        anim.SetTrigger("Attack");
        yield return new WaitForSeconds(0.5f);
        RaycastHit hit;

        if (Physics.Raycast(transform.position + Vector3.up, transform.forward, out hit, 3, targetMask))
        {
            Debug.Log("�÷��̾� ����!!");
            statusController.DecreaseHP(attackDamage);
        }
        else
        {
            Debug.Log("�÷��̾� ������");
        }

        yield return new WaitForSeconds(attackDelay);
        isAttacking = false;
        StartCoroutine(ChaseTargetCoroutine());

    }
}
