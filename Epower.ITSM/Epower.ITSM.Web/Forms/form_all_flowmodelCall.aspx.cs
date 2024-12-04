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
using Epower.ITSM.SqlDAL;
using Epower.DevBase.BaseTools;

namespace Epower.ITSM.Web.Forms
{
	/// <summary>
	/// form_All_FlowModel 的摘要说明。
	/// </summary>
    public partial class form_all_flowmodelCall : BasePage
	{


		long lngAppID = 0;
        protected string strExtPara = "";

        /// <summary>
        /// 是否是属于弹出窗口的情况
        /// </summary>
        protected bool IsNewWin
        {
            get { if (Request["NewWin"] != null) return true; else return false; }
        }

        /// <summary>
        /// 设置母版页面按钮
        /// </summary>
        protected void SetParentButtonEvent()
        {
            this.Master.TableVisible = false;
        }

		protected void Page_Load(object sender, System.EventArgs e)
		{
            SetParentButtonEvent();
			// 在此处放置用户代码以初始化页面
			long lngUserID = (long)Session["UserID"];



            if (Request.QueryString["ep"] != null)
            {
                strExtPara = Request.QueryString["ep"];

                if (strExtPara.IndexOf("t") != -1)
                {
                    trCust.Visible = true;

                    #region 展示用户信息
                    string extendParameter = strExtPara.Replace("t", "");
                    Br_ECustomerDP ec = new Br_ECustomerDP();
                    string sWhere = " AND (Mobile=" + StringTool.SqlQ(extendParameter) + " OR TEL1=" + StringTool.SqlQ(extendParameter) + ") ";
                    DataTable dt1 = ec.GetDataTable(sWhere, "");
                    foreach (DataRow row in dt1.Rows)
                    {
                        labCustAddr.Text = row["ShortName"].ToString();
                        lblCustDeptName.Text = row["CustDeptName"].ToString();
                        lblEmail.Text = row["Email"].ToString();
                        lbljob.Text = row["job"].ToString();
                        lblAddr.Text = row["Address"].ToString();
                        labContact.Text = row["LinkMan1"].ToString();
                        labCTel.Text = row["Tel1"].ToString();
                        lblMastCust.Text = row["MastCustName"].ToString();
                    }

                    if (dt1 == null || dt1.Rows.Count <= 0)
                    {
                        labCTel.Text = extendParameter;
                    }
                    #endregion
                }
                else
                    trCust.Visible = false;

            }

            if (Request.QueryString["AppID"] != null)
			{
				lngAppID = long.Parse(Request.QueryString["AppID"]);
                if (!(Request.QueryString["ep"] != null && Request.QueryString["ep"].ToString() == "0"))
                    Session["FromUrl"]= Epower.ITSM.Base.Constant.ApplicationPath + "/Forms/form_All_FlowModel.aspx?AppID=" + lngAppID.ToString();
			}
			else
			{
                Session["FromUrl"] = Epower.ITSM.Base.Constant.ApplicationPath + "/Forms/form_All_FlowModel.aspx";
			}
			DataSet ds;
			if(lngAppID == 0)
			{
				CtrTitle1.Title = "事件登记流程清单";
				ds = FlowModel.GetAllCanStartFlowModels(lngUserID);
			}
			else
			{
				CtrTitle1.Title = epApp.GetAppName(lngAppID) + "登记流程清单";   //获取ＡＰＰ名称
				ds = FlowModel.GetAllCanStartFlowModels(lngUserID,lngAppID);
			}

	
				grdFlowModel.DataSource = ds.Tables[0].DefaultView;
				grdFlowModel.DataBind();
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdFlowModel_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                e.Item.Attributes.Add("onmouseover", "currentcolor=this.style.backgroundColor;this.style.backgroundColor='#FFFBE8'");
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=currentcolor");

                string lngFlowModelID = e.Item.Cells[grdFlowModel.Columns.Count-1].Text.Trim();
                e.Item.Attributes.Add("ondblclick", "AddNewFlow(" + lngFlowModelID + ");window.close();");
            }
        }
	}
}
