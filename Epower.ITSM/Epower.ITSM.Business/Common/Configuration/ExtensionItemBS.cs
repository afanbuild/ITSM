/*******************************************************************
 * 版权所有：深圳市非凡信息技术有限公司
 * 描述：流程自定义扩展项配置-业务逻辑代码
 * 
 * 
 * 创建人：孙绍棕
 * 创建日期：2013-12-02
 * *****************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Xml;
using Epower.ITSM.SqlDAL.Customer;
using System.Data.OracleClient;
using Epower.ITSM.SqlDAL;
using Epower.DevBase.Organization.Base;
using Epower.DevBase.BaseTools;
using System.Web.UI.WebControls;

namespace Epower.ITSM.Business.Common.Configuration
{
    /// <summary>
    /// 流程自定义扩展项
    /// </summary>
    public class ExtensionItemBS
    {

        /// <summary>
        /// 取拥有扩展项的流程模型列表
        /// </summary>
        /// <param name="lngAppID">应用编号</param>
        /// <param name="lngFlowModelID">流程模型编号</param>
        /// <param name="strSearchKey">查询字符串</param>
        /// <returns></returns>
        public DataTable GetExtFlowModelList(long lngAppID, long lngFlowModelID, String strSearchKey)
        {
            StringBuilder sbSql = new StringBuilder();
            if (lngAppID > 0)
                sbSql.AppendFormat(" And AppID = {0} ", lngAppID);
            if (lngFlowModelID > 0)
                sbSql.AppendFormat(" And FlowModelID = {0} ", lngFlowModelID);

            if (!String.IsNullOrEmpty(strSearchKey))
                sbSql.AppendFormat(" And ( appname like {0} or flowmodelname like {0} )", StringTool.SqlQ("%" + strSearchKey + "%"));

            Br_ExtensionsItemsDP extensionItemDP = new Br_ExtensionsItemsDP();
            DataTable dtExFlowModelList = extensionItemDP.GetExtFlowModelList(sbSql.ToString());

            return dtExFlowModelList;
        }

        /// <summary>
        /// 取指定流程模型的扩展项
        /// </summary>
        /// <param name="lngFlowModelID">流程模型编号</param>
        /// <returns></returns>
        public DataTable GetExItemListByFlowModelID(long lngFlowModelID)
        {
            OracleConnection conn = null;
            try
            {
                DataTable dtExItemList = ExtensionItemBS.CreateNullTable();

                if (lngFlowModelID <= -1)
                    return dtExItemList;


                conn = ConfigTool.GetConnection();

                BR_ExtendsFieldsDP ee = new BR_ExtendsFieldsDP();
                DataTable dt = ee.GetDataTable(String.Format(" AND GroupID = {0} ", lngFlowModelID),
                    "  ");


                foreach (DataRow item in dt.Rows)
                {
                    String strXML = item["keyvalue"].ToString();
                    DataTable dtExItem = ConvertXmlStrToDataTable(strXML);

                    foreach (DataRow exItem in dtExItem.Rows)
                    {
                        DataRow drNew = dtExItemList.NewRow();
                        drNew["ID"] = exItem["ID"];
                        drNew["CHName"] = exItem["CHName"];
                        drNew["Default"] = exItem["Default"];
                        drNew["IsMust"] = exItem["IsMust"];
                        drNew["Group"] = exItem["Group"];
                        drNew["TypeName"] = exItem["TypeName"];
                        drNew["OrderBy"] = exItem["OrderBy"];
                        drNew["isChack"] = exItem["isChack"];
                        drNew["IsSelect"] = exItem["IsSelect"];
                        drNew["GroupID"] = exItem["GroupID"];
                        drNew["GroupName"] = exItem["GroupName"];

                        dtExItemList.Rows.Add(drNew);
                    }

                }

                return dtExItemList;
            }
            finally
            {
                if (conn != null)
                    ConfigTool.CloseConnection(conn);
            }
        }

        /// <summary>
        /// 保存扩展项
        /// </summary>
        /// <param name="lngAppID">应用编号</param>
        /// <param name="lngFlowModelID">流程模型编号</param>
        /// <param name="dtExItemList">流程扩展项列表</param>
        /// <returns></returns>
        public bool SaveExItems(long lngAppID, long lngFlowModelID, DataTable dtExItemList)
        {
            string strXml = "";
            //strXml = GetSchemaXml(dt);
            string strID = "";

            bool saved = true, save_fail = false;
            OracleConnection conn = null;
            OracleTransaction trans = null;
            try
            {
                conn = ConfigTool.GetConnection();
                conn.Open();

                trans = conn.BeginTransaction();

                Br_ExtensionsItemsDP extensionsItemsDP = new Br_ExtensionsItemsDP();
                extensionsItemsDP.DeleteExItemListByFlowModelID(trans, lngFlowModelID);    // 移除 Br_ExtensionsItems 的扩展项列表

                BR_ExtendsFieldsDP ee = new BR_ExtendsFieldsDP();
                ee.DeleteExItemListByFlowModelID(trans, lngFlowModelID);    // 移除 Br_ExtendsFields 的扩展项列表                

                for (int index = 0; index < dtExItemList.Rows.Count; index++)
                {
                    DataRow dr = dtExItemList.Rows[index];

                    //int fieldID = int.Parse(Br_ExtensionsItemsDP.getMaxFiledID().ToString());
                    long lngFieldID = EpowerGlobal.EPGlobal.GetNextID("br_ex_fieldid");

                    DataTable dt = ExtensionItemBS.CreateNullTable();
                    dt.Rows.Add(dr.ItemArray);
                    dt.Rows[0]["ID"] = lngFieldID;    // 设置扩展项编号

                    dr["nfieldid"] = lngFieldID;    // 新扩展项编号

                    strXml = ConvertDataTableToXmlStr(dt);

                    ee.SaveExItemList(trans, lngAppID, lngFlowModelID, strXml);


                    Br_ExtensionsItemsDP brExItem = new Br_ExtensionsItemsDP();
                    brExItem.FieldID = lngFieldID.ToString();
                    brExItem.CHNAME = dr["CHName"].ToString();

                    Int32 intItemTypeIndex = ExtensionItemBS.ConvertItemTypeNameToIndex(dr["TypeName"].ToString());
                    brExItem.ITEMTYPE = intItemTypeIndex;

                    brExItem.GROUPID = int.Parse(lngFlowModelID.ToString());
                    brExItem.DELETED = (int)eO_Deleted.eNormal;
                    brExItem.GroupName = String.Empty;    // 废弃字段

                    extensionsItemsDP.SaveExItemList(trans, lngAppID, lngFlowModelID, brExItem);

                }


                // # 若老的扩展项信息删除后, 需同步更新扩展项显示方式表
                ExtensionDisplayWayBS extensionDisplayWayBS = new ExtensionDisplayWayBS();
                extensionDisplayWayBS.UpdateExDisplayFieldID(trans, lngFlowModelID, dtExItemList);

                UpdateExDeployFieldID(trans, lngFlowModelID, dtExItemList);


                trans.Commit();

                return saved;
            }
            catch (Exception ex)
            {
                if (trans != null)
                    trans.Rollback();

                throw ex;
            }
            finally
            {
                if (conn != null)
                    ConfigTool.CloseConnection(conn);
            }


        }


        /// <summary>
        /// 删除拥有扩展项的流程模型列表
        /// </summary>
        /// <returns></returns>
        public bool DeleteExItemFlowModelList(List<long> listFlowModelID)
        {
            bool deleted = true;
            OracleConnection conn = null;
            OracleTransaction trans = null;
            try
            {
                conn = ConfigTool.GetConnection();
                conn.Open();

                trans = conn.BeginTransaction();

                ExtensionDisplayWayBS extensionDisplayWayBS = new ExtensionDisplayWayBS();

                foreach (long lngFlowModelID in listFlowModelID)
                {
                    DeleteExItemsByFlowModelID(trans, lngFlowModelID);
                    extensionDisplayWayBS.DeleteExFlowModel(trans, lngFlowModelID);
                }

                trans.Commit();

                return deleted;
            }
            catch (Exception ex)
            {
                if (trans != null)
                    trans.Rollback();

                throw ex;
            }
            finally
            {
                if (conn != null)
                    ConfigTool.CloseConnection(conn);
            }
        }

        /// <summary>
        /// 删除指定流程模型下的扩展项
        /// </summary>
        /// <param name="trans">事务对象</param>
        /// <param name="lngFlowModelID">流程模型编号</param>        
        /// <returns></returns>
        public void DeleteExItemsByFlowModelID(OracleTransaction trans, long lngFlowModelID)
        {
            Br_ExtensionsItemsDP extensionsItemsDP = new Br_ExtensionsItemsDP();
            extensionsItemsDP.DeleteExItemListByFlowModelID(trans, lngFlowModelID);    // 移除 Br_ExtensionsItems 的扩展项列表

            BR_ExtendsFieldsDP ee = new BR_ExtendsFieldsDP();
            ee.DeleteExItemListByFlowModelID(trans, lngFlowModelID);    // 移除 Br_ExtendsFields 的扩展项列表
        }


        #region DataTable 和 XML 之间互转的方法

        /// <summary>
        /// 将DataTable转换成XML字串
        /// </summary>
        /// <param name="dtExItemList"></param>
        /// <returns></returns>
        private string ConvertDataTableToXmlStr(DataTable dtExItemList)
        {
            if (dtExItemList.Rows.Count == 0)
                return "";

            XmlDocument xmlDoc = new XmlDocument();
            // xmlDoc.LoadXml(@"<?xml version=""1.0"" encoding=""utf-8"" ?><EquScheme></EquScheme>");
            xmlDoc.LoadXml(@"<EquScheme></EquScheme>");
            XmlElement xmlEle = xmlDoc.CreateElement("temp");
            XmlElement xmlRoot = xmlDoc.CreateElement("temp");
            XmlElement xmlGroup = xmlDoc.CreateElement("temp");


            xmlDoc.DocumentElement.SetAttribute("Title", "用户自定义扩展项");

            //创建基础信息部分
            DataRow[] rows = dtExItemList.Select("TypeName not in ('关联配置','备注信息') ", "Group,OrderBy");

            bool blnHasBase = false;
            string sLastBaseGroup = null;  //有可能遇到空的组名


            string sBaseGroup = "";
            int i = 0;
            foreach (DataRow row in rows)
            {
                sBaseGroup = row["Group"].ToString().Trim();
                if (sBaseGroup != sLastBaseGroup)
                {
                    //创建组结点

                    xmlGroup = xmlDoc.CreateElement("BaseItem");
                    xmlGroup.SetAttribute("Title", sBaseGroup);
                    sLastBaseGroup = sBaseGroup;
                    xmlDoc.DocumentElement.AppendChild(xmlGroup);
                }

                xmlEle = xmlDoc.CreateElement("AttributeItem");
                xmlEle.SetAttribute("ID", row["ID"].ToString().Trim());
                xmlEle.SetAttribute("Type", "varchar");
                xmlEle.SetAttribute("CHName", row["CHName"].ToString().Trim());
                xmlEle.SetAttribute("IsMust", row["IsMust"].ToString().Trim());

                if (row["TypeName"].ToString() == "基础信息")
                {
                    xmlEle.SetAttribute("Default", row["Default"].ToString().Trim());
                    xmlEle.SetAttribute("CtrlType", "TextBox");
                }
                else if (row["TypeName"].ToString() == "下拉选择")
                {

                    xmlEle.SetAttribute("Default", row["Default"].ToString().Trim() == "" ? "-1" : row["Default"].ToString().Trim());
                    xmlEle.SetAttribute("CtrlType", "DropDownList");
                }
                else if (row["TypeName"].ToString() == "部门信息")
                {
                    xmlEle.SetAttribute("Default", row["Default"].ToString().Trim() == "" ? "-1" : row["Default"].ToString().Trim());
                    xmlEle.SetAttribute("CtrlType", "deptList");
                }
                else if (row["TypeName"].ToString() == "用户信息")
                {
                    xmlEle.SetAttribute("Default", row["Default"].ToString().Trim() == "" ? "0" : row["Default"].ToString().Trim());
                    xmlEle.SetAttribute("CtrlType", "UserList");
                }
                else if (row["TypeName"].ToString() == "日期类型")
                {
                    xmlEle.SetAttribute("Default", row["Default"].ToString().Trim());
                    xmlEle.SetAttribute("CtrlType", "Time");
                }
                else if (row["TypeName"].ToString() == "数值类型")
                {
                    xmlEle.SetAttribute("Default", row["Default"].ToString().Trim() == "" ? "0" : row["Default"].ToString().Trim());
                    xmlEle.SetAttribute("CtrlType", "Number");
                }
                xmlEle.SetAttribute("OrderBy", row["OrderBy"].ToString().Trim() == "" ? "0" : row["OrderBy"].ToString().Trim());//排序
                xmlEle.SetAttribute("isChack", row["isChack"].ToString().Trim() == "" ? "0" : row["isChack"].ToString().Trim());//是否时间  
                xmlEle.SetAttribute("IsSelect", row["IsSelect"].ToString().Trim() == "" ? "0" : row["IsSelect"].ToString().Trim());//是否查询条件
                xmlEle.SetAttribute("GroupName", row["GroupName"].ToString().Trim());
                xmlEle.SetAttribute("GroupID", row["GroupID"].ToString().Trim());

                xmlGroup.AppendChild(xmlEle);
            }

            //创建关联配置部分
            rows = dtExItemList.Select("TypeName='关联配置'", "Group,OrderBy");
            blnHasBase = false;
            sLastBaseGroup = null;
            sBaseGroup = "";
            i = 0;
            foreach (DataRow row in rows)
            {
                i++;
                sBaseGroup = row["Group"].ToString().Trim();
                if (sBaseGroup != sLastBaseGroup)
                {
                    //创建组结点

                    xmlGroup = xmlDoc.CreateElement("RelationConfig");
                    xmlGroup.SetAttribute("Title", sBaseGroup);
                    sLastBaseGroup = sBaseGroup;
                    xmlDoc.DocumentElement.AppendChild(xmlGroup);
                }
                xmlEle = xmlDoc.CreateElement("AttributeItem");
                xmlEle.SetAttribute("ID", row["ID"].ToString().Trim());
                xmlEle.SetAttribute("IsMust", row["IsMust"].ToString().Trim());
                xmlEle.SetAttribute("CHName", row["CHName"].ToString().Trim());
                xmlEle.SetAttribute("Default", row["Default"].ToString().Trim());
                xmlEle.SetAttribute("OrderBy", row["OrderBy"].ToString().Trim() == "" ? "0" : row["OrderBy"].ToString().Trim());
                xmlEle.SetAttribute("isChack", row["isChack"].ToString().Trim() == "" ? "0" : row["isChack"].ToString().Trim());//是否时间      
                xmlEle.SetAttribute("IsSelect", row["IsSelect"].ToString().Trim() == "" ? "0" : row["IsSelect"].ToString().Trim());//是否查询条件
                xmlEle.SetAttribute("GroupName", row["GroupName"].ToString().Trim());
                xmlEle.SetAttribute("GroupID", row["GroupID"].ToString().Trim());
                xmlGroup.AppendChild(xmlEle);
            }

            rows = dtExItemList.Select("TypeName='备注信息'", "Group,OrderBy");
            blnHasBase = false;
            sLastBaseGroup = null;
            sBaseGroup = "";
            i = 0;
            foreach (DataRow row in rows)
            {
                i++;
                sBaseGroup = row["Group"].ToString().Trim();
                if (sBaseGroup != sLastBaseGroup)
                {
                    //创建组结点

                    xmlGroup = xmlDoc.CreateElement("Remark");
                    xmlGroup.SetAttribute("Title", sBaseGroup);
                    sLastBaseGroup = sBaseGroup;
                    xmlDoc.DocumentElement.AppendChild(xmlGroup);
                }
                xmlEle = xmlDoc.CreateElement("AttributeItem");
                xmlEle.SetAttribute("ID", row["ID"].ToString().Trim());
                xmlEle.SetAttribute("CHName", row["CHName"].ToString().Trim());
                xmlEle.SetAttribute("IsMust", row["IsMust"].ToString().Trim());
                xmlEle.SetAttribute("Default", row["Default"].ToString().Trim());
                xmlEle.SetAttribute("CtrlType", "MultiLine");
                xmlEle.SetAttribute("OrderBy", row["OrderBy"].ToString().Trim() == "" ? "0" : row["OrderBy"].ToString().Trim());
                xmlEle.SetAttribute("isChack", row["isChack"].ToString().Trim() == "" ? "0" : row["isChack"].ToString().Trim());//是否时间  
                xmlEle.SetAttribute("IsSelect", row["IsSelect"].ToString().Trim() == "" ? "0" : row["IsSelect"].ToString().Trim());//是否查询条件
                xmlEle.SetAttribute("GroupName", row["GroupName"].ToString().Trim());
                xmlEle.SetAttribute("GroupID", row["GroupID"].ToString().Trim());

                xmlGroup.AppendChild(xmlEle);
            }

            return xmlDoc.InnerXml;

        }

        /// <summary>
        /// 将XML字串转换成DataTable
        /// </summary>
        /// <param name="strXML"></param>
        /// <returns></returns>
        private DataTable ConvertXmlStrToDataTable(String strXML)
        {
            DataTable dtExItemList = ExtensionItemBS.CreateNullTable();

            if (strXML != String.Empty)
            {
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.LoadXml(strXML);

                AddBaseSchemeDatas(xmldoc, ref dtExItemList);

                XmlNodeList relnodes = xmldoc.SelectNodes("EquScheme/RelationConfig");

                foreach (XmlNode relnode in relnodes)
                {
                    AddRelSchemeDatas(relnode, ref dtExItemList);
                }

                relnodes = xmldoc.SelectNodes("EquScheme/Remark");

                foreach (XmlNode relnode in relnodes)
                {
                    AddRelRemarkDatas(relnode, ref dtExItemList);
                }
            }

            return dtExItemList;
        }


        /// <summary>
        /// 备注信息
        /// </summary>
        /// <param name="relnode"></param>
        /// <param name="dt"></param>
        private void AddRelRemarkDatas(XmlNode relnode, ref DataTable dt)
        {
            XmlNodeList ns = null;

            string strGroup = "";
            string strTypeName = "备注信息";

            if (relnode.Attributes["Title"] != null)
            {
                strGroup = relnode.Attributes["Title"].Value;
            }
            ns = relnode.SelectNodes("AttributeItem");
            object[] values = new object[11];
            foreach (XmlNode n in ns)
            {
                values[0] = (object)n.Attributes["ID"].Value;
                values[1] = (object)n.Attributes["CHName"].Value;
                values[2] = (object)n.Attributes["Default"].Value;
                values[3] = (object)n.Attributes["IsMust"].Value;
                values[4] = (object)strGroup;
                values[5] = (object)strTypeName;
                values[6] = (object)n.Attributes["OrderBy"].Value;
                values[7] = (object)n.Attributes["isChack"].Value;
                values[8] = (object)n.Attributes["IsSelect"].Value;
                values[9] = (object)n.Attributes["GroupID"].Value;
                values[10] = (object)n.Attributes["GroupName"].Value;
                dt.Rows.Add(values);

            }
        }

        /// <summary>
        /// 关联信息
        /// </summary>
        /// <param name="relnode"></param>
        /// <param name="dt"></param>
        private void AddRelSchemeDatas(XmlNode relnode, ref DataTable dt)
        {
            XmlNodeList ns = null;

            string strGroup = "";
            string strTypeName = "关联配置";

            if (relnode.Attributes["Title"] != null)
            {
                strGroup = relnode.Attributes["Title"].Value;
            }
            ns = relnode.SelectNodes("AttributeItem");
            object[] values = new object[11];
            foreach (XmlNode n in ns)
            {
                values[0] = (object)n.Attributes["ID"].Value;
                values[1] = (object)n.Attributes["CHName"].Value;
                values[2] = (object)n.Attributes["Default"].Value;
                values[3] = (object)n.Attributes["IsMust"].Value;
                values[4] = (object)strGroup;
                values[5] = (object)strTypeName;
                values[6] = (object)n.Attributes["OrderBy"].Value;
                values[7] = (object)n.Attributes["isChack"].Value;
                values[8] = (object)n.Attributes["IsSelect"].Value;
                values[9] = (object)n.Attributes["GroupID"].Value;
                values[10] = (object)n.Attributes["GroupName"].Value;
                dt.Rows.Add(values);

            }
        }

        /// <summary>
        /// 基础信息
        /// </summary>
        /// <param name="xmldoc"></param>
        /// <param name="dt"></param>
        private void AddBaseSchemeDatas(XmlDocument xmldoc, ref DataTable dt)
        {
            XmlNodeList bnodes = xmldoc.SelectNodes("EquScheme/BaseItem");
            string strGroup = "";
            foreach (XmlNode node in bnodes)
            {
                strGroup = "";

                if (node.Attributes["Title"] != null)
                {
                    strGroup = node.Attributes["Title"].Value;
                }
                object[] values = new object[11];

                XmlNodeList ns = node.SelectNodes("AttributeItem");

                foreach (XmlNode n in ns)
                {
                    values[0] = (object)n.Attributes["ID"].Value;
                    values[1] = (object)n.Attributes["CHName"].Value;
                    values[2] = (object)n.Attributes["Default"].Value;
                    values[3] = (object)n.Attributes["IsMust"].Value;
                    values[4] = (object)strGroup;
                    if (n.Attributes["CtrlType"].Value == "TextBox")
                    {
                        values[5] = (object)"基础信息";
                    }
                    else if (n.Attributes["CtrlType"].Value == "DropDownList")
                    {
                        values[5] = (object)"下拉选择";
                    }
                    else if (n.Attributes["CtrlType"].Value == "deptList")
                    {
                        values[5] = (object)"部门信息";
                    }
                    else if (n.Attributes["CtrlType"].Value == "UserList")
                    {
                        values[5] = (object)"用户信息";
                    }
                    else if (n.Attributes["CtrlType"].Value == "Time")
                    {
                        values[5] = (object)"日期类型";
                    }
                    else if (n.Attributes["CtrlType"].Value == "Number")
                    {
                        values[5] = (object)"数值类型";
                    }
                    else
                    {
                        values[5] = (object)"基础信息";
                    }
                    values[6] = (object)n.Attributes["OrderBy"].Value;
                    values[7] = (object)n.Attributes["isChack"].Value;
                    values[8] = (object)n.Attributes["IsSelect"].Value;
                    values[9] = (object)n.Attributes["GroupID"].Value;
                    values[10] = (object)n.Attributes["GroupName"].Value;
                    dt.Rows.Add(values);
                }
            }
        }

        #endregion


        /// <summary>
        /// 创建空的 datatable 结构
        /// </summary>
        /// <returns></returns>
        public static DataTable CreateNullTable()
        {
            DataTable dt = new DataTable("Schema");

            dt.Columns.Add("ID");
            dt.Columns.Add("CHName");
            dt.Columns.Add("Default");
            dt.Columns.Add("IsMust");//是否必填
            dt.Columns.Add("Group");
            dt.Columns.Add("TypeName");
            dt.Columns.Add("OrderBy");//排序
            dt.Columns.Add("isChack");//是否时间
            dt.Columns.Add("IsSelect");//是否设置为查询条件

            dt.Columns.Add("GroupID");
            dt.Columns.Add("GroupName");  //所属分组
            dt.Columns.Add("nfieldid");    // 新扩展项编号

            return dt;
        }

        /// <summary>
        /// 将ItemTypeName转换成ItemTypeIndex
        /// </summary>
        /// <param name="strItemTypeName">扩展项类型名</param>
        /// <returns></returns>
        public static Int32 ConvertItemTypeNameToIndex(String strItemTypeName)
        {
            Int32 intItemTypeIndex = 0;
            switch (strItemTypeName)
            {
                case "基础信息":
                    intItemTypeIndex = 0;
                    break;
                case "关联配置":
                    intItemTypeIndex = 1;
                    break;
                case "备注信息":
                    intItemTypeIndex = 2;
                    break;
                case "下拉选择":
                    intItemTypeIndex = 3;
                    break;
                case "部门信息":
                    intItemTypeIndex = 4;
                    break;
                case "用户信息":
                    intItemTypeIndex = 5;
                    break;
                case "日期类型":
                    intItemTypeIndex = 6;
                    break;
                case "数值类型":
                    intItemTypeIndex = 7;
                    break;
            }

            return intItemTypeIndex;

        }

        /// <summary>
        /// 将ItemTypeIndex转换成ItemTypeName
        /// </summary>
        /// <param name="itemTypeIndex">扩展项类型索引</param>
        /// <returns></returns>
        public static String ConvertIndexToItemTypeName(Int32 itemTypeIndex)
        {
            String strItemTypeName = String.Empty;
            switch (itemTypeIndex)
            {
                case 0:
                    strItemTypeName = "基础信息";
                    break;
                case 1:
                    strItemTypeName = "关联配置";
                    break;
                case 2:
                    strItemTypeName = "备注信息";
                    break;
                case 3:
                    strItemTypeName = "下拉选择";
                    break;
                case 4:
                    strItemTypeName = "部门信息";
                    break;
                case 5:
                    strItemTypeName = "用户信息";
                    break;
                case 6:
                    strItemTypeName = "日期类型";
                    break;
                case 7:
                    strItemTypeName = "数值类型";
                    break;
            }

            return strItemTypeName;

        }


        /// <summary>
        /// 加载流程模型列表到-DropDownList
        /// </summary>
        public static void LoadFlowModelListToDropDownList(DropDownList ddlObj, long lngAppID)
        {
            string stWhere = " and AppID=" + lngAppID.ToString();
            stWhere = stWhere + " and status=1 and deleted=0 ";
            ddlObj.DataSource = MailAndMessageRuleDP.getAllFlowModel(stWhere).DefaultView;
            ddlObj.DataTextField = "flowname";
            ddlObj.DataValueField = "oflowmodelid";

            ddlObj.DataBind();
            ddlObj.Items.Insert(0, " ");
        }

        /// <summary>
        /// 加载应用列表到-DropDownList
        /// </summary>
        public static void LoadAppListToDropDownList(DropDownList ddlObj)
        {
            List<KeyValuePair<String, String>> listApp = new List<KeyValuePair<string, string>>();
            listApp.Add(new KeyValuePair<string, string>("-1", "==请选择=="));
            listApp.Add(new KeyValuePair<string, string>("1026", "事件单"));
            listApp.Add(new KeyValuePair<string, string>("210", "问题单"));
            listApp.Add(new KeyValuePair<string, string>("420", "变更单"));

            ddlObj.DataSource = listApp;
            ddlObj.DataTextField = "Value";
            ddlObj.DataValueField = "Key";

            ddlObj.DataBind();
        }



        /// <summary>
        /// 更新equ_deploy所属的FieldID
        /// </summary>
        /// <param name="trans">事务对象</param>
        /// <param name="lngFlowModelID">流程模型编号</param>
        /// <param name="dtExItemList">扩展项列表</param>
        /// <returns></returns>
        public void UpdateExDeployFieldID(OracleTransaction trans,
            long lngFlowModelID, DataTable dtExItemList)
        {
            foreach (DataRow dr in dtExItemList.Rows)
            {
                long ofieldid = long.Parse(dr["id"].ToString());
                long nfieldid = long.Parse(dr["nfieldid"].ToString());

                Equ_DeskDP.UpdateExFieldID(trans, nfieldid, ofieldid);
            }
        }


    }
}
