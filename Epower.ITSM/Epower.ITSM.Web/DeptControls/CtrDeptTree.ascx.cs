/*******************************************************************
 * 版权所有：
 * Description：部门展示树控件
 * 
 * 
 * Create By  ：
 * Create Date：
 * *****************************************************************/
using System;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Epower.DevBase.Organization;
using Epower.DevBase.Organization.Base;
using Epower.DevBase.BaseTools;
using System.Collections.Generic;
using Epower.DevBase.Organization.SqlDAL;
using System.Text;

namespace Epower.ITSM.Web.DeptControls
{


    public partial class CtrDeptTree : System.Web.UI.UserControl
    {
        #region Public Property

        /// <summary>
        /// 部门树切换选择节点时触发的委托
        /// </summary>
        /// <param name="sender">事件发起对象</param>
        /// <param name="node">选择的节点</param>
        public delegate void SelectedChangeHandler(object sender, TreeNode node);

        /// <summary>
        /// 部门树切换选择节点时触发的事件
        /// </summary>
        public event SelectedChangeHandler SelectedChangeEvent;


        /// <summary>
        /// 当前选中部门的编号
        /// </summary>
        public long CurrentDeptId
        {
            get
            {
                if (tvDept.SelectedNode != null)
                    return long.Parse(tvDept.SelectedNode.Value);

                return -1;
            }
        }

        /// <summary>
        /// 当前选中部门的编号
        /// </summary>
        public String CurrentDeptName
        {
            get
            {
                if (tvDept.SelectedNode != null)
                    return tvDept.SelectedNode.Text;

                return "";
            }
        }

        #endregion

        public string RootImg
        {
            get
            {
                if (ViewState["RootImg"] != null)
                    return ViewState["RootImg"].ToString();
                else
                    return "..\\Images\\root.gif";
            }
            set
            {
                ViewState["RootImg"] = value;
            }
        }

        public string NodeImg
        {
            get
            {
                if (ViewState["NodeImg"] != null)
                    return ViewState["NodeImg"].ToString();
                else
                    return "..\\Images\\FlowDesign\\333.ico";
            }
            set
            {
                ViewState["NodeImg"] = value;
            }
        }

        /// <summary>
        ///  生成一级的
        /// </summary>
        public bool FirstLevel
        {
            get
            {
                if (ViewState["FirstLevel"] != null)
                    return bool.Parse(ViewState["FirstLevel"].ToString());
                else
                    return false;
            }
            set
            {
                ViewState["FirstLevel"] = value;
            }
        }

        private string strNodeIndex = "0";
        private long lngCurrDeptID = 0;



        /// <summary>
        /// 显示部门树的时候是否包含本部门及下级部门
        /// </summary>
        public bool LimitCurr
        {
            get
            {
                return ViewState["LimitCurr"] == null ? false : bool.Parse(ViewState["LimitCurr"].ToString());
            }
            set { ViewState["LimitCurr"] = value; }
        }

        /// <summary>
        /// 当前部门编号
        /// </summary>
        public long CurrDeptID
        {
            get { return ViewState["CurrDeptID"] == null ? 0 : StringTool.String2Long(ViewState["CurrDeptID"].ToString()); }
            set { ViewState["CurrDeptID"] = value; }
        }

        /// <summary>
        /// 是否注册客户端事件
        /// </summary>
        public bool IsRegisterClientEvent
        {
            get
            {
                return ViewState["IsRegisterClientEvent"] == null ? false : bool.Parse(ViewState["IsRegisterClientEvent"].ToString());
            }
            set { ViewState["IsRegisterClientEvent"] = value; }
        }


        private string deptid;
        /// <summary>
        /// 部门ID
        /// </summary>
        public string Deptid
        {
            get { return deptid; }
            set { deptid = value; }
        }


        //DataTable _dataTable;

        protected void Page_Load(object sender, System.EventArgs e)
        {

            //_dataTable = DeptDP.GetAllDeptForTree();

            if (!Page.IsPostBack)
            {
                Session["deptvaluectr"] = "1";
                LoadData();
            }
        }

        private void InitTreeView()
        {
            long lngRootID = long.Parse(Session["RootDeptID"].ToString());
            if (Session["OldDeptID"] != null)
            {
                lngCurrDeptID = long.Parse(Session["OldDeptID"].ToString());

            }
            else
            {
                lngCurrDeptID = lngRootID;
            }


            tvDept.Nodes.Clear();
            if (lngRootID == 0)
            {
                return;
            }


            ODeptCollection dc = DeptControl.GetAllDeptCollection();

            TreeNode root = new TreeNode();
            root.Text = dc.GetODept(lngRootID).Name;
            root.Value = lngRootID.ToString();


            root.Expanded = true;


            if (dc.GetODept(lngRootID).DeptKind == eO_DeptKind.eOrg)
            {
                root.ImageUrl = RootImg;
            }
            else
            {
                if (dc.GetODept(lngRootID).IsTemp == 0)
                {
                    root.ImageUrl = NodeImg;
                }
                else
                {
                    root.ImageUrl = NodeImg;
                }
            }

            tvDept.Nodes.Add(root);//添加跟目录
            AddSubDepts(ref root, dc, lngRootID);//添加子目录


            tvDept.Attributes.Add("onclick", "Tree_Click(event,'')");
            //tvDept.Attributes.Add("ondblclick", "javascript:return Tree_DBClick(event, '')");
        }

        private void AddSubDepts(ref TreeNode root, ODeptCollection dc, long lngID)
        {
            TreeNode node;
            foreach (ODept d in dc)
            {
                if (d.ParentID == lngID && d.ID != d.ParentID)
                {

                    //当限制显示当前部门时判断
                    //if (d.ID != this.CurrDeptID || this.LimitCurr == false)
                    //{
                    node = new TreeNode();
                    node.Value = d.ID.ToString();
                    node.Text = d.Name;

                    node.Expanded = false;


                    if (d.DeptKind == eO_DeptKind.eOrg)
                    {
                        node.ImageUrl = RootImg;
                    }
                    else
                    {
                        if (d.IsTemp == 0)
                        {
                            node.ImageUrl = NodeImg;
                        }
                        else
                        {
                            node.ImageUrl = NodeImg;
                        }
                    }
                    root.ChildNodes.Add(node);
                    //if (!FirstLevel)
                    //{
                    //    AddSubDepts(ref node, dc, d.ID);
                    //}

                    //}
                }
            }
        }

        #region Web 窗体设计器生成的代码
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: 该调用是 ASP.NET Web 窗体设计器所必需的。
            //
            InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        ///		设计器支持所需的方法 - 不要使用代码编辑器
        ///		修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {

        }
        #endregion

        #region 加载部门树 - 2013-03-29 @孙绍棕

        /// <summary>
        /// 加载部门树
        /// </summary>
        private void LoadData()
        {
            long lngDeptId = GetDeptIdFromSession();

            String strDeptId = Request.QueryString["deptid"];
            if (!String.IsNullOrEmpty(strDeptId))
            {
                lngDeptId = Convert.ToInt64(strDeptId);

                if (lngDeptId > 1)
                {
                    ShowNodePath(lngDeptId);
                }
                else
                {
                    ShowNodeList(1, tvDept.SelectedNode);
                }

            }
            else if (lngDeptId > 1)
            {
                ShowNodePath(lngDeptId);
            }
            else
            {
                ShowNodeList(1, tvDept.SelectedNode);
            }

            if (IsRegisterClientEvent)
            {
                tvDept.Attributes.Add("onclick", "if (typeof(Tree_Click) != 'undefined') { Tree_Click();}");
                tvDept.Attributes.Add("ondblclick", "if (typeof(Tree_DBClick) != 'undefined') { Tree_DBClick();}");
            }
        }

        #endregion

        #region 从会话状态中获取设置的部门编号 - 2013-03-29 @孙绍棕

        /// <summary>
        /// 从会话状态中获取设置的部门编号
        /// </summary>
        /// <returns></returns>
        private long GetDeptIdFromSession()
        {
            string DeptID = "0";
            if (Session["OldDeptID"] != null)
                DeptID = Session["OldDeptID"].ToString();
            else
                DeptID = Session["RootDeptID"].ToString();

            if (Session["RootDeptID"] != null && Session["RootDeptID"].ToString() != "0")
            {
                return long.Parse(DeptID);
            }

            return -1;
        }

        #endregion

        #region 在指定的部门节点下创建部门树 - 2013-03-29 @孙绍棕

        /// <summary>
        /// 在指定的部门节点下创建部门树
        /// </summary>
        /// <param name="lngDeptId">部门编号</param>
        /// <param name="treeNode">部门节点</param>
        /// <returns></returns>
        private TreeNode ShowNodeList(long lngDeptId, TreeNode treeNode)
        {
            DataTable dataTable = DeptDP.GetDeptById(lngDeptId);

            if (dataTable == null) return null;

            DataRowView drvDeptInfo = dataTable.DefaultView[0];
            String strDeptFullId = Convert.ToString(drvDeptInfo["fullid"]);

            Int32 intFirstLevel = strDeptFullId.Length + 6;      // 第一层
            Int32 intSecondLevel = strDeptFullId.Length + 12;    // 第二层

            DataTable dtNearbyDeptInfo = DeptDP.GetNearbyDeptId(lngDeptId, intFirstLevel, intSecondLevel);

            DataView dataView = new DataView(dtNearbyDeptInfo);
            dataView.RowFilter = String.Format("parentid = {0}", lngDeptId);
            dataView.Sort = "sortid asc";

            if (treeNode == null)
            {
                treeNode = CreateNode(drvDeptInfo);
                tvDept.Nodes.Add(treeNode);
            }

            ShowNodePath(dataView, treeNode);

            if (!treeNode.Expanded.HasValue || treeNode.Expanded.Value == false)
                treeNode.Expand();

            treeNode.Selected = true;

            return treeNode;
        }

        #endregion

        #region 使用部门表递归的创建部门节点 - 2013-03-29 @孙绍棕

        /// <summary>
        /// 使用部门表递归的创建部门节点
        /// </summary>
        /// <param name="dataView">部门表视图</param>
        /// <param name="treeNode">父部门节点</param>
        private void ShowNodePath(DataView dataView, TreeNode treeNode)
        {
            foreach (DataRowView dr in dataView)
            {
                long lngNodeId = Convert.ToInt64(dr["deptid"]);

                TreeNode childNode = CreateNode(dr);

                treeNode.ChildNodes.Add(childNode);
                childNode.Collapse();

                DataView dvNext = new DataView(dataView.Table);
                dvNext.RowFilter = String.Format("parentid = {0}", lngNodeId);
                dvNext.Sort = "sortid asc";

                if (dvNext.Count > 0)
                {
                    ShowNodePath(dvNext, childNode);
                }
            }
        }

        #endregion

        #region 寻找并展开指定的部门节点 - 2013-03-29 @孙绍棕

        /// <summary>
        /// 寻找并展开指定的部门节点
        /// </summary>
        /// <param name="lngDeptId">部门编号</param>
        private void ShowNodePath(long lngDeptId)
        {
            String strDeptFullId = DeptDP.GetDeptFullID(lngDeptId);
            // 015465 015466 015524 015527

            List<String> listDeptId = new List<String>();

            Int32 intStart = 0;
            while (intStart < strDeptFullId.Length)
            {
                String strDeptId = strDeptFullId.Substring(intStart, 6);
                listDeptId.Add(strDeptId.Remove(0, 1));

                intStart = intStart + 6;
            }


            String strClickedNodeId = listDeptId[listDeptId.Count - 1];

            listDeptId.Insert(0, "1");
            listDeptId.RemoveAt(listDeptId.Count - 1);

            TreeNode treeNode = LoopExpandNode(listDeptId);

            for (int index = 0; index < treeNode.ChildNodes.Count; index++)
            {
                TreeNode node = treeNode.ChildNodes[index];

                if (node.Value.Equals(strClickedNodeId))
                {
                    node.Selected = true;
                    break;
                }
            }
        }

        #endregion

        #region 寻找并展开指定的部门节点 - 2013-03-29 @孙绍棕

        /// <summary>
        /// 寻找并展开指定的部门节点
        /// </summary>
        /// <param name="listDeptId">部门节点路径表</param>
        /// <returns></returns>
        private TreeNode LoopExpandNode(List<String> listDeptId)
        {
            TreeNode treeNode = null;

            foreach (String strDeptId in listDeptId)
            {
                long lngDeptId = long.Parse(strDeptId);

                treeNode = tvDept.SelectedNode;

                if (treeNode != null)
                {
                    for (int index = 0; index < treeNode.ChildNodes.Count; index++)
                    {
                        TreeNode node = treeNode.ChildNodes[index];

                        if (node.Value.Equals(strDeptId))
                        {
                            node.ChildNodes.Clear();

                            treeNode = node;
                            break;
                        }
                    }
                }
                //0 015465 015466 015524 015527
                treeNode = ShowNodeList(lngDeptId, treeNode);
            }


            return treeNode;
        }

        #endregion

        #region 创建部门节点 - 2013-03-29 @孙绍棕

        /// <summary>
        /// 创建部门节点
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private TreeNode CreateNode(DataRowView dr)
        {
            TreeNode childNode = new TreeNode();

            childNode.Text = dr["deptname"].ToString();

            childNode.Value = dr["deptid"].ToString();

            int deptkind = Convert.ToInt32(dr["deptkind"].ToString());
            int istemp = Convert.ToInt32(dr["istemp"].ToString());
            if (deptkind == Convert.ToInt32(eO_DeptKind.eOrg))
            {
                childNode.ImageUrl = RootImg;
            }
            else
            {
                if (istemp == 0)
                {
                    childNode.ImageUrl = NodeImg;
                }
                else
                {
                    childNode.ImageUrl = NodeImg;
                }
            }

            return childNode;
        }

        #endregion

        #region 点击树节点触发事件 - 2013-03-29 @孙绍棕

        /// <summary>
        /// 点击树节点触发事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void tvDept_SelectedNodeChanged(object sender, EventArgs e)
        {
            TreeNode Node = tvDept.SelectedNode;
            string value = Node.Value;
            string deptvalue = Session["deptvaluectr"].ToString();

            if (deptvalue.IndexOf(value) < 0)
            {
                //getDataNode(long.Parse(Node.Value), Node);
                deptvalue = deptvalue + "," + value;
                Session["deptvaluectr"] = deptvalue;

                long lngDeptId = long.Parse(Node.Value);

                Node.ChildNodes.Clear();
                ShowNodeList(lngDeptId, Node);
            }


            if (SelectedChangeEvent != null)
                SelectedChangeEvent(sender, tvDept.SelectedNode);

            Page.RegisterStartupScript("redirect", "<script>redirect(" + long.Parse(Node.Value) + ")</script>");
        }

        #endregion

        #region 点击"+"图标触发节点事件 - 2013-03-29 @孙绍棕

        /// <summary>
        /// 点击"+"图标触发节点事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void tvDept_TreeNodeExpanded(object sender, TreeNodeEventArgs e)
        {
            TreeNode Node = e.Node;
            string value = Node.Value;
            string deptvalue = Session["deptvaluectr"].ToString();

            if (deptvalue.IndexOf(value) < 0)
            {
                //getDataNode(long.Parse(Node.Value), Node);
                deptvalue = deptvalue + "," + value;
                Session["deptvaluectr"] = deptvalue;

                long lngDeptId = long.Parse(Node.Value);

                Node.ChildNodes.Clear();
                ShowNodeList(lngDeptId, Node);
            }

            if (SelectedChangeEvent != null)
                SelectedChangeEvent(sender, tvDept.SelectedNode);
        }

        #endregion
    }
}
