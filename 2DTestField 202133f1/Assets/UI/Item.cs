using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public int item_id = 0;
    public void interacted(PlayerColtroller player){
        player.ui.add_item_to_backpack(item_id);
        if(!player.ui.backpack_is_full()) Destroy(gameObject);
    }
}
