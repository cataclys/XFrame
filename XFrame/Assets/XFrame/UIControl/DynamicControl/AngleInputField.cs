//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//using UniRx;

//public class AngleInputField : DynamicControl
//{
//    public InputField AngleInput;
//    public Text Name;
//    public Slider AngleSlider;


//    public List<AssetIcon> Icons;


//    public void SetData(List<AssetIcon> assetData)
//    {
//        AngleSlider.OnValueChangedAsObservable().Subscribe(x =>
//        {
//            foreach (var item in Icons)
//            {
//                item.Angle.Value = x;
//                AngleInput.text = item.Angle.ToString();
//            }
//        }).AddTo(gameObject);
//        AngleInput.OnValueChangedAsObservable().Subscribe(x =>
//        {
//            foreach (var item in Icons)
//            {
//                AngleSlider.value = float.Parse(x);
//            }
//        }).AddTo(gameObject);



//        Icons = assetData;
//        if (Icons.Count == 1)
//        {
//            AngleSlider.value = Icons[0].Angle.Value;
//            AngleInput.text = AngleSlider.value.ToString();
//        }
//        else
//        {
//            AngleSlider.value = 0;
//            AngleInput.text = "0";
//        }
//        SetMode();
//    }
//    public override void SetEnabled(bool enabled)
//    {
//        AngleInput.interactable = enabled;
//        AngleSlider.interactable = enabled;
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
//                    AngleInput.interactable = false;
//                    AngleSlider.interactable = false;
//                    break;
//                default:
//                    throw new ArgumentOutOfRangeException();
//            }
//        }
//    }
//}
