using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinExchangerButton : MonoBehaviour
{
    [HideInInspector] public UI ui;
    [HideInInspector] public int button_number;
    [SerializeField] TextMeshProUGUI price;
    public void ButtonClicked(){
        if(ui.items_in_backpack[button_number] == 0) return;
        ui.ItemSold(button_number);
    }
    public void SetPrice(int _price){
        price.text = _price.ToString();
    }
}
