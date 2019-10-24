using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Object = UnityEngine.Object;

/// <summary>
///  UI管理
///  作者：徐振升
///  最后更新：2019-09-02
///  联系方式：QQ:359059686
/// </summary>
public class UIManager : Singleton<UIManager>
{
    #region 字段
    private Dictionary<UIViewName, UIView> UIViews = new Dictionary<UIViewName, UIView>();
    private Dictionary<UIViewName, string> UIViewPath = new Dictionary<UIViewName, string>
    {
        [UIViewName.消防控制室UI] = "UIPrefab/UIViewObjectInfo",
    };
    // 添加页面
    public void Add(UIViewName viewName, UIView uiView)
    {
        if (UIViews.ContainsKey(viewName))
        {
            MessageBox.Show($"页面已存在，无法添加", "错误");
        }
        else
        {
            UIViews.Add(viewName, uiView);
        }
    }
    // 移除页面
    public void Remove(UIViewName viewName)
    {
        if (UIViews.ContainsKey(viewName))
        {
            UIViews.Remove(viewName);
        }
        else
        {
            MessageBox.Show($"页面不存在，无法移除", "错误");
        }
    }
    #endregion

    #region 加载
    // 加载
    private UIView LoadView(UIViewName viewName)
    {
        if (UIViewPath.ContainsKey(viewName))
        {
            if (UIViews.ContainsKey(viewName))
            {
                return UIViews[viewName];
            }
            else
            {
                // 加载预设
                GameObject prefab = Resources.Load<GameObject>(UIViewPath[viewName]);

                if (prefab != null)
                {
                    Transform parent = GameObject.Find("MainCanvas").transform;
                    //实例化
                    GameObject gameObject = GameObject.Instantiate(prefab, parent);
                    gameObject.name = $"[UIView]{viewName}";
                    UIView uiView = gameObject.GetComponent<UIView>();
                    //UIViews.Add(viewName, uiView);
                    return uiView;
                }
                else
                {
                    MessageBox.Show($"加载{viewName}预设失败", "错误");
                    return null;
                }
            }
        }
        else
        {
            MessageBox.Show($"{viewName}预设路径不存在", "错误");
            return null;
        }
    }
    #endregion

    /// <summary>
    /// 如果该页面已经创建则返回该页面，否则返回null
    /// </summary>
    public UIView GetView(UIViewName viewName)
    {
        if (UIViews.ContainsKey(viewName))
        {
            return UIViews[viewName];
        }
        else
        {
            return null;
        }
    }
    /// <summary>
    /// 如果该页面已经创建则显示该页面，否则创建该页面并显示
    /// </summary>
    /// <param name="viewName"></param>
    public void Show(UIViewName viewName)
    {
        UIView view = LoadView(viewName);
        view.Show();
    }
    /// <summary>
    /// 泛型数据类型设置并显示
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="viewName"></param>
    /// <param name="data"></param>
    public void Show<T>(UIViewName viewName, T data)
        where T : new()
    {
        UIView<T> view = LoadView(viewName) as UIView<T>;
        view.Show(data);
    }
    /// <summary>
    /// 隐藏页面
    /// </summary>
    /// <param name="viewName"></param>
    public void Hide(UIViewName viewName)
    {
        UIView view = LoadView(viewName);
        view.Hide();
    }
}
public enum UIViewName
{
    消防控制室UI,
}



