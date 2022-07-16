using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeflectableProjectile : MonoBehaviour
{
    public GameObject parent , hit_effect , clink_effect;
    public Vector3 direction;
    public float speed = 10f , ttl = 5f;
    public int damage = 0;
    Rigidbody2D body;
    float radius = 0.05f;
    LayerMask attack_layer;
    [SerializeField] Collider2D collision;
    void Awake(){
        body = GetComponent<Rigidbody2D>();
        attack_layer = LayerMask.GetMask("Attack");
    }
    void Start()
    {
        StartCoroutine(time_to_live(ttl));
    }
    IEnumerator time_to_live(float time){
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
    void FixedUpdate()
    {
        body.MovePosition(body.transform.position + direction * speed * Time.deltaTime);
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position , radius , attack_layer);
        for(int i = 0; i < hits.Length; i++){
            if(hits[i] == collision){
                continue;
            }
            Hitbox thing_hitbox = hits[i].GetComponent<Hitbox>();
            if(thing_hitbox != null && thing_hitbox.parent != parent){
                try{
                    if(thing_hitbox.parent.GetComponent<Character>() != null) thing_hitbox.hit(damage , parent , hit_effect);
                    else if(thing_hitbox.parent.GetComponent<BreakableObject>() != null) thing_hitbox.hit(damage , parent , clink_effect);
                }
                catch{
                    thing_hitbox.hit(damage , gameObject , hit_effect);
                }
                Destroy(gameObject);
            }
        }
    }
    public void init(GameObject _parent , Vector3 _direction , int _damage , GameObject _hit_effect)
    {
        parent = _parent;
        direction = _direction;
        damage = _damage;
        hit_effect = _hit_effect;
    }
    void OnDrawGizmos(){
        Gizmos.DrawWireSphere(transform.position , radius);
    }
}
