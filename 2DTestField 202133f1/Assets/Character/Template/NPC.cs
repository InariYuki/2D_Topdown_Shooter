using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NPC : MonoBehaviour
{
    Character character;
    ArtificialIntelligence AI;
    PlayerColtroller player;
    private void Awake() {
        character = GetComponent<Character>();
        AI = GetComponent<ArtificialIntelligence>();
        generate_interaction_menu();
        toggle_action_menu();
    }
    public string[] interact_methods = {"Chat" , "Intimidate" , "Steal" , "Assassinate"};
    public int[] action_success_rate = {0 , 0 , 50 , 50};
    public int[] items_in_backpack = {1 , 1 , 1 , 1 , 1 , 1 , 1 , 1 , 1 , 1 , 1 , 1 , 1 , 1 , 1 , 1 , 1 , 1 , 1 , 1};
    string dialogue = "Hi I am a kitsune";
    [SerializeField] Transform action_menu;
    [SerializeField] InteractiomMenuButton button;
    void generate_interaction_menu(){
        RectTransform rect = action_menu.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(240 , 5 + 35 * interact_methods.Length);
        for(int i = 0; i < interact_methods.Length; i++){
            InteractiomMenuButton button_instanced = Instantiate(button , action_menu.position , Quaternion.identity , action_menu);
            button_instanced.init(interact_methods[i] , this);
        }
    }
    public void interacted(PlayerColtroller _player){
        player = _player;
        toggle_action_menu();
        player.attack_locked = action_menu_opened;
    }
    public bool action_menu_opened;
    public void toggle_action_menu(){
        action_menu.gameObject.SetActive(!action_menu.gameObject.activeSelf);
        action_menu_opened = action_menu.gameObject.activeSelf;
    }
    public void action(string _action_string){
        if(action_menu_opened){
            toggle_action_menu();
            player.attack_locked = action_menu_opened;
        }
        if(_action_string == interact_methods[0]){
            talk();
        }
        else if(_action_string == interact_methods[1]){
            intimidate(player);
        }
        else if(_action_string == interact_methods[2]){
            steal(player);
        }
        else if(_action_string == interact_methods[3]){
            assassinate(player);
        }
    }
    [SerializeField] TextMeshProUGUI dialogue_box;
    void talk(){
        StopCoroutine(dialogue_disappear());
        dialogue_box.text = dialogue;
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
