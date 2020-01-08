//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.EventSystems;
//using UnityEngine.UI;
//using XFrame.UI;
//using UniRx;
//using UnityEngine.Events;
//using UnityEngine.AddressableAssets;
//using System;
//using System.Linq;
//using UnityEngine.ResourceManagement.AsyncOperations;
//using Newtonsoft.Json;
//using System.Text.RegularExpressions;
//using XTreeView;

//public class WorkingArea : Singleton<WorkingArea>, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
//{
//    /// <summary>
//    /// 当前建筑编号
//    /// </summary>
//    public string CurrentBuildingId;

//    //当前选择的素材
//    public Asset CurrentAsset;
//    //背景图片
//    public RawImage Background;
//    //当前总平面图
//    public SitePlan CurrentSitePlan;
//    //当前总平面图数据
//    public SitePlanData CurrentSitePlanData;
//    //当前楼层
//    public BuildingArea CurrentBuildingArea;
//    //当前楼层数据
//    public BuildingAreaData CurrentBuildingAreaData;
//    // 是否位于绘制区域内
//    public bool MouseIsInWorkingAre = false;
//    // 影子
//    public GameObject ShadowIcon;
//    // 提示
//    public Tips Tips;
//    // 绘制状态
//    public DrawState DrawState;
//    /// <summary>
//    /// 图标数据对象
//    /// </summary>
//    public List<GameObject> AssetObject = new List<GameObject>();
//    /// <summary>
//    /// 图标数据
//    /// </summary>
//    private List<AssetData> _assetData = new List<AssetData>();
//    //灾情图标对象
//    public List<GameObject> DisasterObject = new List<GameObject>();

//    /// <summary>
//    /// 选中的对象
//    /// </summary>
//    public List<GameObject> SelectedObjects;
//    /// <summary>
//    /// 初始屏幕坐标
//    /// </summary>
//    private Vector3 _initialScreenMousePos;
//    /// <summary>
//    /// 结束屏幕坐标
//    /// </summary>
//    private Vector3 _finalScreenMousePos;
//    /// <summary>
//    /// 是否使用框选
//    /// </summary>
//    public bool RectTool;
//    //得到canvas的ugui坐标
//    public RectTransform canvas;
//    //得到图片的ugui坐标
//    private RectTransform imgRect;
//    //用来得到鼠标和图片的差值
//    Vector2 offset = new Vector3();
//    // 选择事件
//    public class SelectedEvent : UnityEvent<GameObject, bool> { }
//    public SelectedEvent OnSelect = new SelectedEvent();
//    // 选择完成事件
//    public class SelectedOverEvent : UnityEvent { }
//    public SelectedOverEvent OnSelectOver = new SelectedOverEvent();
//    // 移动
//    public Transform DragUGUI;
//    // 父对象
//    private RectTransform parentRect;
//    // 当前正在绘制的图形
//    private GameObject CurrentDrawingIcon;
//    //当前正在绘制的图标数据
//    private AssetData CurrentDrawingIconData;
//    /// <summary>
//    /// 当前选择节点数据
//    /// </summary>
//    public DisasterNodeInfo CurrentDisasterNodeInfo;
//    /// <summary>
//    /// 唤醒
//    /// </summary>
//    private void Awake()
//    {
//        WorkingArea.Instance.Initialize();
//        Background = GetComponent<RawImage>();
//        parentRect = transform.parent.GetComponent<RectTransform>();
//        canvas = transform.parent.GetComponent<RectTransform>();
//        imgRect = GetComponent<RectTransform>();

//        DrawState = DrawState.EndDrawing;
//        RectTool = false;
//        SelectedObjects = new List<GameObject>();


//        // 赋予选择对象移动能力和选中效果和层次
//        OnSelect.AddListener((go, b) =>
//        {
//            if (b)
//            {
//                if (DragUGUI.childCount <= 0)
//                {
//                    var data = go.GetComponent<AssetIcon>().Data;
//                    switch (data.InteractiveMode)
//                    {
//                        case InteractiveMode.Single:
//                            DragUGUI.transform.position = go.transform.position;
//                            ((RectTransform)DragUGUI.transform).sizeDelta = ((RectTransform)go.transform).sizeDelta * ((RectTransform)go.transform).localScale.x;
//                            break;
//                        case InteractiveMode.Multiple:
//                            DragUGUI.transform.position = go.transform.position;
//                            ((RectTransform)DragUGUI.transform).sizeDelta = new Vector2(50f, 50f);
//                            break;
//                        case InteractiveMode.MultipleClosed:
//                            var size = go.transform.Find("FillingArea/Size");
//                            DragUGUI.transform.position = size.position;
//                            ((RectTransform)DragUGUI.transform).sizeDelta = ((RectTransform)size.transform).sizeDelta * ((RectTransform)size.transform).localScale.x;
//                            break;
//                        default:
//                            throw new ArgumentOutOfRangeException();
//                    }
//                }
//                go.transform.SetParent(DragUGUI);
//                go.gameObject.GetComponent<AssetIcon>().ShowOutline(true);
//                go.transform.SetAsLastSibling();
//                DragUGUI.SetAsLastSibling();
//            }
//            else
//            {
//                go.transform.SetParent(transform);
//                go.gameObject.GetComponent<AssetIcon>().ShowOutline(false);
//            }
//        });
//        // 选择完成
//        OnSelectOver.AddListener(() =>
//        {
//            MessageSystem.Broadcast<List<GameObject>>(Msg.OnSelectOver, SelectedObjects);
//        });
//        //监听事件
//        MessageSystem.AddListener<Asset>(Msg.SelectedAssetInfoToggle, SetData);
//        MessageSystem.AddListener(Msg.DrawingFinish, DrawingFinish);
//        //打开图片文件
//        MessageSystem.AddListener<string, Texture2D>(Msg.OpenFileImage, UploadTexture);
//        MessageSystem.AddListener<List<string>>(Msg.SelectAssetIcon, SelectedAsset);
//        MessageSystem.AddListener<string, bool>(Msg.ItemToggleValueChanged, ItemToggleValueChanged);
//    }

//    private void ItemToggleValueChanged(string objectId, bool arg2)
//    {
//        foreach (var item in AssetObject)
//        {
//            if (item.GetInstanceID().ToString() == objectId)
//            {
//                item.SetActive(arg2);
//            }
//        }
//    }

//    public void SelectedAsset(List<string> ids)
//    {
//        if (!Input.GetKey(KeyCode.LeftControl))
//        {
//            DeselectAll();
//        }
//        foreach (var id in ids)
//        {
//            GameObject newGameObj = AssetObject.Find(go => go.GetInstanceID().ToString() == id);
//            if (newGameObj == null || SelectedObjects.Contains(newGameObj) != false) continue;
//            SelectedObjects.Add(newGameObj);
//            OnSelect.Invoke(newGameObj, true);
//        }
//        OnSelectOver.Invoke();
//    }
//    /// <summary>
//    /// 上传图片
//    /// </summary>
//    /// <param name="fileName"></param>
//    /// <param name="texture"></param>
//    public void UploadTexture(string fileName, Texture2D texture)
//    {
//        if (CurrentSitePlan != null)
//        {
//            Debug.Log(fileName);
//            //序列化
//            byte[] bArray = texture.EncodeToJPG();
//            HttpManager.Instance.PostFile(fileName, bArray, result =>
//            {
//                CurrentSitePlan.ImageUrl = HttpManager.Instance.GetUrl(WebAPI.DownImage + $"WebPlan2D/{result.Id}");
//                HttpManager.Instance.Put<SitePlan>(WebAPI.EditorSitePlans + CurrentSitePlan.Id, CurrentSitePlan, () =>
//                {
//                    ShowBackground(texture);
//                }, MessageBox.Show);
//            });
//        }
//        else if (CurrentBuildingArea != null)
//        {
//            //序列化
//            var bArray = texture.EncodeToJPG();
//            HttpManager.Instance.PostFile(fileName, bArray, result =>
//            {
//                CurrentBuildingArea.ImageUrl = HttpManager.Instance.GetUrl(WebAPI.DownImage + $"WebPlan2D/{result.Id}");
//                HttpManager.Instance.Put<BuildingArea>(WebAPI.UpdateBuildingAreas + CurrentBuildingArea.Id, CurrentBuildingArea, () =>
//                {
//                    ShowBackground(texture);
//                }, MessageBox.Show);
//            });
//        }
//    }

//    private void Start()
//    {
//        gameObject.SetActive(false);
//    }
//    public void Save()
//    {
//        DeselectAll();
//        switch (GameManager.GameMode)
//        {
//            case GameMode.BaseInfo:
//                _assetData.Clear();
//                foreach (var item in AssetObject)
//                {
//                    AssetIcon assetIcon = item.GetComponent<AssetIcon>();
//                    _assetData.Add(assetIcon.GetData());
//                }
//                if (CurrentSitePlan != null)
//                {
//                    CurrentSitePlanData.Data = JsonConvert.SerializeObject(_assetData);
//                    if (string.IsNullOrEmpty(CurrentSitePlanData.Id))
//                    {
//                        HttpManager.Instance.Post<SitePlanData>(WebAPI.CreateSitePlanData, CurrentSitePlanData, x =>
//                        {
//                            MessageBox.Show("创建总平面图数据成功");
//                        });
//                    }
//                    else
//                    {
//                        HttpManager.Instance.Put<SitePlanData>(WebAPI.PutSitePlanData + CurrentSitePlanData.Id, CurrentSitePlanData, () =>
//                        {
//                            MessageBox.Show("修改总平面图数据成功");
//                        });
//                    }

//                }
//                else if (CurrentBuildingArea != null)
//                {
//                    CurrentBuildingAreaData.Data = JsonConvert.SerializeObject(_assetData);
//                    if (string.IsNullOrEmpty(CurrentBuildingAreaData.Id))
//                    {
//                        HttpManager.Instance.Post<BuildingAreaData>(WebAPI.CreateBuildingAreaData, CurrentBuildingAreaData, x =>
//                        {
//                            MessageBox.Show("创建楼层数据成功");
//                        });
//                    }
//                    else
//                    {
//                        HttpManager.Instance.Put<BuildingAreaData>(WebAPI.PutBuildingAreaData + CurrentBuildingAreaData.Id, CurrentBuildingAreaData, () =>
//                        {
//                            MessageBox.Show("修改楼层数据成功");
//                        });
//                    }
//                }
//                break;
//            case GameMode.WorkInfo:

//                break;
//            default:
//                throw new ArgumentOutOfRangeException();
//        }
//    }

//    public void Load()
//    {
//        DrawingCancel();
//        UIManager.GetView<UIViewMain>().RefreshTreeView();
//        foreach (var item in _assetData)
//        {
//            switch (item.InteractiveMode)
//            {
//                case InteractiveMode.Single:
//                    CreateSinglePointIconWithData(item, AssetObject);
//                    break;
//                case InteractiveMode.Multiple:
//                    CreateMultipleWithData(item, AssetObject);
//                    break;
//                case InteractiveMode.MultipleClosed:
//                    CreateMultipleClosedWithData(item, AssetObject);
//                    break;
//                default:
//                    throw new ArgumentOutOfRangeException();
//            }
//        }
//        //TODO 销毁节点游戏对象
//        foreach (var item in DisasterObject)
//        {
//            Addressables.ReleaseInstance(item);
//        }
//        DisasterObject.Clear();
//        //todo 如果游戏模式为想定作业模式，加载当前节点中当前楼层数据
//        if (GameManager.GameMode == GameMode.WorkInfo)
//        {
//            LoadNodeData();
//        }
//    }

//    public void Test()
//    {
//        if (CurrentSitePlan != null)
//        {
//            GetSitePlanData(CurrentSitePlan);
//        }
//        else if (CurrentBuildingArea != null)
//        {
//            GetBuildingAreaData(CurrentBuildingArea);
//        }
//    }
//    public void LoadNodeData()
//    {
//        //TODO  加载节点游戏对象
//        if (CurrentDisasterNodeInfo?.Data == null) return; ;
//        var data = JsonConvert.DeserializeObject<NodeData>(CurrentDisasterNodeInfo.Data);
//        //判断节点是否有数据存在
//        if (data == null || !data.Data.ContainsKey(GameManager.CurrentBuildingId) || !data.Data[GameManager.CurrentBuildingId].ContainsKey(GameManager.CurrentFloorId)) return;
//        var loadData = data.Data[GameManager.CurrentBuildingId][GameManager.CurrentFloorId];
//        if (loadData != null)
//            foreach (var item in loadData)
//            {
//                switch (item.InteractiveMode)
//                {
//                    case InteractiveMode.Single:
//                        CreateSinglePointIconWithData(item, DisasterObject);
//                        break;
//                    case InteractiveMode.Multiple:
//                        CreateMultipleWithData(item, DisasterObject);
//                        break;
//                    case InteractiveMode.MultipleClosed:
//                        CreateMultipleClosedWithData(item, DisasterObject);
//                        break;
//                }
//            }
//    }
//    /// <summary>
//    /// 获取节点数据
//    /// </summary>
//    public string GetNodeData(string json)
//    {
//        //获得节点工作区数据
//        var tempData = new List<AssetData>();
//        foreach (var item in DisasterObject)
//        {
//            var data = item.GetComponent<AssetIcon>().GetData();
//            tempData.Add(data);
//        }
//        //反序列化数据
//        NodeData nodeData = string.IsNullOrEmpty(json) ? new NodeData() : JsonConvert.DeserializeObject<NodeData>(json);
//        // 如果不存在当前建筑的节点数据，新建
//        if (!nodeData.Data.ContainsKey(GameManager.CurrentBuildingId))
//        {
//            nodeData.Data.Add(GameManager.CurrentBuildingId,new Dictionary<string, List<AssetData>>());
//        }
//        // 如果不存在当前建筑楼层的数据，新建
//        if (!nodeData.Data[GameManager.CurrentBuildingId].ContainsKey(GameManager.CurrentFloorId))
//        {
//            nodeData.Data[GameManager.CurrentBuildingId].Add(GameManager.CurrentFloorId,new List<AssetData>());
//        }
//        nodeData.Data[GameManager.CurrentBuildingId][GameManager.CurrentFloorId] = tempData;
//        string jsonData = JsonConvert.SerializeObject(nodeData);
//        return jsonData;
//    }
//    /// <summary>
//    /// 创建完成
//    /// </summary>
//    private void DrawingFinish()
//    {
//        if (CurrentDrawingIcon)
//        {
//            CreateTreeViewItem(CurrentAsset.FireElementId, CurrentDrawingIcon.GetInstanceID().ToString());
//            switch (CurrentAsset.InteractiveMode)
//            {
//                case InteractiveMode.Single:
//                    CurrentDrawingIcon.GetComponent<AssetIcon>().SetData(CurrentDrawingIconData);
//                    if (GameManager.GameMode == GameMode.BaseInfo)
//                    {
//                        AssetObject.Add(CurrentDrawingIcon);
//                    }
//                    else
//                    {
//                        DisasterObject.Add(CurrentDrawingIcon);
//                    }
//                    CurrentDrawingIcon = null;
//                    break;
//                case InteractiveMode.Multiple:
//                    CurrentDrawingIcon.GetComponent<MultipleAssetIcon>().Draw();
//                    CurrentDrawingIcon.GetComponent<AssetIcon>().SetData(CurrentDrawingIconData);
//                    if (GameManager.GameMode == GameMode.BaseInfo)
//                    {
//                        AssetObject.Add(CurrentDrawingIcon);
//                    }
//                    else
//                    {
//                        DisasterObject.Add(CurrentDrawingIcon);
//                    }
//                    CurrentDrawingIcon = null;
//                    ShadowIcon.SetActive(false);
//                    Tips.SetName("点击");
//                    break;
//                case InteractiveMode.MultipleClosed:
//                    CreateLine(ShadowIcon.transform.position, CurrentDrawingIcon.transform.position);
//                    Transform lineList = CurrentDrawingIcon.GetComponent<MultipleClosedAssetIcon>().LineList;
//                    int index = CurrentDrawingIcon.GetComponent<MultipleClosedAssetIcon>().linePointObjects.Count - 1;
//                    GameObject pointA = CurrentDrawingIcon.GetComponent<MultipleClosedAssetIcon>().linePointObjects[index];
//                    GameObject pointB = CurrentDrawingIcon.GetComponent<MultipleClosedAssetIcon>().linePointObjects[0];
//                    Addressables.InstantiateAsync("Line", lineList).Completed += loaded =>
//                    {
//                        GameObject line = loaded.Result;
//                        line.GetComponent<Line>().PointA = pointA;
//                        line.GetComponent<Line>().PointB = pointB;
//                        CurrentDrawingIcon.GetComponent<MultipleClosedAssetIcon>().Draw();
//                        CurrentDrawingIcon.GetComponent<AssetIcon>().SetData(CurrentDrawingIconData);
//                        if (GameManager.GameMode == GameMode.BaseInfo)
//                        {
//                            AssetObject.Add(CurrentDrawingIcon);
//                        }
//                        else
//                        {
//                            DisasterObject.Add(CurrentDrawingIcon);
//                        }
//                        CurrentDrawingIcon = null;
//                        CurrentDrawingIconData = null;
//                        ShadowIcon.SetActive(false);
//                        Tips.SetName("点击");
//                    };
//                    break;
//            }

//        }
//    }
//    /// <summary>
//    /// 创建TreeViewItem
//    /// </summary>
//    /// <param name="fireElementId"></param>
//    /// <param name="instanceID"></param>
//    public void CreateTreeViewItem(string fireElementId, string instanceID)
//    {
//        UIManager.GetView<UIViewMain>().CreateItemView(fireElementId, instanceID);
//    }
//    private void CancelSelectAssetInfo()
//    {
//        SetState(DrawState.EndDrawing);
//        CurrentAsset = null;
//    }

//    public void Reset()
//    {
//        DrawState = DrawState.EndDrawing;
//        RectTool = false;
//        SelectedObjects = new List<GameObject>();
//    }
//    /// <summary>
//    /// 固定更新
//    /// </summary>
//    private void FixedUpdate()
//    {
//        TipsUpdate();
//        ShadowIconUpdate();
//        MouseScrollWheel();
//        InputControl();
//    }

//    private void TipsUpdate()
//    {
//        Tips.transform.position = Input.mousePosition;
//    }

//    private void InputControl()
//    {
//        // 点了右键就结束绘制
//        if (Input.GetMouseButton(1))
//        {
//            DrawingCancel();
//        }
//        //删除
//        if (Input.GetKeyUp(KeyCode.Delete))
//        {
//            foreach (var item in SelectedObjects)
//            {
//                AssetObject.Remove(item);
//                Destroy(item);
//                //移除消防要素
//                UIManager.GetView<UIViewMain>().DeleteItemView(item.GetInstanceID().ToString());
//            }
//            DeselectAll();
//        }
//    }

//    /// <summary>
//    /// 设置工作区状态
//    /// </summary>
//    /// <param name="drawState"></param>
//    public void SetState(DrawState drawState)
//    {
//        DrawState = drawState;
//    }
//    /// <summary>
//    /// 设置工作区数据
//    /// </summary>
//    public void SetData(Asset assetInfo)
//    {
//        if (isActiveAndEnabled)
//        {
//            CurrentAsset = assetInfo;
//            if (assetInfo == null)
//            {
//                DeselectAll();
//                DestroyShadowIcon();
//                DestroyCurrentIcon();
//            }
//            else
//            {
//                Tips.Show();
//                Tips.SetName("点击");
//                CreateShadowIcon();
//            }
//        }
//    }


//    #region 影子
//    /// <summary>
//    /// 创建影子图标
//    /// </summary>
//    public void CreateShadowIcon()
//    {
//        switch (CurrentAsset.InteractiveMode)
//        {
//            case InteractiveMode.Single:
//                CreateSingleShadowIcon();
//                break;
//            case InteractiveMode.Multiple:
//                CreateMultipleShadowIcon();
//                break;
//            case InteractiveMode.MultipleClosed:
//                CreateMultipleClosedShadowIcon();
//                break;
//        }
//        DrawState = DrawState.Drawing;
//    }

//    private void CreateMultipleClosedShadowIcon()
//    {
//        //Cursor.visible = false;
//        Addressables.InstantiateAsync("Line", gameObject.transform).Completed += obj =>
//        {
//            ShadowIcon = obj.Result;
//            ShadowIcon.GetComponent<Image>().color = Color.white;
//            ShadowIcon.GetComponent<RectTransform>().sizeDelta = new Vector2(1, 1);
//            ShadowIcon.transform.GetComponent<RectTransform>().SetPivot(PivotPresets.MiddleLeft);
//            ShadowIcon.SetActive(false);
//        };
//    }

//    private void CreateMultipleShadowIcon()
//    {
//        Addressables.InstantiateAsync("Arrows", gameObject.transform).Completed += obj =>
//        {
//            ShadowIcon = obj.Result;
//            ShadowIcon.SetActive(false);
//        };
//    }

//    private void CreateSingleShadowIcon()
//    {
//        Addressables.InstantiateAsync(AssetManager.SinglepointIconPath, gameObject.transform).Completed += obj =>
//        {
//            ShadowIcon = obj.Result;
//            ((RectTransform)ShadowIcon.transform).sizeDelta = new Vector2(64, 64);
//            ShadowIcon.GetComponent<SinglepointIcon>().SetShadowMode();
//            ShadowIcon.GetComponent<SinglepointIcon>().ImageUrl.Value = CurrentAsset.ImageUrl;
//        };
//    }

//    // 改变影子图标的颜色
//    public void ChangeShadowIconColor(Color color)
//    {
//        if (ShadowIcon != null)
//        {
//            ShadowIcon.GetComponent<Image>().color = color;
//        }
//    }
//    /// <summary>
//    /// 删除影子图标
//    /// </summary>
//    public void DestroyShadowIcon()
//    {
//        //如果单点影子图标不为空，删除影子图标
//        if (ShadowIcon != null)
//        {
//            Addressables.ReleaseInstance(ShadowIcon);
//            ShadowIcon = null;
//        }
//    }
//    /// <summary>
//    /// 删除当前正在绘制的图标
//    /// </summary>
//    public void DestroyCurrentIcon()
//    {
//        if (CurrentDrawingIcon != null)
//        {
//            Addressables.ReleaseInstance(CurrentDrawingIcon);
//            CurrentDrawingIcon = null;
//        }
//    }
//    /// <summary>
//    /// 移动影子图标
//    /// </summary>
//    public void MoveShadowIcon()
//    {
//        if (ShadowIcon == null)
//        {
//            return;
//        }
//        switch (CurrentAsset.InteractiveMode)
//        {
//            case InteractiveMode.Single:
//                // 移动影子图标
//                ShadowIcon.transform.position = Input.mousePosition;
//                // 限制影子图标区域
//                var imgRect = ShadowIcon.transform as RectTransform;
//                var parentRect = transform as RectTransform;
//                Vector3 pos = transform.localPosition;

//                Vector3 minPosition = parentRect.rect.min - imgRect.rect.min * imgRect.localScale.x;
//                Vector3 maxPosition = parentRect.rect.max - imgRect.rect.max * imgRect.localScale.x;

//                pos.x = Mathf.Clamp(imgRect.localPosition.x, minPosition.x, maxPosition.x);
//                pos.y = Mathf.Clamp(imgRect.localPosition.y, minPosition.y, maxPosition.y);

//                imgRect.localPosition = pos;
//                // 设置在最前
//                ShadowIcon.transform.SetAsLastSibling();
//                break;
//            case InteractiveMode.Multiple:
//                CreateLine(ShadowIcon.transform.position, Input.mousePosition + (ShadowIcon.transform.position - Input.mousePosition).normalized, 64);
//                break;
//            case InteractiveMode.MultipleClosed:
//                CreateLine(ShadowIcon.transform.position, Input.mousePosition + (ShadowIcon.transform.position - Input.mousePosition).normalized);
//                break;
//        }
//    }
//    public void CreateLine(Vector3 pos1, Vector3 pos2, int height = 1)
//    {
//        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(transform as RectTransform, pos1, UIManager.GetCanvas().GetComponent<Canvas>().worldCamera, out var posTemp1))
//        {
//            pos1 = posTemp1;
//        }

//        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(transform as RectTransform, pos2, UIManager.GetCanvas().GetComponent<Canvas>().worldCamera, out var posTemp2))
//        {
//            pos2 = posTemp2;
//        }
//        ShadowIcon.transform.right = ((pos2 - pos1)).normalized;//改变线条的朝向
//        var distance = Vector3.Distance(pos1, pos2);//计算两点的距离
//        ShadowIcon.transform.GetComponent<RectTransform>().sizeDelta = new Vector3((distance), height, 1);
//    }
//    /// <summary>
//    /// 创建单点图标
//    /// </summary>
//    /// <returns></returns>
//    public void CreateSinglePointIcon()
//    {
//        //新建数据
//        var tempList = new List<PropertyInfo>();
//        if (CurrentAsset.PropertyInfos != null)
//        {
//            foreach (var item in CurrentAsset.PropertyInfos)
//            {
//                var tempItem = item.Clone();
//                tempList.Add(tempItem);
//            }
//        }
//        CurrentDrawingIconData = new AssetData()
//        {
//            Angle = CurrentAsset.Angle,
//            Color = CurrentAsset.Color,
//            Enabled = CurrentAsset.Enabled,
//            FillMode = CurrentAsset.FillMode,
//            FireElementId = CurrentAsset.FireElementId,
//            FixedSize = CurrentAsset.FixedSize,
//            Height = 64,
//            Id = Guid.NewGuid().ToString("N"),
//            ImageUrl = CurrentAsset.ImageUrl,
//            InteractiveMode = CurrentAsset.InteractiveMode,
//            MultiPoint = null,
//            Point = ((RectTransform)ShadowIcon.transform).anchoredPosition,
//            Name = CurrentAsset.Name,
//            PropertyInfos = tempList,
//            Width = 64
//        };
//        Addressables.InstantiateAsync(AssetManager.SinglepointIconPath, ShadowIcon.transform.position, ShadowIcon.transform.rotation, gameObject.transform).Completed += obj =>
//        {
//            SinglepointIcon go = obj.Result.GetComponent<SinglepointIcon>();
//            CurrentDrawingIcon = go.gameObject;
//            DrawingFinish();
//        };
//    }
//    /// <summary>
//    /// 创建单点图标
//    /// </summary>
//    /// <param name="data"></param>
//    public void CreateSinglePointIconWithData(AssetData data, List<GameObject> list)
//    {
//        Addressables.InstantiateAsync(AssetManager.SinglepointIconPath, gameObject.transform).Completed += obj =>
//          {
//              CreateTreeViewItem(data.FireElementId, obj.Result.GetInstanceID().ToString());
//              SinglepointIcon go = obj.Result.GetComponent<SinglepointIcon>();
//              go.SetData(data);
//              list.Add(go.gameObject);
//          };
//    }
//    public void CreateMultipleIcon()
//    {
//        if (CurrentDrawingIcon == null)
//        {
//            ShadowIcon.transform.position = Input.mousePosition;
//            //新建数据
//            CurrentDrawingIconData = new AssetData()
//            {
//                Angle = CurrentAsset.Angle,
//                Color = CurrentAsset.Color,
//                Enabled = CurrentAsset.Enabled,
//                FillMode = CurrentAsset.FillMode,
//                FireElementId = CurrentAsset.FireElementId,
//                FixedSize = CurrentAsset.FixedSize,
//                Height = CurrentAsset.Height,
//                Id = Guid.NewGuid().ToString("N"),
//                ImageUrl = CurrentAsset.ImageUrl,
//                InteractiveMode = CurrentAsset.InteractiveMode,
//                MultiPoint = new List<Vector2>(),
//                Point = ((RectTransform)ShadowIcon.transform).anchoredPosition,
//                Name = CurrentAsset.Name,
//                PropertyInfos = CurrentAsset.PropertyInfos,
//                Width = CurrentAsset.Width
//            };
//            DrawState = DrawState.Drawing;
//            Vector2 tempPos = Input.mousePosition;
//            CurrentDrawingIconData.MultiPoint.Add(Input.mousePosition);
//            // 开始绘制多边形结束按钮
//            Addressables.InstantiateAsync("MultiplepointIcon", gameObject.transform).Completed += obj =>
//            {
//                CurrentDrawingIcon = obj.Result;
//                CurrentDrawingIcon.transform.position = tempPos;
//                //CurrentDrawingIcon.GetComponent<MultipleClosedAssetIcon>().FillingArea.color = Color.red;
//                ShadowIcon.SetActive(true);
//                Transform pointList = CurrentDrawingIcon.GetComponent<MultipleAssetIcon>().PointList;
//                Addressables.InstantiateAsync("Point", pointList).Completed += loaded =>
//                {
//                    GameObject point = loaded.Result;
//                    point.transform.position = tempPos;
//                    //point.GetComponent<Point>().WorkingArea = this;
//                    point.GetComponent<Point>().assetIcon = CurrentDrawingIcon.GetComponent<MultipleAssetIcon>();
//                    // 把标记点保存
//                    CurrentDrawingIcon.GetComponent<MultipleAssetIcon>().LinePointObjects.Add(point);
//                    // 设置起始点
//                    CurrentDrawingIcon.GetComponent<MultipleAssetIcon>().StartPoint = point;
//                    ShadowIcon.transform.position = tempPos;
//                };
//            };
//        }
//        else
//        {
//            Transform pointList = CurrentDrawingIcon.GetComponent<MultipleAssetIcon>().PointList;
//            Vector2 tempPos = Input.mousePosition;
//            CurrentDrawingIconData.MultiPoint.Add(Input.mousePosition);
//            Addressables.InstantiateAsync("Point", pointList).Completed += obj =>
//            {
//                GameObject point = obj.Result;
//                point.transform.position = tempPos;
//                //point.GetComponent<Point>().WorkingArea = this;
//                point.GetComponent<Point>().assetIcon = CurrentDrawingIcon.GetComponent<MultipleAssetIcon>();

//                int index = CurrentDrawingIcon.GetComponent<MultipleAssetIcon>().LinePointObjects.Count - 1;
//                GameObject pointA = CurrentDrawingIcon.GetComponent<MultipleAssetIcon>().LinePointObjects[index];
//                GameObject pointB = point;
//                // 把标记点保存
//                CurrentDrawingIcon.GetComponent<MultipleAssetIcon>().LinePointObjects.Add(point);
//                CurrentDrawingIcon.GetComponent<MultipleAssetIcon>().EndPoint = point;
//                Transform lineList = CurrentDrawingIcon.GetComponent<MultipleAssetIcon>().LineList;

//                Addressables.InstantiateAsync("Arrows", lineList).Completed += loaded =>
//                {
//                    GameObject line = loaded.Result;
//                    line.GetComponent<Arrows>().PointA = pointA;
//                    line.GetComponent<Arrows>().PointB = pointB;
//                    //line.GetComponent<Line>().WorkingArea = this;
//                    ShadowIcon.transform.position = tempPos;
//                };
//            };
//        }
//    }
//    public void CreateMultipleWithData(AssetData data, List<GameObject> list)
//    {
//        Addressables.InstantiateAsync("MultiplepointIcon", gameObject.transform).Completed += obj =>
//        {
//            CreateTreeViewItem(data.FireElementId, obj.Result.GetInstanceID().ToString());
//            ((RectTransform)obj.Result.transform).anchoredPosition = data.Point;
//            MultipleAssetIcon icon = obj.Result.GetComponent<MultipleAssetIcon>();
//            for (int i = 0; i < data.MultiPoint.Count; i++)
//            {
//                Vector2 tempPos = data.MultiPoint[i];
//                int num = i;
//                Addressables.InstantiateAsync("Point", icon.PointList).Completed += obj1 =>
//                {
//                    GameObject point = obj1.Result;
//                    ((RectTransform)point.transform).anchoredPosition = tempPos;
//                    point.name = num.ToString();
//                    // 把标记点保存
//                    icon.LinePointObjects.Add(point);
//                    if (num < data.MultiPoint.Count - 1)
//                    {
//                        Addressables.InstantiateAsync("Arrows", icon.LineList).Completed += loaded2 =>
//                        {
//                            GameObject line = loaded2.Result;
//                            line.name = num.ToString();
//                            icon.Lines.Add(line);
//                            if (num == data.MultiPoint.Count - 2)
//                            {
//                                //画结束部分
//                                icon.SetPointLine();
//                                icon.Draw();
//                                icon.SetData(data);
//                                list.Add(icon.gameObject);
//                            }
//                        };
//                    }
//                };
//            };
//        };
//    }
//    public void CreateMultipleClosedIcon()
//    {
//        if (CurrentDrawingIcon == null)
//        {
//            ShadowIcon.transform.position = Input.mousePosition;
//            //新建数据
//            CurrentDrawingIconData = new AssetData()
//            {
//                Angle = CurrentAsset.Angle,
//                Color = CurrentAsset.Color,
//                Enabled = CurrentAsset.Enabled,
//                FillMode = CurrentAsset.FillMode,
//                FireElementId = CurrentAsset.FireElementId,
//                FixedSize = CurrentAsset.FixedSize,
//                Height = CurrentAsset.Height,
//                Id = Guid.NewGuid().ToString("N"),
//                ImageUrl = CurrentAsset.ImageUrl,
//                InteractiveMode = CurrentAsset.InteractiveMode,
//                MultiPoint = new List<Vector2>(),
//                Point = ((RectTransform)ShadowIcon.transform).anchoredPosition,
//                Name = CurrentAsset.Name,
//                PropertyInfos = CurrentAsset.PropertyInfos,
//                Width = CurrentAsset.Width
//            };
//            DrawState = DrawState.Drawing;
//            Vector2 tempPos = Input.mousePosition;
//            CurrentDrawingIconData.MultiPoint.Add(Input.mousePosition);
//            // 开始绘制多边形结束按钮
//            Addressables.InstantiateAsync("MultiplepointIconClosed", gameObject.transform).Completed += obj =>
//            {
//                CurrentDrawingIcon = obj.Result;
//                CurrentDrawingIcon.transform.position = tempPos;
//                //CurrentDrawingIcon.GetComponent<MultipleClosedAssetIcon>().FillingArea.color = Color.red;
//                ShadowIcon.SetActive(true);
//                Transform pointList = CurrentDrawingIcon.GetComponent<MultipleClosedAssetIcon>().PointList;
//                Addressables.InstantiateAsync("Point", pointList).Completed += loaded =>
//                {
//                    GameObject point = loaded.Result;
//                    point.transform.position = tempPos;
//                    //point.GetComponent<Point>().WorkingArea = this;
//                    point.GetComponent<Point>().assetIcon = CurrentDrawingIcon.GetComponent<MultipleClosedAssetIcon>();
//                    // 把标记点保存
//                    CurrentDrawingIcon.GetComponent<MultipleClosedAssetIcon>().linePointObjects.Add(point);
//                    // 设置起始点
//                    CurrentDrawingIcon.GetComponent<MultipleClosedAssetIcon>().StartPoint = point;
//                    ShadowIcon.transform.position = tempPos;
//                };
//            };
//        }
//        else
//        {
//            Transform pointList = CurrentDrawingIcon.GetComponent<MultipleClosedAssetIcon>().PointList;
//            Vector2 tempPos = Input.mousePosition;
//            CurrentDrawingIconData.MultiPoint.Add(Input.mousePosition);
//            Addressables.InstantiateAsync("Point", pointList).Completed += obj =>
//            {
//                GameObject point = obj.Result;
//                point.transform.position = tempPos;
//                //point.GetComponent<Point>().WorkingArea = this;
//                point.GetComponent<Point>().assetIcon = CurrentDrawingIcon.GetComponent<MultipleClosedAssetIcon>();

//                int index = CurrentDrawingIcon.GetComponent<MultipleClosedAssetIcon>().linePointObjects.Count - 1;
//                GameObject pointA = CurrentDrawingIcon.GetComponent<MultipleClosedAssetIcon>().linePointObjects[index];
//                GameObject pointB = point;
//                // 把标记点保存
//                CurrentDrawingIcon.GetComponent<MultipleClosedAssetIcon>().linePointObjects.Add(point);
//                CurrentDrawingIcon.GetComponent<MultipleClosedAssetIcon>().EndPoint = point;
//                Transform lineList = CurrentDrawingIcon.GetComponent<MultipleClosedAssetIcon>().LineList;

//                Addressables.InstantiateAsync("Line", lineList).Completed += loaded =>
//                 {
//                     GameObject line = loaded.Result;
//                     line.GetComponent<Line>().PointA = pointA;
//                     line.GetComponent<Line>().PointB = pointB;
//                     //line.GetComponent<Line>().WorkingArea = this;
//                     ShadowIcon.transform.position = tempPos;
//                 };
//            };
//        }
//    }
//    public void CreateMultipleClosedWithData(AssetData data, List<GameObject> list)
//    {
//        Addressables.InstantiateAsync("MultiplepointIconClosed", gameObject.transform).Completed += obj =>
//        {
//            CreateTreeViewItem(data.FireElementId, obj.Result.GetInstanceID().ToString());
//            ((RectTransform)obj.Result.transform).anchoredPosition = data.Point;
//            MultipleClosedAssetIcon icon = obj.Result.GetComponent<MultipleClosedAssetIcon>();
//            for (int i = 0; i < data.MultiPoint.Count; i++)
//            {
//                Vector2 tempPos = data.MultiPoint[i];
//                int num = i;
//                Addressables.InstantiateAsync("Point", icon.PointList).Completed += obj1 =>
//                {
//                    GameObject point = obj1.Result;
//                    ((RectTransform)point.transform).anchoredPosition = tempPos;
//                    point.name = num.ToString();
//                    // 把标记点保存
//                    icon.linePointObjects.Add(point);
//                    Addressables.InstantiateAsync("Line", icon.LineList).Completed += loaded2 =>
//                    {
//                        GameObject line = loaded2.Result;
//                        line.name = num.ToString();
//                        icon.Lines.Add(line);
//                        if (num == data.MultiPoint.Count - 1)
//                        {
//                            //画结束部分
//                            icon.SetPointLine();
//                            icon.Draw();
//                            icon.SetData(data);
//                            list.Add(icon.gameObject);
//                        }
//                    };
//                };
//            };
//        };
//    }
//    #endregion

//    #region 创建操作
//    /// <summary>
//    /// 创建图标
//    /// </summary>
//    public void DrawingIcon(PointerEventData eventData)
//    {
//        if (eventData.pointerEnter == gameObject && DrawState == DrawState.Drawing)
//        {
//            switch (CurrentAsset.InteractiveMode)
//            {
//                case InteractiveMode.Single:
//                    CreateSinglePointIcon();
//                    break;
//                case InteractiveMode.Multiple:
//                    CreateMultipleIcon();
//                    break;
//                case InteractiveMode.MultipleClosed:
//                    CreateMultipleClosedIcon();
//                    break;
//            }
//        }
//    }
//    /// <summary>
//    /// 取消创建
//    /// </summary>
//    public void DrawingCancel()
//    {
//        DeselectAll();
//        //绘制结束
//        SetState(DrawState.EndDrawing);
//        MessageSystem.Broadcast(Msg.DrawingCancel);
//    }
//    /// <summary>
//    /// 影子图标更新
//    /// </summary>
//    public void ShadowIconUpdate()
//    {
//        switch (DrawState)
//        {
//            case DrawState.EndDrawing:
//                // 删除影子图标
//                DestroyShadowIcon();
//                // 删除当前绘制图标
//                DestroyCurrentIcon();
//                Tips.Hide();
//                break;
//            case DrawState.Drawing:
//                //移动影子图标
//                MoveShadowIcon();
//                break;
//        }
//    }
//    #endregion
//    #region 选择操作

//    public void SelectIcon(PointerEventData eventData)
//    {
//        // 选择目标不是画布，进行选择，点中画布取消所有选择
//        if (eventData.pointerEnter != gameObject && DrawState != DrawState.Drawing)
//        {
//            RaycastHit2D hit = Physics2D.Raycast(new Vector2(eventData.position.x, eventData.position.y), Vector2.zero);
//            if (hit.collider != null)
//            {
//                GameObject go = hit.collider.gameObject;
//                //没有按住左边Control键，取消所有选择
//                if (!Input.GetKey(KeyCode.LeftControl))
//                {
//                    DeselectAll();
//                }
//                SelectOrDeselectDepends(go);
//            }
//        }
//        else
//        {
//            DeselectAll();
//        }
//    }
//    /// <summary>
//    /// 选择或者取消选择
//    /// </summary>
//    void SelectOrDeselectDepends(GameObject GO)
//    {
//        if (SelectedObjects.Contains(GO))
//        {
//            SelectedObjects.Remove(GO.gameObject);
//            OnSelect.Invoke(GO, false);
//        }
//        else
//        {
//            SelectedObjects.Add(GO.gameObject);
//            OnSelect.Invoke(GO, true);
//        }
//        OnSelectOver.Invoke();
//    }
//    public void Select(GameObject GO)
//    {
//        //没有按住左边Control键，取消所有选择
//        if (!Input.GetKey(KeyCode.LeftControl))
//        {
//            DeselectAll();
//        }
//        if (!SelectedObjects.Contains(GO))
//        {
//            SelectedObjects.Add(GO.gameObject);
//            OnSelect.Invoke(GO, true);
//        }
//        OnSelectOver.Invoke();
//    }

//    /// <summary>
//    /// 取消全部选择
//    /// </summary>
//    public void DeselectAll()
//    {
//        for (int i = 0; i < SelectedObjects.Count; i++)
//        {
//            OnSelect.Invoke(SelectedObjects[i], false);
//        }
//        SelectedObjects.Clear();
//        OnSelectOver.Invoke();
//    }
//    /// <summary>
//    /// 选择全部
//    /// </summary>
//    void SelectAll(Collider2D[] colliders)
//    {
//        if (colliders.Length != 0)
//        {
//            foreach (Collider2D col in colliders)
//            {
//                GameObject newGameObj = col.gameObject;
//                if (SelectedObjects.Contains(newGameObj) == false)
//                {
//                    //newGameObj.GetComponent<SetOutline>().ShowOutline(true);
//                    SelectedObjects.Add(newGameObj);
//                    OnSelect.Invoke(newGameObj, true);
//                }
//            }
//            OnSelectOver.Invoke();
//        }
//    }
//    /// <summary>
//    /// 绘制框选区域
//    /// </summary>
//    void OnGUI()
//    {
//        if (RectTool == true)
//        {

//            Vector3 init = _initialScreenMousePos;
//            Vector3 final = _finalScreenMousePos;


//            float smallX = Mathf.Min(init.x, final.x);
//            float largeX = Mathf.Max(init.x, final.x);


//            float smallY = Mathf.Min(Screen.height - init.y, Screen.height - final.y);
//            float largeY = Mathf.Max(Screen.height - init.y, Screen.height - final.y);


//            DrawScreenRect(new Rect(smallX, smallY, largeX - smallX, largeY - smallY), new Color(0.8f, 0.8f, 0.95f, 0.25f));
//            DrawScreenRectBorder(new Rect(smallX, smallY, largeX - smallX, largeY - smallY), 2, Color.green);
//        }
//    }

//    private static Texture2D _staticRectTexture;
//    private static GUIStyle _staticRectStyle;
//    public static void DrawScreenRect(Rect rect, Color color)
//    {
//        if (_staticRectTexture == null)
//        {
//            _staticRectTexture = new Texture2D(1, 1);
//        }

//        if (_staticRectStyle == null)
//        {
//            _staticRectStyle = new GUIStyle();
//        }

//        _staticRectTexture.SetPixel(0, 0, color);
//        _staticRectTexture.Apply();

//        _staticRectStyle.normal.background = _staticRectTexture;

//        GUI.Box(rect, GUIContent.none, _staticRectStyle);
//    }
//    public static void DrawScreenRectBorder(Rect rect, float thickness, Color color)
//    {
//        // Top
//        DrawScreenRect(new Rect(rect.xMin, rect.yMin, rect.width, thickness), color);
//        // Left
//        DrawScreenRect(new Rect(rect.xMin, rect.yMin, thickness, rect.height), color);
//        // Right
//        DrawScreenRect(new Rect(rect.xMax - thickness, rect.yMin, thickness, rect.height), color);
//        // Bottom
//        DrawScreenRect(new Rect(rect.xMin, rect.yMax - thickness, rect.width, thickness), color);
//    }
//    /// <summary>
//    /// 拖拽开始
//    /// </summary>
//    public void OnBeginDrag(PointerEventData eventData)
//    {
//        //Debug.Log("begin");
//        if (Input.GetKey(KeyCode.LeftControl))
//        {
//            RectTool = true;
//            _initialScreenMousePos = Input.mousePosition;
//        }
//        else
//        {
//            Vector2 mouseDown = eventData.position;
//            Vector2 mouseUguiPos = new Vector2();
//            bool isRect = RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas, mouseDown, eventData.enterEventCamera, out mouseUguiPos);
//            if (isRect)
//            {
//                offset = imgRect.anchoredPosition - mouseUguiPos;
//            }
//        }
//    }
//    /// <summary>
//    /// 拖拽结束
//    /// </summary>
//    public void OnEndDrag(PointerEventData eventData)
//    {
//        //Debug.Log("end");
//        if (Input.GetKey(KeyCode.LeftControl))
//        {
//            if (RectTool == true)
//            {
//                Collider2D[] inRect = Physics2D.OverlapAreaAll(_initialScreenMousePos, _finalScreenMousePos);

//                SelectAll(inRect);
//            }
//        }
//        else
//        {
//            offset = Vector2.zero;
//        }
//        RectTool = false;
//    }

//    public void OnDrag(PointerEventData eventData)
//    {
//        //Debug.Log("Drag");
//        if (Input.GetKey(KeyCode.LeftControl))
//        {
//            if (RectTool && MouseIsInWorkingAre)
//            {
//                _finalScreenMousePos = Input.mousePosition;
//            }

//        }
//        else
//        {
//            Vector2 mouseDrag = eventData.position;
//            Vector2 uguiPos = new Vector2();
//            bool isRect = RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas, mouseDrag, eventData.enterEventCamera, out uguiPos);

//            if (isRect)
//            {
//                imgRect.anchoredPosition = offset + uguiPos;
//                ClampToParentWindow();
//            }
//        }
//    }
//    #endregion
//    #region 鼠标操作
//    /// <summary>
//    /// 鼠标单点操作
//    /// </summary>
//    /// <param name="eventData"></param>
//    public void OnPointerClick(PointerEventData eventData)
//    {
//        if (!eventData.dragging)
//        {
//            switch (eventData.button)
//            {
//                case PointerEventData.InputButton.Left:
//                    // 确定绘制
//                    DrawingIcon(eventData);
//                    // 选择图标
//                    SelectIcon(eventData);
//                    break;
//                case PointerEventData.InputButton.Right:

//                    break;
//            }
//        }
//    }
//    /// <summary>
//    /// 鼠标进入工作区
//    /// </summary>
//    /// <param name="eventData"></param>
//    public void OnPointerEnter(PointerEventData eventData)
//    {
//        //MessageBox.Show($"进入{eventData.pointerEnter.name}");
//        MouseIsInWorkingAre = true;
//    }
//    /// <summary>
//    /// 鼠标离开工作区
//    /// </summary>
//    /// <param name="eventData"></param>
//    public void OnPointerExit(PointerEventData eventData)
//    {
//        //MessageBox.Show($"退出{eventData.pointerEnter.name}");
//        MouseIsInWorkingAre = false;
//    }
//    /// <summary>
//    /// 鼠标滚轮操作
//    /// </summary>
//    public void MouseScrollWheel()
//    {
//        if (MouseIsInWorkingAre)
//        {
//            float delX = Input.mousePosition.x - transform.position.x;
//            float delY = Input.mousePosition.y - transform.position.y;

//            float scaleX = delX / GetComponent<RectTransform>().rect.width / transform.localScale.x;
//            float scaleY = delY / GetComponent<RectTransform>().rect.height / transform.localScale.y;


//            if (Input.GetAxis("Mouse ScrollWheel") > 0)
//            {
//                if (transform.localScale.x >= 4f)
//                {
//                    return;
//                }
//                transform.localScale += Vector3.one * 0.1f;
//                GetComponent<RectTransform>().pivot += new Vector2(scaleX, scaleY);
//                transform.position += new Vector3(delX, delY, 0);
//                SetSize();
//            }
//            else if (Input.GetAxis("Mouse ScrollWheel") < 0)
//            {
//                if (transform.localScale.x <= 0.5f)
//                {
//                    return;
//                }
//                transform.localScale += Vector3.one * -0.1f;
//                GetComponent<RectTransform>().pivot += new Vector2(scaleX, scaleY);
//                transform.position += new Vector3(delX, delY, 0);
//                SetSize();
//            }
//        }
//    }
//    /// <summary>
//    /// 设置窗体大小
//    /// </summary>
//    public void SetSize()
//    {
//        var x1 = 2 * (imgRect.rect.width * imgRect.localScale.x);
//        var y1 = 2 * (imgRect.rect.height * imgRect.localScale.y);
//        var clampX = Mathf.Clamp(x1, Screen.width, Screen.width * 20);
//        var clampY = Mathf.Clamp(y1, Screen.height, Screen.height * 20);
//        parentRect.sizeDelta = new Vector2(clampX, clampY);
//    }
//    /// <summary>
//    /// 固定在父窗口内
//    /// </summary>
//    private void ClampToParentWindow()
//    {
//        var pos = transform.localPosition;

//        var minPosition = parentRect.rect.min - imgRect.rect.min * imgRect.localScale.x;
//        var maxPosition = parentRect.rect.max - imgRect.rect.max * imgRect.localScale.x;

//        pos.x = Mathf.Clamp(imgRect.localPosition.x, minPosition.x, maxPosition.x);
//        pos.y = Mathf.Clamp(imgRect.localPosition.y, minPosition.y, maxPosition.y);

//        imgRect.localPosition = pos;
//    }

//    /// <summary>
//    /// 加载建筑区域数据
//    /// </summary>
//    internal void GetBuildingAreaData(BuildingArea buildingArea)
//    {
//        CurrentSitePlan = null;
//        CurrentBuildingArea = buildingArea;
//        HttpManager.Instance.Get<BuildingAreaData>(WebAPI.GetBuildingAreaData + buildingArea.Id, data =>
//        {
//            ClearAssetObject();
//            ClearBackground();
//            if (IsUrl(CurrentBuildingArea.ImageUrl))
//            {
//                //加载背景图
//                HttpManager.Instance.GetImage(CurrentBuildingArea.ImageUrl, texture =>
//                {
//                    ShowBackground(texture);
//                    if (data != null && !string.IsNullOrEmpty(data.Id))
//                    {
//                        CurrentBuildingAreaData = data;
//                        Debug.Log(data.Data);
//                        var assetDatas = JsonConvert.DeserializeObject<List<AssetData>>(data.Data);
//                        SetAssetData(assetDatas);
//                        Load();
//                    }
//                    else
//                    {
//                        CurrentBuildingAreaData = new BuildingAreaData {BuildingAreaId = buildingArea.Id};
//                        UIManager.GetView<UIViewMain>().RefreshTreeView();
//                    }
//                });

//            }
//            else
//            {
//                //显示创建背景图页面
//                UIManager.Show<UIViewCreateBackground>();
//            }
//        }, () =>
//        {
//            MessageBox.Show("获取建筑区域数据失败");
//        });
//    }
//    /// <summary>
//    /// 加载总平面图数据
//    /// </summary>
//    internal void GetSitePlanData(SitePlan sitePlan)
//    {
//        CurrentSitePlan = sitePlan;
//        CurrentBuildingArea = null;
//        HttpManager.Instance.Get<SitePlanData>(WebAPI.GetSitePlanData + CurrentSitePlan.Id, data =>
//        {
//            ClearAssetObject();
//            ClearBackground();
//            if (IsUrl(CurrentSitePlan.ImageUrl))
//            {
//                //加载背景图
//                HttpManager.Instance.GetImage(CurrentSitePlan.ImageUrl, ShowBackground);
//            }
//            else
//            {
//                //显示创建背景图页面
//                UIManager.Show<UIViewCreateBackground>();
//            }
//            if (data != null && !string.IsNullOrEmpty(data.Id))
//            {
//                CurrentSitePlanData = data;
//                Debug.Log(data.Data);
//                var assetDatas = JsonConvert.DeserializeObject<List<AssetData>>(data.Data);
//                SetAssetData(assetDatas);
//                Load();
//            }
//            else
//            {
//                CurrentSitePlanData = new SitePlanData { SitePlanId = sitePlan.Id };
//                UIManager.GetView<UIViewMain>().RefreshTreeView();
//            }
//        }, () =>
//        {
//            MessageBox.Show("获取建筑区域数据失败");
//        });
//    }
//    /// <summary>
//    /// 清空背景
//    /// </summary>
//    public void ClearBackground()
//    {
//        Destroy(Background.texture);
//        Background.texture = null;
//        Resources.UnloadUnusedAssets();
//        gameObject.SetActive(false);
//    }
//    public void ShowBackground(Texture2D texture)
//    {
//        UIManager.Hide<UIViewCreateBackground>();
//        Background.texture = texture;
//        gameObject.SetActive(true);
//    }
//    /// <summary>
//    /// 判断一个字符串是否为url
//    /// </summary>
//    /// <param name="str"></param>
//    /// <returns></returns>
//    public static bool IsUrl(string str)
//    {
//        try
//        {
//            string Url = @"^http(s)?://";
//            return Regex.IsMatch(str, Url);
//        }
//        catch (Exception ex)
//        {
//            return false;
//        }
//    }
//    /// <summary>
//    /// 清空素材对象
//    /// </summary>
//    public void ClearAssetObject()
//    {
//        DeselectAll();
//        _assetData.Clear();
//        foreach (var item in AssetObject)
//        {
//            Addressables.ReleaseInstance(item);
//            //移除消防要素
//            UIManager.GetView<UIViewMain>().DeleteItemView(item.GetInstanceID().ToString());
//        }
//        AssetObject.Clear();
//    }
//    public void SetAssetData(List<AssetData> data)
//    {
//        _assetData = data;
//    }
//    #endregion
//}
///// <summary>
///// 绘制状态
///// </summary>
//public enum DrawState
//{
//    /// <summary>
//    /// 结束绘图
//    /// </summary>
//    EndDrawing,
//    /// <summary>
//    /// 绘图
//    /// </summary>
//    Drawing,
//}
