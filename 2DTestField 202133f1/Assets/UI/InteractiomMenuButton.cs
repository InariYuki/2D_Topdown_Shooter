using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiomMenuButton : MonoBehaviour
{
    public UI ui;
    public PlayerColtroller player;
    public NPC npc;
    public string action_string;
    public void on_button_clicked(){
        ui.toggle_interaction_menu(null);
        player.camera_controller.is_dynamic = !ui.interaction_menu_opened;
        npc.interacted(player , action_string);
    }
}
