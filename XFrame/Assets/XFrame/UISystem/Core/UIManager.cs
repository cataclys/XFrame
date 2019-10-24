using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.EventSystems;
using UnityEngine.ResourceManagement.AsyncOperations;
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
       [UIViewName.Login]  = "LoginView",
    };

    // 添加页面
    public void Add(UIViewName viewName, UIView uiView)
    {
        if (UIViews.ContainsKey(viewName))
        {
            MessageBox.Show($"[{viewName}]已存在", "UIManager.Add");
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
            MessageBox.Show($"[{viewName}]不存在", "UIManager.Remove");
        }
    }
    /// <summary>
    /// 获取UI画布
    /// </summary>
    public GameObject GetCanvas()
    {
        GameObject gameObject = GameObject.Find("Canvas");
        if (gameObject == null || gameObject.GetComponent<Canvas>() == null)
        {
            gameObject = CreateCanvas("Canvas");
        }
        return gameObject;
    }
    // 创建新的UI目录
    private GameObject CreateCanvas(string name)
    {
        GameObject gameObject = new GameObject(name);
        gameObject.layer = LayerMask.NameToLayer("UI");
        Canvas canvas = gameObject.AddComponent<Canvas>();
        //UICanvas = canvas;
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        CanvasScaler canvasScaler = gameObject.AddComponent<CanvasScaler>();
        canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasScaler.referenceResolution = new Vector2(1920f, 1080f);
        canvasScaler.matchWidthOrHeight = 1f;
        gameObject.AddComponent<GraphicRaycaster>();
        CreateEventSystem();
        return gameObject;
    }
    // 创建UI子目录
    private void CreateUIRoot(string name, GameObject obj)
    {
        GameObject gameObject = new GameObject(name);
        gameObject.layer = LayerMask.NameToLayer("UI");
        RectTransform rect = gameObject.AddComponent<RectTransform>();
        gameObject.transform.SetParent(obj.transform);
        rect.sizeDelta = Vector2.zero;
        rect.localScale = Vector3.one;
        rect.localPosition = Vector3.zero;
        rect.SetAnchor(AnchorPresets.StretchAll);
    }
    // 创建UIEvent
    private void CreateEventSystem()
    {
        EventSystem eventSystem = Object.FindObjectOfType<EventSystem>();
        if (eventSystem == null)
        {
            GameObject gameObject = new GameObject("EventSystem");
            eventSystem = gameObject.AddComponent<EventSystem>();
            gameObject.AddComponent<StandaloneInputModule>();
        }
    }
    #endregion

    #region 加载
    // 加载
    private async Task<UIView> LoadView(UIViewName viewName)
    {
        if (UIViewPath.ContainsKey(viewName))
        {
            if (UIViews.ContainsKey(viewName))
            {
                return UIViews[viewName];
            }
            else
            {
                //// 加载预设
                //GameObject prefab = Resources.Load<GameObject>(UIViewPath[viewName]);

                //if (prefab != null)
                //{
                //    Transform parent = GameObject.Find("MainCanvas").transform;
                //    //实例化
                //    GameObject gameObject = GameObject.Instantiate(prefab, parent);
                //    gameObject.name = $"[UIView]{viewName}";
                //    UIView uiView = gameObject.GetComponent<UIView>();
                //    //UIViews.Add(viewName, uiView);
                //    return uiView;
                //}
                //else
                //{
                //    MessageBox.Show($"加载{viewName}预设失败", "错误");
                //    return null;
                //}
                AsyncOperationHandle<GameObject> handle = Addressables.InstantiateAsync(UIViewPath[viewName],GetCanvas().transform);
                await handle.Task;
                handle.Result.name = $"[UIView]{viewName}";
                UIView view = handle.Result.GetComponent<UIView>();
                return view;
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
    public T GetView<T>(UIViewName viewName)
        where T : UIView
    {
        if (UIViews.ContainsKey(viewName))
        {
            return UIViews[viewName] as T;
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
    public async void Show(UIViewName viewName)
    {
        Task<UIView> task = LoadView(viewName);
        //UIView view = await LoadView(viewName).Result;
        await task;
        task.Result.Show();
    }
    /// <summary>
    /// 泛型数据类型设置并显示
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="viewName"></param>
    /// <param name="data"></param>
    public void Show<T, TReactive>(UIViewName viewName, T data)
        where TReactive : ISetData<T>, new()
    {
        UIView<T, TReactive> view = LoadView(viewName) as UIView<T, TReactive>;
        view.Show(data);
    }
    /// <summary>
    /// 隐藏页面
    /// </summary>
    /// <param name="viewName"></param>
    public void Hide(UIViewName viewName)
    {
        UIView view = GetView(viewName);
        if (view!=null)
        {
            view.Hide();
        }
    }

    public override void Initialize()
    {
        throw new NotImplementedException();
    }
}
public enum UIViewName
{
    Login
}



