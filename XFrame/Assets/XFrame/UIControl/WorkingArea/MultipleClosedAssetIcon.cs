
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UniRx;
//using UnityEngine;
//using UnityEngine.UI;
//using UniRx;

//public class MultipleClosedAssetIcon : AssetIcon
//{
//    public List<GameObject> linePointObjects;
//    public List<GameObject> Lines;
//    public FillingArea FillingArea;
//    public Text TextName;
//    public PolygonCollider2D PolygonCollider2D;
//    public Transform LineList;
//    public Transform PointList;
//    public override void Awake()
//    {
//        base.Awake();
//        Name.Subscribe(ChangeName).AddTo((gameObject));
//        ColorProperty.Subscribe(ChangeColor).AddTo(gameObject);
//    }

//    public void ChangeName(string showName)
//    {
//        TextName.text = showName;
//    }
//    public void ChangeColor(Color color)
//    {
//        FillingArea.color = color;
//        Data.Color = $"#{ColorUtility.ToHtmlStringRGBA(ColorProperty.Value)}";
//    }
//    void Start()
//    {
//        PolygonCollider2D = GetComponent<PolygonCollider2D>();
//    }

//    public override AssetData GetData()
//    {
//        Data = base.GetData();
//        for (var i = 0; i < linePointObjects.Count; i++)
//        {
//            Data.MultiPoint[i] = ((RectTransform) linePointObjects[i].transform).anchoredPosition;
//        }
//        return Data;
//    }

//    public override void ShowOutline(bool b)
//    {
//        //base.ShowOutline(b);
//        PointList.gameObject.SetActive(b);
//        LineList.gameObject.SetActive(b);
//    }
//    public void Draw()
//    {
//        PointList.gameObject.SetActive(false);
//        LineList.gameObject.SetActive(false);
//        FillingArea.enabled = true;
//        IsFinished = true;

//        //绘制多边形结束
//        //WorkingArea.DrawState = DrawState.DrawingOver;
//        //颜色
//        //if (assetIconData.AssetInfo != null)
//        //{
//        //    foreach (var item in assetIconData.AssetInfo.AssetsProperties)
//        //    {
//        //        if (item.PropertyName == "颜色")
//        //        {
//        //            Color newColor;
//        //            ColorUtility.TryParseHtmlString(item.PropertyValue, out newColor);
//        //            //Debug.Log(newColor);
//        //            if (assetIconData.AssetInfo.Enabled)
//        //            {
//        //                FillingArea.color = newColor;
//        //            }
//        //            else
//        //            {
//        //                FillingArea.color = new Color(0.2f,0.2f,0.2f,0.2f);
//        //            }
//        //        }
//        //    }
//        //}
//    }

//    public void SetPointLine()
//    {
//        for (var i = 0; i < Lines.Count; i++)
//        {
//            var line = Lines[i].GetComponent<Line>();
//            line.PointA = linePointObjects[i];
//            line.PointB = i + 1 < Lines.Count ? linePointObjects[i + 1] : linePointObjects[0];
//        }
//    }
//}
