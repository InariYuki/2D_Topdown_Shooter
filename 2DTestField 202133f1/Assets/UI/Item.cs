using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    Rigidbody2D body;
    SpriteRenderer sprite_renderer;
    Vector3 velocity;
    private void Awake() {
        body = GetComponent<Rigidbody2D>();
        sprite_renderer = GetComponent<SpriteRenderer>();
    }
    private void FixedUpdate() {
        velocity = Vector3.Lerp(velocity , Vector3.zero , 0.1f);
        body.MovePosition(transform.position + velocity * 0.02f);
    }
    int item_id = 0;
    public void SetParameters(int _item_id , Sprite _sprite){
        item_id = _item_id;
        sprite_renderer.sprite = _sprite;
    }
    public void interacted(PlayerColtroller player , int interact_state){
        if(interact_state == 1) return;
        player.ui.add_item_to_backpack(item_id);
        if(!player.ui.backpack_is_full()) Destroy(gameObject);
    }
    public void BurstMoveAway(){
        velocity = new Vector3(Random.Range(-1f , 1f) , Random.Range(-1f , 1f)) * 2f;
    }
}
