using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/Item" , fileName = "Item")]
public class ItemScriptable : ScriptableObject
{
    public DragDrop ui_image_template;
    public Item item_template;
    public ItemData[] items;
}
[System.Serializable] public class ItemData{
    public int item_id;
    public int price;
    public Sprite item_sprite , ui_image;
    public GameObject item_instanced;
    public bool is_weapon , is_armor , usable;
}