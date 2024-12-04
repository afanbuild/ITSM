/*******************************************************************
 * 版权所有：
 * Description：工作流上面控制按钮
 * 
 * 
 * Create By  ：
 * Create Date：
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
    ///		CtrButtons 的摘要说明。
    /// </summary>
    public partial class CtrButtons : System.Web.UI.UserControl
    {
        public string sApplicationUrl = Constant.ApplicationPath;     //虚拟路径

        private eOA_FlowProcessType iFPType = eOA_FlowProcessType.efptReadFinished;

        bool blnSaveV = true;
        bool blnBackV = true;
        bool blnTakeBackV = true;
        bool blnExitV = true;
        bool blnPrintV = true;
        bool blnViewFlowV = true;
        bool blnAddOpinionV = false;
        bool blnViewPause = false;
        bool blnFlowPaused = false;      //流程是否为暂停状态
        bool blnTransV = true;           //是否显示转发
        bool blnAssistV = true;           //是否显示协作
        bool blnReDoBackV = false;        //是否显示重审

        bool blnIsReceiving = false;   //是否处于接收状态,接收状态有些功能不能操作,如转发.

        bool blnExtButtonV1 = false;
        bool blnExtButtonV2 = false;
        bool blnExtButtonV3 = false;
        bool blnExtButtonV4 = false;

        string sExtButton1 = "扩展1";
        string sExtButton2 = "扩展2";
        string sExtButton3 = "扩展3";
        string sExtButton4 = "扩展4";

        string sExtFunction1 = "";
        string sExtFunction2 = "";
        string sExtFunction3 = "";
        string sExtFunction4 = "";


        string strPrintFunction = "printdiv();";  //缺省打印方法

        protected string strExitUrl = "";

        long lngMessageID = 0;

        long lngAppID = 0;
        long lngFlowID = 0;
        long lngFlowModelID = 0;
        long lngNodeModelID = 0;

        #region 属性

        /// <summary>
        /// 是否显示保存
        /// </summary>
        public bool SaveVisible
        {
            set { blnSaveV = value; }
        }

        /// <summary>
        /// 是否显示补充意见
        /// </summary>
        public bool AddOpinionVisible
        {
            set { blnAddOpinionV = value; }
        }

        /// <summary>
        /// 是否显示转发
        /// </summary>
        public bool TransVisible
        {
            set { blnTransV = value; }
        }

        /// <summary>
        /// 是否显示协作
        /// </summary>
        public bool AssistVisible
        {
            set { blnAssistV = value; }
        }

        /// <summary>
        /// 是否流程为暂停状态
        /// </summary>
        public bool IsFlowPaused
        {
            set { blnFlowPaused = value; }
        }

        /// <summary>
        /// 是否显示暂停恢复
        /// </summary>
        public bool PauseVisible
        {
            set { blnViewPause = value; }
        }

        /// <summary>
        /// 是否显示打印
        /// </summary>
        public bool PrintVisible
        {
            set { blnPrintV = value; }
        }

        /// <summary>
        /// 是否显示查看流程
        /// </summary>
        public bool ViewFlowVisible
        {
            set { blnViewFlowV = value; }
        }

        /// <summary>
        /// 打印函数
        /// </summary>
        public string PrintFunction
        {
            set { strPrintFunction = value; }
        }

        /// <summary>
        /// 是否显示回收
        /// </summary>
        public bool TakeBackVisible
        {
            set { blnTakeBackV = value; }
        }

        /// <summary>
        /// 是否显示退回
        /// </summary>
        public bool BackVisible
        {
            set { blnBackV = value; }
        }

        /// <summary>
        /// 是否显示重审,母版页根据已有值做一次判断提高性能
        /// </summary>
        public bool ReDoBackVisible
        {
            set { blnReDoBackV = value; }
        }

        /// <summary>
        /// 是否显示离开
        /// </summary>
        public bool ExitVisible
        {
            set { blnExitV = value; }
        }

        /// <summary>
        /// 是否处于接收状态
        /// </summary>
        public bool IsReceiving
        {
            set { blnIsReceiving = value; }
        }
        /// <summary>
        /// 退出到的ＵＲＬ
        /// </summary>
        public string ExitToUrl
        {
            set { strExitUrl = value; }
        }

        /// <summary>
        /// 消息编号
        /// </summary>
        public long MessageID
        {
            //2008-03-15修改,回发后还要访问到的属性,必须用ViewState保存
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
        /// 应用编号
        /// </summary>
        public long AppID
        {
            set { lngAppID = value; }
        }

        /// <summary>
        /// 流程编号
        /// </summary>
        public long FlowID
        {
            set { lngFlowID = value; }
        }

        /// <summary>
        /// 流程模型编号
        /// </summary>
        public long FlowModelID
        {
            //2008-03-17修改,回发后还要访问到的属性,必须用ViewState保存
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
        /// 环节模型编号（2009-06-10增加，优化特殊权限处理）
        /// </summary>
        public long NodeModelID
        {
            //回发后还要访问到的属性,必须用ViewState保存
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
        /// 流程处理类别
        /// </summary>
        public eOA_FlowProcessType FlowProcessType
        {
            set { iFPType = value; }
        }

        /// <summary>
        /// 是否显示扩展１
        /// </summary>
        public bool Button1Visible
        {
            set { blnExtButtonV1 = value; }
        }

        /// <summary>
        /// 扩展按纽１名称
        /// </summary>
        public string ButtonName1
        {
            set { sExtButton1 = value; }
        }

        /// <summary>
        /// 扩展按纽脚本１
        /// </summary>
        public string Button1Function
        {
            set { sExtFunction1 = value; }
        }


        /// <summary>
        /// 是否显示扩展２
        /// </summary>
        public bool Button2Visible
        {
            set { blnExtButtonV2 = value; }
        }

        /// <summary>
        /// 扩展按纽2名称
        /// </summary>
        public string ButtonName2
        {
            set { sExtButton2 = value; }
        }

        /// <summary>
        /// 扩展按纽脚本2
        /// </summary>
        public string Button2Function
        {
            set { sExtFunction2 = value; }
        }

        /// <summary>
        /// 是否显示扩展3
        /// </summary>
        public bool Button3Visible
        {
            set { blnExtButtonV3 = value; }
        }

        /// <summary>
        /// 扩展按纽3名称
        /// </summary>
        public string ButtonName3
        {
            set { sExtButton3 = value; }
        }

        /// <summary>
        /// 扩展按纽脚本3
        /// </summary>
        public string Button3Function
        {
            set { sExtFunction3 = value; }
        }

        /// <summary>
        /// 是否显示扩展4
        /// </summary>
        public bool Button4Visible
        {
            set { blnExtButtonV4 = value; }
        }

        /// <summary>
        /// 扩展按纽4名称
        /// </summary>
        public string ButtonName4
        {
            set { sExtButton4 = value; }
        }

        /// <summary>
        /// 扩展按纽脚本4
        /// </summary>
        public string Button4Function
        {
            set { sExtFunction4 = value; }
        }

        /// <summary>
        /// 是否需要关闭窗体
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

        #region 是否自助模式 余向前 2013-04-19
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
                #region 修改展示按钮 余向前 2013-04-19
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

            //开头标记
            sb.Append("<TABLE id=\"ctrtabbuttons\" cellSpacing=\"1\" cellPadding=\"1\" width=\"100%\" border=\"0\"><TR>");

            //删除按钮
            EpowerCom.Message msg = new EpowerCom.Message();
            if (msg.CanDeleteFlow(lngFlowID, (long)Session["UserID"]) == true 
                || FlowModel.hasDeleteFlowRight(lngFlowID, (long)Session["UserID"]) == true)
            {
                sb.Append("<TD align=right " + (blnHas == true ? "" : "width=\"100%\" ") + "><INPUT class=\"btnClass\" id=\"ctrcmdDeleteFlow" +
                    "\" style=\"WIDTH: 68px; HEIGHT: 24px\" onclick=\"DoMainFlowDelete();\" type=\"button\" value=\"删除" +
                    "\" name=\"ctrcmdDeleteFlow\" Height=\"24\"></TD>");

                blnHas = true;
            }

            if (lngFlowID != 0)
            {
                if (FlowModel.hasAbortFlowRight((long)Session["UserID"], lngFlowModelID, lngFlowID) == true)
                {
                    sb.Append("<TD align=right " + (blnHas == true ? "" : "width=\"100%\" ") + "><INPUT class=\"btnClass\" id=\"ctrcmdAbortFlow" +
                        "\" style=\"WIDTH: 68px; HEIGHT: 24px\" onclick=\"OpenAbortFlow(" + lngMessageID.ToString() + ");\" type=\"button\" value=\"流程终止" +
                        "\" name=\"ctrcmdDeleteFlow\" Height=\"24\"></TD>");

                    blnHas = true;
                }
            }


            if (blnViewPause == true && lngMessageID != 0 && blnIsReceiving == false)
            {
                if (blnFlowPaused == true)
                {
                    sb.Append("<TD align=right " + (blnHas == true ? "" : "width=\"100%\" ") + "><INPUT class=\"btnClass\" id=\"ctrPause" +
                        "\" style=\"WIDTH: 68px; HEIGHT: 24px\" onclick=\"DoMainFlowPause(1);\" type=\"button\" value=\"取消暂停" +
                        "\" name=\"ctrTransmit\" Height=\"24\"></TD>");
                }
                else
                {
                    sb.Append("<TD align=right " + (blnHas == true ? "" : "width=\"100%\" ") + "><INPUT class=\"btnClass\" id=\"ctrPause" +
                        "\" style=\"WIDTH: 68px; HEIGHT: 24px\" onclick=\"DoMainFlowPause(0);\" type=\"button\" value=\"流程暂停" +
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
                            "\" style=\"WIDTH: 68px; HEIGHT: 24px\" onclick=\"DoLeftTakeBack();\" type=\"button\" value=\"回收" +
                            "\" name=\"ctrcmdTakeBack\" Height=\"24\"></TD>");

                        blnHas = true;
                    }
                    break;

                case eOA_FlowProcessType.efptNew:
                    if (blnSaveV == true)
                    {
                        sb.Append("<TD align=right " + (blnHas == true ? "" : "width=\"100%\" ") + "><INPUT class=\"btnClass\" id=\"ctrcmdSave" +
                            "\" style=\"WIDTH: 68px; HEIGHT: 24px\" onclick=\"DoLeftSave();\" type=\"button\" value=\"暂存" +
                            "\" name=\"ctrcmdSave\" Height=\"24\"></TD>");
                        blnHas = true;
                    }
                    break;
                case eOA_FlowProcessType.efptNormal:
                    if (blnSaveV == true)
                    {
                        sb.Append("<TD align=right " + (blnHas == true ? "" : "width=\"100%\" ") + "><INPUT class=\"btnClass\" id=\"ctrcmdSave" +
                            "\" style=\"WIDTH: 68px; HEIGHT: 24px\" onclick=\"DoLeftSave();\" type=\"button\" value=\"暂存" +
                            "\" name=\"ctrcmdSave\" Height=\"24\"></TD>");
                        blnHas = true;
                    }
                    if (blnBackV == true)
                    {
                        long lngPreID = FlowDP.GetSenderID(lngMessageID);
                        //string strCaption = (lngPreID == (long)Session["UserID"]) ? "返回" : "退回";
                        string strCaption = (lngPreID == (long)Session["UserID"]) ? "退回" : "退回";
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
                                "\" style=\"WIDTH: 68px; HEIGHT: 24px\" onclick=\"DoLeftReDoBack(" + lngNextNMID.ToString() + "," + lngNextNMType.ToString() + "," + lngNextUserID.ToString() + ");\" type=\"button\" value=\"" + "重审" +
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
                    "\" style=\"WIDTH: 68px; HEIGHT: 24px\" onclick=\"DoAddOpinion();\" type=\"button\" value=\"补充意见" +
                    "\" name=\"ctrAddOpinion\" Height=\"24\"></TD>");
                blnHas = true;
            }


            if (blnViewFlowV == true)
            {
                sb.Append("<TD align=right " + (blnHas == true ? "" : "width=\"100%\" ") + "><INPUT class=\"btnClass\" id=\"ctrViewFlow" +
                    "\" style=\"WIDTH: 68px; HEIGHT: 24px\" onclick=\"DoViewFlow();\" type=\"button\" value=\"查看流程" +
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
                    "\" style=\"WIDTH: 68px; HEIGHT: 24px\" onclick=\"" + strPrintFunction + "\" type=\"button\" value=\"打印" +
                    "\" name=\"ctrcmdPrint\" Height=\"24\"></TD>");
                blnHas = true;
            }

            if (blnExitV == true)
            {
                sb.Append("<TD align=right " + (blnHas == true ? "" : "width=\"100%\" ") + "><INPUT class=\"btnClass\" id=\"ctrcmdExit" +
                    "\" style=\"WIDTH: 68px; HEIGHT: 24px\" onclick=\"DoExit();\" type=\"button\" value=\"退出" +
                    "\" name=\"ctrcmdExit\" Height=\"24\"></TD>");
                blnHas = true;
            }




            //结束标记
            sb.Append("</TR></TABLE>");

            ltlButtons.Text = sb.ToString();
        }

        #region 加载自助模式下展示按钮 余向前 2013-04-19
        /// <summary>
        /// 加载自助模式下展示按钮
        /// </summary>
        private void LoadButtonsHtmlSelfMode()
        {
            StringBuilder sb = new StringBuilder("");

            bool blnHas = false;

            //开头标记
            sb.Append("<TABLE id=\"ctrtabbuttons\" cellSpacing=\"1\" cellPadding=\"1\" width=\"100%\" border=\"0\"><TR>");

            sb.Append("<TD align=right " + (blnHas == true ? "" : "width=\"100%\" ") + "><INPUT class=\"btnClass\" id=\"ctrViewFlow" +
                    "\" style=\"WIDTH: 68px; HEIGHT: 24px\" onclick=\"DoViewFlow();\" type=\"button\" value=\"查看流程" +
                    "\" name=\"ctrViewFlow\" Height=\"24\"></TD>");

            blnHas = true;

            sb.Append("<TD align=right " + (blnHas == true ? "" : "width=\"100%\" ") + "><INPUT class=\"btnClass\" id=\"ctrcmdExit" +
                    "\" style=\"WIDTH: 68px; HEIGHT: 24px\" onclick=\"DoExit();\" type=\"button\" value=\"退出" +
                    "\" name=\"ctrcmdExit\" Height=\"24\"></TD>");

            //结束标记
            sb.Append("</TR></TABLE>");

            ltlButtons.Text = sb.ToString();
        }
        #endregion

        #region Web 窗体设计器生成的代码
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: 该调用是 ASP.NET Web 窗体设计器所必需的。
            //
            InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        ///		设计器支持所需的方法 - 不要使用代码编辑器
        ///		修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {

        }
        #endregion
    }
}
