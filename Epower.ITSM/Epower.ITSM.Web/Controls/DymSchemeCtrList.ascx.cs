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
using Epower.ITSM.SqlDAL.ES_TBLCS;

namespace Epower.ITSM.Web.Controls
{
    public partial class DymSchemeCtrList : System.Web.UI.UserControl
    {

        #region 属性定义区
        /// <summary>
        /// 设备分类编号 [注意两中用法只能取一种,要么设置 分类ID,要么直接设置 SCHEMA]
        /// </summary>
        public long EquCategoryID
        {
            set
            {
                ViewState[this.ID + "EquCategoryID"] = value;
                //回发时赋值 重新加载界面
                LoadHtmlControls(value);

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

        private void LoadHtmlControlsForSchema(string strXml)
        {
            Equ_SchemaItemsDP ee = new Equ_SchemaItemsDP();
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
                bnodes = xmldoc.SelectNodes("EquScheme/BaseItem");
                AddBaseSchemeControls();

                remarkNodes = xmldoc.SelectNodes("EquScheme/Remark");
                AddRemarkSchemeControls();

                relnodes = xmldoc.SelectNodes("EquScheme/RelationConfig");
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

            if (Version != -1)
            {
                //取历史版本的strXml
                strXml = Equ_DeskDP.isversionRtnXml(Version, long.Parse(ViewState["EquID"].ToString()));
                if (strXml == "")
                {
                    strXml = Epower.ITSM.SqlDAL.Equ_SubjectDP.GetCatalogSchema(lngID);
                }
            }
            else
            {
                strXml = Epower.ITSM.SqlDAL.Equ_SubjectDP.GetCatalogSchema(lngID);
            }


            Equ_SchemaItemsDP ee = new Equ_SchemaItemsDP();
            if (Version != -1)
            {
                //取历史值的情况
                _fields = ee.GetAllFieldsHistory();
            }
            else
            {
                _fields = ee.GetAllFields();   //获取最新的配置项情况
            }
            //回发情况下，清除前先保存一下之前控件的值
            //if (Page.IsPostBack == true)
            //{
            //    Hashtable ht = new Hashtable();
            //    ValueCollection(this, ref ht);
            //    _xmlHt = ht;
            //}
            //清除现有所有控件
            this.Controls.Clear();
            if (strXml != "")
            {
                //由于做了清除需要重新注册脚本
                RegisterClientScrip();
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.LoadXml(strXml);
                baseNode = xmldoc.DocumentElement;
                bnodes = xmldoc.SelectNodes("EquScheme/BaseItem");
                AddBaseSchemeControls();

                remarkNodes = xmldoc.SelectNodes("EquScheme/Remark");
                AddRemarkSchemeControls();

                relnodes = xmldoc.SelectNodes("EquScheme/RelationConfig");
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
                            ((UserPicker)pControl).DeptID = vardept.DeptID;
                            ((UserPicker)pControl).UserID = 0;
                            ((UserPicker)pControl).UserName = "";

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
                                deploy.EquID = long.Parse(((CtrTextDropList)pControl).Attributes["EquID"]);
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
                                deploy.EquID = long.Parse(((CheckBox)pControl).Attributes["EquID"]);
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
                                deploy.EquID = long.Parse(((ctrFlowCataDropList)pControl).Attributes["EquID"]);
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
                                deploy.EquID = long.Parse(((DeptPicker)pControl).Attributes["EquID"]);
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
                                deploy.EquID = long.Parse(((UserPicker)pControl).Attributes["EquID"]);
                                deploy.FieldID = long.Parse(((UserPicker)pControl).Attributes["Tag"]);
                                deploy.CHName = ((UserPicker)pControl).Attributes["sFieldName"];
                                deploy.Value = ((UserPicker)pControl).UserID.ToString();
                                list.Add(((UserPicker)pControl).Attributes["Tag"], deploy);
                            }
                            break;

                        case "controls_ctrdateandtimev2_ascx":
                            if (pControl.ID.StartsWith("tDynamic"))
                            {
                                EQU_deploy deploy = new EQU_deploy();
                                deploy.ID = long.Parse(((CtrDateAndTimeV2)pControl).Attributes["deployID"]);
                                deploy.EquID = long.Parse(((CtrDateAndTimeV2)pControl).Attributes["EquID"]);
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
                                deploy.EquID = long.Parse(((CtrDateAndTime)pControl).Attributes["EquID"]);
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
                                deploy.EquID = long.Parse(((CtrFlowNumeric)pControl).Attributes["EquID"]);
                                deploy.FieldID = long.Parse(((CtrFlowNumeric)pControl).Attributes["Tag"]);
                                deploy.CHName = ((CtrFlowNumeric)pControl).Attributes["sFieldName"];
                                deploy.Value = ((CtrFlowNumeric)pControl).Value.ToString();
                                list.Add(((CtrFlowNumeric)pControl).Attributes["Tag"], deploy);
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
            tr.Attributes.Add("class", "listTitleNew");
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
                if (relnode.Attributes["Title"].Value.Trim() != "")
                {
                    lab1.Text = relnode.Attributes["Title"].Value;
                }
                else
                {
                    lab1.Text = "关联信息";
                }

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
                    chkTemp.Attributes.Add("EquID", ViewState["EquID"].ToString());//存资产id
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
                            lnkTemp.ToolTip = "查看配置项［" + lnkTemp.Text + "］相同的资产列表";

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
                        ctdl.FieldsSourceType = 1;   //异步获取方式
                        ctdl.FieldsSourceID = "SchemaItem_" + sID;
                        ctdl.ID = "tDynamictxt" + sID;
                        ctdl.Attributes.Add("Tag", sID);

                        ctdl.Attributes.Add("sFieldName", sFieldName);//存名字
                        ctdl.Attributes.Add("EquID", ViewState["EquID"].ToString());//存资产id
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
                            ctdl.Visible = false;


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
            XmlNodeList ns = null;
            HtmlTable tabMain = new HtmlTable();
            HtmlTableRow tr = new HtmlTableRow();
            HtmlTableCell tc = new HtmlTableCell();
            HtmlImage im = new HtmlImage();
            Label lab1 = new Label();
            //循环递归
            tabMain.ID = "RemarkTab" + iRel.ToString();
            tabMain.Attributes.Add("width", "100%");
            tabMain.Attributes.Add("align", "center");
            tabMain.Attributes.Add("class", "listNewContent");

            //添加标题
            tr.ID = tabMain.ID + "RemarkTR";
            tr.Attributes.Add("class", "listTitleNew");
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

        #region 添加下拉控件值  --yanghw  2011-08-10
        //此控件修改
        private void AddDropDownListRow(ref HtmlTableRow tr, ref int rowcount, XmlNode n, ref int iLab, int width)
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
            Equ_SchemaItemsDP ee = new Equ_SchemaItemsDP();
            ee = ee.GetReCorded(sID);

            //long RootID = long.Parse(n.Attributes["Default"].Value == "" ? "0" : n.Attributes["Default"].Value);
            long RootID = long.Parse(ee.CatalogID.ToString() == "" ? "1" : ee.CatalogID.ToString());

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
                ctdl.Attributes.Add("EquID", ViewState["EquID"].ToString());//存资产id
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
                //ctdl.Width = Unit.Parse("70%");

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
                    tc.Attributes.Add("title", "查看配置项［" + sSetValue.Trim() + "］相同的资产列表");



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
                    tc.Attributes.Add("onclick", "javascript:window.open('../EquipmentManager/frmEqu_SameSchemaItem.aspx?ItemFieldID=" + sID.Trim() + "&ItemFieldValue='+encodeURIComponent('" + sSetValue.Trim() + "'),'newwindow','scrollbars=yes,resizable=yes,top=0,left=0,width=800px,height=600');");
                }
                AddControl(tr, tc);
            }
        }
        #endregion

        #region 数值类型控件  --yanghw  2011-08-11

        //数值类型
        private void AddNumberRow(ref HtmlTableRow tr, ref int rowcount, XmlNode n, ref int iLab, int width)
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
                ctdl.Attributes.Add("EquID", ViewState["EquID"].ToString());//存资产id
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
                    tc.Attributes.Add("title", "查看配置项［" + sSetValue.Trim() + "］相同的资产列表");





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
                    tc.Attributes.Add("onclick", "javascript:window.open('../EquipmentManager/frmEqu_SameSchemaItem.aspx?ItemFieldID=" + sID.Trim() + "&ItemFieldValue='+encodeURIComponent('" + sSetValue.Trim() + "'),'newwindow','scrollbars=yes,resizable=yes,top=0,left=0,width=800px,height=600');");
                }
                AddControl(tr, tc);
            }
        }
        #endregion

        #region 时间类型控件  --yanghw  2011-08-11
        //时间类型
        private void AddTimeRow(ref HtmlTableRow tr, ref int rowcount, XmlNode n, ref int iLab, int width)
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
            string isChack = n.Attributes["isChack"].Value;// 是否时间 0, 不是; 1, 是.
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
                ctdl.Attributes.Add("EquID", ViewState["EquID"].ToString());//存资产id
                if (deploy != null)
                {
                    ctdl.Attributes.Add("deployID", deploy.ID.ToString());//对象ID编号值
                }
                else
                {
                    ctdl.Attributes.Add("deployID", "0");//对象ID编号值
                }

                //   ctdl.CatelogID = long.Parse(sSetValue == "" ? "0" : sSetValue);


                if (isChack != "1")
                {
                    ctdl.ShowTime = false;
                    ctdl.ShowMinute = false;
                }

                //给 deptId赋值
                ctdl.dateTimeString = sSetValue;                

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
                    tc.Attributes.Add("title", "查看配置项［" + sSetValue.Trim() + "］相同的资产列表");


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
                    tc.Attributes.Add("onclick", "javascript:window.open('../EquipmentManager/frmEqu_SameSchemaItem.aspx?ItemFieldID=" + sID.Trim() + "&ItemFieldValue='+encodeURIComponent('" + sSetValue.Trim() + "'),'newwindow','scrollbars=yes,resizable=yes,top=0,left=0,width=800px,height=600');");
                }
                AddControl(tr, tc);
            }
        }
        #endregion


        #region 用户信息控件  --yanghw  2011-08-10
        //部门控件
        private void AddUserRow(ref HtmlTableRow tr, ref int rowcount, XmlNode n, ref int iLab, int width)
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
                ctdl.Attributes.Add("EquID", ViewState["EquID"].ToString());//存资产id
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
                    tc.Attributes.Add("title", "查看配置项［" + sSetValue.Trim() + "］相同的资产列表");


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
                    tc.Attributes.Add("onclick", "javascript:window.open('../EquipmentManager/frmEqu_SameSchemaItem.aspx?ItemFieldID=" + sID.Trim() + "&ItemFieldValue='+encodeURIComponent('" + sSetValue.Trim() + "'),'newwindow','scrollbars=yes,resizable=yes,top=0,left=0,width=800px,height=600');");
                }
                AddControl(tr, tc);
            }
        }
        #endregion

        #region 部门控件  --yanghw  2011-08-10
        //部门控件
        private void AddDetpRow(ref HtmlTableRow tr, ref int rowcount, XmlNode n, ref int iLab, int width)
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
                ctdl.Attributes.Add("EquID", ViewState["EquID"].ToString());//存资产id
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
                    tc.Attributes.Add("title", "查看配置项［" + sSetValue.Trim() + "］相同的资产列表");
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
                    tc.Attributes.Add("onclick", "javascript:window.open('../EquipmentManager/frmEqu_SameSchemaItem.aspx?ItemFieldID=" + sID.Trim() + "&ItemFieldValue='+encodeURIComponent('" + sSetValue.Trim() + "'),'newwindow','scrollbars=yes,resizable=yes,top=0,left=0,width=800px,height=600');");
                }
                AddControl(tr, tc);
            }
        }
        #endregion

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
            tr.Attributes.Add("class", "listTitleNew");
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
                if (relnode.Attributes["Title"].Value.Trim() != "")
                {
                    lab1.Text = relnode.Attributes["Title"].Value;
                }
                else
                {
                    lab1.Text = "基础信息";
                }



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

                if (n.Attributes["CtrlType"].Value == "TextBox")//textbox
                {
                    AddBaseRow(ref tr, ref rowcount, n, ref iLab, width);
                }
                else if (n.Attributes["CtrlType"].Value == "DropDownList")// 下拉控件
                {
                    AddDropDownListRow(ref tr, ref rowcount, n, ref iLab, width);
                }
                else if (n.Attributes["CtrlType"].Value == "deptList")// 部门
                {
                    AddDetpRow(ref tr, ref rowcount, n, ref iLab, width);
                }
                else if (n.Attributes["CtrlType"].Value == "UserList")// 用户
                {
                    AddUserRow(ref tr, ref rowcount, n, ref iLab, width);
                }
                else if (n.Attributes["CtrlType"].Value == "Time")// 日期
                {
                    AddTimeRow(ref tr, ref rowcount, n, ref iLab, width);
                }
                else if (n.Attributes["CtrlType"].Value == "Number")// 数值
                {
                    AddNumberRow(ref tr, ref rowcount, n, ref iLab, width);
                }
                AddControl(tabMain, tr);
            }
            AddControl(this, tabMain);
        }

        //此控件修改
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
                ctdl.Attributes.Add("EquID", ViewState["EquID"].ToString());//存资产id
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
                    ctdl.Visible = false;
                    ctdl.ContralState = eOA_FlowControlState.eReadOnly;

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
                    tc.Attributes.Add("title", "查看配置项［" + sSetValue.Trim() + "］相同的资产列表");

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
                ctdl.Attributes.Add("EquID", ViewState["EquID"].ToString());//存资产id
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
                    ctdl.Visible = false;
                    ctdl.ContralState = eOA_FlowControlState.eReadOnly;

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
                    tc.Attributes.Add("title", "查看配置项［" + sSetValue.Trim() + "］相同的资产列表");
                    tc.Attributes.Add("style", "CURSOR: hand;");
                    tc.Attributes.Add("onclick", "javascript:window.open('../EquipmentManager/frmEqu_SameSchemaItem.aspx?ItemFieldID=" + sID.Trim() + "&ItemFieldValue='+encodeURIComponent('" + sSetValue.Trim() + "'),'newwindow','scrollbars=yes,resizable=yes,top=0,left=0,width=800px,height=600');");
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
}
