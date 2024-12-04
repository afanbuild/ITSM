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
    public partial class CustSchemeCtr : System.Web.UI.UserControl
    {
        #region 属性定义区
        /// <summary>
        /// 设备分类编号 [注意两中用法只能取一种,要么设置 分类ID,要么直接设置 SCHEMA]
        /// </summary>
        public long BrCategoryID
        {
            set
            {
                ViewState[this.ID + "BrCategoryID"] = value;
                //回发时赋值 重新加载界面
                LoadHtmlControls(value);

            }
            get
            {
                if (ViewState[this.ID + "BrCategoryID"] == null)
                {
                    return 0;
                }
                else
                {
                    return (long)ViewState[this.ID + "BrCategoryID"];
                }
            }
        }

        /// <summary>
        /// 设备配置schema  [注意两中用法只能取一种,要么设置 分类ID,要么直接设置 SCHEMA]
        /// </summary>
        public string BrCategorySchema
        {
            set
            {
                ViewState[this.ID + "BrCategorySchema"] = value;
                //回发时赋值 重新加载界面
                LoadHtmlControlsForSchema(value);
            }
            get
            {
                if (ViewState[this.ID + "BrCategorySchema"] == null)
                {
                    return "";
                }
                else
                {
                    return ViewState[this.ID + "BrCategorySchema"].ToString();
                }
            }
        }
        private Hashtable _xmlHt = new Hashtable();
        private Hashtable _fields = new Hashtable();
        /// <summary>
        /// 控件返回的配置XML值

        /// </summary>
        public string ControlXmlValue
        {
            get
            {
                Hashtable ht = new Hashtable();
                ValueCollection(this, ref ht);
                return GetHashTableXmlValue(ht);
            }
            set
            {
                _xmlHt = SetHashTableXmlValue(value);
                ViewState[this.ID + "xmlHT"] = _xmlHt;
            }
        }

        private Hashtable _xmlHtComp = new Hashtable();
        private bool hasCompare = false;
        /// <summary>
        /// 控件XML值，用于比较区别,仅仅是只读状态下才用得上
        /// </summary>
        public string CompControlXmlValue
        {
            set
            {
                hasCompare = true;   //只要有过赋值 表示需要比较

                _xmlHtComp = SetHashTableXmlValue(value);
                ViewState[this.ID + "xmlHTComp"] = _xmlHtComp;
            }
        }
        private bool _blnReadOnly = false;
        /// <summary>
        /// 是否为只读状态

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

        #region 动态加载控件


        private void LoadHtmlControlsForSchema(string strXml)
        {
            Br_SchemaItemsDP ee = new Br_SchemaItemsDP();
            _fields = ee.GetAllFields();   //获取最新的配置项情况

            //回发情况下，清除前先保存一下之前控件的值

            if (Page.IsPostBack == true)
            {
                Hashtable ht = new Hashtable();
                ValueCollection(this, ref ht);
                _xmlHt = ht;
            }
            //清除现有所有控件

            this.Controls.Clear();
            if (strXml != "")
            {
                //由于做了清除需要重新注册脚本

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
            strXml = Epower.ITSM.SqlDAL.Br_SubjectDP.GetCatalogSchema(lngID);
            Br_SchemaItemsDP ee = new Br_SchemaItemsDP();
            _fields = ee.GetAllFields();   //获取最新的配置项情况

            //回发情况下，清除前先保存一下之前控件的值

            if (Page.IsPostBack == true)
            {
                Hashtable ht = new Hashtable();
                ValueCollection(this, ref ht);
                _xmlHt = ht;
            }
            //清除现有所有控件

            this.Controls.Clear();
            if (strXml != "")
            {
                //由于做了清除需要重新注册脚本

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
        /// 是否当前在编辑状态下更改了分类。 [这种情况下，对于没有存在原来取值的情况下，获取配置的缺省设置] 不需要viewstate 保存
        /// </summary>
        public void SetChangeCatalogTrue()
        {
            _blnChangeCatalog = true;
        }

        /// <summary>
        /// 是否当前是否为新增分类。 [在没有原来取值情况下，获取配置的缺省设置] 不需要viewstate 保存
        /// </summary>
        public void SetAddEquTrue()
        {
            _blnAddEqu = true;
        }

        private void RegisterClientScrip()
        {
            if (Page.ClientScript.IsClientScriptBlockRegistered("ShowTableDym") == false)
                Page.ClientScript.RegisterClientScriptBlock(GetType(), "ShowTableDym", @"function ShowTableDym(imgCtrl,imgTableID,tabName)
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
                                                        }", true);

        }

        #endregion

        #region xml与hashtable之间转换函数
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


        #region 递归获取控件的值

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


        #region 填加控件 私有方法
        private void AddRelationSchemeControls(XmlNode relnode, int iRel)
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

            //添加标题
            tr.ID = tabMain.ID + "RelTR";
            tr.Attributes.Add("class", "listTitleRightNew");
            tc.ID = tabMain.ID + "RelTD";
            tc.Attributes.Add("align", "left");
            tc.Attributes.Add("vAlign", "top");
            tc.Attributes.Add("class", "bt_di");
            if (relnode.Attributes["Title"] != null)
            {
                im.ID = tabMain.ID + "imgBase";
                im.Attributes.Add("class", "icon");
                im.Attributes.Add("onclick", "ShowTableDym(this,'" + this.ClientID + "_" + tabMain.ClientID + "','RelTab');");
                im.Attributes.Add("height", "16");
                im.Attributes.Add("width", "16");
                im.Attributes.Add("align", "absbottom");
                im.Attributes.Add("src", "../Images/icon_collapseall.gif");
                AddControl(tc, im);

                lab1.ID = tabMain.ID + "Title";
                lab1.Text = relnode.Attributes["Title"].Value;

                AddControl(tc, lab1);
            }

            AddControl(tr, tc);
            AddControl(tabMain, tr);
            AddControl(this, tabMain);
            //内容
            tabMain = new HtmlTable();
            tabMain.ID = "RelTabContent" + iRel.ToString();
            tabMain.Attributes.Add("width", "100%");
            tabMain.Attributes.Add("align", "center");
            tabMain.Attributes.Add("class", "listContent");
            int iLab = 0;
            //添加新行 
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
                string sSetValue = "";   //设置的值

                string sSetValueComp = ""; //用于比较的值

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
                            //新建 或 编辑状态下 更改过 分类
                            sSetValue = n.Attributes["Default"].Value;
                        }
                    }
                    CheckBox chkTemp = new CheckBox();
                    chkTemp.ID = "tDynamicchk" + sID;
                    if (this.ReadOnly == false)
                    {   //只读情况下 另外加一个链接表示名称

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
                        //只读情况下填加 查看相同配置的链接


                        HyperLink lnkTemp = new HyperLink();
                        lnkTemp.Text = sCHName;

                        iLab++;
                        lnkTemp.ID = tr.ID + "lnk" + iLab.ToString();

                        //找不到同时也需要比较的情况[新加的配置等情况]
                        if (hasCompare == true)
                        {
                            if (_xmlHtComp[sID] != null)
                            {
                                //存在历史比较值的情况下，如果不同，则红色显示
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
                    labTemp.Text = sCHName;
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

            //添加标题
            tr.ID = tabMain.ID + "BaseTR";
            tc.ID = tabMain.ID + "BaseTD";
            tc.Attributes.Add("align", "left");
            tc.Attributes.Add("vAlign", "top");
            tc.Attributes.Add("class", "listTitleRightNew");

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

            //内容
            tabMain = new HtmlTable();

            tabMain.ID = "BaseTabContent";
            tabMain.Attributes.Add("width", "100%");
            tabMain.Attributes.Add("align", "center");
            tabMain.Attributes.Add("class", "listContent");

            int iRow = 0;
            int iCell = 0;
            int iLab = 0;
            foreach (XmlNode node in bnodes)//分组
            {
                ns = node.SelectNodes("AttributeItem");
                int itemCount = ns.Count / 3 + (ns.Count % 3 == 0 ? 0 : 1);
                //添加新行 
                iRow++;
                tr = new HtmlTableRow();
                tr.ID = "BaseTab" + iRow.ToString();
                iCell++;
                tc = new HtmlTableCell();
                tc.ID = tr.ID + "_Cell" + iCell.ToString();
                tc.Attributes.Add("class", "listTitleRight");
                tc.Attributes.Add("style", "word-break:break-all;width:12%;");
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



                foreach (XmlNode n in ns)//分组项
                {
                    string sID = "";
                    string sSetValue = "";   //设置的值

                    string sSetValueComp = ""; //用于比较的值

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
                                //新建 或 编辑状态下 更改过 分类
                                sSetValue = n.Attributes["Default"].Value;
                            }
                        }

                        iLab++;
                        Label labTemp;

                        if (this.ReadOnly == true)
                        {
                            //只读情况下填加 查看相同配置的链接

                            HyperLink lnkTemp = new HyperLink();
                            lnkTemp.Text = sCHName + ":";
                            //lnkTemp.ToolTip = "查看配置项［" + lnkTemp.Text + "］相同的资产列表";

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
                        ctdl.FieldsSourceType = 1;   //异步获取方式
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


                            //找不到同时也需要比较的情况[新加的配置等情况]
                            if (hasCompare == true)
                            {
                                if (_xmlHtComp[sID] != null)
                                {
                                    //存在历史比较值的情况下，如果不同，则红色显示
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
            foreach (XmlNode node in bnodes)//分组
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
            foreach (XmlNode node in remarkNodes)//分组
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



         
            //添加标题
            tr.ID = tabMain.ID + "RemarkTR";
            tr.Attributes.Add("class", "listTitleRightNew");
            tc.ID = tabMain.ID + "RemarkTD";
            tc.Attributes.Add("align", "left");
            tc.Attributes.Add("vAlign", "top");
            tc.Attributes.Add("class", "bt_di");
            if (relnode.Attributes["Title"] != null)
            {
                im.ID = tabMain.ID + "imgRemark";
                im.Attributes.Add("class", "icon");
                im.Attributes.Add("onclick", "ShowTableDym(this,'" + this.ClientID + "_" + tabMain.ClientID + "','RemarkTab');");
                im.Attributes.Add("height", "16");
                im.Attributes.Add("width", "16");
                im.Attributes.Add("align", "absbottom");
                im.Attributes.Add("src", "../Images/icon_collapseall.gif");
                AddControl(tc, im);

                lab1.ID = tabMain.ID + "Title";
                lab1.Text = relnode.Attributes["Title"].Value;

                AddControl(tc, lab1);
            }

            AddControl(tr, tc);
            AddControl(tabMain, tr);
            AddControl(this, tabMain);
            //内容
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



            //添加标题
            tr.ID = tabMain.ID + "RelTR";
            tr.Attributes.Add("class", "listTitleRightNew");
            tc.ID = tabMain.ID + "RelTD";
            tc.Attributes.Add("align", "left");
            tc.Attributes.Add("vAlign", "top");
            tc.Attributes.Add("class", "bt_di");
            if (relnode.Attributes["Title"] != null)
            {
                im.ID = tabMain.ID + "imgBase";
                im.Attributes.Add("class", "icon");
                im.Attributes.Add("onclick", "ShowTableDym(this,'" + this.ClientID + "_" + tabMain.ClientID + "','RelTab');");
                im.Attributes.Add("height", "16");
                im.Attributes.Add("width", "16");
                im.Attributes.Add("align", "absbottom");
                im.Attributes.Add("src", "../Images/icon_collapseall.gif");
                AddControl(tc, im);

                lab1.ID = tabMain.ID + "Title";
                lab1.Text = relnode.Attributes["Title"].Value;

                AddControl(tc, lab1);
            }

            AddControl(tr, tc);
            AddControl(tabMain, tr);
            AddControl(this, tabMain);
            //内容
            tabMain = new HtmlTable();
            tabMain.ID = "RelTabContent" + iRel.ToString();
            tabMain.Attributes.Add("width", "100%");
            tabMain.Attributes.Add("align", "center");
            tabMain.Attributes.Add("class", "listContent");
            int iLab = 0;
            ns = relnode.SelectNodes("AttributeItem");
            int rowcount = 0;
            int width = ns.Count == 1 ? 82 : 35;
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
                    width = 82;
                }
                AddBaseRow(ref tr, ref rowcount, n, ref iLab, width);
                AddControl(tabMain, tr);
            }
            AddControl(this, tabMain);
        }

        private void AddBaseRow(ref HtmlTableRow tr, ref int rowcount, XmlNode n, ref int iLab, int width)
        {
            HtmlTableCell tc = null;
            tc = new HtmlTableCell();
            tc.ID = tr.ID + "_Cell" + rowcount.ToString();
            tc.Attributes.Add("class", "listTitleRight");
            tc.Attributes.Add("style", "word-break:break-all;width:12%;");
            string sID = "";
            string sSetValue = "";   //设置的值

            string sSetValueComp = ""; //用于比较的值

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
                        //新建 或 编辑状态下 更改过 分类
                        sSetValue = n.Attributes["Default"].Value;
                    }
                }

                iLab++;
                Label labTemp;


                CtrTextDropList ctdl = (Epower.ITSM.Web.Controls.CtrTextDropList)LoadControl("~/controls/CtrTextDropList.ascx");
                ctdl.FieldsSourceType = 1;   //异步获取方式
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


                    //找不到同时也需要比较的情况[新加的配置等情况]
                    if (hasCompare == true)
                    {
                        if (_xmlHtComp[sID] != null)
                        {
                            //存在历史比较值的情况下，如果不同，则红色显示
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
                if (rowcount % 4 == 0)
                {
                    tc.Attributes.Add("style", "word-break:break-all;");
                }
                else
                {
                    if (width != 82)
                    {
                        tc.Attributes.Add("style", "word-break:break-all;width:" + width.ToString() + "%");
                    }
                    else
                    {
                        tc.Attributes.Add("style", "word-break:break-all;");
                    }
                }



                if (width == 82)
                {
                    tc.ColSpan = 3;
                }

                AddControl(tc, ctdl);
                if (this.ReadOnly == true)
                {
                    tc.InnerText = sSetValue;

                }
                //tc.InnerText = sSetValue;
                AddControl(tr, tc);
            }
        }


        /// <summary>
        /// 备注框
        /// </summary>
        /// <param name="tr"></param>
        /// <param name="rowcount"></param>
        /// <param name="n"></param>
        /// <param name="iLab"></param>
        private void AddRemarkRow(ref HtmlTableRow tr, ref int rowcount, XmlNode n, ref int iLab)
        {
            HtmlTableCell tc = null;
            tc = new HtmlTableCell();
            tc.ID = tr.ID + "_Cell" + rowcount.ToString();
            tc.Attributes.Add("class", "listTitleRight");
            tc.Attributes.Add("style", "word-break:break-all;width:12%;");
            string sID = "";
            string sSetValue = "";   //设置的值

            string sSetValueComp = ""; //用于比较的值

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
                        //新建 或 编辑状态下 更改过 分类
                        sSetValue = n.Attributes["Default"].Value;
                    }
                }

                iLab++;
                Label labTemp;

                CtrTextDropList ctdl = (Epower.ITSM.Web.Controls.CtrTextDropList)LoadControl("~/controls/CtrTextDropList.ascx");
                ctdl.FieldsSourceType = 1;   //异步获取方式
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


                    //找不到同时也需要比较的情况[新加的配置等情况]
                    if (hasCompare == true)
                    {
                        if (_xmlHtComp[sID] != null)
                        {
                            //存在历史比较值的情况下，如果不同，则红色显示
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
                tc.Attributes.Add("style", "word-break:break-all;");
                AddControl(tc, ctdl);
                if (this.ReadOnly == true)
                {
                    tc.InnerText = sSetValue;

                }
                //tc.InnerText = sSetValue;
                AddControl(tr, tc);
            }
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
        #endregion
    }
}
