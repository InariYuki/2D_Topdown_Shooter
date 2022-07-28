using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectorHandler : MonoBehaviour
{
    [SerializeField] Transform spawn_point_container;
    [HideInInspector] public List<Vector2> spawn_points = new List<Vector2>();
    private void Awake() {
        for(int i = 0; i < spawn_point_container.childCount; i++){
            spawn_points.Add(spawn_point_container.GetChild(i).position);
        }
    }
}
