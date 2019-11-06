using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetOutline : MonoBehaviour
{
    public Outline[] Outlines;
    public void ShowOutline(bool b)
    {
        // 初始化
        //if (Outlines.Length == 0)
        //{
            Outlines = transform.GetComponents<Outline>();
        //}
        if (Outlines.Length == 0)
        {
            Outlines = transform.GetComponentsInChildren<Outline>();
        }
        // 描边
        foreach (var item in Outlines)
        {
            item.enabled = b;
        }
    }
}
