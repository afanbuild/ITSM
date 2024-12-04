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
using Epower.ITSM.SqlDAL;
using Epower.DevBase.BaseTools;
using Epower.ITSM.Base;
using Epower.DevBase.Organization.SqlDAL;
using System.Text;
using EpowerCom;
using Epower.ITSM.SqlDAL.Print;
using Epower.ITSM.SqlDAL.Service;

namespace Epower.ITSM.Web.AppForms
{
    /// <summary>
    /// MailMessageTemEdit 的摘要说明。
    /// </summary>
    public partial class frm_EmailIssueEdit : BasePage
    {
        #region SetParentButtonEvent
        /// <summary>
        /// 设置父窗体按钮事件
        /// </summary>
        protected void SetParentButtonEvent()
        {
            this.Master.OperatorID = Constant.CustumManager;
            this.Master.IsCheckRight = true;
            if (this.Request.QueryString["id"] != null)
            {
                this.Master.MainID = this.Request.QueryString["id"].ToString();
            }
           
            this.Master.Master_Button_GoHistory_Click += new Global_BtnClick(Master_Master_Button_GoHistory_Click);            
        
            this.Master.ShowNewButton(false);
            this.Master.ShowDeleteButton(false);
            this.Master.ShowSaveButton(false);
            this.Master.ShowBackUrlButton(true);
        }

        #endregion

     


        #region Master_Master_Button_GoHistory_Click

        /// <summary>
        /// 返回
        /// </summary>
        void Master_Master_Button_GoHistory_Click()
        {
            Response.Redirect("frm_EmailIssue.aspx");

        }

        #endregion

        #region Page_Load

        /// <summary>
        /// 页面加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, System.EventArgs e)
        {
            //设置主页面
            SetParentButtonEvent();
            if (!IsPostBack)
            {
                LoadFlowModels();
                if (Master.MainID.ToString() != "")
                {
                    DataTable dt = EmialIssue.getEmailIssueTable(long.Parse(this.Master.MainID));
                    if (dt.Rows.Count > 0)
                    {
                        Lb_FromEmail.Text = dt.Rows[0]["FromEmail"].ToString();
                        lb_EmailTitle.Text = dt.Rows[0]["EmailTitle"].ToString();
                        lb_Emailcontent.Text =StringTool.ParseForHtml(dt.Rows[0]["EmailContant"].ToString());

                        if (dt.Rows[0]["Statue"].ToString() == "1")
                        {
                            Btn_baoZhang.Visible = false;
                        }
                    }
                }
 


            }
        }

        #endregion



        #region radio

        private void LoadFlowModels()
        {
            radioFlowModel.Items.Clear();
            DataTable dt = AppFieldConfigDP.GetFlowModelListForOID(1026, (long)Session["UserID"]);
            radioFlowModel.DataSource = dt.DefaultView;
            radioFlowModel.DataTextField = "FlowName";
            radioFlowModel.DataValueField = "OFlowModelID";
            radioFlowModel.DataBind();


        }

        #endregion

        protected void Btn_baoZhang_Click(object sender, EventArgs e)
        {
            if (radioFlowModel.SelectedIndex != -1)
            {

                Session["IssueEmailCutReqSubject"] = lb_EmailTitle.Text.Trim();
                Session["IssueEmailCutReqContext"] = lb_Emailcontent.Text.Trim();
                string flowmodelid = "0";
                string templateid = "0";
                flowmodelid = radioFlowModel.SelectedValue;
                EmialIssue.updateStatue(long.Parse(Master.MainID.ToString()));//更改报障状态
                Response.Write("<script>window.open('../Forms/oa_AddNew.aspx?flowmodelid=" + flowmodelid + "&IsFirst=true&ep=" + templateid + "','MainFrame','scrollbars=no,status=yes ,resizable=yes,width=680,height=500');</script>");

            }
            else
            {
                PageTool.MsgBox(this.Page,"请选择报障的流程！");
            }

        }



      

    }
}
