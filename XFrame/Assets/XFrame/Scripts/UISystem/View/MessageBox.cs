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

    public static List<MessageBox> messageBoxes = new List<MessageBox>();

    public static int count = 3;
    public int height = 52;
    //public override void Awake()
    //{
    //    rectTransform = GetComponent<RectTransform>();
    //}

    //public override void OnDestroy()
    //{

    //}

    public static void Show(string message)
    {
        UIManager.Show<MessageBox>(Guid.NewGuid().ToString(), view =>
        {
            view.SetContentNone(message);
            //GameManager.Instance.Messages.Enqueue(view);
        });
    }


    public void DoAnimation()
    {
        rectTransform.SetAnchor(AnchorPresets.TopCenter);
        rectTransform.DOScale(1, 0.2f);
        foreach (var item in messageBoxes)
        {
            RectTransform itemRect = item.transform as RectTransform;
            float offset = itemRect.anchoredPosition.y - height;
            float maxOffset = -height * (count);
            if (offset > maxOffset)
            {
                itemRect.DOAnchorPosY(offset, 0.2f);
            }
        }
        messageBoxes.Add(this);
        StartCoroutine(DestroyGameObject());
    }

    public IEnumerator DestroyGameObject()
    {
        yield return new WaitForSeconds(5f);
        messageBoxes.Remove(this);
        Destroy(gameObject);
    }
    public void SetContentNone(string message)
    {
        //Title.SetActive(false);
        ContentNone.SetActive(true);
        ContentYesNo.SetActive(false);
        TextContentNone.text = message;
    }
}
