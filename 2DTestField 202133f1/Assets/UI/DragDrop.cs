using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragDrop : MonoBehaviour , IBeginDragHandler , IEndDragHandler , IDragHandler
{
    RectTransform rect_transform;
    CanvasGroup canvas_group;
    Image image;
    [HideInInspector] public Canvas ui_canvas;
    [HideInInspector] public UI ui;
    [HideInInspector] public int current_in_slot_id = 0 , item_id = 0;
    public bool is_weapon = false , is_armor = false , usable = false;
    private void Awake() {
        rect_transform = GetComponent<RectTransform>();
        canvas_group = GetComponent<CanvasGroup>();
        image = GetComponent<Image>();
    }
    public void SetParameters(Canvas c , UI _ui , int _slot_id , int id , Sprite sprite , bool weapon , bool armor , bool use){
        ui_canvas = c;
        ui = _ui;
        current_in_slot_id = _slot_id;
        item_id = id;
        GetComponent<Image>().sprite = sprite;
        is_weapon = weapon;
        is_armor = armor;
        usable = use;
    }
    public void OnBeginDrag(PointerEventData eventData){
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
        if(slot == null){ //drop item
            if(current_in_slot_id == 24){
                ui.player.unequip_weapon();
            }
            if(current_in_slot_id == 25){
                ui.player.unequip_armor();
            }
            ItemData target_item = ui.Item_database.FindObject(item_id);
            Item item_dropped = Instantiate(ui.Item_database.item_template , ui.player.feet.position , Quaternion.identity , ui.object_holder);
            item_dropped.SetParameters(item_id , ui.Item_database.FindObject(item_id).item_sprite);
            ui.items_in_backpack[current_in_slot_id] = 0;
            Destroy(gameObject);
        }
        else if(slot.slot_id > 19 && slot.slot_id < 24){
            if(usable){
                move_item_to_slot(slot);
            }
            else{
                return_to_original_slot();
            }
        }
        else if(slot.slot_id == 24){
            if(is_weapon){
                ui.player.unequip_weapon();
                move_item_to_slot(slot);
                Instantiate(ui.Item_database.FindObject(item_id).item_instanced , ui.player.weapon.position , Quaternion.identity , ui.player.weapon);
                StartCoroutine(wait_to_change_weapon());
            }
            else{
                return_to_original_slot();
            }
        }
        else if(slot.slot_id == 25){
            if(is_armor){
                ui.player.unequip_armor();
                move_item_to_slot(slot);
                Instantiate(ui.Item_database.FindObject(item_id).item_instanced , ui.player.armor_holder.position , Quaternion.identity , ui.player.armor_holder);
                StartCoroutine(wait_to_change_armor());
            }
            else{
                return_to_original_slot();
            }
        }
        else{
            if(current_in_slot_id == 24){
                ui.player.unequip_weapon();
            }
            else if(current_in_slot_id == 25){
                ui.player.unequip_armor();
            }
            move_item_to_slot(slot);
        }
    }
    IEnumerator wait_to_change_weapon(){
        yield return new WaitForSeconds(0.001f);
        ui.player.equip_weapon();
    }
    IEnumerator wait_to_change_armor(){
        yield return new WaitForSeconds(0.001f);
        ui.player.equip_armor();
    }
    void move_item_to_slot(Slot target_slot){
        if(ui.items_in_backpack[target_slot.slot_id] == 0){
            ui.items_in_backpack[target_slot.slot_id] = item_id;
            ui.items_in_backpack[current_in_slot_id] = 0;
        }
        else{
            int temp = ui.items_in_backpack[target_slot.slot_id];
            ui.items_in_backpack[target_slot.slot_id] = item_id;
            ui.items_in_backpack[current_in_slot_id] = temp;
            DragDrop image_in_target_slot = target_slot.transform.GetChild(0).GetComponent<DragDrop>();
            image_in_target_slot.transform.position = ui.slots[current_in_slot_id].transform.position;
            image_in_target_slot.transform.SetParent(ui.slots[current_in_slot_id].transform);
            image_in_target_slot.current_in_slot_id = current_in_slot_id;
        }
        current_in_slot_id = target_slot.slot_id;
    }
    void return_to_original_slot(){
        transform.position = ui.slots[current_in_slot_id].transform.position;
        transform.SetParent(ui.slots[current_in_slot_id].transform);
    }
    public void use(){
        if(!usable) return;
        if(item_id == 6) UsableItem.HealthPack(this , ui , current_in_slot_id);
        if(item_id == 7) UsableItem.HackerComputer(ui);
    }
}
