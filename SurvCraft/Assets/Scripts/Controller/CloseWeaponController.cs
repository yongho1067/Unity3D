using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �̿ϼ� Ŭ���� = �߻� Ŭ����
public abstract class CloseWeaponController : MonoBehaviour
{
   

    // ���� ������ ����.
    [SerializeField] protected CloseWeapon currentCloseWeapon;

    protected bool isAttack = false;
    protected bool isSwing = false; // ���� �ֵθ�����

    protected RaycastHit hitinfo;

    /// <summary>
    /// ��Ŭ���� ����
    /// </summary>
    protected void TryAttack()
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
    protected IEnumerator AttackCoroutine()
    {
        isAttack = true;
        currentCloseWeapon.anim.SetTrigger("Attack");

        yield return new WaitForSeconds(currentCloseWeapon.attackAble); // ���� �� ���� ���� ����
        isSwing = true;

        // ���� Ȱ��ȭ ����
        StartCoroutine(HitCoroutine());

        yield return new WaitForSeconds(currentCloseWeapon.attackUnable); // ���� �� ���� ������ ����
        isSwing = false;

        yield return new WaitForSeconds(currentCloseWeapon.attackDelay - currentCloseWeapon.attackAble - currentCloseWeapon.attackUnable); // ���� �� ���� ������ ������ ���� ���� ������
        isAttack = false;
    }

    /// <summary>
    /// ���� Ȱ��ȭ�� ��� ������Ʈ�� ���� ����
    /// </summary>
    /// abstract = �̿ϼ� / �߻� �ڷ�ƾ
    protected abstract IEnumerator HitCoroutine();


    /// <summary>
    /// �浹�� ��� ������Ʈ�� �ִ���
    /// </summary>
    protected bool CheckObject()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hitinfo, currentCloseWeapon.range))
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// ���� ��ü
    /// </summary>
    /// 
    /// �ϼ� �Լ�������, �߰� ������ ������ �Լ�
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
