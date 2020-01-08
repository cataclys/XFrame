using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XFrame.UI;
using UniRx;

public class MessageInput : UIView
{
    public InputField InputField;
    public Button BtnOK;
    public Button BtnClose;
    public Text TitleText;

    public string GetText()
    {
        return InputField.text;
    }

    public void SetTitle(string titleName)
    {
        TitleText.text = titleName;
    }

    public override void Hide()
    {
        Destroy(gameObject);
    }
}
