using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : MonoBehaviour
{
    public int damage = 25;
    [SerializeField] GameObject hit_effect;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    GameObject parent;
    SpriteRenderer sprite;
    public bool cooling_down = false;
    public float cool_down_time = 0.3f;
    public void init(GameObject _parent){
        parent = _parent;
        sprite = GetComponent<SpriteRenderer>();
    }
    public GameObject first_swing , second_swing;
    int combo_count = 1;
    public void normal_attack(){
        if(cooling_down == false){
            if(combo_count == 1){
                first_swing.GetComponent<NormalSwing>().parent = parent;
                first_swing.GetComponent<NormalSwing>().damage = damage;
                first_swing.GetComponent<NormalSwing>().hit_effect = hit_effect;
                Instantiate(first_swing , parent.GetComponent<Character>().attack_point.transform.position , parent.GetComponent<Character>().attack_point.transform.rotation , parent.GetComponent<Character>().attack_point.transform);
                combo_count++;
            }
            else{
                second_swing.GetComponent<NormalSwing>().parent = parent;
                second_swing.GetComponent<NormalSwing>().damage = damage;
                second_swing.GetComponent<NormalSwing>().hit_effect = hit_effect;
                Instantiate(second_swing , parent.GetComponent<Character>().attack_point.transform.position , parent.GetComponent<Character>().attack_point.transform.rotation , parent.GetComponent<Character>().attack_point.transform);
                combo_count = 1;
            }
            sprite.enabled = false;
            cooling_down = true;
            StartCoroutine(wait_for_cooldown(cool_down_time));
        }
    }
    public void special_attack(){
        Debug.Log("special");
    }
    IEnumerator wait_for_cooldown(float time){
        yield return new WaitForSeconds(time);
        cooling_down = false;
        sprite.enabled = true;
    }
}
