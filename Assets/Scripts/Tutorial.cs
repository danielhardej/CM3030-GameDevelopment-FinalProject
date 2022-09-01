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
using TMPro;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private Image _img;
    [SerializeField] private Sprite _pressed, _default;

    public GameObject Quest1;
    public GameObject Quest2;
    public GameObject Quest3;
    public GameObject Quest4;

    public Slider progressSlider;

    public TextMeshProUGUI progressLabel;

    public bool isActive = false;
    public bool isComplete = false;

    void Start()
    {
        Quest1.SetActive(true);
        Quest2.SetActive(false);
        Quest3.SetActive(false);
        Quest4.SetActive(false);

        InputSystem.onEvent.ForDevice<Keyboard>()
            .Where(e => e.type == new FourCC('S', 'T', 'A', 'T'))
            .Call(act =>
            {
                var button = act.GetFirstButtonPressOrNull();

                if (button != null)
                {
                    if (button.displayName == "Up" || button.displayName == "W" ||
                        button.displayName == "Down" || button.displayName == "S"||
                        button.displayName == "Left" || button.displayName == "A" ||
                        button.displayName == "Right" || button.displayName == "D")
                    {
                        if(Quest1.activeInHierarchy == true)
                        {
                            ActivateAndDeactivateQuests(Quest1, Quest2, 0.25f);
                        }
                    }

                    if (button.displayName == "Space")
                    {
                        if(Quest3.activeInHierarchy)
                        {
                            ActivateAndDeactivateQuests(Quest3, Quest4, 0.75f);
                        }
                    }

                    if (button.displayName == "Esc")
                    {
                        Debug.Log("Esc Pressed");
                    }
                }
            });

        InputSystem.onEvent.ForDevice<Mouse>()
            .Where(e => e.type == new FourCC('S', 'T', 'A', 'T'))
            .Call(action =>
            {
                var button = action.GetFirstButtonPressOrNull();

                if (button != null)
                {
                    if (button.displayName == "Left Button")
                    {
                        if(Quest3.activeInHierarchy)
                        {
                            ActivateAndDeactivateQuests(Quest3, Quest4, 0.75f);
                        }
                    }
                }
                else
                {
                    if(Quest2.activeInHierarchy)
                    {
                        ActivateAndDeactivateQuests(Quest2, Quest3, 0.5f);
                    }
                }

            });
    }

    private void ActivateAndDeactivateQuests(GameObject firstQuest, GameObject secondQuest, float progress)
    {
        firstQuest.SetActive(false);
        secondQuest.SetActive(true);
        progressSlider.SetValueWithoutNotify(progress);
        progressLabel.SetText($"{progress:P0}");
    }

}
