using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour , IDropHandler
{
    public UI ui;
    public void OnDrop(PointerEventData eventData){
        if(eventData.pointerDrag != null){
            eventData.pointerDrag.transform.SetParent(transform);
            eventData.pointerDrag.transform.position = transform.position;
        }
    }
}
