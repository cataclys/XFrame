using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class AssetManager : Singleton<AssetManager>
{
    // 资源引用
    public Dictionary<string, Object> Assets = new Dictionary<string, Object>();

    public void LoadAssetBunlde<T>(string uri, string assetName, UnityAction<Object> unityAction)
    {
        StartCoroutine(Load<T>(uri, assetName, unityAction));
    }
    IEnumerator Load<T>(string bundleUri,string assetName,UnityAction<Object> unityAction)
        
    {
        var uwr = UnityWebRequestAssetBundle.GetAssetBundle(bundleUri);
        yield return uwr.SendWebRequest();

        AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(uwr);
        var loadAsset = bundle.LoadAssetAsync<T>(assetName);
        yield return loadAsset;

        if (unityAction!=null)
        {
            unityAction(loadAsset.asset);
        }

        //bundle.Unload(false);
    }
}
public class AssetInfo
{
    public string Name { get; set; }
    public Object Asset { get; set; }

}
