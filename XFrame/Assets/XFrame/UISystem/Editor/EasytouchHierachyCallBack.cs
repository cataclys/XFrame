using System;
using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public class XFrameHierachyCallBack
{
    // 层级窗口项回调
    private static readonly EditorApplication.HierarchyWindowItemCallback hiearchyItemCallback;

    private static Texture2D uiViewIcon;
    private static Texture2D UIViewIcon
    {
        get
        {
            if (XFrameHierachyCallBack.uiViewIcon == null)
            {
                XFrameHierachyCallBack.uiViewIcon = (Texture2D)Resources.Load("Icons/iconUIView");
            }
            return XFrameHierachyCallBack.uiViewIcon;
        }
    }

    /// <summary>
    /// 静态构造
    /// </summary>
    static XFrameHierachyCallBack()
    {
        XFrameHierachyCallBack.hiearchyItemCallback = new EditorApplication.HierarchyWindowItemCallback(XFrameHierachyCallBack.DrawHierarchyIcon);
        EditorApplication.hierarchyWindowItemOnGUI = (EditorApplication.HierarchyWindowItemCallback)Delegate.Combine(
            EditorApplication.hierarchyWindowItemOnGUI,
            XFrameHierachyCallBack.hiearchyItemCallback);

    }

    // 绘制icon方法
    private static void DrawHierarchyIcon(int instanceID, Rect selectionRect)
    {
        GameObject gameObject = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
        if (gameObject && gameObject.GetComponent<UIView>())
        {
            // 设置icon的位置与尺寸（Hierarchy窗口的左上角是起点）
            Rect rect = new Rect(selectionRect.x + selectionRect.width - 16f, selectionRect.y, 16f, 16f);
            // 画icon
            GUI.DrawTexture(rect, XFrameHierachyCallBack.UIViewIcon);
        }
    }
}