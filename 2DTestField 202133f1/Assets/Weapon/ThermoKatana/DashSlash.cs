using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashSlash : MonoBehaviour
{
    public GameObject parent , hit_effect , clink_effect;
    public int damage;
    public float rotation;
    public void init(GameObject _parent , int _damage , float _rotation , GameObject _hit_effect)
    {
        parent = _parent;
        damage = _damage;
        rotation = _rotation;
        hit_effect = _hit_effect;
        slash();
    }
    [SerializeField] LayerMask attack_mask;
    void slash(){
        Collider2D[] hits = Physics2D.OverlapCapsuleAll(transform.position , new Vector2(1.2f , 0.3f) , CapsuleDirection2D.Horizontal , rotation ,  attack_mask);
        for(int i = 0; i < hits.Length ; i++){
            Hitbox things_hitbox = hits[i].GetComponent<Hitbox>();
            if(things_hitbox != null){
                if(things_hitbox.parent == parent){
                    continue;
                }
                if(things_hitbox.parent.GetComponent<Character>() != null) things_hitbox.hit(damage , parent , hit_effect);
                else if(things_hitbox.parent.GetComponent<BreakableObject>() != null) things_hitbox.hit(damage , parent , clink_effect);
            }
        }
    }
    void end(){
        Destroy(gameObject);
    }
}
