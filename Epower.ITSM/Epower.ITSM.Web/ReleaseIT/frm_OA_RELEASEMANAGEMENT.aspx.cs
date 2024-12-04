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
using Epower.ITSM.SqlDAL.Service;
using Epower.ITSM.Web.Controls;

namespace Epower.ITSM.Web.AppForms
{
    public partial class frm_OA_ReleaseManagement : BasePage
    {
        #region 变量区 
        private objFlow oFlow;
        private FlowForms myFlowForms;//母版页
        private long lngRequestID = 0;   //2009-04-28 增加 表示 公共请求的 ID

        private bool BLSQSOURCE
        {
            get {
                if (ViewState["BLSQSOURCE"] != null)
                    return (bool)ViewState["BLSQSOURCE"];
                return false;
            }
            set
            {
                ViewState["BLSQSOURCE"] = value;
            }
        }

        private bool BLAPPIMPLE
        {
            get
            { 
                if (ViewState["BLAPPIMPLE"] != null)
                    return (bool)ViewState["BLAPPIMPLE"];
                return false;
            }
            set
            {
                ViewState["BLAPPIMPLE"] = value;
            }
        }

        private bool BLSYSIMPLE
        {
            get
            { 
                if (ViewState["BLSYSIMPLE"] != null)
                    return (bool)ViewState["BLSYSIMPLE"];
                return false;
            }
            set
            {
                ViewState["BLSYSIMPLE"] = value;
            }
        }

        private bool BLEQUIPEIMPLE
        {
            get
            { 
                if (ViewState["BLEQUIPEIMPLE"] != null)
                    return (bool)ViewState["BLEQUIPEIMPLE"];
                return false;
            }
            set
            {
                ViewState["BLEQUIPEIMPLE"] = value;
            }
        }

        private bool BLNETIMPLE
        {
            get
            { 
                if (ViewState["BLNETIMPLE"] != null)
                    return (bool)ViewState["BLNETIMPLE"];
                return false;
            }
            set
            {
                ViewState["BLNETIMPLE"] = value;
            }
        }

        private bool BLOTHERIMPLE
        {
            get
            { 
                if (ViewState["BLOTHERIMPLE"] != null)
                    return (bool)ViewState["BLOTHERIMPLE"];
                return false;
            }
            set
            {
                ViewState["BLOTHERIMPLE"] = value;
            }
        }

        private bool BLPLANRELEASEDATE
        {
            get
            { 
                if (ViewState["BLPLANRELEASEDATE"] != null)
                    return (bool)ViewState["BLPLANRELEASEDATE"];
                return false;
            }
            set
            {
                ViewState["BLPLANRELEASEDATE"] = value;
            }
        }

        protected string ReleaseID
        {
            get { if (ViewState["ReleaseID"] != null) return ViewState["ReleaseID"].ToString(); else return "0"; }
            set { ViewState["ReleaseID"] = value; }
        }

        /// <summary>
        /// 取得流程FlowID
        /// </summary>
        public string FlowID
        {
            get
            {
                if (ViewState["frm_Issue_Base_FlowID"] != null)
                {
                    return ViewState["frm_Issue_Base_FlowID"].ToString();
                }
                else
                {
                    return "0";
                }
            }
            set
            {
                ViewState["frm_Issue_Base_FlowID"] = value;
            }
        }

        /// <summary>
        /// 应用id
        /// </summary>
        public string AppID
        {
            get
            {
                if (ViewState["frm_Issue_Base_AppID"] != null)
                {
                    return ViewState["frm_Issue_Base_AppID"].ToString();
                }
                else
                {
                    return "0";
                }
            }
            set
            {
                ViewState["frm_Issue_Base_AppID"] = value;
            }
        }
        /// <summary>
        /// 流程模型id
        /// </summary>
        public string FlowModelID
        {
            get
            {
                if (ViewState["frm_Issue_Base_FlowModelID"] != null)
                {
                    return ViewState["frm_Issue_Base_FlowModelID"].ToString();
                }
                else
                {
                    return "0";
                }
            }
            set
            {
                ViewState["frm_Issue_Base_FlowModelID"] = value;
            }
        }

        /// <summary>打印方式 0 IE方式，1 Report Service方式
        /// 
        /// </summary>
        public string PrintMode
        {
            get
            {
                return CommonDP.GetConfigValue("PrintMode", "PrintMode").ToString();
            }
        }

        #endregion

        /// <summary>页面加载
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            myFlowForms = (FlowForms)this.Master;
            myFlowForms.mySetContentReadOnly += new FlowForms.DoContentActions(Master_mySetContentVisible);  //设置页面只读
            myFlowForms.myGetFormsValue += new FlowForms.GetFormsValue(Master_myGetFormsValue);              //取得页面值
            myFlowForms.mySetFormsValue += new FlowForms.DoContentActions(Master_mySetFormsValue);           //设置页面值
            myFlowForms.myPreClickCustomize += new FlowForms.DoContentSubmitValid(Master_myPreClickCustomize);//
            myFlowForms.myPreSaveClickCustomize += new FlowForms.DoContentValid(Master_myPreSaveClickCustomize);//
            myFlowForms.blnSMSNotify = true;                                                                //屏蔽短信通知                                                              //屏蔽流程控制框
            myFlowForms.blnEmail = true;
            myFlowForms.blnShowFlowOP = true;
            InitPage();  //初始化页面数据
  
            if (!Page.IsPostBack)
            {
                //txtCTel.Text = ECustomerDP.GetUserPhone(long.Parse(Session["UserID"].ToString()));
            }
        }

        /// <summary>初始化页面数据
        /// 
        /// </summary>
        private void InitPage()
        {
            CtrVersionName.Attributes["onchange"] = "TransferValue();";
            CtrVersionName.Attributes.Add("onblur", "javascript:MaxLength(this,50,'输入内容超出限定长度：');");            
        }

        #region 流程方法与事件
        /// <summary>提交执行事件
        /// 
        /// </summary>
        /// <param name="lngActionID"></param>
        /// <param name="strActionName"></param>
        /// <returns></returns>
        private bool Master_myPreClickCustomize(long lngActionID, string strActionName)
        {

            return SaveDetailItem();
        }

        /// <summary>暂存
        /// 
        /// </summary>
        /// <returns></returns>
        private bool Master_myPreSaveClickCustomize()
        {
            return SaveDetailItem();
        }
        
        /// <summary>设置页面值与属性
        /// 
        /// </summary>
        private void Master_mySetFormsValue()
        {
            myFlowForms.FormTitle = "发布管理流程";
            #region Master_mySetFormsValue

            #region 打印时传入参数
            //打印时需要传入的参数
            this.FlowID = myFlowForms.oFlow.FlowID.ToString();
            this.FlowModelID = myFlowForms.oFlow.FlowModelID.ToString();
            this.AppID = myFlowForms.oFlow.AppID.ToString();
            #endregion 


            ImplDataProcess dp = new ImplDataProcess(myFlowForms.oFlow.AppID);
            DataSet ds = dp.GetFieldsDataSet(myFlowForms.oFlow.FlowID, myFlowForms.oFlow.OpID);
            DataTable dt = ds.Tables[0];

            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    ReleaseID = row["RMID"].ToString();//获取ID

                    CtrVersionName.Value = row["VERSIONNAME"].ToString();
                    CtrVersionCode.Value = row["VERSIONCODE"].ToString();
                    if (row["RELEASEDATE"].ToString() != "")
                        CtrDCreateRegTime.dateTime = DateTime.Parse(row["RELEASEDATE"].ToString());
                    ctrReleaseScope.CatelogID = long.Parse(row["RELEASESCOPEID"].ToString());
                    //UserPicker1.UserID = long.Parse(row["RELEASEPERSONID"].ToString());
                    //UserPicker1.UserName = row["RELEASEPERSONNAME"].ToString();
                    hidCustID.Value = row["RELEASEPERSONID"].ToString();
                    labCustAddr.Text = txtCustAddr.Text = row["RELEASEPERSONNAME"].ToString();

                    ctrVersionKind.CatelogID = long.Parse(row["VERSIONKINDID"].ToString());
                    ctrVersionType.CatelogID = long.Parse(row["VERSIONTYPEID"].ToString());

                    txtContent.Text = row["RELEASECONTENT"].ToString();
                    lblContent.Text = row["RELEASECONTENT"].ToString();

                    //CtrPhone.Value = row["RELEASEPHONE"].ToString();
                    labCTel.Text = txtCTel.Text = row["RELEASEPHONE"].ToString();
                }
                else
                {

                    txtCustAddr.Text = Session["UserDefaultCustomerName"].ToString();
                    hidCustID.Value = Session["UserDefaultCustomerID"].ToString();

                    if (hidCustID.Value != "0" && hidCustID.Value != "")
                    {
                        //取得客户资料
                        Br_ECustomerDP ec = new Br_ECustomerDP();
                        ec = ec.GetReCorded(long.Parse(hidCustID.Value));
                        hidCustID.Value = ec.ID.ToString();
                        hidCust.Value = ec.ShortName;
                        txtCustAddr.Text = ec.ShortName;
                        txtCTel.Text = ec.Tel1;
                        labCTel.Text = ec.Tel1;
                    }
                }
                
            }
            #endregion

            #region set visible
            setFieldCollection setFields = myFlowForms.oFlow.setFields;

            foreach (setField sf in setFields)
            {
                if (sf.Editable == true && sf.Visibled == true)
                {
                    continue;
                }
                switch (sf.Name.ToUpper ())
                {
                    case "VERSIONNAME":
                        CtrVersionName.ContralState = eOA_FlowControlState.eReadOnly;
                        if (!sf.Visibled)
                            CtrVersionName.ContralState = eOA_FlowControlState.eHidden;
                        break;
                    case "VERSIONCODE":
                        CtrVersionCode.ContralState = eOA_FlowControlState.eReadOnly;
                        if (!sf.Visibled)
                            CtrVersionCode.ContralState = eOA_FlowControlState.eHidden;
                        break;
                    case "RELEASEDATE":
                        CtrDCreateRegTime.ContralState = eOA_FlowControlState.eReadOnly;
                        if (!sf.Visibled)
                            CtrDCreateRegTime.ContralState = eOA_FlowControlState.eHidden;
                        break;
                    case "RELEASESCOPENAME":
                        ctrReleaseScope.ContralState = eOA_FlowControlState.eReadOnly;
                        if (!sf.Visibled)
                            ctrReleaseScope.ContralState = eOA_FlowControlState.eHidden;
                        break;
                    case "RELEASEPERSONNAME":
                        txtCustAddr.Visible = false;
                        cmdCust.Visible = false;
                        if ( sf.Visibled )
                        {
                            labCustAddr.Visible = true;
                        }

                        break;
                    case "VERSIONKINDNAME":
                        ctrVersionKind.ContralState = eOA_FlowControlState.eReadOnly;
                        if (!sf.Visibled)
                            ctrVersionKind.ContralState = eOA_FlowControlState.eHidden;
                        break;
                    case "VERSIONTYPENAME":
                        ctrVersionType.ContralState = eOA_FlowControlState.eReadOnly;
                        if (!sf.Visibled)
                            ctrVersionType.ContralState = eOA_FlowControlState.eHidden;
                        break;
                    case "RELEASECONTENT":
                        txtContent.Visible = false;
                        if (sf.Visibled == true)
                            lblContent.Visible = true;
                        break;
                    case "RELEASEPHONE":

                        txtCTel.Visible = false;
                        if ( sf.Visibled)
                            labCTel.Visible = true; 
                        break;

                    default:
                        break;
                }
            }

            BindGrid(long.Parse(ReleaseID), setFields);
            #endregion
        }

        /// <summary>设置页面只读
        /// 
        /// </summary>
        private void Master_mySetContentVisible()
        {
            if (CtrVersionName.ContralState != eOA_FlowControlState.eHidden) //
                CtrVersionName.ContralState = eOA_FlowControlState.eReadOnly;

            if (CtrVersionCode.ContralState != eOA_FlowControlState.eHidden)
                CtrVersionCode.ContralState = eOA_FlowControlState.eReadOnly;

            if (CtrDCreateRegTime.ContralState != eOA_FlowControlState.eHidden)
                CtrDCreateRegTime.ContralState = eOA_FlowControlState.eReadOnly;

            if (ctrReleaseScope.ContralState != eOA_FlowControlState.eHidden)
                ctrReleaseScope.ContralState = eOA_FlowControlState.eReadOnly;

            if (txtCustAddr.Visible == true)
                labCustAddr.Visible = true;
            txtCustAddr.Visible = false;
            cmdCust.Visible = false;

            if (ctrVersionKind.ContralState != eOA_FlowControlState.eHidden)
                ctrVersionKind.ContralState = eOA_FlowControlState.eReadOnly;

            if (ctrVersionType.ContralState != eOA_FlowControlState.eHidden)
                ctrVersionType.ContralState = eOA_FlowControlState.eReadOnly; 
          
            if (txtContent.Visible == true)//具体内容
                lblContent.Visible = true;
            txtContent.Visible = false;
 
            if (txtCTel.Visible == true)
                labCTel.Visible = true;
            txtCTel.Visible = false;

            SetFareDetailReadOnly();
        }
        
        /// <summary>取得页面值
        /// 
        /// </summary>
        /// <param name="lngActionID"></param>
        /// <param name="strActionName"></param>
        /// <returns></returns>
        private XmlDocument Master_myGetFormsValue(long lngActionID, string strActionName)
        {
            FieldValues fv = new FieldValues();
            fv.Add("RMID", ReleaseID );
            fv.Add ("VERSIONNAME", CtrVersionName .Value.Trim ());
            fv.Add ("VERSIONCODE", CtrVersionCode.Value.Trim ());
            fv.Add ("RELEASEDATE",CtrDCreateRegTime.dateTime.ToString().Trim() );
            fv.Add ("RELEASESCOPEID", ctrReleaseScope.CatelogID .ToString());
            fv.Add ("RELEASESCOPENAME", ctrReleaseScope.CatelogValue .ToString ());
            fv.Add ("REGUSERID",Session["UserID"].ToString() );
            fv.Add("REGUSERNAME", Session["PersonName"].ToString());
            fv.Add ("REGDEPTID",Session["UserDeptID"].ToString());
            fv.Add ("REGDEPTNAME", Session["UserDeptName"].ToString ());

            fv.Add("RELEASEPERSONID", hidCustID.Value.ToString () );
            fv.Add("RELEASEPERSONNAME",txtCustAddr .Text .ToString () );

            fv.Add ("VERSIONKINDID", ctrVersionKind.CatelogID.ToString () );
            fv.Add ("VERSIONKINDNAME", ctrVersionKind .CatelogValue .ToString () );
            fv.Add ("VERSIONTYPEID", ctrVersionType .CatelogID .ToString ());
            fv.Add ("VERSIONTYPENAME", ctrVersionType.CatelogValue .ToString () );
            fv.Add ("RELEASECONTENT",  txtContent.Text );

            fv.Add ("RELEASEPHONE", txtCTel .Text.ToString () );

            fv.Add("REGORGID", Session["UserOrgID"].ToString());

            fv.Add("EmailNotify", myFlowForms.EmailValue.ToString());
            fv.Add("SMSNotify", myFlowForms.NotifyValue.ToString());

            XmlDocument xmlDoc = fv.GetXmlObject();
            return xmlDoc;
        }
        #endregion

        #region grid操作

        /// <summary>设置子版本信息为只读
        /// 
        /// </summary>
        private void SetFareDetailReadOnly()
        {
            foreach (DataGridItem row in gvBillItem.Items)
            {

                ((CtrDateAndTime)row.FindControl("CtrSUGGESTDATE")).Visible = false;
                     

                ((TextBox)row.FindControl("txtVERSIONCODE")).Visible = false;
                ((TextBox)row.FindControl("txtRESPONSIBLEPERSON")).Visible = false;
                ((TextBox)row.FindControl("txtAPPIMPLE")).Visible = false;
                ((TextBox)row.FindControl("txtSYSIMPLE")).Visible = false;
                ((TextBox)row.FindControl("txtEQUIPEIMPLE")).Visible = false;
                ((TextBox)row.FindControl("txtNETIMPLE")).Visible = false;
                ((TextBox)row.FindControl("txtOTHERIMPLE")).Visible = false;
                ((TextBox)row.FindControl("txtPLANRELEASEDATE")).Visible = false;
                //((CtrDateAndTime)row.FindControl("CtrPLANRELEASEDATE")).Visible = false;

                ((Label)row.FindControl("lblSUGGESTDATE")).Visible = true;
                ((Label)row.FindControl("lblVERSIONCODE")).Visible = true;
                ((Label)row.FindControl("lblRESPONSIBLEPERSON")).Visible = true;
                ((Label)row.FindControl("lblAPPIMPLE")).Visible = true;
                ((Label)row.FindControl("lblSYSIMPLE")).Visible = true;
                ((Label)row.FindControl("lblEQUIPEIMPLE")).Visible = true;
                ((Label)row.FindControl("lblNETIMPLE")).Visible = true;
                ((Label)row.FindControl("lblOTHERIMPLE")).Visible = true;
                ((Label)row.FindControl("lblPLANRELEASEDATE")).Visible = true; 



                //gvBillItem.Columns[0].Visible = false;
                gvBillItem.Columns[gvBillItem.Columns.Count - 1].Visible = false;
            }
            gvBillItem.ShowFooter = false;
        }

        /// <summary>绑定grid的值，即版本投产要求
        /// 
        /// </summary>
        /// <param name="id"></param>        
        private bool SaveDetailItem()
        {
            long lngID = 0;
            try
            {
                DataTable dt = GetDetailItem(true);

                lngID = ReleaseManagementDP.SaveSubVersionItem (dt, long.Parse (ReleaseID )); 
                if (lngID == 0)
                    return false;
                else
                    ReleaseID  = lngID.ToString(); //带回到处理类中
                return true;
            }
            catch //(Exception e)
            {
                return false;
            } 
        }

        /// <summary>获取子版本数据
        /// /// 
        /// </summary>
        /// <param name="isAll"></param>
        /// <returns></returns>
        private DataTable GetDetailItem(bool isAll)
        {
            DataTable dt = (DataTable)ViewState["ItemData"];
            dt.Rows.Clear();
            int iCostID = 0;
            DataRow dr;

            foreach (DataGridItem row in gvBillItem.Controls[0].Controls)
            {
                iCostID++;
                if (row.ItemType == ListItemType.Footer)
                {
                    //string aProductID = ((HtmlInputHidden)row.FindControl("hidAddDFareCode")).Value; //
                    if (isAll)
                        continue;
                    //string SUGGESTDATE = ((TextBox)row.FindControl("txtSUGGESTDATEAdd")).Text;
                    string SUGGESTDATE = ((CtrDateAndTime)row.FindControl("CtrSUGGESTDATEAdd")).dateTime.ToShortDateString();
                    string VERSIONCODE = ((TextBox)row.FindControl("txtVERSIONCODEAdd")).Text;
                    string RESPONSIBLEPERSON = ((TextBox)row.FindControl("txtRESPONSIBLEPERSONAdd")).Text;
                    string APPIMPLE = ((TextBox)row.FindControl("txtAPPIMPLEAdd")).Text;
                    string SYSIMPLE = ((TextBox)row.FindControl("txtSYSIMPLEAdd")).Text;
                    string EQUIPEIMPLE = ((TextBox)row.FindControl("txtEQUIPEIMPLEAdd")).Text;
                    string NETIMPLE = ((TextBox)row.FindControl("txtNETIMPLEAdd")).Text;
                    string OTHERIMPLE = ((TextBox)row.FindControl("txtOTHERIMPLEAdd")).Text;
                    string PLANRELEASEDATE = ((TextBox)row.FindControl("txtPLANRELEASEDATEAdd")).Text;
                    //string PLANRELEASEDATE = ((CtrDateAndTime)row.FindControl("CtrPLANRELEASEDATEAdd")).dateTime.ToShortDateString ();
         
                    if (string.IsNullOrEmpty(SUGGESTDATE) || string.IsNullOrEmpty(VERSIONCODE) || string.IsNullOrEmpty(RESPONSIBLEPERSON))
                        continue;
                    dr = dt.NewRow();
                    dr["SMSID"] = ReleaseID;
                    dr["SUGGESTDATE"] = SUGGESTDATE;
                    dr["VERSIONCODE"] = VERSIONCODE;
                    dr["RESPONSIBLEPERSON"] = RESPONSIBLEPERSON;
                    dr["APPIMPLE"] = APPIMPLE;
                    dr["SYSIMPLE"] = SYSIMPLE;
                    dr["EQUIPEIMPLE"] = EQUIPEIMPLE;
                    dr["NETIMPLE"] = NETIMPLE;
                    dr["OTHERIMPLE"] = OTHERIMPLE;
                    dr["PLANRELEASEDATE"] = PLANRELEASEDATE;
                    dt.Rows.Add(dr);
                }
                else if (row.ItemType == ListItemType.Item || row.ItemType == ListItemType.AlternatingItem)
                {
                    //string ProductID = ((HtmlInputHidden)row.FindControl("hidDFareCode")).Value; //id
                    //string SUGGESTDATE = ((TextBox)row.FindControl("txtSUGGESTDATE")).Text;
                    string SUGGESTDATE = ((CtrDateAndTime)row.FindControl("CtrSUGGESTDATE")).dateTime.ToShortDateString();
                    string VERSIONCODE = ((TextBox)row.FindControl("txtVERSIONCODE")).Text;
                    string RESPONSIBLEPERSON = ((TextBox)row.FindControl("txtRESPONSIBLEPERSON")).Text;
                    string APPIMPLE = ((TextBox)row.FindControl("txtAPPIMPLE")).Text;
                    string SYSIMPLE = ((TextBox)row.FindControl("txtSYSIMPLE")).Text;
                    string EQUIPEIMPLE = ((TextBox)row.FindControl("txtEQUIPEIMPLE")).Text;
                    string NETIMPLE = ((TextBox)row.FindControl("txtNETIMPLE")).Text;
                    string OTHERIMPLE = ((TextBox)row.FindControl("txtOTHERIMPLE")).Text;
                    string PLANRELEASEDATE = ((TextBox)row.FindControl("txtPLANRELEASEDATE")).Text;
                    //string PLANRELEASEDATE = ((CtrDateAndTime)row.FindControl("CtrPLANRELEASEDATE")).dateTime.ToShortDateString ();

                    dr = dt.NewRow();
                    dr["SMSID"] = ReleaseID;
                    dr["SUGGESTDATE"] = SUGGESTDATE;
                    dr["VERSIONCODE"] = VERSIONCODE;
                    dr["RESPONSIBLEPERSON"] = RESPONSIBLEPERSON;
                    dr["APPIMPLE"] = APPIMPLE;
                    dr["SYSIMPLE"] = SYSIMPLE;
                    dr["EQUIPEIMPLE"] = EQUIPEIMPLE;
                    dr["NETIMPLE"] = NETIMPLE;
                    dr["OTHERIMPLE"] = OTHERIMPLE;
                    dr["PLANRELEASEDATE"] = PLANRELEASEDATE;
                    dt.Rows.Add(dr);
                }
            }
            ViewState["ItemData"] = dt;

            //注意这里同时　重新计算合计金额
            //labTotalAmount.Text = dTotal.ToString();
            return dt;
        }  

        private void BindGrid(long id ,setFieldCollection setFields )
        {
            DataTable dtItem = ReleaseManagementDP.GetCLFareItem(id);
            ViewState["ItemData"] = dtItem;
            gvBillItem.DataSource = dtItem;

            gvBillItem.DataBind();
            gvBillItem.Visible = true;

            DataRow dr;
            for (int i = 0; i < gvBillItem.Items.Count; i++)
            {
                dr = dtItem.Rows[i];
                ((CtrDateAndTime)gvBillItem.Items[i].Cells[1].FindControl("CtrSUGGESTDATE")).dateTime = Convert.ToDateTime(DateTime.Parse(dr["SUGGESTDATE"].ToString()).ToShortDateString());
            }
            foreach (setField sf in setFields)
            {
                if (sf.Editable == true && sf.Visibled == true)
                {
                    continue;
                }
                 switch (sf.Name.ToUpper ())
                {
                    case "SQSOURCE":
                        BLSQSOURCE = true;
                        if (!sf.Visibled)
                        {
                            foreach (DataGridItem row in gvBillItem.Items)
                            {
                                ((Label)row.FindControl("lblSUGGESTDATE")).Text = "--";
                                ((Label)row.FindControl("lblVERSIONCODE")).Text = "--";
                                ((Label)row.FindControl("lblRESPONSIBLEPERSON")).Text = "--";
                            }
                        }
                        break;
                    case "APPIMPLE":
                        BLAPPIMPLE = true;
                       
                        if (!sf.Visibled)
                        {
                            foreach (DataGridItem row in gvBillItem.Items)
                            {
                                ((Label)row.FindControl("lblAPPIMPLE")).Text = "--";
                            }
                        }
                        break;
                    case "SYSIMPLE":
                        BLSYSIMPLE = true;
                        if (!sf.Visibled)
                        {
                            foreach (DataGridItem row in gvBillItem.Items)
                            {
                                ((Label)row.FindControl("lblSYSIMPLE")).Text = "--";
                            }
                        }
                        break;
                    case "EQUIPEIMPLE":
                        BLEQUIPEIMPLE = true;
                        if (!sf.Visibled)
                        {
                            foreach (DataGridItem row in gvBillItem.Items)
                            {
                                ((Label)row.FindControl("lblEQUIPEIMPLE")).Text = "--";
                            }
                        }
                        break;
                    case "NETIMPLE":
                        BLNETIMPLE = true;
                        if (!sf.Visibled)
                        {
                            foreach (DataGridItem row in gvBillItem.Items)
                            {
                                ((Label)row.FindControl("lblNETIMPLE")).Text = "--";
                            }
                        }
                        break;
                    case "OTHERIMPLE":
                        BLOTHERIMPLE = true;
                        if (!sf.Visibled)
                        {
                            foreach (DataGridItem row in gvBillItem.Items)
                            {
                                ((Label)row.FindControl("lblOTHERIMPLE")).Text = "--";
                            }
                        }
                        break;
                    case "PLANRELEASEDATE":
                        BLPLANRELEASEDATE = true;
                        if (!sf.Visibled)
                        {
                            foreach (DataGridItem row in gvBillItem.Items)
                            {
                                ((Label)row.FindControl("lblPLANRELEASEDATE")).Text = "--";
                            }
                        }
                        break;
                    case "ADD":
                        gvBillItem.ShowFooter = false;
                        gvBillItem.Columns[gvBillItem.Columns.Count - 1].Visible = false;
                        break; 
                    default:
                        break;
                }
            }
            VisibleGrid();
        }

        private void VisibleGrid()
        {
            if (BLSQSOURCE)
            {
                foreach (DataGridItem row in gvBillItem.Items)
                {
                    //((TextBox)row.FindControl("txtSUGGESTDATE")).Visible = false;
                    ((CtrDateAndTime)row.FindControl("CtrSUGGESTDATE")).Visible = false;

                    ((TextBox)row.FindControl("txtVERSIONCODE")).Visible = false;
                    ((TextBox)row.FindControl("txtRESPONSIBLEPERSON")).Visible = false;

                    ((Label)row.FindControl("lblSUGGESTDATE")).Visible = true;
                    ((Label)row.FindControl("lblVERSIONCODE")).Visible = true;
                    ((Label)row.FindControl("lblRESPONSIBLEPERSON")).Visible = true;
                }
                foreach (DataGridItem row in gvBillItem.Controls[0].Controls)
                {
                    if (row.ItemType == ListItemType.Footer)
                    {
                        row.FindControl("CtrSUGGESTDATEAdd").Visible = false;
                        row.FindControl("txtVERSIONCODEAdd").Visible = false;
                        row.FindControl("txtRESPONSIBLEPERSONAdd").Visible = false;
                    }
                }
                
            }
            if (BLAPPIMPLE)
            {
                foreach (DataGridItem row in gvBillItem.Items)
                {
                    ((TextBox)row.FindControl("txtAPPIMPLE")).Visible = false;
                    ((Label)row.FindControl("lblAPPIMPLE")).Visible = true;
                }
                foreach (DataGridItem row in gvBillItem.Controls[0].Controls)
                {
                    if (row.ItemType == ListItemType.Footer)
                    {
                        row.FindControl("txtAPPIMPLEAdd").Visible = false;
                    }
                }
            }
            if (BLSYSIMPLE)
            {
                foreach (DataGridItem row in gvBillItem.Items)
                {
                    ((TextBox)row.FindControl("txtSYSIMPLE")).Visible = false;
                    ((Label)row.FindControl("lblSYSIMPLE")).Visible = true;
                }
                foreach (DataGridItem row in gvBillItem.Controls[0].Controls)
                {
                    if (row.ItemType == ListItemType.Footer)
                    {
                        row.FindControl("txtSYSIMPLEAdd").Visible = false;
                    }
                }
            }
            if (BLEQUIPEIMPLE)
            {
                foreach (DataGridItem row in gvBillItem.Items)
                {
                    ((TextBox)row.FindControl("txtEQUIPEIMPLE")).Visible = false;
                    ((Label)row.FindControl("lblEQUIPEIMPLE")).Visible = true;
                }
                foreach (DataGridItem row in gvBillItem.Controls[0].Controls)
                {
                    if (row.ItemType == ListItemType.Footer)
                    {
                        row.FindControl("txtEQUIPEIMPLEAdd").Visible = false;
                    }
                }
            }
            if (BLNETIMPLE)
            {
                foreach (DataGridItem row in gvBillItem.Items)
                {
                    ((TextBox)row.FindControl("txtNETIMPLE")).Visible = false;
                    ((Label)row.FindControl("lblNETIMPLE")).Visible = true;
                }
                foreach (DataGridItem row in gvBillItem.Controls[0].Controls)
                {
                    if (row.ItemType == ListItemType.Footer)
                    {
                        row.FindControl("txtNETIMPLEAdd").Visible = false;
                    }
                }
            }
            if (BLOTHERIMPLE)
            {
                foreach (DataGridItem row in gvBillItem.Items)
                {
                    ((TextBox)row.FindControl("txtOTHERIMPLE")).Visible = false;
                    ((Label)row.FindControl("lblOTHERIMPLE")).Visible = true;
                }
                foreach (DataGridItem row in gvBillItem.Controls[0].Controls)
                {
                    if (row.ItemType == ListItemType.Footer)
                    {
                        row.FindControl("txtOTHERIMPLEAdd").Visible = false;
                    }
                }
            }
            if (BLPLANRELEASEDATE)
            {
                foreach (DataGridItem row in gvBillItem.Items)
                {
                    ((TextBox)row.FindControl("txtPLANRELEASEDATE")).Visible = false;
                    //((CtrDateAndTime)row.FindControl("CtrPLANRELEASEDATE")).Visible = false;
                    ((Label)row.FindControl("lblPLANRELEASEDATE")).Visible = true;
                }
                foreach (DataGridItem row in gvBillItem.Controls[0].Controls)
                {
                    if (row.ItemType == ListItemType.Footer)
                    {
                        //row.FindControl("CtrPLANRELEASEDATEAdd").Visible = false;
                        row.FindControl("txtPLANRELEASEDATEAdd").Visible = false;
                    }
                }
            }
        }
        protected void gvBillItem_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            DataTable dt = new DataTable();
            if (e.CommandName == "Delete")
            {
                dt = GetDetailItem(true);
                dt.Rows.RemoveAt(e.Item.ItemIndex);
                gvBillItem.DataSource = dt.DefaultView;
                gvBillItem.DataBind();
                VisibleGrid();
            }
            else if (e.CommandName == "Add")
            {
                dt = GetDetailItem(false);
                gvBillItem.DataSource = dt.DefaultView;
                gvBillItem.DataBind();
                VisibleGrid();
            }
            //重新计算
            //注意这里同时　重新计算合计金额
            //labTotalAmount.Text = GetDetailTotalAmount().ToString("N");
        }

        protected void gvBillItem_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
        }
        #endregion 
    }
}
