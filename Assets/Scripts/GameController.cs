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

    public int score = 0;
    public int scoreModifier = 100;

    public void IncreaseScore(int value)
    {
        if(value > 0)
        {
            score += value;
        }
        HUDController?.SendMessage("SetScore", score * scoreModifier);
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
