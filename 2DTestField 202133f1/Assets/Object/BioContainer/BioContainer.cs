using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BioContainer : MonoBehaviour
{
    [SerializeField] Sprite full , empty , off , broken;
    SpriteRenderer sprite;
    StaticObject obj_self;
    private void Awake() {
        sprite = GetComponent<SpriteRenderer>();
        obj_self = GetComponent<StaticObject>();
    }
    [SerializeField] GameObject glass_break_effect;
    public void destroy(){
        sprite.sprite = broken;
        Instantiate(glass_break_effect , obj_self.feet.position , Quaternion.identity);
    }
}
