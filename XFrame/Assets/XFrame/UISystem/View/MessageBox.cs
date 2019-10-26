using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using DG.Tweening;
using UniRx;
using XFrame.UI;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

public class MessageBox : UIView
{
    //标题
    public GameObject Title;
    public Text TextTitle;
    //内容
    public GameObject ContentNone;
    public Text TextContentNone;
    public GameObject ContentYesNo;
    public Text TextContentYesNo;
    //OK
    public Button ButtonOK;
    //YesNo
    public Button ButtonYes;
    public Button ButtonNo;

    public static int MaxCount = 3;
    //MessageBox 队列
    public static Queue<MessageBox> MessageBoxQueue = new Queue<MessageBox>();

    public void Start()
    {
        StartCoroutine(Hiding());
    }

    private IEnumerator Hiding()
    {
        yield return new WaitForSeconds(5f);
        gameObject.SetActive(false);
    }

    public override void Awake()
    {
        SceneManager.sceneLoaded += (a, b) =>
        {
            MessageBoxQueue.Clear();
        };
    }
    public override void OnDestroy()
    {

    }
    public override void Show(object data = null)
    {
        gameObject.SetActive(true);
        // 动画
        RectTransform rect = transform as RectTransform;
        rect.DOScale(1, 0.2f);
        foreach (var item in MessageBoxQueue)
        {
            if (item != this)
            {
                RectTransform itemRect = item.transform as RectTransform;
                float offset = itemRect.anchoredPosition.y - 110;
                float distance = rect.anchoredPosition.y + offset;
                itemRect.DOAnchorPosY(distance, 0.2f);
            }
        }
    }
    public override void Hide()
    {

        // 动画
        Destroy(gameObject);
    }
    public static async void Show(string message)
    {
        // 如果队列数量大于最大数量等于
        if (MessageBoxQueue.Count >= MaxCount)
        {
            //出站
            MessageBox dBox = MessageBoxQueue.Dequeue();
            dBox.Hide();
        }
        Task<MessageBox> task = UIManager.Instance.LoadView<MessageBox>("MessageBox");
        await task;
        MessageBox box = task.Result;
        SetDefault(box);
        box.ContentNone.SetActive(true);
        box.TextContentNone.text = message;
        (box.transform as RectTransform).SetAnchor(AnchorPresets.TopCenter);
        box.Show();
        MessageBoxQueue.Enqueue(box);
    }
    public static async void ShowOk(string message, string caption)
    {
        Task<MessageBox> task = UIManager.Instance.LoadView<MessageBox>("Default");
        await task;
        MessageBox box = task.Result;
        SetDefault(box);
        box.Title.SetActive(true);
        box.TextTitle.text = caption;
        box.ContentYesNo.SetActive(true);
        box.TextContentYesNo.text = message;
        box.Show();
    }
    public static async void ShowYesNo(string message, string caption)
    {
        Task<MessageBox> task = UIManager.Instance.LoadView<MessageBox>("Default");
        await task;
        MessageBox box = task.Result;
        box.TextTitle.text = caption;
        box.TextContentYesNo.text = message;

        box.Show();
    }

    public static void SetDefault(MessageBox box)
    {
        box.Title.SetActive(false);
        box.ContentNone.SetActive(false);
        box.ContentYesNo.SetActive(false);
    }
}
