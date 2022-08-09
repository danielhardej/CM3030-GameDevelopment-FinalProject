using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUDController : MonoBehaviour
{

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
}
