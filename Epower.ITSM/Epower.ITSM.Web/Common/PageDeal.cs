/*******************************************************************
 *
 * Description:设置页面显示值
 * 
 * 
 * Create By  :zhumc
 * Create Date:2008年4月11日
 * *****************************************************************/
using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml;

using Epower.ITSM.SqlDAL;
using System.Web.Caching;
using Epower.DevBase.BaseTools;
using Epower.ITSM.Web.Controls;

namespace Epower.ITSM.Web
{
    /// <summary>
    /// 
    /// </summary>
    public class PageDeal
    {
        private static string Key = "EA_DefineLanguageCache";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pControl"></param>
        public static void SetLanguage(Control pControl)
        {
            SetPageLanguage(pControl, GetDataTable());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sKey"></param>
        /// <returns></returns>
        public static string GetLanguageValue(string sKey)
        {
            string sReturn = string.Empty;
            DataTable dt = new DataTable();
            dt = GetDataTable();
            DataRow[] arrdr = dt.Select("KeyName=" + Epower.DevBase.BaseTools.StringTool.SqlQ(sKey.Trim()));
            if (arrdr.Length > 0)
            {
                sReturn = arrdr[0]["KeyValue"].ToString();
            }
            return sReturn;
        }


        /// <summary>
        /// 取自定义信息项.
        /// </summary>
        /// <param name="strColumnName">数据列名</param>
        /// <param name="strGroupName">分组名</param>
        /// <returns></returns>
        public static string GetLanguageValue(string strColumnName, string strGroupName)
        {
            string sReturn = string.Empty;

            DataTable dt = GetDataTable();
            DataRow[] arrdr = dt.Select(String.Format("KeyName = {0} AND groups = {1}",
StringTool.SqlQ(strColumnName.Trim()),
StringTool.SqlQ(strGroupName.Trim())));

            if (arrdr.Length > 0)
            {
                sReturn = arrdr[0]["KeyValue"].ToString();
            }
            return sReturn;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private static DataTable GetDataTable()
        {
            DataTable dt;
            if (HttpRuntime.Cache[Key] == null)
            {
                EA_DefineLanguageDP ee = new EA_DefineLanguageDP();
                dt = ee.GetDataTable(string.Empty, string.Empty);
                //插入缓存
                HttpRuntime.Cache.Insert(Key, dt);
            }
            else
            {
                dt = (DataTable)HttpRuntime.Cache[Key];
            }
            return dt;
        }

        #region 得到控件的值

        /// <summary>
        /// 得到控件的值,使用注意事项[放在PageLoad事件的最后位置]
        /// </summary>
        /// <param name="arrCtr">控件数组</param>
        /// <param name="ht"></param>
        public static void GetPageQueryParam(Control[] arrCtr, Control pgControl, string PageID)
        {
            System.Collections.Hashtable ht = new System.Collections.Hashtable();
            ht.Add("PageID", PageID);
            if (pgControl != null)
            {
                string strType = pgControl.GetType().Name;
                string strCtrID = pgControl.ClientID;
                switch (strType)
                {
                    case "controls_controlpage_ascx":
                        ht.Add(strCtrID + "_CurrentPage", ((Label)((Epower.ITSM.Web.Controls.ControlPage)pgControl).FindControl("lblPageIndex")).Text.ToString().Split('/')[0].Trim());
                        ht.Add(strCtrID + "_PageSize", ((DropDownList)((Epower.ITSM.Web.Controls.ControlPage)pgControl).FindControl("dplPageButtonCount")).SelectedItem.Value.ToString());
                        break;
                    case "controls_controlpagefoot_ascx":
                        ht.Add(strCtrID + "_CurrentPage", ((Label)((Epower.ITSM.Web.Controls.ControlPageFoot)pgControl).FindControl("LabCurrentPage")).Text.ToString());
                        int currentpage = ((Epower.ITSM.Web.Controls.ControlPageFoot)pgControl).CurrentPage;
                        ht.Add(strCtrID + "_PageSize", ((DropDownList)((Epower.ITSM.Web.Controls.ControlPageFoot)pgControl).FindControl("drpPageSize")).SelectedItem.Value.ToString());
                        break;
                    default:
                        break;
                }
            }
            for (int i = 0; i < arrCtr.Length; i++)
            {
                GetControlValue(arrCtr[i], ht);
                if (arrCtr[i].HasControls())
                {
                    GetPageQueryParam(arrCtr[i], ht);
                }
            }
            HttpContext.Current.Session["PageQueryParm"] = ht;
        }

        /// <summary>
        /// 得到控件的值
        /// </summary>
        /// <param name="pControl"></param>
        /// <param name="ht"></param>
        private static void GetPageQueryParam(Control pControl, System.Collections.Hashtable ht)
        {
            foreach (Control psubControl in pControl.Controls)  /*遍历所有子节点*/
            {
                GetControlValue(psubControl, ht);
            }
            for (int i = 0; i < pControl.Controls.Count; i++)
            {
                foreach (Control psubControl in pControl.Controls[i].Controls)  /*遍历所有子节点*/
                {
                    if (psubControl.HasControls())
                    {
                        GetPageQueryParam(psubControl, ht);
                    }
                }
            }
        }

        /// <summary>
        /// 得到控件的值
        /// </summary>
        /// <param name="ctrol"></param>
        /// <param name="ht"></param>
        private static void GetControlValue(Control ctrol, System.Collections.Hashtable ht)
        {
            string strType = ctrol.GetType().Name;
            string strCtrID = ctrol.ClientID;
            switch (strType)
            {
                case "controls_ctrdateandtime_ascx":
                    ht.Add(strCtrID, ((Epower.ITSM.Web.Controls.CtrDateAndTime)ctrol).dateTime.ToString());
                    break;
                case "controls_userpicker_ascx":
                    string upid = ((HtmlInputHidden)((Epower.ITSM.Web.Controls.UserPicker)ctrol).FindControl("hidUser")).Value.Trim().ToString() == "" ? "0" : ((HtmlInputHidden)((Epower.ITSM.Web.Controls.UserPicker)ctrol).FindControl("hidUser")).Value.Trim().ToString();
                    ht.Add(strCtrID, upid + "|" + ((HtmlInputHidden)((Epower.ITSM.Web.Controls.UserPicker)ctrol).FindControl("hidUserName")).Value.Trim().ToString());
                    break;
                case "controls_userpickermult_ascx":
                    string upmid = ((HtmlInputHidden)((Epower.ITSM.Web.Controls.UserPickerMult)ctrol).FindControl("hidUser")).Value.Trim().ToString() == "" ? "0" : ((HtmlInputHidden)((Epower.ITSM.Web.Controls.UserPickerMult)ctrol).FindControl("hidUser")).Value.Trim().ToString();
                    ht.Add(strCtrID, upmid + "|" + ((HtmlInputHidden)((Epower.ITSM.Web.Controls.UserPickerMult)ctrol).FindControl("hidUserName")).Value.Trim().ToString());
                    break;
                case "controls_servicestaff_ascx":
                    string ssid = ((HtmlInputHidden)((Epower.ITSM.Web.Controls.ServiceStaff)ctrol).FindControl("hidUser")).Value.Trim().ToString() == "" ? "0" : ((HtmlInputHidden)((Epower.ITSM.Web.Controls.ServiceStaff)ctrol).FindControl("hidUser")).Value.Trim().ToString();
                    ht.Add(strCtrID, ssid + "|" + ((HtmlInputHidden)((Epower.ITSM.Web.Controls.ServiceStaff)ctrol).FindControl("hidUserName")).Value.Trim().ToString());
                    break;
                case "controls_ctrequcatadroplist_ascx":
                    ht.Add(strCtrID, ((Epower.ITSM.Web.Controls.ctrEquCataDropList)ctrol).CatelogID.ToString());
                    break;
                case "controls_ctrflowformtext_ascx":
                    ht.Add(strCtrID, ((Epower.ITSM.Web.Controls.CtrFlowFormText)ctrol).Value);
                    break;
                case "controls_ctrflownumeric_ascx":
                    ht.Add(strCtrID, ((Epower.ITSM.Web.Controls.CtrFlowNumeric)ctrol).Value);
                    break;
                case "controls_ctrflowremark_ascx":
                    ht.Add(strCtrID, ((Epower.ITSM.Web.Controls.CtrFlowRemark)ctrol).Value);
                    break;
                case "controls_ctrflowcatadroplist_ascx":
                    ht.Add(strCtrID, ((Epower.ITSM.Web.Controls.ctrFlowCataDropList)ctrol).CatelogID.ToString());
                    break;
                case "controls_ctrtextdroplist_ascx":
                    ht.Add(strCtrID, ((Epower.ITSM.Web.Controls.CtrTextDropList)ctrol).Value);
                    break;
                case "controls_servicestaffmastcust_ascx":
                    string sscid = ((HtmlInputHidden)((Epower.ITSM.Web.Controls.ServiceStaffMastCust)ctrol).FindControl("hidUser")).Value.Trim().ToString() == "" ? "0" : ((HtmlInputHidden)((Epower.ITSM.Web.Controls.ServiceStaffMastCust)ctrol).FindControl("hidUser")).Value.Trim().ToString();
                    ht.Add(strCtrID, sscid + "|" + ((HtmlInputHidden)((Epower.ITSM.Web.Controls.ServiceStaffMastCust)ctrol).FindControl("hidUserName")).Value.Trim().ToString());
                    break;
                case "controls_deptpicker_ascx":
                    string dpid = ((HtmlInputHidden)((Epower.ITSM.Web.Controls.DeptPicker)ctrol).FindControl("hidDept")).Value.Trim().ToString() == "" ? "0" : ((HtmlInputHidden)((Epower.ITSM.Web.Controls.DeptPicker)ctrol).FindControl("hidDept")).Value.Trim().ToString();
                    ht.Add(strCtrID, dpid + "|" + ((HtmlInputHidden)((Epower.ITSM.Web.Controls.DeptPicker)ctrol).FindControl("hidDeptName")).Value.Trim().ToString());
                    break;
                case "DropDownList":
                    ht.Add(strCtrID, ((DropDownList)ctrol).SelectedValue);
                    break;
                case "TextBox":
                    ht.Add(strCtrID, ((TextBox)ctrol).Text);
                    break;
                case "CheckBox":
                    ht.Add(strCtrID, ((CheckBox)ctrol).Checked.ToString());
                    break;
                case "ListBox":
                    ListBox lbl = (ListBox)ctrol;
                    string lbllist = "";
                    foreach (ListItem li in lbl.Items)
                    {
                        if (li.Selected)
                        {
                            lbllist += li.Value;
                        }
                    }
                    ht.Add(strCtrID, lbllist);
                    break;
                case "CheckBoxList":
                    CheckBoxList cbl = (CheckBoxList)ctrol;
                    string cbllist = "";
                    foreach (ListItem li in cbl.Items)
                    {
                        if (li.Selected)
                        {
                            cbllist += li.Value;
                        }
                    }
                    ht.Add(strCtrID, cbllist);
                    break;
                case "RadioButton":
                    ht.Add(strCtrID, ((RadioButton)ctrol).Checked.ToString());
                    break;
                case "RadioButtonList":
                    RadioButtonList rbl = (RadioButtonList)ctrol;
                    string rbllist = "";
                    foreach (ListItem li in rbl.Items)
                    {
                        if (li.Selected)
                        {
                            rbllist += li.Value;
                        }
                    }
                    ht.Add(strCtrID, rbllist);
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region 给控件赋值

        /// <summary>
        /// 给控件附值,使用注意事项[放在在查询条件控件邦定之后，查询结果集之前的位置]
        /// </summary>
        /// <param name="arrCtr"></param>
        /// <param name="ht"></param>
        /// <param name="PageID"></param>
        public static void SetPageQueryParam(Control[] arrCtr, Control pgControl, string PageID)
        {
            if (HttpContext.Current.Session["PageQueryParm"] != null)
            {
                System.Collections.Hashtable ht = (System.Collections.Hashtable)HttpContext.Current.Session["PageQueryParm"];
                if (ht["PageID"] != null && ht["PageID"].ToString() == PageID)
                {
                    if (pgControl != null)
                    {
                        string strType = pgControl.GetType().Name;
                        string strCtrID = pgControl.ClientID;
                        switch (strType)
                        {

                            case "controls_controlpage_ascx":
                                int currpage = 1;
                                if (ht[strCtrID + "_CurrentPage"] != null && !string.IsNullOrEmpty(ht[strCtrID + "_CurrentPage"].ToString()))
                                {
                                    currpage = int.Parse(ht[strCtrID + "_CurrentPage"].ToString());
                                }
                                int pagesize = 5;
                                if (ht[strCtrID + "_PageSize"] != null && !string.IsNullOrEmpty(ht[strCtrID + "_PageSize"].ToString()))
                                {
                                    currpage = int.Parse(ht[strCtrID + "_PageSize"].ToString());
                                }
                                ((Epower.ITSM.Web.Controls.ControlPage)pgControl).CurrentPage = currpage;
                                ((Epower.ITSM.Web.Controls.ControlPage)pgControl).PageSize = pagesize;
                                break;
                            case "controls_controlpagefoot_ascx":
                                int currpage2 = 1;
                                if (ht[strCtrID + "_CurrentPage"] != null && !string.IsNullOrEmpty(ht[strCtrID + "_CurrentPage"].ToString()))
                                {
                                    currpage2 = int.Parse(ht[strCtrID + "_CurrentPage"].ToString());
                                }
                                int pagesize2 = 5;
                                if (ht[strCtrID + "_PageSize"] != null && !string.IsNullOrEmpty(ht[strCtrID + "_PageSize"].ToString()))
                                {
                                    pagesize2 = int.Parse(ht[strCtrID + "_PageSize"].ToString());
                                }
                                //int pagesize = string.IsNullOrEmpty(ht[strCtrID + "_CurrentPage"].ToString()) ? 1 : int.Parse(ht[strCtrID + "_CurrentPage"].ToString());
                                ((Epower.ITSM.Web.Controls.ControlPageFoot)pgControl).CurrentPage = currpage2;
                                ((Epower.ITSM.Web.Controls.ControlPageFoot)pgControl).PageSize = pagesize2;// int.Parse(ht[strCtrID + "_PageSize"].ToString());
                                break;
                            default:
                                break;
                        }
                    }
                    for (int i = 0; i < arrCtr.Length; i++)
                    {
                        SetControlValue(arrCtr[i], ht);
                        if (arrCtr[i].HasControls())
                        {
                            SetPageQueryParam(arrCtr[i], ht, PageID);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 给控件附值
        /// </summary>
        /// <param name="pControl"></param>
        /// <param name="ht"></param>
        /// <param name="PageID"></param>
        private static void SetPageQueryParam(Control pControl, System.Collections.Hashtable ht, string PageID)
        {
            if (ht != null)
            {
                if (ht["PageID"].ToString() == PageID)
                {
                    foreach (Control psubControl in pControl.Controls)  /*遍历所有子节点*/
                    {
                        SetControlValue(psubControl, ht);
                    }
                }
                for (int i = 0; i < pControl.Controls.Count; i++)
                {
                    foreach (Control psubControl in pControl.Controls[i].Controls)  /*遍历所有子节点*/
                    {
                        if (psubControl.HasControls())
                        {
                            SetPageQueryParam(psubControl, ht, PageID);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 给控件附值
        /// </summary>
        /// <param name="ctrol"></param>
        /// <param name="ht"></param>
        private static void SetControlValue(Control ctrol, System.Collections.Hashtable ht)
        {
            string strType = ctrol.GetType().Name;
            string strCtrID = ctrol.ClientID;
            switch (strType)
            {
                case "controls_ctrdateandtime_ascx":
                    ((Epower.ITSM.Web.Controls.CtrDateAndTime)ctrol).dateTime = DateTime.Parse(ht[strCtrID].ToString());
                    break;
                case "controls_userpicker_ascx":
                    ((Epower.ITSM.Web.Controls.UserPicker)ctrol).UserID = long.Parse(ht[strCtrID].ToString() == "" ? "0" : ht[strCtrID].ToString().Split('|')[0]);
                    ((Epower.ITSM.Web.Controls.UserPicker)ctrol).UserName = ht[strCtrID].ToString() == "" ? "0" : ht[strCtrID].ToString().Split('|')[1];
                    break;
                case "controls_userpickermult_ascx":
                    ((Epower.ITSM.Web.Controls.UserPickerMult)ctrol).UserID = ht[strCtrID].ToString() == "" ? "0" : ht[strCtrID].ToString().Split('|')[0]; ;
                    ((Epower.ITSM.Web.Controls.UserPickerMult)ctrol).UserName = ht[strCtrID].ToString() == "" ? "0" : ht[strCtrID].ToString().Split('|')[1]; ;
                    break;
                case "controls_servicestaff_ascx":
                    ((Epower.ITSM.Web.Controls.ServiceStaff)ctrol).UserID = long.Parse(ht[strCtrID].ToString() == "" ? "0" : ht[strCtrID].ToString().Split('|')[0]);
                    ((Epower.ITSM.Web.Controls.ServiceStaff)ctrol).UserName = ht[strCtrID].ToString() == "" ? "0" : ht[strCtrID].ToString().Split('|')[1];
                    break;
                case "controls_ctrequcatadroplist_ascx":
                    ((Epower.ITSM.Web.Controls.ctrEquCataDropList)ctrol).CatelogID = long.Parse(ht[strCtrID].ToString() == "" ? "0" : ht[strCtrID].ToString());
                    break;
                case "controls_ctrflowformtext_ascx":
                    ((Epower.ITSM.Web.Controls.CtrFlowFormText)ctrol).Value = ht[strCtrID].ToString();
                    break;
                case "controls_ctrflownumeric_ascx":
                    ((Epower.ITSM.Web.Controls.CtrFlowNumeric)ctrol).Value = ht[strCtrID].ToString();
                    break;
                case "controls_ctrflowremark_ascx":
                    ((Epower.ITSM.Web.Controls.CtrFlowRemark)ctrol).Value = ht[strCtrID].ToString();
                    break;
                case "controls_ctrflowcatadroplist_ascx":
                    ((Epower.ITSM.Web.Controls.ctrFlowCataDropList)ctrol).CatelogID = long.Parse(ht[strCtrID].ToString() == "" ? "0" : ht[strCtrID].ToString());
                    break;
                case "controls_ctrtextdroplist_ascx":
                    ((Epower.ITSM.Web.Controls.CtrTextDropList)ctrol).Value = ht[strCtrID].ToString();
                    break;
                case "controls_servicestaffmastcust_ascx":
                    ((Epower.ITSM.Web.Controls.ServiceStaffMastCust)ctrol).UserID = ht[strCtrID].ToString().Split('|')[0];
                    ((Epower.ITSM.Web.Controls.ServiceStaffMastCust)ctrol).UserName = ht[strCtrID].ToString().Split('|')[1];
                    break;
                case "controls_deptpicker_ascx":
                    ((Epower.ITSM.Web.Controls.DeptPicker)ctrol).DeptID = long.Parse(ht[strCtrID].ToString() == "" ? "0" : ht[strCtrID].ToString().Split('|')[0]);
                    ((Epower.ITSM.Web.Controls.DeptPicker)ctrol).DeptName = ht[strCtrID].ToString() == "" ? "0" : ht[strCtrID].ToString().Split('|')[1];
                    break;
                case "DropDownList":
                    ((DropDownList)ctrol).SelectedIndex = ((DropDownList)ctrol).Items.IndexOf(((DropDownList)ctrol).Items.FindByValue(ht[strCtrID].ToString()));
                    break;
                case "TextBox":
                    ((TextBox)ctrol).Text = ht[strCtrID].ToString();
                    break;
                case "CheckBox":
                    ((CheckBox)ctrol).Checked = bool.Parse(ht[strCtrID].ToString());
                    break;
                case "ListBox":
                    ListBox lbl = (ListBox)ctrol;
                    string lbllist = ht[strCtrID].ToString();
                    string[] arrlbl = lbllist.Split(',');
                    foreach (ListItem li in lbl.Items)
                    {
                        foreach (string s in arrlbl)
                        {
                            if (li.Value == s)
                            {
                                li.Selected = true;
                                break;
                            }
                        }
                    }
                    break;
                case "CheckBoxList":
                    CheckBoxList cbl = (CheckBoxList)ctrol;
                    string cbllist = ht[strCtrID].ToString();
                    string[] arrcbl = cbllist.Split(',');
                    foreach (ListItem li in cbl.Items)
                    {
                        foreach (string s in arrcbl)
                        {
                            if (li.Value == s)
                            {
                                li.Selected = true;
                                break;
                            }
                        }
                    }
                    break;
                case "RadioButton":
                    ((RadioButton)ctrol).Checked = bool.Parse(ht[strCtrID].ToString());
                    break;
                case "RadioButtonList":
                    RadioButtonList rbl = (RadioButtonList)ctrol;
                    string rbllist = ht[strCtrID].ToString();
                    string[] arrrbl = rbllist.Split(',');
                    foreach (ListItem li in rbl.Items)
                    {
                        foreach (string s in arrrbl)
                        {
                            if (li.Value == s)
                            {
                                li.Selected = true;
                                break;
                            }
                        }
                    }
                    break;

                default:
                    break;
            }
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pControl"></param>
        private static void SetPageLanguage(Control pControl, DataTable dt)
        {
            string pstrID = string.Empty;
            string strType = string.Empty;
            string strTest = string.Empty;
            foreach (Control psubControl in pControl.Controls)  /*遍历所有子节点*/
            {
                strType = psubControl.GetType().Name;
                switch (strType)
                {
                    case "HtmlTable":
                    case "HtmlTableRow":
                    case "HtmlTableCell":
                    case "HtmlForm":
                    case "HtmlGenericControl":
                    case "ContentPlaceHolder":
                        SetPageLanguage(psubControl, dt);
                        break;

                    case "Literal":
                        DataRow[] drarr;
                        pstrID = ((Literal)psubControl).ID.ToString();
                        drarr = dt.Select("KeyName=" + Epower.DevBase.BaseTools.StringTool.SqlQ(pstrID));
                        if (drarr.Length > 0)
                        {
                            ((Literal)psubControl).Text = drarr[0]["KeyValue"].ToString();
                        }
                        break;
                    default:
                        //SetPageLanguage(psubControl, dt);
                        break;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pControl"></param>
        public static void ClearPageControls(Control pControl)
        {
            string pstrID = string.Empty;
            string strType = string.Empty;
            string strTest = string.Empty;
            foreach (Control psubControl in pControl.Controls)  /*遍历所有子节点*/
            {
                strType = psubControl.GetType().Name;
                switch (strType)
                {
                    case "HtmlTable":
                    case "HtmlTableRow":
                    case "HtmlTableCell":
                    case "HtmlForm":
                    case "HtmlGenericControl":
                    case "ContentPlaceHolder":
                        ClearPageControls(psubControl);
                        break;

                    case "TextBox":
                        ((TextBox)psubControl).Text = string.Empty;
                        break;
                    case "CheckBox":
                        ((CheckBox)psubControl).Checked = false;
                        break;
                    case "controls_ctrflowcatadroplistnew_ascx":
                        ((ctrFlowCataDropListNew)psubControl).RootID = 0;
                        break;
                    case "controls_ctrflownumeric_ascx":
                        ((CtrFlowNumeric)psubControl).Value = "";
                        break;
                    default:
                        //SetPageLanguage(psubControl, dt);
                        break;
                }
            }
        }

        public static string GetLanguageValue1(string sKey, string sGroup)
        {
            string sReturn = string.Empty;
            DataTable dt = new DataTable();
            dt = GetDataTable();
            string swhere = "column_Name=" + Epower.DevBase.BaseTools.StringTool.SqlQ(sKey.Trim()) + " and groups='" + sGroup + "'";
            DataRow[] arrdr = dt.Select(swhere);
            if (arrdr.Length > 0)
            {
                sReturn = arrdr[0]["KeyValue"].ToString();
            }
            return sReturn;
        }
    }
}
