/*
 * by duanqs
 * */
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
using Epower.DevBase.BaseTools;
using Epower.DevBase.Organization.SqlDAL;

namespace Epower.ITSM.Web.Forms
{
	/// <summary>
	/// frmActorCondList 的摘要说明。
	/// </summary>
    public partial class frmActorCondList : BasePage
	{

		protected void Page_Load(object sender, System.EventArgs e)
		{
			CtrTitle.Title="条件人员列表";
			if(!IsPostBack)
			{
				LoadData();
			}
		}
        protected void cmdQuery_Click(object sender, EventArgs e)
        {
            LoadData();
        }

		private void LoadData()
		{

            string sWhere = "";

            if(txtCondID.Text.Trim()!="")
            {
                string condID = txtCondID.Text.Trim();
                sWhere += " and condid like "+StringTool.SqlQ("%"+condID.ToString()+"%");
            }
            if (txtCondName.Text.Trim()!="")
            {
                string condName = txtCondName.Text.Trim();
                sWhere += " and condname like " + StringTool.SqlQ("%"+condName.ToString()+"%");
            }
			long lngSystemID = (long)Session["SystemID"];
			string strRangeID = Session["RangeID"].ToString();

			//DataTable dt=ActorCondDP.GetActorCondList(lngSystemID,strRangeID);
            DataTable dt = ActorCondDP.GetActorCondList(sWhere,lngSystemID,strRangeID);
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


		//删除数据
		protected void cmdDelete_Click(object sender, System.EventArgs e)
		{
			foreach(DataGridItem item in dgActorConds.Items)
			{
				bool isSelect=((CheckBox)item.Cells[2].FindControl("chkSelect")).Checked;
				if(isSelect)
				{
					string sID=item.Cells[1].Text;
					ActorCondEntity ace=new ActorCondEntity();
					ace.CondId=StringTool.String2Long(sID);
					try
					{
						ace.Delete();
						
					}catch(Exception ee)
					{
                        PageTool.MsgBox(this, ee.Message.ToString());
					}		
				}
			}
			LoadData();
		}

		//加载数据
		protected void cmdLoad_Click(object sender, System.EventArgs e)
		{
			LoadData();
		}

		
	}
}
