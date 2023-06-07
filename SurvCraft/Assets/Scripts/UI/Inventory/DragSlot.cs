using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragSlot : MonoBehaviour
{
    static public DragSlot instance;

    public ItemSlot itemSlot;

    // 아이템 이미지
    [SerializeField] private Image itemImage;

    private void Start()
    {
        instance = this;
    }

    public void DragSetImage(Image _itemImage)
    {
        itemImage.sprite = _itemImage.sprite;
        SetColor(1f);
    }

    public void SetColor(float alpha)
    {
        Color color = itemImage.color;
        color.a = alpha;

        itemImage.color = color;
    }
}
