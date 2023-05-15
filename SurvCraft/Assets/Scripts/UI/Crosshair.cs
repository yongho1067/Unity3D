using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private GunController gunController;

    // ũ�ν���� ���¿� ���� ���� ��Ȯ��
    private float gunAccuracy;

    // �ʿ��ϸ� HUD ȣ��, �ʿ� ������ HUD ��Ȱ��ȭ 
    [SerializeField] private GameObject crosshairHUD;

    public void WalkingAnimation(bool flag)
    {
        WeaponManager.currentWeaponAnim.SetBool("Walk", flag);
        animator.SetBool("isWalk", flag);
    }

    public void RunningAnimation(bool flag)
    {
        WeaponManager.currentWeaponAnim.SetBool("Run", flag);
        animator.SetBool("isRun", flag);
    }

    public void JumpingAnimation(bool flag)
    {
        animator.SetBool("isRun", flag);
    }

    public void CrouchingAnimation(bool flag)
    {
        animator.SetBool("isCrouch", flag);
    }

    public void FineSightAnimation(bool flag)
    {
        animator.SetBool("isFineSight", flag);
    }

    public void FireAnimation()
    {
        if (animator.GetBool("isWalk"))
        {
            animator.SetTrigger("Walk_Fire");
        }
        else if (animator.GetBool("isCrouch"))
        {
            animator.SetTrigger("Crouch_Fire");
        }
        else
        {
            animator.SetTrigger("Idle_Fire");
        }
    }

    public float GetAccuracy()
    {
        if (animator.GetBool("isWalk"))
        {
            gunAccuracy = 0.08f;
        }
        else if (animator.GetBool("isCrouch"))
        {
            gunAccuracy = 0.02f;
        }
        else if (animator.GetBool("isRun"))
        {
            gunAccuracy = 0.1f;
        }
        else if (gunController.GetFineSightMode())
        {
            gunAccuracy = 0.001f;
        }
        else
        {
            gunAccuracy = 0.04f;
        }
        Debug.Log(gunAccuracy);
        return gunAccuracy;
    }

}
