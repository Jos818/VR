//SalesmanAnimSwitch.cs by Joseph Panara for VR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Activates the Salesman animation during the tutorial cutscene
public class SalesmanAnimSwitch : MonoBehaviour
{
    public GameObject salesman;
    private Animator animator;

    void Start()
    {
        animator = salesman.GetComponent<Animator>();
        animator.SetBool("IsRunning", true);
    }

    void Update()
    {
        animator.SetBool("IsRunning", true);
    }
}
