using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class ExpandViewItem : MonoBehaviour
{
    public ExpandView ParentView;
    public Text NameText;
    public GameObject ExpandContentRoot;
    public Button ExpandBtn;
    public Image ExpandBtnImage;
    public Button BtnAdd;
    public IExpandViewData Data;
    private bool IsExpand;
    public float NormalSize = 50f;

    public void Awake()
    {
        ExpandBtn.OnClickAsObservable().Subscribe(unit => { OnExpandBtnClicked(); }).AddTo(gameObject);
    }

    public void OnExpandChanged()
    {
        RectTransform rt = gameObject.GetComponent<RectTransform>();
        rt.parent.GetComponent<ContentSizeFitter>().enabled = false;
        if (IsExpand)
        {
            float height = ((RectTransform)ExpandContentRoot.transform).sizeDelta.y;
            rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, NormalSize + height);
            rt.parent.GetComponent<ContentSizeFitter>().enabled = true;
            ExpandContentRoot.transform.localScale = Vector3.one;
            ExpandBtnImage.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0, 0, 270);

        }
        else
        {
            rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, NormalSize);
            rt.parent.GetComponent<ContentSizeFitter>().enabled = true;
            ExpandContentRoot.transform.localScale = Vector3.zero;
            ExpandBtnImage.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0, 0, 0);
        }
    }

    public void SetExpand(bool b)
    {
        IsExpand = b;
        StartCoroutine(Refresh());

    }
    public IEnumerator Refresh()
    {
        yield return new WaitForEndOfFrame();
        OnExpandChanged();
    }
    private void OnExpandBtnClicked()
    {
        //ParentView?.ExpandOthers(this);
        IsExpand = !IsExpand;
        OnExpandChanged();
    }


    internal void SetItemData(IExpandViewData itemData)
    {
        Data = itemData;
        Data.Name.SubscribeToText(NameText);
        Data.Enabled.Subscribe(b =>
        {
            gameObject.SetActive(b);
        });
        // 刷新
        OnExpandChanged();
    }
}

