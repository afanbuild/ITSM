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
using Epower.DevBase.Organization.Base;
using Epower.DevBase.Organization.SqlDAL;

namespace Epower.ITSM.Web.Forms
{
	/// <summary>
	/// frmEditOperate 的摘要说明。
	/// </summary>
	public partial class frmEditOperate : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.RequiredFieldValidator RequiredFieldValidator1;
		protected System.Web.UI.WebControls.RequiredFieldValidator RequiredFieldValidator2;
		
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if(!IsPostBack)
			{
				if(this.Request.QueryString["operateid"]!=null)
				{
					string sId=this.Request.QueryString["operateid"].ToString();

					LoadOpType();

					LoadData(StringTool.String2Long(sId));
				}
			}
		}

		
		#region 加载数据[LoadOpType,LoadData]
		/// <summary>
		/// 加载操作类别下拉列表
		/// </summary>
		private void LoadOpType()
		{
			ListItem lt;
		//	ListItem lt=new ListItem("","0");
		//	dpdOpType.Items.Add(lt);

			lt=new ListItem("功能类",((int)eO_OperateType.eFunction).ToString());
			dpdOpType.Items.Add(lt);
				
			lt=new ListItem("分析类",((int)eO_OperateType.eQuery).ToString());
			dpdOpType.Items.Add(lt);
		}
		/// <summary>
		/// 加载一个操作的详细资料
		/// </summary>
		/// <param name="lngId"></param>
		private void LoadData(long lngId)
		{
			OperateEntity oe;
			if(lngId==0)
			{
				oe=new OperateEntity();
			}else
			{
				oe=new OperateEntity(lngId);
			}
			labID.Text=oe.OperatrID.ToString();
			txtOpName.Text=oe.OpName;
			dpdOpType.SelectedValue=((int)oe.OperateType).ToString();
			txtSql.Text=oe.SqlStatement;
			txtDesc.Text=oe.OpDesc;
			txtParam.Text=oe.Paramaters;
			txtConnectSystem.Text=oe.ConnectSystem;
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
		/// 设计器支持所需的方法 - 不要使用代码编辑器修改
		/// 此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{    

		}
		#endregion

		/// <summary>
		/// 提交更新
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void cmdSave_Click(object sender, System.EventArgs e)
		{
			OperateEntity oe=new OperateEntity();
			oe.SystemID = (long)Session["SystemID"];
			oe.OpDesc=txtDesc.Text.ToString();
			oe.OperateType=(eO_OperateType)int.Parse(dpdOpType.SelectedValue.ToString());
			oe.OperatrID=StringTool.String2Long(labID.Text.ToString());
			oe.OpName=txtOpName.Text.ToString();
			oe.SqlStatement=txtSql.Text.ToString();
			oe.Paramaters=txtParam.Text.ToString();
			oe.ConnectSystem=txtConnectSystem.Text.ToString();

			try
			{
				oe.Save();
                Response.Write("<script>window.opener.location.href=window.opener.location.href;window.close(); </script>");
			}catch(Exception ee)
			{
				PageTool.MsgBox(this,"保存操作项时出现错误,错误为:\n"+ee.Message.ToString());
			}
			
		}

		

	}
}
