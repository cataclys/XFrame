using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PromptBoxSystem : Singleton<PromptBoxSystem>
{
    //MessageBox 队列
    Queue<MessageBox> MessageBoxQueue = new Queue<MessageBox>();

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("ShowMsg", 0f, 0.5f);
    }
    public void Enqueue(MessageBox box)
    {
        MessageBoxQueue.Enqueue(box);
    }

    private IEnumerator DoDestroy(MessageBox box)
    {
        yield return new WaitForSeconds(5f);
        UIManager.Instance.DestroyView<MessageBox>(box.ViewName);
    }
    private IEnumerator DoAnimation(MessageBox box)
    {
        box.Show();
        yield return new WaitForEndOfFrame();
        box.DialogBox.rectTransform.DOScale(1f, 0.8f);
        //box.DialogBox.DOFade(1f, 0.5f);
        foreach (var item in UIManager.Instance.GetViews<MessageBox>().Values)
        {
            MessageBox temp = item as MessageBox;
            if (box != temp && temp.IsActive() && temp.Background.activeSelf == false)
            {
                RectTransform rect = temp.DialogBox.rectTransform;
                float offset = box.DialogBox.rectTransform.sizeDelta.y + 2;
                float distance = rect.anchoredPosition.y - offset;
                //Debug.Log(offset);
                temp.DialogBox.rectTransform.DOAnchorPosY(distance, 0.5f);
            }
        }
    }

    // Update is called once per frame
    void ShowMsg()
    {
        if (MessageBoxQueue.Count > 0)
        {
            MessageBox box = MessageBoxQueue.Dequeue();
            StartCoroutine(DoAnimation(box));
            StartCoroutine(DoDestroy(box));
        }
    }
}
