/****************************************************************************
 * 
 * description:�¼�����
 * 
 * 
 * 
 * Create by:zhumingchun
 * Create Date:2007-09-20
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

using Epower.ITSM.SqlDAL;
using Epower.DevBase.Organization.SqlDAL;
using Epower.ITSM.Base;
using Epower.DevBase.BaseTools;
using Epower.ITSM.SqlDAL;

namespace Epower.ITSM.Web.ProbleForms
{
    /// <summary>
    /// 
    /// </summary>
    public partial class frmPro_ProblemAnalyseMain : BasePage
    {
        double dscale;
        double dEffect;
        double dStress;
        string sProble_FlowID = string.Empty;

        #region EventFlowID
        /// <summary>
        /// 
        /// </summary>
        protected string EventFlowID
        {
            get { if (Request["FlowID"] != null) return Request["FlowID"].ToString(); else return "0"; }
        }
        #endregion 

        #region ҳ����� Page_Load
        /// <summary>
        /// ҳ�����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Master.TableVisible = false;
            if (!IsPostBack)
            {
                InitPage();
                LoadData();
            }
        }
        #endregion 

        #region ҳ�����ݳ�ʼ�� InitPage
        /// <summary>
        /// 
        /// </summary>
        private void InitPage()
        {
            DataTable dt = ProblemDealDP.GetDataByFlowID(EventFlowID);
            foreach (DataRow dr in dt.Rows)
            {
                lblEventTitle.Text = dr["Subject"].ToString();
            }
        }
        #endregion 

        #region ������Ȩ�أ���Ӱ��ȣ��ܽ����ԣ�����FlowID CreateTotle
        /// <summary>
        /// ȡ�÷����ܷ���ֵ
        /// </summary>
        /// <returns></returns>
        private void CreateTotle(ref double sScale, ref double sEffect, ref double sStress,ref string sArrFlowID)
        {
            foreach (DataGridItem row in dgPro_ProblemAnalyse.Items)
            {
                string sDScale= ((TextBox)row.FindControl("txtScale")).Text;
                sScale += sDScale == "" ? 0.0 : double.Parse(sDScale);

                string sDEffect = ((TextBox)row.FindControl("txtEffect")).Text;
                sEffect += sDEffect == "" ? 0.0 : double.Parse(sDEffect);

                string sDStress = ((TextBox)row.FindControl("txtStress")).Text;
                sStress += sDStress == "" ? 0.0 : double.Parse(sDStress);

                sArrFlowID += row.Cells[1].Text.Trim() + ",";
            }
        }
        #endregion

        #region ȡ����ϸ����GetDetailItem
        /// <summary>
        /// ȡ����ϸ����
        /// </summary>
        /// <param name="isAll"></param>
        /// <returns></returns>
        private DataTable GetDetailItem()
        {
            DataTable dt = (DataTable)Session["ProblemItemData"];
            dt.Rows.Clear();
            int iCostID = 0;
            DataRow dr;
            foreach (DataGridItem row in dgPro_ProblemAnalyse.Controls[0].Controls)
            {
                iCostID++;
                if (row.ItemType == ListItemType.Item || row.ItemType == ListItemType.AlternatingItem)
                {
                    string sDScale = ((TextBox)row.FindControl("txtScale")).Text;
                    string sDEffect = ((TextBox)row.FindControl("txtEffect")).Text;
                    string sDStress = ((TextBox)row.FindControl("txtStress")).Text;
                    string sDRemark = ((TextBox)row.FindControl("txtRemark")).Text;

                    dr = dt.NewRow();
                    dr["Problem_FlowID"] = row.Cells[1].Text.ToString();
                    dr["Problem_Title"] = row.Cells[4].Text.ToString();
                    dr["Event_FlowID"] = row.Cells[2].Text.ToString(); ;
                    dr["Event_Title"] = row.Cells[3].Text.ToString();
                    dr["Scale"] = sDScale == "" ? 0.00 : double.Parse(sDScale);
                    dr["Effect"] = sDEffect == "" ? 0.00 : double.Parse(sDEffect);
                    dr["Stress"] = sDStress == "" ? 0.00 : double.Parse(sDStress);
                    dr["Remark"] = sDRemark;

                    dt.Rows.Add(dr);
                    
                }
            }
            Session["ProblemItemData"] = dt;
            return dt;
        }
        #endregion

        #region ��ϸɾ���¼� dgPro_ProblemAnalyse_ItemCommand
        /// <summary>
        /// ������ϸ������ɾ���¼�
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void dgPro_ProblemAnalyse_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            DataTable dt = GetDetailItem();
            if (e.CommandName == "Delete")
            {
                dt.Rows.RemoveAt(e.Item.ItemIndex);
            }
            dgPro_ProblemAnalyse.DataSource = dt.DefaultView;
            dgPro_ProblemAnalyse.DataBind();

            CreateTotle(ref dscale, ref dEffect, ref dStress, ref sProble_FlowID);
            lblScale.Text = dscale.ToString();
            lblEffect.Text = dEffect.ToString();
            lblStress.Text = dStress.ToString();
            this.hidCustArrIDold.Value = sProble_FlowID;
        }
        #endregion 

        #region �������� btnSave_Click
        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            DataTable dt = GetDetailItem();
            ProblemDealDP.SaveItem(dt, EventFlowID, lblEventTitle.Text.Trim(), Session["UserID"].ToString(), Session["PersonName"].ToString(), Session["UserDeptID"].ToString());

            CreateTotle(ref dscale, ref dEffect, ref dStress, ref sProble_FlowID);
            lblScale.Text = dscale.ToString();
            lblEffect.Text = dEffect.ToString();
            lblStress.Text = dStress.ToString();
            this.hidCustArrIDold.Value = sProble_FlowID;

            PageTool.MsgBox(this, "����ɹ�!");
        }
        #endregion 

        #region ���� btnAdd_Click
        /// <summary>
        /// ����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (hidFlag.Value.ToUpper() == "OK")
            {
                LoadProblemData();
            }
        }
        #endregion 

        #region �������� LoadData
        /// <summary>
        /// ��������
        /// </summary>
        private void LoadData()
        {
            DataTable dt = ProblemDealDP.GetProblemAnalsysByEvent(EventFlowID);
            Session["ProblemItemData"] = dt;
            dgPro_ProblemAnalyse.DataSource = dt.DefaultView;
            dgPro_ProblemAnalyse.DataBind();

            CreateTotle(ref dscale, ref dEffect, ref dStress, ref sProble_FlowID);
            lblScale.Text = dscale.ToString();
            lblEffect.Text = dEffect.ToString();
            lblStress.Text = dStress.ToString();
            this.hidCustArrIDold.Value = sProble_FlowID;
        }
        #endregion 

        #region �����������ݣ�������ԭ������ LoadProblemData
        /// <summary>
        /// ��������
        /// </summary>
        private void LoadProblemData()
        {
            string sArr = string.Empty;
            string sArrold = this.hidCustArrIDold.Value.Trim();
            string[] arr = this.hidCustArrID.Value.Trim().Split(',');
            for (int i = 0; i < arr.Length - 1; i++)
            {
                if (sArrold.IndexOf(arr[i] + ",") == -1)
                {
                    sArr += arr[i] + ",";
                    sArrold += arr[i] + ",";
                }
            }
            this.hidCustArrIDold.Value = sArrold;

            if (!string.IsNullOrEmpty(sArr))
            {
                DataTable dtProblem = ProblemDealDP.GetProblemAnalsys(sArr, EventFlowID, this.lblEventTitle.Text.Trim());

                DataTable dt = GetDetailItem();
                dt.Merge(dtProblem);
                dgPro_ProblemAnalyse.DataSource = dt.DefaultView;
                dgPro_ProblemAnalyse.DataBind();

                CreateTotle(ref dscale, ref dEffect, ref dStress, ref sProble_FlowID);
                lblScale.Text = dscale.ToString();
                lblEffect.Text = dEffect.ToString();
                lblStress.Text = dStress.ToString();
            }
        }
        #endregion 
    }
}
