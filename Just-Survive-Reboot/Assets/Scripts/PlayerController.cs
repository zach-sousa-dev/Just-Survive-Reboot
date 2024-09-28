using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController cc;
    private Vector3 move = Vector3.zero;
    private Vector3 rawInput = Vector3.zero;
    private Vector3 processedInput = Vector3.zero;

    [Header("Control")]
    [SerializeField] private float mouseSens = 1f;
    [SerializeField] private float recoilLerpTime = 1f;
    [SerializeField] private float recoilEndpointTolerance = 0.1f;

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
    [field: SerializeField] private float headRadius;
    [field: SerializeField] private LayerMask ignoreMask;
    [field: SerializeField] private Vector3 headTransform;

    private Camera cam;

    public float RotationX { get; set; }
    [field: SerializeField] public float RecoilX { get; set; }  //  the target recoil rotation
    [field: SerializeField] public float RecoilRotationX { get; set; }  //  the actual recoil rotation (lerped value)
    float t = 0;
    private float recoilStartTime;

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



        RotationX += -Input.GetAxis("Mouse Y") * mouseSens; //  rotation for the camera based on Mouse Y, * sensitivity

        //  VVV RECOIL VVV
        RotationX += RecoilRotationX * Time.deltaTime;  //  rotation for the camera based on lerped recoil
        if (RecoilX != 0)   //  this means that there is currently recoil being applied to the camera
        {
            if(recoilStartTime == -1f)  //  if this is the first frame of recoil
            {
                recoilStartTime = Time.time;    //  reset the initial time so we can start tracking the elapsed, which should only last recoilLerpTime seconds
            }

            float elapsedTime = Time.time - recoilStartTime;  
            t = Mathf.Clamp01(elapsedTime / recoilLerpTime);    //  map t (interpolator) accordingly to the elapsedTime
            RecoilRotationX = Mathf.Lerp(RecoilRotationX, RecoilX, t);  //  apply the lerp

            if (t >= 0.99f)     //  this shot of recoil has completed, reset values
            {
                RecoilRotationX = 0;
                RecoilX = 0;
                recoilStartTime = -1f; //   using -1f to check if its the first frame of recoil next time, could and probably should be a bool flag
            }
        }

        RotationX = Mathf.Clamp(RotationX, -lookXLimit, lookXLimit);                                    //avoid flipping >180 degrees
        cam.transform.localRotation = Quaternion.Euler(RotationX, 0, 0);                                //do camera rotation
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * mouseSens, 0);             //rotate player based on Mouse X



        if (!cc.isGrounded) {
            yVel -= gravity * Time.deltaTime;
            if (Physics.CheckSphere(transform.position + headTransform, headRadius, ignoreMask))
            {
                Debug.Log("kek");
                yVel = -Mathf.Abs(yVel);
            }
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

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position + headTransform, headRadius);
    }
}
