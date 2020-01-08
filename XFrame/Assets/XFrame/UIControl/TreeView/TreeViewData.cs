

namespace XTreeView
{
    /// <summary>
    /// 树形菜单数据
    /// </summary>
    public class TreeViewData
    {
        /// <summary>
        /// id
        /// </summary>
        public string Id;
        /// <summary>
        /// 数据所属的父ID
        /// </summary>
        public string ParentID;
        /// <summary>
        /// 数据内容
        /// </summary>
        public string Name;
        /// <summary>
        /// 顺序
        /// </summary>
        public int Order;
        /// <summary>
        /// 是否计算数量
        /// </summary>
        public bool Computed;
        /// <summary>
        /// 数量
        /// </summary>
        public int Number;
        /// <summary>
        /// 是否显示
        /// </summary>
        public bool Enable = true;

        //public TreeViewData()
        //{
        //}
        //public TreeViewData(FireElement data)
        //{
        //    Id = data.Id;
        //    ParentID = data.ParentId;
        //    Name = data.Name;
        //    Order = data.Order;
        //    Computed = data.Computed;
        //    Number = 0;
        //    Enable = true;
            
        //}
    }
}