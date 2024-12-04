/****************************************************************************
 * 
 * description:知识搜索
 * 
 * 
 * 
 * Create by:
 * Create Date:2008-03-10
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

using Epower.DevBase.BaseTools;
using Epower.ITSM.SqlDAL;
using Epower.DevBase.Organization.SqlDAL;
using Epower.ITSM.Base;

namespace Epower.ITSM.Web.InformationManager
{
    /// <summary>
    /// 
    /// </summary>
    public partial class frmInfSearch : BasePage
    {

        #region SetParentButtonEvent
        /// <summary>
        /// 
        /// </summary>
        protected void SetParentButtonEvent()
        {
            this.Master.TableVisible = false;
        }
        #endregion

        #region Master_Master_Button_Query_Click
        /// <summary>
        /// 
        /// </summary>
        void Master_Master_Button_Query_Click()
        {
            ControlPage1.DataGridToControl.CurrentPageIndex = 0;
            DataTable dt = LoadData();
            dgInf_Information.DataSource = dt.DefaultView;
            dgInf_Information.DataBind();
        }
        #endregion

        #region Page_Load
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, System.EventArgs e)
        {
            SetParentButtonEvent();
            ControlPage1.On_PostBack += new EventHandler(ControlPage1_On_PostBack);
            ControlPage1.DataGridToControl = dgInf_Information;
            // 暂时未做,从内容当中获取 常用关键字,根据 inf_tags表中记录 匹配计算 抽取常用词汇 
            //缺省内容中提取内容字眼
            //if (Request.QueryString["Content"] != null)
            //    txtContent.Text = GetMasterKeysFromContent(Request.QueryString["Content"].Trim());

            if (!IsPostBack)
            {
                txtPKey.Width = "400px";
                txtPKey.FieldsSourceType = 1;
                //txtPKey.FieldsSourceID = "infSeach_001";
                txtPKey.FieldsSourceID = "PinYin_infSeach";  //这个PinYin_开头，可以智能拼音检索
                txtPKey.OnTimeXmlHttp = true;   //实时获取需要设置

                Ctrtitle1.Title = "知识搜索";

                if (Request["KeyWord"] != null)
                    txtPKey.Value = Request["KeyWord"].ToString().Trim();
                //快速搜索
                if (Request["svalue"] != null)
                {
                    txtPKey.Value = Request["svalue"].ToString().Trim();
                }

                btnSearch_Click(null, null);
            }
        }
        #endregion

        #region LoadData
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sCalalogID"></param>
        /// <returns></returns>
        private DataTable LoadData()
        {
            DataTable dt;
            string sWhere = string.Empty;
            string sOrder = string.Empty;
            if (txtPKey.Value.Trim() != string.Empty)
            {
                sWhere += " And (PKey like " + StringTool.SqlQ("%" + txtPKey.Value.Trim() + "%");
                sWhere += " or Title like " + StringTool.SqlQ("%" + txtPKey.Value.Trim() + "%");

                /*     
                 * Date: 2013-08-05 14:00
                 * summary: 增加对知识内容的模糊查询                 
                 *                  
                 * modified: sunshaozong@gmail.com     
                 */
                sWhere += String.Format(" or PlainContent like '%{0}%' ", txtPKey.Value.Trim());

                sWhere += " or Tags like " + StringTool.SqlQ("%" + txtPKey.Value.Trim() + "%") + ")";
            }
            else
            {
                sWhere = " And 1<>1";
            }
            Inf_InformationDP ee = new Inf_InformationDP();
            dt = ee.SearchInf(sWhere, sOrder); ;
            Session["Inf_InformationSimple"] = dt;
            if (txtPKey.Value.Trim() != string.Empty)
            {
                //增加关键字的查询次数
                ee.DealKeyWordTag(txtPKey.Value.Trim(), 3);
            }

            return dt;
        }
        #endregion

        #region  Bind
        /// <summary>
        /// 
        /// </summary>
        private void Bind()
        {
            DataTable dt;
            if (Session["Inf_InformationSimple"] == null)
            {
                dt = LoadData();
            }
            else
            {
                dt = (DataTable)Session["Inf_InformationSimple"];
            }
            dgInf_Information.DataSource = dt.DefaultView;
            dgInf_Information.DataBind();
        }
        #endregion

        #region ControlPage1_On_PostBack
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ControlPage1_On_PostBack(object sender, EventArgs e)
        {
            Bind();
        }
        #endregion

        #region dgInf_Information_ItemCreated
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dgInf_Information_ItemCreated(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                DataGrid dg = (DataGrid)sender;
                int j = 0;
                for (int i = 0; i < e.Item.Cells.Count; i++)
                {
                    if (i > 1 && i < 5)
                    {
                        j = i - 0;
                        e.Item.Cells[i].Attributes.Add("onclick", "sortTable('" + dg.ClientID + "'," + j.ToString() + ",0);");
                    }
                }
            }
        }
        #endregion

        #region 高级搜索 lnkSearch_Click
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkSearch_Click(object sender, EventArgs e)
        {
            Server.Transfer("frmInf_MainShow.htm");
        }
        #endregion

        #region 搜索 btnSearch_Click
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            DataTable dt = LoadData();
            Session["Inf_InformationSimple"] = dt;
            dgInf_Information.DataSource = dt.DefaultView;
            dgInf_Information.DataBind();

            if (txtPKey.Value.Trim() != string.Empty)
            {
                Inf_InformationDP ee = new Inf_InformationDP();
                DataTable dtkey = ee.GetKeyWord(txtPKey.Value.Trim());
                DataList1.DataSource = dtkey.DefaultView;
                DataList1.DataBind();

                if (dtkey.Rows.Count > 0)
                    table2.Visible = true;
                else
                    table2.Visible = false;
            }
            else
            {
                table2.Visible = false;
            }
        }
        #endregion
    }
}
