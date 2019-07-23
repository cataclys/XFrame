using ILRuntime.CLR.Method;
using ILRuntime.CLR.TypeSystem;
using ILRuntime.Runtime.Enviorment;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// ILRuntime
/// </summary>
public class ILRuntimeManager : Singleton<ILRuntimeManager>
{
    /// <summary>
    /// AppDomain是ILRuntime的入口，整个游戏全局就一个
    /// </summary>
    AppDomain appdomain;
    /// <summary>
    /// 执行方法时需要访问
    /// </summary>
    MemoryStream fs;
    private void Start()
    {
        LoadHotFixAssembly();
    }
    public void LoadHotFixAssembly()
    {
        StartCoroutine(LoadHotFixAssemblyCoroutine());
    }

    IEnumerator LoadHotFixAssemblyCoroutine()
    {
        //首先实例化ILRuntime的AppDomain，AppDomain是一个应用程序域，每个AppDomain都是一个独立的沙盒
        appdomain = new AppDomain();
        //从持久化目录读取热更代码
        using (UnityWebRequest uwr = UnityWebRequest.Get($"{Application.streamingAssetsPath}/Hotfix.dll"))
        {
            yield return uwr.SendWebRequest();
            if (uwr.isNetworkError||uwr.isHttpError)
            {
                Debug.Log("读取错误");
            }
            else
            {
                byte[] dll = uwr.downloadHandler.data;
                //using (MemoryStream fs = new MemoryStream(dll))
                //{
                    //执行方法时需要引用数据流，不能释放
                //}
                fs = new MemoryStream(dll);
                appdomain.LoadAssembly(fs, null, null);
                InitializeILRuntime();
                OnHotFixLoaded();
            }
        }
    }
    void InitializeILRuntime()
    {
        //这里做一些ILRuntime的注册，HelloWorld示例暂时没有需要注册的
    }

    void OnHotFixLoaded()
    {
        Debug.Log("调用无参数静态方法");
        //调用无参数静态方法，appdomain.Invoke("类名", "方法名", 对象引用, 参数列表);
        appdomain.Invoke("HotFix_Project.InstanceClass", "StaticFunTest", null, null);
        //调用带参数的静态方法
        Debug.Log("调用带参数的静态方法");
        appdomain.Invoke("HotFix_Project.InstanceClass", "StaticFunTest2", null, 123);


        Debug.Log("通过IMethod调用方法");
        //预先获得IMethod，可以减低每次调用查找方法耗用的时间
        IType type = appdomain.LoadedTypes["HotFix_Project.InstanceClass"];
        //根据方法名称和参数个数获取方法
        IMethod method = type.GetMethod("StaticFunTest", 0);

        appdomain.Invoke(method, null, null);

        Debug.Log("指定参数类型来获得IMethod");
        IType intType = appdomain.GetType(typeof(int));
        //参数类型列表
        List<IType> paramList = new List<ILRuntime.CLR.TypeSystem.IType>();
        paramList.Add(intType);
        //根据方法名称和参数类型列表获取方法
        method = type.GetMethod("StaticFunTest2", paramList, null);
        appdomain.Invoke(method, null, 456);

        Debug.Log("实例化热更里的类");
        object obj = appdomain.Instantiate("HotFix_Project.InstanceClass", new object[] { 233 });
        //第二种方式
        object obj2 = ((ILType)type).Instantiate();

        Debug.Log("调用成员方法");
        int id = (int)appdomain.Invoke("HotFix_Project.InstanceClass", "get_ID", obj, null);
        Debug.Log("!! HotFix_Project.InstanceClass.ID = " + id);
        id = (int)appdomain.Invoke("HotFix_Project.InstanceClass", "get_ID", obj2, null);
        Debug.Log("!! HotFix_Project.InstanceClass.ID = " + id);

        Debug.Log("调用泛型方法");
        IType stringType = appdomain.GetType(typeof(string));
        IType[] genericArguments = new IType[] { stringType };
        appdomain.InvokeGenericMethod("HotFix_Project.InstanceClass", "GenericMethod", genericArguments, null, "TestString");

        Debug.Log("获取泛型方法的IMethod");
        paramList.Clear();
        paramList.Add(intType);
        genericArguments = new IType[] { intType };
        method = type.GetMethod("GenericMethod", paramList, genericArguments);
        appdomain.Invoke(method, null, 33333);
    }
    private void OnDestroy()
    {
        if (fs != null)
        {
            fs.Dispose();
            fs = null;
        }
    }
    /// <summary>
    /// 没有参数的静态方法
    /// </summary>
    /// <param name="className"></param>
    /// <param name="methodName"></param>
    public void InvokeStaticMethod(string className, string methodName)
    {
        IType type = appdomain.LoadedTypes[className];
        IMethod method = type.GetMethod(methodName, 0);
        appdomain.Invoke(method, null, null);
    }

    //public void InvokeStaticMethod(string className, string methodName)
    //{
        
    //}
}
