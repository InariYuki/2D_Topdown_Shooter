using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObject : MonoBehaviour
{
    [SerializeField] Sprite broken;
    [SerializeField] int max_health = 200;
    [SerializeField] int damage_threshold = 100;
    int health;
    SpriteRenderer sprite;
    Collider2D collision;
    private void Awake() {
        sprite = GetComponent<SpriteRenderer>();
        collision = GetComponent<Collider2D>();
        health = max_health;
    }
    public void hit(int damage){
        if(damage >= damage_threshold){
            health -= damage;
        }
        if(health <= 0){
            collision.enabled = false;
            sprite.sprite = broken;
            for(int i = 0; i < transform.childCount; i++){
                Destroy(transform.GetChild(i).gameObject);
            }
        }
    }
}
