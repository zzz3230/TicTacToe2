#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EditorExt : EditorWindow
{
    [MenuItem("UI/Cheats")]
    public static void ShowWindow()
    {
        Instantiate(Resources.Load(""));
    }
}

#endif