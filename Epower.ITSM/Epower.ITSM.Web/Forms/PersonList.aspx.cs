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
	/// PersonList 的摘要说明。
	/// </summary>
	public partial class PersonList : BasePage
	{
	
		long lngAppID=0;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// 在此处放置用户代码以初始化页面
			
						
		
			
			long lngDeptID = 1;   //根部门ID

			if(Request.QueryString["DeptID"] != null)
			{
				lngDeptID = long.Parse(Request.QueryString["DeptID"]);
				//后续的装入 通过暂存在部门列表页面的 参数传递（！！！因为SESSION值可能因为其它的页面打开而改变！！！，而部门列表页面没有服务器交互）
				lngAppID = long.Parse(Request.QueryString["AppID"]);	

			}
			else
			{
				//第一次装入时通过SESSION取 应用ID
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
						//系统预定义的编号不显示出来
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
			// CODEGEN：该调用是 ASP.NET Web 窗体设计器所必需的。
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
				//UIGlobal.MsgBox(this,"请选择具体的人员");
			}
		}
	}
}
