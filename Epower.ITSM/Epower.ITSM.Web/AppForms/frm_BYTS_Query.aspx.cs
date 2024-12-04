/*******************************************************************
 * 版权所有：
 * Description：投诉（查询） * 
 * 
 * Create By  ：zhumingchun
 * Create Date：2007-07-23
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
using System.Xml;
using System.Drawing;

using EpowerCom;
using EpowerGlobal;
using Epower.ITSM.Base;
using Epower.DevBase.Organization.Base;
using Epower.DevBase.Organization.SqlDAL;
using Epower.ITSM.SqlDAL;


namespace Epower.ITSM.Web.AppForms
{
    public partial class frm_BYTS_Query : BasePage
    {
        #region 变量
        long lngUserID = 0;
        long lngDeptID = 0;
        long lngOrgID = 0;
        bool blnDisplayFeedBack = false; //回访问权限
        RightEntity reTrace = null;  //服务跟踪权限 
        #endregion 

        #region 方法区
        #region 设置父窗体按钮事件SetParentButtonEvent
        /// <summary>
        /// 设置父窗体按钮事件
        /// </summary>
        protected void SetParentButtonEvent()
        {
            this.Master.Master_Button_Query_Click += new Global_BtnClick(Master_Master_Button_Query_Click);
            this.Master.Master_Button_ExportExcel_Click += new Global_BtnClick(Master_Master_Button_ExportExcel_Click);
            this.Master.Master_Button_New_Click += new Global_BtnClick(Master_Master_Button_New_Click);
            this.Master.ShowQueryPageButton();
            this.Master.ShowDeleteButton(false);
            this.Master.ShowNewButton(true);
            this.Master.ShowExportExcelButton(true);

            this.Master.MainID = "1";     //设置页面的ID编号，如果为查询页面，则设置为1
        }
        #endregion 

        #region 申请Master_Master_Button_New_Click
        /// <summary>
        /// 
        /// </summary>
        void Master_Master_Button_New_Click()
        {
            Response.Redirect("~/Forms/form_all_flowmodel.aspx?appid=320");
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

        #region 设置按钮名称
        /// <summary>
        /// 获取按钮名称   详情  或详情/评估
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        protected string GetButtonValue(int status)
        {
            string strRet = "详情";

            if (status == (int)e_FlowStatus.efsEnd && blnDisplayFeedBack == true)
            {
                strRet = "详情/回访";

            }
            return strRet;

        }
        #endregion 

        #region 页面加载事件
        /// <summary>
        /// 页面加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            SetParentButtonEvent();
            lngUserID = (long)Session["UserID"];
            lngDeptID = (long)Session["UserDeptID"];
            lngOrgID = (long)Session["UserOrgID"];

            cpByts.On_PageIndexChanged = new Epower.ITSM.Web.Controls.ControlPageFoot.ControlPageFootDelegate(BindData);

            //获取评估(回访)权限
            blnDisplayFeedBack = CheckRight(Constant.feedbackright);

            //获得投诉权限
            reTrace = (RightEntity)((Hashtable)Session["UserAllRights"])[Constant.BYTSQuery];
            if (!Page.IsPostBack)
            {
                cboStatus.Items.Add(new ListItem("所有状态", "-1"));
                cboStatus.Items.Add(new ListItem("--正在处理", ((int)e_FlowStatus.efsHandle).ToString()));
                cboStatus.Items.Add(new ListItem("--正常结束", ((int)e_FlowStatus.efsEnd).ToString()));
                cboStatus.Items.Add(new ListItem("--流程暂停", ((int)e_FlowStatus.efsStop).ToString()));
                cboStatus.Items.Add(new ListItem("--流程终止", ((int)e_FlowStatus.efsAbort).ToString()));

                cboStatus.SelectedIndex = 1;

                //设置起始日期
                string sQueryBeginDate = "0";
                if (CommonDP.GetConfigValue("Other", "QueryBeginDate") != null)
                    sQueryBeginDate = CommonDP.GetConfigValue("Other", "QueryBeginDate").ToString();
                if (sQueryBeginDate == "0")
                {
                    txtMsgDateBegin.Text = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
                }
                else
                {
                    txtMsgDateBegin.Text = DateTime.Parse(sQueryBeginDate).ToString("yyyy-MM-dd");
                }
                txtMsgDateEnd.Text = DateTime.Now.ToString("yyyy-MM-dd");

                gridUndoMsg.Columns[gridUndoMsg.Columns.Count-1].Visible = CheckRight(Constant.admindeleteflow);  //删除流程权限
               
                Session["FromUrl"] = "../AppForms/frm_BYTS_Query.aspx";

                //快速搜索
                if (Request["svalue"] != null)
                {
                    txtBYPersonName.Text = Request["svalue"].ToString().Trim();
                }

                BindData();
               
            }

            //初始化日期
            this.Img1.Attributes.Add("onclick", "fPopUpDlg('../Controls/Calendar/calendar.htm',document.all." + this.Ctr_ReceiveTime.ClientID + ", 'winpop', 234, 261);return false");
            this.Img2.Attributes.Add("onclick", "fPopUpDlg('../Controls/Calendar/calendar.htm',document.all." + this.Ctr_ReceiveTimeEnd.ClientID + ", 'winpop', 234, 261);return false");
            this.imgSBegin.Attributes.Add("onclick", "fPopUpDlg('../Controls/Calendar/calendar.htm',document.all." + this.txtMsgDateBegin.ClientID + ", 'winpop', 234, 261);return false");
            this.imgEEnd.Attributes.Add("onclick", "fPopUpDlg('../Controls/Calendar/calendar.htm',document.all." + this.txtMsgDateEnd.ClientID + ", 'winpop', 234, 261);return false");
        }
        #endregion 

        #region 显示页面地址
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
        
        #region 数据绑定
        /// <summary>
        /// 数据绑定
        /// </summary>
        private void BindData()
        {
            int iRowCount = 0;
            XmlDocument xmlDoc = GetXmlValue();
            BYTSDP ee = new BYTSDP();
            DataTable dt = ee.GetIssuesForCond(xmlDoc.InnerXml, lngUserID, lngDeptID, lngOrgID, reTrace, string.Empty, this.cpByts.PageSize, this.cpByts.CurrentPage, ref iRowCount);
            gridUndoMsg.DataSource = dt.DefaultView;
            gridUndoMsg.Attributes.Add("style", "word-break:break-all;word-wrap:break-word");
            gridUndoMsg.DataBind();
            this.cpByts.RecordCount = iRowCount;
            this.cpByts.Bind();
        }


        private void IssueExportExcel()
        {
            XmlDocument xmlDoc = GetXmlValue();
            DataTable dt = BYTSDP.GetIssuesForCond(xmlDoc.InnerXml, lngUserID, lngDeptID, lngOrgID, reTrace, string.Empty);
            Epower.ITSM.Web.Common.ExcelExport.ExportBYList(this, dt, Session["UserID"].ToString());
        }

        #endregion 

        #region  生成查询XML字符串 GetXmlValue
        /// <summary>
        /// 生成查询XML字符串
        /// </summary>
        /// <returns></returns>
        private XmlDocument GetXmlValue()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement xmlRoot = xmlDoc.CreateElement("Fields");
            XmlElement xmlEle;
            #region Status    状态
            xmlEle = xmlDoc.CreateElement("Field");
            xmlEle.SetAttribute("FieldName", "Status");
            xmlEle.SetAttribute("Value", cboStatus.SelectedValue.ToString());
            xmlRoot.AppendChild(xmlEle);
            #endregion
            #region Status    投诉人
            xmlEle = xmlDoc.CreateElement("Field");
            xmlEle.SetAttribute("FieldName", "BYPersonName");
            xmlEle.SetAttribute("Value", txtBYPersonName.Text.ToString());
            xmlRoot.AppendChild(xmlEle);
            #endregion
            #region Status   被投诉人
            xmlEle = xmlDoc.CreateElement("Field");
            xmlEle.SetAttribute("FieldName", "Project");
            xmlEle.SetAttribute("Value", UserPicker1.UserID.ToString());
            xmlRoot.AppendChild(xmlEle);
            #endregion
            #region Status    来源渠道
            xmlEle = xmlDoc.CreateElement("Field");
            xmlEle.SetAttribute("FieldName", "Source");
            xmlEle.SetAttribute("Value", CataSource.CatelogID.ToString());
            xmlRoot.AppendChild(xmlEle);
            #endregion
            #region Status    类型

            xmlEle = xmlDoc.CreateElement("Field");
            xmlEle.SetAttribute("FieldName", "Type");
            xmlEle.SetAttribute("Value", CataType.CatelogID.ToString());
            xmlRoot.AppendChild(xmlEle);
            #endregion
            #region Status    性质
            xmlEle = xmlDoc.CreateElement("Field");
            xmlEle.SetAttribute("FieldName", "Kind");
            xmlEle.SetAttribute("Value", CataKind.CatelogID.ToString());
            xmlRoot.AppendChild(xmlEle);
            #endregion
            #region Status    接收时间开始时间
            xmlEle = xmlDoc.CreateElement("Field");
            xmlEle.SetAttribute("FieldName", "BY_ReceiveTime");
            xmlEle.SetAttribute("Value", Ctr_ReceiveTime.Text.ToString().Trim());
            xmlRoot.AppendChild(xmlEle);
            #endregion
            #region Status    接收时间结束时间
            xmlEle = xmlDoc.CreateElement("Field");
            xmlEle.SetAttribute("FieldName", "BY_ReceiveTimeEnd");
            xmlEle.SetAttribute("Value", Ctr_ReceiveTimeEnd.Text.ToString().Trim());
            xmlRoot.AppendChild(xmlEle);
            #endregion
            #region Status    登记起始日期
            xmlEle = xmlDoc.CreateElement("Field");
            xmlEle.SetAttribute("FieldName", "MessageBegin");
            xmlEle.SetAttribute("Value", txtMsgDateBegin.Text.ToString());
            xmlRoot.AppendChild(xmlEle);
            #endregion
            #region Status    登记截止日期
            xmlEle = xmlDoc.CreateElement("Field");
            xmlEle.SetAttribute("FieldName", "MessageEnd");
            xmlEle.SetAttribute("Value", txtMsgDateEnd.Text.ToString());
            xmlRoot.AppendChild(xmlEle);
            #endregion
            xmlDoc.AppendChild(xmlRoot);

            return xmlDoc;
        }
        #endregion 

        #region 查询事件Master_Master_Button_Query_Click
        /// <summary>
        /// 查询事件
        /// </summary>
        void Master_Master_Button_Query_Click()
        {
            BindData();
        }
        #endregion 

        #region 数据绑定
        /// <summary>
        /// 数据绑定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridUndoMsg_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                //当超过整个流程预计处理时限未处理的，红低显示
                if (int.Parse(e.Item.Cells[10].Text.Trim()) < 0)
                {
                    for (int i = 0; i < e.Item.Cells.Count; i++)
                    {
                        e.Item.Cells[i].ForeColor = Color.Red;
                    }
                }
                if (e.Item.Cells[7].Text.Trim() == "20")
                {
                    e.Item.Cells[7].Text = "正在处理";
                }
                else if (e.Item.Cells[7].Text.Trim() == "30")
                {
                    e.Item.Cells[7].Text = "正常结束";
                }
                else if (e.Item.Cells[7].Text.Trim() == "40")
                {
                    e.Item.Cells[7].Text = "流程暂停";
                }
                else if (e.Item.Cells[7].Text.Trim() == "50")
                {
                    e.Item.Cells[7].Text = "流程终止";
                }


                e.Item.Attributes.Add("onmouseover", "currentcolor=this.style.backgroundColor;this.style.backgroundColor='#FFFBE8'");
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=currentcolor");
                string sFlowID = DataBinder.Eval(e.Item.DataItem, "FlowID").ToString();
                e.Item.Attributes.Add("ondblclick", "window.open('../Forms/frmIssueView.aspx?FlowID=" + sFlowID.ToString() + "','MainFrame','scrollbars=yes,resizable=yes,top=0,left=0,width=window.availWidth-12,height=window.availHeight-10');");


                ((Label)e.Item.FindControl("Lb_ServiceNo")).Attributes.Add("onmouseover", "ShowDetailsInfo(this," + DataBinder.Eval(e.Item.DataItem, "By_id").ToString() + ",400);");

            }
        }
        #endregion 

        #region 检查权限
        /// <summary>
        /// 检查权限
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

        #region 得到标签的查询条件SQL  CreateWhereSql
        /// <summary>
        /// 得到标签的查询条件
        /// </summary>
        /// <param name="strkind"></param>
        /// <returns></returns>
        private string CreateWhereSql(string strkind)
        {
            string sWhere = string.Empty;                                //组合条件        
            int iOverHours = 0;                                         //超时小时数
            switch (strkind)
            {
                case "1":   //由我登记
                    sWhere = " AND a.RegUserID= " + lngUserID.ToString();
                    break;
                case "2":
                    //CtrTitle1.Title = "服务跟踪(我参与处理)";
                    sWhere = " AND a.flowid in (SELECT distinct flowid FROM es_message WHERE receiverid = " + lngUserID.ToString() +
                        " AND actortype =" + ((int)e_ActorClass.fmMasterActor).ToString() + ")";
                    break;
                case "3":
                   // CtrTitle1.Title = "服务跟踪(超时事件)";
                    iOverHours = 0;
                    sWhere += " AND ((datediff('Minute',sysdate,nvl(b.expectendtime,sysdate)) < (0- " + (iOverHours * 60).ToString() + ") AND " +
                          " b.status <> " + ((int)e_FlowStatus.efsEnd).ToString() + ")  OR " +
                          "( datediff('Minute',nvl(b.endtime,sysdate),nvl(b.expectendtime,sysdate)) < (0- " + (iOverHours * 60).ToString() + ")" +
                          " AND b.status = " + ((int)e_FlowStatus.efsEnd).ToString() + ") )";
                    break;
                case "4":
                    iOverHours = 48;
                    //CtrTitle1.Title = "服务跟踪(超时48小时事件)";
                    sWhere += " AND ((datediff('Minute',sysdate,nvl(b.expectendtime,sysdate)) < (0- " + (iOverHours * 60).ToString() + ") AND " +
                          " b.status <> " + ((int)e_FlowStatus.efsEnd).ToString() + ")  OR " +
                          "( datediff('Minute',nvl(b.endtime,sysdate),nvl(b.expectendtime,sysdate)) < (0- " + (iOverHours * 60).ToString() + ")" +
                          " AND b.status = " + ((int)e_FlowStatus.efsEnd).ToString() + ") )";
                    break;
                case "5":
                    //CtrTitle1.Title = "服务跟踪(超时完成事件)";
                    sWhere += " AND datediff('Minute',nvl(b.endtime,sysdate),nvl(b.expectendtime,sysdate)) < 0 AND b.status= " + ((int)e_FlowStatus.efsEnd).ToString();
                    break;
                case "6":
                    //CtrTitle1.Title = "服务跟踪(超时未完成事件)";
                    sWhere += " AND datediff('Minute',sysdate,nvl(b.expectendtime,sysdate)) < 0 AND b.status<> " + ((int)e_FlowStatus.efsEnd).ToString();
                    break;
                default:
                    break;
            }
            return sWhere;
        }
        #endregion 
       

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridUndoMsg_ItemCreated(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                DataGrid dg = (DataGrid)sender;
                for (int i = 0; i < e.Item.Cells.Count; i++)
                {
                    if (i<8)
                    {
                        e.Item.Cells[i].Attributes.Add("onclick", "sortTable('" + dg.ClientID + "'," + i.ToString() + ",0);");
                    }
                }
            }
        }

        #endregion 

        #region 删除流程事件 gridUndoMsg_DeleteCommand
        /// <summary>
        /// 删除流程事件
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void gridUndoMsg_DeleteCommand(object source, DataGridCommandEventArgs e)
        {
            BindData();
        }
        #endregion 
  
    }
}
