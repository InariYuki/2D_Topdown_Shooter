using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticObject : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        sprite_ctrl();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    [SerializeField] SpriteRenderer horizontal , vertical;
    [SerializeField] Transform feet;
    void sprite_ctrl(){
        horizontal.sortingOrder = (int)(-feet.position.y * 100);
        vertical.sortingOrder = (int)(-feet.position.y * 100);
    }
}
