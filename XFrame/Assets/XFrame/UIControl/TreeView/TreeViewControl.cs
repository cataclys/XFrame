using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
namespace XTreeView
{
    /// <summary>
    /// 树形菜单控制器
    /// </summary>
    public class TreeViewControl : MonoBehaviour
    {
        /// <summary>
        /// 当前树形菜单的数据源
        /// </summary>
        [HideInInspector]
        public List<TreeViewData> Data = null;
        /// <summary>
        /// 树形菜单中元素的模板
        /// </summary>
        public GameObject Template;
        /// <summary>
        /// 树形菜单中元素的根物体
        /// </summary>
        public Transform TreeItems;
        /// <summary>
        /// 树形菜单的纵向排列间距
        /// </summary>
        public int VerticalItemSpace = 2;
        /// <summary>
        /// 树形菜单的横向排列间距
        /// </summary>
        public int HorizontalItemSpace = 25;
        /// <summary>
        /// 树形菜单中元素的宽度
        /// </summary>
        public int ItemWidth = 305;
        /// <summary>
        /// 树形菜单中元素的高度
        /// </summary>
        public int ItemHeight = 35;
        /// <summary>
        /// 所有子元素的鼠标点击回调事件
        /// </summary>
        public delegate void ClickItemdelegate(GameObject item);
        public event ClickItemdelegate ClickItemEvent;

        //当前树形菜单中的所有元素
        private List<GameObject> _treeViewItems;
        //当前树形菜单中的所有元素克隆体（刷新树形菜单时用于过滤计算）
        private List<GameObject> _treeViewItemsClone;
        //树形菜单当前刷新队列的元素位置索引
        private int _yIndex = 0;
        //树形菜单当前刷新队列的元素最大层级
        private int _hierarchy = 0;
        //正在进行刷新
        private bool _isRefreshing = false;

        void Awake()
        {
            ClickItemEvent += ClickItemTemplate;
        }
        /// <summary>
        /// 鼠标点击子元素事件
        /// </summary>
        public void ClickItem(GameObject item)
        {
            ClickItemEvent(item);
        }
        void ClickItemTemplate(GameObject item)
        {
            //空的事件，不这样做的话ClickItemEvent会引发空引用异常
        }

        /// <summary>
        /// 返回指定名称的子元素是否被勾选
        /// </summary>
        public bool ItemIsCheck(string itemName)
        {
            for (int i = 0; i < _treeViewItems.Count; i++)
            {
                if (_treeViewItems[i].transform.Find("TreeViewText").GetComponent<Text>().text == itemName)
                {
                    return _treeViewItems[i].transform.Find("TreeViewToggle").GetComponent<Toggle>().isOn;
                }
            }
            return false;
        }
        /// <summary>
        /// 返回树形菜单中被勾选的所有子元素名称
        /// </summary>
        public List<string> ItemsIsCheck()
        {
            List<string> items = new List<string>();

            for (int i = 0; i < _treeViewItems.Count; i++)
            {
                if (_treeViewItems[i].transform.Find("TreeViewToggle").GetComponent<Toggle>().isOn)
                {
                    items.Add(_treeViewItems[i].transform.Find("TreeViewText").GetComponent<Text>().text);
                }
            }

            return items;
        }

        private List<TreeViewData> sortData;
        public void SortData()
        { 

            sortData = new List<TreeViewData>();
            while (Data.Count>0)
            {
                // 递归所有的
                for (int i = 0; i < Data.Count; i++)
                {
                    if (Data.Any(j => j.Id == Data[i].ParentID)) continue;
                    sortData.Add(Data[i]);
                    Data.RemoveAt(i);
                    i--;
                }
            }

            Data = sortData;
        }
        /// <summary>
        /// 生成树形菜单
        /// </summary>
        public void GenerateTreeView()
        {
            //删除可能已经存在的树形菜单元素
            if (_treeViewItems != null)
            {
                for (int i = 0; i < _treeViewItems.Count; i++)
                {
                    Destroy(_treeViewItems[i]);
                }
                _treeViewItems.Clear();
            }
            //重新创建树形菜单元素
            _treeViewItems = new List<GameObject>();
            #region old
            ////顶层节点
            //foreach (var t in Data.Where(item => item.ParentID == null))
            //{
            //    TreeViewItem itemView = Instantiate(Template).GetComponent<TreeViewItem>();
            //    itemView.SetHierarchy(0);
            //    itemView.SetParent(null);

            //    itemView.transform.name = "TreeViewItem";
            //    itemView.Data = t;
            //    itemView.TreeViewText.text = $"{t.Name}";
            //    itemView.TreeViewCountText.gameObject.SetActive(t.Computed);
            //    itemView.transform.SetParent(TreeItems);
            //    itemView.transform.localPosition = Vector3.zero;
            //    itemView.transform.localScale = Vector3.one;
            //    itemView.transform.localRotation = Quaternion.Euler(Vector3.zero);
            //    itemView.gameObject.SetActive(true);

            //    _treeViewItems.Add(itemView.gameObject);
            //}
            ////次级节点
            //foreach (var t in Data.Where(item => item.ParentID != null))
            //{
            //    TreeViewItem itemView = Instantiate(Template).GetComponent<TreeViewItem>();
            //    TreeViewItem tvi = _treeViewItems
            //        .Find(a => a.GetComponent<TreeViewItem>().Data.Id == t.ParentID)
            //        .GetComponent<TreeViewItem>();
            //    itemView.SetHierarchy(tvi.GetHierarchy() + 1);
            //    itemView.SetParent(tvi);
            //    tvi.AddChildren(itemView);


            //    itemView.transform.name = "TreeViewItem";
            //    itemView.Data = t;
            //    itemView.TreeViewText.text = $"{t.Name}";
            //    itemView.TreeViewCountText.gameObject.SetActive(t.Computed);
            //    itemView.transform.SetParent(TreeItems);
            //    itemView.transform.localPosition = Vector3.zero;
            //    itemView.transform.localScale = Vector3.one;
            //    itemView.transform.localRotation = Quaternion.Euler(Vector3.zero);
            //    itemView.gameObject.SetActive(true);

            //    _treeViewItems.Add(itemView.gameObject);
            //}
            #endregion
            for (int i = 0; i < Data.Count; i++)
            {
                var itemView = Instantiate(Template).GetComponent<TreeViewItem>();
                if (Data[i].ParentID == null)
                {
                    itemView.SetHierarchy(0);
                    itemView.SetParent(null);
                }
                else
                {
                    TreeViewItem tvi = _treeViewItems.Find(a => a.GetComponent<TreeViewItem>().Data.Id == Data[i].ParentID).GetComponent<TreeViewItem>();
                    itemView.SetHierarchy(tvi.GetHierarchy() + 1);
                    itemView.SetParent(tvi);
                    tvi.AddChildren(itemView);
                }

                itemView.transform.name = "TreeViewItem";
                itemView.Data = Data[i];
                itemView.TreeViewText.text = $"{Data[i].Name}";
                itemView.TreeViewCountText.gameObject.SetActive(Data[i].Computed);
                itemView.transform.SetParent(TreeItems);
                itemView.transform.localPosition = Vector3.zero;
                itemView.transform.localScale = Vector3.one;
                itemView.transform.localRotation = Quaternion.Euler(Vector3.zero);
                itemView.gameObject.SetActive(true);

                _treeViewItems.Add(itemView.gameObject);
            }
        }
        /// <summary>
        /// 根据数据创建一个节点
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public TreeViewItem CreateItemView(TreeViewData data)
        {
            TreeViewItem itemView = Instantiate(Template).GetComponent<TreeViewItem>();
            TreeViewItem tvi = _treeViewItems.Find(a => a.GetComponent<TreeViewItem>().Data.Id == data.ParentID).GetComponent<TreeViewItem>();
            // 单位不存在该消防分类
            if (tvi == null)
            {
                MessageBox.Show("单位不存在该消防分类");
                return null;
            }
            tvi.AddChildren(itemView);
            tvi.Data.Number += 1;
            //tvi.TreeViewCountText.text = tvi.Data.Computed ? $"  ({tvi.Data.Number})" : "";
            //
            itemView.SetHierarchy(tvi.GetHierarchy() + 1);
            itemView.SetParent(tvi);
            itemView.transform.name = "TreeViewItem";
            itemView.Data = data;
            itemView.TreeViewText.text = $"{tvi.Data.Name + tvi.Data.Number}";
            itemView.TreeViewCountText.gameObject.SetActive(itemView.Data.Computed);
            itemView.transform.SetParent(TreeItems);
            itemView.transform.localPosition = Vector3.zero;
            itemView.transform.localScale = Vector3.one;
            itemView.transform.localRotation = Quaternion.Euler(Vector3.zero);
            itemView.gameObject.SetActive(true);
            _treeViewItems.Add(itemView.gameObject);



            return itemView;
        }
        public void DeleteItemView(string id)
        {
            var go = _treeViewItems.Find(a => a.GetComponent<TreeViewItem>().Data.Id == id);
            if (go == null) return;
            // 找到对应节点
            TreeViewItem tvi = go.GetComponent<TreeViewItem>();
            // 找到对应节点的父节点
            TreeViewItem parent = _treeViewItems.Find(a => a.GetComponent<TreeViewItem>().Data.Id == tvi.Data.ParentID).GetComponent<TreeViewItem>();
            parent.RemoveChildren(tvi);
            parent.Data.Number -= 1;
            //parent.TreeViewCountText.text = parent.Data.Computed ? $"  ({parent.Data.Number})" : "";
            // 从对象池中移除
            _treeViewItems.Remove(tvi.gameObject);
            // 销毁
            Destroy(tvi.gameObject);
        }
        /// <summary>
        /// 刷新树形菜单
        /// </summary>
        public void RefreshTreeView()
        {
            //上一轮刷新还未结束
            if (_isRefreshing)
            {
                return;
            }

            _isRefreshing = true;
            _yIndex = 0;
            _hierarchy = 0;

            //复制一份菜单
            _treeViewItemsClone = new List<GameObject>(_treeViewItems);

            //用复制的菜单进行刷新计算
            for (int i = 0; i < _treeViewItemsClone.Count; i++)
            {
                //已经计算过或者不需要计算位置的元素
                if (_treeViewItemsClone[i] == null || !_treeViewItemsClone[i].activeSelf)
                {
                    continue;
                }

                TreeViewItem tvi = _treeViewItemsClone[i].GetComponent<TreeViewItem>();
                // 刷新子节点数量
                tvi.TreeViewCountText.text = $"  ({tvi.GetChildrenNumber()})";

                _treeViewItemsClone[i].GetComponent<RectTransform>().localPosition = new Vector3(tvi.GetHierarchy() * HorizontalItemSpace, _yIndex, 0);
                _treeViewItemsClone[i].GetComponent<RectTransform>().sizeDelta = new Vector2(ItemWidth - tvi.GetHierarchy() * HorizontalItemSpace, ItemHeight);
                _yIndex += (-(ItemHeight + VerticalItemSpace));
                if (tvi.GetHierarchy() > _hierarchy)
                {
                    _hierarchy = tvi.GetHierarchy();
                }
                //如果子元素是展开的，继续向下刷新
                if (tvi.IsExpanding)
                {
                    RefreshTreeViewChild(tvi);
                }

                _treeViewItemsClone[i] = null;
            }

            //重新计算滚动视野的区域
            float x = _hierarchy * HorizontalItemSpace + ItemWidth;
            float y = Mathf.Abs(_yIndex);
            transform.GetComponent<ScrollRect>().content.sizeDelta = new Vector2(x, y);

            //清空复制的菜单
            _treeViewItemsClone.Clear();

            _isRefreshing = false;
        }

        /// <summary>
        /// 刷新元素的所有子元素
        /// </summary>
        void RefreshTreeViewChild(TreeViewItem tvi)
        {
            //如果没有子对象，不显示展开按钮
            if (tvi.GetChildrenNumber() == 0)
            {
                tvi.BtnExpanding.gameObject.SetActive(false);
            }
            else
            {
                tvi.BtnExpanding.gameObject.SetActive(true);
            }
            for (int i = 0; i < tvi.GetChildrenNumber(); i++)
            {
                // 刷新子节点数量
                //Debug.Log((tvi.GetChildrenByIndex(i).Data.Name));
                tvi.GetChildrenByIndex(i).TreeViewCountText.text = $"  ({tvi.GetChildrenByIndex(i).GetChildrenNumber()})";

                tvi.GetChildrenByIndex(i).gameObject.GetComponent<RectTransform>().localPosition = new Vector3(tvi.GetChildrenByIndex(i).GetHierarchy() * HorizontalItemSpace, _yIndex, 0);
                tvi.GetChildrenByIndex(i).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(ItemWidth - tvi.GetChildrenByIndex(i).GetHierarchy() * HorizontalItemSpace, ItemHeight);
                _yIndex += (-(ItemHeight + VerticalItemSpace));
                if (tvi.GetChildrenByIndex(i).GetHierarchy() > _hierarchy)
                {
                    _hierarchy = tvi.GetChildrenByIndex(i).GetHierarchy();
                }

                //如果子元素是展开的，继续向下刷新
                if (tvi.GetChildrenByIndex(i).IsExpanding)
                {
                    RefreshTreeViewChild(tvi.GetChildrenByIndex(i));
                }

                int index = _treeViewItemsClone.IndexOf(tvi.GetChildrenByIndex(i).gameObject);
                if (index >= 0)
                {
                    _treeViewItemsClone[index] = null;
                }
            }
        }
    }
}