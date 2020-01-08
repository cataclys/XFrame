//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//using UnityEngine.EventSystems;
//using UnityEngine.Events;
//using XFrame.UI;

//public class SelectionSystem : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
//{


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
//            //Messenger<List<GameObject>>.Invoke(MessengerType.OnSelectOver, selectedObjects);
//        });
//    }

//    void Update()
//    {
//        if (Input.GetKeyDown(KeyCode.Delete) && selectedObjects.Count > 0)
//        {
//            foreach (var item in selectedObjects)
//            {
//                //UIManager.Instance.GetView<UIViewRight>().DeleteItemView(item.GetInstanceID());
//                Destroy(item);
//            }
//            selectedObjects.Clear();
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


 
  
//    public void OnPointerClick(PointerEventData eventData)
//    {
//        // 
//        if (!eventData.dragging && WorkingArea.DrawState== DrawState.DrawingSingleIcon)
//        {
//            // 没有拖动，通过射线判断点中图标
//            RaycastHit2D hit = Physics2D.Raycast(new Vector2(eventData.position.x, eventData.position.y), Vector2.zero);
//            if (hit.collider != null)
//            {
//                GameObject go = hit.collider.gameObject;
//                if (eventData.button == PointerEventData.InputButton.Left)
//                {
//                    // 选择对象中没有这个对象
//                    if (!selectedObjects.Contains(go) && selectedObjects.Count != 0 && !Input.GetKey(KeyCode.LeftControl))
//                    {
//                        //没有按住左边Control键，取消所有选择
//                        DeselectAll();
//                    }
//                    SelectOrDeselectDepends(go);
//                }
//                //if (eventData.button == PointerEventData.InputButton.Right)
//                //{
//                //    if (selectedObjects.Count != 0)
//                //    {
//                //        if (!Input.GetKey(KeyCode.LeftControl))
//                //        {
//                //            DeselectAll();
//                //        }
//                //    }
//                //    Select(go);
//                //}
//            }
//            //else
//            //{
//            //    if (selectedObjects.Count != 0)
//            //    {
//            //        if (!Input.GetKey(KeyCode.LeftControl))
//            //        {
//            //            DeselectAll();
//            //        }
//            //        UIManager.HideView<UIViewRightClick>();
//            //    }
//            //}
//            //if (eventData.button == PointerEventData.InputButton.Right)
//            //{
//            //    UIManager.HideView<UIViewRightClick>();
//            //    UIManager.ShowView<UIViewRightClick>();
//            //    UIManager.GetView<UIViewRightClick>().rectTransform.position = eventData.position;
//            //    if (hit.collider != null)
//            //    {
//            //        UIManager.GetView<UIViewRightClick>().SetPaste(false);
//            //    }
//            //    else
//            //    {
//            //        UIManager.GetView<UIViewRightClick>().SetPaste(true);
//            //    }
//            //}
//            //// 只要点击左键就取消右键菜单
//            //if (eventData.button == PointerEventData.InputButton.Left)
//            //{
//            //    UIManager.HideView<UIViewRightClick>();
//            //}
//            //// 如果按住Control 这只不能粘贴
//            //if (Input.GetKey(KeyCode.LeftControl))
//            //{
//            //    UIManager.GetView<UIViewRightClick>().SetPaste(false);
//            //}
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
