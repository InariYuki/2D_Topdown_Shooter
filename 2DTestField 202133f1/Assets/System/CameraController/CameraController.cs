using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    /*
        manually set:
            cam
            target
            speed
    */
    public Camera cam;
    [SerializeField] Transform target;
    [SerializeField] float speed;
    Vector3 deviation = Vector2.zero;
    public bool is_dynamic = true;
    void FixedUpdate()
    {
        if(target == null) return;
        if(is_dynamic) deviation = (cam.ScreenToWorldPoint(Input.mousePosition) - target.position) / 3;
        else deviation = Vector3.zero;
        Vector2 final_position = Vector2.Lerp(cam.transform.position , target.position + deviation , speed * Time.deltaTime);
        cam.transform.position = new Vector3(final_position.x , final_position.y , -10f);
    }
}
