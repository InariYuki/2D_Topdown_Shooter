using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour , IDropHandler
{
    public int slot_id;
    public void OnDrop(PointerEventData eventData){
        if(eventData.pointerDrag != null){
            Transform dragged_item = eventData.pointerDrag.transform;
            dragged_item.SetParent(transform);
            dragged_item.position = transform.position;
        }
    }
}
