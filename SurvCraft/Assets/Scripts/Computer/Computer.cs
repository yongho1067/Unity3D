using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Kit
{
    public string KitName;
    public string kitContent;
    public string[] needItemName;
    public int[] needItemNum;

    public GameObject go_Kit_Prefab;

}

public class Computer : MonoBehaviour
{
    [SerializeField] private Kit[] kits; // 키트 아이템들
    [SerializeField] private Transform tf_ItemAppear; // 아이템 생성 위치
    [SerializeField] private GameObject go_BaseUI; // 컴퓨터 메인 화면 UI

    private bool isCraft = false; // 아이템 제작 중인지 -> 아이템 제작 딜레이
    public bool isPowerOn = false; // 컴퓨터 전원 켜졌는지

    private AudioSource audioSource;
    [SerializeField] private AudioClip sound_ButtonClick;
    [SerializeField] private AudioClip sound_Beep;
    [SerializeField] private AudioClip sound_PowerOn;

    // 필요한 컴포넌트
    [SerializeField] private Inventory inventory;
    [SerializeField] private ComputerToolTip toolTip;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if(isPowerOn)
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                PowerOff();
            }    
        }
    }

    public void PowerOn()
    {
        PlaySE(sound_PowerOn);

        GameManager.isPowerOnComputer = true;

        isPowerOn = true;
        go_BaseUI.SetActive(true);
    }

    private void PowerOff()
    {
        GameManager.isPowerOnComputer = false;

        isPowerOn = false;
        go_BaseUI.SetActive(false);
        toolTip.HideToolTip();
    }

    public void ShowToolTip(int _buttonNum)
    {
        toolTip.ShowToolTip(kits[_buttonNum].KitName, kits[_buttonNum].kitContent, kits[_buttonNum].needItemName, kits[_buttonNum].needItemNum);
    }

    public void HideToolTip()
    {
        toolTip.HideToolTip();
    }

    private void PlaySE(AudioClip _clip)
    {
        audioSource.clip = _clip;
        audioSource.Play();
    }

    public void ClickButton(int _slotNum)
    {
        PlaySE(sound_ButtonClick);

        if(!isCraft)
        {
            if(!CheckIngredient(_slotNum)) // 재료 체크
            {
                return;
            }

            isCraft = true;
            UseIngredient(_slotNum); // 재료 사용

            StartCoroutine(CraftCoroutine(_slotNum)); // Kits 생성
            PowerOff();
        }
    }

    private bool CheckIngredient(int _slotNum)
    {
        for (int i = 0; i < kits[_slotNum].needItemName.Length; i++)
        {
            if (inventory.GetItemCount(kits[_slotNum].needItemName[i]) < kits[_slotNum].needItemNum[i])
            {
                PlaySE(sound_Beep);
                return false;
            }
        }
        return true;
    }

    private void UseIngredient(int _slotNum)
    {
        for (int i = 0; i < kits[_slotNum].needItemName.Length; i++)
        {
            inventory.SetItemCount(kits[_slotNum].needItemName[i], kits[_slotNum].needItemNum[i]);
        }
    }

    IEnumerator CraftCoroutine(int _slotNum)
    {
        yield return new WaitForSeconds(2f);

        Instantiate(kits[_slotNum].go_Kit_Prefab, tf_ItemAppear.position, Quaternion.identity);
        isCraft = false;
    }


}
