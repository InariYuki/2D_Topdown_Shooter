using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public GameObject parent;
    public bool can_take_hit = true;
    public void hit(int damage , GameObject attacker , GameObject hit_effect){
        if(can_take_hit == false) return;
        ArtificialIntelligence parent_ai = parent.GetComponent<ArtificialIntelligence>();
        PlayerColtroller parent_playerctl = parent.GetComponent<PlayerColtroller>();
        if(parent_ai != null) parent_ai.hit(damage , attacker);
        else if(parent_playerctl != null) parent_playerctl.hit(damage , attacker);
        Instantiate(hit_effect , transform.position , Quaternion.identity);
    }
}
