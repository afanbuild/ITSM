/****************************************************************************
 * 
 * description:知识评分
 * 
 * 
 * 
 * Create by:
 * Create Date:2007-09-26
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
using System.Text;

using Epower.ITSM.SqlDAL;

namespace Epower.ITSM.Web.InformationManager
{
    /// <summary>
    /// 
    /// </summary>
    public partial class frmInf_Score : BasePage
    {
        /// <summary>
        /// 
        /// </summary>
        protected string sKBID
        {
            get
            {
                return Request["KBID"] != null ? Request["KBID"].ToString() : "0";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected decimal ID
        {
            get
            {
                if (ViewState["ScoreID"] != null)
                    return decimal.Parse(ViewState["ScoreID"].ToString());
                else
                    return 0;
            }
            set
            {
                ViewState["ScoreID"] = value;
            }
        }

        /// <summary>
        /// 页面加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DataTable dt = LoadData();
                foreach (DataRow dr in dt.Rows)
                {
                    txtScore.Text = dr["Score"].ToString();
                    ID = decimal.Parse(dr["ID"].ToString());
                }

                lblUserName.Text = Session["PersonName"].ToString();
                lblTime.Text = DateTime.Now.ToString();
            }
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        /// <returns></returns>
        protected DataTable LoadData()
        {
            Inf_ScoreDP ee = new Inf_ScoreDP();
            string sWhere = " and KBID=" + sKBID + " and UserID=" + Session["UserID"].ToString();
            DataTable dt = ee.GetDataTable(sWhere, string.Empty);
            return dt;
        }

        /// <summary>
        /// 评分
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            Inf_ScoreDP ee = new Inf_ScoreDP();
            ee.Score = int.Parse(txtScore.Text.Trim());
            ee.KBID = decimal.Parse(sKBID);
            ee.RegTime = DateTime.Now;
            ee.UserID = decimal.Parse(Session["UserID"].ToString());
            ee.UserName = Session["PersonName"].ToString();
            if (ID == 0)
            {
                ee.InsertRecorded(ee);
            }
            else
            {
                ee.ID = ID;
                ee.UpdateRecorded(ee);
            }

            StringBuilder sbText = new StringBuilder();
            sbText.Append("<script>");
            // 关闭窗口
            sbText.Append("top.close();");
            sbText.Append("</script>");
            Page.RegisterStartupScript(DateTime.Now.ToString(), sbText.ToString());
            Response.Write(sbText.ToString());
        }
    }
}
