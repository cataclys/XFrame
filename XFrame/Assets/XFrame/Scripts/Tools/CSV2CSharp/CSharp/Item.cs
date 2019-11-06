using System.Collections.Generic;
using System;

/// <summary>
/// Item.cs——CSV信息类
/// </summary>
public class Item : ICloneable
{

    /// <summary>
    /// 名字
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 预设名称
    /// </summary>
    public string PrefabName { get; set; }

    /// <summary>
    /// Icon名称
    /// </summary>
    public string IconName { get; set; }

    /// <summary>
    /// Icon父对象
    /// </summary>
    public string IconParent { get; set; }

    /// <summary>
    /// 描述
    /// </summary>
    public string Descrption { get; set; }

    /// <summary>
    /// 距离y
    /// </summary>
    public float distanceY { get; set; }

    /// <summary>
    /// 层级(室内2048-室外4096-内外6144)
    /// </summary>
    public int layerMask { get; set; }

    /// <summary>
    /// 位置
    /// </summary>
    public List<float> position { get; set; }

    /// <summary>
    /// 角度
    /// </summary>
    public List<float> rotation { get; set; }

    /// <summary>
    /// 大小
    /// </summary>
    public List<float> scale { get; set; }

    /// <summary>
    /// 父对象
    /// </summary>
    public string parent { get; set; }

    /// <summary>
    /// 颜色
    /// </summary>
    public List<int> color { get; set; }

    /// <summary>
    /// 纹理
    /// </summary>
    public string mesh { get; set; }

    /// <summary>
    /// 创建方式
    /// </summary>
    public int buildType { get; set; }

    /// <summary>
    /// 类型
    /// </summary>
    //public ItemType Type { get; set; }

    public object Clone()
    {
        return MemberwiseClone();
    }
}
