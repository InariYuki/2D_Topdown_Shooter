using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavBox : MonoBehaviour
{
    public float radius = 0.5f;
    [SerializeField] LayerMask Navbox;
    [SerializeField] LayerMask Obstacle;
    [SerializeField] Collider2D collision;
    public List<NavBox> next_hops = new List<NavBox>();
    // Start is called before the first frame update
    void Start()
    {
        connect();
    }
    void connect(){
        Collider2D[] neighbors = Physics2D.OverlapCircleAll(transform.position , radius , Navbox);
        foreach(Collider2D neighbor in neighbors){
            if(neighbor == collision){
                continue;
            }
            Vector2 vec = neighbor.transform.position - transform.position;
            if(!Physics2D.Raycast(transform.position + new Vector3(0.07f , 0.07f), vec.normalized , vec.magnitude , Obstacle) &&
                !Physics2D.Raycast(transform.position + new Vector3(-0.07f , 0.07f) , vec.normalized , vec.magnitude , Obstacle) &&
                !Physics2D.Raycast(transform.position + new Vector3(0.07f , -0.07f) , vec.normalized , vec.magnitude , Obstacle) &&
                !Physics2D.Raycast(transform.position + new Vector3(-0.07f , -0.07f) , vec.normalized , vec.magnitude , Obstacle)){
                Debug.DrawLine(transform.position , neighbor.transform.position , Color.blue , 100f);
                next_hops.Add(neighbor.GetComponent<NavBox>());
            }
        }
    }
    void OnDrawGizmos(){
        Gizmos.DrawWireSphere(transform.position , radius);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
