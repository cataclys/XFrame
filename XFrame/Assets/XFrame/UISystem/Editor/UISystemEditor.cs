using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class UISystemEditor : Editor
{
    [MenuItem("GameObject/UI/UICanvas")]
    static void CreateUICanvas()
    {
        UISystem.CreateUICanvas("UICanvas");
    }

    [MenuItem("GameObject/UI/UIView")]
    static void CreateUIView()
    {
        UISystem.CreateUICanvas("UICanvas");
    }
}
