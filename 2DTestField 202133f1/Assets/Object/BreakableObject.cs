using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObject : MonoBehaviour
{
    [SerializeField] int max_health = 200;
    [SerializeField] int damage_threshold = 100;
    int health;
    Collider2D collision;
    private void Awake() {
        collision = GetComponent<Collider2D>();
        health = max_health;
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
        Instantiate(pieces , transform.position , Quaternion.identity);
        Destroy(gameObject);
    }
}
