using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    ItemRegister item_database;
    private void Awake() {
        backpack = GetComponentInChildren<Backpack>();
        item_database = GetComponent<ItemRegister>();
    }
    private void Start() {
        for(int i = 0 ; i < backpack.slots.Length; i++){
            Slot backback_slot = backpack.slots[i].GetComponent<Slot>();
            backback_slot.ui = this;
            backback_slot.slot_id = i;
        }
        backpack.gameObject.SetActive(false);
    }
    public Backpack backpack;
    public int[] items_in_backpack = new int[20];
    public bool toggle_backpack(){
        if(backpack.gameObject.activeSelf){
            backpack.gameObject.SetActive(false);
            return false;
        }
        else{
            backpack.gameObject.SetActive(true);
            return true;
        }
    }
    public void add_item_to_backpack(int item_id){
        for(int i = 0; i < items_in_backpack.Length; i++){
            if(items_in_backpack[i] == 0){
                items_in_backpack[i] = item_id;
                GameObject item_image = Instantiate(item_database.item_id_to_image(item_id) , backpack.slots[i].transform.position , Quaternion.identity ,  backpack.slots[i].transform);
                backpack.slots[i] = item_image;
                DragDrop item = item_image.GetComponent<DragDrop>();
                item.ui = GetComponent<Canvas>();
                item.current_in_slot_id = i;
                item.item_id = item_id;
                return;
            }
        }
    }
    public bool backpack_is_full(){
        for(int i = 0; i < items_in_backpack.Length;i++){
            if(items_in_backpack[i] != 0) return false;
        }
        return true;
    }
}
