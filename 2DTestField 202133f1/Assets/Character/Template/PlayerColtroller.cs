using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerColtroller : MonoBehaviour
{
    [SerializeField] Hitbox hitbox;
    // Start is called before the first frame update
    void Start()
    {
        initial_parameters();
    }
    void initial_parameters(){

    }

    // Update is called once per frame
    void Update()
    {
        player_input();
    }
    Vector2 direction;
    public Character character;
    public Camera cam;
    void player_input(){
        direction.x = Input.GetAxisRaw("Horizontal");
        direction.y = Input.GetAxisRaw("Vertical");
        character.direction = direction.normalized;
        if(Input.GetKeyDown(KeyCode.Mouse0)){
            character.normal_attack();
        }
        else if(Input.GetKeyDown(KeyCode.Mouse1)){
            character.special_attack();
        }
        character.target_position = cam.ScreenToWorldPoint(Input.mousePosition);
    }
    public void hit(int damage , GameObject attacker){
        character.velocity = (transform.position - attacker.transform.position).normalized * 5f;
    }
}
