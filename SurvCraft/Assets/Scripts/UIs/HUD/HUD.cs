using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUD : MonoBehaviour
{
    // �ʿ��� ������Ʈ
    [SerializeField] private GunController gunController;
    private Gun currentGun;

    // �ʿ��ϸ� HUD ȣ��, �ʿ� ������ HUD ��Ȱ��ȭ
    [SerializeField] private GameObject bulletHUD;

    // Text UI�� �Ѿ� ���� �ݿ�
    [SerializeField] private TextMeshProUGUI[] text_Bullet;

    private void Update()
    {
        CheckBullet();
    }

    private void CheckBullet()
    {
        currentGun = gunController.GetGun();
        text_Bullet[0].text = currentGun.carryBulletCount.ToString();
        text_Bullet[1].text = currentGun.reloadBulletCount.ToString();
        text_Bullet[2].text = currentGun.currentBulletCount.ToString();
    }




}
