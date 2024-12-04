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
	/// form_All_FlowModel 的摘要说明。
	/// </summary>
    public partial class form_All_FlowModel : BasePage
	{


		long lngAppID = 0;
        protected string strExtPara = "";
        protected string strProblemFlowID = string.Empty;       //存放问题单发起变更时，存放的问题的FlowID

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
            }

            if(Request.QueryString["epProblem"] != null)
            {
                strProblemFlowID = Request.QueryString["epProblem"].ToString();
            }

            if (Request.QueryString["AppID"] != null)
			{
				lngAppID = long.Parse(Request.QueryString["AppID"]);

			}
			else
			{
                Session["FromUrl"] = Epower.ITSM.Base.Constant.ApplicationPath + "/Forms/form_All_FlowModel.aspx";
			}
			DataSet ds;
			if(lngAppID == 0)
			{
				CtrTitle1.Title = "所有流程清单";
				ds = FlowModel.GetAllCanStartFlowModels(lngUserID);
			}
			else
			{
				CtrTitle1.Title = epApp.GetAppName(lngAppID) + "登记流程清单";   //获取ＡＰＰ名称
				ds = FlowModel.GetAllCanStartFlowModels(lngUserID,lngAppID);
			}

            Session["ProblemFlowID"] = strProblemFlowID;

			if (ds.Tables[0].Rows.Count==1)
			{
                if (IsNewWin == false)
                {
                    Response.Redirect(Epower.ITSM.Base.Constant.ApplicationPath + "/Forms/OA_AddNew.aspx?flowmodelid=" + ds.Tables[0].Rows[0]["FlowModelID"] + (strExtPara == "" ? "" : "&ep=" + strExtPara) + (strProblemFlowID == "" ? "" : "&epProblem=" + strProblemFlowID));
                }
                else
                {
                    Response.Redirect(Epower.ITSM.Base.Constant.ApplicationPath + "/Forms/OA_AddNew.aspx?NewWin=true&flowmodelid=" + ds.Tables[0].Rows[0]["FlowModelID"] + (strExtPara == "" ? "" : "&ep=" + strExtPara) + (strProblemFlowID == "" ? "" : "&epProblem=" + strProblemFlowID));
                }

			}
			else
			{
				grdFlowModel.DataSource = ds.Tables[0].DefaultView;
				grdFlowModel.DataBind();
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
	}
}
