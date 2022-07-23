using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    [HideInInspector] public int key_id;
    public void interacted(PlayerColtroller player , int interact_state){
        if(interact_state == 1) return;
        player.ui.keys_in_backpack.Add(key_id);
        Destroy(gameObject);
    }
}
