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
    // TODO pc.ver.txt 中没有删除已经删除的问文件

    /// <summary>
    /// 版本状态
    /// </summary>
    public Text UpdateState;
    /// <summary>
    /// 下载进度文本
    /// </summary>
    public Text LoadingText;
    /// <summary>
    /// 下载进度条
    /// </summary>
    public Slider ProgressSlider;
    /// <summary>
    /// 版本服务器路径
    /// </summary>
    string downloadPath = "http://192.168.1.214/hotfix";
    /// <summary>
    /// 需要更新的组
    /// </summary>
    List<string> wantdownGroup = new List<string>();
    /// <summary>
    /// 是否正在下载
    /// </summary>
    bool indown = false;
    /// <summary>
    /// 总大小
    /// </summary>
    ulong totalSize = 0;
    /// <summary>
    /// 下载大小
    /// </summary>
    ulong downloadSize = 0;
    /// <summary>
    /// 更新的平台
    /// </summary>
    string group =

#if UNITY_EDITOR
        "pc";
#elif UNITY_STANDALONE_WIN
        "pc";
#elif UNITY_ANDROID
        "android";
#endif

    const decimal KB = 1024;
    const decimal MB = KB * 1024;
    const decimal GB = MB * 1024;

    public override void Awake()
    {
        wantdownGroup.Add(group);
    }
    public override void Start()
    {
        CheckUpdate();
    }
    /// <summary>
    /// 检查更新
    /// </summary>
    public void CheckUpdate()
    {
        // 初始化
        ResourceSystem.Instance.BeginInit(downloadPath, OnInitFinish, wantdownGroup);
        SetState("检查更新");
    }
    /// <summary>
    /// 设置状态
    /// </summary>
    /// <param name="str"></param>
    void SetState(string str)
    {
        UpdateState.text = str;
    }
    /// <summary>
    /// 检查资源完成
    /// </summary>
    /// <param name="err"></param>
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
    /// <summary>
    /// 资源文件下载完成回调
    /// </summary>
    /// <param name="resInfo"></param>
    /// <param name="error"></param>
    private void DownloadResInfo(LocalVersion.ResInfo resInfo, Exception error)
    {
        
    }

    /// <summary>
    /// 下载完成
    /// </summary>
    void DownLoadFinish()
    {
        indown = false;
        SetState("更新完成");
        ProgressSlider.value = 100;
        LoadingText.text = 100 + "%";
        SceneManager.LoadScene("MainScene");
    }

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
