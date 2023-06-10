using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InputNumber : MonoBehaviour
{
    private bool activated = false;

    [SerializeField] private TextMeshProUGUI text_Preview;
    [SerializeField] private TextMeshProUGUI text_Input;

    [SerializeField] private GameObject uiBase;
    [SerializeField] private ActionController actionController;
    [SerializeField] private TMP_InputField inputField;

    private void Update()
    {
        if (activated)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                Ok();
            }
            else if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Tab))
            {
                Cancel();
            }
        }  
    }
    public void Call()
    {
        uiBase.SetActive(true);
        activated = true;
        inputField.text = "";
        text_Preview.text = DragSlot.instance.itemSlot.itemCount.ToString();
    }

    public void Cancel()
    {
        activated = false;
        uiBase.SetActive(false);
        DragSlot.instance.SetColor(0f);
        DragSlot.instance.itemSlot = null;
    }

    public void Ok()
    {
        int num;

        if (inputField.text != null)
        {
            if (int.TryParse(inputField.text, out num))
            {
                if (num > DragSlot.instance.itemSlot.itemCount)
                {
                    num = DragSlot.instance.itemSlot.itemCount;
                }
            }
            else
            {
                // 형식 변환 실패
                // 기본 값 또는 오류 처리를 수행
                num = int.Parse(text_Preview.text);
            }
        }
        else
        {
            num = int.Parse(text_Preview.text);
        }

        DragSlot.instance.SetColor(0f);
        StartCoroutine(DropItemCoroutine(num));
        
    }

    IEnumerator DropItemCoroutine(int num)
    {
        for (int i = 0; i < num; i++)
        {
            Instantiate(DragSlot.instance.itemSlot.item.itemPrefab, actionController.transform.position + (actionController.transform.forward * 2), Quaternion.identity);
            DragSlot.instance.itemSlot.SetSlotCount(-1);
            yield return new WaitForSeconds(0.05f);
        }

        activated = false;
        DragSlot.instance.itemSlot = null;
        uiBase.SetActive(false);
    }
}
