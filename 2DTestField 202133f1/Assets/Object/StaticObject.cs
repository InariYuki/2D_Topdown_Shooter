using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticObject : MonoBehaviour
{
    SpriteRenderer sprite_renderer;
    [HideInInspector] public Transform feet;
    private void Awake() {
        sprite_renderer = GetComponent<SpriteRenderer>();
        feet = transform.GetChild(0);
    }
    void Start(){
        sprite_renderer.sortingOrder = (int)(-feet.position.y * 100);
    }
}
