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
    private void Awake() {
        rect_transform = GetComponent<RectTransform>();
        canvas_group = GetComponent<CanvasGroup>();
    }
    public void OnPointerDown(PointerEventData eventData){

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
        if(slot == null){
            Instantiate(ui.item_database.item_id_to_item(item_id) , ui.player.feet.position , Quaternion.identity);
            ui.items_in_backpack[current_in_slot_id] = 0;
            Destroy(gameObject);
        }
        else{
            if(ui.items_in_backpack[slot.slot_id] == 0){
                ui.items_in_backpack[current_in_slot_id] = 0;
                ui.items_in_backpack[slot.slot_id] = item_id;
                current_in_slot_id = slot.slot_id;
            }
            else{
                transform.SetParent(ui.slots[current_in_slot_id].transform);
                transform.position = ui.slots[current_in_slot_id].transform.position;
            }
        }
    }
    public void use(){
        print(current_in_slot_id + " " + item_id);
    }
}
