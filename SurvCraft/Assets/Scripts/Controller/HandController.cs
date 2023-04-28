using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    // ���� ������ Hand�� Ÿ�� ����.
    [SerializeField] private Hand currentHand;

    private bool isAttack = false;
    private bool isSwing = false; // ���� �ֵθ�����

    private RaycastHit hitinfo;

    void Update()
    {
        TryAttack();
    }

    /// <summary>
    /// ��Ŭ���� ����
    /// </summary>
    private void TryAttack()
    {
        if (Input.GetButton("Fire1")) // ��Ŭ��
        {
            if (!isAttack)
            {
                StartCoroutine(AttackCoroutine());
            }
        }
    }

    /// <summary>
    /// ������ ���� �����̿� ���� �ڵ� ����
    /// </summary>
    IEnumerator AttackCoroutine()
    {
        isAttack = true;
        currentHand.anim.SetTrigger("Attack");

        yield return new WaitForSeconds(currentHand.attackAble); // ���� �� ���� ���� ����
        isSwing = true;

        // ���� Ȱ��ȭ ����
        StartCoroutine(HitCoroutine());

        yield return new WaitForSeconds(currentHand.attackUnable); // ���� �� ���� ������ ����
        isSwing = false;

        yield return new WaitForSeconds(currentHand.attackDelay - currentHand.attackAble - currentHand.attackUnable); // ���� �� ���� ������ ������ ���� ���� ������
        isAttack = false;
    }

    /// <summary>
    /// ���� Ȱ��ȭ�� ��� ������Ʈ�� ���� ����
    /// </summary>
    IEnumerator HitCoroutine()
    {
        while (isSwing)
        {
            if (CheckObject())
            {
                // ������Ʈ�� �浹��
                isSwing = false;
                Debug.Log(hitinfo.transform.name);
            }
            yield return null;
        }
    }

    /// <summary>
    /// �浹�� ��� ������Ʈ�� �ִ���
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
