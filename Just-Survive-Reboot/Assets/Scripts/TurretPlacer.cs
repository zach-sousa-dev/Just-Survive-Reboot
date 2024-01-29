using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretPlacer : MonoBehaviour
{
    private Camera cam;

    [Header("Settings")]
    [SerializeField] private float range;
    [SerializeField] private GameObject turretPrefab;
    [SerializeField] private CheckIfClear turretPreview;
    [SerializeField] private GameObject player;
    [SerializeField] private AudioClip placementSound;
    [SerializeField] private AudioClip failureSound;
    [SerializeField] private LayerMask layerMask;
    private AudioSource src;
    private bool toggleOn = true;

    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        src = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.G)) {
            toggleOn = !toggleOn;
        }

        turretPreview.SetVisisbility(false);
        if (toggleOn) {
            RaycastHit hit;
            
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, range, layerMask)) {
                turretPreview.SetVisisbility(true);
                turretPreview.transform.position = hit.point;
                if(Input.GetKeyDown(KeyCode.Mouse0)) {
                    if(!turretPreview.IsColliding()) {
                        Instantiate(turretPrefab, hit.point, turretPreview.transform.rotation);
                        src.PlayOneShot(placementSound);
                    } else {
                        src.PlayOneShot(failureSound);
                    }
                    
                }
                //turretPreview.transform.rotation = Quaternion.Euler(0, player.transform.rotation.y, 0);
            }
        }
    }
}
