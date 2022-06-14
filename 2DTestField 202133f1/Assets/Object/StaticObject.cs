using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticObject : MonoBehaviour
{
    [SerializeField] SpriteRenderer sprite_renderer;
    [SerializeField] Transform feet;
    [SerializeField] Collider2D collision;
    private void Awake() {
        sprite_renderer = GetComponent<SpriteRenderer>();
        collision = GetComponent<Collider2D>();
        feet = transform.GetChild(0);
    }
    void Start(){
        sprite_renderer.sortingOrder = (int)(-feet.position.y * 100);
        if(sprite_renderer.flipX){
            Vector2 offset = collision.offset;
            collision.offset = new Vector2(-offset.x , offset.y);
        }
    }
}
