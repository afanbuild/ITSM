/*******************************************************************
 * 版权所有：
 * Description：分类树展示控件
 * 
 * 
 * Create By  ： yxq
 * Create Date： 2011-08-10
 * *****************************************************************/
using System;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Epower.DevBase.BaseTools;
using Epower.ITSM.Base;
using Microsoft.Web.UI.WebControls;
using Epower.DevBase.Organization.SqlDAL;
using Epower.ITSM.SqlDAL;

namespace Epower.ITSM.Web.Controls
{
	/// <summary>
	///		CtrCatalogTree 的摘要说明。
	/// </summary>
    public partial class ctrcatalogtreeNew : System.Web.UI.UserControl
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

        public long lngRootID
        {
            get
            {
                return ViewState["CurrlngRootID"] == null ? 0 : StringTool.String2Long(ViewState["CurrlngRootID"].ToString()); 
            }
            set {
                ViewState["CurrlngRootID"] = value;
            }
        }

		protected void Page_Load(object sender, System.EventArgs e)
		{
			// 在此处放置用户代码以初始化页面
			if(Page.IsPostBack == false)
			{
                if (Request.QueryString["RootID"] != null)
                {
                    lngRootID = long.Parse(Request.QueryString["RootID"] == "" ? "0" : Request.QueryString["RootID"]);
                }
				InitTreeView();
			}
		}

		private void InitTreeView()
		{
            OCatalogCollection cc = CatalogControl.GetAllCatalogCollectionbyRooID(lngRootID);

			tvCatalog.Nodes.Clear();
			Microsoft.Web.UI.WebControls.TreeNode root = new Microsoft.Web.UI.WebControls.TreeNode();
			if(cc.GetOCatalog(lngRootID) != null)	//如果还没有为该机构添加根分类，则提示。
			{
				root.Text = cc.GetOCatalog(lngRootID).Name;
			}
			else
			{
				Response.Write("<script>alert('对不起还没有添加根分类，请联系管理员！');</script>");
				return;
			}
			root.ID = lngRootID.ToString();
			//root.ImageUrl = @"..\Images\flow_modify.ico";
			root.ImageUrl = @"..\Images\catalog.ico";
			root.Expanded = true;
			tvCatalog.Nodes.Add(root);

			if(lngCurrCatalogID == lngRootID)
			{
				strNodeIndex = root.GetNodeIndex();
			}

            AddSubCatalogs(ref root, cc, lngRootID, "0.");
			setSelectedExpand();
			tvCatalog.Attributes.Add("onclick","Tree_Click()");
            tvCatalog.Attributes.Add("ondblclick", "if (typeof(Tree_DBClick) != 'undefined') { Tree_DBClick();}");
		}

		private void setSelectedExpand()
		{
			//发生错误保持焦点在根节点
			try
			{
				Microsoft.Web.UI.WebControls.TreeNode node = tvCatalog.GetNodeFromIndex(strNodeIndex);
				SetParentExpand(node);
				node.Expanded = true;

				tvCatalog.SelectedNodeIndex = strNodeIndex;
			}
			catch
			{
			}
			
		}

		private void SetParentExpand(Microsoft.Web.UI.WebControls.TreeNode node)
		{
			//string strCurrIndex = node.GetNodeIndex();
			string strIndex = ((Microsoft.Web.UI.WebControls.TreeNode)node.Parent).GetNodeIndex();
			if(strIndex != "0")
			{
				Microsoft.Web.UI.WebControls.TreeNode pnode = (Microsoft.Web.UI.WebControls.TreeNode)node.Parent;
				SetParentExpand(pnode);
				//Microsoft.Web.UI.WebControls.TreeNode tnode = tvCatalog.GetNodeFromIndex(strCurrIndex);
				pnode.Expanded = true;
			}
		}

        private void AddSubCatalogs(ref Microsoft.Web.UI.WebControls.TreeNode root, OCatalogCollection cc, long lngID, string strIndex)
        {
            Microsoft.Web.UI.WebControls.TreeNode node;
            int iPoint = 0;
            foreach (OCatalog c in cc)
            {
                if (c.ParentID == lngID && c.ID != c.ParentID)
                {
                    //当限制显示当前部门时判断
                    if (c.ID != this.CurrCatalogID || this.LimitCurr == false)
                    {
                        node = new Microsoft.Web.UI.WebControls.TreeNode();
                        node.ID = c.ID.ToString();
                        node.Text = c.Name;
                        node.Expanded = false;

                        node.ImageUrl = @"..\Images\left_data.gif";
                        AddSubCatalogs(ref node, cc, c.ID, strIndex + iPoint.ToString() + ".");
                        root.Nodes.Add(node);



                        if (c.ID == lngCurrCatalogID)
                        {
                            strNodeIndex = node.GetNodeIndex();
                            if (strNodeIndex.Length == 0)
                            {
                                strNodeIndex = strIndex + iPoint.ToString();
                            }
                        }

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
