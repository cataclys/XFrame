using System;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UI;
using UniRx;
namespace XFrame.UI
{
    /// <summary>
    /// UI页面
    /// </summary>
    public class UIView : BaseBehaviour
    {
        // 页面名称
        public string ViewName = "Default";
        public UIViewType UIViewType;
        public Action OnShow;
        public Action OnHide;
        public object DataSource;
        public virtual void Awake()
        {
            UIManager.Instance.Add(this);
        }
        public virtual void OnDestroy()
        {
            UIManager.Instance?.Remove(this);
        }
        /// <summary>
        /// 显示
        /// </summary>
        public virtual void Show(object data = null)
        {
            DataSource = data;
            Refresh();
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
        /// <summary>
        /// 隐藏
        /// </summary>
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
        /// <summary>
        /// 刷新
        /// </summary>
        public virtual void Refresh()
        {

        }

    }
    public enum UIViewType
    {
        Panel,
        Popup,
    }
    //public class UIViewData
    //{

    //}
    //public class UIView<T, TReactive> : UIView
    //    where TReactive : ISetData<T>, new()
    //{
    //    /// <summary>
    //    /// 数据
    //    /// </summary>
    //    protected TReactive DataSource { get; private set; } = new TReactive();
    //    /// <summary>
    //    /// 显示
    //    /// </summary>
    //    /// <param name="data"></param>
    //    public virtual void Show(T data)
    //    {
    //        DataSource.SetData(data);
    //        base.Show();
    //    }
    //}


    ////TODO  修改名字为IData 新增GetData方法
    //public interface ISetData<T>
    //{
    //    //新增属性GetData  SetData 时修改此属性的值
    //    //T Data { get; set; }
    //    void SetData(T t);
    //}
}