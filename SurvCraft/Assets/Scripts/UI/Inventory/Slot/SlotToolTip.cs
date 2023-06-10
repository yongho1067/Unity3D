using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SlotToolTip : MonoBehaviour
{
    [SerializeField] GameObject Base;
    [SerializeField] private TextMeshProUGUI itemName_text;
    [SerializeField] private TextMeshProUGUI itemDesc_text;
    [SerializeField] private TextMeshProUGUI itemCount_text;
    [SerializeField] private TextMeshProUGUI itemRightClick_text;

    public void ShowToolTip(Item item, int count)
    {
        Base.SetActive(true);

        itemName_text.text = item.itemName;
        itemDesc_text.text = item.itemDesc;
        itemCount_text.text = count + "개";

        if (item.itemType == Item.ItemType.Equipment)
        {
            itemRightClick_text.text = "우클릭 - 장착";
        }
        else if (item.itemType == Item.ItemType.Used)
        {
            itemRightClick_text.text = "우클릭 - 사용";
        }
        else
        {
            itemRightClick_text.text = "";
        }
    }

    public void HideToolTip()
    {
        Base.SetActive(false);
    }

}
