using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayCollisionNoise : MonoBehaviour
{
    private AudioSource src;
    [SerializeField] private AudioClip sound;

    // Start is called before the first frame update
    void Start()
    {
        src = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision) {
        if(collision.gameObject.tag != "Player")
        src.PlayOneShot(sound);
    }
}
