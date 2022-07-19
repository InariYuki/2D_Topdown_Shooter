using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IntroMission : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI hint;
    public BioContainer player_container;
    UI ui;
    private void Update() {
        if(Input.GetKeyDown(KeyCode.X)){
            if(! container_broken){
                container_broken = true;
                player_container.break_container();
                ui.set_player_sprite(1 , 2 , 0);
                ui.move_player_to_position(new Vector2(1.76f , 0.48f));
                stage_one();
            }
        }
    }
    private void FixedUpdate() {
        hintzone_glass();
        hint_zone_assassination();
        if(watcher.dead){
            stage_two();
        }
        stage_two_check();
        stage_three();
        escape_zone_check();
    }
    bool can_trigger = true;
    [SerializeField] BreakableObject glass_1 , glass_2;
    void hintzone_glass(){
        if(glass_1.broken || glass_2.broken){
            return;
        }
        Collider2D[] hits = Physics2D.OverlapBoxAll(new Vector2(1.96f , 1.86f) , Vector2.one * 0.5f , 0);
        if(hits.Length == 0) return;
        List<Collider2D> hits_list = new List<Collider2D>();
        hits_list.AddRange(hits);
        if(hits_list.Contains(ui.player.collision)){
            if(can_trigger){
                can_trigger = false;
                ui.player_ctl.player_talk("This looks fragile.");
            }
        }
        else{
            can_trigger = true;
        }
    }
    [SerializeField] Character watcher;
    bool assassination_hint_can_trigger = true;
    void hint_zone_assassination(){
        if(watcher.dead) return;
        Collider2D[] hits = Physics2D.OverlapBoxAll(new Vector2(3f , 1.83f) , Vector2.one * 0.5f , 0);
        if(hits.Length == 0) return;
        List<Collider2D> hits_list = new List<Collider2D>();
        hits_list.AddRange(hits);
        if(hits_list.Contains(ui.player.collision)){
            if(assassination_hint_can_trigger){
                assassination_hint_can_trigger = false;
                ui.player_ctl.player_talk("Seems like he can't hear me. Maybe I can walk behind him and....press X");
            }
        }
        else{
            assassination_hint_can_trigger = true;
        }
    }
    bool container_broken = false;
    public void mission_start(UI _ui){
        ui = _ui;
    }
    bool stage_one_start = false;
    string mission_msg = "Escape the facility by any means!";
    void stage_one(){
        if(stage_one_start) return;
        stage_one_start = true;
        hint.text = "Use WASD to move\nleft mouse button to attack.";
        ui.add_hint_msg(mission_msg);
        StartCoroutine(close_hint());
    }
    bool stage_two_start = false;
    string mission_msg2 = "(Optional) Search the closet nearby";
    string mission_msg3 = "Use the computer to unlock the door";
    [SerializeField] Stash mission_closet;
    void stage_two(){
        if(stage_two_start) return;
        stage_two_start = true;
        ui.add_hint_msg(mission_msg2);
    }
    bool stage_two_finished = false;
    void stage_two_check(){
        if(ui.current_interacting_stash == mission_closet && stage_two_start && !stage_two_finished){
            stage_two_finished = true;
            ui.remove_hint_message(mission_msg2);
            ui.add_hint_msg(mission_msg3);
        }
    }
    [SerializeField] Door mission_door;
    bool stage_three_finished = false;
    void stage_three(){
        if(!mission_door.door_locked && !stage_three_finished && stage_two_finished){
            ui.remove_hint_message(mission_msg3);
            stage_three_finished = true;
        }
    }
    void escape_zone_check(){
        Collider2D[] hits = Physics2D.OverlapBoxAll(new Vector2(13f , 4.9f) , Vector2.one * 0.5f , 0);
        if(hits.Length == 0) return;
        List<Collider2D> hits_list = new List<Collider2D>();
        hits_list.AddRange(hits);
        if(hits_list.Contains(ui.player.collision)){
            ui.clear_hint_text();
            Destroy(gameObject);
        }
    }
    IEnumerator close_hint(){
        yield return new WaitForSeconds(3f);
        hint.text = "";
        hint.transform.position = new Vector2(224f , 128f);
    }
}
