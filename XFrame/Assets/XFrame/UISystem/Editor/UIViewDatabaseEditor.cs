using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
using System.IO;
using System.Collections.Generic;
using Object = UnityEngine.Object;

public class UIViewDatabaseEditor : EditorWindow
{
    // UIView预设列表
    List<FileInfo> uiViewList = new List<FileInfo>();
    // 当前文件
    private FileInfo currentView;
    // 滑动布局
    private Vector2 m_scrollPos;

    [MenuItem("Tools/UIView预设")]
    public static void Init()
    {
        UIViewDatabaseEditor window = EditorWindow.GetWindow<UIViewDatabaseEditor>();
        window.minSize = new Vector2(800, 400);
        window.titleContent = new GUIContent("UIView预设查看器");
        window.Show();
    }

    void OnEnable()
    {
        uiViewList.Clear();
        GetDirs(Application.dataPath);
    }

    public void GetFiles(string dir)
    {
        try
        {
            //得到文件
            string[] files = Directory.GetFiles(dir);
            //循环文件
            foreach (string file in files)
            {
                //得到后缀名
                string exname = file.Substring(file.LastIndexOf(".") + 1);
                //if (".txt|.aspx".IndexOf(file.Substring(file.LastIndexOf(".") + 1)) > -1)
                //找到预设文件
                if (".prefab".IndexOf(file.Substring(file.LastIndexOf(".") + 1)) > -1)
                {
                    //建立FileInfo对象
                    FileInfo fi = new FileInfo(file);
                    int resourcesIndex = fi.FullName.IndexOf("Resources");
        
                    string resourcesItem = fi.FullName.Substring(resourcesIndex).Replace("Resources\\", "").Replace('\\','/');

                    string prefabName = $"{resourcesItem.Substring(0, resourcesItem.LastIndexOf("."))}";
                    UIView view = Resources.Load<UIView>(prefabName);
                    //Debug.Log(prefabName);
                    //Debug.Log(view);
                    if (view != null)
                    {
                        Debug.Log(view.GetType());
                        uiViewList.Add(fi);
                    }
                }
            }
        }
        catch
        {

        }
    }
    //得到所有文件夹
    public void GetDirs(string path)
    {

        try
        {
            string[] dirs = Directory.GetDirectories(path);
            foreach (string dir in dirs)
            {
                //递归
                GetDirs(dir);
                if (dir.Contains(@"Resources"))
                {
                    //得到所有文件夹里面的文件
                    GetFiles(dir);
                }
            }
        }
        catch
        {
        }
    }
    void OnGUI()
    {

        EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
        DisplayListArea();
        DisplayMainArea();
        EditorGUILayout.EndHorizontal();
    }

    void DisplayListArea()
    {
        EditorGUILayout.BeginVertical(GUILayout.Width(250));
        EditorGUILayout.Space();

        m_scrollPos = EditorGUILayout.BeginScrollView(m_scrollPos, "box", GUILayout.ExpandHeight(true));

        for (int i = 0; i < uiViewList.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            if (currentView == uiViewList[i])
            {
                GUI.color = Color.green;
            }
            else
            {
                GUI.color = Color.white;
            }
            string viewName = uiViewList[i].Name.Substring(0, uiViewList[i].Name.LastIndexOf("."));
            if (GUILayout.Button(viewName, GUI.skin.window, GUILayout.Height(100), GUILayout.ExpandWidth(true)))
            {
                currentView = uiViewList[i];
                int assetsIndex = currentView.FullName.IndexOf("Assets");
                string assetsItem = currentView.FullName.Substring(assetsIndex);
                Object o = AssetDatabase.LoadAssetAtPath<Object>(assetsItem);
                EditorGUIUtility.PingObject(o);//黄色乒乓动画
                Selection.activeObject = o;//选中
            }
            GUI.color = Color.white;

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();
        }

        EditorGUILayout.EndScrollView();

        EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
        EditorGUILayout.LabelField("UIView: " + uiViewList.Count, GUILayout.Width(100));

        if (GUILayout.Button("刷新"))
        {
            uiViewList.Clear();
            GetDirs(Application.dataPath);
        }


        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space();
        EditorGUILayout.EndVertical();
    }

    void DisplayMainArea()
    {
        EditorGUILayout.BeginVertical();
        EditorGUILayout.HelpBox($"点击左侧Prefab可以快速定位Prefab位置", MessageType.Info);
        if (currentView!=null)
        {
            EditorGUILayout.HelpBox($"点击左侧Prefab可以快速定位Prefab位置", MessageType.Info);
        }
        EditorGUILayout.EndVertical();
    }
}