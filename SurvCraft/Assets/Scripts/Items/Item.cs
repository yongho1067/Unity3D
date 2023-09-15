using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Item", menuName = "New Item/item")]
public class Item : ScriptableObject
{
    public string itemName;
    public Sprite itemImage;
    public GameObject itemPrefab;
    [TextArea] public string itemDesc; // 아이템의 설명

    public GameObject kitPrefab; // 키트 프리팹 
    public GameObject kitPreviewPrefab; // 키트 프리뷰 프리팹

    // 무기 유형
    public string weaponType;

    // 아이템 유형
    public ItemType itemType;

    public enum ItemType
    {
        Equipment,
        Used,
        Ingredient,
        Kit,
        ETC
    }
}
