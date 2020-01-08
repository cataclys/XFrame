//using System.Collections;
//using System.Collections.Generic;
//using UniRx;
//using UnityEngine;
//using UnityEngine.EventSystems;
//using UnityEngine.UI;
//using UniRx;
//using System;

//public class AssetIcon : MonoBehaviour
//{
//    //起始点
//    public GameObject StartPoint;
//    //结束点
//    public GameObject EndPoint;
//    //是否完成
//    public bool IsFinished = false;
//    //描边
//    private Outline[] _outlines;
//    //图标数据
//    public AssetData Data = new AssetData();

//    #region 数据驱动
//    public ReactiveProperty<Vector2> Position = new ReactiveProperty<Vector2>();
//    //宽度
//    public ReactiveProperty<ushort> Width = new ReactiveProperty<ushort>();
//    //高度
//    public ReactiveProperty<ushort> Height = new ReactiveProperty<ushort>();
//    //角度
//    public FloatReactiveProperty Angle = new FloatReactiveProperty();
//    //类型
//    public ReactiveProperty<InteractiveMode> InteractiveMode = new ReactiveProperty<InteractiveMode>();
//    //名称
//    public ReactiveProperty<string> Name = new ReactiveProperty<string>();
//    //图片地址
//    public ReactiveProperty<string> ImageUrl = new ReactiveProperty<string>();
//    //颜色
//    public ReactiveProperty<Color> ColorProperty = new ReactiveProperty<Color>();
//    #endregion


//    public virtual void Awake()
//    {
//        Position.Subscribe(ChangePosition).AddTo(gameObject);
//        Width.Subscribe(ChangeWidth).AddTo(gameObject);
//        Height.Subscribe(ChangeHeight).AddTo(gameObject);
//        Angle.Subscribe(ChangeAngle).AddTo(gameObject);
//    }

//    private void ChangePosition(Vector2 position)
//    {
//        ((RectTransform)transform).anchoredPosition = position;
//    }

//    private void ChangeAngle(float angle)
//    {
//        transform.rotation = Quaternion.Euler(0, 0, angle);
//        Data.Angle = angle;
//    }

//    private void ChangeHeight(ushort height)
//    {

//        var x = ((RectTransform)transform).sizeDelta.x;
//        var y = height;
//        ((RectTransform)transform).sizeDelta = new Vector2(x, y);
//        Data.Height = height;
//    }

//    private void ChangeWidth(ushort width)
//    {
//        var x = width;
//        var y = ((RectTransform)transform).sizeDelta.y;
//        ((RectTransform)transform).sizeDelta = new Vector2(x, y);
//        Data.Width = width;
//    }

//    public virtual void ShowOutline(bool b)
//    {
//        _outlines = transform.GetComponents<Outline>();
//        if (_outlines.Length == 0)
//        {
//            _outlines = transform.GetComponentsInChildren<Outline>();
//        }
//        // 描边
//        foreach (var item in _outlines)
//        {
//            item.enabled = b;
//        }
//    }
//    public virtual AssetData GetData()
//    {
//        Data.Point = ((RectTransform)transform).anchoredPosition;
//        Data.Width = Width.Value;
//        Data.Height = Height.Value;
//        Data.Angle = Angle.Value;
//        Data.Color = $"#{ColorUtility.ToHtmlStringRGBA(ColorProperty.Value)}";
//        Data.Name = Name.Value;
//        return Data;
//    }

//    public virtual void SetData(AssetData data)
//    {
//        Data = data;
//        Position.Value = data.Point;
//        Width.Value = data.Width;
//        Height.Value = data.Height;
//        Angle.Value = data.Angle;
//        Name.Value = data.Name;
//        ImageUrl.Value = data.ImageUrl;
//        ColorUtility.TryParseHtmlString(data.Color, out var newColor);
//        ColorProperty.Value = newColor;
//        InteractiveMode.Value = data.InteractiveMode;
//    }
//}
