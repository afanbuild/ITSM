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
using System.Drawing;
using Epower.ITSM.SqlDAL;
using Epower.DevBase.BaseTools;
using Epower.ITSM.SqlDAL.Flash;

namespace Epower.ITSM.Web.AnalsysForms
{
    public partial class frmzhcststatisfied : BasePage
    {

        int nLastYear = 0;
        decimal iItems = 0;
        decimal iFeedBack = 0;
        decimal iStatisfied = 0;
        //�ϴβ�ѯʱѡ������,�Ż���,���ⲻ��Ҫ�Ĳ�ѯ
        #region ���ø����尴ť�¼�SetParentButtonEvent
        /// <summary>
        /// ���ø����尴ť�¼�

        /// </summary>
        protected void SetParentButtonEvent()
        {

            this.Master.Master_Button_ExportExcel_Click += new Global_BtnClick(btnToExcel_Click);
            this.Master.ShowExportExcelButton(true);
            this.Master.MainID = "1";     //����ҳ���ID��ţ����Ϊ��ѯҳ�棬������Ϊ1
        }


        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            SetParentButtonEvent();
            this.ctrFCDServiceType.mySelectedIndexChanged += new EventHandler(ctrFCDServiceType_mySelectedIndexChanged);

            this.ctrFCDWTType.mySelectedIndexChanged += new EventHandler(ctrFCDWTType_mySelectedIndexChanged);

            if (!IsPostBack)
            {
                //��ȡdbo�ؼ���ȱʡֵ


                SetYearSelectDboValue();

                SetManageOfficeDboValue();

                SetMastCustomDboValue();

                LoadData();
            }
            else
            {
                if (ViewState["LastYear"] != null)
                {
                    nLastYear = (int)ViewState["LastYear"];
                }
            }
        }

        void ctrFCDServiceType_mySelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }



        void ctrFCDWTType_mySelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        protected void dpdYear_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            LoadData();
        }

        protected void dpdManageOffice_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        protected void dpdMastCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void SetYearSelectDboValue()
        {
            DataTable dt = ZHServiceDP.GetIssuesYears();
            DataView dv = new DataView(dt);
            dpdYear.DataSource = dv;
            dpdYear.DataTextField = "years";
            dpdYear.DataValueField = "years";
            dpdYear.DataBind();
            dt.Dispose();
        }


        private void SetManageOfficeDboValue()
        {
            DataTable dt = ZHServiceDP.GetManageOffices();
            DataView dv = new DataView(dt);
            dv.Sort = "deptid";

            dpdManageOffice.DataSource = dv;
            dpdManageOffice.DataTextField = "deptname";
            dpdManageOffice.DataValueField = "deptid";
            dpdManageOffice.DataBind();

            dpdManageOffice.Items.Insert(0, new ListItem("ȫ��", "0"));

            dt.Dispose();
        }

        /// <summary>
        /// 
        /// </summary>
        private void SetMastCustomDboValue()
        {
            DataTable dt = ZHServiceDP.GetMastCustomer();
            DataView dv = new DataView(dt);
            dv.Sort = "ID";
            dpdMastCustomer.DataSource = dv;
            dpdMastCustomer.DataTextField = "ShortName";
            dpdMastCustomer.DataValueField = "ID";
            dpdMastCustomer.DataBind();

            dpdMastCustomer.Items.Insert(0, new ListItem("ȫ��", "0"));

            dt.Dispose();
        }

        private void LoadData()
        {

            int nYear = Epower.DevBase.BaseTools.StringTool.String2Int(dpdYear.SelectedValue);
            long lngServiceTypeID = 0;
            long lngWTTypeID = 0;


            lngServiceTypeID = (ctrFCDServiceType.CatelogID == ctrFCDServiceType.RootID) ? 0 : ctrFCDServiceType.CatelogID;
            lngWTTypeID = (ctrFCDWTType.CatelogID == ctrFCDWTType.RootID) ? 0 : ctrFCDWTType.CatelogID;

            long lngDeptID = long.Parse(dpdManageOffice.SelectedValue);

            string strManageOffic = dpdManageOffice.SelectedItem.Text;


            DataTable dt = ZHServiceDP.GetAnalysisStatisfied(nYear, lngServiceTypeID, lngWTTypeID, lngDeptID, long.Parse(dpdMastCustomer.SelectedValue));


            ReportDiv.InnerHtml = FlashCS.MSPublicFlashUrl3D(dt, "���", "ȫ���������ȷ���", "���", "���", "../FlashReoport/Flash/MSLine.swf", "100%", "248", true, 0);

            //ReportDiv.InnerHtml = FlashCS.MSPublicFlashUrl3DRate(dt, "nYear", "ȫ���������ȷ���", "nYear", "nYear", "../FlashReoport/Flash/MSLine.swf", "100%", "248", true, 2);

            iItems = 0;
            iFeedBack = 0;
            iStatisfied = 0;

            dt = ZHServiceDP.GetAnalysisStatisfiedGrid(nYear, lngServiceTypeID, lngWTTypeID, lngDeptID, long.Parse(dpdMastCustomer.SelectedValue));

            #region ��ҳ��չʾ�б���Ӻϼ��� 2013-5-24 �������
            string strdivGrid = "";
            strdivGrid += "<table cellspacing=\"1\" cellpadding=\"1\"  border=\"0\" class=\"listContent\" border=\"1\" id=\"tableGrid\" style=\"width:100%;\">";
            strdivGrid += "<tr class=\"listTitleNew_1\" align=\"center\">";
            decimal num1 = 0;
            decimal num2 = 0;
            decimal num3 = 0;
            //for (int i = 0; i < dt.Columns.Count-2; i++)
            //{

            // if (i == 0)
            //{
            strdivGrid += "<td  style='width:16%;'>�ϼ�</td>";

            //}
            //if (i != 0)
            //{
            for (int j = 0; j < dt.Rows.Count; j++)
            {
                if (dt.Rows[j]["�¼�����"] != null && !string.IsNullOrEmpty(dt.Rows[j]["�¼�����"].ToString()))
                {
                    num1 += Convert.ToDecimal(dt.Rows[j]["�¼�����"].ToString());
                }
                if (dt.Rows[j]["�طô���"] != null && !string.IsNullOrEmpty(dt.Rows[j]["�طô���"].ToString()))
                {
                    num2 += Convert.ToDecimal(dt.Rows[j]["�طô���"].ToString());
                }
                if (dt.Rows[j]["�������"] != null && !string.IsNullOrEmpty(dt.Rows[j]["�������"].ToString()))
                {
                    num3 += Convert.ToDecimal(dt.Rows[j]["�������"].ToString());
                }
            }
            //}
            //}
            strdivGrid += "<td style='width:16%;'>" + num1.ToString() + "</td>";
            strdivGrid += "<td style='width:16%;'>" + num2.ToString() + "</td>";
            strdivGrid += "<td style='width:16%;'>" + num3.ToString() + "</td>";
            if (num2 != 0&&num1!=0)
            {

                strdivGrid += "<td style='width:16%;'>" + decimal.Round(((num2 / num1) * 100), 2).ToString() + "%" + "</td>";
            }
            else
            {
                strdivGrid += "<td style='width:16%;'>0.00%</td>";
            }
            if (num3 != 0&&num2!=0)
            {
                strdivGrid += "<td style='width:17%;'>" + decimal.Round(((num3 / num2) * 100), 2).ToString()+ "%" + "</td>";
            }
            else 
            {
                strdivGrid += "<td style='width:17%;'>0.00%</td>";
            }

            strdivGrid += "</tr>";
            strdivGrid += "</table>";

            divGrid.InnerHtml = strdivGrid;
            #endregion

            grdTypeDirection.DataSource = dt;
            //grdTypeDirection.DataSource = dt;
            grdTypeDirection.DataBind();
        }

        protected void btnToExcel_Click()
        {
            try
            {
                LoadData(); ;
                ToExcel();
            }
            catch { }
        }


        private void ToExcel()
        {
            try
            {
                System.IO.StringWriter tw = new System.IO.StringWriter();
                System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);

                hw.WriteFullBeginTag("html");
                hw.WriteLine();
                hw.WriteFullBeginTag("head");
                hw.WriteLine();
                hw.WriteLine("<meta http-equiv=Content-Type Content=text/html; charset=utf-8>");
                hw.WriteEndTag("head");
                hw.WriteLine();
                hw.WriteFullBeginTag("body");
                hw.WriteLine();

                hw.WriteLine("<table><tr><td><font size=\"3\">ȫ���������ȷ���</font></td></tr></table>");
                // ReportDiv.RenderControl(hw);
                hw.WriteLine("<br />");
                this.grdTypeDirection.RenderControl(hw);
                //this.UltraChart1.RenderControl(hw);

                hw.WriteEndTag("body");
                hw.WriteLine();
                hw.WriteEndTag("html");

                //�����.xls�ļ�
                Response.ContentType = "application/vnd.ms-excel";
                Response.AppendHeader("Content-Disposition", "attachment;filename=" + System.Web.HttpUtility.UrlEncode("ȫ���������ȷ���", System.Text.Encoding.UTF8) + ".xls");
                Response.Charset = "utf-8";
                Response.Write(tw.ToString());
                Response.End();
            }
            catch
            {
            }
        }



        protected void grdTypeDirection_ItemDataBound(object sender, DataGridItemEventArgs e)
        {

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                if (e.Item.Cells[1].Text != "&nbsp;")
                {
                    iItems += decimal.Parse(e.Item.Cells[1].Text);
                }
                if (e.Item.Cells[2].Text != "&nbsp;")
                {
                    iFeedBack += decimal.Parse(e.Item.Cells[2].Text);
                }
                if (e.Item.Cells[3].Text != "&nbsp;")
                {
                    iStatisfied += decimal.Parse(e.Item.Cells[3].Text);
                }
                if (iFeedBack > 0)
                {
                    decimal iAvgStatis = iStatisfied / iFeedBack;
                    lblOnTimeRate.Text = iAvgStatis.ToString("00.00%");
                }
            }
        }

    }
}
