using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticObject : MonoBehaviour
{
<<<<<<< Updated upstream
    [SerializeField] SpriteRenderer sprite_renderer;
    [SerializeField] Transform feet;
=======
    SpriteRenderer sprite_renderer;
    [HideInInspector] public Transform feet;
>>>>>>> Stashed changes
    private void Awake() {
        sprite_renderer = GetComponent<SpriteRenderer>();
        feet = transform.GetChild(0);
    }
    void Start(){
        sprite_renderer.sortingOrder = (int)(-feet.position.y * 100);
    }
}
