using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopButton : MonoBehaviour
{
    [HideInInspector] public UI ui;
    [HideInInspector] public int button_number;
    public void button_clicked(){
        if(transform.childCount == 2){
            ui.item_baught(button_number);
        }
    }
}
