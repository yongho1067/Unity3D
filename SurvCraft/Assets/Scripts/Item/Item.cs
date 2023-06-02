using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Item", menuName = "New Item/item")]
public class Item : ScriptableObject
{
    public string itemName;
    public Sprite itemImage;
    public GameObject itemPrefab;

    // 무기 유형
    public string weaponType;

    // 아이템 유형
    public ItemType itemType;

    public enum ItemType
    {
        Equipment,
        Used,
        Ingredient,
        ETC
    }
}
