using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TutorialButton))]
public class TutorialButtonEditor : Editor
{
    // public override void OnInspectorGUI()
    // {
    //     base.OnInspectorGUI();

    //     var tutorialButton = new UnityEditor.SerializedObject(target);

    //     EditorGUILayout.PropertyField(tutorialButton.FindProperty("audioSource"));

    //     EditorGUILayout.PropertyField(tutorialButton.FindProperty("highlightedSprite"));
    // }
}
