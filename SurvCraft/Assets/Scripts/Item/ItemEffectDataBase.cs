using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEffectDataBase : MonoBehaviour
{
    private const string HP = "HP", SP = "SP", DP = "DP", HUNGRY = "HUNGRY", THIRSTY = "THIRSTY", SATISFY = "SATISFY";

    [SerializeField] private ItemEffect[] itemEffects;

    [SerializeField] private WeaponManager weaponManager;
    [SerializeField] private StatusController statusController;
    [SerializeField] private SlotToolTip slotToolTip;
    [SerializeField] private QuickSlotController quickSlotController;

    #region QuickSlot 징검다리
    public void isActivatedQuickSlot(int num)
    {
        quickSlotController.isActivatedQuickSlot(num);
    }
    #endregion

    #region SlootToolTip 징검다리
    public void ShowToolTip(Item item, int count)
    {
        slotToolTip.ShowToolTip(item, count);
    }
    
    public void HideToolTip()
    {
        slotToolTip.HideToolTip();
    }
    #endregion

    public void UseItem(Item item)
    {
        if (item.itemType == Item.ItemType.Equipment)
        {
            StartCoroutine(weaponManager.ChangeWeaponCoroutine(item.weaponType, item.itemName));
        }
        else if (item.itemType == Item.ItemType.Used)
        {
            for (int i = 0; i < itemEffects.Length; i++)
            {
                if (itemEffects[i].itemName == item.itemName)
                {
                    for(int j = 0; j < itemEffects[i].effectPart.Length; j++)
                    {
                        switch (itemEffects[i].effectPart[j])
                        {
                            case HP:
                                statusController.IncreaseHP(itemEffects[i].effectNum[j]);
                                break;
                            case SP:
                                statusController.IncreaseSP(itemEffects[i].effectNum[j]);
                                break;
                            case DP:
                                statusController.IncreaseDP(itemEffects[i].effectNum[j]);
                                break;
                            case HUNGRY:
                                statusController.IncreaseHungry(itemEffects[i].effectNum[j]);
                                break;
                            case THIRSTY:
                                statusController.IncreaseThirsty(itemEffects[i].effectNum[j]);
                                break;
                            case SATISFY:
                                break;
                            default:
                                Debug.Log("잘못된 Status effect 적용");
                                break;
                        }
                        Debug.Log(item.itemName + "을 사용했습니다.");
                    }
                    return;
                }
            }
            Debug.Log("일치하는 아이템이 없습니다.");
        }
        
    }
}

[System.Serializable]
public class ItemEffect
{
    [Tooltip("HP SP DP HUNGRY THIRSTY SATISFY 만 가능합니다")]
    public string itemName;
    public string[] effectPart;
    public int[] effectNum;
}
