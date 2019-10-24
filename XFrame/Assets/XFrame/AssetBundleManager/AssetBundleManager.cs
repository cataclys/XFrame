using UnityEngine;
using System;
using System.Collections.Generic;
using Object = UnityEngine.Object;
using UnityEngine.SceneManagement;

/// <summary>
/// 已加载AssetBundle
/// </summary>
public class LoadedAssetBundle
{
    public AssetBundle AssetBundle;
    public int ReferencedCount;

    internal event Action unload;

    internal void OnUnload()
    {
        AssetBundle.Unload(false);
        if (unload != null)
            unload();
    }

    public LoadedAssetBundle(AssetBundle assetBundle)
    {
        AssetBundle = assetBundle;
        ReferencedCount = 1;
    }
}

/// <summary>
/// 单例资源包管理者
/// </summary>
public class AssetBundleManager : Singleton<AssetBundleManager>
{
    public string BaseDownloadingURL = "";


    public AssetBundleManifest AssetBundleManifest = null;
    // 已经加载的AssetBundle
    public Dictionary<string, LoadedAssetBundle> LoadedAssetBundles = new Dictionary<string, LoadedAssetBundle>();
    public Dictionary<string, string[]> Dependencies = new Dictionary<string, string[]>();


    public LoadedAssetBundle GetLoadedAssetBundle(string assetBundleName)
    {

        LoadedAssetBundle bundle = null;
        LoadedAssetBundles.TryGetValue(assetBundleName, out bundle);
        if (bundle == null)
            return null;

        // No dependencies are recorded, only the bundle itself is required.
        string[] dependencies = null;
        if (!Dependencies.TryGetValue(assetBundleName, out dependencies))
            return bundle;

        // Make sure all dependencies are loaded
        foreach (var dependency in dependencies)
        {

            // Wait all the dependent assetBundles being loaded.
            LoadedAssetBundle dependentBundle;
            LoadedAssetBundles.TryGetValue(dependency, out dependentBundle);
            if (dependentBundle == null)
                return null;
        }

        return bundle;
    }

    public bool IsAssetBundleDownloaded(string assetBundleName)
    {
        return LoadedAssetBundles.ContainsKey(assetBundleName);
    }

    /// <summary>
    /// 加载AssetBundleManifest
    /// </summary>
    /// <returns>manifestAssetBundle名字</returns>
    public void Initialize()
    {
        string assetBundleManifestName = GetAssetBundleManifestName();
        AssetBundleManifest = LoadAsset<AssetBundleManifest>(assetBundleManifestName, "AssetBundleManifest");
    }
    /// <summary>
    /// 根据运行时获取AssetBundleManifest名称
    /// </summary>
    /// <returns>AssetBundleManifest名称</returns>
    public static string GetAssetBundleManifestName()
    {
        switch (Application.platform)
        {
            case RuntimePlatform.Android:
                return "Android";
            case RuntimePlatform.WindowsPlayer:
                return "StandaloneWindows";
            // 添加更多平台支持
            // 同样添加到 GetPlatformForAssetBundles(RuntimePlatform) 方法.
            default:
                return null;
        }
    }

    #region 加载AseetBundle
    /// <summary>
    /// 加载AssetBundle
    /// </summary>
    /// <param name="assetBundleName"></param>
    /// <param name="isLoadingAssetBundleManifest"></param>
    protected LoadedAssetBundle LoadAssetBundle(string assetBundleName)
    {
        LoadedAssetBundle loadedAssetBundle = null;
        LoadedAssetBundles.TryGetValue(assetBundleName, out loadedAssetBundle);
        if (loadedAssetBundle != null)
        {
            loadedAssetBundle.ReferencedCount++;
            return loadedAssetBundle;
        }
        else
        {
            string path = BaseDownloadingURL + assetBundleName;
       
            AssetBundle assetBundle = AssetBundle.LoadFromFile($"{path}");
            if (assetBundle!=null)
            {
                loadedAssetBundle = new LoadedAssetBundle(assetBundle);

                LoadedAssetBundles.Add(assetBundleName, loadedAssetBundle);

                LoadDependencies(assetBundleName);
                return loadedAssetBundle;
            }
            else
            {
                return null;
            }

           
        }
    }
    /// <summary>
    /// 加载AssetBundle的所有关联项
    /// </summary>
    /// <param name="assetBundleName"></param>
    protected void LoadDependencies(string assetBundleName)
    {
        if (AssetBundleManifest == null)
        {
            return;
        }

        // 从AssetBundleManifest 中获取所有关联项
        string[] dependencies = AssetBundleManifest.GetAllDependencies(assetBundleName);
        if (dependencies.Length == 0)
            return;
        // 记录并加载关联项
        Dependencies.Add(assetBundleName, dependencies);
        for (int i = 0; i < dependencies.Length; i++)
            LoadAssetBundle(dependencies[i]);
    }
    #endregion

    #region 卸载AssetBundle
    /// <summary>
    /// 卸载assetbundle
    /// </summary>
    public void UnloadAssetBundle(string assetBundleName)
    {
        UnloadAssetBundleInternal(assetBundleName);
        UnloadDependencies(assetBundleName);
    }
    /// <summary>
    /// 卸载关联项
    /// </summary>
    /// <param name="assetBundleName"></param>
    void UnloadDependencies(string assetBundleName)
    {
        string[] dependencies = null;
        if (!Dependencies.TryGetValue(assetBundleName, out dependencies))
            return;

        // Loop dependencies.
        foreach (var dependency in dependencies)
        {
            UnloadAssetBundleInternal(dependency);
        }
        Dependencies.Remove(assetBundleName);
    }
    /// <summary>
    /// 卸载AssetBundle
    /// </summary>
    /// <param name="assetBundleName"></param>
    void UnloadAssetBundleInternal(string assetBundleName)
    {
        LoadedAssetBundle bundle = GetLoadedAssetBundle(assetBundleName);
        if (bundle == null)
            return;

        if (--bundle.ReferencedCount == 0)
        {
            bundle.OnUnload();
            LoadedAssetBundles.Remove(assetBundleName);
        }
    }
    #endregion

    #region 加载AssetBundle中的资源
    /// <summary>
    /// 同步加载AssetBundle中的资源
    /// </summary>
    public T LoadAsset<T>(string assetBundleName, string assetName)
        where T : Object
    {
        LoadedAssetBundle loadedAssetBundle = LoadAssetBundle(assetBundleName);
        var obj = loadedAssetBundle.AssetBundle.LoadAsset<T>(assetName);
        return obj;
    }
    /// <summary>
    /// 同步加载AssetBundle中的场景
    /// </summary>
    public void LoadLevel(string assetBundleName, string levelName, bool isAdditive)
    {
        LoadedAssetBundle loadedAssetBundle = LoadAssetBundle(assetBundleName);
        if (loadedAssetBundle.AssetBundle.isStreamedSceneAssetBundle)
        {
            if (isAdditive)
            {
                SceneManager.LoadScene(levelName,LoadSceneMode.Additive);
            }
            else
            {
                SceneManager.LoadScene(levelName);
            }
        }
    }

    public void LoadAssetAsync(string assetBundleName, string assetName)
    {

    }

    public void LoadLevelAsync(string assetBundleName, string levelName, bool isAdditive)
    {
 
    }
    #endregion
}
