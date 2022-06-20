using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeflectableProjectile : MonoBehaviour
{
    public GameObject parent , hit_effect;
    public Vector3 direction;
    public float speed = 10f , ttl = 5f;
    public int damage = 0;
    Rigidbody2D body;
    float radius = 0.05f;
    [SerializeField] LayerMask layermask;
    [SerializeField] Collider2D collision;
    void Awake(){
        body = GetComponent<Rigidbody2D>();
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
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius, layermask);
        for (int i = 0; i < hits.Length; i++){
            if(hits[0] == collision){
                continue;
            }
            Hitbox thing_hitbox = hits[0].GetComponent<Hitbox>();
            if(thing_hitbox != null && thing_hitbox.parent != parent){
                thing_hitbox.hit(damage , parent , hit_effect);
                Destroy(gameObject);
            }
            else if(hits[0].GetComponent<StaticObject>() != null){
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
