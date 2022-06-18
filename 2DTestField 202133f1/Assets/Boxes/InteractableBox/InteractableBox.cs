using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableBox : MonoBehaviour
{
    Computer computer;
    private void Awake() {
        computer = GetComponent<Computer>();
    }
    public void interacted(){
        if(computer != null) computer.interacted();
    }
}
