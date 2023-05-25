using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseWeapon : MonoBehaviour
{
    public string closeWeaponName; // ���� ���� �̸�
    public float range; // ���� ����
    public int damage; // ���ݷ�
    public float workSpeed; // �۾� �ӵ�
    public float attackDelay; // ���� �ӵ�
    public float attackAble; // ���� Ȱ��ȭ ����
    public float attackUnable; // ���� ��Ȱ��ȭ ���� 

    public float workDelay; // �۾� �ӵ�
    public float workAble; // �۾� Ȱ��ȭ ����
    public float workUnable; // �۾� ��Ȱ��ȭ ���� 

    public Animator anim; // �ִϸ��̼�

    // ���� ����
    public bool isHand;
    public bool isAxe;
    public bool isPickaxe;


}
