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
	/// form_FlowModel_Set ��ժҪ˵����
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
			// �ڴ˴������û������Գ�ʼ��ҳ��
			lngUserID = (long)Session["UserID"];
			//lngUserID = 1025;

			CtrTitle1.Title = "������������";

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
			// CODEGEN���õ����� ASP.NET Web ���������������ġ�
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
			
			//����
			FlowModel.UserFlowModelSet(lngUserID,strIDList);
            Epower.DevBase.BaseTools.PageTool.MsgBox(this, "����ɹ���");

			//Response.Write("<script>window.parent.leftFrame.location.reload();</script>");



		}
	}
}
