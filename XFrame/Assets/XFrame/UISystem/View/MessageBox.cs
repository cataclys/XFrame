using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using DG.Tweening;
using UniRx;
using XLua;
[LuaCallCSharp]
public class MessageBox : UIView
{

    public Text TextMsg;
    public GameObject Background;
    public Image DialogBox;
    public GameObject ButtonOK;
    public Button BtnOK;
    public GameObject ButtonYesNo;
    public Button BtnYes;
    public Button BtnNo;

    //public override UIViewType ViewType { get; set; } = UIViewType.None;

    protected override void Awake()
    {
        base.Awake();
        BtnOK.OnClickAsObservable().Subscribe(a => { UIManager.Instance.DestroyView<MessageBox>(ViewName); }).AddTo(gameObject);
        BtnYes.OnClickAsObservable().Subscribe(a => { UIManager.Instance.DestroyView<MessageBox>(ViewName); }).AddTo(gameObject);
        BtnNo.OnClickAsObservable().Subscribe(a => { UIManager.Instance.DestroyView<MessageBox>(ViewName); }).AddTo(gameObject);
    }
    [LuaCallCSharp]
    public static void Show(string msg)
    {
        Show(msg, Color.white, 18, MessageBoxType.DialogBox_OK, null, null);
    }
    public static void Show(string msg, UnityAction yes, UnityAction no)
    {
        Show(msg, Color.white, 18, MessageBoxType.DialogBox_YesNo, yes, no);
    }
    public static void Show(string msg,int fontSize)
    {
        Show(msg, Color.white, fontSize, MessageBoxType.PromptBox, null, null);
    }
    public static void Show(string msg, Color color, int fontSize)
    {
        Show(msg, color, fontSize, MessageBoxType.PromptBox, null, null);
    }
    private static void Show(string msg, Color color, int fontSize, MessageBoxType messageBoxType, UnityAction yes, UnityAction no)
    {
        switch (messageBoxType)
        {
            case MessageBoxType.DialogBox_OK:
                MessageBox dialogBoxOK = UIManager.Instance.GetView<MessageBox>(Guid.NewGuid().ToString());
                dialogBoxOK.TextMsg.text = msg;
                dialogBoxOK.ButtonOK.SetActive(true);
                dialogBoxOK.ButtonYesNo.SetActive(false);
                dialogBoxOK.Background.SetActive(true);
                dialogBoxOK.Show();
                break;
            case MessageBoxType.DialogBox_YesNo:
                MessageBox dialogBoxYesNo = UIManager.Instance.GetView<MessageBox>(Guid.NewGuid().ToString());
                dialogBoxYesNo.TextMsg.text = msg;
                dialogBoxYesNo.ButtonOK.SetActive(false);
                dialogBoxYesNo.ButtonYesNo.SetActive(true);
                dialogBoxYesNo.Background.SetActive(true);
                if (yes != null)
                {
                    dialogBoxYesNo.BtnYes.OnClickAsObservable().Subscribe(a => { yes(); }).AddTo(dialogBoxYesNo.gameObject);
                }
                if (no != null)
                {
                    dialogBoxYesNo.BtnNo.OnClickAsObservable().Subscribe(a => { no(); }).AddTo(dialogBoxYesNo.gameObject);
                }
                dialogBoxYesNo.Show();
                break;
            case MessageBoxType.PromptBox:
                MessageBox promptBox = UIManager.Instance.GetView<MessageBox>(Guid.NewGuid().ToString());
                promptBox.ButtonOK.SetActive(false);
                promptBox.ButtonYesNo.SetActive(false);
                promptBox.Background.SetActive(false);
                promptBox.TextMsg.text = msg;
                promptBox.TextMsg.color = color;
                promptBox.TextMsg.fontSize = fontSize;
                promptBox.DialogBox.rectTransform.localScale = Vector3.zero;
                promptBox.DialogBox.rectTransform.SetPivot(PivotPresets.TopCenter);
                promptBox.DialogBox.rectTransform.SetAnchor(AnchorPresets.TopCenter);
                
                //promptBox.DialogBox.color = new Color(promptBox.DialogBox.color.r, promptBox.DialogBox.color.g, promptBox.DialogBox.color.b, 0f);
                PromptBoxSystem.Instance.Enqueue(promptBox);
                break;
        }

    }
}

public enum MessageBoxType
{
    DialogBox_OK,
    DialogBox_YesNo,
    PromptBox
}
