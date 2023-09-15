using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ComputerToolTip : MonoBehaviour
{
    [SerializeField] private GameObject go_BaseUI; // Tool Tip UI

    [SerializeField] private TextMeshProUGUI kitName;
    [SerializeField] private TextMeshProUGUI kitContent;
    [SerializeField] private TextMeshProUGUI kitNeedItem;

    public void ShowToolTip(string _kitName, string _kitContent, string[] _needItem, int[] _needItemNum)
    {
        go_BaseUI.SetActive(true);

        kitName.text = _kitName;
        kitContent.text = _kitContent;

        for(int i =0; i<_needItem.Length; i++)
        {
            kitNeedItem.text += _needItem[i];
            kitNeedItem.text += " x " + _needItemNum[i].ToString() + "\n";
        }
    }

    public void HideToolTip()
    {
        go_BaseUI.SetActive(false);

        kitName.text = "";
        kitContent.text = "";
        kitNeedItem.text = "";

    }
}
