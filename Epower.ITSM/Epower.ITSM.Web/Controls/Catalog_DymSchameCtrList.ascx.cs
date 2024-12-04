
/*******************************************************************
 * 版权所有：深圳市非凡信息技术有限公司
 * 描述：常用类别配置项用户控件

 * 
 * 
 * 创建人：余向前
 * 创建日期：2013-05-20 
 * 
 * 修改日志：
 * 修改时间：
 * 修改描述：

 * *****************************************************************/
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
    public partial class Catalog_DymSchameCtrList : System.Web.UI.UserControl
    {

        #region 属性定义区

        /// <summary>
        /// 设置关联类型(必须配置)
        /// 对应的值由Epower.ITSM.Base下的FMEnum.cs里的eSchemaRelateType枚举提供
        /// </summary>
        public eSchemaRelateType SchemaRelateType
        {
            set
            {
                ViewState[this.ID + "RelateType"] = (int)value;
            }
            get
            {
                return (eSchemaRelateType)ViewState[this.ID + "RelateType"];
            }
        }

        /// <summary>
        /// 常用分类ID [注意两中用法只能取一种,要么设置 分类ID,要么直接设置 SCHEMA]
        /// </summary>
        public long CatalogID
        {
            set
            {
                ViewState[this.ID + "CatalogID"] = value;
                //回发时赋值 重新加载界面
                LoadHtmlControls(value);

            }
            get
            {
                if (ViewState[this.ID + "CatalogID"] == null)
                {
                    return 0;
                }
                else
                {
                    return (long)ViewState[this.ID + "CatalogID"];
                }
            }
        }

        /// <summary>
        /// 类别配置schema  [注意两中用法只能取一种,要么设置 分类ID,要么直接设置 SCHEMA]
        /// </summary>
        public string CatalogSchema
        {
            set
            {
                ViewState[this.ID + "CatalogSchema"] = value;
                //回发时赋值 重新加载界面
                LoadHtmlControlsForSchema(value);
            }
            get
            {
                if (ViewState[this.ID + "CatalogSchema"] == null)
                {
                    return "";
                }
                else
                {
                    return ViewState[this.ID + "CatalogSchema"].ToString();
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

        /// <summary>
        /// 存储常用类别配置项表的所有配置项信息
        /// </summary>
        private Hashtable _fields = new Hashtable();

        /// <summary>
        /// 返回控件的值List形式
        /// </summary>
        public List<BR_Schema_Deploy> contorRtnValue
        {
            get
            {
                Hashtable ht = new Hashtable();
                ValueCollection(this, ref ht);
                return GetHashTableRtnValue(ht);
            }
        }

        /// <summary>
        /// 关联ID
        /// </summary>
        public long RelateID
        {
            set
            {
                ViewState[this.ID + "RelateID"] = value;
                _xmlHt = SetHashTableXmlValue();
                if (value == 0)
                {
                    SetAddEquTrue();
                }
                ViewState[this.ID + "xmlHT"] = _xmlHt;
            }
            get
            {
                if (ViewState[this.ID + "RelateID"] == null)
                    return 0;
                else
                    return long.Parse(ViewState[this.ID + "RelateID"].ToString());
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
                LoadHtmlControls(CatalogID);

                _xmlHt = (Hashtable)ViewState[this.ID + "xmlHT"];
            }
        }

        #region 根据配置XML串加载控件
        /// <summary>
        /// 根据配置XML串加载控件
        /// </summary>
        /// <param name="strXml"></param>
        private void LoadHtmlControlsForSchema(string strXml)
        {
            BR_CatalogSchemaItemsDP ee = new BR_CatalogSchemaItemsDP();
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
        #endregion

        #region 根据传入的分类ID加载控件
        /// <summary>
        /// 根据传入的分类ID加载控件
        /// </summary>
        /// <param name="lngID"></param>
        private void LoadHtmlControls(long lngID)
        {
            string strXml = "";
            //获取分类的配置XML串信息
            strXml = CatalogDP.GetCatalogSchema(lngID);


            BR_CatalogSchemaItemsDP ee = new BR_CatalogSchemaItemsDP();

            _fields = ee.GetAllFields();   //获取最新的配置项情况

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
        #endregion

        #region 是否当前在编辑状态下更改了分类
        /// <summary>
        /// 是否当前在编辑状态下更改了分类。 [这种情况下，对于没有存在原来取值的情况下，获取配置的缺省设置] 不需要viewstate 保存
        /// </summary>
        public void SetChangeCatalogTrue()
        {
            _blnChangeCatalog = true;
        }
        #endregion

        #region 是否当前是否为新增分类
        /// <summary>
        /// 是否当前是否为新增分类。 [在没有原来取值情况下，获取配置的缺省设置] 不需要viewstate 保存
        /// </summary>
        public void SetAddEquTrue()
        {
            _blnAddEqu = true;
        }
        #endregion

        #region 给页面注册脚本
        /// <summary>
        /// 给页面注册脚本
        /// </summary>
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

        #region List与hashtable之间转换函数
        /// <summary>
        /// 把Hashtable转化成List对象，以便于使用
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        private List<BR_Schema_Deploy> GetHashTableRtnValue(Hashtable ht)
        {
            List<BR_Schema_Deploy> list = new List<BR_Schema_Deploy>();  //实例对象集合       
            IDictionaryEnumerator myEnumerator = ht.GetEnumerator();
            while (myEnumerator.MoveNext())
            {
                BR_Schema_Deploy Entity = (BR_Schema_Deploy)(myEnumerator.Value);//给对象赋值
                list.Add(Entity);
            }
            return list;         //返回list 数据集合
        }

        /// <summary>
        /// 获得资产的 值，并把 资产属性的集合 转化成 Hashtable 对象，便于xml操作
        /// </summary>        
        /// <returns></returns>
        private Hashtable SetHashTableXmlValue()
        {
            long lngRelateID = CTools.ToInt64(ViewState[this.ID + "RelateID"].ToString());
            int intRelateType = CTools.ToInt(ViewState[this.ID + "RelateType"].ToString());

            Hashtable ht = new Hashtable();
            string strTmp = "";
            try
            {
                //取对象的值 把对象值加载到 Hashtable 中
                List<BR_Schema_Deploy> deployList = BR_Schema_Deploy.getDeployList(lngRelateID, intRelateType);
                if (deployList.Count > 0)
                {
                    foreach (BR_Schema_Deploy deploy in deployList)
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

        #region 改变部门，改变用户
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
                                //把控件中的值赋给 对象BR_Schema_Deploy
                                BR_Schema_Deploy deploy = new BR_Schema_Deploy();
                                deploy.ID = CTools.ToInt64(((CtrTextDropList)pControl).Attributes["deployID"]);
                                deploy.RelateID = CTools.ToInt64(((CtrTextDropList)pControl).Attributes["RelateID"]);
                                deploy.RelateType = CTools.ToInt(((CtrTextDropList)pControl).Attributes["RelateType"]);
                                deploy.FieldID = CTools.ToInt64(((CtrTextDropList)pControl).Attributes["Tag"]);
                                deploy.CHName = ((CtrTextDropList)pControl).Attributes["sFieldName"];
                                deploy.Value = ((CtrTextDropList)pControl).Value;
                                list.Add(((CtrTextDropList)pControl).Attributes["Tag"], deploy);
                            }
                            break;
                        case "CheckBox":
                            if (pControl.ID.StartsWith("tDynamic"))
                            {
                                BR_Schema_Deploy deploy = new BR_Schema_Deploy();
                                deploy.ID = CTools.ToInt64(((CheckBox)pControl).Attributes["deployID"]);
                                deploy.RelateID = CTools.ToInt64(((CheckBox)pControl).Attributes["RelateID"]);
                                deploy.RelateType = CTools.ToInt(((CheckBox)pControl).Attributes["RelateType"]);
                                deploy.FieldID = CTools.ToInt64(((CheckBox)pControl).Attributes["Tag"]);
                                deploy.CHName = ((CheckBox)pControl).Attributes["sFieldName"];
                                deploy.Value = ((CheckBox)pControl).Checked == true ? "1" : "0";
                                list.Add(((CheckBox)pControl).Attributes["Tag"], deploy);
                            }
                            break;
                        case "controls_ctrflowcatadroplist_ascx"://下拉控件
                            if (pControl.ID.StartsWith("tDynamic"))
                            {
                                BR_Schema_Deploy deploy = new BR_Schema_Deploy();
                                deploy.ID = CTools.ToInt64(((ctrFlowCataDropList)pControl).Attributes["deployID"]);
                                deploy.RelateID = CTools.ToInt64(((ctrFlowCataDropList)pControl).Attributes["RelateID"]);
                                deploy.RelateType = CTools.ToInt(((ctrFlowCataDropList)pControl).Attributes["RelateType"]);
                                deploy.FieldID = CTools.ToInt64(((ctrFlowCataDropList)pControl).Attributes["Tag"]);
                                deploy.CHName = ((ctrFlowCataDropList)pControl).Attributes["sFieldName"];
                                deploy.Value = ((ctrFlowCataDropList)pControl).CatelogID.ToString();
                                list.Add(((ctrFlowCataDropList)pControl).Attributes["Tag"], deploy);
                            }
                            break;
                        case "controls_ctrflowcatacheckbox_ascx"://复选控件
                            if (pControl.ID.StartsWith("tDynamic"))
                            {
                                BR_Schema_Deploy deploy = new BR_Schema_Deploy();
                                deploy.ID = CTools.ToInt64(((ctrFlowCataCheckBox)pControl).Attributes["deployID"]);
                                deploy.RelateID = CTools.ToInt64(((ctrFlowCataCheckBox)pControl).Attributes["RelateID"]);
                                deploy.RelateType = CTools.ToInt(((ctrFlowCataCheckBox)pControl).Attributes["RelateType"]);
                                deploy.FieldID = CTools.ToInt64(((ctrFlowCataCheckBox)pControl).Attributes["Tag"]);
                                deploy.CHName = ((ctrFlowCataCheckBox)pControl).Attributes["sFieldName"];
                                deploy.Value = ((ctrFlowCataCheckBox)pControl).CatelogValue;
                                list.Add(((ctrFlowCataCheckBox)pControl).Attributes["Tag"], deploy);
                            }
                            break;
                        case "controls_deptpicker_ascx"://部门控件
                            if (pControl.ID.StartsWith("tDynamic"))
                            {
                                BR_Schema_Deploy deploy = new BR_Schema_Deploy();
                                deploy.ID = CTools.ToInt64(((DeptPicker)pControl).Attributes["deployID"]);
                                deploy.RelateID = CTools.ToInt64(((DeptPicker)pControl).Attributes["RelateID"]);
                                deploy.RelateType = CTools.ToInt(((DeptPicker)pControl).Attributes["RelateType"]);
                                deploy.FieldID = CTools.ToInt64(((DeptPicker)pControl).Attributes["Tag"]);
                                deploy.CHName = ((DeptPicker)pControl).Attributes["sFieldName"];
                                deploy.Value = ((DeptPicker)pControl).DeptID.ToString();
                                list.Add(((DeptPicker)pControl).Attributes["Tag"], deploy);
                            }
                            break;
                        case "controls_userpicker_ascx"://用户信息控件
                            if (pControl.ID.StartsWith("tDynamic"))
                            {
                                BR_Schema_Deploy deploy = new BR_Schema_Deploy();
                                deploy.ID = CTools.ToInt64(((UserPicker)pControl).Attributes["deployID"]);
                                deploy.RelateID = CTools.ToInt64(((UserPicker)pControl).Attributes["RelateID"]);
                                deploy.RelateType = CTools.ToInt(((UserPicker)pControl).Attributes["RelateType"]);
                                deploy.FieldID = CTools.ToInt64(((UserPicker)pControl).Attributes["Tag"]);
                                deploy.CHName = ((UserPicker)pControl).Attributes["sFieldName"];
                                deploy.Value = ((UserPicker)pControl).UserID.ToString();
                                list.Add(((UserPicker)pControl).Attributes["Tag"], deploy);
                            }
                            break;
                        case "controls_ctrdateandtimev2_ascx"://时间控件
                            if (pControl.ID.StartsWith("tDynamic"))
                            {
                                BR_Schema_Deploy deploy = new BR_Schema_Deploy();
                                deploy.ID = CTools.ToInt64(((CtrDateAndTimeV2)pControl).Attributes["deployID"]);
                                deploy.RelateID = CTools.ToInt64(((CtrDateAndTimeV2)pControl).Attributes["RelateID"]);
                                deploy.RelateType = CTools.ToInt(((CtrDateAndTimeV2)pControl).Attributes["RelateType"]);
                                deploy.FieldID = CTools.ToInt64(((CtrDateAndTimeV2)pControl).Attributes["Tag"]);
                                deploy.CHName = ((CtrDateAndTimeV2)pControl).Attributes["sFieldName"];
                                deploy.Value = ((CtrDateAndTimeV2)pControl).dateTimeString.ToString();
                                list.Add(((CtrDateAndTimeV2)pControl).Attributes["Tag"], deploy);
                            }
                            break;
                        case "controls_ctrflownumeric_ascx"://数字输入框控件
                            if (pControl.ID.StartsWith("tDynamic"))
                            {
                                BR_Schema_Deploy deploy = new BR_Schema_Deploy();
                                deploy.ID = CTools.ToInt64(((CtrFlowNumeric)pControl).Attributes["deployID"]);
                                deploy.RelateID = CTools.ToInt64(((CtrFlowNumeric)pControl).Attributes["RelateID"]);
                                deploy.RelateType = CTools.ToInt(((CtrFlowNumeric)pControl).Attributes["RelateType"]);
                                deploy.FieldID = CTools.ToInt64(((CtrFlowNumeric)pControl).Attributes["Tag"]);
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

        #region 添加关联类型
        /// <summary>
        /// 添加关联类型
        /// </summary>
        /// <param name="relnode"></param>
        /// <param name="iRel"></param>
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
                    BR_Schema_Deploy deploy = null;
                    if (_xmlHt[sID] != null)
                    {
                        //获得控件的值
                        deploy = (BR_Schema_Deploy)(_xmlHt[sID]);
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
                    chkTemp.Attributes.Add("RelateID", ViewState[this.ID + "RelateID"].ToString());//存关联ID
                    chkTemp.Attributes.Add("RelateType", ViewState[this.ID + "RelateType"].ToString());//存关联类型
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
        #endregion

        #region 添加基本类型
        /// <summary>
        /// 添加基本类型
        /// </summary>
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
        /// <summary>
        /// 添加基本类型
        /// </summary>
        /// <param name="relnode"></param>
        /// <param name="iRel"></param>
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
                else if (n.Attributes["CtrlType"].Value == "CheckBox")// 复选框
                {
                    AddCheckBoxRow(ref tr, ref rowcount, n, ref iLab, width);
                }
                AddControl(tabMain, tr);
            }
            AddControl(this, tabMain);
        }

        #endregion

        #region 添加备注类型
        /// <summary>
        /// 添加备注类型
        /// </summary>
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

        #endregion

        #region 添加下拉控件值
        /// <summary>
        /// 添加下拉控件值
        /// </summary>
        /// <param name="tr"></param>
        /// <param name="rowcount"></param>
        /// <param name="n"></param>
        /// <param name="iLab"></param>
        /// <param name="width"></param>
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
            BR_CatalogSchemaItemsDP ee = new BR_CatalogSchemaItemsDP();
            ee = ee.GetReCorded(sID);

            long RootID = CTools.ToInt64(ee.CatalogID.ToString() == "" ? "1" : ee.CatalogID.ToString());

            if (_fields[sID] != null)
            {
                sCHName = _fields[sID].ToString();
            }

            if (sCHName != "")
            {
                //资产属性字段值 类对象
                BR_Schema_Deploy deploy = null;
                if (_xmlHt[sID] != null)
                {
                    //获得控件的值
                    deploy = (BR_Schema_Deploy)(_xmlHt[sID]);
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

                ctdl.ID = "tDynamictxt" + sID;
                ctdl.Attributes.Add("Tag", sID);// 存id 的值
                ctdl.Attributes.Add("sFieldName", sFieldName);//存名字                
                ctdl.Attributes.Add("RelateID", ViewState[this.ID + "RelateID"].ToString());//存关联ID
                ctdl.Attributes.Add("RelateType", ViewState[this.ID + "RelateType"].ToString());//存关联类型
                if (deploy != null)
                {
                    ctdl.Attributes.Add("deployID", deploy.ID.ToString());//对象ID编号值
                }
                else
                {
                    ctdl.Attributes.Add("deployID", "0");//对象ID编号值
                }
                ctdl.RootID = RootID;
                ctdl.CatelogID = CTools.ToInt64(sSetValue == "" ? "0" : sSetValue);

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
                AddControl(tr, tc);
            }
        }
        #endregion

        #region 数值类型控件
        /// <summary>
        /// 数值类型控件
        /// </summary>
        /// <param name="tr"></param>
        /// <param name="rowcount"></param>
        /// <param name="n"></param>
        /// <param name="iLab"></param>
        /// <param name="width"></param>
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
            long RootID = CTools.ToInt64(n.Attributes["Default"].Value == "" ? "0" : n.Attributes["Default"].Value);

            if (_fields[sID] != null)
            {
                sCHName = _fields[sID].ToString();
            }

            if (sCHName != "")
            {
                //资产属性字段值 类对象
                BR_Schema_Deploy deploy = null;
                if (_xmlHt[sID] != null)
                {
                    //获得控件的值
                    deploy = (BR_Schema_Deploy)(_xmlHt[sID]);
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
                ctdl.ID = "tDynamictxt" + sID;
                ctdl.Attributes.Add("Tag", sID);// 存id 的值
                ctdl.Attributes.Add("sFieldName", sFieldName);//存名字
                ctdl.Attributes.Add("RelateID", ViewState[this.ID + "RelateID"].ToString());//存关联ID
                ctdl.Attributes.Add("RelateType", ViewState[this.ID + "RelateType"].ToString());//存关联类型
                if (deploy != null)
                {
                    ctdl.Attributes.Add("deployID", deploy.ID.ToString());//对象ID编号值
                }
                else
                {
                    ctdl.Attributes.Add("deployID", "0");//对象ID编号值
                }
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
                AddControl(tr, tc);
            }
        }
        #endregion

        #region 时间类型控件
        /// <summary>
        /// 时间类型控件
        /// </summary>
        /// <param name="tr"></param>
        /// <param name="rowcount"></param>
        /// <param name="n"></param>
        /// <param name="iLab"></param>
        /// <param name="width"></param>
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
            string isChack = n.Attributes["isChack"].Value;//是否显示小时分钟
            string IsMust = n.Attributes["IsMust"].Value;//是否必填
            //绑定值
            long RootID = CTools.ToInt64(n.Attributes["Default"].Value == "" ? "0" : n.Attributes["Default"].Value);

            if (_fields[sID] != null)
            {
                sCHName = _fields[sID].ToString();
            }

            if (sCHName != "")
            {
                //资产属性字段值 类对象
                BR_Schema_Deploy deploy = null;
                if (_xmlHt[sID] != null)
                {
                    //获得控件的值
                    deploy = (BR_Schema_Deploy)(_xmlHt[sID]);
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
                ctdl.ID = "tDynamictxt" + sID;
                ctdl.Attributes.Add("Tag", sID);// 存id 的值
                ctdl.Attributes.Add("sFieldName", sFieldName);//存名字
                ctdl.Attributes.Add("RelateID", ViewState[this.ID + "RelateID"].ToString());//存关联ID
                ctdl.Attributes.Add("RelateType", ViewState[this.ID + "RelateType"].ToString());//存关联类型
                if (deploy != null)
                {
                    ctdl.Attributes.Add("deployID", deploy.ID.ToString());//对象ID编号值
                }
                else
                {
                    ctdl.Attributes.Add("deployID", "0");//对象ID编号值
                }

                //赋值
                ctdl.dateTimeString = sSetValue;
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

                labTemp = new Label();
                labTemp.ID = tr.ID + "lab" + iLab.ToString();
                labTemp.Text = sCHName;


                if (this.ReadOnly == true)
                {
                    ctdl.ContralState = eOA_FlowControlState.eReadOnly;
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
                AddControl(tr, tc);
            }
        }
        #endregion

        #region 用户信息控件
        /// <summary>
        /// 用户信息控件
        /// </summary>
        /// <param name="tr"></param>
        /// <param name="rowcount"></param>
        /// <param name="n"></param>
        /// <param name="iLab"></param>
        /// <param name="width"></param>
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
            long RootID = CTools.ToInt64(n.Attributes["Default"].Value == "" ? "0" : n.Attributes["Default"].Value);

            if (_fields[sID] != null)
            {
                sCHName = _fields[sID].ToString();
            }

            if (sCHName != "")
            {
                //资产属性字段值 类对象
                BR_Schema_Deploy deploy = null;
                if (_xmlHt[sID] != null)
                {
                    //获得控件的值
                    deploy = (BR_Schema_Deploy)(_xmlHt[sID]);
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
                ctdl.ID = "tDynamictxt" + sID;
                ctdl.Attributes.Add("Tag", sID);// 存id 的值
                ctdl.Attributes.Add("sFieldName", sFieldName);//存名字
                ctdl.Attributes.Add("RelateID", ViewState[this.ID + "RelateID"].ToString());//存关联ID
                ctdl.Attributes.Add("RelateType", ViewState[this.ID + "RelateType"].ToString());//存关联类型
                if (deploy != null)
                {
                    ctdl.Attributes.Add("deployID", deploy.ID.ToString());//对象ID编号值
                }
                else
                {
                    ctdl.Attributes.Add("deployID", "0");//对象ID编号值
                }

                //赋值
                ctdl.UserID = CTools.ToInt64(sSetValue == "" ? "0" : sSetValue);

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
                AddControl(tr, tc);
            }
        }
        #endregion

        #region 部门控件
        /// <summary>
        /// 部门控件
        /// </summary>
        /// <param name="tr"></param>
        /// <param name="rowcount"></param>
        /// <param name="n"></param>
        /// <param name="iLab"></param>
        /// <param name="width"></param>
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
            long RootID = CTools.ToInt64(n.Attributes["Default"].Value == "" ? "0" : n.Attributes["Default"].Value);

            if (_fields[sID] != null)
            {
                sCHName = _fields[sID].ToString();
            }

            if (sCHName != "")
            {
                //资产属性字段值 类对象
                BR_Schema_Deploy deploy = null;
                if (_xmlHt[sID] != null)
                {
                    //获得控件的值
                    deploy = (BR_Schema_Deploy)(_xmlHt[sID]);
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
                ctdl.ID = "tDynamictxt" + sID;
                ctdl.Attributes.Add("Tag", sID);// 存id 的值
                ctdl.Attributes.Add("sFieldName", sFieldName);//存名字
                ctdl.Attributes.Add("RelateID", ViewState[this.ID + "RelateID"].ToString());//存关联ID
                ctdl.Attributes.Add("RelateType", ViewState[this.ID + "RelateType"].ToString());//存关联类型
                if (deploy != null)
                {
                    ctdl.Attributes.Add("deployID", deploy.ID.ToString());//对象ID编号值
                }
                else
                {
                    ctdl.Attributes.Add("deployID", "0");//对象ID编号值
                }
                //给 deptId赋值
                ctdl.DeptID = CTools.ToInt64(sSetValue == "" ? "0" : sSetValue);

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
                AddControl(tr, tc);
            }
        }
        #endregion

        #region 文本输入控件
        /// <summary>
        /// 添加文本输入控件
        /// </summary>
        /// <param name="tr"></param>
        /// <param name="rowcount"></param>
        /// <param name="n"></param>
        /// <param name="iLab"></param>
        /// <param name="width"></param>
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
                BR_Schema_Deploy deploy = null;
                if (_xmlHt[sID] != null)
                {
                    //获得控件的值
                    deploy = (BR_Schema_Deploy)(_xmlHt[sID]);
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
                ctdl.Attributes.Add("RelateID", ViewState[this.ID + "RelateID"].ToString());//存关联ID
                ctdl.Attributes.Add("RelateType", ViewState[this.ID + "RelateType"].ToString());//存关联类型
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
                    ctdl.ContralState = eOA_FlowControlState.eReadOnly;

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
                AddControl(tr, tc);
            }
        }
        #endregion

        #region 备注控件
        /// <summary>
        /// 备注控件
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
            string sFieldName = "";
            sFieldName = n.Attributes["CHName"].Value;
            string IsMust = n.Attributes["IsMust"].Value;//是否必填
            if (_fields[sID] != null)
            {
                sCHName = _fields[sID].ToString();
            }

            if (sCHName != "")
            {

                BR_Schema_Deploy deploy = null;
                if (_xmlHt[sID] != null)
                {
                    //获得控件的值
                    deploy = (BR_Schema_Deploy)(_xmlHt[sID]);
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
                ctdl.Attributes.Add("RelateID", ViewState[this.ID + "RelateID"].ToString());//存关联ID
                ctdl.Attributes.Add("RelateType", ViewState[this.ID + "RelateType"].ToString());//存关联类型
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
                    ctdl.ContralState = eOA_FlowControlState.eReadOnly;
                }

                AddControl(tc, labTemp);
                AddControl(tr, tc);
                rowcount++;
                tc = new HtmlTableCell();
                tc.ID = tr.ID + "_Cell" + rowcount.ToString();
                tc.Attributes.Add("class", "list");
                tc.Attributes.Add("style", "word-break:break-all;");
                AddControl(tc, ctdl);
                AddControl(tr, tc);
            }
        }

        #endregion

        #region 添加复选控件值
        /// <summary>
        /// 添加复选控件值
        /// </summary>
        /// <param name="tr"></param>
        /// <param name="rowcount"></param>
        /// <param name="n"></param>
        /// <param name="iLab"></param>
        /// <param name="width"></param>
        private void AddCheckBoxRow(ref HtmlTableRow tr, ref int rowcount, XmlNode n, ref int iLab, int width)
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
            BR_CatalogSchemaItemsDP ee = new BR_CatalogSchemaItemsDP();
            ee = ee.GetReCorded(sID);

            long RootID = CTools.ToInt64(ee.CatalogID.ToString() == "" ? "1" : ee.CatalogID.ToString());

            if (_fields[sID] != null)
            {
                sCHName = _fields[sID].ToString();
            }

            if (sCHName != "")
            {
                //资产属性字段值 类对象
                BR_Schema_Deploy deploy = null;
                if (_xmlHt[sID] != null)
                {
                    //获得控件的值
                    deploy = (BR_Schema_Deploy)(_xmlHt[sID]);
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


                ctrFlowCataCheckBox ctdl = (Epower.ITSM.Web.Controls.ctrFlowCataCheckBox)LoadControl("~/controls/ctrFlowCataCheckBox.ascx");

                ctdl.ID = "tDynamictxt" + sID;
                ctdl.Attributes.Add("Tag", sID);// 存id 的值
                ctdl.Attributes.Add("sFieldName", sFieldName);//存名字                
                ctdl.Attributes.Add("RelateID", ViewState[this.ID + "RelateID"].ToString());//存关联ID
                ctdl.Attributes.Add("RelateType", ViewState[this.ID + "RelateType"].ToString());//存关联类型
                if (deploy != null)
                {
                    ctdl.Attributes.Add("deployID", deploy.ID.ToString());//对象ID编号值
                }
                else
                {
                    ctdl.Attributes.Add("deployID", "0");//对象ID编号值
                }
                ctdl.RootID = RootID;
                ctdl.CatelogValue = sSetValue;

                labTemp = new Label();
                labTemp.ID = tr.ID + "lab" + iLab.ToString();
                labTemp.Text = sCHName;


                if (this.ReadOnly == true)
                {
                    ctdl.ContralState = eOA_FlowControlState.eReadOnly;

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
                AddControl(tr, tc);
            }
        }
        #endregion

        #region 给父控件上加子控件
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
                if (!bFound)
                {
                    pSubControl.EnableViewState = false;
                    pParentControl.Controls.Add(pSubControl);
                }
            }
        }
        #endregion
    }
}
