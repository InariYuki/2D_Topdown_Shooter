using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteDatabase : MonoBehaviour
{
    [SerializeField] bool random_appearence = false;
    [SerializeField] bool random_armor = false;
    [SerializeField] bool always_has_weapon = false;
    int has_weapon_threshold = 30;
    [SerializeField] SpriteData sprite_database;
    Character character;
    NPC npc;
    private void Awake() {
        character = GetComponent<Character>();
        npc = GetComponent<NPC>();
    }
    private void Start() {
        if(random_appearence){
            set_random_sprite();
            if(random_armor){
                Instantiate(npc.ui.Item_database.items[(Random.Range(3 , 6))].item_instanced , character.armor_holder.position , Quaternion.identity , character.armor_holder);
                character.equip_armor();
            }
            if(always_has_weapon){
                has_weapon_threshold = 100;
            }
            if(Random.Range(0 , 100) < has_weapon_threshold){
                Instantiate(npc.ui.Item_database.items[(Random.Range(1 , 3))].item_instanced , character.weapon.position , Quaternion.identity , character.weapon);
                character.equip_weapon();
            }
        }
    }
    public void set_sprite(int head_index , int body_index , int limb_index){
        Head head = sprite_database.head_database[head_index];
        Body body = sprite_database.body_database[body_index];
        Limb limb = sprite_database.limb_database[limb_index];
        character.carcass_head_s = head.head_s;
        character.carcass_head_f = head.head_f;
        character.carcass_head_b = head.head_b;
        character.carcass_body_s = body.body_s;
        character.carcass_body_f = body.body_f;
        character.carcass_body_b = body.body_b;
        character.carcass_right_hand_s = limb.right_hand_s;
        character.carcass_left_hand_s = limb.left_hand_s;
        character.carcass_right_leg_s = limb.right_leg_s;
        character.carcass_left_leg_s = limb.left_leg_s;
        character.carcass_right_hand_s = limb.right_hand_v;
        character.carcass_left_hand_s = limb.left_hand_v;
        character.carcass_right_leg_s = limb.right_leg_v;
        character.carcass_left_leg_s = limb.left_leg_v;
        character.set_carcass();
    }
    public void set_random_sprite(){
        set_sprite(Random.Range(0 , sprite_database.head_database.Count) , Random.Range(0 , sprite_database.body_database.Count) , Random.Range(0 , sprite_database.limb_database.Count));
    }
}
