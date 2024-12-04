/*******************************************************************
 * 版权所有：
 * Description：事件单快速登单页面
 * Create By  ：SuperMan
 * Create Date：2011-08-18
 * *****************************************************************/
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
using Epower.DevBase.Organization.SqlDAL;
using Epower.ITSM.SqlDAL;
using Epower.ITSM.Base;
using EpowerCom;
using System.Xml;


namespace Epower.ITSM.Web.Forms
{
    /// <summary>
    /// frmActorCondList 的摘要说明。
    /// </summary>
    public partial class CST_Issue_Base_Fast : BasePage
    {
      
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

        #region 变量定义

        private static string strCondID = "";
        private static string strCondIDValue = "";

        #endregion

        #region 窗体加载

        protected void Page_Load(object sender, System.EventArgs e)
        {
            SetLitText();

            if (Request.QueryString["CondID"] != null)
            {
                //如果事件请求模板ID不为空
                strCondID = Request.QueryString["CondID"].ToString();

                EA_ShortCutTemplateDP ee = new EA_ShortCutTemplateDP();
                this.txtModelName.Text = ee.GetTemplsByID(strCondID).Rows[0]["TemplateName"].ToString();
                this.labModelName.Text = ee.GetTemplsByID(strCondID).Rows[0]["TemplateName"].ToString();
            }

            if (!Page.IsPostBack)
            {
                LoadIssueTemplaties();
                CheckTemp();
                if (CheckRight(Constant.CustomerService) == false)
                {
                    cmdOK.Enabled = false;

                    txtCustAddr.Enabled = false;
                    cmdCust.Disabled = true;
                    txtEqu.Enabled = false;
                    cmdEqu.Disabled = true;
                }
            }
        }

        /// <summary>
        /// 设置自定义名称
        /// 廖世进 2013-05-15 添加
        /// </summary>
        void SetLitText()
        {
            this.LitCustName.Text = PageDeal.GetLanguageValue("CST_CustName");
            this.LitEquipmentName.Text = PageDeal.GetLanguageValue("CST_EquName");
            this.LitContent.Text = PageDeal.GetLanguageValue("CST_Content");

        }
        #endregion

        #region 确定

        protected void cmdOK_Click(object sender, EventArgs e)
        {
            #region 将要传递的参数拼成xml对象

            FieldValues fv = new FieldValues();

            fv.Add("ReqContext", txtContext.Text.Trim());              //详细描述
            fv.Add("ReqCustID", hidCustID.Value.Trim());               //客户ID
            fv.Add("ReqCustName", txtCustAddr.Text.Trim());            //客户名称
            fv.Add("EquID", hidEqu.Value.Trim());                      //资产ID
            fv.Add("ReqEquName", txtEqu.Text.Trim());                  //资产名称

            Session["IssueShortCutXml"] = fv.GetXmlObject().InnerXml;

            #endregion

            string temp = strCondIDValue;
            string flowmodelid = "0";
            string templateid = "0";
            flowmodelid = temp.Split("|".ToCharArray())[1];
            templateid = temp.Split("|".ToCharArray())[0];
            Response.Write("<script>window.open('../Forms/oa_AddNew.aspx?flowmodelid=" + flowmodelid + "&IsFirst=true&ep=" + templateid + "','MainFrame','scrollbars=no,status=no ,resizable=no,width=680,height=500');window.close();</script>");
        }

        #endregion

        #region 导入模板方法

        private void LoadIssueTemplaties()
        {
            ddlTemplaties.Items.Clear();
            EA_ShortCutTemplateDP ee = new EA_ShortCutTemplateDP();
            DataTable dt = ee.GetMyTemplaties(strCondID, (long)Session["UserID"], false);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["TemplateID"].ToString().Trim() == strCondID.Trim())
                {
                    strCondIDValue = dt.Rows[i]["IDAndOFlowModelID"].ToString().Trim();
                }
            }

            ddlTemplaties.DataSource = dt.DefaultView;
            ddlTemplaties.DataTextField = "TemplateName";
            ddlTemplaties.DataValueField = "IDAndOFlowModelID";
            ddlTemplaties.DataBind();

            if (dt.Rows.Count == 0)
                cmdOK.Enabled = false;
        }

        #endregion

        #region 检查模板

        private void CheckTemp()
        {
            string flowmodelid = "0";
            flowmodelid = strCondIDValue.Split("|".ToCharArray())[1];//此流程模型是用于自助服务请从新设置
            bool flag = AppFieldConfigDP.CheckFlowModel(long.Parse(flowmodelid), 9351);
            if (!flag)
            {
                lblMsg.Text = "此流程模型是用于自助服务请从新设置";
            }
        }

        #endregion

        #region 检查权限 CheckRight
        /// <summary>
        /// 检查权限

        /// </summary>
        /// <param name="OperatorID"></param>
        /// <returns></returns>
        private bool CheckRight(long OperatorID)
        {
            RightEntity re = (RightEntity)((Hashtable)Session["UserAllRights"])[OperatorID];
            if (re == null)
                return false;
            else
                return re.CanRead;
        }
        #endregion
    }
}
