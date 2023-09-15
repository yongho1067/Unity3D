using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : CloseWeaponController
{ 
    // 활성화 여부
    public static bool isActivate = true;
    [SerializeField] private QuickSlotController quickSlotController;
    public static Item currentKit; // 설치하려는 킷

    private bool isPreview = false;

    private GameObject go_Preview; // 설치할 키트 프리뷰
    private Vector3 previewPos; // 설치할 키트 위치

    [SerializeField] private float addRange; // 건축시 추가 사정거리

    private void Start()
    {
        WeaponManager.currentWeapon = currentCloseWeapon.GetComponent<Transform>();
        WeaponManager.currentWeaponAnim = currentCloseWeapon.anim;
    }

    void Update()
    {
        if (isActivate)
        {
            // 손에 키트아이템을 들고있지 않은 경우 -> 보통 상태 -> 기존의 기능을 구현해야함
            if(currentKit == null)
            {
                if (QuickSlotController.handItem == null && GameManager.canPlayerMove)
                {
                    TryAttack();
                }
                else
                {
                    TryEating();
                }
            }
            // 손에 키트아이템이 있는 경우 -> 특별한 상태 -> 해당하는 기능만 구현되야함
            else
            {
                if(!isPreview)
                {
                    InstallPreviewKit();
                }
                PreviewPosUpdate();
                BuildKit();
            }
        }
    }

    private void InstallPreviewKit()
    {
        isPreview = true;
        go_Preview = Instantiate(currentKit.kitPreviewPrefab, transform.position, Quaternion.identity);
    }

    private void PreviewPosUpdate()
    {
        if(Physics.Raycast(transform.position, transform.forward, out hitinfo, currentCloseWeapon.range + addRange, layerMask))
        {
            previewPos = hitinfo.point;
            go_Preview.transform.position = previewPos;
        }
    }

    private void BuildKit()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if(go_Preview.GetComponent<PreviewObject>().IsBuildable())
            {
                quickSlotController.DecreaseSelectedItem(); // 선택된 슬롯 아이템 차감
                GameObject temp = Instantiate(currentKit.kitPrefab, previewPos, Quaternion.identity);
                temp.name = currentKit.itemName;
                Destroy(go_Preview);
                currentKit = null;
                isPreview = false;
            }
        }
    }

    public void Cancel()
    {
        Destroy(go_Preview);
        currentKit = null;
        isPreview = false;
    }

    private void TryEating()
    {
        if (Input.GetButton("Fire2") && !quickSlotController.GetisCoolTime())
        {
            currentCloseWeapon.anim.SetTrigger("Eat");
            quickSlotController.DecreaseSelectedItem();
        }
    }

    protected override IEnumerator HitCoroutine()
    {
        while (isSwing)
        {
            if (CheckObject())
            {
                if(hitinfo.transform.tag == "Grass")
                {
                    hitinfo.transform.GetComponent<Grass>().Damage();
                }
                else if (hitinfo.transform.tag == "WeakAnimal")
                {
                    SoundManager.soundManager.PlaySE("Animal_Hit");
                    hitinfo.transform.GetComponent<WeakAnimal>().Damage(1, transform.position);
                }else if (hitinfo.transform.tag == "StrongAnimal")
                {
                    SoundManager.soundManager.PlaySE("Animal_Hit");
                    // StrongAnimal 스크립트 생성시 교체
                    hitinfo.transform.GetComponent<WeakAnimal>().Damage(1, transform.position);
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
