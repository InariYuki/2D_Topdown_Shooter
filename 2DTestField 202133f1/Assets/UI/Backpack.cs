using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Backpack : MonoBehaviour
{
    ItemRegister item_database;
    Canvas ui;
    [SerializeField] GameObject[] slots = new GameObject[20];
    public void init(ItemRegister _item_database , Canvas _ui){
        item_database = _item_database;
        ui = _ui;
    }
    public void display_items(int[] items){
        if(items[0] == 0) return;
        for(int i = 0; i < items.Length ; i++){
            if(items[i] == 0 || slots[i].transform.childCount != 0) continue;
            DragDrop item = Instantiate(item_database.item_id_to_image(items[i]) , slots[i].transform.position , Quaternion.identity ,  slots[i].transform).GetComponent<DragDrop>();
            item.ui = ui;
            item.parent = transform;
        }
    }
}
