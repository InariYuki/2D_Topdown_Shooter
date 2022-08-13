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
}
