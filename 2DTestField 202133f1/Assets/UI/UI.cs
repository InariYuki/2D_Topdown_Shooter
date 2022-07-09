using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
<<<<<<< Updated upstream
    ItemRegister item_database;
=======
    public CameraController camera_controller;
    [HideInInspector] public Character player;
    [HideInInspector] public ItemRegister item_database;
>>>>>>> Stashed changes
    private void Awake() {
        backpack_script = backpack.GetComponent<Backpack>();
        item_database = GetComponent<ItemRegister>();
    }
    private void Start() {
        for(int i = 0 ; i < backpack_script.slots.Length; i++) backpack_script.slots[i].GetComponent<Slot>().ui = this;
    }
<<<<<<< Updated upstream
    [SerializeField] GameObject backpack;
    Backpack backpack_script;
    public int[] items_in_backpack = new int[20];
    public bool toggle_backpack(){
        if(backpack.activeSelf){
            backpack.SetActive(false);
            return false;
        }
        else{
            backpack.SetActive(true);
            return true;
        }
=======
    [SerializeField] Transform backpack , hotbar , equipment , npc_backpack;
    [HideInInspector] public int[] items_in_backpack = new int[46];
    [HideInInspector] public GameObject[] slots = new GameObject[46];
    [HideInInspector] public bool backpack_opened;
    public void toggle_backpack(){
        backpack.gameObject.SetActive(!backpack.gameObject.activeSelf);
        equipment.gameObject.SetActive(!equipment.gameObject.activeSelf);
        backpack_opened = backpack.gameObject.activeSelf;
>>>>>>> Stashed changes
    }
    public void add_item_to_backpack(int item_id){
        for(int i = 0; i < items_in_backpack.Length; i++){
            if(items_in_backpack[i] == 0){
                print("1");
                items_in_backpack[i] = item_id;
                GameObject item_image = Instantiate(item_database.item_id_to_image(item_id) , backpack_script.slots[i].transform.position , Quaternion.identity ,  backpack_script.slots[i].transform);
                backpack_script.slots[i] = item_image;
                item_image.GetComponent<DragDrop>().ui = GetComponent<Canvas>();
                return;
            }
        }
    }
    public bool backpack_is_full(){
        for(int i = 0; i < items_in_backpack.Length;i++){
            if(items_in_backpack[i] != 0) return false;
        }
        return true;
    }
<<<<<<< Updated upstream
=======
    public void use_hotbar_item(int slot_id){ // 1 = 20 , 2 = 21 , 3 = 22 , 4 = 23
        Transform slot = slots[slot_id].transform;
        if(slot.childCount == 0) return;
        slot.GetChild(0).GetComponent<DragDrop>().use();
    }
    [HideInInspector] public bool npc_backpack_opened;
    [HideInInspector] public NPC current_interacting_npc;
    public void toggle_NPC_backpack(NPC npc){
        npc_backpack.gameObject.SetActive(!npc_backpack.gameObject.activeSelf);
        npc_backpack_opened = npc_backpack.gameObject.activeSelf;
        if(!npc_backpack.gameObject.activeSelf){
            int[] override_backpack = new int[20];
            for(int i = 0; i < 20 ; i++){
                override_backpack[i] = items_in_backpack[i+26];
            }
            current_interacting_npc.items_in_backpack = override_backpack;
            return;
        }
        for(int i = 0 ; i < npc_backpack.childCount ; i++){
            if(slots[i+26].transform.childCount != 0) Destroy(slots[i+26].transform.GetChild(0).gameObject);
        }
        for(int i = 0; i < npc.items_in_backpack.Length ; i++){
            if(npc.items_in_backpack[i] == 0) continue;
            DragDrop item = Instantiate(item_database.item_id_to_image(npc.items_in_backpack[i]) , slots[i+26].transform.position , Quaternion.identity , slots[i+26].transform).GetComponent<DragDrop>();
            item.ui_canvas = GetComponent<Canvas>();
            item.current_in_slot_id = i + 26;
            item.item_id = npc.items_in_backpack[i];
            item.ui = this;
            item.ui.items_in_backpack[i + 26] = npc.items_in_backpack[i];
        }
        current_interacting_npc = npc;
    }
    [SerializeField] GameObject[] sectors = new GameObject[0];
    void generate_map(int map_width , int map_height)
    {
        for (int i = 0; i < map_height; i++)
        {
            for (int j = 0; j < map_width; j++)
            {
                Instantiate(sectors[Random.Range(0 , sectors.Length)] , new Vector2(i , j) * 6.4f , Quaternion.identity);
            }
        }
    }
    [SerializeField] GameObject player_obj;
    public void spawn_player(Vector2 pos){
        PlayerColtroller player_instanced = Instantiate(player_obj , pos , Quaternion.identity).GetComponent<PlayerColtroller>();
        player_instanced.init(this);
        camera_controller.target = player.transform;
        camera_controller.is_dynamic = true;
    }
    [SerializeField] IntroMission intro_map;
    [SerializeField] GameObject main_menu;
    public void new_game_button_pressed(){
        main_menu.SetActive(false);
        IntroMission intro_mission = Instantiate(intro_map , Vector3.zero , Quaternion.identity);
        intro_mission.init(this);
        camera_controller.target = intro_mission.spawn_point;
    }
    public void exit_button_pressed(){

    }
>>>>>>> Stashed changes
}
