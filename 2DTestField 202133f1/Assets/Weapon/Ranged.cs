using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ranged : MonoBehaviour
{
    GameObject parent;
    SpriteRenderer sprite;
    [SerializeField] GameObject hit_effect;
    public int damage = 30;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(weapon_on_hand_instanced == null) return;
        if((parent.GetComponent<Character>().attack_point.transform.position.x - parent.GetComponent<Character>().pivot.transform.position.x) > 0){
            weapon_on_hand_instanced.GetComponentInChildren<SpriteRenderer>().flipY = false;
        }
        else{
            weapon_on_hand_instanced.GetComponentInChildren<SpriteRenderer>().flipY = true;
        }
    }
    public void init(GameObject _parent){
        parent = _parent;
        sprite = GetComponent<SpriteRenderer>();
    }
    public bool drawed = false;
    [SerializeField] GameObject weapon_on_hand;
    GameObject weapon_on_hand_instanced;
    public void draw_or_put_weapon(){
        if(drawed){
            sprite.enabled = true;
            Destroy(weapon_on_hand_instanced);
            weapon_on_hand_instanced = null;
            drawed = false;
        }
        else{
            sprite.enabled = false;
            weapon_on_hand_instanced = Instantiate(weapon_on_hand , parent.GetComponent<Character>().attack_point.transform.position , parent.GetComponent<Character>().attack_point.transform.rotation , parent.GetComponent<Character>().attack_point.transform);
            drawed = true;
        }
    }
    [SerializeField] GameObject bullet;
    public float cool_down_time = 0.3f;
    bool cooling_down = false;
    public void normal_attack(){
        if(drawed == false){
            Debug.Log("weapon not drawed");
            return;
        }
        if(cooling_down) return;
        cooling_down = true;
        weapon_on_hand_instanced.GetComponent<RangedOnHand>().animator.SetTrigger("fire");
        GameObject bullet_instanced = Instantiate(bullet , weapon_on_hand_instanced.GetComponent<RangedOnHand>().muzzle_position.position , parent.GetComponent<Character>().attack_point.transform.rotation);
        bullet_instanced.GetComponent<DeflectableProjectile>().init(parent , (weapon_on_hand_instanced.GetComponent<RangedOnHand>().muzzle_position.position - weapon_on_hand_instanced.transform.position).normalized , damage , hit_effect);
        StartCoroutine(weapon_cool_down(cool_down_time));
    }
    IEnumerator weapon_cool_down(float time){
        yield return new WaitForSeconds(time);
        cooling_down = false;
    }
}
