using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stash : MonoBehaviour
{
    public int[] items_in_backpack = {1 , 2 , 1 , 2 , 2 , 1 , 2 , 1 , 1 , 1 , 2 , 2 , 2 , 1 , 2 , 1 , 2 , 1 , 1 , 2};
    public void interacted(PlayerColtroller player , int interact_state){
        if(interact_state == 1) return;
        player.ui.toggle_stash(this);
        player.ui.toggle_backpack();
        player.ui.camera_controller.is_dynamic = false;
    }
}
