using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroMission : MonoBehaviour
{
    public Transform spawn_point;
    UI ui;
    private void Update() {
        if(Input.GetKeyDown(KeyCode.X)){
            break_out_container();
        }
    }
    public void init(UI _ui){
        ui = _ui;
    }
    bool break_out = false;
    void break_out_container(){
        if(break_out) return;
        break_out = true;
        spawn_point.GetComponent<BioContainer>().destroy();
        ui.spawn_player(spawn_point.position + Vector3.down * 0.1f);
    }
}
