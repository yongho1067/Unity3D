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
        if (Input.GetButton("Fire2"))
        {
            quickSlotController.Eatitem();
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
