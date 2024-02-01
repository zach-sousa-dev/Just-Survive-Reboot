using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    [SerializeField] private float swayMultiplier = 1f;
    [SerializeField] private float mouseSens = 1f;
    [SerializeField] private float maxSway = 1f;
    [SerializeField] private float swaySpeed = 1f;

    private float rawX;
    private float rotationX;

    private float rawY;
    private float rotationY;

    private Vector3 initRotation;


    // Start is called before the first frame update
    void Start()
    {
        initRotation = transform.localRotation.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        rawX = -Input.GetAxis("Mouse Y") * mouseSens * swayMultiplier;                                             //rotation for the camera based on Mouse Y, * sensitivity
        rawX = Mathf.Clamp(rawX, -maxSway, maxSway);

        rotationX = Mathf.Lerp(rotationX, rawX, Time.deltaTime * swaySpeed);

        rawY = -Input.GetAxis("Mouse X") * mouseSens * swayMultiplier;                                             //rotation for the camera based on Mouse Y, * sensitivity
        rawY = Mathf.Clamp(rawY, -maxSway, maxSway);

        rotationY = Mathf.Lerp(rotationY, rawY, Time.deltaTime * swaySpeed);

        transform.localRotation = Quaternion.Euler(initRotation.x + rotationX, initRotation.y + rotationY, initRotation.z);
    }
}
