using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckIfClear : MonoBehaviour
{
    [SerializeField] private int collisions = 0;
    [SerializeField] public bool isColliding = false;
    private bool visible = false;
    [SerializeField] private Vector3 boxOffset;
    [SerializeField] private LayerMask checkForLayers;
    [SerializeField] private Material mtrl;
    [SerializeField] private Color good;
    [SerializeField] private Color bad;

    void Start() {
        
    }

    // Update is called once per frame
    void Update()
    {
        Collider[] colliders = Physics.OverlapBox(transform.position + boxOffset, transform.localScale, Quaternion.identity, checkForLayers);
        Debug.Log(colliders.Length + " is the lengt");
        if(colliders.Length > 0) {
            isColliding = true;
            mtrl.color = bad;
        } else {
            isColliding = false;
            mtrl.color = good;
        }

        foreach(MeshRenderer mr in GetComponentsInChildren<MeshRenderer>()) {
            mr.enabled = visible;
        }

        foreach (Collider c in colliders) {
            Debug.Log(c);
        }
    }

    public bool IsColliding() {
        return isColliding;
    }

    public void SetVisisbility(bool b) {
        visible = b;
    }

    private void OnDrawGizmos() {
        Gizmos.DrawWireCube(transform.position + boxOffset, transform.localScale);
    }

}
