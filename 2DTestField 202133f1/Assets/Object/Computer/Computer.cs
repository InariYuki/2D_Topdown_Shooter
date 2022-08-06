using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Computer : MonoBehaviour
{
    private void Awake() {
        BootComputer();
        generate_interaction_menu();
        toggle_action_menu();
    }
    public List<string> interact_methods = new List<string>{"Read E-mail"};
    PlayerColtroller player;
    public void interacted(PlayerColtroller _player , int interact_state){
        player = _player;
        if(interact_state == 0){
            if(action_menu_opened == false) toggle_action_menu();
        }
        else{
            if(action_menu_opened) toggle_action_menu();
        }
        player.attack_locked = action_menu_opened;
    }
    [SerializeField] Transform action_menu;
    [SerializeField] InteractiomMenuButton button;
    void generate_interaction_menu(){
        RectTransform rect = action_menu.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(240 , 5 + 35 * interact_methods.Count);
        for(int i = 0; i < interact_methods.Count; i++){
            InteractiomMenuButton button_instanced = Instantiate(button , action_menu.position , Quaternion.identity , action_menu);
            button_instanced.init(interact_methods[i] , this);
        }
    }
    void BootComputer(){
        if(traps.Count != 0){
            interact_methods.Add("Control traps");
        }
        if(doors.Count + gates.Count != 0){
            interact_methods.Add("Control doors");
        }
    }
    public void action(string _action_string){
        if(action_menu_opened){
            toggle_action_menu();
            player.attack_locked = action_menu_opened;
        }
        if(_action_string == "Read E-mail") read_email();
        else if(_action_string == "Control doors") door_control();
        else if(_action_string == "Control traps") trap_control();
    }
    void read_email(){
        player.player_talk("Thats rude!");
    }
    [SerializeField] List<GameObject> traps = new List<GameObject>();
    void trap_control(){
        for(int i = 0; i < traps.Count; i++){
            LaserTrap laser_trap = traps[i].GetComponent<LaserTrap>();
            //Add trap type here
            if(laser_trap != null){
                laser_trap.toggle_laser_trap();
            }
            //if trap is not null add trap control here
        }
    }
    [SerializeField] List<Door> doors = new List<Door>();
    [SerializeField] List<Gate> gates = new List<Gate>();
    void door_control(){
        for(int i = 0; i < doors.Count; i++){
            doors[i].door_control();
        }
        for(int i = 0; i < gates.Count; i++){
            gates[i].GateControl();
        }
    }
    bool action_menu_opened;
    void toggle_action_menu(){
        action_menu.gameObject.SetActive(!action_menu.gameObject.activeSelf);
        action_menu_opened = action_menu.gameObject.activeSelf;
    }
}
