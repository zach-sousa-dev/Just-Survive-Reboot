using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckIfClear : MonoBehaviour
{
    [SerializeField] private int collisions = 0;
    [SerializeField] private bool isColliding = false;

    // Update is called once per frame
    void Update()
    {
        if(collisions == 0) {
            isColliding = false;
        } else {
            isColliding = true;
        }
    }

    private void OnCollisionEnter(Collision collision) {
        collisions++;
    }

    private void OnCollisionExit(Collision collision) {
        collisions--;
    }

    public bool IsColliding() {
        return isColliding;
    }
}
