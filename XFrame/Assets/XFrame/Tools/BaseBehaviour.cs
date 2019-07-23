#define Debug

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void Callback();
public delegate void Callback<T>(T arg1);
public delegate void Callback<T, U>(T arg1, U arg2);

public class BaseBehaviour : MonoBehaviour
{
    #region 消息表
    private static Dictionary<string, Delegate> eventTable = new Dictionary<string, Delegate>();

    public void AddListener(string eventType, Callback handler)
    {
        // 加锁保证线程安全
        lock (eventTable)
        {
            if (!eventTable.ContainsKey(eventType))
            {
                eventTable.Add(eventType, null);
            }
            eventTable[eventType] = (Callback)eventTable[eventType] + handler;
        }
    }

    public void RemoveListener(string eventType, Callback handler)
    {
        lock (eventTable)
        {
            if (eventTable.ContainsKey(eventType))
            {
                eventTable[eventType] = (Callback)eventTable[eventType] - handler;

                if (eventTable[eventType] == null)
                {
                    eventTable.Remove(eventType);
                }
            }
        }
    }

    public void Invoke(string eventType)
    {
        Delegate d;
        if (eventTable.TryGetValue(eventType, out d))
        {
            Callback callback = (Callback)d;

            if (callback != null)
            {
                callback();
            }
        }
    }

    public void AddListener<T>(string eventType, Callback<T> handler)
    {
        lock (eventTable)
        {

            if (!eventTable.ContainsKey(eventType))
            {
                eventTable.Add(eventType, null);
            }
            eventTable[eventType] = (Callback<T>)eventTable[eventType] + handler;
        }
    }

    public void RemoveListener<T>(string eventType, Callback<T> handler)
    {
        lock (eventTable)
        {
            if (eventTable.ContainsKey(eventType))
            {
                eventTable[eventType] = (Callback<T>)eventTable[eventType] - handler;

                if (eventTable[eventType] == null)
                {
                    eventTable.Remove(eventType);
                }
            }
        }
    }

    public void Invoke<T>(string eventType, T arg1)
    {
        Delegate d;
        if (eventTable.TryGetValue(eventType, out d))
        {
            Callback<T> callback = (Callback<T>)d;

            if (callback != null)
            {
                callback(arg1);
            }
        }
    }

    public void AddListener<T, U>(string eventType, Callback<T, U> handler)
    {
        lock (eventTable)
        {
            if (!eventTable.ContainsKey(eventType))
            {
                eventTable.Add(eventType, null);
            }
            eventTable[eventType] = (Callback<T, U>)eventTable[eventType] + handler;
        }
    }

    public void RemoveListener<T, U>(string eventType, Callback<T, U> handler)
    {
        lock (eventTable)
        {
            if (eventTable.ContainsKey(eventType))
            {
                eventTable[eventType] = (Callback<T, U>)eventTable[eventType] - handler;

                if (eventTable[eventType] == null)
                {
                    eventTable.Remove(eventType);
                }
            }
        }
    }

    public void Invoke<T, U>(string eventType, T arg1, U arg2)
    {
        Delegate d;
        if (eventTable.TryGetValue(eventType, out d))
        {
            Callback<T, U> callback = (Callback<T, U>)d;

            if (callback != null)
            {
                callback(arg1, arg2);
            }
        }
    }
    #endregion
    public void Println(object msg)
    {
#if Debug
        Debug.Log($"[{GetType()}] {msg}");
#endif
    }
//    因为考虑性能能方面的问题，一般指标在系统开启时会被预先加载到内存，但新创建的指标需要加载到内存，已有指标需要更新也需要覆盖原dll文件。新创建的指标很容易就放到指标“库”（指标dll文件存放的目录），但要覆盖原dll文件就不容易了，原因是dll文件被其他程序占用了。

//其实文件被其他程序占用的情况我们经常遇到，主要是其他程序在使用文件时没有释放文件的句柄，从这里入手不能发现解决方法，就是把文件加载到内存，并且释放文件句柄。

//将原加载动态库的代码：

//Assembly assembly = Assembly.LoadFile(assemblyFile);

//    改成：

//byte[] assemblyBuf = File.ReadAllBytes(assemblyFile);
//    Assembly assembly = Assembly.Load(assemblyBuf);
}
