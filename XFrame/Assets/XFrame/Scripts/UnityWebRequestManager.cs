using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;
using XFrame.UI;

public class HttpManager : Singleton<HttpManager>
{
    //服务端地址
    public string ServeAddress = "http://192.168.1.249:8000";

    //用户登录信息
    public static string Token;

    public string GetUrl(string api)
    {
        return $"{ServeAddress}{api}";
    }

    /// <summary>
    /// 用户验证的Get请求
    /// </summary>
    public void Get<T>(string api, Action<T> success, Action error = null)
    {
        StartCoroutine(GetCoroutine<T>(GetUrl(api), success, error));
    }

    IEnumerator GetCoroutine<T>(string uri, Action<T> success = null, Action error = null)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            webRequest.SetRequestHeader("accept", "application/json");
            webRequest.SetRequestHeader("Authorization", "Bearer " + Token);
            //https自定义证书
            //webRequest.certificateHandler = new MyCertificateHandler();
            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError || webRequest.isHttpError)
            {
                MessageBox.Show($"发生错误：{webRequest.error}");
                error?.Invoke();
            }
            else
            {
                //Debug.Log($"接收：{webRequest.downloadHandler.text}");
                T t = JsonConvert.DeserializeObject<T>(webRequest.downloadHandler.text);
                success?.Invoke(t);
            }
        }
    }


    public void Post<T>(string api, object obj, Action<T> success = null, Action<string> error = null)
    {
        StartCoroutine(PostCoroutine<T>(GetUrl(api), obj, success, error));
    }

    private IEnumerator PostCoroutine<T>(string url, object obj, Action<T> success = null, Action<string> error = null)
    {
        //Debug.Log(url);
        using (UnityWebRequest uwr = new UnityWebRequest(url, "POST"))
        {
            string jsonParam = JsonConvert.SerializeObject(obj);
            byte[] body = Encoding.UTF8.GetBytes(jsonParam);
            uwr.uploadHandler = new UploadHandlerRaw(body);
            uwr.downloadHandler = new DownloadHandlerBuffer();
            uwr.SetRequestHeader("Content-Type", "application/json;charset=utf-8");
            uwr.SetRequestHeader("Authorization", "Bearer " + Token);
            yield return uwr.SendWebRequest();
            if (uwr.isHttpError || uwr.isNetworkError)
            {
                error?.Invoke(uwr.error);
                Debug.Log(uwr.error);
                Debug.Log(uwr.downloadHandler.text);
            }
            else
            {
                T t = JsonConvert.DeserializeObject<T>(uwr.downloadHandler.text);
                success?.Invoke(t);
            }
        }

    }

    #region Put

    public void Put<T>(string api, T obj, Action success = null, Action<string> error = null)
    {
        StartCoroutine(PutCoroutine<T>(GetUrl(api), obj, success, error));
    }

    private IEnumerator PutCoroutine<T>(string url, T obj, Action success = null, Action<string> error = null)
    {
        Debug.Log($"PUT：{url}");
        string jsonParam = JsonConvert.SerializeObject(obj);
        byte[] body = Encoding.UTF8.GetBytes(jsonParam);
        using (UnityWebRequest uwr = UnityWebRequest.Put(url, body))
        {
            uwr.SetRequestHeader("accept", "*/*");
            uwr.SetRequestHeader("Content-Type", "application/json;charset=utf-8");
            uwr.SetRequestHeader("Authorization", "Bearer " + Token);
            yield return uwr.SendWebRequest();
            if (uwr.isHttpError || uwr.isNetworkError)
            {
                error?.Invoke(uwr.error);
                MessageBox.Show($"错误：{uwr.error}");
            }
            else
            {
                success?.Invoke();
            }
        }

    }



    #endregion

    #region Delete

    public void Delete(string api, Action success = null, Action<string> error = null)
    {
        StartCoroutine(UnityWebRequestDelete(GetUrl(api), success, error));
    }

    private static IEnumerator UnityWebRequestDelete(string url, Action success = null, Action<string> error = null)
    {
        Debug.Log($"Delete：{url}");
        using (var uwr = UnityWebRequest.Delete(url))
        {
            uwr.SetRequestHeader("accept", "*/*");
            uwr.SetRequestHeader("Content-Type", "application/json;charset=utf-8");
            uwr.SetRequestHeader("Authorization", "Bearer " + Token);
            yield return uwr.SendWebRequest();
            if (uwr.isHttpError || uwr.isNetworkError)
            {
                error?.Invoke(uwr.error);
                MessageBox.Show($"错误：{uwr.error}");
            }
            else
            {
                success?.Invoke();
            }
        }

    }

    #endregion


    //上传文件
    public void PostFile<T>(string fileName, byte[] data, Action<T> actionResult, Action error = null,
        Action<float> progress = null)
    {
        StartCoroutine(PostFileCoroutine<T>(fileName, data, actionResult, error, progress));
    }

    IEnumerator PostFileCoroutine<T>(string fileName, byte[] data, Action<T> actionResult, Action error,
        Action<float> progress)
    {
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormFileSection("file", data, fileName, "application/octet-stream"));
        //请求服务器
        using (UnityWebRequest http = UnityWebRequest.Post(GetUrl("WebPlan2D"), formData))
        {
            http.SetRequestHeader("Access-Control-Allow-Credentials", "true");
            http.SetRequestHeader("Access-Control-Allow-Headers",
                "Accept, X-Access-Token, X-Application-Name, X-Request-Sent-Time");
            http.SetRequestHeader("Access-Control-Allow-Methods", "GET, POST, OPTIONS");
            http.SetRequestHeader("Access-Control-Allow-Origin", "*");
            yield return http.SendWebRequest();
            if (http.isHttpError || http.isNetworkError)
            {
                Debug.Log(http.error);
                error?.Invoke();
            }
            else
            {
                Debug.Log(http.downloadHandler.text);
                var temp = JsonConvert.DeserializeObject<T>(http.downloadHandler.text);
                actionResult(temp);
            }
        }
    }

    //下载图片
    public void GetImage(string url, Action<Texture2D> actionResult)
    {
        if (string.IsNullOrEmpty(url) || !url.Contains("/"))
        {
            return;
        }

        string realUrl = url;
        if (!IsUrl(realUrl))
        {
            realUrl = GetUrl(url);
        }

        StartCoroutine(GetImageCoroutine(realUrl, actionResult));
    }

    /// <summary>
    /// 判断一个字符串是否为url
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static bool IsUrl(string str)
    {
        try
        {
            string Url = @"^http(s)?://";
            return Regex.IsMatch(str, Url);
        }
        catch (Exception ex)
        {
            Debug.Log(ex.ToString());
            return false;
        }
    }

    IEnumerator GetImageCoroutine(string url, Action<Texture2D> actionResult)
    {
        //Debug.Log(url);
        //请求服务器
        using (UnityWebRequest http = UnityWebRequestTexture.GetTexture(url))
        {
            yield return http.SendWebRequest();
            if (http.isHttpError || http.isNetworkError)
            {
                //Debug.Log(http.error);
                actionResult(null);
            }
            else
            {
                Texture2D texture = ((DownloadHandlerTexture) http.downloadHandler).texture;
                //Debug.Log(texture);
                actionResult?.Invoke(texture);
            }
        }
    }
}

