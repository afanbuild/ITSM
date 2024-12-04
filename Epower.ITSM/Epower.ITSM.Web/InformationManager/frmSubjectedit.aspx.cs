/****************************************************************************
 * 
 * description:知识库类别管理页面操作
 * 
 * 
 * 
 * Create by:
 * Create Date:2007-09-22
 * *************************************************************************/
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
using Epower.DevBase.BaseTools;
using Epower.DevBase.Organization;
using Epower.DevBase.Organization.Base;
using Epower.DevBase.Organization.SqlDAL;
using Epower.ITSM.SqlDAL;

namespace Epower.ITSM.Web.InformationManager
{
	/// <summary>
    /// frmSubjectedit 的摘要说明。
	/// </summary>
    public partial class frmSubjectedit : BasePage
	{
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, System.EventArgs e)
        {
            // 在此处放置用户代码以初始化页面
            CtrTitle.Title = "知识类别管理";

            if (!IsPostBack)
            {
                string sSubjectId = "0";
                if (this.Request.QueryString["SubjectID"] != null)
                {
                    sSubjectId = this.Request.QueryString["SubjectID"].ToString();
                    // 记录当前分类
                    Session["OldSubjectID"] = long.Parse(sSubjectId);

                    hidCatalogID.Value = sSubjectId;
                    LoadData(StringTool.String2Long(sSubjectId));
                }
                CtrSetUserOtherRight1.OperateType = 20;
                if (string.IsNullOrEmpty(sSubjectId))
                {
                    CtrSetUserOtherRight1.OperateID = 0;
                }
                else
                {
                    CtrSetUserOtherRight1.OperateID = int.Parse(sSubjectId);
                }
                //知识分级控制
                if (CommonDP.GetConfigValue("Other", "InformationLimit") != null && CommonDP.GetConfigValue("Other", "InformationLimit") == "1")
                {
                    string sFullID = "1";
                    sFullID = Inf_SubjectDP.GetSubjectFullID(long.Parse(sSubjectId));
                    if (sSubjectId == "1" || sFullID.Length > 6)    //如果不为根分类
                        this.trRight.Visible = false;
                    else
                        this.trRight.Visible = true;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lngCatalogID"></param>
        private void LoadData(long lngSubjectID)
        {
            DataTable dt = Inf_SubjectDP.GetSubjectByID(lngSubjectID);

            if (dt.Rows.Count < 1)
            {
                cmdAdd.Enabled = false;
                cmdSave.Enabled = false;
                cmdDelete.Enabled = false;
            }

            foreach(DataRow dr in dt.Rows)
            {
                txtSubjectName.Text = dr["CatalogName"].ToString();
                txtDesc.Text = dr["Remark"].ToString();
                hidPCatalogID.Value = dr["ParentID"].ToString();
                txtSortID.Text = dr["SortID"].ToString();
                if (hidPCatalogID.Value == "-1")	//(lngCatalogID == 1)//根分类
                {
                    txtPCatalogName.Text = "无";
                }
                if (hidPCatalogID.Value == "-1")
                {
                    //第一级 不容许修改
                    cmdSave.Enabled = false;
                    cmdDelete.Enabled = false;
                }
                txtPCatalogName.Text = Inf_SubjectDP.GetSubjectName(StringTool.String2Long(hidPCatalogID.Value));
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

        protected void cmdSave_Click(object sender, System.EventArgs e)
        {
            if (txtSubjectName.Text.Trim() == "")
            {
                labMsg.Text = "名称不能为空!";
                return;
            }
            else if (hidPCatalogID.Value == "0")
            {
                labMsg.Text = "请选择上级!";
                return;
            }
            else
            {
                labMsg.Text = "";
            }

            string sOrgID = string.Empty;
            if (Session["UserOrgID"] != null)
            {
                sOrgID = Session["UserOrgID"].ToString();
            }
            else
            {
                sOrgID = "-1";
            }

            try
            {
                string strSubjectID = Inf_SubjectDP.Save(StringTool.String2Long(hidCatalogID.Value),
                      txtSubjectName.Text.Trim(), StringTool.String2Long(hidPCatalogID.Value),
                      int.Parse(txtSortID.Text.Trim()), txtDesc.Text.Trim(), sOrgID);
                hidCatalogID.Value = strSubjectID;

               // Session["OldSubjectID"] = strSubjectID;

                //知识分级控制
                if (CommonDP.GetConfigValue("Other", "InformationLimit") != null && CommonDP.GetConfigValue("Other", "InformationLimit") == "1")
                {
                    string sFullID = "1";
                    sFullID = Inf_SubjectDP.GetSubjectFullID(long.Parse(strSubjectID));
                    if (strSubjectID == "1" || sFullID.Length > 6)    //如果不为根分类
                        this.trRight.Visible = false;
                    else
                        this.trRight.Visible = true;
                }

                //如过从查询分类界面打开，保存后关闭并刷新父窗体
                if (Request["CloseAction"] != null && Request["CloseAction"].ToString().Equals("1"))
                {
                    PageTool.AddJavaScript(this, "window.parent.returnValue = 'refresh';self.close();");
                }
                else
                {
                    PageTool.AddJavaScript(this, "window.parent.contents.location='frmInformationContent.aspx'");
                    PageTool.AddJavaScript(this, "window.parent.subjectinfo.location='frmSubjectedit.aspx?Subjectid=" + hidCatalogID.Value + "'");
                }
            }
            catch (Exception ee)
            {
                PageTool.MsgBox(this, ee.Message.ToString());
            }

        }

        protected void cmdAdd_Click(object sender, System.EventArgs e)
        {
            string sSubjectId = this.Request.QueryString["SubjectID"].ToString();
            txtSubjectName.Text = string.Empty;
            txtDesc.Text = string.Empty;
            hidPCatalogID.Value = string.Empty;
            hidCatalogID.Value = string.Empty;//清空分类标识

            //加载默认上级分类，默认主管，默认领导
            DataTable dt = Inf_SubjectDP.GetSubjectByID(StringTool.String2Long(sSubjectId));
            foreach (DataRow dr in dt.Rows)
            {
                hidPCatalogID.Value = sSubjectId == null ? "0" : sSubjectId.Trim();
                txtPCatalogName.Text = dr["CatalogName"].ToString();
                if (hidPCatalogID.Value != "-1")
                {
                    //第一级，可以新增下一级 
                    cmdSave.Enabled = true;
                }
            }
            txtSortID.Text = "-1";
            this.trRight.Visible = false;
        }

        protected void cmdDelete_Click(object sender, System.EventArgs e)
        {
            try
            {
                Inf_SubjectDP.Delete(long.Parse(hidCatalogID.Value.Trim()));

                this.trRight.Visible = false;
                //Session["OldSubjectID"] = strSubjectID;

                //如过从查询分类界面打开，保存后关闭并刷新父窗体
                if (Request["CloseAction"] != null && Request["CloseAction"].ToString().Equals("1"))
                    PageTool.AddJavaScript(this, "window.parent.returnValue = 'refresh';self.close();");
                else
                    PageTool.AddJavaScript(this, "window.parent.contents.location='frmInformationContent.aspx';window.location='about:blank'");
            }
            catch (Exception ee)
            {
                PageTool.MsgBox(this, "删除分类时出现错误，错误为：" + ee.Message.ToString());
            }
        }
    }
}
