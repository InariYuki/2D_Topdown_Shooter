using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableBox : MonoBehaviour
{
    Computer computer;
    Item item;
    private void Awake() {
        computer = GetComponent<Computer>();
        item = GetComponent<Item>();
    }
    public void interacted(PlayerColtroller player){
        if(computer != null) computer.interacted();
        else if(item != null) item.interacted(player);
    }
}
