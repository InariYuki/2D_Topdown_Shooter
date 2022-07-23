using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stash : MonoBehaviour
{
    public int[] items_in_backpack = {0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0};
    [SerializeField] bool locked = false;
    bool player_has_key = false;
    public void interacted(PlayerColtroller player , int interact_state){
        if(interact_state == 1) return;
        if(locked && !player_has_key){
            return;
        }
        player.ui.toggle_stash(this);
        player.ui.toggle_backpack();
        player.ui.camera_controller.is_dynamic = false;
    }
    public void remove_item(int slot){
        items_in_backpack[slot] = 0;
    }
    void check_player_key(PlayerColtroller player){

    }
}
