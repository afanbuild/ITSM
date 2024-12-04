/****************************************************************************
 * 
 * description:通用流程应用表单
 * 
 * 
 * 
 * Create by:
 * Create Date:2010-01-07
 * *************************************************************************/
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Xml;
using System.IO;
using EpowerCom;
using EpowerGlobal;
using appDataProcess;
using Epower.DevBase.BaseTools;
using Epower.DevBase.Organization.SqlDAL;
using Epower.DevBase.Organization.Base;
using Epower.ITSM.SqlDAL;
using Epower.ITSM.Base;
using Epower.ITSM.Web.Controls;

namespace Epower.ITSM.Web.AppForms
{
	/// <summary>
    /// app_pub_normal_tabform 的摘要说明。
	/// </summary>
    public partial class app_pub_normal_tabform : BasePage
    {
        #region 变量声明
        private FlowFormsTab myFlowForms;
        #endregion

        #region lngFlowModelID
        /// <summary>
        /// 
        /// </summary>
        protected long lngFlowModelID
        {
            get
            {
                return myFlowForms.lngFlowModelID;
            }
        }
        #endregion

        #region lngMessageID
        /// <summary>
        /// 
        /// </summary>
        protected long lngMessageID
        {
            get
            {
                return myFlowForms.lngMessageID;
            }
        }
        #endregion 

        #region Page_Load 调用的函数
        /// <summary>
        /// 
        /// </summary>
        private void InitClientScript()
        {
            if (!IsPostBack)
            {
                txtFlowName.Attributes["onchange"] = "TransferValue();";

                //页面输入太长控制
                //页面输入太长控制
                txtFlowName.Attributes.Add("onblur", "javascript:MaxLength(this,100,'输入标题名称长度超出限定长度：');");
            }
        }

        #endregion

        #region 页面初始化 Page_Load
        /// <summary>
        /// 页面初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, System.EventArgs e)
        {
            myFlowForms = (FlowFormsTab)this.Master;
            myFlowForms.mySetContentReadOnly += new FlowFormsTab.DoContentActions(Master_mySetContentReadOnly);
            myFlowForms.myGetFormsValue += new FlowFormsTab.GetFormsValue(Master_myGetFormsValue);
            myFlowForms.mySetFormsValue += new FlowFormsTab.DoContentActions(Master_mySetFormsValue);
            myFlowForms.blnSMSNotify = false;   //屏蔽短信通知
            //myFlowForms.blnShowFlowOP = false;  //屏蔽流程控制框

            

            //防止用户通过IE后退按纽重复提交
            Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
            InitClientScript();
        }
        #endregion

        #region 设置页面值 Master_mySetFormsValue
        /// <summary>
        /// 设置页面值
        /// </summary>
        private void Master_mySetFormsValue()
        {
            #region Master_mySetFormsValue
            InitInterface();
            #endregion 
        }
        #endregion

        #region 设置只读性 Master_mySetContentReadOnly
        /// <summary>
        /// 
        /// </summary>
        private void Master_mySetContentReadOnly()
        {
            labFlowName.Visible = true;
            txtFlowName.Visible = false;
            rFlowName.Visible = false;

            if (Ctrdateandtime1.ContralState != eOA_FlowControlState.eHidden)
                Ctrdateandtime1.ContralState = eOA_FlowControlState.eReadOnly;

            if (Ctrdateandtime2.ContralState != eOA_FlowControlState.eHidden)
                Ctrdateandtime2.ContralState = eOA_FlowControlState.eReadOnly;

            if (Ctrdateandtime3.ContralState != eOA_FlowControlState.eHidden)
                Ctrdateandtime3.ContralState = eOA_FlowControlState.eReadOnly;

            if (Ctrdateandtime4.ContralState != eOA_FlowControlState.eHidden)
                Ctrdateandtime4.ContralState = eOA_FlowControlState.eReadOnly;

            if (CtrFlowFormText1.ContralState != eOA_FlowControlState.eHidden)
                CtrFlowFormText1.ContralState = eOA_FlowControlState.eReadOnly;

            if (CtrFlowFormText2.ContralState != eOA_FlowControlState.eHidden)
                CtrFlowFormText2.ContralState = eOA_FlowControlState.eReadOnly;

            if (CtrFlowFormText3.ContralState != eOA_FlowControlState.eHidden)
                CtrFlowFormText3.ContralState = eOA_FlowControlState.eReadOnly;

            if (CtrFlowFormText4.ContralState != eOA_FlowControlState.eHidden)
                CtrFlowFormText4.ContralState = eOA_FlowControlState.eReadOnly;

            if (CtrFlowFormText5.ContralState != eOA_FlowControlState.eHidden)
                CtrFlowFormText5.ContralState = eOA_FlowControlState.eReadOnly;

            if (CtrFlowFormText6.ContralState != eOA_FlowControlState.eHidden)
                CtrFlowFormText6.ContralState = eOA_FlowControlState.eReadOnly;

            if (CtrFlowFormText7.ContralState != eOA_FlowControlState.eHidden)
                CtrFlowFormText7.ContralState = eOA_FlowControlState.eReadOnly;

            if (CtrFlowFormText8.ContralState != eOA_FlowControlState.eHidden)
                CtrFlowFormText8.ContralState = eOA_FlowControlState.eReadOnly;

            if (CtrFlowNumeric1.ContralState != eOA_FlowControlState.eHidden)
                CtrFlowNumeric1.ContralState = eOA_FlowControlState.eReadOnly;

            if (CtrFlowNumeric2.ContralState != eOA_FlowControlState.eHidden)
                CtrFlowNumeric2.ContralState = eOA_FlowControlState.eReadOnly;

            if (CtrFlowNumeric3.ContralState != eOA_FlowControlState.eHidden)
                CtrFlowNumeric3.ContralState = eOA_FlowControlState.eReadOnly;

            if (CtrFlowNumeric4.ContralState != eOA_FlowControlState.eHidden)
                CtrFlowNumeric4.ContralState = eOA_FlowControlState.eReadOnly;

            if (CtrFlowNumeric5.ContralState != eOA_FlowControlState.eHidden)
                CtrFlowNumeric5.ContralState = eOA_FlowControlState.eReadOnly;

            if (CtrFCate1.ContralState != eOA_FlowControlState.eHidden)
                CtrFCate1.ContralState = eOA_FlowControlState.eReadOnly;

            if (CtrFCate2.ContralState != eOA_FlowControlState.eHidden)
                CtrFCate2.ContralState = eOA_FlowControlState.eReadOnly;

            if (CtrFCate3.ContralState != eOA_FlowControlState.eHidden)
                CtrFCate3.ContralState = eOA_FlowControlState.eReadOnly;

            if (CtrFCate4.ContralState != eOA_FlowControlState.eHidden)
                CtrFCate4.ContralState = eOA_FlowControlState.eReadOnly;

            if (CtrFCate5.ContralState != eOA_FlowControlState.eHidden)
                CtrFCate5.ContralState = eOA_FlowControlState.eReadOnly;

            chkBool1.Enabled = false;
            chkBool2.Enabled = false;
            chkBool3.Enabled = false;
            chkBool4.Enabled = false;

            if (CtrFlowRemark1.ContralState != eOA_FlowControlState.eHidden)
                CtrFlowRemark1.ContralState = eOA_FlowControlState.eReadOnly;

            lblDesc.Visible = true;
            ftxtDesc.Visible = false;
        }
        #endregion 

        #region 取得页面值生成XML Master_myGetFormsValue
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private XmlDocument Master_myGetFormsValue(long lngActionID, string strActionName)
        {
            FieldValues fv = new FieldValues();

            fv.Add("flowname", txtFlowName == null ? labFlowName.Text.Trim() : txtFlowName.Text.Trim());
            fv.Add("startdate", labStartDate.Text.Trim());
            fv.Add("applyname", labApplyName.Text.Trim());
            fv.Add("deptname", labDeptName.Text.Trim());
            fv.Add("date1", Ctrdateandtime1.dateTime.ToString());
            fv.Add("date2", Ctrdateandtime2.dateTime.ToString());
            fv.Add("date3", Ctrdateandtime3.dateTime.ToString());
            fv.Add("date4", Ctrdateandtime4.dateTime.ToString());
            fv.Add("string1", CtrFlowFormText1.Value.Trim());
            fv.Add("string2", CtrFlowFormText2.Value.Trim());
            fv.Add("string3", CtrFlowFormText3.Value.Trim());
            fv.Add("string4", CtrFlowFormText4.Value.Trim());
            fv.Add("string5", CtrFlowFormText5.Value.Trim());
            fv.Add("string6", CtrFlowFormText6.Value.Trim());
            fv.Add("string7", CtrFlowFormText7.Value.Trim());
            fv.Add("string8", CtrFlowFormText8.Value.Trim());

            fv.Add("number1", CtrFlowNumeric1.Value.Trim());
            fv.Add("number2", CtrFlowNumeric2.Value.Trim());
            fv.Add("number3", CtrFlowNumeric3.Value.Trim());
            fv.Add("number4", CtrFlowNumeric4.Value.Trim());
            fv.Add("number5", CtrFlowNumeric5.Value.Trim());

            if (CtrFCate1.CatelogID != -1)
            {
                fv.Add("cate1", CtrFCate1.CatelogID.ToString());
                fv.Add("CateValue1", CtrFCate1.CatelogValue);
            }
            else
                fv.Add("cate1", ViewState["Field_Cate1"] != null ? ViewState["Field_Cate1"].ToString() : "");

            if (CtrFCate2.CatelogID != -1)
            {
                fv.Add("cate2", CtrFCate2.CatelogID.ToString());
                fv.Add("CateValue2", CtrFCate2.CatelogValue);
            }
            else
                fv.Add("cate2", ViewState["Field_Cate2"] != null ? ViewState["Field_Cate2"].ToString() : "");

            if (CtrFCate3.CatelogID != -1)
            {
                fv.Add("cate3", CtrFCate3.CatelogID.ToString());
                fv.Add("CateValue3", CtrFCate3.CatelogValue);
            }
            else
                fv.Add("cate3", ViewState["Field_Cate3"] != null ? ViewState["Field_Cate3"].ToString() : "");

            if (CtrFCate4.CatelogID != -1)
            {
                fv.Add("cate4", CtrFCate4.CatelogID.ToString());
                fv.Add("CateValue4", CtrFCate4.CatelogValue);
            }
            else
                fv.Add("cate4", ViewState["Field_Cate4"] != null ? ViewState["Field_Cate4"].ToString() : "");

            if (CtrFCate5.CatelogID != -1)
            {
                fv.Add("cate5", CtrFCate5.CatelogID.ToString());
                fv.Add("CateValue5", CtrFCate5.CatelogValue);
            }
            else
                fv.Add("cate5", ViewState["Field_Cate5"] != null ? ViewState["Field_Cate5"].ToString() : "");

            fv.Add("bool1", chkBool1.Checked == false ? "0" : "1");
            fv.Add("bool2", chkBool2.Checked == false ? "0" : "1");
            fv.Add("bool3", chkBool3.Checked == false ? "0" : "1");
            fv.Add("bool4", chkBool4.Checked == false ? "0" : "1");

            fv.Add("remark1", CtrFlowRemark1.Value.Trim());
            fv.Add("applyid", Session["UserID"].ToString());
            fv.Add("deptid", Session["UserDeptID"].ToString());

            fv.Add("description", ftxtDesc.Text.Trim());  //复杂表单
            //fv.Add("description", lblDesc.Text.Trim());  //复杂表单

            XmlDocument xmlDoc = fv.GetXmlObject();       

            return xmlDoc;
        }
        #endregion 

        #region 初始化页面显示数据 InitInterface
        /// <summary>
        /// 初始化界面，MessageID 为0 则肯定是起草流程，
        /// 
        /// </summary>
        private void InitInterface()
        {
            #region 获取表单数据
            objFlow oFlow = myFlowForms.oFlow;
            myFlowForms.FormTitle = oFlow.FlowName;
            DataTable dt = null;
            if (oFlow.MessageID != 0)
            {
               
                ImplDataProcess dp = new ImplDataProcess(oFlow.AppID);
                DataSet ds = dp.GetFieldsDataSet(oFlow.FlowID, oFlow.OpID);
                dt = ds.Tables[0];
            }
            else
            {
                txtFlowName.Text = string.Empty;// oFlow.FlowName.ToString();
                labFlowName.Text = StringTool.ParseForHtml(txtFlowName.Text);

                labApplyName.Text = Session["PersonName"].ToString();
                labDeptName.Text = Session["UserDeptName"].ToString();
                labStartDate.Text = DateTime.Now.ToString("yyyy-MM-dd H:mm:ss");
            }


            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];

                    txtFlowName.Text = row["FlowName"].ToString();
                    labFlowName.Text = StringTool.ParseForHtml(txtFlowName.Text);
                    txtFlowName.Visible = false;
                    labFlowName.Visible = true;
                    rFlowName.Visible = false;

                    labApplyName.Text = row["ApplyName"].ToString();
                    labDeptName.Text = row["DeptName"].ToString();

                    ViewState["ApplyID"] = row["ApplyID"].ToString();
                    ViewState["DeptID"] = row["DeptID"].ToString();

                    if (row["StartDate"].ToString() != "")
                    {
                        labStartDate.Text = DateTime.Parse(row["StartDate"].ToString()).ToString("yyyy-MM-dd H:mm:ss");
                    }
                    else
                    {
                        labStartDate.Text = "";
                    }

                    if (row["EndDate"].ToString() == "")
                    {
                        labEndDate.Text = "--";
                    }
                    else
                    {
                        labEndDate.Text = DateTime.Parse(row["EndDate"].ToString()).ToString("yyyy-MM-dd H:mm:ss");
                    }

                    if (row["Date1"].ToString() != "")
                    {
                        Ctrdateandtime1.dateTime = DateTime.Parse(row["Date1"].ToString());
                    }
                    if (row["Date2"].ToString() != "")
                    {
                        Ctrdateandtime2.dateTime = DateTime.Parse(row["Date2"].ToString());
                    }
                    if (row["Date3"].ToString() != "")
                    {
                        Ctrdateandtime3.dateTime = DateTime.Parse(row["Date3"].ToString());
                    }
                    if (row["Date4"].ToString() != "")
                    {
                        Ctrdateandtime4.dateTime = DateTime.Parse(row["Date4"].ToString());
                    }

                    CtrFlowFormText1.Value = row["String1"].ToString();

                    CtrFlowFormText2.Value = row["String2"].ToString();

                    CtrFlowFormText3.Value = row["String3"].ToString();

                    CtrFlowFormText4.Value = row["String4"].ToString();

                    CtrFlowFormText5.Value = row["String5"].ToString();

                    CtrFlowFormText6.Value = row["String6"].ToString();

                    CtrFlowFormText7.Value = row["String7"].ToString();

                    CtrFlowFormText8.Value = row["String8"].ToString();

                    CtrFlowNumeric1.Value = row["Number1"].ToString();

                    CtrFlowNumeric2.Value = row["Number2"].ToString();

                    CtrFlowNumeric3.Value = row["Number3"].ToString();

                    CtrFlowNumeric4.Value = row["Number4"].ToString();

                    CtrFlowNumeric5.Value = row["Number5"].ToString();

                    CtrFlowRemark1.Value = row["Remark1"].ToString();

                    ftxtDesc.Text = row["Description"].ToString();
                    lblDesc.Text = ftxtDesc.Text.Trim();

                    if (row["Cate1"].ToString() == "")
                    {
                        ViewState["Field_Cate1"] = 0;
                    }
                    else
                    {
                        ViewState["Field_Cate1"] = long.Parse(row["Cate1"].ToString());
                    }
                    if (row["Cate2"].ToString() == "")
                    {
                        ViewState["Field_Cate2"] = 0;
                    }
                    else
                    {
                        ViewState["Field_Cate2"] = long.Parse(row["Cate2"].ToString());
                    }
                    if (row["Cate3"].ToString() == "")
                    {
                        ViewState["Field_Cate3"] = 0;
                    }
                    else
                    {
                        ViewState["Field_Cate3"] = long.Parse(row["Cate3"].ToString());
                    }
                    if (row["Cate4"].ToString() == "")
                    {
                        ViewState["Field_Cate4"] = 0;
                    }
                    else
                    {
                        ViewState["Field_Cate4"] = long.Parse(row["Cate4"].ToString());
                    }
                    if (row["Cate5"].ToString() == "")
                    {
                        ViewState["Field_Cate5"] = 0;
                    }
                    else
                    {
                        ViewState["Field_Cate5"] = long.Parse(row["Cate5"].ToString());
                    }


                    if (row["Bool1"].ToString() == "")
                    {
                        chkBool1.Checked = false;
                    }
                    else
                    {
                        chkBool1.Checked = int.Parse(row["Bool1"].ToString()) == 0 ? false : true;
                    }

                    if (row["Bool2"].ToString() == "")
                    {
                        chkBool2.Checked = false;
                    }
                    else
                    {
                        chkBool2.Checked = int.Parse(row["Bool2"].ToString()) == 0 ? false : true;
                    }
                    if (row["Bool3"].ToString() == "")
                    {
                        chkBool3.Checked = false;
                    }
                    else
                    {
                        chkBool3.Checked = int.Parse(row["Bool3"].ToString()) == 0 ? false : true;
                    }
                    if (row["Bool4"].ToString() == "")
                    {
                        chkBool4.Checked = false;
                    }
                    else
                    {
                        chkBool4.Checked = int.Parse(row["Bool4"].ToString()) == 0 ? false : true;
                    }

                    this.Page.Title = row["FlowName"].ToString();
                }

            }
            else
            {
                ViewState["Field_Cate1"] = 0;
                ViewState["Field_Cate2"] = 0;
                ViewState["Field_Cate3"] = 0;
                ViewState["Field_Cate4"] = 0;
                ViewState["Field_Cate5"] = 0;
            }
            #endregion

            #region 设定字段标题属性  并设置分类控件的值(这时才读取它的CateRootID)
            DataTable dt1 = FlowModel.GetCommAppFlowModelFields(oFlow.FlowModelID);
            DataRow dr1 = null;
            if (dt1.Rows.Count > 0)
                dr1 = dt1.Rows[0];

            if (dr1 != null)
            {
                if (dr1["cate1validate"].ToString() == "1")
                {
                    subCate1.Text = dr1["Cate1"].ToString();
                    CtrFCate1.RootID = long.Parse(dr1["Cate1RootID"].ToString());
                    CtrFCate1.CatelogID = long.Parse(ViewState["Field_Cate1"].ToString());
                }
                else
                {
                    ShowCate1.Visible = false;
                }

                if (dr1["cate2validate"].ToString() == "1")
                {
                    subCate2.Text = dr1["Cate2"].ToString();
                    CtrFCate2.RootID = long.Parse(dr1["Cate2RootID"].ToString());
                    CtrFCate2.CatelogID = long.Parse(ViewState["Field_Cate2"].ToString());

                }
                else
                {
                    ShowCate2.Visible = false;
                }

                if (dr1["cate3validate"].ToString() == "1")
                {
                    subCate3.Text = dr1["Cate3"].ToString();
                    CtrFCate3.RootID = long.Parse(dr1["Cate3RootID"].ToString());
                    CtrFCate3.CatelogID = long.Parse(ViewState["Field_Cate3"].ToString());

                }
                else
                {
                    ShowCate3.Visible = false;
                }

                if (dr1["cate4validate"].ToString() == "1")
                {
                    subCate4.Text = dr1["Cate4"].ToString();
                    CtrFCate4.RootID = long.Parse(dr1["Cate4RootID"].ToString());
                    CtrFCate4.CatelogID = long.Parse(ViewState["Field_Cate4"].ToString());

                }
                else
                {
                    ShowCate4.Visible = false;
                }

                if (dr1["cate5validate"].ToString() == "1")
                {
                    subCate5.Text = dr1["Cate5"].ToString();
                    CtrFCate5.RootID = long.Parse(dr1["Cate5RootID"].ToString());
                    CtrFCate5.CatelogID = long.Parse(ViewState["Field_Cate5"].ToString());

                }
                else
                {
                    ShowCate5.Visible = false;
                }

                if (dr1["date1validate"].ToString() == "1")
                {
                    subDate1.Text = dr1["Date1"].ToString();
                }
                else
                {
                    ShowDate1.Visible = false;
                }
                if (dr1["date2validate"].ToString() == "1")
                {
                    subDate2.Text = dr1["Date2"].ToString();
                }
                else
                {
                    ShowDate2.Visible = false;
                }
                if (dr1["date3validate"].ToString() == "1")
                {
                    subDate3.Text = dr1["Date3"].ToString();
                }
                else
                {
                    ShowDate3.Visible = false;
                }
                if (dr1["date4validate"].ToString() == "1")
                {
                    subDate4.Text = dr1["Date4"].ToString();
                }
                else
                {
                    ShowDate4.Visible = false;
                }
                if (dr1["string1validate"].ToString() == "1")
                {
                    subString1.Text = dr1["String1"].ToString();
                }
                else
                {
                    ShowString1.Visible = false;
                }
                if (dr1["string2validate"].ToString() == "1")
                {
                    subString2.Text = dr1["String2"].ToString();
                }
                else
                {
                    ShowString2.Visible = false;
                }
                if (dr1["string3validate"].ToString() == "1")
                {
                    subString3.Text = dr1["String3"].ToString();
                }
                else
                {
                    ShowString3.Visible = false;
                }
                if (dr1["string4validate"].ToString() == "1")
                {
                    subString4.Text = dr1["String4"].ToString();
                }
                else
                {
                    ShowString4.Visible = false;
                }
                if (dr1["string5validate"].ToString() == "1")
                {
                    subString5.Text = dr1["String5"].ToString();
                }
                else
                {
                    ShowString5.Visible = false;
                }
                if (dr1["string6validate"].ToString() == "1")
                {
                    subString6.Text = dr1["String6"].ToString();
                }
                else
                {
                    ShowString6.Visible = false;
                }
                if (dr1["string7validate"].ToString() == "1")
                {
                    subString7.Text = dr1["String7"].ToString();
                }
                else
                {
                    ShowString7.Visible = false;
                }
                if (dr1["string8validate"].ToString() == "1")
                {
                    subString8.Text = dr1["String8"].ToString();
                }
                else
                {
                    ShowString8.Visible = false;
                }
                if (dr1["number1validate"].ToString() == "1")
                {
                    subNumber1.Text = dr1["Number1"].ToString();
                }
                else
                {
                    ShowNumber1.Visible = false;
                }
                if (dr1["number2validate"].ToString() == "1")
                {
                    subNumber2.Text = dr1["Number2"].ToString();
                }
                else
                {
                    ShowNumber2.Visible = false;
                }
                if (dr1["number3validate"].ToString() == "1")
                {
                    subNumber3.Text = dr1["Number3"].ToString();
                }
                else
                {
                    ShowNumber3.Visible = false;
                }
                if (dr1["number4validate"].ToString() == "1")
                {
                    subNumber4.Text = dr1["Number4"].ToString();
                }
                else
                {
                    ShowNumber4.Visible = false;
                }
                if (dr1["number5validate"].ToString() == "1")
                {
                    subNumber5.Text = dr1["Number5"].ToString();
                }
                else
                {
                    ShowNumber5.Visible = false;
                }

                if (dr1["Remark1validate"].ToString() == "1")
                {
                    subRemark1.Text = dr1["Remark1"].ToString();
                }
                else
                {
                    ShowRemark1.Visible = false;
                }

                if (dr1["Bool1validate"].ToString() == "1")
                {
                    subBool1.Text = dr1["Bool1"].ToString();
                }
                else
                {
                    ShowBool1.Visible = false;
                }

                if (dr1["Bool2validate"].ToString() == "1")
                {
                    subBool2.Text = dr1["Bool2"].ToString();
                }
                else
                {
                    ShowBool2.Visible = false;
                }
                if (dr1["Bool3validate"].ToString() == "1")
                {
                    subBool3.Text = dr1["Bool3"].ToString();
                }
                else
                {
                    ShowBool3.Visible = false;
                }
                if (dr1["Bool4validate"].ToString() == "1")
                {
                    subBool4.Text = dr1["Bool4"].ToString();
                }
                else
                {
                    ShowBool4.Visible = false;
                }

                if (dr1["DescValidate"].ToString() == "1")
                {
                    if (ftxtDesc.Text.Trim() == string.Empty)
                    {
                        ftxtDesc.Text = dr1["Description"].ToString();
                        lblDesc.Text = dr1["Description"].ToString();
                    }
                    subFbox.Text = dr1["Fbox"].ToString();
                }
                else
                {
                    ShowDesc.Visible = false;
                }

                #region  //设置必填
                CtrFlowFormText1.TextToolTip = dr1["String1"].ToString();
                CtrFlowFormText2.TextToolTip = dr1["String2"].ToString();
                CtrFlowFormText3.TextToolTip = dr1["String3"].ToString();
                CtrFlowFormText4.TextToolTip = dr1["String4"].ToString();
                CtrFlowFormText5.TextToolTip = dr1["String5"].ToString();
                CtrFlowFormText6.TextToolTip = dr1["String6"].ToString();
                CtrFlowFormText7.TextToolTip = dr1["String7"].ToString();
                CtrFlowFormText8.TextToolTip = dr1["String8"].ToString();
                CtrFlowNumeric1.TextToolTip = dr1["Number1"].ToString();
                CtrFlowNumeric2.TextToolTip = dr1["Number2"].ToString();
                CtrFlowNumeric3.TextToolTip = dr1["Number3"].ToString();
                CtrFlowNumeric4.TextToolTip = dr1["Number4"].ToString();
                CtrFlowNumeric5.TextToolTip = dr1["Number5"].ToString();
                CtrFCate1.TextToolTip = dr1["Cate1"].ToString();
                CtrFCate2.TextToolTip = dr1["Cate2"].ToString();
                CtrFCate3.TextToolTip = dr1["Cate3"].ToString();
                CtrFCate4.TextToolTip = dr1["Cate4"].ToString();
                CtrFCate5.TextToolTip = dr1["Cate5"].ToString();
                Ctrdateandtime1.TextToolTip = dr1["Date1"].ToString();
                Ctrdateandtime2.TextToolTip = dr1["Date2"].ToString();
                Ctrdateandtime3.TextToolTip = dr1["Date3"].ToString();
                Ctrdateandtime4.TextToolTip = dr1["Date4"].ToString();

                if (dr1["String1Must"].ToString() == "1")
                    CtrFlowFormText1.MustInput = true;
                if (dr1["String2Must"].ToString() == "1")
                    CtrFlowFormText2.MustInput = true;
                if (dr1["String3Must"].ToString() == "1")
                    CtrFlowFormText3.MustInput = true;
                if (dr1["String4Must"].ToString() == "1")
                    CtrFlowFormText4.MustInput = true;
                if (dr1["String5Must"].ToString() == "1")
                    CtrFlowFormText5.MustInput = true;
                if (dr1["String6Must"].ToString() == "1")
                    CtrFlowFormText6.MustInput = true;
                if (dr1["String7Must"].ToString() == "1")
                    CtrFlowFormText7.MustInput = true;
                if (dr1["String8Must"].ToString() == "1")
                    CtrFlowFormText8.MustInput = true;
                if (dr1["Number1Must"].ToString() == "1")
                    CtrFlowNumeric1.MustInput = true;
                if (dr1["Number2Must"].ToString() == "1")
                    CtrFlowNumeric2.MustInput = true;
                if (dr1["Number3Must"].ToString() == "1")
                    CtrFlowNumeric3.MustInput = true;
                if (dr1["Number4Must"].ToString() == "1")
                    CtrFlowNumeric4.MustInput = true;
                if (dr1["Number5Must"].ToString() == "1")
                    CtrFlowNumeric5.MustInput = true;
                if (dr1["Cate1Must"].ToString() == "1")
                    CtrFCate1.MustInput = true;
                if (dr1["Cate2Must"].ToString() == "1")
                    CtrFCate2.MustInput = true;
                if (dr1["Cate3Must"].ToString() == "1")
                    CtrFCate3.MustInput = true;
                if (dr1["Cate4Must"].ToString() == "1")
                    CtrFCate4.MustInput = true;
                if (dr1["Cate5Must"].ToString() == "1")
                    CtrFCate5.MustInput = true;
                if (dr1["Date1Must"].ToString() == "1")
                    Ctrdateandtime1.MustInput = true;
                if (dr1["Date2Must"].ToString() == "1")
                    Ctrdateandtime2.MustInput = true;
                if (dr1["Date3Must"].ToString() == "1")
                    Ctrdateandtime3.MustInput = true;
                if (dr1["Date4Must"].ToString() == "1")
                    Ctrdateandtime4.MustInput = true;
                #endregion 
                #region 是否显示时间
                if (dr1["Date1Show"].ToString() == "1")
                    Ctrdateandtime1.ShowTime = true;
                else
                    Ctrdateandtime1.ShowTime = false;
                if (dr1["Date2Show"].ToString() == "1")
                    Ctrdateandtime2.ShowTime = true;
                else
                    Ctrdateandtime2.ShowTime = false;
                if (dr1["Date3Show"].ToString() == "1")
                    Ctrdateandtime3.ShowTime = true;
                else
                    Ctrdateandtime3.ShowTime = false;
                if (dr1["Date4Show"].ToString() == "1")
                    Ctrdateandtime4.ShowTime = true;
                else
                    Ctrdateandtime4.ShowTime = false;
                #endregion 
            }
            #endregion
            #region 设定字段属性
            setFieldCollection setFields = oFlow.setFields;
            foreach (setField sf in setFields)
            {
                if (sf.Editable == true && sf.Visibled == true)
                {
                }
                else
                {
                    switch (sf.Name)
                    {

                        case "date1":
                            if (sf.Visibled == false)
                            {
                                ShowDate1.Visible = false;
                            }
                            else
                            {
                                Ctrdateandtime1.ContralState = eOA_FlowControlState.eReadOnly;
                            }
                            break;
                        case "date2":
                            if (sf.Visibled == false)
                            {
                                ShowDate2.Visible = false;
                            }
                            else
                            {
                                Ctrdateandtime2.ContralState = eOA_FlowControlState.eReadOnly;
                            }
                            break;
                        case "date3":
                            if (sf.Visibled == false)
                            {
                                ShowDate3.Visible = false;
                            }
                            else
                            {
                                Ctrdateandtime3.ContralState = eOA_FlowControlState.eReadOnly;
                            }
                            break;
                        case "date4":
                            if (sf.Visibled == false)
                            {
                                ShowDate4.Visible = false;
                            }
                            else
                            {
                                Ctrdateandtime4.ContralState = eOA_FlowControlState.eReadOnly;
                            }
                            break;
                        case "string1":
                            if (sf.Visibled == false)
                            {
                                ShowString1.Visible = false;
                            }
                            else
                            {
                                CtrFlowFormText1.ContralState = eOA_FlowControlState.eReadOnly;
                            }
                            break;
                        case "string2":
                            if (sf.Visibled == false)
                            {
                                ShowString2.Visible = false;
                            }
                            else
                            {
                                CtrFlowFormText2.ContralState = eOA_FlowControlState.eReadOnly;
                            }
                            break;
                        case "string3":
                            if (sf.Visibled == false)
                            {
                                ShowString3.Visible = false;
                            }
                            else
                            {
                                CtrFlowFormText3.ContralState = eOA_FlowControlState.eReadOnly;
                            }
                            break;
                        case "string4":
                            if (sf.Visibled == false)
                            {
                                ShowString4.Visible = false;
                            }
                            else
                            {
                                CtrFlowFormText4.ContralState = eOA_FlowControlState.eReadOnly;
                            }
                            break;
                        case "string5":
                            if (sf.Visibled == false)
                            {
                                ShowString5.Visible = false;
                            }
                            else
                            {
                                CtrFlowFormText5.ContralState = eOA_FlowControlState.eReadOnly;
                            }
                            break;
                        case "string6":
                            if (sf.Visibled == false)
                            {
                                ShowString6.Visible = false;
                            }
                            else
                            {
                                CtrFlowFormText6.ContralState = eOA_FlowControlState.eReadOnly;
                            }
                            break;
                        case "string7":
                            if (sf.Visibled == false)
                            {
                                ShowString7.Visible = false;
                            }
                            else
                            {
                                CtrFlowFormText7.ContralState = eOA_FlowControlState.eReadOnly;
                            }
                            break;
                        case "string8":
                            if (sf.Visibled == false)
                            {
                                ShowString8.Visible = false;
                            }
                            else
                            {
                                CtrFlowFormText8.ContralState = eOA_FlowControlState.eReadOnly;
                            }
                            break;
                        case "number1":
                            if (sf.Visibled == false)
                            {
                                ShowNumber1.Visible = false;
                            }
                            else
                            {
                                CtrFlowNumeric1.ContralState = eOA_FlowControlState.eReadOnly;
                            }
                            break;
                        case "number2":
                            if (sf.Visibled == false)
                            {
                                ShowNumber2.Visible = false;
                            }
                            else
                            {
                                CtrFlowNumeric2.ContralState = eOA_FlowControlState.eReadOnly;
                            }
                            break;
                        case "number3":
                            if (sf.Visibled == false)
                            {
                                ShowNumber3.Visible = false;
                            }
                            else
                            {
                                CtrFlowNumeric3.ContralState = eOA_FlowControlState.eReadOnly;
                            }
                            break;
                        case "number4":
                            if (sf.Visibled == false)
                            {
                                ShowNumber4.Visible = false;
                            }
                            else
                            {
                                CtrFlowNumeric4.ContralState = eOA_FlowControlState.eReadOnly;
                            }
                            break;
                        case "number5":
                            if (sf.Visibled == false)
                            {
                                ShowNumber5.Visible = false;
                            }
                            else
                            {
                                CtrFlowNumeric5.ContralState = eOA_FlowControlState.eReadOnly;
                            }
                            break;
                       
                        case "cate1":
                            CtrFCate1.ContralState = eOA_FlowControlState.eReadOnly;
                            if (sf.Visibled == false)
                            {
                                CtrFCate1.ContralState = eOA_FlowControlState.eHidden;
                                ShowCate1.Visible = false;
                            }
                            break;
                        case "cate2":
                            CtrFCate2.ContralState = eOA_FlowControlState.eReadOnly;
                            if (sf.Visibled == false)
                            {
                                CtrFCate2.ContralState = eOA_FlowControlState.eHidden;
                                ShowCate2.Visible = false;
                            }
                            break;
                        case "cate3":
                            CtrFCate3.ContralState = eOA_FlowControlState.eReadOnly;
                            if (sf.Visibled == false)
                            {
                                CtrFCate3.ContralState = eOA_FlowControlState.eHidden;
                                ShowCate3.Visible = false;
                            }
                            break;
                        case "cate4":
                            CtrFCate4.ContralState = eOA_FlowControlState.eReadOnly;
                            if (sf.Visibled == false)
                            {
                                CtrFCate4.ContralState = eOA_FlowControlState.eHidden;
                                ShowCate4.Visible = false;
                            }
                            break;
                            
                        case "cate5":
                            CtrFCate5.ContralState = eOA_FlowControlState.eReadOnly;
                            if (sf.Visibled == false)
                            {
                                CtrFCate5.ContralState = eOA_FlowControlState.eHidden;
                                ShowCate5.Visible = false;
                            }
                            break;
                        case "bool1":
                            if (sf.Visibled == false)
                            {
                                ShowBool1.Visible = false;
                            }
                            else
                            {
                                chkBool1.Enabled = false;
                            }
                            break;
                        case "bool2":
                            if (sf.Visibled == false)
                            {
                                ShowBool2.Visible = false;
                            }
                            else
                            {
                                chkBool2.Enabled = false;
                            }
                            break;
                        case "bool3":
                            if (sf.Visibled == false)
                            {
                                ShowBool3.Visible = false;
                            }
                            else
                            {
                                chkBool3.Enabled = false;
                            }
                            break;
                        case "bool4":
                            if (sf.Visibled == false)
                            {
                                ShowBool4.Visible = false;
                            }
                            else
                            {
                                chkBool4.Enabled = false;
                            }
                            break;
                        case "remark1":
                            if (sf.Visibled == false)
                            {
                                ShowRemark1.Visible = false;
                            }
                            else
                            {
                                CtrFlowRemark1.ContralState = eOA_FlowControlState.eReadOnly;
                            }
                            break;
                        case "description":
                            if (sf.Visibled == false)
                            {
                                ShowDesc.Visible = false;
                            }
                            else
                            {
                                lblDesc.Visible = true;
                                ftxtDesc.Visible = false;
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
            #endregion
        }
        #endregion

      
        #region 加--号 SpecTransText
        /// <summary>
        /// 加--号
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private string SpecTransText(string str)
        {
            string strR = str;
            if (str == "")
            {
                strR = "--";
            }
            if (str == "--")
            {
                strR = "";
            }
            return strR;
        }
        #endregion
    }
}
