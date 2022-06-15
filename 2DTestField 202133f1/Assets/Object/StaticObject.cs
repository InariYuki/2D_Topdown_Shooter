using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticObject : MonoBehaviour
{
    [SerializeField] SpriteRenderer sprite_renderer;
    [SerializeField] Transform feet;
    private void Awake() {
        sprite_renderer = GetComponent<SpriteRenderer>();
        feet = transform.GetChild(0);
    }
    void Start(){
        sprite_renderer.sortingOrder = (int)(-feet.position.y * 100);
    }
}
