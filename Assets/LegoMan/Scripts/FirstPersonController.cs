using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FirstPersonController : MonoBehaviour
{
    /// <summary>
    /// Move the player charactercontroller based on horizontal and vertical axis input
    /// </summary>
    [HideInInspector]
    public float yVelocity = 0f;
    [Range(-5f,-25f)]
    public float gravity = -15f;
    //the speed of the player movement
    [Range(5f,15f)]
    public float movementSpeed = 10f;
    //jump speed
    [Range(5f,15f)]
    public float jumpSpeed = 10f;

    //now the camera so we can move it up and down
    public Transform pitchTransform;
    float pitch = 0f;
    [Range(1f,90f)]
    public float maxPitch = 85f;
    [Range(-1f, -90f)]
    public float minPitch = -85f;
    [Range(0.5f, 5f)]
    public float mouseSensitivity = 2f;

    //variable for the shooting and delay
    float shootDelay = 0.5f;
    float shootTimer = 0f;
    public LaserGun laserGun;


    //the charachtercompononet for moving us
    CharacterController cc;

    //add in the animator- should probably be done in seperate class but this is less complicated
    public Animator playerAnimator;

    private void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Look();
        Move();
        Shoot();
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
        pitchTransform.localRotation = rot;
    }

    void Move()
    {
        //update speed based onn the input
        Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        input = Vector3.ClampMagnitude(input, 1f);
        //set animator if true moving, false if not
        bool moving = input.magnitude > 0.01f ? true : false;
        playerAnimator.SetBool("moving", moving);
        //transofrm it based off the player transform and scale it by movement speed
        Vector3 move = transform.TransformVector(input) * movementSpeed;
        //is it on the ground
        if (cc.isGrounded)
        {
            yVelocity = 0;
            //check for jump here
            if (Input.GetButtonDown("Jump"))
            {
                yVelocity = jumpSpeed;
                //trigger animation
                playerAnimator.SetTrigger("jump");
            }
        }
        //now add the gravity to the yvelocity
        yVelocity += gravity * Time.deltaTime;
        move.y = yVelocity;
        //and finally move
        cc.Move(move * Time.deltaTime);
    }


    public void Shoot()
    {
        shootTimer += Time.deltaTime;
        if (Input.GetButtonDown("Fire1") && shootTimer > shootDelay)
        {
            shootTimer = 0f;
            playerAnimator.SetTrigger("shoot");
            laserGun.Shoot();
        }
    }


}
