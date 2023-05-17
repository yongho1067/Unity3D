using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeController : CloseWeaponController
{
    // 활성화 여부
    public static bool isActivate = false;


    protected void Update()
    { 
        if (isActivate)
        {
            TryAttack();
        }

    }
    protected override IEnumerator HitCoroutine()
    {
        while (isSwing)
        {
            if (CheckObject())
            {
                isSwing = false;
                Debug.Log(hitinfo.transform.name);
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
