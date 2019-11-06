using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class WebCameraManager : MonoBehaviour
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
    /// <summary>  
    /// 初始化摄像头
    /// </summary>  
    public IEnumerator Start()
    {
        yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
        if (Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            WebCamDevice[] devices = WebCamTexture.devices;
            DeviceName = devices[0].name;
            _webCamera = new WebCamTexture(DeviceName, (int)CameraSize.x, (int)CameraSize.y, (int)CameraFPS);

            Texture.texture = _webCamera;
            _webCamera.Play();
        }
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

            gameObject.SetActive(false);
        });
        BtnCancel.onClick.AddListener(() =>
        {
            Play();
            BtnOK.gameObject.SetActive(false);
            BtnCancel.gameObject.SetActive(false);
            Btn1.gameObject.SetActive(true);
        });
    }


    public void Play()
    {
        _webCamera.Play();
    }


    public void StopCamera()
    {
        _webCamera.Stop();
    }

    public void Pause()
    {
        _webCamera.Pause();
    }
    public void Save()
    {
        Texture2D source = Texture2Texture2D(Texture.texture);
        //这里可以转 JPG PNG EXR  Unity都封装了固定的Api
        byte[] bytes = source.EncodeToPNG();
        //然后保存为图片
        File.WriteAllBytes(Application.streamingAssetsPath + "/" + Time.time + ".png", bytes);
    }

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