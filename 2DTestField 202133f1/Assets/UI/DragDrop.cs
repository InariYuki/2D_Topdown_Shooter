using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour , IPointerDownHandler , IBeginDragHandler , IEndDragHandler , IDragHandler
{
    RectTransform rect_transform;
    CanvasGroup canvas_group;
    public Canvas ui;
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
    }
    public void OnDrag(PointerEventData eventData){
        rect_transform.anchoredPosition += eventData.delta / ui.scaleFactor;
    }
    public void OnEndDrag(PointerEventData eventData){
        canvas_group.blocksRaycasts = true;
        canvas_group.alpha = 1f;
        if(transform.parent.GetComponent<Slot>() == null){
            print("item dropped");
        }
    }
}
