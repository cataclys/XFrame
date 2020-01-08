using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class ExpandViewOneOrTwoItem : MonoBehaviour
{
    public Text NameText;
    public RectTransform ExpandContentRoot;
    public Button ExpandBtn;
    public Image ExpanBtnImage;
    public IExpandViewData Data;
    public ExpandViewOneOrTwoItem Other;
    public bool IsExpand;

    public void Awake()
    {
        ExpandBtn.onClick.AddListener(OnExpandBtnClicked);
        //Data.Name.SubscribeToText(NameText);
        //Data.Enabled.Subscribe(b =>
        //{
        //    gameObject.SetActive(b);
        //});
        OnExpandChanged();
    }
    public void SetName(string name)
    {
        NameText.text = name;
    }
    public void OnExpandChanged()
    {
        //RectTransform rt = gameObject.GetComponent<RectTransform>();
        if (IsExpand)
        {
            ExpanBtnImage.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0, 0, 270);
            if (Other.IsExpand)
            {
                // other 展开 自己展一半 other 缩小一半
                //ExpandContentRoot.sizeDelta = new Vector2(330f, 462.5f);
                //rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 512.5f);
                ChangeSize(442.5f);
                Other.ChangeSize(442.5f);

            }
            else
            {
                // other 折叠状态
                //ExpandContentRoot.sizeDelta = new Vector2(330f, 925f);
                //rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 975f);    
                ChangeSize(885f);
            }

            ExpandContentRoot.localScale = Vector3.one;
            //ExpanBtnImage.color = Color.white;
        }
        else
        {
            ExpanBtnImage.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0, 0, 0);
            if (Other.IsExpand)
            {
                Other.ChangeSize(885f);
            }
            else
            {

            }
            // 分半展开
            //rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 50f);
            //rt.parent.GetComponent<ContentSizeFitter>().enabled = true;
            ExpandContentRoot.localScale = Vector3.zero;
            ChangeSize(0f);
            //ExpanBtnImage.color = Color.black;
            //}
        }
        // 通知控制器自己当前的状态

    }
    public void ChangeSize(float height)
    {
        RectTransform rt = gameObject.GetComponent<RectTransform>();
        rt.parent.GetComponent<ContentSizeFitter>().enabled = false;
        if (height != 0)
        {
            ExpandContentRoot.sizeDelta = new Vector2(282f, height);
        }
        rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height + 50f);
        rt.parent.GetComponent<ContentSizeFitter>().enabled = true;
    }
    void OnExpandBtnClicked()
    {

        IsExpand = !IsExpand;
        OnExpandChanged();
    }


    internal void SetItemData(IExpandViewData itemData)
    {
        Data = itemData;
        // 刷新
        OnExpandChanged();
    }
}

