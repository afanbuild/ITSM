/****************************************************************************
 * 
 * description:知识库管理
 * 
 * 
 * 
 * Create by:
 * Create Date:2007-09-19
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
    public partial class frmInf_InformationMain : BasePage
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

        #region SetParentButtonEvent
        /// <summary>
        /// 
        /// </summary>
        protected void SetParentButtonEvent()
        {
            this.Master.OperatorID = Constant.InfManager;
            if (IsSelect)
                this.Master.IsCheckRight = false;
            else
            {
                this.Master.IsCheckRight = true;
                dgInf_Information.Columns[6].Visible = this.Master.GetEditRight();
                dgInf_Information.Columns[7].Visible = this.Master.GetEditRight();                

            }
            this.Master.Master_Button_New_Click += new Global_BtnClick(Master_Master_Button_New_Click);
            this.Master.Master_Button_Query_Click += new Global_BtnClick(Master_Master_Button_Query_Click);
            this.Master.Master_Button_Delete_Click += new Global_BtnClick(Master_Master_Button_Delete_Click);
            this.Master.Master_Button_ExportExcel_Click += new Global_BtnClick(Master_Master_Button_ExportExcel_Click);
           
            this.Master.ShowQueryPageButton();
            this.Master.ShowExportExcelButton(true);
            this.Master.MainID = "1";

            if (IsSelect)  //如果为选择时
            {
                this.Master.ShowNewButton(false);
                this.Master.ShowDeleteButton(false);
                this.Master.ShowExportExcelButton(false);
                dgInf_Information.Columns[0].Visible = false;
                dgInf_Information.Columns[6].Visible = false;
                dgInf_Information.Columns[7].Visible = false;                
                CtrKBCataDropList1.InformationLimit = true;   //控制类别范围
                this.Master.TableVisible = false;
                if (Request["IsGaoji"] == null)
                {
                    visbletr0.Visible = false;
                    visbletr1.Visible = false;
                    visbletr2.Visible = false;
                    InfSearch.Visible = true;
                }
                else
                {
                    this.Master.TableVisible = true;
                }
            }
        }
        #endregion

        #region 导出EXCEL事件Master_Master_Button_ExportExcel_Click
        /// <summary>
        /// 查询事件
        /// </summary>
        void Master_Master_Button_ExportExcel_Click()
        {
           
             IssueExportExcel();
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
            Bind("");
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
           Bind("");
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
            cpInformation.On_PageIndexChanged = new Epower.ITSM.Web.Controls.ControlPageFoot.ControlPageFootDelegate(DataGrid);
            

           
            // 暂时未做,从内容当中获取 常用关键字,根据 inf_tags表中记录 匹配计算 抽取常用词汇 
            //缺省内容中提取内容字眼
            //if (Request.QueryString["Content"] != null)
            //    txtContent.Text = GetMasterKeysFromContent(Request.QueryString["Content"].Trim());

            if (!IsPostBack)
            {
                //设置显示
                PageDeal.SetLanguage(this.Controls[0]);
                SetHeaderText();               
               

                //缺省关键字初值
                if (Request.QueryString["PKey"] != null)
                    txtKeyName.Text = Request.QueryString["PKey"].Trim();

                if (Request.QueryString["IsIncludeSub"] != null)
                {
                    string sInclude = Request.QueryString["IsIncludeSub"].Trim();
                    if (sInclude == "true")
                        chkIncludeSub.Checked = true;
                }
                if (Request["subjectid"] != null)
                {
                    CtrKBCataDropList1.CatelogID = decimal.Parse(Request["subjectid"].ToString());
                    Bind(Request["subjectid"].ToString());
                }
                else
                {
                    Bind("");
                }
            }
        }
        #endregion

        #region 设置datagrid标头显示 余向前 2013-05-20
        /// <summary>
        /// 设置datagrid标头显示
        /// </summary>
        private void SetHeaderText()
        {
            dgInf_Information.Columns[2].HeaderText = PageDeal.GetLanguageValue("info_Title");
            dgInf_Information.Columns[3].HeaderText = PageDeal.GetLanguageValue("info_PKey");
            dgInf_Information.Columns[4].HeaderText = PageDeal.GetLanguageValue("info_TypeName");
            dgInf_Information.Columns[5].HeaderText = PageDeal.GetLanguageValue("info_Tags");
        }
        #endregion

        #region LoadData

        public void DataGrid()
        {
            Bind("");
        }


        private void IssueExportExcel()
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
            string strFullID = Inf_SubjectDP.GetSubjectFullID(long.Parse(CtrKBCataDropList1.CatelogID.ToString()));
            if ((CtrKBCataDropList1.CatelogID.ToString() == "1" || CtrKBCataDropList1.CatelogID.ToString() == "-1") && chkIncludeSub.Checked == true)
            {
            }
            else if (chkIncludeSub.Checked == true && strFullID.Length > 0)
            {
                //zxl
                sWhere += " And substr(FullID,1," + strFullID.Length.ToString() + ") = " + StringTool.SqlQ(strFullID);
               // sWhere += " And SUBSTRING(FullID,1," + strFullID.Length.ToString() + ") = " + StringTool.SqlQ(strFullID);
            }
            else
            {
                sWhere += " And FullID = " + StringTool.SqlQ(strFullID);
            }
            if (txtTags.Text.Trim() != string.Empty)
            {
                sWhere += " And Tags like " + StringTool.SqlQ("%" + txtTags.Text.Trim() + "%");
            }

            sOrder = " order by updatetime desc ";

            Inf_InformationDP ee = new Inf_InformationDP();

            dt = ee.GetDataTable(sWhere, sOrder, IsSelect); 
            Epower.ITSM.Web.Common.ExcelExport.ExportKBList(this, dt, Session["UserID"].ToString());
        }

        #endregion

        #region  Bind
        /// <summary>
        /// 
        /// </summary>
        private void Bind(string StrFull)
        {
            DataTable dt;
            int iRowCount = 0;
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
                sWhere += " And PlainContent like " + StringTool.SqlQ("%" + txtContent.Text.Trim() + "%");
            }
            if (txtKeyName.Text.Trim() != string.Empty)
            {
                sWhere += " And (PKey like " + StringTool.SqlQ("%" + txtKeyName.Text.Trim() + "%");
                sWhere += " or Title like " + StringTool.SqlQ("%" + txtKeyName.Text.Trim() + "%");
                sWhere += " or Tags like " + StringTool.SqlQ("%" + txtKeyName.Text.Trim() + "%") + ")";
            }

            string sCalalogID = string.Empty;
            if (StrFull != "")
            {
                sCalalogID = StrFull;
            }
            else
            {
                sCalalogID = CtrKBCataDropList1.CatelogID.ToString();
            }
            string strFullID = Inf_SubjectDP.GetSubjectFullID(long.Parse(sCalalogID));
            if ((sCalalogID == "1" || sCalalogID == "-1") && chkIncludeSub.Checked == true)
            {
            }
            else if (chkIncludeSub.Checked == true && strFullID.Length > 0)
            {
                sWhere += " And substr(FullID,1," + strFullID.Length.ToString() + ") = " + StringTool.SqlQ(strFullID);
            }
            else
            {
                sWhere += " And FullID = " + StringTool.SqlQ(strFullID);
            }
            if (txtTags.Text.Trim() != string.Empty)
            {
                sWhere += " And Tags like " + StringTool.SqlQ("%" + txtTags.Text.Trim() + "%");
            }

            sOrder = " order by updatetime desc ";

            Inf_InformationDP ee = new Inf_InformationDP();

            dt = ee.GetDataTable(sWhere, sOrder, IsSelect, this.cpInformation.PageSize, this.cpInformation.CurrentPage, ref iRowCount); 
            if (txtPKey.Text.Trim() != string.Empty)
            {
                //增加关键字的查询次数
                ee.DealKeyWordTag(txtPKey.Text.Trim(), 3);
            }
            
            dgInf_Information.DataSource = dt.DefaultView;
            dgInf_Information.DataBind();
            this.cpInformation.RecordCount = iRowCount;
            this.cpInformation.Bind();
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
            if (e.CommandName == "link")
            {
                Response.Redirect("frmRelMain.aspx?KBID=" + e.Item.Cells[1].Text.ToString() + "&subjectid=" + CatalogID);
            }
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
                int j=0;
                for (int i = 0; i < e.Item.Cells.Count; i++)
                {
                    if (i > 1 && i < 6)
                    {
                        if(IsSelect)
                             j = i - 2;
                        else
                             j = i - 1;
                        e.Item.Cells[i].Attributes.Add("onclick", "sortTable('" + dg.ClientID + "'," + j.ToString() + ",0);");
                    }
                }
            }
        }
        #endregion 

        protected void dgInf_Information_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                string sID = e.Item.Cells[1].Text.Trim();
                if (Request["IsSelect"] == null)
                {
                    e.Item.Attributes.Add("ondblclick", "window.open('frmInf_InformationEdit.aspx?id=" + e.Item.Cells[1].Text.ToString() + "&subjectid=" + CatalogID + "&randomid='+GetRandom(),'subjectinfo','scrollbars=yes,resizable=yes')");
                }
                else
                {
                    e.Item.Attributes.Add("ondblclick", "window.open('frmKBShow.aspx?KBID=" + e.Item.Cells[1].Text.ToString() +"')");
                }
                e.Item.Attributes.Add("onmouseover", "currentcolor=this.style.backgroundColor;this.style.backgroundColor='#FFFBE8'");
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=currentcolor");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Bind("");
        }
    }
}
