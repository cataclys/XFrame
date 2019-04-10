//// C# example.
//using UnityEditor;
//using UnityEditor.Build.Reporting;
//using System;
//using System.Collections.Generic;
//using UnityEngine;
//using System.IO;

//public class ScriptBatch
//{
//    /*
//     * 广西消防总队-二维预案编制工具
//南宁消防支队-二维预案编制工具
//柳州消防支队-二维预案编制工具
//桂林消防支队-二维预案编制工具
//梧州消防支队-二维预案编制工具
//北海消防支队-二维预案编制工具
//防城港消防支队-二维预案编制工具
//钦州消防支队-二维预案编制工具
//贵港消防支队-二维预案编制工具
//玉林消防支队-二维预案编制工具
//白色消防支队-二维预案编制工具
//贺州消防支队-二维预案编制工具
//河池消防支队-二维预案编制工具
//来宾消防支队-二维预案编制工具
//崇左消防支队-二维预案编制工具
//北京安信科创软件有限公司-通用
//     */
//    [MenuItem("打包工具/选择创建渠道包")]
//    public static void BuildSelectedGame()
//    {
//        //获取保存的目录路径
//        //string path = EditorUtility.SaveFolderPanel("选择构建目录", "", "默认名称");
//        //获取选择的文件路径
//        string path = EditorUtility.OpenFilePanel("选择要构建的Config文件", "", "txt");
//        Debug.Log($"选择文件：{path}");
//        string[] levels = FindEnabledEditorScenes();
//        foreach (var item in levels)
//        {
//            Debug.Log($"构建场景：{item}");
//        }
//        //如果存在配置文件，先删除
//        string file = $"{Application.streamingAssetsPath}/Config.txt";
//        if (File.Exists(file))
//        {
//            Debug.Log("初始目录中包含Config文件，删除");
//            File.Delete(file);
//        }
//        FileUtil.CopyFileOrDirectory(path, file);
//        //获取保存的目录路径
//        string savePath = EditorUtility.SaveFilePanel("选择构建目录", "", "", "apk");
//        Debug.Log(savePath);
//        var report = BuildPipeline.BuildPlayer(levels, savePath, BuildTarget.Android, BuildOptions.None);
//        if (report.summary.result != BuildResult.Succeeded)
//        {
//            throw new Exception("构建失败");
//        }
//        ////每次build删除之前的残留
//        //if (Directory.Exists(target_dir))
//        //{
//        //    if (File.Exists(target_name))
//        //    {
//        //        File.Delete(target_name);
//        //    }
//        //}
//        //else
//        //{
//        //    Directory.CreateDirectory(target_dir);
//        //}
//        //
//        //FileUtil.CopyFileOrDirectory("Assets/Templates/Config.txt", path + "Config.txt");
//        //// 构建
//        //var report = BuildPipeline.BuildPlayer(levels, path + "/BuiltGame.exe", BuildTarget.Android, BuildOptions.None);
//        //if (report.summary.result != BuildResult.Succeeded)
//        //{
//        //    throw new Exception("构建失败");
//        //}
//        //AssetDatabase.CopyAsset("", "");
//        // 拷贝文件到指定目录
//        //FileUtil.CopyFileOrDirectory("Assets/Templates/Readme.txt", path + "Readme.txt");

//        // Run the game (Process class from System.Diagnostics).
//        //Process proc = new Process();
//        //proc.StartInfo.FileName = path + "/BuiltGame.exe";
//        //proc.Start();
//    }
//    [MenuItem("打包工具/创建所有渠道包")]
//    public static void BuildAllGame()
//    {
//        BuildOneGame("F:/2DPlan2018/Assets/Configs/北京安信科创软件有限公司-通用/Config.txt");
//        BuildOneGame("F:/2DPlan2018/Assets/Configs/白色消防支队-二维预案编制工具/Config.txt");
//        BuildOneGame("F:/2DPlan2018/Assets/Configs/北海消防支队-二维预案编制工具/Config.txt");
//        BuildOneGame("F:/2DPlan2018/Assets/Configs/广西消防总队-二维预案编制工具/Config.txt");
//        BuildOneGame("F:/2DPlan2018/Assets/Configs/南宁消防支队-二维预案编制工具/Config.txt");
//        BuildOneGame("F:/2DPlan2018/Assets/Configs/柳州消防支队-二维预案编制工具/Config.txt");
//        BuildOneGame("F:/2DPlan2018/Assets/Configs/桂林消防支队-二维预案编制工具/Config.txt");
//        BuildOneGame("F:/2DPlan2018/Assets/Configs/梧州消防支队-二维预案编制工具/Config.txt");
//        BuildOneGame("F:/2DPlan2018/Assets/Configs/防城港消防支队-二维预案编制工具/Config.txt");
//        BuildOneGame("F:/2DPlan2018/Assets/Configs/钦州消防支队-二维预案编制工具/Config.txt");
//        BuildOneGame("F:/2DPlan2018/Assets/Configs/贵港消防支队-二维预案编制工具/Config.txt");
//        BuildOneGame("F:/2DPlan2018/Assets/Configs/玉林消防支队-二维预案编制工具/Config.txt");
//        BuildOneGame("F:/2DPlan2018/Assets/Configs/白色消防支队-二维预案编制工具/Config.txt");
//        BuildOneGame("F:/2DPlan2018/Assets/Configs/贺州消防支队-二维预案编制工具/Config.txt");
//        BuildOneGame("F:/2DPlan2018/Assets/Configs/河池消防支队-二维预案编制工具/Config.txt");
//        BuildOneGame("F:/2DPlan2018/Assets/Configs/来宾消防支队-二维预案编制工具/Config.txt");
//        BuildOneGame("F:/2DPlan2018/Assets/Configs/崇左消防支队-二维预案编制工具/Config.txt");
//    }
//    public static void BuildOneGame(string path)
//    {
//        string[] levels = FindEnabledEditorScenes();

//        //如果存在配置文件，先删除
//        string file = $"{Application.streamingAssetsPath}/Config.txt";
//        if (File.Exists(file))
//        {
//            File.Delete(file);
//        }
//        FileUtil.CopyFileOrDirectory(path, file);
//        string[] test = path.Split('/');
//        string filePath = @"F:\2DPlan2018\Build\" + $"{test[4]}.apk";
//        var report = BuildPipeline.BuildPlayer(levels, filePath, BuildTarget.Android, BuildOptions.None);
//        if (report.summary.result != BuildResult.Succeeded)
//        {
//            throw new Exception("构建失败");
//        }
//        else
//        {
//            Debug.Log($"{filePath}构建成功。");
//        }
//    }
//    //获取所有激活的场景
//    private static string[] FindEnabledEditorScenes()
//    {
//        List<string> EditorScenes = new List<string>();
//        foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
//        {
//            if (!scene.enabled) continue;
//            EditorScenes.Add(scene.path);
//        }
//        return EditorScenes.ToArray();
//    }
//}