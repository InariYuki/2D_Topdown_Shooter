using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform cam;
    Camera camera_instanced;
    [SerializeField] Transform target;
    [SerializeField] float speed;
    // Start is called before the first frame update
    private void Awake() {
        camera_instanced = cam.GetComponent<Camera>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 final_position = Vector2.Lerp(cam.position , target.position + (camera_instanced.ScreenToWorldPoint(Input.mousePosition) - target.position)/3 , speed * Time.deltaTime);
        cam.position = new Vector3(final_position.x , final_position.y , -10f);
    }
}
