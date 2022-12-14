using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

[RequireComponent(typeof(CharacterController))]
public class FirstPersonController : NetworkBehaviour
{
    /// <summary>
    /// Move the player charactercontroller based on horizontal and vertical axis input
    /// </summary>
    /// 
    [SerializeField] private PlayerHealth playerhealth;

    float yVelocity = 0f;
    [Range(5f, 25f)]
    public float gravity = 15f;
    //the speed of the player movement
    [Range(5f, 15f)]
    public float movementSpeed = 10f;
    //jump speed
    [Range(5f, 15f)]
    public float jumpSpeed = 10f;

    //now the camera so we can move it up and down
    public Transform cameraTransform;
    float pitch = 0f;
    [Range(1f, 90f)]
    public float maxPitch = 85f;
    [Range(-1f, -90f)]
    public float minPitch = -85f;
    [Range(0.5f, 10f)]
    public float mouseSensitivity = 2f;

    public Animator animator;

    //the charachtercompononet for moving us
    CharacterController cc;

    private void Start()
    {
        cc = GetComponent<CharacterController>();
        //cameraTransform = GetComponentInChildren<Camera>().transform;
        if (!isLocalPlayer)
        {
            GetComponentInChildren<Camera>().enabled = false;
            //GetComponentInChildren<AudioListener>().enabled = false;
        }
        //Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (isLocalPlayer)
        {

            Look();
            if (playerhealth.isDead != true)
            {
                Move();
            }

        }
    }

    void Look()
    {
        //get the mouse inpuit axis values
        float xInput = Input.GetAxis("Mouse X") * mouseSensitivity;
        float yInput = Input.GetAxis("Mouse Y") * mouseSensitivity;
        //turn the whole object based on the x input
        transform.Rotate(0, xInput, 0);
        //now add on y input to pitch, and clamp it
        pitch -= yInput;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
        //create the local rotation value for the camera and set it
        Quaternion rot = Quaternion.Euler(pitch, 0, 0);
        cameraTransform.localRotation = rot;
    }

    void Move()
    {
        //update speed based onn the input
        Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        input = Vector3.ClampMagnitude(input, 1f);

        //if (input.magnitude > 0.1f)
        //{
        //    animator.SetBool("moving", true);
        //}
        //else
        //{
        //    animator.SetBool("moving", false);
        //}
        //evaluate the moving parameter in animator
        animator.SetBool("moving", input.magnitude > 0.1f);

        //transofrm it based off the player transform and scale it by movement speed
        Vector3 move = transform.TransformVector(input) * movementSpeed;
        //is it on the ground
        if (cc.isGrounded)
        {
            yVelocity = -gravity * Time.deltaTime;
            //check for jump here
            if (Input.GetButtonDown("Jump"))
            {
                yVelocity = jumpSpeed;
                animator.SetTrigger("jump");
                GetComponent<NetworkAnimator>().SetTrigger("jump");
            }
        }
        //now add the gravity to the yvelocity
        yVelocity -= gravity * Time.deltaTime;
        move.y = yVelocity;
        //and finally move
        cc.Move(move * Time.deltaTime);
    }



}
