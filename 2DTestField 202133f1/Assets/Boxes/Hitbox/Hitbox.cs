using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public GameObject parent;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void hit(int damage , GameObject attacker){
        if(parent.GetComponent<ArtificialIntelligence>() != null){
            parent.GetComponent<ArtificialIntelligence>().hit(damage , attacker);
        }
        else if(parent.GetComponent<PlayerColtroller>() != null){
            parent.GetComponent<PlayerColtroller>().hit(damage , attacker);
        }
        else if(parent.GetComponent<NormalSwing>() != null){
            parent.GetComponent<NormalSwing>().recoil(attacker);
        }
    }
}
