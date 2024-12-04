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
using EpowerCom;
using Epower.ITSM.SqlDAL;
using Epower.ITSM.SqlDAL.Flash;

namespace Epower.ITSM.Web.AnalsysForms
{
    /// <summary>
    /// frmAvgTime ��ժҪ˵����
    /// </summary>
    public partial class frmAvgTime : BasePage
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

        #region  Page_Load
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, System.EventArgs e)
        {
            SetParentButtonEvent();
            if (!Page.IsPostBack)
            {
                ///������ʼ����
                string sQueryBeginDate = string.Empty;
                //sQueryBeginDate = DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-01";
                sQueryBeginDate = (DateTime.Now.Year - 1).ToString() + "-" + DateTime.Now.Month.ToString() + "-01";
                ctrDateTime.BeginTime = DateTime.Parse(sQueryBeginDate).ToString("yyyy-MM-dd");
                ctrDateTime.EndTime = DateTime.Now.ToString("yyyy-MM-dd");
                DoBind();
                if (dpoFlows.Items.Count > 0)
                {
                    QueryResult();
                }
            }
            else
            {
                if (Request.Form["__EVENTTARGET"] == "datarange")
                {
                    QueryResult();
                }
            }
        }
        #endregion

        #region DoBind
        /// <summary>
        /// 
        /// </summary>
        private void DoBind()
        {
            DataSet ds;
            if (System.Configuration.ConfigurationSettings.AppSettings["SystemModel"] == "1")
            {
                long lngOrgID = (long)Session["UserOrgID"];
                ds = FlowDP.GetFlowListForSelect(lngOrgID);
            }
            else
            {
                ds = FlowModel.GetFlowListForSelect();
            }
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                dpoFlows.Items.Add(new ListItem(ds.Tables[0].Rows[i]["FlowName"].ToString(), ds.Tables[0].Rows[i]["OFlowModelID"].ToString()));

            }

            if (ds.Tables[0].Rows.Count > 0)
            {
                dpoFlows.SelectedIndex = 0;

            }
        }
        #endregion

        #region QueryResult
        /// <summary>
        /// 
        /// </summary>
        private void QueryResult()
        {
            long lngOID = 0;
            string strStart = "";
            string strEnd = "";
            DataSet ds;
            strStart = ctrDateTime.BeginTime;
            strEnd = ctrDateTime.EndTime;
            lngOID = long.Parse(dpoFlows.SelectedValue);
            ds = flowAnalysis.AnalysisAvgTimeForTask(lngOID, strStart, strEnd);
            dgResult.DataSource = ds;
            dgResult.DataBind();
            DataTable dt;
            dt = FlowDP.GetAnalysisDataForMessageStatus(lngOID, strStart, strEnd);
            ReportDiv2.InnerHtml = FlashCS.PublicFlashUrl2D(dt, "״̬", "����", "����ʦ�����������", "", "", "../FlashReoport/Flash/Pie2D.swf", "100%", "248", true, 2);
            dt = FlowDP.GetAnalysisDataForMessageFinished(lngOID, strStart, strEnd);
            ReportDiv.InnerHtml = FlashCS.PublicFlashUrl2D(dt, "���", "����", "����ʦ�����������", "", "", "../FlashReoport/Flash/Pie2D.swf", "100%", "248", true, 2);

        }
        #endregion

        protected void btnSelect_Click(object sender, EventArgs e)
        {
            QueryResult();
        }


        #region btnToExcel_Click
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnToExcel_Click()
        {
            try
            {
                this.QueryResult();
                this.ToExcel();
            }
            catch { }
        }
        #endregion

        #region ToExcel
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

                hw.WriteLine("<table><tr><td><font size=\"3\">ƽ���������ڷ���(����ʱ��)</font></td></tr></table>");
                this.dgResult.RenderControl(hw);
                hw.WriteLine("<br>");
              //  this.ReportDiv.RenderControl(hw);
               // this.ReportDiv2.RenderControl(hw);

                hw.WriteEndTag("body");
                hw.WriteLine();
                hw.WriteEndTag("html");

                //�����.xls�ļ�
                Response.ContentType = "application/vnd.ms-excel";
                Response.AppendHeader("Content-Disposition", "attachment;filename=" + System.Web.HttpUtility.UrlEncode("ƽ���������ڷ���(����ʱ��)", System.Text.Encoding.UTF8) + ".xls");
                Response.Charset = "utf-8";
                Response.Write(tw.ToString());
                Response.End();
            }
            catch
            {
            }
        }
        #endregion

        #region dpoFlows_SelectedIndexChanged
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dpoFlows_SelectedIndexChanged(object sender, EventArgs e)
        {
            QueryResult();
        }
        #endregion



        #region dgResult_ItemDataBound
        protected void dgResult_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            int iTmp = 0;
            decimal dlTmp = 0;
            string sUnit = "��ʱ";
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                //�ı���ɫ (��������ʱ�޵Ļ���������ʾΪ��ɫ)
                switch (e.Item.Cells[4].Text.Trim())
                {
                    case "0":
                        sUnit = "��ʱ";
                        break;
                    case "1":
                        sUnit = "����";
                        break;
                    case "2":
                        sUnit = "������";
                        break;
                    case "3":
                        sUnit = "����";
                        break;
                    case "4":
                        sUnit = "Сʱ";
                        break;
                    default:
                        break;
                }
                dlTmp = Math.Round(decimal.Parse(e.Item.Cells[3].Text), 2);
                if (e.Item.Cells[2].Text.Trim() != "0" && decimal.Parse(e.Item.Cells[2].Text) < dlTmp)
                {
                    e.Item.Cells[1].ForeColor = Color.Red;
                }
                if (e.Item.Cells[0].Text.Trim() == "1")
                {
                    e.Item.Cells[0].Text = "��ǰ�汾";
                }
                else
                {
                    iTmp = int.Parse(e.Item.Cells[0].Text.Trim()) - 1;
                    e.Item.Cells[0].Text = "ǰ" + iTmp.ToString() + "�汾";
                }

                if (e.Item.Cells[2].Text.Trim() == "0")
                {
                    e.Item.Cells[2].Text = "����ʱ";
                }
                else
                {
                    e.Item.Cells[2].Text = e.Item.Cells[2].Text + sUnit;
                }

                e.Item.Cells[3].Text = dlTmp.ToString() + sUnit;
            }
        }
        #endregion
    }
}
