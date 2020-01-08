using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Security.Cryptography;
using UnityEngine.Events;
using UnityEngine.U2D;
using UnityEngine.Networking;
using System.Text.RegularExpressions;
using XFrame.UI;
/// <summary>
/// 资源系统
/// 徐振升 2019-04-17
/// 资源管理模块
/// </summary>
public class AssetManager1 : Singleton<AssetManager>
{
    // 精灵对象池
    private Dictionary<string, Sprite> Sprites = new Dictionary<string, Sprite>();
    // 楼层对象池
    private Dictionary<string, Texture2D> Textures = new Dictionary<string, Texture2D>();

    public SpriteAtlas LoadingAtlas;
    //本地路径
    public string LocalUrl
    {
        get;
        private set;
    }
    //远程路径
    public string RemoteUrl
    {
        get;
        private set;
    }
    //缓存地址
    //public string CacheUrl
    //{
    //    get;
    //    private set;
    //}

    void Awake()
    {
        LocalUrl = Application.streamingAssetsPath;
        //CacheUrl = ;
        RemoteUrl = "http://localhost";
        LoadingAtlas = Resources.Load<SpriteAtlas>("LoadingAtlas");
    }
    public Sprite GetSprite(string name)
    {
        Sprite temp = LoadingAtlas.GetSprite(name);
        Resources.UnloadUnusedAssets();
        return temp;
    }
    //public void LoadAssetInfo(Asset assetInfo, UnityAction<Sprite> onLoad)
    //{
        //var ms = Regex.Matches(assetInfo.FillValue, @"(?<=/)\w*?\.(jpg|png)");
        //LoadSprite(ms[0].Value, onLoad, assetInfo.FillValue);
    //}
    public void LoadSprite(string name, UnityAction<Sprite> onLoad, string url = "")
    {
        // 对象池中查找
        if (Sprites.ContainsKey(name))
        {
            onLoad(Sprites[name]);
        }
        else
        {
            // 对象池中不存在，去持久化目录加载
            string uriCache = "";//$"{Application.persistentDataPath}/{WebRequestManager.Instance.identityInfo.userId}/{name}";
            if (File.Exists(uriCache))
            {
                StartCoroutine(LoadSpriteCoroutine(uriCache, name, (textureCache) =>
                {
                    if (textureCache != null)
                    {
                        if (!Sprites.ContainsKey(name))
                        {
                            Sprites.Add(name, CreateSprite(textureCache));
                        }
                        onLoad(Sprites[name]);
                    }
                }));
            }
            else
            {
                string uriRemote = url;
                StartCoroutine(LoadSpriteCoroutine(uriRemote, name, (c) =>
                {
                    if (c != null)
                    {
                        SaveToCache(uriCache, c.EncodeToPNG());
                        if (!Sprites.ContainsKey(name))
                        {
                            Sprites.Add(name, CreateSprite(c));
                        }
                        onLoad(Sprites[name]);
                    }
                }));
            }
        }

    }
    public void LoadTexture(string path, UnityAction<Texture2D> onLoad)
    {
        var ms = Regex.Matches(path, @"(?<=/)\w*?\.(jpg|png)");
        string fileName = ms[0].Value;

        // 对象池中查找
        if (Textures.ContainsKey(fileName))
        {
            onLoad(Textures[fileName]);
        }
        else
        {
            // 对象池中不存在，去持久化目录加载
            //string uriCache = $"{Application.persistentDataPath}/{WebRequestManager.Instance.identityInfo.userId}/{fileName}";
            string uriCache = "";
            if (File.Exists(uriCache))
            {
                StartCoroutine(LoadSpriteCoroutine(uriCache, fileName, (textureCache) =>
                {
                    if (textureCache != null)
                    {
                        textureCache.Compress(false);
                        if (!Textures.ContainsKey(fileName))
                        {
                            Textures.Add(fileName, textureCache);
                        }
                        onLoad(Textures[fileName]);
                    }
                }));
            }
            else
            {
                string uriRemote = path;
                StartCoroutine(LoadSpriteCoroutine(uriRemote, name, (c) =>
                {
                    if (c != null)
                    {
                        SaveToCache(uriCache, c.EncodeToJPG());
                        c.Compress(false);
                        if (!Textures.ContainsKey(fileName))
                        {
                            Textures.Add(fileName, c);
                        }
                        onLoad(Textures[fileName]);
                    }
                }));
            }
        }
    }
    private IEnumerator LoadSpriteCoroutine(string path, string name, UnityAction<Texture2D> onLoad)
    {
        //Debug.Log(path);
        //记载图片
        //UIManager.ShowView<UIViewLoading>();
        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(path))
        {
            yield return uwr.SendWebRequest();
            //记载图片
            //UIManager.HideView<UIViewLoading>();
            if (uwr.isNetworkError || uwr.isHttpError)
            {
                Debug.Log(uwr.error);
            }
            else
            {
                // Get downloaded asset bundle
                var texture = DownloadHandlerTexture.GetContent(uwr);
                onLoad(texture);
            }
        }
    }
    //通过texture2d 生成Sprite
    public Sprite CreateSprite(Texture2D texture)
    {
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
        return sprite;
    }

    public void SaveToCache(string path, byte[] data)
    {
        string outpath = Path.GetDirectoryName(path);
        if (Directory.Exists(outpath) == false)
        {
            Directory.CreateDirectory(outpath);
        }
        using (var s = File.Create(path))
        {
            s.Write(data, 0, data.Length);
        }
    }

    //public void LoadFloorPlanTexture()
    //{

    //}

    //IEnumerator LoadTexture(FloorPlanResponse floorPlan, UnityAction<Texture2D> unityAction)
    //{
    //    if (Textures.ContainsKey(floorPlan.floorName))
    //    {
    //        unityAction(Textures[floorPlan.floorName]);
    //    }
    //    else
    //    {
    //        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(floorPlan.imageUrl))
    //        {
    //            UIManager.Instance.ShowView<UIViewLoading>();
    //            yield return uwr.SendWebRequest();
    //            if (uwr.isNetworkError || uwr.isHttpError)
    //            {
    //                Debug.Log(uwr.error);
    //            }
    //            else
    //            {
    //                // Get downloaded asset bundle
    //                var texture = DownloadHandlerTexture.GetContent(uwr);
    //                texture.Compress(false);
    //                Textures.Add(floorPlan.floorName, texture);
    //                if (unityAction != null)
    //                {
    //                    unityAction(texture);
    //                }
    //            }
    //            UIManager.Instance.HideView<UIViewLoading>();
    //        }
    //    }       
    //}
}

