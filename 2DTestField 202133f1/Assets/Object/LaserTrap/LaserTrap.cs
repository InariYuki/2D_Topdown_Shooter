using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTrap : MonoBehaviour
{
    LayerMask attack_layer;
    [SerializeField] LineRenderer line_renderer_r , line_renderer_l , line_renderer_u , line_renderer_d;
    private void Awake() {
        attack_layer = LayerMask.GetMask("Attack");
    }
    private void FixedUpdate() {
        if(!trap_active) return;
        rotate();
        laser_cast();
    }
    bool trap_active = true;
    public void toggle_laser_trap(){
        trap_active = !trap_active;
        if(!trap_active){
            line_renderer_r.enabled = false;
            line_renderer_l.enabled = false;
            line_renderer_u.enabled = false;
            line_renderer_d.enabled = false;
        }
        else{
            line_renderer_r.enabled = true;
            line_renderer_l.enabled = true;
            line_renderer_u.enabled = true;
            line_renderer_d.enabled = true;
        }
    }
    void rotate(){
        transform.Rotate(new Vector3(0 , 0 , -1f));
    }
    [SerializeField] Transform muzzle_right , muzzle_left , muzzle_up , muzzle_down;
    [SerializeField] GameObject hit_effect;
    void laser_cast(){
        Vector2 vec_r = muzzle_right.position - transform.position;
        RaycastHit2D hit_r = Physics2D.Raycast(muzzle_right.position , vec_r.normalized , vec_r.magnitude * 20f , attack_layer);
        line_renderer_r.SetPositions(new Vector3[] {muzzle_right.position , hit_r.point});
        Vector2 vec_l = muzzle_left.position - transform.position;
        RaycastHit2D hit_l = Physics2D.Raycast(muzzle_left.position , vec_l.normalized , vec_l.magnitude * 20f , attack_layer);
        line_renderer_l.SetPositions(new Vector3[] {muzzle_left.position , hit_l.point});
        Vector2 vec_u = muzzle_up.position - transform.position;
        RaycastHit2D hit_u = Physics2D.Raycast(muzzle_up.position , vec_u.normalized , vec_u.magnitude * 20f , attack_layer);
        line_renderer_u.SetPositions(new Vector3[] {muzzle_up.position , hit_u.point});
        Vector2 vec_d = muzzle_down.position - transform.position;
        RaycastHit2D hit_d = Physics2D.Raycast(muzzle_down.position , vec_d.normalized , vec_d.magnitude * 20f , attack_layer);
        line_renderer_d.SetPositions(new Vector3[] {muzzle_down.position , hit_d.point});
        Hitbox hitbox_hit_r = hit_r.collider.GetComponent<Hitbox>();
        Hitbox hitbox_hit_l = hit_l.collider.GetComponent<Hitbox>();
        Hitbox hitbox_hit_u = hit_u.collider.GetComponent<Hitbox>();
        Hitbox hitbox_hit_d = hit_d.collider.GetComponent<Hitbox>();
        if(hitbox_hit_r != null && hitbox_hit_r.parent.GetComponent<Character>() != null) hitbox_hit_r.hit(80 , null , hit_effect);
        if(hitbox_hit_l != null && hitbox_hit_l.parent.GetComponent<Character>() != null) hitbox_hit_l.hit(80 , null , hit_effect);
        if(hitbox_hit_u != null && hitbox_hit_u.parent.GetComponent<Character>() != null) hitbox_hit_u.hit(80 , null , hit_effect);
        if(hitbox_hit_d != null && hitbox_hit_d.parent.GetComponent<Character>() != null) hitbox_hit_d.hit(80 , null , hit_effect);
    }
}
