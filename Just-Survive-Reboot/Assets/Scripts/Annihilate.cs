using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Annihilate : MonoBehaviour
{

    [SerializeField] private float timerSeconds;
    private float startTime;
    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.realtimeSinceStartup;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.realtimeSinceStartup > startTime + timerSeconds) {
            Destroy(gameObject);
        }
    }
}
