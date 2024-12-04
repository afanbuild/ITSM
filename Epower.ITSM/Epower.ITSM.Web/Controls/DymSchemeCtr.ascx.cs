using System;
using System.Data;
using System.Xml;
using System.IO;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using Epower.ITSM.Base;
using Epower.DevBase.BaseTools;
using Epower.ITSM.SqlDAL;

namespace Epower.ITSM.Web.Controls
{
    public partial class DymSchemeCtr : System.Web.UI.UserControl
    {

        #region ���Զ�����
        /// <summary>
        /// �豸������ [ע�������÷�ֻ��ȡһ��,Ҫô���� ����ID,Ҫôֱ������ SCHEMA]
        /// </summary>
        public long EquCategoryID
        {
            set {
                 ViewState[this.ID + "EquCategoryID"] = value;                
                     //�ط�ʱ��ֵ ���¼��ؽ���
                     LoadHtmlControls(value);
                 
            }
            get {
                if (ViewState[this.ID + "EquCategoryID"] == null)
                {
                    return 0;
                }
                else
                {
                    return (long)ViewState[this.ID + "EquCategoryID"];
                }
            }
        }

        /// <summary>
        /// �豸����schema  [ע�������÷�ֻ��ȡһ��,Ҫô���� ����ID,Ҫôֱ������ SCHEMA]
        /// </summary>
        public string EquCategorySchema
        {
            set
            {
                ViewState[this.ID + "EquCategorySchema"] = value;
                //�ط�ʱ��ֵ ���¼��ؽ���
                LoadHtmlControlsForSchema(value);
            }
            get
            {
                if (ViewState[this.ID + "EquCategorySchema"] == null)
                {
                    return "";
                }
                else
                {
                    return ViewState[this.ID + "EquCategorySchema"].ToString();
                }
            }
        }
        private Hashtable _xmlHt = new Hashtable();
        private Hashtable _fields = new Hashtable();
        /// <summary>
        /// �ؼ����ص�����XMLֵ
        /// </summary>
        public string ControlXmlValue
        {
            get
            {
                Hashtable ht = new Hashtable();
                ValueCollection(this, ref ht);
                return GetHashTableXmlValue(ht);
                return "";
            }
            set
            {
                _xmlHt = SetHashTableXmlValue(value);
                ViewState[this.ID + "xmlHT"] = _xmlHt;
                string xx = value;
            }
        }

        private Hashtable _xmlHtComp = new Hashtable();
        private bool hasCompare = false;
        /// <summary>
        /// �ؼ�XMLֵ�����ڱȽ�����,������ֻ��״̬�²��õ���
        /// </summary>
        public string CompControlXmlValue
        {           
            set
            {
                hasCompare = true;   //ֻҪ�й���ֵ ��ʾ��Ҫ�Ƚ�
                _xmlHtComp = SetHashTableXmlValue(value);
                ViewState[this.ID + "xmlHTComp"] = _xmlHtComp;
            }
        }
        private bool _blnReadOnly = false;
        /// <summary>
        /// �Ƿ�Ϊֻ��״̬
        /// </summary>
        public bool ReadOnly
        {
            set
            {
                ViewState[this.ID + "ReadOnly"] = value;
            }
            get
            {
                if (ViewState[this.ID + "ReadOnly"] == null)
                    return _blnReadOnly;
                else
                    return (bool)ViewState[this.ID + "ReadOnly"];
            }
        }       

        #endregion

        private bool _blnChangeCatalog = false;
        private bool _blnAddEqu = false;       

        XmlNode baseNode = null;
        XmlNodeList bnodes = null;
        XmlNodeList relnodes = null;
        XmlNodeList remarkNodes = null;
        protected void Page_Load(object sender, EventArgs e)
        {           
            if (Page.IsPostBack == false)
            {               
            }
            else
            {
                _xmlHt = (Hashtable)ViewState[this.ID + "xmlHT"];
            }
            //LoadHtmlControls(this.EquCategoryID);
        }
        
        #region ��̬���ؿؼ�

        private void LoadHtmlControlsForSchema(string strXml)
        {            
            Equ_SchemaItemsDP ee = new Equ_SchemaItemsDP();
            _fields = ee.GetAllFields();   //��ȡ���µ����������
            //�ط�����£����ǰ�ȱ���һ��֮ǰ�ؼ���ֵ
            if (Page.IsPostBack == true)
            {
                Hashtable ht = new Hashtable();
                ValueCollection(this, ref ht);
                _xmlHt = ht;
            }
            //����������пؼ�
            this.Controls.Clear();
            if (strXml != "")
            {
                //�������������Ҫ����ע��ű�
                RegisterClientScrip();
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.LoadXml(strXml);
                baseNode = xmldoc.DocumentElement;
                bnodes = xmldoc.SelectNodes("EquScheme/Base/BaseItem");
                relnodes = xmldoc.SelectNodes("EquScheme/RelationConfig");
                remarkNodes = xmldoc.SelectNodes("EquScheme/Remark");
                AddBaseSchemeControls();
                AddRemarkSchemeControls();
                int iRel = 0;
                foreach (XmlNode relnode in relnodes)
                {
                    iRel++;
                    AddRelationSchemeControls(relnode, iRel);
                }
            }
        }

        private void LoadHtmlControls(long lngID)
        {
            string strXml = "";
            strXml = Epower.ITSM.SqlDAL.Equ_SubjectDP.GetCatalogSchema(lngID);
            Equ_SchemaItemsDP ee = new Equ_SchemaItemsDP();
            _fields = ee.GetAllFields();   //��ȡ���µ����������
            //�ط�����£����ǰ�ȱ���һ��֮ǰ�ؼ���ֵ
            if (Page.IsPostBack == true)
            {
                Hashtable ht = new Hashtable();
                ValueCollection(this, ref ht);
                _xmlHt = ht;
            }
            //����������пؼ�
            this.Controls.Clear();
            if (strXml != "")
            {
                //�������������Ҫ����ע��ű�
                RegisterClientScrip();
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.LoadXml(strXml);
                baseNode = xmldoc.DocumentElement;
                bnodes = xmldoc.SelectNodes("EquScheme/Base/BaseItem");
                relnodes = xmldoc.SelectNodes("EquScheme/RelationConfig");
                AddBaseSchemeControls();
                remarkNodes = xmldoc.SelectNodes("EquScheme/Remark");
                AddRemarkSchemeControls();
                int iRel = 0;
                foreach (XmlNode relnode in relnodes)
                {
                    iRel++;
                    AddRelationSchemeControls(relnode, iRel);
                }
            }         
        }

        /// <summary>
        /// �Ƿ�ǰ�ڱ༭״̬�¸����˷��ࡣ [��������£�����û�д���ԭ��ȡֵ������£���ȡ���õ�ȱʡ����] ����Ҫviewstate ����
        /// </summary>
        public void  SetChangeCatalogTrue()
        {
            _blnChangeCatalog = true;
        }

        /// <summary>
        /// �Ƿ�ǰ�Ƿ�Ϊ�������ࡣ [��û��ԭ��ȡֵ����£���ȡ���õ�ȱʡ����] ����Ҫviewstate ����
        /// </summary>
        public void SetAddEquTrue()
        {
            _blnAddEqu = true;
        }

        private void RegisterClientScrip()
        {
            if(Page.ClientScript.IsClientScriptBlockRegistered("ShowTableDym") == false)
                Page.ClientScript.RegisterClientScriptBlock(GetType(),"ShowTableDym", @"function ShowTableDym(imgCtrl,imgTableID,tabName)
                                                        {
                                                              var ImgPlusScr =""../Images/icon_expandall.gif""	;      	// pic Plus  +
                                                              var ImgMinusScr =""../Images/icon_collapseall.gif""	;	    // pic Minus - 
                                                              var TableID = imgTableID.replace(tabName,tabName +""Content"");
                                                              var className;
                                                              var objectFullName;
                                                              var tableCtrl;
                                                              tableCtrl = document.getElementById(TableID);
                                                              if(imgCtrl.src.indexOf(""icon_expandall"") != -1)
                                                              {
                                                                tableCtrl.style.display ="""";
                                                                imgCtrl.src = ImgMinusScr ;
                                                              }
                                                              else
                                                              {
                                                                tableCtrl.style.display =""none"";
                                                                imgCtrl.src = ImgPlusScr ;		 
                                                              }
                                                        }",true);

        }

        #endregion

        #region xml��hashtable֮��ת������
        private string GetHashTableXmlValue(Hashtable ht)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement xmlRoot = xmlDoc.CreateElement("Fields");
            XmlElement xmlEle;           
            IDictionaryEnumerator myEnumerator = ht.GetEnumerator();
            while (myEnumerator.MoveNext())
            {
                xmlEle = xmlDoc.CreateElement("Field");
                xmlEle.SetAttribute("FieldName", myEnumerator.Key.ToString());
                xmlEle.SetAttribute("Value", myEnumerator.Value.ToString());
                xmlRoot.AppendChild(xmlEle);
            }
            xmlDoc.AppendChild(xmlRoot);
            return xmlDoc.InnerXml;
        }


        private Hashtable SetHashTableXmlValue(string strXml)
        {
            Hashtable ht = new Hashtable();
            string strTmp = "";
            try
            {

                XmlTextReader tr = new XmlTextReader(new StringReader(strXml));
                while (tr.Read())
                {
                    if (tr.Name == "Field" && tr.NodeType == XmlNodeType.Element)
                    {
                        strTmp = tr.GetAttribute("Value").Trim();
                        ht.Add(tr.GetAttribute("FieldName"), strTmp);
                    }
                }
                tr.Close();
            }
            catch
            {
            }
            return ht;
        }
        #endregion


        #region �ݹ��ȡ�ؼ���ֵ
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
                    strType = pControl.GetType().Name;
                    switch (strType)
                    {
                        case "HtmlTable":
                        case "HtmlTableRow":
                        case "HtmlTableCell":
                            ValueCollection(pControl, ref list);
                            break;

                        case "controls_ctrtextdroplist_ascx":
                            if (pControl.ID.StartsWith("tDynamic"))
                            {
                                list.Add(((CtrTextDropList)pControl).Attributes["Tag"], ((CtrTextDropList)pControl).Value);
                            }
                            break;
                            
                        case "CheckBox":
                            if (pControl.ID.StartsWith("tDynamic"))
                            {
                                list.Add(((CheckBox)pControl).Attributes["Tag"], ((CheckBox)pControl).Checked == true ? "1" : "0");
                            }
                            break;
                       
                        default:
                            break;
                    }                    
                }
            }
        }
        #endregion


        #region ��ӿؼ� ˽�з���
        private void AddRelationSchemeControls(XmlNode relnode,int iRel)
        {
            XmlNodeList ns = null;
            HtmlTable tabMain = new HtmlTable();
            HtmlTableRow tr = new HtmlTableRow();
            HtmlTableCell tc = new HtmlTableCell();
            HtmlImage im = new HtmlImage();
            Label lab1 = new Label();
           
            tabMain.ID = "RelTab" + iRel.ToString();
            tabMain.Attributes.Add("width", "100%");
            tabMain.Attributes.Add("align", "center");
            tabMain.Attributes.Add("class", "listNewContent");

            //��ӱ���
            tr.ID = tabMain.ID + "RelTR";
            tc.ID = tabMain.ID + "RelTD";
            tc.Attributes.Add("align", "left");
            tc.Attributes.Add("vAlign", "top");
            tc.Attributes.Add("class", "listTitleNew");
            if (relnode.Attributes["Title"] != null)
            {
                im.ID = tabMain.ID + "imgBase";
                im.Attributes.Add("class", "icon");
                im.Attributes.Add("onclick", "ShowTableDym(this,'" + this.ClientID + "_" + tabMain.ClientID + "','RelTab');");
                im.Attributes.Add("height", "16");
                im.Attributes.Add("width", "16");
                im.Attributes.Add("src", "../Images/icon_collapseall.gif");
                AddControl(tc, im);

                lab1.ID = tabMain.ID + "Title";
                lab1.Text = relnode.Attributes["Title"].Value;

                AddControl(tc, lab1);
            }

            AddControl(tr, tc);
            AddControl(tabMain, tr);
            AddControl(this, tabMain);
            //����
            tabMain = new HtmlTable();
            tabMain.ID = "RelTabContent" + iRel.ToString();
            tabMain.Attributes.Add("width", "100%");
            tabMain.Attributes.Add("align", "center");
            tabMain.Attributes.Add("class", "listContent");           
            int iLab = 0;          
            //������� 
            tr = new HtmlTableRow();
            tr.ID = tabMain.ID + "RelTab";
            tc = new HtmlTableCell();
            tc.ID = tr.ID + "_Cell";            
            tc.Attributes.Add("class", "list");
            tc.Attributes.Add("style", "word-break:break-all");
            ns = relnode.SelectNodes("AttributeItem");
            foreach (XmlNode n in ns)
            {
                string sID = "";
                string sSetValue = "";   //���õ�ֵ
                string sSetValueComp = ""; //���ڱȽϵ�ֵ
                string sCHName = "";
                sID = n.Attributes["ID"].Value;
                if (_fields[sID] != null)
                {
                    sCHName = _fields[sID].ToString();
                }
                if (sCHName != "")
                {
                    if (_xmlHt[sID] != null)
                        sSetValue = _xmlHt[sID].ToString();
                    if (sSetValue == "")
                    {
                        if (_blnAddEqu == true || _blnChangeCatalog == true)
                        {
                            //�½� �� �༭״̬�� ���Ĺ� ����
                            sSetValue = n.Attributes["Default"].Value;
                        }
                    }
                    CheckBox chkTemp = new CheckBox();
                    chkTemp.ID = "tDynamicchk" + sID;
                    if (this.ReadOnly == false)
                    {   //ֻ������� �����һ�����ӱ�ʾ����
                        chkTemp.Text = sCHName;
                    }
                    chkTemp.Checked = (sSetValue == "1" ? true : false);
                    chkTemp.Attributes.Add("Tag", sID);
                    if (this.ReadOnly == true)
                    {
                        chkTemp.Enabled = false;                        
                    }
                    AddControl(tc, chkTemp);
                    if (this.ReadOnly == true)
                    {
                        //ֻ���������� �鿴��ͬ���õ�����
                        HyperLink lnkTemp = new HyperLink();
                        lnkTemp.Text = sCHName;
                        if (sSetValue == "1")
                        {
                            lnkTemp.ToolTip = "�鿴���ã�" + lnkTemp.Text + "�ݵ������ʲ�";
                        }
                        else
                        {
                            lnkTemp.ToolTip = "�鿴û�����ã�" + lnkTemp.Text + "�ݵ������ʲ�";
                        }

                        lnkTemp.Attributes.Add("style", "CURSOR: hand");
                        lnkTemp.Attributes.Add("onclick", "javascript:window.open('../EquipmentManager/frmEqu_SameSchemaItem.aspx?ItemFieldID=" + sID.Trim() + "&ItemFieldValue='+encodeURIComponent('" + sSetValue.Trim() + "'),'newwindow','scrollbars=yes,resizable=yes,top=0,left=0,width=800px,height=600');");
                        iLab++;
                        lnkTemp.ID = tr.ID + "lnk" + iLab.ToString();

                        //�Ҳ���ͬʱҲ��Ҫ�Ƚϵ����[�¼ӵ����õ����]
                        if (hasCompare == true)
                        {
                            if (_xmlHtComp[sID] != null)
                            {
                                //������ʷ�Ƚ�ֵ������£������ͬ�����ɫ��ʾ
                                sSetValueComp = _xmlHtComp[sID].ToString();

                                if (sSetValueComp != sSetValue)
                                    lnkTemp.ForeColor = System.Drawing.Color.Red;
                            }
                            else
                            {
                                lnkTemp.ForeColor = System.Drawing.Color.Red;
                            }
                        }                      
                        AddControl(tc, lnkTemp);
                    }
                    //frmEqu_SameSchemaItem
                    Label labTemp = new Label();
                    labTemp.Text = "   ";
                    iLab++;
                    labTemp.ID = tr.ID + "lab" + iLab.ToString();
                    AddControl(tc, labTemp);
                }
            }            
            AddControl(tr, tc);
            AddControl(tabMain, tr);         
            AddControl(this, tabMain);
        }

        private void AddBaseSchemeControls2()
        {
            if (bnodes.Count == 0)
            {
                return;
            }
            XmlNodeList ns = null;
            HtmlTable tabMain = new HtmlTable();
            HtmlTableRow tr = new HtmlTableRow();
            HtmlTableCell tc = new HtmlTableCell();
            HtmlImage im = new HtmlImage();
            Label lab1 = new Label();

            //��ӱ���
            tr.ID = tabMain.ID + "BaseTR";
            tc.ID = tabMain.ID + "BaseTD";
            tc.Attributes.Add("align", "left");
            tc.Attributes.Add("vAlign", "top");
            tc.Attributes.Add("class", "listTitleNew");

            tabMain.ID = "BaseTab";
            tabMain.Attributes.Add("width", "100%");
            tabMain.Attributes.Add("align", "center");

            im.ID = tabMain.ID + "imgBase";
            im.Attributes.Add("class", "icon");
            im.Attributes.Add("onclick", "ShowTableDym(this,'" + this.ClientID + "_" + tabMain.ClientID + "','BaseTab');");
            im.Attributes.Add("height", "16");
            im.Attributes.Add("width", "16");
            im.Attributes.Add("src", "../Images/icon_collapseall.gif");
            AddControl(tc, im);

            lab1.ID = tabMain.ID + "Title";
            lab1.Text = baseNode.Attributes["Title"].Value;
            AddControl(tc, lab1);

            AddControl(tr, tc);
            AddControl(tabMain, tr);
            AddControl(this, tabMain);

            //����
            tabMain = new HtmlTable();

            tabMain.ID = "BaseTabContent";
            tabMain.Attributes.Add("width", "100%");
            tabMain.Attributes.Add("align", "center");
            tabMain.Attributes.Add("class", "listContent");

            int iRow = 0;
            int iCell = 0;
            int iLab = 0;
            foreach (XmlNode node in bnodes)//����
            {
                ns = node.SelectNodes("AttributeItem");
                int itemCount = ns.Count / 3 + (ns.Count % 3 == 0 ? 0 : 1);
                //������� 
                iRow++;
                tr = new HtmlTableRow();
                tr.ID = "BaseTab" + iRow.ToString();
                iCell++;
                tc = new HtmlTableCell();
                tc.ID = tr.ID + "_Cell" + iCell.ToString();
                tc.Attributes.Add("class", "listTitle");
                tc.Attributes.Add("align", "right");
                tc.Attributes.Add("style", "width:10%;");
                //tc.RowSpan = itemCount;

                if (node.Attributes["Title"] != null)
                {
                    tc.InnerText = (node.Attributes["Title"].Value == "") ? "" : node.Attributes["Title"].Value + ":";
                }

                AddControl(tr, tc);

                tc = new HtmlTableCell();
                iCell++;
                tc.ID = tr.ID + "_Cell" + iCell.ToString();
                tc.Attributes.Add("class", "list");
                tc.Attributes.Add("style", "word-break:break-all;width:90%;");



                foreach (XmlNode n in ns)//������
                {
                    string sID = "";
                    string sSetValue = "";   //���õ�ֵ
                    string sSetValueComp = ""; //���ڱȽϵ�ֵ
                    string sCHName = "";

                    sID = n.Attributes["ID"].Value;

                    if (_fields[sID] != null)
                    {
                        sCHName = _fields[sID].ToString();
                    }

                    if (sCHName != "")
                    {

                        if (_xmlHt[sID] != null)
                            sSetValue = _xmlHt[sID].ToString();

                        if (sSetValue == "")
                        {
                            if (_blnAddEqu == true || _blnChangeCatalog == true)
                            {
                                //�½� �� �༭״̬�� ���Ĺ� ����
                                sSetValue = n.Attributes["Default"].Value;
                            }
                        }

                        iLab++;
                        Label labTemp;

                        if (this.ReadOnly == true)
                        {
                            //ֻ���������� �鿴��ͬ���õ�����
                            HyperLink lnkTemp = new HyperLink();
                            lnkTemp.Text = sCHName + ":";
                            lnkTemp.ToolTip = "�鿴�������" + lnkTemp.Text + "����ͬ���ʲ��б�";

                            lnkTemp.Attributes.Add("style", "CURSOR: hand");
                            lnkTemp.Attributes.Add("onclick", "javascript:window.open('../EquipmentManager/frmEqu_SameSchemaItem.aspx?ItemFieldID=" + sID.Trim() + "&ItemFieldValue='+encodeURIComponent('" + sSetValue.Trim() + "'),'newwindow','scrollbars=yes,resizable=yes,top=0,left=0,width=800px,height=600');");
                            iLab++;
                            lnkTemp.ID = tr.ID + "lnk" + iLab.ToString();
                            AddControl(tc, lnkTemp);

                        }
                        else
                        {
                            labTemp = new Label();
                            labTemp.ID = tr.ID + "lab" + iLab.ToString();
                            labTemp.Text = sCHName + ":";

                            AddControl(tc, labTemp);
                        }

                        CtrTextDropList ctdl = (Epower.ITSM.Web.Controls.CtrTextDropList)LoadControl("~/controls/CtrTextDropList.ascx");
                        ctdl.FieldsSourceType = 1;   //�첽��ȡ��ʽ
                        ctdl.FieldsSourceID = "SchemaItem_" + sID;
                        ctdl.ID = "tDynamictxt" + sID;
                        ctdl.Attributes.Add("Tag", sID);
                        ctdl.MaxLength = 500;
                        ctdl.Value = sSetValue;

                        iLab++;
                        labTemp = new Label();
                        labTemp.ID = tr.ID + "lab" + iLab.ToString();
                        labTemp.Text = sSetValue;


                        if (this.ReadOnly == true)
                        {
                            ctdl.Visible = false;


                            //�Ҳ���ͬʱҲ��Ҫ�Ƚϵ����[�¼ӵ����õ����]
                            if (hasCompare == true)
                            {
                                if (_xmlHtComp[sID] != null)
                                {
                                    //������ʷ�Ƚ�ֵ������£������ͬ�����ɫ��ʾ
                                    sSetValueComp = _xmlHtComp[sID].ToString();

                                    if (sSetValueComp != sSetValue)
                                        labTemp.ForeColor = System.Drawing.Color.Red;
                                }
                                else
                                {
                                    labTemp.ForeColor = System.Drawing.Color.Red;
                                }
                            }

                        }
                        else
                        {
                            labTemp.Visible = false;
                        }
                        AddControl(tc, labTemp);
                        AddControl(tc, ctdl);
                        labTemp = new Label();
                        labTemp.Text = "   ";
                        iLab++;
                        labTemp.ID = "lab" + iLab.ToString();
                        AddControl(tc, labTemp);

                    }
                }

                AddControl(tr, tc);
                AddControl(tabMain, tr);
            }

            AddControl(this, tabMain);
        }

        private void AddBaseSchemeControls()
        {
            if (bnodes.Count == 0)
            {
                return;
            }           
            int count = 0;
            foreach (XmlNode node in bnodes)//����
            {
                AddBaseItem(node, count);
                count++;
            }
        }

        private void AddRemarkSchemeControls()
        {
            if (remarkNodes.Count == 0)
            {
                return;
            }
            int count = 0;
            foreach (XmlNode node in remarkNodes)//����
            {
                AddRemarkItem(node, count);
                count++;
            }
        }

        private void AddRemarkItem(XmlNode relnode, int iRel)
        {
            XmlNodeList ns = null;
            HtmlTable tabMain = new HtmlTable();
            HtmlTableRow tr = new HtmlTableRow();
            HtmlTableCell tc = new HtmlTableCell();
            HtmlImage im = new HtmlImage();
            Label lab1 = new Label();

            tabMain.ID = "RemarkTab" + iRel.ToString();
            tabMain.Attributes.Add("width", "100%");
            tabMain.Attributes.Add("align", "center");
            tabMain.Attributes.Add("class", "listNewContent");

            //��ӱ���
            tr.ID = tabMain.ID + "RemarkTR";
            tc.ID = tabMain.ID + "RemarkTD";
            tc.Attributes.Add("align", "left");
            tc.Attributes.Add("vAlign", "top");
            tc.Attributes.Add("class", "listTitleNew");
            if (relnode.Attributes["Title"] != null)
            {
                im.ID = tabMain.ID + "imgRemark";
                im.Attributes.Add("class", "icon");
                im.Attributes.Add("onclick", "ShowTableDym(this,'" + this.ClientID + "_" + tabMain.ClientID + "','RemarkTab');");
                im.Attributes.Add("height", "16");
                im.Attributes.Add("width", "16");
                im.Attributes.Add("src", "../Images/icon_collapseall.gif");
                AddControl(tc, im);

                lab1.ID = tabMain.ID + "Title";
                lab1.Text = relnode.Attributes["Title"].Value;

                AddControl(tc, lab1);
            }

            AddControl(tr, tc);
            AddControl(tabMain, tr);
            AddControl(this, tabMain);
            //����
            tabMain = new HtmlTable();
            tabMain.ID = "RemarkTabContent" + iRel.ToString();
            tabMain.Attributes.Add("width", "100%");
            tabMain.Attributes.Add("align", "center");
            tabMain.Attributes.Add("class", "listContent");
            int iLab = 0;
            ns = relnode.SelectNodes("AttributeItem");
            int rowcount = 0;
            foreach (XmlNode n in ns)
            {
                rowcount++;
                tr = new HtmlTableRow();
                tr.ID = tabMain.ID + "RemarkTab" + rowcount.ToString();
                AddRemarkRow(ref tr, ref rowcount, n, ref iLab);
                AddControl(tabMain, tr);
            }
            AddControl(this, tabMain);
        }

        

        private void AddBaseItem(XmlNode relnode, int iRel)
        {
            XmlNodeList ns = null;
            HtmlTable tabMain = new HtmlTable();
            HtmlTableRow tr = new HtmlTableRow();
            HtmlTableCell tc = new HtmlTableCell();
            HtmlImage im = new HtmlImage();
            Label lab1 = new Label();

            tabMain.ID = "RelTab" + iRel.ToString();
            tabMain.Attributes.Add("width", "100%");
            tabMain.Attributes.Add("align", "center");
            tabMain.Attributes.Add("class", "listNewContent");

            //��ӱ���
            tr.ID = tabMain.ID + "RelTR";
            tc.ID = tabMain.ID + "RelTD";
            tc.Attributes.Add("align", "left");
            tc.Attributes.Add("vAlign", "top");
            tc.Attributes.Add("class", "listTitleNew");
            if (relnode.Attributes["Title"] != null)
            {
                im.ID = tabMain.ID + "imgBase";
                im.Attributes.Add("class", "icon");
                im.Attributes.Add("onclick", "ShowTableDym(this,'" + this.ClientID + "_" + tabMain.ClientID + "','RelTab');");
                im.Attributes.Add("height", "16");
                im.Attributes.Add("width", "16");
                im.Attributes.Add("src", "../Images/icon_collapseall.gif");
                AddControl(tc, im);

                lab1.ID = tabMain.ID + "Title";
                lab1.Text = relnode.Attributes["Title"].Value;

                AddControl(tc, lab1);
            }

            AddControl(tr, tc);
            AddControl(tabMain, tr);
            AddControl(this, tabMain);
            //����
            tabMain = new HtmlTable();
            tabMain.ID = "RelTabContent" + iRel.ToString();
            tabMain.Attributes.Add("width", "100%");
            tabMain.Attributes.Add("align", "center");
            tabMain.Attributes.Add("class", "listContent");
            int iLab = 0;           
            ns = relnode.SelectNodes("AttributeItem");
            int rowcount = 0;
            int width = ns.Count == 1 ? 90 : 40;
            foreach (XmlNode n in ns)
            {
                rowcount++;
                if (rowcount % 4 == 1)
                {
                    tr = new HtmlTableRow();
                    tr.ID = tabMain.ID + "RelTab" + rowcount.ToString();
                }
                if ((ns.Count * 2) % 4 != 0 && rowcount == (ns.Count * 2) - 1)
                {
                    width = 90;
                }
                AddBaseRow(ref tr, ref rowcount, n, ref iLab,width);
                AddControl(tabMain, tr);
            }             
            AddControl(this, tabMain);
        }

        private void AddBaseRow(ref HtmlTableRow tr, ref int rowcount, XmlNode n,ref int iLab,int width)
        {
            HtmlTableCell tc = null;
            tc = new HtmlTableCell();
            tc.ID = tr.ID + "_Cell" + rowcount.ToString();
            tc.Attributes.Add("class", "listTitle");
            tc.Attributes.Add("style", "word-break:break-all;width:10%;text-align:right;");
            string sID = "";
            string sSetValue = "";   //���õ�ֵ
            string sSetValueComp = ""; //���ڱȽϵ�ֵ
            string sCHName = "";

            sID = n.Attributes["ID"].Value;

            if (_fields[sID] != null)
            {
                sCHName = _fields[sID].ToString();
            }

            if (sCHName != "")
            {

                if (_xmlHt[sID] != null)
                    sSetValue = _xmlHt[sID].ToString();

                if (sSetValue == "")
                {
                    if (_blnAddEqu == true || _blnChangeCatalog == true)
                    {
                        //�½� �� �༭״̬�� ���Ĺ� ����
                        sSetValue = n.Attributes["Default"].Value;
                    }
                }

                iLab++;
                Label labTemp;

               
                CtrTextDropList ctdl = (Epower.ITSM.Web.Controls.CtrTextDropList)LoadControl("~/controls/CtrTextDropList.ascx");
                ctdl.FieldsSourceType = 1;   //�첽��ȡ��ʽ
                ctdl.FieldsSourceID = "SchemaItem_" + sID;
                ctdl.ID = "tDynamictxt" + sID;
                ctdl.Attributes.Add("Tag", sID);
                ctdl.MaxLength = 500;
                ctdl.Value = sSetValue;

                
                labTemp = new Label();
                labTemp.ID = tr.ID + "lab" + iLab.ToString();
                labTemp.Text = sCHName;


                if (this.ReadOnly == true)
                {
                    ctdl.Visible = false;


                    //�Ҳ���ͬʱҲ��Ҫ�Ƚϵ����[�¼ӵ����õ����]
                    if (hasCompare == true)
                    {
                        if (_xmlHtComp[sID] != null)
                        {
                            //������ʷ�Ƚ�ֵ������£������ͬ�����ɫ��ʾ
                            sSetValueComp = _xmlHtComp[sID].ToString();

                            if (sSetValueComp != sSetValue)
                                labTemp.ForeColor = System.Drawing.Color.Red;
                        }
                        else
                        {
                            labTemp.ForeColor = System.Drawing.Color.Red;
                        }
                    }
                }
                AddControl(tc, labTemp);
                AddControl(tr, tc);
                rowcount++;
                tc = new HtmlTableCell();
                tc.ID = tr.ID + "_Cell" + rowcount.ToString();
                tc.Attributes.Add("class", "list");
                tc.Attributes.Add("style", "word-break:break-all;width:"+ width.ToString() +"%");
                if (width == 90)
                {
                    tc.ColSpan = 3;
                }

                AddControl(tc, ctdl);
                if (this.ReadOnly == true)
                {
                    tc.InnerText = sSetValue;
                    tc.Attributes.Add("title", "�鿴�������" + sSetValue.Trim() + "����ͬ���ʲ��б�");
                    tc.Attributes.Add("style", "CURSOR: hand");
                    tc.Attributes.Add("onclick", "javascript:window.open('../EquipmentManager/frmEqu_SameSchemaItem.aspx?ItemFieldID=" + sID.Trim() + "&ItemFieldValue='+encodeURIComponent('" + sSetValue.Trim() + "'),'newwindow','scrollbars=yes,resizable=yes,top=0,left=0,width=800px,height=600');");
                }
                AddControl(tr, tc);
            }
        }

        private void AddRemarkRow(ref HtmlTableRow tr, ref int rowcount, XmlNode n, ref int iLab)
        {
            HtmlTableCell tc = null;
            tc = new HtmlTableCell();
            tc.ID = tr.ID + "_Cell" + rowcount.ToString();
            tc.Attributes.Add("class", "listTitle");
            tc.Attributes.Add("style", "word-break:break-all;width:10%;text-align:right;");
            string sID = "";
            string sSetValue = "";   //���õ�ֵ
            string sSetValueComp = ""; //���ڱȽϵ�ֵ
            string sCHName = "";

            sID = n.Attributes["ID"].Value;

            if (_fields[sID] != null)
            {
                sCHName = _fields[sID].ToString();
            }

            if (sCHName != "")
            {

                if (_xmlHt[sID] != null)
                    sSetValue = _xmlHt[sID].ToString();

                if (sSetValue == "")
                {
                    if (_blnAddEqu == true || _blnChangeCatalog == true)
                    {
                        //�½� �� �༭״̬�� ���Ĺ� ����
                        sSetValue = n.Attributes["Default"].Value;
                    }
                }

                iLab++;
                Label labTemp;

                CtrTextDropList ctdl = (Epower.ITSM.Web.Controls.CtrTextDropList)LoadControl("~/controls/CtrTextDropList.ascx");
                ctdl.FieldsSourceType = 1;   //�첽��ȡ��ʽ
                ctdl.TextMode = TextBoxMode.MultiLine;
                ctdl.FieldsSourceID = "SchemaItem_" + sID;
                ctdl.ID = "tDynamictxt" + sID;
                ctdl.Attributes.Add("Tag", sID);
                ctdl.Value = sSetValue;
                ctdl.MaxLength = 500;
                ctdl.Width = "100%";

                
                labTemp = new Label();
                labTemp.ID = tr.ID + "lab" + iLab.ToString();
                labTemp.Text = sCHName;


                if (this.ReadOnly == true)
                {
                    ctdl.Visible = false;


                    //�Ҳ���ͬʱҲ��Ҫ�Ƚϵ����[�¼ӵ����õ����]
                    if (hasCompare == true)
                    {
                        if (_xmlHtComp[sID] != null)
                        {
                            //������ʷ�Ƚ�ֵ������£������ͬ�����ɫ��ʾ
                            sSetValueComp = _xmlHtComp[sID].ToString();

                            if (sSetValueComp != sSetValue)
                                labTemp.ForeColor = System.Drawing.Color.Red;
                        }
                        else
                        {
                            labTemp.ForeColor = System.Drawing.Color.Red;
                        }
                    }
                }
                AddControl(tc, labTemp);
                AddControl(tr, tc);
                rowcount++;
                tc = new HtmlTableCell();
                tc.ID = tr.ID + "_Cell" + rowcount.ToString();
                tc.Attributes.Add("class", "list");
                tc.Attributes.Add("style", "word-break:break-all;width:90%");
                AddControl(tc, ctdl);
                if (this.ReadOnly == true)
                {
                    tc.InnerText = sSetValue;
                    tc.Attributes.Add("title", "�鿴�������" + sSetValue.Trim() + "����ͬ���ʲ��б�");
                    tc.Attributes.Add("style", "CURSOR: hand");
                    tc.Attributes.Add("onclick", "javascript:window.open('../EquipmentManager/frmEqu_SameSchemaItem.aspx?ItemFieldID=" + sID.Trim() + "&ItemFieldValue='+encodeURIComponent('" + sSetValue.Trim() + "'),'newwindow','scrollbars=yes,resizable=yes,top=0,left=0,width=800px,height=600');");
                }
                AddControl(tr, tc);
            }
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
        #endregion
    }
}
