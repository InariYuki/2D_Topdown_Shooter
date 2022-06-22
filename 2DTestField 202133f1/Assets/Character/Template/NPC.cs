using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    Character character;
    ArtificialIntelligence AI;
    private void Awake() {
        character = GetComponent<Character>();
        AI = GetComponent<ArtificialIntelligence>();
    }
    public string[] interact_methods = {"Chat" , "Intimidate" , "Steal" , "Assassinate"};
    public int[] items_in_backpack = {1 , 1 , 1 , 1 , 1 , 1 , 1 , 1 , 1 , 1 , 1 , 1 , 1 , 1 , 1 , 1 , 1 , 1 , 1 , 1};
    string dialogue = "Hi I am a kitsune";
    public void interacted(PlayerColtroller player , string action_string){
        switch(action_string){
            case "Chat":
                talk();
                break;
            case "Intimidate":
                intimidate();
                break;
            case "Steal":
                steal(player);
                break;
            case "Assassinate":
                assassinate();
                break;
        }
    }
    void talk(){
        print(dialogue + gameObject);
    }
    void intimidate(){
        print("I am so angry" + gameObject);
    }
    void steal(PlayerColtroller player){
        player.ui.toggle_NPC_backpack(this);
        player.ui.toggle_backpack();
        player.camera_controller.is_dynamic = false;
    }
    void assassinate(){
        print("X_X" + gameObject);
    }
}
