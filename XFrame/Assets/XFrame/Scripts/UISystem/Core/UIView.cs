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
    public class UIView : MonoBehaviour
    {
        // 页面名称
        public string ViewName = "Default";
        public UIViewType UIViewType;
        public Action OnShow;
        public Action OnHide;
        [HideInInspector]
        public RectTransform rectTransform;
        public virtual void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            UIManager.Add(this);
        }
        public virtual void OnDestroy()
        {
            UIManager.Remove(this);
        }
        /// <summary>
        /// 显示
        /// </summary>
        public virtual void Show()
        {
            Refresh();
            if (gameObject.activeSelf) return;
            gameObject.SetActive(true);
            OnShow?.Invoke();
            // 设置层级到最上层
            transform.SetAsLastSibling();
        }
        /// <summary>
        /// 隐藏
        /// </summary>
        public virtual void Hide()
        {
            //显示的情况下才隐藏
            if (!gameObject.activeSelf) return;
            gameObject.SetActive(false);
            OnHide?.Invoke();
        }
        public void ShowHide()
        {
            gameObject.SetActive(!gameObject.activeSelf);
            // 设置层级到最上层
            transform.SetAsLastSibling();
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

    //TODO  修改名字为IData 新增GetData方法
    public interface ISetData<T>
    {
        //新增属性GetData  SetData 时修改此属性的值
        //T Data { get; set; }
        void SetData(T t);
    }
}