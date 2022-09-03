using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//This class exists so that the default Editor logic isnt run for the Tutorial Button
//It doesnt need to do anything just prevent the default logic from hiding our custom
//fields.
[CustomEditor(typeof(TutorialButton))]
public class TutorialButtonEditor : Editor
{}
