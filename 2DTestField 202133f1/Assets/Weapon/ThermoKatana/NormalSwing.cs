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
        for(int i = 0; i < things_attacked.Length; i++){
            Hitbox things_hitbox = things_attacked[i].GetComponent<Hitbox>();
            DeflectableProjectile things_defelectable_projectile = things_attacked[i].GetComponent<DeflectableProjectile>();
            NormalSwing things_normal_swing = things_attacked[i].GetComponent<NormalSwing>();
            if(things_hitbox != null){
                if(things_hitbox.parent == parent){
                    continue;
                }
                things_hitbox.hit(damage , parent , hit_effect);
            }
            else if(things_defelectable_projectile != null){
                things_defelectable_projectile.direction = -things_defelectable_projectile.direction;
                things_defelectable_projectile.parent = parent;
                Instantiate(clink_effect , things_attacked[i].transform.position , Quaternion.identity);
            }
            else if(things_normal_swing != null && things_normal_swing.parent != parent){
                things_normal_swing.recoil(parent);
                Instantiate(clink_effect , (things_attacked[i].transform.position + center.transform.position)/2 , Quaternion.identity);
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
