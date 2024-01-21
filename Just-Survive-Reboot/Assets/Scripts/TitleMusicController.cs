using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleMusicController : MonoBehaviour
{
    [SerializeField] private AudioClip intro;
    [SerializeField] private AudioClip loop;
    private AudioSource src;

    // Start is called before the first frame update
    void Start()
    {
        src = GetComponent<AudioSource>();
        src.loop = false;
        src.clip = intro;
        src.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (src.isPlaying == false && src.loop == false) {
            src.clip = loop;
            src.loop = true;
            src.Play();
        }
    }
}
