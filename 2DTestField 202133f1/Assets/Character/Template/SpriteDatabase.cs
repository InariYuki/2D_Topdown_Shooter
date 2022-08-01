using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteDatabase : MonoBehaviour
{
    [SerializeField] bool random_appearence = false;
    [SerializeField] bool random_armor = false;
    [SerializeField] bool always_has_weapon = false;
    int has_weapon_threshold = 30;
    [SerializeField] List<Sprite> head_s = new List<Sprite>();
    [SerializeField] List<Sprite> head_f = new List<Sprite>();
    [SerializeField] List<Sprite> head_b = new List<Sprite>();
    [SerializeField] List<Sprite> body_s = new List<Sprite>();
    [SerializeField] List<Sprite> body_f = new List<Sprite>();
    [SerializeField] List<Sprite> body_b = new List<Sprite>();
    [SerializeField] List<Sprite> right_hand_s = new List<Sprite>();
    [SerializeField] List<Sprite> left_hand_s = new List<Sprite>();
    [SerializeField] List<Sprite> right_leg_s = new List<Sprite>();
    [SerializeField] List<Sprite> left_leg_s = new List<Sprite>();
    [SerializeField] List<Sprite> right_hand_v = new List<Sprite>();
    [SerializeField] List<Sprite> left_hand_v = new List<Sprite>();
    [SerializeField] List<Sprite> right_leg_v = new List<Sprite>();
    [SerializeField] List<Sprite> left_leg_v = new List<Sprite>();
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
        character.carcass_head_s = head_s[head_index];
        character.carcass_head_f = head_f[head_index];
        character.carcass_head_b = head_b[head_index];
        character.carcass_body_s = body_s[body_index];
        character.carcass_body_f = body_f[body_index];
        character.carcass_body_b = body_b[body_index];
        character.carcass_right_hand_s = right_hand_s[limb_index];
        character.carcass_left_hand_s = left_hand_s[limb_index];
        character.carcass_right_leg_s = right_leg_s[limb_index];
        character.carcass_left_leg_s = left_leg_s[limb_index];
        character.carcass_right_hand_v = right_hand_v[limb_index];
        character.carcass_left_hand_v = left_hand_v[limb_index];
        character.carcass_right_leg_v = right_leg_v[limb_index];
        character.carcass_left_leg_v = left_leg_v[limb_index];
        character.set_carcass();
        character.sprite_init();
    }
    public void set_random_sprite(){
        set_sprite(Random.Range(0 , head_b.Count) , Random.Range(0 , body_b.Count) , Random.Range(0 , right_hand_s.Count));
    }
}
