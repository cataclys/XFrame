
public enum UIViewType
{
    /// <summary>
    /// 没有规则
    /// </summary>
    None,
    /// <summary>
    /// 单一显示，同时只会存在一个面板窗口，显示新的面板，前一个面板会被隐藏，可以调用ShowViewBack() 来显示之前的面板，当前面板会被隐藏
    /// </summary>
    Panel,
    Popup
}