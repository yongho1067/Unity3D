using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : CloseWeaponController
{ 
    // Ȱ��ȭ ����
    public static bool isActivate = true;
    [SerializeField] private QuickSlotController quickSlotController;
    public static Item currentKit; // ��ġ�Ϸ��� Ŷ

    private bool isPreview = false;

    private GameObject go_Preview; // ��ġ�� ŰƮ ������
    private Vector3 previewPos; // ��ġ�� ŰƮ ��ġ

    [SerializeField] private float addRange; // ����� �߰� �����Ÿ�

    private void Start()
    {
        WeaponManager.currentWeapon = currentCloseWeapon.GetComponent<Transform>();
        WeaponManager.currentWeaponAnim = currentCloseWeapon.anim;
    }

    void Update()
    {
        if (isActivate)
        {
            // �տ� ŰƮ�������� ������� ���� ��� -> ���� ���� -> ������ ����� �����ؾ���
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
            // �տ� ŰƮ�������� �ִ� ��� -> Ư���� ���� -> �ش��ϴ� ��ɸ� �����Ǿ���
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
                quickSlotController.DecreaseSelectedItem(); // ���õ� ���� ������ ����
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
                    // StrongAnimal ��ũ��Ʈ ������ ��ü
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
