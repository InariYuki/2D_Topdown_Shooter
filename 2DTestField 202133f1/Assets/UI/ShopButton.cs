using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopButton : MonoBehaviour
{
    [HideInInspector] public UI ui;
    [HideInInspector] public int button_number;
    [SerializeField] TextMeshProUGUI price_tag;
    public void button_clicked(){
        if(transform.childCount == 2){
            ui.item_baught(button_number);
        }
    }
    public void set_price(int price){
        price_tag.text = price.ToString();
    }
}
