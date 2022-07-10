using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableBox : MonoBehaviour
{
    Computer computer;
    Item item;
    NPC npc;
    Stash stash;
    SpriteRenderer sprite;
    private void Awake() {
        computer = GetComponent<Computer>();
        item = GetComponent<Item>();
        npc = GetComponent<NPC>();
        stash = GetComponent<Stash>();
        sprite = GetComponent<SpriteRenderer>();
    }
    public void interacted(PlayerColtroller player , int interact_state){
        if(sprite != null) sprite.color = Color.white;
        else if(npc != null) npc.undisplay();
        if(computer != null) computer.interacted(player , interact_state);
        else if(item != null) item.interacted(player , interact_state);
        else if(npc != null) npc.interacted(player , interact_state);
        else if(stash != null) stash.interacted(player , interact_state);
    }
    public void display_interaction_hint(){
        if(sprite != null) sprite.color = Color.green;
        else if(npc != null) npc.display_string("interact");
    }
}
