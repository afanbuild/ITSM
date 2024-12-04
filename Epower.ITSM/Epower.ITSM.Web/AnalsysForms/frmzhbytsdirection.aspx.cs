/****************************************************************************
 * 
 * description:Ͷ�����Ʒ���
 * 
 * 
 * 
 * Create by:
 * Create Date:2007-08-01
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
using System.Drawing;
using Epower.ITSM.SqlDAL;
using Epower.DevBase.BaseTools;
using Epower.ITSM.SqlDAL.Flash;


namespace Epower.ITSM.Web.AnalsysForms
{
    public partial class frmzhbytsdirection : BasePage
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
            UserPicker1.OnChangeScript = "UserPickerChange();";
            SetParentButtonEvent();
            CataKind.mySelectedIndexChanged += new EventHandler(CataKind_mySelectedIndexChanged);
            if (!IsPostBack)
            {
                //��ȡdbo�ؼ���ȱʡֵ
                SetYearSelectDboValue();

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

        #region ѡ��Ͷ������ʱ�����¼� CataKind_mySelectedIndexChanged
        /// <summary>
        /// ѡ��Ͷ������ʱ�����¼�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void CataKind_mySelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }
        #endregion

        protected void dpdYear_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            LoadData();
        }



        protected void dpdManageOffice_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        /// <summary>
        /// 
        /// </summary>
        private void SetYearSelectDboValue()
        {
            DataTable dt = BYTSDP.GetBYTSYears();
            DataView dv = new DataView(dt);
           // dv.Sort = "years";
            
            dpdYear.DataSource = dv;
            dpdYear.DataTextField = "years";
            dpdYear.DataValueField = "years";
            dpdYear.DataBind();

            dt.Dispose();
        }

        private void LoadData()
        {
            

            int nYear = Epower.DevBase.BaseTools.StringTool.String2Int(dpdYear.SelectedValue);

            long lngBYKind = 0;
            lngBYKind = (CataKind.CatelogID == CataKind.RootID) ? 0 : CataKind.CatelogID;


            Font f = new Font("����", 9);

            long lngDeptID = UserPicker1.UserID;
            string strManageOffic = UserPicker1.UserName;

            DataTable dt;
            if (nLastYear != nYear)
            {
                dt = BYTSDP.GetAnalysisBYTSDirectionGrid(nYear);
                grdTypeDirection.DataSource = dt.DefaultView;
                grdTypeDirection.DataBind();
                nLastYear = nYear;
                ViewState["LastYear"] = nYear;
            }

           

            dt = BYTSDP.GetAnalysisBYTSDirection(nYear, lngDeptID, lngBYKind);

            ReportDiv.InnerHtml = FlashCS.MSPublicFlashUrl3D(dt, "Ͷ������", "ȫ��Ͷ�����Ʒ���", "����", "�·�", "../FlashReoport/Flash/MSLine.swf", "100%", "248", true, 2);

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

                hw.WriteLine("<table><tr><td><font size=\"3\">Ͷ�����Ʒ���</font></td></tr></table>");
                this.dgTypesCount.RenderControl(hw);
                this.grdTypeDirection.RenderControl(hw);
                //this.tdResultChart.RenderControl(hw);


                hw.WriteEndTag("body");
                hw.WriteLine();
                hw.WriteEndTag("html");

                //�����.xls�ļ�
                Response.ContentType = "application/vnd.ms-excel";
                //Response.AddHeader("Content-Disposition","attachment; filename=�����¼����Ʒ���(�¼�����).xls");  --������
                Response.AppendHeader("Content-Disposition", "attachment;filename=" + System.Web.HttpUtility.UrlEncode("Ͷ�����Ʒ���", System.Text.Encoding.UTF8) + ".xls");
                Response.Charset = "utf-8";
                Response.Write(tw.ToString());
                Response.End();
            }
            catch
            {
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnOk_Click(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}
