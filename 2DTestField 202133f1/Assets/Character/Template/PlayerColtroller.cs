using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerColtroller : MonoBehaviour
{
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
        ui.player = character;
    }
    void Start()
    {
        initial_parameters();
    }
    void initial_parameters(){
        character.speed = character.top_speed;
    }
    void Update()
    {
        player_input();
    }
    private void FixedUpdate() {
        character.target_position = camera_controller.cam.ScreenToWorldPoint(Input.mousePosition);
        search_interactable_object();
    }
    Vector2 direction;
    Character character;
    public CameraController camera_controller;
    public UI ui;
    public bool attack_locked;
    void player_input(){
        if(Input.GetKeyDown(KeyCode.Tab)){
            ui.toggle_backpack();
            if(ui.npc_backpack_opened){
                ui.toggle_NPC_backpack(null);
            }
            camera_controller.is_dynamic = !ui.backpack_opened;
        }
        if(ui.backpack_opened || ui.npc_backpack_opened) return;
        direction.x = Input.GetAxisRaw("Horizontal");
        direction.y = Input.GetAxisRaw("Vertical");
        character.direction = direction.normalized;
        if(Input.GetKeyDown(KeyCode.Mouse0)){
            if(attack_locked) return;
            character.normal_attack();
        }
        else if(Input.GetKeyDown(KeyCode.Mouse1)){
            if(attack_locked) return;
            character.special_attack();
        }
        if(Input.GetKeyDown(KeyCode.X) && nearest_interactable_object != null){
            nearest_interactable_object.GetComponent<InteractableBox>().interacted(this , 0);
        }
        if(Input.GetKeyDown(KeyCode.Alpha1)){
            ui.use_hotbar_item(20);
        } 
        else if(Input.GetKeyDown(KeyCode.Alpha2)){
            ui.use_hotbar_item(21);
        } 
        else if(Input.GetKeyDown(KeyCode.Alpha3)){
            ui.use_hotbar_item(22);
        } 
        else if(Input.GetKeyDown(KeyCode.Alpha4)){
            ui.use_hotbar_item(23);
        }
    }
    public void hit(int damage , GameObject attacker){
        character.velocity = (transform.position - attacker.transform.position).normalized * 5f;
    }
    LayerMask obstacle;
    GameObject nearest_interactable_object = null;
    void search_interactable_object(){
        Collider2D[] nearby_objects = Physics2D.OverlapCircleAll(character.feet.position , 0.1f);
        Dictionary<float , GameObject> dist_obj_dict = new Dictionary<float, GameObject>();
        List<float> dist = new List<float>();
        for(int i = 0 ; i < nearby_objects.Length ; i++){
            if(nearby_objects[i].GetComponent<InteractableBox>() == null) continue;
            Vector2 vec = nearby_objects[i].transform.position - character.feet.position;
            RaycastHit2D hit = Physics2D.Raycast(character.feet.position , vec.normalized , vec.magnitude , obstacle);
            if(!hit || hit.collider.gameObject == nearby_objects[i].gameObject){
                dist.Add(vec.magnitude);
                dist_obj_dict[vec.magnitude] = nearby_objects[i].gameObject;
            }
        }
        if(dist.Count == 0){
            if(nearest_interactable_object != null){
                nearest_interactable_object.GetComponent<InteractableBox>().interacted(this , 1);
                nearest_interactable_object = null;
            }
            return;
        }
        dist.Sort();
        nearest_interactable_object = dist_obj_dict[dist[0]];
    }
    [SerializeField] TextMeshProUGUI dialogue;
    public void player_talk(string talk){
        StopCoroutine(talk_finished());
        dialogue.gameObject.SetActive(true);
        dialogue.text = talk;
        StartCoroutine(talk_finished());
    }
    IEnumerator talk_finished(){
        yield return new WaitForSeconds(2f);
        dialogue.gameObject.SetActive(false);
    }
}
