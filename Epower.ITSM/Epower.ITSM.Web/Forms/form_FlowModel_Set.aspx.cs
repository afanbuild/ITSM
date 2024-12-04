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
using EpowerCom;
using EpowerGlobal;

namespace Epower.ITSM.Web.Forms
{
	/// <summary>
	/// form_FlowModel_Set 的摘要说明。
	/// </summary>
	/// 

    public partial class form_FlowModel_Set : BasePage
	{



		private long lngUserID=0;
		
		protected bool GetCheckedValue(int i)
		{
			return (i==1);
		}
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// 在此处放置用户代码以初始化页面
			lngUserID = (long)Session["UserID"];
			//lngUserID = 1025;

			CtrTitle1.Title = "常用流程设置";

			if(!Page.IsPostBack)
			{
				DataSet ds = FlowModel.GetAllCanStartFlowModelsSet(lngUserID);

				grdFlowModel.DataSource = ds.Tables[0].DefaultView;
				grdFlowModel.DataBind();
			}
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN：该调用是 ASP.NET Web 窗体设计器所必需的。
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

		protected void btnOK_Click(object sender, System.EventArgs e)
		{
			System.Web.UI.WebControls.CheckBox chk;
			string strIDList="";

			foreach (DataGridItem di in grdFlowModel.Items)
			{
				chk = (CheckBox)di.FindControl("CheckBox1");

				if(chk.Checked==true && di.Cells[4].Text.Trim() != "")
				{
					strIDList = strIDList +  "," + di.Cells[4].Text ;
				}
			}

			if (strIDList.StartsWith(","))
				strIDList = strIDList.Substring(1);
			
			//配置
			FlowModel.UserFlowModelSet(lngUserID,strIDList);
            Epower.DevBase.BaseTools.PageTool.MsgBox(this, "保存成功！");

			//Response.Write("<script>window.parent.leftFrame.location.reload();</script>");



		}
	}
}
