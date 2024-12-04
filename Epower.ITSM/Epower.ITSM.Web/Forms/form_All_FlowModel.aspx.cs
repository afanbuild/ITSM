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
	/// form_All_FlowModel ��ժҪ˵����
	/// </summary>
    public partial class form_All_FlowModel : BasePage
	{


		long lngAppID = 0;
        protected string strExtPara = "";
        protected string strProblemFlowID = string.Empty;       //������ⵥ������ʱ����ŵ������FlowID

        /// <summary>
        /// �Ƿ������ڵ������ڵ����
        /// </summary>
        protected bool IsNewWin
        {
            get { if (Request["NewWin"] != null) return true; else return false; }
        }

        /// <summary>
        /// ����ĸ��ҳ�水ť
        /// </summary>
        protected void SetParentButtonEvent()
        {
            this.Master.TableVisible = false;
        }

		protected void Page_Load(object sender, System.EventArgs e)
		{
            SetParentButtonEvent();
			// �ڴ˴������û������Գ�ʼ��ҳ��
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
				CtrTitle1.Title = "���������嵥";
				ds = FlowModel.GetAllCanStartFlowModels(lngUserID);
			}
			else
			{
				CtrTitle1.Title = epApp.GetAppName(lngAppID) + "�Ǽ������嵥";   //��ȡ���У�����
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
			// CODEGEN���õ����� ASP.NET Web ���������������ġ�
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
