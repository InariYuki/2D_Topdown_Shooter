using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ranged : MonoBehaviour
{
    GameObject parent;
    SpriteRenderer sprite;
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
    bool drawed = false;
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
    public void normal_attack(){
        if(drawed == false){
            Debug.Log("weapon not drawed");
            return;
        }
        Debug.Log("bang");
    }
}
