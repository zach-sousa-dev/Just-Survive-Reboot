using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController cc;
    private Vector3 move = Vector3.zero;
    private Vector2 input = Vector2.zero;

    private float mouseSens = 1f;

    [SerializeField] private float movementSpeed = 10f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float gravity = 5f;
    [SerializeField] private float yVel = 0;

    private Camera cam;

    private float rotationX;
    private float lookXLimit = 90f;

    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        cam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));                        //get the axis from WASD
        if(input.magnitude > 1f) {                                                                          //I do this if statement because if I don't it messes with deceleration of the cc
            input.Normalize(); 
        }
        move = transform.forward * input.y + transform.right * input.x;                                     //multiply by movement speed
        move = move * movementSpeed;


        rotationX += -Input.GetAxis("Mouse Y") * mouseSens;                                             //rotation for the camera based on Mouse Y, * sensitivity
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);                                    //avoid flipping >180 degrees
        cam.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);                                //do camera rotation
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * mouseSens, 0);             //rotate player based on Mouse X

        if (!cc.isGrounded) {
            yVel -= gravity * Time.deltaTime;
        } else {
            yVel = (-cc.stepOffset / Time.deltaTime) / 100;
        }

        if (Input.GetKeyDown(KeyCode.Space) && cc.isGrounded) {
            yVel = jumpForce;
        }

        move.y = yVel;

        cc.Move(move * Time.deltaTime * movementSpeed);                                                     //execute move
    }
}
