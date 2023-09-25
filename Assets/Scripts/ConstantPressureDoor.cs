//ConstantPressureDoor.cs by Joseph Panara for VR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This door type lets you assign any number of buttons that must be pressed simulaneously to open the door. The door will close if any button is unpressed.
public class ConstantPressureDoor : MonoBehaviour
{
    public Animator animator;
    private AudioSource audio;
    public AudioClip open;
    public AudioClip close;
    public List<PressureDoorButton> buttons;
    bool dontopen = true;
    bool playaud;

    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        audio = gameObject.GetComponent<AudioSource>();
    }

    void Update()
    {
        foreach (PressureDoorButton button in buttons)
        {
            if (button.active == false)
            {
                if (dontopen == false && animator.GetBool("Open") == true)
                {
                    audio.clip = close;
                    audio.Play();
                    playaud = true;
                }
                dontopen = true;
                animator.SetBool("Open", false);
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
            if (playaud == true)
            {
                audio.clip = open;
                audio.Play();
                playaud = false;
            }
            
        }

    }
}
