using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashSlash : MonoBehaviour
{
    public GameObject parent , hit_effect;
    public int damage;
    public float rotation;
    // Start is called before the first frame update
    void Start()
    {
        slash();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    [SerializeField] LayerMask attack_mask;
    void slash(){
        Collider2D[] hits = Physics2D.OverlapCapsuleAll(transform.position , new Vector2(1.2f , 0.3f) , CapsuleDirection2D.Horizontal , rotation ,  attack_mask);
        foreach(Collider2D things in hits){
            if(things.GetComponent<Hitbox>() != null){
                if(things.GetComponent<Hitbox>().parent == parent){
                    continue;
                }
                things.GetComponent<Hitbox>().hit(damage , parent , hit_effect);
            }
        }
    }
    void end(){
        Destroy(gameObject);
    }
}
