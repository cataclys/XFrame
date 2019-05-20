using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
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
        canvasScaler.matchWidthOrHeight = 1f;
        gameObject.AddComponent<GraphicRaycaster>();
        // 创建UI类型根目录
        foreach (var itemType in Enum.GetValues(typeof(UIViewType)))
        {
            CreateUIRoot(itemType.ToString(), gameObject);
        }
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
    // 创建页面
    #endregion
}
