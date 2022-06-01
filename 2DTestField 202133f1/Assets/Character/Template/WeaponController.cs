using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public Character parent;
    SpriteRenderer weapon_sprite_renderer;
    public int state = 0; //0 down 1 up
    // Start is called before the first frame update
    void Start()
    {

    }
    public void init(){
        weapon_sprite_renderer = GetComponentInChildren<SpriteRenderer>();
    }
    // Update is called once per frame
    void Update()
    {
        check_state_value();
    }
    public Transform feet;
    void check_state_value(){
        if(weapon_sprite_renderer == null) return;
        switch(state){
            case 0:
                weapon_sprite_renderer.sortingOrder = parent.order - 4;
                weapon_sprite_renderer.flipX = false;
                break;
            case 1:
                weapon_sprite_renderer.sortingOrder = parent.order + 4;
                weapon_sprite_renderer.flipX = true;
                break;
        }
    }
}
