
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
	/// frmActorMemberEdit 的摘要说明。
	/// </summary>
	public partial class frmActorMemberEdit : BasePage
	{
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if(!IsPostBack)
			{
				CtrTitle.Title="用户组成员维护";
				if(this.Request.QueryString["ActorMemberID"]!=null &&
						this.Request.QueryString["ActorID"]!=null)
				{
					string sID=this.Request.QueryString["ActorMemberID"];
					string sActorID=this.Request.QueryString["ActorID"];
					
					LoadActorTypeList();

					LoadData(StringTool.String2Long(sID));
					//用户组ID
					hidActorID.Value=sActorID;

					
				}
			}
		}

		private void LoadActorTypeList()
		{
			ListItem item;
//			ListItem item=new ListItem("","0");
//			dpdActorType.Items.Add(item);

			item=new ListItem("部门","10");
			dpdActorType.Items.Add(item);

			item=new ListItem("人员","20");
			dpdActorType.Items.Add(item);

			item=new ListItem("条件人员","30");
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

			//显示对象Name
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

		protected void cmdSave_Click(object sender, System.EventArgs e)
		{
			ActorMemberEntity ame=new ActorMemberEntity();
			ame.ActorID=StringTool.String2Long(hidActorID.Value);
			ame.ID=StringTool.String2Long(hidID.Value);
			ame.ObjectID=StringTool.String2Long(hidObjectID.Value.ToString());
			ame.ActorType=(eO_ActorObject)StringTool.String2Int(dpdActorType.SelectedValue.ToString());
			// 注意只有新增时才保存了rangeid
			ame.RangeID = Session["RangeID"].ToString();
			if(ame.ActorID==0)
			{
                PageTool.MsgBox(this, "未指定相应用户组,保存失败。");
				return;
			}
			if(ame.ActorType==0 || ame.ObjectID==0)
			{
				PageTool.MsgBox(this,"数据输入不完整,请完善后再保存。");
				return;
			}
			try
			{
				if(ActorMemberEntity.Is_ActorMemeber_Exist(ame.ActorID.ToString(),((int)ame.ActorType).ToString(),ame.ObjectID.ToString()))
				{
                    PageTool.MsgBox(this, "成员已存在该组中,请重新输入!");
				}
				else
				{
					ame.Save();
					hidID.Value=ame.ID.ToString();
                    PageTool.AddJavaScript(this, "window.opener.location.href=window.opener.location.href;window.close();");
				}


			}catch(Exception ex)
			{
				PageTool.MsgBox(this,"保存用户组成员时出现错误,错误为:\n"+ex.Message.ToString());
			}
			
		}
	}
}
