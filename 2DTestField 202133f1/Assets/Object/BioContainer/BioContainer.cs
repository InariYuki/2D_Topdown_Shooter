using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BioContainer : MonoBehaviour
{
    [SerializeField] Sprite full , empty , broken , off;
    [SerializeField] GameObject glass_break_effect;
    StaticObject static_object;
    SpriteRenderer sprite;
    private void Awake() {
        sprite = GetComponent<SpriteRenderer>();
        static_object = GetComponent<StaticObject>();
    }
    public void break_container(){
        sprite.sprite = broken;
        Instantiate(glass_break_effect , static_object.feet.position , Quaternion.identity);
    }
}
