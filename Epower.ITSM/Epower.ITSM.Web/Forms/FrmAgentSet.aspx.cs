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
    /// FrmAgentSet 的摘要说明。
    /// </summary>
    public partial class FrmAgentSet : BasePage
    {

        /// <summary>
        /// 设置母版页面按钮
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

            CtrTitle1.Title = "出差授权";

            if (intStatus == 0)
            {
                lblMsg.Text = "出差授权没有生效";
                lblMsg.ForeColor = Color.Black;
                cmdStatus.Text = "启用出差授权";
            }
            else
            {
                lblMsg.Text = "出差授权已经生效";
                lblMsg.ForeColor = Color.Red;
                cmdStatus.Text = "暂停出差授权";
            }

            if (!Page.IsPostBack)
            {

                DataSet ds = EPSystem.GetAllAgents(lngUserID);

                grdAgent.DataSource = ds.Tables[0].DefaultView;
                grdAgent.DataBind();
            }
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

        protected void cmdStatus_Click(object sender, System.EventArgs e)
        {

            long lngUserID = (long)Session["UserID"];

            int intStatus = Miscellany.SetAgentStatus(lngUserID);

            Session["AgentStatus"] = intStatus;

            if (intStatus == 0)
            {
                lblMsg.Text = "出差授权没有生效";
                lblMsg.ForeColor = Color.Black;
                cmdStatus.Text = "启用出差授权";
            }
            else
            {
                lblMsg.Text = "出差授权已经生效";
                lblMsg.ForeColor = Color.Red;
                cmdStatus.Text = "暂停出差授权";
            }



            Response.Redirect("FrmAgentSet.aspx");


        }

        protected void cmdCancel_Click(object sender, System.EventArgs e)
        {
            long lngUserID = (long)Session["UserID"];

            if ((int)Session["AgentStatus"] != 0)
            {
                //设置代理无效
                int intStatus = Miscellany.SetAgentStatus(lngUserID);

                Session["AgentStatus"] = intStatus;
            }

            Miscellany.CancelAgent(lngUserID);


            Response.Redirect("FrmAgentSet.aspx");


        }
    }
}
