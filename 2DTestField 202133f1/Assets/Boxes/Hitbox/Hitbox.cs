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
    public bool can_take_hit = true;
    public void hit(int damage , GameObject attacker , GameObject hit_effect){
        if(can_take_hit == false) return;
        Instantiate(hit_effect , transform.position , Quaternion.identity);
        if(parent.GetComponent<ArtificialIntelligence>() != null){
            parent.GetComponent<ArtificialIntelligence>().hit(damage , attacker);
        }
        else if(parent.GetComponent<PlayerColtroller>() != null){
            parent.GetComponent<PlayerColtroller>().hit(damage , attacker);
        }
    }
}
