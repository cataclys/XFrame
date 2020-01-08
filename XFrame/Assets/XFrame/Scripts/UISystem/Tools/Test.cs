using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    public RectTransform rt1;
    public RectTransform rt2;

    private Rect rect;
    CanvasScaler canvaScaler;

    void Awake()
    {
        canvaScaler = GetComponent<CanvasScaler>();
        rect = new Rect(-Screen.width / 2, -Screen.height / 2, Screen.width, Screen.height);
        float scale = canvaScaler.matchWidthOrHeight == 1 ? canvaScaler.referenceResolution.y / (float)Screen.height : canvaScaler.referenceResolution.x / (float)Screen.width;
        rect = new Rect(rect.x * scale, rect.y * scale, rect.width * scale, rect.height * scale);
    }


    void Update()
    {
        if (null == rt1 || null == rt2) { return; }
        MathEx.AnchorTo(rt1, UGUISide.TopLeft, rt2, UGUISide.BottomRight, transform);
        MathEx.SetUIArea(rt1, rect, transform);
    }
}
