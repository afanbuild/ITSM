using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Epower.ITSM.SqlDAL.ES_TBLCS;
using System.Xml;
using System.Collections.Generic;
using Epower.ITSM.SqlDAL;
using Epower.ITSM.Base;
using Epower.ITSM.SqlDAL.Customer;
using System.Text;
using System.Text.RegularExpressions;
using Epower.ITSM.Business.Common.Configuration;

namespace Epower.ITSM.Web.Controls
{
    public partial class Extension_DaySchemeCtrList : System.Web.UI.UserControl
    {
        #region 属性定义区

        /// <summary>
        /// 环节模型编号
        /// </summary>
        public long NodeModelID
        {
            get
            {
                if (ViewState[this.ID + "NodeModelID"] == null)
                    return 0;
                else
                    return long.Parse(ViewState[this.ID + "NodeModelID"].ToString());
            }
            set { ViewState[this.ID + "NodeModelID"] = value; }
        }

        /// <summary>
        /// 设备分类编号 [注意两中用法只能取一种,要么设置 分类ID,要么直接设置 SCHEMA]
        /// </summary>
        public long EquCategoryID
        {
            set
            {
                long lngFlowModelID = FlowDP.GetOFlowModelID(value);    // 取原始流程模型编号
                //回发时赋值 重新加载界面

                ViewState[this.ID + "EquCategoryID"] = lngFlowModelID;
                LoadHtmlControls(lngFlowModelID);

            }
            get
            {
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
        /// 设备配置schema  [注意两中用法只能取一种,要么设置 分类ID,要么直接设置 SCHEMA]
        /// </summary>
        public string EquCategorySchema
        {
            set
            {
                ViewState[this.ID + "EquCategorySchema"] = value;
                //回发时赋值 重新加载界面
                //LoadHtmlControlsForSchema(value);
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
        private Hashtable _xmlHt
        {
            get
            {
                if (ViewState[this.ID + "xmlHT"] == null)
                {
                    return new Hashtable();
                }
                else
                {
                    return (Hashtable)ViewState[this.ID + "xmlHT"];
                }
            }
            set
            {
                ViewState[this.ID + "xmlHT"] = value;
            }

        }

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
                return "";// GetHashTableXmlValue(ht);

            }
        }

        //控件返回的值

        public List<EQU_deploy> contorRtnValue
        {
            get
            {
                Hashtable ht = new Hashtable();
                ValueCollection(this, ref ht);
                return GetHashTableRtnValue(ht);
            }
        }

        /// <summary>
        /// 获得资产id
        /// </summary>
        public long EquID
        {
            set
            {
                ViewState["EquID"] = value;
                _xmlHt = SetHashTableXmlValue(value);
                if (value == 0)
                {
                    SetAddEquTrue();
                }
                ViewState[this.ID + "xmlHT"] = _xmlHt;
            }
        }

        //资产有比较值时 的 hashTable 
        private Hashtable _xmlHtComp
        {
            get
            {
                if (ViewState[this.ID + "xmlHTComp"] == null)
                {
                    return new Hashtable();
                }
                else
                {
                    return (Hashtable)ViewState[this.ID + "xmlHTComp"];
                }
            }
            set
            {
                ViewState[this.ID + "xmlHTComp"] = value;
            }

        }

        private bool hasCompare = false;
        /// <summary>
        /// 当有变更时，传入变更单编号

        /// </summary>
        public long FlowID
        {
            set
            {
                hasCompare = true;   //只要有过赋值 表示需要比较

                if (isChange)
                {
                    _xmlHt = SetChangeHashTableXmlValue(value, long.Parse(ViewState["EquID"].ToString()));
                    ViewState[this.ID + "xmlHT"] = _xmlHt;

                    _xmlHtComp = SetHashTableXmlValue(long.Parse(ViewState["EquID"].ToString()));
                    ViewState[this.ID + "xmlHTComp"] = _xmlHtComp;
                }
                else
                {
                    _xmlHt = SetChangeHashTableXmlValue(value, long.Parse(ViewState["EquID"].ToString()));
                    ViewState[this.ID + "xmlHT"] = _xmlHt;

                    _xmlHtComp = SetHashTablehostoryXmlValue(long.Parse(ViewState["EquID"].ToString()), value);
                    ViewState[this.ID + "xmlHTComp"] = _xmlHtComp;
                }
            }
        }


        /// <summary>
        /// 是否对历史进行比较 （只有在资产基线图时用到此属性）
        /// </summary>
        public bool isVersion
        {
            set
            {
                ViewState[this.ID + "isVersion"] = value;
            }
            get
            {
                if (ViewState[this.ID + "isVersion"] == null)
                {
                    return false;
                }
                else
                {
                    return (bool)ViewState[this.ID + "isVersion"];
                }


            }
        }
        /// <summary>
        /// 当非变更情况下，历史版本号之间的比较
        /// </summary>
        public long Version
        {
            set
            {
                hasCompare = true;

                _xmlHt = SetHashTableVersionXmlValue(long.Parse(ViewState["EquID"].ToString()), value);
                ViewState[this.ID + "xmlHT"] = _xmlHt;

                ViewState[this.ID + "Version"] = value;

                ///资产基线图时才用到比较

                if (isVersion)
                {
                    int VersionTopOne = int.Parse(value.ToString()) - 1;
                    _xmlHtComp = SetHashTableVersionXmlValue(long.Parse(ViewState["EquID"].ToString()), VersionTopOne);
                    ViewState[this.ID + "xmlHTComp"] = _xmlHtComp;
                }
            }
            get
            {
                if (ViewState[this.ID + "Version"] != null)
                {
                    return long.Parse(ViewState[this.ID + "Version"].ToString());
                }
                else
                {
                    return -1;
                }
            }

        }


        /// <summary>
        /// 是否正在变更中

        /// </summary>
        public bool isChange
        {
            set
            {
                ViewState[this.ID + "isChange"] = value;
            }
            get
            {
                if (ViewState[this.ID + "isChange"] == null)
                {
                    return false;
                }
                else
                {
                    return (bool)ViewState[this.ID + "isChange"];
                }


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
        XmlNodeList DropDownListNodes = null;//下拉控件

        XmlNodeList DeptNodes = null;//部门信息控件
        XmlNodeList UserNodes = null;//用户信息控件
        XmlNodeList TimeNodes = null;//日期类型控件
        XmlNodeList NumberNodes = null;//数值类型控件


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



        //private void LoadHtmlControls(long lngID)
        //{

        //    string strXml = "";

        //    int iRel = 0;
        //    BR_ExtendsFieldsDP br_extendsFields = new BR_ExtendsFieldsDP();
        //    DataTable dt = br_extendsFields.GetDataTable(decimal.Parse(lngID.ToString()));
        //    if (dt.Rows.Count > 0)
        //    {
        //        Br_ExtensionsItemsDP br_items = new Br_ExtensionsItemsDP();

        //        if (Version != -1)
        //        {
        //            //取历史值的情况
        //            _fields = br_items.GetAllFieldsHistory();
        //        }
        //        else
        //        {
        //            _fields = br_items.GetAllFields();   //获取最新的配置项情况

        //        }

        //        //清除现有所有控件
        //        this.Controls.Clear();


        //        #region 重新组装XML串 余向前 2013-11-19

        //        //由于存储的时候每一条扩展信息在数据库里是一条记录 ,需要把所有记录重新组装到一个XML串里
        //        //以避免显示的时候每条扩展信息占一行位置

        //        XmlDocument xmlDocNew = new XmlDocument();
        //        XmlElement xmlRoot = xmlDocNew.CreateElement("EquScheme");
        //        xmlRoot.SetAttribute("Title", "用户自定义扩展项");

        //        XmlElement xmlbn = xmlDocNew.CreateElement("BaseItem");
        //        xmlbn.SetAttribute("Title", "");
        //        XmlElement xmlrn = xmlDocNew.CreateElement("Remark");
        //        xmlrn.SetAttribute("Title", "");
        //        XmlElement xmlreln = xmlDocNew.CreateElement("RelationConfig");
        //        xmlreln.SetAttribute("Title", "");

        //        foreach (DataRow dr in dt.Rows)
        //        {
        //            string str = "";

        //            if (Version != -1)
        //            {
        //                //取历史版本的strXml
        //                str = Equ_DeskDP.isversionRtnXml(Version, long.Parse(ViewState["EquID"].ToString()));
        //                if (str == "")
        //                {
        //                    str = dr["KeyValue"].ToString();
        //                }
        //            }
        //            else
        //            {
        //                str = dr["KeyValue"].ToString();
        //            }

        //            XmlDocument xmldoc = new XmlDocument();
        //            xmldoc.LoadXml(str);

        //            XmlNodeList bn = xmldoc.SelectNodes("EquScheme/BaseItem");

        //            if (bn != null && bn.Count > 0)
        //            {
        //                foreach (XmlNode xn in bn)
        //                {
        //                    xmlbn.InnerXml += xn.InnerXml;
        //                }

        //            }
        //            XmlNodeList rn = xmldoc.SelectNodes("EquScheme/Remark");

        //            if (rn != null && rn.Count > 0)
        //            {
        //                foreach (XmlNode xn in rn)
        //                {
        //                    xmlrn.InnerXml += xn.InnerXml;
        //                }

        //            }
        //            XmlNodeList reln = xmldoc.SelectNodes("EquScheme/RelationConfig");

        //            if (reln != null && reln.Count > 0)
        //            {
        //                foreach (XmlNode xn in reln)
        //                {
        //                    xmlreln.InnerXml += xn.InnerXml;
        //                }

        //            }

        //        }

        //        if (!xmlbn.OuterXml.Equals("<BaseItem Title=\"\" />"))
        //        {
        //            xmlRoot.AppendChild(xmlbn);
        //        }

        //        if (!xmlrn.OuterXml.Equals("<Remark Title=\"\" />"))
        //        {
        //            xmlRoot.AppendChild(xmlrn);
        //        }

        //        if (!xmlreln.OuterXml.Equals("<RelationConfig Title=\"\" />"))
        //        {
        //            xmlRoot.AppendChild(xmlreln);
        //        }


        //        xmlDocNew.AppendChild(xmlRoot);

        //        #endregion

        //        strXml = xmlDocNew.InnerXml;


        //        if (strXml != "")
        //        {
        //            //由于做了清除需要重新注册脚本


        //            RegisterClientScrip();

        //            XmlDocument xmldoc = new XmlDocument();
        //            xmldoc.LoadXml(strXml);
        //            baseNode = xmldoc.DocumentElement;
        //            bnodes = xmldoc.SelectNodes("EquScheme/BaseItem");

        //            AddBaseSchemeControls();

        //            remarkNodes = xmldoc.SelectNodes("EquScheme/Remark");
        //            AddRemarkSchemeControls();

        //            relnodes = xmldoc.SelectNodes("EquScheme/RelationConfig");

        //            foreach (XmlNode relnode in relnodes)
        //            {
        //                iRel++;
        //                AddRelationSchemeControls(relnode, iRel);
        //            }
        //        }
        //    }

        //}

        private void LoadHtmlControls(long lngID)
        {
            string strXml = "";

            int iRel = 0;
            BR_ExtendsFieldsDP br_extendsFields = new BR_ExtendsFieldsDP();
            DataTable dt = br_extendsFields.GetDataTable(decimal.Parse(lngID.ToString()));
            if (dt.Rows.Count > 0)
            {
                ExtensionDisplayWayBS extensionDisplayWayBS = new ExtensionDisplayWayBS();
                DataTable dtExDisplayWay = extensionDisplayWayBS.GetExDisplayWayList(EquCategoryID/* => FlowModelID */, NodeModelID);    // 取扩展项显示方式列表


                Br_ExtensionsItemsDP br_items = new Br_ExtensionsItemsDP();

                if (Version != -1)
                {
                    //取历史值的情况
                    _fields = br_items.GetAllFieldsHistory();
                }
                else
                {
                    _fields = br_items.GetAllFields();   //获取最新的配置项情况

                }
                //清除现有所有控件

                this.Controls.Clear();

                SortedDictionary<String, List<String>> dictBaseItem = new SortedDictionary<string, List<String>>();
                Regex _r = new Regex("title=\"(.*?)\"", RegexOptions.IgnoreCase | RegexOptions.Multiline);



                foreach (DataRow dr in dt.Rows)
                {
                    if (Version != -1)
                    {
                        //取历史版本的strXml
                        strXml = Equ_DeskDP.isversionRtnXml(Version, long.Parse(ViewState["EquID"].ToString()));
                        if (strXml == "")
                        {
                            strXml = dr["KeyValue"].ToString();
                        }
                    }
                    else
                    {
                        strXml = dr["KeyValue"].ToString();
                    }

                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(strXml);


                    #region 验证是否可见? - 2013-12-04 @孙绍棕

                    /*
                      若设置为不可见，则返回                     
                     */

                    bool isVisible = IsExItemVisible(doc, dtExDisplayWay);

                    if (!isVisible)
                    {
                        continue;
                    }

                    #endregion

                    Match mTitle = _r.Match(doc.DocumentElement.InnerXml);
                    String strTitle = mTitle.Groups[1].Value.Trim();

                    if (!dictBaseItem.ContainsKey(strTitle))
                    {
                        dictBaseItem.Add(strTitle, new List<String>());
                    }

                    dictBaseItem[strTitle].Add(doc.DocumentElement.InnerXml);

                }

                StringBuilder sbXMLBaseItem = new StringBuilder();
                sbXMLBaseItem.Append("<EquScheme Title=\"用户自定义扩展项\">");

                foreach (KeyValuePair<String, List<String>> item in dictBaseItem)
                {
                    item.Value.Sort(new ExprPropertyComparser("orderby"));

                    StringBuilder sbBaseItemXML = new StringBuilder();
                    String strNodeName = String.Empty;

                    foreach (String baseItem in item.Value)
                    {
                        XmlDocument docBaseItem = new XmlDocument();
                        docBaseItem.LoadXml(baseItem);
                        //XmlNode xmlBaseItem = docBaseItem.SelectSingleNode("/EquScheme/BaseItem");
                        XmlNode xmlBaseItem = docBaseItem.DocumentElement;

                        if (sbBaseItemXML.Length <= 0)
                        {
                            sbBaseItemXML.AppendFormat("<{1} Title=\"{0}\">", xmlBaseItem.Attributes["Title"].Value, xmlBaseItem.Name);
                            strNodeName = xmlBaseItem.Name;
                        }


                        String strAttrXML = xmlBaseItem.InnerXml;
                        sbBaseItemXML.Append(strAttrXML);
                    }

                    sbBaseItemXML.AppendFormat("</{0}>", strNodeName);

                    sbXMLBaseItem.Append(sbBaseItemXML.ToString());
                }

                sbXMLBaseItem.Append("</EquScheme>");

                strXml = sbXMLBaseItem.ToString();

                if (strXml != "")
                {
                    //由于做了清除需要重新注册脚本

                    RegisterClientScrip();
                    XmlDocument xmldoc = new XmlDocument();
                    xmldoc.LoadXml(strXml);
                    baseNode = xmldoc.DocumentElement;
                    bnodes = xmldoc.DocumentElement.ChildNodes;

                    AddBaseSchemeControls(dtExDisplayWay);

                    //remarkNodes = xmldoc.SelectNodes("EquScheme/Remark");
                    //AddRemarkSchemeControls();

                    //relnodes = xmldoc.SelectNodes("EquScheme/RelationConfig");

                    //foreach (XmlNode relnode in relnodes)
                    //{
                    //    iRel++;
                    //    AddRelationSchemeControls(relnode, iRel);
                    //}
                }

            }

        }



        /// <summary>
        /// 取扩展项是否可见?
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="dtExDisplayWay"></param>
        /// <returns></returns>
        private bool IsExItemVisible(XmlDocument doc, DataTable dtExDisplayWay)
        {
            XmlNode xmlNode = doc.DocumentElement.SelectSingleNode("//AttributeItem");
            XmlAttribute attrID = xmlNode.Attributes["ID"];

            if (attrID == null) return false;    // ID属性不存在, 则认为不可见.

            long lngFieldID = long.Parse(attrID.Value);

            foreach (DataRow dr in dtExDisplayWay.Rows)
            {
                if (lngFieldID == long.Parse(dr["fieldid"].ToString()))
                {
                    int displayStatus;
                    int.TryParse(dr["displaystatus"].ToString(), out displayStatus);

                    /*
                     0: 可见，可编辑
                     * 1: 可见，不可编辑
                     * 2：不可见，不可编辑
                     */
                    if (displayStatus >= 2)
                        return false;
                }
            }

            return true;
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
                                                                var tbl = $(imgCtrl).parent().parent().parent().parent().next('table');
                                                                var status = tbl.css('display');
                                                                if (status == 'table') {
                                                                    tbl.hide();imgCtrl.src = ImgPlusScr ;
                                                                    
                                                                }else { tbl.show();	imgCtrl.src = ImgMinusScr ;	 }

                                                              return;
                                                              var TableID = imgTableID.replace(tabName,tabName +""Content"");
                                                              var className;
                                                              var objectFullName;
                                                              var tableCtrl;
                                                              tableCtrl = document.getElementById(TableID);
                                                              if(imgCtrl.src.indexOf(""icon_expandall"") != -1)
                                                              {
                                                                tableCtrl.style.display ="""";
                                                                
                                                              }
                                                              else
                                                              {
                                                                tableCtrl.style.display =""none"";
                                                                
                                                              }
                                                        }", true);

        }

        #endregion

        #region xml与hashtable之间转换函数  --yanghw 2011-08-10


        //把Hashtable转化成 List<EQU_deploy> 对象，以便于使用
        private List<EQU_deploy> GetHashTableRtnValue(Hashtable ht)
        {
            List<EQU_deploy> list = new List<EQU_deploy>();  //实例对象集合       
            IDictionaryEnumerator myEnumerator = ht.GetEnumerator();
            while (myEnumerator.MoveNext())
            {
                EQU_deploy Entity = (EQU_deploy)(myEnumerator.Value);//给对象赋值

                list.Add(Entity);
            }
            return list;         //返回list 数据集合
        }

        /// <summary>
        /// 获得资产的 值，并把 资产属性的集合 转化成 Hashtable 对象，便于xml操作
        /// </summary>
        /// <param name="EquID"></param>
        /// <returns></returns>
        private Hashtable SetHashTableXmlValue(long EquID)
        {
            Hashtable ht = new Hashtable();
            string strTmp = "";
            try
            {
                //取对象的值 把对象值加载到 Hashtable 中

                List<EQU_deploy> deployList = EQU_deploy.getEQU_deployList(EquID);
                if (deployList.Count > 0)
                {
                    foreach (EQU_deploy deploy in deployList)
                    {
                        ht.Add(deploy.FieldID.ToString(), deploy);
                    }
                }
            }
            catch
            {
            }
            return ht;
        }
        /// <summary>
        /// 获得历史变更资产的值，并把 资产属性的集合 转化成历史资产的Hashtable 对象，便于xml操作(用于变更单)
        /// </summary>
        /// <param name="EquID"></param>
        /// <returns></returns>
        private Hashtable SetHashTablehostoryXmlValue(long EquID, long FlowId)
        {
            Hashtable ht = new Hashtable();
            string strTmp = "";
            try
            {
                //取对象的值 把对象值加载到 Hashtable 中

                List<EQU_deploy> deployList = EQU_deployHistory.getEQU_deployHostoryList(EquID, FlowId);
                if (deployList.Count > 0)
                {
                    foreach (EQU_deploy deploy in deployList)
                    {
                        ht.Add(deploy.FieldID.ToString(), deploy);
                    }
                }
            }
            catch
            {
            }
            return ht;
        }

        /// <summary>
        /// 在没有变更情况下，历史版本的比较（用于关联图）

        /// </summary>
        /// <param name="EquID">资产id</param>
        /// <param name="lngVersion">版本号</param>
        /// <returns></returns>
        private Hashtable SetHashTableVersionXmlValue(long EquID, long lngVersion)
        {
            Hashtable ht = new Hashtable();
            string strTmp = "";
            try
            {
                //取对象的值 把对象值加载到 Hashtable 中

                List<EQU_deploy> deployList = null;
                if (Equ_DeskDP.isversion(lngVersion, EquID))
                {
                    //当前版本为最新版本时的 对象处理
                    deployList = EQU_deploy.getEQU_deployList(EquID);
                }
                else
                {
                    //当前版本 不是最新版本时的代码处理

                    deployList = EQU_deployHistory.getEQU_deployVersionList(EquID, lngVersion);
                }


                if (deployList.Count > 0)
                {
                    foreach (EQU_deploy deploy in deployList)
                    {
                        ht.Add(deploy.FieldID.ToString(), deploy);
                    }
                }
            }
            catch
            {
            }
            return ht;
        }
        #endregion

        #region SetChangeHashTableXmlValue  取变更临时表信息

        /// <summary>
        /// 获得资产的 值，并把 资产属性的集合 转化成 Hashtable 对象，便于xml操作
        /// </summary>
        /// <param name="EquID"></param>
        /// <returns></returns>
        private Hashtable SetChangeHashTableXmlValue(long FlowId, long EquID)
        {
            Hashtable ht = new Hashtable();
            string strTmp = "";
            try
            {
                //取对象的值 把对象值加载到 Hashtable 中


                List<EQU_deploy> deployList = Equ_DeskChangeDeploy.getEQU_ChangeDeployList(FlowId, EquID);
                if (deployList.Count > 0)
                {
                    foreach (EQU_deploy deploy in deployList)
                    {
                        ht.Add(deploy.FieldID.ToString(), deploy);
                    }
                }
                else
                {
                    //处在变更时，且未变更的时候，取原资产配置项的值

                    deployList = EQU_deploy.getEQU_deployList(EquID);
                    if (deployList.Count > 0)
                    {
                        foreach (EQU_deploy deploy in deployList)
                        {
                            ht.Add(deploy.FieldID.ToString(), deploy);
                        }
                    }
                }
            }
            catch
            {
            }
            return ht;
        }
        #endregion

        #region
        /// <summary>
        /// 改变部门，改变用户

        /// </summary>
        /// <param name="detp"></param>
        private void setUserConter(object sender, EventArgs e)
        {

            setUserDeptid(this, ((DeptPicker)sender));

        }
        /// <summary>
        /// 当部门值发生更改时 控件值重新绑定

        /// </summary>
        /// <param name="ctrRoot"></param>
        /// <param name="vardept"></param>
        private void setUserDeptid(Control ctrRoot, DeptPicker vardept)
        {
            foreach (Control pControl in ctrRoot.Controls)  /*遍历所有子节点*/
            {
                string strType = "";
                strType = pControl.GetType().Name;
                switch (strType)
                {

                    case "HtmlTable":
                    case "HtmlTableRow":
                    case "HtmlTableCell":
                        setUserDeptid(pControl, vardept);
                        break;
                    case "controls_userpicker_ascx"://用户信息控件
                        if (pControl.ID.StartsWith("tDynamic"))
                        {
                            //((UserPicker)pControl).DeptID = vardept.DeptID;
                            //((UserPicker)pControl).UserID = 0;
                            //((UserPicker)pControl).UserName = "";

                        }
                        break;
                    default:
                        break;
                }
            }
        }




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
                                //把控件中的值赋给 对象EQU_deploy
                                EQU_deploy deploy = new EQU_deploy();
                                deploy.ID = long.Parse(((CtrTextDropList)pControl).Attributes["deployID"]);
                                if (((CtrTextDropList)pControl).Attributes["EquID"] != null)
                                {
                                    deploy.EquID = long.Parse(((CtrTextDropList)pControl).Attributes["EquID"]);
                                }
                                else
                                {
                                    deploy.EquID = 0;
                                }

                                deploy.FieldID = long.Parse(((CtrTextDropList)pControl).Attributes["Tag"]);
                                deploy.CHName = ((CtrTextDropList)pControl).Attributes["sFieldName"];
                                deploy.Value = ((CtrTextDropList)pControl).Value;
                                list.Add(((CtrTextDropList)pControl).Attributes["Tag"], deploy);
                            }
                            break;

                        case "CheckBox":
                            if (pControl.ID.StartsWith("tDynamic"))
                            {

                                EQU_deploy deploy = new EQU_deploy();
                                deploy.ID = long.Parse(((CheckBox)pControl).Attributes["deployID"]);
                                if (((CheckBox)pControl).Attributes["EquID"] != null)
                                {
                                    deploy.EquID = long.Parse(((CheckBox)pControl).Attributes["EquID"]);
                                }
                                else
                                {
                                    deploy.EquID = 0;
                                }

                                deploy.FieldID = long.Parse(((CheckBox)pControl).Attributes["Tag"]);
                                deploy.CHName = ((CheckBox)pControl).Attributes["sFieldName"];
                                deploy.Value = ((CheckBox)pControl).Checked == true ? "1" : "0";
                                list.Add(((CheckBox)pControl).Attributes["Tag"], deploy);
                            }
                            break;
                        case "controls_ctrflowcatadroplist_ascx"://下拉控件
                            if (pControl.ID.StartsWith("tDynamic"))
                            {
                                EQU_deploy deploy = new EQU_deploy();
                                deploy.ID = long.Parse(((ctrFlowCataDropList)pControl).Attributes["deployID"]);
                                if (((ctrFlowCataDropList)pControl).Attributes["EquID"] != null)
                                {
                                    deploy.EquID = long.Parse(((ctrFlowCataDropList)pControl).Attributes["EquID"]);
                                }
                                else
                                {
                                    deploy.EquID = 0;
                                }

                                deploy.FieldID = long.Parse(((ctrFlowCataDropList)pControl).Attributes["Tag"]);
                                deploy.CHName = ((ctrFlowCataDropList)pControl).Attributes["sFieldName"];
                                deploy.Value = ((ctrFlowCataDropList)pControl).CatelogID.ToString();
                                list.Add(((ctrFlowCataDropList)pControl).Attributes["Tag"], deploy);
                            }
                            break;
                        case "controls_deptpicker_ascx"://部门控件
                            if (pControl.ID.StartsWith("tDynamic"))
                            {
                                EQU_deploy deploy = new EQU_deploy();
                                deploy.ID = long.Parse(((DeptPicker)pControl).Attributes["deployID"]);
                                if (((DeptPicker)pControl).Attributes["EquID"] != null)
                                {
                                    deploy.EquID = long.Parse(((DeptPicker)pControl).Attributes["EquID"]);
                                }
                                else
                                {
                                    deploy.EquID = 0;
                                }

                                deploy.FieldID = long.Parse(((DeptPicker)pControl).Attributes["Tag"]);
                                deploy.CHName = ((DeptPicker)pControl).Attributes["sFieldName"];
                                deploy.Value = ((DeptPicker)pControl).DeptID.ToString();
                                list.Add(((DeptPicker)pControl).Attributes["Tag"], deploy);
                            }
                            break;
                        case "controls_userpicker_ascx"://用户信息控件
                            if (pControl.ID.StartsWith("tDynamic"))
                            {
                                EQU_deploy deploy = new EQU_deploy();
                                deploy.ID = long.Parse(((UserPicker)pControl).Attributes["deployID"]);
                                if (((UserPicker)pControl).Attributes["EquID"] != null)
                                {
                                    deploy.EquID = long.Parse(((UserPicker)pControl).Attributes["EquID"]);
                                }
                                else
                                {
                                    deploy.EquID = 0;
                                }

                                deploy.FieldID = long.Parse(((UserPicker)pControl).Attributes["Tag"]);
                                deploy.CHName = ((UserPicker)pControl).Attributes["sFieldName"];
                                deploy.Value = ((UserPicker)pControl).UserID.ToString();
                                list.Add(((UserPicker)pControl).Attributes["Tag"], deploy);
                            }
                            break;
                        case "controls_ctrdateandtimev2_ascx":    // 时间v2控件
                            if (pControl.ID.StartsWith("tDynamic"))
                            {
                                EQU_deploy deploy = new EQU_deploy();
                                deploy.ID = long.Parse(((CtrDateAndTimeV2)pControl).Attributes["deployID"]);
                                if (((CtrDateAndTimeV2)pControl).Attributes["EquID"] != null)
                                {
                                    deploy.EquID = long.Parse(((CtrDateAndTimeV2)pControl).Attributes["EquID"]);
                                }
                                else
                                {
                                    deploy.EquID = 0;
                                }

                                deploy.FieldID = long.Parse(((CtrDateAndTimeV2)pControl).Attributes["Tag"]);
                                deploy.CHName = ((CtrDateAndTimeV2)pControl).Attributes["sFieldName"];
                                deploy.Value = ((CtrDateAndTimeV2)pControl).dateTimeString.ToString();
                                list.Add(((CtrDateAndTimeV2)pControl).Attributes["Tag"], deploy);
                            }

                            break;
                        case "controls_ctrdateandtime_ascx"://时间控件
                            if (pControl.ID.StartsWith("tDynamic"))
                            {
                                EQU_deploy deploy = new EQU_deploy();
                                deploy.ID = long.Parse(((CtrDateAndTime)pControl).Attributes["deployID"]);
                                if (((CtrDateAndTime)pControl).Attributes["EquID"] != null)
                                {
                                    deploy.EquID = long.Parse(((CtrDateAndTime)pControl).Attributes["EquID"]);
                                }
                                else
                                {
                                    deploy.EquID = 0;
                                }

                                deploy.FieldID = long.Parse(((CtrDateAndTime)pControl).Attributes["Tag"]);
                                deploy.CHName = ((CtrDateAndTime)pControl).Attributes["sFieldName"];
                                deploy.Value = ((CtrDateAndTime)pControl).dateTimeString.ToString();
                                list.Add(((CtrDateAndTime)pControl).Attributes["Tag"], deploy);
                            }
                            break;
                        case "controls_ctrflownumeric_ascx"://用户信息控件
                            if (pControl.ID.StartsWith("tDynamic"))
                            {
                                EQU_deploy deploy = new EQU_deploy();
                                deploy.ID = long.Parse(((CtrFlowNumeric)pControl).Attributes["deployID"]);
                                if (((CtrFlowNumeric)pControl).Attributes["EquID"] != null)
                                {
                                    deploy.EquID = long.Parse(((CtrFlowNumeric)pControl).Attributes["EquID"]);
                                }
                                else
                                {
                                    deploy.EquID = 0;
                                }

                                deploy.FieldID = long.Parse(((CtrFlowNumeric)pControl).Attributes["Tag"]);
                                deploy.CHName = ((CtrFlowNumeric)pControl).Attributes["sFieldName"];
                                deploy.Value = ((CtrFlowNumeric)pControl).Value.ToString();
                                list.Add(((CtrFlowNumeric)pControl).Attributes["Tag"], deploy);
                            }
                            break;
                        case "controls_extension_dayschemectrlist_ascx":
                            if (pControl.ID.StartsWith(""))
                            {

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

            string prefix = "RelTab" + System.Guid.NewGuid().ToString();
            tabMain.ID = prefix + iRel.ToString();


            tabMain.Attributes.Add("width", "100%");
            tabMain.Attributes.Add("align", "center");
            tabMain.Attributes.Add("class", "listNewContent");



            //添加标题

            if (relnode.Attributes["Title"] != null)
            {
                tr.ID = tabMain.ID + "RelTR";
                tr.Attributes.Add("class", "listTitleNew");
                tc.ID = tabMain.ID + "RelTD";
                tc.Attributes.Add("align", "left");
                tc.Attributes.Add("vAlign", "top");
                tc.Attributes.Add("class", "bt_di");

                im.ID = tabMain.ID + "imgBase";
                im.Attributes.Add("class", "icon");
                im.Attributes.Add("onclick", "ShowTableDym(this,'" + this.ClientID + "_" + tabMain.ClientID + "','RelTab');");
                im.Attributes.Add("height", "16");
                im.Attributes.Add("width", "16");
                im.Attributes.Add("align", "absbottom");
                im.Attributes.Add("src", "../Images/icon_collapseall.gif");
                AddControl(tc, im);

                lab1.ID = tabMain.ID + "Title";
                if (relnode.Attributes["Title"].Value.Trim() != "")
                {
                    lab1.Text = relnode.Attributes["Title"].Value;
                }
                else
                {
                    lab1.Text = "关联信息";
                }

                //AddControl(tc, lab1);
                //AddControl(tr, tc);
                //AddControl(tabMain, tr);
            }


            AddControl(this, tabMain);
            //内容
            tabMain = new HtmlTable();
            tabMain.ID = prefix + "RelTabContent" + iRel.ToString();
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

                string sFieldName = "";
                sFieldName = n.Attributes["CHName"].Value;

                if (_fields[sID] != null)
                {
                    sCHName = _fields[sID].ToString();
                }
                if (sCHName != "")
                {
                    EQU_deploy deploy = null;
                    if (_xmlHt[sID] != null)
                    {
                        //获得控件的值

                        deploy = (EQU_deploy)(_xmlHt[sID]);
                        sSetValue = deploy.Value;
                    }
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


                    chkTemp.Attributes.Add("sFieldName", sFieldName);//存名字

                    //chkTemp.Attributes.Add("EquID", ViewState["EquID"].ToString());//存资产id
                    if (deploy != null)
                    {
                        chkTemp.Attributes.Add("deployID", deploy.ID.ToString());//对象ID编号值

                    }
                    else
                    {
                        chkTemp.Attributes.Add("deployID", "0");//对象ID编号值

                    }

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
                        if (sSetValue == "1")
                        {
                            lnkTemp.ToolTip = "查看配置［" + lnkTemp.Text + "］的所有资产";
                        }
                        else
                        {
                            lnkTemp.ToolTip = "查看没有配置［" + lnkTemp.Text + "］的所有资产";
                        }

                        lnkTemp.Attributes.Add("style", "CURSOR: hand");
                        lnkTemp.Attributes.Add("onclick", "javascript:window.open('../EquipmentManager/frmEqu_SameSchemaItem.aspx?ItemFieldID=" + sID.Trim() + "&ItemFieldValue='+encodeURIComponent('" + sSetValue.Trim() + "'),'newwindow','scrollbars=yes,resizable=yes,top=0,left=0,width=800px,height=600');");
                        iLab++;
                        lnkTemp.ID = tr.ID + "lnk" + iLab.ToString();

                        //找不到同时也需要比较的情况[新加的配置等情况]
                        if (hasCompare == true)
                        {
                            if (_xmlHtComp[sID] != null)
                            {
                                //存在历史比较值的情况下，如果不同，则红色显示
                                EQU_deploy deployList = (EQU_deploy)(_xmlHtComp[sID]);
                                //存在历史比较值的情况下，如果不同，则红色显示
                                sSetValueComp = deployList.Value;

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

                    string sFieldName = "";
                    sFieldName = n.Attributes["CHName"].Value;

                    if (_fields[sID] != null)
                    {
                        sCHName = _fields[sID].ToString();
                    }

                    if (sCHName != "")
                    {

                        EQU_deploy deploy = null;
                        if (_xmlHt[sID] != null)
                        {
                            //获得控件的值

                            deploy = (EQU_deploy)(_xmlHt[sID]);
                            sSetValue = deploy.Value;
                        }

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

                            lnkTemp.Attributes.Add("style", "CURSOR: hand");
                            //lnkTemp.Attributes.Add("onclick", "javascript:window.open('../EquipmentManager/frmEqu_SameSchemaItem.aspx?ItemFieldID=" + sID.Trim() + "&ItemFieldValue='+encodeURIComponent('" + sSetValue.Trim() + "'),'newwindow','scrollbars=yes,resizable=yes,top=0,left=0,width=800px,height=600');");
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

                        ctdl.Attributes.Add("sFieldName", sFieldName);//存名字

                        //ctdl.Attributes.Add("EquID", ViewState["EquID"].ToString());//存资产id
                        if (deploy != null)
                        {
                            ctdl.Attributes.Add("deployID", deploy.ID.ToString());//对象ID编号值

                        }
                        else
                        {
                            ctdl.Attributes.Add("deployID", "0");//对象ID编号值

                        }

                        ctdl.MaxLength = 500;
                        ctdl.Value = sSetValue;

                        iLab++;
                        labTemp = new Label();
                        labTemp.ID = tr.ID + "lab" + iLab.ToString();
                        labTemp.Text = sSetValue;


                        if (this.ReadOnly == true)
                        {
                            //ctdl.Visible = false;


                            //找不到同时也需要比较的情况[新加的配置等情况]
                            if (hasCompare == true)
                            {
                                if (_xmlHtComp[sID] != null)
                                {
                                    EQU_deploy deployList = (EQU_deploy)(_xmlHtComp[sID]);
                                    //存在历史比较值的情况下，如果不同，则红色显示
                                    sSetValueComp = deployList.Value;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dtExDisplayWay">扩展项显示方式列表</param>
        private void AddBaseSchemeControls(DataTable dtExDisplayWay)
        {
            if (bnodes.Count == 0)
            {
                return;
            }

            String strOldTitle = "-1";
            String strNewTitle = String.Empty;
            int count = 0;


            foreach (XmlNode node in bnodes)//分组
            {
                // 可见的处理位置.

                strNewTitle = node.Attributes["Title"].Value.Trim();
                if (strNewTitle.Equals(strOldTitle))
                {
                    AddBaseItem(node, count, true, dtExDisplayWay);
                }
                else
                {
                    AddBaseItem(node, count, false, dtExDisplayWay);
                }


                count++;

                strOldTitle = node.Attributes["Title"].Value.Trim();
            }
        }

        #endregion


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
            AddRemarkItem(relnode, iRel, false);
        }

        private void AddRemarkItem(XmlNode relnode, int iRel, bool isEqualGroup)
        {
            XmlNodeList ns = null;
            HtmlTable tabMain = new HtmlTable();
            HtmlTableRow tr = new HtmlTableRow();
            HtmlTableCell tc = new HtmlTableCell();
            HtmlImage im = new HtmlImage();
            Label lab1 = new Label();
            //循环递归
            string prefix = "RemarkTab" + System.Guid.NewGuid().ToString();
            tabMain.ID = prefix + iRel.ToString();

            tabMain.Attributes.Add("width", "100%");
            tabMain.Attributes.Add("align", "center");
            tabMain.Attributes.Add("class", "listNewContent");

            //添加标题

            if (relnode.Attributes["Title"] != null && isEqualGroup == false)
            {
                tr.ID = tabMain.ID + "RemarkTR";
                tr.Attributes.Add("class", "listTitleNew");
                tc.ID = tabMain.ID + "RemarkTD";
                tc.Attributes.Add("align", "left");
                tc.Attributes.Add("vAlign", "top");
                tc.Attributes.Add("class", "bt_di");

                im.ID = tabMain.ID + "imgRemark";
                im.Attributes.Add("class", "icon");
                im.Attributes.Add("onclick", "ShowTableDym(this,'" + this.ClientID + "_" + tabMain.ClientID + "','RemarkTab');");
                im.Attributes.Add("height", "16");
                im.Attributes.Add("width", "16");
                im.Attributes.Add("align", "absbottom");
                // im.Attributes.Add("src", "../Images/icon_collapseall.gif");
                im.Attributes.Add("src", "../Images/icon_collapseall.gif");

                AddControl(tc, im);

                lab1.ID = tabMain.ID + "Title";
                if (relnode.Attributes["Title"].Value.Trim() != "")
                {
                    lab1.Text = relnode.Attributes["Title"].Value;
                }
                else
                {
                    lab1.Text = "备注信息";
                }

                AddControl(tc, lab1);

                AddControl(tr, tc);
                AddControl(tabMain, tr);

            }

            AddControl(this, tabMain);
            //内容
            tabMain = new HtmlTable();
            tabMain.ID = prefix + "RemarkTabContent" + iRel.ToString();
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
                AddRemarkRow(ref tr, ref rowcount, n, ref iLab, 0);
                AddControl(tabMain, tr);
            }
            AddControl(this, tabMain);
        }

        #region 添加下拉控件值  --yanghw  2011-08-10
        //此控件修改

        private void AddDropDownListRow(ref HtmlTableRow tr, ref int rowcount, XmlNode n, ref int iLab, int width, int displayStatus)
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
            string sFieldName = n.Attributes["CHName"].Value;//显示列名称

            string IsMust = n.Attributes["IsMust"].Value;//是否必填
            //绑定值


            //根据配置项获取对应的下拉rootID
            Br_ExtensionsItemsDP extension = new Br_ExtensionsItemsDP();
            extension.GetReCorded(sID);
            //Equ_SchemaItemsDP ee = new Equ_SchemaItemsDP();
            //ee = ee.GetReCorded(sID);


            long RootID = long.Parse(n.Attributes["Default"].Value == "" ? "0" : n.Attributes["Default"].Value);
            //long RootID = long.Parse(ee.CatalogID.ToString() == "" ? "1" : ee.CatalogID.ToString());

            if (_fields[sID] != null)
            {
                sCHName = _fields[sID].ToString();
            }

            if (sCHName != "")
            {
                //资产属性字段值 类对象

                EQU_deploy deploy = null;
                if (_xmlHt[sID] != null)
                {
                    //获得控件的值

                    deploy = (EQU_deploy)(_xmlHt[sID]);
                    sSetValue = deploy.Value;
                }
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


                ctrFlowCataDropList ctdl = (Epower.ITSM.Web.Controls.ctrFlowCataDropList)LoadControl("~/controls/ctrFlowCataDropList.ascx");
                //ctdl.FieldsSourceType = 1;   //异步获取方式
                //ctdl.TextMode = TextBoxMode.MultiLine;
                //ctdl.FieldsSourceID = "SchemaItem_" + sID;
                ctdl.ID = "tDynamictxt" + sID;
                ctdl.Attributes.Add("Tag", sID);// 存id 的值

                ctdl.Attributes.Add("sFieldName", sFieldName);//存名字

                // ctdl.Attributes.Add("EquID", ViewState["EquID"].ToString());//存资产id
                if (deploy != null)
                {
                    ctdl.Attributes.Add("deployID", deploy.ID.ToString());//对象ID编号值

                }
                else
                {
                    ctdl.Attributes.Add("deployID", "0");//对象ID编号值

                }
                ctdl.RootID = RootID;
                ctdl.CatelogID = long.Parse(sSetValue == "" ? "0" : sSetValue);
                // ctdl.Width = Unit.Parse("70%");

                if (IsMust == "1")
                {
                    ctdl.MustInput = true;
                    ctdl.TextToolTip = sCHName;
                }

                labTemp = new Label();
                labTemp.ID = tr.ID + "lab" + iLab.ToString();
                labTemp.Text = sCHName;


                if (this.ReadOnly == true)
                {
                    ctdl.ContralState = eOA_FlowControlState.eReadOnly;

                }

                if (displayStatus == 1)    // 可见, 不可编辑
                {
                    ctdl.ContralState = eOA_FlowControlState.eReadOnly;
                    ctdl.MustInput = false;
                }

                //找不到同时也需要比较的情况[新加的配置等情况]
                if (hasCompare == true)
                {
                    if (_xmlHtComp[sID] != null)
                    {
                        //存在历史比较值的情况下，如果不同，则红色显示
                        //sSetValueComp = _xmlHtComp[sID].ToString();
                        EQU_deploy deployList = (EQU_deploy)(_xmlHtComp[sID]);
                        //存在历史比较值的情况下，如果不同，则红色显示
                        sSetValueComp = deployList.Value;

                        if (sSetValueComp != sSetValue)
                            labTemp.ForeColor = System.Drawing.Color.Red;
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
                    //  tc.InnerText = sSetValue;
                    //tc.Attributes.Add("title", "查看配置项［" + sSetValue.Trim() + "］相同的资产列表");



                    if (rowcount % 4 == 0)
                    {
                        tc.Attributes.Add("style", "word-break:break-all;");
                    }
                    else
                    {
                        if (width != 82)
                        {
                            tc.Attributes.Add("style", "CURSOR: hand;width:" + width.ToString() + "%");
                        }
                        else
                        {
                            tc.Attributes.Add("style", "word-break:break-all;");
                        }
                    }
                    //tc.Attributes.Add("onclick", "javascript:window.open('../EquipmentManager/frmEqu_SameSchemaItem.aspx?ItemFieldID=" + sID.Trim() + "&ItemFieldValue='+encodeURIComponent('" + sSetValue.Trim() + "'),'newwindow','scrollbars=yes,resizable=yes,top=0,left=0,width=800px,height=600');");
                }
                AddControl(tr, tc);
            }
        }
        #endregion

        #region 数值类型控件  --yanghw  2011-08-11

        //数值类型

        private void AddNumberRow(ref HtmlTableRow tr, ref int rowcount, XmlNode n, ref int iLab, int width, int displayStatus)
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
            string sFieldName = n.Attributes["CHName"].Value;//显示列名称

            string IsMust = n.Attributes["IsMust"].Value;//是否必填
            //绑定值

            long RootID = long.Parse(n.Attributes["Default"].Value == "" ? "0" : n.Attributes["Default"].Value);

            if (_fields[sID] != null)
            {
                sCHName = _fields[sID].ToString();
            }

            if (sCHName != "")
            {
                //资产属性字段值 类对象

                EQU_deploy deploy = null;
                if (_xmlHt[sID] != null)
                {
                    //获得控件的值

                    deploy = (EQU_deploy)(_xmlHt[sID]);
                    sSetValue = deploy.Value;
                }
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


                CtrFlowNumeric ctdl = (Epower.ITSM.Web.Controls.CtrFlowNumeric)LoadControl("~/controls/CtrFlowNumeric.ascx");
                //ctdl.FieldsSourceType = 1;   //异步获取方式
                //ctdl.TextMode = TextBoxMode.MultiLine;
                //ctdl.FieldsSourceID = "SchemaItem_" + sID;
                ctdl.ID = "tDynamictxt" + sID;
                ctdl.Attributes.Add("Tag", sID);// 存id 的值

                ctdl.Attributes.Add("sFieldName", sFieldName);//存名字

                // ctdl.Attributes.Add("EquID", ViewState["EquID"].ToString());//存资产id
                if (deploy != null)
                {
                    ctdl.Attributes.Add("deployID", deploy.ID.ToString());//对象ID编号值

                }
                else
                {
                    ctdl.Attributes.Add("deployID", "0");//对象ID编号值

                }

                //   ctdl.CatelogID = long.Parse(sSetValue == "" ? "0" : sSetValue);


                //给 deptId赋值

                ctdl.Value = sSetValue;


                if (IsMust == "1")
                {
                    ctdl.MustInput = true;
                    ctdl.TextToolTip = sCHName;
                }

                labTemp = new Label();
                labTemp.ID = tr.ID + "lab" + iLab.ToString();
                labTemp.Text = sCHName;


                if (this.ReadOnly == true)
                {
                    ctdl.ContralState = eOA_FlowControlState.eReadOnly;
                }

                if (displayStatus == 1)    // 可见, 不可编辑
                {
                    ctdl.ContralState = eOA_FlowControlState.eReadOnly;
                    ctdl.MustInput = false;
                }

                //找不到同时也需要比较的情况[新加的配置等情况]
                if (hasCompare == true)
                {
                    if (_xmlHtComp[sID] != null)
                    {
                        //存在历史比较值的情况下，如果不同，则红色显示
                        EQU_deploy deployList = (EQU_deploy)(_xmlHtComp[sID]);
                        //存在历史比较值的情况下，如果不同，则红色显示
                        sSetValueComp = deployList.Value;

                        if (sSetValueComp != sSetValue)
                            labTemp.ForeColor = System.Drawing.Color.Red;
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
                    // tc.InnerText = sSetValue;
                    //tc.Attributes.Add("title", "查看配置项［" + sSetValue.Trim() + "］相同的资产列表");





                    if (rowcount % 4 == 0)
                    {
                        tc.Attributes.Add("style", "word-break:break-all;");
                    }
                    else
                    {
                        if (width != 82)
                        {
                            tc.Attributes.Add("style", "CURSOR: hand;width:" + width.ToString() + "%");
                        }
                        else
                        {
                            tc.Attributes.Add("style", "word-break:break-all;");
                        }
                    }
                    //tc.Attributes.Add("onclick", "javascript:window.open('../EquipmentManager/frmEqu_SameSchemaItem.aspx?ItemFieldID=" + sID.Trim() + "&ItemFieldValue='+encodeURIComponent('" + sSetValue.Trim() + "'),'newwindow','scrollbars=yes,resizable=yes,top=0,left=0,width=800px,height=600');");
                }
                AddControl(tr, tc);
            }
        }
        #endregion

        #region 时间类型控件  --yanghw  2011-08-11
        //时间类型
        private void AddTimeRow(ref HtmlTableRow tr, ref int rowcount, XmlNode n, ref int iLab, int width, int displayStatus)
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
            string sFieldName = n.Attributes["CHName"].Value;//显示列名称

            string isChack = n.Attributes["isChack"].Value;//显示列名称

            string IsMust = n.Attributes["IsMust"].Value;//是否必填
            //绑定值

            long RootID = long.Parse(n.Attributes["Default"].Value == "" ? "0" : n.Attributes["Default"].Value);

            if (_fields[sID] != null)
            {
                sCHName = _fields[sID].ToString();
            }

            if (sCHName != "")
            {
                //资产属性字段值 类对象

                EQU_deploy deploy = null;
                if (_xmlHt[sID] != null)
                {
                    //获得控件的值

                    deploy = (EQU_deploy)(_xmlHt[sID]);
                    sSetValue = deploy.Value;
                }
                if (sSetValue == "")
                {
                    if (_blnAddEqu == true || _blnChangeCatalog == true)
                    {
                        //新建 或 编辑状态下 更改过 分类
                        if (n.Attributes["Default"].Value == "1")
                        {
                            sSetValue = System.DateTime.Now.ToShortDateString();
                        }
                    }
                }

                iLab++;
                Label labTemp;


                CtrDateAndTimeV2 ctdl = (Epower.ITSM.Web.Controls.CtrDateAndTimeV2)LoadControl("~/Controls/CtrDateAndTimeV2.ascx");
                //ctdl.FieldsSourceType = 1;   //异步获取方式
                //ctdl.TextMode = TextBoxMode.MultiLine;
                //ctdl.FieldsSourceID = "SchemaItem_" + sID;
                ctdl.ID = "tDynamictxt" + sID;
                ctdl.Attributes.Add("Tag", sID);// 存id 的值

                ctdl.Attributes.Add("sFieldName", sFieldName);//存名字

                //ctdl.Attributes.Add("EquID", ViewState["EquID"].ToString());//存资产id
                if (deploy != null)
                {
                    ctdl.Attributes.Add("deployID", deploy.ID.ToString());//对象ID编号值

                }
                else
                {
                    ctdl.Attributes.Add("deployID", "0");//对象ID编号值

                }

                //   ctdl.CatelogID = long.Parse(sSetValue == "" ? "0" : sSetValue);


                //给 deptId赋值

                if (isChack != "1")
                {
                    ctdl.ShowTime = false;
                    ctdl.ShowMinute = false;
                }

                if (IsMust == "1")
                {
                    ctdl.MustInput = true;
                    ctdl.TextToolTip = sCHName;
                }

                ctdl.dateTimeString = sSetValue;

                labTemp = new Label();
                labTemp.ID = tr.ID + "lab" + iLab.ToString();
                labTemp.Text = sCHName;


                if (this.ReadOnly == true)
                {
                    ctdl.ContralState = eOA_FlowControlState.eReadOnly;
                }


                if (displayStatus == 1)    // 可见, 不可编辑
                {
                    ctdl.ContralState = eOA_FlowControlState.eReadOnly;
                    ctdl.MustInput = false;
                }

                //找不到同时也需要比较的情况[新加的配置等情况]
                if (hasCompare == true)
                {
                    if (_xmlHtComp[sID] != null)
                    {
                        //存在历史比较值的情况下，如果不同，则红色显示
                        EQU_deploy deployList = (EQU_deploy)(_xmlHtComp[sID]);
                        //存在历史比较值的情况下，如果不同，则红色显示
                        sSetValueComp = deployList.Value;

                        if (sSetValueComp != sSetValue)
                            labTemp.ForeColor = System.Drawing.Color.Red;
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
                    // tc.InnerText = sSetValue;
                    //tc.Attributes.Add("title", "查看配置项［" + sSetValue.Trim() + "］相同的资产列表");


                    if (rowcount % 4 == 0)
                    {
                        tc.Attributes.Add("style", "word-break:break-all;");
                    }
                    else
                    {
                        if (width != 82)
                        {
                            tc.Attributes.Add("style", "CURSOR: hand;width:" + width.ToString() + "%");
                        }
                        else
                        {
                            tc.Attributes.Add("style", "word-break:break-all;");
                        }
                    }
                    //tc.Attributes.Add("onclick", "javascript:window.open('../EquipmentManager/frmEqu_SameSchemaItem.aspx?ItemFieldID=" + sID.Trim() + "&ItemFieldValue='+encodeURIComponent('" + sSetValue.Trim() + "'),'newwindow','scrollbars=yes,resizable=yes,top=0,left=0,width=800px,height=600');");
                }
                AddControl(tr, tc);
            }
        }
        #endregion


        #region 用户信息控件  --yanghw  2011-08-10
        //部门控件
        private void AddUserRow(ref HtmlTableRow tr, ref int rowcount, XmlNode n, ref int iLab, int width, int displayStatus)
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
            string sFieldName = n.Attributes["CHName"].Value;//显示列名称

            string IsMust = n.Attributes["IsMust"].Value;//是否必填
            //绑定值

            long RootID = long.Parse(n.Attributes["Default"].Value == "" ? "0" : n.Attributes["Default"].Value);

            if (_fields[sID] != null)
            {
                sCHName = _fields[sID].ToString();
            }

            if (sCHName != "")
            {
                //资产属性字段值 类对象

                EQU_deploy deploy = null;
                if (_xmlHt[sID] != null)
                {
                    //获得控件的值

                    deploy = (EQU_deploy)(_xmlHt[sID]);
                    sSetValue = deploy.Value;
                }
                if (sSetValue == "")
                {
                    if (_blnAddEqu == true || _blnChangeCatalog == true)
                    {
                        //新建 或 编辑状态下 更改过 分类
                        if (n.Attributes["Default"].Value == "1")
                        {
                            sSetValue = Session["UserID"].ToString();
                        }
                    }
                }

                iLab++;
                Label labTemp;


                UserPicker ctdl = (Epower.ITSM.Web.Controls.UserPicker)LoadControl("~/controls/UserPicker.ascx");
                //ctdl.FieldsSourceType = 1;   //异步获取方式
                //ctdl.TextMode = TextBoxMode.MultiLine;
                //ctdl.FieldsSourceID = "SchemaItem_" + sID;
                ctdl.ID = "tDynamictxt" + sID;
                ctdl.Attributes.Add("Tag", sID);// 存id 的值

                ctdl.Attributes.Add("sFieldName", sFieldName);//存名字

                // ctdl.Attributes.Add("EquID", ViewState["EquID"].ToString());//存资产id
                if (deploy != null)
                {
                    ctdl.Attributes.Add("deployID", deploy.ID.ToString());//对象ID编号值

                }
                else
                {
                    ctdl.Attributes.Add("deployID", "0");//对象ID编号值

                }

                //   ctdl.CatelogID = long.Parse(sSetValue == "" ? "0" : sSetValue);


                //给 deptId赋值

                ctdl.UserID = long.Parse(sSetValue == "" ? "0" : sSetValue);

                if (IsMust == "1")
                {
                    ctdl.MustInput = true;
                    ctdl.TextToolTip = sCHName;
                }

                labTemp = new Label();
                labTemp.ID = tr.ID + "lab" + iLab.ToString();
                labTemp.Text = sCHName;


                if (this.ReadOnly == true)
                {
                    ctdl.ContralState = eOA_FlowControlState.eReadOnly;
                }

                if (displayStatus == 1)    // 可见, 不可编辑
                {
                    ctdl.ContralState = eOA_FlowControlState.eReadOnly;
                    ctdl.MustInput = false;
                }

                //找不到同时也需要比较的情况[新加的配置等情况]
                if (hasCompare == true)
                {
                    if (_xmlHtComp[sID] != null)
                    {
                        //存在历史比较值的情况下，如果不同，则红色显示
                        EQU_deploy deployList = (EQU_deploy)(_xmlHtComp[sID]);
                        //存在历史比较值的情况下，如果不同，则红色显示
                        sSetValueComp = deployList.Value;

                        if (sSetValueComp != sSetValue)
                            labTemp.ForeColor = System.Drawing.Color.Red;
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
                    // tc.InnerText = sSetValue;
                    //tc.Attributes.Add("title", "查看配置项［" + sSetValue.Trim() + "］相同的资产列表");


                    if (rowcount % 4 == 0)
                    {
                        tc.Attributes.Add("style", "word-break:break-all;");
                    }
                    else
                    {
                        if (width != 82)
                        {
                            tc.Attributes.Add("style", "CURSOR: hand;width:" + width.ToString() + "%");
                        }
                        else
                        {
                            tc.Attributes.Add("style", "word-break:break-all;");
                        }
                    }
                    //tc.Attributes.Add("onclick", "javascript:window.open('../EquipmentManager/frmEqu_SameSchemaItem.aspx?ItemFieldID=" + sID.Trim() + "&ItemFieldValue='+encodeURIComponent('" + sSetValue.Trim() + "'),'newwindow','scrollbars=yes,resizable=yes,top=0,left=0,width=800px,height=600');");
                }
                AddControl(tr, tc);
            }
        }
        #endregion

        #region 部门控件  --yanghw  2011-08-10
        //部门控件
        private void AddDetpRow(ref HtmlTableRow tr, ref int rowcount, XmlNode n, ref int iLab, int width, int displayStatus)
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
            string sFieldName = n.Attributes["CHName"].Value;//显示列名称

            string IsMust = n.Attributes["IsMust"].Value;//是否必填
            //绑定值

            long RootID = long.Parse(n.Attributes["Default"].Value == "" ? "0" : n.Attributes["Default"].Value);

            if (_fields[sID] != null)
            {
                sCHName = _fields[sID].ToString();
            }

            if (sCHName != "")
            {
                //资产属性字段值 类对象

                EQU_deploy deploy = null;
                if (_xmlHt[sID] != null)
                {
                    //获得控件的值

                    deploy = (EQU_deploy)(_xmlHt[sID]);
                    sSetValue = deploy.Value;
                }
                if (sSetValue == "")
                {
                    if (_blnAddEqu == true || _blnChangeCatalog == true)
                    {
                        //新建 或 编辑状态下 更改过 分类
                        if (n.Attributes["Default"].Value == "1")
                        {
                            sSetValue = Session["UserDeptID"].ToString();
                        }
                    }
                }

                iLab++;
                Label labTemp;


                DeptPicker ctdl = (Epower.ITSM.Web.Controls.DeptPicker)LoadControl("~/controls/DeptPicker.ascx");
                //ctdl.FieldsSourceType = 1;   //异步获取方式
                //ctdl.TextMode = TextBoxMode.MultiLine;
                //ctdl.FieldsSourceID = "SchemaItem_" + sID;
                ctdl.ID = "tDynamictxt" + sID;
                ctdl.Attributes.Add("Tag", sID);// 存id 的值

                ctdl.Attributes.Add("sFieldName", sFieldName);//存名字

                // ctdl.Attributes.Add("EquID", ViewState["EquID"].ToString());//存资产id
                if (deploy != null)
                {
                    ctdl.Attributes.Add("deployID", deploy.ID.ToString());//对象ID编号值

                }
                else
                {
                    ctdl.Attributes.Add("deployID", "0");//对象ID编号值

                }
                //给 deptId赋值

                ctdl.DeptID = long.Parse(sSetValue == "" ? "0" : sSetValue);

                if (IsMust == "1")
                {
                    ctdl.MustInput = true;
                    ctdl.TextToolTip = sCHName;
                }

                labTemp = new Label();
                labTemp.ID = tr.ID + "lab" + iLab.ToString();
                labTemp.Text = sCHName;
                ctdl.deptTexChange += new EventHandler(setUserConter);

                if (this.ReadOnly == true)
                {
                    ctdl.ContralState = eOA_FlowControlState.eReadOnly;
                }

                if (displayStatus == 1)    // 可见, 不可编辑
                {
                    ctdl.ContralState = eOA_FlowControlState.eReadOnly;
                    ctdl.MustInput = false;
                }

                //找不到同时也需要比较的情况[新加的配置等情况]
                if (hasCompare == true)
                {
                    if (_xmlHtComp[sID] != null)
                    {
                        //存在历史比较值的情况下，如果不同，则红色显示
                        EQU_deploy deployList = (EQU_deploy)(_xmlHtComp[sID]);
                        //存在历史比较值的情况下，如果不同，则红色显示
                        sSetValueComp = deployList.Value;

                        if (sSetValueComp != sSetValue)
                            labTemp.ForeColor = System.Drawing.Color.Red;
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
                    //   tc.InnerText = sSetValue;
                    //tc.Attributes.Add("title", "查看配置项［" + sSetValue.Trim() + "］相同的资产列表");
                    if (rowcount % 4 == 0)
                    {
                        tc.Attributes.Add("style", "word-break:break-all;");
                    }
                    else
                    {
                        if (width != 82)
                        {
                            tc.Attributes.Add("style", "CURSOR: hand;width:" + width.ToString() + "%");
                        }
                        else
                        {
                            tc.Attributes.Add("style", "word-break:break-all;");
                        }
                    }
                    //tc.Attributes.Add("onclick", "javascript:window.open('../EquipmentManager/frmEqu_SameSchemaItem.aspx?ItemFieldID=" + sID.Trim() + "&ItemFieldValue='+encodeURIComponent('" + sSetValue.Trim() + "'),'newwindow','scrollbars=yes,resizable=yes,top=0,left=0,width=800px,height=600');");
                }
                AddControl(tr, tc);
            }
        }
        #endregion

        private void AddBaseItem(XmlNode relnode, int iRel)
        {
            AddBaseItem(relnode, iRel, false, null);
        }

        private void AddBaseItem(XmlNode relnode, int iRel, bool isEqualGroup, DataTable dtExDisplayWay)
        {
            XmlNodeList ns = null;
            HtmlTable tabMain = new HtmlTable();
            HtmlTableRow tr = new HtmlTableRow();
            HtmlTableCell tc = new HtmlTableCell();
            HtmlImage im = new HtmlImage();
            Label lab1 = new Label();
            string prefix = "RelTab" + System.Guid.NewGuid().ToString();
            tabMain.ID = prefix + iRel.ToString();
            tabMain.Attributes.Add("width", "100%");
            tabMain.Attributes.Add("align", "center");
            tabMain.Attributes.Add("class", "listNewContent");



            //添加标题

            if (relnode.Attributes["Title"] != null && isEqualGroup == false)
            {
                tr.ID = tabMain.ID + "RelTR";
                tr.Attributes.Add("class", "listTitleNew");
                tc.ID = tabMain.ID + "RelTD";
                tc.Attributes.Add("align", "left");
                tc.Attributes.Add("vAlign", "top");
                tc.Attributes.Add("class", "bt_di");

                im.ID = tabMain.ID + "imgBase";
                im.Attributes.Add("class", "icon");
                im.Attributes.Add("onclick", "ShowTableDym(this,'" + this.ClientID + "_" + tabMain.ClientID + "','RelTab');");
                im.Attributes.Add("height", "16");
                im.Attributes.Add("width", "16");
                im.Attributes.Add("align", "absbottom");
                im.Attributes.Add("src", "../Images/icon_collapseall.gif");
                AddControl(tc, im);

                lab1.ID = tabMain.ID + "Title";
                if (relnode.Attributes["Title"].Value.Trim() != "")
                {
                    lab1.Text = relnode.Attributes["Title"].Value;
                }
                else
                {
                    lab1.Text = "基础信息";
                }



                AddControl(tc, lab1);


                AddControl(tr, tc);
                AddControl(tabMain, tr);
            }

            AddControl(this, tabMain);
            //内容
            tabMain = new HtmlTable();
            tabMain.ID = prefix + "RelTabContent" + iRel.ToString();
            tabMain.Attributes.Add("width", "100%");
            tabMain.Attributes.Add("align", "center");
            tabMain.Attributes.Add("class", "listContent");
            int iLab = 0;
            ns = relnode.SelectNodes("AttributeItem");
            bool isEvenNumber = ns.Count % 2 == 0;    // 是否偶数
            int rowcount = 0;
            int width = ns.Count == 1 ? 82 : 35;
            foreach (XmlNode n in ns)
            {
                #region 取该扩展项的可见可编辑状态 - 2013-12-04 @孙绍棕

                int displayStatus = 0;    // 默认可见，可编辑                
                foreach (DataRow dr in dtExDisplayWay.Rows)
                {
                    if (dr["FieldID"].ToString().Equals(n.Attributes["ID"].Value))
                    {
                        int.TryParse(dr["displaystatus"].ToString(), out displayStatus);
                        break;
                    }
                }

                #endregion



                rowcount++;
                if (rowcount % 4 == 1)
                {
                    if (isEvenNumber == false && tr.Cells.Count == 2)
                    {
                        // 若是奇数且该行只有两列, 则需合并单元格.
                        tr.Cells[1].ColSpan = 3;
                    }

                    tr = new HtmlTableRow();
                    tr.ID = tabMain.ID + "RelTab" + rowcount.ToString();
                }
                if ((ns.Count * 2) % 4 != 0 && rowcount == (ns.Count * 2) - 1)
                {
                    width = 82;
                }


                if (n.Attributes["CtrlType"] == null || relnode.Name.Contains("RelationConfig"))    // 关联配置
                {
                    tc = new HtmlTableCell();
                    tc.ID = tr.ID + "_Cell";
                    tc.Attributes.Add("class", "list");
                    tc.Attributes.Add("style", "word-break:break-all");

                    string sID = "";
                    string sSetValue = "";   //设置的值

                    string sSetValueComp = ""; //用于比较的值

                    string sCHName = "";
                    sID = n.Attributes["ID"].Value;

                    string sFieldName = "";
                    sFieldName = n.Attributes["CHName"].Value;

                    if (_fields[sID] != null)
                    {
                        sCHName = _fields[sID].ToString();
                    }
                    if (sCHName != "")
                    {
                        EQU_deploy deploy = null;
                        if (_xmlHt[sID] != null)
                        {
                            //获得控件的值

                            deploy = (EQU_deploy)(_xmlHt[sID]);
                            sSetValue = deploy.Value;
                        }
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


                        chkTemp.Attributes.Add("sFieldName", sFieldName);//存名字

                        //chkTemp.Attributes.Add("EquID", ViewState["EquID"].ToString());//存资产id
                        if (deploy != null)
                        {
                            chkTemp.Attributes.Add("deployID", deploy.ID.ToString());//对象ID编号值

                        }
                        else
                        {
                            chkTemp.Attributes.Add("deployID", "0");//对象ID编号值

                        }


                        if (displayStatus == 1)    // 可见, 不可编辑
                        {
                            chkTemp.Enabled = false;
                        }

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
                            if (sSetValue == "1")
                            {
                                lnkTemp.ToolTip = "查看配置［" + lnkTemp.Text + "］的所有资产";
                            }
                            else
                            {
                                lnkTemp.ToolTip = "查看没有配置［" + lnkTemp.Text + "］的所有资产";
                            }

                            lnkTemp.Attributes.Add("style", "CURSOR: hand");
                            lnkTemp.Attributes.Add("onclick", "javascript:window.open('../EquipmentManager/frmEqu_SameSchemaItem.aspx?ItemFieldID=" + sID.Trim() + "&ItemFieldValue='+encodeURIComponent('" + sSetValue.Trim() + "'),'newwindow','scrollbars=yes,resizable=yes,top=0,left=0,width=800px,height=600');");
                            iLab++;
                            lnkTemp.ID = tr.ID + "lnk" + iLab.ToString();

                            //找不到同时也需要比较的情况[新加的配置等情况]
                            if (hasCompare == true)
                            {
                                if (_xmlHtComp[sID] != null)
                                {
                                    //存在历史比较值的情况下，如果不同，则红色显示
                                    EQU_deploy deployList = (EQU_deploy)(_xmlHtComp[sID]);
                                    //存在历史比较值的情况下，如果不同，则红色显示
                                    sSetValueComp = deployList.Value;

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

                    AddControl(tr, tc);
                }
                else if (n.Attributes["CtrlType"].Value == "TextBox")//textbox
                {
                    AddBaseRow(ref tr, ref rowcount, n, ref iLab, width, displayStatus);
                }
                else if (n.Attributes["CtrlType"].Value == "DropDownList")// 下拉控件
                {
                    AddDropDownListRow(ref tr, ref rowcount, n, ref iLab, width, displayStatus);
                }
                else if (n.Attributes["CtrlType"].Value == "deptList")// 部门
                {
                    AddDetpRow(ref tr, ref rowcount, n, ref iLab, width, displayStatus);
                }
                else if (n.Attributes["CtrlType"].Value == "UserList")// 用户
                {
                    AddUserRow(ref tr, ref rowcount, n, ref iLab, width, displayStatus);
                }
                else if (n.Attributes["CtrlType"].Value == "Time")// 日期
                {
                    AddTimeRow(ref tr, ref rowcount, n, ref iLab, width, displayStatus);
                }
                else if (n.Attributes["CtrlType"].Value == "Number")// 数值
                {
                    AddNumberRow(ref tr, ref rowcount, n, ref iLab, width, displayStatus);
                }
                else if (n.Attributes["CtrlType"].Value == "MultiLine")// 备注
                {
                    AddRemarkRow(ref tr, ref rowcount, n, ref iLab, displayStatus);
                }




                AddControl(tabMain, tr);
            }

            if (isEvenNumber == false && tr.Cells.Count == 2)
            {
                // 若是奇数且该行只有两列, 则需合并单元格.
                tr.Cells[1].ColSpan = 3;
            }

            AddControl(this, tabMain);
        }

        /// <summary>
        /// 扩展项字段: 基础信息
        /// </summary>
        /// <param name="tr"></param>
        /// <param name="rowcount"></param>
        /// <param name="n"></param>
        /// <param name="iLab"></param>
        /// <param name="width"></param>
        /// <param name="displayStatus"></param>
        private void AddBaseRow(ref HtmlTableRow tr, ref int rowcount, XmlNode n, ref int iLab, int width, int displayStatus)
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
            // string sFieldName = "";
            sID = n.Attributes["ID"].Value;
            string sFieldName = "";
            sFieldName = n.Attributes["CHName"].Value;
            string IsMust = n.Attributes["IsMust"].Value;//是否必填
            if (_fields[sID] != null)
            {
                sCHName = _fields[sID].ToString();
            }

            if (sCHName != "")
            {
                //资产属性字段值 类对象

                EQU_deploy deploy = null;
                if (_xmlHt[sID] != null)
                {
                    //获得控件的值

                    deploy = (EQU_deploy)(_xmlHt[sID]);
                    sSetValue = deploy.Value;
                }
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
                ctdl.Attributes.Add("Tag", sID);// 存id 的值

                ctdl.Attributes.Add("sFieldName", sFieldName);//存名字

                //ctdl.Attributes.Add("EquID", ViewState["EquID"].ToString());//存资产id
                if (deploy != null)
                {
                    ctdl.Attributes.Add("deployID", deploy.ID.ToString());//对象ID编号值

                }
                else
                {
                    ctdl.Attributes.Add("deployID", "0");//对象ID编号值

                }


                if (IsMust == "1")
                {
                    ctdl.MustInput = true;
                    ctdl.TextToolTip = sCHName;
                }

                ctdl.MaxLength = 500;
                ctdl.Value = sSetValue;


                labTemp = new Label();
                labTemp.ID = tr.ID + "lab" + iLab.ToString();
                labTemp.Text = sCHName;


                if (this.ReadOnly == true)
                {
                    // ctdl.Visible = false;
                    ctdl.ContralState = eOA_FlowControlState.eReadOnly;

                }

                if (displayStatus == 1)    // 可见, 不可编辑
                {
                    ctdl.ContralState = eOA_FlowControlState.eReadOnly;
                    ctdl.MustInput = false;
                }

                //找不到同时也需要比较的情况[新加的配置等情况]
                if (hasCompare == true)
                {
                    if (_xmlHtComp[sID] != null)
                    {
                        //存在历史比较值的情况下，如果不同，则红色显示
                        EQU_deploy deployList = (EQU_deploy)(_xmlHtComp[sID]);
                        //存在历史比较值的情况下，如果不同，则红色显示
                        sSetValueComp = deployList.Value;

                        if (sSetValueComp != sSetValue)
                            labTemp.ForeColor = System.Drawing.Color.Red;
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
                    //tc.Attributes.Add("title", "查看配置项［" + sSetValue.Trim() + "］相同的资产列表");

                    if (rowcount % 4 == 0)
                    {
                        tc.Attributes.Add("style", "word-break:break-all;");
                    }
                    else
                    {
                        if (width != 82)
                        {
                            tc.Attributes.Add("style", "CURSOR: hand;width:" + width.ToString() + "%");
                        }
                        else
                        {
                            tc.Attributes.Add("style", "word-break:break-all;");
                        }
                    }
                    //tc.Attributes.Add("onclick", "javascript:window.open('../EquipmentManager/frmEqu_SameSchemaItem.aspx?ItemFieldID=" + sID.Trim() + "&ItemFieldValue='+encodeURIComponent('" + sSetValue.Trim() + "'),'newwindow','scrollbars=yes,resizable=yes,top=0,left=0,width=800px,height=600');");
                }
                AddControl(tr, tc);
            }
        }

        private void AddRemarkRow(ref HtmlTableRow tr, ref int rowcount, XmlNode n, ref int iLab, int displayStatus)
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
            string sFieldName = "";
            sFieldName = n.Attributes["CHName"].Value;
            string IsMust = n.Attributes["IsMust"].Value;//是否必填
            if (_fields[sID] != null)
            {
                sCHName = _fields[sID].ToString();
            }

            if (sCHName != "")
            {

                EQU_deploy deploy = null;
                if (_xmlHt[sID] != null)
                {
                    //获得控件的值

                    deploy = (EQU_deploy)(_xmlHt[sID]);
                    sSetValue = deploy.Value;
                }

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
                ctdl.Attributes.Add("Tag", sID);// 存id 的值

                ctdl.Attributes.Add("sFieldName", sFieldName);//存名字

                // ctdl.Attributes.Add("EquID", ViewState["EquID"].ToString());//存资产id
                if (deploy != null)
                {
                    ctdl.Attributes.Add("deployID", deploy.ID.ToString());//对象ID编号值

                }
                else
                {
                    ctdl.Attributes.Add("deployID", "0");//对象ID编号值

                }

                ctdl.Value = sSetValue;
                ctdl.MaxLength = 500;
                ctdl.Width = "98%";
                if (IsMust == "1")
                {
                    ctdl.MustInput = true;
                    ctdl.TextToolTip = sCHName;
                }

                labTemp = new Label();
                labTemp.ID = tr.ID + "lab" + iLab.ToString();
                labTemp.Text = sCHName;


                if (this.ReadOnly == true)
                {
                    // ctdl.Visible = false;
                    ctdl.ContralState = eOA_FlowControlState.eReadOnly;

                }

                if (displayStatus == 1)    // 可见, 不可编辑
                {
                    ctdl.ContralState = eOA_FlowControlState.eReadOnly;
                    ctdl.MustInput = false;
                }

                //找不到同时也需要比较的情况[新加的配置等情况]
                if (hasCompare == true)
                {
                    if (_xmlHtComp[sID] != null)
                    {
                        EQU_deploy deployList = (EQU_deploy)(_xmlHtComp[sID]);
                        //存在历史比较值的情况下，如果不同，则红色显示
                        sSetValueComp = deployList.Value;
                        if (sSetValueComp != sSetValue)
                            labTemp.ForeColor = System.Drawing.Color.Red;
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
                    //tc.Attributes.Add("title", "查看配置项［" + sSetValue.Trim() + "］相同的资产列表");
                    tc.Attributes.Add("style", "CURSOR: hand;");
                    //tc.Attributes.Add("onclick", "javascript:window.open('../EquipmentManager/frmEqu_SameSchemaItem.aspx?ItemFieldID=" + sID.Trim() + "&ItemFieldValue='+encodeURIComponent('" + sSetValue.Trim() + "'),'newwindow','scrollbars=yes,resizable=yes,top=0,left=0,width=800px,height=600');");
                }

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

    public class ExprPropertyComparser : IComparer<String>
    {
        private Regex _r;
        private String _strCompareProperty;
        public ExprPropertyComparser(String strCompareProperty)
        {
            this._strCompareProperty = strCompareProperty;

            if (this._strCompareProperty.Equals("title"))    // 按title属性排序
                _r = new Regex("title=\"(.*?)\"", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            else    // 按id属性排序
                _r = new Regex("orderby=\"(.*?)\"", RegexOptions.IgnoreCase | RegexOptions.Multiline);
        }

        #region IComparer<string> 成员

        public int Compare(string x, string y)
        {
            // Title="aadsf"

            Match mX = _r.Match(x);
            Match mY = _r.Match(y);

            String strX = mX.Groups[1].Value.Trim();
            String strY = mY.Groups[1].Value.Trim();

            if (this._strCompareProperty.Equals("title"))    // 按title属性排序
            {
                return strX.CompareTo(strY);
            }
            else    // 按id属性排序
            {
                return int.Parse(strX).CompareTo(int.Parse(strY));
            }

        }

        #endregion
    }

}