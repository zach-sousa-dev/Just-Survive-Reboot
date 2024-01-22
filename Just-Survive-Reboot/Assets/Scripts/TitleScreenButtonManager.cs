using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class TitleScreenButtonManager : MonoBehaviour
{
    //all the UI element references
    [Header("Canvases")]
    [SerializeField] private Canvas settingsScreen;
    [SerializeField] private Canvas playScreen;

    [Header("Buttons")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button backButton;

    [Header("Sliders")]
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider volSlider;

    [Header("TMP InputFields")]
    [SerializeField] private TMP_InputField mouseSensField;

    [Header("Sounds")]
    [SerializeField] private AudioClip selectSound;
    private AudioSource src;

    private bool showSettings = false;  //controls whether to show the settings screen


    // Start is called before the first frame update
    void Start()
    {   
        //buttons
        playButton.onClick.AddListener(PlayGame);
        playButton.onClick.AddListener(playSound);

        settingsButton.onClick.AddListener(enterSettings);
        settingsButton.onClick.AddListener(playSound);

        backButton.onClick.AddListener(exitSettings);
        backButton.onClick.AddListener(playSound);


        src = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //show settings logic
        if(showSettings) {
            settingsScreen.enabled = true;
            playScreen.enabled = false;
        } else {
            settingsScreen.enabled = false;
            playScreen.enabled = true;
        }
    }

    /**
     * PlayGame
     * Loads the "Game" scene
     */
    void PlayGame() {
        SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }

    /**
     * enterSettings
     * Enables the settings screen
     */
    void enterSettings() {
        showSettings = true;
    }

    /**
     * exitSettings
     * Closes the settings screen
     */
    void exitSettings() {
        showSettings = false;
    }

    /**
     * playSound
     * Plays the select sound
     */
    void playSound() {
        src.PlayOneShot(selectSound);
    }
}
