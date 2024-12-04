/****************************************************************************
 * 
 * description:����ͳ�Ʒ���
 * 
 * 
 * 
 * Create by:
 * Create Date:2007-08-03
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

namespace Epower.ITSM.Web.AnalsysForms
{
    public partial class frmzhCstMaterial : BasePage
    {
        #region ���ø����尴ť�¼�SetParentButtonEvent
        /// <summary>
        /// ���ø����尴ť�¼�

        /// </summary>
        protected void SetParentButtonEvent()
        {
            this.Master.Master_Button_ExportExcel_Click += new Global_BtnClick(btnToExcel_Click);
            this.Master.ShowExportExcelButton(true);
        }


        #endregion

        #region ҳ���ʼ�� Page_Load
        /// <summary>
        /// ҳ���ʼ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, System.EventArgs e)
        {
            SetParentButtonEvent();
            this.ctrFCDServiceType.mySelectedIndexChanged += new EventHandler(ctrFCDServiceType_mySelectedIndexChanged);
            this.CtrFCDDealStatus.mySelectedIndexChanged += new EventHandler(CtrFCDDealStatus_mySelectedIndexChanged);

            txtBeginDate.Attributes["onblur"] = "if(isDate(document.all." + txtBeginDate.ClientID + ".value) == false){document.all." + txtBeginDate.ClientID + ".value = '" + txtBeginDate.Text + "';return false }else{if(document.all." + txtBeginDate.ClientID + ".value != '" + txtBeginDate.Text + "')__doPostBack('datarange', '');};";
            txtEndDate.Attributes["onblur"] = "if(isDate(document.all." + txtEndDate.ClientID + ".value) == false){document.all." + txtEndDate.ClientID + ".value = '" + txtEndDate.Text + "';return false }else{if(document.all." + txtEndDate.ClientID + ".value != '" + txtEndDate.Text + "')__doPostBack('datarange', '');};";
            if (!IsPostBack)
            {
                //��ȡdbo�ؼ���ȱʡֵ
                //������ʼ����
                string sQueryBeginDate = "0";
                if (CommonDP.GetConfigValue("Other", "QueryBeginDate") != null)
                    sQueryBeginDate = CommonDP.GetConfigValue("Other", "QueryBeginDate").ToString();
                if (sQueryBeginDate == "0")
                {
                    txtBeginDate.Text = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
                }
                else
                {
                    txtBeginDate.Text = DateTime.Parse(sQueryBeginDate).ToString("yyyy-MM-dd");
                }
                txtEndDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
                imgBeginDate.Attributes.Add("onclick", "fPopUpDlg('../Controls/Calendar/calendar.htm',document.all." + txtBeginDate.ClientID + ", 'winpop', 234, 261);__doPostBack('datarange', '')");
                imgEndDate.Attributes.Add("onclick", "fPopUpDlg('../Controls/Calendar/calendar.htm',document.all." + txtEndDate.ClientID + ", 'winpop', 234, 261);__doPostBack('datarange', '');");
                SetManageOfficeDboValue();  //�������β���
                SetMastCustomDboValue();
                LoadData();
            }
            else
            {
                if (Request.Form["__EVENTTARGET"] == "datarange")
                {
                    LoadData();
                }
            }
        }

        #endregion

        #region ѡ�����״̬ʱ�����¼� CtrFCDDealStatus_mySelectedIndexChanged
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void CtrFCDDealStatus_mySelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }
        #endregion 

        #region ѡ��������ʱ�����¼� ctrFCDServiceType_mySelectedIndexChanged
        /// <summary>
        /// ѡ��������ʱ�����¼�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ctrFCDServiceType_mySelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }
        #endregion 

        #region ѡ�����β���ʱ�����¼� dpdManageOffice_SelectedIndexChanged
        /// <summary>
        /// ѡ�����β���ʱ�����¼�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dpdManageOffice_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }
        #endregion 

        #region �������β��� SetManageOfficeDboValue
        /// <summary>
        /// �������β���
        /// </summary>
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
        #endregion 

        #region �������� LoadData
        /// <summary>
        /// ��������
        /// </summary>
        private void LoadData()
        {
            string strBeginDate = txtBeginDate.Text.Trim();
            string strEndDate = txtEndDate.Text.Trim();
            long lngDeptID = long.Parse(dpdManageOffice.SelectedValue);
            string strManageOffic = dpdManageOffice.SelectedItem.Text;
            long lngServiceTypeID = (ctrFCDServiceType.CatelogID == ctrFCDServiceType.RootID) ? 0 : ctrFCDServiceType.CatelogID;
            long lngStatusID = (CtrFCDDealStatus.CatelogID == CtrFCDDealStatus.RootID) ? 0 : CtrFCDDealStatus.CatelogID;
            DataTable dt;
            dt = ZHServiceDP.GetMaterialStat(strBeginDate, strEndDate, lngDeptID, lngServiceTypeID, lngStatusID,long.Parse(dpdMastCustomer.SelectedValue));
            dgMaterialStat.DataSource = dt.DefaultView;
            dgMaterialStat.DataBind();
        }
        #endregion

        #region ����Excel btnToExcel_Click
        /// <summary>
        /// ����Excel
        /// </summary>
        protected void btnToExcel_Click()
        {
            try
            {
                LoadData(); ;
                ToExcel();
            }
            catch { }
        }
        
        /// <summary>
        /// 
        /// </summary>
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

                hw.WriteLine("<table><tr><td><font size=\"3\">���Ϸ���ͳ�Ʒ���</font></td></tr></table>");
                this.dgMaterialStat.RenderControl(hw);


                hw.WriteEndTag("body");
                hw.WriteLine();
                hw.WriteEndTag("html");

                //�����.xls�ļ�
                Response.ContentType = "application/vnd.ms-excel";
                //Response.AddHeader("Content-Disposition","attachment; filename=�����¼����Ʒ���(�¼�����).xls");  --������
                Response.AppendHeader("Content-Disposition", "attachment;filename=" + System.Web.HttpUtility.UrlEncode("���Ϸ���ͳ�Ʒ���", System.Text.Encoding.UTF8) + ".xls");
                Response.Charset = "utf-8";
                Response.Write(tw.ToString());
                Response.End();
            }
            catch
            {
            }
        }
        #endregion

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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dpdMastCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }

    }
}
