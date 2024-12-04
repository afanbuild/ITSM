/*
 * by duanqs
 * */
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

namespace Epower.ITSM.Web.Forms
{
	/// <summary>
	/// frmActorCondList ��ժҪ˵����
	/// </summary>
    public partial class frmActorCondList : BasePage
	{

		protected void Page_Load(object sender, System.EventArgs e)
		{
			CtrTitle.Title="������Ա�б�";
			if(!IsPostBack)
			{
				LoadData();
			}
		}
        protected void cmdQuery_Click(object sender, EventArgs e)
        {
            LoadData();
        }

		private void LoadData()
		{

            string sWhere = "";

            if(txtCondID.Text.Trim()!="")
            {
                string condID = txtCondID.Text.Trim();
                sWhere += " and condid like "+StringTool.SqlQ("%"+condID.ToString()+"%");
            }
            if (txtCondName.Text.Trim()!="")
            {
                string condName = txtCondName.Text.Trim();
                sWhere += " and condname like " + StringTool.SqlQ("%"+condName.ToString()+"%");
            }
			long lngSystemID = (long)Session["SystemID"];
			string strRangeID = Session["RangeID"].ToString();

			//DataTable dt=ActorCondDP.GetActorCondList(lngSystemID,strRangeID);
            DataTable dt = ActorCondDP.GetActorCondList(sWhere,lngSystemID,strRangeID);
			dgActorConds.DataSource=dt.DefaultView;
			dgActorConds.DataBind();
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


		//ɾ������
		protected void cmdDelete_Click(object sender, System.EventArgs e)
		{
			foreach(DataGridItem item in dgActorConds.Items)
			{
				bool isSelect=((CheckBox)item.Cells[2].FindControl("chkSelect")).Checked;
				if(isSelect)
				{
					string sID=item.Cells[1].Text;
					ActorCondEntity ace=new ActorCondEntity();
					ace.CondId=StringTool.String2Long(sID);
					try
					{
						ace.Delete();
						
					}catch(Exception ee)
					{
                        PageTool.MsgBox(this, ee.Message.ToString());
					}		
				}
			}
			LoadData();
		}

		//��������
		protected void cmdLoad_Click(object sender, System.EventArgs e)
		{
			LoadData();
		}

		
	}
}
