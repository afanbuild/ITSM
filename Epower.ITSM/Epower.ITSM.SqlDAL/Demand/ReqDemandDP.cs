/*******************************************************************
 * 版权所有：深圳市非凡信息技术有限公司
 * 描述："需求管理" 的数据库访问类，在此定义需求管理模块需要与数据库交互的相关方法。
 * 
 * 
 * 创建人：孙绍棕
 * 创建日期：2013-04-25
 * 
 * 修改日志：
 * 修改时间：2013-04-25 修改人：孙绍棕
 * 修改描述： * 
 * 修改日志：增加取需求年份的方法(GetReqDemandYears), 获取需求涉及的管理处(GetManageOffices), 全年度需求发生趋势分析(GetAnalysisDirection). 这三个方法用于需求的年度数据分析报表.

 * 修改时间：2013-05-03 修改人：孙绍棕
 * 修改描述： 

 * *****************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using EpowerCom;
using System.Data;
using Epower.DevBase.Organization.SqlDAL;
using Epower.DevBase.BaseTools;
using System.Data.OracleClient;
using EpowerGlobal;
using Epower.DevBase.Organization.Base;
using System.Xml;
using Epower.ITSM.Net;

namespace Epower.ITSM.SqlDAL.Demand
{
    /// <summary>
    /// "需求管理" 的数据库访问类

    /// </summary>
    public class ReqDemandDP
    {
        public ReqDemandDP() { }

        #region 更新需求单 - 2013-04-25 @孙绍棕


        /// <summary>
        /// 更新需求单
        /// </summary>
        /// <param name="trans">事务对象</param>
        /// <param name="lngFlowID">流程编号</param>
        /// <param name="lngNodeModelID">流程环节编号</param>
        /// <param name="fieldValueList">表单键值集</param>
        /// <returns>T, 成功. F, 失败</returns>
        public Boolean UpdateByFlowID(OracleTransaction trans,
            long lngFlowID,
            long lngNodeModelID,
            FieldValues fieldValueList)
        {

            #region Step 1: 构造SQL语句.

            StringBuilder sbSqlScript = new StringBuilder(" UPDATE Req_Demand SET ");

            // 流程环节
            sbSqlScript.AppendFormat(" NodeModelID = {0},", lngNodeModelID);

            // 客户
            sbSqlScript.AppendFormat(" CustUserID = {0},", fieldValueList.GetFieldValue("CustUserID").Value);
            sbSqlScript.AppendFormat(" CustUserName = {0},", StringTool.SqlQ(fieldValueList.GetFieldValue("CustUserName").Value));
            sbSqlScript.AppendFormat(" CustAddress = {0},", StringTool.SqlQ(fieldValueList.GetFieldValue("CustAddress").Value));
            sbSqlScript.AppendFormat(" CustContact = {0},", StringTool.SqlQ(fieldValueList.GetFieldValue("CustContact").Value));
            sbSqlScript.AppendFormat(" CustTel = {0},", StringTool.SqlQ(fieldValueList.GetFieldValue("CustTel").Value));
            sbSqlScript.AppendFormat(" CustDeptName = {0},", StringTool.SqlQ(fieldValueList.GetFieldValue("CustDeptName").Value));
            sbSqlScript.AppendFormat(" CustEmail = {0},", StringTool.SqlQ(fieldValueList.GetFieldValue("CustEmail").Value));
            sbSqlScript.AppendFormat(" CustMastName = {0},", StringTool.SqlQ(fieldValueList.GetFieldValue("CustMastName").Value));
            sbSqlScript.AppendFormat(" CustJobID = {0},", fieldValueList.GetFieldValue("CustJobID").Value);
            sbSqlScript.AppendFormat(" CustJob = {0},", StringTool.SqlQ(fieldValueList.GetFieldValue("CustJob").Value));

            // 需求类别

            sbSqlScript.AppendFormat(" DemandTypeID = {0},", fieldValueList.GetFieldValue("DemandTypeID").Value);
            sbSqlScript.AppendFormat(" DemandTypeName = {0},", StringTool.SqlQ(fieldValueList.GetFieldValue("DemandTypeName").Value));

            // 资产
            sbSqlScript.AppendFormat(" Equipmentid = {0},", fieldValueList.GetFieldValue("Equipmentid").Value);
            sbSqlScript.AppendFormat(" EquipmentName = {0},", StringTool.SqlQ(fieldValueList.GetFieldValue("EquipmentName").Value));
            sbSqlScript.AppendFormat(" EquipmentCatalogID = {0},", fieldValueList.GetFieldValue("EquipmentCatalogID").Value);
            sbSqlScript.AppendFormat(" EquipmentCatalogName = {0},", StringTool.SqlQ(fieldValueList.GetFieldValue("EquipmentCatalogName").Value));

            // 需求主题和描述
            sbSqlScript.AppendFormat(" DemandSubject = {0},", StringTool.SqlQ(fieldValueList.GetFieldValue("DemandSubject").Value));
            sbSqlScript.AppendFormat(" DemandContent = {0},", StringTool.SqlQ(fieldValueList.GetFieldValue("DemandContent").Value));

            //需求模板相关
            sbSqlScript.AppendFormat(" ReqTempID = {0},", fieldValueList.GetFieldValue("ReqTempID").Value);
            sbSqlScript.AppendFormat(" IsUseReqTempID = {0},", fieldValueList.GetFieldValue("IsUseReqTempID").Value);

            // 需求状态
            sbSqlScript.AppendFormat(" DemandStatusID = {0},", fieldValueList.GetFieldValue("DemandStatusID").Value);
            sbSqlScript.AppendFormat(" DemandStatus = {0}", StringTool.SqlQ(fieldValueList.GetFieldValue("DemandStatus").Value));

            // Where 子句
            sbSqlScript.AppendFormat(" WHERE FlowID = {0} ", lngFlowID);

            #endregion

            #region Step 2: 执行SQL语句.

            OracleDbHelper.ExecuteNonQuery(trans, CommandType.Text, sbSqlScript.ToString());

            #endregion

            return true;
        }

        #endregion

        #region 添加新的需求单 - 2013-04-25 @孙绍棕


        /// <summary>
        /// 添加新的需求单
        /// </summary>
        /// <param name="trans">事件对象</param>
        /// <param name="lngFlowID">流程编号</param>
        /// <param name="lngNodeModelID">流程环节编号</param>
        /// <param name="lngFlowModelID">流程模型编号</param>
        /// <param name="fieldValueList">表单键值集</param>
        /// <returns>大于 0, 返回生成的需求编号. 小于等于 0, 需求单没有添加成功.</returns>
        public long Add(OracleTransaction trans,
            long lngFlowID,
            long lngNodeModelID,
            long lngFlowModelID,
            FieldValues fieldValueList)
        {
            Int32 intCodeRuleID = 10007;

            long lngReqDemandID = EPGlobal.GetNextID("Req_Demand_ID");    // 取自增编号

            String strBuildCode = String.Empty;
            RuleCodeDP.GetCodeBH2(intCodeRuleID, ref strBuildCode);    // 取需求单号前缀            

            String strReqDemandID = String.Format("{0}{1}",
                strBuildCode, lngReqDemandID);    // 生成需求单号


            #region Step 1: 构造SQL语句.

            StringBuilder sbSqlScript = new StringBuilder(@"
                    INSERT INTO Req_Demand(ID, 
                                           FlowID, 
                                           FlowModelID, 
                                           NodeModelID, 
                                           DemandNo,

                                           CustUserID,
                                           CustUserName,
                                           CustAddress,
                                           CustContact,
                                           CustTel,
                                           CustDeptName,
                                           CustEmail,
                                           CustMastName,
                                           CustJobID,
                                           CustJob,

                                           RegUserID,
                                           RegUserName,
                                           RegDeptID,
                                           RegDeptName,
                                           RegOrgID,
                                           RegTime,
                                           
                                           DemandTypeID,
                                           DemandTypeName,
                                           Equipmentid,
                                           EquipmentName,
                                           EquipmentCatalogID,
                                           EquipmentCatalogName,

                                           DemandSubject,
                                           DemandContent,
                                            
                                           DemandStatusID,
                                           DemandStatus,
                                           ReqTempID,
                                           IsUseReqTempID
                                            )");

            #endregion

            #region Step 2: 添值.

            sbSqlScript.Append(" VALUES ( ");

            // 基本
            sbSqlScript.AppendFormat("{0},", lngReqDemandID);    // > ID.
            sbSqlScript.AppendFormat("{0},", lngFlowID);    // > FlowID.
            sbSqlScript.AppendFormat("{0},", lngFlowModelID);    // > FlowModelID.
            sbSqlScript.AppendFormat("{0},", lngNodeModelID);    // > NodeModelID.
            sbSqlScript.AppendFormat("{0},", StringTool.SqlQ(strReqDemandID));    // > DemandNo.

            // 客户
            sbSqlScript.AppendFormat("{0},", fieldValueList.GetFieldValue("CustUserID").Value);    // > CustUserID.
            sbSqlScript.AppendFormat("{0},", StringTool.SqlQ(fieldValueList.GetFieldValue("CustUserName").Value));    // > CustUserName.
            sbSqlScript.AppendFormat("{0},", StringTool.SqlQ(fieldValueList.GetFieldValue("CustAddress").Value));    // > CustAddress.
            sbSqlScript.AppendFormat("{0},", StringTool.SqlQ(fieldValueList.GetFieldValue("CustContact").Value));    // > CustContact.
            sbSqlScript.AppendFormat("{0},", StringTool.SqlQ(fieldValueList.GetFieldValue("CustTel").Value));    // > CustTel.
            sbSqlScript.AppendFormat("{0},", StringTool.SqlQ(fieldValueList.GetFieldValue("CustDeptName").Value));    // > CustDeptName.
            sbSqlScript.AppendFormat("{0},", StringTool.SqlQ(fieldValueList.GetFieldValue("CustEmail").Value));    // > CustEmail.
            sbSqlScript.AppendFormat("{0},", StringTool.SqlQ(fieldValueList.GetFieldValue("CustMastName").Value));    // > CustMastName.
            sbSqlScript.AppendFormat("{0},", fieldValueList.GetFieldValue("CustJobID").Value);    // > CustJobID.
            sbSqlScript.AppendFormat("{0},", StringTool.SqlQ(fieldValueList.GetFieldValue("CustJob").Value));    // > CustJob.

            // 登单人

            sbSqlScript.AppendFormat("{0},", fieldValueList.GetFieldValue("RegUserID").Value);    // > RegUserID.
            sbSqlScript.AppendFormat("{0},", StringTool.SqlQ(fieldValueList.GetFieldValue("RegUserName").Value));    // > RegUserName.
            sbSqlScript.AppendFormat("{0},", fieldValueList.GetFieldValue("RegDeptID").Value);    // > RegDeptID.
            sbSqlScript.AppendFormat("{0},", StringTool.SqlQ(fieldValueList.GetFieldValue("RegDeptName").Value));    // > RegDeptName.
            sbSqlScript.AppendFormat("{0},", fieldValueList.GetFieldValue("RegOrgID").Value);    // > RegOrgID.
            sbSqlScript.AppendFormat("{0},", WrapDateString(fieldValueList.GetFieldValue("RegTime")));    // > RegTime.            

            // 需求类别

            sbSqlScript.AppendFormat("{0},", fieldValueList.GetFieldValue("DemandTypeID").Value);    // > DemandTypeID.
            sbSqlScript.AppendFormat("{0},", StringTool.SqlQ(fieldValueList.GetFieldValue("DemandTypeName").Value));    // > DemandTypeName.

            // 资产
            sbSqlScript.AppendFormat("{0},", fieldValueList.GetFieldValue("Equipmentid").Value);    // > Equipmentid.
            sbSqlScript.AppendFormat("{0},", StringTool.SqlQ(fieldValueList.GetFieldValue("EquipmentName").Value));    // > EquipmentName.
            sbSqlScript.AppendFormat("{0},", fieldValueList.GetFieldValue("EquipmentCatalogID").Value);    // > EquipmentCatalogID.
            sbSqlScript.AppendFormat("{0},", StringTool.SqlQ(fieldValueList.GetFieldValue("EquipmentCatalogName").Value));    // > EquipmentCatalogName.

            // 主题和内容

            sbSqlScript.AppendFormat("{0},", StringTool.SqlQ(fieldValueList.GetFieldValue("DemandSubject").Value));    // > DemandSubject.
            sbSqlScript.AppendFormat("{0},", StringTool.SqlQ(fieldValueList.GetFieldValue("DemandContent").Value));    // > DemandContent.

            // 需求状态

            sbSqlScript.AppendFormat("{0},", fieldValueList.GetFieldValue("DemandStatusID").Value);    // > DemandStatusID.
            sbSqlScript.AppendFormat("{0},", StringTool.SqlQ(fieldValueList.GetFieldValue("DemandStatus").Value));    // > DemandStatus.

            sbSqlScript.AppendFormat("{0},", fieldValueList.GetFieldValue("ReqTempID").Value);    // > ReqTempID.
            sbSqlScript.AppendFormat("{0}", fieldValueList.GetFieldValue("IsUseReqTempID").Value);    // > IsUseReqTempID.

            sbSqlScript.Append(" ) ");

            #endregion

            #region Step 3: 执行SQL语句.

            OracleDbHelper.ExecuteNonQuery(trans, CommandType.Text, sbSqlScript.ToString());

            #endregion

            return lngReqDemandID;
        }

        #endregion

        #region 查询需求单 - 2013-04-25 @孙绍棕


        /// <summary>
        /// 查询需求单
        /// </summary>
        /// <param name="strWhere">高级查询: 动态生成的SQL脚本</param>
        /// <param name="lngUserID">用户编号</param>
        /// <param name="lngDeptID">部门编号</param>
        /// <param name="lngOrgID">机构编号</param>
        /// <param name="re">权限对象</param>
        /// <param name="intPageSize">页长</param>
        /// <param name="intPageIndex">页码</param>
        /// <param name="intRowCount">总记录数</param>
        /// <returns>需求单列表</returns>
        public DataTable FetchList(string strWhere,
            long lngUserID,
            long lngDeptID,
            long lngOrgID,
            RightEntity re,
            int intPageSize,
            int intPageIndex,
            ref int intRowCount)
        {
            if (String.IsNullOrEmpty(strWhere))
                strWhere = " 1 = 1 ";

            strWhere = strWhere + VerifyUserRight(re, lngOrgID, lngDeptID, lngUserID);    // 验证用户权限

            OracleConnection conn = ConfigTool.GetConnection();
            DataTable dtReqDemand = null;
            try
            {
                dtReqDemand = OracleDbHelper.ExecuteDataTable(conn,
                "v_reqdemand",
                "*",
                " ORDER BY ID DESC",
                intPageSize,
                intPageIndex,
                strWhere,
                ref intRowCount);
            }
            finally { ConfigTool.CloseConnection(conn); }

            return dtReqDemand;
        }

        #endregion

        #region 快速查询需求单 - 2013-04-27 @孙绍棕

        /// <summary>        
        /// 快速查询需求单
        /// </summary>
        /// <param name="strXmlcond">快速查询条件XML串</param>
        /// <param name="lngUserID">用户编号</param>
        /// <param name="lngDeptID">部门编号</param>
        /// <param name="lngOrgID">机构编号</param>
        /// <param name="re">权限对象</param>
        /// <param name="intPageSize">页长</param>
        /// <param name="intPageIndex">页码</param>
        /// <param name="intRowCount">总记录数</param>
        /// <returns>快速查询出的需求单列表</returns>
        public DataTable FetchListByFastSearch(
            string strXmlcond,
            long lngUserID,
            long lngDeptID,
            long lngOrgID,
            RightEntity re,
            int intPageSize,
            int intPageIndex,
            ref int intRowCount)
        {
            #region Step 1. 定义基础变量

            string strTmp = string.Empty;               //临时存放各查询条件的值
            string strWhere = " 1=1 ";                  //存放where语句
            string strList = string.Empty;              //权限使用

            #endregion

            #region Step 2. 定义表单字段变量

            string strDemandNo = string.Empty;         //需求单号
            string strEquipmentName = string.Empty;           //资产名称
            string strFlowStatus = string.Empty;        //流程状态
            string DemandStatus = string.Empty;            //需求状态
            string strRegBeginTime = string.Empty;      //登记开始时间
            string strRegEndTime = string.Empty;        //登记结束时间

            string strDemandSubject = string.Empty;           //需求主题
            string strDemandTypeID = "0";                 //需求类别ID

            string strRegUser = string.Empty;         //登记人

            #endregion

            #region Step 3. 提取需求单号, 从快速查询条件XML串中.

            XmlTextReader tr = new XmlTextReader(new System.IO.StringReader(strXmlcond));
            while (tr.Read())
            {
                if (tr.Name == "Field" && tr.NodeType == XmlNodeType.Element)
                {
                    strTmp = tr.GetAttribute("Value").Trim();

                    switch (tr.GetAttribute("FieldName"))
                    {
                        case "DemandNo":           //问题单号
                            strDemandNo = strTmp;
                            break;
                    }
                }
            }
            tr.Close();

            #endregion

            #region Step 4. 拼接查询条件

            if (strDemandNo != string.Empty)
            {
                strWhere += " AND DemandNo like " + StringTool.SqlQ("%" + strDemandNo.Trim() + "%");    //    需求单号
            }

            #endregion

            #region Step 5. 验证用户权限

            strWhere = strWhere + VerifyUserRight(re, lngOrgID, lngDeptID, lngUserID);

            #endregion

            #region Step 6. 执行SQL语句

            DataTable dt = null;
            OracleConnection cn = ConfigTool.GetConnection();
            try
            {
                dt = OracleDbHelper.ExecuteDataTable(cn,
                    "v_reqdemand",
                    "*",
                    " ORDER BY ID DESC",
                    intPageSize,
                    intPageIndex,
                    strWhere,
                    ref intRowCount);
            }
            finally
            {
                ConfigTool.CloseConnection(cn);
            }

            #endregion

            return dt;
        }

        #endregion

        #region 查询正在处理, 发生时间为近一个月的需求单 - 2013-04-27 @孙绍棕

        /// <summary>
        /// 查询正在处理, 发生时间为近一个月的需求单
        /// </summary>        
        /// <param name="lngUserID">用户编号</param>
        /// <param name="lngDeptID">部门编号</param>
        /// <param name="lngOrgID">机构编号</param>
        /// <param name="re">权限对象</param>
        /// <param name="intPageSize">页长</param>
        /// <param name="intPageIndex">页码</param>
        /// <param name="intRowCount">总记录数</param>
        /// <returns>正在处理, 发生时间为近一个月的需求单列表</returns>
        public DataTable FetchListForRecentMonth(
            long lngUserID,
            long lngDeptID,
            long lngOrgID,
            RightEntity re,
            int intPageSize,
            int intPageIndex,
            ref int intRowCount)
        {
            #region Step 1. 定义基本变量

            string strWhere = " 1=1 ";

            #endregion

            #region Step 2. 设置最近一个月的SQL-Where条件

            strWhere += " and Status = 20 and RegTime >= to_date(" + StringTool.SqlQ(DateTime.Now.AddMonths(-1).ToShortDateString()) + ",'yyyy-MM-dd')";

            #endregion

            #region Step 3.  验证用户权限

            strWhere = strWhere + VerifyUserRight(re, lngOrgID, lngDeptID, lngUserID);

            #endregion

            #region Step 4.  执行SQL语句

            DataTable dt = null;
            OracleConnection cn = ConfigTool.GetConnection();

            try
            {
                dt = OracleDbHelper.ExecuteDataTable(cn, "v_reqdemand",
                    "*",
                    " ORDER BY ID DESC",
                    intPageSize,
                    intPageIndex,
                    strWhere,
                    ref intRowCount);
            }
            finally
            {
                ConfigTool.CloseConnection(cn);
            }

            #endregion

            return dt;
        }

        #endregion


        #region 取需求的年份 - 2013-05-03 @孙绍棕

        /// <summary>
        /// 取需求的年份
        /// </summary>
        /// <returns></returns>
        public DataTable GetReqDemandYears()
        {
            string sSql = "select distinct datepart('year',regtime) as years from req_demand order by datepart('year',regtime) desc";

            OracleConnection cn = ConfigTool.GetConnection();
            try
            {
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, sSql);
                return dt;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                ConfigTool.CloseConnection(cn);
            }
        }


        #endregion

        #region 获取需求涉及的管理处 - 2013-05-03 @孙绍棕

        /// <summary>
        /// 获取需求涉及的管理处
        /// </summary>
        /// <returns></returns>
        public DataTable GetManageOffices()
        {
            string sSql = "SELECT deptid,deptname FROM ts_dept WHERE deptid in (select distinct orgid from req_demand)";

            OracleConnection cn = ConfigTool.GetConnection();
            try
            {
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, sSql);
                return dt;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                ConfigTool.CloseConnection(cn);
            }
        }

        #endregion


        #region 全年度需求发生趋势分析 - 2013-05-03 @孙绍棕

        /// <summary>
        /// 全年度需求发生趋势分析
        /// </summary>
        /// <param name="nYear"></param>
        /// <param name="lngServiceTypeID"></param>
        /// <param name="lngDeptID"></param>
        /// <returns></returns>
        public DataTable GetAnalysisDirection(int nYear, long lngServiceTypeID, long lngDeptID, long lngMastCustomer)
        {
            
            long lngCatalogID = lngServiceTypeID;
            string sFullID = string.Empty;
            if (lngServiceTypeID == -1 || lngServiceTypeID == 0)
                lngCatalogID = 1003;          

            DataTable dt = null;



            OracleConnection cn = ConfigTool.GetConnection();
            if (cn.State != ConnectionState.Open)
            {
                cn.Open();
            }

            try
            {
                OracleParameter[] parms = {
                      new OracleParameter("nYear",OracleType.Number,4),
                      new OracleParameter("lngServiceTypeID",OracleType.Number,4),
                      new OracleParameter("lngDeptID",OracleType.Number,4),
                      new OracleParameter("lngMastCustomer",OracleType.Number,4),
                      new OracleParameter("p_cursor",OracleType.Cursor)
                };

                parms[0].Direction = ParameterDirection.Input;
                parms[1].Direction = ParameterDirection.Input;
                parms[2].Direction = ParameterDirection.Input;
                parms[3].Direction = ParameterDirection.Input;
                parms[4].Direction = ParameterDirection.Output;

                parms[0].Value = nYear;
                parms[1].Value = lngCatalogID;
                parms[2].Value = lngDeptID;
                parms[3].Value = lngMastCustomer;

                //DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, sSql);

                dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.StoredProcedure, "PROC_RPT_RD_AnnualDataAnalysis", parms);

                return dt;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                ConfigTool.CloseConnection(cn);
            }
        }

        #endregion

        #region 验证用户查询权限 - 2013-04-25 @孙绍棕


        /// <summary>
        /// 验证用户查询权限
        /// </summary>
        /// <param name="re">权限对象</param>
        /// <param name="lngOrgID">机构编号</param>
        /// <param name="lngDeptID">部门编号</param>
        /// <param name="lngUserID">用户编号</param>
        /// <returns>SQL-Where条件语句</returns>
        private String VerifyUserRight(RightEntity re,
            long lngOrgID,
            long lngDeptID,
            long lngUserID)
        {
            String strWhere = String.Empty;    // SQL-Where条件脚本
            String strList = String.Empty;    // 权限需要用到


            if (re == null || re.CanRead == false)
            {
                //查询出空结果
                strWhere += " AND flowid = -1 ";
            }
            else if (re != null && re.CanRead == true)
            {
                #region 范围条件
                switch (re.RightRange)
                {
                    case eO_RightRange.eFull:
                        strWhere += "";
                        break;
                    case eO_RightRange.ePersonal:
                        strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = Req_Demand.flowid AND receiverid = " + lngUserID.ToString() + ")";
                        break;
                    case eO_RightRange.eDeptDirect:
                        strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = Req_Demand.flowid AND recdeptid = " + lngDeptID.ToString() + ")";
                        break;
                    case eO_RightRange.eOrgDirect:
                        strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = Req_Demand.flowid AND recorgid = " + lngOrgID.ToString() + ")";
                        break;
                    case eO_RightRange.eDept:
                        strList = DeptDP.GetDeptFullID(lngDeptID);
                        if (strList.Trim().Length > 0)
                        {
                            //不是根部门并有找到

                            strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = Req_Demand.flowid AND recdeptid in (select deptid from ts_dept where fullid like " + StringTool.SqlQ(strList + "%") + "))";
                        }
                        break;
                    case eO_RightRange.eOrg:
                        strList = DeptDP.GetDeptFullID(lngOrgID);
                        if (strList.Trim().Length > 0)
                        {
                            //不是根部门并有找到

                            strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = Req_Demand.flowid AND recorgid in (select distinct deptid from ts_dept where deptkind = 1 and fullid like " + StringTool.SqlQ(strList + "%") + "))";
                        }
                        break;
                    default:
                        strWhere += "";
                        break;
                }
                #endregion
            }

            return strWhere;

        }

        #endregion

        #region tool method

        /// <summary>
        /// 封装日期字符串

        /// </summary>
        /// <param name="fieldValue">字段键和值</param>
        /// <returns>"null" or "to_date('xxxx-xx-xx xx:xx:xx', 'yyyy-MM-dd HH24:mi:ss')" </returns>
        private String WrapDateString(FieldValue fieldValue)
        {
            if (String.IsNullOrEmpty(fieldValue.Value))
            {
                return "null";
            }

            return String.Format("to_date({0}, 'yyyy-MM-dd HH24:mi:ss')", StringTool.SqlQ(fieldValue.Value));
        }

        #endregion
    }
}
