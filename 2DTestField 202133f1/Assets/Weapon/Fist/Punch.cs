using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Punch : MonoBehaviour
{
    public int damage = 10;
    public GameObject parent , hit_effect , clink_effect;
    [SerializeField] LayerMask attack;
    public void punch(){
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position , 0.1f , attack);
        for(int i = 0; i < hits.Length ; i++){
            Hitbox hit = hits[i].GetComponent<Hitbox>();
            if(hit != null){
                if(hit.parent == parent) continue;
                if(hit.parent.GetComponent<Character>() != null) hit.hit(damage , parent , hit_effect);
                else if(hit.parent.GetComponent<BreakableObject>() != null) hit.hit(damage , parent , clink_effect);
            }
        }
    }
    public void end(){
        Destroy(gameObject);
    }
    void OnDrawGizmosSelected(){
        Gizmos.DrawWireSphere(transform.position , 0.1f);
    }
}
