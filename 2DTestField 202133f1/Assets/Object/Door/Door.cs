using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    Animator animator;
    SpriteRenderer sprite;
    LayerMask default_layer;
    BoxCollider2D collision;
    [SerializeField] Sprite locked_door_sprite;
    [SerializeField] Transform feet;
    [SerializeField] List<NavBox> navboxes_needs_to_reconnect = new List<NavBox>();
    private void Awake() {
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        default_layer = LayerMask.GetMask("Default");
        collision = GetComponent<BoxCollider2D>();
    }
    private void Start() {
        reconnect_navboxes();
    }
    private void FixedUpdate() {
        detect_character();
    }
    private void OnDrawGizmos() {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(feet.position , detect_radius);
    }
    void door_open(){
        animator.SetBool("door_open" , true);
        collision.enabled = false;
    }
    void door_close(){
        animator.SetBool("door_open" , false);
        collision.enabled = true;
    }
    bool door_locked = false;
    void door_lock(){
        door_close();
        door_locked = true;
        animator.enabled = false;
        sprite.sprite = locked_door_sprite;
        reconnect_navboxes();
    }
    void door_unlock(){
        door_locked = false;
        animator.enabled = true;
        reconnect_navboxes();
    }
    public void door_control(){
        if(door_locked){
            door_unlock();
        }
        else{
            door_lock();
        }
    }
    void reconnect_navboxes(){
        if(door_locked){
            for(int i = 0; i < navboxes_needs_to_reconnect.Count; i++){
                navboxes_needs_to_reconnect[i].connect();
            }
        }
        else{
            collision.enabled = false;
            for(int i = 0; i < navboxes_needs_to_reconnect.Count; i++){
                navboxes_needs_to_reconnect[i].connect();
            }
            collision.enabled = true;
        }
    }
    [SerializeField] float detect_radius;
    void detect_character(){
        if(door_locked) return;
        Collider2D[] people = Physics2D.OverlapCircleAll(feet.position , detect_radius , default_layer);
        if(people.Length != 0){
            door_open();
        }
        else{
            door_close();
        }
    }
}
