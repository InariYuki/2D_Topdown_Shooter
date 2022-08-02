using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinExchanger : MonoBehaviour
{
    public void Interacted(PlayerColtroller player , int interact_state){
        if(interact_state == 1) return;
        player.ui.ToggleCoinExchanger();
    }
}
