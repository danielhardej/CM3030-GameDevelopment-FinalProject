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
    public GameObject creditScreen;

    public void ToggleCredits()
    {
        creditScreen.SetActive(!creditScreen.activeInHierarchy);
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

        yield return null;
    }

    public IEnumerator StartingGame()
    {
        //load scene after the tutorial is completed
        var op = SceneManager.LoadSceneAsync("MainScene", LoadSceneMode.Single);

        while (!op.isDone)
        {
            yield return null;
        }
    }

}
