using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Drawing;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using EpowerCom;
using EpowerGlobal;
using Epower.DevBase.BaseTools;
using System.Xml;
using Epower.ITSM.SqlDAL;
using Epower.ITSM.Base;
using appDataProcess;
using Epower.DevBase.Organization.SqlDAL;
using Epower.DevBase.Organization.Base;
using System.Text;

namespace Epower.ITSM.Web.QuestHouse
{
    public partial class frmFlow_QuestHouse : BasePage
    {
        public string FlowID
        {
            get
            {
                if (ViewState["frmFlow_QuestHouse_FlowID"] != null)
                {
                    return ViewState["frmFlow_QuestHouse_FlowID"].ToString();
                }
                else
                {
                    return "0";
                }
            }
            set
            {
                ViewState["frmFlow_QuestHouse_FlowID"] = value;
            }
        }

        /// <summary>
        /// 应用id
        /// </summary>
        public string AppID
        {
            get
            {
                if (ViewState["frmFlow_QuestHouse_AppID"] != null)
                {
                    return ViewState["frmFlow_QuestHouse_AppID"].ToString();
                }
                else
                {
                    return "0";
                }
            }
            set
            {
                ViewState["frmFlow_QuestHouse_AppID"] = value;
            }
        }

        /// <summary>
        /// 流程模型id
        /// </summary>
        public string FlowModelID
        {
            get
            {
                if (ViewState["frmFlow_QuestHouse_FlowModelID"] != null)
                {
                    return ViewState["frmFlow_QuestHouse_FlowModelID"].ToString();
                }
                else
                {
                    return "0";
                }
            }
            set
            {
                ViewState["frmFlow_QuestHouse_FlowModelID"] = value;
            }
        }

        /// <summary>
        /// 取得流程MessageID
        /// </summary>
        public string MessageID
        {
            get
            {
                if (ViewState["MessageID"] != null)
                {
                    return ViewState["MessageID"].ToString();
                }
                else
                {
                    return "0";
                }
            }
            set
            {
                ViewState["MessageID"] = value;
            }
        }

        #region  变量
        private objFlow oFlow;
        private FlowForms myFlowForms;
        #endregion
        public string sApplicationUrl = Epower.ITSM.Base.Constant.ApplicationPath;     //虚拟路径
        protected void Page_Load(object sender, EventArgs e)
        {
            myFlowForms = (FlowForms)this.Master;
            if (!IsPostBack)
            {
                //设置显示
                PageDeal.SetLanguage(this.Controls[0]);

                myFlowForms.EmailValue = true;
                Session["showdisplay1"] = "";
                Session["showdisplay2"] = "";
                //InitDll();
            }
            myFlowForms.mySetContentReadOnly += new FlowForms.DoContentActions(Master_mySetContentReadOnly);
            myFlowForms.myGetFormsValue += new FlowForms.GetFormsValue(Master_myGetFormsValue);
            myFlowForms.mySetFormsValue += new FlowForms.DoContentActions(Master_mySetFormsValue);
            myFlowForms.myPreClickCustomize += new FlowForms.DoContentSubmitValid(Master_myPreClickCustomize);
            myFlowForms.myPreSaveClickCustomize += new FlowForms.DoContentValid(Master_myPreSaveClickCustomize);
            //部门
            if (lb_execDeptname.Text.Trim() == "")
            {
                if (HiddeptName.Value.Trim() != "")
                {
                    lb_execDeptname.Text = HiddeptName.Value;
                }
            }
            //电话
            if (lb_execByPhone.Text.Trim() == "")
            {
                if (HidexecByPhone.Value.Trim() != "")
                {
                    lb_execByPhone.Text = HidexecByPhone.Value;
                }
            }

            //工号
            if (lb_execByGH.Text.Trim() == "")
            {
                if (HidexecByGP.Value.Trim() != "")
                {
                    lb_execByGH.Text = HidexecByGP.Value;
                }
            }

            //申请人            if (Lb_execByName.Text.Trim() == "")
            {
                if (HidexecByName.Value.Trim() != "")
                {
                    Lb_execByName.Text = HidexecByName.Value;
                }
            }


        }


        #region 设置页面为只读 Master_mySetContentReadOnly
        /// <summary>
        /// 设置页面为只读        /// </summary>
        void Master_mySetContentReadOnly()
        {
            #region Master_mySetContentReadOnly
            if (ctr_JRdate.ContralState != eOA_FlowControlState.eHidden)
            {
                ctr_JRdate.ContralState = eOA_FlowControlState.eReadOnly;
                //this.chkSelSave.Visible = false;
            }
            if (ctr_OutDate.ContralState != eOA_FlowControlState.eHidden)
                ctr_OutDate.ContralState = eOA_FlowControlState.eReadOnly;
            if (ctrTXJZname.ContralState != eOA_FlowControlState.eHidden)
            {
                ddlTXJZ.Enabled = false;
                ctrTXJZname.ContralState = eOA_FlowControlState.eReadOnly;
            }
            if (ctrFlowIsOkScan.ContralState != eOA_FlowControlState.eHidden)
                ctrFlowIsOkScan.ContralState = eOA_FlowControlState.eReadOnly;
            if (CtrFlowRemark1.ContralState != eOA_FlowControlState.eHidden)
                CtrFlowRemark1.ContralState = eOA_FlowControlState.eReadOnly;
            if (Session["showdisplay1"].ToString() == "")
            {
                Session["showdisplay1"] = "1";
            }
            if (Session["showdisplay2"].ToString() == "")
            {
                Session["showdisplay2"] = "1";
            }

            if (cblAddress.Visible == true)
            {
                cblAddress.Enabled = false;
            }

            if (btn_execByname.Visible == true)
            {
                btn_execByname.Visible = false;
            }
            if (ddl_IsBudan.Visible == true)
            {
                ddl_IsBudan.Enabled = false;
            }

            if (ddlActionType.Visible == true)
            {
                ddlActionType.Enabled = false;
            }
            #endregion
        }
        #endregion

        #region 取得页面值生成XML Master_myGetFormsValue
        /// <summary>
        /// 取得页面值生成XML
        /// </summary>
        /// <returns></returns>
        XmlDocument Master_myGetFormsValue(long lngActionID, string strActionName)
        {

            oFlow = myFlowForms.oFlow;
            FieldValues fv = new FieldValues();

            if (HouseID.Value == "")
            {
                HouseID.Value = EPGlobal.GetNextID("Flow_QuestHouseID").ToString();
            }
            fv.Add("HouseID", HouseID.Value==""? "0" : HouseID.Value);
            fv.Add("createbyid", lb_createById.Value==""? "0" : lb_createById.Value);
            fv.Add("createbyname", lb_createByName.Text);
            fv.Add("createbydeptid", hd_deptId.Value == "" ? "0" : hd_deptId.Value);
            fv.Add("createbydeptname", lb_createDeptname.Text);
            fv.Add("CJRByPhone", Lb_createPhone.Text);
            fv.Add("execbyid", HidexecById.Value=="" ? "0" : HidexecById.Value);
            fv.Add("execbyname", HidexecByName.Value);
            fv.Add("execbyphone", HidexecByPhone.Value);
            fv.Add("execbydeptid", HiddeptId.Value=="" ? "0" : HiddeptId.Value);
            fv.Add("execbydeptname", HiddeptName.Value);
            fv.Add("execbyno", HidexecByGP.Value);
            fv.Add("createdate", lb_createDate.Text);
            fv.Add("comeindate", ctr_JRdate.dateTime.ToString());
            fv.Add("outdate", ctr_OutDate.dateTime.ToString());
            fv.Add("zgflowdate", ZGFlowDate.Text);
            fv.Add("SZSZGflowDate", SZSZGflowDate.Text);
            fv.Add("sqdescr", CtrFlowRemark1.Value.Replace("\r\n", ""));
            fv.Add("txjzname", ctrTXJZname.Value);
            fv.Add("txjzis", ddlTXJZ.SelectedValue);
            fv.Add("isokscan", ctrFlowIsOkScan.CatelogValue);
            fv.Add("EmailNotify", myFlowForms.EmailValue.ToString());
            fv.Add("SMSNotify", myFlowForms.NotifyValue.ToString());
            //取得服务单前缀，生成服务单号            string sBuildCode = string.Empty;
            string sServiceNo = this.Lb_ITILNO.Text.Trim();
            if (sServiceNo == string.Empty)
            {
                sServiceNo = RuleCodeDP.GetCodeBH2(10006, ref sBuildCode);
            }
            string strAddressName = "";
            string strAddress = "";
            foreach (ListItem li in cblAddress.Items)
            {
                if (li.Selected)
                {
                    strAddress += li.Value + ",";
                    strAddressName += li.Text + ",";
                }
            }

            string stIPerson = "姓名：" + HidexecByName.Value + "#工号：" + HidexecByGP.Value + "#部门：" + HiddeptName.Value + "；";
            DataTable dt = Flow_QuestHouse.getUser(long.Parse(HouseID.Value));//行内人员
            foreach (DataRow dr in dt.Rows)
            {
                stIPerson += "姓名：" + dr["username"].ToString() + "#工号：" + dr["logName"].ToString() + "#部门：" + dr["DeptName"].ToString() + "；";
            }
            string stOPerson = "";
            DataTable dt2 = Flow_QuestHouse.getCompurUser(long.Parse(HouseID.Value));//行外人员
            foreach (DataRow dr in dt2.Rows)
            {
                stOPerson += "姓名：" + dr["peopleName"].ToString() + "#证件号码：" + dr["CardNO"].ToString() + "#电话：" + dr["PeoplePhone"].ToString() + "#公司名称：" + dr["computeName"].ToString() + "；";
            }
            string stIPUser = "";
            DataTable dt3 = Flow_QuestHouse.getExecUserIP(long.Parse(HouseID.Value));//IP及用户信息


            foreach (DataRow dr in dt3.Rows)
            {
                stIPUser += "IP：" + dr["IP"].ToString() + "#用户名：" + Flow_QuestHouse.getExecUser(long.Parse(HouseID.Value), dr["IP"].ToString()) + "；";
            }



            fv.Add("IsBudan", ddl_IsBudan.SelectedValue.ToString());
            fv.Add("iscaozuoj", "0");
            fv.Add("isahouse", "0");
            fv.Add("isbhouse", "0");
            fv.Add("ischouse", "0");
            fv.Add("ActionTypeID", ddlActionType.SelectedItem.Value);
            fv.Add("ActionTypeName", ddlActionType.SelectedItem.Text);
            fv.Add("OpObjId", "0");
            fv.Add("OpObj", "");
            fv.Add("ITILNO", sBuildCode + sServiceNo);
            fv.Add("Address", strAddress);
            fv.Add("AddressName", strAddressName);
            fv.Add("path", CommonDP.GetConfigValue("Other", "ActionHouesFilePath"));
            fv.Add("souesspath", CommonDP.GetConfigValue("Other", "ModeActionHouesFilePath"));
            fv.Add("IPerson", stIPerson);
            fv.Add("OPerson", stOPerson);
            fv.Add("IPUser", stIPUser);
            fv.Add("frmNumber", sBuildCode + sServiceNo);
            XmlDocument xmlDoc = fv.GetXmlObject();
            return xmlDoc;
        }
        #endregion

        #region
        /// <summary> 
        /// 设置窗体值
        /// </summary>
        void Master_mySetFormsValue()
        {
            oFlow = myFlowForms.oFlow;
                ImplDataProcess dp = new ImplDataProcess(myFlowForms.oFlow.AppID);

                DataSet ds = dp.GetFieldsDataSet(myFlowForms.oFlow.FlowID, myFlowForms.oFlow.OpID);
                DataTable dt = ds.Tables[0];
                myFlowForms.FormTitle = myFlowForms.oFlow.FlowName;
                this.FlowID = myFlowForms.oFlow.FlowID.ToString();
                this.MessageID = myFlowForms.oFlow.MessageID.ToString();
                this.AppID = myFlowForms.oFlow.AppID.ToString();
                this.FlowModelID = myFlowForms.oFlow.FlowModelID.ToString();
                hidFlowID.Value = myFlowForms.oFlow.FlowID.ToString();
                hidActorClass.Value = myFlowForms.oFlow.ActorClass.ToString();


                    if (dt.Rows.Count > 0)
                    {

                        HouseID.Value = dt.Rows[0]["HouseID"].ToString();//进入操作间的逐渐id
                        Lb_ITILNO.Text = dt.Rows[0]["ITILNO"].ToString();

                        lb_createById.Value = dt.Rows[0]["createbyid"].ToString();

                        if (dt.Rows[0]["IsBudan"].ToString() == "1")
                        {
                            ddl_IsBudan.Dispose();
                            ddl_IsBudan.Items.FindByValue("1").Selected = true;
                        }
                        else
                        {
                            ddl_IsBudan.Dispose();
                            ddl_IsBudan.Items.FindByValue("0").Selected = true;
                        }

                        lb_createByName.Text = dt.Rows[0]["createbyname"].ToString();

                        hd_deptId.Value = dt.Rows[0]["createbydeptid"].ToString();

                        Lb_createPhone.Text = dt.Rows[0]["CJRByPhone"].ToString();


                        lb_createDeptname.Text = dt.Rows[0]["createbydeptname"].ToString();

                        HidexecById.Value = dt.Rows[0]["execbyid"].ToString();

                        Lb_execByName.Text = dt.Rows[0]["execbyname"].ToString();

                        lb_execByPhone.Text = dt.Rows[0]["execbyphone"].ToString();

                        HiddeptId.Value = dt.Rows[0]["execbydeptid"].ToString();

                        lb_execDeptname.Text = dt.Rows[0]["execbydeptname"].ToString();

                        lb_execByGH.Text = dt.Rows[0]["execbyno"].ToString();


                        lb_createDate.Text = dt.Rows[0]["createdate"].ToString();

                        ctr_JRdate.dateTime = Convert.ToDateTime(dt.Rows[0]["comeindate"].ToString());

                        ctr_OutDate.dateTime = Convert.ToDateTime(dt.Rows[0]["outdate"].ToString());

                        ZGFlowDate.Text = dt.Rows[0]["zgflowdate"].ToString();

                        // SZSZGflowDate.Text = dt.Rows[0]["sqdescr"].ToString();
                        ddlActionType.SelectedIndex = ddlActionType.Items.IndexOf(ddlActionType.Items.FindByValue(dt.Rows[0]["ActionTypeID"].ToString()));

                        SZSZGflowDate.Text = dt.Rows[0]["SZSZGflowDate"].ToString();

                        CtrFlowRemark1.Value = dt.Rows[0]["sqdescr"].ToString();
                        HidexecByName.Value = Lb_execByName.Text;
                        ctrTXJZname.Value = dt.Rows[0]["txjzname"].ToString();
                        ddlTXJZ.SelectedValue = dt.Rows[0]["txjzis"].ToString();
                        HidexecByGP.Value = lb_execByGH.Text;
                        HidexecByPhone.Value = lb_execByPhone.Text;
                        HiddeptName.Value = lb_execDeptname.Text;
                        ctrFlowIsOkScan.CatelogValue = dt.Rows[0]["isokscan"].ToString();
                        string strAddress = dt.Rows[0]["Address"].ToString();
                        if (dt.Rows[0]["iscaozuoj"].ToString() == "1")
                        {
                            strAddress += "1,";
                        }
                        if (dt.Rows[0]["isahouse"].ToString() == "1")
                        {
                            strAddress += "2,";
                        }
                        if (dt.Rows[0]["isbhouse"].ToString() == "1")
                        {
                            strAddress += "3,";
                        }
                        if (dt.Rows[0]["ischouse"].ToString() == "1")
                        {
                            strAddress += "4,";
                        }
                        if (strAddress.Length > 0)
                        {
                            if (strAddress.IndexOf(',') != -1)
                            {
                                string[] arrAddress = strAddress.Split(',');
                                foreach (string id in arrAddress)
                                {
                                    foreach (ListItem li in cblAddress.Items)
                                    {
                                        if (li.Value == id.Trim())
                                        {
                                            li.Selected = true;
                                            break;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                foreach (ListItem li in cblAddress.Items)
                                {
                                    if (li.Value == strAddress.Trim())
                                    {
                                        li.Selected = true;
                                        break;
                                    }
                                }
                            }
                        }
                    
                }
                else
                {

                    if (HouseID.Value == "")
                    {
                        HouseID.Value = EPGlobal.GetNextID("Flow_QuestHouseID").ToString();
                    }
                    lb_createByName.Text = Session["PersonName"].ToString();
                    lb_createById.Value = Session["UserID"].ToString();
                    Lb_execByName.Text = lb_createByName.Text;
                    HidexecByName.Value = lb_createByName.Text;
                    HidexecById.Value = lb_createById.Value;
                    DataTable dtu = Epower.DevBase.Organization.SqlDAL.UserDP.GetUserInfoById(long.Parse(HidexecById.Value));
                    if (dtu.Rows.Count > 0)
                    {
                        lb_execByGH.Text = dtu.Rows[0]["LOGINNAME"].ToString();
                        HidexecByGP.Value = lb_execByGH.Text;
                        lb_execByPhone.Text = dtu.Rows[0]["MOBILE"].ToString();
                        HidexecByPhone.Value = lb_execByPhone.Text;
                        lb_execDeptname.Text = dtu.Rows[0]["DeptName"].ToString();
                        HiddeptName.Value = lb_execDeptname.Text;
                        HiddeptId.Value = dtu.Rows[0]["DeptId"].ToString();
                    }
                    Epower.DevBase.Organization.SqlDAL.UserEntity user = new UserEntity(Session["UserName"].ToString());
                    if (user.LoginName != "")
                    {
                        Lb_createPhone.Text = user.Mobile;//客户经理电话
                        long deptid = long.Parse(Session["UserDeptID"].ToString());//客户经理所在部门id
                        hd_deptId.Value = deptid.ToString();
                        lb_createDeptname.Text = Session["UserDeptName"].ToString();//部门名称

                    }

                    lb_createDate.Text = System.DateTime.Now.ToString();


                }

            #region 设置页面展示


            setFieldCollection setFields = oFlow.setFields;
            foreach (setField sf in setFields)
            {
                if (sf.Editable == true && sf.Visibled == true)
                {
                    continue;
                }
                switch (sf.Name.ToLower())
                {
                    case "address":
                        cblAddress.Enabled = false;
                        if (sf.Visibled == false)
                        {
                            cblAddress.Visible = false;
                        }
                        break;
                    case "appuser":
                        btn_execByname.Visible = false;
                        if (sf.Visibled == false)
                        {
                            Lb_execByName.Visible = false;
                            lb_execDeptname.Visible = false;
                            lb_execByGH.Visible = false;
                            lb_execByPhone.Visible = false;
                        }
                        break;
                    //基本信息
                    case "isbudan":
                        ddl_IsBudan.Enabled = false;
                        if (sf.Visibled == false)
                        {
                            ddl_IsBudan.Visible = false;
                        }
                        break;
                    case "jrdate":
                        ctr_JRdate.ContralState = eOA_FlowControlState.eReadOnly;
                        if (sf.Visibled == false)
                        {
                            ctr_JRdate.ContralState = eOA_FlowControlState.eHidden;
                        }
                        break;
                    case "outdate":
                        ctr_OutDate.ContralState = eOA_FlowControlState.eReadOnly;
                        if (sf.Visibled == false)
                        {
                            ctr_OutDate.ContralState = eOA_FlowControlState.eHidden;
                        }
                        break;
                    case "execbyuser"://进入人

                        Session["showdisplay2"] = "1";
                        Session["showdisplay1"] = "1";
                        //Session["showdisplay3"] = "1";

                       
                        break;
                    case "txjzname":
                        ctrTXJZname.ContralState = eOA_FlowControlState.eReadOnly;
                        ddlTXJZ.Enabled = false;
                        if (sf.Visibled == false)
                        {
                            ctrTXJZname.ContralState = eOA_FlowControlState.eHidden;
                            ddlTXJZ.Visible = false;
                        }
                        break;
                    case "isokscan":
                        ctrFlowIsOkScan.ContralState = eOA_FlowControlState.eReadOnly;
                        if (sf.Visibled == false)
                        {
                            ctrFlowIsOkScan.ContralState = eOA_FlowControlState.eHidden;
                        }
                        break;
                    case "actiontype":
                        ddlActionType.Enabled = false;
                        if (sf.Visibled == false)
                        {
                            ddlActionType.Visible = false;
                        }
                        break;
                    case "remark":
                        CtrFlowRemark1.ContralState = eOA_FlowControlState.eReadOnly;
                        if (sf.Visibled == false)
                        {
                            CtrFlowRemark1.ContralState = eOA_FlowControlState.eHidden;
                        }
                        break;

                    default:
                        break;

                }
            }
            #endregion


        }
        #endregion

        #region  提交前执行事件Master_myPreClickCustomize
        /// <summary>
        /// 提交前执行事件

        /// </summary>
        /// <returns></returns>
        bool Master_myPreClickCustomize(long lngActionID, string strActionName)
        {
            DateTime cblctr_JRdate = ctr_JRdate.dateTime;  //预计进入日期、时间

            DateTime cblctr_OutDate = ctr_OutDate.dateTime;//预计离开日期、时间

            if (cblctr_JRdate >= cblctr_OutDate)
            {
                PageTool.MsgBox(this.Page, "预计进入日期、时间：" + cblctr_JRdate.ToString() + "不能大于或等于预计离开日期、时间" + cblctr_OutDate.ToString() + "！");
                return false;
            }
            else
            {
                return true;
            }

        }
        #endregion

        #region 暂存时前面执行事件 Master_myPreSaveClickCustomize

        /// <summary>
        /// 暂存时前面执行事件

        /// </summary>
        /// <returns></returns>
        bool Master_myPreSaveClickCustomize()
        {
            return true;
        }
        #endregion








    }


}
