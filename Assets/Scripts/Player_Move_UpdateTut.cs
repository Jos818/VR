//NOT USED IN PROJECT
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Move_UpdateTut : MonoBehaviour {

    public float playerSpeed = 10;
    public int playerJumpPower = 1250;
    private float moveX;
    private float moveY;
    private Rigidbody playerrb;

    private float elapsedTime;

    [Tooltip("Only change this if your character is having problems jumping when they shouldn't or not jumping at all.")]
    public float distToGround = 1.0f;
    public bool inControl = true;

    [Tooltip("Everything you jump on should be put in a ground layer. Without this, your player probably* is able to jump infinitely")]
    public LayerMask GroundLayer;

    Animator m_Animator;
    //Collider for Grab Range
    public Collider grabrange;
    //Grabbable Object in Grab Range
    public GameObject grabtarget;
    //Object Once Grabbed By The Player
    public GameObject grabbedobject;
    //Rigidbody for Grabbed Object
    Rigidbody grabbedrb;
    //Location for the Grabbed Object to Go
    Transform inhand;
    //Bool to Show Something is Currently Grabbed
    public bool grabbing;
    //Bool to make sure player doesn't immediately throw after grabbing
    public bool grabbed;
    //Location to Drop Grabbed Object
    //Transform drop;
    //Vector and Force Multiplier for Drop Action (1,1,0) and 120 work for simple drop in objects with Mass of 1 and size of standard unity cube
    public Vector3 dropvec;
    public float dropforce;
    public bool charging;
    //Tutorial Bools
    public bool nomove;
    public bool nograb;
    public bool nodrop;
    public bool nocharge;
    public bool nospin;




    void Start(){

        m_Animator = gameObject.transform.GetChild(0).GetComponent<Animator>();
        playerrb = gameObject.GetComponent<Rigidbody>();
        elapsedTime = 0;
        grabrange = gameObject.transform.GetChild(0).GetComponent<Collider>();
        inhand = gameObject.transform.GetChild(1);
        //drop = gameObject.transform.GetChild(2);
        grabbing = false;
        dropvec = new Vector3 (0, 1, 0);
        charging = false;
        grabbed = false;


    }

    // Update is called once per frame
    void Update()
    {
        

        if (inControl)
        {
            PlayerMove();
        }
        
        }

    void PlayerMove()
    {
        //CONTROLS
        var x = Input.GetAxis("HorizontalWASD") * Time.deltaTime * playerSpeed;
        var z = Input.GetAxis("VerticalWASD") * Time.deltaTime * playerSpeed;
            //Rigidbody.AddForce prevents player from clipping through walls
            //transform.Translate(0, 0, z);
            if (nomove == false)
            {
                playerrb.AddForce((transform.forward) * z * 2000);
            }
            if (nospin == false)
            {
                transform.Rotate(0, (x * 10), 0);
            } 
        dropvec = transform.forward;
        dropvec = new Vector3 (dropvec.x, 1, dropvec.z);
        
        /*if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            Jump();
        }*/
        //GRAB MECHANIC
        if (grabtarget != null && Input.GetKeyDown("space") && grabbing == false && nograb == false)
        {
            Grab();
        }
        if (Input.GetKeyUp("space") && grabbing == true && grabbed == false)
        {
            grabbed = true;
        }
        if (Input.GetKey("space") && grabbing == true && grabbed == true && nocharge == false)
        {
            Charge();
        }
        if (Input.GetKeyUp("space") && grabbing == true && charging == true && nodrop == false)
        {
            Drop();
        }

            //ANIMATIONS
            if (x != 0 || z != 0){
            //Debug.Log("x and z are " + x + " " + z);
            m_Animator.SetBool("IsRunning", true);
            m_Animator.SetBool("IsIdle",false);
        }
        else{
            m_Animator.SetBool("IsRunning", false); 
            m_Animator.SetBool("IsIdle",true);
        }

        
        /*if (Input.GetButtonDown("Jump"))
            m_Animator.SetBool("IsJumping", true);
        else
            m_Animator.SetBool("IsJumping", false);
            */
      
        //PHYSICS
        gameObject.GetComponent<Rigidbody>().velocity = new Vector2(moveX * playerSpeed, gameObject.GetComponent<Rigidbody>().velocity.y);
        
    }

    void Jump()
    {
        //JUMPING CODE
        GetComponent<Rigidbody>().AddForce(Vector3.up * playerJumpPower);

    }

    public bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, distToGround + GroundLayer);

        //if (hit.collider != null)
        //{
        //    return true;
        //}
        //return false;

    }

    public void SetControl(bool b)
    {
        inControl = b;
    }

    public void Grab()
    {
        grabbedobject = grabtarget;
        grabbedrb = grabbedobject.GetComponent<Rigidbody>();
        grabbedrb.isKinematic = true;
        grabbedobject.transform.position = inhand.position;
        grabbedobject.transform.parent = this.gameObject.transform;
        m_Animator.SetBool("isGrabbing", true);
        if (grabbedobject.name == "CameraObj")
        {
            //grabbedobject.transform.position = inhand.position;
            grabbedobject.transform.GetChild(0).GetComponent<CameraMovementTut>().grabbed = true;
            grabbedobject.transform.GetChild(0).eulerAngles = new Vector3(transform.eulerAngles.x, (transform.eulerAngles.y), transform.eulerAngles.z);
        }
        else
        {
            //grabbedobject.transform.localPosition = new Vector3 (-.4f, 1.8f, 0.6f);
        }
        grabbing = true;
    }
    public void Drop()
    {
        if (grabbedobject.name == "CameraObj")
        {
            grabbedobject.transform.GetChild(0).GetComponent<CameraMovementTut>().grabbed = false;
            grabbedobject.transform.GetChild(0).eulerAngles = new Vector3(transform.eulerAngles.x, (transform.eulerAngles.y), transform.eulerAngles.z);
        }
        grabbedrb.isKinematic = false;
        grabbedrb.AddForce(dropvec * dropforce);
        StartCoroutine(ThrowAnim());
        m_Animator.SetBool("isGrabbing", false);
        grabbedobject.transform.parent = null;
        grabbedobject = null;
        grabbedrb = null;
        grabbing = false;
        dropforce = 120;
        charging = false;
        grabbed = false;

    }
    public void Charge()
    {
        if (dropforce >= 180)
        {
            m_Animator.SetBool("isCharged", true);
        }
        if (dropforce <= 500)
            {
            charging = true;
                dropforce += 150f * Time.deltaTime;
                
            }   
    }
    void OnTriggerEnter(Collider target)
    {
        if (target.gameObject.tag == "Grabbable")
        {
            grabtarget = target.gameObject;
        }
    }
    void OnTriggerExit(Collider target)
    {
        if (target.gameObject.tag == "Grabbable")
        {
            grabtarget = null;
        }
    }
    IEnumerator ThrowAnim()
    {
        m_Animator.SetBool("isDropping", true);
        yield return new WaitForSeconds(0.5f);
        m_Animator.SetBool("isDropping", false);
        m_Animator.SetBool("isCharged", false);
    }
}
