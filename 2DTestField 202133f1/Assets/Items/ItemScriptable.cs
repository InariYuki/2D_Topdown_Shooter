using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(menuName = "Scriptable/Item" , fileName = "Item")]
public class ItemScriptable : ScriptableObject
{
    public DragDrop ui_image_template;
    public Item item_template;
    public ItemData[] items;
    public ItemData FindObject(int item_id){
        IEnumerable<ItemData> item = items.Where(item => item.item_id == item_id);
        List<ItemData> found_items = item.ToList();
        if(found_items.Count == 0){
            Debug.Log("No such item");
            return null;
        }
        return found_items[0];
    }
}
[System.Serializable] public class ItemData{
    public int item_id;
    public int price;
    public Sprite item_sprite , ui_image;
    public GameObject item_instanced;
    public bool is_weapon , is_armor , usable;
}