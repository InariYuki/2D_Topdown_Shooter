using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Corpse : MonoBehaviour
{
    [SerializeField] GameObject head , body;
    void Start()
    {
        make_corpse();
    }
    void make_corpse(){
        GameObject head_instanced = Instantiate(head , transform.position , Quaternion.identity);
        GameObject body_instanced = Instantiate(body , transform.position , Quaternion.identity);
        head_instanced.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-1f , 1f) , Random.Range(0f , 1f)) * 100f);
        body_instanced.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-1f , 1f) , Random.Range(0f , -1f)) * 100f);
    }
}
