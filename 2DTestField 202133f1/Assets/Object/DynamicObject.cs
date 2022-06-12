using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicObject : MonoBehaviour
{
    [SerializeField] SpriteRenderer sprite;
    [SerializeField] Transform feet;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        sprite.sortingOrder = (int)(-feet.position.y * 100);
    }
}
