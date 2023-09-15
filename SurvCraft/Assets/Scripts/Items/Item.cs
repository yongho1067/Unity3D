using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Item", menuName = "New Item/item")]
public class Item : ScriptableObject
{
    public string itemName;
    public Sprite itemImage;
    public GameObject itemPrefab;
    [TextArea] public string itemDesc; // �������� ����

    public GameObject kitPrefab; // ŰƮ ������ 
    public GameObject kitPreviewPrefab; // ŰƮ ������ ������

    // ���� ����
    public string weaponType;

    // ������ ����
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
