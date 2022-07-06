using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtificialIntelligence : MonoBehaviour
{
    /*summary
        manually input:
            max_health
            stop_count
            stop_diration
            idle
            static_patrol
            is_elite
    */
    Hitbox hitbox;
    public int max_health = 100;
    int health;
    Character parent;
    private void Awake() {
        hitbox = GetComponentInChildren<Hitbox>();
        parent = GetComponent<Character>();
        default_layer = LayerMask.GetMask("Default");
        attack_layer = LayerMask.GetMask("Attack");
        Navbox = LayerMask.GetMask("Navigation");
        Obstacle = LayerMask.GetMask("Obstacle");
    }
    void Start()
    {
        initial_parameters();
        free_roam_init();
    }
    void initial_parameters(){
        health = max_health;
        before_search_position = find_nearest_navbox(parent.feet.position).transform.position;
    }
    NavBox start_navbox , end_navbox;
    List<Vector3> find_path(Vector3 pos){
        start_navbox = find_nearest_navbox(pos);
        end_navbox = find_nearest_navbox(parent.feet.position);
        if(start_navbox == null || end_navbox == null){
            Debug.Log("Destination or start position is not set");
            return null;
        }
        NavBox current_navbox = start_navbox;
        List<NavBox> unpicked_navboxes = new List<NavBox>();
        List<NavBox> picked_navboxes = new List<NavBox>();
        Dictionary<NavBox , NavBox> box_parent_dict = new Dictionary<NavBox, NavBox>();
        picked_navboxes.Add(current_navbox);
        box_parent_dict[current_navbox] = current_navbox;
        while(current_navbox != end_navbox){
            Dictionary<float , NavBox> cost_box_dict = new Dictionary<float, NavBox>();
            List<float> cost = new List<float>();
            for(int i = 0; i < current_navbox.next_hops.Count; i++){
                if(! unpicked_navboxes.Contains(current_navbox.next_hops[i]) && ! picked_navboxes.Contains(current_navbox.next_hops[i])){
                    unpicked_navboxes.Add(current_navbox.next_hops[i]);
                }
            }
            for(int i = 0; i < unpicked_navboxes.Count; i++){
                if(! box_parent_dict.ContainsKey(unpicked_navboxes[i])){
                    box_parent_dict[unpicked_navboxes[i]] = current_navbox;
                }
                float box_cost = (unpicked_navboxes[i].transform.position - transform.position).magnitude + (unpicked_navboxes[i].transform.position - end_navbox.transform.position).magnitude;
                cost_box_dict[box_cost] = unpicked_navboxes[i];
                cost.Add(box_cost);
            }
            if(cost.Count == 0){
                print("no path found now exit");
                return new List<Vector3>();
            }
            cost.Sort();
            current_navbox = cost_box_dict[cost[0]];
            unpicked_navboxes.Remove(cost_box_dict[cost[0]]);
            picked_navboxes.Add(current_navbox);
        }
        List<NavBox> box_path = new List<NavBox>();
        for(int i = 0; i < 2; i++){
            box_path.Add(current_navbox);
            current_navbox = box_parent_dict[current_navbox];
        }
        List<Vector3> path = new List<Vector3>();
        for(int i = 0; i < box_path.Count; i++){
            path.Add(box_path[i].transform.position);
        }
        path.Add(pos);
        return path;
    }
    public int ai_state = 0; //0 = idle , 1 = attack , 2 = search
    void FixedUpdate()
    {
        if (parent.dead) return;
        switch(ai_state){
            case 0:
                free_roam();
                break;
            case 1:
                attack_mode();
                break;
            case 2:
                search_mode();
                break;
            default:
                break;
        }
    }
    float sight_distance = 2f , view_angle = 80;
    LayerMask default_layer;
    void sight(){
        Collider2D[] chatacter_colliders = Physics2D.OverlapCircleAll(transform.position , sight_distance , default_layer);
        for(int i = 0; i < chatacter_colliders.Length; i++){
            Vector2 vec = chatacter_colliders[i].transform.position - transform.position;
            if(! Physics2D.Raycast(parent.feet.position , vec.normalized , vec.magnitude , Obstacle) && Vector3.Angle(vec.normalized , parent.facing_direction) <= view_angle){
                Debug.DrawLine(parent.feet.position , chatacter_colliders[i].transform.position , Color.cyan);
                if(enemies.Contains(chatacter_colliders[i].gameObject)){
                    attack_mode_init(chatacter_colliders[i].gameObject);
                }
            }
        }
    }
    NavBox previous = null , current = null;
    bool rest = false , trigger = false;
    public int stop_count = 3;
    public float stop_duration = 1f;
    public bool idle = false , static_patrol = false;
    int step_count = 0 , free_roam_substate = 0; //static patrol NPC is 1 , non static is 0
    void free_roam_init(){
        StopAllCoroutines();
        previous = null;
        current = null;
        rest = false;
        trigger = false;
        step_count = 0;
        if(parent.ranged_weapon != null && parent.ranged_weapon.drawed) parent.special_attack();
        ai_state = 0;
        if(static_patrol){
            free_roam_substate = 1;
        }
        else{
            free_roam_substate = 0;
        }
    }
    void free_roam(){
        sight();
        switch(free_roam_substate){
            case 0:
                if(idle) return;
                if(current == null) current = find_nearest_navbox(parent.feet.position);
                parent.speed = parent.top_speed / 3;
                if((current.transform.position - transform.position).magnitude > 0.1f){
                    parent.direction = current.transform.position - transform.position;
                }
                else{
                    if(rest){
                        parent.direction = Vector2.zero;
                        previous = null;
                        if(trigger == false){
                            trigger = true;
                            StartCoroutine(rest_for_a_while(stop_duration));
                        }
                        return;
                    }
                    before_search_position = current.transform.position;
                    List<NavBox> nexts = new List<NavBox>();
                    for(int i = 0; i < current.next_hops.Count; i++){
                        nexts.Add(current.next_hops[i]);
                    }
                    if(nexts.Count > 1) nexts.Remove(previous);
                    previous = current;
                    current = nexts[Random.Range(0 , nexts.Count)];
                    if(stop_count != 0) step_count++;
                    if(step_count == stop_count && stop_count != 0){
                        rest = true;
                        step_count = 0;
                    }
                }
                break;
            case 1:
                Vector3 vec = before_search_position - transform.position;
                if(vec.magnitude > 0.1f){
                    parent.speed = parent.top_speed;
                    go_to(before_search_position);
                }
                else{
                    parent.direction = Vector3.zero;
                    parent.speed = parent.top_speed / 3;
                    free_roam_substate = 0;
                }
                break;
        }
    }
    IEnumerator rest_for_a_while(float time){
        yield return new WaitForSeconds(time);
        rest = false;
        trigger = false;
    }
    GameObject current_enemy = null;
    Character current_enemy_character = null;
    int attack_mode_substate = 0; //0 = aggresive , 1 = retreat
    bool attack_mode_substate_decided = false , attack_mode_substate_timer_start = false;
    [SerializeField] bool is_elite;
    void attack_mode_init(GameObject attacker){
        StopAllCoroutines();
        attack_mode_substate = 0;
        attack_mode_substate_decided = false;
        attack_mode_substate_timer_start = false;
        current_enemy = attacker;
        current_enemy_character = current_enemy.GetComponent<Character>();
        if(parent.ranged_weapon != null && parent.ranged_weapon.drawed == false){
            parent.special_attack();
        }
        ai_state = 1;
    }
    void attack_mode(){
        if(current_enemy == null || current_enemy_character.dead){
            current_enemy = null;
            current_enemy_character = null;
            free_roam_init();
            return;
        }
        Vector2 vec = current_enemy.transform.position - transform.position;
        if(Physics2D.Raycast(parent.feet.position , vec.normalized , vec.magnitude , Obstacle)){
            search_mode_init(current_enemy_character.feet.transform.position);
            return;
        }
        if(attack_mode_substate_decided == false){
            attack_mode_substate_decided = true;
            attack_mode_substate = Random.Range(0 , 2);
            if(attack_mode_substate_timer_start == false){
                attack_mode_substate_timer_start = true;
                StartCoroutine(change_attack_substate());
            }
        }
        parent.speed = parent.top_speed;
        parent.target_position = current_enemy.transform.position;
        if(parent.melee_weapon != null){
            if((current_enemy.transform.position - transform.position).magnitude > 0.3f){
                if(is_elite){
                    deflect_bullet();
                }
                parent.direction = current_enemy.transform.position - transform.position;
            }
            else{
                switch(attack_mode_substate){
                    case 0:
                        parent.direction = Vector2.zero;
                        parent.normal_attack();
                        break;
                    case 1:
                        parent.velocity = (Quaternion.AngleAxis(Random.Range(-90 , 90) , Vector3.forward) * (transform.position - current_enemy.transform.position)).normalized * 8f;
                        break;
                }
            }
        }
        else if(parent.ranged_weapon != null){
            if((current_enemy.transform.position - transform.position).magnitude > 2f){
                parent.direction = current_enemy.transform.position - transform.position;
            }
            else if((current_enemy.transform.position - transform.position).magnitude < 1.8f){
                parent.direction = transform.position - current_enemy.transform.position;
            }
            else{
                parent.direction = Vector2.zero;
            }
            parent.normal_attack();
        }
    }
    LayerMask attack_layer;
    void deflect_bullet(){
        Collider2D[] attacks = Physics2D.OverlapCircleAll(transform.position , 0.7f , attack_layer);
        for(int i = 0; i < attacks.Length; i++){
            DeflectableProjectile incomming = attacks[i].GetComponent<DeflectableProjectile>();
            if(incomming != null && incomming.parent != gameObject){
                parent.target_position = attacks[i].transform.position;
                parent.normal_attack();
                break;
            }
        }
    }
    float attack_mode_substate_time = 1f;
    IEnumerator change_attack_substate(){
        yield return new WaitForSeconds(attack_mode_substate_time);
        attack_mode_substate_decided = false;
        attack_mode_substate_timer_start = false;
    }
    Vector3 search_position , substate_2_search_position , before_search_position , substate_2_start_position;
    int search_substate = 0 , substate_2_search_times = 0;
    bool substate_2_search_position_picked = false;
    void search_mode_init(Vector3 position){
        StopAllCoroutines();
        NavBox nearest_navbox = find_nearest_navbox(position);
        if(nearest_navbox == null){
            free_roam_init();
            return;
        }
        search_position = nearest_navbox.transform.position;
        search_substate = 0;
        substate_2_search_times = 0;
        substate_2_search_position_picked = false;
        if(parent.ranged_weapon != null && parent.ranged_weapon.drawed){
            parent.special_attack();
        }
        ai_state = 2;
    }
    void search_mode(){
        sight();
        switch(search_substate){
            case 0:
                Vector3 target_vector = search_position - parent.feet.position;
                if(target_vector.magnitude > 0.1f){
                    parent.speed = parent.top_speed;
                    go_to(search_position);
                }
                else{
                    search_substate = 1;
                    substate_2_start_position = transform.position;
                    parent.speed = parent.top_speed/3;
                    parent.direction = Vector3.zero;
                    StartCoroutine(search_give_up());
                }
                break;
            case 1:
                if(substate_2_search_position_picked == false){
                    substate_2_search_position = substate_2_start_position + new Vector3(Random.Range(-0.05f , 0.05f) , Random.Range(-0.05f , 0.05f) , 0);
                    if(! Physics2D.Raycast(transform.position , (substate_2_search_position - transform.position).normalized , (substate_2_search_position - transform.position).magnitude , Obstacle)){
                        substate_2_search_position_picked = true;
                    }
                    return;
                }
                if((substate_2_search_position - transform.position).magnitude > 0.04f){
                    parent.direction = (substate_2_search_position - transform.position).normalized;
                }
                else{
                    parent.direction = Vector2.zero;
                    substate_2_search_position_picked = false;
                    substate_2_search_times++;
                }
                break;
        }
    }
    IEnumerator search_give_up(){
        yield return new WaitForSeconds(3);
        if(ai_state == 2 && search_substate == 1) free_roam_init();
    }
    void go_to(Vector3 target_position){
        if((transform.position - target_position).magnitude < 0.1f){
            parent.direction = Vector3.zero;
            return;
        }
        Vector3 vec = target_position - parent.feet.position;
        Vector3 upper_right = parent.feet.position + new Vector3(0.05f , 0.02f) , lower_right = parent.feet.position + new Vector3(0.05f , -0.02f) , upper_left = parent.feet.position + new Vector3(-0.05f , 0.02f) , lower_left = parent.feet.position + new Vector3(-0.05f , -0.02f);
        if(Physics2D.Raycast(upper_right , vec.normalized , vec.magnitude , Obstacle) ||
            Physics2D.Raycast(upper_left , vec.normalized , vec.magnitude , Obstacle) ||
            Physics2D.Raycast(lower_right , vec.normalized , vec.magnitude , Obstacle) ||
            Physics2D.Raycast(lower_left , vec.normalized , vec.magnitude , Obstacle)){
            List<Vector3> path = find_path(target_position);
            for(int i = 0; i < path.Count - 1; i++){
                Debug.DrawLine(path[i] , path[i+1] , Color.green);
            }
            Vector2 dir = path[1] - path[0];
            List<RaycastHit2D> hits = new List<RaycastHit2D>();
            RaycastHit2D[][] all_hits = {
                Physics2D.RaycastAll(path[0] + new Vector3(0.07f , 0.07f) , dir.normalized , dir.magnitude , default_layer) ,
                Physics2D.RaycastAll(path[0] + new Vector3(-0.07f , 0.07f) , dir.normalized , dir.magnitude , default_layer) ,
                Physics2D.RaycastAll(path[0] + new Vector3(0.07f , -0.07f) , dir.normalized , dir.magnitude , default_layer) , 
                Physics2D.RaycastAll(path[0] + new Vector3(-0.07f , -0.07f) , dir.normalized , dir.magnitude , default_layer) ,
                Physics2D.RaycastAll(path[0] , dir.normalized , dir.magnitude , default_layer)
            };
            for(int i = 0; i < all_hits.Length; i++){
                hits.AddRange(all_hits[i]);
            }
            bool on_track = false;
            for(int i = 0 ; i < hits.Count; i++){
                if(hits[i].transform.gameObject == gameObject){
                    on_track = true;
                    break;
                }
            }
            if(on_track){
                parent.direction = (path[1] - parent.feet.position).normalized;
            }
            else{
                parent.direction = (path[0] - parent.feet.position).normalized;
            }
        }
        else{
            parent.direction = vec.normalized;
        }
    }
    float radius = 0.5f;
    LayerMask Navbox , Obstacle;
    NavBox find_nearest_navbox(Vector3 pos){
        Collider2D[] neighbors = Physics2D.OverlapCircleAll(pos , radius , Navbox);
        if(neighbors.Length == 0) return null;
        List<Collider2D> accessable_navbox = new List<Collider2D>();
        for(int i = 0; i < neighbors.Length ; i++){
            Vector2 vec = neighbors[i].transform.position - pos;
            if(!Physics2D.Raycast(pos , vec.normalized , vec.magnitude , Obstacle)) accessable_navbox.Add(neighbors[i]);
        }
        if(accessable_navbox.Count == 0){
            for(int i = 0; i < neighbors.Length; i++){
                Debug.DrawLine(pos , neighbors[i].transform.position);
            }
            return null;
        }
        List<float> distances = new List<float>();
        Dictionary<float , NavBox> dist_box_dict = new Dictionary<float, NavBox>();
        for(int i = 0; i < accessable_navbox.Count ; i++){
            float dist = (accessable_navbox[i].transform.position - pos).magnitude;
            distances.Add(dist);
            dist_box_dict[dist] = accessable_navbox[i].GetComponent<NavBox>();
        }
        distances.Sort();
        return dist_box_dict[distances[0]];
    }
    List<GameObject> enemies = new List<GameObject>();
    public void hit(int damage , GameObject attacker){
        if(!enemies.Contains(attacker)){
            enemies.Add(attacker);
        }
        parent.velocity = (transform.position - attacker.transform.position).normalized * 5f;
        health -= damage;
        if(health <= 0){
            health = 0;
            parent.die();
        }
        search_mode_init(attacker.GetComponent<Character>().feet.position);
    }
}
