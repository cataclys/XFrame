using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

/// <summary>
/// 存储系统
/// </summary>
public class SaveSystem
{
#if UNITY_WEBGL
    [DllImport("__Internal")]
    public static extern void SyncFiles();

    [DllImport("__Internal")]
    public static extern void WindowAlert(string message);
#endif

    #region 持久化路径
    /// <summary>
    ///  保存指定名称的数据类为文件
    /// </summary>
    /// <returns>保存成功ture,保存失败flase</returns>
    /// <param name="fileData">数据类</param>
    /// <param name="filename">文件名称</param>
    public static bool Save(SaveFile fileData, string filename)
    {
        return SaveToFile(fileData, GetSavePath(filename));
    }

    /// <summary>
    /// 读取指定名称的文件
    /// </summary>
    /// <returns>数据类</returns>
    /// <param name="fileName">文件名称</param>
    public static SaveFile Load(string fileName)
    {
        return LoadFromFile(GetSavePath(fileName));
    }

    /// <summary>
    /// 保存文件到指定路径
    /// </summary>
    /// <param name="fileData">数据类</param>
    /// <param name="filePath">文件路径</param>
    /// <returns></returns>
    private static bool SaveToFile(SaveFile fileData, string filePath)
    {
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream;

            try
            {
                if (DoesFileExists(filePath))
                {
                    File.WriteAllText(filePath, string.Empty);
                    fileStream = File.Open(filePath, FileMode.Open);
                }
                else
                {
                    fileStream = File.Create(filePath);
                }

                binaryFormatter.Serialize(fileStream, fileData);
                fileStream.Close();

#if UNITY_WEBGL
                SyncFiles();
#endif

                return true;
            }
            catch (Exception e)
            {
                PlatformSafeMessage("Failed to Save: " + e.Message);
                return false;
            }
        }
        else
        {
            Directory.CreateDirectory(Application.streamingAssetsPath);

            BinaryFormatter formatter = new BinaryFormatter();

            using (FileStream stream = new FileStream(filePath, FileMode.Create))
            {
                try
                {
                    formatter.Serialize(stream, fileData);
                }
                catch (Exception)
                {
                    return false;
                }
                return true;
            }
        }
    }

    /// <summary>
    /// 读取指定路径的文件
    /// </summary>
    /// <param name="filePath">文件路径</param>
    /// <returns>数据类</returns>
    private static SaveFile LoadFromFile(string filePath)
    {
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            SaveFile data = null;

            try
            {
                if (DoesFileExists(filePath))
                {
                    BinaryFormatter binaryFormatter = new BinaryFormatter();
                    FileStream fileStream = File.Open(filePath, FileMode.Open);

                    data = (SaveFile)binaryFormatter.Deserialize(fileStream);
                    fileStream.Close();
                }
                else
                {
                    PlatformSafeMessage("文件未找到。");
                }
            }
            catch (Exception e)
            {
                PlatformSafeMessage("加载失败: " + e.Message);
            }
            return data;
        }
        else
        {
            if (DoesFileExists(filePath))
            {
                BinaryFormatter formatter = new BinaryFormatter();

                using (FileStream stream = new FileStream(filePath, FileMode.Open))
                {
                    try
                    {
                        return formatter.Deserialize(stream) as SaveFile;
                    }
                    catch (Exception)
                    {
                        Debug.LogWarning("无法打开文件，确认文件未被占用。");
                        return null;
                    }
                }
            }
            else if (filePath.Contains("://"))
            {
                Debug.Log(" android file: " + filePath);

                WWW www = new WWW(filePath);
                while (!www.isDone) { }

                BinaryFormatter formatter = new BinaryFormatter();

                using (MemoryStream ms = new MemoryStream(www.bytes))
                {
                    try
                    {
                        return formatter.Deserialize(ms) as SaveFile;
                    }
                    catch (Exception)
                    {
                        Debug.LogWarning("无法打开文件，确认文件未被占用。");
                        return null;
                    }
                }
            }
            else
            {
                Debug.LogWarningFormat("{0}不存在", filePath);
                return null;
            }
        }
    }
    #endregion

    #region 自定义路径
    /// <summary>
    /// 保存自定义路径
    /// </summary>
    /// <returns>保存成功，保存失败</returns>
    /// <param name="fileData">数据类</param>
    /// <param name="filePath">自定义路径</param>
    public static bool Save(SaveFile fileData, string fileName, string filePath)
    {
        string path = string.Format("{0}/{1}", filePath, fileName);

        return SaveToFile(fileData, path);
    }

    /// <summary>
    /// 读取自定义路径
    /// </summary>
    /// <returns>数据类</returns>
    /// <param name="filePath">自定义路径</param>
    public static SaveFile Load(string fileName, string filePath)
    {
        string path = string.Format("{0}/{1}", filePath, fileName);

        return LoadFromFile(path);
    }
    #endregion

    #region  辅助方法
    private static bool DoesFileExists(string path)
    {
        return File.Exists(path);
    }

    /// <summary>
    /// 返回文件保存路径
    /// </summary>
    /// <param name="name">文件名称</param>
    /// <returns></returns>
    private static string GetSavePath(string name)
    {
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            return string.Format("{0}/{1}.sav", Application.persistentDataPath, name);
        }
        else
        {
            return Path.Combine(Application.persistentDataPath, name + ".sav");
        }
    }

    private static void PlatformSafeMessage(string message)
    {
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
#if UNITY_WEBGL
            WindowAlert(message);
#endif
        }
        else
        {
            Debug.LogWarning(message);
        }
    }
    #endregion

    #region Json/Bson存储
    /// <summary>
    /// 判断文件是否存在
    /// </summary>
    public static bool IsFileExists(string fileName)
    {
        return File.Exists(fileName);
    }

    /// <summary>
    /// 判断文件夹是否存在
    /// </summary>
    public static bool IsDirectoryExists(string fileName)
    {
        return Directory.Exists(fileName);
    }

    /// <summary>
    /// 创建一个文本文件    
    /// </summary>
    /// <param name="fileName">文件路径</param>
    /// <param name="content">文件内容</param>
    public static void CreateFile(string fileName, string content)
    {
        StreamWriter streamWriter = File.CreateText(fileName);
        streamWriter.Write(content);
        streamWriter.Close();
    }

    /// <summary>
    /// 创建一个文件夹
    /// </summary>
    public static void CreateDirectory(string fileName)
    {
        //文件夹存在则返回
        if (IsDirectoryExists(fileName))
            return;
        Directory.CreateDirectory(fileName);
    }

    public static void SetDataJson(string fileName, object pObject)
    {
        using (StreamWriter streamWriter = File.CreateText(fileName))
        {
            //将对象序列化为字符串
            string toSave = JsonConvert.SerializeObject(pObject);
            streamWriter.Write(toSave);
        }
    }
    public static void SetDataBson(string fileName, object pObject)
    {
        using (var stream = new MemoryStream())
        {
            var serializer = new JsonSerializer();
            var writer = new BsonWriter(stream);
            serializer.Serialize(writer, pObject);
            stream.Seek(0, SeekOrigin.Begin);
            using (FileStream fs = new FileStream(fileName, FileMode.Create))
            {
                byte[] buff = stream.ToArray();
                fs.Write(buff, 0, buff.Length);
//#if UNITY_WEBGL
//                SyncFiles();
//#endif
            }
        }
    }
    public static T GetDataBson<T>(string fileName)
    {
        if (!IsFileExists(fileName))
        {
            return default(T);
        }
        using (var streamReader = File.OpenRead(fileName))
        {
            var serializer = new JsonSerializer();
            var reader = new BsonReader(streamReader);
            return serializer.Deserialize<T>(reader);
        }
    }
    public static T GetDataJson<T>(string fileName)
    {
        using (StreamReader streamReader = File.OpenText(fileName))
        {
            string data = streamReader.ReadToEnd();
            return JsonConvert.DeserializeObject<T>(data);
        }
    }
    #endregion
}