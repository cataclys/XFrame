using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using XLua;
public class Main : BaseBehaviour
{

    //private void Start()
    //{
        //LuaBehaviour.luaEnv.AddLoader((ref string filename) =>
        //{
        //    string script = "return {ccc = 9999}";
        //    return System.Text.Encoding.UTF8.GetBytes(script);
        //});
        //LuaBehaviour.luaEnv.DoString("require 'main'");

        //ResourceSystem.Instance.verLocal.groups["pc"].listfiles["hotfix"].BeginLoadAssetBundle((a, b) =>
        //{
        //    var test = (TextAsset)a.mainAsset;
        //    //先把DLL以TextAsset类型取出来,在把bytes给Assembly.Load方法读取准备进入反射操作
        //    Assembly aly = Assembly.Load(test.bytes);
        //    gameObject.AddComponent(Type.GetType("Game"));
        //});
    //}
    IEnumerator Start()
    {
        AssetBundle assetBundle = AssetBundle.LoadFromFile($"{Application.streamingAssetsPath}/StandaloneWindows");
        //读取AssetBundleManifest字段数据
        AssetBundleManifest manifest = assetBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        //需要加载的关联bundle
        string[] dependencies = manifest.GetAllDependencies("canvas");
        foreach (string dependency in dependencies)
        {
            AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, dependency));
        }
        //加载bundle
        var uwr = UnityWebRequestAssetBundle.GetAssetBundle($"{Application.streamingAssetsPath}/canvas");
        yield return uwr.SendWebRequest();

        AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(uwr);

        var login = bundle.LoadAssetAsync<GameObject>("canvas");
        yield return login;
        Instantiate(login.asset);

    }
    ////加载场景
    //IEnumerator Start()
    // {

    //     Println("Hello Unity");
    //     //加载平台关联bundle文件
    //     AssetBundle assetBundle = AssetBundle.LoadFromFile($"{Application.streamingAssetsPath}/StandaloneWindows");
    //     //读取AssetBundleManifest字段数据
    //     AssetBundleManifest manifest = assetBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
    //     //需要加载的关联bundle
    //     string[] dependencies = manifest.GetAllDependencies("firsttiledemo");
    //     foreach (string dependency in dependencies)
    //     {
    //         AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, dependency));
    //     }
    //     //加载bundle
    //     var uwr = UnityWebRequestAssetBundle.GetAssetBundle($"{Application.streamingAssetsPath}/firsttiledemo");
    //     yield return uwr.SendWebRequest();

    //     AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(uwr);

    //     if (bundle.isStreamedSceneAssetBundle)
    //     {
    //         SceneManager.LoadScene("FirstTileDemo");
    //     }

    // }
}
