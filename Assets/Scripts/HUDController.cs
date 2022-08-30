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
    private TextMeshProUGUI timeLable;
    private int score;
    private int displayedScore;

    // Start is called before the first frame update
    void Start()
    {
        scoreLabel = GetComponentInChildren<TextMeshProUGUI>();
        score = 0;
        displayedScore = 0;

        timeLable = transform.Find("Canvas/TimeLabel").GetComponent<TextMeshProUGUI>(); 

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
        
        scoreLabel.SetText($"{displayedScore.ToString("n0")}");
        timeLable.SetText(GetFormattedTime());
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
