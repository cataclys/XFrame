#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;

// 已加载的assetBundle引用计数
public class LoadedAssetBundle
{
    public AssetBundle m_AssetBundle;
    public int m_ReferencedCount;

    public LoadedAssetBundle(AssetBundle assetBundle)
    {
        m_AssetBundle = assetBundle;
        m_ReferencedCount = 1;
    }
}

// AB管理类
public class AssetBundleManager : SingletonMonoBehaviour<AssetBundleManager>
{
    static string m_BaseDownloadingURL = "";
    static string[] m_Variants = { };
    static AssetBundleManifest m_AssetBundleManifest = null;
#if UNITY_EDITOR
    static int m_SimulateAssetBundleInEditor = -1;
    const string kSimulateAssetBundles = "SimulateAssetBundles";
#endif

    static Dictionary<string, LoadedAssetBundle> m_LoadedAssetBundles = new Dictionary<string, LoadedAssetBundle>();
    static Dictionary<string, WWW> m_DownloadingWWWs = new Dictionary<string, WWW>();
    static Dictionary<string, string> m_DownloadingErrors = new Dictionary<string, string>();
    static List<AssetBundleLoadOperation> m_InProgressOperations = new List<AssetBundleLoadOperation>();
    static Dictionary<string, string[]> m_Dependencies = new Dictionary<string, string[]>();

    // 下载路径
    public static string BaseDownloadingURL
    {
        get { return m_BaseDownloadingURL; }
        set { m_BaseDownloadingURL = value; }
    }

    // 
    public static string[] Variants
    {
        get { return m_Variants; }
        set { m_Variants = value; }
    }

    // AssetBundleManifest
    public static AssetBundleManifest AssetBundleManifestObject
    {
        set { m_AssetBundleManifest = value; }
    }
    const string AssetBundlesPath = "/AssetBundles/";


    //初始化
    protected IEnumerator Initialize()
    {
#if UNITY_EDITOR
        //Debug.Log("当前模式：" + (SimulateAssetBundleInEditor ? "模拟模式" : "正常模式"));
#endif

        string platformFolderForAssetBundles =
#if UNITY_EDITOR
            GetPlatformFolderForAssetBundles(EditorUserBuildSettings.activeBuildTarget);
#else
			GetPlatformFolderForAssetBundles(Application.platform);
#endif

        //下载路径
        string relativePath = GetRelativePath();
        BaseDownloadingURL = relativePath + AssetBundlesPath + platformFolderForAssetBundles + "/";
        var request = Initialize(platformFolderForAssetBundles);
        if (request != null)
        {
            yield return StartCoroutine(request);
        }
        Initializing = false;
    }
    // 真实路径
    public string GetRelativePath()
    {
        if (Application.isEditor)
            //return "file://" + System.Environment.CurrentDirectory.Replace("\\", "/");
            return "file://" + Application.streamingAssetsPath;
        //else if (Application.isWebPlayer)
        //    return Path.GetDirectoryName(Application.absoluteURL).Replace("\\", "/") + "/StreamingAssets";
        else if (Application.isMobilePlatform || Application.isConsolePlatform)
            return Application.streamingAssetsPath;
        else
            return "file://" + Application.streamingAssetsPath;
    }

#if UNITY_EDITOR
    public static string GetPlatformFolderForAssetBundles(BuildTarget target)
    {
        switch (target)
        {
            case BuildTarget.Android:
                return "Android";
            case BuildTarget.iOS:
                return "iOS";
            case BuildTarget.WebGL:
                return "WebGL";
            case BuildTarget.StandaloneWindows:
            case BuildTarget.StandaloneWindows64:
                return "Windows";
            case BuildTarget.StandaloneOSXIntel:
            case BuildTarget.StandaloneOSXIntel64:
            case BuildTarget.StandaloneOSX:
                return "OSX";
            default:
                return null;
        }
    }
#endif

    static string GetPlatformFolderForAssetBundles(RuntimePlatform platform)
    {
        switch (platform)
        {
            case RuntimePlatform.Android:
                return "Android";
            case RuntimePlatform.IPhonePlayer:
                return "iOS";
            case RuntimePlatform.WebGLPlayer:
                return "WebGL";
            case RuntimePlatform.WindowsPlayer:
                return "Windows";
            case RuntimePlatform.OSXPlayer:
                return "OSX";
            default:
                return null;
        }
    }
    //IEnumerator Start()
    //{
    //    yield return StartCoroutine(Initialize());
    //}
    public bool Initializing = false;
    public IEnumerator Load<T>(string assetBundleName, string assetName, Action<T> callback)
        where T : UnityEngine.Object
    {
        while (Initializing)
        {
            yield return new WaitForEndOfFrame();
        }
        if (m_AssetBundleManifest == null)
        {
            //Debug.Log("未初始化！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！");
            Initializing = true;
            yield return StartCoroutine(Initialize());
        }
        //Debug.Log(string.Format("在第{0}帧开始加载{1}", Time.frameCount, assetName));

        // 从AB中加载资源
        AssetBundleLoadAssetOperation request = LoadAssetAsync(assetBundleName, assetName, typeof(T));
        if (request == null)
            yield break;
        yield return StartCoroutine(request);

        // 获取资源T
        T asset = request.GetAsset<T>();
        //Debug.Log(string.Format("加载{0}在第{1}帧时{2}", assetName, Time.frameCount, (asset == null ? "失败" : "成功")));
        if (asset != null)
        {
            callback(asset);
            //添加到资源缓存池
            //ResourceManager.AddAsset(asset.name, asset);
            UnloadAssetBundle(assetBundleName);
        }
    }
    //protected IEnumerator LoadLevel(string assetBundleName, string levelName, bool isAdditive)
    //{
    //    Debug.Log("Start to load scene " + levelName + " at frame " + Time.frameCount);

    //    // Load level from assetBundle.
    //    AssetBundleLoadOperation request = AssetBundleManager.LoadLevelAsync(assetBundleName, levelName, isAdditive);
    //    if (request == null)
    //        yield break;
    //    yield return StartCoroutine(request);

    //    // This log will only be output when loading level additively.
    //    Debug.Log("Finish loading scene " + levelName + " at frame " + Time.frameCount);
    //}
#if UNITY_EDITOR
    // 是否要在编辑器中模拟assetbundle，而不实际构建它们
    public static bool SimulateAssetBundleInEditor
    {
        get
        {
            if (m_SimulateAssetBundleInEditor == -1)
                m_SimulateAssetBundleInEditor = EditorPrefs.GetBool(kSimulateAssetBundles, true) ? 1 : 0;

            return m_SimulateAssetBundleInEditor != 0;
        }
        set
        {
            int newValue = value ? 1 : 0;
            if (newValue != m_SimulateAssetBundleInEditor)
            {
                m_SimulateAssetBundleInEditor = newValue;
                EditorPrefs.SetBool(kSimulateAssetBundles, value);
            }
        }
    }
#endif

    // 获取已加载的AssetBundle，只在成功下载所有依赖项时返回vaild对象。
    static public LoadedAssetBundle GetLoadedAssetBundle(string assetBundleName, out string error)
    {
        if (m_DownloadingErrors.TryGetValue(assetBundleName, out error))
            return null;

        LoadedAssetBundle bundle = null;
        m_LoadedAssetBundles.TryGetValue(assetBundleName, out bundle);
        if (bundle == null)
            return null;

        // No dependencies are recorded, only the bundle itself is required.
        string[] dependencies = null;
        if (!m_Dependencies.TryGetValue(assetBundleName, out dependencies))
            return bundle;

        // Make sure all dependencies are loaded
        foreach (var dependency in dependencies)
        {
            if (m_DownloadingErrors.TryGetValue(assetBundleName, out error))
                return bundle;

            // Wait all the dependent assetBundles being loaded.
            LoadedAssetBundle dependentBundle;
            m_LoadedAssetBundles.TryGetValue(dependency, out dependentBundle);
            if (dependentBundle == null)
                return null;
        }

        return bundle;
    }

    // Load AssetBundleManifest.
    static public AssetBundleLoadManifestOperation Initialize(string manifestAssetBundleName)
    {
#if UNITY_EDITOR
        // If we're in Editor simulation mode, we don't need the manifest assetBundle.
        if (SimulateAssetBundleInEditor)
            return null;
#endif

        LoadAssetBundle(manifestAssetBundleName, true);
        var operation = new AssetBundleLoadManifestOperation(manifestAssetBundleName, "AssetBundleManifest", typeof(AssetBundleManifest));
        m_InProgressOperations.Add(operation);
        return operation;
    }

    // Load AssetBundle and its dependencies.
    static protected void LoadAssetBundle(string assetBundleName, bool isLoadingAssetBundleManifest = false)
    {
#if UNITY_EDITOR
        // If we're in Editor simulation mode, we don't have to really load the assetBundle and its dependencies.
        if (SimulateAssetBundleInEditor)
            return;
#endif

        if (!isLoadingAssetBundleManifest)
            assetBundleName = RemapVariantName(assetBundleName);

        // Check if the assetBundle has already been processed.
        bool isAlreadyProcessed = LoadAssetBundleInternal(assetBundleName, isLoadingAssetBundleManifest);

        // Load dependencies.
        if (!isAlreadyProcessed && !isLoadingAssetBundleManifest)
            LoadDependencies(assetBundleName);
    }

    // Remaps the asset bundle name to the best fitting asset bundle variant.
    static protected string RemapVariantName(string assetBundleName)
    {
        string[] bundlesWithVariant = m_AssetBundleManifest.GetAllAssetBundlesWithVariant();

        // If the asset bundle doesn't have variant, simply return.
        if (System.Array.IndexOf(bundlesWithVariant, assetBundleName) < 0)
            return assetBundleName;

        string[] split = assetBundleName.Split('.');

        int bestFit = int.MaxValue;
        int bestFitIndex = -1;
        // Loop all the assetBundles with variant to find the best fit variant assetBundle.
        for (int i = 0; i < bundlesWithVariant.Length; i++)
        {
            string[] curSplit = bundlesWithVariant[i].Split('.');
            if (curSplit[0] != split[0])
                continue;

            int found = System.Array.IndexOf(m_Variants, curSplit[1]);
            if (found != -1 && found < bestFit)
            {
                bestFit = found;
                bestFitIndex = i;
            }
        }

        if (bestFitIndex != -1)
            return bundlesWithVariant[bestFitIndex];
        else
            return assetBundleName;
    }

    // Where we actuall call WWW to download the assetBundle.
    static protected bool LoadAssetBundleInternal(string assetBundleName, bool isLoadingAssetBundleManifest)
    {
        // Already loaded.
        LoadedAssetBundle bundle = null;
        m_LoadedAssetBundles.TryGetValue(assetBundleName, out bundle);
        if (bundle != null)
        {
            bundle.m_ReferencedCount++;
            return true;
        }

        // @TODO: Do we need to consider the referenced count of WWWs?
        // In the demo, we never have duplicate WWWs as we wait LoadAssetAsync()/LoadLevelAsync() to be finished before calling another LoadAssetAsync()/LoadLevelAsync().
        // But in the real case, users can call LoadAssetAsync()/LoadLevelAsync() several times then wait them to be finished which might have duplicate WWWs.
        if (m_DownloadingWWWs.ContainsKey(assetBundleName))
            return true;

        WWW download = null;
        string url = m_BaseDownloadingURL + assetBundleName;

        // For manifest assetbundle, always download it as we don't have hash for it.
        if (isLoadingAssetBundleManifest)
            download = new WWW(url);
        else
            download = WWW.LoadFromCacheOrDownload(url, m_AssetBundleManifest.GetAssetBundleHash(assetBundleName), 0);

        m_DownloadingWWWs.Add(assetBundleName, download);

        return false;
    }

    // Where we get all the dependencies and load them all.
    static protected void LoadDependencies(string assetBundleName)
    {
        if (m_AssetBundleManifest == null)
        {
            Debug.LogError("Please initialize AssetBundleManifest by calling AssetBundleManager.Initialize()");
            return;
        }

        // Get dependecies from the AssetBundleManifest object..
        string[] dependencies = m_AssetBundleManifest.GetAllDependencies(assetBundleName);
        if (dependencies.Length == 0)
            return;

        for (int i = 0; i < dependencies.Length; i++)
            dependencies[i] = RemapVariantName(dependencies[i]);

        // Record and load all dependencies.
        m_Dependencies.Add(assetBundleName, dependencies);
        for (int i = 0; i < dependencies.Length; i++)
            LoadAssetBundleInternal(dependencies[i], false);
    }

    // Unload assetbundle and its dependencies.
    static public void UnloadAssetBundle(string assetBundleName)
    {
#if UNITY_EDITOR
        // If we're in Editor simulation mode, we don't have to load the manifest assetBundle.
        if (SimulateAssetBundleInEditor)
            return;
#endif

        //Debug.Log(m_LoadedAssetBundles.Count + " assetbundle(s) in memory before unloading " + assetBundleName);

        UnloadAssetBundleInternal(assetBundleName);
        UnloadDependencies(assetBundleName);

        //Debug.Log(m_LoadedAssetBundles.Count + " assetbundle(s) in memory after unloading " + assetBundleName);
    }

    static protected void UnloadDependencies(string assetBundleName)
    {
        string[] dependencies = null;
        if (!m_Dependencies.TryGetValue(assetBundleName, out dependencies))
            return;

        // Loop dependencies.
        foreach (var dependency in dependencies)
        {
            UnloadAssetBundleInternal(dependency);
        }

        m_Dependencies.Remove(assetBundleName);
    }

    static protected void UnloadAssetBundleInternal(string assetBundleName)
    {
        string error;
        LoadedAssetBundle bundle = GetLoadedAssetBundle(assetBundleName, out error);
        if (bundle == null)
            return;

        if (--bundle.m_ReferencedCount == 0)
        {
            bundle.m_AssetBundle.Unload(false);
            m_LoadedAssetBundles.Remove(assetBundleName);
            //Debug.Log("AssetBundle " + assetBundleName + " has been unloaded successfully");
        }
    }

    void Update()
    {
        // Collect all the finished WWWs.
        var keysToRemove = new List<string>();
        foreach (var keyValue in m_DownloadingWWWs)
        {
            WWW download = keyValue.Value;

            // If downloading fails.
            if (download.error != null)
            {
                m_DownloadingErrors.Add(keyValue.Key, download.error);
                keysToRemove.Add(keyValue.Key);
                continue;
            }

            // If downloading succeeds.
            if (download.isDone)
            {
                //Debug.Log("Downloading " + keyValue.Key + " is done at frame " + Time.frameCount);
                m_LoadedAssetBundles.Add(keyValue.Key, new LoadedAssetBundle(download.assetBundle));
                keysToRemove.Add(keyValue.Key);
            }
        }

        // Remove the finished WWWs.
        foreach (var key in keysToRemove)
        {
            WWW download = m_DownloadingWWWs[key];
            m_DownloadingWWWs.Remove(key);
            download.Dispose();
        }

        // Update all in progress operations
        for (int i = 0; i < m_InProgressOperations.Count;)
        {
            if (!m_InProgressOperations[i].Update())
            {
                m_InProgressOperations.RemoveAt(i);
            }
            else
                i++;
        }
    }

    // Load asset from the given assetBundle.
    static public AssetBundleLoadAssetOperation LoadAssetAsync(string assetBundleName, string assetName, System.Type type)
    {
        AssetBundleLoadAssetOperation operation = null;
#if UNITY_EDITOR
        if (SimulateAssetBundleInEditor)
        {
            string[] assetPaths = AssetDatabase.GetAssetPathsFromAssetBundleAndAssetName(assetBundleName, assetName);
            if (assetPaths.Length == 0)
            {
                Debug.LogError("There is no asset with name \"" + assetName + "\" in " + assetBundleName);
                return null;
            }

            // 根据类型从AssetDatabase中读取资源
            UnityEngine.Object target = AssetDatabase.LoadAssetAtPath(assetPaths[0], type);
            // 加载
            operation = new AssetBundleLoadAssetOperationSimulation(target);
        }
        else
#endif
        {
            LoadAssetBundle(assetBundleName);
            operation = new AssetBundleLoadAssetOperationFull(assetBundleName, assetName, type);

            m_InProgressOperations.Add(operation);
        }

        return operation;
    }

    //    // Load level from the given assetBundle.
    //    static public AssetBundleLoadOperation LoadLevelAsync(string assetBundleName, string levelName, bool isAdditive)
    //    {
    //        AssetBundleLoadOperation operation = null;
    //#if UNITY_EDITOR
    //        if (SimulateAssetBundleInEditor)
    //        {
    //            string[] levelPaths = AssetDatabase.GetAssetPathsFromAssetBundleAndAssetName(assetBundleName, levelName);
    //            if (levelPaths.Length == 0)
    //            {
    //                ///@TODO: The error needs to differentiate that an asset bundle name doesn't exist
    //                //        from that there right scene does not exist in the asset bundle...

    //                Debug.LogError("There is no scene with name \"" + levelName + "\" in " + assetBundleName);
    //                return null;
    //            }

    //            if (isAdditive)
    //                EditorApplication.LoadLevelAdditiveInPlayMode(levelPaths[0]);
    //            else
    //                EditorApplication.LoadLevelInPlayMode(levelPaths[0]);

    //            operation = new AssetBundleLoadLevelSimulationOperation();
    //        }
    //        else
    //#endif
    //        {
    //            LoadAssetBundle(assetBundleName);
    //            operation = new AssetBundleLoadLevelOperation(assetBundleName, levelName, isAdditive);

    //            m_InProgressOperations.Add(operation);
    //        }

    //        return operation;
    //    }
}