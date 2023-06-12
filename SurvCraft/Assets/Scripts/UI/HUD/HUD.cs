using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUD : MonoBehaviour
{
    // 필요한 컴포넌트
    [SerializeField] private GunController gunController;
    private Gun currentGun;

    // 필요하면 HUD 호출, 필요 없으면 HUD 비활성화
    [SerializeField] private GameObject bulletHUD;

    // Text UI에 총알 갯수 반영
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
