/****************************************************************************
 * 
 * description:���񼶱�չʾ
 * 
 * 
 * 
 * Create by:
 * Create Date:2007-11-02
 * *************************************************************************/
using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Text;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Epower.ITSM.SqlDAL;
using Epower.ITSM.Base;


namespace Epower.ITSM.Web.AppForms
{
    public partial class frmServiceProtocolShow : BasePage
    {
        /// <summary>
        /// �ͻ����
        /// </summary>
        protected string sKBID
        {
            get
            {
                return Request["ID"] != null ? Request["ID"].ToString() : "0";
            }
        }

        /// <summary>
        /// �ʲ����
        /// </summary>
        protected string EquID
        {
            get
            {
                return Request["EquID"] != null ? Request["EquID"].ToString() : "0";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected string UserName
        {
            get
            {
                return Session["PersonName"] != null ? Session["PersonName"].ToString() : string.Empty;
            }
        }

        /// <summary>
        /// �ͻ���չ����ģ��ID
        /// </summary>
        protected long CustSchemaID
        {
            get
            {
                if (ViewState["CustSchemaID"] != null)
                    return long.Parse(ViewState["CustSchemaID"].ToString());
                else
                    return 0;
            }
            set
            {
                ViewState["CustSchemaID"] = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (!IsPostBack)
            {
                if ((sKBID != "0" && sKBID != "-1") || (EquID != "0" && EquID != "-1"))
                {
                    LoadData();
                    this.Master.TableVisible = false;
                    PageDeal.SetLanguage(this.Controls[0]);
                    tbMessage.Visible = false;
                }
                else
                {
                    this.Master.TableVisible = false;
                    tbMessage.Visible = true;
                    TableImg1.Visible = false;
                    Table1.Visible = false;
                    TableImg2.Visible = false;
                    Table2.Visible = false;
                    ctrattachment1.Visible = false;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void LoadData()
        {

            //������񼶱�
            if (sKBID != "0" && sKBID != "-1")
            {
                Br_ECustomerDP ee = new Br_ECustomerDP();
                ee = ee.GetReCorded(long.Parse(sKBID));
                if (ee == null)
                {
                    return;
                }
                this.LblTitle.Text = ee.ShortName.Trim();
                lblFullName.Text = ee.FullName.Trim();
                lblCustomCode.Text = ee.CustomCode.Trim();
                lblCustomerType.Text = ee.CustomerTypeName.Trim();
                lblCustDeptName.Text = ee.CustDeptName == null ? "" : ee.CustDeptName.Trim();

                lblContact.Text = ee.LinkMan1.Trim();
                lblCTel.Text = ee.Tel1.Trim();
                lblCustAddress.Text = ee.Address.Trim();
                lblRights.Text = ee.Rights == null ? "" : ee.Rights.Trim();
                lblRemark.Text = ee.Remark == null ? "" : ee.Remark.Trim();
                lblEmail.Text = ee.Email == null ? "" : ee.Email.Trim();
                if (lblEmail.Text.Length > 0)
                {
                    lblEmail.NavigateUrl = "mailto:" + lblEmail.Text;
                }


                Br_MastCustomerDP pBr_MastCustomerDP = new Br_MastCustomerDP();
                pBr_MastCustomerDP = pBr_MastCustomerDP.GetReCorded(long.Parse(ee.MastCustID.ToString()));
                if (pBr_MastCustomerDP == null)
                {
                    return;
                }
                lblMastShortName.Text = pBr_MastCustomerDP.ShortName.ToString();               
                this.LblContent.Text = string.IsNullOrEmpty(pBr_MastCustomerDP.ServiceProtocol) ? "" : pBr_MastCustomerDP.ServiceProtocol.ToString();

                #region �ͻ���չģ��
                DataTable dt = new DataTable();
                dt = Br_SubjectDP.GetSubject();
                if (dt != null && dt.Rows.Count > 0)
                {
                    //ģ��
                    CustSchemaID = long.Parse(Br_SubjectDP.GetSubject().Rows[0]["CatalogID"].ToString());
                }
                #endregion

                //�ͻ�������չ��
                CustSchemeCtr1.ControlXmlValue = ee.SchemaValue;
                CustSchemeCtr1.BrCategoryID = CustSchemaID;

            }
            else
            {
                TableImg1.Visible = false;
                Table1.Visible = false;
            }


            //�ʲ�������Ϣ
            if (EquID != "0" && EquID != "-1")
            {
                Equ_DeskDP eeEqu = new Equ_DeskDP();
                eeEqu = eeEqu.GetReCorded(long.Parse(EquID));
                if (eeEqu == null)
                {
                    return;
                }
                this.lblEqu.Text = eeEqu.Name;
                lblCode.Text = eeEqu.Code.ToString();//�ʲ����
                lblListName.Text = eeEqu.ListName;//�ʲ�Ŀ¼
                lblEquDeskName.Text = eeEqu.Name;//�ʲ�����
                lblCustom.Text = eeEqu.CostomName.ToString();//�����ͻ�
                lblServiceBeginTime.Text = eeEqu.ServiceBeginTime.ToString("yyyy-MM-dd");//���޿�ʼ����
                lblServiceEndTime.Text = eeEqu.ServiceEndTime.ToString("yyyy-MM-dd");//���޽�������
                lblEquStatus.Text = eeEqu.EquStatusName.ToString();//�ʲ�״̬
                lblPartBankName.Text = eeEqu.partBankName;//ά������
                lblPartBranchName.Text = eeEqu.partBranchName;//ά������

                hidProvideID.Value = eeEqu.Provide.ToString();
                DymSchemeCtr1.EquID = long.Parse(eeEqu.ID.ToString());
                DymSchemeCtr1.ReadOnly = true;
                DymSchemeCtr1.EquCategoryID = (long)eeEqu.CatalogID;

                lblConfigureInfo.Text = eeEqu.ConfigureInfo.ToString();

                #region �������ؼ���ֵ
                ctrattachment1.AttachmentType = eOA_AttachmentType.eZC;
                ctrattachment1.FlowID = (long)eeEqu.ID;
                ctrattachment1.ReadOnly = true;
                #endregion
            }
            else
            {
                TableImg2.Visible = false;
                Table2.Visible = false;
                ctrattachment1.Visible = false;
            }

        }
    }
}
