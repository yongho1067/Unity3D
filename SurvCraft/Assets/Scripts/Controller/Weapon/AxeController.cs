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
                if (hitinfo.transform.tag == "Twig")
                {
                    hitinfo.transform.GetComponent<Twig>().Damage(transform);
                }
                else if (hitinfo.transform.tag == "Tree")
                {
                    hitinfo.transform.GetComponent<Tree>().Damage(hitinfo.point, transform.eulerAngles.y);
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
