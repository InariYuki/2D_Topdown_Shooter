using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    ItemRegister item_database;
    private void Awake() {
        item_database = GetComponent<ItemRegister>();
        for(int i = 0; i < backpack.childCount; i++){
            slots[i] = backpack.GetChild(i).gameObject;
        }
        for(int i = 0 ; i < backpack.childCount; i++){
            Slot slot = slots[i].GetComponent<Slot>();
            slot.slot_id = i;
        }
    }
    private void Start() {
        backpack.gameObject.SetActive(false);
    }
    [SerializeField] Transform backpack;
    public int[] items_in_backpack = new int[20];
    public GameObject[] slots = new GameObject[20];
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
                GameObject item_image = Instantiate(item_database.item_id_to_image(item_id) , slots[i].transform.position , Quaternion.identity ,  slots[i].transform);
                DragDrop item = item_image.GetComponent<DragDrop>();
                item.ui_canvas = GetComponent<Canvas>();
                item.current_in_slot_id = i;
                item.item_id = item_id;
                item.ui = this;
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
