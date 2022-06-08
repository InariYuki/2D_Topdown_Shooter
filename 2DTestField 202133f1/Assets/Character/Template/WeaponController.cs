using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public Character parent;
    public SpriteRenderer weapon_sprite_renderer;
    public int state = 0; //0 down 1 up
    // Start is called before the first frame update
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
        check_state_value();
    }
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
