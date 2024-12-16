using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FOVController : MonoBehaviour
{
    [field: SerializeField] private float DefaultFOV { get; set; }
    [field: SerializeField] public float FOVMultiplier { get; set; }
    [field: SerializeField] private float FOVChangeRate { get; set; }
    private Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        FOVMultiplier = 0;
        cam = gameObject.GetComponent<Camera>();
        cam.fieldOfView = DefaultFOV;
    }

    // Update is called once per frame
    void Update()
    {
        cam.fieldOfView = Mathf.MoveTowards(cam.fieldOfView, DefaultFOV * FOVMultiplier, FOVChangeRate * Time.deltaTime);
    }
}
