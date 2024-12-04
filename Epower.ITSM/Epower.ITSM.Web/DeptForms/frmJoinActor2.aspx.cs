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
	/// frmJoinActor ��ժҪ˵����
	/// </summary>
	public partial class frmJoinActor2 : BasePage
	{

		#region Web ������������ɵĴ���
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: �õ����� ASP.NET Web ���������������ġ�
			//
			InitializeComponent();
			base.OnInit(e);
			this.Page.Response.Buffer = false;
			this.Page.Response.Expires = -1;
		}
		
		/// <summary>
		/// �����֧������ķ��� - ��Ҫʹ�ô���༭���޸�
		/// �˷��������ݡ�
		/// </summary>
		private void InitializeComponent()
		{    

		}
		#endregion

		#region Custom Function

		/// <summary>
		/// �����������
		/// </summary>
		/// <param name="ID"></param>
		/// <param name="ActorID"></param>
		/// <param name="ActorType"></param>
		/// <param name="ObjectID"></param>
		private void JoinActor(long ID,long ActorID,eO_ActorObject ActorType,long ObjectID)
		{
			ActorMemberEntity ame=new ActorMemberEntity();
			ame.ID=ID;
			ame.ActorID=ActorID;
			ame.ObjectID=ObjectID;
			ame.ActorType=ActorType;
			ame.RangeID=Session["RangeID"].ToString();
			if(ame.ActorID==0)
			{
				PageTool.MsgBox(this,"δָ����Ӧ�û���,����ʧ�ܡ�");
				return;
			}
			if(ame.ActorType==0 || ame.ObjectID==0)
			{
				PageTool.MsgBox(this,"�������벻����,�����ƺ��ٱ��档");
				return;
			}
			try
			{
				if(ActorMemberEntity.Is_ActorMemeber_Exist(ActorID.ToString(),((int)ActorType).ToString(),ObjectID.ToString()))
				{
					PageTool.AddJavaScript(this,"alert('�Ѵ��ڸ�����'); top.close();");
				}
				else
				{
					ame.Save();

                    if (Request["TypeFrm"] != null && Request["TypeFrm"] == "frmJoinActor_Container")
                    {
                        PageTool.AddJavaScript(this, "alert('�ɹ������û��飡');");
                        Response.Write("<script language=javascript> window.opener.location.reload(); top.close();</script>");


                    }
                    else { 
                       // && Request["frmJoinActor_Container"]==""
					//PageTool.AddJavaScript(this,"alert('�ɹ������û��飡'); window.close();");
                    PageTool.AddJavaScript(this, "alert('�ɹ������û��飡');");
                    Response.Write("<script language=javascript>top.close();</script>");
                    }
				}

			}
			catch(Exception ex)
			{
				PageTool.MsgBox(this,"�����û����Աʱ���ִ���,����Ϊ:\n"+ex.Message.ToString());
			}
			
		}


		private eO_ActorObject getActorType(string ActorType)
		{
			eO_ActorObject eAT;
			switch(ActorType)
			{
				case "10":
					eAT=eO_ActorObject.eDeptActor ; 
					break;

				case "20":
					eAT=eO_ActorObject.eUserActor ; 
					break;

				case "30":
					eAT=eO_ActorObject.eCondUserActor ;
					break;

				default:
					eAT=eO_ActorObject.eUserActor ;
					break;
			}
			return eAT;
		}
		#endregion

		#region Form Event
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// �ڴ˴������û������Գ�ʼ��ҳ��
            if (!IsPostBack)
            {
                CtrActorTree1.Height =  Unit.Parse("460");
                CtrActorTree1.Width = Unit.Parse("100%");
            }
		}

		protected void cmdJoin_Click(object sender, System.EventArgs e)
		{
			long ActorID=StringTool.String2Long(hidActorID.Value);
			eO_ActorObject ActorType=getActorType(Request.QueryString["ActorType"].ToString());
			long lngObjectID=StringTool.String2Long(Request.QueryString["ObjectID"].ToString());

			JoinActor(0,ActorID,ActorType,lngObjectID);
		}
		#endregion

	}
}
