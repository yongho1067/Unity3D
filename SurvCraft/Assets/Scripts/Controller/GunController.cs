using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField] private Gun currentGun;

    private float currentFireRate;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        GunFireRateCalc();
        TryFire();
    }

    /// <summary>
    /// 총알 발사 간격 계산
    /// </summary>
    private void GunFireRateCalc()
    {
        if(currentFireRate > 0)
        {
            currentFireRate -= Time.deltaTime;
        }
    }

    /// <summary>
    /// 조건에 맞게 Fire 실행
    /// </summary>
    private void TryFire()
    {
        if (Input.GetButton("Fire1") && currentFireRate <= 0)
        {
            Fire();
        }
    }

    /// <summary>
    /// 발사를 위한 과정
    /// </summary>
    private void Fire()
    {
        currentFireRate = currentGun.fireRate;
        Shoot();
    }

    /// <summary>
    /// 사격
    /// </summary>
    private void Shoot()
    {
        PlaySE(currentGun.fire_Sound);
        currentGun.muzzleFlash.Play();
        Debug.Log("FIRE");
    }

    private void PlaySE(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }
}
