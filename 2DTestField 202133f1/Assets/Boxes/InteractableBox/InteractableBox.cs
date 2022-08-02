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
    PlayerComputer player_computer;
    Key key;
    TravelTerminal travel_terminal;
    CoinExchanger coin_exchanger;
    private void Awake() {
        computer = GetComponent<Computer>();
        item = GetComponent<Item>();
        npc = GetComponent<NPC>();
        stash = GetComponent<Stash>();
        sprite = GetComponent<SpriteRenderer>();
        player_computer = GetComponent<PlayerComputer>();
        key = GetComponent<Key>();
        travel_terminal = GetComponent<TravelTerminal>();
        coin_exchanger = GetComponent<CoinExchanger>();
    }
    public void interacted(PlayerColtroller player , int interact_state){
        if(sprite != null) sprite.color = Color.white;
        else if(npc != null) npc.undisplay();
        if(computer != null) computer.interacted(player , interact_state);
        else if(item != null) item.interacted(player , interact_state);
        else if(npc != null) npc.interacted(player , interact_state);
        else if(stash != null) stash.interacted(player , interact_state);
        else if(player_computer != null) player_computer.interacted(player , interact_state);
        else if(key != null) key.interacted(player , interact_state);
        else if(travel_terminal != null) travel_terminal.interacted(player , interact_state);
        else if(coin_exchanger != null) coin_exchanger.Interacted(player , interact_state);
    }
    public void display_interaction_hint(){
        if(sprite != null) sprite.color = Color.green;
        else if(npc != null) npc.display_string("interact");
    }
}
