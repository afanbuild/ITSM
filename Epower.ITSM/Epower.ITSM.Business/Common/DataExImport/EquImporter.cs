/*******************************************************************
 * 版权所有：深圳市非凡信息技术有限公司
 * 描述：配置管理-数据导入-资产导入
 * 
 * 
 * 创建人：孙绍棕
 * 创建日期：2013-12-18
 * *****************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.OleDb;
using Epower.ITSM.SqlDAL.ES_TBLCS;
using Epower.ITSM.SqlDAL;
using Epower.DevBase.Organization.SqlDAL;
using Epower.ITSM.Base;
using System.IO;
using System.Xml;
using EpowerCom;
using Epower.DevBase.BaseTools;

namespace Epower.ITSM.Business.Common.DataExImport
{
    /// <summary>
    /// 资产列表导入
    /// </summary>
    public class EquImporter : ExcelImporter
    {

        /// <summary>
        /// 用户编号
        /// </summary>
        private long _userID;

        /// <summary>
        /// 用户名
        /// </summary>
        private String _userName;

        /// <summary>
        /// 用户部门编号
        /// </summary>
        private long _userDeptID;

        /// <summary>
        /// 用户部门名
        /// </summary>
        private String _userDeptName;

        /// <summary>
        /// 导入结果
        /// </summary>
        private String _importResult;

        /// <summary>
        /// 导入成功条数
        /// </summary>
        public Int32 ImportedCount
        {
            get;
            private set;
        }

        /// <summary>
        /// 导入失败条数
        /// </summary>
        public Int32 ImportFaildCount
        {
            get;
            private set;
        }

        /// <summary>
        /// 资产配置项
        /// </summary>
        private List<EQU_deploy> _listEquDeploy;

        /// <summary>
        /// 设置用户信息
        /// </summary>
        /// <param name="lngUserID">用户编号</param>
        /// <param name="strUserName">用户名</param>
        /// <param name="lngDeptID">部门编号</param>
        /// <param name="strDeptName">部门名</param>
        public void SetupUserInfo(long lngUserID, String strUserName, long lngDeptID, String strDeptName)
        {
            this._userID = lngUserID;
            this._userName = strUserName;
            this._userDeptID = lngDeptID;
            this._userDeptName = strDeptName;
        }

        /// <summary>
        /// 取资产列表
        /// </summary>
        /// <param name="strFileURL"></param>
        /// <returns></returns>
        private DataTable GetEquList(String strFileURL)
        {
            // 查询语句
            string sql = "SELECT * FROM [资产导入$]";
            DataSet ds = new DataSet();
            OleDbDataAdapter da = new OleDbDataAdapter(sql, this.GetConnStr(strFileURL));
            da.Fill(ds);

            return ds.Tables[0];
        }


        /// <summary>
        /// 初始化(给默认值)资产的其他属性
        /// </summary>
        /// <param name="equDeskModel"></param>
        private void InitOtherProperty(Equ_DeskDP equDeskModel)
        {
            equDeskModel.FullID = Equ_SubjectDP.GetSubjectFullID(long.Parse(equDeskModel.CatalogID.ToString()));
            equDeskModel.ConfigureInfo = "";

            equDeskModel.Deleted = (int)eRecord_Status.eNormal;
            equDeskModel.RegUserID = this._userID;
            equDeskModel.RegUserName = this._userName;
            equDeskModel.RegTime = DateTime.Now;
            equDeskModel.RegDeptID = this._userDeptID;
            equDeskModel.RegDeptName = this._userDeptName;

            equDeskModel.ListID = 0;
            equDeskModel.ListName = "";

        }

        /// <summary>
        /// 设置列值
        /// </summary>
        /// <param name="strColumnName">列名</param>
        /// <param name="equDeskModel">客户对象</param>
        /// <param name="row">行对象</param>
        /// <returns></returns>
        private bool SetValue(String strColumnName, Equ_DeskDP equDeskModel, DataRow row)
        {
            Object objVal = row[strColumnName];    // 取某列值
            if (objVal == null || String.IsNullOrEmpty(objVal.ToString()) == true)
                return false;


            bool isOK = true;

            String strVal = objVal.ToString().Trim();

            switch (strColumnName)
            {
                case "资产编号":

                    equDeskModel.Code = strVal;

                    break;
                case "资产名称":

                    equDeskModel.Name = strVal;

                    break;
                case "资产类别":

                    equDeskModel.CatalogName = strVal;

                    if (CommonDP.AddEqu_Category(strVal))
                    {
                        String strCatalogID = CommonDP.SelectEqu_Category(strVal);

                        if (strCatalogID.Trim() == String.Empty)
                            strCatalogID = "0";

                        equDeskModel.CatalogID = long.Parse(strCatalogID);
                    }
                    else
                    {
                        isOK = false;
                    }

                    break;
                case "所属客户":

                    equDeskModel.Costom = decimal.Parse(CommonDP.getCustID(strVal));
                    equDeskModel.CostomName = strVal;

                    break;
                case "保修开始时间":

                    equDeskModel.ServiceBeginTime = strVal == "" ? DateTime.MinValue : Convert.ToDateTime(strVal);

                    break;
                case "保修结束时间":

                    equDeskModel.ServiceEndTime = strVal == "" ? DateTime.MinValue : Convert.ToDateTime(strVal);

                    break;
                case "维护机构":    // 占位分支, 无实际用处


                    equDeskModel.partBankName = strVal;


                    break;
                case "维护部门":    // 占位分支, 无实际用处

                    equDeskModel.partBranchName = strVal;

                    break;
                case "维护部门编号":

                    //String strBranchID = DeptDP.getDeptId_in_Org(Convert.ToInt64(equDeskModel.partBankId), strVal);
                    long lngDeptID = long.Parse(strVal);    // 维护部门编号                    

                    String strDeptName = DeptDP.GetDeptName(lngDeptID);

                    if (String.IsNullOrEmpty(strDeptName))
                    {
                        isOK = false;
                        break;
                    }

                    equDeskModel.partBranchName = strDeptName;
                    equDeskModel.partBranchId = Convert.ToDecimal(lngDeptID);

                    long lngOrgID = DeptDP.GetDirectOrg(lngDeptID);
                    String strOrgName = DeptDP.GetDeptName(lngOrgID);


                    // # 使用部门编号, 取其直属机构编号和名字

                    equDeskModel.partBankName = strOrgName;    // 机构名
                    equDeskModel.partBankId = Convert.ToDecimal(lngOrgID);    // 机构编号

                    break;
                case "资产状态":

                    try
                    {
                        if (CommonDP.AddEquStatueType(strVal))
                        {
                            String strEquStatus = CommonDP.SelectEquStatueType(strVal);
                            equDeskModel.EquStatusID = strEquStatus == "" ? 0 : decimal.Parse(strEquStatus);    // 获得资产状态编号
                            equDeskModel.EquStatusName = strVal;
                        }
                        else
                        {
                            isOK = false;
                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                        isOK = false;
                        E8Logger.Error(ex);
                    }


                    break;
                case "服务单位":

                    Br_MastCustomerDP ee = new Br_MastCustomerDP();
                    ee = ee.GetReCorded(strVal);

                    if (ee.ID <= 0)
                    {
                        isOK = false;
                        break;
                    }

                    equDeskModel.Mastcustid = ee.ID;//服务单位

                    break;
                default:    // 其他情况

                    List<EQU_deploy> listEquDeploy = SetupExItem(strColumnName, strVal);
                    long lngFieldID = listEquDeploy[0].FieldID;

                    UpdateExItem(strColumnName, lngFieldID, equDeskModel.CatalogID, equDeskModel.CatalogName);

                    if (this._listEquDeploy == null)
                        this._listEquDeploy = new List<EQU_deploy>();

                    this._listEquDeploy.Add(listEquDeploy[0]);

                    break;
            }

            return isOK;
        }

        /// <summary>
        /// 设置资产配置项
        /// </summary>
        /// <param name="strColumnName">列名</param>
        /// <param name="strVal">值</param>
        /// <returns></returns>
        private List<EQU_deploy> SetupExItem(String strColumnName, String strVal)
        {
            List<EQU_deploy> listEquDeploy = new List<EQU_deploy>();

            string FieldId = string.Empty;
            string ItemType = string.Empty;
            if (CommonDP.AddEqu_SchemaItemsWithStoredProc(strColumnName))
            {
                FieldId = CommonDP.SelectEqu_SchemaItemsType(strColumnName, ref ItemType);
            }

            EQU_deploy deploy = new EQU_deploy();
            deploy.FieldID = long.Parse(FieldId);
            deploy.CHName = strColumnName;


            switch (ItemType)
            {
                case "1"://关联配置

                    deploy.Value = "1";

                    break;
                case "3"://下拉选择

                    deploy.Value = CatalogDP.GetCatalogIDbyName(strVal);

                    break;
                case "4"://部门信息

                    deploy.Value = DeptDP.GetDeptIDbyName(strVal);

                    break;
                case "5"://用户信息

                    deploy.Value = UserDP.GetUserIDbyName(strVal);

                    break;
                case "6"://日期类型

                    try
                    {
                        deploy.Value = DateTime.Parse(strVal).ToString();
                    }
                    catch
                    {
                        deploy.Value = string.Empty;
                    }

                    break;
                default:

                    deploy.Value = strVal;

                    break;
            }

            listEquDeploy.Add(deploy);

            return listEquDeploy;
        }

        /// <summary>
        /// 更新资产配置项
        /// </summary>
        /// <param name="strColumnName">列名</param>
        /// <param name="lngFieldID">配置项编号</param>
        /// <param name="strEquTypeID">资产类别编号</param>
        /// <param name="strEquTypeName">资产类别名</param>
        private void UpdateExItem(String strColumnName, long lngFieldID, decimal strEquTypeID, String strEquTypeName)
        {
            //读取xml
            string xml = CommonDP.SelectEqu_CategoryconfigureSchema(strEquTypeID.ToString());
            DataTable table = createTable(xml);
            //判断是否已经存在
            if (getXmlDoc(xml, strColumnName) == false)
            {
                #region 取配置项类型
                string stritemname = string.Empty;
                string itemtype = string.Empty;
                CommonDP.SelectEqu_SchemaItemsType(strColumnName, ref itemtype);

                switch (itemtype)
                {
                    case "0":

                        stritemname = "基础信息";

                        break;
                    case "1":

                        stritemname = "关联配置";

                        break;
                    case "2":

                        stritemname = "备注信息";

                        break;
                    case "3":

                        stritemname = "下拉选择";

                        break;
                    case "4":

                        stritemname = "部门信息";

                        break;
                    case "5":

                        stritemname = "用户信息";

                        break;
                    case "6":

                        stritemname = "日期类型";

                        break;
                    case "7":

                        stritemname = "数值类型";

                        break;
                    default:

                        stritemname = "基础信息";

                        break;
                }

                #endregion


                object[] values = new object[9];
                values[0] = (object)lngFieldID;
                values[1] = (object)strColumnName;
                values[2] = (object)"";
                values[3] = (object)"0";
                values[4] = (object)"配置信息";
                values[5] = (object)stritemname;
                values[6] = (object)"0";
                values[7] = (object)"0";
                values[8] = (object)"0";
                table.Rows.Add(values);
            }


            CommonDP.UpdateEqu_CategoryconfigureSchema(strEquTypeID.ToString(), GetSchemaXml(table, strEquTypeName));    // 更新表

        }

        /// <summary>
        /// 资产导入日志
        /// </summary>
        /// <param name="strImportResult"></param>
        private void WriteImportLog(String strImportResult)
        {

            if (strImportResult == String.Empty)
            {
                return;
            }

            string strDirLog = @"c:\EpowerLog\";
            if (!System.IO.Directory.Exists(strDirLog))
            {
                System.IO.Directory.CreateDirectory(strDirLog);
            }

            string strLogFileName = strDirLog + "资产导入日志" + System.DateTime.Now.ToString("yyyyMMdd") + ".log";

            using (StreamWriter sw = new StreamWriter(strLogFileName, true))
            {
                sw.WriteLine(System.DateTime.Now.ToString() + "资产导入错误");
                sw.WriteLine(strImportResult);
            }

        }

        /// <summary>
        /// 资产列表导入
        /// </summary>
        /// <param name="strFileURL">资产列表Excel文件地址</param>
        /// <param name="sbResult">导入结果</param>
        /// <returns>T, 成功; F, 失败</returns>
        public override bool Exec(string strFileURL, ref StringBuilder sbResult)
        {
            int succeed = 0;
            int lost = 0;
            int rowIndex = 1;

            DataTable dtEqu = this.GetEquList(strFileURL);


            foreach (DataRow dr in dtEqu.Rows)
            {

                if (this._listEquDeploy != null)
                    this._listEquDeploy.Clear();

                rowIndex += 1;

                Equ_DeskDP equDeskModel = new Equ_DeskDP();

                try
                {


                    bool isOK = true;
                    String strColumnName = String.Empty;
                    List<String> listColumn = new List<string>();

                    foreach (DataColumn dc in dtEqu.Columns)
                    {
                        strColumnName = dc.ColumnName;
                        isOK = SetValue(strColumnName, equDeskModel, dr);

                        if (!isOK)
                        {
                            listColumn.Add(strColumnName);
                        }
                    }

                    if (equDeskModel.Name == String.Empty)    // 资产名为空
                    {
                        lost += 1;

                        sbResult.AppendFormat(@"数据行【{0}】导入失败！原因: 资产名称不能为空。<br /><br/>",
                                rowIndex);

                        continue;
                    }

                    if (listColumn.Count > 0)    // 必填字段为空或内容不正确
                    {
                        lost += 1;

                        sbResult.AppendFormat(@"数据行【{0}】导入失败！原因: 资产【{1}】的列【{2}】的值为空或数据不正确。<br /><br/>",
                            rowIndex, equDeskModel.Name, String.Join("、", listColumn.ToArray()));

                        continue;
                    }

                    bool isRepeatCode = equDeskModel.CodeIsTow(equDeskModel.Code);

                    if (isRepeatCode)    // 资产编号重复
                    {
                        lost += 1;

                        sbResult.AppendFormat(@"数据行【{0}】导入失败！原因: 资产【{1}】的编号【{2}】重复。<br /><br/>",
                                rowIndex, equDeskModel.Name, equDeskModel.Code);

                        continue;
                    }



                    InitOtherProperty(equDeskModel);

                    if (this._listEquDeploy == null)
                        this._listEquDeploy = new List<EQU_deploy>();

                    equDeskModel.InsertRecorded(equDeskModel, this._listEquDeploy);
                    succeed += 1;
                }
                catch (Exception ex)
                {
                    lost += 1;

                    sbResult.AppendFormat(@"数据行【{0}】导入失败！原因: 资产【{1}】由于[{2}]导入失败。<br /><br/>",
                                rowIndex, equDeskModel.Name, ex.Message);
                }


            }


            this.ImportedCount = succeed;
            this.ImportFaildCount = lost;

            WriteImportLog(sbResult.ToString());

            return true;


        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private string GetSchemaXml(DataTable dt, string SubjectName)
        {
            if (dt.Rows.Count == 0)
                return "";

            XmlDocument xmlDoc = new XmlDocument();
            // xmlDoc.LoadXml(@"<?xml version=""1.0"" encoding=""utf-8"" ?><EquScheme></EquScheme>");
            xmlDoc.LoadXml(@"<EquScheme></EquScheme>");
            XmlElement xmlEle = xmlDoc.CreateElement("temp");
            XmlElement xmlRoot = xmlDoc.CreateElement("temp");
            XmlElement xmlGroup = xmlDoc.CreateElement("temp");


            xmlDoc.DocumentElement.SetAttribute("Title", SubjectName);

            //创建基础信息部分
            DataRow[] rows = dt.Select("TypeName not in ('关联配置','备注信息') ", "Group,OrderBy");

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



                xmlGroup.AppendChild(xmlEle);
            }

            //创建关联配置部分
            rows = dt.Select("TypeName='关联配置'", "Group,OrderBy");
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
                xmlGroup.AppendChild(xmlEle);
            }

            rows = dt.Select("TypeName='备注信息'", "Group,OrderBy");
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

                xmlGroup.AppendChild(xmlEle);
            }

            return xmlDoc.InnerXml;

        }

        public DataTable createTable(string strXmlTemp)
        {
            DataTable dt = CreateNullTable();

            if (strXmlTemp != "")
            {
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.LoadXml(strXmlTemp);

                AddBaseSchemeDatas(xmldoc, ref dt);

                XmlNodeList relnodes = xmldoc.SelectNodes("EquScheme/RelationConfig");

                foreach (XmlNode relnode in relnodes)
                {
                    AddRelSchemeDatas(relnode, ref dt);
                }

                relnodes = xmldoc.SelectNodes("EquScheme/Remark");

                foreach (XmlNode relnode in relnodes)
                {
                    AddRelRemarkDatas(relnode, ref dt);
                }

            }
            return dt;
        }


        /// <summary>
        /// 
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
                object[] values = new object[9];

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
                    dt.Rows.Add(values);
                }
            }
        }

        /// <summary>
        /// 
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
            object[] values = new object[9];
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
                dt.Rows.Add(values);

            }
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
            object[] values = new object[9];
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
                dt.Rows.Add(values);

            }
        }

        public bool getXmlDoc(string Xml, string rtValue)
        {
            int i = 0;
            if (Xml.Trim() == "")
            {
                return false;
            }
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml(Xml);
            XmlNodeList bnodes = xmldoc.SelectNodes("EquScheme/BaseItem");
            if (bnodes.Count > 0)
            {
                foreach (XmlNode relnode in bnodes)
                {
                    XmlNodeList ns = relnode.SelectNodes("AttributeItem");
                    foreach (XmlNode n in ns)
                    {

                        if (n.Attributes["CHName"] != null && n.Attributes["CHName"].Value.ToString() == rtValue)
                        {
                            i = 1;
                        }
                    }
                }
            }
            bnodes = xmldoc.SelectNodes("EquScheme/RelationConfig");
            if (bnodes.Count > 0)
            {
                foreach (XmlNode relnode in bnodes)
                {
                    XmlNodeList ns = relnode.SelectNodes("AttributeItem");
                    foreach (XmlNode n in ns)
                    {

                        if (n.Attributes["CHName"] != null && n.Attributes["CHName"].Value.ToString() == rtValue)
                        {
                            i = 1;
                        }
                    }
                }
            }
            bnodes = xmldoc.SelectNodes("EquScheme/Remark");
            if (bnodes.Count > 0)
            {
                foreach (XmlNode relnode in bnodes)
                {
                    XmlNodeList ns = relnode.SelectNodes("AttributeItem");
                    foreach (XmlNode n in ns)
                    {

                        if (n.Attributes["CHName"] != null && n.Attributes["CHName"].Value.ToString() == rtValue)
                        {
                            i = 1;
                        }
                    }
                }
            }
            if (i > 0)
            {
                return true;
            }
            else
            {
                return false;
            }


        }

        private DataTable CreateNullTable()
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




            return dt;
        }

    }
}
