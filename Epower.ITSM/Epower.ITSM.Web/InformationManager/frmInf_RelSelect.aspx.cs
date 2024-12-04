/****************************************************************************
 * 
 * description:知识库关联选择
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

using Epower.DevBase.BaseTools;
using Epower.ITSM.SqlDAL;
using Epower.DevBase.Organization.SqlDAL;
using Epower.ITSM.Base;

namespace Epower.ITSM.Web.InformationManager
{
    public partial class frmInf_RelSelect : BasePage
    {
        #region 属性
        /// <summary>
        /// 设备分类ID
        /// </summary>
        protected string CatalogID
        {
            get
            {
                return CtrKBCataDropList1.CatelogID.ToString();
            }
        }

        /// <summary>
        /// 是否只读
        /// </summary>
        protected bool IsSelect
        {
            get { if (Request["IsSelect"] != null) return true; else return false; }
        }
        #endregion 
        #region 获得传过来的参数用来判断父窗体的控件id的名称的相同部分
        /// <summary>
        /// 获得传过来的参数用来判断父窗体的控件id的名称的相同部分
        /// </summary>
        public string Opener_ClientId
        {

            get
            {
                return (Request["Opener_ClientId"] == null) ? "" : Request["Opener_ClientId"].ToString().Replace("hidClientId_ForOpenerPage", "");
            }
        }
        #endregion

        #region SetParentButtonEvent
        /// <summary>
        /// 
        /// </summary>
        protected void SetParentButtonEvent()
        {
            this.Master.Master_Button_Query_Click += new Global_BtnClick(Master_Master_Button_Query_Click);
            this.Master.ShowQueryButton(true);
        }
        #endregion

        #region Master_Master_Button_New_Click
        /// <summary>
        /// 
        /// </summary>
        void Master_Master_Button_New_Click()
        {
            Response.Redirect("frmInf_InformationEdit.aspx?subjectid=" + CatalogID);
        }
        #endregion

        #region Master_Master_Button_Query_Click
        /// <summary>
        /// 
        /// </summary>
        void Master_Master_Button_Query_Click()
        {
            ControlPage1.DataGridToControl.CurrentPageIndex = 0;
            DataTable dt = LoadData(CatalogID);
            dgInf_Information.DataSource = dt.DefaultView;
            dgInf_Information.DataBind();
        }
        #endregion

        #region Master_Master_Button_Delete_Click
        /// <summary>
        /// 
        /// </summary>
        void Master_Master_Button_Delete_Click()
        {
            Inf_InformationDP ee = new Inf_InformationDP();
            foreach (DataGridItem itm in dgInf_Information.Items)
            {
                if (itm.ItemType == ListItemType.AlternatingItem ||
                    itm.ItemType == ListItemType.Item)
                {
                    string sID = itm.Cells[1].Text;
                    CheckBox chkdel = (CheckBox)itm.Cells[0].FindControl("chkDel");
                    if (chkdel.Checked)
                    {
                        ee.DeleteRecorded(long.Parse(sID));
                    }
                }
            }
            DataTable dt = LoadData(CatalogID);
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
            if (!IsPostBack)
            {
                DataTable dt;
                if (Request["subjectid"] != null)
                {
                    CtrKBCataDropList1.CatelogID = decimal.Parse(Request["subjectid"].ToString());
                    dt = LoadData(Request["subjectid"].ToString());
                }
                else
                {
                    dt = LoadData(CtrKBCataDropList1.CatelogID.ToString());
                }
                dgInf_Information.DataSource = dt.DefaultView;
                dgInf_Information.DataBind();
            }
        }
        #endregion

        #region LoadData
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sCalalogID"></param>
        /// <returns></returns>
        private DataTable LoadData(string sCalalogID)
        {
            DataTable dt;
            string sWhere = string.Empty;
            string sOrder = string.Empty;
            if (txtTitle.Text.Trim() != string.Empty)
            {
                sWhere += " And Title like " + StringTool.SqlQ("%" + txtTitle.Text.Trim() + "%");
            }
            if (txtPKey.Text.Trim() != string.Empty)
            {
                sWhere += " And PKey like " + StringTool.SqlQ("%" + txtPKey.Text.Trim() + "%");



            }
            if (txtContent.Text.Trim() != string.Empty)
            {
                sWhere += " And Content like " + StringTool.SqlQ("%" + txtContent.Text.Trim() + "%");
            }
            string strFullID = Inf_SubjectDP.GetSubjectFullID(long.Parse(sCalalogID));
            if ((sCalalogID == "1" || sCalalogID == "-1") && chkIncludeSub.Checked == true)
            {
            }
            else if (chkIncludeSub.Checked == true && strFullID.Length > 0)
            {
                //====zxl==
               // sWhere += " And SUBSTRING(FullID,1," + strFullID.Length.ToString() + ") = " + StringTool.SqlQ(strFullID);
                sWhere += " And substr(FullID,1," + strFullID.Length.ToString() + ") = " + StringTool.SqlQ(strFullID);

                //==
            }
            else
            {
                sWhere += " And FullID = " + StringTool.SqlQ(strFullID);
            }
            Inf_InformationDP ee = new Inf_InformationDP();
            dt = ee.GetDataTable(sWhere, sOrder,false); ;
            Session["Inf_Information"] = dt;
            if (txtPKey.Text.Trim() != string.Empty)
            {
                //增加关键字的查询次数
                ee.DealKeyWordTag(txtPKey.Text.Trim(), 3);
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
            if (Session["Inf_Information"] == null)
            {
                dt = LoadData(CatalogID);
            }
            else
            {
                dt = (DataTable)Session["Inf_Information"];
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

        #region  dgInf_Information_ItemCommand
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void dgInf_Information_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "edit")
            {
                Response.Redirect("frmInf_InformationEdit.aspx?id=" + e.Item.Cells[1].Text.ToString() + "&subjectid=" + CatalogID);
            }
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            foreach (DataGridItem itm in dgInf_Information.Items)
            {
                if (itm.ItemType == ListItemType.AlternatingItem ||
                    itm.ItemType == ListItemType.Item)
                {
                    string sID = itm.Cells[1].Text;
                    CheckBox chkdel = (CheckBox)itm.Cells[0].FindControl("chkDel");
                    if (chkdel.Checked)
                    {
                        sb.Append(sID + ",");
                    }
                }
            }
            StringBuilder sbText = new StringBuilder();
            sbText.Append("<script>");
            sbText.Append("var arr = new Array();");
            // ID
            sbText.Append("arr[0] ='" + sb.ToString() + "';");
            //=======zxl======

            sbText.Append(" if(arr != '')");
            sbText.Append("{ ");
            sbText.Append("window.opener.document.getElementById('" + Opener_ClientId + "hidCustArrID').value=arr[0];");
            sbText.Append("window.opener.document.getElementById('" + Opener_ClientId + "hidFlag').value='OK';");
                
            sbText.Append("}");
            sbText.Append("else{");
            sbText.Append("window.opener.document.getElementById('" + Opener_ClientId + "hidFlag').value='ON';");
            sbText.Append("}");

            sbText.Append("window.opener.document.getElementById('" + Opener_ClientId + "btnAddHid').click();");
            //=========zxl===
            // 关闭窗口
            sbText.Append("top.close();");
            sbText.Append("</script>");
            // 向客户端发送
            Page.RegisterStartupScript(DateTime.Now.ToString(), sbText.ToString());
            Response.Write(sbText.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnClose_Click(object sender, EventArgs e)
        {
            StringBuilder sbText = new StringBuilder();
            sbText.Append("<script>");
            // 关闭窗口
            sbText.Append("top.close();");
            sbText.Append("</script>");
            // 向客户端发送
            Page.RegisterStartupScript(DateTime.Now.ToString(), sbText.ToString());
            Response.Write(sbText.ToString());
        }

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
                for (int i = 0; i < e.Item.Cells.Count; i++)
                {
                    if (i > 1 && i < 5)
                    {
                        int j = i - 1;
                        e.Item.Cells[i].Attributes.Add("onclick", "sortTable('" + dg.ClientID + "'," + j.ToString() + ",0);");
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dgInf_Information_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                e.Item.Attributes.Add("ondblclick", "window.open('frmInf_InformationEdit.aspx?id=" + e.Item.Cells[1].Text + "&randomid='+GetRandom(),'MainFrame','scrollbars=yes,resizable=yes')");
                e.Item.Attributes.Add("onmouseover", "currentcolor=this.style.backgroundColor;this.style.backgroundColor='#FFFBE8'");
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=currentcolor");
            }
        }
    }
}
