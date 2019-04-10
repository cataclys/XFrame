using System;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UI;
using UniRx;

/// <summary>
/// UI页面
/// </summary>
public class UIView : UIBehaviour
{
    //UIView名字
    public string ViewName;
    //类型
    public UIViewType ViewType;
    // 层次
    public int Z_Index { get { return transform.GetSiblingIndex(); } set { transform.SetSiblingIndex(value); } }
    public Action OnShow;
    public Action OnHide;
    // 显示
    public virtual void Show()
    {
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
            if (OnShow != null)
            {
                OnShow();
            }
            // 设置层级到最上层
            transform.SetAsLastSibling();
        }
    }
    // 隐藏
    public virtual void Hide()
    {
        //显示的情况下才隐藏
        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);
            if (OnHide != null)
            {
                OnHide();
            }
        }
    }
}

