//MultiSimulButtonDoor.cs by Joseph Panara for VR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This door type lets you assign any number of buttons that must be pressed simulaneously to open the door
public class MultiSimulButtonDoor : MonoBehaviour
{
    public Animator animator;
    public List<PressureDoorButton> buttons;
    bool dontopen = false;
    private AudioSource audio;
    bool playaud = true;

    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        animator.enabled = false;
        audio = gameObject.GetComponent<AudioSource>();
    }

    //Checks if all buttons are being pressed simultaneously, then opens the door
    void Update()
    {
        foreach (PressureDoorButton button in buttons)
        {
            if (button.active == false)
            {
                dontopen = true;
                break;
            }
            else
            {
                dontopen = false;
            }
            
                
        }
        if (dontopen == false)
        {
            animator.SetBool("Open", true);
            animator.enabled = true;
            if (playaud == true)
            {
                audio.Play();
                playaud = false;
            }
            
        }

    }
}
