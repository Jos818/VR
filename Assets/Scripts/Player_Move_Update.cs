//Player_Move_Update.cs by Joseph Panara for VR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script handles player controls, including movement, grab, and throw mechanics
public class Player_Move_Update : MonoBehaviour {

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
    private PlayerAudioManager playerAudio;
    //Collider for Grab Range
    public Collider grabrange;
    //Grabbable Object in Grab Range
    public GameObject grabtarget;
    //Object Once Grabbed By The Player
    public GameObject grabbedobject;
    public Collider grabcollider;
    //Rigidbody for Grabbed Object
    public Rigidbody grabbedrb;
    //Location for the Grabbed Object to Go
    public Transform inhand;
    //Bool to Show Something is Currently Grabbed
    public bool grabbing;
    //Bool to make sure player doesn't immediately throw after grabbing
    public bool grabbed;
    //Trajectory Line
    public TrajectoryLine traj;
    public bool trajbool;
    //Vector and Force Multiplier for Drop Action (1,1,0) and 120 work for simple drop in objects with Mass of 1 and size of standard unity cube
    public Vector3 dropvec;
    public float dropforce;
    public bool charging;
    public bool chargecancel;
    public bool dropping;
    public GameObject cameraobj;
    //Tutorial Bools
    public bool nomove;
    public bool nograb;
    public bool nodrop;
    public bool nocharge;
    public bool nospin;



    void Start(){
        //Sets up necessary colliders and components
        playerAudio = GetComponent<PlayerAudioManager>();
        m_Animator = gameObject.transform.GetChild(0).GetComponent<Animator>();
        elapsedTime = 0;
        playerrb = gameObject.GetComponent<Rigidbody>();
        grabrange = gameObject.transform.GetChild(0).GetComponent<Collider>();
        inhand = gameObject.transform.GetChild(1);

        //Sets the Camera's controls and orientation to the state when it is being held by the player
        if (grabbedobject)
        {
            if (grabbedobject.name == "CameraObj")
            {
                grabbedrb = grabbedobject.GetComponent<Rigidbody>();
                grabbedrb.isKinematic = true;
                grabbedobject.transform.GetChild(0).GetComponent<CameraMovement>().grabbed = true;
                grabbedobject.transform.GetChild(0).eulerAngles = new Vector3(transform.eulerAngles.x, (transform.eulerAngles.y), transform.eulerAngles.z);
                grabbing = true;
                grabcollider.enabled = true;
            }
        }
        else
        {
            grabbing = false;
            grabbed = false;
        }
        
        dropvec = new Vector3 (0, 1, 0);
        charging = false;

    }


    void Update()
    {
        if (inControl)
        {
            PlayerMove();  
        }
        else
        {
            m_Animator.SetBool("IsRunning", false);
            m_Animator.SetBool("IsIdle", true);
            playerAudio.WalkSource.Stop();
        }
        
        if (grabbedobject && grabbedrb == null)
        {
            grabbedrb = grabbedobject.GetComponent<Rigidbody>();
        }
    }

    void PlayerMove()
    {
        //Variables that handle player movement and rotation
        var x = Input.GetAxis("HorizontalWASD") * Time.deltaTime * playerSpeed;
        var z = Input.GetAxis("VerticalWASD") * Time.deltaTime * playerSpeed;
        //Rigidbody.AddForce prevents player from clipping through walls
        if (nomove == false)
        {
            playerrb.AddForce((transform.forward) * z * 2000);
        }
        if (nospin == false)
        {
            transform.Rotate(0, (x * 10), 0);
        }
        HandlePlayerMovementAudio(x, z);
        //Sets the direction the player will throw/drop held objects to the player's front
        dropvec = transform.forward;
        dropvec = new Vector3 (dropvec.x, 1, dropvec.z);

        //GRAB MECHANIC
        //Grabs with Space Bar if there is an object in range, the controls are unlocked, and the player isn't already grabbing an object
        if (grabtarget != null && Input.GetKeyDown("space") && grabbing == false && nograb == false)
        {
            Grab();
        }
        if (Input.GetKeyUp("space") && grabbing == true && grabbed == false)
        {
            grabbed = true;
        }
        //By holding Space Bar the player begins charging a throw move if they are holding an object and the controls are unlocked
        if (Input.GetKey("space") && grabbing == true && grabbed == true && nocharge == false)
        {
            if (chargecancel == false)
            {
                Charge();
            }
            
        }
        //When Space Bar is released while a grabbed object is being held and charging has begun, the grabbed object is dropped and the trajectory line is hid
        if (Input.GetKeyUp("space") && grabbing == true && charging == true)
        {
            if (chargecancel == false)
            {
                if (nodrop == false)
                {
                    Drop();
                    traj.TurnOff();
                }
            }
            else
            {
                charging = false;
                chargecancel = false;
            }
        }
        //Allows the player to cancel the charge and drop actions by hitting a Control Key while holding onto Space Bar
        if (dropforce > 120)
        {
            if (Input.GetKey("right ctrl") || Input.GetKey("left ctrl"))
            {
                m_Animator.SetBool("isCharged", false);
                dropforce = 120;
                chargecancel = true;
                traj.TurnOff();
                playerAudio.ChargeSource.Stop();
            }
        }


        //ANIMATIONS
        if (x != 0 || z != 0){

            m_Animator.SetBool("IsRunning", true);
            m_Animator.SetBool("IsIdle",false);
        }
        else{
            m_Animator.SetBool("IsRunning", false); 
            m_Animator.SetBool("IsIdle",true);
        }

       
      
        //PHYSICS
        gameObject.GetComponent<Rigidbody>().velocity = new Vector2(moveX * playerSpeed, gameObject.GetComponent<Rigidbody>().velocity.y);
        
    }

    public bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, distToGround + GroundLayer);

    }

    public void SetControl(bool b)
    {
        inControl = b;
    }

    //The Grab function temporarily suspends gravity for the held object and attaches it to the player
    public void Grab()
    {
        dropping = false;
        grabbedobject = grabtarget;
        grabbedrb = grabbedobject.GetComponent<Rigidbody>();
        grabbedrb.isKinematic = true;
        grabbedobject.transform.position = inhand.position;
        grabbedobject.transform.parent = this.gameObject.transform;
        //Enables a collider that simulates collision for the grabbed object, so the player cannot walk places with the grabbed object clipping through walls
        grabcollider.enabled = true;
        m_Animator.SetBool("isGrabbing", true);
        //Alters camera rotation and movement if the grabbed object is the Camera
        if (grabbedobject.name == "CameraObj")
        {
            grabbedobject.transform.GetChild(0).GetComponent<CameraMovement>().grabbed = true;
            grabbedobject.transform.GetChild(0).eulerAngles = new Vector3(transform.eulerAngles.x, (transform.eulerAngles.y), transform.eulerAngles.z);
        }
        grabbing = true;
    }
    //The Drop function uncouples the grabbed object from the player, reapplies gravity, and applies the dropforce value as a force to grabbed object's Rigidbody
    public void Drop()
    {
        playerAudio.ChargeSource.Stop();
        if (!playerAudio.ThrowSource.isPlaying && playerAudio.ThrowSource.clip != null)
        {
            playerAudio.ThrowSource.Play();
        }
        grabcollider.enabled = false;
        //Sets camera rotation and controls to the ungrabbed state
        if (grabbedobject.name == "CameraObj")
        {
            grabbedobject.transform.GetChild(0).GetComponent<CameraMovement>().grabbed = false;
            grabbedobject.transform.GetChild(0).eulerAngles = new Vector3(transform.eulerAngles.x, (transform.eulerAngles.y), transform.eulerAngles.z);
        }
        grabbedrb.isKinematic = false;
        grabbedrb.AddForce(dropvec * dropforce);
        //StopAllCoroutines();
        StartCoroutine(ThrowAnim());
        m_Animator.SetBool("isGrabbing", false);
        grabbedobject.transform.parent = null;
        grabbedobject = null;
        grabbing = false;
        dropforce = 120;
        charging = false;
        grabbed = false;
        dropping = true;

    }
    //While Space Bar is being held, the Charge function increases the force that will be applied in the Drop function until a set limit
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
            if (!playerAudio.ChargeSource.isPlaying && playerAudio.ChargeSource.clip != null)
            {
                playerAudio.ChargeSource.Play();
            }
        }
            //Enables the Trajectory Line
        traj.DrawProjection();
    }
    //Checks if an object in the player's range is grabbable
    void OnTriggerStay(Collider target)
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
    //Plays the throw animation
    IEnumerator ThrowAnim()
    {
        m_Animator.SetBool("isDropping", true);
        yield return new WaitForSeconds(0.5f);
        trajbool = true;
        m_Animator.SetBool("isDropping", false);
        m_Animator.SetBool("isCharged", false);
    }
    //Sets the camera rotation and controls to the grabbed state
    public void ResetCamera()
    {
        if (grabbedobject)
        {
            grabbedobject.transform.parent = null;
            grabbedobject = null;
            grabbedrb.useGravity = false;
            grabbedrb.isKinematic = false;
            grabbedrb.useGravity = true;
            grabbedrb = null;
        }
        grabbedobject = cameraobj;
        grabbedrb = grabbedobject.GetComponent<Rigidbody>();
        grabbedrb.isKinematic = true;
        cameraobj.transform.position = inhand.position;
        cameraobj.transform.parent = this.gameObject.transform;
        cameraobj.transform.GetChild(0).GetComponent<CameraMovement>().grabbed = true;
        cameraobj.transform.GetChild(0).eulerAngles = new Vector3(transform.eulerAngles.x, (transform.eulerAngles.y), transform.eulerAngles.z);
        grabcollider.enabled = true;
        m_Animator.SetBool("isGrabbing", true);
        grabbing = true;
        grabbed = true;
    }
    //Plays audio while the player is moving
    void HandlePlayerMovementAudio(float HorizontalMovement, float VerticleMovement)
    {
        if (m_Animator.GetBool("IsRunning"))
        {
            if (!playerAudio.WalkSource.isPlaying && playerAudio.WalkSource.clip != null)
            {
                playerAudio.WalkSource.Play();
            }
        }
        else
        {
            if (playerAudio.WalkSource.isPlaying && playerAudio.WalkSource.clip != null)
            {
                playerAudio.WalkSource.Stop();
            }
        }
    }
}
