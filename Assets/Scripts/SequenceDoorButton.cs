//SequenceDoorButton.cs by Joseph Panara for VR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//A button type that operates doors that require buttons to only be pressed once, usually in a code sequence
public class SequenceDoorButton : MonoBehaviour
{
    public bool active;
    public bool locked;
    public bool timed;
    public int time;
    public Animator animator;
    public AudioSource audio;
    public AudioClip press;
    public AudioClip unpress;
    public bool playaud;

    void Start()
    {
        active = false;
        playaud = true;
        animator = gameObject.GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
    }

    //Checks if either the player or a grabbable object is on the button to activate
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Grabbable"))
        {
            if (locked == false)
            {
                active = true;
                animator.SetBool("Pressed", true);
                if (timed == true)
                {
                    StartCoroutine(Timer());
                }
                if (playaud == true)
                {
                    audio.clip = press;
                    audio.Play();
                    playaud = false;
                }
            }
            
        }
    }
    //A function that allows the button to be set on a timer before deactivating
    private IEnumerator Timer()
    {
        yield return new WaitForSeconds(time);
        active = false;
        animator.SetBool("Pressed", false);
        audio.clip = unpress;
        audio.Play();
        playaud = true;

    }
}
