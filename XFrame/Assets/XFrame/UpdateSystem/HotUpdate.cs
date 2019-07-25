using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
/// <summary>
/// 资源热更新脚本
/// </summary>
public class HotUpdate : UIPanel
{
    public Text UpdateState;

    public Text LoadingText;

    public Slider ProgressSlider;


    // TODO pc.ver.txt 中没有删除已经删除的问文件
    string downloadPath = "http://192.168.1.214/hotfix";
    // 需要检查的平台
    List<string> wantdownGroup = new List<string>();

    bool indown = false;

    ulong totalSize = 0;

    ulong downloadSize = 0;

    string group =

#if UNITY_EDITOR
        "pc";
#elif UNITY_STANDALONE_WIN
        "pc";
#elif UNITY_ANDROID
        "android";
#endif

    public override void Awake()
    {
        wantdownGroup.Add(group);
    }
    public override void Start()
    {
        CheckUpdate();
    }
    // 检查更新
    public void CheckUpdate()
    {
        // 初始化
        ResourceSystem.Instance.BeginInit(downloadPath, OnInitFinish, wantdownGroup);
        SetState("检查更新");
    }
    void SetState(string str)
    {
        UpdateState.text = str;
    }
    // 检查资源完成
    void OnInitFinish(Exception err)
    {
        if (err == null)
        {
            ResourceSystem.Instance.taskState.Clear();
            //List<string> wantdownGroup = new List<string>();
            //wantdownGroup.Add(group);
            var downlist = ResourceSystem.Instance.GetNeedDownloadRes(wantdownGroup);

            Debug.Log("服务器版本：" + ResourceSystem.Instance.verRemote.ver);
            Debug.Log("本地版本：" + ResourceSystem.Instance.verLocal.ver);
            // 是否有更新
            if (((List<LocalVersion.ResInfo>)downlist).Count > 0)
            {
                SetState("发现新版本");
                foreach (var d in downlist)
                {
                    totalSize += (ulong)d.size;
                    d.Download(null);
                }
                ResourceSystem.Instance.WaitForTaskFinish(DownLoadFinish);
                indown = true;
            }
            else
            {
                SetState("版本无变化");
                SceneManager.LoadScene("MainScene");
            }
        }
        else
        {
            switch (err.Message)
            {
                case "Cannot connect to destination host":
                    SetState("无法连接到资源服务器");
                    break;
                default:
                    SetState(err.Message);
                    break;
            }
        }
    }

    private void DownloadResInfo(LocalVersion.ResInfo resInfo, Exception error)
    {
        
    }

    // 资源更新完成
    void DownLoadFinish()
    {
        indown = false;
        SetState("更新完成");
        ProgressSlider.value = 100;
        LoadingText.text = 100 + "%";
        SceneManager.LoadScene("MainScene");
        //foreach (var file in ResourceSystem.Instance.verLocal.groups["test1_ios"].listfiles.Values)
        //{
        //    if (file.FileName.Contains(".jpg"))
        //    {
        //        file.BeginLoadTexture2D((tex, tag) =>
        //        {
        //            loadedTexs.Add(tex);
        //        });
        //    }
        //}
        //ResourceSystem.Instance.verLocal.groups["test1_ios"].listfiles["background"].BeginLoadTexture2D
    }
    const decimal KB = 1024;
    const decimal MB = KB * 1024;
    const decimal GB = MB * 1024;
    void Update()
    {
        if (indown)
        {
            downloadSize = ResourceSystem.Instance.taskState.downloadedSize + ResourceSystem.Instance.taskState.downloadsize;

            ProgressSlider.value = (float)Math.Round(((double)downloadSize / totalSize) * 100, 2);

            string showText = "";

            if (totalSize > GB)
            {
                //GB
                decimal totalSizeGB = Math.Round(totalSize / GB, 1);
                decimal currentSizeGB = Math.Round(downloadSize / GB, 1);
                showText = $"{currentSizeGB}/{totalSizeGB}GB";
            }
            else if (totalSize > MB)
            {
                //MB
                decimal totalSizeMB = Math.Round(totalSize / MB, 1);
                decimal currentSizeMB = Math.Round(downloadSize / MB, 1);
                showText = $"{currentSizeMB}/{totalSizeMB}MB";
            }
            else if (totalSize > KB)
            {
                //KB
                decimal totalSizeKB = Math.Round(totalSize / KB, 1);
                decimal currentSizeKB = Math.Round(downloadSize / KB, 1);
                showText = $"{currentSizeKB}/{totalSizeKB}KB";
            }
            else
            {
                //B
                showText = $"{downloadSize}/{totalSize}B";
            }
            LoadingText.text = $"{showText}  {ProgressSlider.value}%";
        }
    }
}
