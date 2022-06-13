using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalSwing : MonoBehaviour
{
    public int damage;
    public GameObject parent , hit_effect , clink_effect;
    public LayerMask attack_layer;
    [SerializeField] Transform center;
    // Start is called before the first frame update
    void Start()
    {

    }
    void OnDrawGizmosSelected(){
        Gizmos.DrawWireSphere(center.position , 0.2f);
    }
    // Update is called once per frame
    void Update()
    {

    }
    public void init(GameObject _parent , int _damage, GameObject _hit_effect , GameObject _clink_effect)
    {
        parent = _parent;
        damage = _damage;
        hit_effect = _hit_effect;
        clink_effect = _clink_effect;
    }
    void swing(){
        Collider2D[] things_attacked = Physics2D.OverlapCircleAll(center.position , 0.2f , attack_layer);
        foreach(Collider2D things in things_attacked){
            if(things.GetComponent<Hitbox>() != null){
                if(things.GetComponent<Hitbox>().parent == parent){
                    continue;
                }
                things.GetComponent<Hitbox>().hit(damage , parent , hit_effect);
            }
            else if(things.GetComponent<DeflectableProjectile>() != null){
                things.GetComponent<DeflectableProjectile>().direction = -things.GetComponent<DeflectableProjectile>().direction;
                things.GetComponent<DeflectableProjectile>().parent = parent;
                Instantiate(clink_effect , things.transform.position , Quaternion.identity);
            }
            else if(things.GetComponent<NormalSwing>() != null && things.GetComponent<NormalSwing>().parent != parent){
                things.GetComponent<NormalSwing>().recoil(parent);
                Instantiate(clink_effect , (things.transform.position + center.transform.position)/2 , Quaternion.identity);
            }
        }
    }
    public void recoil(GameObject attacker){
        if(attacker != parent.gameObject){
            parent.GetComponent<Character>().velocity = (parent.transform.position - attacker.transform.position).normalized * 8f;
        }
    }
    void end(){
        Destroy(gameObject);
    }
}
