//********************************************************
// ͨ��Config����ά������
// ֧�� TEXT CHECKBOX ����ѡ�� ���ڵ�,����ͳһ��У�鷽ʽ
// 
//*********************************************************
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


namespace Epower.ITSM.Web.Common
{
    public partial class frmConfigSet : BasePage
    {
        private string sConfigName = "";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            sConfigName = Request.QueryString["configName"].Trim();

            string ConfigFile = HttpRuntime.AppDomainAppPath + "Config\\" + sConfigName + ".xml";

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(ConfigFile);

            SetParentButtonEvent();
            XmlNode noderoot = xmlDoc.DocumentElement.SelectSingleNode("Items[@Name='" + "SetMail" + "' ]");

            if (Page.IsPostBack == false)
            {
                Ctrtitle1.Title = noderoot.Attributes["Title"].Value.Trim();
            }

            LoadHtmlControls(noderoot);
            

        }

        #region ���ø����尴ť�¼�SetParentButtonEvent
        /// <summary>
        /// ���ø����尴ť�¼�
        /// </summary>
        protected void SetParentButtonEvent()
        {
            this.Master.Master_Button_Save_Click += new Global_BtnClick(Master_Master_Button_Save_Click);
            this.Master.ShowSaveButton(true);
        }

        void Master_Master_Button_Save_Click()
        {
            Hashtable ht = new Hashtable();
            ValueCollection(tdDyTable, ref ht);

            SaveXmlFile(ht);
        }

        void SaveXmlFile(Hashtable ht)
        {
            sConfigName = Request.QueryString["configName"].Trim();

            string strXmlTemp = "";

            string ConfigFile = HttpRuntime.AppDomainAppPath + "Config\\" + sConfigName + ".xml";

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(ConfigFile);

            XmlNode noderoot = xmlDoc.DocumentElement.SelectSingleNode("Items[@Name='" + "SetMail" + "' ]");

            XmlNodeList nodes = noderoot.SelectNodes("Item");


            foreach (XmlNode node in nodes)
            {
                //�������ñ�������,Ϊ���򲻱���
                if(ht[node.Attributes["Name"].Value].ToString().Trim().Length >0)
                    node.Attributes["Value"].Value = ht[node.Attributes["Name"].Value].ToString();
            }

            xmlDoc.Save(ConfigFile);

        }

        
        #endregion

        /// <summary>
        /// �ݹ��ȡ�ؼ���ֵ
        /// </summary>
        /// <param name="ctrRoot"></param>
        /// <param name="list"></param>
        private void ValueCollection(Control ctrRoot, ref Hashtable list)
        {
            string strType = "";
            foreach (Control pControl in ctrRoot.Controls)  /*���������ӽڵ�*/
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
                                list.Add(((CheckBox)pControl).Attributes["Tag"], (((CheckBox)pControl).Checked==true?"true":"false"));
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


        private void LoadHtmlControls(XmlNode xmlnode)
        {


            HtmlTable tab = new HtmlTable();
            tab.ID = "tDynamic";
            tab.Attributes.Add("width", "80%");
            tab.Attributes.Add("class", "tablebody");
            HtmlTableRow tr = new HtmlTableRow();
            HtmlTableCell tc = new HtmlTableCell();

            XmlNodeList nodes = xmlnode.SelectNodes("Item");

            string sConfigContent = "";
            if (xmlnode.Attributes["ConfigContent"] != null)
                sConfigContent = xmlnode.Attributes["ConfigContent"].Value;


            string sItemDesc = "";
           
            int iRow = 0;
            foreach (XmlNode node in nodes)
            {
                iRow++;
                tr = new HtmlTableRow();
                tr.ID = "tDynamicRow" + iRow;
                AddControlByNode(node,iRow,ref tr);

                AddControl(tab, tr);

                sItemDesc = "";
                if (node.Attributes["ItemContent"] != null)
                    sItemDesc = node.Attributes["ItemContent"].Value;

                if (sItemDesc.Length > 0)
                {
                    //���������
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

            if (sConfigContent.Length > 0)
            {
                iRow++;
                tr = new HtmlTableRow();
                tr.ID = "tDyContectRow" + iRow;

                tc = new HtmlTableCell();
                tc.ID = "tDyContectTD_" + iRow;
                tc.InnerText = "��������";
                tc.Attributes.Add("class", "listTitle");
                tc.Attributes.Add("colspan", "2");

                AddControl(tr, tc);

                AddControl(tab, tr);

                iRow++;
                tr = new HtmlTableRow();
                tr.ID = "tDyContectRow" + iRow;

                tc = new HtmlTableCell();
                tc.ID = "tDyContectTD_" + iRow;
                tc.InnerText = sConfigContent;
                tc.Attributes.Add("class", "list");
                tc.Attributes.Add("colspan", "2");

                AddControl(tr, tc);

                AddControl(tab, tr);
            }

            AddControl(tdDyTable, tab);

            
        }

        /// <summary>
        /// �����ؼ��ϼ��ӿؼ�
        /// </summary>
        /// <param name="pParentControl">���ؼ�����</param>
        /// <param name="pSubControl">�ӿؼ�����</param>
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
        /// ���ݽڵ㶯̬���ؿؼ�
        /// </summary>
        /// <param name="node"></param>
        /// <param name="iRow"></param>
        /// <param name="ctrl"></param>
        private void AddControlByNode(XmlNode node, int iRow,ref HtmlTableRow ctrl)
        {
            int iCell = 1;
            //��ӱ�ǩTD
            string strNodeType = node.Attributes["ControlType"].Value;
           
            
            
            HtmlTableCell tc = new HtmlTableCell();
            tc.ID = "tTitleDynamicCell_" + iRow.ToString() + "_" + iCell.ToString();
            tc.Attributes.Add("nowrap", "nowrap");
            tc.Attributes.Add("align", "center");
            tc.Attributes.Add("class", "listTitle");
            tc.Attributes.Add("width", "30%");
            tc.InnerText = node.Attributes["Desc"].Value;

            AddControl(ctrl, tc);
            //��ӿؼ�TD���ؼ�

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
                    //���У��ؼ�
                    RegularExpressionValidator contVal = new RegularExpressionValidator();
                    contVal.ControlToValidate = control.ID;
                    contVal.ID = "tdyValidate_" + iRow.ToString();
                    contVal.ErrorMessage = "��������ȷ�ĸ�ʽ";
                    contVal.ValidationExpression = sValidation;

                    AddControl(tc, contVal);
                }
            }
            
            AddControl(ctrl, tc);

            


        }


        /// <summary>
        ///  ��ȡ���õĿؼ�
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
                    ((TextBox)control).Width = new Unit("70%");
                    ((TextBox)control).Attributes.Add("Tag", node.Attributes["Name"].Value);
                    ((TextBox)control).Text = strValue;
                    break;
                case "checkbox":
                    control = new CheckBox();
                    control.ID = "tDynamic" + "_chk_" + node.Attributes["Name"].Value;
                    ((CheckBox)control).Attributes.Add("Tag", node.Attributes["Name"].Value);
                    ((CheckBox)control).Text =  node.Attributes["Desc"].Value;
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
