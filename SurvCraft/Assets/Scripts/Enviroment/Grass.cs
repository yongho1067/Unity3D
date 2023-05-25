using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grass : MonoBehaviour
{
    [SerializeField] private int hp;

    // 이펙트 제거 시간
    [SerializeField] private float destroyTime;

    // 이펙트 폭발력 세기
    [SerializeField] private float force;

    // 타격 효과
    [SerializeField] private GameObject hit_effect_prefab;

    private Rigidbody[] rigidbodies;
    private BoxCollider[] boxColliders;

    [SerializeField] private string hit_Sound;
    [SerializeField] GameObject instance_TEMP;

    private void Start()
    {
        rigidbodies = transform.GetComponentsInChildren<Rigidbody>();
        boxColliders = transform.GetComponentsInChildren<BoxCollider>();
    }

    public void Damage()
    {
        hp--;

        Hit();    

        if(hp <= 0)
        {
            hp = 0;
            Destruction();
        }
    }

    private void Hit()
    {
        SoundManager.soundManager.PlaySE(hit_Sound);

        GameObject instance = Instantiate(hit_effect_prefab, transform.position + Vector3.up, Quaternion.identity, instance_TEMP.transform);
        Destroy(instance, destroyTime);
    }

    private void Destruction()
    {
        for(int i = 0; i < rigidbodies.Length; i++)
        {
            rigidbodies[i].useGravity = true;
            
            // 폭발세기, 폭발위치, 폭발반경
            rigidbodies[i].AddExplosionForce(force, transform.position, 1f);
            boxColliders[i].enabled = true;
        }

        Destroy(gameObject, destroyTime);
    }

}
