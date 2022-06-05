using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedOnHand : MonoBehaviour
{
    public Animator animator;
    public Transform muzzle_position;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
