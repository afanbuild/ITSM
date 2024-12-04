/*******************************************************************
 * ��Ȩ���У�
 * Description���¼������ٵǵ�ҳ��
 * Create By  ��SuperMan
 * Create Date��2011-08-18
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
    /// frmActorCondList ��ժҪ˵����
    /// </summary>
    public partial class CST_Issue_Base_Fast : BasePage
    {
      
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

        #region ��������

        private static string strCondID = "";
        private static string strCondIDValue = "";

        #endregion

        #region �������

        protected void Page_Load(object sender, System.EventArgs e)
        {
            SetLitText();

            if (Request.QueryString["CondID"] != null)
            {
                //����¼�����ģ��ID��Ϊ��
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
        /// �����Զ�������
        /// ������ 2013-05-15 ���
        /// </summary>
        void SetLitText()
        {
            this.LitCustName.Text = PageDeal.GetLanguageValue("CST_CustName");
            this.LitEquipmentName.Text = PageDeal.GetLanguageValue("CST_EquName");
            this.LitContent.Text = PageDeal.GetLanguageValue("CST_Content");

        }
        #endregion

        #region ȷ��

        protected void cmdOK_Click(object sender, EventArgs e)
        {
            #region ��Ҫ���ݵĲ���ƴ��xml����

            FieldValues fv = new FieldValues();

            fv.Add("ReqContext", txtContext.Text.Trim());              //��ϸ����
            fv.Add("ReqCustID", hidCustID.Value.Trim());               //�ͻ�ID
            fv.Add("ReqCustName", txtCustAddr.Text.Trim());            //�ͻ�����
            fv.Add("EquID", hidEqu.Value.Trim());                      //�ʲ�ID
            fv.Add("ReqEquName", txtEqu.Text.Trim());                  //�ʲ�����

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

        #region ����ģ�巽��

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

        #region ���ģ��

        private void CheckTemp()
        {
            string flowmodelid = "0";
            flowmodelid = strCondIDValue.Split("|".ToCharArray())[1];//������ģ�������������������������
            bool flag = AppFieldConfigDP.CheckFlowModel(long.Parse(flowmodelid), 9351);
            if (!flag)
            {
                lblMsg.Text = "������ģ�������������������������";
            }
        }

        #endregion

        #region ���Ȩ�� CheckRight
        /// <summary>
        /// ���Ȩ��

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
