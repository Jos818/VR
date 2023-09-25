//GrabAnimations.cs by Joseph Panara for VR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Manages a squash-and-stretch animation for certain grabbable objects, to show they can be interacted with
public class GrabAnimations : MonoBehaviour
{
    private Player_Move_Update player;
    private Animator animator;
    bool readytograb;

    //Gets the necessary components
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.enabled = false;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player_Move_Update>();
    }

    //Controls whether the animation is playing
    void Update()
    {
        if (readytograb == true)
        {
            animator.enabled = true;
        }
        else
        {
            animator.enabled = false;
        }
    }
    
    //Allows the animation to play if the player is in range of the object, is not already holding the object, and is not holding another object
    private void OnTriggerEnter(Collider other)
    {
        if (other == player.grabrange && player.grabbedobject != this.gameObject && player.grabbedobject == null)
        {
            readytograb = true;
        }


    }
    private void OnTriggerStay(Collider other)
    {
            if (other == player.grabrange && player.grabbedobject != this.gameObject && player.grabbedobject == null)
            {
            readytograb = true;
        }
            
      

    }
    //Prevents the animation from playing when the player is no longer in range to grab the object
    private void OnTriggerExit(Collider other)
    {
            if (other == player.grabrange)
            {
            readytograb = false;
        }
    }
}
