using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Computer : MonoBehaviour
{
    string[] interact_methods = {"control locked doors" , "control traps"};
    public void interacted(int interact_state){
        if(interact_state == 0){
            print("Hello I am computer");
        }
        else{
            
        }
    }
}
