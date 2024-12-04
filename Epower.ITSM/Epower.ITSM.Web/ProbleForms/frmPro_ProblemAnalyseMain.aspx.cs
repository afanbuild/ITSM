/****************************************************************************
 * 
 * description:事件分析
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

        #region 页面加载 Page_Load
        /// <summary>
        /// 页面加载
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

        #region 页面数据初始化 InitPage
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

        #region 生成总权重，总影响度，总紧迫性，问题FlowID CreateTotle
        /// <summary>
        /// 取得费用总费用值
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

        #region 取得详细资料GetDetailItem
        /// <summary>
        /// 取得详细资料
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

        #region 明细删除事件 dgPro_ProblemAnalyse_ItemCommand
        /// <summary>
        /// 费用明细新增，删除事件
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

        #region 保存数据 btnSave_Click
        /// <summary>
        /// 保存数据
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

            PageTool.MsgBox(this, "保存成功!");
        }
        #endregion 

        #region 新增 btnAdd_Click
        /// <summary>
        /// 新增
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

        #region 加载数据 LoadData
        /// <summary>
        /// 加载数据
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

        #region 加载问题数据，并整合原有数据 LoadProblemData
        /// <summary>
        /// 加载数据
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
