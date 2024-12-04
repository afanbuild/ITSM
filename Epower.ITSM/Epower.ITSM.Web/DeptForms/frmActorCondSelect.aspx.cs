/*
 *	by duanqs
 */
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
using Epower.DevBase.Organization.SqlDAL;

namespace Epower.ITSM.Web.Forms
{
	/// <summary>
	/// frmActorCondSelect ��ժҪ˵����
	/// </summary>
    public partial class frmActorCondSelect : BasePage
	{
		//protected Epower.ITSM.Web.Controls.CtrTitle CtrTitle;

		protected void Page_Load(object sender, System.EventArgs e)
		{
			//CtrTitle.Title="�û��������б�";
			if(!IsPostBack)
			{
				LoadData();
			}
		}

        #region ��ô������Ĳ��������жϸ�����Ŀؼ�id�����Ƶ���ͬ����
        /// <summary>
        /// ��ô������Ĳ��������жϸ�����Ŀؼ�id�����Ƶ���ͬ����
        /// </summary>
        public string Opener_ClientId
        {

            get
            {
                return (Request["Opener_ClientId"] == null) ? "" : Request["Opener_ClientId"].ToString().Replace("hidClientId_ForOpenerPage", "");
            }
        }
        #endregion

		private void LoadData()
		{
			long lngSystemID = (long)Session["SystemID"];
			string strRangeID = Session["RangeID"].ToString();

			DataTable dt=ActorCondDP.GetActorCondList(lngSystemID,strRangeID);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void dgActorConds_ItemCommand(object source, DataGridCommandEventArgs e)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dgActorConds_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Label labCondID = (Label)e.Item.FindControl("labCondID");
                Label labCondName = (Label)e.Item.FindControl("labCondName");
                e.Item.Attributes.Add("ondblclick", "DbSelectclick('" + labCondID.Text.Trim() + "','" + labCondName.Text.Trim()+ "')");
            }
        }
	}
}
