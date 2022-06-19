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
        character.speed = character.top_speed;
    }

    // Update is called once per frame
    void Update()
    {
        player_input();
    }
    private void FixedUpdate() {
        character.target_position = cam.ScreenToWorldPoint(Input.mousePosition);
        search_interactable_object();
    }
    Vector2 direction;
    public Character character;
    public Camera cam;
    bool inventory_opened = false;
    public UI ui;
    void player_input(){
        direction.x = Input.GetAxisRaw("Horizontal");
        direction.y = Input.GetAxisRaw("Vertical");
        character.direction = direction.normalized;
        if(Input.GetKeyDown(KeyCode.Mouse0)){
            if(inventory_opened) return;
            character.normal_attack();
        }
        else if(Input.GetKeyDown(KeyCode.Mouse1)){
            if(inventory_opened) return;
            character.special_attack();
        }
        else if(Input.GetKeyDown(KeyCode.X) && nearest_interactable_object != null){
            if(inventory_opened) return;
            nearest_interactable_object.GetComponent<InteractableBox>().interacted(this);
        }
        else if(Input.GetKeyDown(KeyCode.I)){
            inventory_opened = ui.toggle_backpack();
        }
    }
    public void hit(int damage , GameObject attacker){
        character.velocity = (transform.position - attacker.transform.position).normalized * 5f;
    }
    [SerializeField] LayerMask obstacle;
    GameObject nearest_interactable_object = null;
    void search_interactable_object(){
        Collider2D[] nearby_objects = Physics2D.OverlapCircleAll(character.feet.position , 0.1f);
        Dictionary<float , GameObject> dist_obj_dict = new Dictionary<float, GameObject>();
        List<float> dist = new List<float>();
        foreach(Collider2D obj in nearby_objects){
            if(obj.GetComponent<InteractableBox>() == null) continue;
            Vector2 vec = obj.transform.position - character.feet.position;
            RaycastHit2D hit = Physics2D.Raycast(character.feet.position , vec.normalized , vec.magnitude , obstacle);
            if(!hit || hit.collider.gameObject == obj.gameObject){
                dist.Add(vec.magnitude);
                dist_obj_dict[vec.magnitude] = obj.gameObject;
            }
        }
        if(dist.Count == 0){
            nearest_interactable_object = null;
            return;
        }
        dist.Sort();
        nearest_interactable_object = dist_obj_dict[dist[0]];
    }
}
