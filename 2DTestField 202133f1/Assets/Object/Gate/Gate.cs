using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    Animator animator;
    Collider2D collision;
    Hitbox hitbox;
    StaticObject object_self;
    LayerMask navigation_layer;
    private void Awake() {
        animator = GetComponent<Animator>();
        collision = GetComponent<Collider2D>();
        hitbox = GetComponentInChildren<Hitbox>();
        object_self = GetComponent<StaticObject>();
        navigation_layer = LayerMask.GetMask("Navigation");
    }
    bool gate_open = false;
    public void GateControl(){
        if(gate_open){
            GateClose();
        }
        else{
            GateOpen();
        }
        gate_open = !gate_open;
        animator.SetBool("door_open" , gate_open);
    }
    void GateOpen(){
        collision.enabled = false;
        hitbox.DisableHitbox();
        ReconnectNavboxes();
    }
    void GateClose(){
        collision.enabled = true;
        hitbox.EnableHitbox();
        ReconnectNavboxes();
    }
    void ReconnectNavboxes(){
        Collider2D[] nearby_navboxes = Physics2D.OverlapCircleAll(object_self.feet.position , 0.5f , navigation_layer);
        for(int i = 0; i < nearby_navboxes.Length; i++){
            nearby_navboxes[i].GetComponent<NavBox>().connect();
        }
    }
}
