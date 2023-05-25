using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{

    // ���� ���� ������
    [SerializeField] private GameObject[] treePieces;
    [SerializeField] private GameObject treeCenter;

    // ���� Ÿ�� ȿ�� (����)
    [SerializeField] private GameObject hit_Effect;

    // ���� ���� ���� �ð�
    [SerializeField] private float hit_Effect_DestroyTime;
    
    // ����� ���� ���� �ð�
    [SerializeField] private float destroyTime;


    [SerializeField] private CapsuleCollider parent_Collider;

    // �ڽ� Ʈ�� ������ �� �ʿ��� ������Ʈ Ȱ��ȭ �� �߷� Ȱ��ȭ
    [SerializeField] private CapsuleCollider child_Collider;
    [SerializeField] private Rigidbody child_Rigidbody;

}
