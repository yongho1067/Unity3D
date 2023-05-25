using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{

    // 깎일 나무 조각들
    [SerializeField] private GameObject[] treePieces;
    [SerializeField] private GameObject treeCenter;

    // 나무 타격 효과 (파편)
    [SerializeField] private GameObject hit_Effect;

    // 나무 파편 제거 시간
    [SerializeField] private float hit_Effect_DestroyTime;
    
    // 벌목시 나무 제거 시간
    [SerializeField] private float destroyTime;


    [SerializeField] private CapsuleCollider parent_Collider;

    // 자식 트리 쓰러질 때 필요한 컴포넌트 활성화 및 중력 활성화
    [SerializeField] private CapsuleCollider child_Collider;
    [SerializeField] private Rigidbody child_Rigidbody;

}
