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
                ui.move_player_to_position(new Vector2(1.76f , 0.48f));
                stage_one();
            }
        }
    }
    private void FixedUpdate() {
        hintzone_glass();
    }
    bool can_trigger = true;
    void hintzone_glass(){
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
    bool container_broken = false;
    public void mission_start(UI _ui){
        ui = _ui;
    }
    bool stage_one_start = false;
    void stage_one(){
        if(stage_one_start) return;
        stage_one_start = true;
        hint.text = "Use WASD to move\nleft mouse button to attack.";
        StartCoroutine(close_hint());
    }
    IEnumerator close_hint(){
        yield return new WaitForSeconds(3f);
        hint.text = "";
        hint.transform.position = new Vector2(224f , 128f);
    }
}
