using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor.PackageManager;
using UnityEngine;

public class PickaxeController : CloseWeaponController
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
                if(hitinfo.transform.tag == "Rock")
                {
                    hitinfo.transform.GetComponent<Rock>().Mining();
                }
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
