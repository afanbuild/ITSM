/*******************************************************************
 * 版权所有：
 * Description：操作项多选控件
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

	/// <summary>
	///		CtrOperates 的摘要说明。
	/// </summary>
	public partial class CtrOperates : System.Web.UI.UserControl
	{

		#region 属性
		/// <summary>
		/// 登录用户所在部门ID
		/// </summary>
		public long SystemID
		{
			set{ViewState[this.ID+"SystemID"]=value;}
			get
			{
				if(ViewState[this.ID+"SystemID"]!=null)
					return (long)ViewState[this.ID+"SystemID"];
				else
					return 0;
			}
		}
        /// <summary>
        /// 
        /// </summary>
        public string OpCatalog
        {
            set { ViewState[this.ID + "OpCatalog"] = value; }
            get
            {
                if (ViewState[this.ID + "OpCatalog"] != null)
                    return ViewState[this.ID + "OpCatalog"].ToString();
                else
                    return "0";
            }
        }
		#endregion

		protected void Page_Load(object sender, System.EventArgs e)
		{
			if(!IsPostBack)
			{
				LoadData();
			}
		}

		public void LoadData()
		{
			if(this.SystemID!=0)
			{
                DataTable dt = OperateDP.GetAllOperate(this.SystemID, this.OpCatalog);
				dlLeaders.DataSource=dt.DefaultView;
				dlLeaders.DataBind();
				if(dt.Rows.Count==0)
					dlLeaders.Visible=false;
			}
		}

		public string SelectOpIDS
		{
			set{ViewState[this.ID+"SelectOpIDS"]=value;}
			get
			{
				if(ViewState[this.ID+"SelectOpIDS"]!=null)
					return ViewState[this.ID+"SelectOpIDS"].ToString();
				else
					return "";
			}
		}

		public string GetSelectOpIDs()
		{
			string sName="";
			return GetSelectOpIDs(ref sName); 
		}

		/// <summary>
		/// 获取被选用户ID(逗号分割)
		/// </summary>
		/// <param name="sNames">返回名称('\n'分割)</param>
		/// <returns></returns>
		public string GetSelectOpIDs( ref string sNames)
		{
			CheckBox chkSelect;
			HtmlInputHidden hidLeaderID;
			HtmlInputHidden hidLeaderName;
			string sID="";
			sNames="";
			foreach(DataListItem item in dlLeaders.Items)
			{
				chkSelect=(CheckBox)item.FindControl("chkSelect");
				hidLeaderID=(HtmlInputHidden)item.FindControl("hidLeaderID");
				hidLeaderName=(HtmlInputHidden)item.FindControl("hidLeaderName");
				if(chkSelect.Checked)
				{
					sID+=hidLeaderID.Value+",";
					sNames+=hidLeaderName.Value+"\n";
				}
			}
			sID=sID.TrimEnd(',');
			sNames=sNames.TrimEnd('\n');
			return sID;
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
			this.dlLeaders.ItemDataBound += new System.Web.UI.WebControls.DataListItemEventHandler(this.dlLeaders_ItemDataBound);

		}
		#endregion

		private void dlLeaders_ItemDataBound(object sender, System.Web.UI.WebControls.DataListItemEventArgs e)
		{
			if(e.Item.ItemType==ListItemType.Item || e.Item.ItemType==ListItemType.AlternatingItem)
			{
				CheckBox chkSelect=(CheckBox)e.Item.FindControl("chkSelect");
				HtmlInputHidden hidLeaderID=(HtmlInputHidden)e.Item.FindControl("hidLeaderID");
				if((this.SelectOpIDS+",").IndexOf(hidLeaderID.Value+",")>=0)
				{
					chkSelect.Checked=true;
					chkSelect.ForeColor=Color.Red; 
				}
			}
		}


	}
}
