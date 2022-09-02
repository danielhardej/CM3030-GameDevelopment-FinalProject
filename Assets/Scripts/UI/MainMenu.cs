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
using System;


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
        if(PlayerPrefs.HasKey("ShowedTutorial"))
        {
            yield return StartGame();

            yield break;
        }

        loadingScreen.SetActive(true);

        PlayerPrefs.SetInt("ShowedTutorial", 1);

        yield return null;
    }

    public IEnumerator StartGame()
    {
        //load scene after the tutorial is completed
        var op = SceneManager.LoadSceneAsync("MainScene", LoadSceneMode.Single);

        while (!op.isDone)
        {
            yield return null;
        }
    }

}
