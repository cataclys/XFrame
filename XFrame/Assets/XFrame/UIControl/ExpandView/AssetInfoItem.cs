//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//using UniRx;

//public class AssetInfoItem : MonoBehaviour
//{
//    public Image Image;
//    public Text Name;
//    public Toggle BtnAsset;
//    //
//    public Asset Data;
//    private void Awake()
//    {
//        BtnAsset.group = transform.parent.parent.parent.GetComponent<ToggleGroup>();
//        BtnAsset.OnValueChangedAsObservable().Subscribe(b =>
//        {
//            if (b)
//            {
//                Name.color = Color.white;
//                MessageSystem.Broadcast<Asset>(Msg.SelectedAssetInfoToggle, Data);
//            }
//            else
//            {
//                MessageSystem.Broadcast<Asset>(Msg.SelectedAssetInfoToggle, null);
//                Name.color = Color.black;
//            }
//        }).AddTo(gameObject);
//        MessageSystem.AddListener(Msg.DrawingCancel, SetToggleOff);
//    }
//    private void OnDestroy()
//    {
//        MessageSystem.RemoveListener(Msg.DrawingCancel, SetToggleOff);
//    }
//    private void SetToggleOff()
//    {
//        if (BtnAsset.isOn)
//        {
//            BtnAsset.isOn = false;
//        }
//    }
//    public void SetItemData(Asset item)
//    {
//        Data = item;
//        BtnAsset.interactable = Data.Enabled;
//        Name.text = Data.Name;
//        HttpManager.Instance.GetImage(item.ImageUrl, texture2d =>
//        {
//            Image.sprite = Sprite.Create(texture2d, new Rect(0, 0, 128, 128), new Vector2(0.5f, 0.5f));
//        });
//    }
//}
