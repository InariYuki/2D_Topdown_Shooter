using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI : MonoBehaviour
{
    [HideInInspector] public Character player;
    [HideInInspector] public PlayerColtroller player_ctl;
    public ItemScriptable Item_database;
    public Transform object_holder;
    public CameraController camera_controller;
    Canvas canvas;
    private void Awake() {
        canvas = GetComponent<Canvas>();
        for(int i = 0; i < 46; i++){
            if(i < 20){ // 0-19
                slots[i] = backpack.GetChild(i).gameObject;
            }
            else if(i < 24){ //20 - 23
                slots[i] = hotbar.GetChild(i - 20).gameObject;
            }
            else if(i < 26){ //24 - 25
                slots[i] = equipment.GetChild(i - 24).gameObject;
            }
            else{ // 26 - 45
                slots[i] = npc_backpack.GetChild(i - 26).gameObject;
            }
        }
        for(int i = 0 ; i < 46; i++){
            Slot slot = slots[i].GetComponent<Slot>();
            slot.slot_id = i;
        }
        for(int i = 0; i < 20; i++){
            ShopButton shop_button = shop.transform.GetChild(i).GetComponent<ShopButton>();
            shop_button.ui = this;
            shop_button.button_number = i;
        }
    }
    private void Start() {
        backpack.gameObject.SetActive(false);
        equipment.gameObject.SetActive(false);
        npc_backpack.gameObject.SetActive(false);
        money_display.text = player_money.ToString();
    }
    [SerializeField] GameObject player_computer;
    public void toggle_player_computer(){
        player_computer.SetActive(true);
        camera_controller.is_dynamic = false;
        player_ctl.all_control_locked = true;
    }
    public void close_player_computer(){
        player_computer.SetActive(false);
        camera_controller.is_dynamic = true;
        player_ctl.all_control_locked = false;
    }
    [SerializeField] GameObject pause_menu;
    public void toggle_pause_menu(){
        pause_menu.SetActive(true);
        camera_controller.is_dynamic = false;
        player_ctl.all_control_locked = true;
    }
    public void close_pause_menu(){
        pause_menu.SetActive(false);
        camera_controller.is_dynamic = true;
        player_ctl.all_control_locked = false;
    }
    [SerializeField] Transform backpack , hotbar , equipment , npc_backpack;
    [HideInInspector] public int[] items_in_backpack = new int[46];
    [HideInInspector] public List<int> keys_in_backpack = new List<int>();
    [HideInInspector] public GameObject[] slots = new GameObject[46];
    [HideInInspector] public bool backpack_opened;
    public void toggle_backpack(){
        backpack.gameObject.SetActive(!backpack.gameObject.activeSelf);
        equipment.gameObject.SetActive(!equipment.gameObject.activeSelf);
        backpack_opened = backpack.gameObject.activeSelf;
    }
    public void add_item_to_backpack(int item_id){
        for(int i = 0; i < 20; i++){
            if(items_in_backpack[i] == 0){
                items_in_backpack[i] = item_id;
                DragDrop item_image = Instantiate(Item_database.ui_image_template , slots[i].transform.position , Quaternion.identity ,  slots[i].transform);
                item_image.SetParameters(canvas , this , i , item_id , Item_database.items[item_id].ui_image , Item_database.items[item_id].is_weapon , Item_database.items[item_id].is_armor , Item_database.items[item_id].usable);
                return;
            }
        }
    }
    public bool backpack_is_full(){
        for(int i = 0; i < 20;i++){
            if(items_in_backpack[i] == 0) return false;
        }
        return true;
    }
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
            DragDrop item = Instantiate(Item_database.ui_image_template , slots[i+26].transform.position , Quaternion.identity , slots[i+26].transform).GetComponent<DragDrop>();
            item.SetParameters(canvas , this , i + 26 , npc.items_in_backpack[i] , Item_database.items[npc.items_in_backpack[i]].ui_image , Item_database.items[npc.items_in_backpack[i]].is_weapon , Item_database.items[npc.items_in_backpack[i]].is_armor , Item_database.items[npc.items_in_backpack[i]].usable);
            items_in_backpack[i + 26] = npc.items_in_backpack[i];
        }
        current_interacting_npc = npc;
    }
    [HideInInspector] public Stash current_interacting_stash;
    public void toggle_stash(Stash stash){
        npc_backpack.gameObject.SetActive(!npc_backpack.gameObject.activeSelf);
        npc_backpack_opened = npc_backpack.gameObject.activeSelf;
        if(!npc_backpack.gameObject.activeSelf){
            int[] override_backpack = new int[20];
            for(int i = 0; i < 20 ; i++){
                override_backpack[i] = items_in_backpack[i+26];
            }
            current_interacting_stash.items_in_backpack = override_backpack;
            return;
        }
        for(int i = 0 ; i < npc_backpack.childCount ; i++){
            if(slots[i+26].transform.childCount != 0) Destroy(slots[i+26].transform.GetChild(0).gameObject);
        }
        for(int i = 0; i < stash.items_in_backpack.Length ; i++){
            if(stash.items_in_backpack[i] == 0) continue;
            DragDrop item = Instantiate(Item_database.ui_image_template , slots[i+26].transform.position , Quaternion.identity , slots[i+26].transform).GetComponent<DragDrop>();
            item.SetParameters(canvas , this , i + 26 , stash.items_in_backpack[i] , Item_database.items[stash.items_in_backpack[i]].ui_image , Item_database.items[stash.items_in_backpack[i]].is_weapon , Item_database.items[stash.items_in_backpack[i]].is_armor , Item_database.items[stash.items_in_backpack[i]].usable);
            items_in_backpack[i + 26] = stash.items_in_backpack[i];
        }
        current_interacting_stash = stash;
    }
    [SerializeField] GameObject shop;
    [HideInInspector] public bool shop_opened = false;
    Stash current_shop;
    public void toggle_shop(Stash stash){
        current_shop = stash;
        for(int i = 0; i < 20; i++){
            Transform shop_slot = shop.transform.GetChild(i);
            ShopButton sell_slot = shop_slot.GetComponent<ShopButton>();
            sell_slot.set_price(0);
            if(shop_slot.childCount == 2) Destroy(shop_slot.GetChild(1).gameObject);
            if(stash.items_in_backpack[i] != 0){
                //Instantiate(item_database.item_id_to_image(stash.items_in_backpack[i]) , shop_slot.position , Quaternion.identity , shop.transform.GetChild(i)).GetComponent<CanvasGroup>().blocksRaycasts = false;
                DragDrop item_on_shelf = Instantiate(Item_database.ui_image_template, shop_slot.position , Quaternion.identity , shop.transform.GetChild(i));
                item_on_shelf.GetComponent<Image>().sprite = Item_database.items[stash.items_in_backpack[i]].ui_image;
                item_on_shelf.GetComponent<CanvasGroup>().blocksRaycasts = false;
                sell_slot.set_price(Item_database.items[stash.items_in_backpack[i]].price);
            }
        }
        shop.SetActive(true);
        toggle_backpack();
        shop_opened = true;
        camera_controller.is_dynamic = false;
    }
    public void close_shop(){
        shop.SetActive(false);
        shop_opened = false;
        camera_controller.is_dynamic = true;
    }
    [SerializeField] TextMeshProUGUI money_display;
    int player_money = 500;
    public void item_baught(int button_number){
        if(backpack_is_full()) return;
        if(player_money < Item_database.items[current_shop.items_in_backpack[button_number]].price){
            player_ctl.player_talk("I don't have enough money :(");
            return;
        }
        add_item_to_backpack(current_shop.items_in_backpack[button_number]);
        player_money -= Item_database.items[current_shop.items_in_backpack[button_number]].price;
        money_display.text = player_money.ToString();
        current_shop.remove_item(button_number);
        Destroy(shop.transform.GetChild(button_number).GetChild(1).gameObject);
    }
    [HideInInspector] public int hint_count = 0;
    [SerializeField] GameObject hint_panel;
    public void add_hint_msg(string message){
        if(hint_count > 2){
            //can't take any mission
            return;
        }
        if(!hint_panel.activeSelf){
            hint_panel.SetActive(true);
        }
        for(int i = 0; i < 3; i++){
            TextMeshProUGUI mission = hint_panel.transform.GetChild(i).GetComponent<TextMeshProUGUI>();
            if(mission.text == ""){
                mission.text = message;
                hint_count++;
                break;
            }
        }
    }
    public void remove_hint_message(string message){
        for(int i = 0; i < 3; i++){
            TextMeshProUGUI mission = hint_panel.transform.GetChild(i).GetComponent<TextMeshProUGUI>();
            if(mission.text == message){
                mission.text = "";
                hint_count--;
                break;
            }
        }
        if(hint_count == 0){
            hint_panel.SetActive(false);
        }
    }
    public void clear_hint_text(){
        for(int i = 0; i < 3; i++){
            TextMeshProUGUI mission = hint_panel.transform.GetChild(i).GetComponent<TextMeshProUGUI>();
            mission.text = "";
        }
        hint_panel.SetActive(false);
    }
    public void move_player_to_position(Vector2 pos){
        player.transform.position = pos;
        player_ctl.all_control_locked = false;
        camera_controller.target = player.transform;
        camera_controller.is_dynamic = true;
    }
    public void set_player_sprite(int head_index , int body_index , int limb_index){
        player.GetComponent<SpriteDatabase>().set_sprite(head_index , body_index , limb_index);
        player.set_carcass();
        player.sprite_init();
    }
    [SerializeField] IntroMission intro_scene;
    [SerializeField] GameObject main_menu;
    public void new_game_button_pressed(){
        IntroMission intro_scene_instanced = Instantiate(intro_scene , new Vector3(0 , 0 , 10f) , Quaternion.identity);
        intro_scene_instanced.mission_start(this);
        player_ctl.all_control_locked = true;
        camera_controller.target = intro_scene_instanced.player_container.transform;
        camera_controller.is_dynamic = false;
        main_menu.SetActive(false);
    }
    [SerializeField] GameObject player_home;
    public void load_game_button_pressed(){
        player_home.SetActive(true);
        move_player_to_position(new Vector2(1.7f , 0.5f));
        main_menu.SetActive(false);
    }
    [SerializeField] MissionMap mission_map;
    public void city_sector_button_pressed(){
        mission_map.generate_city_sector(4 , 4);
        player_home.SetActive(false);
        close_player_computer();
        move_player_to_position(new Vector2(Random.Range(1 , 3) , Random.Range(1 , 3)) * 6.4f);
    }
    [SerializeField] GameObject travel_terminal;
    public void ToggleTravelTerminal(){
        travel_terminal.SetActive(true);
        camera_controller.is_dynamic = false;
        player_ctl.all_control_locked = true;
    }
    public void CloseTravelTerminal(){
        travel_terminal.SetActive(false);
        camera_controller.is_dynamic = true;
        player_ctl.all_control_locked = false;
    }
    public void ReturnHome(){
        CloseTravelTerminal();
        mission_map.RemoveCitySector();
        ClearItemsOnGround();
        player_home.SetActive(true);
        move_player_to_position(new Vector2(1.7f , 0.5f));
    }
    void ClearItemsOnGround(){
        for(int i = 0 ; i < object_holder.childCount; i++){
            Destroy(object_holder.GetChild(i).gameObject);
        }
    }
}
