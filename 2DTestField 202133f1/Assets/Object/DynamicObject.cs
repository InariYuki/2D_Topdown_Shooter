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
        sprite.sortingOrder = Mathf.RoundToInt(-feet.position.y * 100);
    }
}
