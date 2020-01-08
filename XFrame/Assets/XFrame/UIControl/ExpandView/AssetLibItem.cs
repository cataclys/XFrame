//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UniRx;
//using Newtonsoft.Json;
//using UnityEngine.UI;
//using UnityEngine.AddressableAssets;

//public class AssetLibItem : ExpandViewItem
//{
//    //public GameObject AssetInfoPrefab;
//    //// 素材信息
//    //private ReactiveCollection<AssetInfoResponse> assetInfos = new ReactiveCollection<AssetInfoResponse>();

//    public void Start()
//    {
       
//    }

//    public void LoadAssets()
//    {
//        HttpManager.Instance.Get<List<Asset>>($"{WebAPI.GetAsset}{Data.Id.Value}", GetAssetSuccess, GetAssetError);
//    }

//    private void GetAssetError()
//    {
        
//    }

//    private void GetAssetSuccess(List<Asset> libs)
//    {
//        foreach (var item in libs)
//        {
//            Addressables.InstantiateAsync("AssetItem", ExpandContentRoot.transform).Completed += obj =>
//            {
//                AssetInfoItem assetInfoItem = obj.Result.GetComponent<AssetInfoItem>();
//                assetInfoItem.SetItemData(item);
//            };
//        }
//    }
//    //private void GenerateItem()
//    //{
//    //    foreach (var item in assetInfos)
//    //    {
//    //        StartCoroutine(Generate(item));
//    //    }
//    //}

//    //private IEnumerator Generate(AssetInfoResponse item)
//    //{
//    //    //Debug.Log(item.Enabled);
//    //    yield return new WaitForSeconds(0.02f);
//    //    // 创建素材库
//    //    GameObject go = Instantiate(AssetInfoPrefab, ExpandContentRoot.transform);
//    //    // 设置素材库属性
//    //    AssetInfoItem itemScript = go.GetComponent<AssetInfoItem>();
//    //    itemScript.SetItemData(item);
//    //    go.SetActive(true);
//    //}
//}
