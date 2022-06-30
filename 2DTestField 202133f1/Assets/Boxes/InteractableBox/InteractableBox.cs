using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableBox : MonoBehaviour
{
    Computer computer;
    Item item;
    NPC npc;
    private void Awake() {
        computer = GetComponent<Computer>();
        item = GetComponent<Item>();
        npc = GetComponent<NPC>();
    }
    public void interacted(PlayerColtroller player , int interact_state){
        if(computer != null) computer.interacted(interact_state);
        else if(item != null) item.interacted(player , interact_state);
        else if(npc != null) npc.interacted(player , interact_state);
    }
}
