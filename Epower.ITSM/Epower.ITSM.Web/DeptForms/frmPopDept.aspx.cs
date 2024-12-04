using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Epower.DevBase.Organization.SqlDAL;
using Epower.DevBase.Organization.Base;

namespace Epower.ITSM.Web.Forms
{
	/// <summary>
	/// frmPopDept 的摘要说明。
	/// </summary>
	public partial class frmPopDept : BasePage
	{
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


        private long mlngCurrDeptID = 1;
        protected long lngCurrDeptID
        {
            get
            {
                if (Request.QueryString["CurrDeptID"] != null)
                {
                    if (Request.QueryString["CurrDeptID"].Length > 0)
                        mlngCurrDeptID = long.Parse(Request.QueryString["CurrDeptID"]);
                    else
                        mlngCurrDeptID = long.Parse(Session["UserDeptID"].ToString());
                }
                else
                {
                    mlngCurrDeptID = long.Parse(Session["UserDeptID"].ToString());
                }
                return mlngCurrDeptID;
            }
        }
        /// <summary>
        /// 获得传过来的参数判断哪个父页面传过来的。
        /// </summary>
        public string TypeFrm
        {
            get
            {

                if (Request.QueryString["TypeFrm"] != null)
                {
                    return Request.QueryString["TypeFrm"];
                }
                else
                {
                    return "";
                }
            }
        }
        /// <summary>
        /// 获得传过来的参数用来判断父页面的id名称
        /// </summary>
        public string Opener_ClientId
        {

            get
            {
                return (Request["Opener_ClientId"] == null) ? "" : Request["Opener_ClientId"].ToString().Replace("hidClientId_ForOpenerPage", "");
            }
        }
        /// <summary>
        /// 获取父页面传过来的按钮id
        /// </summary>
        public string ObjID
        {
            get { return String.IsNullOrEmpty(Request["objID"]) ? "" : Request["objID"].ToString(); }
        }


		protected void Page_Load(object sender, System.EventArgs e)
		{
            CtrDeptTree.SelectedChangeEvent += new Epower.ITSM.Web.DeptControls.CtrDeptTree.SelectedChangeHandler(CtrDeptTree_SelectedChangeEvent);

            if (!Page.IsPostBack)
            {
                Session["deptvalue"] = "1";
                LoadData_new();
            }
		}

        void CtrDeptTree_SelectedChangeEvent(object sender, TreeNode node)
        {
            if (node != null && node.Value != null)
            {
                hidDeptID.Value = node.Value + "@" + node.Text;
                deptname.Text = "选择的部门：" + node.Text;
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
		/// 设计器支持所需的方法 - 不要使用代码编辑器修改
		/// 此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{    

		}
		#endregion




        private void LoadData_new()
        {


            try
            {
                //DataTable table = new DataTable();
                //table = DeptDP.Get_One_Depts(-1);       //得到所有所有父节点，放到DataTable中，这里默认根节点的父节为0
                //BindNode_noe(table, tvDept.Nodes);   //绑定所有的父节点     
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        //private void BindNode_noe(DataTable table, TreeNodeCollection node)
        //{
        //    DataView dv = new DataView(table);
        //    TreeNode NewNode;
        //    foreach (DataRowView dr in dv)
        //    {
        //        NewNode = new TreeNode();

        //        NewNode.Text = dr["deptname"].ToString();
        //        NewNode.Value = dr["deptid"].ToString();
        //        int deptkind = Convert.ToInt32(dr["deptkind"].ToString());
        //        int istemp = Convert.ToInt32(dr["istemp"].ToString());
        //        if (deptkind == Convert.ToInt32(eO_DeptKind.eOrg))
        //        {
        //            NewNode.ImageUrl = RootImg;

        //        }
        //        else
        //        {
        //            if (istemp == 0)
        //            {
        //                NewNode.ImageUrl = NodeImg;
        //            }
        //            else
        //            {
        //                NewNode.ImageUrl = NodeImg;
        //            }
        //        }

        //        node.Add(NewNode);

        //        #region 二级菜单
        //        //查找第二级的菜单
        //        long lngRootID = long.Parse(Session["RootDeptID"].ToString());
        //        DataTable table1 = new DataTable();
        //        table1 = DeptDP.Get_One_Depts(lngRootID);
        //        DataView dv1 = new DataView(table1);
        //        TreeNode NewNode1;
        //        foreach (DataRowView dr1 in dv1)
        //        {
        //            NewNode1 = new TreeNode();

        //            NewNode1.Text = dr1["deptname"].ToString();
        //            NewNode1.Value = dr1["deptid"].ToString();
        //            int deptkind1 = Convert.ToInt32(dr1["deptkind"].ToString());
        //            int istemp1 = Convert.ToInt32(dr1["istemp"].ToString());
        //            if (deptkind1 == Convert.ToInt32(eO_DeptKind.eOrg))
        //            {
        //                NewNode1.ImageUrl = RootImg;

        //            }
        //            else
        //            {
        //                if (istemp1 == 0)
        //                {
        //                    NewNode1.ImageUrl = NodeImg;
        //                }
        //                else
        //                {
        //                    NewNode1.ImageUrl = NodeImg;
        //                }
        //            }

        //            NewNode.ChildNodes.Add(NewNode1);
        //        }
        //        #endregion
        //    }
        //}

        //private void BindNode(DataTable table, TreeNodeCollection node)
        //{
        //    DataView dv = new DataView(table);
        //    TreeNode NewNode;
        //    foreach (DataRowView dr in dv)
        //    {
        //        NewNode = new TreeNode();

        //        NewNode.Text = dr["deptname"].ToString();
        //        NewNode.Value = dr["deptid"].ToString();
        //        int deptkind = Convert.ToInt32(dr["deptkind"].ToString());
        //        int istemp = Convert.ToInt32(dr["istemp"].ToString());
        //        if (deptkind == Convert.ToInt32(eO_DeptKind.eOrg))
        //        {
        //            NewNode.ImageUrl = RootImg;

        //        }
        //        else
        //        {
        //            if (istemp == 0)
        //            {
        //                NewNode.ImageUrl = NodeImg;
        //            }
        //            else
        //            {
        //                NewNode.ImageUrl = NodeImg;
        //            }
        //        }
        //        node.Add(NewNode);
        //    }
        //}

        //private void getDataNode(long ParentId, TreeNode Node)
        //{
        //    BindNode(DeptDP.Get_One_Depts(ParentId), Node.ChildNodes);      //向结点填充数据.
        //}

        //protected void tvDept_SelectedNodeChanged(object sender, EventArgs e)
        //{

        //    TreeNode Node = tvDept.SelectedNode;
        //    string value = Node.Value;
        //    string deptvalue = Session["deptvalue"].ToString();

        //    if (deptvalue.IndexOf(value) < 0)
        //    {
        //        getDataNode(long.Parse(Node.Value), Node);
        //        deptvalue = deptvalue + "," + value;
        //        Session["deptvalue"] = deptvalue;
        //    }

        //    hidDeptID.Value = Node.Value + "@" + Node.Text;
        //    deptname.Text = "选择的部门：" + Node.Text;
        //}
        

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            Response.Write(hidDeptID.Value );
        }
	}
}
