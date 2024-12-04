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
    /// FrmAgentSet ��ժҪ˵����
    /// </summary>
    public partial class FrmAgentSet : BasePage
    {

        /// <summary>
        /// ����ĸ��ҳ�水ť
        /// </summary>
        protected void SetParentButtonEvent()
        {
            //this.Master.TableVisible = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, System.EventArgs e)
        {
            SetParentButtonEvent();

            long lngUserID = (long)Session["UserID"];
            //long lngUserID = 1015;
            int intStatus = (int)Session["AgentStatus"];

            CtrTitle1.Title = "������Ȩ";

            if (intStatus == 0)
            {
                lblMsg.Text = "������Ȩû����Ч";
                lblMsg.ForeColor = Color.Black;
                cmdStatus.Text = "���ó�����Ȩ";
            }
            else
            {
                lblMsg.Text = "������Ȩ�Ѿ���Ч";
                lblMsg.ForeColor = Color.Red;
                cmdStatus.Text = "��ͣ������Ȩ";
            }

            if (!Page.IsPostBack)
            {

                DataSet ds = EPSystem.GetAllAgents(lngUserID);

                grdAgent.DataSource = ds.Tables[0].DefaultView;
                grdAgent.DataBind();
            }
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

        protected void cmdStatus_Click(object sender, System.EventArgs e)
        {

            long lngUserID = (long)Session["UserID"];

            int intStatus = Miscellany.SetAgentStatus(lngUserID);

            Session["AgentStatus"] = intStatus;

            if (intStatus == 0)
            {
                lblMsg.Text = "������Ȩû����Ч";
                lblMsg.ForeColor = Color.Black;
                cmdStatus.Text = "���ó�����Ȩ";
            }
            else
            {
                lblMsg.Text = "������Ȩ�Ѿ���Ч";
                lblMsg.ForeColor = Color.Red;
                cmdStatus.Text = "��ͣ������Ȩ";
            }



            Response.Redirect("FrmAgentSet.aspx");


        }

        protected void cmdCancel_Click(object sender, System.EventArgs e)
        {
            long lngUserID = (long)Session["UserID"];

            if ((int)Session["AgentStatus"] != 0)
            {
                //���ô�����Ч
                int intStatus = Miscellany.SetAgentStatus(lngUserID);

                Session["AgentStatus"] = intStatus;
            }

            Miscellany.CancelAgent(lngUserID);


            Response.Redirect("FrmAgentSet.aspx");


        }
    }
}
