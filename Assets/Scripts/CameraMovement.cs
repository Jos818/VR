//CameraMovement.cs by Joseph Panara for VR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//This script handles the Camera movement controls
public class CameraMovement : MonoBehaviour
{
    bool vertmov;
    public bool grabbed;
    bool grabrotate;
    public float x;
    public float y;
    public float vmov;
    public GameObject player;
    public TextMeshProUGUI rotationtext;
    public bool text;
    //Tutorial bools used to restrict movement available to player at different parts of the tutorial
    public bool tutorial;
    public bool locked = false;
    public bool vertlock = false;
    public bool horilock = false;
    public bool shiftlock = false;

    void Start()
    {
        grabrotate = false;
        if (tutorial == true)
        {
            locked = true;
            text = false;
        }
    }

    void Update()
    {
        if (locked == false)
        {
            //Variable that controls telescopic vertical movement of the camera (it can move up and down slightly)
            vmov = Input.GetAxis("VerticalArrows") * Time.deltaTime;

            //Handles horizontal rotation of the camera. Linked to WASD when grabbed, arrows otherwise
            if (horilock == false)
            {
                if (grabbed == true)
                {
                    x += Input.GetAxis("HorizontalWASD") * Time.deltaTime * 100.0f;
                }
                else
                {
                    x += Input.GetAxis("HorizontalArrows") * Time.deltaTime * 100.0f;
                }
            }

            //Inputting left shift changes the vertical arrows function from vertical rotation to telescopic vertical movement
            if (Input.GetKey("left shift"))
            {
                if (shiftlock == false)
                {
                    vertmov = true;
                }
            }
            else 
            {
                vertmov = false;
            }
            if (vertmov == true)
            {
                transform.Translate(0, vmov, 0);
            }
            else if (vertmov == false)
            {
                if (vertlock == false)
                {
                    y += Input.GetAxis("VerticalArrows") * Time.deltaTime * 100.0f;
                }
            }
            //Sets Up/Down Rotation Limits
            y = Mathf.Clamp(y, -90, 90);
   
            if (grabbed == true)
            {
                if (text == false)
                {
                    StartCoroutine(RotationText());
                }
                if (grabrotate == false)
                {
                    x = (player.transform.localEulerAngles.y);
                    grabrotate = true;
                }
            }
            else
            {
                grabrotate = false;
                if (text == true)
                {
                    StartCoroutine(RotationText());
                }
            }
            transform.rotation = Quaternion.Euler(-y, x, 0);



            //Sets Z Rotation Limits
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, 0);
            //Sets Vertical Movement Limits
            Vector3 movementlim = transform.localPosition;
            movementlim.x = Mathf.Clamp(movementlim.x, 0, 0);
            movementlim.y = Mathf.Clamp(movementlim.y, 0, 1);
            movementlim.z = Mathf.Clamp(movementlim.z, 0, 0);
            transform.localPosition = movementlim;
        }

        //Text appears telling the player what control scheme operates horizontal rotation. Automatic rotation = WASD, and is linked with player movement. Manual rotation = arrow keys
        IEnumerator RotationText()
        {
            if (text == false)
            {
                rotationtext.text = new string("AUTOMATIC ROTATION ON");
                text = true;
            }
            else if (text == true)
            {
                rotationtext.text = new string("MANUAL ROTATION ON");
                text = false;
            }
            rotationtext.gameObject.active = true;
            yield return new WaitForSeconds(1f);
            rotationtext.gameObject.active = false;
        }

    }
}
