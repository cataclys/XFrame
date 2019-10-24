using UnityEngine;
using System;
using System.Collections.Generic;
using Object = UnityEngine.Object;
using UnityEngine.SceneManagement;

/// <summary>
/// �Ѽ���AssetBundle
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
/// ������Դ��������
/// </summary>
public class AssetBundleManager : Singleton<AssetBundleManager>
{
    public string BaseDownloadingURL = "";


    public AssetBundleManifest AssetBundleManifest = null;
    // �Ѿ����ص�AssetBundle
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
    /// ����AssetBundleManifest
    /// </summary>
    /// <returns>manifestAssetBundle����</returns>
    public void Initialize()
    {
        string assetBundleManifestName = GetAssetBundleManifestName();
        AssetBundleManifest = LoadAsset<AssetBundleManifest>(assetBundleManifestName, "AssetBundleManifest");
    }
    /// <summary>
    /// ��������ʱ��ȡAssetBundleManifest����
    /// </summary>
    /// <returns>AssetBundleManifest����</returns>
    public static string GetAssetBundleManifestName()
    {
        switch (Application.platform)
        {
            case RuntimePlatform.Android:
                return "Android";
            case RuntimePlatform.WindowsPlayer:
                return "StandaloneWindows";
            // ��Ӹ���ƽ̨֧��
            // ͬ����ӵ� GetPlatformForAssetBundles(RuntimePlatform) ����.
            default:
                return null;
        }
    }

    #region ����AseetBundle
    /// <summary>
    /// ����AssetBundle
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
    /// ����AssetBundle�����й�����
    /// </summary>
    /// <param name="assetBundleName"></param>
    protected void LoadDependencies(string assetBundleName)
    {
        if (AssetBundleManifest == null)
        {
            return;
        }

        // ��AssetBundleManifest �л�ȡ���й�����
        string[] dependencies = AssetBundleManifest.GetAllDependencies(assetBundleName);
        if (dependencies.Length == 0)
            return;
        // ��¼�����ع�����
        Dependencies.Add(assetBundleName, dependencies);
        for (int i = 0; i < dependencies.Length; i++)
            LoadAssetBundle(dependencies[i]);
    }
    #endregion

    #region ж��AssetBundle
    /// <summary>
    /// ж��assetbundle
    /// </summary>
    public void UnloadAssetBundle(string assetBundleName)
    {
        UnloadAssetBundleInternal(assetBundleName);
        UnloadDependencies(assetBundleName);
    }
    /// <summary>
    /// ж�ع�����
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
    /// ж��AssetBundle
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

    #region ����AssetBundle�е���Դ
    /// <summary>
    /// ͬ������AssetBundle�е���Դ
    /// </summary>
    public T LoadAsset<T>(string assetBundleName, string assetName)
        where T : Object
    {
        LoadedAssetBundle loadedAssetBundle = LoadAssetBundle(assetBundleName);
        var obj = loadedAssetBundle.AssetBundle.LoadAsset<T>(assetName);
        return obj;
    }
    /// <summary>
    /// ͬ������AssetBundle�еĳ���
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
