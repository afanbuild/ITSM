/*******************************************************************
 * ��Ȩ���У�
 * Description���������ѡ�ؼ�
 * 
 * 
 * Create By  ��
 * Create Date��
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
	///		CtrOperates ��ժҪ˵����
	/// </summary>
	public partial class CtrOperates : System.Web.UI.UserControl
	{

		#region ����
		/// <summary>
		/// ��¼�û����ڲ���ID
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
		/// ��ȡ��ѡ�û�ID(���ŷָ�)
		/// </summary>
		/// <param name="sNames">��������('\n'�ָ�)</param>
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
		///		�����֧������ķ��� - ��Ҫʹ�ô���༭��
		///		�޸Ĵ˷��������ݡ�
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
