using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class SpawnSplatter : MonoBehaviour
{
    private Camera cam;

    [SerializeField] private Enemy target;
    [SerializeField] private GameObject smallSplatter;
    [SerializeField] private GameObject bigSplatter;
    [SerializeField] private LayerMask layerMask;

    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            if (Physics.Raycast(cam.transform.position, target.transform.position - cam.transform.position, out RaycastHit hit, Mathf.Infinity, layerMask)) {
                GameObject splatter = Instantiate(smallSplatter, hit.point + (hit.normal * 0.2f), Quaternion.Euler(hit.normal));
                splatter.transform.SetParent(target.transform);
            }
        }
        

        if (Input.GetMouseButtonDown(1)) {
            if (Physics.Raycast(cam.transform.position, target.transform.Find("Head").transform.position - cam.transform.position, out RaycastHit hit, Mathf.Infinity, layerMask)) {
                GameObject splatter = Instantiate(bigSplatter, hit.point + (hit.normal * 0.2f), Quaternion.Euler(hit.normal));
                splatter.transform.SetParent(target.transform);
            }
        }
    }
}
