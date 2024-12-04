/****************************************************************************
 * 
 * description:设备类别管理树结构展示
 * 
 * 
 * 
 * Create by:
 * Create Date:2007-08-22
 * *************************************************************************/

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

using Epower.ITSM.SqlDAL;

namespace Epower.ITSM.Web.EquipmentManager
{
	/// <summary>
    ///		ctrSubjecttree 的摘要说明。
	/// </summary>
	/// 
    public partial class ctrSubjecttree : System.Web.UI.UserControl
    {
        #region 变量区
        private string strNodeIndex = "0";
		private long lngCurrSubjectID = 0;
        #endregion 

        #region 属性区
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
		/// 当前科目编号
		/// </summary>
		public long CurrSubjectID
		{
            get { return ViewState["CurrSubjectID"] == null ? 0 : StringTool.String2Long(ViewState["CurrSubjectID"].ToString()); }
            set { ViewState["CurrSubjectID"] = value; }
        }
        #endregion 

        #region 方法区

        #region 页面加载 Page_Load
        /// <summary>
        /// 页面加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, System.EventArgs e)
		{
			if(Page.IsPostBack == false)
			{
				InitTreeView();
			}
        }
        #endregion 

        #region 初始化树 InitTreeView
        /// <summary>
        /// 
        /// </summary>
        private void InitTreeView()
		{
            long lngRootID = 1;
            if (Session["OldEQSubectID"] != null)
            {
                lngCurrSubjectID = long.Parse(Session["OldEQSubectID"].ToString());
            }
            DataTable dt = Equ_SubjectDP.GetSubjects();
            tvSubject.Nodes.Clear();
            TreeNode root = new TreeNode();
            if (Equ_SubjectDP.GetRootSubject().Rows.Count > 0)	//如果还没有为该机构添加根分类，则提示。
            {
                root.Text = Equ_SubjectDP.GetRootSubject().Rows[0]["CatalogName"].ToString();
            }
            else
            {
                Response.Write("<script>alert('对不起还没有添加根类别，请联系管理员！');</script>");
                return;
            }
            root.Value = lngRootID.ToString();
            root.ImageUrl = @"..\Images\catalog.ico";
            root.Expanded = true;
            tvSubject.Nodes.Add(root);

            AddSubSubjects(ref root, dt, lngRootID, "0.");
            
            tvSubject.Attributes.Add("onclick", "Tree_Click()");

        }
        #endregion 

        #region 增加子节点 AddSubSubjects
        /// <summary>
        /// 增加子节点
        /// </summary>
        /// <param name="root"></param>
        /// <param name="dt"></param>
        /// <param name="lngID"></param>
        /// <param name="strIndex"></param>
        private void AddSubSubjects(ref TreeNode root, DataTable dt, long lngID, string strIndex)
		{
			TreeNode node;
            int iPoint = 0;
            long lngParentID = 0;
            long lngSubjectID = 0;
			foreach(DataRow dr in dt.Rows)
			{
                lngParentID = long.Parse(dr["ParentID"].ToString());
                lngSubjectID = long.Parse(dr["CatalogID"].ToString());
                if (lngParentID == lngID && lngSubjectID != lngParentID)
				{
                    if (lngSubjectID != this.CurrSubjectID || this.LimitCurr == false)
                    {
                        node = new TreeNode();
                        node.Value = lngSubjectID.ToString();
                        node.Text = dr["CatalogName"].ToString();
                        node.Expanded = false;

                        node.ImageUrl = @"..\Images\left_cata.gif";
                        AddSubSubjects(ref node, dt, lngSubjectID, strIndex + iPoint.ToString() + ".");
                        root.ChildNodes.Add(node);

                        iPoint++;

                    }
				}
			}
        }
        #endregion

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
        #endregion
    }
}
