/*******************************************************************
 * 版权所有：
 * Description：分类树展示控件
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
using Epower.DevBase.BaseTools;
using Epower.ITSM.Base;
using Epower.DevBase.Organization.SqlDAL;
using Epower.ITSM.SqlDAL;

namespace Epower.ITSM.Web.Controls
{
    /// <summary>
    ///		CtrCatalogTree 的摘要说明。
    /// </summary>
    public partial class CtrCatalogTree : System.Web.UI.UserControl
    {

        private string strNodeIndex = "0";
        private long lngCurrCatalogID = 0;

        /// <summary>
        /// 显示分类树的时候是否包含本分类及下级分类
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
        /// 当前分类编号
        /// </summary>
        public long CurrCatalogID
        {
            get { return ViewState["CurrCatalogID"] == null ? 0 : StringTool.String2Long(ViewState["CurrCatalogID"].ToString()); }
            set { ViewState["CurrCatalogID"] = value; }
        }


        protected void Page_Load(object sender, System.EventArgs e)
        {
            // 在此处放置用户代码以初始化页面
            if (Page.IsPostBack == false)
            {
                InitTreeView();
            }
        }

        private void InitTreeView()
        {
            long lngRootID = 1;

            if (System.Configuration.ConfigurationSettings.AppSettings["SystemModel"] == "1")
            {
                //如果是租用的方式,只显示所在机构一层的部门树
                lngRootID = long.Parse(Session["UserOrgID"].ToString());
            }

            if (Session["OldCatalogID"] != null)
            {
                lngCurrCatalogID = long.Parse(Session["OldCatalogID"].ToString());
            }

            OCatalogCollection cc = CatalogControl.GetAllCatalogCollection();

            tvCatalog.Nodes.Clear();
            TreeNode root = new TreeNode();
            if (cc.GetOCatalog(lngRootID) != null)	//如果还没有为该机构添加根分类，则提示。
            {
                root.Text = cc.GetOCatalog(lngRootID).Name;
            }
            else
            {
                Response.Write("<script>alert('对不起还没有添加根分类，请联系管理员！');</script>");
                return;
            }
            root.Value = lngRootID.ToString();
            root.ImageUrl = @"..\Images\catalog.ico";
            root.Expanded = true;
            tvCatalog.Nodes.Add(root);

            AddSubCatalogs(ref root, cc, lngRootID, "0.");
            setSelectedExpand();
            tvCatalog.Attributes.Add("onclick", "Tree_Click()");
            tvCatalog.Attributes.Add("ondblclick", "if (typeof(Tree_DBClick) != 'undefined') { Tree_DBClick();}");
        }

        private void setSelectedExpand()
        {

            TreeNode node = tvCatalog.SelectedNode;

            if (node != null)
                SetParentExpand(node);

        }

        private void SetParentExpand(TreeNode node)
        {
            //递归往上 展开当前选中节点的所有父节点
            TreeNode pnode = node.Parent;
            if (pnode != null)
            {
                pnode.Expanded = true;
                SetParentExpand(pnode);
            }

        }

        private void AddSubCatalogs(ref TreeNode root, OCatalogCollection cc, long lngID, string strIndex)
        {
            TreeNode node;
            int iPoint = 0;
            foreach (OCatalog c in cc)
            {
                if (c.ParentID == lngID && c.ID != c.ParentID)
                {
                    //当限制显示当前部门时判断
                    if (c.ID != this.CurrCatalogID || this.LimitCurr == false)
                    {
                        node = new TreeNode();
                        node.Value = c.ID.ToString();
                        node.Text = c.Name;
                        node.Expanded = false;

                        //设置当前选中的节点   yxq 2013-1-29
                        if (c.ID == lngCurrCatalogID)
                        {
                            node.Selected = true;
                        }

                        node.ImageUrl = @"..\Images\left_cata.gif";
                        AddSubCatalogs(ref node, cc, c.ID, strIndex + iPoint.ToString() + ".");
                        root.ChildNodes.Add(node);

                        iPoint++;

                    }
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
    }
}
