using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using DG.Tweening;
using UniRx;
public class MessageBox : MonoBehaviour
{
    //标题
    public Text Title;
    //内容
    public Text Content;
    //OK
    public GameObject ButtonOK;
    public Button BtnOK;
    //YesNo
    public GameObject ButtonYesNo;
    public Button BtnYes;
    public Button BtnNo;
    //Cancel
    public GameObject ButtonCancel;
    public Button BtnCancel;
    // 预设
    public static GameObject Current;

    public void Awake()
    {
        BtnOK.OnClickAsObservable().Subscribe(click =>
        {
            Destroy(gameObject);
        }).AddTo(gameObject);
        BtnYes.OnClickAsObservable().Subscribe(click =>
        {
            Destroy(gameObject);
        }).AddTo(gameObject);
        BtnNo.OnClickAsObservable().Subscribe(click =>
        {
            Destroy(gameObject);
        }).AddTo(gameObject);
        BtnCancel.OnClickAsObservable().Subscribe(click =>
        {
            Destroy(gameObject);
        }
        ).AddTo(gameObject);
    }
    public static void Show(string message, string caption, MessageBoxButtons buttons = MessageBoxButtons.OK)
    {
        // 加载预设
        GameObject prefab = Resources.Load<GameObject>("MessageBox");
        // 实例化对象
        Transform parent = UIManager.Instance.GetCanvas().GetComponent<Transform>();
        // 设置Box属性
        if (Current==null)
        {
            Current = Instantiate(prefab, parent);
        }
        MessageBox box = Current.GetComponent<MessageBox>();
        box.Title.text = caption;
        box.Content.text = message;
        switch (buttons)
        {
            case MessageBoxButtons.None:
                box.ButtonOK.SetActive(false);
                box.ButtonYesNo.SetActive(false);
                box.ButtonCancel.SetActive(false);
                break;
            case MessageBoxButtons.OK:
                box.ButtonOK.SetActive(true);
                box.ButtonYesNo.SetActive(false);
                box.ButtonCancel.SetActive(false);
                break;
            case MessageBoxButtons.YesNo:
                box.ButtonOK.SetActive(false);
                box.ButtonYesNo.SetActive(true);
                box.ButtonCancel.SetActive(false);
                break;
            case MessageBoxButtons.Cancel:
                box.ButtonOK.SetActive(false);
                box.ButtonYesNo.SetActive(false);
                box.ButtonCancel.SetActive(true);
                break;
        }
    }

    public static void Close()
    {
        if (Current!=null)
        {
            Destroy(Current);
        }
    }
}

public enum MessageBoxButtons
{
    None,
    OK,
    YesNo,
    Cancel
}
