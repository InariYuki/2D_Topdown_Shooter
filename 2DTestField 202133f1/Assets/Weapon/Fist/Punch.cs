using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Punch : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public int damage = 10;
    public GameObject parent , hit_effect;
    [SerializeField] LayerMask attack;
    public void punch(){
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position , 0.1f , attack);
        for(int i = 0; i < hits.Length; i++){
            if(hits[i].GetComponent<Hitbox>() != null){
                if(hits[i].GetComponent<Hitbox>().parent == parent){
                    continue;
                }
                hits[i].GetComponent<Hitbox>().hit(damage , parent , hit_effect);
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
