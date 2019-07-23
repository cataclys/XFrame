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
    private string downloadPath = "http://192.168.1.214/hotfix";

    public Text UpdateState;

    public Text LoadingText;

    public Slider ProgressSlider;

    bool indown = false;

    float totalSize = 0;

    string group =
#if UNITY_STANDALONE_WIN
        "pc";
#elif UNITY_ANDROID
        "pc";
#elif UNITY_WEBGL
        "web";
#else
        "pc";
#endif
    private void Awake()
    {
        ILRuntimeManager.Instance.Init();
    }
    public override void Start()
    {
        CheckUpdate();
    }
    // 检查更新
    public void CheckUpdate()
    {
        // 需要检查的平台
        List<string> wantdownGroup = new List<string>();
        wantdownGroup.Add(group);
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
            List<string> wantdownGroup = new List<string>();
            wantdownGroup.Add(group);
            var downlist = ResourceSystem.Instance.GetNeedDownloadRes(wantdownGroup);

            Debug.Log("服务器版本：" + ResourceSystem.Instance.verRemote.ver);
            Debug.Log("本地版本：" + ResourceSystem.Instance.verLocal.ver);
            // 是否有更新
            if (((List<LocalVersion.ResInfo>)downlist).Count > 0)
            {
                SetState("发现新版本");
                foreach (var d in downlist)
                {
                    totalSize += d.size;
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

    void Update()
    {
        if (indown)
        {
            ProgressSlider.value = ResourceSystem.Instance.taskState.per() * 100;
            LoadingText.text = Mathf.RoundToInt(ProgressSlider.value).ToString() + "%";
            SetState(Mathf.RoundToInt(ResourceSystem.Instance.taskState.per() * totalSize / 1024) + "KB" + "/" + (totalSize / 1024) + "KB");
        }
    }
}
