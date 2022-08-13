using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsableItem : MonoBehaviour
{
    public static void HealthPack(DragDrop item_image , UI ui , int slot_id){
        print("Health + 25");
        ui.items_in_backpack[slot_id] = 0;
        Destroy(item_image.gameObject);
    }
    public static void HackerComputer(UI ui){
        ui.select_box.enabled = true;
        ui.select_box.interact_object_type = 0;
        ui.player_ctl.control_state = 1;
    }
}
