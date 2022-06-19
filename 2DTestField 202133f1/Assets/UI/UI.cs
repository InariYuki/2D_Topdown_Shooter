using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    ItemRegister item_database;
    private void Awake() {
        backpack_script = backpack.GetComponent<Backpack>();
        item_database = GetComponent<ItemRegister>();
    }
    private void Start() {
        for(int i = 0 ; i < backpack_script.slots.Length; i++) backpack_script.slots[i].GetComponent<Slot>().ui = this;
    }
    [SerializeField] GameObject backpack;
    Backpack backpack_script;
    public int[] items_in_backpack = new int[20];
    public bool toggle_backpack(){
        if(backpack.activeSelf){
            backpack.SetActive(false);
            return false;
        }
        else{
            backpack.SetActive(true);
            return true;
        }
    }
    public void add_item_to_backpack(int item_id){
        for(int i = 0; i < items_in_backpack.Length; i++){
            if(items_in_backpack[i] == 0){
                items_in_backpack[i] = item_id;
                GameObject item_image = Instantiate(item_database.item_id_to_image(item_id) , backpack_script.slots[i].transform.position , Quaternion.identity ,  backpack_script.slots[i].transform);
                backpack_script.slots[i] = item_image;
                item_image.GetComponent<DragDrop>().ui = GetComponent<Canvas>();
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
