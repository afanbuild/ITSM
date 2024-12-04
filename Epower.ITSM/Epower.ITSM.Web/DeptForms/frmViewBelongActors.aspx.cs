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
	/// ViewBelongActors 的摘要说明。
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
							PageTool.MsgBox(this,"对不起，您不能删除该用户组。原因：该用户组由上级管理员指定，请与上级管理员联系。");
						}
					}
					catch(Exception ex)
					{
                        PageTool.MsgBox(this, ex.Message);
					}
			
				}
				else
				{
					PageTool.MsgBox(this,"用户是以部门或条件用户方式加入该用户组，不能单独从该用户组中移除。");
				}
			}
			else
			{
				PageTool.MsgBox(this,"请选择用户组。");
			}
		}


	}
}
