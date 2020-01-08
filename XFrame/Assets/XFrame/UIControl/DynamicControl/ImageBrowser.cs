//using DG.Tweening;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//using UniRx;
//using System;
//using System.IO;
//using System.Runtime.InteropServices;
//using UnityEngine.Networking;
//using System.Text.RegularExpressions;
//using SFB;

//public class ImageBrowser : DynamicControl
//{
//    /// <summary>
//    /// 图片预设
//    /// </summary>
//    public GameObject ItemPrefab;
//    /// <summary>
//    /// 
//    /// </summary>
//    public ScrollRect scrollRect;
//    public Button BtnLeft;
//    public Button BtnRight;
//    /// <summary>
//    /// 图片选择按钮
//    /// </summary>
//    public Button BtnImageSelect;
//    /// <summary>
//    /// 选择的图片
//    /// </summary>
//    public Texture2D SelectedImageTexture;
//    /// <summary>
//    /// 图片容器
//    /// </summary>
//    public Transform ContentImage;
//    /// <summary>
//    /// 图片对象集合
//    /// </summary>
//    public List<RectTransform> Items;
//    public int Index = 0;
//    //图片数量
//    public int ImageCount;

//    public List<PropertyInfo> Data;
//    //图片数量文本
//    public Text LblImageNumric;
//    //上传进度条
//    public Slider SliderUploadImage;
//    //遮罩
//    public GameObject ImageMask;
//    // 按钮 删除图片
//    public Button BtnDelete;

//    RectTransform viewPointTransform;
//    RectTransform contentTransform;
//    private void Start()
//    {
//        viewPointTransform = scrollRect.viewport;
//        contentTransform = scrollRect.content;
//        BtnLeft.onClick.AddListener(() =>
//        {
//            Index--;
//            Index = Mathf.Clamp(Index, 0, Items.Count - 1);
//            CenterOnItem(Items[Index]);
//            RefreshImageButton();

//        });
//        BtnRight.onClick.AddListener(() =>
//        {
//            Index++;
//            Index = Mathf.Clamp(Index, 0, Items.Count - 1);
//            CenterOnItem(Items[Index]);
//            RefreshImageButton();
//        });
//        BtnDelete.OnClickAsObservable().Subscribe(u =>
//        {
//            if (Items.Count > 0)
//            {
//                string imageName = Items[Index].gameObject.name;
//                // 根据id删除对应的数据
//                PropertyInfo temp = Data.Find(item => item.PropertyName == imageName);
//                Data.Remove(temp);
//                //Debug.Log(ParentView.Data.Count);
//                // 移除gameObject,刷新界面
//                Destroy(Items[Index].gameObject);
//                Items.Remove(Items[Index]);
//                bool b = true;
//                if (Index == 0)
//                {
//                    b = false;
//                }
//                Index--;
//                Index = Mathf.Clamp(Index, 0, Items.Count - 1);
//                if (Items.Count > 0 && b)
//                {
//                    CenterOnItem(Items[Index]);
//                }
//                RefreshImageButton();
//                //
//                //WorkingArea.IsDataChanged = true;
//                LblImageNumric.text = $"{Items.Count}/{ImageCount}";
//                //// 保存编辑
//                //Messenger.Invoke(MessengerType.Save);
//            }
//        });
//        BtnImageSelect.OnClickAsObservable().Subscribe(u =>
//        {
//            //判断是否到达上传图片上限
//            if (Items.Count >= ImageCount)
//            {
//                MessageBox.Show($"图片数量已经到达上限");
//                return;
//            }
//            ImageSelect();
//            //WorkingArea.IsDataChanged = true;
//        });
//        // 设置图片最大数量
//        LblImageNumric.text = $"{Items.Count}/{ImageCount}";
//    }
//#if UNITY_WEBGL && !UNITY_EDITOR
//    //
//    // WebGL
//    //
//    [DllImport("__Internal")]
//    private static extern void UploadFile(string gameObjectName, string methodName, string filter, bool multiple);

//    // Called from browser
//    public void OnFileUpload(string url) {
//        StartCoroutine(OutputRoutine(url));
//    }
//#endif
//    /// <summary>
//    /// 选择要上传的图片
//    /// </summary>
//    private void ImageSelect()
//    {
//#if UNITY_EDITOR || UNITY_STANDALONE
//        var extensions = new[] {
//                new ExtensionFilter("Image Files", "png", "jpg", "jpeg" ),
//            };
//        StandaloneFileBrowser.OpenFilePanelAsync("选择图片", "", extensions, false, (string[] paths) =>
//        {
//            if (paths.Length <= 0)
//            {
//                return;
//            }
//            StartCoroutine(OutputRoutine(new Uri(paths[0]).AbsoluteUri));
//        });
//#endif

//#if UNITY_WEBGL && !UNITY_EDITOR
//         UploadFile(gameObject.name, "OnFileUpload", ".png, .jpg, .jpeg", false);
//#endif
//    }
//    private IEnumerator OutputRoutine(string url)
//    {
//        var loader = new WWW(url);
//        yield return loader;
//        // 插入一张图片，并且显示上传进度条
//        CreateImage(url);
//        // 显示进度条
//        ShowUploadProgress();
//        // 上传图片
//        byte[] bArray = loader.texture.EncodeToJPG();
//        HttpManager.Instance.PostFile(Path.GetFileName(url), bArray, result =>
//        {
//            var tempImage = new PropertyInfo()
//            {
//                PropertyType = PropertyType.Image,
//                PropertyName = result.Id,
//                PropertyValue = HttpManager.Instance.GetUrl(WebAPI.DownImage + $"WebPlan2D/{result.Id}")
//            };
//            Data.Add(tempImage);
//            LblImageNumric.text = $"{Items.Count}/{ImageCount}";
//        }, HideUploadProgress, SetUploadProgress);
//    }

//    //private void UploadSuccess(UpImageUrlGet obj)
//    //{
//    //    Debug.Log(obj.data.url);
//    //    // 创建新的图片属性
//    //    AssetsProperty tempImage = new AssetsProperty()
//    //    {
//    //        PropertyType = BuildingPropertyType.Image,
//    //        PropertyValue = obj.data.url,
//    //    };
//    //    // 添加到属性列表中
//    //    //ParentView.Data.Add(tempImage);
//    //    GameObject.FindObjectOfType<SelectionSystem>().selectedObjects[0]?.GetComponent<SingleAssetIcon>().assetIconData.AssetInfo.AssetsProperties.Add(tempImage);
//    //    // 设置图片最大数量
//    //    LblImageNumric.text = $"{Items.Count}/{ImageNumric.PropertyValue}";
//    //    //// 保存编辑
//    //    //Messenger.Invoke(MessengerType.Save);
//    //}
//    public void SetData(List<PropertyInfo> list)
//    {
//        Data = list;
//        Data.ForEach(item =>
//        {
//            if (item.PropertyType == PropertyType.Image)
//            {
//                CreateImage(item.PropertyValue).name = item.PropertyName;
//            }
//        });
//    }
//    // 创建图片
//    public GameObject CreateImage(string path)
//    {
//        GameObject tempObj = GameObject.Instantiate(ItemPrefab, ContentImage);
//        Items.Add(tempObj.transform as RectTransform);
//        //读取本地图片显示
//        LoadImage(path, a =>
//         {
//             tempObj.GetComponent<RawImage>().texture = a;
//         });
//        // 刷新按钮
//        RefreshImageButton();
//        tempObj.SetActive(true);
//        return tempObj;
//    }
//    // 刷新前后图片按钮
//    public void RefreshImageButton()
//    {
//        if (Items.Count == 0 || Items.Count == 1)
//        {
//            BtnLeft.gameObject.SetActive(false);
//            BtnRight.gameObject.SetActive(false);
//        }
//        else if (Index == 0)
//        {
//            BtnLeft.gameObject.SetActive(false);
//            BtnRight.gameObject.SetActive(true);
//        }
//        else if (Index == Items.Count - 1)
//        {
//            BtnRight.gameObject.SetActive(false);
//            BtnLeft.gameObject.SetActive(true);
//        }
//        else
//        {
//            BtnRight.gameObject.SetActive(true);
//            BtnLeft.gameObject.SetActive(true);
//        }
//    }
//    // 加载本地图片
//    public void LoadImage(string path, Action<Texture2D> onLoad)
//    {
//        StartCoroutine(LoadTexture2D(path, onLoad));
//    }
//    public IEnumerator LoadTexture2D(string path, Action<Texture2D> onLoad)
//    {
//        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(path))
//        {
//            yield return uwr.SendWebRequest();

//            if (uwr.isNetworkError || uwr.isHttpError)
//            {
//                Debug.Log(uwr.error);
//            }
//            else
//            {
//                // Get downloaded asset bundle
//                var texture = DownloadHandlerTexture.GetContent(uwr);
//                texture.Compress(false);
//                onLoad(texture);
//            }
//        }
//    }
//    // 显示进度条
//    public void ShowUploadProgress()
//    {
//        ImageMask.SetActive(true);
//        SliderUploadImage.value = 0;
//        SliderUploadImage.gameObject.SetActive(true);
//    }
//    // 隐藏进度条
//    public void HideUploadProgress()
//    {
//        ImageMask.SetActive(false);
//        SliderUploadImage.value = 0;
//        SliderUploadImage.gameObject.SetActive(false);
//    }
//    /// <summary>
//    /// 设置SliderUploadImage的显示进度
//    /// </summary>
//    public void SetUploadProgress(float value)
//    {
//        SliderUploadImage.value = value;
//        if (value >= 1)
//        {
//            HideUploadProgress();
//        }
//    }
//    /// <summary>
//    /// 指定一个 item让其定位到ScrollRect中间
//    /// </summary>
//    /// <param name="target">需要定位到的目标</param>
//    public void CenterOnItem(RectTransform target)
//    {
//        // Item is here
//        var itemCenterPositionInScroll = GetWorldPointInWidget(scrollRect.GetComponent<RectTransform>(), GetWidgetWorldPoint(target));

//        // But must be here
//        var targetPositionInScroll = GetWorldPointInWidget(scrollRect.GetComponent<RectTransform>(), GetWidgetWorldPoint(viewPointTransform));

//        // So it has to move this distance
//        var difference = targetPositionInScroll - itemCenterPositionInScroll;
//        difference.z = 0f;

//        var newNormalizedPosition = new Vector2(difference.x / (contentTransform.rect.width - viewPointTransform.rect.width),
//            difference.y / (contentTransform.rect.height - viewPointTransform.rect.height));

//        newNormalizedPosition = scrollRect.normalizedPosition - newNormalizedPosition;

//        newNormalizedPosition.x = Mathf.Clamp01(newNormalizedPosition.x);
//        newNormalizedPosition.y = Mathf.Clamp01(newNormalizedPosition.y);

//        DOTween.To(() => scrollRect.normalizedPosition, x => scrollRect.normalizedPosition = x, newNormalizedPosition, 0.5f);
//    }

//    Vector3 GetWidgetWorldPoint(RectTransform target)
//    {
//        //pivot position + item size has to be included
//        var pivotOffset = new Vector3(
//            (0.5f - target.pivot.x) * target.rect.size.x,
//            (0.5f - target.pivot.y) * target.rect.size.y,
//            0f);
//        var localPosition = target.localPosition + pivotOffset;
//        return target.parent.TransformPoint(localPosition);
//    }

//    Vector3 GetWorldPointInWidget(RectTransform target, Vector3 worldPoint)
//    {
//        return target.InverseTransformPoint(worldPoint);
//    }
//    void CenterToSelected(GameObject selected)
//    {
//        var target = selected.GetComponent<RectTransform>();

//        Vector3 maskCenterPos = viewPointTransform.position + (Vector3)viewPointTransform.rect.center;
//        Debug.Log("遮罩位置: " + maskCenterPos);
//        Vector3 itemCenterPos = target.position;
//        Debug.Log("子对象中心位置: " + itemCenterPos);
//        Vector3 difference = maskCenterPos - itemCenterPos;
//        difference.z = 0;

//        Vector3 newPos = contentTransform.position + difference;

//        DOTween.To(() => contentTransform.position, x => contentTransform.position = x, newPos, 5);
//    }
//}
