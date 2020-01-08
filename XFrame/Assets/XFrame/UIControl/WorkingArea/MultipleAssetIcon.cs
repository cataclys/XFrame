
//using System.Collections;
//using System.Collections.Generic;
//using UniRx;
//using UnityEngine;
//using UnityEngine.EventSystems;
//using UnityEngine.UI;

//public class MultipleAssetIcon : AssetIcon
//{

//    public Transform LineList;
//    public Transform PointList;
//    public List<GameObject> LinePointObjects;
//    public List<GameObject> Lines;
//    public override AssetData GetData()
//    {
//        Data = base.GetData();
//        for (var i = 0; i < LinePointObjects.Count; i++)
//        {
//            Data.MultiPoint[i] = ((RectTransform) LinePointObjects[i].transform).anchoredPosition;
//        }
//        return Data;
//    }
//    public override void ShowOutline(bool b)
//    {
//        base.ShowOutline(b);
//        PointList.gameObject.SetActive(b);
//    }
//    public void Draw()
//    {
//        PointList.gameObject.SetActive(false);
//        IsFinished = true;
//    }
//    public void SetPointLine()
//    {
//        for (var i = 0; i < Lines.Count; i++)
//        {
//            var line = Lines[i].GetComponent<Arrows>();
//            line.PointA = LinePointObjects[i];
//            line.PointB = LinePointObjects[i + 1];
//        }
//    }
//}
