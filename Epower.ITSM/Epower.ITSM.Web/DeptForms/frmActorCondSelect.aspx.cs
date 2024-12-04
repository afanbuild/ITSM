/*
 *	by duanqs
 */
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

namespace Epower.ITSM.Web.Forms
{
	/// <summary>
	/// frmActorCondSelect 的摘要说明。
	/// </summary>
    public partial class frmActorCondSelect : BasePage
	{
		//protected Epower.ITSM.Web.Controls.CtrTitle CtrTitle;

		protected void Page_Load(object sender, System.EventArgs e)
		{
			//CtrTitle.Title="用户组条件列表";
			if(!IsPostBack)
			{
				LoadData();
			}
		}

        #region 获得传过来的参数用来判断父窗体的控件id的名称的相同部分
        /// <summary>
        /// 获得传过来的参数用来判断父窗体的控件id的名称的相同部分
        /// </summary>
        public string Opener_ClientId
        {

            get
            {
                return (Request["Opener_ClientId"] == null) ? "" : Request["Opener_ClientId"].ToString().Replace("hidClientId_ForOpenerPage", "");
            }
        }
        #endregion

		private void LoadData()
		{
			long lngSystemID = (long)Session["SystemID"];
			string strRangeID = Session["RangeID"].ToString();

			DataTable dt=ActorCondDP.GetActorCondList(lngSystemID,strRangeID);
			dgActorConds.DataSource=dt.DefaultView;
			
			dgActorConds.DataBind();
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void dgActorConds_ItemCommand(object source, DataGridCommandEventArgs e)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dgActorConds_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Label labCondID = (Label)e.Item.FindControl("labCondID");
                Label labCondName = (Label)e.Item.FindControl("labCondName");
                e.Item.Attributes.Add("ondblclick", "DbSelectclick('" + labCondID.Text.Trim() + "','" + labCondName.Text.Trim()+ "')");
            }
        }
	}
}
