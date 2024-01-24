using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycler : MonoBehaviour
{
    [Header("Skybox")]
    [SerializeField] private Material skybox;
    [SerializeField] private float rotationSpeed;

    [Header("Light")]
    [SerializeField] private Light sun;
    [SerializeField] private float maxIntensity;
    [SerializeField] private float minIntensity;

    [Header("General")]
    [SerializeField] private float dayLength = 1f;
    [SerializeField] private float dayPercent = 0f;
    private float dayCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        skybox.SetFloat("_Rotation", Time.time * rotationSpeed);

        sun.transform.rotation = Quaternion.Euler(Utilities.map(dayPercent, 0f, 1f, 0f, 359f), sun.transform.rotation.y, sun.transform.rotation.z);
        sun.intensity = Mathf.PingPong(dayCount * dayLength, 1);
        Debug.Log(dayCount*dayLength);

        dayCount = (Time.time / dayLength);
        dayPercent = (Time.time / dayLength) - (int)dayCount;
    }
}
