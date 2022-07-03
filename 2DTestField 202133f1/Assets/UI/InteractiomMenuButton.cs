using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InteractiomMenuButton : MonoBehaviour
{
    public UI ui;
    public PlayerColtroller player;
    TextMeshProUGUI button_text;
    string action_text;
    NPC npc;
    Computer computer;
    private void Awake() {
        button_text = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }
    public void init(string action_string , NPC _npc , float success_rate){
        action_text = action_string;
        button_text.text = action_string + " " + ((success_rate == 0) ? "" : success_rate + "%");
        npc = _npc;
    }
    public void init(string action_string , Computer _computer){
        action_text = action_string;
        button_text.text = action_string;
        computer = _computer;
    }
    public void on_button_clicked(){
        if(npc != null) npc.action(action_text);
        if(computer != null) computer.action(action_text);
    }
}
