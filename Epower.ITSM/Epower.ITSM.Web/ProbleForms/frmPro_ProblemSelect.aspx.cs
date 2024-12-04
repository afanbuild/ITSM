/****************************************************************************
 * 
 * description:事件分析问题选择
 * 
 * 
 * 
 * Create by:zhumingchun
 * Create Date:2007-09-20
 * *************************************************************************/
using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using Epower.ITSM.SqlDAL;
using Epower.DevBase.Organization.SqlDAL;
using Epower.ITSM.Base;
using Epower.DevBase.BaseTools;

namespace Epower.ITSM.Web.ProbleForms
{
    public partial class frmPro_ProblemSelect : BasePage
    {
        RightEntity re = null;

        /// <summary>
        /// 设置父窗体按钮事件
        /// </summary>
        protected void SetParentButtonEvent()
        {
            this.Master.Master_Button_Query_Click += new Global_BtnClick(Master_Master_Button_Query_Click);
            this.Master.ShowQueryButton(true);
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
        /// 查询事件
        /// </summary>
        void Master_Master_Button_Query_Click()
        {
            LoadData();
        }

        /// <summary>
        /// 页面加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            SetParentButtonEvent();

            ControlPage1.DataGridToControl = dgProblem;
            ControlPage1.On_PostBack += new EventHandler(ControlPage1_On_PostBack);
            if (!IsPostBack)
            {

                //设置显示
                PageDeal.SetLanguage(this.Controls[0]);
                dgProblem.Columns[2].HeaderText = PageDeal.GetLanguageValue("litProbleSubject");
                dgProblem.Columns[3].HeaderText = PageDeal.GetLanguageValue("litProbleType");
                dgProblem.Columns[4].HeaderText = PageDeal.GetLanguageValue("litProbleLevel");
                dgProblem.Columns[5].HeaderText = PageDeal.GetLanguageValue("litProbleRegUserName");
                dgProblem.Columns[6].HeaderText = PageDeal.GetLanguageValue("litProbleState");


                re = (RightEntity)((Hashtable)Session["UserAllRights"])[Constant.QuestionTrace];


                ctrDateSelectTime1.BeginTime = DateTime.Now.AddDays(-7).ToShortDateString();
                ctrDateSelectTime1.EndTime = DateTime.Now.ToShortDateString();

                LoadData();
            }
        }

        /// <summary>
        /// 数据加载
        /// </summary>
        private void LoadData()
        {
            string sWhere = "";
            if (!string.IsNullOrEmpty(CtrFlowProblemState.CatelogValue.Trim()))
                sWhere += " and State= " + CtrFlowProblemState.CatelogID.ToString();
            if (CataProblemType.CatelogID != -1 && CataProblemType.CatelogID != 1006)
                sWhere += " and Problem_Type= " + CataProblemType.CatelogID.ToString();
            if (CataProblemLevel.CatelogID != -1 && CataProblemLevel.CatelogID != 1007)
                sWhere += " and Problem_Level= " + CataProblemLevel.CatelogID.ToString();
            if (!string.IsNullOrEmpty(txtRegUser.Text.Trim()))
                sWhere += " and RegUserName like " + StringTool.SqlQ("%" + txtRegUser.Text.Trim() + "%");
            if (!string.IsNullOrEmpty(ctrDateSelectTime1.BeginTime.Trim()))
                sWhere += " and RegTime>= to_date(" + StringTool.SqlQ(ctrDateSelectTime1.BeginTime.Trim()) + ",'yyyy-MM-dd HH24:mi:ss')";
            if (!string.IsNullOrEmpty(ctrDateSelectTime1.EndTime.Trim()))
                sWhere += " and RegTime<= to_date(" + StringTool.SqlQ(DateTime.Parse(ctrDateSelectTime1.EndTime).AddDays(1).ToShortDateString()) + ",'yyyy-MM-dd HH24:mi:ss')";

            if (!string.IsNullOrEmpty(txtTitle.Text.Trim()))
                sWhere += " and Problem_Title like " + StringTool.SqlQ("%" + txtTitle.Text.Trim() + "%");

            long lngUserID = (long)Session["UserID"];
            long lngDeptID = (long)Session["UserDeptID"];
            long lngOrgID = (long)Session["UserOrgID"];
            //获得权限
            RightEntity reTrace = (RightEntity)((Hashtable)Session["UserAllRights"])[Constant.QuestionTrace];
            DataTable dt = ProblemDealDP.GetProblemDealData(lngUserID, lngDeptID, lngOrgID, reTrace, sWhere); ;
            DataView dv = new DataView(dt);
            dgProblem.DataSource = dv;
            dgProblem.DataBind();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ControlPage1_On_PostBack(object sender, EventArgs e)
        {
            LoadData();
        }

        #region 显示页面地址
        /// <summary>
        /// 显示页面地址
        /// </summary>
        /// <param name="lngNoticeID"></param>
        /// <returns></returns>
        protected string GetUrl(decimal lngFlowID)
        {
            string sUrl = "";
            sUrl = "javascript:window.open('../Forms/frmIssueView.aspx?FlowID=" + lngFlowID.ToString() + "','MainFrame','scrollbars=yes,resizable=yes,top=0,left=0,width=window.availWidth-12,height=window.availHeight-10');";
            return sUrl;
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            foreach (DataGridItem itm in dgProblem.Items)
            {
                if (itm.ItemType == ListItemType.AlternatingItem ||
                    itm.ItemType == ListItemType.Item)
                {
                    string sID = itm.Cells[8].Text;
                    CheckBox chkdel = (CheckBox)itm.Cells[0].FindControl("chkDel");
                    if (chkdel.Checked)
                    {
                        sb.Append(sID + ",");
                    }
                }
            }
            StringBuilder sbText = new StringBuilder();
            sbText.Append("<script>");
            sbText.Append("var arr = new Array();");
            // ID
            sbText.Append("arr[0] ='" + sb.ToString() + "';");
            //=======zxl==
            //sbText.Append("window.parent.returnValue = arr;");

            sbText.Append(" var value=arr;");
            sbText.Append("if(typeof(value) != 'undefined' && value[0]!='')");
            sbText.Append("{");
            sbText.Append(" window.opener.document.getElementById('" + Opener_ClientId + "hidCustArrID').value=arr[0];");
            sbText.Append(" window.opener.document.getElementById('" + Opener_ClientId + "hidFlag').value='OK'; ");
            sbText.Append(" window.opener.document.getElementById('" + Opener_ClientId + "hidBtnAdd').click();  ");

            sbText.Append("}");
            sbText.Append(" else {");
            sbText.Append(" window.opener.document.getElementById('" + Opener_ClientId + "hidFlag').value='NO';");
            sbText.Append("}");

          //  sbText.Append("self.opener.location.reload(); ");

            //=======zxl==
            // 关闭窗口
            sbText.Append("top.close();");
            sbText.Append("</script>");
            // 向客户端发送
            Page.RegisterStartupScript(DateTime.Now.ToString(), sbText.ToString());

           // Response.Write(sbText.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnClose_Click(object sender, EventArgs e)
        {
            StringBuilder sbText = new StringBuilder();
            sbText.Append("<script>");
            // 关闭窗口
            sbText.Append("top.close();");
            sbText.Append("</script>");
            // 向客户端发送
            Page.RegisterStartupScript(DateTime.Now.ToString(), sbText.ToString());
            Response.Write(sbText.ToString());
        }

        protected void dgProblem_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {

                string sID = e.Item.Cells[8].Text + ",";
                e.Item.Attributes.Add("ondblclick", "SelectOndbClick('" + sID + "');");


            }
        }
    }
}