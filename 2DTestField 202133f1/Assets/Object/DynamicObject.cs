using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicObject : MonoBehaviour
{
    SpriteRenderer sprite;
    [SerializeField] Transform feet;
    void Awake(){
        sprite = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        sprite.sortingOrder = (int)(-feet.position.y * 100);
    }
}
