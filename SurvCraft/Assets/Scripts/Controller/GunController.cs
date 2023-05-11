using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    // ���� ������ ��
    [SerializeField] private Gun currentGun;

    // ���� �ӵ� ���
    private float currentFireRate;

    private AudioSource audioSource;

    private bool isReload = false;
    [HideInInspector] public bool isFineSightMode = false;

    // ������ �ϱ� ���� ��
    [SerializeField] private Vector3 originPos;

    // �浹 ���� �޾ƿ�
    private RaycastHit hitinfo;

    [SerializeField] private new Camera camera;

    // �ǰ� ����Ʈ
    [SerializeField] GameObject hit_effect_prefab;


    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        GunFireRateCalc();
        TryFire();
        TryReload();
        TryFineSight();
    }

    #region �߻�
    /// <summary>
    /// �Ѿ� �߻� ���� ���
    /// </summary>
    private void GunFireRateCalc()
    {
        if(currentFireRate > 0)
        {
            currentFireRate -= Time.deltaTime;
        }
    }

    /// <summary>
    /// ���ǿ� �°� Fire ����
    /// </summary>
    private void TryFire()
    {
        if (Input.GetButton("Fire1") && currentFireRate <= 0 && !isReload)
        {
            Fire();
        }
    }

    /// <summary>
    /// �߻縦 ���� ����
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
    /// ���
    /// </summary>
    private void Shoot()
    {
        currentGun.currentBulletCount--;
        currentFireRate = currentGun.fireRate; // ���� �ӵ� ����
        PlaySE(currentGun.fire_Sound);
        currentGun.muzzleFlash.Play();

        // �ѱ� �ݵ� �ڷ�ƾ ����

        StopAllCoroutines();
        StartCoroutine(RetroActionCoroutine());

        Hit();

    }

    /// <summary>
    /// ��� ���� ����
    /// </summary>
    private void PlaySE(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }
    #endregion

    #region ����
    /// <summary>
    /// ��Ŭ�� �� ������
    /// </summary>
    private void TryFineSight()
    {
        if (Input.GetButtonDown("Fire2") && !isReload)
        {
            FineSight();
        }
    }

    /// <summary>
    /// ������ ����
    /// </summary>
    public void CancelFineSight()
    {
        if (isFineSightMode)
        {
            FineSight();
        }
    }

    /// <summary>
    /// ������
    /// </summary>
    private void FineSight()
    {
        isFineSightMode = !isFineSightMode;
        currentGun.anim.SetBool("FineSightMode", isFineSightMode);

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
    /// ������ �� �ѱ� ��ġ ����
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
    /// ������ ���� �� �ѱ� ����ġ 
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
    /// �ѱ� �ݵ�
    /// </summary>
    IEnumerator RetroActionCoroutine()
    {
        Vector3 recoilBack = new Vector3(currentGun.retroActionForce, originPos.y, originPos.z); // ������ ������ �� �ִ�ݵ�
        Vector3 retroActionRecoilBack = new Vector3(currentGun.retroActionFineSightForce, currentGun.fineSightOriginPos.y, currentGun.fineSightOriginPos.z); // ������ �� �ִ�ݵ� 

        if (!isFineSightMode)
        {
            currentGun.transform.localPosition = originPos;

            // �ݵ� ����
            while (currentGun.transform.localPosition.x <= currentGun.retroActionForce - 0.02f) // ���� �� 0.02
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, recoilBack, 0.4f);
                yield return null;
            }

            // ����ġ
            while (currentGun.transform.localPosition != originPos)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, originPos, 0.1f);
                yield return null;
            }
        }
        else
        {
            currentGun.transform.localPosition = currentGun.fineSightOriginPos;

            // �ݵ� ����
            while (currentGun.transform.localPosition.x <= currentGun.retroActionFineSightForce - 0.02f) // ���� �� 0.02
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, retroActionRecoilBack, 0.4f);
                yield return null;
            }

            // ����ġ
            while (currentGun.transform.localPosition != currentGun.fineSightOriginPos)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, currentGun.fineSightOriginPos, 0.1f);
                yield return null;
            }
        }
    }
    #endregion

    #region Ÿ��

    private void Hit()
    {
        if(Physics.Raycast(camera.transform.position, camera.transform.forward, out hitinfo, currentGun.range))
        {
            // hitinfo.point => hit �̺�Ʈ�� �߻��� ������Ʈ�� ��ǥ
            // hitinfo.normal => �浹�� ��ü�� ǥ��
            GameObject instance = Instantiate(hit_effect_prefab, hitinfo.point, Quaternion.LookRotation(hitinfo.normal));
            Destroy(instance, 2f);

        }
    }
    #endregion

    #region ������
    /// <summary>
    /// Ű���� R ������ ������
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
    /// �Ѿ� �������� �Ѿ� ���� ����
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
            Debug.Log("�Ѿ� ����");
        }

        isReload = false;
    }
    #endregion


    public Gun GetGun()
    {
        return currentGun;
    }

    
}
