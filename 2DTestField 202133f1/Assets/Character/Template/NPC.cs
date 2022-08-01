using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NPC : MonoBehaviour
{
    Character character;
    ArtificialIntelligence AI;
    [HideInInspector] public UI ui;
    private void Awake() {
        character = GetComponent<Character>();
        AI = GetComponent<ArtificialIntelligence>();
        ui = GameObject.Find("UI").GetComponent<UI>();
        generate_interaction_menu();
        toggle_action_menu();
    }
    private void Start() {
        GenerateRandomItem();
    }
    public List<string> interact_methods = new List<string>{"Chat" , "Intimidate" , "Steal" , "Assassinate"};
    public int[] action_success_rate = {0 , 0 , 50 , 50};
    public int[] items_in_backpack = {0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0};
    [SerializeField] Transform action_menu;
    [SerializeField] InteractiomMenuButton button;
    [SerializeField] int item_spawn_rate = 20;
    void GenerateRandomItem(){
        for(int i = 0; i < 20; i++){
            items_in_backpack[i] = 0;
            if(Random.Range(0 , 100) < item_spawn_rate){
                items_in_backpack[i] = Random.Range(1 , ui.Item_database.items.Length - 1);
            }
        }
    }
    void generate_interaction_menu(){
        RectTransform rect = action_menu.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(240 , 5 + 35 * interact_methods.Count);
        for(int i = 0; i < interact_methods.Count; i++){
            InteractiomMenuButton button_instanced = Instantiate(button , action_menu.position , Quaternion.identity , action_menu);
            button_instanced.init(interact_methods[i] , this , action_success_rate[i]);
        }
    }
    public void interacted(PlayerColtroller _player , int interact_state){
        if(interact_state == 0){
            if(action_menu_opened == false) toggle_action_menu();
        }
        else{
            if(action_menu_opened) toggle_action_menu();
        }
        ui.player_ctl.attack_locked = action_menu_opened;
    }
    bool action_menu_opened;
    void toggle_action_menu(){
        action_menu.gameObject.SetActive(!action_menu.gameObject.activeSelf);
        action_menu_opened = action_menu.gameObject.activeSelf;
    }
    public void action(string _action_string){
        if(action_menu_opened){
            toggle_action_menu();
            ui.player_ctl.attack_locked = action_menu_opened;
        }
        if(_action_string == "Chat"){
            talk();
        }
        else if(_action_string == "Intimidate"){
            intimidate(ui.player_ctl);
        }
        else if(_action_string == "Steal"){
            steal(ui.player_ctl);
        }
        else if(_action_string == "Assassinate"){
            assassinate(ui.player_ctl);
        }
        else if(_action_string == "Shop"){
            shop(ui.player_ctl);
        }
    }
    [SerializeField] TextMeshProUGUI dialogue_box;
    void talk(){
        say("What");
    }
    void intimidate(PlayerColtroller player){
        AI.hit(0 , player.gameObject);
        say("You are dead!");
    }
    void steal(PlayerColtroller player){
        if (Random.Range(0 , 100) > action_success_rate[2]){
            AI.hit(0 , player.gameObject);
            say("Nice try you bastard!");
            return;
        }
        player.ui.toggle_NPC_backpack(this);
        player.ui.toggle_backpack();
        player.ui.camera_controller.is_dynamic = false;
    }
    void assassinate(PlayerColtroller player){
        if (Random.Range(0, 100) > action_success_rate[3]){
            AI.hit(0 , player.gameObject);
            say("You merderer!");
            return;
        }
        character.die();
    }
    [SerializeField] Stash NPC_stash;
    void shop(PlayerColtroller player){
        player.ui.toggle_shop(NPC_stash);
    }
    [SerializeField] Key key;
    public void drop_key(){
        if(NPC_stash != null){
            Key key_instanced = Instantiate(key , character.feet.position , Quaternion.identity , ui.object_holder);
            key_instanced.key_id = NPC_stash.key;
        }
    }
    bool saying = false;
    Coroutine say_coroutine;
    void say(string something){
        if(say_coroutine != null){
            StopCoroutine(say_coroutine);
        }
        dialogue_box.text = something;
        dialogue_box.gameObject.SetActive(true);
        saying = true;
        say_coroutine = StartCoroutine(dialogue_disappear());
    }
    IEnumerator dialogue_disappear(){
        yield return new WaitForSeconds(2f);
        dialogue_box.gameObject.SetActive(false);
        saying = false;
    }
    public void display_string(string text){
        if(saying) return;
        try{
            dialogue_box.text = text;
            dialogue_box.gameObject.SetActive(true);
        }
        catch{}
    }
    public void undisplay(){
        if(saying) return;
        try{
            dialogue_box.gameObject.SetActive(false);
        }
        catch{}
    }
    public void DropItems(){
        for(int i = 0; i < 20; i++){
            if(items_in_backpack[i] != 0){
                Item dropped_item = Instantiate(ui.Item_database.item_template , transform.position , Quaternion.identity , ui.object_holder);
                dropped_item.SetParameters(items_in_backpack[i] , ui.Item_database.items[items_in_backpack[i]].item_sprite);
                dropped_item.BurstMoveAway();
                items_in_backpack[i] = 0;
            }
        }
    }
}
