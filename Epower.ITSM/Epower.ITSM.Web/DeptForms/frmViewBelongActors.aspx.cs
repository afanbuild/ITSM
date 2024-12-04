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
using Epower.DevBase.Organization.Base;

namespace Epower.ITSM.Web.Forms
{
	/// <summary>
	/// ViewBelongActors ��ժҪ˵����
	/// </summary>
	public partial class frmViewBelongActors : BasePage
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if(!this.IsPostBack)
			{
				string strRangeID = Session["RangeID"].ToString();
				long lngSystemID =StringTool.String2Long(Session["SystemID"].ToString());
				long lngUserID = StringTool.String2Long(Request.QueryString["UserID"]);
				hidUserID.Value = lngUserID.ToString();

				string strActors =UserDP.Get_BelongActors(lngSystemID,strRangeID,lngUserID);
				if(strActors.Length>1) 
				{
					strActors = strActors.Substring(0,strActors.Length-1);
					string[] arrayActors = strActors.Split(char.Parse("\n"));

					string[] ActorItem=null;
					foreach(string Actor in arrayActors)
					{
						ActorItem=Actor.Split(char.Parse("^"));
						lsbActors.Items.Add(new ListItem(ActorItem[1],ActorItem[0]));
					}
				}
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

		protected void btnDelete_ServerClick(object sender, System.EventArgs e)
		{
			if(this.lsbActors.SelectedItem!=null)
			{
				long lngRecordID=ActorMemberEntity.Get_RecordID(this.lsbActors.SelectedValue,((int)eO_ActorObject.eUserActor).ToString(),hidUserID.Value);
				
				
				if(lngRecordID>-1)
				{
					try
					{
						if(ActorDP.IsActorInRange(lngRecordID,Session["RangeID"].ToString()))
						{

							ActorMemberEntity ame=new ActorMemberEntity();
							ame.ID=lngRecordID;
							ame.Delete();
							this.lsbActors.Items.Remove(this.lsbActors.SelectedItem);
						}
						else
						{
							PageTool.MsgBox(this,"�Բ���������ɾ�����û��顣ԭ�򣺸��û������ϼ�����Աָ���������ϼ�����Ա��ϵ��");
						}
					}
					catch(Exception ex)
					{
                        PageTool.MsgBox(this, ex.Message);
					}
			
				}
				else
				{
					PageTool.MsgBox(this,"�û����Բ��Ż������û���ʽ������û��飬���ܵ����Ӹ��û������Ƴ���");
				}
			}
			else
			{
				PageTool.MsgBox(this,"��ѡ���û��顣");
			}
		}


	}
}
