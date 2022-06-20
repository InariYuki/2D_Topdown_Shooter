using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour , IPointerDownHandler , IBeginDragHandler , IEndDragHandler , IDragHandler
{
    RectTransform rect_transform;
    CanvasGroup canvas_group;
    public Canvas ui;
    public int current_in_slot_id = 0 , item_id = 0;
    private void Awake() {
        rect_transform = GetComponent<RectTransform>();
        canvas_group = GetComponent<CanvasGroup>();
    }
    public void OnPointerDown(PointerEventData eventData){

    }
    public void OnBeginDrag(PointerEventData eventData){
        transform.SetParent(ui.transform);
        canvas_group.blocksRaycasts = false;
        canvas_group.alpha = 0.6f;
        ui.GetComponent<UI>().items_in_backpack[current_in_slot_id] = 0;
        ui.GetComponent<UI>().backpack.slots[current_in_slot_id] = null;
    }
    public void OnDrag(PointerEventData eventData){
        rect_transform.anchoredPosition += eventData.delta / ui.scaleFactor;
    }
    public void OnEndDrag(PointerEventData eventData){
        canvas_group.blocksRaycasts = true;
        canvas_group.alpha = 1f;
        Slot slot = transform.parent.GetComponent<Slot>();
        if(slot == null){
            print("item dropped");
        }
        else{
            current_in_slot_id = slot.slot_id;
            ui.GetComponent<UI>().items_in_backpack[current_in_slot_id] = item_id;
            ui.GetComponent<UI>().backpack.slots[current_in_slot_id] = gameObject;
        }
    }
}
