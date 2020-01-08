//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using UnityEngine;
//using UnityEngine.UI;
//using UniRx;
//using UniRx.Triggers;

//public class TextBoxAsset : SelfAdaptionInputField
//{
//    public List<AssetIcon> Icons;

//    public void SetWidth(List<AssetIcon> icons)
//    {
//        inputField.OnValueChangedAsObservable().Subscribe(x =>
//        {
//            if (string.IsNullOrEmpty(x)) return;
//            foreach (var item in Icons)
//            {
//                if (ushort.TryParse(x, out var result))
//                {
//                    item.Width.Value = result;
//                }
//            }
//        }).AddTo(gameObject);
//        inputField.OnSelectAsObservable().Subscribe(x=> { inputField.placeholder.GetComponent<Text>().text = ""; }).AddTo(gameObject);
//        Icons = icons;
//        lable.text = "宽度";
//        var isShow = Icons.All(icon => icon.Data.Width == Icons[0].Data.Width);
//        if (isShow)
//        {
//            inputField.text = Icons[0].Width.ToString();
//        }
//        else
//        {
//            inputField.text = "";
//            inputField.placeholder.GetComponent<Text>().text = "--";
//        }
//        SetMode();
//    }
//    public void SetHeight(List<AssetIcon> icons)
//    {
//        inputField.OnValueChangedAsObservable().Subscribe(x => {
//            foreach (var item in Icons)
//            {
//                if (ushort.TryParse(x, out var result))
//                {
//                    item.Height.Value = result;
//                }
//            }
//        }).AddTo(gameObject);
//        inputField.OnSelectAsObservable().Subscribe(x => { inputField.placeholder.GetComponent<Text>().text = ""; }).AddTo(gameObject);
//        Icons = icons;
//        lable.text = "高度";
//        var isShow = Icons.All(icon => icon.Data.Height == Icons[0].Data.Height);
//        if (isShow)
//        {
//            inputField.text = Icons[0].Height.ToString();
//        }
//        else
//        {
//            inputField.text = "";
//            inputField.placeholder.GetComponent<Text>().text = "--";
//        }
//        SetMode();
//    }

//    private void SetMode()
//    {
//        foreach (var item in Icons)
//        {
//            switch (item.InteractiveMode.Value)
//            {
//                case InteractiveMode.Single:
//                    break;
//                case InteractiveMode.Multiple:
//                case InteractiveMode.MultipleClosed:
//                    inputField.interactable = false;
//                    break;
//                default:
//                    throw new ArgumentOutOfRangeException();
//            }
//        }
//    }
//}