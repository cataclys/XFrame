using UnityEngine;

/// <summary>
/// 继承MonoBehaviour，因为需要Coroutines.
/// </summary>
public class SingletonMonoBehaviour<T> : MonoBehaviour 
    where T : MonoBehaviour
{
    //静态私有字段
    private static T _instance;
    //静态锁
    private static object _lock = new object();
    //静态公共属性
    public static T Instance
    {
        //获取对象
        get
        {
            //对象是单例的，程序运行期间只会创建一次，销毁后就不存在了。
            if (applicationIsQuitting)
            {
                Debug.LogWarning("[Singleton] Instance '" + typeof(T) +
                    "' 已经销毁." +
                    " 不需要再次创建.");
                return null;
            }
            //加锁
            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = (T)FindObjectOfType(typeof(T));

                    if (FindObjectsOfType(typeof(T)).Length > 1)
                    {
                        Debug.LogError("[Singleton] 发生异常 " +
                            " - 单例类只能有1个" +
                            " 修复错误");
                        return _instance;
                    }

                    if (_instance == null)
                    {
                        GameObject singleton = new GameObject();
                        _instance = singleton.AddComponent<T>();
                        singleton.name = "(Singleton) " + typeof(T).ToString();

                        DontDestroyOnLoad(singleton);

                        Debug.LogWarning("[Singleton]  " + typeof(T) +
                            " 不存在场景中 '" + singleton +
                            "' 已经通过 DontDestroyOnLoad 创建");
                    }
                    else
                    {
                        Debug.LogWarning("[Singleton] 已经创建: " +
                            _instance.gameObject.name);
                    }
                }
                //返回单例字段
                return _instance;
            }
        }
    }
    //静态私有字段，判断开关
    private static bool applicationIsQuitting = false;
    /// <summary>
    /// Unity退出的时候,销毁顺序是随机的。
    /// 单例只会在程序退出后销毁。
    /// 如果脚本调用实例时它已经被销毁, 
    /// 它将会创建一个对象停留在编辑器场景
    /// 停止播放器也不会销毁它
    /// 确保不会产生这种BUG
    /// </summary>
    public void OnDestroy()
    {
        applicationIsQuitting = true;
    }
}