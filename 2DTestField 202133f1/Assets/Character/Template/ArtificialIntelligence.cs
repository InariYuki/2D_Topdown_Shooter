using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtificialIntelligence : MonoBehaviour
{
    [SerializeField] Hitbox hitbox;
    public int max_health = 100;
    int health;
    [SerializeField] Character parent;
    // Start is called before the first frame update
    void Start()
    {
        initial_parameters();
    }
    void initial_parameters(){
        health = max_health;
    }
    NavBox start_navbox , end_navbox;
    List<Vector3> find_path(Vector3 pos){
        start_navbox = find_nearest_navbox(transform.position);
        end_navbox = find_nearest_navbox(pos);
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
            foreach(NavBox nav_box in current_navbox.next_hops){
                if(! unpicked_navboxes.Contains(nav_box) && ! picked_navboxes.Contains(nav_box)){
                    unpicked_navboxes.Add(nav_box);
                }
            }
            foreach(NavBox nav_box in unpicked_navboxes){
                if(! box_parent_dict.ContainsKey(nav_box)){
                    box_parent_dict[nav_box] = current_navbox;
                }
                cost_box_dict[(nav_box.transform.position - transform.position).magnitude + (nav_box.transform.position - end_navbox.transform.position).magnitude] = nav_box;
                cost.Add((nav_box.transform.position - transform.position).magnitude + (nav_box.transform.position - end_navbox.transform.position).magnitude);
            }
            cost.Sort();
            current_navbox = cost_box_dict[cost[0]];
            unpicked_navboxes.Remove(cost_box_dict[cost[0]]);
            picked_navboxes.Add(current_navbox);
        }
        List<NavBox> box_path = new List<NavBox>();
        while(current_navbox != start_navbox){
            box_path.Add(current_navbox);
            current_navbox = box_parent_dict[current_navbox];
        }
        box_path.Add(current_navbox);
        List<Vector3> reversed_path = new List<Vector3>();
        foreach(NavBox box in box_path){
            reversed_path.Add(box.transform.position);
        }
        List<Vector3> path = new List<Vector3>();
        for(int i = reversed_path.Count - 1; i >=0;i--){
            path.Add(reversed_path[i]);
        }
        path.Add(pos);
        return path;
    }
    // Update is called once per frame
    void Update()
    {
        attack_mode();
    }
    float sight_distance = 2f , view_angle = 80;
    [SerializeField] LayerMask default_layer;
    List<Character> sight(){
        Collider2D[] chatacter_colliders = Physics2D.OverlapCircleAll(transform.position , sight_distance , default_layer);
        List<Character> detected_characters = new List<Character>();
        foreach(Collider2D character_collider in chatacter_colliders){
            if(! Physics2D.Raycast(transform.position , (character_collider.transform.position - transform.position).normalized , (character_collider.transform.position - transform.position).magnitude , Obstacle) && Vector3.Angle((character_collider.transform.position - transform.position).normalized , parent.facing_direction) <= view_angle){
                detected_characters.Add(character_collider.GetComponent<Character>());
                Debug.DrawLine(transform.position , character_collider.transform.position , Color.cyan);
            }
        }
        return detected_characters;
    }
    NavBox previous = null , current = null;
    bool rest = false , trigger = false;
    public int stop_count = 3;
    public float stop_duration = 1f;
    int step_count = 0;
    void free_roam_init(){
        previous = null;
        current = null;
        rest = false;
        trigger = false;
        step_count = 0;
    }
    void free_roam(){
        if(current == null) current = find_nearest_navbox(transform.position);
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
            List<NavBox> nexts = new List<NavBox>();
            foreach(NavBox box in current.next_hops){
                nexts.Add(box);
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
    }
    IEnumerator rest_for_a_while(float time){
        yield return new WaitForSeconds(time);
        rest = false;
        trigger = false;
    }
    GameObject current_enemy = null;
    void attack_mode(){
        if(current_enemy == null) return;
        parent.target_position = current_enemy.transform.position;
        if(parent.melee_weapon != null){
            if((current_enemy.transform.position - transform.position).magnitude > 0.3f){
                parent.direction = current_enemy.transform.position - transform.position;
            }
            else{
                parent.direction = Vector2.zero;
                parent.normal_attack();
            }
        }
    }
    Vector3 target_position;
    void go_to(){
        if((transform.position - target_position).magnitude < 0.1f){
            parent.direction = Vector3.zero;
            return;
        }
        List<Vector3> path = find_path(target_position);
        if(! Physics2D.Raycast(transform.position , (path[1] - transform.position).normalized , (path[1] - transform.position).magnitude , Obstacle)){
            parent.direction = (path[1] - transform.position).normalized;
            Debug.DrawLine(transform.position , path[1] , Color.red);
        }
        else{
            parent.direction = (path[0] - transform.position).normalized;
            Debug.DrawLine(transform.position , path[0] , Color.red);
        }
    }
    float radius = 0.5f;
    [SerializeField] LayerMask Navbox;
    [SerializeField] LayerMask Obstacle;
    NavBox find_nearest_navbox(Vector3 pos){
        Collider2D[] neighbors = Physics2D.OverlapCircleAll(pos , radius , Navbox);
        if(neighbors.Length == 0) return null;
        List<float> distances = new List<float>();
        Dictionary<float , NavBox> dist_box_dict = new Dictionary<float, NavBox>();
        foreach(Collider2D neighbor in neighbors){
            float dist = (neighbor.transform.position - transform.position).magnitude;
            distances.Add(dist);
            dist_box_dict[dist] = neighbor.GetComponent<NavBox>();
        }
        distances.Sort();
        return dist_box_dict[distances[0]];
    }
    public void hit(int damage , GameObject attacker){
        parent.velocity = (transform.position - attacker.transform.position).normalized * 40f;
        health -= damage;
        current_enemy = attacker;
        Debug.Log("hit");
    }
}
