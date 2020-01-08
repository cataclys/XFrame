using System;
using System.Collections.Generic;

//public delegate void Callback();
//public delegate void Callback<T>(T arg1);
//public delegate void Callback<T, U>(T arg1, U arg2);

public class MessageSystem
{
    #region 消息表
    private static Dictionary<Msg, Delegate> eventTable = new Dictionary<Msg, Delegate>();

    public static void AddListener(Msg eventType, Action handler)
    {
        // 加锁保证线程安全
        lock (eventTable)
        {
            if (!eventTable.ContainsKey(eventType))
            {
                eventTable.Add(eventType, null);
            }
            eventTable[eventType] = (Action)eventTable[eventType] + handler;
        }
    }

    public static void RemoveListener(Msg eventType, Action handler)
    {
        lock (eventTable)
        {
            if (eventTable.ContainsKey(eventType))
            {
                eventTable[eventType] = (Action)eventTable[eventType] - handler;

                if (eventTable[eventType] == null)
                {
                    eventTable.Remove(eventType);
                }
            }
        }
    }

    public static void Broadcast(Msg eventType)
    {
        Delegate d;
        if (eventTable.TryGetValue(eventType, out d))
        {
            Action callback = (Action)d;

            if (callback != null)
            {
                callback();
            }
        }
    }

    public static void AddListener<T>(Msg eventType, Action<T> handler)
    {
        lock (eventTable)
        {

            if (!eventTable.ContainsKey(eventType))
            {
                eventTable.Add(eventType, null);
            }
            eventTable[eventType] = (Action<T>)eventTable[eventType] + handler;
        }
    }

    public static void RemoveListener<T>(Msg eventType, Action<T> handler)
    {
        lock (eventTable)
        {
            if (eventTable.ContainsKey(eventType))
            {
                eventTable[eventType] = (Action<T>)eventTable[eventType] - handler;

                if (eventTable[eventType] == null)
                {
                    eventTable.Remove(eventType);
                }
            }
        }
    }

    public static void Broadcast<T>(Msg eventType, T arg1)
    {
        Delegate d;
        if (eventTable.TryGetValue(eventType, out d))
        {
            Action<T> callback = (Action<T>)d;

            if (callback != null)
            {
                callback(arg1);
            }
        }
    }

    public static void AddListener<T, U>(Msg eventType, Action<T, U> handler)
    {
        lock (eventTable)
        {
            if (!eventTable.ContainsKey(eventType))
            {
                eventTable.Add(eventType, null);
            }
            eventTable[eventType] = (Action<T, U>)eventTable[eventType] + handler;
        }
    }

    public static void RemoveListener<T, U>(Msg eventType, Action<T, U> handler)
    {
        lock (eventTable)
        {
            if (eventTable.ContainsKey(eventType))
            {
                eventTable[eventType] = (Action<T, U>)eventTable[eventType] - handler;

                if (eventTable[eventType] == null)
                {
                    eventTable.Remove(eventType);
                }
            }
        }
    }

    public static void Broadcast<T, U>(Msg eventType, T arg1, U arg2)
    {
        Delegate d;
        if (eventTable.TryGetValue(eventType, out d))
        {
            Action<T, U> callback = (Action<T, U>)d;

            if (callback != null)
            {
                callback(arg1, arg2);
            }
        }
    }
    #endregion
    

}

public enum Msg
{
    /// <summary>
    /// 选中素材信息按钮
    /// </summary>
    SelectedAssetInfoToggle,
    /// <summary>
    /// 反选素材信息按钮
    /// </summary>
    UnSelectedAssetInfoToggle,
    /// <summary>
    /// 结束绘制
    /// </summary>
    DrawingCancel,
    /// <summary>
    /// 完成绘制
    /// </summary>
    DrawingFinish,
    /// <summary>
    /// 刷新总平面图
    /// </summary>
    RefreshSitePlans,
    /// <summary>
    /// 刷新楼层
    /// </summary>
    RefreshBuildingFloor,
    /// <summary>
    /// 创建楼层
    /// </summary>
    CreateFloor,
    /// <summary>
    /// 打开图片文件
    /// </summary>
    OpenFileImage,
    /// <summary>
    /// 选择结束
    /// </summary>
    OnSelectOver,
    /// <summary>
    /// 选择素材
    /// </summary>
    SelectAssetIcon,
    /// <summary>
    /// 消防要素toggle按钮值变化
    /// </summary>
    ItemToggleValueChanged
}
