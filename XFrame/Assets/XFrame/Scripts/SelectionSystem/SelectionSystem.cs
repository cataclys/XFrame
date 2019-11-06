//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//using UnityEngine.EventSystems;
//using UnityEngine.Events;

//public class SelectionSystem : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
//{
//    /// <summary>
//    /// 选中的对象
//    /// </summary>
//    public List<GameObject> selectedObjects;
//    /// <summary>
//    /// 初始屏幕坐标
//    /// </summary>
//    Vector3 initialScreenMousePos;
//    /// <summary>
//    /// 结束屏幕坐标
//    /// </summary>
//    Vector3 finalScreenMousePos;
//    /// <summary>
//    /// 是否使用框选
//    /// </summary>
//    public bool rectTool;

//    public bool isInBackground = false;
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

//    void Start()
//    {
//        canvas = transform.parent.GetComponent<RectTransform>();
//        imgRect = GetComponent<RectTransform>();

//        selectedObjects = new List<GameObject>();
//        rectTool = false;

//        // 赋予选择对象移动能力和选中效果和层次
//        OnSelect.AddListener((go, b) =>
//        {
//            if (b)
//            {
//                go.transform.SetParent(DragUGUI);
//                go.gameObject.GetComponent<SetOutline>().ShowOutline(true);
//                go.transform.SetAsLastSibling();
//                DragUGUI.SetAsLastSibling();
//                Debug.Log("确定选择：" + go.name);

//            }
//            else
//            {
//                go.transform.SetParent(transform);
//                go.gameObject.GetComponent<SetOutline>().ShowOutline(false);
//                Debug.Log("取消选择：" + go.name);
//            }
//        });
//        // 选择完成
//        OnSelectOver.AddListener(() =>
//        {
//            Messenger<List<GameObject>>.Invoke(MessengerType.OnSelectOver, selectedObjects);
//        });
//    }

//    void Update()
//    {
//        if (Input.GetKeyDown(KeyCode.Delete) && selectedObjects.Count > 0)
//        {
//            foreach (var item in selectedObjects)
//            {
//                UIManager.Instance.GetView<UIViewRight>().DeleteItemView(item.GetInstanceID());
//                Destroy(item);
//            }
//            selectedObjects.Clear();
//            WorkingArea.IsDataChanged = true;
//        }

//        //    Vector3 screenMousePos;

//        //    if (Input.GetMouseButtonDown(0))
//        //    {
//        //        screenMousePos = Input.mousePosition;

//        //        initialScreenMousePos = screenMousePos;

//        //        RaycastHit2D hit = Physics2D.Raycast(new Vector2(screenMousePos.x, screenMousePos.y), Vector3.forward);

//        //        if (hit.collider != null)
//        //        {
//        //            GameObject go = hit.collider.gameObject;
//        //            Debug.Log(go);
//        //            if (!selectedObjects.Contains(go))
//        //            {
//        //                if (selectedObjects.Count != 0)
//        //                {
//        //                    if (!Input.GetKey(KeyCode.LeftControl))
//        //                    {
//        //                        deselectAll();
//        //                    }
//        //                }
//        //            }
//        //            selectOrDeselectDepends(go);
//        //        }
//        //        else
//        //        {
//        //            if (Input.GetKey(KeyCode.LeftControl))
//        //            {
//        //                rectTool = true;
//        //            }
//        //            else
//        //            {
//        //                deselectAll();
//        //            }
//        //        }
//        //    }
//        //    else 
//        //    {
//        //        screenMousePos = Input.mousePosition;

//        //        finalScreenMousePos = screenMousePos;

//        //        if (Input.GetMouseButtonUp(0)) 
//        //        {
//        //            if (rectTool == true)
//        //            {
//        //                Collider2D[] inRect = Physics2D.OverlapAreaAll(initialScreenMousePos, finalScreenMousePos);

//        //                selectAll(inRect);

//        //                rectTool = false;
//        //            }
//        //        }
//        //    }
//    }


//    /// <summary>
//    /// 选择或者取消选择
//    /// </summary>
//    void SelectOrDeselectDepends(GameObject GO)
//    {
//        if (selectedObjects.Contains(GO))
//        {
//            selectedObjects.Remove(GO.gameObject);
//            OnSelect.Invoke(GO, false);
//        }
//        else
//        {
//            selectedObjects.Add(GO.gameObject);
//            OnSelect.Invoke(GO, true);
//        }
//        OnSelectOver.Invoke();
//    }
//    public void Select(GameObject GO)
//    {
//        if (!selectedObjects.Contains(GO))
//        {
//            selectedObjects.Add(GO.gameObject);
//            OnSelect.Invoke(GO, true);
//        }
//        OnSelectOver.Invoke();
//    }

//    /// <summary>
//    /// 取消全部
//    /// </summary>
//    public void DeselectAll()
//    {
//        for (int i = 0; i < selectedObjects.Count; i++)
//        {
//            //selectedObjects[i].GetComponent<SetOutline>().ShowOutline(false);
//            OnSelect.Invoke(selectedObjects[i], false);
//        }
//        selectedObjects.Clear();
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
//                if (selectedObjects.Contains(newGameObj) == false)
//                {
//                    //newGameObj.GetComponent<SetOutline>().ShowOutline(true);
//                    selectedObjects.Add(newGameObj);
//                    OnSelect.Invoke(newGameObj, true);
//                }
//            }
//            OnSelectOver.Invoke();
//        }
//    }

//    void OnGUI()
//    {
//        if (rectTool == true)
//        {

//            Vector3 init = initialScreenMousePos;
//            Vector3 final = finalScreenMousePos;


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

//    public void OnBeginDrag(PointerEventData eventData)
//    {
//        //Debug.Log("begin");
//        if (Input.GetKey(KeyCode.LeftControl))
//        {
//            rectTool = true;
//            initialScreenMousePos = Input.mousePosition;
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

//    public void OnEndDrag(PointerEventData eventData)
//    {
//        //Debug.Log("end");
//        if (Input.GetKey(KeyCode.LeftControl))
//        {
//            if (rectTool == true)
//            {
//                Collider2D[] inRect = Physics2D.OverlapAreaAll(initialScreenMousePos, finalScreenMousePos);

//                SelectAll(inRect);
//            }
//        }
//        else
//        {
//            offset = Vector2.zero;
//        }
//        rectTool = false;
//    }

//    public void OnDrag(PointerEventData eventData)
//    {
//        //Debug.Log("Drag");
//        if (Input.GetKey(KeyCode.LeftControl))
//        {
//            if (rectTool && isInBackground)
//            {
//                finalScreenMousePos = Input.mousePosition;
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
//            }
//        }
//    }
//    public void OnPointerClick(PointerEventData eventData)
//    {
//        if (offset == Vector2.zero)
//        {
//            //Debug.Log("Click");
//            //GameObject go = null;
//            //if (eventData.pointerEnter.GetComponent<SetOutline>() != null)
//            //{
//            //    go = eventData.pointerEnter;
//            //}
//            //else if (eventData.pointerEnter.transform.parent.parent.GetComponent<SetOutline>() != null)
//            //{
//            //    //todo 选中outline  做特殊处理  如果点击到的是Size 要做什么处理
//            //    go = eventData.pointerEnter.transform.parent.parent.gameObject;
//            //}
//            RaycastHit2D hit = Physics2D.Raycast(new Vector2(eventData.position.x, eventData.position.y), Vector2.zero);

//            if (hit.collider != null)
//            {
//                GameObject go = hit.collider.gameObject;
//                if (eventData.button == PointerEventData.InputButton.Left)
//                {
//                    if (!selectedObjects.Contains(go))
//                    {
//                        if (selectedObjects.Count != 0)
//                        {
//                            if (!Input.GetKey(KeyCode.LeftControl))
//                            {
//                                DeselectAll();
//                            }
//                        }
//                    }
//                    SelectOrDeselectDepends(go);
//                }
//                if (eventData.button == PointerEventData.InputButton.Right)
//                {
//                    if (selectedObjects.Count != 0)
//                    {
//                        if (!Input.GetKey(KeyCode.LeftControl))
//                        {
//                            DeselectAll();
//                        }
//                    }
//                    Select(go);
//                }
//            }
//            else
//            {
//                if (selectedObjects.Count != 0)
//                {
//                    if (!Input.GetKey(KeyCode.LeftControl))
//                    {
//                        DeselectAll();
//                    }
//                    UIManager.Instance.HideView<UIViewRightClick>();
//                }
//            }
//            if (eventData.button == PointerEventData.InputButton.Right)
//            {
//                UIManager.Instance.HideView<UIViewRightClick>();
//                UIManager.Instance.ShowView<UIViewRightClick>();
//                UIManager.Instance.GetView<UIViewRightClick>().rectTransform.position = eventData.position;
//                if (hit.collider != null)
//                {
//                    UIManager.Instance.GetView<UIViewRightClick>().SetPaste(false);
//                }
//                else
//                {
//                    UIManager.Instance.GetView<UIViewRightClick>().SetPaste(true);
//                }
//            }
//            // 只要点击左键就取消右键菜单
//            if (eventData.button == PointerEventData.InputButton.Left)
//            {
//                UIManager.Instance.HideView<UIViewRightClick>();
//            }
//            // 如果按住Control 这只不能粘贴
//            if (Input.GetKey(KeyCode.LeftControl))
//            {
//                UIManager.Instance.GetView<UIViewRightClick>().SetPaste(false);
//            }
//        }
//    }
//    public void OnPointerEnter(PointerEventData eventData)
//    {
//        //Debug.Log("enter");
//        isInBackground = true;
//    }

//    public void OnPointerExit(PointerEventData eventData)
//    {
//        //Debug.Log("exite");
//        isInBackground = false;
//    }
//}
