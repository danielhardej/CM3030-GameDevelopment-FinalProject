using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using TMPro;

public class HUDController : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject backgroundMusicObject;
    private AudioSource backgroundMusic;
    private float backgroundMusicVolume;
    private TextMeshProUGUI scoreLabel;
    private int score;
    private int displayedScore;

    // Start is called before the first frame update
    void Start()
    {
        scoreLabel = GetComponentInChildren<TextMeshProUGUI>();
        score = 0;
        displayedScore = 0;

        backgroundMusic = backgroundMusicObject?.GetComponent<AudioSource>();
        backgroundMusicVolume = backgroundMusic.volume;
    }

    // Update is called once per frame
    void Update()
    {
        if(displayedScore < score)
        {
            displayedScore += 1;
        }
        else if (displayedScore > score)
        {
            displayedScore -= 1;
        }
        
        scoreLabel.SetText($"SCORE: {displayedScore}");
    }

    public void SetScore(int value)
    {
        score = value;
    }

    public void OnPauseGame()
    {
        StartCoroutine(SlowDownGame());
        StartCoroutine(SetPitch(0.9f));
    }

    public void UnpauseGame()
    {
        StartCoroutine(SpeedUpGame());
        StartCoroutine(SetPitch(1f));
    }

    private IEnumerator SpeedUpGame()
    {
        if(pauseMenu.activeInHierarchy)
        {

            var currentVolume = backgroundMusicVolume;
            var step = (currentVolume - backgroundMusicVolume)/240f;

            pauseMenu?.SetActive(false);
            for (int i = 0; i < 240; i++)
            {
                Time.timeScale += 1/240f;
                
                currentVolume -= step;
                backgroundMusic.volume = currentVolume;

                if(Time.timeScale >= 1)
                {
                    Time.timeScale = 1;
                    break;
                }
                yield return null;
            }
        }
    }

    private IEnumerator SlowDownGame()
    {
        if(!pauseMenu.activeInHierarchy)
        {
            var currentVolume = backgroundMusicVolume;
            var step = (currentVolume - backgroundMusicVolume/2f)/240f;

            for (int i = 0; i < 240; i++)
            {
                Time.timeScale -= 1/240f;

                currentVolume -= step;
                backgroundMusic.volume = currentVolume;
                
                if(Time.timeScale <= 0)
                {
                    Time.timeScale = 0;
                    break;
                }
                yield return null;
            }
            pauseMenu?.SetActive(true);
        }
    }

    private IEnumerator SetPitch(float pitch)
    {

        if(backgroundMusic != null)
        {
            pitch = Mathf.Clamp(pitch, 0f, 1f);

            var currentPitch = backgroundMusic.pitch;
            var step = (currentPitch - pitch)/240;

            for (int i = 0; i < 240; i++)
            {
                currentPitch -= step;
                backgroundMusic.pitch = currentPitch;
                yield return null;
            }
        }
    }
}
