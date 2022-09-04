/*
* Written by Ivan Kruger.
* This class manages the state of the game including the score and game over condition.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GameController : MonoBehaviour
{

    public static GameController Instance { get; private set; }

    public GameObject HUDController;
    [SerializeField]
    public GameObject GameOverMenu;

    public int score = 0;
    public int scoreModifier = 1;

    public void IncreaseScore(int value)
    {
        if(value > 0)
        {
            score += value;
        }
        HUDController?.SendMessage("SetScore", score * scoreModifier);
    }

    public void UpdatePlayerHealth(float value)
    {
        if(value > 0 && value <= 1)
        {
            HUDController?.SendMessage("SetHealth", value);
        }

        if(value == 0)
        {
            GameOverMenu.gameObject.SetActive(true);
        }
    }

    void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
