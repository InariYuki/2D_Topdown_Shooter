using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stash : MonoBehaviour
{
    public int[] items_in_backpack = {0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0};
    [SerializeField] bool locked = false;
    [HideInInspector] public int key = 0;
    private void Start() {
        key = Random.Range(0 , 65536);
    }
    public void interacted(PlayerColtroller player , int interact_state){
        if(interact_state == 1) return;
        if(locked && !player.ui.keys_in_backpack.Contains(key)){
            print("locked!");
            return;
        }
        if(locked){
            locked = false;
            player.ui.keys_in_backpack.Remove(key);
        }
        player.ui.toggle_stash(this);
        player.ui.toggle_backpack();
        player.ui.camera_controller.is_dynamic = false;
    }
    public void remove_item(int slot){
        items_in_backpack[slot] = 0;
    }
}
