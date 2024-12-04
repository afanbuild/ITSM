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
using EpowerCom;
using EpowerGlobal;
using Epower.ITSM.Base;
using Epower.ITSM.SqlDAL;
using Epower.DevBase.BaseTools;
using Epower.DevBase.Organization.SqlDAL;
using Epower.DevBase.Organization.Base;

namespace Epower.ITSM.Web
{
    public partial class FlowFormsUserDefine : System.Web.UI.MasterPage
    {

        #region 属性声明
        //母版页表单指定处理对象XML 范例
        //FormDefineTool fdt = new FormDefineTool();

        //fdt.Add(10007, "系统部", e_ActorClass.fmMasterActor, e_DeptORUser.fmDept);

        //fdt.Add(10005, "局领导", e_ActorClass.fmReaderActor, e_DeptORUser.fmDept);

        //fdt.Add(10002, "郑天中", e_ActorClass.fmMasterActor, e_DeptORUser.fmUser);

        //fdt.Add(10003, "陈应", e_ActorClass.fmReaderActor, e_DeptORUser.fmUser);

        //fdt.Add(10002, "郑天中", e_ActorClass.fmMasterActor, e_DeptORUser.fmUser);

        //strFormDefineXml = fdt.GetXmlObject().InnerXml;

        public string sApplicationUrl = Constant.ApplicationPath;     //虚拟路径
        /// <summary>
        /// 母版页上保存表单指定处理对象的属性


        /// </summary>
        public string FormDefineXmlValue
        {
            set
            {
                ViewState[v_FormDefineXmlValue] = value;
            }
            get
            {
                if (ViewState[v_FormDefineXmlValue] != null)
                {
                    return ViewState[v_FormDefineXmlValue].ToString();
                }
                else
                {
                    return "";
                }
            }

        }
        /// <summary>
        /// 短信通知选择框是否显示

        /// </summary>
        public bool blnSMSNotify
        {
            set
            {
                chkNotify.Visible = value;
            }
            get
            {
                return chkNotify.Visible;
            }
        }
        /// <summary>
        /// 邮件发送选择框是否显示


        /// </summary>
        public bool blnEmail
        {
            set
            {
                chkEmail.Visible = value;
            }
            get
            {
                return chkEmail.Visible;
            }
        }
        /// <summary>
        /// 取得是否短信通知值


        /// </summary>
        public bool NotifyValue
        {
            set
            {
                chkNotify.Checked = value;
            }
            get
            {
                return chkNotify.Checked;
            }
        }
        /// <summary>
        /// 取得是否发送邮件值


        /// </summary>
        public bool EmailValue
        {
            set
            {
                chkEmail.Checked = value;
            }
            get
            {
                return chkEmail.Checked;
            }
        }

        /// <summary>
        /// 流程操作框是否显示

        /// </summary>
        public bool blnShowFlowOP
        {
            set
            {
                ShowFlowOP.Visible = value;
            }
            get
            {
                return ShowFlowOP.Visible;
            }
        }

        /// <summary>
        /// 表单标题
        /// </summary>
        public string FormTitle
        {
            set { CtrTitle1.Title = value + "[" + ViewState[v_NodeName] + "]"; }
        }

        /// <summary>
        /// 其实流程临时传过来的FLOWID
        /// </summary>
        public long TempFlowID
        {
            set
            {
                ViewState["TempFlowID"] = value;
            }
            get
            {
                if (ViewState["TempFlowID"] != null)
                {
                    return (long)ViewState["TempFlowID"];
                }
                else
                {
                    return 0;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public eOA_AttachmentType TempAttachmentType
        {
            set
            {
                ViewState["FlowTempAttachmentType"] = (int)value;
            }
            get
            {
                if (ViewState["FlowTempAttachmentType"] != null)
                {
                    return (eOA_AttachmentType)int.Parse(ViewState["FlowTempAttachmentType"].ToString());
                }
                else
                {
                    return eOA_AttachmentType.eNormal;
                }
            }
        }

        #region 扩展按钮控制
        /// <summary>
        /// 是否显示扩展１
        /// </summary>
        public bool Button1Visible
        {
            set { CtrButtons1.Button1Visible = value; }
        }
        /// <summary>
        /// 扩展按纽１名称
        /// </summary>
        public string ButtonName1
        {
            set { CtrButtons1.ButtonName1 = value; }
        }
        /// <summary>
        /// 扩展按纽脚本１
        /// </summary>
        public string Button1Function
        {
            set { CtrButtons1.Button1Function = value; }
        }
        /// <summary>
        /// 是否显示扩展２
        /// </summary>
        public bool Button2Visible
        {
            set { CtrButtons1.Button2Visible = value; }
        }

        /// <summary>
        /// 扩展按纽2名称
        /// </summary>
        public string ButtonName2
        {
            set { CtrButtons1.ButtonName2 = value; }
        }

        /// <summary>
        /// 扩展按纽脚本2
        /// </summary>
        public string Button2Function
        {
            set { CtrButtons1.Button2Function = value; }
        }

        /// <summary>
        /// 是否显示扩展3
        /// </summary>
        public bool Button3Visible
        {
            set { CtrButtons1.Button3Visible = value; }
        }

        /// <summary>
        /// 扩展按纽3名称
        /// </summary>
        public string ButtonName3
        {
            set { CtrButtons1.ButtonName3 = value; }
        }

        /// <summary>
        /// 扩展按纽脚本3
        /// </summary>
        public string Button3Function
        {
            set { CtrButtons1.Button3Function = value; }
        }

        /// <summary>
        /// 是否显示扩展4
        /// </summary>
        public bool Button4Visible
        {
            set { CtrButtons1.Button4Visible = value; }
        }

        /// <summary>
        /// 扩展按纽4名称
        /// </summary>
        public string ButtonName4
        {
            set { CtrButtons1.ButtonName4 = value; }
        }

        /// <summary>
        /// 扩展按纽脚本4
        /// </summary>
        public string Button4Function
        {
            set { CtrButtons1.Button4Function = value; }
        }
        #endregion
        #endregion

        #region 变量声明
        //试图状态Key常量
        private const string v_FlowID = "FlowID";
        private const string v_NodeName = "FlowNodeName";
        private const string v_NodeModelID = "FlowNodeModelID";
        private const string v_AttachRight = "AttributesRight";
        private const string v_MessageType = "MessageType";
        private const string v_MessageState = "MessageState";
        private const string v_MsgActorClass = "MsgActorClass";
        private const string v_AppID = "AppID";
        private const string v_OpID = "OPID";
        private const string v_StartID = "StartID";
        private const string v_StartName = "StartName";
        private const string v_RegUserID = "RegUserID";
        private const string v_RegDeptID = "RegDeptID";
        private const string v_FormDefineXmlValue = "FormDefineXmlValue";
        private const string v_ReadOnly = "FlowReadOnly";
        private const string v_FlowProcType = "FlowProcType";


        private const string v_ViewKMRef = "ViewKMRef";


        #region 供内容页访问的变量,内容页根据这些变量值做相应的控制



        /// <summary>
        /// 用户编号
        /// </summary>
        public long lngUserID = 0;

        /// <summary>
        /// 流程对象
        /// </summary>
        public objFlow oFlow;

        /// <summary>
        /// 流程模型编号
        /// </summary>
        public long lngFlowModelID;

        /// <summary>
        /// 环节模型编号
        /// </summary>
        public long lngNodeModelID;


        /// <summary>
        /// 当前消息编号
        /// </summary>
        public long lngMessageID;

        /// <summary>
        /// 是否阅知时保存意见


        /// </summary>
        public int intIsReaderSaveOpinion = 0;

        /// <summary>
        /// 是否页面只读
        /// </summary>
        public bool m_blnReadOnly = false;

        /// <summary>
        /// 是否阅知
        /// </summary>
        public bool m_bIsReader = false;
        #endregion

        private eOA_FlowProcessType iFPType = eOA_FlowProcessType.efptReadFinished;

        #endregion

        #region 委托 事件定义 表单处理接口

        //内容页获取表单值的定义
        public delegate XmlDocument GetFormsValue(long lngActionID, string strActionName);

        /// <summary>
        /// 内容页获得表单值的处理接口,返回XML对象
        /// </summary>
        public event GetFormsValue myGetFormsValue;

        //内容页设置表单值 和设置表单只读的定义
        public delegate void DoContentActions();

        //内容页通过母版页中 __doPostBackCustomize 进行post操作的定义


        public delegate void DoPostBackCustomize(string strSender, string strPara);

        //定义内容页用户自定义检查的
        public delegate bool DoContentValid();

        //定义内容页用户自定义检查的
        public delegate bool DoContentSubmitValid(long lngActionID, string strActionName);

        /// <summary>
        /// 内容页设置表单



        /// </summary>
        public event DoContentActions mySetFormsValue;


        public event DoContentActions mySetContentReadOnly;



        /// <summary>
        /// 提交流程前内容页处理接口
        /// </summary>
        public event DoContentSubmitValid myPreClickCustomize;

        /// <summary>
        /// 暂存流程保存前内容页处理接口
        /// </summary>
        public event DoContentValid myPreSaveClickCustomize;


        //内容页通过母版页中 __doPostBackCustomize 进行post操作的接口


        public event DoPostBackCustomize myPostBackCustomize;

        #endregion

        #region Page_Load调用的函数



        //将Flow信息存储在ViewState中，以备使用
        /// <summary>
        /// 
        /// </summary>
        private void SaveFlowInfo()
        {

            ViewState[v_FlowID] = oFlow.FlowID;
            ViewState[v_NodeName] = oFlow.NodeName;
            ViewState[v_AttachRight] = oFlow.AttachRight;
            ViewState[v_MessageType] = oFlow.MessageType;
            ViewState[v_MessageState] = oFlow.MessageStatus;
            ViewState[v_AppID] = oFlow.AppID;
            ViewState[v_OpID] = oFlow.OpID;
            ViewState[v_StartID] = oFlow.StartedID;
            ViewState[v_StartName] = oFlow.StartedName;
            ViewState[v_MsgActorClass] = oFlow.ActorClass;

        }

        /// <summary>
        /// 
        /// </summary>
        private void InitClientScript()
        {
            txtOpinion.Attributes.Add("onblur", "javascript:MaxLength(this,500,'输入处理意见超出限定长度：');");
        }

        #endregion

        /// <summary>
        /// 根据oFlow计算 iFPType 及 m_blnReadOnly 的值
        /// </summary>
        private void GetFormStatusValues()
        {

            if (lngMessageID != 0)
            {
                if (oFlow.ReceiverID != lngUserID && oFlow.ReceiverID != 0)
                {

                    iFPType = eOA_FlowProcessType.eftpLookOtherMsg;
                    m_blnReadOnly = true;
                }
                else
                {

                    if (oFlow.MessageStatus == e_MessageStatus.emsHandle)
                    {
                        if (oFlow.ReceiverID == 0)
                        {
                            iFPType = eOA_FlowProcessType.eftpReceiveMsg;
                            m_blnReadOnly = true;
                        }
                        else
                        {
                            if (oFlow.ActorClass == e_ActorClass.fmMasterActor || oFlow.ActorClass == e_ActorClass.fmInfluxActor)
                            {
                                iFPType = eOA_FlowProcessType.efptNormal;
                            }
                            else
                            {
                                iFPType = eOA_FlowProcessType.efptReader;
                            }
                        }
                    }
                    else if (oFlow.MessageStatus == e_MessageStatus.emsFinished || oFlow.MessageStatus == e_MessageStatus.emsWaiting || oFlow.MessageStatus == e_MessageStatus.emsStop)
                    {
                        m_blnReadOnly = true;
                        if (oFlow.MessageStatus == e_MessageStatus.emsWaiting)
                        {
                            iFPType = eOA_FlowProcessType.eftpWaitingMsg;
                        }
                        if (oFlow.MessageStatus == e_MessageStatus.emsFinished)
                        {
                            if (oFlow.ActorClass == e_ActorClass.fmMasterActor || oFlow.ActorClass == e_ActorClass.fmInfluxActor)
                            {
                                iFPType = eOA_FlowProcessType.efptNormalFinished;
                            }
                            else
                            {
                                iFPType = eOA_FlowProcessType.efptReadFinished;
                            }
                        }
                        if (oFlow.MessageStatus == e_MessageStatus.emsStop)
                        {
                            iFPType = eOA_FlowProcessType.eftpStopMsg;
                        }



                    }
                }
            }
            else
            {
                iFPType = eOA_FlowProcessType.efptNew;
            }

            ViewState[v_ReadOnly] = m_blnReadOnly;
            ViewState[v_FlowProcType] = iFPType;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (Session["UserID"] == null) { Response.Redirect("~/default.aspx"); }

            lngUserID = (long)Session["UserID"];
            this.cmdHidden.Click += new System.EventHandler(this.cmdHidden_Click);
            this.cmdAttemper.Click += new EventHandler(cmdAttemper_Click);
            this.cmdHiddenSave.Click += new System.EventHandler(this.cmdHiddenSave_Click);
            this.Unload += new EventHandler(FlowForms_Unload);

            lngFlowModelID = long.Parse(Request.QueryString["FlowModelID"]);
            lngMessageID = long.Parse(Request.QueryString["MessageID"]);
            hidAutoPass.Value = "false";
            
            if (lngMessageID == 0)
            {
                //启动流程时 不需要意见输入筐
                this.ShowOpinionEdit.Visible = false;
            }

            InitClientScript();



            long flowID;

            if (Page.IsPostBack == false)
            {
                ShowGC.Visible = false;
                oFlow = new objFlow((long)Session["UserID"], lngFlowModelID, lngMessageID);

                GetFormStatusValues();    //计算 m_blnReadOnly 和 iFPType 变量的值 2010.1.4 改为在母版页上计算



                lngFlowModelID = oFlow.FlowModelID;

                ViewState["v_FlowModelID"] = lngFlowModelID;
                ViewState[v_NodeModelID] = oFlow.NodeModelID;
                lngNodeModelID = oFlow.NodeModelID;

                if (ViewState["v_FlowID"] == null)
                {
                    ViewState["v_FlowID"] = oFlow.FlowID;
                }
                flowID = oFlow.FlowID;

                EpowerCom.Message msg = new EpowerCom.Message();
                if (msg.CanDeleteFlow(oFlow.FlowID, (long)Session["UserID"])
                    || FlowModel.hasDeleteFlowRight((long)Session["UserID"], oFlow.FlowID))
                {
                    cmdDeleteFlow.Visible = true;
                }
                else
                {
                    cmdDeleteFlow.Visible = false;
                }

                SaveFlowInfo();//将Flow信息存储在ViewState中，以备使用
                InitInterface(lngFlowModelID, lngMessageID, oFlow);



                //绑定补充意见
                InitMsgProcess(oFlow.FlowID);


                if (m_blnReadOnly == false && lngMessageID != 0)
                {
                    //设置快速处理意见录入内容
                    setOpinions();
                }
            }
            else
            {
                string strSender = hidTarget.Value;
                flowID = (long)ViewState["v_FlowID"];

                lngFlowModelID = (long)ViewState["v_FlowModelID"];
                lngNodeModelID = (long)ViewState[v_NodeModelID];

                m_blnReadOnly = (bool)ViewState[v_ReadOnly];
                iFPType = (eOA_FlowProcessType)ViewState[v_FlowProcType];

                if (strSender == "litMPList")
                {
                    //判断是否是删除 补充意见
                    long lngMPID = long.Parse(hidPara.Value);

                    FlowDP.DeleteMsgProcess(lngMPID);
                    //绑定补充意见
                    InitMsgProcess((long)ViewState[v_FlowID]);
                }

                if (myPostBackCustomize != null)
                    myPostBackCustomize(strSender, hidPara.Value);

                if (!string.IsNullOrEmpty(this.hidChange.Value.Trim()))
                {
                    if (m_blnReadOnly == false && lngMessageID != 0)
                    {
                        //设置快速处理意见录入内容
                        setOpinions();
                        this.hidChange.Value = string.Empty;
                    }
                }
            }

            //2010-05-03 自动通过时指定参数 并计算缺省动作编号
            if (Request.QueryString["autopass"] != null && Request.QueryString["autopass"] == "true")
            {
                hidDefaultActionID.Value = FlowModel.GetDefaultNodeActionID(lngFlowModelID, lngNodeModelID).ToString();
                txtOpinion.Text = "【自动处理】";

                hidAutoPass.Value = "true";
            }


            //会签、阅知、协办、沟通等非主办情况下显示意见,之前在只读状态下隐藏了处理意见

            
            if ((e_ActorClass)ViewState[v_MsgActorClass] != e_ActorClass.fmMasterActor && m_blnReadOnly == false)
            {
                //处理意见
                ShowCL.Visible = true;

            }

            //不管页面是否只读情况下附件特殊控制


            if (ViewState[v_AttachRight].ToString() == "0")
            {
                CtrAttachment1.ReadOnly = true;
            }
            else
            {
                //协办 处理时可以上传附件


                if ((e_MessageStatus)ViewState[v_MessageState] == e_MessageStatus.emsHandle && (e_ActorClass)ViewState[v_MsgActorClass] == e_ActorClass.fmAssistActor)
                {
                    CtrAttachment1.ReadOnly = false;
                }
            }
            hidPara.Value = string.Empty;     //避免重复提交
            hidTarget.Value = string.Empty;   //避免重复提交
        }

        void cmdAttemper_Click(object sender, EventArgs e)
        {
            long lngActionID = 0;
            string strActionName = "";


            #region 子表单获取表单内容


            XmlDocument xmlDoc = new XmlDocument();
            if (myGetFormsValue != null)
                xmlDoc = myGetFormsValue(lngActionID, strActionName);
            #endregion

            this.Page.ClientScript.RegisterStartupScript(this.GetType(), "TempKey1", @"<script language='javascript'>window.parent.header.flowInfo.FormXMLValue.value=" + StringTool.JavaScriptQ(xmlDoc.InnerXml) +
                            ";if(typeof(window.parent.header.flowInfo.FormDefineValue)!='undefined'){window.parent.header.flowInfo.FormDefineValue.value=" + StringTool.JavaScriptQ(this.FormDefineXmlValue) + ";}" +
                           @"window.parent.header.SendFlowForAttemper('" + hidAttemperID.Value + "','" + lngFlowModelID.ToString() + @"');</script>");

        }

        #region 释放临时session
        private void FlowForms_Unload(object sender, EventArgs e)
        {
            if (Session["ExtendParameter"] != null)
            {
                Session.Remove("ExtendParameter");
            }
        }
        #endregion



        /// <summary>
        /// 设置快速意见录入内容
        /// </summary>
        private void setOpinions()
        {
            long lngUserID = long.Parse(Session["UserID"].ToString());
            DataTable dt = EA_DefinePersonOpinionDP.GetDefinePersonOp(lngUserID);

            string strTmpKey = "";
            string strTmpValue = "";
            ddlOpinions.Items.Clear();
            ddlOpinions.Items.Add(new ListItem("", "0"));
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                strTmpKey = dt.Rows[i]["ID"].ToString();
                strTmpValue = dt.Rows[i]["Name"].ToString();
                ddlOpinions.Items.Add(new ListItem(strTmpValue, strTmpKey));

            }
            if (dt.Rows.Count > 0)
                ddlOpinions.Attributes.Add("onchange", "AddOpinionContent();");

            dt.Dispose();
        }


        /// <summary>
        /// 设置补充处理意见的值

        /// </summary>
        /// <param name="lngFlowID"></param>
        private void InitMsgProcess(long lngFlowID)
        {
            DataTable dt = FlowDP.GetMsgProcess(lngFlowID);
            int iCount = 0;

            string sProcess = "";

            int iContactAuto = 0;
            int iRowCount = 0;
            iContactAuto = int.Parse(CommonDP.GetConfigValue("Other", "ContactAuto"));

            foreach (DataRow dr in dt.Rows)
            {
                iCount++;
                string sDate = dr["mptime"].ToString();
                string sOpinion = dr["mpcontent"].ToString();
                string sUserName = dr["name"].ToString();
                string sFeedBackID = dr["mpid"].ToString();
                string sUser = dr["userid"].ToString();
                string sIsAuto = dr["isauto"].ToString();
                string sNodeName = dr["NodeName"].ToString();

                //补充意见
                sOpinion = sOpinion.Trim() == "" ? "" : "&nbsp;<font color=green>" + StringTool.ParseForHtml(sNodeName + sOpinion) + "</font>";

                string sTR = "";
                sTR += AddTD(sDate + "&nbsp;", "noWrap class='list'");
                if (iContactAuto == 0 || iContactAuto == 2)
                {
                    sTR += AddTD(AddContactMenu(dr["email"].ToString(), iRowCount++), "class='list'");
                }
                if (iContactAuto == 1 || iContactAuto == 2)
                {
                    sTR += AddTD(AddContactQQMenu(dr["QQ"].ToString(), iRowCount++), " class='list'");
                }

                if (lngUserID == long.Parse(sUser) && sIsAuto == "")
                {
                    //sTR += AddTD(@"<INPUT type='button' id='btnDelm" + iCount.ToString() + @"' value='删除' class='btnClass' onClick=""__doPostBackCustomize('litMPList','" + sFeedBackID + @"');return false;"">", "nowrap  class='list'");
                    sOpinion += "(<A href='#' onclick=\"if(confirm('是否真的要删除？')){ __doPostBackCustomize('" + "litMPList" + "','" + sFeedBackID + @"');return false;}"">删除</A>)";
                }
                sTR += AddTD(sUserName + "&nbsp;", "nowrap class='list'") +
                    AddTD(sOpinion, "width=100%  class='list'");

                sTR = "<tr>" + sTR + "</tr>";
                sProcess += sTR;
            }
            if (sProcess != "")
            {
                litMPList.Text = "<table class='listContent'  >" + sProcess + "</table>";
                trMPList.Visible = true;
            }
            else
            {
                litMPList.Text = SP();
                trMPList.Visible = false;
            }
        }

        private static string SP()
        {
            return "&nbsp;";
        }

        private string AddContactMenu(string sUserEmail, int iRowCount)
        {
            string strHtml;
            if (sUserEmail.Trim() != "")
            {
                strHtml = @"<TABLE><TR><TD><IMG id='tdImgContact" + iRowCount.ToString() +
                    " alt='' width='12' src='" + Constant.ApplicationPath + "/images/blank.gif' onload=\"IMNRC('" + sUserEmail +
                    "')\"></TD></TR></TABLE>";
            }
            else
            {
                strHtml = @"<TABLE><TR><TD><IMG id='tdImgContact" + iRowCount.ToString() +
                    " alt='' width='12' src='" + Constant.ApplicationPath + "/images/blank.gif' ></TD></TR></TABLE>";
            }
            return strHtml;
        }

        private string AddContactQQMenu(string sUserQQ, int iRowCount)
        {
            string strHtml;
            if (sUserQQ.Trim() != "")
            {
                strHtml = @"<TABLE><TR><TD><IMG id='tdImgContactQQ" + iRowCount.ToString() +
                    " alt='' width='12' src='" + Constant.ApplicationPath + "/images/a2.ico' onclick=\"window.open('http://wpa.qq.com/msgrd?V=1&Uin=" + sUserQQ +
                    "')\"></TD></TR></TABLE>";
            }
            else
            {
                strHtml = @"<TABLE><TR><TD><IMG id='tdImgContactQQ" + iRowCount.ToString() +
                    " alt='' width='12' src='" + Constant.ApplicationPath + "/images/blank.gif' ></TD></TR></TABLE>";
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


        /// <summary>
        /// 初始化界面，MessageID 为0 则肯定是起草流程，



        /// 
        /// </summary>
        /// <param name="lngFlowModelID"></param>
        /// <param name="lngMessageID"></param>
        private void InitInterface(long lngFlowModelID, long lngMessageID, objFlow oFlow)
        {


            //if (m_blnReadOnly == true)  //设置表单只读  
            //    SetFormReadOnly();

            //当前消息为阅知 协办 会签 沟通，并且未完，可以编辑意见的
            if (oFlow.ActorClass == e_ActorClass.fmReaderActor || oFlow.ActorClass == e_ActorClass.fmAssistActor || oFlow.ActorClass == e_ActorClass.fmInfluxActor || oFlow.ActorClass == e_ActorClass.fmCommunicActor)
            {
                m_bIsReader = true;  //表示阅知 协办 会签 沟通


            }

            #region 获取表单内容
            if (mySetFormsValue != null)
                mySetFormsValue();
            #endregion

            if (lngMessageID != 0)
            {

                ShowGC.Visible = true;
            }


            #region 过程、工作流按键 属性



            CtrlProcess1.FlowID = oFlow.FlowID;
            CtrlProcess1.FlowModelID = oFlow.FlowModelID;
            CtrlProcess1.FlowProcessType = iFPType;

            CtrActions1.FlowModelID = oFlow.FlowModelID;
            CtrActions1.NodeModelID = oFlow.NodeModelID;


            CtrActions1.IsReader = m_bIsReader;
            

            CtrActions1.ReadOnly = m_blnReadOnly;

            //如果是驳回并且仅重审，则不显示动作 2010-05-01
            if (oFlow.ReceiveType == e_MsgReceiveType.emrtBackHasDoneRedo)
            {
                CtrActions1.ReadOnly = true;
            }

            CtrImportance1.Importance = oFlow.Importance;
            //2009-06-10修改为新的接口获取特殊权限，优化
            //CtrFlowSpecRight1.MessageID = oFlow.MessageID;
            CtrFlowSpecRight1.NodeModelID = oFlow.NodeModelID;
            CtrFlowSpecRight1.FlowModelID = oFlow.FlowModelID;
            CtrFlowSpecRight1.FlowID = oFlow.FlowID;
            CtrFlowSpecRight1.MessageID = oFlow.MessageID;


            //如果当前消息的接收者和当前用户不一致 或 流程结束和暂停状态 或消息的状态为结束状态，子流程列表不显示 
            if (oFlow.ReceiverID != lngUserID || oFlow.FlowStatus != e_FlowStatus.efsHandle || oFlow.MessageStatus == e_MessageStatus.emsFinished)
            {
                CtrLinkFlows1.IsVisible = false;
                //CtrLinkFlows1.Visible = false;
            }
            else
            {
                CtrLinkFlows1.UserID = oFlow.ReceiverID;
                CtrLinkFlows1.MessageID = oFlow.MessageID;
            }

            CtrButtons1.FlowProcessType = iFPType;
            CtrButtons1.MessageID = oFlow.MessageID;
            CtrButtons1.FlowID = oFlow.FlowID;
            CtrButtons1.FlowModelID = oFlow.FlowModelID;
            CtrButtons1.NodeModelID = oFlow.NodeModelID;

            //设置接收按钮是否可见
            if (iFPType == eOA_FlowProcessType.eftpReceiveMsg)
            {
                ShowReceiveBtn.Visible = true;
                CtrButtons1.IsReceiving = true;
                CtrFlowSpecRight1.IsReceiving = true;
            }
            else
            {
                ShowReceiveBtn.Visible = false;
            }

            //判断退回和回收按钮是否可见
            if (oFlow.MessageID == 0)
            {
                //新增状态下，回收和退回一定没有，减少系统计算 （20091206）


                CtrButtons1.BackVisible = false;
                CtrButtons1.TakeBackVisible = false;
            }
            else
            {
                if (oFlow.BackRight == e_IsTrue.fmTrue && m_blnReadOnly == false)
                {
                    CtrButtons1.BackVisible = MessageDep.CanSendBackFlow(lngMessageID);
                }
                else
                {
                    CtrButtons1.BackVisible = false;
                }
                if (oFlow.TakeBackRight == e_IsTrue.fmTrue)
                {
                    CtrButtons1.TakeBackVisible = MessageDep.CanTakeBackFlow(lngMessageID);
                }
                else
                {
                    CtrButtons1.TakeBackVisible = false;
                }
            }


            //判断重审按钮是否可见
            if (oFlow.MessageID != 0 && (oFlow.ReceiveType == e_MsgReceiveType.emrtBackHasDone || oFlow.ReceiveType == e_MsgReceiveType.emrtBackHasDoneRedo)) 
            {
                //当前为驳回的才可见

                CtrButtons1.ReDoBackVisible = true;
            }
            else
            {
                CtrButtons1.ReDoBackVisible = false;
            }


            //判断流程的暂停状态


            if (lngMessageID != 0 && oFlow.FlowStatus != e_FlowStatus.efsEnd &&  oFlow.FlowStatus != e_FlowStatus.efsAbort && CheckRight(Constant.PauseContinueFlow) == true)
            {
                CtrButtons1.PauseVisible = true;
            }


            if (oFlow.FlowStatus == e_FlowStatus.efsStop)
            {
                CtrButtons1.IsFlowPaused = true;


            }

            CtrAttachment1.FlowID = oFlow.FlowID;

            if (lngMessageID == 0 && TempFlowID != 0)   //如果为起草流程，且有设置传过来的流程ID
            {
                CtrAttachment1.TempFlowID = TempFlowID;
                CtrAttachment1.TempAttachmentType = TempAttachmentType;
            }


            #endregion



            //2009-04-27 修改： 流程结束后也可以补充意见
            //if (oFlow.MessageStatus == e_MessageStatus.emsFinished && oFlow.FlowStatus == e_FlowStatus.efsHandle)
            if (oFlow.MessageStatus == e_MessageStatus.emsFinished)
            {
                //判断 当前用户是否参与
                if (FlowDP.IsFlowUser(oFlow.FlowID, lngUserID) == true && oFlow.FlowStatus != e_FlowStatus.efsAbort)
                {


                    CtrButtons1.AddOpinionVisible = true;
                }

            }
            if (m_blnReadOnly == true || oFlow.ActorClass != e_ActorClass.fmMasterActor)
                SetFormReadOnly();


        }

        #region SetFormReadOnly
        private void SetFormReadOnly()
        {
            #region 表单只读处理
            if (mySetContentReadOnly != null)
                mySetContentReadOnly();
            #endregion

            CtrAttachment1.ReadOnly = true;

            //处理意见
            ShowCL.Visible = false;

            ShowFlowOP1.Visible = false;
            ShowFlowOP2.Visible = false;
            //ShowFlowOP.Visible = false;

        }



        #endregion

        public bool CheckRight(long OperatorID)
        {
            //return ((RightDP.GetUserRight(OperatorID,UserID).RightValue & (int)eO_OperateRight.eCanRead) !=0);
            RightEntity re = (RightEntity)((Hashtable)Session["UserAllRights"])[OperatorID];
            if (re == null)
                return false;
            else
                return re.CanRead;
        }



        #region SaveData



        private void cmdHidden_Click(object sender, System.EventArgs e)
        {

            long lngActionID = 0;
            if (hidActionID.Value != "")
            {
                lngActionID = long.Parse(hidActionID.Value);
            }
            string strActionName = hidActionName.Value.Trim();

            if (myPreClickCustomize != null)
                if (myPreClickCustomize(lngActionID, strActionName) == false)
                    return;


            string strAttachXml = CtrAttachment1.AttachXML;

            if (m_bIsReader == false)
            {
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), "Key1", "<script language='javascript'>if(typeof(window.parent.header.flowInfo.Attachment)!='undefined'){window.parent.header.flowInfo.Attachment.value=" + StringTool.JavaScriptQ(HttpUtility.UrlEncode(strAttachXml)) + "}</script>");
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), "Key2", "<script>window.parent.header.flowInfo.Importance.value=" + CtrImportance1.Importance.ToString() + "</script>");
            }
            if (m_blnReadOnly == false)
            {
                #region 子表单获取表单内容
                XmlDocument xmlDoc = new XmlDocument();
                //XmlDocument xmlDoc = GetAllValues();
                if (myGetFormsValue != null)
                    xmlDoc = myGetFormsValue(lngActionID, strActionName);
                #endregion
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), "Key3", "<script language='javascript'>window.parent.header.flowInfo.FormXMLValue.value=" + StringTool.JavaScriptQ(xmlDoc.InnerXml) + ";</script>");
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), "Key4", "<script language='javascript'>if(typeof(window.parent.header.flowInfo.FormDefineValue)!='undefined'){window.parent.header.flowInfo.FormDefineValue.value=" + StringTool.JavaScriptQ(this.FormDefineXmlValue) + ";}window.parent.header.SendFlowPub();</script>");
            }
            else if (intIsReaderSaveOpinion == 1)
            {
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), "Key5", "<script language='javascript'>window.parent.header.ReaderOverPub('no','yes');</script>");
            }
        }

        private void cmdHiddenSave_Click(object sender, System.EventArgs e)
        {
            if (myPreSaveClickCustomize != null)
                if (myPreSaveClickCustomize() == false)
                    return;

            if (m_bIsReader == false)
            {
                string strAttachXml = CtrAttachment1.AttachXML;
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), "Key1", "<script language='javascript'>if(typeof(window.parent.header.flowInfo.Attachment)!='undefined'){window.parent.header.flowInfo.Attachment.value=" + StringTool.JavaScriptQ(HttpUtility.UrlEncode(strAttachXml)) + "}</script>");
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), "Key2", "<script>window.parent.header.flowInfo.Importance.value=" + CtrImportance1.Importance.ToString() + "</script>");
            }
            if (m_blnReadOnly == false)
            {
                #region 子表单获取表单内容
                XmlDocument xmlDoc = new XmlDocument();
                if (myGetFormsValue != null)
                    xmlDoc = myGetFormsValue(0, "");

                #endregion
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), "Key3", "<script language='javascript'>window.parent.header.flowInfo.FormXMLValue.value=" + StringTool.JavaScriptQ(xmlDoc.InnerXml) + ";window.parent.header.TempSavePub();</script>");
            }
            else if (intIsReaderSaveOpinion == 1)
            {
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), "Key4", "<script language='javascript'>window.parent.header.ReaderOverPub('yes','yes');</script>");
            }
        }
        #endregion


        protected void cmdMsgProcess_Click(object sender, System.EventArgs e)
        {
            long lngFlowID = (long)ViewState[v_FlowID];

            FlowDP.AddMsgProcess(lngFlowID, long.Parse(Session["UserID"].ToString()), txtMsgProcess.Value.Trim());


            this.InitMsgProcess(lngFlowID);
        }

        protected void cmdDeleteFlow_Click1(object sender, EventArgs e)
        {
            Message msg = new Message();

            long lngFlowID = (long)ViewState[v_FlowID];
            try
            {
                msg.AdminDeleteFlow(lngFlowID, long.Parse(Session["UserID"].ToString()),"有权限删除");

                if (Session["FromUrl"] != null)
                {
                    Response.Redirect(Session["FromUrl"].ToString());
                }
                else
                {
                    Response.Redirect(Constant.ApplicationPath + "/Forms/FrmContent.aspx");
                }

            }
            catch (Exception err)
            {
                PageTool.MsgBox(this.Page, err.Message);
            }
        }

        protected void cmdPauseFlow_Click1(object sender, EventArgs e)
        {
            Message msg = new Message();

            long lngFlowID = (long)ViewState[v_FlowID];
            string sType = "0";
            sType = hidPauseType.Value.Trim();
            try
            {
                if (sType == "0")
                {
                    msg.PauseFlow(lngFlowID, long.Parse(Session["UserID"].ToString()), hidPauseFlow.Value.Trim());
                }
                else
                {
                    msg.ContinueFlow(lngFlowID, long.Parse(Session["UserID"].ToString()));
                }
                Response.Redirect(Constant.ApplicationPath + "/Forms/flow_Normal.aspx?MessageID=" + lngMessageID.ToString());

            }
            catch (Exception err)
            {
                PageTool.MsgBox(this.Page, err.Message);
            }
        }

    }
}
