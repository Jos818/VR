//DoorCodeReset.cs by Joseph Panara for VR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Code for a button that resets the attached door's access code when pressed by the player
//Also controls audio and animations for the button
public class DoorCodeReset : MonoBehaviour
{
    [SerializeField] private CodeSequenceDoor door;
    public Animator animator;
    public AudioSource audio;
    public AudioClip change;
    public AudioClip unpress;
    public bool playaud = true;

    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Grabbable"))
        {
            door.CodeGen();
            animator.SetBool("Pressed", true);
            if (playaud == true)
            {
                audio.clip = change;
                audio.Play();
                playaud = false;
            }

        }
    }
    private void OnTriggerExit(Collider collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Grabbable"))
        {
            animator.SetBool("Pressed", false);
            audio.clip = unpress;
            audio.Play();
            playaud = true;
        }
    }
}
