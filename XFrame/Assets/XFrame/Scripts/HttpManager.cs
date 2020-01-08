//using Newtonsoft.Json;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.IO;
//using System.Runtime.InteropServices;
//using System.Runtime.Serialization;
//using System.Runtime.Serialization.Formatters.Binary;
//using System.Text;
//using UnityEngine;
//using UnityEngine.Networking;

//public class HttpManager : Singleton<HttpManager>
//{
//    /// <summary>
//    /// 单位基本信息
//    /// </summary>
//    public string GetBuildingBasicInfos
//    {
//        get { return $"localhost:8001/api/BuildingBasicInfos?name={Application.productName}"; }
//    }

//    [DllImport("__Internal")]
//    private static extern string StringReturnValueFunction();
//    public void Get<T>(string url, Action<T> action)
//    {
//        StartCoroutine(GetCoroutine<T>(url, action));
//    }
//    private IEnumerator GetCoroutine<T>(string url, Action<T> action)
//    {
//        UnityWebRequest uwr = UnityWebRequest.Get(url);
//        yield return uwr.SendWebRequest();

//        if (uwr.isNetworkError || uwr.isHttpError)
//        {
//            Debug.Log(uwr.error);
//        }
//        else
//        {
//            Debug.Log(uwr.downloadHandler.text);
//            string json = uwr.downloadHandler.text;
//            T obj = JsonConvert.DeserializeObject<T>(json);
//            action?.Invoke(obj);
//        }
//    }

//    //删除
//    public void Delete(string url, Action action = null)
//    {
//        StartCoroutine(DeleteCoroutine(url, action));
//    }
//    private IEnumerator DeleteCoroutine(string url, Action action = null)
//    {
//        UnityWebRequest uwr = UnityWebRequest.Delete(url);
//        yield return uwr.SendWebRequest();
//        if (uwr.isHttpError || uwr.isNetworkError)
//        {
//            Debug.Log(uwr.error);
//        }
//        else
//        {
//            action?.Invoke();
//            Debug.Log($"数据上传完成");
//        }
//    }
//    //上传文件
//    public void PostFile<T>(string fileName, T obj, Action<NodeWebResult> actionResult)
//    {
//        StartCoroutine(PostFileCoroutine<T>(fileName, obj, actionResult));
//    }
//    IEnumerator PostFileCoroutine<T>(string fileName, T obj, Action<NodeWebResult> actionResult)
//    {
//        JsonSerializerSettings settings = new JsonSerializerSettings
//        {
//            TypeNameHandling = TypeNameHandling.All
//        };
//        string json = JsonConvert.SerializeObject(obj, settings);
//        //序列化
//        byte[] bArray = Encoding.UTF8.GetBytes(json);
//        ////设置表单数据

//        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
//        formData.Add(new MultipartFormFileSection("file", bArray,fileName, "application/octet-stream"));
//        //请求服务器
//        using (UnityWebRequest http = UnityWebRequest.Post(string.Format(PostObjects, "WebPlan"), formData))
//        {
//            yield return http.SendWebRequest();
//            if (http.isHttpError || http.isNetworkError)
//            {
//                Debug.Log(http.error);
//            }
//            else
//            {
//                Debug.Log(http.downloadHandler.text);
//                var temp = JsonConvert.DeserializeObject<NodeWebResult>(http.downloadHandler.text);
//                actionResult(temp);
//            }
//        }
//    }
//    //上传文件
//    public void GetFile<T>(string fileName, Action<T> actionResult)
//        where T : class
//    {
//        StartCoroutine(GetFileCoroutine<T>(fileName, actionResult));
//    }
//    IEnumerator GetFileCoroutine<T>(string fileName, Action<T> actionResult)
//        where T : class
//    {
//        Debug.Log(string.Format(GetObjects, "WebPlan", fileName));
//        //请求服务器
//        using (UnityWebRequest http = UnityWebRequest.Get(string.Format(GetObjects, "WebPlan", fileName)))
//        {
//            yield return http.SendWebRequest();
//            if (http.isHttpError || http.isNetworkError)
//            {
//                Debug.Log(http.error);
//            }
//            else
//            {
//                string json = Encoding.UTF8.GetString(http.downloadHandler.data);
//                Debug.Log(json);
//                JsonSerializerSettings settings = new JsonSerializerSettings
//                {
//                    TypeNameHandling = TypeNameHandling.All
//                };
//                T t = JsonConvert.DeserializeObject<T>(json, settings);
//                actionResult?.Invoke(t);
//            }
//        }
//    }
//    //上传图片
//    public void PostImage(string fileName, Texture2D texture2d, Action<NodeWebResult> actionResult)
//    {
//        if (!texture2d.name.Equals("DefualtSprite"))
//        {
//            StartCoroutine(PostImageCoroutine(fileName, texture2d, actionResult));
//        }
//        else
//        {
//            actionResult(null);
//        }
//    }
//    IEnumerator PostImageCoroutine(string fileName, Texture2D texture2d, Action<NodeWebResult> actionResult)
//    {
//        //序列化
//        byte[] bArray = texture2d.EncodeToJPG();
//        //设置表单数据
//        WWWForm form = new WWWForm();
//        form.AddBinaryData("file", bArray, fileName);
//        Debug.Log(string.Format(PostObjects, "WebPlan"));
//        //请求服务器
//        using (UnityWebRequest http = UnityWebRequest.Post(string.Format(PostObjects, "WebPlan"), form))
//        {
//            yield return http.SendWebRequest();
//            if (http.isHttpError || http.isNetworkError)
//            {
//                Debug.Log(http.error);
//            }
//            else
//            {
//                Debug.Log(http.downloadHandler.text);
//                var temp = JsonConvert.DeserializeObject<NodeWebResult>(http.downloadHandler.text);
//                actionResult(temp);
//            }
//        }
//    }

//    //下载图片
//    public void GetImage(string fileName, Action<Texture2D> actionResult)
//    {
//        StartCoroutine(GetImageCoroutine(fileName, actionResult));
//    }
//    IEnumerator GetImageCoroutine(string fileName, Action<Texture2D> actionResult)
//    {
//        //Debug.Log(string.Format(GetObjects, "WebPlan", fileName));
//        //请求服务器
//        using (UnityWebRequest http = UnityWebRequestTexture.GetTexture(string.Format(GetObjects, "WebPlan", fileName)))
//        {
//            yield return http.SendWebRequest();
//            if (http.isHttpError || http.isNetworkError)
//            {
//                //Debug.Log(http.error);
//                actionResult(null);
//            }
//            else
//            {
//                Texture2D texture = ((DownloadHandlerTexture)http.downloadHandler).texture;
//                //Debug.Log(texture);
//                actionResult?.Invoke(texture);
//            }
//        }
//    }
//}