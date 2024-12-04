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

namespace Epower.ITSM.Web.Forms
{
	/// <summary>
	/// PersonList ��ժҪ˵����
	/// </summary>
	public partial class PersonList : BasePage
	{
	
		long lngAppID=0;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// �ڴ˴������û������Գ�ʼ��ҳ��
			
						
		
			
			long lngDeptID = 1;   //������ID

			if(Request.QueryString["DeptID"] != null)
			{
				lngDeptID = long.Parse(Request.QueryString["DeptID"]);
				//������װ�� ͨ���ݴ��ڲ����б�ҳ��� �������ݣ���������ΪSESSIONֵ������Ϊ������ҳ��򿪶��ı䣡�������������б�ҳ��û�з�����������
				lngAppID = long.Parse(Request.QueryString["AppID"]);	

			}
			else
			{
				//��һ��װ��ʱͨ��SESSIONȡ Ӧ��ID
                if (Session["OldDeptID"] != null && Session["OldDeptID"].ToString()!="")
                    lngDeptID = long.Parse(Session["OldDeptID"].ToString());
				lngAppID = long.Parse(Session["AppID"].ToString());
			}
			ViewState["AppID"] = lngAppID;
			if(Page.IsPostBack == false)
			{
				ActorCollection ac = Miscellany.GetMasterActorsForDept(lngDeptID);
				lstPerson.Items.Clear();
				foreach (Actor a in ac)
				{
					if(a.ID >= 1000)
					{
						//ϵͳԤ����ı�Ų���ʾ����
						lstPerson.Items.Add(new ListItem(a.Name,a.ID.ToString()));
					}
				}
			}
			else
			{
				lngAppID = (long)ViewState["AppID"];
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

		protected void Button1_Click(object sender, System.EventArgs e)
		{
			long lngAgentID =0;
			if(lstPerson.SelectedIndex >= 0)
				lngAgentID = long.Parse(lstPerson.SelectedItem.Value);
			if(lngAgentID !=0)
			{
				Server.Transfer("form_Agent_Change_submit.aspx?AppID=" + lngAppID.ToString() + "&AgentID=" + lngAgentID.ToString());
			}
			else
			{
				//UIGlobal.MsgBox(this,"��ѡ��������Ա");
			}
		}
	}
}
