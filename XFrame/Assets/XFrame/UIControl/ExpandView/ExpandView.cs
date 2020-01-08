using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class ExpandView : MonoBehaviour
{
    /// <summary>
    /// 是否折叠其他
    /// </summary>
    public bool ShowOther = false;
    /// <summary>
    /// 折叠子项父对象
    /// </summary>
    public Transform ItemsParent;
    /// <summary>
    /// 折叠子项集合
    /// </summary>
    public Dictionary<int, ExpandViewItem> Items = new Dictionary<int, ExpandViewItem>();
    /// <summary>
    /// 
    /// </summary>
    public int ItemTotalCount;
    ///// <summary>
    ///// 创建完成
    ///// </summary>
    //public Action OnAllFinished;

    public Action<int, GameObject> OnFinished;
    /// <summary>
    /// 子对象名称
    /// </summary>
    public string ItemName = "ExpandViewItem";


    public void InitExpandView(int itemCount, Action<int, GameObject> finished)
    {
        ItemTotalCount = itemCount;
        OnFinished = finished;
        for (int i = 0; i < ItemTotalCount; i++)
        {
            CreateItem(i);
        }
    }
    public void DeleteAll()
    {
        foreach (var expandViewItem in Items.Values)
        {
            Addressables.ReleaseInstance(expandViewItem.gameObject);
        }
        Items.Clear();
    }

    /// <summary>
    /// 创建折叠子项
    /// </summary>
    /// <param name="index"></param>
    private void CreateItem(int index)
    {
        Addressables.InstantiateAsync(ItemName, ItemsParent).Completed += x =>
        {
            var item = x.Result.GetComponent<ExpandViewItem>();
            item.name = index.ToString();
            item.ParentView = this;
            Items.Add(index, item);
            OnFinished?.Invoke(index, x.Result);
        };
    }

    public void RefreshItemsCount(int count)
    {
        ItemTotalCount = count;
        DeleteAll();
        for (int i = 0; i < ItemTotalCount; i++)
        {
            CreateItem(i);
        }
    }
    /// <summary>
    /// 折叠其他项
    /// </summary>
    public void ExpandOthers(ExpandViewItem expandViewItem)
    {
        if (ShowOther) return;
        foreach (var item in Items.Values.Where(i => i != expandViewItem))
        {
            item.SetExpand(false);
        }
    }
}
/// <summary>
/// 手风琴Item数据
/// </summary>
public class ExpandViewItemData
{
    public ReactiveProperty<string> Id { get; set; } = new ReactiveProperty<string>("");
    public ReactiveProperty<string> Name { get; set; } = new ReactiveProperty<string>("");
    public ReactiveProperty<bool> IsExpand { get; set; } = new ReactiveProperty<bool>(true);
    public ReactiveProperty<bool> Enabled { get; set; } = new ReactiveProperty<bool>(true);
}