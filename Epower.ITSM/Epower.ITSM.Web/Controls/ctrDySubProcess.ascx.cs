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

        #region ���Զ�����
        /// <summary>
        /// �����б�,��ʽ FlowID|FlowModelID,FlowID|FlowModelID....
        /// </summary>
        public string SubFlowList
        {
            set
            {
                ViewState[this.ID + "SubFlowList"] = value;

                //�ط�ʱ��ֵ ���¼��ؽ���
               

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
                litProcess.Text = "<table width='100%' ><tr><td class='listTitle' width='100%'>�����̴������</td></tr><tr><td width='100%'>" + litProcess.Text + "</td></tr></table>";
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
                                //--��������
                                dvTO.RowFilter = "MessageID=" + dv[n]["TMessageID"].ToString() + " AND receiverid <> 0";

                                if (dvTO.Count > 0)
                                {
                                    e_MsgReceiveType mrt = (e_MsgReceiveType)StringTool.String2Long(dvTO[0]["receivetype"].ToString());
                                    e_MsgSelfType mst = (e_MsgSelfType)StringTool.String2Long(dvTO[0]["messagetype"].ToString());
                                    switch (mrt)
                                    {
                                        case e_MsgReceiveType.emrtaNormal:
                                            sReceiveType = "�ύ";
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
                                        sTo += dvTO[0]["tactors"].ToString() + ",";//+
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

                            //// ----   �Ƿ�����������̡�---//
                            //string sLinkFlowID = dr["LinkFlowID"].ToString();
                            //if (sLinkFlowID != "")
                            //{
                            //    //���ڹ���
                            //    if (lngUserID == long.Parse(dr["receiverid"].ToString()))
                            //    {
                            //        sTo = SP() + "<A href=\"javascript:alert('����......');\">�鿴��������</A>";
                            //    }
                            //    else
                            //    {
                            //        sTo = SP() + "������������������";
                            //    }
                            //}

                            //-----------------��ɫ����--------------------//
                            string sActorType = "";
                            e_ActorClass at = (e_ActorClass)StringTool.String2Long(dr["actortype"].ToString());
                            switch (at)
                            {
                                case e_ActorClass.fmAssistActor:
                                    sActorType = "Э��";
                                    break;
                                case e_ActorClass.fmMasterActor:
                                    sActorType = "����";
                                   
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
                                    sTransmit += dr["tactors"].ToString() + SP() +
                                    "(" + dr["factors"].ToString() + dr["receivetime"].ToString() + "ת" + ":" + ST + ")";

                                    if (sOpinion.Length != 0)
                                    {
                                        sTransmit += "��" + sOpinion + "��;&nbsp;&nbsp;";
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
                                        sReader += "��" + sOpinion + "��;&nbsp;&nbsp;";
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
                                    scol = scol + "[��ǩ����]";
                                }
                                if ((e_MsgReceiveType)StringTool.String2Long(dr["receivetype"].ToString()) == e_MsgReceiveType.emrtReDoBack)
                                {
                                    scol = scol + "[����]";
                                }
                                scol += "&nbsp;" + sStatusName;
                                if (sOpinion + sTo != "")//����ͺ�����Ϣ
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
                    string sFstatus = (fs == e_FlowStatus.efsEnd) ? "����" : (fs == e_FlowStatus.efsStop ? "��ͣ" : "��ֹ");
                    sTrFlowEnd = "<tr  class='list'>" + AddTD("--�������Ѿ�" + sFstatus + "--", "colspan=4") + "</tr>";
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
                    LoadHtmlControls(this.SubFlowList);   //Ŀǰ�Ƕ�̬���ؿؼ�,�ط�ʱ��Ҫ��ִ��һ��,�Ժ��Ϊ�̶��ؼ��ķ�ʽ
            //}
        }
    }
}