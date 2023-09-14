using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : CloseWeaponController
{ 
    // 활성화 여부
    public static bool isActivate = true;
    [SerializeField] private QuickSlotController quickSlotController;

    private void Start()
    {
        WeaponManager.currentWeapon = currentCloseWeapon.GetComponent<Transform>();
        WeaponManager.currentWeaponAnim = currentCloseWeapon.anim;
    }

    void Update()
    {
        if (isActivate)
        {
            if(QuickSlotController.handItem == null)
            {
                TryAttack();
            }
            else
            {
                TryEating();
            }

            
        }
    }
    private void TryEating()
    {
        if (Input.GetButton("Fire2") && !quickSlotController.GetisCoolTime())
        {
            currentCloseWeapon.anim.SetTrigger("Eat");
            quickSlotController.DecreaseSelectedItem();
        }
    }

    protected override IEnumerator HitCoroutine()
    {
        while (isSwing)
        {
            if (CheckObject())
            {
                if(hitinfo.transform.tag == "Grass")
                {
                    hitinfo.transform.GetComponent<Grass>().Damage();
                }
                else if (hitinfo.transform.tag == "WeakAnimal")
                {
                    SoundManager.soundManager.PlaySE("Animal_Hit");
                    hitinfo.transform.GetComponent<WeakAnimal>().Damage(1, transform.position);
                }else if (hitinfo.transform.tag == "StrongAnimal")
                {
                    SoundManager.soundManager.PlaySE("Animal_Hit");
                    // StrongAnimal 스크립트 생성시 교체
                    hitinfo.transform.GetComponent<WeakAnimal>().Damage(1, transform.position);
                }

                isSwing = false;
            }
            yield return null;
        }
    }
    public override void CloseWeaponChange(CloseWeapon closeWeapon)
    {
        base.CloseWeaponChange(closeWeapon);
        isActivate = true;
    }
}
