
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ColorBoard : DynamicControl
{
    public List<Toggle> Toggles;
    public Image ShowColor;
    public Slider Diaphaneity;
    public Text DiaphaneityText;

    public ColorEvent onColorChanged;

    private void Awake()
    {
        onColorChanged = new ColorEvent();
        foreach (var item in Toggles)
        {
            item.onValueChanged.AddListener(a =>
            {
                if (!a) return;
                ShowColor.color = new Color(item.GetComponent<Image>().color.r, item.GetComponent<Image>().color.g, item.GetComponent<Image>().color.b, ShowColor.color.a);
                onColorChanged?.Invoke(ShowColor.color);
            });
        }
        Diaphaneity.onValueChanged.AddListener(a =>
        {
            DiaphaneityText.text = $"{Math.Round(a * 100)}%";
            ShowColor.color = new Color(ShowColor.color.r, ShowColor.color.g, ShowColor.color.b, a);
            onColorChanged?.Invoke(ShowColor.color);

        });
    }

    //public void SetData()
    //{
    //    ColorUtility.TryParseHtmlString(item.PropertyValue, out var newColor);
    //    ShowColor.color = newColor;
    //    Diaphaneity.value = newColor.a;
    //}
    public override void SetEnabled(bool b)
    {
        base.SetEnabled(b);
        foreach (var item in Toggles)
        {
            item.interactable = b;
        }
        Diaphaneity.interactable = b;
    }
    public class ColorEvent : UnityEvent<Color>
    {

    }
}
