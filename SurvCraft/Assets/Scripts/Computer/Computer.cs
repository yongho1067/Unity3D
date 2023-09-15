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
    [SerializeField] private Kit[] kits; // ŰƮ �����۵�
    [SerializeField] private Transform tf_ItemAppear; // ������ ���� ��ġ
    [SerializeField] private GameObject go_BaseUI; // ��ǻ�� ���� ȭ�� UI

    private bool isCraft = false; // ������ ���� ������ -> ������ ���� ������
    public bool isPowerOn = false; // ��ǻ�� ���� ��������

    private AudioSource audioSource;
    [SerializeField] private AudioClip sound_ButtonClick;
    [SerializeField] private AudioClip sound_Beep;
    [SerializeField] private AudioClip sound_PowerOn;

    // �ʿ��� ������Ʈ
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
            if(!CheckIngredient(_slotNum)) // ��� üũ
            {
                return;
            }

            isCraft = true;
            UseIngredient(_slotNum); // ��� ���

            StartCoroutine(CraftCoroutine(_slotNum)); // Kits ����
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
