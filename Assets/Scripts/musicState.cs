using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class musicState : MonoBehaviour
{
    [SerializeField, Tooltip("Player GameObject")]
    GameObject player;
    PlayerStateMachine playerState;

    [Header("Music")]
    [SerializeField, Tooltip("List of music to cycle through")]
    List<AudioClip> inGameMusic;
    [SerializeField, Tooltip("Game over music")]
    AudioClip GameOver;

    AudioSource audioSource;

    private int songRotation;
    private bool gameOver;

    // Start is called before the first frame update
    void Start()
    {
        playerState = player.GetComponent<PlayerStateMachine>();
        audioSource = GetComponent<AudioSource>();
        songRotation = 0;
        gameOver = false;

        StartCoroutine(PlayMusic());
    }

    // Update is called once per frame
    void Update()
    {
        // If the player is dead
        if(playerState.isPlayerDead && !gameOver)
        {
            //Debug.Log("Player dead");
            // Stop the main game song rotation
            StopCoroutine(nameof(PlayMusic));
            // Stop playing the current song
            audioSource.Stop();
            // Set this flag true so the code only fires once
            gameOver = true;
            // Change to our GameOver music
            audioSource.volume = 0.5f;
            audioSource.clip = GameOver;
            // Play the song
            audioSource.Play();
        }
    }

    IEnumerator PlayMusic()
    {
        audioSource.volume = 0.24f;
        // Get the next clip to play
        AudioClip clip = inGameMusic[songRotation];

        // Play selected song
        //Debug.Log("Now playing... " + clip.name);
        audioSource.clip = clip;
        audioSource.Play();

        // Increment to select the next song
        songRotation++;

        // If we've incremented too far, loop back around
        if (songRotation >= inGameMusic.Count)
        {
            songRotation = 0;
        }

        // Get the length of the selected song
        float songLength = clip.length;
        //Debug.Log("Waiting " + songLength + " until next song...");

        // Wait that long before playing the next one
        yield return new WaitForSeconds(songLength + 1);

        // Play the next song!
        //Debug.Log("Looping back!");
        StartCoroutine(PlayMusic());
    }
}
