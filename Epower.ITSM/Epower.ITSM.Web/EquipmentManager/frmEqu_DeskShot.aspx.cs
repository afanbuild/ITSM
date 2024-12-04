using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Drawing;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using Epower.ITSM.SqlDAL;
using Epower.ITSM.Base;

namespace Epower.ITSM.Web.EquipmentManager
{
    public partial class frmEqu_DeskShot : BasePage
    {
        bool blnComp = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            long lngID = 0;
            long lngVersion = -1;
            if (this.Request.QueryString["id"] != null)
                lngID = long.Parse(Request.QueryString["id"]);

            if (this.Request.QueryString["Version"] != null)
                lngVersion = long.Parse(Request.QueryString["Version"] == "" ? "-1" : Request.QueryString["Version"]);

            if (this.Request.QueryString["Compare"] != null)
                blnComp = true;
            if (!IsPostBack)
            {
                if (lngID != 0)
                {
                    //������ʾ
                    PageDeal.SetLanguage(this);
                    LoadData(lngID, lngVersion);

                }
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            //���HTML
            System.IO.StringWriter html = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter tw = new System.Web.UI.HtmlTextWriter(html);
            base.Render(tw);
            Response.Clear();
            //�����Ƿ������� 0 ��, 1 ��
            string sTmp = html.ToString();
            Response.Write("<table width='550px' height='100%'><tr><td>" + sTmp.Trim() + "<td></tr></table>");
            Response.Flush();
            Response.End();
        }

        private void LoadData(long lngID, long lngVersion)
        {

            Equ_DeskDP ee = new Equ_DeskDP();

            if (lngVersion == -1)
            {
                ee = ee.GetReCorded(lngID);

                ShowVersion.Visible = false;
            }
            else
            {
                ee = ee.GetReCordedForVersion(lngID, lngVersion);

                lblVersion.Text = ee.Version.ToString();
            }

            lblEquStatus.Text = CatalogDP.GetCatalogName((long)ee.EquStatusID);//�ʲ�״̬
            lblEquDeskName.Text = ee.Name.ToString();//�ʲ�����
            lblCode.Text = ee.Code.ToString();//�ʲ����
            lblCustom.Text = ee.CostomName;//�����ͻ�
            lblPartBankName.Text = ee.partBankName;//ά������
            lblPartBranchName.Text = ee.partBranchName;//ά������
            lblMastCust.Text = Br_MastCustomerDP.GetMastCustName(long.Parse(ee.Mastcustid.ToString()));
            lblListName.Text = ee.ListName;//�ʲ�Ŀ¼
            lblServiceBeginTime.Text = ee.ServiceBeginTime.ToShortDateString();//��ʼ����
            lblServiceEndTime.Text = ee.ServiceEndTime.ToShortDateString();//��������
            if (blnComp == true)
            {
                //�Ƚ�ǰһ���汾
                Equ_DeskDP ePre = new Equ_DeskDP();
                if (lngVersion != -1)
                {
                    ePre = ePre.GetReCordedForPreVersion(lngID, lngVersion);
                }
                else
                {
                    ePre = ePre.GetReCordedForLastHistory(lngID);
                }

                if (ePre.ID != 0)
                {
                    #region �Ƚϰ汾����

                    if (lblPartBankName.Text != ePre.partBankName)//ά������
                        lblPartBankName.ForeColor = Color.Red;

                    if (lblPartBranchName.Text != ePre.partBranchName)//ά������
                        lblPartBranchName.ForeColor = Color.Red;

                    //����λ
                    if(lblMastCust.Text != Br_MastCustomerDP.GetMastCustName(long.Parse(ePre.Mastcustid.ToString())))
                        lblMastCust.ForeColor = Color.Red;

                    if (lblListName.Text != ePre.ListName.ToString())//�ʲ�Ŀ¼
                        lblListName.ForeColor = Color.Red;

                    if (lblEquStatus.Text != CatalogDP.GetCatalogName((long)ePre.EquStatusID))//�ʲ�״̬
                        lblEquStatus.ForeColor = Color.Red;

                    if (lblEquDeskName.Text != ePre.Name.ToString())//�ʲ�����
                        lblEquDeskName.ForeColor = Color.Red;

                    if (lblCode.Text != ePre.Code.ToString())//�ʲ�����
                        lblCode.ForeColor = Color.Red;

                    if (lblCustom.Text != ePre.CostomName)//�����ͻ�
                        lblCustom.ForeColor = Color.Red;

                    if (lblServiceBeginTime.Text != ePre.ServiceBeginTime.ToShortDateString())//��ʼ����
                        lblServiceBeginTime.ForeColor = Color.Red;

                    if (lblServiceEndTime.Text != ePre.ServiceEndTime.ToShortDateString())//��������
                        lblServiceEndTime.ForeColor = Color.Red;

                    //�ؼ�����Ƚ�
                 //   DymSchemeCtr1.CompControlXmlValue = ePre.ConfigureValue;

                    #endregion
                }
            }

            //DymSchemeCtr1.ControlXmlValue = ee.ConfigureValue;
            if (lngVersion == -1)
            {
                DymSchemeCtrList1.ReadOnly = true;
                DymSchemeCtrList1.EquID = long.Parse(ee.ID.ToString());
                DymSchemeCtrList1.EquCategoryID = long.Parse(ee.CatalogID.ToString());
            }
            else
            { 
                
                DymSchemeCtrList1.ReadOnly = true;
                DymSchemeCtrList1.EquID = long.Parse(ee.ID.ToString());
                DymSchemeCtrList1.isVersion = true;
                DymSchemeCtrList1.Version = lngVersion;                
                DymSchemeCtrList1.EquCategoryID = long.Parse(ee.CatalogID.ToString());                
            }

        }
    }
}
