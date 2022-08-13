using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectBox : MonoBehaviour
{
    UI ui;
    [HideInInspector] public int interact_object_type = 0; //0 = computer , 1 = NPC
    Computer on_hover_computer;
    Computer current_computer;
    NPC on_hover_NPC;
    NPC current_npc;
    LayerMask obstacle_layer;
    private void Awake() {
        ui = GetComponent<UI>();
        obstacle_layer = LayerMask.GetMask("Obstacle");
    }
    private void FixedUpdate() {
        SelectModeOn();
    }
    public void SelectModeOn(){
        Vector3 mouse_position = ui.camera_controller.cam.ScreenToWorldPoint(Input.mousePosition);
        Collider2D[] interactables_in_range = Physics2D.OverlapCircleAll(mouse_position , 0.3f , obstacle_layer);
        List<float> distances = new List<float>();
        Dictionary<float , Computer> dist_to_com_dict = new Dictionary<float, Computer>();
        for(int i = 0; i < interactables_in_range.Length; i++){
            if(interactables_in_range[i].GetComponent<InteractableBox>() == null) continue;
            if(interact_object_type == 0){
                Computer computer = interactables_in_range[i].GetComponent<Computer>();
                if(computer != null){
                    float dist = (computer.transform.position - mouse_position).magnitude;
                    distances.Add(dist);
                    dist_to_com_dict[dist] = computer;
                }
            }
            else if(interact_object_type == 1){
                NPC npc = interactables_in_range[i].GetComponent<NPC>();
                if(npc != null){

                }
            }
        }
        if(distances.Count == 0){
            on_hover_computer = null;
            on_hover_NPC = null;
            return;
        }
        distances.Sort();
        if(interact_object_type == 0){
            on_hover_computer = dist_to_com_dict[distances[0]];
        }
        else if(interact_object_type == 1){

        }
    }
    public void SelectObject(){
        if(on_hover_computer != null){
            current_computer = on_hover_computer;
            current_computer.GetComponent<InteractableBox>().interacted(ui.player_ctl , 0);
        }
        else if(on_hover_NPC != null){
            print("NPC!");
        }
        else{
            if(current_computer != null){
                current_computer.GetComponent<InteractableBox>().interacted(ui.player_ctl , 1);
                current_computer = null;
            }
            ui.player_ctl.control_state = 0;
            enabled = false;
        }
    }
}
