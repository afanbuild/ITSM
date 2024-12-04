/*******************************************************************
 * 版权所有：
 * Description：巡检维保（查询）
 * 
 * 
 * Create By  ：zhumingchun
 * Create Date：2007-10-06
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
using Epower.ITSM.SqlDAL;
using Epower.DevBase.Organization.SqlDAL;

namespace Epower.ITSM.Web.EquipmentManager
{
    public partial class frm_Equ_PatrolBaseQuery : BasePage
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

        #region 申请
        /// <summary>
        /// 
        /// </summary>
        void Master_Master_Button_New_Click()
        {
            Response.Redirect("~/Forms/form_all_flowmodel.aspx?appid=410");
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

            cpfPatrolBase.On_PageIndexChanged = new Epower.ITSM.Web.Controls.ControlPageFoot.ControlPageFootDelegate(Bind);
            if (!IsPostBack)
            {
                InitDropDown();
                //设置起始日期
                string sQueryBeginDate = string.Empty;
                sQueryBeginDate = DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-01";
                ctrDateTime.BeginTime = DateTime.Parse(sQueryBeginDate).ToString("yyyy-MM-dd");
                ctrDateTime.EndTime = DateTime.Now.ToString("yyyy-MM-dd");
                Bind();
                Session["FromUrl"] = "../EquipmentManager/frm_Equ_PatrolBaseQuery.aspx";

            }

            RightEntity re = (RightEntity)((Hashtable)Session["UserAllRights"])[Constant.EquPatrolQuery];
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

        #region  初始化下拉列表 InitDropDown
        /// <summary>
        /// 初始化下拉列表
        /// </summary>
        private void InitDropDown()
        {
            cboStatus.Items.Add(new ListItem("流程状态", "-1"));
            cboStatus.Items.Add(new ListItem("--正在处理", ((int)e_FlowStatus.efsHandle).ToString()));
            cboStatus.Items.Add(new ListItem("--正常结束", ((int)e_FlowStatus.efsEnd).ToString()));
            cboStatus.Items.Add(new ListItem("--流程暂停", ((int)e_FlowStatus.efsStop).ToString()));
            cboStatus.Items.Add(new ListItem("--流程终止", ((int)e_FlowStatus.efsAbort).ToString()));
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
            if (cboStatus.SelectedValue != "-1")  //状态
            {
                sWhere += " AND status = " + cboStatus.SelectedValue;
            }
            if (CtrFlowTitle.Value.Trim() != string.Empty)  //标题
            {
                sWhere += " And Title like " + StringTool.SqlQ("%" + CtrFlowTitle.Value.Trim() + "%");
            }
            if (txtRegName.Text.Trim() != string.Empty)   //登记人
            {
                sWhere += " And RegUserName like " + StringTool.SqlQ("%" + txtRegName.Text.Trim() + "%");
            }
            if (txtRegDeptName.Text.Trim() != string.Empty)  //登记人部门
            {
                sWhere += " And RegDeptName like " + StringTool.SqlQ("%" + txtRegDeptName.Text.Trim() + "%");
            }
            string strBeginDate = ctrDateTime.BeginTime;
            string strEndDate = ctrDateTime.EndTime;

            if (strBeginDate.Trim() != string.Empty)   //登记日期
                sWhere += " And RegTime >=to_date(" + StringTool.SqlQ(strBeginDate.Trim()) + ",'yyyy-MM-dd HH24:mi:ss')";
            if (strEndDate.Trim() != string.Empty)
                sWhere += " And RegTime <to_date(" + StringTool.SqlQ(DateTime.Parse(strEndDate).AddDays(1).ToShortDateString()) + ",'yyyy-MM-dd HH24:mi:ss')";
            if (txtEquName.Text.Trim() != string.Empty)  //设备/产品名称	
            {
                sWhere += " And ID in (select PatrolID from Equ_PatrolItemData where 1=1 and EquName like " + StringTool.SqlQ("%" + txtEquName.Text.Trim() + "%") + ")";
            }
            if (txtItemName.Text.Trim() != string.Empty)  //巡检项
            {
                sWhere += " And ID in (select PatrolID from Equ_PatrolItemData where 1=1 and ItemName like " + StringTool.SqlQ("%" + txtItemName.Text.Trim() + "%") + ")";
            }

            if (txtPatrolName.Text.Trim() != string.Empty)  //巡检人
            {
                sWhere += " And ID in (select PatrolID from Equ_PatrolItemData where 1=1 and PatrolName like " + StringTool.SqlQ("%" + txtPatrolName.Text.Trim() + "%") + ")";
            }

            
            DataTable dt = Equ_PatrolDataDP.GetFieldsTable(long.Parse(Session["UserID"].ToString()), long.Parse(Session["UserDeptID"].ToString()), long.Parse(Session["UserOrgID"].ToString()),
                            (Epower.DevBase.Organization.SqlDAL.RightEntity)((Hashtable)Session["UserAllRights"])[Epower.ITSM.Base.Constant.EquPatrolQuery], sWhere,this.cpfPatrolBase.PageSize, this.cpfPatrolBase.CurrentPage, ref iRowCount);
            grd.DataSource = dt.DefaultView;
            grd.Attributes.Add("style", "word-break:break-all;word-wrap:break-word");

            grd.DataBind();
            this.cpfPatrolBase.RecordCount = iRowCount;
            this.cpfPatrolBase.Bind();

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
                for (int i = 0; i < e.Item.Cells.Count-2; i++)
                {
                    if (i >= 3)
                    {
                        j = i - 3;
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
                e.Item.Attributes.Add("onmouseover", "currentcolor=this.style.backgroundColor;this.style.backgroundColor='#FFFBE8'");
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=currentcolor");

                string strFlowID = DataBinder.Eval(e.Item.DataItem, "FlowID").ToString();
                e.Item.Attributes.Add("ondblclick", "window.open('../Forms/frmIssueView.aspx?FlowID=" + strFlowID.ToString() + "','MainFrame','scrollbars=yes,resizable=yes,top=0,left=0,width=window.availWidth-12,height=window.availHeight-10');");
            }
        }

        /// <summary>
        /// 删除记录后, 刷新数据.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void hidd_btnDelete_Click(object sender, EventArgs e)
        {
            Bind();
        }
    }
}
