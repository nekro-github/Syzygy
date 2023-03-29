using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerController : MonoBehaviour {
    //public variables
    [SerializeField] private Transform cam;
    private Transform camParent;
    [SerializeField] private Vector3 startRotation = new Vector3(0,0,0);
    [SerializeField] private float speed = 5;
    [SerializeField] private float sprintSpeed = 10;
    private float speedBeforeJump = 5;
    [SerializeField] private float jumpForce = 7;
    //private variables
    private Vector3 rot;
    private bool onGround = false;
    private double lastJumpTime;
    [SerializeField] private float maxOrientateRotateDegrees = 1.25f;
    [HideInInspector] public bool snapOrientation = true;
    [HideInInspector] public bool Orient = true;

    private float InputHorizontal = 0;
    private float InputVertical = 0;
    private float InputMouseX = 0;
    private float InputMouseY = 0;
    //scripts on self
    private Rigidbody rb;
    private UIController controller;
    //animator variables
    Animator animator;
    int isWalkingHash;
    int isRunningHash;

    #if UNITY_EDITOR
    void OnValidate() {
        if (Application.isEditor) {
            cam.localRotation = Quaternion.Euler(startRotation.x,0,0);
            cam.parent.localRotation = Quaternion.Euler(0,startRotation.y,0);
        }
    }
    #endif
    void Start() {
        //init regular variables
        rb = GetComponent<Rigidbody>();
        controller = GetComponent<UIController>();
        camParent = cam.parent;
        rot = startRotation;
        lastJumpTime = Time.realtimeSinceStartup;
        //init animation variables
        animator = GameObject.Find("Astronaut").GetComponent<Animator>();
        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");
    }

    void Update() {
        //get inputs
        InputHorizontal = Input.GetAxisRaw("Horizontal");
        InputVertical = Input.GetAxisRaw("Vertical");
        InputMouseX = Input.GetAxisRaw("Mouse X");
        InputMouseY = Input.GetAxisRaw("Mouse Y");
        //animation stuff
        bool isMoving = (InputHorizontal != 0 || InputVertical != 0) && !controller.isPaused;
        bool isRunning = isMoving && (onGround ? Input.GetKey(KeyCode.LeftShift) : (speedBeforeJump == sprintSpeed));
        bool isWalking = isMoving && !isRunning;
        animator.SetBool("isWalking", isWalking);
        animator.SetBool("isRunning", isRunning);
        //check that the game isnt paused in some way
        if (!controller.isPaused && !controller.teleportMenuOpen) {
            //looking around
            rot = new Vector3(Mathf.Clamp(rot.x-InputMouseY,-90,90),(rot.y+InputMouseX)%360,0);//set rotation vector based on mouse movement
            //jumping
            if (onGround) {
                if (Input.GetKeyDown("space")) {
                    //if player is on the ground and presses space add a force upwards
                    rb.AddForce(transform.up*jumpForce,ForceMode.Impulse);
                    lastJumpTime = Time.realtimeSinceStartup;
                    onGround = false;
                    speedBeforeJump = ((Input.GetKey(KeyCode.LeftShift)) ? sprintSpeed : speed);//saves speed player was moving before jump
                }
            }
            if (Time.realtimeSinceStartup-lastJumpTime > 0.125) {
                // sends ray down relative to the player and checks if there is anything within 1.1 meters
                RaycastHit hit;
                if (Physics.Raycast(transform.position, -transform.up, out hit, 1.1f)) { onGround = true; }
                else { onGround = false; }// if cant find the ground set onGround to false
            } else onGround = false;
        }

        cam.localRotation = Quaternion.Euler(rot.x,0,0);// player rotation based on rotation vector
        camParent.localRotation = Quaternion.Euler(0,rot.y,0);
    }
    void FixedUpdate() {
        //moving
        float vy = Vector3.Dot(rb.velocity,transform.up);//get vertical velocity
        // if the player is on the ground change the speed based on if the player is holding shift, 
        // if not on ground use the speed it was going last before jumping.
        // finally multiply by a scale factor
        float curSpeed = (onGround ? ((Input.GetKey(KeyCode.LeftShift) && onGround) ? sprintSpeed : speed) : speedBeforeJump ) * 50;
        curSpeed = (!controller.isPaused && !controller.teleportMenuOpen && (InputVertical != 0 || InputHorizontal != 0)) ? curSpeed : 0;// check that the game isnt paused in some way
        rb.velocity = (camParent.right*InputHorizontal + camParent.forward*InputVertical).normalized*(Time.fixedDeltaTime*curSpeed) + transform.up*vy;// normalized move vector multiplied with deltaTime and the speed of the player

        //orienting
        if (Orient) {
            Vector3 gravity = Vector3.zero;
            Planet strongest = null;
            float strongestGrav = 0;
            Planet[] planets = FindObjectsOfType(typeof(Planet)) as Planet[];
            for(int i = 0; i < planets.Length; i++) {
                Vector3 vec = (planets[i].transform.position-transform.position);
                float grav = (  Constants.G*planets[i].mass / (planets[i].radius*((vec.magnitude-0.5f)*2/planets[i].Scale)).Pow(2.0f)).asFloat;//calculate strength of gravity
                if (grav > 0.0625f) {
                    gravity += vec.normalized * grav;// add gravity in the correct direction to the overall gravity vector
                    if (grav > strongestGrav && grav > 0.25f) {//find planet with strongest gravity(used for orientation)
                        strongestGrav = grav;
                        strongest = planets[i];
                    }
                }
            }
            Physics.gravity = gravity;// set unity gravity
            if (strongest != null) {
                print(strongest.transform.name + ": " + strongestGrav);
                //code for orienting the strongest gravity planet
                Vector3 vec = (strongest.transform.position-transform.position).normalized;//vector from player to planet
                Quaternion finalRot = Quaternion.FromToRotation(transform.up,-vec)*transform.rotation;//goal rotation
                if (!snapOrientation) { transform.rotation = Quaternion.RotateTowards(transform.rotation, finalRot, maxOrientateRotateDegrees); }// regular turning only a few degrees at a time
                else transform.rotation = Quaternion.RotateTowards(transform.rotation, finalRot, 360); snapOrientation = false;// instead of slowling turning to the direction of the planets gravitational field it will snap to it
            }
        } else Physics.gravity = Vector3.up*-9.81f;
    }
}
