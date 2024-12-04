/*******************************************************************
 * 版权所有：
 * Description：知识审批（查询）
 * 
 * 
 * Create By  ：zhumingchun
 * Create Date：2007-09-28
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

using Epower.ITSM.Base;
using EpowerGlobal;
using Epower.DevBase.BaseTools;
using Epower.DevBase.Organization.SqlDAL;
using Epower.ITSM.SqlDAL;

namespace Epower.ITSM.Web.InformationManager
{
    public partial class frm_KBBaseQuery : BasePage
    {
        #region 设置父窗体按钮事件SetParentButtonEvent
        /// <summary>
        /// 设置父窗体按钮事件
        /// </summary>
        protected void SetParentButtonEvent()
        {
            this.Master.Master_Button_Query_Click += new Global_BtnClick(Master_Master_Button_Query_Click);
            this.Master.Master_Button_New_Click += new Global_BtnClick(Master_Master_Button_New_Click);
            this.Master.ShowQueryButton(true);
            this.Master.ShowNewButton(true);
            this.Master.MainID = "1";     //设置页面的ID编号，如果为查询页面，则设置为1
        }
        #endregion 

        #region 申请 Master_Master_Button_New_Click
        /// <summary>
        /// 
        /// </summary>
        void Master_Master_Button_New_Click()
        {            
            Response.Redirect("~/Forms/form_all_flowmodel.aspx?appid=400");
        }
        #endregion 

        #region 查询事件Master_Master_Button_Query_Click
        /// <summary>
        /// 查询事件
        /// </summary>
        void Master_Master_Button_Query_Click()
        {
            Bind();
        }
        #endregion 

        #region 页面初始化 Page_Load
        /// <summary>
        /// 页面初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            SetParentButtonEvent();

            cpKBBase.On_PageIndexChanged = new Epower.ITSM.Web.Controls.ControlPageFoot.ControlPageFootDelegate(Bind);
            if (!IsPostBack)
            {
                //设置显示
                PageDeal.SetLanguage(this.Controls[0]);
                SetHeaderText();
                                            
                InitDropDown();
                //设置起始日期
                string sQueryBeginDate = "0";
                if (CommonDP.GetConfigValue("Other", "QueryBeginDate") != null)
                    sQueryBeginDate =CommonDP.GetConfigValue("Other", "QueryBeginDate").ToString();
                if (sQueryBeginDate == "0")
                {
                    ctrDateSelectTime1.BeginTime = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
                }
                else
                {
                    ctrDateSelectTime1.BeginTime = DateTime.Parse(sQueryBeginDate).ToString("yyyy-MM-dd");
                }
                ctrDateSelectTime1.EndTime = DateTime.Now.ToShortDateString();

                Bind();
                Session["FromUrl"] = "../InformationManager/frm_KBBaseQuery.aspx";
                //应用管理员删除权限
           
            }

            //grd.Columns[grd.Columns.Count - 1].Visible = CheckRight(Constant.admindeleteflow);

            RightEntity re = (RightEntity)((Hashtable)Session["UserAllRights"])[Constant.InfKBQuery];
            #region  删除权限逻辑控制
            //yanghw  优先流程删除权限  然后再判断操作项的权限
            //应用管理员删除权限
            if (CheckRight(Constant.admindeleteflow))
            {
                grd.Columns[grd.Columns.Count - 1].Visible = true;
            }
            else
            {
                if (re != null)
                    grd.Columns[grd.Columns.Count - 1].Visible = re.CanDelete;
                else
                    grd.Columns[grd.Columns.Count - 1].Visible = false;

            }
            //新增权限
            if (re.CanAdd != true)
            {
                this.Master.ShowNewButton(false);
            }


            #endregion
        }
        #endregion 

        #region 设置datagrid标头显示 余向前 2013-05-20
        /// <summary>
        /// 设置datagrid标头显示
        /// </summary>
        private void SetHeaderText()
        {
            grd.Columns[0].HeaderText = PageDeal.GetLanguageValue("info_Title");
            grd.Columns[1].HeaderText = PageDeal.GetLanguageValue("info_PKey");
            grd.Columns[2].HeaderText = PageDeal.GetLanguageValue("info_TypeName");
        }
        #endregion

        #region  初始化下拉列表 InitDropDown
        /// <summary>
        /// 初始化下拉列表
        /// </summary>
        private void InitDropDown()
        {
            cboStatus.Items.Add(new ListItem("所有状态", "-1"));
            cboStatus.Items.Add(new ListItem("--正在处理", ((int)e_FlowStatus.efsHandle).ToString()));
            cboStatus.Items.Add(new ListItem("--正常结束", ((int)e_FlowStatus.efsEnd).ToString()));
            cboStatus.Items.Add(new ListItem("--流程暂停", ((int)e_FlowStatus.efsStop).ToString()));
            cboStatus.Items.Add(new ListItem("--流程终止", ((int)e_FlowStatus.efsAbort).ToString()));
            cboStatus.SelectedIndex = 1;
        }
        #endregion s
         
        #region 绑定数据 Bind
        /// <summary>
        /// 绑定数据
        /// </summary>
        private void Bind()
        {
            #region bind
            int iRowCount = 0;
            string sWhere = "";
            if (cboStatus.SelectedValue != "-1")
            {
                sWhere += " AND status = " + cboStatus.SelectedValue;
            }
            if (txtTitle.Text.Trim() != string.Empty)
            {
                sWhere += " And Title like " + StringTool.SqlQ("%" + txtTitle.Text.Trim() + "%");
            }
            if (txtPKey.Text.Trim() != string.Empty)
            {
                sWhere += " And PKey like " + StringTool.SqlQ("%" + txtPKey.Text.Trim() + "%");
            }
            if (txtTags.Text.Trim() != string.Empty)
            {
                sWhere += " And Tags like " + StringTool.SqlQ("%" + txtTags.Text.Trim() + "%");
            }
            if (txtContent.Text.Trim() != string.Empty)
            {
                sWhere += " And Content like " + StringTool.SqlQ("%" + txtContent.Text.Trim() + "%");
            }
            if (ctrDateSelectTime1.BeginTime.Trim() != string.Empty)
                sWhere += " And RegTime >=to_date(" + StringTool.SqlQ(ctrDateSelectTime1.BeginTime.Trim()) + ",'yyyy-MM-dd HH24:mi:ss')";
            if (ctrDateSelectTime1.EndTime.Trim() != string.Empty)
                sWhere += " And RegTime <to_date(" + StringTool.SqlQ(DateTime.Parse(ctrDateSelectTime1.EndTime).AddDays(1).ToShortDateString()) + ",'yyyy-MM-dd HH24:mi:ss')";

            DataTable dt = Epower.ITSM.SqlDAL.Inf_InformationDP.GetFieldsTable(long.Parse(Session["UserID"].ToString()), long.Parse(Session["UserDeptID"].ToString()), long.Parse(Session["UserOrgID"].ToString()),
                            (Epower.DevBase.Organization.SqlDAL.RightEntity)((Hashtable)Session["UserAllRights"])[Epower.ITSM.Base.Constant.InfKBQuery], sWhere, this.cpKBBase.PageSize, this.cpKBBase.CurrentPage, ref iRowCount);
            grd.DataSource = dt.DefaultView;
            grd.Attributes.Add("style", "word-break:break-all;word-wrap:break-word");

            grd.DataBind();
            this.cpKBBase.RecordCount = iRowCount;
            this.cpKBBase.Bind();
            #endregion
        }
        #endregion 

        #region 显示页面地址 GetUrl
        /// <summary>
        /// 显示页面地址
        /// </summary>
        /// <param name="lngNoticeID"></param>
        /// <returns></returns>
        protected string GetUrl(decimal lngFlowID)
        {
            string sUrl = "";
            sUrl = "javascript:window.open('../Forms/frmIssueView.aspx?FlowID=" + lngFlowID.ToString() + "','MainFrame','scrollbars=yes,resizable=yes,top=0,left=0,width=window.availWidth-12,height=window.availHeight-10');";
            return sUrl;
        }
        #endregion 

        

        #region 排序 grd_ItemCreated
        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_ItemCreated(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                DataGrid dg = (DataGrid)sender;
                int j = 0;
                for (int i = 0; i < e.Item.Cells.Count; i++)
                {
                    if (i < 5)
                    {
                        j = i - 0;
                        e.Item.Cells[i].Attributes.Add("onclick", "sortTable('" + dg.ClientID + "'," + j.ToString() + ",0);");
                    }
                }
            }
        }
        #endregion 

        #region 检查权限 CheckRight
        /// <summary>
        /// 
        /// </summary>
        /// <param name="OperatorID"></param>
        /// <returns></returns>
        private bool CheckRight(long OperatorID)
        {
            RightEntity re = (RightEntity)((Hashtable)Session["UserAllRights"])[OperatorID];
            if (re == null)
                return false;
            else
                return re.CanRead;
        }
        #endregion

        #region 删除流程gridUndoMsg_DeleteCommand
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void gridUndoMsg_DeleteCommand(object source, DataGridCommandEventArgs e)
        {
            Bind();
        }
        #endregion 

        protected void grd_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                if (e.Item.Cells[4].Text.Trim() == "20")
                {
                    e.Item.Cells[4].Text = "正在处理";
                }
                else if (e.Item.Cells[4].Text.Trim() == "30")
                {
                    e.Item.Cells[4].Text = "正常结束";
                }
                else if (e.Item.Cells[4].Text.Trim() == "40")
                {
                    e.Item.Cells[4].Text = "流程暂停";
                }
                else if (e.Item.Cells[4].Text.Trim() == "50")
                {
                    e.Item.Cells[4].Text = "流程终止";
                }


                string lngFlowID = DataBinder.Eval(e.Item.DataItem, "FlowID").ToString();
                e.Item.Attributes.Add("onmouseover", "currentcolor=this.style.backgroundColor;this.style.backgroundColor='#FFFBE8'");
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=currentcolor");
                e.Item.Attributes.Add("ondblclick", "window.open('../Forms/frmIssueView.aspx?FlowID=" + lngFlowID.ToString() + "&randomid='+GetRandom(),'MainFrame','scrollbars=yes,resizable=yes')");
            }
        }
        /// <summary>
        /// 删除后。调用按钮事件。重新绑定。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void hidd_btnDelete_Click(object sender, EventArgs e)
        {
            Bind();
        }
    }
}
