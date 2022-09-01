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


public class Tutorial : MonoBehaviour
{
    [SerializeField] private Image _img;
    [SerializeField] private Sprite _pressed, _default;

    public bool isActive = false;
    public bool isComplete = false;

    void Start()
    {
        InputSystem.onEvent.ForDevice<Keyboard>()
            .Where(e => e.type == new FourCC('S', 'T', 'A', 'T'))
            .Call(act =>
            {
                var button = act.GetFirstButtonPressOrNull();

                if (button != null)
                {
                    if (button.displayName == "Up" || button.displayName == "W")
                    {
                        Debug.Log("Up Pressed");
                    }

                    if (button.displayName == "Down" || button.displayName == "S")
                    {
                        Debug.Log("Down Pressed");
                    }

                    if (button.displayName == "Left" || button.displayName == "A")
                    {
                        Debug.Log("Left Pressed");
                    }

                    if (button.displayName == "Right" || button.displayName == "D")
                    {
                        Debug.Log("Right Pressed");
                    }

                    if (button.displayName == "Space")
                    {
                        Debug.Log("Space Pressed");
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
                        Debug.Log("Mouse Clicked");
                    }
                }

            });
    }

}
