using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TravelTerminal : MonoBehaviour
{
    public void interacted(PlayerColtroller player , int interact_state){
        if(interact_state == 1) return;
        player.ui.ToggleTravelTerminal();
    }
}
