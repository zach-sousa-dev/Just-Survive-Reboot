using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController cc;
    private Vector3 move = Vector3.zero;
    private Vector3 rawInput = Vector3.zero;
    private Vector3 processedInput = Vector3.zero;

    private float mouseSens = 1f;

    [Header("Ground Control")]
    [SerializeField] private float runningSpeed = 10f;
    [SerializeField] private float walkingSpeed = 10f;
    [SerializeField] private bool shiftInversion = false;
    private bool isRunning = false;
    private float movementSpeed = 10f;

    [Header("In-Air Movement")]
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float gravity = 5f;
    [SerializeField] private float yVel = 0;
    [SerializeField] private float airMovementAccel = 0.5f;

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
        rawInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));                        //get the axis from WASD

        processedInput = transform.forward * (Mathf.Sign(rawInput.z) * 1) + transform.right * (Mathf.Sign(rawInput.x) * 1);     //I don't like the gliding effect of Unity's input system. This forces the inputs to be 1 or -1.
        if (rawInput.z == 0) {
            processedInput = transform.forward * 0 + transform.right * (Mathf.Sign(rawInput.x) * 1);                         //deal with edge cases of zeros
        }
        if (rawInput.x == 0) {
            processedInput = transform.forward * (Mathf.Sign(rawInput.z) * 1) + transform.right * 0;
        }
        if (rawInput.z == 0 && rawInput.x == 0) {
            processedInput = transform.forward * 0 + transform.right * 0;
        }
        processedInput.Normalize();
        if (cc.isGrounded) {
            if (Input.GetKey(KeyCode.LeftShift) == !shiftInversion) {
                movementSpeed = runningSpeed;
                isRunning= true;//currently not used but should still be here
            } else {
                movementSpeed = walkingSpeed;
                isRunning= false;
            }
            move = processedInput * movementSpeed;    //multiply by movement speed
        } else {
            move = Vector3.Lerp(move, processedInput * (movementSpeed), airMovementAccel * Time.deltaTime);
        }



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



        cc.Move(move * Time.deltaTime);                                                     //execute move
    }


    /**
     * enable/disableRunInversion
     * These are used to toggle whether or not we want running to be default or bound to shift
     */
    public void enableRunInversion() {
        shiftInversion = true;
    }
    public void disableRunInversion() {
        shiftInversion = false;
    }
}
