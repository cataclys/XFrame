using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using XLua;
using Object = UnityEngine.Object;

/// <summary>
///  UI管理
///  作者：徐振升
///  最后更新：2019-04-01 15：30
///  联系方式：QQ:359059686
/// </summary>
public class UIManager : Singleton<UIManager>
{
    #region 字段
    // UI画布
    public static Canvas UICanvas;

    private Dictionary<Type, Dictionary<string, UIView>> Views = new Dictionary<Type, Dictionary<string, UIView>>();
    private Dictionary<Type, string> UIPath = new Dictionary<Type, string>();
    // UI视图 栈
    private Stack<UIView> viewStack = new Stack<UIView>();
    #endregion

    private void Awake()
    {
        SceneManager.sceneLoaded += ClearViewOnSceneLoaded;
    }

    private void ClearViewOnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        ClearView();
    }
    #region 根目录
    // 获取对象目录
    public Transform GetUIElementRoot(string rootName)
    {
        GameObject gameObject = GameObject.Find("UICanvas");
        if (gameObject == null || gameObject.GetComponent<Canvas>() == null)
        {
            gameObject = CreateUICanvas("UICanvas");
        }
        Transform transform = gameObject.transform.Find(rootName);
        if (transform == null)
        {
            CreateUIElementRoot(rootName, gameObject);
            transform = gameObject.transform.Find(rootName);
        }
        return transform;
    }
    // 创建新的UI目录
    public static GameObject CreateUICanvas(string name)
    {
        GameObject gameObject = new GameObject(name);
        gameObject.layer = LayerMask.NameToLayer("UI");
        Canvas canvas = gameObject.AddComponent<Canvas>();
        UICanvas = canvas;
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
    private static void CreateUIElementRoot(string name, GameObject obj)
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
    private static void CreateEventSystem()
    {
        EventSystem eventSystem = Object.FindObjectOfType<EventSystem>();
        if (eventSystem == null)
        {
            GameObject gameObject = new GameObject("EventSystem");
            eventSystem = gameObject.AddComponent<EventSystem>();
            gameObject.AddComponent<StandaloneInputModule>();
        }
    }
    // 注册UI预设路径
    private void Register<T>(string prefabPath)
        where T : UIView
    {
        UIPath.Add(typeof(T), prefabPath);
    }
    #endregion

    #region 加载
    // 加载
    private T LoadView<T>(string viewName)
        where T : UIView
    {
        Type tempKey = typeof(T);
        if (!UIPath.ContainsKey(tempKey))
        {
            ////属性标签只能使用常量，暂时放弃
            //var info = typeof(T);
            //var classAttribute = (ConfigPath)Attribute.GetCustomAttribute(info, typeof(ConfigPath));
            //if (classAttribute != null)
            //{
            //    //todo 注册UI
            //    Register<T>(classAttribute.Path);
            //}
            //else
            //{
            //    // 没有注册先注册
            //    Register<T>($"UIView/{typeof(T).ToString()}");
            //    Debug.LogError($"{typeof(T).ToString()}未添加ConfigPath标签，无法通过配置注册,使用UIView/{typeof(T).ToString()}注册");
            //    //return;
            //}
            Register<T>($"UIView/{typeof(T).ToString()}");
        }
        string tempPath = UIPath[tempKey];
        GameObject gameObject = Resources.Load<GameObject>(tempPath);



        gameObject.SetActive(false);
        if (gameObject != null && gameObject.GetComponent<T>() != null)
        {
            //实例化
            GameObject ui = GameObject.Instantiate(gameObject);
            ui.name = gameObject.name;
            //记录初始大小
            Vector3 anchorPos3D = ui.GetComponent<RectTransform>().anchoredPosition3D;
            Vector3 anchorPos = ui.GetComponent<RectTransform>().anchoredPosition;
            Vector3 sizeDel = ui.GetComponent<RectTransform>().sizeDelta;
            Vector3 scale = ui.GetComponent<RectTransform>().localScale;
            //设置父对象
            T view = ui.GetComponent<T>();
            Transform parent = GetUIElementRoot(view.ViewType.ToString());
            ui.transform.SetParent(parent);
            //还原初始大小
            ui.GetComponent<RectTransform>().anchoredPosition3D = anchorPos3D;
            ui.GetComponent<RectTransform>().anchoredPosition = anchorPos;
            ui.GetComponent<RectTransform>().sizeDelta = sizeDel;
            ui.GetComponent<RectTransform>().localScale = scale;

            view.ViewName = viewName;
            view.gameObject.name = view.ViewName;

            Views[typeof(T)].Add(viewName, view);


            return view;
        }
        else
        {
            return null;
            // Resource根目录没有发现UIView的Prefab,请添加。
            throw new Exception($"没有发现{tempPath}");
        }
    }
    #endregion

    #region 销毁
    // 销毁
    public void DestroyView<T>(string viewName)
    {
        if (Views.ContainsKey(typeof(T)) && Views[typeof(T)].ContainsKey(viewName))
        {
            GameObject temp = Views[typeof(T)][viewName].gameObject;
            Views[typeof(T)].Remove(viewName);
            GameObject.Destroy(temp);
        }
    }
    //清空页面
    public void ClearView()
    {
        //清空引用
        Views.Clear();
        viewStack.Clear();
    }
    #endregion

    #region 获取
    /// <summary>
    /// 获取一个UIView页面
    /// </summary>
    /// <typeparam name="T">页面名称</typeparam>
    /// <returns>页面脚本对象</returns>
    public T GetView<T>()
        where T : UIView
    {
        string viewName = typeof(T).ToString();
        return GetView<T>(viewName);
    }
    public T GetView<T>(string viewName)
        where T : UIView
    {
        if (!Views.ContainsKey(typeof(T)))
        {
            Views.Add(typeof(T), new Dictionary<string, UIView>());
        }
        T view = null;
        //没有页面先加载
        if (!Views[typeof(T)].ContainsKey(viewName))
        {
            view = LoadView<T>(viewName);
        }
        else
        {
            view = Views[typeof(T)][viewName] as T;
        }
        return view;
    }
    /// <summary>
    /// 获取所有指定类型页面
    /// </summary>
    public Dictionary<string, UIView> GetViews<T>()
        where T : UIView
    {
        if (!Views.ContainsKey(typeof(T)))
        {
            MessageBox.Show($"没有{typeof(T)}类型的界面");
            return null;
        }
        else
        {
            return Views[typeof(T)];
        }
    }
    #endregion

    #region 显示
    /// <summary>
    /// 显示界面
    /// </summary>
    /// <typeparam name="T">界面类型</typeparam>
    public void ShowView<T>()
        where T : UIView
    {
        string viewName = typeof(T).ToString();
        ShowView<T>(viewName);
    }
    // 显示指定名称页面
    public void ShowView<T>(string viewName)
         where T : UIView
    {
        T view = GetView<T>(viewName);
        if (view != null)
        {
            //如果是面板
            if (view.ViewType == UIViewType.Panel)
            {
                if (viewStack.Count > 0)  // 当前栈顶的面板隐藏
                {
                    viewStack.Peek().Hide();
                }
                viewStack.Push(view);  // 新面板入栈
            }
            view.Show();
        }
    }
    // 显示前一个页面
    public void ShowBackView()
    {
        UIView topPanel = viewStack.Pop();
        topPanel.Hide();  // 当前栈顶的面板出栈

        if (viewStack.Count > 0)
        {
            viewStack.Peek().Show();  //旧面板恢复
        }
    }
    #endregion

    #region 隐藏
    public void HideView<T>()
    where T : UIView
    {
        string viewName = typeof(T).ToString();
        HideView<T>(viewName);
    }
    public void HideView<T>(string viewName)
         where T : UIView
    {
        T view = GetView<T>(viewName);
        if (view != null)
        {
            view.Hide();
        }
    }
    #endregion
}


