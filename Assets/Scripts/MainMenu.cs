/*
* Written by Emilia Hardej and Safa Chawich.
* Following this tutorial https://youtu.be/YMj2qPq9CP8
* This class manages the behaviour of MainMenu, Credits, and Loading screen.
*/
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;


public class MainMenu : MonoBehaviour
{
    public GameObject loadingScreen;
    public Slider slider;
    public TextMeshProUGUI text;

    //array to hold Quest1, Quest2, Quest3, Quest4
    public GameObject[] tutorialState;

    public void DisplayCredits()
    {
        
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void OnLoadLevel()
    {
        // called by the Start button
        StartCoroutine(BeginTutorial());
    }

    private IEnumerator BeginTutorial()
    {
        loadingScreen.SetActive(true);

        float progress = 0;
        
        while (progress != 100)
        {
            slider.value = progress;
            text.SetText($"{progress * 100f + "%"}");
            yield return null;
        }

        
    }

    public void SelectTutorialState()
    {
        // loop over the tutorial states and activate each one in turn?
        // not sure where to call this
        for (int i = 0; i < tutorialState.Length; i++)
        tutorialState[i].SetActive(true);
    }

    private IEnumerator StartingGame()
    {
        //load scene after the tutorial is completed
        var op = SceneManager.LoadSceneAsync("MainScene", LoadSceneMode.Single);

        while (!op.isDone)
        {
            yield return null;
        }
    }

}
