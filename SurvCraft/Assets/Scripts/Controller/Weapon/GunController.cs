using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    // 활성화 여부
    public static bool isActivate = false;
    
    // 현재 장착된 총
    [SerializeField] private Gun currentGun;

    // 연사 속도 계산
    private float currentFireRate;

    private AudioSource audioSource;

    private bool isReload = false;
    [HideInInspector] public bool isFineSightMode = false;

    // 정조준 하기 이전 값
    [SerializeField] private Vector3 originPos;

    // 충돌 정보 받아옴
    private RaycastHit hitinfo;

    [SerializeField] private new Camera camera;

    // 피격 이펙트
    [SerializeField] GameObject hit_effect_prefab;

    // 피격 이펙트 하이라키 보관함
    [SerializeField] GameObject hit_effect_prefab_temp;

    private Crosshair crosshair;



    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        crosshair = FindObjectOfType<Crosshair>();
    }

    private void Update()
    {
        if (isActivate)
        {
            GunFireRateCalc();
            TryFire();
            TryReload();
            TryFineSight();
        }
        
    }

    #region 발사
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
        if (Input.GetButton("Fire1") && currentFireRate <= 0 && !isReload)
        {
            Fire();
        }
    }

    /// <summary>
    /// 발사를 위한 과정
    /// </summary>
    private void Fire()
    {
        if (!isReload)
        {
            if (currentGun.currentBulletCount > 0)
            {
                Shoot();
            }
            else
            {
                CancelFineSight();
                StartCoroutine(ReloadCoroutine());
            }
        }
        
    }

    /// <summary>
    /// 사격
    /// </summary>
    private void Shoot()
    {
        crosshair.FireAnimation();

        currentGun.currentBulletCount--;
        currentFireRate = currentGun.fireRate; // 연사 속도 재계산
        PlaySE(currentGun.fire_Sound);
        currentGun.muzzleFlash.Play();

        // 총기 반동 코루틴 실행

        StopAllCoroutines();
        StartCoroutine(RetroActionCoroutine());

        Hit();

    }

    /// <summary>
    /// 사격 사운드 실행
    /// </summary>
    private void PlaySE(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.volume = 0.1f;
        audioSource.Play();
    }
    #endregion

    #region 조준
    /// <summary>
    /// 우클릭 시 정조준
    /// </summary>
    private void TryFineSight()
    {
        if (Input.GetButtonDown("Fire2") && !isReload)
        {
            FineSight();
        }
    }

    /// <summary>
    /// 정조준 해제
    /// </summary>
    public void CancelFineSight()
    {
        if (isFineSightMode)
        {
            FineSight();
        }
    }

    /// <summary>
    /// 정조준
    /// </summary>
    private void FineSight()
    {
        isFineSightMode = !isFineSightMode;
        currentGun.anim.SetBool("FineSightMode", isFineSightMode);
        crosshair.FineSightAnimation(isFineSightMode);

        if (isFineSightMode)
        {
            StopAllCoroutines();
            StartCoroutine(FineSightActivateCoroutine());
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(FineSightDeactivateCoroutine());
        }
    }

    /// <summary>
    /// 정조준 시 총구 위치 변경
    /// </summary>
    IEnumerator FineSightActivateCoroutine()
    {
        while (currentGun.transform.localPosition != currentGun.fineSightOriginPos)
        {
            currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, currentGun.fineSightOriginPos, 0.2f);
            yield return null;
        }
    }

    /// <summary>
    /// 정조준 해제 시 총구 원위치 
    /// </summary>
    IEnumerator FineSightDeactivateCoroutine()
    {
        while (currentGun.transform.localPosition != originPos)
        {
            currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, originPos, 0.2f);
            yield return null;
        }
    }

    /// <summary>
    /// 총기 반동
    /// </summary>
    IEnumerator RetroActionCoroutine()
    {
        Vector3 recoilBack = new Vector3(currentGun.retroActionForce, originPos.y, originPos.z); // 정조준 안했을 시 최대반동
        Vector3 retroActionRecoilBack = new Vector3(currentGun.retroActionFineSightForce, currentGun.fineSightOriginPos.y, currentGun.fineSightOriginPos.z); // 정조준 시 최대반동 

        if (!isFineSightMode)
        {
            currentGun.transform.localPosition = originPos;

            // 반동 시작
            while (currentGun.transform.localPosition.x <= currentGun.retroActionForce - 0.02f) // 여유 값 0.02
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, recoilBack, 0.4f);
                yield return null;
            }

            // 원위치
            while (currentGun.transform.localPosition != originPos)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, originPos, 0.1f);
                yield return null;
            }
        }
        else
        {
            currentGun.transform.localPosition = currentGun.fineSightOriginPos;

            // 반동 시작
            while (currentGun.transform.localPosition.x <= currentGun.retroActionFineSightForce - 0.02f) // 여유 값 0.02
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, retroActionRecoilBack, 0.4f);
                yield return null;
            }

            // 원위치
            while (currentGun.transform.localPosition != currentGun.fineSightOriginPos)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, currentGun.fineSightOriginPos, 0.1f);
                yield return null;
            }
        }
    }

    public bool GetFineSightMode()
    {
        return isFineSightMode;
    }
    #endregion

    #region 타격

    private void Hit()
    {
        if(Physics.Raycast(camera.transform.position, camera.transform.forward + // x축 랜덤 , y축 랜덤 반동 + 총의 기존반동
            new Vector3(Random.Range(-crosshair.GetAccuracy() - currentGun.accuracy, crosshair.GetAccuracy() + currentGun.accuracy),
                        Random.Range(-crosshair.GetAccuracy() - currentGun.accuracy, crosshair.GetAccuracy() + currentGun.accuracy), 0f) 
            , out hitinfo, currentGun.range))
        {
            // hitinfo.point => hit 이벤트가 발생한 오브젝트의 좌표
            // hitinfo.normal => 충돌한 객체의 표면
            GameObject instance = Instantiate(hit_effect_prefab, hitinfo.point, Quaternion.LookRotation(hitinfo.normal), hit_effect_prefab_temp.transform);
            Destroy(instance, 2f);

        }
    }
    #endregion

    #region 재장전
    /// <summary>
    /// 키보드 R 누를시 재장전
    /// </summary>
    private void TryReload()
    {
        if(Input.GetKeyDown(KeyCode.R) && !isReload && currentGun.currentBulletCount < currentGun.reloadBulletCount)
        {
            CancelFineSight();
            StartCoroutine(ReloadCoroutine());
        }
    }

    /// <summary>
    /// 재장전 정지
    /// </summary>
    public void CancelReload()
    {
        if (isReload)
        {
            StopAllCoroutines();
            isReload = false;
        }
    }

    /// <summary>
    /// 총알 재장전시 총얄 갯수 연산
    /// </summary>
    IEnumerator ReloadCoroutine()
    {
        if(currentGun.carryBulletCount > 0)
        {
            isReload = true;
            currentGun.anim.SetTrigger("Reload");

            currentGun.carryBulletCount += currentGun.currentBulletCount;
            currentGun.currentBulletCount = 0;

            yield return new WaitForSeconds(currentGun.reloadTime);
            
            if(currentGun.carryBulletCount >= currentGun.reloadBulletCount)
            {
                currentGun.currentBulletCount = currentGun.reloadBulletCount;
                currentGun.carryBulletCount -= currentGun.reloadBulletCount;
            }
            else
            {
                currentGun.currentBulletCount = currentGun.carryBulletCount;
                currentGun.carryBulletCount = 0;
            }
        }
        else
        {
            Debug.Log("총알 부족");
        }

        isReload = false;
    }
    #endregion

    #region 기타
    public Gun GetGun()
    {
        return currentGun;
    }

    /// <summary>
    /// 무기 교체
    /// </summary>
    public void GunChange(Gun gun)
    {
        if(WeaponManager.currentWeapon != null)
        {
            WeaponManager.currentWeapon.gameObject.SetActive(false);
        }

        currentGun = gun;
        WeaponManager.currentWeapon = currentGun.GetComponent<Transform>();
        WeaponManager.currentWeaponAnim = currentGun.anim;

        currentGun.transform.localPosition = Vector3.zero;
        currentGun.gameObject.SetActive(true);
        isActivate = true;
    }
    #endregion


}
