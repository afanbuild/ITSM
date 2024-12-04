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

using EpowerCom;
using EpowerGlobal;

namespace Epower.ITSM.Web.Forms
{
    public partial class frmFlowNodeShot :BasePage
    {
        long lngFlowModelID = 0;
        long lngNodeModelID = 0;
        long lngFlowID = 0;
        e_FMNodeType lngNodeType = e_FMNodeType.eEmpty;
        int iHasDo = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Request.QueryString["FlowModelid"] != null)
                lngFlowModelID = long.Parse(Request.QueryString["FlowModelid"]);

            if (this.Request.QueryString["NodeModelid"] != null)
                lngNodeModelID = long.Parse(Request.QueryString["NodeModelid"]);

            if (this.Request.QueryString["Flowid"] != null)
                lngFlowID = long.Parse(Request.QueryString["Flowid"]);
            if (this.Request.QueryString["HasDo"] != null)
                iHasDo = int.Parse(Request.QueryString["HasDo"]);
            if (!Page.IsPostBack)
            {
                if (lngFlowModelID != 0 && lngNodeModelID != 0)
                {
                    ObjNodeModel onode = new ObjNodeModel(lngFlowModelID, lngNodeModelID);
                    lngNodeType = onode.Type;

                    if (lngNodeType != e_FMNodeType.fmStart && lngNodeType != e_FMNodeType.fmEnd && lngNodeType != e_FMNodeType.fmUnite && lngNodeType != e_FMNodeType.fmDistribute)
                    {

                        lblType.Text = onode.NodeBusName;
                        lblName.Text = onode.NodeName;
                        lblRights.Text = GetRightsList(onode);
                        if (onode.TotalHours != 0 || onode.WarningHours != 0)
                            lblTimes.Text = "���ʱ��:" + onode.TotalHours + getTimeUnit(onode.TimeUnit) + ",Ԥ��ʱ��:" + onode.WarningHours + getTimeUnit(onode.TimeUnit);
                        if (onode.Remark.Length == 0)
                        {
                            trRemark.Visible = false;
                        }
                        else
                        {
                            lblRemark.Text = onode.Remark;
                        }
                        if (iHasDo == 1 && lngFlowID != 0)
                        {
                            trActors.Visible = false;
                            litMessage.Text = getMessageInfo(onode);
                        }
                        else
                        {
                            trDealInfo.Visible = false;
                            litProcess.Text = getNodeActorsInfo(onode);
                            if (litProcess.Text == "")
                                trActors.Visible = false;
                        }
                    }
                }
            }

            
        }

        private string getActorName(e_ActorType at, long lngID)
        {
            string ret = string.Empty;
            switch (at)
            {
                case e_ActorType.fmPersonActor:
                    ret = EPSystem.GetUserName(lngID);
                    break;
                case e_ActorType.fmDeptActor:
                    ret = EPSystem.GetDeptName(lngID);
                    break;
                case e_ActorType.fmSystemActors:
                    ret = EPSystem.GetSysActorName(lngID);
                    break;
                case e_ActorType.fmSenderColleague:
                    ret = "�۷�����Ա�Ĳ��ų�Ա��";
                    break;
                case e_ActorType.fmSenderLeader:
                    ret = "�۷�����Ա�Ĳ������ܣ�";
                    break;
                case e_ActorType.fmStarter:
                    ret = "�������Ա��";
                    break;
                case e_ActorType.fmStarterColleague:
                    ret = "�������Ա�Ĳ��ų�Ա��";
                    break;
                case e_ActorType.fmStarterLeader:
                    ret = "�������Ա�Ĳ������ܣ�";
                    break;
                case e_ActorType.fmStarterDown:
                    ret = "�������Ա�¼����ų�Ա��";
                    break;
                case e_ActorType.fmStarterUp:
                    ret = "�������Ա�ϼ����ų�Ա��";
                    break;
                case e_ActorType.fmStarterSame:
                    ret = "�������Աͬ�����ų�Ա��";
                    break;
                case e_ActorType.fmSenderDown:
                    ret = "�۷�����Ա�¼����ų�Ա��";
                    break;
                case e_ActorType.fmSenderUp:
                    ret = "�۷�����Ա�ϼ����ų�Ա��";
                    break;
                case e_ActorType.fmSenderSame:
                    ret = "�۷�����Աͬ�����ų�Ա��";
                    break;
                        
                case e_ActorType.fmNodeActor:
                    ret = "�ۻ���(" + FlowModel.GetFlowNodeModelName(lngFlowModelID, lngID) + ")�������";
                    break;
                case e_ActorType.fmNodeColleague:
                    ret = "�ۻ���(" + FlowModel.GetFlowNodeModelName(lngFlowModelID, lngID) + ")����Ĳ��ų�Ա��";
                    break;
                case e_ActorType.fmNodeLeader:
                    ret = "�ۻ���(" + FlowModel.GetFlowNodeModelName(lngFlowModelID, lngID) + ")����Ĳ������ܣ�";
                    break;
                case e_ActorType.fmFormDefine:
                    ret = "�۱�ָ����Ա��";
                    break;
                case e_ActorType.fmSenderUpLeader:
                    ret = "�۷�����Ա���ϼ����ܣ�";
                    break;
                case e_ActorType.fmStarterUpLeader:
                    ret = "�������Ա���ϼ����ܣ�";
                    break;
                case e_ActorType.fmSenderProfUpLeader:
                    ret = "�۷�����Ա��רҵ�ϼ����ܣ�";
                    break;
                case e_ActorType.fmStarterProfUpLeader:
                    ret = "�������Ա��רҵ�ϼ����ܣ�";
                    break;
                case e_ActorType.fmStarterProfUp:
                    ret = "�������Աרҵ�ϼ����ų�Ա��";
                    break;
                case e_ActorType.fmSenderProfUp:
                    ret = "�۷�����Աרҵ�ϼ����ų�Ա��";
                    break;
                case e_ActorType.fmNodeUpLeader:
                    ret = "�ۻ���(" + FlowModel.GetFlowNodeModelName(lngFlowModelID, lngID) + ")������ϼ����ܣ�";
                    break;
                case e_ActorType.fmNodeProfUpLeader:
                    ret = "�ۻ���(" + FlowModel.GetFlowNodeModelName(lngFlowModelID, lngID) + ")�����רҵ�ϼ����ܣ�";
                    break;
                case e_ActorType.fmCondActor:
                    ret = "<font color=green>(" + FlowModel.GetActorCondName(lngID) + ")</font>";
                    break;
                case e_ActorType.fmActorExt:
                    ret = "<font color=red>(" + FlowModel.GetActorExtendName(lngID) + ")</font>";
                    break;
                default:
                    break;
            }
            return ret;
        }

        private string getNodeActorsInfo(ObjNodeModel o)
        {
            string rM = "";
            string rR = "";
            DataTable dt = o.getNodeActorsInfo(lngFlowModelID, lngNodeModelID);
            foreach (DataRow r in dt.Rows)
            {
                e_ActorClass type = (e_ActorClass)int.Parse(r["actorclass"].ToString());
                string sActorName = getActorName((e_ActorType)int.Parse(r["actortype"].ToString()),long.Parse(r["actorid"].ToString()));

                if (type == e_ActorClass.fmMasterActor)
                {
                    if (rM == "")
                        rM = "����:";
                    rM += sActorName + " ";
                }
                else
                {
                    if (rR == "")
                        rR = "������:";
                    rR += sActorName + " ";
                    
                }
            }

            return rM + (rR == "" ? "" : "</br>") + rR;
        }

        private string getMessageInfo(ObjNodeModel o)
        {
            string rM = string.Empty;
            string rR = string.Empty;
            DataTable dt = o.getProcessInfo(lngFlowID, lngNodeModelID, lngFlowModelID);
            foreach (DataRow r in dt.Rows)
            {
                e_ActorClass type = (e_ActorClass)int.Parse(r["actortype"].ToString());
                long lngUserID = long.Parse(r["receiverid"].ToString());
                if( type != e_ActorClass.fmReaderActor)
                {
                    if ((e_MessageStatus)int.Parse(r["status"].ToString()) == e_MessageStatus.emsFinished)
                    {
                        rM += EPSystem.GetUserName(lngUserID) + "(" + "<font color=green>" + getActorClassName(type) + "</font>) ";
                    }
                    else
                    {
                        rM += EPSystem.GetUserName(lngUserID) + "(" + "<font color=red>" + getActorClassName(type) + "</font>) ";
                    }
                }
                else
                {
                    if ((e_MessageStatus)int.Parse(r["status"].ToString()) == e_MessageStatus.emsFinished)
                    {
                        rR += EPSystem.GetUserName(lngUserID) + "(" + "<font color=green>" + getActorClassName(type) + "</font>) ";
                    }
                    else
                    {
                        rR += EPSystem.GetUserName(lngUserID) + "(" + "<font color=red>" + getActorClassName(type) + "</font>) ";
                    }
                }

            }
            return rM + (rR == "" ? "" : "</br>") + rR;

        }

        private string getActorClassName(e_ActorClass at)
        {
            string sActorType = string.Empty;
            switch (at)
            {
                case e_ActorClass.fmInfluxActor:
                    sActorType = "��ǩ";
                    break;
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
            }
            return sActorType;
        }
    

        private string getTimeUnit(int i)
        {
            string ret = "��ʱ";
            switch (i)
            {
                case 0:
                    ret = "��ʱ";
                    break;
                case 1:
                    ret = "����";
                    break;
                case 2:
                    ret = "������";
                    break;
                case 3:
                    ret = "����";
                    break;
                case 4:
                    ret = "Сʱ";
                    break;
            }
            return ret;
        }

        private string GetRightsList(ObjNodeModel o)
        {
            string ret = "";
            ret +=  ((o.CanBack == e_IsTrue.fmTrue) ? "�˻�|" : "");
            
            ret += ((o.CanBackHasDone == e_IsTrue.fmTrue) ? "����|" : "");
            ret +=  ((o.CanCommunic == e_IsTrue.fmTrue) ? "��ͨ|" : "");
            ret += ((o.CanJump == e_IsTrue.fmTrue) ? "��ת|" : "");
            ret += ((o.CanTakeBack == e_IsTrue.fmTrue) ? "����|" : "");
            ret +=  ((o.CanTransmit == e_IsTrue.fmTrue) ? "����|" : "");
            ret += ((o.TakeOver == e_IsTrue.fmTrue) ? "����|" : "");
            ret += ((o.CanAssist == e_IsTrue.fmTrue) ? "Э��|" : "");
            ret +=  ((o.CanAttemper == e_IsTrue.fmTrue) ? "����|" : "");
            ret +=  ((o.ViewAttach == e_IsTrue.fmTrue) ? "����|" : "");
            ret +=  ((o.CanAutoPass == e_IsTrue.fmTrue) ? "�Զ�ͨ��|" : "");
            ret +=  ((o.CanCustLimit == e_IsTrue.fmTrue) ? "����ʱ��|" : "");
            if (ret.EndsWith("|"))
                ret = ret.Substring(0,ret.Length - 1);


            return ret;
        }

        protected override void Render(HtmlTextWriter writer)
        {
            //���HTML
            if (lngNodeType != e_FMNodeType.fmStart && lngNodeType != e_FMNodeType.fmEnd && lngNodeType != e_FMNodeType.fmUnite && lngNodeType != e_FMNodeType.fmDistribute)
            {

                System.IO.StringWriter html = new System.IO.StringWriter();
                System.Web.UI.HtmlTextWriter tw = new System.Web.UI.HtmlTextWriter(html);
                tabRender.RenderControl(tw);

                Response.Clear();
                string sTmp = html.ToString();
                Response.Write(sTmp);
                Response.Flush();
                Response.End();
            }
            else
            {

                Response.Clear();
                Response.Write("");
                Response.Flush();
                Response.End();
            }
        }

    }
}
