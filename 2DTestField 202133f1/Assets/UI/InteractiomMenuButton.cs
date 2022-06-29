using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InteractiomMenuButton : MonoBehaviour
{
    public UI ui;
    public PlayerColtroller player;
    public NPC npc;
    public TextMeshProUGUI button_text;
    private void Awake() {
        button_text = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }
    public void init(string action_string , NPC _npc){
        button_text.text = action_string;
        npc = _npc;
    }
    public void on_button_clicked(){
        npc.action(button_text.text);
    }
}
