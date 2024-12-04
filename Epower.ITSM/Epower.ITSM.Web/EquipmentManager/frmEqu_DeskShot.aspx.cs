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
                    //设置显示
                    PageDeal.SetLanguage(this);
                    LoadData(lngID, lngVersion);

                }
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            //输出HTML
            System.IO.StringWriter html = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter tw = new System.Web.UI.HtmlTextWriter(html);
            base.Render(tw);
            Response.Clear();
            //返回是否有新增 0 无, 1 有
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

            lblEquStatus.Text = CatalogDP.GetCatalogName((long)ee.EquStatusID);//资产状态
            lblEquDeskName.Text = ee.Name.ToString();//资产名称
            lblCode.Text = ee.Code.ToString();//资产编号
            lblCustom.Text = ee.CostomName;//所属客户
            lblPartBankName.Text = ee.partBankName;//维护机构
            lblPartBranchName.Text = ee.partBranchName;//维护部门
            lblMastCust.Text = Br_MastCustomerDP.GetMastCustName(long.Parse(ee.Mastcustid.ToString()));
            lblListName.Text = ee.ListName;//资产目录
            lblServiceBeginTime.Text = ee.ServiceBeginTime.ToShortDateString();//开始日期
            lblServiceEndTime.Text = ee.ServiceEndTime.ToShortDateString();//结束日期
            if (blnComp == true)
            {
                //比较前一个版本
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
                    #region 比较版本区别

                    if (lblPartBankName.Text != ePre.partBankName)//维护机构
                        lblPartBankName.ForeColor = Color.Red;

                    if (lblPartBranchName.Text != ePre.partBranchName)//维护部门
                        lblPartBranchName.ForeColor = Color.Red;

                    //服务单位
                    if(lblMastCust.Text != Br_MastCustomerDP.GetMastCustName(long.Parse(ePre.Mastcustid.ToString())))
                        lblMastCust.ForeColor = Color.Red;

                    if (lblListName.Text != ePre.ListName.ToString())//资产目录
                        lblListName.ForeColor = Color.Red;

                    if (lblEquStatus.Text != CatalogDP.GetCatalogName((long)ePre.EquStatusID))//资产状态
                        lblEquStatus.ForeColor = Color.Red;

                    if (lblEquDeskName.Text != ePre.Name.ToString())//资产名称
                        lblEquDeskName.ForeColor = Color.Red;

                    if (lblCode.Text != ePre.Code.ToString())//资产编码
                        lblCode.ForeColor = Color.Red;

                    if (lblCustom.Text != ePre.CostomName)//所属客户
                        lblCustom.ForeColor = Color.Red;

                    if (lblServiceBeginTime.Text != ePre.ServiceBeginTime.ToShortDateString())//开始日期
                        lblServiceBeginTime.ForeColor = Color.Red;

                    if (lblServiceEndTime.Text != ePre.ServiceEndTime.ToShortDateString())//结束日期
                        lblServiceEndTime.ForeColor = Color.Red;

                    //控件里面比较
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
