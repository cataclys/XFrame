using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using XFrame.UI;
using Application = UnityEngine.Application;
using Button = UnityEngine.UI.Button;

public class UIViewWebCamera : UIView
{
    public string DeviceName;
    public Vector2 CameraSize;
    public float CameraFPS;

    //接收返回的图片数据  
    WebCamTexture _webCamera;
    public RawImage Texture;//作为显示摄像头的面板

    public Button Btn1;
    public Button BtnOK;
    public Button BtnCancel;
    public Button BtnClose;
    public Toggle ToggleChange;

    //BaseItemData itemData;
    public string BaseUri;

    public Action Test = null;
    /// <summary>  
    /// 初始化摄像头
    /// </summary>  
    private void Awake()
    {
        base.Awake();
        BtnOK.gameObject.SetActive(false);
        BtnCancel.gameObject.SetActive(false);
        Btn1.gameObject.SetActive(true);
        Btn1.onClick.AddListener(() =>
        {
            Pause();
            BtnOK.gameObject.SetActive(true);
            BtnCancel.gameObject.SetActive(true);
            Btn1.gameObject.SetActive(false);
        });
        BtnOK.onClick.AddListener(() =>
        {
            Save();
            BtnOK.gameObject.SetActive(false);
            BtnCancel.gameObject.SetActive(false);
            Btn1.gameObject.SetActive(true);
            if (Test != null)
            {
                Test();
                Test = null;
            }
            Hide();
        });
        BtnCancel.onClick.AddListener(() =>
        {
            Play();
            BtnOK.gameObject.SetActive(false);
            BtnCancel.gameObject.SetActive(false);
            Btn1.gameObject.SetActive(true);
        });
        BtnClose.onClick.AddListener(Hide);
        ToggleChange.onValueChanged.AddListener(b =>
        {
            Stop();
            if (b)
            {
                StartCoroutine(OpenFront());
                MessageBox.Show("1");
            }
            else
            {
                StartCoroutine(OpenBack());
                MessageBox.Show("2");
            }
        });
    }
    /// <summary>
    /// 初始化摄像头
    /// </summary>
    private IEnumerator Start()
    {
        yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
        if (Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            WebCamDevice[] devices = WebCamTexture.devices;
            //MessageBox.Show(devices.Length.ToString());
            if (devices.Length > 0)
            {
                //读取前置摄像头
                DeviceName = devices[0].name;
                _webCamera = new WebCamTexture(DeviceName, (int)CameraSize.x, (int)CameraSize.y, (int)CameraFPS);
                Texture.texture = _webCamera;
                _webCamera.Play();
            }
        }
    }

    public IEnumerator OpenFront()
    {
        yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
        if (Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            WebCamDevice[] devices = WebCamTexture.devices;
            //MessageBox.Show(devices.Length.ToString());
            if (devices.Length > 0)
            {
                //读取前置摄像头
                DeviceName = devices[0].name;
                _webCamera = new WebCamTexture(DeviceName, (int)CameraSize.x, (int)CameraSize.y, (int)CameraFPS);
                Texture.texture = _webCamera;
                Play();
            }
        }
    }
    public IEnumerator OpenBack()
    {
        yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
        if (Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            WebCamDevice[] devices = WebCamTexture.devices;
            //MessageBox.Show(devices.Length.ToString());
            if (devices.Length > 1)
            {
                //读取前置摄像头
                DeviceName = devices[1].name;
                _webCamera = new WebCamTexture(DeviceName, (int)CameraSize.x, (int)CameraSize.y, (int)CameraFPS);
                Texture.texture = _webCamera;
                Play();
            }
        }
    }

    /// <summary>
    /// 播放
    /// </summary>
    public void Play()
    {
        _webCamera?.Play();
    }
    /// <summary>
    /// 停止
    /// </summary>
    public void Stop()
    {
        _webCamera?.Stop();
    }
    /// <summary>
    /// 暂停
    /// </summary>
    public void Pause()
    {
        _webCamera?.Pause();
    }
    /// <summary>
    /// 保存图片
    /// </summary>
    public void Save()
    {
        Texture2D source = Texture2Texture2D(Texture.texture);
        //这里可以转 JPG PNG EXR  Unity都封装了固定的Api
        byte[] bytes = source.EncodeToPNG();
        BaseUri = Application.persistentDataPath + "/temp.png";
        //然后保存为图片
        File.WriteAllBytes(BaseUri, bytes);
    }
    /// <summary>
    /// texture转texture2d
    /// </summary>
    Texture2D Texture2Texture2D(Texture texture)
    {
        Texture2D texture2D = new Texture2D(texture.width, texture.height, TextureFormat.RGBA32, false);

        RenderTexture currentRT = RenderTexture.active;

        //RenderTexture 的原理参考： https://blog.csdn.net/leonwei/article/details/54972653
        RenderTexture renderTexture = RenderTexture.GetTemporary(texture.width, texture.height, 32);
        Graphics.Blit(texture, renderTexture);

        RenderTexture.active = renderTexture;
        texture2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        texture2D.Apply();

        RenderTexture.active = currentRT;
        RenderTexture.ReleaseTemporary(renderTexture);

        return texture2D;
    }
}