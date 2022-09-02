/*
* Written by Ivan Kruger.
* This class manages the behaviour of the mini-quests on the Tutorial screen.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using System.Linq;
using System;
using TMPro;

public class Tutorial : MonoBehaviour
{

    public MainMenu mainMenu;

    [Header("Quest1")]
    public GameObject Quest1;

    [Header("Direction Tutorial Buttons")]
    public TutorialButton tutorialButtonW;
    public TutorialButton tutorialButtonA;
    public TutorialButton tutorialButtonS;
    public TutorialButton tutorialButtonD;
    public TutorialButton tutorialButtonUp;
    public TutorialButton tutorialButtonLeft;
    public TutorialButton tutorialButtonDown;
    public TutorialButton tutorialButtonRight;

    [Header("Quest 2")]
    public GameObject Quest2;
    public TutorialButton moveMouseButton;

    [Header("Quest 3")]
    public GameObject Quest3;
    public TutorialButton clickButton;
    public TutorialButton spaceButton;

    [Header("Quest 4")]
    public GameObject Quest4;
    public TutorialButton escapeButton;

    [Header("Progress Indicator")]
    public Slider progressSlider;
    public TextMeshProUGUI progressLabel;

    private float _progress = 0f;

    private IDisposable clickHandler;
    private IDisposable keyHandler;

    void Start()
    {
        Quest1.SetActive(true);
        Quest2.SetActive(false);
        Quest3.SetActive(false);
        Quest4.SetActive(false);

        keyHandler = InputSystem.onEvent.ForDevice<Keyboard>()
            .Where(e => e.type == new FourCC('S', 'T', 'A', 'T'))
            .Call(act =>
            {
                var button = act.GetFirstButtonPressOrNull();

                tutorialButtonW.SetLowlighted();
                tutorialButtonA.SetLowlighted();
                tutorialButtonS.SetLowlighted();
                tutorialButtonD.SetLowlighted();
                tutorialButtonUp.SetLowlighted();
                tutorialButtonLeft.SetLowlighted();
                tutorialButtonRight.SetLowlighted();
                tutorialButtonDown.SetLowlighted();

                spaceButton.SetLowlighted();

                escapeButton.SetLowlighted();

                if (button != null)
                {
                    switch (button.displayName)
                    {
                        case "W":
                        case "Up":
                            tutorialButtonW.SetHighlighted();
                            tutorialButtonUp.SetHighlighted();
                            break;
                        case "A":
                        case "Left":
                            tutorialButtonA.SetHighlighted();
                            tutorialButtonLeft.SetHighlighted();
                            break;
                        case "S":
                        case "Down":
                            tutorialButtonS.SetHighlighted();
                            tutorialButtonDown.SetHighlighted();
                            break;
                        case "D":
                        case "Right":
                            tutorialButtonD.SetHighlighted();
                            tutorialButtonRight.SetHighlighted();
                            break;
                        case "Space":
                            spaceButton.SetHighlighted();
                            break;
                        case "Esc":
                            escapeButton.SetHighlighted();
                            break;
                    }

                    if (button.displayName == "Up" || button.displayName == "W" ||
                        button.displayName == "Down" || button.displayName == "S"||
                        button.displayName == "Left" || button.displayName == "A" ||
                        button.displayName == "Right" || button.displayName == "D")
                    {
                        
                        if(Quest1.activeInHierarchy)
                        {
                            _progress += 0.25f;
                            if(_progress > 1f)
                            {
                                ActivateAndDeactivateQuests(Quest1, Quest2, 0f);
                            }
                        }
                    }

                    if (button.displayName == "Space")
                    {
                        if(Quest3.activeInHierarchy)
                        {
                            if(_progress > 1f)
                            {
                                ActivateAndDeactivateQuests(Quest3, Quest4, 0f);
                                return;
                            }
                            _progress += 0.1f;
                        }
                    }

                    if (button.displayName == "Esc")
                    {
                        if(_progress > 1f)
                        {
                            keyHandler.Dispose();
                            clickHandler.Dispose();
                            StartCoroutine(mainMenu.StartingGame());
                            return;
                        }
                        _progress += 0.1f;
                    }
                }
            });

        clickHandler = InputSystem.onEvent.ForDevice<Mouse>()
            .Where(e => e.type == new FourCC('S', 'T', 'A', 'T'))
            .Call(action =>
            {
                var button = action.GetFirstButtonPressOrNull();

                clickButton.SetLowlighted();

                if (button != null)
                {
                    if (button.displayName == "Left Button")
                    {
                        if(Quest3.activeInHierarchy)
                        {
                            clickButton.SetHighlighted();
                            if(_progress > 1f)
                            {
                                ActivateAndDeactivateQuests(Quest3, Quest4, 0f);
                                return;
                            }
                            _progress += 0.1f;
                        }
                    }
                }
                else
                {
                    if(Quest2.activeInHierarchy)
                    {
                        if(_progress > 1f)
                        {
                            ActivateAndDeactivateQuests(Quest2, Quest3, 0f);
                            return;
                        }
                        _progress += 0.005f;
                    }
                }

            });
    }

    void Update()
    {
        progressSlider.value = _progress;
        progressLabel.SetText($"{_progress:P0}");
    }

    private void ActivateAndDeactivateQuests(GameObject firstQuest, GameObject secondQuest, float progress)
    {
        firstQuest.SetActive(false);
        secondQuest.SetActive(true);
        _progress = progress;
    }

}
