using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;
using System.Collections;
using System.Data;
using System.Security.Cryptography;
using Newtonsoft.Json.Bson;
using System.Runtime.InteropServices;

    public class ConverterHelp
    {
        // 分隔符
        private static char ColumnSeparator = ',';
        private static char ValueSeparator = ';';

        public static string CSV2JSON(string csv)
        {
            //获得所有行
            var lines = csv.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);

            //第一行为字段名称    id,name
            //第二行为类型        string,string
            //第三行为注释        ID,姓名
            //第四行数据          001,徐振升
            if (lines.Count() < 3) return "无效的 CSV文件! 第一行为字段名称，第二行为类型，第三行为注释，//第四行数据";

            //第一行为key
            var keys = lines[0].Split(ColumnSeparator);

            //第三行为value
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            for (int i = 3; i < lines.Count(); i++)
            {
                if (i != 3) sb.Append(',');

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

        public static string JSON2CSV(string json)
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
                return e.ToString();
            }
        }
        // 键值对转Json
        public static string TEXT2JSON(string txt)
        {
            //获得所有行
            var lines = txt.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            //拆分字符串
            StringBuilder sb = new StringBuilder();
            //开始拼接Json
            sb.Append("{");
            for (int i = 0; i < lines.Count(); i++)
            {
                if (i != 0) sb.Append(',');
                var subLines = lines[i].Split('=').Select(v => v.Trim()).ToArray();
                sb.Append(string.Format("\"{0}\":\"{1}\"", subLines[0].Trim(), subLines[1].Trim()));
            }
            sb.Append("}");
            string tempString = sb.ToString();

            object parsedJson = JsonConvert.DeserializeObject(tempString);
            return JsonConvert.SerializeObject(parsedJson, Formatting.Indented);
        }
    }

