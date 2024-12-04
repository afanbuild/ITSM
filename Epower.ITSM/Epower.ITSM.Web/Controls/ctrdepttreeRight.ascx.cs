/*******************************************************************
 * 版权所有：
 * Description：部门树权限控件
 * Create By  ：yanghw
 * Create Date：2011-07-22
 * *****************************************************************/
namespace Epower.ITSM.Web.Controls
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using Epower.DevBase.Organization;
	using Epower.DevBase.Organization.Base;
	//using Microsoft.Web.UI.WebControls;
	using Epower.DevBase.BaseTools;
    using Epower.DevBase.Organization.SqlDAL;
	/// <summary>
	///		CtrDeptTree 的摘要说明。
	/// </summary>
	/// 

    public partial class ctrdepttreeRight : System.Web.UI.UserControl
	{

		private string strNodeIndex = "0";
		private long lngCurrDeptID = 0;


		/// <summary>
		/// 设置树的高度
		/// </summary>
		public System.Web.UI.WebControls.Unit TreeHeight
		{
			set{tvDept.Height=value;}
		}


        public long Right
        {
            get {
                return ViewState["Right"] == null ? 0 : long.Parse(ViewState["Right"].ToString());
            }
            set
            {
                ViewState["Right"] = value;
            }
        }


		/// <summary>
		/// 显示部门树的时候是否包含本部门及下级部门
		/// </summary>
		public bool LimitCurr
		{
			get
			{
				return ViewState["LimitCurr"]==null?false:bool.Parse(ViewState["LimitCurr"].ToString());
			}
			set{ViewState["LimitCurr"]=value;}
		}

        /// <summary>
        /// 显示部门树的时候是否包含本部门及下级部门
        /// </summary>
        public bool LimitOrg
        {
            get
            {
                return ViewState["LimitOrg"] == null ? false : bool.Parse(ViewState["LimitOrg"].ToString());
            }
            set { ViewState["LimitOrg"] = value; }
        }

		/// <summary>
		/// 当前部门编号
		/// </summary>
		public long CurrOrgID
		{
            get { return ViewState["CurrOrgID"] == null ? 0 : StringTool.String2Long(ViewState["CurrOrgID"].ToString()); }
            set { ViewState["CurrOrgID"] = value; }
		}


        /// <summary>
        /// 当前部门编号
        /// </summary>
        public long CurrDeptID
        {
            get { return ViewState["CurrDeptID"] == null ? 0 : StringTool.String2Long(ViewState["CurrDeptID"].ToString()); }
            set { ViewState["CurrDeptID"] = value; }
        }


		protected void Page_Load(object sender, System.EventArgs e)
		{
			if(Page.IsPostBack == false)
			{
				InitTreeView();
			}
		}
		private void InitTreeView()
		{
			long lngRootID = 1;

			if( System.Configuration.ConfigurationSettings.AppSettings["SystemModel"] == "1")
			{
				//如果是租用的方式,只显示所在机构一层的部门树
				lngRootID = long.Parse(Session["UserOrgID"].ToString());
			}

			//long lngRootID = (long)Session["UserDeptID"];
			if(Session["OldDeptID"]!= null)
			{
				lngCurrDeptID = long.Parse(Session["OldDeptID"].ToString());				
			}
			else
			{
				lngCurrDeptID =long.Parse(Session["UserDeptID"].ToString());
			}

            RightEntity nRightEntity = (RightEntity)((System.Collections.Hashtable)Session["UserAllRights"])[Right];
            if (nRightEntity.RightRange == eO_RightRange.ePersonal || nRightEntity.RightRange == eO_RightRange.eDeptDirect)
            {
                //所在部门 或 个人权限
                lngRootID = long.Parse(Session["UserDeptID"].ToString());
                LimitCurr = true;
            }
            else if (nRightEntity.RightRange == eO_RightRange.eDept)
            {
                //所属部门
                lngRootID = long.Parse(Session["UserDeptID"].ToString());
                LimitCurr = false;
            }
            else if (nRightEntity.RightRange == eO_RightRange.eOrgDirect)//所在机构
            {
                lngRootID = long.Parse(Session["UserOrgID"].ToString());
                LimitOrg = true;
            }
            else if (nRightEntity.RightRange == eO_RightRange.eOrg)//所属机构
            {
                lngRootID = long.Parse(Session["UserOrgID"].ToString());
                LimitOrg = false;
            }
            if (lngRootID == 0)
            {
                return;
            }

            ODeptCollection dc = DeptControl.GetAllDeptCollection();
			

			tvDept.Nodes.Clear();
			TreeNode root = new TreeNode();
			root.Text = dc.GetODept(lngRootID).Name;            
			root.Value = lngRootID.ToString();
			//root.ImageUrl = @"..\Images\flow_modify.ico";
            if (dc.GetODept(lngRootID).DeptKind != eO_DeptKind.eOrg)
            {   
                root.ImageUrl = @"..\Images\FlowDesign\333.ico";
            }
            else
            {
                root.ImageUrl = @"..\Images\Flow\1.bmp";
            }
			
			root.Expanded = true;
			tvDept.Nodes.Add(root);

			AddSubDepts(ref root,dc,lngRootID,"0.");
			
            tvDept.Attributes.Add("onclick", "if (typeof(Tree_Click) != 'undefined') { Tree_Click();}");
            tvDept.Attributes.Add("ondblclick", "if (typeof(Tree_DBClick) != 'undefined') { Tree_DBClick();}");


		}

		private void AddSubDepts(ref TreeNode root,ODeptCollection dc,long lngID,string strIndex)
		{
			TreeNode node;
			int iPoint = 0;
			foreach(ODept d in dc)
			{
                if (d.ParentID == lngID && d.ID != d.ParentID)
                {

                    RightEntity nRightEntity = (RightEntity)((System.Collections.Hashtable)Session["UserAllRights"])[Right];
                    if (nRightEntity.RightRange == eO_RightRange.ePersonal || nRightEntity.RightRange == eO_RightRange.eDeptDirect || nRightEntity.RightRange == eO_RightRange.eDept)
                    {
                        #region  
                        //当限制显示当前部门时判断
                        if ((d.ID != this.CurrDeptID && this.LimitCurr == false))
                        {

                            node = new TreeNode();
                            node.Value = d.ID.ToString();
                            node.Text = d.Name;
                            node.Expanded = false;

                            if (d.DeptKind != eO_DeptKind.eOrg)
                            {
                                node.ImageUrl = @"..\Images\FlowDesign\333.ico";
                            }
                            else
                            {
                                node.ImageUrl = @"..\Images\Flow\1.bmp";
                            }
                            AddSubDepts(ref node, dc, d.ID, strIndex + iPoint.ToString() + ".");
                            root.ChildNodes.Add(node);

                            iPoint++;

                        }
                        #endregion 
                    }
                    else if (nRightEntity.RightRange == eO_RightRange.eOrgDirect)
                    {
                        //所在机构
                        #region 所在机构
                        if (d.OrgID == this.CurrOrgID && d.ID != this.CurrOrgID && d.DeptKind != eO_DeptKind.eOrg)
                        {
                            node = new TreeNode();
                            node.Value = d.ID.ToString();
                            node.Text = d.Name;
                            node.Expanded = false;

                            if (d.DeptKind != eO_DeptKind.eOrg)
                            {
                                node.ImageUrl = @"..\Images\FlowDesign\333.ico";
                            }
                            else
                            {
                                node.ImageUrl = @"..\Images\Flow\1.bmp";
                            }
                            AddSubDepts(ref node, dc, d.ID, strIndex + iPoint.ToString() + ".");
                            root.ChildNodes.Add(node);

                            iPoint++;

                        }
                        #endregion 
                    }
                    else if (nRightEntity.RightRange == eO_RightRange.eOrg)
                    {
                        //所属于机构
                        #region 属于机构
                        if (d.ID != this.CurrOrgID && LimitOrg == false)
                        {
                            node = new TreeNode();
                            node.Value = d.ID.ToString();
                            node.Text = d.Name;
                            node.Expanded = false;

                            if (d.DeptKind != eO_DeptKind.eOrg)
                            {
                                node.ImageUrl = @"..\Images\FlowDesign\333.ico";
                            }
                            else
                            {
                                node.ImageUrl = @"..\Images\Flow\1.bmp";
                            }
                            AddSubDepts(ref node, dc, d.ID, strIndex + iPoint.ToString() + ".");
                            root.ChildNodes.Add(node);

                            iPoint++;
                        }
                        #endregion 
                    }
                    else if (nRightEntity.RightRange == eO_RightRange.eFull)
                    {
                        #region 全局
                        node = new TreeNode();
                        node.Value = d.ID.ToString();
                        node.Text = d.Name;
                        node.Expanded = false;

                        if (d.DeptKind != eO_DeptKind.eOrg)
                        {
                            node.ImageUrl = @"..\Images\FlowDesign\333.ico";
                        }
                        else
                        {
                            node.ImageUrl = @"..\Images\Flow\1.bmp";
                        }
                        AddSubDepts(ref node, dc, d.ID, strIndex + iPoint.ToString() + ".");
                        root.ChildNodes.Add(node);

                        iPoint++;
                        #endregion 
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
