using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public GameObject pivot , attack_point;
    Rigidbody2D char_ctrl;
    public Collider2D collision;
    public Vector2 direction = Vector3.zero , velocity = Vector3.zero , facing_direction = Vector3.right;
    public float top_speed = 10f , speed = 0;
    float acceleration = 10f;
    void Awake(){
        char_ctrl = GetComponent<Rigidbody2D>();
        weapon_ctrl = GetComponentInChildren<WeaponController>();
        collision = GetComponent<Collider2D>();
    }
    void Start(){
        equip_weapon();
    }
    void FixedUpdate()
    {
        if (dead) return;
        movement_loop();
        soft_collision();
        attack_loop();
        body_sprite_ctrl();
        handle_render_order();
    }
    public Melee melee_weapon = null;
    public Ranged ranged_weapon = null;
    void equip_weapon(){
        if(weapon_ctrl.transform.childCount == 0) return;
        Transform weapon = weapon_ctrl.transform.GetChild(0);
        melee_weapon = weapon.GetComponent<Melee>();
        ranged_weapon = weapon.GetComponent<Ranged>();
        if(melee_weapon != null || ranged_weapon != null){
            weapon_ctrl.weapon_sprite_renderer = weapon.GetComponent<SpriteRenderer>();
            if(melee_weapon != null) melee_weapon.init(this.gameObject);
            else ranged_weapon.init(this.gameObject);
        }
    }
    public Vector3 target_position = Vector3.zero;
    void attack_loop(){
        Vector2 facing = target_position - pivot.transform.position;
        pivot.transform.localRotation = Quaternion.Euler(0 , 0 , Mathf.Rad2Deg * Mathf.Atan2(facing.y , facing.x));
    }
    [SerializeField] Hitbox hitbox;
    [SerializeField] GameObject punch;
    bool punch_cooling_down = false;
    float punch_cool_down_time = 0.2f;
    public void normal_attack(){
        if(melee_weapon != null){
            disable_hand_sprite();
            hitbox.can_take_hit = false;
            melee_weapon.normal_attack();
            StartCoroutine(hitbox_time(melee_weapon.cool_down_time));
        }
        else if(ranged_weapon != null) ranged_weapon.normal_attack();
        else{
            if(punch_cooling_down) return;
            punch_cooling_down = true;
            disable_hand_sprite();
            punch.GetComponent<Punch>().parent = gameObject;
            Instantiate(punch , attack_point.transform.position , attack_point.transform.rotation , attack_point.transform);
            StartCoroutine(punch_cool_down(punch_cool_down_time));
        }
    }
    IEnumerator punch_cool_down(float time){
        yield return new WaitForSeconds(time);
        punch_cooling_down = false;
        enable_hand_sprite();
    }
    public void special_attack(){
        if(melee_weapon != null){
            disable_hand_sprite();
            hitbox.can_take_hit = false;
            melee_weapon.special_attack();
            StartCoroutine(hitbox_time(melee_weapon.cool_down_time));
        }
        else if(ranged_weapon != null) ranged_weapon.draw_or_put_weapon();
    }
    IEnumerator hitbox_time(float time){
        yield return new WaitForSeconds(time);
        enable_hand_sprite();
        hitbox.can_take_hit = true;
    }
    void disable_hand_sprite(){
        right_hand.enabled = false;
        left_hand.enabled = false;
    }
    void enable_hand_sprite(){
        left_hand.enabled = true;
        right_hand.enabled = true;
    }
    void movement_loop(){
        direction = direction.normalized;
        velocity = Vector3.Lerp(velocity , direction * speed , acceleration * Time.deltaTime);
        char_ctrl.MovePosition(char_ctrl.position + velocity * Time.deltaTime);
    }
    float soft_collision_radius = 0.025f;
    [SerializeField] LayerMask soft_layer;
    void soft_collision(){
        Collider2D[] clds = Physics2D.OverlapCircleAll(feet.position , soft_collision_radius , soft_layer);
        foreach(Collider2D cld in clds){
            Character cld_char = cld.GetComponent<Character>();
            if(cld_char!= null && cld_char == this) continue;
            Vector2 collision_vector = cld.transform.position - feet.transform.position;
            Vector2 hit_point_normal = Physics2D.Raycast(feet.position , collision_vector.normalized , collision_vector.magnitude , soft_layer).normal;
            velocity += hit_point_normal.normalized * 0.1f;
        }
    }
    public Sprite head_s , head_f , head_b , body_s , body_f , body_b , right_hand_s , left_hand_s , right_leg_s , left_leg_s , right_hand_v , left_hand_v , right_leg_v , left_leg_v;
    public SpriteRenderer head , body , right_hand , left_hand , right_leg , left_leg;
    public Animator left_leg_animator , right_leg_animator;
    WeaponController weapon_ctrl;
    public Transform weapon , feet;
    public int order;
    int looking_at = 0;
    void body_sprite_ctrl(){
        if(direction != Vector2.zero){
            facing_direction = direction;
            if(direction.x >= 0.5f || direction.x <= -0.5f){
                right_leg_animator.Play("WalkH1");
                left_leg_animator.Play("WalkH2");
                head.sprite = head_s;
                body.sprite = body_s;
                right_hand.sprite = right_hand_s;
                left_hand.sprite = left_hand_s;
                right_leg.sprite = right_leg_s;
                left_leg.sprite = left_leg_s;
                looking_at = 0;
                weapon_ctrl.state = 0;
                if(direction.x > 0){
                    head.flipX = false;
                    body.flipX = false;
                    right_hand.flipX = false;
                    left_hand.flipX = false;
                    right_leg.flipX = false;
                    left_leg.flipX = false;
                    weapon.localPosition = new Vector2(-0.01f , -0.02f);
                }
                else{
                    head.flipX = true;
                    body.flipX = true;
                    right_hand.flipX = true;
                    left_hand.flipX = true;
                    right_leg.flipX = true;
                    left_leg.flipX = true;
                    weapon.localPosition = new Vector2(0.01f , -0.02f);
                }
            }
            else{
                right_leg_animator.Play("WalkV1");
                left_leg_animator.Play("WalkV2");
                right_hand.sprite = right_hand_v;
                left_hand.sprite = left_hand_v;
                right_leg.sprite = right_leg_v;
                left_leg.sprite = left_leg_v;
                if(direction.y > 0){
                    head.sprite = head_b;
                    body.sprite = body_b;
                    looking_at = 2;
                    weapon_ctrl.state = 1;
                    weapon.localPosition = new Vector2(0f , -0.03f);
                }
                else{
                    head.sprite = head_f;
                    body.sprite = body_f;
                    looking_at = 1;
                    weapon_ctrl.state = 0;
                    weapon.localPosition = new Vector2(0f , -0.01f);
                }
            }
        }
        else{
            right_leg_animator.Play("Idle");
            left_leg_animator.Play("Idle");
        }
    }
    void handle_render_order(){
        order = (int)(-feet.position.y * 100);
        head.sortingOrder = order;
        switch(looking_at){ //0 horizontal 1 down 2 up
            case 0:
                body.sortingOrder = order + 2;
                right_hand.sortingOrder = order + 3;
                left_hand.sortingOrder = order + 1;
                right_leg.sortingOrder = order + 3;
                left_leg.sortingOrder = order + 1;
                break;
            case 1:
                body.sortingOrder = order + 1;
                right_hand.sortingOrder = order + 2;
                left_hand.sortingOrder = order + 2;
                right_leg.sortingOrder = order + 2;
                left_leg.sortingOrder = order + 2;
                break;
            case 2:
                body.sortingOrder = order - 1;
                right_hand.sortingOrder = order - 2;
                left_hand.sortingOrder = order - 2;
                right_leg.sortingOrder = order - 2;
                left_leg.sortingOrder = order - 2;
                break;
        }
    }
    [SerializeField] GameObject corpse;
    bool _dead = false;
    public bool dead{get{return _dead;}}
    public void die(){
        if (_dead) return;
        _dead = true;
        Instantiate(corpse , transform.position , Quaternion.identity);
        foreach(Transform child in transform) Destroy(child.gameObject);
        StartCoroutine(wait_to_destroy(5f));
    }
    IEnumerator wait_to_destroy(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
