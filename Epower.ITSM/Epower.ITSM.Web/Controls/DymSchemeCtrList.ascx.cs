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

        #region ���Զ�����
        /// <summary>
        /// �豸������ [ע�������÷�ֻ��ȡһ��,Ҫô���� ����ID,Ҫôֱ������ SCHEMA]
        /// </summary>
        public long EquCategoryID
        {
            set
            {
                ViewState[this.ID + "EquCategoryID"] = value;
                //�ط�ʱ��ֵ ���¼��ؽ���
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
        /// �ؼ����ص�����XMLֵ
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

        //�ؼ����ص�ֵ
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
        /// ����ʲ�id
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

        //�ʲ��бȽ�ֵʱ �� hashTable 
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
        /// ���б��ʱ�������������
        /// </summary>
        public long FlowID
        {
            set
            {
                hasCompare = true;   //ֻҪ�й���ֵ ��ʾ��Ҫ�Ƚ�
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
        /// �Ƿ����ʷ���бȽ� ��ֻ�����ʲ�����ͼʱ�õ������ԣ�
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
        /// ���Ǳ������£���ʷ�汾��֮��ıȽ�
        /// </summary>
        public long Version
        {
            set
            {
                hasCompare = true;

                _xmlHt = SetHashTableVersionXmlValue(long.Parse(ViewState["EquID"].ToString()), value);
                ViewState[this.ID + "xmlHT"] = _xmlHt;

                ViewState[this.ID + "Version"] = value;

                ///�ʲ�����ͼʱ���õ��Ƚ�
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
        /// �Ƿ����ڱ����
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
        XmlNodeList DropDownListNodes = null;//�����ؼ�

        XmlNodeList DeptNodes = null;//������Ϣ�ؼ�
        XmlNodeList UserNodes = null;//�û���Ϣ�ؼ�
        XmlNodeList TimeNodes = null;//�������Ϳؼ�
        XmlNodeList NumberNodes = null;//��ֵ���Ϳؼ�

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
                //ȡ��ʷ�汾��strXml
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
                //ȡ��ʷֵ�����
                _fields = ee.GetAllFieldsHistory();
            }
            else
            {
                _fields = ee.GetAllFields();   //��ȡ���µ����������
            }
            //�ط�����£����ǰ�ȱ���һ��֮ǰ�ؼ���ֵ
            //if (Page.IsPostBack == true)
            //{
            //    Hashtable ht = new Hashtable();
            //    ValueCollection(this, ref ht);
            //    _xmlHt = ht;
            //}
            //����������пؼ�
            this.Controls.Clear();
            if (strXml != "")
            {
                //�������������Ҫ����ע��ű�
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
        /// �Ƿ�ǰ�ڱ༭״̬�¸����˷��ࡣ [��������£�����û�д���ԭ��ȡֵ������£���ȡ���õ�ȱʡ����] ����Ҫviewstate ����
        /// </summary>
        public void SetChangeCatalogTrue()
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

        #region xml��hashtable֮��ת������  --yanghw 2011-08-10


        //��Hashtableת���� List<EQU_deploy> �����Ա���ʹ��
        private List<EQU_deploy> GetHashTableRtnValue(Hashtable ht)
        {
            List<EQU_deploy> list = new List<EQU_deploy>();  //ʵ�����󼯺�       
            IDictionaryEnumerator myEnumerator = ht.GetEnumerator();
            while (myEnumerator.MoveNext())
            {
                EQU_deploy Entity = (EQU_deploy)(myEnumerator.Value);//������ֵ
                list.Add(Entity);
            }
            return list;         //����list ���ݼ���
        }

        /// <summary>
        /// ����ʲ��� ֵ������ �ʲ����Եļ��� ת���� Hashtable ���󣬱���xml����
        /// </summary>
        /// <param name="EquID"></param>
        /// <returns></returns>
        private Hashtable SetHashTableXmlValue(long EquID)
        {
            Hashtable ht = new Hashtable();
            string strTmp = "";
            try
            {
                //ȡ�����ֵ �Ѷ���ֵ���ص� Hashtable ��
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
        /// �����ʷ����ʲ���ֵ������ �ʲ����Եļ��� ת������ʷ�ʲ���Hashtable ���󣬱���xml����(���ڱ����)
        /// </summary>
        /// <param name="EquID"></param>
        /// <returns></returns>
        private Hashtable SetHashTablehostoryXmlValue(long EquID, long FlowId)
        {
            Hashtable ht = new Hashtable();
            string strTmp = "";
            try
            {
                //ȡ�����ֵ �Ѷ���ֵ���ص� Hashtable ��
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
        /// ��û�б������£���ʷ�汾�ıȽϣ����ڹ���ͼ��
        /// </summary>
        /// <param name="EquID">�ʲ�id</param>
        /// <param name="lngVersion">�汾��</param>
        /// <returns></returns>
        private Hashtable SetHashTableVersionXmlValue(long EquID, long lngVersion)
        {
            Hashtable ht = new Hashtable();
            string strTmp = "";
            try
            {
                //ȡ�����ֵ �Ѷ���ֵ���ص� Hashtable ��
                List<EQU_deploy> deployList = null;
                if (Equ_DeskDP.isversion(lngVersion, EquID))
                {
                    //��ǰ�汾Ϊ���°汾ʱ�� ������
                    deployList = EQU_deploy.getEQU_deployList(EquID);
                }
                else
                {
                    //��ǰ�汾 �������°汾ʱ�Ĵ��봦��
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

        #region SetChangeHashTableXmlValue  ȡ�����ʱ����Ϣ

        /// <summary>
        /// ����ʲ��� ֵ������ �ʲ����Եļ��� ת���� Hashtable ���󣬱���xml����
        /// </summary>
        /// <param name="EquID"></param>
        /// <returns></returns>
        private Hashtable SetChangeHashTableXmlValue(long FlowId, long EquID)
        {
            Hashtable ht = new Hashtable();
            string strTmp = "";
            try
            {
                //ȡ�����ֵ �Ѷ���ֵ���ص� Hashtable ��

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
                    //���ڱ��ʱ����δ�����ʱ��ȡԭ�ʲ��������ֵ
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
        /// �ı䲿�ţ��ı��û�
        /// </summary>
        /// <param name="detp"></param>
        private void setUserConter(object sender, EventArgs e)
        {

            setUserDeptid(this, ((DeptPicker)sender));

        }
        /// <summary>
        /// ������ֵ��������ʱ �ؼ�ֵ���°�
        /// </summary>
        /// <param name="ctrRoot"></param>
        /// <param name="vardept"></param>
        private void setUserDeptid(Control ctrRoot, DeptPicker vardept)
        {
            foreach (Control pControl in ctrRoot.Controls)  /*���������ӽڵ�*/
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
                    case "controls_userpicker_ascx"://�û���Ϣ�ؼ�
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
                                //�ѿؼ��е�ֵ���� ����EQU_deploy
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
                        case "controls_ctrflowcatadroplist_ascx"://�����ؼ�
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
                        case "controls_deptpicker_ascx"://���ſؼ�
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
                        case "controls_userpicker_ascx"://�û���Ϣ�ؼ�
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
                        case "controls_ctrdateandtime_ascx"://ʱ��ؼ�
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
                        case "controls_ctrflownumeric_ascx"://�û���Ϣ�ؼ�
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


        #region ��ӿؼ� ˽�з���
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



            //��ӱ���
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
                    lab1.Text = "������Ϣ";
                }

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
                        //��ÿؼ���ֵ
                        deploy = (EQU_deploy)(_xmlHt[sID]);
                        sSetValue = deploy.Value;
                    }
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


                    chkTemp.Attributes.Add("sFieldName", sFieldName);//������
                    chkTemp.Attributes.Add("EquID", ViewState["EquID"].ToString());//���ʲ�id
                    if (deploy != null)
                    {
                        chkTemp.Attributes.Add("deployID", deploy.ID.ToString());//����ID���ֵ
                    }
                    else
                    {
                        chkTemp.Attributes.Add("deployID", "0");//����ID���ֵ
                    }

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
                                EQU_deploy deployList = (EQU_deploy)(_xmlHtComp[sID]);
                                //������ʷ�Ƚ�ֵ������£������ͬ�����ɫ��ʾ
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

            //��ӱ���
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



                foreach (XmlNode n in ns)//������
                {
                    string sID = "";
                    string sSetValue = "";   //���õ�ֵ
                    string sSetValueComp = ""; //���ڱȽϵ�ֵ
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
                            //��ÿؼ���ֵ
                            deploy = (EQU_deploy)(_xmlHt[sID]);
                            sSetValue = deploy.Value;
                        }

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

                        ctdl.Attributes.Add("sFieldName", sFieldName);//������
                        ctdl.Attributes.Add("EquID", ViewState["EquID"].ToString());//���ʲ�id
                        if (deploy != null)
                        {
                            ctdl.Attributes.Add("deployID", deploy.ID.ToString());//����ID���ֵ
                        }
                        else
                        {
                            ctdl.Attributes.Add("deployID", "0");//����ID���ֵ
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


                            //�Ҳ���ͬʱҲ��Ҫ�Ƚϵ����[�¼ӵ����õ����]
                            if (hasCompare == true)
                            {
                                if (_xmlHtComp[sID] != null)
                                {
                                    EQU_deploy deployList = (EQU_deploy)(_xmlHtComp[sID]);
                                    //������ʷ�Ƚ�ֵ������£������ͬ�����ɫ��ʾ
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
            foreach (XmlNode node in bnodes)//����
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
            //ѭ���ݹ�
            tabMain.ID = "RemarkTab" + iRel.ToString();
            tabMain.Attributes.Add("width", "100%");
            tabMain.Attributes.Add("align", "center");
            tabMain.Attributes.Add("class", "listNewContent");

            //��ӱ���
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
                    lab1.Text = "��ע��Ϣ";
                }
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

        #region ��������ؼ�ֵ  --yanghw  2011-08-10
        //�˿ؼ��޸�
        private void AddDropDownListRow(ref HtmlTableRow tr, ref int rowcount, XmlNode n, ref int iLab, int width)
        {
            HtmlTableCell tc = null;
            tc = new HtmlTableCell();
            tc.ID = tr.ID + "_Cell" + rowcount.ToString();
            tc.Attributes.Add("class", "listTitleRight");
            tc.Attributes.Add("style", "word-break:break-all;width:12%;");
            string sID = "";
            string sSetValue = "";   //���õ�ֵ
            string sSetValueComp = ""; //���ڱȽϵ�ֵ
            string sCHName = "";
            sID = n.Attributes["ID"].Value;
            string sFieldName = n.Attributes["CHName"].Value;//��ʾ������
            string IsMust = n.Attributes["IsMust"].Value;//�Ƿ����
            //��ֵ

            //�����������ȡ��Ӧ������rootID
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
                //�ʲ������ֶ�ֵ �����
                EQU_deploy deploy = null;
                if (_xmlHt[sID] != null)
                {
                    //��ÿؼ���ֵ
                    deploy = (EQU_deploy)(_xmlHt[sID]);
                    sSetValue = deploy.Value;
                }
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


                ctrFlowCataDropList ctdl = (Epower.ITSM.Web.Controls.ctrFlowCataDropList)LoadControl("~/controls/ctrFlowCataDropList.ascx");
                //ctdl.FieldsSourceType = 1;   //�첽��ȡ��ʽ
                //ctdl.TextMode = TextBoxMode.MultiLine;
                //ctdl.FieldsSourceID = "SchemaItem_" + sID;
                ctdl.ID = "tDynamictxt" + sID;
                ctdl.Attributes.Add("Tag", sID);// ��id ��ֵ
                ctdl.Attributes.Add("sFieldName", sFieldName);//������
                ctdl.Attributes.Add("EquID", ViewState["EquID"].ToString());//���ʲ�id
                if (deploy != null)
                {
                    ctdl.Attributes.Add("deployID", deploy.ID.ToString());//����ID���ֵ
                }
                else
                {
                    ctdl.Attributes.Add("deployID", "0");//����ID���ֵ
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
                //�Ҳ���ͬʱҲ��Ҫ�Ƚϵ����[�¼ӵ����õ����]
                if (hasCompare == true)
                {
                    if (_xmlHtComp[sID] != null)
                    {
                        //������ʷ�Ƚ�ֵ������£������ͬ�����ɫ��ʾ
                        //sSetValueComp = _xmlHtComp[sID].ToString();
                        EQU_deploy deployList = (EQU_deploy)(_xmlHtComp[sID]);
                        //������ʷ�Ƚ�ֵ������£������ͬ�����ɫ��ʾ
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
                    tc.Attributes.Add("title", "�鿴�������" + sSetValue.Trim() + "����ͬ���ʲ��б�");



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

        #region ��ֵ���Ϳؼ�  --yanghw  2011-08-11

        //��ֵ����
        private void AddNumberRow(ref HtmlTableRow tr, ref int rowcount, XmlNode n, ref int iLab, int width)
        {
            HtmlTableCell tc = null;
            tc = new HtmlTableCell();
            tc.ID = tr.ID + "_Cell" + rowcount.ToString();
            tc.Attributes.Add("class", "listTitleRight");
            tc.Attributes.Add("style", "word-break:break-all;width:12%;");
            string sID = "";
            string sSetValue = "";   //���õ�ֵ
            string sSetValueComp = ""; //���ڱȽϵ�ֵ
            string sCHName = "";
            sID = n.Attributes["ID"].Value;
            string sFieldName = n.Attributes["CHName"].Value;//��ʾ������
            string IsMust = n.Attributes["IsMust"].Value;//�Ƿ����
            //��ֵ
            long RootID = long.Parse(n.Attributes["Default"].Value == "" ? "0" : n.Attributes["Default"].Value);

            if (_fields[sID] != null)
            {
                sCHName = _fields[sID].ToString();
            }

            if (sCHName != "")
            {
                //�ʲ������ֶ�ֵ �����
                EQU_deploy deploy = null;
                if (_xmlHt[sID] != null)
                {
                    //��ÿؼ���ֵ
                    deploy = (EQU_deploy)(_xmlHt[sID]);
                    sSetValue = deploy.Value;
                }
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


                CtrFlowNumeric ctdl = (Epower.ITSM.Web.Controls.CtrFlowNumeric)LoadControl("~/controls/CtrFlowNumeric.ascx");
                //ctdl.FieldsSourceType = 1;   //�첽��ȡ��ʽ
                //ctdl.TextMode = TextBoxMode.MultiLine;
                //ctdl.FieldsSourceID = "SchemaItem_" + sID;
                ctdl.ID = "tDynamictxt" + sID;
                ctdl.Attributes.Add("Tag", sID);// ��id ��ֵ
                ctdl.Attributes.Add("sFieldName", sFieldName);//������
                ctdl.Attributes.Add("EquID", ViewState["EquID"].ToString());//���ʲ�id
                if (deploy != null)
                {
                    ctdl.Attributes.Add("deployID", deploy.ID.ToString());//����ID���ֵ
                }
                else
                {
                    ctdl.Attributes.Add("deployID", "0");//����ID���ֵ
                }

                //   ctdl.CatelogID = long.Parse(sSetValue == "" ? "0" : sSetValue);


                //�� deptId��ֵ
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
                //�Ҳ���ͬʱҲ��Ҫ�Ƚϵ����[�¼ӵ����õ����]
                if (hasCompare == true)
                {
                    if (_xmlHtComp[sID] != null)
                    {
                        //������ʷ�Ƚ�ֵ������£������ͬ�����ɫ��ʾ
                        EQU_deploy deployList = (EQU_deploy)(_xmlHtComp[sID]);
                        //������ʷ�Ƚ�ֵ������£������ͬ�����ɫ��ʾ
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
                    tc.Attributes.Add("title", "�鿴�������" + sSetValue.Trim() + "����ͬ���ʲ��б�");





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

        #region ʱ�����Ϳؼ�  --yanghw  2011-08-11
        //ʱ������
        private void AddTimeRow(ref HtmlTableRow tr, ref int rowcount, XmlNode n, ref int iLab, int width)
        {
            HtmlTableCell tc = null;
            tc = new HtmlTableCell();
            tc.ID = tr.ID + "_Cell" + rowcount.ToString();
            tc.Attributes.Add("class", "listTitleRight");
            tc.Attributes.Add("style", "word-break:break-all;width:12%;");
            string sID = "";
            string sSetValue = "";   //���õ�ֵ
            string sSetValueComp = ""; //���ڱȽϵ�ֵ
            string sCHName = "";
            sID = n.Attributes["ID"].Value;
            string sFieldName = n.Attributes["CHName"].Value;//��ʾ������
            string isChack = n.Attributes["isChack"].Value;// �Ƿ�ʱ�� 0, ����; 1, ��.
            string IsMust = n.Attributes["IsMust"].Value;//�Ƿ����            
            //��ֵ
            long RootID = long.Parse(n.Attributes["Default"].Value == "" ? "0" : n.Attributes["Default"].Value);

            if (_fields[sID] != null)
            {
                sCHName = _fields[sID].ToString();
            }

            if (sCHName != "")
            {
                //�ʲ������ֶ�ֵ �����
                EQU_deploy deploy = null;
                if (_xmlHt[sID] != null)
                {
                    //��ÿؼ���ֵ
                    deploy = (EQU_deploy)(_xmlHt[sID]);
                    sSetValue = deploy.Value;

                    
                }
                if (sSetValue == "")
                {
                    if (_blnAddEqu == true || _blnChangeCatalog == true)
                    {
                        //�½� �� �༭״̬�� ���Ĺ� ����
                        if (n.Attributes["Default"].Value == "1")
                        {
                            sSetValue = System.DateTime.Now.ToShortDateString();
                        }
                    }
                }

                iLab++;
                Label labTemp;


                CtrDateAndTimeV2 ctdl = (Epower.ITSM.Web.Controls.CtrDateAndTimeV2)LoadControl("~/Controls/CtrDateAndTimeV2.ascx");
                //ctdl.FieldsSourceType = 1;   //�첽��ȡ��ʽ
                //ctdl.TextMode = TextBoxMode.MultiLine;
                //ctdl.FieldsSourceID = "SchemaItem_" + sID;
                ctdl.ID = "tDynamictxt" + sID;
                ctdl.Attributes.Add("Tag", sID);// ��id ��ֵ
                ctdl.Attributes.Add("sFieldName", sFieldName);//������
                ctdl.Attributes.Add("EquID", ViewState["EquID"].ToString());//���ʲ�id
                if (deploy != null)
                {
                    ctdl.Attributes.Add("deployID", deploy.ID.ToString());//����ID���ֵ
                }
                else
                {
                    ctdl.Attributes.Add("deployID", "0");//����ID���ֵ
                }

                //   ctdl.CatelogID = long.Parse(sSetValue == "" ? "0" : sSetValue);


                if (isChack != "1")
                {
                    ctdl.ShowTime = false;
                    ctdl.ShowMinute = false;
                }

                //�� deptId��ֵ
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
                //�Ҳ���ͬʱҲ��Ҫ�Ƚϵ����[�¼ӵ����õ����]
                if (hasCompare == true)
                {
                    if (_xmlHtComp[sID] != null)
                    {
                        //������ʷ�Ƚ�ֵ������£������ͬ�����ɫ��ʾ
                        EQU_deploy deployList = (EQU_deploy)(_xmlHtComp[sID]);
                        //������ʷ�Ƚ�ֵ������£������ͬ�����ɫ��ʾ
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
                    tc.Attributes.Add("title", "�鿴�������" + sSetValue.Trim() + "����ͬ���ʲ��б�");


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


        #region �û���Ϣ�ؼ�  --yanghw  2011-08-10
        //���ſؼ�
        private void AddUserRow(ref HtmlTableRow tr, ref int rowcount, XmlNode n, ref int iLab, int width)
        {
            HtmlTableCell tc = null;
            tc = new HtmlTableCell();
            tc.ID = tr.ID + "_Cell" + rowcount.ToString();
            tc.Attributes.Add("class", "listTitleRight");
            tc.Attributes.Add("style", "word-break:break-all;width:12%;");
            string sID = "";
            string sSetValue = "";   //���õ�ֵ
            string sSetValueComp = ""; //���ڱȽϵ�ֵ
            string sCHName = "";
            sID = n.Attributes["ID"].Value;
            string sFieldName = n.Attributes["CHName"].Value;//��ʾ������
            string IsMust = n.Attributes["IsMust"].Value;//�Ƿ����
            //��ֵ
            long RootID = long.Parse(n.Attributes["Default"].Value == "" ? "0" : n.Attributes["Default"].Value);

            if (_fields[sID] != null)
            {
                sCHName = _fields[sID].ToString();
            }

            if (sCHName != "")
            {
                //�ʲ������ֶ�ֵ �����
                EQU_deploy deploy = null;
                if (_xmlHt[sID] != null)
                {
                    //��ÿؼ���ֵ
                    deploy = (EQU_deploy)(_xmlHt[sID]);
                    sSetValue = deploy.Value;
                }
                if (sSetValue == "")
                {
                    if (_blnAddEqu == true || _blnChangeCatalog == true)
                    {
                        //�½� �� �༭״̬�� ���Ĺ� ����
                        if (n.Attributes["Default"].Value == "1")
                        {
                            sSetValue = Session["UserID"].ToString();
                        }
                    }
                }

                iLab++;
                Label labTemp;


                UserPicker ctdl = (Epower.ITSM.Web.Controls.UserPicker)LoadControl("~/controls/UserPicker.ascx");
                //ctdl.FieldsSourceType = 1;   //�첽��ȡ��ʽ
                //ctdl.TextMode = TextBoxMode.MultiLine;
                //ctdl.FieldsSourceID = "SchemaItem_" + sID;
                ctdl.ID = "tDynamictxt" + sID;
                ctdl.Attributes.Add("Tag", sID);// ��id ��ֵ
                ctdl.Attributes.Add("sFieldName", sFieldName);//������
                ctdl.Attributes.Add("EquID", ViewState["EquID"].ToString());//���ʲ�id
                if (deploy != null)
                {
                    ctdl.Attributes.Add("deployID", deploy.ID.ToString());//����ID���ֵ
                }
                else
                {
                    ctdl.Attributes.Add("deployID", "0");//����ID���ֵ
                }

                //   ctdl.CatelogID = long.Parse(sSetValue == "" ? "0" : sSetValue);


                //�� deptId��ֵ
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
                //�Ҳ���ͬʱҲ��Ҫ�Ƚϵ����[�¼ӵ����õ����]
                if (hasCompare == true)
                {
                    if (_xmlHtComp[sID] != null)
                    {
                        //������ʷ�Ƚ�ֵ������£������ͬ�����ɫ��ʾ
                        EQU_deploy deployList = (EQU_deploy)(_xmlHtComp[sID]);
                        //������ʷ�Ƚ�ֵ������£������ͬ�����ɫ��ʾ
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
                    tc.Attributes.Add("title", "�鿴�������" + sSetValue.Trim() + "����ͬ���ʲ��б�");


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

        #region ���ſؼ�  --yanghw  2011-08-10
        //���ſؼ�
        private void AddDetpRow(ref HtmlTableRow tr, ref int rowcount, XmlNode n, ref int iLab, int width)
        {
            HtmlTableCell tc = null;
            tc = new HtmlTableCell();
            tc.ID = tr.ID + "_Cell" + rowcount.ToString();
            tc.Attributes.Add("class", "listTitleRight");
            tc.Attributes.Add("style", "word-break:break-all;width:12%;");
            string sID = "";
            string sSetValue = "";   //���õ�ֵ
            string sSetValueComp = ""; //���ڱȽϵ�ֵ
            string sCHName = "";
            sID = n.Attributes["ID"].Value;
            string sFieldName = n.Attributes["CHName"].Value;//��ʾ������
            string IsMust = n.Attributes["IsMust"].Value;//�Ƿ����
            //��ֵ
            long RootID = long.Parse(n.Attributes["Default"].Value == "" ? "0" : n.Attributes["Default"].Value);

            if (_fields[sID] != null)
            {
                sCHName = _fields[sID].ToString();
            }

            if (sCHName != "")
            {
                //�ʲ������ֶ�ֵ �����
                EQU_deploy deploy = null;
                if (_xmlHt[sID] != null)
                {
                    //��ÿؼ���ֵ
                    deploy = (EQU_deploy)(_xmlHt[sID]);
                    sSetValue = deploy.Value;
                }
                if (sSetValue == "")
                {
                    if (_blnAddEqu == true || _blnChangeCatalog == true)
                    {
                        //�½� �� �༭״̬�� ���Ĺ� ����
                        if (n.Attributes["Default"].Value == "1")
                        {
                            sSetValue = Session["UserDeptID"].ToString();
                        }
                    }
                }

                iLab++;
                Label labTemp;


                DeptPicker ctdl = (Epower.ITSM.Web.Controls.DeptPicker)LoadControl("~/controls/DeptPicker.ascx");
                //ctdl.FieldsSourceType = 1;   //�첽��ȡ��ʽ
                //ctdl.TextMode = TextBoxMode.MultiLine;
                //ctdl.FieldsSourceID = "SchemaItem_" + sID;
                ctdl.ID = "tDynamictxt" + sID;
                ctdl.Attributes.Add("Tag", sID);// ��id ��ֵ
                ctdl.Attributes.Add("sFieldName", sFieldName);//������
                ctdl.Attributes.Add("EquID", ViewState["EquID"].ToString());//���ʲ�id
                if (deploy != null)
                {
                    ctdl.Attributes.Add("deployID", deploy.ID.ToString());//����ID���ֵ
                }
                else
                {
                    ctdl.Attributes.Add("deployID", "0");//����ID���ֵ
                }
                //�� deptId��ֵ
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

                //�Ҳ���ͬʱҲ��Ҫ�Ƚϵ����[�¼ӵ����õ����]
                if (hasCompare == true)
                {
                    if (_xmlHtComp[sID] != null)
                    {
                        //������ʷ�Ƚ�ֵ������£������ͬ�����ɫ��ʾ
                        EQU_deploy deployList = (EQU_deploy)(_xmlHtComp[sID]);
                        //������ʷ�Ƚ�ֵ������£������ͬ�����ɫ��ʾ
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
                    tc.Attributes.Add("title", "�鿴�������" + sSetValue.Trim() + "����ͬ���ʲ��б�");
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



            //��ӱ���
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
                    lab1.Text = "������Ϣ";
                }



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
                else if (n.Attributes["CtrlType"].Value == "DropDownList")// �����ؼ�
                {
                    AddDropDownListRow(ref tr, ref rowcount, n, ref iLab, width);
                }
                else if (n.Attributes["CtrlType"].Value == "deptList")// ����
                {
                    AddDetpRow(ref tr, ref rowcount, n, ref iLab, width);
                }
                else if (n.Attributes["CtrlType"].Value == "UserList")// �û�
                {
                    AddUserRow(ref tr, ref rowcount, n, ref iLab, width);
                }
                else if (n.Attributes["CtrlType"].Value == "Time")// ����
                {
                    AddTimeRow(ref tr, ref rowcount, n, ref iLab, width);
                }
                else if (n.Attributes["CtrlType"].Value == "Number")// ��ֵ
                {
                    AddNumberRow(ref tr, ref rowcount, n, ref iLab, width);
                }
                AddControl(tabMain, tr);
            }
            AddControl(this, tabMain);
        }

        //�˿ؼ��޸�
        private void AddBaseRow(ref HtmlTableRow tr, ref int rowcount, XmlNode n, ref int iLab, int width)
        {
            HtmlTableCell tc = null;
            tc = new HtmlTableCell();
            tc.ID = tr.ID + "_Cell" + rowcount.ToString();
            tc.Attributes.Add("class", "listTitleRight");
            tc.Attributes.Add("style", "word-break:break-all;width:12%;");
            string sID = "";
            string sSetValue = "";   //���õ�ֵ
            string sSetValueComp = ""; //���ڱȽϵ�ֵ
            string sCHName = "";
            // string sFieldName = "";
            sID = n.Attributes["ID"].Value;
            string sFieldName = "";
            sFieldName = n.Attributes["CHName"].Value;
            string IsMust = n.Attributes["IsMust"].Value;//�Ƿ����
            if (_fields[sID] != null)
            {
                sCHName = _fields[sID].ToString();
            }

            if (sCHName != "")
            {
                //�ʲ������ֶ�ֵ �����
                EQU_deploy deploy = null;
                if (_xmlHt[sID] != null)
                {
                    //��ÿؼ���ֵ
                    deploy = (EQU_deploy)(_xmlHt[sID]);
                    sSetValue = deploy.Value;
                }
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
                ctdl.Attributes.Add("Tag", sID);// ��id ��ֵ
                ctdl.Attributes.Add("sFieldName", sFieldName);//������
                ctdl.Attributes.Add("EquID", ViewState["EquID"].ToString());//���ʲ�id
                if (deploy != null)
                {
                    ctdl.Attributes.Add("deployID", deploy.ID.ToString());//����ID���ֵ
                }
                else
                {
                    ctdl.Attributes.Add("deployID", "0");//����ID���ֵ
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

                //�Ҳ���ͬʱҲ��Ҫ�Ƚϵ����[�¼ӵ����õ����]
                if (hasCompare == true)
                {
                    if (_xmlHtComp[sID] != null)
                    {
                        //������ʷ�Ƚ�ֵ������£������ͬ�����ɫ��ʾ
                        EQU_deploy deployList = (EQU_deploy)(_xmlHtComp[sID]);
                        //������ʷ�Ƚ�ֵ������£������ͬ�����ɫ��ʾ
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
                    tc.Attributes.Add("title", "�鿴�������" + sSetValue.Trim() + "����ͬ���ʲ��б�");

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
            string sSetValue = "";   //���õ�ֵ
            string sSetValueComp = ""; //���ڱȽϵ�ֵ
            string sCHName = "";

            sID = n.Attributes["ID"].Value;
            string sFieldName = "";
            sFieldName = n.Attributes["CHName"].Value;
            string IsMust = n.Attributes["IsMust"].Value;//�Ƿ����
            if (_fields[sID] != null)
            {
                sCHName = _fields[sID].ToString();
            }

            if (sCHName != "")
            {

                EQU_deploy deploy = null;
                if (_xmlHt[sID] != null)
                {
                    //��ÿؼ���ֵ
                    deploy = (EQU_deploy)(_xmlHt[sID]);
                    sSetValue = deploy.Value;
                }

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
                ctdl.Attributes.Add("Tag", sID);// ��id ��ֵ
                ctdl.Attributes.Add("sFieldName", sFieldName);//������
                ctdl.Attributes.Add("EquID", ViewState["EquID"].ToString());//���ʲ�id
                if (deploy != null)
                {
                    ctdl.Attributes.Add("deployID", deploy.ID.ToString());//����ID���ֵ
                }
                else
                {
                    ctdl.Attributes.Add("deployID", "0");//����ID���ֵ
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
                //�Ҳ���ͬʱҲ��Ҫ�Ƚϵ����[�¼ӵ����õ����]
                if (hasCompare == true)
                {
                    if (_xmlHtComp[sID] != null)
                    {
                        EQU_deploy deployList = (EQU_deploy)(_xmlHtComp[sID]);
                        //������ʷ�Ƚ�ֵ������£������ͬ�����ɫ��ʾ
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
                    tc.Attributes.Add("title", "�鿴�������" + sSetValue.Trim() + "����ͬ���ʲ��б�");
                    tc.Attributes.Add("style", "CURSOR: hand;");
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
