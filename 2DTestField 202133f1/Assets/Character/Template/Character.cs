using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public GameObject pivot , attack_point;
    Rigidbody2D char_ctrl;
    public Vector2 direction = Vector2.zero , velocity = Vector2.zero , facing_direction = Vector2.right;
    [SerializeField] float top_speed = 10f;
    float speed = 0 , acceleration = 10f;
    // Start is called before the first frame update
    void Start()
    {
        get_components();
        initial_parameters();
        equip_weapon();
    }
    void initial_parameters(){
        speed = top_speed;
    }
    void get_components(){
        char_ctrl = GetComponent<Rigidbody2D>();
        weapon_ctrl = GetComponentInChildren<WeaponController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        movement_loop();
        attack_loop();
        body_sprite_ctrl();
        handle_render_order();
    }
    public Melee melee_weapon;
    public Ranged ranged_weapon;
    void equip_weapon(){
        if(weapon_ctrl.GetComponentInChildren<Melee>() != null){
            melee_weapon = weapon_ctrl.GetComponentInChildren<Melee>();
            weapon_ctrl.init();
            melee_weapon.init(this.gameObject);
        }
        else if(weapon_ctrl.GetComponentInChildren<Ranged>() != null){
            ranged_weapon = weapon_ctrl.GetComponentInChildren<Ranged>();
            weapon_ctrl.init();
            ranged_weapon.init(this.gameObject);
        }
    }
    public Vector3 target_position = Vector3.zero;
    void attack_loop(){
        Vector2 facing = target_position - pivot.transform.position;
        pivot.transform.localRotation = Quaternion.Euler(0 , 0 , Mathf.Rad2Deg * Mathf.Atan2(facing.y , facing.x));
    }
    [SerializeField] Hitbox hitbox;
    public void normal_attack(){
        if(melee_weapon != null){
            right_hand_s.enabled = false;
            left_hand_s.enabled = false;
            right_hand_v.enabled = false;
            left_hand_v.enabled = false;
            hitbox.can_take_hit = false;
            melee_weapon.normal_attack();
            StartCoroutine(hitbox_time(melee_weapon.cool_down_time));
        }
        else if(ranged_weapon != null){
            ranged_weapon.normal_attack();
        }
        else{
            Debug.Log("Fist attack");
        }
    }
    public void special_attack(){
        if(melee_weapon != null){
            Debug.Log("melee special attack");
        }
        else if(ranged_weapon != null){
            ranged_weapon.draw_or_put_weapon();
        }
        else{
            Debug.Log("Fist special attack");
        }
    }
    IEnumerator hitbox_time(float time){
        yield return new WaitForSeconds(time);
        hitbox.can_take_hit = true;
        if(looking_at == 0){
            right_hand_s.enabled = true;
            left_hand_s.enabled = true;
        }
        else{
            right_hand_v.enabled = true;
            left_hand_v.enabled = true;
        }
    }
    void movement_loop(){
        direction = direction.normalized;
        velocity = Vector2.Lerp(velocity , direction * speed , acceleration * Time.deltaTime);
        char_ctrl.MovePosition(char_ctrl.position + velocity * Time.deltaTime);
    }
    public SpriteRenderer head_s , head_f , head_b , body_s , body_f , body_b , right_hand_s , left_hand_s , right_leg_s , left_leg_s , right_hand_v , left_hand_v , right_leg_v , left_leg_v;
    public Animator left_leg_animator , right_leg_animator , left_leg_animator_v , right_leg_animator_v;
    WeaponController weapon_ctrl;
    public Transform weapon , feet;
    public int order;
    int looking_at = 0; //0 horizontal 1 down 2 up
    void body_sprite_ctrl(){
        if(direction != Vector2.zero){
            facing_direction = direction;
            if(direction.x >= 0.5f || direction.x <= -0.5f){
                looking_at = 0;
                weapon_ctrl.state = 0;
                right_leg_animator.SetBool("moving" , true);
                left_leg_animator.SetBool("moving" , true);
                head_s.enabled = true;
                head_f.enabled = false;
                head_b.enabled = false;
                body_s.enabled = true;
                body_f.enabled = false;
                body_b.enabled = false;
                right_hand_s.enabled = true;
                left_hand_s.enabled = true;
                right_leg_s.enabled = true;
                left_leg_s.enabled = true;
                right_hand_v.enabled = false;
                left_hand_v.enabled = false;
                right_leg_v.enabled = false;
                left_leg_v.enabled = false;
                if(direction.x > 0){
                    weapon.localPosition = new Vector2(-0.01f , -0.02f);
                    head_s.flipX = false;
                    body_s.flipX = false;
                    right_hand_s.flipX = false;
                    left_hand_s.flipX = false;
                    right_leg_s.flipX = false;
                    left_leg_s.flipX = false;
                }
                else{
                    weapon.localPosition = new Vector2(0.01f , -0.02f);
                    head_s.flipX = true;
                    body_s.flipX = true;
                    right_hand_s.flipX = true;
                    left_hand_s.flipX = true;
                    right_leg_s.flipX = true;
                    left_leg_s.flipX = true;
                }
            }
            else{
                left_leg_animator_v.SetBool("moving" , true);
                right_leg_animator_v.SetBool("moving" , true);
                head_s.enabled = false;
                body_s.enabled = false;
                right_hand_s.enabled = false;
                left_hand_s.enabled = false;
                right_leg_s.enabled = false;
                left_leg_s.enabled = false;
                right_hand_v.enabled = true;
                left_hand_v.enabled = true;
                right_leg_v.enabled = true;
                left_leg_v.enabled = true;
                if(direction.y > 0){
                    looking_at = 2;
                    weapon_ctrl.state = 1;
                    weapon.localPosition = new Vector2(0f , -0.03f);
                    head_b.enabled = true;
                    head_f.enabled = false;
                    body_f.enabled = false;
                    body_b.enabled = true;
                }
                else{
                    looking_at = 1;
                    weapon_ctrl.state = 0;
                    weapon.localPosition = new Vector2(0f , -0.01f);
                    head_f.enabled = true;
                    head_b.enabled = false;
                    body_f.enabled = true;
                    body_b.enabled = false;
                }
            }
        }
        else{
            right_leg_animator.SetBool("moving" , false);
            left_leg_animator.SetBool("moving" , false);
            left_leg_animator_v.SetBool("moving" , false);
            right_leg_animator_v.SetBool("moving" , false);
        }
    }
    void handle_render_order(){
        order = (int)(-feet.position.y * 100);
        head_s.sortingOrder = order;
        head_f.sortingOrder = order;
        head_b.sortingOrder = order;
        body_s.sortingOrder = order + 2;
        body_f.sortingOrder = order + 2;
        body_b.sortingOrder = order - 2;
        switch(looking_at){
            case 0:
                right_hand_s.sortingOrder = order + 3;
                left_hand_s.sortingOrder = order + 1;
                right_leg_s.sortingOrder = order + 3;
                left_leg_s.sortingOrder = order + 1;
                break;
            case 1:
                right_hand_v.sortingOrder = order + 3;
                left_hand_v.sortingOrder = order + 3;
                right_leg_v.sortingOrder = order + 3;
                left_leg_v.sortingOrder = order + 3;
                break;
            case 2:
                right_hand_v.sortingOrder = order - 3;
                left_hand_v.sortingOrder = order - 3;
                right_leg_v.sortingOrder = order - 3;
                left_leg_v.sortingOrder = order - 3;
                break;
        }
    }
}