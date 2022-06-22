using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI : MonoBehaviour
{
    public Character player;
    public ItemRegister item_database;
    private void Awake() {
        item_database = GetComponent<ItemRegister>();
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
    }
    private void Start() {
        backpack.gameObject.SetActive(false);
        equipment.gameObject.SetActive(false);
        interaction_menu.gameObject.SetActive(false);
        npc_backpack.gameObject.SetActive(false);
    }
    [SerializeField] Transform backpack , hotbar , equipment , npc_backpack;
    public int[] items_in_backpack = new int[46];
    public GameObject[] slots = new GameObject[46];
    public bool backpack_opened;
    public void toggle_backpack(){
        backpack.gameObject.SetActive(!backpack.gameObject.activeSelf);
        equipment.gameObject.SetActive(!equipment.gameObject.activeSelf);
        backpack_opened = backpack.gameObject.activeSelf;
    }
    public void add_item_to_backpack(int item_id){
        for(int i = 0; i < items_in_backpack.Length - 4; i++){
            if(items_in_backpack[i] == 0){
                items_in_backpack[i] = item_id;
                GameObject item_image = Instantiate(item_database.item_id_to_image(item_id) , slots[i].transform.position , Quaternion.identity ,  slots[i].transform);
                DragDrop item = item_image.GetComponent<DragDrop>();
                item.ui_canvas = GetComponent<Canvas>();
                item.current_in_slot_id = i;
                item.item_id = item_id;
                item.ui = this;
                return;
            }
        }
    }
    public bool backpack_is_full(){
        for(int i = 0; i < items_in_backpack.Length - 4;i++){
            if(items_in_backpack[i] != 0) return false;
        }
        return true;
    }
    public void use_hotbar_item(int slot_id){ // 1 = 20 , 2 = 21 , 3 = 22 , 4 = 23
        Transform slot = slots[slot_id].transform;
        if(slot.childCount == 0) return;
        slot.GetChild(0).GetComponent<DragDrop>().use();
    }
    [SerializeField] RectTransform interaction_menu; //each button buttom -18px
    [SerializeField] GameObject interaction_menu_button;
    public bool interaction_menu_opened = false;
    public void toggle_interaction_menu(NPC npc){
        interaction_menu.gameObject.SetActive(!interaction_menu.gameObject.activeSelf);
        interaction_menu_opened = interaction_menu.gameObject.activeSelf;
        if(!interaction_menu_opened) return;
        for(int i = 0; i < interaction_menu.childCount; i++){
            Destroy(interaction_menu.GetChild(i).gameObject);
        }
        for(int i = 0; i < npc.interact_methods.Length; i++){
            GameObject button_instanced = Instantiate(interaction_menu_button , interaction_menu.position , Quaternion.identity , interaction_menu);
            button_instanced.GetComponentInChildren<TextMeshProUGUI>().text = npc.interact_methods[i];
            InteractiomMenuButton button = button_instanced.GetComponent<InteractiomMenuButton>();
            button.ui = this;
            button.player = player.GetComponent<PlayerColtroller>();
            button.action_string = npc.interact_methods[i];
            button.npc = npc;
        }
        interaction_menu.offsetMin = new Vector2(interaction_menu.offsetMin.x , 186 - 18 * (npc.interact_methods.Length - 1));
    }
    public bool npc_backpack_opened;
    public NPC current_interacting_npc;
    public void toggle_NPC_backpack(NPC npc){
        npc_backpack.gameObject.SetActive(!npc_backpack.gameObject.activeSelf);
        npc_backpack_opened = npc_backpack.gameObject.activeSelf;
        if(!npc_backpack.gameObject.activeSelf) return;
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
        }
        current_interacting_npc = npc;
    }
}
