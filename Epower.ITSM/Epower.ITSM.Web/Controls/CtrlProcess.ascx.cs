/*******************************************************************
 * 版权所有：
 * Description：工作流流程处理控件
 * 
 * 
 * Create By  ：
 * Create Date：
 * *****************************************************************/
using System;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Epower.DevBase.BaseTools;
using EpowerGlobal;
using EpowerCom;
using Epower.ITSM.SqlDAL;
using Epower.ITSM.Base;

namespace Epower.ITSM.Web.Controls
{
    /// <summary>
    ///		CtrlProcess 的摘要说明。
    /// </summary>
    public partial class CtrlProcess : System.Web.UI.UserControl
    {
        private eOA_FlowProcessType iFPType = eOA_FlowProcessType.efptReadFinished;
        private bool bDeleteMessage = false;   //删除会签人员后，会签汇总判断

        #region 属性

        /// <summary>
        /// 流程处理类别
        /// </summary>
        public eOA_FlowProcessType FlowProcessType
        {
            set { iFPType = value; }
        }


        /// <summary>
        /// 
        /// </summary>
        public long FlowID
        {
            set { ViewState["FlowID"] = value; }
            get
            {
                if (ViewState["FlowID"] != null)
                    return long.Parse(ViewState["FlowID"].ToString());
                else
                    return 0;
            }
        }

        public long FlowModelID
        {
            set { ViewState["FlowModelID"] = value; }
            get
            {
                if (ViewState["FlowModelID"] != null)
                    return long.Parse(ViewState["FlowModelID"].ToString());
                else
                    return 0;
            }
        }

        /// <summary>
        /// 是否现实办理类型
        /// 默认显示
        /// </summary>
        public bool ShowHandleType
        {
            set { ViewState["ShowHandleType"] = value; }
            get
            {
                if (ViewState["ShowHandleType"] != null)
                    return (bool)ViewState["ShowHandleType"];
                else
                    return true;
            }
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
            if (!IsPostBack)
            {
                InitPage();
            }
            else
            {
                bDeleteMessage = false;
                string strSender = Request.Form["ctl00$hidTarget"];
                string strPara = Request.Form["ctl00$hidPara"];
                if (strSender == this.ClientID)
                {
                    //判断是否是删除 督办意见
                    long lngDeleteMessageID = long.Parse(strPara);
                    Message message = new Message();
                    message.DeleteNotMasterMessage(long.Parse(Session["UserID"].ToString()), lngDeleteMessageID);
                    bDeleteMessage = true;
                    InitPage();
                }
            }
        }
        #endregion

        #region InitPage
        private void InitPage()
        {
            long lngUserID = long.Parse(Session["UserID"].ToString());

            int iRowCount = 0;

            long lngMessageID = 0;
            long lngReceiverID = 0;
            long lngSenderID = 0;
            int iIsRead = 0;
            long lngCurrUserID = long.Parse(Session["UserID"].ToString());

            int iContactAuto = 0;
            iContactAuto = int.Parse(CommonDP.GetConfigValue("Other", "ContactAuto"));

            bool blnCanAttemper = MessageDep.CanDoHelpDeskAttemper(lngUserID, this.FlowID);
            bool blnOnlyRedo = false;
            bool blnHasPause = MessageProcess.HasFlowPauseLog(this.FlowID);
            string sProcess = "";
            string sMsgIDs = "";
            string sReader = "";
            string sTransmit = "";
            e_FlowStatus fs = Flow.GetFlowStatus(this.FlowID);
            DataTable dt = MessageProcess.GetProcessStatus(this.FlowModelID, this.FlowID);
            DataView dv = new DataView(dt);
            DataView dvTO = new DataView(dt);
            foreach (DataRow dr in dt.Rows)
            {

                lngMessageID = long.Parse(dr["MessageID"].ToString());
                lngReceiverID = long.Parse(dr["receiverid"].ToString());
                lngSenderID = long.Parse(dr["SenderID"].ToString());
                iIsRead = int.Parse(dr["IsRead"].ToString());

                if ((blnCanAttemper == true && iFPType != eOA_FlowProcessType.eftpReceiveMsg) || lngReceiverID > 0)
                {
                    //没有调度权限 并且 未接收的消息不显示出来

                    if (sMsgIDs.IndexOf(lngMessageID.ToString() + ",") < 0)//判断ID不重复
                    {
                        string sDate = dr["dotime"].ToString();
                        string sOpinion = dr["opinion"].ToString();
                        string sAddOpinion = dr["mpcontent"].ToString();   // 2007-08-30 补充业务处理过程描述[来自业务接口]
                        string sToMsgID = dr["tmessageid"].ToString();
                        string sTo = "";
                        string sTR = "";

                        e_MessageStatus ms = (e_MessageStatus)StringTool.String2Long(dr["status"].ToString());

                        //e_NodeStatus ns = (e_NodeStatus)StringTool.String2Long(dr["nodestatus"].ToString());

                        //-------------------接收消息-------------------//
                        dv.RowFilter = "MessageID=" + lngMessageID.ToString();
                        string sReceiveType = "";
                        //TO消息
                        for (int n = 0; n < dv.Count; n++)
                        {
                            #region 内循环代码
                            //--接收类型
                            dvTO.RowFilter = "MessageID=" + dv[n]["TMessageID"].ToString() + " AND receiverid <> 0";

                            if (dvTO.Count > 0)
                            {
                                e_MsgReceiveType mrt = (e_MsgReceiveType)StringTool.String2Long(dvTO[0]["receivetype"].ToString());
                                e_MsgSelfType mst = (e_MsgSelfType)StringTool.String2Long(dvTO[0]["messagetype"].ToString());


                                blnOnlyRedo = false;
                                //仅重审的驳回 不能调度
                                if (mrt == e_MsgReceiveType.emrtBackHasDoneRedo)
                                {
                                    blnOnlyRedo = true;
                                }

                                switch (mrt)
                                {
                                    case e_MsgReceiveType.emrtaNormal:
                                        sReceiveType = "提交";
                                        break;
                                    case e_MsgReceiveType.emrtInfuxSumTo:
                                        sReceiveType = "汇总给";
                                        break;

                                    case e_MsgReceiveType.emrtBack:
                                        if (dvTO[0]["receiverid"].ToString() == dvTO[0]["senderid"].ToString())
                                        {
                                            sReceiveType = "返回";
                                        }
                                        else
                                        {
                                            sReceiveType = "退回";
                                        }
                                        break;
                                    case e_MsgReceiveType.emrtSkip:
                                        sReceiveType = "跳转至";
                                        break;
                                    case e_MsgReceiveType.emrtBackHasDone:
                                    case e_MsgReceiveType.emrtBackHasDoneRedo:
                                    case e_MsgReceiveType.emrtBackHasDoneFlow:
                                        sReceiveType = "驳回到";
                                        break;
                                    case e_MsgReceiveType.emrtTakeOver:
                                        sReceiveType = "交接给";
                                        break;
                                    case e_MsgReceiveType.emrtAttemper:
                                        sReceiveType = "给";
                                        break;
                                    case e_MsgReceiveType.emrtReDoBack:
                                        sReceiveType = "提交重审给";
                                        break;
                                }
                                if (mst != e_MsgSelfType.emstCommunic && mst != e_MsgSelfType.emstAssist && mst != e_MsgSelfType.emstTransmit)
                                {
                                    // 协作 转发  沟通时不在处理过程中展示
                                    sTo += IsDeletedUser(dvTO[0]["tactors"].ToString(), dvTO[0]["deleted"].ToString()) + ",";//+

                                    
                                }
                                //SP()+"进行"+SP()+
                                //dvTO[0]["nodename"].ToString()+";";
                            }
                            else
                            {
                                //没有查询到ＴＭＥＳＳＡＧＥＩＤ详细资料的情况,
                                //理论上的可能性只有当前ReceiverID为0 或者 没有处理，分流等待环节　是没有后续消息 的时候
                                if (ms == e_MessageStatus.emsFinished && fs != e_FlowStatus.efsEnd
                                    && dr["rectactors"].ToString().Trim() != "")
                                {
                                    sTo = SP() + "等待" + SP() + dr["rectactors"].ToString() + " 处理";
                                }
                            }

                            #endregion
                        }

                        if (sTo != "" && dvTO.Count > 0)
                        {
                            sTo = sReceiveType + SP() + sTo.TrimEnd(',') + SP() + "进行" + SP() + dvTO[0]["nodename"].ToString();
                            if ((e_MsgReceiveType)StringTool.String2Long(dr["receivetype"].ToString()) == e_MsgReceiveType.emrtInfuxSumTo)
                            {
                                sTo = sTo + "[会签汇总]";
                            }

                        }

                        //处理意见
                        sOpinion = sOpinion.Trim() == "" ? "" : "&nbsp;处理意见:<font color=green>" + StringTool.ParseForHtml(sOpinion) + "</font>";
                        sAddOpinion = sAddOpinion.Trim() == "" ? "" : "&nbsp;<font color=red>" + StringTool.ParseForHtml(sAddOpinion) + "</font>";

                        //----------处理意见----------------//
                        string sStatusName = "";
                        switch (ms)
                        {
                            case e_MessageStatus.emsFinished:
                                sStatusName = "";
                                break;
                            case e_MessageStatus.emsHandle:
                                sStatusName = "处理中";
                                break;
                            case e_MessageStatus.emsStop:
                                sStatusName = "暂停";
                                break;
                            case e_MessageStatus.emsWaiting:
                                sStatusName = "挂起";
                                break;
                        }

                        // ----   是否关联其它流程　---//
                        string sLinkFlowID = dr["LinkFlowID"].ToString();
                        if (sLinkFlowID != "")
                        {
                            //存在关联
                            //if (lngUserID == long.Parse(dr["receiverid"].ToString()))
                            if (MessageDep.CanTrackSubFlow(lngUserID, long.Parse(dr["receiverid"].ToString()), this.FlowID, this.FlowModelID, long.Parse(dr["nodemodelid"].ToString()), long.Parse(sLinkFlowID)))
                            {
                                sTo = SP() + "<A href=\"javascript:GetSubProcessShot(" + lngMessageID.ToString() + ");\">查看子流程处理过程</A>";
                            }
                            else
                            {
                                sTo = SP() + "关联了其它流程事务";
                            }
                        }

                        //-----------------角色类型--------------------//
                        string sActorType = "";
                        e_ActorClass at = (e_ActorClass)StringTool.String2Long(dr["actortype"].ToString());
                        e_FMNodeType fmnodetype = (e_FMNodeType)StringTool.String2Long(dr["nmtype"].ToString());

                        switch (at)
                        {
                            case e_ActorClass.fmAssistActor:
                                sActorType = "协办";
                                break;
                            case e_ActorClass.fmMasterActor:
                                sActorType = "主办";
                                //if (lngUserID != lngReceiverID && lngReceiverID != 0 && ms == e_MessageStatus.emsHandle && blnCanAttemper == true)
                                if ((lngUserID != lngReceiverID || lngReceiverID == 0) && fmnodetype != e_FMNodeType.fmDraft && ms == e_MessageStatus.emsHandle && blnCanAttemper == true && blnOnlyRedo == false)
                                {
                                    //主办的情况  并且待办 起草环节不调度
                                    sActorType += "(<A href=\"javascript:ProcessDoAttemperCtr(" + lngMessageID.ToString() + ");\">调度</A>)";
                                }
                                break;
                            case e_ActorClass.fmReaderActor:
                                sActorType = "阅知";
                                break;
                            case e_ActorClass.fmCommunicActor:
                                sActorType = "沟通";
                                break;
                            case e_ActorClass.fmInfluxActor:
                                sActorType = "会签";
                                break;
                        }
                        //接收时间     接收者    (环节名称      状态  意见,意见和后续消息)
                        if (at == e_ActorClass.fmReaderActor)
                        {
                            //string ST=ms==e_MessageStatus.emsFinished?"<font color=blue>已阅</font>":"<font color=red>未阅</font>";
                            string ST = "";
                            if (ms == e_MessageStatus.emsFinished && dr["isread"].ToString() == "1")
                            {
                                ST = "<font color=blue>已阅</font>";
                            }
                            else if (dr["isread"].ToString() == "1")
                            {
                                ST = "<font color=green>已读</font>";
                            }
                            else
                            {
                                ST = "<font color=red>未阅</font>";
                            }

                            //string ST = ms == e_MessageStatus.emsFinished ? "<font color=blue>已阅</font>" : "<font color=red>未阅</font>";

                            if (dr["messagetype"].ToString() == ((int)e_MsgSelfType.emstTransmit).ToString())
                            {
                                sTransmit += IsDeletedUser(dr["tactors"].ToString(), dr["deleted"].ToString()) + SP() +
                                "(" + dr["factors"].ToString() + "&nbsp;&nbsp;" + dr["receivetime"].ToString() + "转" + ":" + ST + ")";

                                if (sOpinion.Length != 0)
                                {
                                    sTransmit += "［" + sOpinion + "］";
                                }
                                if (at != e_ActorClass.fmMasterActor && lngSenderID == lngCurrUserID && iIsRead == 0)  //非主办，当前用户是发送人，未读过，可以删除
                                {
                                    //sActorType += "<INPUT class='btnClass' type='button' id='btnDelete" + lngMessageID.ToString() + @"' value='删除' onClick=""if(confirm('是否真的要删除？')){ __doPostBackCustomize('" + this.ClientID + "','" + lngMessageID.ToString() + @"');return false;}"">";
                                    sTransmit += "(<A href='#' onclick=\"if(confirm('是否真的要删除？')){ __doPostBackCustomize('" + this.ClientID + "','" + lngMessageID.ToString() + @"');return false;}"">删除</A>)";
                                }
                                sTransmit += ";&nbsp;&nbsp;";
                            }
                            else
                            {
                                sReader += IsDeletedUser(dr["tactors"].ToString(), dr["deleted"].ToString()) + SP() +
                                "(" + dr["nodename"].ToString() + ":" + ST + ")";
                                if (sOpinion.Length != 0)
                                {
                                    sReader += "［" + sOpinion + "］";
                                }
                                if (at != e_ActorClass.fmMasterActor && lngSenderID == lngCurrUserID && iIsRead == 0)  //非主办，当前用户是发送人，未读过，可以删除
                                {
                                    //sReader += "<INPUT class='btnClass' type='button' id='btnDelete" + lngMessageID.ToString() + @"' value='删除' onClick=""if(confirm('是否真的要删除？')){ __doPostBackCustomize('" + this.ClientID + "','" + lngMessageID.ToString() + @"');return false;}"">";
                                    sReader += "(<A href='#' onclick=\"if(confirm('是否真的要删除？')){ __doPostBackCustomize('" + this.ClientID + "','" + lngMessageID.ToString() + @"');return false;}"">删除</A>)";
                                }
                                sReader += ";&nbsp;&nbsp;";
                            }

                        }
                        else
                        {
                            string scol = dr["nodename"].ToString();
                            if ((e_MsgReceiveType)StringTool.String2Long(dr["receivetype"].ToString()) == e_MsgReceiveType.emrtInfuxSumTo)
                            {
                                scol = scol + "[会签汇总]";
                                if (bDeleteMessage) //20110125 zmc 如果为删除后，会签汇总时，应该跳到会签汇总界面
                                {
                                    Response.Redirect("~/Forms/flow_Normal.aspx?MessageID=" + lngMessageID.ToString());
                                    return;
                                }
                            }
                            if ((e_MsgReceiveType)StringTool.String2Long(dr["receivetype"].ToString()) == e_MsgReceiveType.emrtReDoBack)
                            {
                                scol = scol + "[重审]";
                            }
                            scol += "&nbsp;" + sStatusName;

                            if (sOpinion + sTo != "")//意见和后续消息
                                scol += "," + sAddOpinion + sOpinion + SP() + sTo;

                            if (this.ShowHandleType)
                            {
                                sTR += AddTD(sDate + "&nbsp;", "noWrap class='list'");
                                if (iContactAuto == 0 || iContactAuto == 2)
                                {
                                    sTR += AddTD(AddContactMenu(dr["email"].ToString(), iRowCount++), "class='list'");
                                }
                                if (iContactAuto == 1 || iContactAuto == 2)
                                {
                                    sTR += AddTD(AddContactQQMenu(dr["QQ"].ToString(), iRowCount++), "class='list'");
                                }

                                //朱明春20100104添加
                                if (at != e_ActorClass.fmMasterActor && lngSenderID == lngCurrUserID && iIsRead == 0)  //非主办，当前用户是发送人，未读过，可以删除
                                {
                                    sActorType += "(<A href='#' onclick=\"if(confirm('是否真的要删除？')){ __doPostBackCustomize('" + this.ClientID + "','" + lngMessageID.ToString() + @"');return false;}"">删除</A>)";
                                    //sTR += AddTD(@"<INPUT class='btnClass' type='button' id='btnDelete" + lngMessageID.ToString() + @"' value='删除' onClick=""if(confirm('是否真的要删除？')){ __doPostBackCustomize('" + this.ClientID + "','" + lngMessageID.ToString() + @"');return false;}"">", " nowrap class='list'");
                                }
                                sTR += AddTD(IsDeletedUser(dr["tactors"].ToString(), dr["deleted"].ToString()) + "&nbsp;", " nowrap class='list' onmouseover=\"GetFlowShotInfo(this," + lngReceiverID + ");\" onmouseout=\"hideMe('divShowMessageDetail','none');\" onclick=\"showuser(" + lngReceiverID + ");\" style=\"cursor:hand\" ") +
                                    AddTD(sActorType, "noWrap class='list'") +
                                    AddTD(scol, "width=100% class='list'");
                            }
                            else
                            {
                                sTR += AddTD(sDate + "&nbsp;", "noWrap class='list'");
                                if (iContactAuto == 0 || iContactAuto == 2)
                                {
                                    sTR += AddTD(AddContactMenu(dr["email"].ToString(), iRowCount++), "class='list'");
                                }
                                if (iContactAuto == 1 || iContactAuto == 2)
                                {
                                    sTR += AddTD(AddContactQQMenu(dr["QQ"].ToString(), iRowCount++), "class='list'");
                                }
                                sTR += AddTD(IsDeletedUser(dr["tactors"].ToString(), dr["deleted"].ToString()) + "&nbsp;", "nowrap class='list'") +
                                    AddTD(scol, "width=100%");
                            }

                            sTR = "<tr class='tablebody'>" + sTR + "</tr>";
                            sProcess += sTR;
                        }

                        sMsgIDs += dr["MessageID"].ToString() + ",";
                    }

                }
            }
            string sTrReader = string.Empty;
            string sTrTransmit = string.Empty;
            if (sReader != string.Empty)
                sTrReader = "<tr class='list'>" + AddTD(sReader.TrimEnd(';'), "colspan=6 class='list'") + "</tr>";
            if (sTransmit != string.Empty)
                sTrTransmit = "<tr class='list'>" + AddTD(sTransmit.TrimEnd(';'), "colspan=6 class='list'") + "</tr>";
            string sTrLookPause = "";
            if (blnHasPause == true)
                sTrLookPause = "<tr class='list'>" + AddTD("<A href=\"javascript:GetFlowPauseShot(" + this.FlowID.ToString() + ");\">查看流程暂停记录</A>", "colspan=6 class='list'") + "</tr>";
            string sTrFlowEnd = "";
            if (fs != e_FlowStatus.efsHandle)
            {
                string sFstatus = (fs == e_FlowStatus.efsEnd) ? "结束" : (fs == e_FlowStatus.efsStop ? "暂停" : "终止");
                sTrFlowEnd = "<tr  class='list'>" + AddTD("--流程已经" + sFstatus + "--", "colspan=6 class='list'") + "</tr>";
            }
            if (sProcess != "")
                litProcess.Text = "<table style='width:100%;word-break:break-all;' class='listContent'>" + sProcess + sTrReader + sTrTransmit + sTrLookPause + sTrFlowEnd + "</table>";
            else
                litProcess.Text = SP();
        }
        #endregion

        #region AddContactMenu
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sUserEmail"></param>
        /// <param name="iRowCount"></param>
        /// <returns></returns>
        private string AddContactMenu(string sUserEmail, int iRowCount)
        {
            string strHtml;
            if (sUserEmail.Trim() != "")
            {
                strHtml = @"<TABLE><TR><TD><IMG id='tdImgContact" + iRowCount.ToString() +
                    " alt='' width='12' src='" + Epower.ITSM.Base.Constant.ApplicationPath + "/images/blank.gif' onload=\"IMNRC('" + sUserEmail +
                    "')\"></TD></TR></TABLE>";
            }
            else
            {
                strHtml = @"<TABLE><TR><TD><IMG id='tdImgContact" + iRowCount.ToString() +
                    " alt='' width='12' src='" + Epower.ITSM.Base.Constant.ApplicationPath + "/images/blank.gif' ></TD></TR></TABLE>";
            }
            return strHtml;
        }
        #endregion

        #region AddContactQQMenu
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sUserQQ"></param>
        /// <param name="iRowCount"></param>
        /// <returns></returns>
        private string AddContactQQMenu(string sUserQQ, int iRowCount)
        {
            string strHtml;
            if (sUserQQ.Trim() != "")
            {
                strHtml = @"<TABLE><TR><TD><IMG id='tdImgContactQQ" + iRowCount.ToString() +
                    " alt='' width='12' src='" + Epower.ITSM.Base.Constant.ApplicationPath + "/images/a2.ico' onclick=\"window.open('http://wpa.qq.com/msgrd?V=1&Uin=" + sUserQQ +
                    "')\"></TD></TR></TABLE>";
            }
            else
            {
                strHtml = @"<TABLE><TR><TD><IMG id='tdImgContactQQ" + iRowCount.ToString() +
                    " alt='' width='12' src='" + Epower.ITSM.Base.Constant.ApplicationPath + "/images/blank.gif' ></TD></TR></TABLE>";
            }
            return strHtml;
        }
        #endregion

        #region TD
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sText"></param>
        /// <returns></returns>
        private string AddTD(string sText)
        {
            string str = "<td>" + sText + "</td>";
            return str;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sText"></param>
        /// <param name="sAttrib"></param>
        /// <returns></returns>
        private string AddTD(string sText, string sAttrib)
        {
            string str = "<td " + sAttrib + " >" + sText + "</td>";
            return str;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private static string SP()
        {
            return "&nbsp;";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        private static string SP(int n)
        {
            string str = "";
            for (int j = 0; j <= n; j++)
                str += "&nbsp;";

            

            return str;
        }
        #endregion


        /// <summary>
        /// 若该处理人已被删除,则以灰色标识.
        /// </summary>
        /// <param name="strActorName"></param>
        /// <param name="deleted"></param>
        /// <returns></returns>
        private string IsDeletedUser(string strActorName, string deleted)
        {
            if (deleted == "0")
                return strActorName;

            return string.Format("<span style=\"color:gray;\">{0}</span>", strActorName);
        }

    }
}
