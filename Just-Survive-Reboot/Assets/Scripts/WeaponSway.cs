using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    [SerializeField] private float swayMultiplier = 1f;
    [SerializeField] private float tiltAmount = 10f;
    [SerializeField] private float tiltSpeed = 10f;
    [SerializeField] private float runTiltAmount = 10f;
    [SerializeField] private float runTiltSpeed = 10f;
    [SerializeField] private float mouseSens = 1f;
    [SerializeField] private float maxSway = 1f;
    [SerializeField] private float swaySpeed = 1f;

    private float rawX;
    private float rotationX;

    private float rawY;
    private float rotationY;

    private float tilt;

    private Vector3 initRotation;


    // Start is called before the first frame update
    void Start()
    {
        initRotation = transform.localRotation.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        rawX = Input.GetAxis("Mouse Y") * mouseSens * swayMultiplier;                                          
        rawX = Mathf.Clamp(rawX, -maxSway, maxSway);

        rotationX = Mathf.Lerp(rotationX, rawX, Time.deltaTime * swaySpeed);

        rawY = -Input.GetAxis("Mouse X") * mouseSens * swayMultiplier;                                            
        rawY = Mathf.Clamp(rawY, -maxSway, maxSway);

        rotationY = Mathf.Lerp(rotationY, rawY, Time.deltaTime * swaySpeed);

        if(!Input.GetKey(KeyCode.LeftShift)) {
            if (Input.GetKey(KeyCode.A)) {
                tilt = Mathf.Lerp(tilt, tiltAmount, Time.deltaTime * tiltSpeed);
            } else if (Input.GetKey(KeyCode.D)) {
                tilt = Mathf.Lerp(tilt, -tiltAmount, Time.deltaTime * tiltSpeed);
            } else {
                tilt = Mathf.Lerp(tilt, 0f, Time.deltaTime * tiltSpeed);
            }
        } else {
            if (Input.GetKey(KeyCode.A)) {
                tilt = Mathf.Lerp(tilt, runTiltAmount, Time.deltaTime * runTiltSpeed);
            } else if (Input.GetKey(KeyCode.D)) {
                tilt = Mathf.Lerp(tilt, -runTiltAmount, Time.deltaTime * runTiltSpeed);
            } else {
                tilt = Mathf.Lerp(tilt, 0f, Time.deltaTime * tiltSpeed);
            }
        }

        transform.localRotation = Quaternion.Euler(initRotation.x + rotationX, initRotation.y + rotationY, initRotation.z + tilt);
    }
}
