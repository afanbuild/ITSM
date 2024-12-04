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
using Epower.DevBase.Organization.Base;
namespace Epower.ITSM.Web.Forms
{
	/// <summary>
	/// frmActorInfo ��ժҪ˵����
	/// </summary>
	public partial class frmActorInfo : BasePage
	{
		long actorId;

		protected bool GetVisible(int i)
		{
			bool t = false;
			if( i == (int)eO_ActorObject.eCondUserActor ) 
				t= true;
			return t;
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			CtrTitle.Title="��ɫ��Աά��";
			btnDel.Attributes.Add("onclick", "if(confirm('�Ƿ����Ҫɾ����')){return true;}else{return false;}");
			actorId = Convert.ToInt32(Request.Params.Get("actorId"));
			hidActorID.Value=actorId.ToString();
			if(!IsPostBack)
			{
				LoadData();
				BindData();
			}
			ControlActorMembers.DataGridToControl = dgUserInfo;
		}

		private void LoadData()
		{
			DataTable dt = ActorControl.GetActorList(actorId);
			Session["ACTORMEMBER_DATA"] = dt;
		}

		private void BindData()
		{
			this.dgUserInfo.DataSource = ((DataTable)Session["ACTORMEMBER_DATA"]).DefaultView;
			this.dgUserInfo.DataBind();
		}

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

		protected void btnDel_Click(object sender, System.EventArgs e)
		{
			ArrayList MyList;
			int result = 0;
			
			for (int i=0; i<Request.Form.Count; i++)
			{		
				string requestname = Request.Form.GetKey(i).ToString();

				if (requestname.StartsWith("Chk"))
				{
					result=1;
					MyList = new ArrayList();
					MyList.Add(requestname.Remove(0,3).ToString());		
					for (int j=0; j<MyList.Count; j++)
					{
						string actorid=MyList[j].ToString();
						UserControls.DelUserInfo(Convert.ToInt32(actorid));
					}					
				}
				
			}
			if(result==0)
			{
				Response.Write("<script>alert('����ѡ��Ҫɾ������Ϣ��');</script>");
			}
			
			BindData();
		}

		private void ControlActorMembers_OnPostBack(object sender, EventArgs e)
		{
			LoadData();
			BindData();
		}

		private void cmdAdd_Click(object sender, System.EventArgs e)
		{
			
		}
	}
}
