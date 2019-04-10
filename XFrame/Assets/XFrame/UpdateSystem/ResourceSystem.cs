﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Security.Cryptography;
/// <summary>
/// 资源系统
/// 徐振升 2019-02-16
/// 制作资源更新模块，需要与资源加载模块互动，整理以前的资源管理模块
/// </summary>
public class ResourceSystem : SingletonMonoBehaviour<ResourceSystem>
{
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
    public string CacheUrl
    {
        get;
        private set;
    }
    //任务计数
    public int TaskCount
    {
        get;
        private set;
    }
    // 需要完成的任务
    private Action TaskFinish = null;

    void Awake()
    {
        //初始化本地URL
#if UNITY_ANDROID && !UNITY_EDITOR
        LocalUrl = Application.streamingAssetsPath;
        //Android 比较特别
#else
        LocalUrl = "file://" + Application.streamingAssetsPath;
        //此url 在 windows 及 WP IOS  可以使用   
#endif
        CacheUrl = Path.Combine(Application.persistentDataPath, "vercache");

        sha1 = new SHA1Managed();
        taskState = new TaskState();
    }
    // 等待任务完成
    public void WaitForTaskFinish(Action finish)
    {
        if (TaskFinish != null)
        {
            TaskFinish += finish;
        }
        else
        {
            TaskFinish = finish;
        }
    }
    // 设置自定义缓存路径
    public void SetCacheUrl(string url)
    {
        CacheUrl = url;
    }

    public TaskState taskState
    {
        get;
        private set;
    }
    public void Update()
    {
        //处理可多线加载的任务
        if (runnner.Count < TaskCount && task.Count > 0)
        {
            runnner.Add(new DownTaskRunner(task.Dequeue()));
        }
        List<DownTaskRunner> finished = new List<DownTaskRunner>();
        foreach (var r in runnner)
        {
            if (r.www.isDone)
            {
                taskState.downloadcount++;
                finished.Add(r);
                r.task.onload(r.www, r.task.tag);
            }
        }
        foreach (var f in finished)
        {
            runnner.Remove(f);
        }
        //处理帧检测任务
        foreach (var t in frametask)
        {
            var state = t.Update();
            if (state == FrameState.Slow)
                break;
            if (state == FrameState.Finish)
            {
                frametask.Remove(t);
                taskState.downloadcount++;
                break;
            }
        }
        //监测完成事件
        if (task.Count == 0 && runnner.Count == 0 && frametask.Count == 0 && TaskFinish != null)
        {
            Action tt = TaskFinish;
            TaskFinish = null;
            tt();
        }
    }


    public Queue<DownTask> task = new Queue<DownTask>();
    public List<DownTaskRunner> runnner = new List<DownTaskRunner>();

    List<FrameTask> frametask = new List<FrameTask>();
    public void LoadFromStreamingAssets(string path, string tag, Action<WWW, string> onLoad)
    {
        Load(LocalUrl + "/" + path, tag, onLoad);

    }
    public byte[] LoadFromCacheDirect(string path)
    {

        string url = System.IO.Path.Combine(CacheUrl, path);
        using (System.IO.Stream s = System.IO.File.OpenRead(url))
        {
            byte[] b = new byte[s.Length];
            s.Read(b, 0, (int)s.Length);
            return b;
        }

    }
    public void LoadAssetBundleFromCache(string path, string tag, Action<AssetBundle, string> onLoad)
    {
        frametask.Add(new FrameTaskAssetBundle(path, tag, onLoad));
        taskState.taskcount++;
    }
    public void LoadBytesFromCache(string path, string tag, Action<byte[], string> onLoad)
    {
        frametask.Add(new FrameTaskBytes(path, tag, onLoad));
        taskState.taskcount++;
    }
    public void LoadTexture2DFromCache(string path, string tag, Action<Texture2D, string> onLoad)
    {
        Debug.Log("LoadTexture2DFromCache" + path);
        frametask.Add(new FrameTaskTexture2D(path, tag, onLoad));
        taskState.taskcount++;
    }
    public void LoadStringFromCache(string path, string tag, Action<string, string> onLoad)
    {
        frametask.Add(new FrameTaskString(path, tag, onLoad));
        taskState.taskcount++;
    }
    public void LoadFromRemote(string path, string tag, Action<WWW, string> onLoad)
    {
        Load(RemoteUrl + "/" + path, tag, onLoad);

    }
    void Load(string path, string tag, Action<WWW, string> onLoad)
    {
        task.Enqueue(new DownTask(path, tag, onLoad));
        taskState.taskcount++;
    }
    public void SaveToCache(string path, byte[] data)
    {
        string outfile = System.IO.Path.Combine(CacheUrl, path);
        string outpath = System.IO.Path.GetDirectoryName(outfile);
        if (System.IO.Directory.Exists(outpath) == false)
        {
            System.IO.Directory.CreateDirectory(outpath);
        }
        using (var s = System.IO.File.Create(outfile))
        {
            s.Write(data, 0, data.Length);
        }
        //System.IO.File.WriteAllBytes(outfile, data);

    }
    //检查版本信息
    public void BeginInit(string remotoURL, Action<Exception> onInit, IEnumerable<string> groups, int taskcount = 1, bool checkRemote = true)
    {
        this.RemoteUrl = remotoURL;
        this.TaskCount = taskcount;
        verLocal = new LocalVersion();


        Action<Exception> onInitRemote = (err) =>
            {
                if (err != null)
                {
                    onInit(err);
                    return;
                }
                Debug.Log("(ver)onInitRemote");
                //检查需更新的列表
                int addcount = 0;
                int updatecount = 0;
                verLocal.ver = this.verRemote.ver;
                foreach (var g in groups)
                {
                    if (verLocal.groups.ContainsKey(g) == false)
                    {
                        if (verRemote.groups.ContainsKey(g) == false)
                        {
                            Debug.LogWarning("group:" + g + " 在服务器和本地均不存在");
                            continue;
                        }
                        else
                        {
                            verLocal.groups[g] = new LocalVersion.VerInfo(g, "", 0);
                        }
                    }
                    verLocal.groups[g].groupfilecount = this.verRemote.groups[g].filecount;
                    verLocal.groups[g].grouphash = this.verRemote.groups[g].hash;
                    verLocal.groups[g].listverid = this.verRemote.ver;
                    //Debug.Log("check groups=====================>" + g);
                    foreach (var f in verRemote.groups[g].files)
                    {
                        //Debug.Log("check files=====>" + f.Key);

                        if (verLocal.groups[g].listfiles.ContainsKey(f.Key))
                        {

                            var fl = verLocal.groups[g].listfiles[f.Key];
                            if (fl.hash != f.Value.hash || fl.size != f.Value.length)
                            {
                                // fl.state |= LocalVersion.ResState.ResState_NeedUpdate;
                                fl.needupdate = true;
                                updatecount++;
                                Debug.Log("(ver)update:" + f.Key);
                            }
                        }
                        else
                        {
                            Debug.Log("(ver)add:" + f.Key);
                            verLocal.groups[g].listfiles[f.Key] = new LocalVersion.ResInfo(g, f.Key, f.Value.hash, f.Value.length);
                            verLocal.groups[g].listfiles[f.Key].state = LocalVersion.ResState.ResState_UseRemote;
                            verLocal.groups[g].listfiles[f.Key].needupdate = true;
                            addcount++;
                        }
                    }
                }
                Debug.Log("(ver)addcount:" + addcount + ",updatecount:" + updatecount);
                verLocal.Save(groups);
                onInit(null);
            };
        Action onInitLocal = () =>
            {

                Debug.Log("(ver)onInitLocal");
                //此处仅完成本地资源检查，如果需要服务器同步，调用BeginCheckRemote
                if (!checkRemote)
                {
                    onInit(null);
                }
                else
                {
                    //taskState.Clear();
                    verRemote = new RemoteVersion();
                    verRemote.BeginInit(onInitRemote, groups);
                }
            };
        verLocal.BeginInit(onInitLocal, groups);

        //01先尝试从存储位置初始化Verinfo，没有则从Streaming assets 初始化本地Verinfo
        //02从服务器下载总的资料
        //03完成后回调
    }

    public IEnumerable<LocalVersion.ResInfo> GetNeedDownloadRes(IEnumerable<string> groups)
    {
        List<LocalVersion.ResInfo> infos = new List<LocalVersion.ResInfo>();
        foreach (var g in groups)
        {
            if (verLocal.groups.ContainsKey(g) == false)
            {
                Debug.LogWarning("指定的Group:" + g + " 不存在于版本库中");
                continue;
            }
            foreach (var f in verLocal.groups[g].listfiles.Values)
            {
                if (f.FileName == "Thumbs.db" || f.FileName == "thumbs.db") continue;
                //   Debug.Log("state=="+f.state+"===>" + (f.state & LocalVersion.ResState.ResState_NeedUpdate));
                if (f.needupdate)
                {

                    infos.Add(f);
                }
            }
        }
        return infos;
    }
    public LocalVersion verLocal
    {
        get;
        private set;
    }
    public RemoteVersion verRemote
    {
        get;
        private set;
    }
    public SHA1Managed sha1
    {
        get;
        private set;
    }
}

public class TaskState
{
    public int taskcount = 0;
    //public int tasksize;
    public int downloadcount = 0;
    //public int downloadsize;
    public void Clear()
    {
        taskcount = 0;
        //tasksize = 0;
        downloadcount = 0;
        //downloadsize = 0;
        foreach (var t in ResourceSystem.Instance.task)
        {
            taskcount++;
            //tasksize += t.size;
        }
        foreach (var r in ResourceSystem.Instance.runnner)
        {
            taskcount++;
            //downloadsize += r.task.size;
            downloadcount++;
        }

    }
    public override string ToString()
    {
        return downloadcount + "/" + taskcount;
    }
    public float per()
    {
        return (float)downloadcount / (float)taskcount;
    }
}
//一个下载任务
public class DownTask
{
    public DownTask(string path, string tag, Action<WWW, string> onload)
    {
        this.path = path;
        this.tag = tag;
        this.onload = onload;
    }
    public string path;
    public string tag;
    public Action<WWW, string> onload;
}
public enum FrameState
{
    Nothing,
    Slow,
    Finish,
}
public abstract class FrameTask
{

    public string path;
    public string tag;
    public abstract FrameState Update();
    //
}
public class FrameTaskAssetBundle : FrameTask
{
    public FrameTaskAssetBundle(string path, string tag, Action<AssetBundle, string> onLoad)
    {
        this.onload = onLoad;
        this.tag = tag;
        this.path = path;
    }
    AssetBundleCreateRequest request;
    public Action<AssetBundle, string> onload;
    public override FrameState Update()
    {
        if (request == null)
        {
            byte[] bs = ResourceSystem.Instance.LoadFromCacheDirect(path);
            request = AssetBundle.LoadFromMemoryAsync(bs);
            return FrameState.Slow;
        }
        if (request.isDone)
        {
            onload(request.assetBundle, tag);
            return FrameState.Finish;
        }
        else
        {
            return FrameState.Nothing;
        }
    }
}
public class FrameTaskBytes : FrameTask
{
    public FrameTaskBytes(string path, string tag, Action<byte[], string> onLoad)
    {
        this.onload = onLoad;
        this.tag = tag;
        this.path = path;
    }
    public Action<byte[], string> onload;
    public override FrameState Update()
    {
        byte[] bs = ResourceSystem.Instance.LoadFromCacheDirect(path);
        onload(bs, tag);
        return FrameState.Finish;
    }
}
public class FrameTaskTexture2D : FrameTask
{
    public FrameTaskTexture2D(string path, string tag, Action<Texture2D, string> onLoad)
    {
        this.onload = onLoad;
        this.tag = tag;
        this.path = path;
    }
    public Action<Texture2D, string> onload;
    public override FrameState Update()
    {
        Debug.Log("FrameTaskTexture2D Update" + path);
        byte[] bs = ResourceSystem.Instance.LoadFromCacheDirect(path);
        Texture2D tex = new Texture2D(1, 1, TextureFormat.ARGB32, true);
        tex.LoadImage(bs);

        onload(tex, tag);
        return FrameState.Finish;
    }
}
public class FrameTaskString : FrameTask
{
    public FrameTaskString(string path, string tag, Action<string, string> onLoad)
    {
        this.onload = onLoad;
        this.tag = tag;
        this.path = path;
    }
    public Action<string, string> onload;
    public override FrameState Update()
    {
        byte[] bs = ResourceSystem.Instance.LoadFromCacheDirect(path);
        string t = System.Text.Encoding.UTF8.GetString(bs, 0, bs.Length);

        if ((UInt16)t[0] == 0xFEFF)
        {

            t = t.Substring(1);
        }
        onload(t, tag);
        return FrameState.Finish;
    }
}
//负责下载
public class DownTaskRunner
{
    public DownTaskRunner(DownTask task)
    {
        this.task = task;
        www = new WWW(this.task.path);
    }
    public WWW www;
    public DownTask task;
}