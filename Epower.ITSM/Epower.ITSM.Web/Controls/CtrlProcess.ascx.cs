/*******************************************************************
 * ��Ȩ���У�
 * Description�����������̴���ؼ�
 * 
 * 
 * Create By  ��
 * Create Date��
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
    ///		CtrlProcess ��ժҪ˵����
    /// </summary>
    public partial class CtrlProcess : System.Web.UI.UserControl
    {
        private eOA_FlowProcessType iFPType = eOA_FlowProcessType.efptReadFinished;
        private bool bDeleteMessage = false;   //ɾ����ǩ��Ա�󣬻�ǩ�����ж�

        #region ����

        /// <summary>
        /// ���̴������
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
        /// �Ƿ���ʵ��������
        /// Ĭ����ʾ
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
                    //�ж��Ƿ���ɾ�� �������
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
                    //û�е���Ȩ�� ���� δ���յ���Ϣ����ʾ����

                    if (sMsgIDs.IndexOf(lngMessageID.ToString() + ",") < 0)//�ж�ID���ظ�
                    {
                        string sDate = dr["dotime"].ToString();
                        string sOpinion = dr["opinion"].ToString();
                        string sAddOpinion = dr["mpcontent"].ToString();   // 2007-08-30 ����ҵ�����������[����ҵ��ӿ�]
                        string sToMsgID = dr["tmessageid"].ToString();
                        string sTo = "";
                        string sTR = "";

                        e_MessageStatus ms = (e_MessageStatus)StringTool.String2Long(dr["status"].ToString());

                        //e_NodeStatus ns = (e_NodeStatus)StringTool.String2Long(dr["nodestatus"].ToString());

                        //-------------------������Ϣ-------------------//
                        dv.RowFilter = "MessageID=" + lngMessageID.ToString();
                        string sReceiveType = "";
                        //TO��Ϣ
                        for (int n = 0; n < dv.Count; n++)
                        {
                            #region ��ѭ������
                            //--��������
                            dvTO.RowFilter = "MessageID=" + dv[n]["TMessageID"].ToString() + " AND receiverid <> 0";

                            if (dvTO.Count > 0)
                            {
                                e_MsgReceiveType mrt = (e_MsgReceiveType)StringTool.String2Long(dvTO[0]["receivetype"].ToString());
                                e_MsgSelfType mst = (e_MsgSelfType)StringTool.String2Long(dvTO[0]["messagetype"].ToString());


                                blnOnlyRedo = false;
                                //������Ĳ��� ���ܵ���
                                if (mrt == e_MsgReceiveType.emrtBackHasDoneRedo)
                                {
                                    blnOnlyRedo = true;
                                }

                                switch (mrt)
                                {
                                    case e_MsgReceiveType.emrtaNormal:
                                        sReceiveType = "�ύ";
                                        break;
                                    case e_MsgReceiveType.emrtInfuxSumTo:
                                        sReceiveType = "���ܸ�";
                                        break;

                                    case e_MsgReceiveType.emrtBack:
                                        if (dvTO[0]["receiverid"].ToString() == dvTO[0]["senderid"].ToString())
                                        {
                                            sReceiveType = "����";
                                        }
                                        else
                                        {
                                            sReceiveType = "�˻�";
                                        }
                                        break;
                                    case e_MsgReceiveType.emrtSkip:
                                        sReceiveType = "��ת��";
                                        break;
                                    case e_MsgReceiveType.emrtBackHasDone:
                                    case e_MsgReceiveType.emrtBackHasDoneRedo:
                                    case e_MsgReceiveType.emrtBackHasDoneFlow:
                                        sReceiveType = "���ص�";
                                        break;
                                    case e_MsgReceiveType.emrtTakeOver:
                                        sReceiveType = "���Ӹ�";
                                        break;
                                    case e_MsgReceiveType.emrtAttemper:
                                        sReceiveType = "��";
                                        break;
                                    case e_MsgReceiveType.emrtReDoBack:
                                        sReceiveType = "�ύ�����";
                                        break;
                                }
                                if (mst != e_MsgSelfType.emstCommunic && mst != e_MsgSelfType.emstAssist && mst != e_MsgSelfType.emstTransmit)
                                {
                                    // Э�� ת��  ��ͨʱ���ڴ��������չʾ
                                    sTo += IsDeletedUser(dvTO[0]["tactors"].ToString(), dvTO[0]["deleted"].ToString()) + ",";//+

                                    
                                }
                                //SP()+"����"+SP()+
                                //dvTO[0]["nodename"].ToString()+";";
                            }
                            else
                            {
                                //û�в�ѯ���ԣͣţӣӣ��ǣţɣ���ϸ���ϵ����,
                                //�����ϵĿ�����ֻ�е�ǰReceiverIDΪ0 ���� û�д��������ȴ����ڡ���û�к�����Ϣ ��ʱ��
                                if (ms == e_MessageStatus.emsFinished && fs != e_FlowStatus.efsEnd
                                    && dr["rectactors"].ToString().Trim() != "")
                                {
                                    sTo = SP() + "�ȴ�" + SP() + dr["rectactors"].ToString() + " ����";
                                }
                            }

                            #endregion
                        }

                        if (sTo != "" && dvTO.Count > 0)
                        {
                            sTo = sReceiveType + SP() + sTo.TrimEnd(',') + SP() + "����" + SP() + dvTO[0]["nodename"].ToString();
                            if ((e_MsgReceiveType)StringTool.String2Long(dr["receivetype"].ToString()) == e_MsgReceiveType.emrtInfuxSumTo)
                            {
                                sTo = sTo + "[��ǩ����]";
                            }

                        }

                        //�������
                        sOpinion = sOpinion.Trim() == "" ? "" : "&nbsp;�������:<font color=green>" + StringTool.ParseForHtml(sOpinion) + "</font>";
                        sAddOpinion = sAddOpinion.Trim() == "" ? "" : "&nbsp;<font color=red>" + StringTool.ParseForHtml(sAddOpinion) + "</font>";

                        //----------�������----------------//
                        string sStatusName = "";
                        switch (ms)
                        {
                            case e_MessageStatus.emsFinished:
                                sStatusName = "";
                                break;
                            case e_MessageStatus.emsHandle:
                                sStatusName = "������";
                                break;
                            case e_MessageStatus.emsStop:
                                sStatusName = "��ͣ";
                                break;
                            case e_MessageStatus.emsWaiting:
                                sStatusName = "����";
                                break;
                        }

                        // ----   �Ƿ�����������̡�---//
                        string sLinkFlowID = dr["LinkFlowID"].ToString();
                        if (sLinkFlowID != "")
                        {
                            //���ڹ���
                            //if (lngUserID == long.Parse(dr["receiverid"].ToString()))
                            if (MessageDep.CanTrackSubFlow(lngUserID, long.Parse(dr["receiverid"].ToString()), this.FlowID, this.FlowModelID, long.Parse(dr["nodemodelid"].ToString()), long.Parse(sLinkFlowID)))
                            {
                                sTo = SP() + "<A href=\"javascript:GetSubProcessShot(" + lngMessageID.ToString() + ");\">�鿴�����̴������</A>";
                            }
                            else
                            {
                                sTo = SP() + "������������������";
                            }
                        }

                        //-----------------��ɫ����--------------------//
                        string sActorType = "";
                        e_ActorClass at = (e_ActorClass)StringTool.String2Long(dr["actortype"].ToString());
                        e_FMNodeType fmnodetype = (e_FMNodeType)StringTool.String2Long(dr["nmtype"].ToString());

                        switch (at)
                        {
                            case e_ActorClass.fmAssistActor:
                                sActorType = "Э��";
                                break;
                            case e_ActorClass.fmMasterActor:
                                sActorType = "����";
                                //if (lngUserID != lngReceiverID && lngReceiverID != 0 && ms == e_MessageStatus.emsHandle && blnCanAttemper == true)
                                if ((lngUserID != lngReceiverID || lngReceiverID == 0) && fmnodetype != e_FMNodeType.fmDraft && ms == e_MessageStatus.emsHandle && blnCanAttemper == true && blnOnlyRedo == false)
                                {
                                    //��������  ���Ҵ��� ��ݻ��ڲ�����
                                    sActorType += "(<A href=\"javascript:ProcessDoAttemperCtr(" + lngMessageID.ToString() + ");\">����</A>)";
                                }
                                break;
                            case e_ActorClass.fmReaderActor:
                                sActorType = "��֪";
                                break;
                            case e_ActorClass.fmCommunicActor:
                                sActorType = "��ͨ";
                                break;
                            case e_ActorClass.fmInfluxActor:
                                sActorType = "��ǩ";
                                break;
                        }
                        //����ʱ��     ������    (��������      ״̬  ���,����ͺ�����Ϣ)
                        if (at == e_ActorClass.fmReaderActor)
                        {
                            //string ST=ms==e_MessageStatus.emsFinished?"<font color=blue>����</font>":"<font color=red>δ��</font>";
                            string ST = "";
                            if (ms == e_MessageStatus.emsFinished && dr["isread"].ToString() == "1")
                            {
                                ST = "<font color=blue>����</font>";
                            }
                            else if (dr["isread"].ToString() == "1")
                            {
                                ST = "<font color=green>�Ѷ�</font>";
                            }
                            else
                            {
                                ST = "<font color=red>δ��</font>";
                            }

                            //string ST = ms == e_MessageStatus.emsFinished ? "<font color=blue>����</font>" : "<font color=red>δ��</font>";

                            if (dr["messagetype"].ToString() == ((int)e_MsgSelfType.emstTransmit).ToString())
                            {
                                sTransmit += IsDeletedUser(dr["tactors"].ToString(), dr["deleted"].ToString()) + SP() +
                                "(" + dr["factors"].ToString() + "&nbsp;&nbsp;" + dr["receivetime"].ToString() + "ת" + ":" + ST + ")";

                                if (sOpinion.Length != 0)
                                {
                                    sTransmit += "��" + sOpinion + "��";
                                }
                                if (at != e_ActorClass.fmMasterActor && lngSenderID == lngCurrUserID && iIsRead == 0)  //�����죬��ǰ�û��Ƿ����ˣ�δ����������ɾ��
                                {
                                    //sActorType += "<INPUT class='btnClass' type='button' id='btnDelete" + lngMessageID.ToString() + @"' value='ɾ��' onClick=""if(confirm('�Ƿ����Ҫɾ����')){ __doPostBackCustomize('" + this.ClientID + "','" + lngMessageID.ToString() + @"');return false;}"">";
                                    sTransmit += "(<A href='#' onclick=\"if(confirm('�Ƿ����Ҫɾ����')){ __doPostBackCustomize('" + this.ClientID + "','" + lngMessageID.ToString() + @"');return false;}"">ɾ��</A>)";
                                }
                                sTransmit += ";&nbsp;&nbsp;";
                            }
                            else
                            {
                                sReader += IsDeletedUser(dr["tactors"].ToString(), dr["deleted"].ToString()) + SP() +
                                "(" + dr["nodename"].ToString() + ":" + ST + ")";
                                if (sOpinion.Length != 0)
                                {
                                    sReader += "��" + sOpinion + "��";
                                }
                                if (at != e_ActorClass.fmMasterActor && lngSenderID == lngCurrUserID && iIsRead == 0)  //�����죬��ǰ�û��Ƿ����ˣ�δ����������ɾ��
                                {
                                    //sReader += "<INPUT class='btnClass' type='button' id='btnDelete" + lngMessageID.ToString() + @"' value='ɾ��' onClick=""if(confirm('�Ƿ����Ҫɾ����')){ __doPostBackCustomize('" + this.ClientID + "','" + lngMessageID.ToString() + @"');return false;}"">";
                                    sReader += "(<A href='#' onclick=\"if(confirm('�Ƿ����Ҫɾ����')){ __doPostBackCustomize('" + this.ClientID + "','" + lngMessageID.ToString() + @"');return false;}"">ɾ��</A>)";
                                }
                                sReader += ";&nbsp;&nbsp;";
                            }

                        }
                        else
                        {
                            string scol = dr["nodename"].ToString();
                            if ((e_MsgReceiveType)StringTool.String2Long(dr["receivetype"].ToString()) == e_MsgReceiveType.emrtInfuxSumTo)
                            {
                                scol = scol + "[��ǩ����]";
                                if (bDeleteMessage) //20110125 zmc ���Ϊɾ���󣬻�ǩ����ʱ��Ӧ��������ǩ���ܽ���
                                {
                                    Response.Redirect("~/Forms/flow_Normal.aspx?MessageID=" + lngMessageID.ToString());
                                    return;
                                }
                            }
                            if ((e_MsgReceiveType)StringTool.String2Long(dr["receivetype"].ToString()) == e_MsgReceiveType.emrtReDoBack)
                            {
                                scol = scol + "[����]";
                            }
                            scol += "&nbsp;" + sStatusName;

                            if (sOpinion + sTo != "")//����ͺ�����Ϣ
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

                                //������20100104���
                                if (at != e_ActorClass.fmMasterActor && lngSenderID == lngCurrUserID && iIsRead == 0)  //�����죬��ǰ�û��Ƿ����ˣ�δ����������ɾ��
                                {
                                    sActorType += "(<A href='#' onclick=\"if(confirm('�Ƿ����Ҫɾ����')){ __doPostBackCustomize('" + this.ClientID + "','" + lngMessageID.ToString() + @"');return false;}"">ɾ��</A>)";
                                    //sTR += AddTD(@"<INPUT class='btnClass' type='button' id='btnDelete" + lngMessageID.ToString() + @"' value='ɾ��' onClick=""if(confirm('�Ƿ����Ҫɾ����')){ __doPostBackCustomize('" + this.ClientID + "','" + lngMessageID.ToString() + @"');return false;}"">", " nowrap class='list'");
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
                sTrLookPause = "<tr class='list'>" + AddTD("<A href=\"javascript:GetFlowPauseShot(" + this.FlowID.ToString() + ");\">�鿴������ͣ��¼</A>", "colspan=6 class='list'") + "</tr>";
            string sTrFlowEnd = "";
            if (fs != e_FlowStatus.efsHandle)
            {
                string sFstatus = (fs == e_FlowStatus.efsEnd) ? "����" : (fs == e_FlowStatus.efsStop ? "��ͣ" : "��ֹ");
                sTrFlowEnd = "<tr  class='list'>" + AddTD("--�����Ѿ�" + sFstatus + "--", "colspan=6 class='list'") + "</tr>";
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
        /// ���ô������ѱ�ɾ��,���Ի�ɫ��ʶ.
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
