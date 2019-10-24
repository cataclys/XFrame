﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.EventSystems;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Object = UnityEngine.Object;
namespace XFrame.UI
{


    /// <summary>
    ///  UI管理
    ///  作者：徐振升
    ///  最后更新：2019-09-02
    ///  联系方式：QQ:359059686
    ///  遵循规则=>约定既是配置
    ///  约定对象： 需要UIManager管理的UIView预设
    ///  约定1：预设需要放在prefabRoot目录下
    ///  约定2：预设名字与UIView的名称子类相同
    ///  约定3：ShowView<T>显示的是名称为typeof(T).ToString()的T页面
    ///         ShowView<T>(name)显示的是名称为typeof(T).ToString()+name的T页面
    /// </summary>
    public class UIManager : Singleton<UIManager>
    {
        /// <summary>
        /// UIView预设根目录
        /// </summary>
        private const string uiViewPrefabsRoot = "Assets/Prefabs/";
        /// <summary>
        /// 已经创建的UIView对象池
        /// </summary>
        private Dictionary<string, UIView> UIViewsPool = new Dictionary<string, UIView>();

        /// <summary>
        /// 添加uiView到对象池
        /// </summary>
        /// <param name="uiView">UIView对象</param>
        public void Add(UIView uiView)
        {
            string keyName = FormattingViewName(uiView);
            if (UIViewsPool.ContainsKey(keyName))
            {
                MessageBox.Show($"[{keyName}]已存在，无法创建", "错误");
                uiView.ViewName = "null";
                Destroy(uiView.gameObject);
            }
            else
            {
                UIViewsPool.Add(keyName, uiView);
                uiView.gameObject.name = $"[{keyName}]";
                MessageBox.Show($"[{keyName}]", "新增");
            }
        }
        /// <summary>
        /// 从对象池中移除uiView
        /// </summary>
        /// <param name="uiView">UIView对象</param>
        public void Remove(UIView uiView)
        {
            string viewName = FormattingViewName(uiView);
            if (UIViewsPool.ContainsKey(viewName))
            {
                UIViewsPool.Remove(viewName);
            }
            else
            {
                if (!viewName.Contains("null"))
                {
                    MessageBox.Show($"[{viewName}]不存在，请确定已经创建该页面", "UIManager.Remove");
                }
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
            CreateUIRoot(UIViewType.Panel, gameObject.transform);
            CreateUIRoot(UIViewType.Popup, gameObject.transform);
            CreateEventSystem();
            return gameObject;
        }
        // 创建UI子目录
        private Transform CreateUIRoot(UIViewType uiViewType, Transform transform)
        {
            GameObject gameObject = new GameObject(uiViewType.ToString());
            gameObject.layer = LayerMask.NameToLayer("UI");
            RectTransform rect = gameObject.AddComponent<RectTransform>();
            gameObject.transform.SetParent(transform);
            rect.sizeDelta = Vector2.zero;
            rect.localScale = Vector3.one;
            rect.localPosition = Vector3.zero;
            rect.SetAnchor(AnchorPresets.StretchAll);
            return gameObject.transform;
        }
        // 获取UI子目录
        public Transform GetUIRoot(UIViewType uiViewType)
        {
            Transform canvas = GetCanvas().transform;
            Transform root = canvas.Find(uiViewType.ToString());
            if (root == null)
            {
                return CreateUIRoot(uiViewType, canvas);
            }
            return root;
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


        #region 加载
        /// <summary>
        /// 加载UIView预设
        /// 
        /// </summary>
        /// <param name="viewName"></param>
        /// <returns></returns>
        private async Task<UIView> LoadView<T>(string viewName)
            where T : UIView
        {
            // 格式化名称
            string key = FormattingViewName<T>(viewName);
            // 页面池中存在页面直接取出，否则加载
            if (UIViewsPool.ContainsKey(key))
            {
                return UIViewsPool[key];
            }
            else
            {
                // 获得脚本对应的约定路径
                string prefabPath = $"{uiViewPrefabsRoot}{typeof(T).ToString()}.prefab";
                // 获取预设，这个地方不能直接创建生成，需要把预设SetActive(false),否则名称无法指定，不能正常存入对象池
                AsyncOperationHandle<GameObject> prefabHandle = Addressables.LoadAssetAsync<GameObject>(prefabPath);
                await prefabHandle.Task;
                prefabHandle.Result.SetActive(false);
                UIView prefabView = prefabHandle.Result.GetComponent<UIView>();
                GameObject go = Instantiate(prefabHandle.Result, GetUIRoot(prefabView.UIViewType));
                UIView view = go.GetComponent<UIView>();
                view.ViewName = viewName;
                return view;
            }
        }
        #endregion
        /// <summary>
        /// 格式化UI名称
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="viewName"></param>
        /// <returns></returns>
        public string FormattingViewName<T>(string viewName)
        {
            return $"{typeof(T).ToString()}-{viewName}";
        }
        public string FormattingViewName(UIView uiView)
        {
            return $"{uiView.GetType().ToString()}-{uiView.ViewName}";
        }
        /// <summary>
        /// 获取页面
        /// </summary>
        /// <typeparam name="T">预设上脚本类型</typeparam>
        /// <param name="viewName">预设上脚本属性ViewName</param>
        /// <returns></returns>
        public T GetView<T>(string viewName)
            where T : UIView
        {
            viewName = FormattingViewName<T>(viewName);
            if (UIViewsPool.ContainsKey(viewName))
            {
                return UIViewsPool[viewName] as T;
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
        public void Show<T>()
            where T : UIView
        {
            Show<T>("Default", null);
        }
        public void Show<T>(string viewName)
            where T : UIView
        {
            Show<T>(viewName, null);
        }
        public void Show<T>(object data)
            where T : UIView
        {
            Show<T>("Default", data);
        }
        public async void Show<T>(string viewName, object data)
            where T : UIView
        {
            //viewName = FormattingViewName<T>(viewName);
            Task<UIView> task = LoadView<T>(viewName);
            await task;
            task.Result.Show(data);
        }
        ///// <summary>
        ///// 泛型数据类型设置并显示
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="viewName"></param>
        ///// <param name="data"></param>
        //public void Show<T, TReactive>(string viewName, T data)
        //    where TReactive : ISetData<T>, new()
        //{
        //    UIView<T, TReactive> view = LoadView(viewName) as UIView<T, TReactive>;
        //    view.Show(data);
        //}
        /// <summary>
        /// 隐藏页面
        /// </summary>
        /// <param name="viewName"></param>
        public void Hide<T>(string viewName)
            where T : UIView
        {
            viewName = FormattingViewName<T>(viewName);
            UIView view = GetView<T>(viewName);
            if (view != null)
            {
                view.Hide();
            }
        }
    }
}

