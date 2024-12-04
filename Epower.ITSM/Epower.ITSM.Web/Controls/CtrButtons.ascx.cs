/*******************************************************************
 * ��Ȩ���У�
 * Description��������������ư�ť
 * 
 * 
 * Create By  ��
 * Create Date��
 * *****************************************************************/
using System;
using System.Data;
using System.Drawing;
using System.Configuration;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Text;
using Epower.ITSM.Base;
using Epower.ITSM.SqlDAL;
using EpowerCom;
using EpowerGlobal;
using Epower.DevBase.BaseTools;
using System.Collections;

namespace Epower.ITSM.Web.Controls
{
    /// <summary>
    ///		CtrButtons ��ժҪ˵����
    /// </summary>
    public partial class CtrButtons : System.Web.UI.UserControl
    {
        public string sApplicationUrl = Constant.ApplicationPath;     //����·��

        private eOA_FlowProcessType iFPType = eOA_FlowProcessType.efptReadFinished;

        bool blnSaveV = true;
        bool blnBackV = true;
        bool blnTakeBackV = true;
        bool blnExitV = true;
        bool blnPrintV = true;
        bool blnViewFlowV = true;
        bool blnAddOpinionV = false;
        bool blnViewPause = false;
        bool blnFlowPaused = false;      //�����Ƿ�Ϊ��ͣ״̬
        bool blnTransV = true;           //�Ƿ���ʾת��
        bool blnAssistV = true;           //�Ƿ���ʾЭ��
        bool blnReDoBackV = false;        //�Ƿ���ʾ����

        bool blnIsReceiving = false;   //�Ƿ��ڽ���״̬,����״̬��Щ���ܲ��ܲ���,��ת��.

        bool blnExtButtonV1 = false;
        bool blnExtButtonV2 = false;
        bool blnExtButtonV3 = false;
        bool blnExtButtonV4 = false;

        string sExtButton1 = "��չ1";
        string sExtButton2 = "��չ2";
        string sExtButton3 = "��չ3";
        string sExtButton4 = "��չ4";

        string sExtFunction1 = "";
        string sExtFunction2 = "";
        string sExtFunction3 = "";
        string sExtFunction4 = "";


        string strPrintFunction = "printdiv();";  //ȱʡ��ӡ����

        protected string strExitUrl = "";

        long lngMessageID = 0;

        long lngAppID = 0;
        long lngFlowID = 0;
        long lngFlowModelID = 0;
        long lngNodeModelID = 0;

        #region ����

        /// <summary>
        /// �Ƿ���ʾ����
        /// </summary>
        public bool SaveVisible
        {
            set { blnSaveV = value; }
        }

        /// <summary>
        /// �Ƿ���ʾ�������
        /// </summary>
        public bool AddOpinionVisible
        {
            set { blnAddOpinionV = value; }
        }

        /// <summary>
        /// �Ƿ���ʾת��
        /// </summary>
        public bool TransVisible
        {
            set { blnTransV = value; }
        }

        /// <summary>
        /// �Ƿ���ʾЭ��
        /// </summary>
        public bool AssistVisible
        {
            set { blnAssistV = value; }
        }

        /// <summary>
        /// �Ƿ�����Ϊ��ͣ״̬
        /// </summary>
        public bool IsFlowPaused
        {
            set { blnFlowPaused = value; }
        }

        /// <summary>
        /// �Ƿ���ʾ��ͣ�ָ�
        /// </summary>
        public bool PauseVisible
        {
            set { blnViewPause = value; }
        }

        /// <summary>
        /// �Ƿ���ʾ��ӡ
        /// </summary>
        public bool PrintVisible
        {
            set { blnPrintV = value; }
        }

        /// <summary>
        /// �Ƿ���ʾ�鿴����
        /// </summary>
        public bool ViewFlowVisible
        {
            set { blnViewFlowV = value; }
        }

        /// <summary>
        /// ��ӡ����
        /// </summary>
        public string PrintFunction
        {
            set { strPrintFunction = value; }
        }

        /// <summary>
        /// �Ƿ���ʾ����
        /// </summary>
        public bool TakeBackVisible
        {
            set { blnTakeBackV = value; }
        }

        /// <summary>
        /// �Ƿ���ʾ�˻�
        /// </summary>
        public bool BackVisible
        {
            set { blnBackV = value; }
        }

        /// <summary>
        /// �Ƿ���ʾ����,ĸ��ҳ��������ֵ��һ���ж��������
        /// </summary>
        public bool ReDoBackVisible
        {
            set { blnReDoBackV = value; }
        }

        /// <summary>
        /// �Ƿ���ʾ�뿪
        /// </summary>
        public bool ExitVisible
        {
            set { blnExitV = value; }
        }

        /// <summary>
        /// �Ƿ��ڽ���״̬
        /// </summary>
        public bool IsReceiving
        {
            set { blnIsReceiving = value; }
        }
        /// <summary>
        /// �˳����ģգң�
        /// </summary>
        public string ExitToUrl
        {
            set { strExitUrl = value; }
        }

        /// <summary>
        /// ��Ϣ���
        /// </summary>
        public long MessageID
        {
            //2008-03-15�޸�,�ط���Ҫ���ʵ�������,������ViewState����
            set
            {
                lngMessageID = value;
                ViewState[this.ID + "MessageID"] = value;
            }
            get
            {
                if (ViewState[this.ID + "MessageID"] == null)
                    return lngMessageID;
                else
                    return StringTool.String2Long(ViewState[this.ID + "MessageID"].ToString());
            }
        }

        /// <summary>
        /// Ӧ�ñ��
        /// </summary>
        public long AppID
        {
            set { lngAppID = value; }
        }

        /// <summary>
        /// ���̱��
        /// </summary>
        public long FlowID
        {
            set { lngFlowID = value; }
        }

        /// <summary>
        /// ����ģ�ͱ��
        /// </summary>
        public long FlowModelID
        {
            //2008-03-17�޸�,�ط���Ҫ���ʵ�������,������ViewState����
            set
            {
                lngFlowModelID = value;
                ViewState[this.ID + "FlowModelID"] = value;
            }
            get
            {
                if (ViewState[this.ID + "FlowModelID"] == null)
                    return lngFlowModelID;
                else
                    return StringTool.String2Long(ViewState[this.ID + "FlowModelID"].ToString());
            }
        }

        /// <summary>
        /// ����ģ�ͱ�ţ�2009-06-10���ӣ��Ż�����Ȩ�޴���
        /// </summary>
        public long NodeModelID
        {
            //�ط���Ҫ���ʵ�������,������ViewState����
            set
            {
                lngNodeModelID = value;
                ViewState[this.ID + "NodeModelID"] = value;
            }
            get
            {
                if (ViewState[this.ID + "NodeModelID"] == null)
                    return lngNodeModelID;
                else
                    return StringTool.String2Long(ViewState[this.ID + "NodeModelID"].ToString());
            }
        }

        /// <summary>
        /// ���̴������
        /// </summary>
        public eOA_FlowProcessType FlowProcessType
        {
            set { iFPType = value; }
        }

        /// <summary>
        /// �Ƿ���ʾ��չ��
        /// </summary>
        public bool Button1Visible
        {
            set { blnExtButtonV1 = value; }
        }

        /// <summary>
        /// ��չ��Ŧ������
        /// </summary>
        public string ButtonName1
        {
            set { sExtButton1 = value; }
        }

        /// <summary>
        /// ��չ��Ŧ�ű���
        /// </summary>
        public string Button1Function
        {
            set { sExtFunction1 = value; }
        }


        /// <summary>
        /// �Ƿ���ʾ��չ��
        /// </summary>
        public bool Button2Visible
        {
            set { blnExtButtonV2 = value; }
        }

        /// <summary>
        /// ��չ��Ŧ2����
        /// </summary>
        public string ButtonName2
        {
            set { sExtButton2 = value; }
        }

        /// <summary>
        /// ��չ��Ŧ�ű�2
        /// </summary>
        public string Button2Function
        {
            set { sExtFunction2 = value; }
        }

        /// <summary>
        /// �Ƿ���ʾ��չ3
        /// </summary>
        public bool Button3Visible
        {
            set { blnExtButtonV3 = value; }
        }

        /// <summary>
        /// ��չ��Ŧ3����
        /// </summary>
        public string ButtonName3
        {
            set { sExtButton3 = value; }
        }

        /// <summary>
        /// ��չ��Ŧ�ű�3
        /// </summary>
        public string Button3Function
        {
            set { sExtFunction3 = value; }
        }

        /// <summary>
        /// �Ƿ���ʾ��չ4
        /// </summary>
        public bool Button4Visible
        {
            set { blnExtButtonV4 = value; }
        }

        /// <summary>
        /// ��չ��Ŧ4����
        /// </summary>
        public string ButtonName4
        {
            set { sExtButton4 = value; }
        }

        /// <summary>
        /// ��չ��Ŧ�ű�4
        /// </summary>
        public string Button4Function
        {
            set { sExtFunction4 = value; }
        }

        /// <summary>
        /// �Ƿ���Ҫ�رմ���
        /// </summary>
        protected string FromForms
        {
            get
            {
                if (Session["FromUrl"] != null && Session["FromUrl"].ToString().ToLower() == "close")
                    return "close";
                else
                    return string.Empty;
            }
        }

        #region �Ƿ�����ģʽ ����ǰ 2013-04-19
        private bool mIsSelfMode = false;
        public bool IsSelfMode
        {
            set
            {
                mIsSelfMode = value;
                ViewState[this.ID + "IsSelfMode"] = value;
            }
            get
            {
                if (ViewState[this.ID + "IsSelfMode"] == null)
                    return mIsSelfMode;
                else
                    return bool.Parse(ViewState[this.ID + "IsSelfMode"].ToString());
            }
        }
        #endregion

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, System.EventArgs e)
        {
            strExitUrl = GetFromUrl();
            if (!IsPostBack)
            {
                #region �޸�չʾ��ť ����ǰ 2013-04-19
                if (IsSelfMode)
                    LoadButtonsHtmlSelfMode();
                else
                    LoadButtonsHtml();
                #endregion
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private string GetFromUrl()
        {
            if (Session["FromUrl"] != null)
                return Session["FromUrl"].ToString();
            else
                return "#";
        }



        private void LoadButtonsHtml()
        {

            StringBuilder sb = new StringBuilder("");

            bool blnHas = false;

            //��ͷ���
            sb.Append("<TABLE id=\"ctrtabbuttons\" cellSpacing=\"1\" cellPadding=\"1\" width=\"100%\" border=\"0\"><TR>");

            //ɾ����ť
            EpowerCom.Message msg = new EpowerCom.Message();
            if (msg.CanDeleteFlow(lngFlowID, (long)Session["UserID"]) == true 
                || FlowModel.hasDeleteFlowRight(lngFlowID, (long)Session["UserID"]) == true)
            {
                sb.Append("<TD align=right " + (blnHas == true ? "" : "width=\"100%\" ") + "><INPUT class=\"btnClass\" id=\"ctrcmdDeleteFlow" +
                    "\" style=\"WIDTH: 68px; HEIGHT: 24px\" onclick=\"DoMainFlowDelete();\" type=\"button\" value=\"ɾ��" +
                    "\" name=\"ctrcmdDeleteFlow\" Height=\"24\"></TD>");

                blnHas = true;
            }

            if (lngFlowID != 0)
            {
                if (FlowModel.hasAbortFlowRight((long)Session["UserID"], lngFlowModelID, lngFlowID) == true)
                {
                    sb.Append("<TD align=right " + (blnHas == true ? "" : "width=\"100%\" ") + "><INPUT class=\"btnClass\" id=\"ctrcmdAbortFlow" +
                        "\" style=\"WIDTH: 68px; HEIGHT: 24px\" onclick=\"OpenAbortFlow(" + lngMessageID.ToString() + ");\" type=\"button\" value=\"������ֹ" +
                        "\" name=\"ctrcmdDeleteFlow\" Height=\"24\"></TD>");

                    blnHas = true;
                }
            }


            if (blnViewPause == true && lngMessageID != 0 && blnIsReceiving == false)
            {
                if (blnFlowPaused == true)
                {
                    sb.Append("<TD align=right " + (blnHas == true ? "" : "width=\"100%\" ") + "><INPUT class=\"btnClass\" id=\"ctrPause" +
                        "\" style=\"WIDTH: 68px; HEIGHT: 24px\" onclick=\"DoMainFlowPause(1);\" type=\"button\" value=\"ȡ����ͣ" +
                        "\" name=\"ctrTransmit\" Height=\"24\"></TD>");
                }
                else
                {
                    sb.Append("<TD align=right " + (blnHas == true ? "" : "width=\"100%\" ") + "><INPUT class=\"btnClass\" id=\"ctrPause" +
                        "\" style=\"WIDTH: 68px; HEIGHT: 24px\" onclick=\"DoMainFlowPause(0);\" type=\"button\" value=\"������ͣ" +
                        "\" name=\"ctrTransmit\" Height=\"24\"></TD>");
                }
                blnHas = true;
            }

            switch (iFPType)
            {

                case eOA_FlowProcessType.efptReadFinished:
                case eOA_FlowProcessType.efptNormalFinished:
                    if (blnTakeBackV == true)
                    {
                        sb.Append("<TD align=right " + (blnHas == true ? "" : "width=\"100%\" ") + "><INPUT class=\"btnClass\" id=\"ctrcmdTakeBack" +
                            "\" style=\"WIDTH: 68px; HEIGHT: 24px\" onclick=\"DoLeftTakeBack();\" type=\"button\" value=\"����" +
                            "\" name=\"ctrcmdTakeBack\" Height=\"24\"></TD>");

                        blnHas = true;
                    }
                    break;

                case eOA_FlowProcessType.efptNew:
                    if (blnSaveV == true)
                    {
                        sb.Append("<TD align=right " + (blnHas == true ? "" : "width=\"100%\" ") + "><INPUT class=\"btnClass\" id=\"ctrcmdSave" +
                            "\" style=\"WIDTH: 68px; HEIGHT: 24px\" onclick=\"DoLeftSave();\" type=\"button\" value=\"�ݴ�" +
                            "\" name=\"ctrcmdSave\" Height=\"24\"></TD>");
                        blnHas = true;
                    }
                    break;
                case eOA_FlowProcessType.efptNormal:
                    if (blnSaveV == true)
                    {
                        sb.Append("<TD align=right " + (blnHas == true ? "" : "width=\"100%\" ") + "><INPUT class=\"btnClass\" id=\"ctrcmdSave" +
                            "\" style=\"WIDTH: 68px; HEIGHT: 24px\" onclick=\"DoLeftSave();\" type=\"button\" value=\"�ݴ�" +
                            "\" name=\"ctrcmdSave\" Height=\"24\"></TD>");
                        blnHas = true;
                    }
                    if (blnBackV == true)
                    {
                        long lngPreID = FlowDP.GetSenderID(lngMessageID);
                        //string strCaption = (lngPreID == (long)Session["UserID"]) ? "����" : "�˻�";
                        string strCaption = (lngPreID == (long)Session["UserID"]) ? "�˻�" : "�˻�";
                        sb.Append("<TD align=right " + (blnHas == true ? "" : "width=\"100%\" ") + "><INPUT class=\"btnClass\" id=\"ctrcmdBack" +
                            "\" style=\"WIDTH: 68px; HEIGHT: 24px\" onclick=\"DoLeftBack();\" type=\"button\" value=\"" + strCaption +
                            "\" name=\"ctrcmdBack\" Height=\"24\"></TD>");
                        blnHas = true;
                    }

                    if (blnReDoBackV == true)
                    {

                        long lngNextNMID = 0;
                        long lngNextNMType = 0;
                        long lngNextUserID = 0;

                        if (MessageDep.CanReDoBackFlow(lngMessageID, ref lngNextNMID, ref lngNextNMType, ref lngNextUserID) == true)
                        {
                            sb.Append("<TD align=right " + (blnHas == true ? "" : "width=\"100%\" ") + "><INPUT class=\"btnClass\" id=\"ctrcmdRedoBack" +
                                "\" style=\"WIDTH: 68px; HEIGHT: 24px\" onclick=\"DoLeftReDoBack(" + lngNextNMID.ToString() + "," + lngNextNMType.ToString() + "," + lngNextUserID.ToString() + ");\" type=\"button\" value=\"" + "����" +
                                "\" name=\"ctrcmdRedoBack\" Height=\"24\"></TD>");
                            blnHas = true;
                        }
                    }

                    break;
                case eOA_FlowProcessType.efptReader:
                case eOA_FlowProcessType.eftpLookOtherMsg:
                case eOA_FlowProcessType.eftpWaitingMsg:
                case eOA_FlowProcessType.eftpStopMsg:
                    break;
                default:
                    break;

            }

            if (blnAddOpinionV == true)
            {
                sb.Append("<TD align=right " + (blnHas == true ? "" : "width=\"100%\" ") + "><INPUT class=\"btnClass\" id=\"ctrAddOpinion" +
                    "\" style=\"WIDTH: 68px; HEIGHT: 24px\" onclick=\"DoAddOpinion();\" type=\"button\" value=\"�������" +
                    "\" name=\"ctrAddOpinion\" Height=\"24\"></TD>");
                blnHas = true;
            }


            if (blnViewFlowV == true)
            {
                sb.Append("<TD align=right " + (blnHas == true ? "" : "width=\"100%\" ") + "><INPUT class=\"btnClass\" id=\"ctrViewFlow" +
                    "\" style=\"WIDTH: 68px; HEIGHT: 24px\" onclick=\"DoViewFlow();\" type=\"button\" value=\"�鿴����" +
                    "\" name=\"ctrViewFlow\" Height=\"24\"></TD>");
                blnHas = true;
            }
            if (blnExtButtonV1 == true)
            {
                sb.Append("<TD align=right " + (blnHas == true ? "" : "width=\"100%\" ") + "><INPUT class=\"btnClass\" id=\"ctrcmdExtButton1" +
                    "\" style=\"WIDTH: 68px; HEIGHT: 24px\" onclick=\"" + sExtFunction1 + "\" type=\"button\" value=\"" + sExtButton1 + "" +
                    "\" name=\"ctrcmdExtButton1\" Height=\"24\"></TD>");
                blnHas = true;
            }

            if (blnExtButtonV2 == true)
            {
                sb.Append("<TD align=right " + (blnHas == true ? "" : "width=\"100%\" ") + "><INPUT class=\"btnClass\" id=\"ctrcmdExtButton2" +
                    "\" style=\"WIDTH: 68px; HEIGHT: 24px\" onclick=\"" + sExtFunction2 + "\" type=\"button\" value=\"" + sExtButton2 + "" +
                    "\" name=\"ctrcmdExtButton2\" Height=\"24\"></TD>");
                blnHas = true;
            }
            if (blnExtButtonV3 == true)
            {
                sb.Append("<TD align=right " + (blnHas == true ? "" : "width=\"100%\" ") + "><INPUT class=\"btnClass\" id=\"ctrcmdExtButton1" +
                    "\" style=\"WIDTH: 68px; HEIGHT: 24px\" onclick=\"" + sExtFunction3 + "\" type=\"button\" value=\"" + sExtButton3 + "" +
                    "\" name=\"ctrcmdExtButton1\" Height=\"24\"></TD>");
                blnHas = true;
            }

            if (blnExtButtonV4 == true)
            {
                sb.Append("<TD align=right " + (blnHas == true ? "" : "width=\"100%\" ") + "><INPUT class=\"btnClass\" id=\"ctrcmdExtButton2" +
                    "\" style=\"WIDTH: 68px; HEIGHT: 24px\" onclick=\"" + sExtFunction4 + "\" type=\"button\" value=\"" + sExtButton4 + "" +
                    "\" name=\"ctrcmdExtButton2\" Height=\"24\"></TD>");
                blnHas = true;
            }

            if (blnPrintV == true)
            {
                sb.Append("<TD align=right " + (blnHas == true ? "" : "width=\"100%\" ") + "><INPUT class=\"btnClass\" id=\"ctrcmdPrint" +
                    "\" style=\"WIDTH: 68px; HEIGHT: 24px\" onclick=\"" + strPrintFunction + "\" type=\"button\" value=\"��ӡ" +
                    "\" name=\"ctrcmdPrint\" Height=\"24\"></TD>");
                blnHas = true;
            }

            if (blnExitV == true)
            {
                sb.Append("<TD align=right " + (blnHas == true ? "" : "width=\"100%\" ") + "><INPUT class=\"btnClass\" id=\"ctrcmdExit" +
                    "\" style=\"WIDTH: 68px; HEIGHT: 24px\" onclick=\"DoExit();\" type=\"button\" value=\"�˳�" +
                    "\" name=\"ctrcmdExit\" Height=\"24\"></TD>");
                blnHas = true;
            }




            //�������
            sb.Append("</TR></TABLE>");

            ltlButtons.Text = sb.ToString();
        }

        #region ��������ģʽ��չʾ��ť ����ǰ 2013-04-19
        /// <summary>
        /// ��������ģʽ��չʾ��ť
        /// </summary>
        private void LoadButtonsHtmlSelfMode()
        {
            StringBuilder sb = new StringBuilder("");

            bool blnHas = false;

            //��ͷ���
            sb.Append("<TABLE id=\"ctrtabbuttons\" cellSpacing=\"1\" cellPadding=\"1\" width=\"100%\" border=\"0\"><TR>");

            sb.Append("<TD align=right " + (blnHas == true ? "" : "width=\"100%\" ") + "><INPUT class=\"btnClass\" id=\"ctrViewFlow" +
                    "\" style=\"WIDTH: 68px; HEIGHT: 24px\" onclick=\"DoViewFlow();\" type=\"button\" value=\"�鿴����" +
                    "\" name=\"ctrViewFlow\" Height=\"24\"></TD>");

            blnHas = true;

            sb.Append("<TD align=right " + (blnHas == true ? "" : "width=\"100%\" ") + "><INPUT class=\"btnClass\" id=\"ctrcmdExit" +
                    "\" style=\"WIDTH: 68px; HEIGHT: 24px\" onclick=\"DoExit();\" type=\"button\" value=\"�˳�" +
                    "\" name=\"ctrcmdExit\" Height=\"24\"></TD>");

            //�������
            sb.Append("</TR></TABLE>");

            ltlButtons.Text = sb.ToString();
        }
        #endregion

        #region Web ������������ɵĴ���
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: �õ����� ASP.NET Web ���������������ġ�
            //
            InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        ///		�����֧������ķ��� - ��Ҫʹ�ô���༭��
        ///		�޸Ĵ˷��������ݡ�
        /// </summary>
        private void InitializeComponent()
        {

        }
        #endregion
    }
}
