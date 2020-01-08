using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultipeLine : MonoBehaviour
{
    RectTransform lineRect;
    BoxCollider2D lineBox;
    Vector2 oldSize;

    private void Start()
    {
        lineRect = GetComponent<RectTransform>();
        lineBox = GetComponent<BoxCollider2D>();
    }
    private void FixedUpdate()
    {
        if (lineRect.sizeDelta!=oldSize)
        {
            lineBox.size = lineRect.sizeDelta;
            lineBox.offset = new Vector2(lineRect.sizeDelta.x / 2, 0);
            oldSize = lineRect.sizeDelta;
        }
    }
}
