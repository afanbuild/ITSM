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
using Epower.DevBase.Organization;
using Epower.DevBase.BaseTools;
using Epower.DevBase.Organization.Base;
using Epower.DevBase.Organization.SqlDAL;

namespace Epower.ITSM.Web.Forms
{
	/// <summary>
	/// frmOperates ��ժҪ˵����
	/// </summary>
	public partial class frmOperates : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.Button cmdAdd;

		protected bool GetVisible(int i)
		{
			bool t = false;
			if(i>10000) 
				t= true;
			return t;
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			cmdDelete.Attributes.Add("onclick", "if(!confirm('ɾ�����޷��ָ�,ȷ��Ҫɾ����')){return false;}");
			if(!IsPostBack)
			{
				uTitle.Title="������ά��";

				LoadOperateType();

				LoadData();
				BindData();
			}

			ControlPageOperate.On_PostBack +=new EventHandler(ControlPageOperate_On_PostBack);
			//�趨��ҳ�ؼ�����
			ControlPageOperate.DataGridToControl=dgOperate;
		}
		
		private void LoadOperateType()
		{
			ListItem lt=new ListItem("������",((int)eO_OperateType.eFunction).ToString());
			dpdOpType.Items.Add(lt);
				
			lt=new ListItem("������",((int)eO_OperateType.eQuery).ToString());
			dpdOpType.Items.Add(lt);

			lt=new ListItem("ȫ��","0");
			dpdOpType.Items.Insert(0,lt);		
		}

		#region ���ز�ѯ����

		protected void cmdFind_Click(object sender, System.EventArgs e)
		{
			LoadData();
			BindData();
		}

		//�����ݵ�����
		private void BindData()
		{
			dgOperate.DataSource=((DataTable)Session["OPERATE_EDIT"]).DefaultView;
			dgOperate.DataBind();

			
		}

		//�������ݲ�����
		private void LoadData()
		{
			DataTable dt=new DataTable();
			long lngOPID = 0;
			long lngSystemID = (long)Session["SystemID"];
			if(txtOpID.Text.Trim()=="" && dpdOpType.SelectedValue=="0")
			{
				dt=OperateControl.GetAllOperate(lngSystemID);
			}

			if(txtOpID.Text.Trim()!="")
			{
				try
				{
					lngOPID = long.Parse(txtOpID.Text.ToString());
				}
				catch
				{
					lngOPID = 0;
				}
				dt=OperateControl.GetOperateByOperateID(lngOPID,lngSystemID);
			}

			if(dpdOpType.SelectedValue!="0")
			{
				dt=OperateControl.GetOperateByOperateType((eO_OperateType)(int.Parse(dpdOpType.SelectedValue)),lngSystemID);
			}
			Session["OPERATE_EDIT"]=dt;
		}

		#endregion
		
		#region ɾ��

		

		//ɾ��һ������
		protected void cmdDelete_Click(object sender, System.EventArgs e)
		{
			foreach(DataGridItem item in dgOperate.Items)
			{
				if(((CheckBox)item.Cells[0].FindControl("chkSelect")).Checked)
				{
					string sId=((Label)item.Cells[2].FindControl("labOperateID")).Text.ToString();
					OperateEntity oe=new OperateEntity();
					oe.OperatrID=long.Parse(sId);
					oe.Delete();
				}
			}
			LoadData();
			BindData();
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

		private void ControlPageOperate_On_PostBack(object sender, EventArgs e)
		{
			LoadData();
			BindData();
		}
	}
}
