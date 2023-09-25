//NOT USED IN PROJECT
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CameraMovementTut : MonoBehaviour
{
    bool vertmov;
    public bool grabbed;
    bool grabrotate;
    public float x;
    public float y;
    public float vmov;
    //public float vertpos;
    public GameObject player;
    public TextMeshProUGUI rotationtext;
    bool text;
    //TUTORIAL BOOLS
    public bool locked = false;
    public bool vertlock = false;
    public bool horilock = false;
    public bool shiftlock = false;
    void Start()
    {
        grabrotate = false;
        locked = true;
        text = false;

    }

    void Update()
    {
        if (locked == false)
        {
          vmov = Input.GetAxis("VerticalArrows") * Time.deltaTime;
            
            //Locks Y Rotation When Held
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
            
            
                if (Input.GetKey("left shift"))
                {
                if (shiftlock == false)
                {
                    vertmov = true;
                }
                }
                else //if (Input.GetKeyUp("left shift"))
                {
                    vertmov = false;
                }
                if (vertmov == true)
                {
                    transform.Translate(0, vmov, 0);
                //vertpos += vmov;
                }
                else if (vertmov == false)
                {
                    if(vertlock == false)
                    {
                        y += Input.GetAxis("VerticalArrows") * Time.deltaTime * 100.0f;
                    }
                }
            
           
            //Sets Up/Down Rotation Limits
            y = Mathf.Clamp(y, -90, 90);
            //Actually Rotates
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
        
       
    }
    IEnumerator RotationText()
    {
        if (text == false)
        {
            rotationtext.text = new string ("AUTOMATIC ROTATION ON");
            text = true;
        }
        else if (text == true)
        {
            rotationtext.text = new string ("MANUAL ROTATION ON");
            text = false;
        }
        rotationtext.gameObject.active = true;
        yield return new WaitForSeconds(1f);
        rotationtext.gameObject.active = false;
    }
}
