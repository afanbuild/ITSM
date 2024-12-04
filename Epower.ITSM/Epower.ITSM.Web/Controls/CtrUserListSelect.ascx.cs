/*******************************************************************
 * 版权所有：
 * Description：用户列表多选
 * 
 * 
 * Create By  ：
 * Create Date：
 * *****************************************************************/
namespace Epower.ITSM.Web.Controls
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using Epower.DevBase.Organization.SqlDAL;
	using Epower.DevBase.Organization.Base ;
	/// <summary>
	///		CtrUserListSelect 的摘要说明。
	/// </summary>
	public partial class CtrUserListSelect : System.Web.UI.UserControl
	{

		#region 属性
		/// <summary>
		/// 登录用户所在部门ID
		/// </summary>
		public long UserDeptID
		{
			set{ViewState[this.ID+"UserDeptID"]=value;}
			get
			{
				if(ViewState[this.ID+"UserDeptID"]!=null)
					return (long)ViewState[this.ID+"UserDeptID"];
				else
					return 0;
			}
		}

		/// <summary>
		/// 是否可以多选
		/// </summary>
		public bool MultiSelect
		{
			set{ViewState[this.ID+"MultiSelect"]=value;}
			get
			{
				if(ViewState[this.ID+"MultiSelect"]!=null)
					return (bool)ViewState[this.ID+"MultiSelect"];
				else
					return true;//默认可以多选
			}
		}

		/// <summary>
		/// 选择用户要显示的部门性质
		/// *请用枚举eO_DeptType赋值
		/// *不赋值表示同时显示部门和机构
		/// </summary>
		public eO_DeptType DeptType
		{
			set{ViewState[this.ID+"DeptKind"]=value;}
			get
			{
				if(ViewState[this.ID+"DeptKind"]!=null)
					return (eO_DeptType)ViewState[this.ID+"DeptKind"];
				else
					return eO_DeptType.eDptLeader;
			}
		}

		/// <summary>
		/// 设置被选的部门ID列表
		/// </summary>
		public string SetSelectUserIDS
		{
			set{ViewState[this.ID+"SetSelectUserIDS"]=value;}
			get
			{
				if(ViewState[this.ID+"SetSelectUserIDS"]!=null)
					return ViewState[this.ID+"SetSelectUserIDS"].ToString();
				else
					return "";
			}
		}

		#endregion

		protected void Page_Load(object sender, System.EventArgs e)
		{
			lstUsers.Attributes["ondblclick"]="Choice_Dept("+lstUsers.ClientID+","+lstSelected.ClientID+")";
			lstSelected.Attributes["ondblclick"]="Choice_Dept("+lstSelected.ClientID+","+lstUsers.ClientID+")";
			btnAdd.Attributes["onclick"]="Choice_Dept("+lstUsers.ClientID+","+lstSelected.ClientID+")";
			btnRemove.Attributes["onclick"]="Choice_Dept("+lstSelected.ClientID+","+lstUsers.ClientID+")";
			btnAddAll.Attributes["onclick"]="ChoiceAll_Dept("+lstUsers.ClientID+","+lstSelected.ClientID+")";
			btnClear.Attributes["onclick"]="ChoiceAll_Dept("+lstSelected.ClientID+","+lstUsers.ClientID+")";

			if(!IsPostBack)
			{

				DataTable dt=new DataTable();
				if(this.DeptType==eO_DeptType.eDptLeader)
				{
					dt = DeptDP.GetSameOrgLeader(this.UserDeptID);
				}
				else
				{
					dt = DeptDP.GetSubUsers(this.UserDeptID);
				}
				
				lstUsers.Items.Clear();
				if(dt.Rows.Count!=0)
				{
					lstUsers.DataSource = dt.DefaultView;
					lstUsers.DataTextField="Name";
					lstUsers.DataValueField="UserID";
					lstUsers.DataBind();
				}

				//显示选择的部门
				if(SetSelectUserIDS.Trim(',')!="")
				{
					for(int i=lstUsers.Items.Count-1;i>=0 ;i--)
					{
						if((SetSelectUserIDS+",").IndexOf(lstUsers.Items[i].Value+",")>=0)
						{
							lstSelected.Items.Add(lstUsers.Items[i]);
							hid_IDS.Value+=lstUsers.Items[i].Value+",";
							hid_Names.Value+=lstUsers.Items[i].Text+"\n"; 
							lstUsers.Items.Remove(lstUsers.Items[i]);
						}
					}
					hid_IDS.Value=hid_IDS.Value.TrimEnd(',');
					hid_Names.Value=hid_Names.Value.TrimEnd('\n');   
				}
			}
		}

		public string GetSelectUserIds()
		{
			string sUserID=hid_IDS.Value.TrimEnd(',');
			return sUserID;
		}

		public string GetSelectUserIds(ref string strNames)
		{
			string sIDs=GetSelectUserIds();
			strNames=hid_Names.Value.Replace("\r\n","\n").TrimEnd('\n'); 
			return sIDs;
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
		///		设计器支持所需的方法 - 不要使用代码编辑器
		///		修改此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{

		}
		#endregion
	}
}
