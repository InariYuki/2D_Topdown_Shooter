using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavBox : MonoBehaviour
{
    public float radius = 0.5f;
    public int navbox_tag = 0;
    LayerMask Navbox;
    LayerMask Obstacle;
    Collider2D collision;
    [HideInInspector] public List<NavBox> next_hops = new List<NavBox>();
    private void Awake() {
        collision = GetComponent<Collider2D>();
        Navbox = LayerMask.GetMask("Navigation");
        Obstacle = LayerMask.GetMask("Obstacle");
    }
    void Start()
    {
        connect();
    }
    public void connect(){
        next_hops.Clear();
        Collider2D[] neighbors = Physics2D.OverlapCircleAll(transform.position , radius , Navbox);
        for(int i = 0; i < neighbors.Length; i++){
            if(neighbors[i] == collision){
                continue;
            }
            Vector2 vec = neighbors[i].transform.position - transform.position;
            if(!Physics2D.Raycast(transform.position + new Vector3(0.07f , 0.07f), vec.normalized , vec.magnitude , Obstacle) &&
                !Physics2D.Raycast(transform.position + new Vector3(-0.07f , 0.07f) , vec.normalized , vec.magnitude , Obstacle) &&
                !Physics2D.Raycast(transform.position + new Vector3(0.07f , -0.07f) , vec.normalized , vec.magnitude , Obstacle) &&
                !Physics2D.Raycast(transform.position + new Vector3(-0.07f , -0.07f) , vec.normalized , vec.magnitude , Obstacle)){
                Debug.DrawLine(transform.position , neighbors[i].transform.position , Color.blue , 100f);
                next_hops.Add(neighbors[i].GetComponent<NavBox>());
            }
        }
    }
    void OnDrawGizmos(){
        Gizmos.DrawWireSphere(transform.position , radius);
    }
}
