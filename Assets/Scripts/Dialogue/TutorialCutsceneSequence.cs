//TutorialoCutsceneSequence.cs by Joseph Panara for VR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//This script manages player and camera movement during the interactive tutorial cutscene by limiting the user to only certain controls at a time to teach the controls
public class TutorialCutsceneSequence : MonoBehaviour
{
    public bool lock1;
    public bool lock2;
    public bool lock3;
    public bool lock4;
    public bool lock5;
    public bool lock6;
    public bool lock7;
    public bool lock8;
    public bool lock9;
    public bool lock10;
    public bool lock11;
    public bool ontable;
    public bool done;
    public CameraMovement camera;
    public Player_Move_Update player;
    public TimelineResume tr;
    public GameObject wall;
    public TextMeshProUGUI controls;

    void Start()
    {
        camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraMovement>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player_Move_Update>();
        Physics.IgnoreLayerCollision(9, 8);

    }
    //Unlocks player and camera movement for controls to be practiced
    void OnEnable()
    {
        camera.locked = false;
        player.inControl = false;
        if (done == true)
        {
            StartCoroutine(CutsceneEnd());
        }
        //Activates the tutorial isntruction text
        controls.gameObject.SetActive(true);
    }
    //Relocks the camera while the tutorial is still in progress
    void OnDisable()
    {
        if (done == false)
        {
            camera.locked = true;
        }
        //Deactivates the tutorial isntruction text
        controls.gameObject.SetActive(false);
    }

    //Each block contains a separate set of movement restrictions and instructions for the player to read during the tutorial, managed by the functions below
    void Update()
    {
        if (done == false)
        {

            if (lock1 == true)
            {
                controls.text = new string("Use the Left and Right Arrows to Rotate the Camera Horizontally");
                Lock1();
                if (camera.x >= 180 || camera.x <= -180)
                {
                    lock2 = true;
                    lock1 = false;
                    camera.locked = true;
                    tr.gameObject.SetActive(true);
                    gameObject.SetActive(false);
                }
            }
            if (lock2 == true)
            {
                controls.text = new string("Use the Up and Down Arrows to Rotate the Camera Vertically");
                Lock2();
                if (camera.y == 90)
                {
                    lock3 = true;
                    lock2 = false;
                    camera.locked = true;
                    tr.gameObject.SetActive(true);
                    gameObject.SetActive(false);
                }
            }
            if (lock3 == true)
            {
                if (camera.y == -90)
                {
                    lock4 = true;
                    lock3 = false;
                    camera.locked = true;
                    tr.gameObject.SetActive(true);
                    gameObject.SetActive(false);
                }
            }
            if (lock4 == true)
            {
                if (camera.y <= 1 && camera.y >= -1)
                {
                    lock5 = true;
                    lock4 = false;
                    camera.locked = true;
                    tr.gameObject.SetActive(true);
                    gameObject.SetActive(false);
                }
            }
            if (lock5 == true)
            {
                controls.text = new string("Hold Shift and Use the Up and Down Arrows to Shift the Camera Vertically");
                Lock3();
                if (camera.transform.localPosition.y >= 0.9f)
                {
                    lock6 = true;
                    lock5 = false;
                    camera.locked = true;
                    tr.gameObject.SetActive(true);
                    gameObject.SetActive(false);
                }
            }
            if (lock6 == true)
            {
                if (camera.transform.localPosition.y <= 0.1f)
                {
                    lock7 = true;
                    lock6 = false;
                    camera.locked = true;
                    tr.gameObject.SetActive(true);
                    gameObject.SetActive(false);
                }
            }
            if (lock7 == true)
            {
                if (!player.grabtarget)
                {
                    controls.text = new string("Use WASD to Move to the Camera");
                }
                else
                {
                    controls.text = new string("Press the Space Bar to Pick up Objects");
                }
                Lock4();
                if (player.grabbed == true)
                {
                    lock8 = true;
                    lock7 = false;
                    tr.gameObject.SetActive(true);
                    gameObject.SetActive(false);
                }
            }
            if (lock8 == true)
            {
                controls.text = new string("Press the Space Bar to Drop Held Objects");
                Lock5();
                if (player.grabbed == false && ontable == true)
                {
                    lock9 = true;
                    lock8 = false;
                    tr.gameObject.SetActive(true);
                    gameObject.SetActive(false);
                }
            }
            if (lock9 == true)
            {
                controls.text = new string("Press the Space Bar to Pick up Objects");
                Lock6();
                if (player.grabbed == true)
                {
                    lock10 = true;
                    lock9 = false;
                    tr.gameObject.SetActive(true);
                    gameObject.SetActive(false);
                }
            }
            if (lock10 == true)
            {
                controls.text = new string("Hold the Space Bar and Release to Throw Objects");
                Lock7();
                if (player.dropforce >= 300)
                {
                    player.nodrop = false;            
                }
                else
                {
                    player.nodrop = true;
                }
                if (player.dropping == true)
                {
                    lock11 = true;
                    lock10 = false;
                    tr.gameObject.SetActive(true);
                    gameObject.SetActive(false);
                }
            }
            if (lock11 == true)
            {
                if (!player.grabtarget && player.grabbed == false)
                {
                    controls.text = new string("Use WASD to Move to the Camera");
                }
                else if (player.grabtarget != null && player.grabbed == false)
                {
                    controls.text = new string("Press the Space Bar to Pick up the Camera");
                }
                else if (player.grabbed == true)
                {
                    controls.text = new string("Walk to the Table and Press the Space Bar to Drop the Camera");
                }
                Lock8();
                if (ontable == true)
                {
                    done = true;
                    lock11 = false;
                    tr.gameObject.SetActive(true);
                    gameObject.SetActive(false);
                }
            }
        }
    }

    //The functions that restrict the player and camera controls to what is required during the tutorial
    void Lock1()
    {
        camera.vertlock = true;
        camera.shiftlock = true;
        lock1 = true;
    }
    void Lock2()
    {
        camera.vertlock = false;
        camera.horilock = true;
        camera.shiftlock = true;
    }
    void Lock3()
    {
        camera.vertlock = true;
        camera.shiftlock = false;
    }
    void Lock4()
    {
        camera.locked = true;
        camera.vertlock = false;
        camera.horilock = false;
        player.nospin = true;
        player.nodrop = true;
        player.inControl = true;
    }
    void Lock5()
    {
        player.nomove = false;
        player.nospin = false;
        player.nograb = true;
        player.nocharge = true;
        player.nodrop = false;
        player.charging = true;
        player.inControl = true;
    }
    void Lock6()
    {
        player.nospin = true;
        player.nograb = false;
        player.nodrop = true;
        player.inControl = true;
    }
    void Lock7()
    {
        player.nocharge = false;
        player.nograb = true;
        player.inControl = true;
    }
    void Lock8()
    {
        player.nocharge = false;
        player.nograb = false;
        player.nodrop = false;
        player.nomove = false;
        player.nospin = false;
        camera.locked = false;
        player.inControl = true;
    }
    //Removes all tutorial restrictions and UI elements
    IEnumerator CutsceneEnd()
    {
        Destroy(controls);
        player.inControl = true;
        camera.locked = false;
        Destroy(wall);
        yield return new WaitForSeconds(0.5f);
            Destroy(gameObject);
    }
    //Checks if the camera is on the table during the tutorial
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "CameraObj")
        {
            ontable = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "CameraObj")
        {
            ontable = false;
        }
        

    }
}
