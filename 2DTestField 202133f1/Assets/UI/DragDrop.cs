using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour , IPointerDownHandler , IBeginDragHandler , IEndDragHandler , IDragHandler
{
    RectTransform rect_transform;
    CanvasGroup canvas_group;
    public Canvas ui_canvas;
    public UI ui;
    public int current_in_slot_id = 0 , item_id = 0;
    public bool is_weapon = false , is_armor = false , usable = false;
    private void Awake() {
        rect_transform = GetComponent<RectTransform>();
        canvas_group = GetComponent<CanvasGroup>();
    }
    public void OnPointerDown(PointerEventData eventData){

    }
    public void OnBeginDrag(PointerEventData eventData){
        if(current_in_slot_id == 24) ui.player.unequip_weapon();
        if(current_in_slot_id == 25){
            //unequip armor
        }
        if(current_in_slot_id > 25 && current_in_slot_id < 46){
            for(int i = 0; i < ui.current_interacting_npc.items_in_backpack.Length; i++){
                if(ui.current_interacting_npc.items_in_backpack[i] == item_id){
                    ui.current_interacting_npc.items_in_backpack[i] = 0;
                    break;
                }
            }
        }
        transform.SetParent(ui_canvas.transform);
        canvas_group.blocksRaycasts = false;
        canvas_group.alpha = 0.6f;
    }
    public void OnDrag(PointerEventData eventData){
        rect_transform.anchoredPosition += eventData.delta / ui_canvas.scaleFactor;
    }
    public void OnEndDrag(PointerEventData eventData){
        canvas_group.blocksRaycasts = true;
        canvas_group.alpha = 1f;
        Slot slot = transform.parent.GetComponent<Slot>();
        if(slot == null){
            Instantiate(ui.item_database.item_id_to_item(item_id) , ui.player.feet.position , Quaternion.identity);
            ui.items_in_backpack[current_in_slot_id] = 0;
            Destroy(gameObject);
        }
        else{
            if(ui.items_in_backpack[slot.slot_id] == 0){
                if(slot.slot_id == 24){
                    if(is_weapon){
                        switch_slot(slot);
                        if(ui.player.weapon.childCount == 0){
                            Instantiate(ui.item_database.item_id_to_instanced_item(item_id) , ui.player.weapon.transform.position , Quaternion.identity , ui.player.weapon);
                            ui.player.equip_weapon();
                        }
                    }
                    else{
                        go_back_to_old_slot();
                    }
                }
                else if(slot.slot_id == 25){
                    if(is_armor){
                        switch_slot(slot);
                        //equip armor
                    }
                    else{
                        go_back_to_old_slot();
                    }
                }
                else if(slot.slot_id == 20 || slot.slot_id == 21 || slot.slot_id == 22 || slot.slot_id == 23){
                    if(usable){
                        switch_slot(slot);
                    }
                    else{
                        go_back_to_old_slot();
                    }
                }
                else{
                    switch_slot(slot);
                }
            }
            else{
                go_back_to_old_slot();
            }
        }
    }
    void switch_slot(Slot desired_slot){
        ui.items_in_backpack[current_in_slot_id] = 0;
        ui.items_in_backpack[desired_slot.slot_id] = item_id;
        current_in_slot_id = desired_slot.slot_id;
    }
    void go_back_to_old_slot(){
        transform.SetParent(ui.slots[current_in_slot_id].transform);
        transform.position = ui.slots[current_in_slot_id].transform.position;
        if(current_in_slot_id == 24){
            Instantiate(ui.item_database.item_id_to_instanced_item(item_id) , ui.player.weapon.transform.position , Quaternion.identity , ui.player.weapon);
            ui.player.equip_weapon();
        }
        else if(current_in_slot_id == 25){
            //equip armor
        }
    }
    public void use(){
        print(current_in_slot_id + " " + item_id);
    }
}
