using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class HUDController : MonoBehaviour
{

    public GameObject pauseMenu;
    public GameObject backgroundMusicObject;

    [Tooltip("Menu fade intime in Frames")]
    public int menuFadeInTime = 240;
    private AudioSource backgroundMusic;
    private float backgroundMusicVolume;
    private TextMeshProUGUI scoreLabel;
    private TextMeshProUGUI timeLabel;
    private TextMeshProUGUI healthLabel;
    private TextMeshProUGUI endTimeLabel;
    private TextMeshProUGUI highScoreLabel;
    private int previousScore;
    private int displayedScore;
    private int currentScore;
    private float displayHealth;
    private float startTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        scoreLabel = GetComponentInChildren<TextMeshProUGUI>();
        previousScore = 0;
        displayedScore = 0;
        currentScore = 0;

        displayHealth = 1f;

        timeLabel = transform.Find("Canvas/TimeLabel").GetComponent<TextMeshProUGUI>();
        healthLabel = transform.Find("Canvas/HealthLabel").GetComponent<TextMeshProUGUI>(); 

        backgroundMusic = backgroundMusicObject?.GetComponent<AudioSource>();
        backgroundMusicVolume = backgroundMusic.volume;
    }

    // Update is called once per frame
    void Update()
    {
        // If there has been a change in score, begin moving the bar
        if (currentScore != previousScore)
        {
            // Each frame moves a fraction of the curve
            displayedScore = Mathf.RoundToInt(Mathf.Lerp(previousScore, currentScore, (Time.time - startTime) * 1.5f));
        }
        else
        {
            // Keep track of when we begun changing scores
            startTime = Time.time;
        }

        // If we have fully interpolated to the current score value, reset the check for a change in values
        if (displayedScore >= currentScore)
        {
            displayedScore = currentScore;
            previousScore = currentScore;
        }

        scoreLabel.SetText($"{displayedScore.ToString("n0")}");
        highScoreLabel.SetText($"{displayedScore.ToString("n0")}");
        timeLabel.SetText(GetFormattedTime());
        endTimeLabel.SetText(GetFormattedTime());
        healthLabel.SetText($"{displayHealth:P0}");

    }

    public void SetScore(int value)
    {
        //Debug.Log("Score set to: " + value);
        currentScore = value;
    }

    public void SetHealth(float value)
    {
        displayHealth = value;
        if(value < 0)
        {
            value = 0;
        }
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

    public void QuitGame()
    {
        SceneManager.LoadScene("MenuScene");
        Time.timeScale = 1;
    }

    public void RestartGame()
    {
        StartCoroutine(RestartingGame());
        Time.timeScale = 1;
    }

    private string GetFormattedTime()
    {
        int hours = Mathf.FloorToInt((Time.timeSinceLevelLoad / 60 / 60) % 60);
        int minutes = Mathf.FloorToInt((Time.timeSinceLevelLoad / 60) % 60);
        int seconds = Mathf.FloorToInt(Time.timeSinceLevelLoad % 60);

        return $"{hours:00} : {minutes:00} : {seconds:00}";
    }

    private IEnumerator RestartingGame()
    {
        var op = SceneManager.LoadSceneAsync("MainScene", LoadSceneMode.Single);

        while(!op.isDone)
        {
            yield return null;  
        }
    }

    private IEnumerator SpeedUpGame()
    {
        if(pauseMenu.activeInHierarchy)
        {

            var currentVolume = backgroundMusicVolume;
            var step = (currentVolume - backgroundMusicVolume)/menuFadeInTime;

            pauseMenu?.SetActive(false);
            for (int i = 0; i < menuFadeInTime; i++)
            {
                Time.timeScale += 1f/menuFadeInTime;
                
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
            var step = (currentVolume - backgroundMusicVolume/2f)/menuFadeInTime;

            for (int i = 0; i < menuFadeInTime; i++)
            {
                Time.timeScale -= 1f/menuFadeInTime;

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
            var step = (currentPitch - pitch)/menuFadeInTime;

            for (int i = 0; i < menuFadeInTime; i++)
            {
                currentPitch -= step;
                backgroundMusic.pitch = currentPitch;
                yield return null;
            }
        }
    }
}
