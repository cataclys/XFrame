
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using UniRx;
//using UnityEngine;
//using UnityEngine.AddressableAssets;
//using UnityEngine.UI;

//public class AssetPropertiesView : MonoBehaviour
//{
//    // Item父对象
//    public Transform ItemParent;
//    // 动态创建的控件
//    public List<DynamicControl> Controls = new List<DynamicControl>();
//    // 当前已选择的图标
//    private List<AssetIcon> _iconsGameObjects = new List<AssetIcon>();


//    public void Refresh()
//    {
//        ClearControl();
//        GenerateControl();
//    }
//    // 生成控件
//    public void GenerateControl()
//    {
//        if (_iconsGameObjects.Count > 0)
//        {
//            #region 必须显示的属性
//            //宽度
//            Addressables.InstantiateAsync("TextBoxAsset", ItemParent).Completed += x =>
//            {
//                var width = x.Result.GetComponent<TextBoxAsset>();
//                width.SetWidth(_iconsGameObjects);
//                Controls.Add(width);
//            };
//            //高度
//            Addressables.InstantiateAsync("TextBoxAsset", ItemParent).Completed += y =>
//            {
//                var height = y.Result.GetComponent<TextBoxAsset>();
//                height.SetHeight(_iconsGameObjects);
//                Controls.Add(height);

//            };
//            //角度
//            Addressables.InstantiateAsync("AngleInputField", ItemParent).Completed += z =>
//            {
//                var angle = z.Result.GetComponent<AngleInputField>();
//                angle.SetData(_iconsGameObjects);
//                Controls.Add(angle);

//            };
//            #endregion
//            #region 筛选显示的属性
//            var allColor = _iconsGameObjects.All(item => item.Data.FillMode == FillMode.Color);
//            if (allColor)
//            {
//                Addressables.InstantiateAsync("ColorBoard", ItemParent).Completed += c =>
//                {
//                    var color = c.Result.GetComponent<ColorBoard>();
//                    if (_iconsGameObjects.Count == 1)
//                    {
//                        color.ShowColor.color = _iconsGameObjects[0].ColorProperty.Value;
//                    }
//                    color.onColorChanged.AddListener(cl =>
//                    {
//                        foreach (var item in _iconsGameObjects)
//                        {
//                            item.ColorProperty.Value = cl;
//                        }
//                    });
//                    Controls.Add(color);
//                };
//            }
//            #endregion
//            //PropertyInfo
//            var data = _iconsGameObjects[0].Data.PropertyInfos;
//            foreach (var item in data)
//            {
//                //是否所有item中含有相同属性
//                var isAllItemHas = _iconsGameObjects.All(temp => temp.Data.PropertyInfos.Any(item2 => item2.PropertyName == item.PropertyName) == true);
//                if (!isAllItemHas) return;

//                switch (item.PropertyType)
//                {
//                    case PropertyType.SingleText:
//                    case PropertyType.MultipleText:
//                        Addressables.InstantiateAsync("SelfAdaptionInputField", ItemParent).Completed += z =>
//                        {
//                            var field = z.Result.GetComponent<SelfAdaptionInputField>();
//                            field.name = item.PropertyName;
//                            field.lable.text = item.PropertyName;
//                            field.inputField.placeholder.GetComponent<Text>().text = "一";
//                            _iconsGameObjects.ForEach(assetIcon =>
//                            {
//                                // 找到所有相同的属性值
//                                var propertyInfos = assetIcon.Data.PropertyInfos.FindAll(property => property.PropertyName == field.name);
//                                var showValue = propertyInfos.All(property => property.PropertyValue == item.PropertyValue);
//                                field.inputField.text = showValue ? item.PropertyValue : "";
//                            });
//                            field.inputField.OnValueChangedAsObservable().Subscribe(x =>
//                            {
//                                if (string.IsNullOrEmpty(x)) return;
//                                _iconsGameObjects.ForEach(item4 =>
//                                {
//                                    // 找到所有相同的属性值
//                                    var propertyInfos = item4.Data.PropertyInfos.FindAll(item5 => item5.PropertyName == field.name);
//                                    propertyInfos.ForEach(item5 => { item5.PropertyValue = x; });
//                                });
//                            }).AddTo(gameObject);
//                            Controls.Add(field);
//                        };
//                        break;
//                    case PropertyType.Numeric:
//                        Addressables.InstantiateAsync("SelfAdaptionInputField", ItemParent).Completed += z =>
//                        {
//                            var field = z.Result.GetComponent<SelfAdaptionInputField>();
//                            field.inputField.contentType = InputField.ContentType.IntegerNumber;
//                            field.name = item.PropertyName;
//                            field.lable.text = item.PropertyName;
//                            field.inputField.placeholder.GetComponent<Text>().text = "一";
//                            _iconsGameObjects.ForEach(assetIcon =>
//                            {
//                                // 找到所有相同的属性值
//                                var propertyInfos = assetIcon.Data.PropertyInfos.FindAll(property => property.PropertyName == field.name);
//                                var showValue = propertyInfos.All(property => property.PropertyValue == item.PropertyValue);
//                                field.inputField.text = showValue ? item.PropertyValue : "";
//                            });
//                            field.inputField.OnValueChangedAsObservable().Subscribe(x =>
//                            {
//                                if (string.IsNullOrEmpty(x)) return;
//                                _iconsGameObjects.ForEach(item4 =>
//                                {
//                                    // 找到所有相同的属性值
//                                    var propertyInfos = item4.Data.PropertyInfos.FindAll(item5 => item5.PropertyName == field.name);
//                                    propertyInfos.ForEach(item5 =>
//                                    {
//                                        item5.PropertyValue = x;
//                                    });
//                                });
//                            }).AddTo(gameObject);
//                            Controls.Add(field);
//                        };
//                        break;
//                    case PropertyType.Image:
//                        if (_iconsGameObjects.Count > 1) return;
//                        break;
//                    case PropertyType.ImageNumeric:
//                        if (_iconsGameObjects.Count > 1) return;
//                            Addressables.InstantiateAsync("ImageBrowser", ItemParent).Completed += z =>
//                        {
//                            var image = z.Result.GetComponent<ImageBrowser>();
//                            image.ImageCount = int.Parse(item.PropertyValue);
//                            image.SetData(data);
//                            Controls.Add(image);
//                        };
//                        break;
//                    default:
//                        throw new ArgumentOutOfRangeException();
//                }
//            }
//        }
//    }

//    /// <summary>
//    /// 清空控件
//    /// </summary>
//    public void ClearControl()
//    {
//        foreach (var item in Controls)
//        {
//            Addressables.ReleaseInstance(item.gameObject);
//        }
//        Controls.Clear();
//    }

//    private void OnEnable()
//    {
//        MessageSystem.AddListener<List<GameObject>>(Msg.OnSelectOver, OnSelectOver);
//    }
//    private void OnDisable()
//    {
//        MessageSystem.RemoveListener<List<GameObject>>(Msg.OnSelectOver, OnSelectOver);
//    }

//    private void OnSelectOver(List<GameObject> list)
//    {
//        _iconsGameObjects.Clear();
//        foreach (var item in list.Select(i => i.GetComponent<AssetIcon>()))
//        {
//            _iconsGameObjects.Add(item);
//        }
//        Refresh();
//    }
//}





