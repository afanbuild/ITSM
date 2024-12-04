/****************************************************************************
 * 
 * description:系统参数设置
 * 
 * 
 * 
 * Create by:
 * Create Date:2008-09-08
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
using System.Xml;

using Epower.ITSM.SqlDAL;

namespace Epower.ITSM.Web.Config
{
    public partial class frmSetSystemParams : BasePage
    {
        private string sConfigName = "SystemConfig";

        protected string NodesName
        {
            get 
            {
                if (Request["NodesName"] != null)
                    return Request["NodesName"].ToString();
                else
                    return "SetMail";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            string ConfigFile = HttpRuntime.AppDomainAppPath + "Config\\" + sConfigName + ".xml";

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(ConfigFile);
            XmlNode noderoot = xmlDoc.DocumentElement.SelectSingleNode("Items[@Name='" + NodesName + "' ]");

            if (Page.IsPostBack == false)
            {
                ltlTitle.Text = noderoot.Attributes["Title"].Value.Trim();
                ltlHelp.Text = noderoot.Attributes["ConfigContent"].Value.Trim();
            }

            LoadHtmlControls(noderoot);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button1_Click(object sender, EventArgs e)
        {
            Hashtable ht = new Hashtable();
            ValueCollection(tdDyTable, ref ht);

            SaveXmlFile(ht);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ht"></param>
        void SaveXmlFile(Hashtable ht)
        {
            string ConfigFile = HttpRuntime.AppDomainAppPath + "Config\\" + sConfigName + ".xml";

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(ConfigFile);

            XmlNode noderoot = xmlDoc.DocumentElement.SelectSingleNode("Items[@Name='" + NodesName + "' ]");

            XmlNodeList nodes = noderoot.SelectNodes("Item");


            foreach (XmlNode node in nodes)
            {
                //所有配置必须输入,为空则不保存
                if (node.Attributes["UserEnable"] != null && node.Attributes["UserEnable"].Value == "false")
                { }
                else
                {
                    if (ht[node.Attributes["Name"].Value].ToString().Trim().Length > 0)
                    {
                    
                        node.Attributes["Value"].Value = ht[node.Attributes["Name"].Value].ToString();
                    }
                }
            }

            xmlDoc.Save(ConfigFile);
            Epower.DevBase.BaseTools.PageTool.MsgBox(this, "数据保存成功！");
        }

        /// <summary>
        /// 递归获取控件的值
        /// </summary>
        /// <param name="ctrRoot"></param>
        /// <param name="list"></param>
        private void ValueCollection(Control ctrRoot, ref Hashtable list)
        {
            string strType = "";
            foreach (Control pControl in ctrRoot.Controls)  /*遍历所有子节点*/
            {
                if (pControl.ID != null)
                {

                    if (pControl.ID.StartsWith("tDynamic"))
                    {
                        strType = pControl.GetType().Name;
                        switch (strType)
                        {
                            case "HtmlTable":
                            case "HtmlTableRow":
                            case "HtmlTableCell":
                                ValueCollection(pControl, ref list);
                                break;
                            case "TextBox":
                                list.Add(((TextBox)pControl).Attributes["Tag"], ((TextBox)pControl).Text);
                                break;
                            case "DropDownList":
                                list.Add(((DropDownList)pControl).Attributes["Tag"], ((DropDownList)pControl).SelectedValue);
                                break;
                            case "CheckBox":
                                list.Add(((CheckBox)pControl).Attributes["Tag"], (((CheckBox)pControl).Checked == true ? "true" : "false"));
                                break;
                            case "controls_ctrdateandtime_ascx":
                                list.Add(((Epower.ITSM.Web.Controls.CtrDateAndTime)pControl).Attributes["Tag"], ((Epower.ITSM.Web.Controls.CtrDateAndTime)pControl).dateTimeString);
                                break;
                            default:
                                break;
                        }

                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xmlnode"></param>
        private void LoadHtmlControls(XmlNode xmlnode)
        {
            HtmlTable tab = new HtmlTable();
            tab.ID = "tDynamic";
            tab.Attributes.Add("width", "100%");
            tab.Attributes.Add("class", "listContent");
            HtmlTableRow tr = new HtmlTableRow();
            HtmlTableCell tc = new HtmlTableCell();

            XmlNodeList nodes = xmlnode.SelectNodes("Item");

            string sItemDesc = "";

            int iRow = 0;
            foreach (XmlNode node in nodes)
            {
                if (node.Attributes["UserEnable"] != null && node.Attributes["UserEnable"].Value == "false")
                {
                    continue;
                }
                iRow++;
                tr = new HtmlTableRow();
                tr.ID = "tDynamicRow" + iRow;
                AddControlByNode(node, iRow, ref tr);

                AddControl(tab, tr);

                sItemDesc = "";
                if (node.Attributes["ItemContent"] != null)
                    sItemDesc = node.Attributes["ItemContent"].Value;

                if (sItemDesc.Length > 0)
                {
                    //添加描述行
                    iRow++;
                    tr = new HtmlTableRow();
                    tr.ID = "tDyContectRow" + iRow;

                    tc = new HtmlTableCell();
                    tc.ID = "tDyContectTD_" + iRow;
                    tc.InnerText = sItemDesc;
                    tc.Attributes.Add("class", "list");
                    tc.Attributes.Add("colspan", "2");

                    AddControl(tr, tc);

                    AddControl(tab, tr);
                }
            }

            AddControl(tdDyTable, tab);


        }

        /// <summary>
        /// 给父控件上加子控件
        /// </summary>
        /// <param name="pParentControl">父控件对象</param>
        /// <param name="pSubControl">子控件对象</param>
        protected void AddControl(Control pParentControl, Control pSubControl)
        {
            if (pParentControl != null && pSubControl != null)
            {
                bool bFound = false;
                foreach (Control pControl in pParentControl.Controls)
                {
                    if (pControl.ID == pSubControl.ID)
                    {
                        bFound = true;
                        break;
                    }
                }
                if (!bFound) pParentControl.Controls.Add(pSubControl);
            }
        }

        /// <summary>
        /// 根据节点动态加载控件
        /// </summary>
        /// <param name="node"></param>
        /// <param name="iRow"></param>
        /// <param name="ctrl"></param>
        private void AddControlByNode(XmlNode node, int iRow, ref HtmlTableRow ctrl)
        {
            int iCell = 1;
            //添加标签TD
            string strNodeType = node.Attributes["ControlType"].Value;

            HtmlTableCell tc = new HtmlTableCell();
            tc.ID = "tTitleDynamicCell_" + iRow.ToString() + "_" + iCell.ToString();
            tc.Attributes.Add("nowrap", "nowrap");
            tc.Attributes.Add("align", "center");
            tc.Attributes.Add("class", "listTitle");
            tc.Attributes.Add("width", "30%");
            tc.InnerText = node.Attributes["Desc"].Value;

            AddControl(ctrl, tc);
            //添加控件TD及控件

            tc = new HtmlTableCell();
            iCell++;
            tc.ID = "tDynamicCell_" + iRow.ToString() + "_" + iCell.ToString();
            tc.Attributes.Add("nowrap", "nowrap");
            tc.Attributes.Add("align", "left");
            tc.Attributes.Add("class", "list");
            tc.Attributes.Add("width", "70%");


            Control control = GetSettingControl(node);
            if (control != null)
            {
                AddControl(tc, control);

                string sValidation = "";
                if (node.Attributes["ValidationExpression"] != null)
                    sValidation = node.Attributes["ValidationExpression"].Value;

                if (sValidation.Length > 0)
                {
                    //添加校验控件
                    RegularExpressionValidator contVal = new RegularExpressionValidator();
                    contVal.ControlToValidate = control.ID;
                    contVal.ID = "tdyValidate_" + iRow.ToString();
                    contVal.ErrorMessage = "请输入正确的格式";
                    contVal.ValidationExpression = sValidation;

                    AddControl(tc, contVal);
                }
            }
            AddControl(ctrl, tc);
        }

        /// <summary>
        ///  获取设置的控件
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private Control GetSettingControl(XmlNode node)
        {
            string strCtrlType = node.Attributes["ControlType"].Value;
            string strValue = node.Attributes["Value"].Value;
            Control control;
            switch (strCtrlType.ToLower())
            {
                case "text":
                    control = new TextBox();
                    control.ID = "tDynamic" + "_txt_" + node.Attributes["Name"].Value;
                    if (node.Attributes["TextMode"] != null)
                    {
                        ((TextBox)control).TextMode = TextBoxMode.Password;
                    }
                    ((TextBox)control).Width = new Unit("70%");
                    ((TextBox)control).Attributes.Add("Tag", node.Attributes["Name"].Value);
                    ((TextBox)control).Text = strValue;
                    break;
                case "checkbox":
                    control = new CheckBox();
                    control.ID = "tDynamic" + "_chk_" + node.Attributes["Name"].Value;
                    ((CheckBox)control).Attributes.Add("Tag", node.Attributes["Name"].Value);
                    ((CheckBox)control).Text = node.Attributes["Desc"].Value;
                    if (strValue.ToLower() == "false")
                    {
                        ((CheckBox)control).Checked = false;
                    }
                    else
                    {
                        ((CheckBox)control).Checked = true;
                    }
                    break;
                case "droplist":
                    control = new DropDownList();
                    control.ID = "tDynamic" + "_drp_" + node.Attributes["Name"].Value;
                    ((DropDownList)control).Attributes.Add("Tag", node.Attributes["Name"].Value);
                    ((DropDownList)control).Width = new Unit("70%");
                    string[] sItems = node.Attributes["Dict"].Value.Split(",".ToCharArray());
                    for (int i = 0; i < sItems.Length; i++)
                    {
                        string[] arr = sItems[i].Split("|".ToCharArray());
                        ((DropDownList)control).Items.Add(new ListItem(arr[1], arr[0]));
                    }
                    ((DropDownList)control).SelectedValue = strValue;
                    break;
                case "datetime":

                    control = (Epower.ITSM.Web.Controls.CtrDateAndTime)LoadControl("~/Controls/CtrDateAndTimeV2.ascx");
                    ((Epower.ITSM.Web.Controls.CtrDateAndTime)control).ShowTime = false;
                    control.ID = "tDynamic" + "_date_" + node.Attributes["Name"].Value;

                    ((Epower.ITSM.Web.Controls.CtrDateAndTime)control).Attributes.Add("Tag", node.Attributes["Name"].Value);
                    break;
                default:
                    control = null;
                    break;
            }
            return control;
        }
    }
}
