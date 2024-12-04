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
    public partial class frmzhcstdirection : BasePage
    {

        int nLastYear = 0;
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

        protected void Page_Load(object sender, System.EventArgs e)
        {
            SetParentButtonEvent();
            this.ctrFCDServiceType.mySelectedIndexChanged += new EventHandler(ctrFCDServiceType_mySelectedIndexChanged);
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
           // dv.Sort = "years";
            
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
            long lngServiceTypeID = (ctrFCDServiceType.CatelogID == ctrFCDServiceType.RootID) ? 0 : ctrFCDServiceType.CatelogID;

            long lngDeptID = long.Parse(dpdManageOffice.SelectedValue);

            string strManageOffic = dpdManageOffice.SelectedItem.Text;

            Font f = new Font("����", 9);

            DataTable dt;
            if (nLastYear != nYear)
            {
                nLastYear = nYear;
                ViewState["LastYear"] = nYear;
            }                     

            dt = ZHServiceDP.GetAnalysisDirection(nYear, lngServiceTypeID, lngDeptID, long.Parse(dpdMastCustomer.SelectedValue));

            ReportDiv.InnerHtml = FlashCS.MSPublicFlashUrl3D(dt, "�·�", "������ݷ�������", "����", "�·�", "../FlashReoport/Flash/MSLine.swf", "100%", "248", true, 0);

            #region ��ҳ��չʾ�б���Ӻϼ��� 2013-5-24 ���� ���
            DataRow newDR = dt.NewRow();
            int count1 = 0;
            int count2 = 0;
            int count3 = 0;
            int count4 = 0;
            int count5 = 0;
            int count6 = 0;
            int count7 = 0;
            int count8 = 0;
            int count9 = 0;
            int count10 = 0;
            int count11 = 0;
            int count12 = 0;
            foreach (DataRow dr in dt.Rows)
            {
                count1 += Convert.ToInt32(dr["һ��"].ToString());
                count2 += Convert.ToInt32(dr["����"].ToString());
                count3 += Convert.ToInt32(dr["����"].ToString());
                count4 += Convert.ToInt32(dr["����"].ToString());
                count5 += Convert.ToInt32(dr["����"].ToString());
                count6 += Convert.ToInt32(dr["����"].ToString());
                count7 += Convert.ToInt32(dr["����"].ToString());
                count8 += Convert.ToInt32(dr["����"].ToString());
                count9 += Convert.ToInt32(dr["����"].ToString());
                count10 += Convert.ToInt32(dr["ʮ��"].ToString());
                count11 += Convert.ToInt32(dr["ʮһ��"].ToString());
                count12 += Convert.ToInt32(dr["ʮ����"].ToString());
            }
            newDR["�·�"] = "�ϼ�";
            newDR["һ��"] = count1;
            newDR["����"] = count2;
            newDR["����"] = count3;
            newDR["����"] = count4;
            newDR["����"] = count5;
            newDR["����"] = count6;
            newDR["����"] = count7;
            newDR["����"] = count8;
            newDR["����"] = count9;
            newDR["ʮ��"] = count10;
            newDR["ʮһ��"] = count11;
            newDR["ʮ����"] = count12;
            dt.Rows.Add(newDR);
            #endregion

            dgTypesCount.DataSource = dt.DefaultView;
            dgTypesCount.DataBind();


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

                hw.WriteLine("<table><tr><td><font size=\"3\">������ݷ�������</font></td></tr></table>");
                this.dgTypesCount.RenderControl(hw);
               // this.tdResultChart.RenderControl(hw);


                hw.WriteEndTag("body");
                hw.WriteLine();
                hw.WriteEndTag("html");

                //�����.xls�ļ�
                Response.ContentType = "application/vnd.ms-excel";
                //Response.AddHeader("Content-Disposition","attachment; filename=�����¼����Ʒ���(�¼�����).xls");  --������
                Response.AppendHeader("Content-Disposition", "attachment;filename=" + System.Web.HttpUtility.UrlEncode("������ݷ�������", System.Text.Encoding.UTF8) + ".xls");
                Response.Charset = "utf-8";
                Response.Write(tw.ToString());
                Response.End();
            }
            catch
            {
            }
        }

     
    }
}
