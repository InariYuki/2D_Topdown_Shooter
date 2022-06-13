using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedOnHand : MonoBehaviour
{
    public Animator animator;
    public Transform muzzle_position;
    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }
}
