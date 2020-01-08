using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XFrame.UI;
using UniRx;

public class UIViewMessageYesNo : UIView
{
    public Text Title;
    public Text Content;
    public Button BtnOk;
    public Button BtnNo;

    public override void Hide()
    {
        Destroy(gameObject);
    }

    public void SetTitle(string title)
    {
        Title.text = title;
    }
}
