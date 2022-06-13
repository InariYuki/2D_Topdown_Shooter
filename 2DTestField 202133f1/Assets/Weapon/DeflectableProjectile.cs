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
    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        StartCoroutine(time_to_live(ttl));
    }
    IEnumerator time_to_live(float time){
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        body.MovePosition(body.transform.position + direction * speed * Time.deltaTime);
        foreach(Collider2D thing in Physics2D.OverlapCircleAll(transform.position , radius , layermask)){
            if(thing == collision){
                continue;
            }
            if(thing.GetComponent<Hitbox>() != null && thing.GetComponent<Hitbox>().parent != parent){
                thing.GetComponent<Hitbox>().hit(damage , parent , hit_effect);
                Destroy(gameObject);
            }
            else if(thing.GetComponent<StaticObject>() != null){
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
