using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerColtroller : MonoBehaviour
{
<<<<<<< Updated upstream
    [SerializeField] Hitbox hitbox;
    // Start is called before the first frame update
=======
    /*
        manually input:
            ui
            camera controller
            target
    */
    Hitbox hitbox;
    private void Awake() {
        hitbox = GetComponentInChildren<Hitbox>();
        obstacle = LayerMask.GetMask("Obstacle");
        character = GetComponent<Character>();
    }
>>>>>>> Stashed changes
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
<<<<<<< Updated upstream
        character.target_position = cam.ScreenToWorldPoint(Input.mousePosition);
=======
        character.target_position = ui.camera_controller.cam.ScreenToWorldPoint(Input.mousePosition);
>>>>>>> Stashed changes
        search_interactable_object();
    }
    public void init(UI _ui){
        ui = _ui;
        ui.player = character;
    }
    Vector2 direction;
<<<<<<< Updated upstream
    public Character character;
    public Camera cam;
    bool inventory_opened = false;
    public UI ui;
    void player_input(){
=======
    Character character;
    [HideInInspector] public UI ui;
    public bool attack_locked;
    void player_input(){
        if(Input.GetKeyDown(KeyCode.Tab)){
            ui.toggle_backpack();
            if(ui.npc_backpack_opened){
                ui.toggle_NPC_backpack(null);
            }
            ui.camera_controller.is_dynamic = !ui.backpack_opened;
        }
        if(ui.backpack_opened || ui.npc_backpack_opened) return;
>>>>>>> Stashed changes
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
        else if(Input.GetKeyDown(KeyCode.I)){
            inventory_opened = ui.toggle_backpack();
        }
        if(Input.GetKeyDown(KeyCode.X) && nearest_interactable_object != null){
            if(inventory_opened) return;
            nearest_interactable_object.GetComponent<InteractableBox>().interacted(this);
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
        for(int i = 0; i < nearby_objects.Length; i++){
            if(nearby_objects[i].GetComponent<InteractableBox>() == null) continue;
            Vector2 vec = nearby_objects[i].transform.position - character.feet.position;
            RaycastHit2D hit = Physics2D.Raycast(character.feet.position , vec.normalized , vec.magnitude , obstacle);
            if(!hit || hit.collider.gameObject == nearby_objects[i].gameObject){
                dist.Add(vec.magnitude);
                dist_obj_dict[vec.magnitude] = nearby_objects[i].gameObject;
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
