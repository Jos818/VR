//Elevator.cs by Joseph Panara for VR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//An script that moves an object between 2 locations
public class Elevator : MonoBehaviour
{
    //2 points that determine where the elevator moves
    public Transform pointup;
    public Transform pointdown;

    public float speed = 0.3f;
    public bool up;
    public bool go;
    public bool exited;
    public Player_Move_Update player;
    public CameraMovement camera;
    public AudioSource audio;
    public AudioClip moving;
    public AudioClip arrived;
    public bool playaud;

    void Start()
    {
        exited = true;
        audio = GetComponent<AudioSource>();
    }

    void Update()
    {
        //Checks if the elevator haws been activated and if it has reached its destination
        //Freezes player and camera movement while moving
            if (up == false)
            {
                if (go == true)
                {
                    transform.position = Vector3.MoveTowards(transform.position, pointup.position, speed);
                }
                else
                {
                    transform.position = Vector3.MoveTowards(transform.position, pointdown.position, speed);

                }
                if (transform.position.Equals(pointup.position))
                {
                    up = true;
                exited = false;
                go = false;
                player.gameObject.transform.parent = null;
                player.inControl = true;
                camera.locked = false;
                if (playaud = true)
                {
                    audio.Stop();
                    audio.loop = false;
                    audio.clip = arrived;
                    audio.Play();
                    playaud = false;
                }
            }
            }
        //Same as above, but moves in the opposite direction
        else
        {
            if (go == true)
            {
                transform.position = Vector3.MoveTowards(transform.position, pointdown.position, speed);
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, pointup.position, speed);
            }
                if (transform.position.Equals(pointdown.position))
                {
                    up = false;
                exited = false;
                go = false;
                player.gameObject.transform.parent = null;
                player.inControl = true;
                camera.locked = false;
                if (playaud = true)
                {
                    audio.Stop();
                    audio.loop = false;
                    audio.clip = arrived;
                    audio.Play();
                    playaud = false;
                }
                
            }
            }
        
    }
    //Activates the elevator on player contact
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player")&& exited == true)
        {
            StartCoroutine(Pause());
        }
    }

    //Prepares the elevator to be activated again once the player has exited
    private void OnTriggerExit(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            StopAllCoroutines();
            exited = true;
            if (up == false)
            {
                go = false;
            }
        }
    }
    //Creates a short delay before the elevator starts so the player can leave if the elevator was entered by accident
    private IEnumerator Pause()
    {
        yield return new WaitForSeconds(2);
        player.inControl = false;
        camera.locked = true;
        audio.clip = moving;
        audio.loop = true;
        playaud = true;
        go = true;
        exited = false;
        player.gameObject.transform.parent = this.gameObject.transform;

    }
}
