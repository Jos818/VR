//PressureDoorButton.cs by Joseph Panara for VR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//A button type that is deactivates as soon as it no longer recieves input
public class PressureDoorButton : MonoBehaviour
{
    public bool active;
    public Animator animator;
    public AudioSource audio;
    public AudioClip press;
    public AudioClip unpress;
    private bool playaud = true;

    void Start()
    {
        active = false;
        animator = gameObject.GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
    }

    //Checks if the player or a grabbable object is pressing the button
    private void OnTriggerStay(Collider collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Grabbable"))
        {
            active = true;
            animator.SetBool("Pressed", true);
            if (playaud == true)
            {
                audio.clip = press;
                audio.Play();
                playaud = false;
            }
            
        }
    }

    //Deactivates if nothing is pressing the button
    private void OnTriggerExit(Collider collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Grabbable"))
        {
            active = false;
            animator.SetBool("Pressed", false);
            audio.clip = unpress;
            audio.Play();
            playaud = true;
        }
    }
}
