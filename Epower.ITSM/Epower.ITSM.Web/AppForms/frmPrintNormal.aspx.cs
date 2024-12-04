/****************************************************************************
 * 
 * description:打印表单
 * 
 * 
 * 
 * Create by:
 * Create Date:2009-05-26
 * *************************************************************************/
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

using Epower.ITSM.SqlDAL;
using EpowerGlobal;
using EpowerCom;
using appDataProcess;
using Epower.DevBase.BaseTools;
using Epower.ITSM.Base;

namespace Epower.ITSM.Web.AppForms
{
    public partial class frmPrintNormal : BasePage
    {
        /// <summary>
        /// 初始化页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string strFlowModelID = Request["lngFlowModelID"].ToString();
                string strMessageID = Request["lngMessageID"].ToString();
                InitInterface(long.Parse(strFlowModelID), long.Parse(strMessageID));
            }
        }

        #region 初始化页面显示数据 InitInterface
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lngFlowModelID"></param>
        /// <param name="lngMessageID"></param>
        private void InitInterface(long lngFlowModelID, long lngMessageID)
        {
            #region 获取表单数据
            objFlow oFlow = new objFlow((long)Session["UserID"], lngFlowModelID, lngMessageID);
            CtrlProcess1.FlowID = oFlow.FlowID;
            CtrlProcess1.FlowModelID = oFlow.FlowModelID;
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
                        txtDate1.Text = DateTime.Parse(row["Date1"].ToString()).ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        txtDate1.Text = "";
                    }
                    labDate1.Text = StringTool.ParseForHtml(SpecTransText(txtDate1.Text));

                    if (row["Date2"].ToString() != "")
                    {
                        txtDate2.Text = DateTime.Parse(row["Date2"].ToString()).ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        txtDate2.Text = "";
                    }
                    labDate2.Text = StringTool.ParseForHtml(SpecTransText(txtDate2.Text));



                    txtString1.Text = row["String1"].ToString();
                    labString1.Text = StringTool.ParseForHtml(SpecTransText(txtString1.Text));

                    txtString2.Text = row["String2"].ToString();
                    labString2.Text = StringTool.ParseForHtml(SpecTransText(txtString2.Text));

                    txtString3.Text = row["String3"].ToString();
                    labString3.Text = StringTool.ParseForHtml(SpecTransText(txtString3.Text));

                    txtString4.Text = row["String4"].ToString();
                    labString4.Text = StringTool.ParseForHtml(SpecTransText(txtString4.Text));

                    txtString5.Text = row["String5"].ToString();
                    labString5.Text = StringTool.ParseForHtml(SpecTransText(txtString5.Text));

                    txtString6.Text = row["String6"].ToString();
                    labString6.Text = StringTool.ParseForHtml(SpecTransText(txtString6.Text));

                    txtString7.Text = row["String7"].ToString();
                    labString7.Text = StringTool.ParseForHtml(SpecTransText(txtString7.Text));

                    txtString8.Text = row["String8"].ToString();
                    labString8.Text = StringTool.ParseForHtml(SpecTransText(txtString8.Text));

                    txtNumber1.Text = row["Number1"].ToString();
                    labNumber1.Text = StringTool.ParseForHtml(SpecTransText(txtNumber1.Text));

                    txtNumber2.Text = row["Number2"].ToString();
                    labNumber2.Text = StringTool.ParseForHtml(SpecTransText(txtNumber2.Text));

                    txtNumber3.Text = row["Number3"].ToString();
                    labNumber3.Text = StringTool.ParseForHtml(SpecTransText(txtNumber3.Text));

                    txtNumber4.Text = row["Number4"].ToString();
                    labNumber4.Text = StringTool.ParseForHtml(SpecTransText(txtNumber4.Text));

                    txtNumber5.Text = row["Number5"].ToString();
                    labNumber5.Text = StringTool.ParseForHtml(SpecTransText(txtNumber5.Text));


                    txtRemark1.Text = row["Remark1"].ToString();
                    labRemark1.Text = StringTool.ParseForHtml(SpecTransText(txtRemark1.Text));

                    //ftxtDesc.Text = row["Description"].ToString();
                    lblDesc.Text = row["Description"].ToString();




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
                }

            }
            else
            {
                labString1.Text = "--";
                labString2.Text = "--";
                labString3.Text = "--";
                labString4.Text = "--";
                labString5.Text = "--";
                labString6.Text = "--";
                labString7.Text = "--";
                labString8.Text = "--";

                labDate1.Text = "--";
                labDate2.Text = "--";
                labNumber1.Text = "--";
                labNumber2.Text = "--";
                labNumber3.Text = "--";
                labNumber4.Text = "--";
                labNumber5.Text = "--";

                labRemark1.Text = "--";

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

                    //subCate1.Text = dr1["Cate1"].ToString();
                    ////获取dbo控件的缺省值
                    //SetDboControlValue(ref ddlCate1, long.Parse(dr1["Cate1RootID"].ToString()), long.Parse( ViewState["Field_Cate1"].ToString()));
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
                    ViewState["Field_Date1"] = txtDate1.Text;
                    ShowDate1.Visible = false;
                }
                if (dr1["date2validate"].ToString() == "1")
                {
                    subDate2.Text = dr1["Date2"].ToString();
                }
                else
                {
                    ViewState["Field_Date2"] = txtDate2.Text;
                    ShowDate2.Visible = false;
                }
                if (dr1["string1validate"].ToString() == "1")
                {
                    subString1.Text = dr1["String1"].ToString();
                }
                else
                {
                    ViewState["Field_String1"] = txtString1.Text;
                    ShowString1.Visible = false;
                }
                if (dr1["string2validate"].ToString() == "1")
                {
                    subString2.Text = dr1["String2"].ToString();
                }
                else
                {
                    ViewState["Field_String2"] = txtString2.Text;
                    ShowString2.Visible = false;
                }
                if (dr1["string3validate"].ToString() == "1")
                {
                    subString3.Text = dr1["String3"].ToString();
                }
                else
                {
                    ViewState["Field_String3"] = txtString3.Text;
                    ShowString3.Visible = false;
                }
                if (dr1["string4validate"].ToString() == "1")
                {
                    subString4.Text = dr1["String4"].ToString();
                }
                else
                {
                    ViewState["Field_String4"] = txtString4.Text;
                    ShowString4.Visible = false;
                }
                if (dr1["string5validate"].ToString() == "1")
                {
                    subString5.Text = dr1["String5"].ToString();
                }
                else
                {
                    ViewState["Field_String5"] = txtString5.Text;
                    ShowString5.Visible = false;
                }
                if (dr1["string6validate"].ToString() == "1")
                {
                    subString6.Text = dr1["String6"].ToString();
                }
                else
                {
                    ViewState["Field_String6"] = txtString6.Text;
                    ShowString6.Visible = false;
                }
                if (dr1["string7validate"].ToString() == "1")
                {
                    subString7.Text = dr1["String7"].ToString();
                }
                else
                {
                    ViewState["Field_String7"] = txtString7.Text;
                    ShowString7.Visible = false;
                }
                if (dr1["string8validate"].ToString() == "1")
                {
                    subString8.Text = dr1["String8"].ToString();
                }
                else
                {
                    ViewState["Field_String8"] = txtString8.Text;
                    ShowString8.Visible = false;
                }
                if (dr1["number1validate"].ToString() == "1")
                {
                    subNumber1.Text = dr1["Number1"].ToString();
                }
                else
                {
                    ViewState["Field_Number1"] = txtNumber1.Text;
                    ShowNumber1.Visible = false;
                }
                if (dr1["number2validate"].ToString() == "1")
                {
                    subNumber2.Text = dr1["Number2"].ToString();
                }
                else
                {
                    ViewState["Field_Number2"] = txtNumber2.Text;
                    ShowNumber2.Visible = false;
                }
                if (dr1["number3validate"].ToString() == "1")
                {
                    subNumber3.Text = dr1["Number3"].ToString();
                }
                else
                {
                    ViewState["Field_Number3"] = txtNumber3.Text;
                    ShowNumber3.Visible = false;
                }
                if (dr1["number4validate"].ToString() == "1")
                {
                    subNumber4.Text = dr1["Number4"].ToString();
                }
                else
                {
                    ViewState["Field_Number4"] = txtNumber4.Text;
                    ShowNumber4.Visible = false;
                }
                if (dr1["number5validate"].ToString() == "1")
                {
                    subNumber5.Text = dr1["Number5"].ToString();
                }
                else
                {
                    ViewState["Field_Number5"] = txtNumber5.Text;
                    ShowNumber5.Visible = false;
                }

                if (dr1["Remark1validate"].ToString() == "1")
                {
                    subRemark1.Text = dr1["Remark1"].ToString();
                }
                else
                {
                    Session["Field_Remark1"] = txtRemark1.Text;
                    ShowRemark1.Visible = false;
                }

                if (dr1["Bool1validate"].ToString() == "1")
                {
                    subBool1.Text = dr1["Bool1"].ToString();
                }
                else
                {
                    ViewState["Field_Bool1"] = chkBool1.Checked == false ? 0 : 1;
                    ShowBool1.Visible = false;
                }

                if (dr1["Bool2validate"].ToString() == "1")
                {
                    subBool2.Text = dr1["Bool2"].ToString();
                }
                else
                {
                    ViewState["Field_Bool2"] = chkBool1.Checked == false ? 0 : 1;
                    ShowBool2.Visible = false;
                }

                if (dr1["TitleValidate"].ToString() == "1")
                {
                    showTitle.Visible = true;
                    lblPrintTitle.Text = dr1["PrintTitle"].ToString();
                }
                else
                {
                    showTitle.Visible = false;
                }
                if (dr1["BottomValidate"].ToString() == "1")
                {
                    showBottom.Visible = true;
                    lblPrintBottom.Text = dr1["PrintBottom"].ToString();
                }
                else
                {
                    showBottom.Visible = false;
                }
            }
            #endregion
            #region 设定字段属性
            setFieldCollection setFields = oFlow.setFields;
            foreach (setField sf in setFields)
            {
                switch (sf.Name)
                {

                    case "date1":
                        if (sf.Visibled == false)
                        {
                            ViewState["Field_Date1"] = txtDate1.Text;
                            ShowDate1.Visible = false;
                        }
                        else
                        {
                            labDate1.Visible = true;
                            txtDate1.Visible = false;
                            imgDate1.Visible = false;
                        }
                        break;
                    case "date2":
                        if (sf.Visibled == false)
                        {
                            ViewState["Field_Date2"] = txtDate2.Text;
                            ShowDate2.Visible = false;
                        }
                        else
                        {
                            labDate2.Visible = true;
                            txtDate2.Visible = false;
                            imgDate2.Visible = false;
                        }
                        break;
                    case "string1":
                        if (sf.Visibled == false)
                        {
                            ViewState["Field_String1"] = txtString1.Text;
                            ShowString1.Visible = false;
                        }
                        else
                        {
                            labString1.Visible = true;
                            txtString1.Visible = false;
                        }
                        break;
                    case "string2":
                        if (sf.Visibled == false)
                        {
                            ViewState["Field_String2"] = txtString2.Text;
                            ShowString2.Visible = false;
                        }
                        else
                        {
                            labString2.Visible = true;
                            txtString2.Visible = false;
                        }
                        break;
                    case "string3":
                        if (sf.Visibled == false)
                        {
                            ViewState["Field_String3"] = txtString3.Text;
                            ShowString3.Visible = false;
                        }
                        else
                        {
                            labString3.Visible = true;
                            txtString3.Visible = false;
                        }
                        break;
                    case "string4":
                        if (sf.Visibled == false)
                        {
                            ViewState["Field_String4"] = txtString4.Text;
                            ShowString4.Visible = false;
                        }
                        else
                        {
                            labString4.Visible = true;
                            txtString4.Visible = false;
                        }
                        break;
                    case "string5":
                        if (sf.Visibled == false)
                        {
                            ViewState["Field_String5"] = txtString5.Text;
                            ShowString5.Visible = false;
                        }
                        else
                        {
                            labString5.Visible = true;
                            txtString5.Visible = false;
                        }
                        break;
                    case "string6":
                        if (sf.Visibled == false)
                        {
                            ViewState["Field_String6"] = txtString6.Text;
                            ShowString6.Visible = false;
                        }
                        else
                        {
                            labString6.Visible = true;
                            txtString6.Visible = false;
                        }
                        break;
                    case "string7":
                        if (sf.Visibled == false)
                        {
                            ViewState["Field_String7"] = txtString7.Text;
                            ShowString7.Visible = false;
                        }
                        else
                        {
                            labString7.Visible = true;
                            txtString7.Visible = false;
                        }
                        break;
                    case "string8":
                        if (sf.Visibled == false)
                        {
                            ViewState["Field_String8"] = txtString8.Text;
                            ShowString8.Visible = false;
                        }
                        else
                        {
                            labString8.Visible = true;
                            txtString8.Visible = false;
                        }
                        break;
                    case "number1":
                        if (sf.Visibled == false)
                        {
                            ViewState["Field_Number1"] = txtNumber1.Text;
                            ShowNumber1.Visible = false;
                        }
                        else
                        {
                            labNumber1.Visible = true;
                            txtNumber1.Visible = false;
                        }
                        break;
                    case "number2":
                        if (sf.Visibled == false)
                        {
                            ViewState["Field_Number2"] = txtNumber2.Text;
                            ShowNumber2.Visible = false;
                        }
                        else
                        {
                            labNumber2.Visible = true;
                            txtNumber2.Visible = false;
                        }
                        break;
                    case "number3":
                        if (sf.Visibled == false)
                        {
                            ViewState["Field_Number3"] = txtNumber3.Text;
                            ShowNumber3.Visible = false;
                        }
                        else
                        {
                            labNumber3.Visible = true;
                            txtNumber3.Visible = false;
                        }
                        break;
                    case "number4":
                        if (sf.Visibled == false)
                        {
                            ViewState["Field_Number4"] = txtNumber4.Text;
                            ShowNumber4.Visible = false;
                        }
                        else
                        {
                            labNumber4.Visible = true;
                            txtNumber4.Visible = false;
                        }
                        break;
                    case "number5":
                        if (sf.Visibled == false)
                        {
                            ViewState["Field_Number5"] = txtNumber5.Text;
                            ShowNumber5.Visible = false;
                        }
                        else
                        {
                            labNumber5.Visible = true;
                            txtNumber5.Visible = false;
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
                            ViewState["Field_Bool1"] = chkBool1.Checked == false ? 0 : 1;
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
                            ViewState["Field_Bool2"] = chkBool2.Checked == false ? 0 : 1;
                            ShowBool2.Visible = false;
                        }
                        else
                        {
                            chkBool2.Enabled = false;
                        }
                        break;
                    case "remark1":
                        if (sf.Visibled == false)
                        {
                            Session["Field_Remark1"] = txtRemark1.Text;
                            ShowRemark1.Visible = false;
                        }
                        else
                        {
                            labRemark1.Visible = true;
                            txtRemark1.Visible = false;
                        }
                        break;
                    case "description":
                        if (sf.Visibled == false)
                        {
                            ShowDesc.Visible = false;
                        }
                        else
                        {
                            ShowDesc.Visible = true;
                            lblDesc.Visible = true;
                        }
                        break;
                    default:
                        break;
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
