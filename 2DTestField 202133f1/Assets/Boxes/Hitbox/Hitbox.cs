using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public GameObject parent;
    Collider2D hitbox_collider;
    private void Awake() {
        hitbox_collider = GetComponent<Collider2D>();
    }
    public bool can_take_hit = true;
    public void hit(int damage , GameObject attacker , GameObject hit_effect){
        if(can_take_hit == false) return;
        ArtificialIntelligence parent_ai = parent.GetComponent<ArtificialIntelligence>();
        PlayerColtroller parent_playerctl = parent.GetComponent<PlayerColtroller>();
        BreakableObject parent_object = parent.GetComponent<BreakableObject>();
        if(parent_ai != null) parent_ai.hit(damage , attacker);
        else if(parent_playerctl != null) parent_playerctl.hit(damage , attacker);
        else if(parent_object != null) parent_object.hit(damage);
        Instantiate(hit_effect , transform.position , Quaternion.identity);
    }
    public void DisableHitbox(){
        hitbox_collider.enabled = false;
    }
    public void EnableHitbox(){
        hitbox_collider.enabled = true;
    }
}
