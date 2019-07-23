using Newtonsoft.Json;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Newtonsoft.Json.Bson;

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

    #region  辅助方法

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

    #region Json/CSV
    // 分隔符
    private static char ColumnSeparator = ',';
    private static char ValueSeparator = ';';

    private static string CSV2JSON(string csv)
    {
        //获得所有行
        var lines = csv.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);

        //第一行为字段名称    id,name
        //第二行为数据        001,徐振升
        if (lines.Count() < 2) return "无效的 CSV文件! 第一行为字段名称，第二行为数据";

        //第一行为key
        var keys = lines[0].Split(ColumnSeparator);

        //第三行为value
        StringBuilder sb = new StringBuilder();
        sb.Append("[");
        for (int i = 1; i < lines.Count(); i++)
        {
            if (i != 1) sb.Append(',');

            //列与数值不匹配，返回错误
            var values = lines[i].Split(ColumnSeparator);
            if (values.Count() != keys.Count())
            {
                return "内容格式有错误";
            }

            //开始拼接Json
            sb.Append("{");
            for (int j = 0; j < values.Count(); j++)
            {
                if (j != 0) sb.Append(',');

                // 多值列处理
                if (values[j].Contains(ValueSeparator))
                {
                    var subValues = values[j].Split(ValueSeparator).Select(v => v.Trim()).ToArray();
                    sb.Append(string.Format("\"{0}\":[\"", keys[j].Trim()));
                    sb.Append(string.Join("\"" + ValueSeparator + "\"", subValues));
                    sb.Append("\"]");
                    continue;
                }

                sb.Append(string.Format("\"{0}\":\"{1}\"", keys[j].Trim(), values[j].Trim()));
            }
            sb.Append("}");
        }
        sb.Append("]");
        string tempString = sb.ToString();
        //替换tempString中所有;为,
        tempString = tempString.Replace(';', ',');
        tempString = tempString.Replace(",\"\"", "");
        //return tempString;
        object parsedJson = JsonConvert.DeserializeObject(tempString);
        return JsonConvert.SerializeObject(parsedJson, Formatting.Indented);
    }

    private static string JSON2CSV(string json)
    {
        try
        {
            DataTable dt = JsonConvert.DeserializeObject<DataTable>(json);
            StringBuilder sb = new StringBuilder();

            IEnumerable<string> columnNames = dt.Columns.Cast<DataColumn>().
                                  Select(column => column.ColumnName);
            sb.AppendLine(string.Join(ColumnSeparator.ToString(), columnNames));

            foreach (DataRow row in dt.Rows)
            {
                IEnumerable<string> fields = row.ItemArray.Select(field =>
                {
                    if (field.GetType().IsArray)
                    {
                        return string.Join(ValueSeparator.ToString(), field as string[]);
                    }
                    return field.ToString();
                });
                sb.AppendLine(string.Join(ColumnSeparator.ToString(), fields));
            }

            return sb.ToString().Trim();
        }
        catch (Exception e)
        {
            return "无效的JSON";
        }
    }
    #endregion

    #region 文本文件存储
    /// <summary>
    /// 判断文件是否存在
    /// </summary>
    private static bool IsFileExists(string fileName)
    {
        return File.Exists(fileName);
    }

    /// <summary>
    /// 判断文件夹是否存在
    /// </summary>
    private static bool IsDirectoryExists(string fileName)
    {
        return Directory.Exists(fileName);
    }

    /// <summary>
    /// 保存一个文本文件    
    /// </summary>
    /// <param name="fileName">文本文件路径</param>
    /// <param name="text">文本文件内容</param>
    /// <param name="suffix">文本文件后缀名</param>
    private static void SaveTextFile(string fileName, string text, string suffix)
    {
        //if (IsFileExists(fileName))
        //    return;
        string path = Path.Combine(Application.persistentDataPath, fileName + suffix);
        using (StreamWriter streamWriter = File.CreateText(path))
        {
            streamWriter.Write(text);
        }
#if UNITY_WEBGL
                SyncFiles();
#endif
    }
    public static void SaveBsonFile(string fileName, object obj)
    {
        var filePath = Path.Combine(Application.persistentDataPath, fileName + ".bson");
        using (var fs = File.Open(filePath, FileMode.Create))
        {
            using (var writer = new BsonWriter(fs))
            {
                var serializer = new JsonSerializer();
                serializer.Serialize(writer, obj);
            }
        }
#if UNITY_WEBGL
                SyncFiles();
#endif
    }
    public static T LoadBsonFile<T>(string fileName)
    {
        var filePath = Path.Combine(Application.persistentDataPath, fileName + ".bson");
        T obj;
        using (var fs = File.OpenRead(filePath))
        {
            using (var reader = new BsonReader(fs))
            {
                var serializer = new JsonSerializer();
                obj = serializer.Deserialize<T>(reader);
            }
        }
        return obj;
    }
    public static string LoadTextFile(string fileName, string suffix)
    {
        string path = Path.Combine(Application.persistentDataPath, fileName + suffix);
        if (IsFileExists(path))
        {
            return File.ReadAllText(path);
        }
        return null;
    }
    public static void SaveText(string fileName, string text)
    {
        SaveTextFile(fileName, text, ".txt");
    }
    public static void SaveJson(string fileName, object obj)
    {
        string json = JsonConvert.SerializeObject(obj, Formatting.Indented);
        SaveTextFile(fileName, json, ".json");
    }
    public static void SaveCsv(string fileName, object obj)
    {
        string json = JsonConvert.SerializeObject(obj, Formatting.Indented);
        string csv = JSON2CSV(json);
        SaveTextFile(fileName, csv, ".csv");
    }
    public static T LoadJson<T>(string fileName)
        where T : class
    {
        string json = LoadTextFile(fileName, ".json");
        if (json != null)
        {
            T t = JsonConvert.DeserializeObject<T>(json);
            return t;
        }
        return null;
    }
    public static T LoadCsv<T>(string fileName)
    {
        string csv = LoadTextFile(fileName, ".csv");
        string json = CSV2JSON(csv);
        T t = JsonConvert.DeserializeObject<T>(json);
        return t;
    }
    /// <summary>
    /// 创建一个目录
    /// </summary>
    public static void CreateDirectory(string fileName)
    {
        //文件夹存在则返回
        if (IsDirectoryExists(fileName))
            return;
        Directory.CreateDirectory(fileName);
    }
    #endregion
}