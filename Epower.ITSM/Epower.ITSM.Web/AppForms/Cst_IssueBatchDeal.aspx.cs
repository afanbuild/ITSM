/*******************************************************************
 * 版权所有：
 * Description：批量指派
 * 
 * 
 * Create By  ：zhumc
 * Create Date：2010-12-01
 * *****************************************************************/
using System;
using System.Data;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Drawing;
using Epower.DevBase.BaseTools;
using Epower.ITSM.SqlDAL;
using EpowerCom;
using EpowerGlobal;

namespace Epower.ITSM.Web.AppForms
{
    public partial class Cst_IssueBatchDeal : BasePage
    {
        #region Page_Load
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData();
            }
        }
        #endregion 

        #region 脚本调用方法区
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lngNoticeID"></param>
        /// <returns></returns>
        protected string GetUrl(decimal lngFlowID)
        {
            //暂时没处理分页
            string sUrl = "";
            sUrl = "javascript:window.open('../Forms/frmIssueView.aspx?FlowID=" + lngFlowID.ToString() + "','MainFrame','scrollbars=yes,resizable=yes,top=0,left=0,width=window.availWidth-12,height=window.availHeight-10');";

            return sUrl;


        }
        #endregion 

        #region BindData
        /// <summary>
        /// 
        /// </summary>
        private void BindData()
        {
            long lngUserID = long.Parse(Session["UserID"].ToString());
            DataTable dt = ZHServiceDP.GetBatchDealData(lngUserID,string.Empty);

            gridReceiveMsg.DataSource = dt;
            gridReceiveMsg.DataBind();
        }
        #endregion 

        #region DataGrid排序 gridUndoMsg_ItemCreated
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
                for (int i = 1; i < e.Item.Cells.Count - 10; i++)
                {
                    int j = i - 0;
                    e.Item.Cells[i].Attributes.Add("onclick", "sortTable('" + dg.ClientID + "'," + j.ToString() + ",0);");
                }
            }
        }
        #endregion

        #region btnConfirm_Click 批量指派
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnReceiver_Click(object sender, EventArgs e)
        {
            FormDefineTool fdt = new FormDefineTool();
            fdt.Add(0, " ", e_ActorClass.fmMasterActor, e_DeptORUser.fmUser);
            string strDefineXmlValue = fdt.GetXmlObject().InnerXml;

            int iCount = 0;
            for (int i = 0; i < gridReceiveMsg.Items.Count; i++)
            {
                CheckBox chkSelect = (CheckBox)gridReceiveMsg.Items[i].Cells[0].FindControl("chkSelect");
                if (chkSelect.Checked)
                {
                    long lngFlowID = long.Parse(gridReceiveMsg.Items[i].Cells[14].Text.Trim());
                    long lngMessageID = long.Parse(gridReceiveMsg.Items[i].Cells[15].Text.Trim());
                    long lngNodeModelID = long.Parse(gridReceiveMsg.Items[i].Cells[16].Text.Trim());
                    long lngFlowModelID = long.Parse(gridReceiveMsg.Items[i].Cells[17].Text.Trim());
                    //long lngAppID = long.Parse(gridReceiveMsg.Items[i].Cells[19].Text.Trim());
                    long lngUserID = long.Parse(Session["UserID"].ToString());
                    
                    long lngActionID = -2;
                    //批量审批处理接收动作
                    DataRow[] drs = FlowModel.GetNodeActionNew(lngFlowModelID, lngNodeModelID);
                    foreach (DataRow r in drs)
                    {
                        if (r["busactionid"].ToString().Trim() == "10002")   //总行科技部处理
                        {
                            lngActionID = long.Parse(r["ActionID"].ToString());
                        }
                    }
                    if (lngActionID != -2)
                    {
                        FlowBatchApproved(lngFlowModelID, lngFlowID, lngMessageID, lngActionID, lngUserID, strDefineXmlValue);
                        iCount++;
                    }
                }
            }
            if (iCount != 0)
            {
                PageTool.MsgBox(this, "批量指派成功，指派了" + iCount.ToString() + "张单！");
                BindData();
            }
            else
            {
                Epower.DevBase.BaseTools.PageTool.MsgBox(this, "未选择批量指派记录！");
            }
        }
        #endregion 

        #region FlowBatchApproved   批量指派
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lngFlowModelID"></param>
        /// <param name="lngFlowID"></param>
        /// <param name="lngMessageID"></param>
        /// <param name="lngActionID"></param>
        /// <param name="lngUserID"></param>
        /// <param name="strDefineXmlValue"></param>
        private void FlowBatchApproved( long lngFlowModelID, long lngFlowID, long lngMessageID, long lngActionID, long lngUserID, string strDefineXmlValue)
        {
            string strAttachment = Message.GetAttachmentXml(lngFlowID);
            long lngLinkNodeID = 0;
            long lngLinkNodeType = 0;
            string strFormXMLValue = "";
            string strReceivers = "";
            string sAllRec = "<Receivers></Receivers>";
            string sRecNameList = "";

            Message msgObj = new Message();

            objFlow objflow = new objFlow(lngUserID, lngFlowModelID, lngMessageID);
            strFormXMLValue = GetFormXmlValue(objflow);

            sAllRec = msgObj.GetNextReceivers(lngUserID, lngMessageID, lngFlowModelID, lngActionID, e_SpecRightType.esrtNormal, 0, strFormXMLValue, 1, strDefineXmlValue);

            GenerateMemberSelect(sAllRec, ref lngLinkNodeID, ref lngLinkNodeType, ref strReceivers, ref sRecNameList);

            msgObj.SendFlow(lngUserID, lngMessageID, lngActionID, e_SpecRightType.esrtNormal, lngLinkNodeID, lngLinkNodeType, 1, string.Empty, strFormXMLValue, strReceivers, strAttachment);

        }
        #endregion

        #region 自动获取其中一个处理人的信息
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strRet"></param>
        /// <param name="lngNodeID"></param>
        /// <param name="lngNodeType"></param>
        /// <param name="strRec"></param>
        /// <param name="sNameList"></param>
        private void GenerateMemberSelect(string strRet, ref long lngNodeID, ref long lngNodeType, ref string strRec, ref string sNameList)
        {
            long lngReceiverID = 0;
            string strNodeName = "";
            string strReceiverName = "";
            long lngCurrNodeID = 0;
            e_FMNodeType lngCurrNodeType = e_FMNodeType.fmNormal;   //当前环节的类别

            e_ReceiveActorType lngCurrReceiveType = e_ReceiveActorType.eratNone;

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(strRet);
            int iCount = 0;

            lngNodeID = long.Parse(xmlDoc.DocumentElement.Attributes["LinkNodeID"].Value);
            lngNodeType = long.Parse(xmlDoc.DocumentElement.Attributes["LinkNodeType"].Value);

            XmlNodeList nodes = xmlDoc.SelectNodes("NextReceivers/Nodes/Node");

            XmlDocument xmlRec = new XmlDocument();
            xmlRec.LoadXml("<Receivers></Receivers>");

            foreach (XmlNode node in nodes)
            {

                lngCurrNodeID = long.Parse(node.Attributes["ID"].Value);
                lngCurrNodeType = (e_FMNodeType)(int.Parse(node.Attributes["NodeType"].Value));
                strNodeName = node.Attributes["Name"].Value;

                XmlNodeList workers = node.SelectNodes("Workers/Worker");
                foreach (XmlNode wk in workers)
                {
                    iCount++;
                    //短信处理 默认取每个环节的第一个主办，选择的方式以后开发

                    lngReceiverID = long.Parse(wk.Attributes["ID"].Value);
                    strReceiverName = wk.Attributes["Name"].Value;

                    sNameList += (sNameList.Length > 0 ? "," : "") + strReceiverName + "(" + strNodeName + ")";

                    XmlElement xmlEle = xmlRec.CreateElement("Receiver");
                    xmlEle.SetAttribute("ID", "1");
                    xmlEle.SetAttribute("NodeID", lngCurrNodeID.ToString());
                    xmlEle.SetAttribute("NodeType", ((int)lngCurrNodeType).ToString());
                    xmlEle.SetAttribute("UserID", lngReceiverID.ToString());
                    xmlEle.SetAttribute("Name", strReceiverName);
                    xmlEle.SetAttribute("ActorType", "Worker_");
                    if ((e_FMNodeType)lngNodeType == e_FMNodeType.fmOrgNode || (e_FMNodeType)lngNodeType == e_FMNodeType.fmDisperse)
                    {
                        //机构环节或发散环节只选择组
                        lngCurrReceiveType = (e_ReceiveActorType)(int.Parse(wk.Attributes["ReceiveType"].Value.Trim()));
                        xmlEle.SetAttribute("ReceiveType", ((int)lngCurrReceiveType).ToString());

                    }
                    else
                    {
                        //普通环节设置接收角色为空

                        xmlEle.SetAttribute("ReceiveType", "");
                    }

                    xmlRec.DocumentElement.AppendChild(xmlEle);


                    break;
                }

            }
            strRec = xmlRec.InnerXml;

        }
        #endregion

        #region GetFormXmlValue
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objflow"></param>
        /// <returns></returns>
        private string GetFormXmlValue( EpowerCom.objFlow objflow)
        {
            string sXml = string.Empty;
            appDataProcess.ImplDataProcess dp = new appDataProcess.ImplDataProcess(objflow.AppID);
            DataTable dt = dp.GetFieldsDataSet(objflow.FlowID, objflow.OpID).Tables[0];

            if (dt.Rows.Count > 0)
            {
                FieldValues fv = new FieldValues();
                DataRow row = dt.Rows[0];
                fv.Add("ServiceID", row["SMSID"].ToString());
                fv.Add("Subject", row["Subject"].ToString());
                fv.Add("Content", row["Content"].ToString());
                fv.Add("RegUserID", row["RegUserID"].ToString());
                fv.Add("RegUserName", row["RegUserName"].ToString());
                fv.Add("ServiceLevelID", row["ServiceLevelID"].ToString());
                fv.Add("ServiceLevel", row["ServiceLevel"].ToString());
                fv.Add("ServiceLevelChange", "false");


                fv.Add("ServiceTypeID", row["ServiceTypeID"].ToString());
                fv.Add("ServiceType", row["ServiceType"].ToString());
                fv.Add("ServiceKindID", row["ServiceKindID"].ToString());
                fv.Add("ServiceKind", row["ServiceKind"].ToString());

                fv.Add("EffectID", row["EffectID"].ToString());
                fv.Add("EffectName", row["EffectName"].ToString());
                fv.Add("InstancyID", row["InstancyID"].ToString());
                fv.Add("InstancyName", row["InstancyName"].ToString());
                fv.Add("DealStatusID", row["DealStatusID"].ToString());
                fv.Add("DealStatus", row["DealStatus"].ToString());
                fv.Add("CustTime", row["CustTime"].ToString());
                fv.Add("CustID", row["CustID"].ToString());
                fv.Add("CustName", row["CustName"].ToString());
                fv.Add("CustAddress", row["CustAddress"].ToString());
                fv.Add("Contact", row["Contact"].ToString());
                fv.Add("CTel", row["CTel"].ToString());
                fv.Add("CustCode", row["CustCode"].ToString());
                fv.Add("Email", row["Email"].ToString());
                fv.Add("MastCust", row["MastCust"].ToString());

                fv.Add("EquipmentID", row["EquipmentID"].ToString());
                fv.Add("EquipmentName", row["EquipmentName"].ToString());
                fv.Add("EquPositions", row["EquPositions"].ToString());
                fv.Add("EquCode", row["EquCode"].ToString());
                fv.Add("EquSN", row["EquSN"].ToString());
                fv.Add("EquModel", row["EquModel"].ToString());
                fv.Add("EquBreed", row["EquBreed"].ToString());
                fv.Add("DealContent", row["DealContent"].ToString());
                fv.Add("Outtime", row["Outtime"].ToString());
                fv.Add("ServiceTime", row["ServiceTime"].ToString());
                fv.Add("FinishedTime", row["FinishedTime"].ToString());
                fv.Add("SjwxrID", row["SjwxrID"].ToString());
                fv.Add("Sjwxr", row["Sjwxr"].ToString());
                fv.Add("TotalAmount", row["TotalAmount"].ToString());
                fv.Add("OrgID", row["OrgID"].ToString());
                fv.Add("RegSysUserID", row["RegSysUserID"].ToString());
                fv.Add("RegSysUser", row["RegSysUser"].ToString());

                fv.Add("EmailNotify", "false");
                fv.Add("SMSNotify", "false");

                fv.Add("RegDeptID", row["RegDeptID"].ToString());
                fv.Add("RegDeptName", row["RegDeptName"].ToString());
                fv.Add("serviceno", row["serviceno"].ToString());
                fv.Add("buildCode", row["buildCode"].ToString());
                fv.Add("ReportingTime", row["ReportingTime"].ToString());

                fv.Add("Flag", "false");  //区分标志

                fv.Add("pubRequestID", "0");   //2009-04-25 增加 公众请求ID
                fv.Add("DeskTypeID", row["DeskTypeID"].ToString());
                fv.Add("DeskTypeName", row["DeskTypeName"].ToString());

                fv.Add("TypeID", row["BusinessTypeID"].ToString());
                fv.Add("ActionBusTypeID", row["ActionBusTypeID"].ToString() == "" ? "0" : row["ActionBusTypeID"].ToString());
                fv.Add("ActionBusTypeName", row["ActionBusTypeName"].ToString());

                fv.Add("SolutionDeptID", row["SolutionDeptID"].ToString() == "" ? "0" : row["SolutionDeptID"].ToString());
                fv.Add("SolutionDeptName", row["SolutionDeptName"].ToString());
                fv.Add("ProcessedTime", row["ProcessedTime"].ToString());
                fv.Add("CloseReasonID", row["CloseReasonID"].ToString() == "" ? "0" : row["CloseReasonID"].ToString());
                fv.Add("CloseReasonName", row["CloseReasonName"].ToString());
                fv.Add("ReSouseID", row["ReSouseID"].ToString() == "" ? "0" : row["ReSouseID"].ToString());
                fv.Add("ReSouseName", row["ReSouseName"].ToString());
                fv.Add("CTEL1", row["CTEL1"].ToString());
                sXml = fv.GetXmlObject().InnerXml;
            }
            return sXml;
        }
        #endregion 

        #region gridUndoMsg_ItemDataBound
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridUndoMsg_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            e_FlowStatus fs;
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {

                //当超过整个流程预计处理时限未处理的，红低显示
                fs = (e_FlowStatus)int.Parse(e.Item.Cells[gridReceiveMsg.Columns.Count - 5].Text);

                if (int.Parse(e.Item.Cells[gridReceiveMsg.Columns.Count - 4].Text.Trim()) < 0 && fs != e_FlowStatus.efsEnd)
                {
                    for (int i = 0; i < e.Item.Cells.Count; i++)
                    {
                        e.Item.Cells[i].ForeColor = Color.Red;
                    }
                }
                ((Label)e.Item.FindControl("Lb_ServiceNo")).Attributes.Add("onmouseover", "ShowDetailsInfo(this," + DataBinder.Eval(e.Item.DataItem, "SMSID").ToString() + ",400);");
                e.Item.Attributes.Add("onmouseover", "currentcolor=this.style.backgroundColor;this.style.backgroundColor='#FFFBE8'");
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=currentcolor");
                long lngFlowID = long.Parse(e.Item.Cells[14].Text.Trim());
                e.Item.Attributes.Add("ondblclick", "window.open('../Forms/frmIssueView.aspx?FlowID=" + lngFlowID.ToString() + "&randomid='+GetRandom(),'MainFrame','scrollbars=yes,resizable=yes,top=0,left=0,width=window.availWidth-12,height=window.availHeight-10');");
            }
        }
        #endregion 
    }
}
