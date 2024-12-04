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
using Epower.DevBase.Organization;
using Epower.DevBase.Organization.SqlDAL;

namespace Epower.ITSM.Web.Forms
{
	/// <summary>
	/// frmPopActor ��ժҪ˵����
	/// </summary>
	public partial class frmPopOperates : BasePage
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if(!IsPostBack)
			{	string sOperateID="";
				if(this.Request.QueryString["OperateID"]!=null)
				{
					sOperateID=this.Request.QueryString["OperateID"].ToString().Trim();
				}

				lstOperates.Attributes.Add("OnDblClick","On_ListDblClick()");

                #region Ȩ�����
                ddltRightType.Items.Clear();
                DataTable dt = RightDP.GetRightType();
                ddltRightType.DataTextField = "OpCatalog";
                ddltRightType.DataValueField = "OpCatalog";
                ddltRightType.DataSource = dt;
                ddltRightType.DataBind();

                ddltRightType.Items.Insert(0, new ListItem("--Ȩ�����--", "0"));
                #endregion 

				LoadData();
				BindData();

				if (sOperateID.Length>0)
				{
					lstOperates.SelectedIndex=lstOperates.Items.IndexOf(lstOperates.Items.FindByValue(sOperateID));
				}
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
        /// <summary>
        /// �ж��ĸ�ҳ����ת������
        /// </summary>
        public string TypeFrm
        {
            get
            {

                if (Request.QueryString["TypeFrm"] != null)
                {
                    return Request.QueryString["TypeFrm"];
                }
                else
                {
                    return "";
                }
            }
        }

		//�����ݵ�ListBox
		private void BindData()
		{
			lstOperates.DataSource=((DataTable)Session["OPERATE"]).DefaultView;
			lstOperates.DataTextField ="OpName";
            lstOperates.DataValueField = "OpIDNameType";
			lstOperates.DataBind();			
		}

		//�������ݲ�����
		private void LoadData()
		{
			DataTable dt=new DataTable();
			long lngSystemID = (long)Session["SystemID"];
			dt=OperateControl.GetAllOperate(lngSystemID);
			Session["OPERATE"]=dt;
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddltRightType_SelectedIndexChanged(object sender, EventArgs e)
        {
            long lngSystemID = (long)Session["SystemID"];
            DataTable dt = OperateDP.GetAllOperate(lngSystemID, ddltRightType.SelectedValue);
            Session["OPERATE"] = dt;
            BindData();
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
	}
}
