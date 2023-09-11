using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakAnimal : Animal
{
    public void Run(Vector3 targetPos)
    {
        nav.speed = runSpeed;

        destination = new Vector3(transform.position.x - targetPos.x, 0f, transform.position.z - targetPos.z).normalized;
        currentTime = runTime;

        isWalking = false;
        isRunning = true;

        anim.SetBool("Running", isRunning);
    }

    public override void Damage(int damage, Vector3 targetPos)
    {
        base.Damage(damage, targetPos);
        if (!isDead)
        {
            Run(targetPos);
        }
    }

}
