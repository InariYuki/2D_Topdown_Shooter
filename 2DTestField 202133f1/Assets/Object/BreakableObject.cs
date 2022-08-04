using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObject : MonoBehaviour
{
    [SerializeField] int max_health = 200;
    [SerializeField] int damage_threshold = 100;
    int health;
    Collider2D collision;
    LayerMask navbox_layer;
    StaticObject obj;
    private void Awake() {
        collision = GetComponent<Collider2D>();
        health = max_health;
        obj = GetComponent<StaticObject>();
        navbox_layer = LayerMask.GetMask("Navigation");
    }
    public void hit(int damage){
        if(damage >= damage_threshold){
            health -= damage;
        }
        if(health <= 0){
            destroyed();
        }
    }
    [HideInInspector] public bool broken = false;
    [SerializeField] GameObject pieces;
    void destroyed(){
        broken = true;
        collision.enabled = false;
        Break();
        Instantiate(pieces , transform.position , Quaternion.identity);
        Destroy(gameObject);
    }
    float radius = 0.5f;
    void Break(){
        Collider2D[] colliders = Physics2D.OverlapCircleAll(obj.feet.position , radius , navbox_layer);
        for(int i = 0; i < colliders.Length ; i++){
            colliders[i].GetComponent<NavBox>().connect();
        }
    }
}
