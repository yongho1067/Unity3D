using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseWeapon : MonoBehaviour
{
    public string closeWeaponName; // 근접 무기 이름
    public float range; // 공격 범위
    public int damage; // 공격력
    public float workSpeed; // 작업 속도
    public float attackDelay; // 공격 속도
    public float attackAble; // 공격 활성화 시점
    public float attackUnable; // 공격 비활성화 시점 

    public float workDelay; // 작업 속도
    public float workAble; // 작업 활성화 시점
    public float workUnable; // 작업 비활성화 시점 

    public Animator anim; // 애니메이션

    // 무기 유형
    public bool isHand;
    public bool isAxe;
    public bool isPickaxe;


}
