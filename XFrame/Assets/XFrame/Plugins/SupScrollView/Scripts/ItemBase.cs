using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemBase<T> : MonoBehaviour {

    public T Data;
    public int Index;

    public abstract void Init();
    public void SetData(T data,int index)
    {
        Data = data;
        Index = index;
        UpdateItemData();
        UpdateItemSize();
    }
    public abstract void UpdateItemData();
    public abstract void UpdateItemSize();
}
