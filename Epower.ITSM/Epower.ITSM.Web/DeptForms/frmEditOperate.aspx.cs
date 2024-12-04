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
	/// frmEditOperate ��ժҪ˵����
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

		
		#region ��������[LoadOpType,LoadData]
		/// <summary>
		/// ���ز�����������б�
		/// </summary>
		private void LoadOpType()
		{
			ListItem lt;
		//	ListItem lt=new ListItem("","0");
		//	dpdOpType.Items.Add(lt);

			lt=new ListItem("������",((int)eO_OperateType.eFunction).ToString());
			dpdOpType.Items.Add(lt);
				
			lt=new ListItem("������",((int)eO_OperateType.eQuery).ToString());
			dpdOpType.Items.Add(lt);
		}
		/// <summary>
		/// ����һ����������ϸ����
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


		#region Web ������������ɵĴ���
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: �õ����� ASP.NET Web ���������������ġ�
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// �����֧������ķ��� - ��Ҫʹ�ô���༭���޸�
		/// �˷��������ݡ�
		/// </summary>
		private void InitializeComponent()
		{    

		}
		#endregion

		/// <summary>
		/// �ύ����
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
				PageTool.MsgBox(this,"���������ʱ���ִ���,����Ϊ:\n"+ee.Message.ToString());
			}
			
		}

		

	}
}
