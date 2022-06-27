using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NPC : MonoBehaviour
{
    Character character;
    ArtificialIntelligence AI;
    private void Awake() {
        character = GetComponent<Character>();
        AI = GetComponent<ArtificialIntelligence>();
    }
    public string[] interact_methods = {"Chat" , "Intimidate" , "Steal" , "Assassinate"};
    public int[] action_success_rate = {0 , 0 , 50 , 50};
    public int[] items_in_backpack = {1 , 1 , 1 , 1 , 1 , 1 , 1 , 1 , 1 , 1 , 1 , 1 , 1 , 1 , 1 , 1 , 1 , 1 , 1 , 1};
    string dialogue = "Hi I am a kitsune";
    public void interacted(PlayerColtroller player , string action_string){
        switch(action_string){
            case "Chat":
                talk(player);
                break;
            case "Intimidate":
                intimidate(player);
                break;
            case "Steal":
                steal(player);
                break;
            case "Assassinate":
                assassinate(player);
                break;
        }
    }
    [SerializeField] Transform dialogue_box;
    void talk(PlayerColtroller player){
        StopCoroutine(dialogue_disappear());
        dialogue_box.GetChild(0).GetComponent<TextMeshProUGUI>().text = dialogue;
        dialogue_box.gameObject.SetActive(true);
        StartCoroutine(dialogue_disappear());
    }
    IEnumerator dialogue_disappear(){
        yield return new WaitForSeconds(2f);
        dialogue_box.gameObject.SetActive(false);
    }
    void intimidate(PlayerColtroller player){
        AI.hit(0 , player.gameObject);
    }
    void steal(PlayerColtroller player){
        if (Random.Range(0 , 100) > action_success_rate[2]){
            AI.hit(0 , player.gameObject);
            return;
        }
        player.ui.toggle_NPC_backpack(this);
        player.ui.toggle_backpack();
        player.camera_controller.is_dynamic = false;
    }
    void assassinate(PlayerColtroller player){
        if (Random.Range(0, 100) > action_success_rate[3]){
            AI.hit(0, player.gameObject);
            return;
        }
        character.die();
    }
}
