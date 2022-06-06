using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalSwing : MonoBehaviour
{
    public int damage;
    public GameObject parent;
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
    void swing(){
        Collider2D[] things_attacked = Physics2D.OverlapCircleAll(center.position , 0.2f , attack_layer);
        foreach(Collider2D things in things_attacked){
            if(things.GetComponent<Hitbox>() != null){
                if(things.GetComponent<Hitbox>().parent == parent){
                    continue;
                }
                things.GetComponent<Hitbox>().hit(damage , parent);
            }
            else if(things.GetComponent<DeflectableProjectile>() != null){
                things.GetComponent<DeflectableProjectile>().direction = -things.GetComponent<DeflectableProjectile>().direction;
                things.GetComponent<DeflectableProjectile>().parent = parent;
            }
            else if(things.GetComponent<NormalSwing>() != null){
                things.GetComponent<NormalSwing>().recoil(parent);
            }
        }
    }
    public void recoil(GameObject attacker){
        if(attacker != parent.gameObject){
            parent.GetComponent<Character>().velocity = (parent.transform.position - attacker.transform.position).normalized * 8f;
            Debug.Log("clink");
        }
    }
    void end(){
        Destroy(gameObject);
    }
}
