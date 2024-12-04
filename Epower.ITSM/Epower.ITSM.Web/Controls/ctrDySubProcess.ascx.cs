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
using EpowerGlobal;
using EpowerCom;
using Epower.ITSM.SqlDAL;

namespace Epower.ITSM.Web.Controls
{
    public partial class ctrDySubProcess : System.Web.UI.UserControl
    {

        #region 属性定义区
        /// <summary>
        /// 流程列表,格式 FlowID|FlowModelID,FlowID|FlowModelID....
        /// </summary>
        public string SubFlowList
        {
            set
            {
                ViewState[this.ID + "SubFlowList"] = value;

                //回发时赋值 重新加载界面
               

            }
            get
            {
                if (ViewState[this.ID + "SubFlowList"] == null)
                {
                    return "";
                }
                else
                {
                    return ViewState[this.ID + "SubFlowList"].ToString();
                }

            }
        }
        #endregion


        private void LoadHtmlControls(string sList)
        {
            string[] sFlows = sList.Split(",".ToCharArray());
            bool blnHas = false;
            for (int i = 0; i < sFlows.Length; i++)
            {
                string[] arr = sFlows[i].Split("|".ToCharArray());
                AddSubFlowProcess(long.Parse(arr[0]), long.Parse(arr[1]));
                blnHas = true;
            }
            if (blnHas == true)
            {
                litProcess.Text = "<table width='100%' ><tr><td class='listTitle' width='100%'>子流程处理过程</td></tr><tr><td width='100%'>" + litProcess.Text + "</td></tr></table>";
            }
        }

        private void AddSubFlowProcess(long lngFlowID, long lngFlowModelID)
        {
            long lngUserID = long.Parse(Session["UserID"].ToString());

            int iRowCount = 0;

            long lngMessageID = 0;
            long lngReceiverID = 0;

            int iContactAuto = 0;
            iContactAuto = int.Parse(CommonDP.GetConfigValue("Other", "ContactAuto"));

            if (!IsPostBack)
            {
                string sProcess = "";
                string sMsgIDs = "";
                string sReader = "";
                string sTransmit = "";
                e_FlowStatus fs = Flow.GetFlowStatus(lngFlowID);
                DataTable dt = MessageProcess.GetProcessStatus(lngFlowModelID, lngFlowID);
                DataView dv = new DataView(dt);
                DataView dvTO = new DataView(dt);
                foreach (DataRow dr in dt.Rows)
                {

                    lngMessageID = long.Parse(dr["MessageID"].ToString());
                    lngReceiverID = long.Parse(dr["receiverid"].ToString());

                    if (lngReceiverID > 0)
                    {
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
                                //--接收类型
                                dvTO.RowFilter = "MessageID=" + dv[n]["TMessageID"].ToString() + " AND receiverid <> 0";

                                if (dvTO.Count > 0)
                                {
                                    e_MsgReceiveType mrt = (e_MsgReceiveType)StringTool.String2Long(dvTO[0]["receivetype"].ToString());
                                    e_MsgSelfType mst = (e_MsgSelfType)StringTool.String2Long(dvTO[0]["messagetype"].ToString());
                                    switch (mrt)
                                    {
                                        case e_MsgReceiveType.emrtaNormal:
                                            sReceiveType = "提交";
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
                                        sTo += dvTO[0]["tactors"].ToString() + ",";//+
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

                            //// ----   是否关联其它流程　---//
                            //string sLinkFlowID = dr["LinkFlowID"].ToString();
                            //if (sLinkFlowID != "")
                            //{
                            //    //存在关联
                            //    if (lngUserID == long.Parse(dr["receiverid"].ToString()))
                            //    {
                            //        sTo = SP() + "<A href=\"javascript:alert('待续......');\">查看关联流程</A>";
                            //    }
                            //    else
                            //    {
                            //        sTo = SP() + "关联了其它流程事务";
                            //    }
                            //}

                            //-----------------角色类型--------------------//
                            string sActorType = "";
                            e_ActorClass at = (e_ActorClass)StringTool.String2Long(dr["actortype"].ToString());
                            switch (at)
                            {
                                case e_ActorClass.fmAssistActor:
                                    sActorType = "协办";
                                    break;
                                case e_ActorClass.fmMasterActor:
                                    sActorType = "主办";
                                   
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
                                    sTransmit += dr["tactors"].ToString() + SP() +
                                    "(" + dr["factors"].ToString() + dr["receivetime"].ToString() + "转" + ":" + ST + ")";

                                    if (sOpinion.Length != 0)
                                    {
                                        sTransmit += "［" + sOpinion + "］;&nbsp;&nbsp;";
                                    }
                                    else
                                    {
                                        sTransmit += ";&nbsp;&nbsp;";
                                    }
                                }
                                else
                                {
                                    sReader += dr["tactors"].ToString() + SP() +
                                    "(" + dr["nodename"].ToString() + ":" + ST + ")";
                                    if (sOpinion.Length != 0)
                                    {
                                        sReader += "［" + sOpinion + "］;&nbsp;&nbsp;";
                                    }
                                    else
                                    {
                                        sReader += ";&nbsp;&nbsp;";
                                    }
                                }

                            }
                            else
                            {
                                string scol = dr["nodename"].ToString();
                                if ((e_MsgReceiveType)StringTool.String2Long(dr["receivetype"].ToString()) == e_MsgReceiveType.emrtInfuxSumTo)
                                {
                                    scol = scol + "[会签汇总]";
                                }
                                if ((e_MsgReceiveType)StringTool.String2Long(dr["receivetype"].ToString()) == e_MsgReceiveType.emrtReDoBack)
                                {
                                    scol = scol + "[重审]";
                                }
                                scol += "&nbsp;" + sStatusName;
                                if (sOpinion + sTo != "")//意见和后续消息
                                    scol += "," + sAddOpinion + sOpinion + SP() + sTo;

                                sTR += AddTD(sDate + "&nbsp;", "noWrap class='list'");

                                sTR += AddTD(dr["tactors"].ToString() + "&nbsp;", "nowrap class='list'") +
                                    AddTD(sActorType, "noWrap class='list'") +
                                    AddTD(scol, "width=100% class='list'");
                                

                                sTR = "<tr class='tablebody'>" + sTR + "</tr>";
                                sProcess += sTR;
                            }

                            sMsgIDs += dr["MessageID"].ToString() + ",";
                        }

                    }
                }
                string sTrReader = string.Empty;
                string sTrTransmit = string.Empty;
                string sTrFlowEnd = string.Empty;
                if(sReader!=string.Empty)
                    sTrReader = "<tr class='list'>" + AddTD(sReader.TrimEnd(';'), "colspan=4") + "</tr>";
                if (sTransmit!=string.Empty)
                    sTrTransmit = "<tr class='list'>" + AddTD(sTransmit.TrimEnd(';'), "colspan=4") + "</tr>";
                
                if (fs != e_FlowStatus.efsHandle)
                {
                    string sFstatus = (fs == e_FlowStatus.efsEnd) ? "结束" : (fs == e_FlowStatus.efsStop ? "暂停" : "终止");
                    sTrFlowEnd = "<tr  class='list'>" + AddTD("--子流程已经" + sFstatus + "--", "colspan=4") + "</tr>";
                }
                if (sProcess != "")
                {
                    string sSubTitle = Flow.GetFlowSubjectName(lngFlowID);

                    litProcess.Text += "<table style='width:100%;word-break:break-all;' cellspacing='0' cellpadding='0' class='listContent'>" +
                        //"<tr><td width='20%' class='listTitle' align='center' style='word-break:break-all' ><a href='javascript:viewFlowInfo("+ lngFlowID.ToString() + ");' target='_blank'>" + sSubTitle + "</a></td>" +
                                        "<tr><td width='20%' class='listTitle' align='center' style='word-break:break-all' ><A href='#' onclick=\"window.open('" + Epower.ITSM.Base.Constant.ApplicationPath + "/Forms/frmIssueView.aspx?NewWin=true&FlowID=" + lngFlowID.ToString() + "','','scrollbars=yes,resizable=yes,top=0,left=0,width=window.availWidth-12,height=window.availHeight-10');\" >" + sSubTitle + "</A></td>" +
                                        "<td width='80%'><table cellspacing='2' cellpadding='1' rules='all' border='0' style='border-color:White;border-width:0px;width:100%;word-break:break-all;' class='listContent'>" + sProcess + sTrReader + sTrTransmit + sTrFlowEnd + "</table></td></tr>" +
                                        "</Table>";
                }
                else
                    litProcess.Text += SP();

            }
        }

        private string AddContactMenu(string sUserEmail, int iRowCount)
        {
            string strHtml;
            if (sUserEmail.Trim() != "")
            {
                strHtml = @"<TABLE><TR><TD><IMG id='tdImgContact" + iRowCount.ToString() +
                    " alt='' width='12' src='" + Epower.ITSM.Base.Constant.ApplicationPath +  "/images/blank.gif' onload=\"IMNRC('" + sUserEmail +
                    "')\"></TD></TR></TABLE>";
            }
            else
            {
                strHtml = @"<TABLE><TR><TD><IMG id='tdImgContact" + iRowCount.ToString() +
                    " alt='' width='12' src='" + Epower.ITSM.Base.Constant.ApplicationPath + "/images/blank.gif' ></TD></TR></TABLE>";
            }
            return strHtml;
        }

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


        private string AddTD(string sText)
        {
            string str = "<td>" + sText + "</td>";
            return str;
        }

        private string AddTD(string sText, string sAttrib)
        {
            string str = "<td " + sAttrib + " >" + sText + "</td>";
            return str;
        }

        private static string SP()
        {
            return "&nbsp;";
        }

        private static string SP(int n)
        {
            string str = "";
            for (int j = 0; j <= n; j++)
                str += "&nbsp;";

            return str;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (Page.IsPostBack == false)
            //{
                if (this.SubFlowList != "")
                    LoadHtmlControls(this.SubFlowList);   //目前是动态加载控件,回发时需要再执行一次,以后改为固定控件的方式
            //}
        }
    }
}