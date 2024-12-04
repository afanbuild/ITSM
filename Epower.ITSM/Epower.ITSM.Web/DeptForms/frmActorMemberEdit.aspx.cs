
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
using Epower.DevBase.Organization;
using Epower.DevBase.Organization.Base;
using Epower.DevBase.Organization.SqlDAL;

namespace Epower.ITSM.Web.Forms
{
	/// <summary>
	/// frmActorMemberEdit ��ժҪ˵����
	/// </summary>
	public partial class frmActorMemberEdit : BasePage
	{
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if(!IsPostBack)
			{
				CtrTitle.Title="�û����Աά��";
				if(this.Request.QueryString["ActorMemberID"]!=null &&
						this.Request.QueryString["ActorID"]!=null)
				{
					string sID=this.Request.QueryString["ActorMemberID"];
					string sActorID=this.Request.QueryString["ActorID"];
					
					LoadActorTypeList();

					LoadData(StringTool.String2Long(sID));
					//�û���ID
					hidActorID.Value=sActorID;

					
				}
			}
		}

		private void LoadActorTypeList()
		{
			ListItem item;
//			ListItem item=new ListItem("","0");
//			dpdActorType.Items.Add(item);

			item=new ListItem("����","10");
			dpdActorType.Items.Add(item);

			item=new ListItem("��Ա","20");
			dpdActorType.Items.Add(item);

			item=new ListItem("������Ա","30");
			dpdActorType.Items.Add(item);

			dpdActorType.Attributes.Add("onchange","ClearValues();");
		}

		private void LoadData(long lngID)
		{
			ActorMemberEntity ame=new ActorMemberEntity(lngID);
			hidID.Value=lngID.ToString();
			hidActorID.Value=ame.ActorID.ToString();
			hidObjectID.Value=ame.ObjectID.ToString();
			dpdActorType.SelectedValue=((int)ame.ActorType).ToString();

			//��ʾ����Name
			long lngObjectID=StringTool.String2Long(ame.ObjectID.ToString());
			eO_ActorObject objType=(eO_ActorObject)int.Parse(dpdActorType.SelectedValue);
			if(objType==eO_ActorObject.eDeptActor)
			{
				DeptEntity de=DeptControl.GetDeptEntity(lngObjectID);
				txtObjectName.Text=de.DeptName;
			}else if(objType==eO_ActorObject.eUserActor)
			{
				txtObjectName.Text=UserDP.GetUserName(lngObjectID);
			}else if(objType==eO_ActorObject.eCondUserActor)
			{
				ActorCondEntity ace=new ActorCondEntity(lngObjectID);
				txtObjectName.Text=ace.CondName;
			}
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

		protected void cmdSave_Click(object sender, System.EventArgs e)
		{
			ActorMemberEntity ame=new ActorMemberEntity();
			ame.ActorID=StringTool.String2Long(hidActorID.Value);
			ame.ID=StringTool.String2Long(hidID.Value);
			ame.ObjectID=StringTool.String2Long(hidObjectID.Value.ToString());
			ame.ActorType=(eO_ActorObject)StringTool.String2Int(dpdActorType.SelectedValue.ToString());
			// ע��ֻ������ʱ�ű�����rangeid
			ame.RangeID = Session["RangeID"].ToString();
			if(ame.ActorID==0)
			{
                PageTool.MsgBox(this, "δָ����Ӧ�û���,����ʧ�ܡ�");
				return;
			}
			if(ame.ActorType==0 || ame.ObjectID==0)
			{
				PageTool.MsgBox(this,"�������벻����,�����ƺ��ٱ��档");
				return;
			}
			try
			{
				if(ActorMemberEntity.Is_ActorMemeber_Exist(ame.ActorID.ToString(),((int)ame.ActorType).ToString(),ame.ObjectID.ToString()))
				{
                    PageTool.MsgBox(this, "��Ա�Ѵ��ڸ�����,����������!");
				}
				else
				{
					ame.Save();
					hidID.Value=ame.ID.ToString();
                    PageTool.AddJavaScript(this, "window.opener.location.href=window.opener.location.href;window.close();");
				}


			}catch(Exception ex)
			{
				PageTool.MsgBox(this,"�����û����Աʱ���ִ���,����Ϊ:\n"+ex.Message.ToString());
			}
			
		}
	}
}
