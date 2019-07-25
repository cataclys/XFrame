using System.Collections;
using System.Collections.Generic;
using System.IO;
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
        AssetBundle assetBundle = AssetBundle.LoadFromFile($"{Application.streamingAssetsPath}/StandaloneWindows");
        //读取AssetBundleManifest字段数据
        AssetBundleManifest manifest = assetBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        //需要加载的关联bundle
        string[] dependencies = manifest.GetAllDependencies(assetName);
        foreach (string dependency in dependencies)
        {
            AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, dependency));
        }
        //加载bundle
        var uwr = UnityWebRequestAssetBundle.GetAssetBundle(bundleUri);
        yield return uwr.SendWebRequest();

        AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(uwr);

        var login = bundle.LoadAssetAsync<GameObject>(assetName);
        yield return login;
        Instantiate(login.asset);
    }
}
public class AssetInfo
{
    public string Name { get; set; }
    public Object Asset { get; set; }

}
