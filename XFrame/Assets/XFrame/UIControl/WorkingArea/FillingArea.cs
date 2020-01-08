
//using UnityEngine;
//using System.Collections.Generic;
//using UnityEngine.UI;
//using System;
//using UnityEngine.EventSystems;
//using XFrame.UI;

//public class FillingArea : Graphic, ICanvasRaycastFilter, IPointerEnterHandler,IPointerExitHandler
//{
//    //public List<GameObject> lists;
//    private Image ImageSize;
//    private PolygonCollider2D polygonCollider2D;
//    private Transform Background;

//    Vector2 size;
//    Vector2 centerPoint;

//    //float widthScale = 0;
//    float heightScale = 0;

//    protected override void Start()
//    {
//        polygonCollider2D = transform.parent.GetComponent<PolygonCollider2D>();
//        ImageSize = transform.Find("Size").GetComponent<Image>();
//        Background = transform.parent.parent.gameObject.transform;
//    }
//    private void FixedUpdate()
//    {
//        heightScale = Screen.height / 1080f;
//        Vector2 point = new Vector2(size.x / heightScale, size.y / heightScale);
//        ImageSize.rectTransform.sizeDelta = point / Background.localScale.x;
//        ImageSize.rectTransform.anchoredPosition = centerPoint;
//    }
//    void Update()
//    {
//        SetVerticesDirty();
//    }
//    protected override void OnPopulateMesh(VertexHelper vh)
//    {
//        vh.Clear();
//        List<GameObject> lists = transform.parent.GetComponent<MultipleClosedAssetIcon>().linePointObjects;
//        // 需要动态获取多边形下顶点的坐标
//        List<Vector3> points = new List<Vector3>();
//        List<int> indexes = new List<int>();
//        Vector2[] v = new Vector2[lists.Count];
//        for (int i = 0; i < lists.Count; i++)
//        {
//            points.Add(lists[i].GetComponent<RectTransform>().anchoredPosition);
//            v[i] = lists[i].GetComponent<RectTransform>().anchoredPosition;
//            indexes.Add(i);
//        }
//        polygonCollider2D.points = v;
//        //中心点
//        RectTransformUtility.ScreenPointToLocalPointInRectangle(transform as RectTransform, polygonCollider2D.bounds.center, UIManager.GetCanvas().GetComponent<Canvas>().worldCamera, out centerPoint);
//        //// 大小
//        //RectTransformUtility.ScreenPointToLocalPointInRectangle(UIManager.UICanvas.transform as RectTransform, polygonCollider2D.bounds.size, UIManager.UICanvas.worldCamera, out size);
//        //centerPoint = polygonCollider2D.bounds.center;
//        size = polygonCollider2D.bounds.size;

//        #region 绘制多边形
//        // 添加
//        foreach (var point in points)
//        {
//            vh.AddVert(point, color, Vector3.zero);
//        }
//        // 添加
//        int[] tris = Triangulation.WidelyTriangleIndex(points, indexes).ToArray();
//        for (int i = 0; i + 2 < tris.Length; i += 3)
//        {
//            vh.AddTriangle(tris[i], tris[i + 1], tris[i + 2]);
//        }
//        #endregion
//    }

//    //ICanvasRaycastFilter
//    public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
//    {
//        if (raycastTarget && polygonCollider2D != null)
//        {
//            if (polygonCollider2D.OverlapPoint(sp))
//                return true;
//        }
//        return false;
//    }

//    public void OnPointerEnter(PointerEventData eventData)
//    {
//        WorkingArea.Instance.ChangeShadowIconColor(Color.red);
//    }

//    public void OnPointerExit(PointerEventData eventData)
//    {
//        WorkingArea.Instance.ChangeShadowIconColor(new Color32(255, 255, 255, 128));
//    }
//}





