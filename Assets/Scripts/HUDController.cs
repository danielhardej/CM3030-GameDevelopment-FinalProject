using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using TMPro;

public class HUDController : MonoBehaviour
{
    public GameObject pauseMenu;
    public AudioMixer audioMixer;
    private TextMeshProUGUI scoreLabel;
    private int score;
    private int displayedScore;

    // Start is called before the first frame update
    void Start()
    {
        scoreLabel = GetComponentInChildren<TextMeshProUGUI>();
        score = 0;
        displayedScore = 0;
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
        StartCoroutine(SetLowPassFrquency(100f));
    }

    public void UnpauseGame()
    {
        StartCoroutine(SpeedUpGame());
        StartCoroutine(SetLowPassFrquency(22000f));
    }

    private IEnumerator SpeedUpGame()
    {
        if(pauseMenu.activeInHierarchy)
        {
            pauseMenu?.SetActive(false);
            for (int i = 0; i < 240; i++)
            {
                Time.timeScale += 1/240f;
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
            for (int i = 0; i < 240; i++)
            {
                Time.timeScale -= 1/240f;
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

    private IEnumerator SetLowPassFrquency(float freq)
    {
        audioMixer.GetFloat("BG-LowpassFreq", out var currentFreq);
        var step = (currentFreq - freq)/240;

        for (int i = 0; i < 240; i++)
        {
            currentFreq -= step;
            audioMixer.SetFloat("BG-LowpassFreq", currentFreq);
            yield return null;
        }
    }
}
