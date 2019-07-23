using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEditor;
using Object = UnityEngine.Object;

public class UISystem
{
    #region UISystem
    // 创建新的UI目录
    public static GameObject CreateUICanvas(string name)
    {
        GameObject gameObject = new GameObject(name);
        gameObject.layer = LayerMask.NameToLayer("UI");
        Canvas canvas = gameObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        CanvasScaler canvasScaler = gameObject.AddComponent<CanvasScaler>();
        canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasScaler.referenceResolution = new Vector2(1920f, 1080f);
        canvasScaler.matchWidthOrHeight = 0.5f;
        gameObject.AddComponent<GraphicRaycaster>();
        //// 创建UI类型根目录
        //foreach (var itemType in Enum.GetValues(typeof(UIViewType)))
        //{
        //    CreateUIRoot(itemType.ToString(), gameObject);
        //}
        // 创建事件系统
        CreateEventSystem();
        return gameObject;
    }
    // 创建UI子目录
    private static void CreateUIRoot(string name, GameObject obj)
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
    // 加载UI预设
    private static T LoadView<T>(string viewName,string path)
        where T : UIView
    {
        // 加载预设
        GameObject gameObject = Resources.Load<GameObject>(path);
        // 预设不为空
        if (gameObject != null && gameObject.GetComponent<T>() != null)
        {
            //实例化
            GameObject ui = GameObject.Instantiate(gameObject);
            ui.name = gameObject.name;
            //记录初始大小
            Vector3 anchorPos = ui.GetComponent<RectTransform>().anchoredPosition;
            Vector3 sizeDel = ui.GetComponent<RectTransform>().sizeDelta;
            Vector3 scale = ui.GetComponent<RectTransform>().localScale;
            //设置父对象
            T view = ui.GetComponent<T>();
            Transform parent = null;// GetUIElementRoot(view.ViewType.ToString());
            ui.transform.SetParent(parent);
            //还原初始大小
            ui.GetComponent<RectTransform>().anchoredPosition = anchorPos;
            ui.GetComponent<RectTransform>().sizeDelta = sizeDel;
            ui.GetComponent<RectTransform>().localScale = scale;
  
        }
        return null;
    }
    #endregion
}
