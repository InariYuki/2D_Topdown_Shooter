using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : MonoBehaviour
{
    public int damage = 25;
    public int special_attack_damage = 50;
    [SerializeField] GameObject hit_effect , clink_effect;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    GameObject parent;
    SpriteRenderer sprite;
    public bool cooling_down = false;
    public float cool_down_time = 0.3f;
    public void init(GameObject _parent){
        parent = _parent;
        sprite = GetComponent<SpriteRenderer>();
    }
    public GameObject first_swing , second_swing , special_swing;
    int combo_count = 1;
    public void normal_attack(){
        if(cooling_down == false){
            Character parent_char = parent.GetComponent<Character>();
            if(combo_count == 1){
                GameObject swing_1 = Instantiate(first_swing , parent_char.attack_point.transform.position , parent_char.attack_point.transform.rotation , parent_char.attack_point.transform);
                swing_1.GetComponent<NormalSwing>().init(parent , damage , hit_effect , clink_effect);
                combo_count++;
            }
            else{
                GameObject swing_2 = Instantiate(first_swing, parent_char.attack_point.transform.position, parent_char.attack_point.transform.rotation, parent_char.attack_point.transform);
                swing_2.GetComponent<NormalSwing>().init(parent, damage, hit_effect, clink_effect);
                combo_count = 1;
            }
            sprite.enabled = false;
            cooling_down = true;
            StartCoroutine(wait_for_cooldown(cool_down_time));
        }
    }
    public float special_attack_cooldown_time = 0.6f;
    bool special_attack_cooling_down = false;
    [SerializeField] LayerMask obstacle;
    public void special_attack(){
        if(special_attack_cooling_down) return;
        Character parent_char = parent.GetComponent<Character>();
        Vector3 dash_point = (parent_char.attack_point.transform.position - parent_char.pivot.transform.position).normalized * 0.4f + parent.transform.position;
        Vector2 vec = dash_point - parent.transform.position;
        if(Physics2D.Raycast(parent.transform.position , vec.normalized , vec.magnitude , obstacle)) return;
        special_attack_cooling_down = true;
        sprite.enabled = false;
        GameObject special_swing_instanced = Instantiate(special_swing , (dash_point + parent_char.attack_point.transform.position)/2 , parent_char.attack_point.transform.rotation);
        special_swing_instanced.GetComponent<DashSlash>().init(parent , special_attack_damage , parent_char.attack_point.transform.position.y - parent_char.pivot.transform.position.y > 0 ? Vector2.Angle(dash_point - parent_char.pivot.transform.position, Vector2.right) : -Vector2.Angle(dash_point - parent_char.pivot.transform.position, Vector2.right) , hit_effect);
        parent.transform.position = dash_point;
        StartCoroutine(wait_for_special_cooldown(special_attack_cooldown_time));
    }
    IEnumerator wait_for_cooldown(float time){
        yield return new WaitForSeconds(time);
        cooling_down = false;
        sprite.enabled = true;
    }
    IEnumerator wait_for_special_cooldown(float time){
        yield return new WaitForSeconds(time);
        special_attack_cooling_down = false;
        sprite.enabled = true;
    }
}
