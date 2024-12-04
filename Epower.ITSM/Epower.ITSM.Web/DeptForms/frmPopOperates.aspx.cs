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
	/// frmPopActor 的摘要说明。
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

                #region 权限类别
                ddltRightType.Items.Clear();
                DataTable dt = RightDP.GetRightType();
                ddltRightType.DataTextField = "OpCatalog";
                ddltRightType.DataValueField = "OpCatalog";
                ddltRightType.DataSource = dt;
                ddltRightType.DataBind();

                ddltRightType.Items.Insert(0, new ListItem("--权限类别--", "0"));
                #endregion 

				LoadData();
				BindData();

				if (sOperateID.Length>0)
				{
					lstOperates.SelectedIndex=lstOperates.Items.IndexOf(lstOperates.Items.FindByValue(sOperateID));
				}
			}
		}

        #region 获得传过来的参数用来判断父窗体的控件id的名称的相同部分
        /// <summary>
        /// 获得传过来的参数用来判断父窗体的控件id的名称的相同部分
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
        /// 判断哪个页面跳转过来的
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

		//绑定数据到ListBox
		private void BindData()
		{
			lstOperates.DataSource=((DataTable)Session["OPERATE"]).DefaultView;
			lstOperates.DataTextField ="OpName";
            lstOperates.DataValueField = "OpIDNameType";
			lstOperates.DataBind();			
		}

		//加载数据并缓存
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
	}
}
