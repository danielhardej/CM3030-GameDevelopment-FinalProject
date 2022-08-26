using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void DisplayCredits()
    {
        
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
