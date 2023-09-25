//SalesmanAnimationOff.cs by Joseph Panara for VR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Stops the Salesman Animation during the tutorial cutscene
public class SalesmanAnimationOff : MonoBehaviour
{
    public GameObject salesman;
    private Animator animator;

    void Start()
    {
        animator = salesman.GetComponent<Animator>();
    }

    void Update()
    {
        animator.SetBool("IsRunning", false);
    }
}
