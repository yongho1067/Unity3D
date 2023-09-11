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

    [SerializeField] protected LayerMask layerMask;

    /// <summary>
    /// ��Ŭ���� ����
    /// </summary>
    protected void TryAttack()
    {
        if (!Inventory.inventoryActivated)
        {
            if (Input.GetButton("Fire1")) // ��Ŭ��
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
    /// ������ ���� �����̿� ���� �ڵ� ����
    /// </summary>
    protected IEnumerator AttackCoroutine(string swingType, float attackAble, float attackUnable, float attackDelay)
    {
        isAttack = true;
        currentCloseWeapon.anim.SetTrigger(swingType);

        yield return new WaitForSeconds(attackAble); // ���� �� ���� ���� ����
        isSwing = true;

        // ���� Ȱ��ȭ ����
        StartCoroutine(HitCoroutine());

        yield return new WaitForSeconds(attackUnable); // ���� �� ���� ������ ����
        isSwing = false;

        yield return new WaitForSeconds(attackDelay - attackAble - attackUnable); // ���� �� ���� ������ ������ ���� ���� ������
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
        if (Physics.Raycast(transform.position, transform.forward, out hitinfo, currentCloseWeapon.range, layerMask))
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
