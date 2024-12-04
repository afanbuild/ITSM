/*******************************************************************
 *
 * Description
 * 
 * 班次查询页面
 * Create By  :yxq
 * Create Date:2011年9月8日 星期四
 * *****************************************************************/
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

using Epower.DevBase.BaseTools;
using Epower.ITSM.SqlDAL;
using Epower.ITSM.Base;
using System.Text;

namespace Epower.ITSM.Web.CustManager
{
    public partial class frmBr_OrderClassTypeSel : BasePage
    {
        #region SetParentButtonEvent
        /// <summary>
        /// 
        /// </summary>
        protected void SetParentButtonEvent()
        {
            this.Master.OperatorID = Constant.CBr_OrderClass;
            this.Master.IsCheckRight = true;
            this.dgBr_OrderClassType.Columns[this.dgBr_OrderClassType.Columns.Count - 1].Visible = this.Master.GetEditRight();            
            this.Master.Master_Button_Query_Click += new Global_BtnClick(Master_Master_Button_Query_Click);
            this.Master.ShowQueryButton(true);
            this.Master.MainID = "1";
        }
        #endregion

        #region Master_Master_Button_Query_Click
        /// <summary>
        /// 
        /// </summary>
        void Master_Master_Button_Query_Click()
        {
            BindData();
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
            cpBr_OrderClassType.On_PageIndexChanged = new Epower.ITSM.Web.Controls.ControlPageFoot.ControlPageFootDelegate(BindData);
            if (!IsPostBack)
            {
                BindData();
            }
        }
        #endregion

        #region BindData
        /// <summary>
        /// 
        /// </summary>
        private void BindData()
        {
            DataTable dt;
            string sWhere = " 1=1 And Deleted=0 ";
            string sOrder = " order by id";
            if (txtClassTypeName.Text.Trim() != string.Empty)
            {
                sWhere += " And ClassTypeName like " + StringTool.SqlQ("%" + txtClassTypeName.Text.Trim() + "%");
            }
           
            Br_OrderClassTypeDP ee = new Br_OrderClassTypeDP();
            int iRowCount = 0;
            dt = ee.GetDataTable(sWhere, sOrder, this.cpBr_OrderClassType.PageSize, this.cpBr_OrderClassType.CurrentPage, ref iRowCount);
            dgBr_OrderClassType.DataSource = dt.DefaultView;
            dgBr_OrderClassType.DataBind();
            this.cpBr_OrderClassType.RecordCount = iRowCount;
            this.cpBr_OrderClassType.Bind();
        }
        #endregion

        #region  dgBr_OrderClassType_ItemCommand
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void dgBr_OrderClassType_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "edit")
            {
                string id = e.Item.Cells[0].Text.Replace("&nbsp;", "");
                string sWhere = " And id = " + id;
                Br_OrderClassTypeDP ee = new Br_OrderClassTypeDP();
                DataTable dt = ee.GetDataTable(sWhere, "");
                Json json = new Json(dt);

                string jsonstr = "{record:" + json.ToJson() + "}";

                StringBuilder sbText = new StringBuilder();
                sbText.Append("<script>");
                sbText.Append("var arr = " + jsonstr + ";");
                sbText.Append("window.parent.returnValue = arr;");
                // 关闭窗口
                sbText.Append("top.close();");
                sbText.Append("</script>");
                // 向客户端发送
                Page.RegisterStartupScript(DateTime.Now.ToString(), sbText.ToString());
                Response.Write(sbText.ToString());
            }
        }
        #endregion

        #region dgBr_OrderClassType_ItemCreated
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dgBr_OrderClassType_ItemCreated(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                DataGrid dg = (DataGrid)sender;
                int j = 0;
                for (int i = 0; i < e.Item.Cells.Count - 1; i++)
                {
                    if (i >= 1)
                    {
                        j = i;
                        e.Item.Cells[i].Attributes.Add("onclick", "sortTable('" + dg.ClientID + "'," + j.ToString() + ",0);");
                    }
                }
            }
        }
        #endregion

        #region dgBr_OrderClassType_ItemDataBound
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dgBr_OrderClassType_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                e.Item.Attributes.Add("onmouseover", "currentcolor=this.style.backgroundColor;this.style.backgroundColor='#FFFBE8'");
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=currentcolor");
                string sID = DataBinder.Eval(e.Item.DataItem, "ID").ToString();
                string sWhere = " And id = " + sID;
                Br_OrderClassTypeDP ee = new Br_OrderClassTypeDP();
                DataTable dt = ee.GetDataTable(sWhere, "");
                Json json = new Json(dt);

                string jsonstr = "{record:" + json.ToJson() + "}";
                //zxl
                e.Item.Attributes.Add("onclick", "doubleSelect(" + jsonstr + ");");

              //  e.Item.Attributes.Add("ondblclick", "doubleSelect(" + jsonstr + ");");
            }
        }
        #endregion
    }
}
