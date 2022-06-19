using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
    ItemRegister item_database;
    private void Awake() {
        backpack_script = backpack.GetComponent<Backpack>();
        item_database = GetComponent<ItemRegister>();
    }
    private void Start() {
        backpack_script.init(item_database , GetComponent<Canvas>());
    }
    [SerializeField] GameObject backpack;
    Backpack backpack_script;
    int[] items_in_backpack = new int[20];
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
                break;
            }
        }
        backpack_script.display_items(items_in_backpack);
    }
}
