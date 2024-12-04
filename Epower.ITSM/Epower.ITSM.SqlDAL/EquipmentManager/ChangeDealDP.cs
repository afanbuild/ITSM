/****************************************************************************
 * 
 * description:变更管理数据查询类
 * 
 * 
 * 
 * Create by:zhumingchun
 * Create Date:2008-04-04
 * *************************************************************************/
using System;
using System.Data;
using System.Data.OracleClient;
using Epower.DevBase.BaseTools;
using Epower.DevBase.Organization.Base;
using Epower.DevBase.Organization.SqlDAL;
using Epower.ITSM.Base;
using EpowerGlobal;
using System.Xml;
using System.IO;
using System.Text;

namespace Epower.ITSM.SqlDAL
{
    /// <summary>
    /// ChangeDealDP
    /// </summary>
    public class ChangeDealDP
    {
        /// <summary>
        /// 根据条件获取变更数据表数据
        /// </summary>
        /// <param name="pWhere"></param>
        /// <returns></returns>
        public static DataTable GetChangeDealData(long lngUserID, long lngDeptID, long lngOrgID, RightEntity re, string pWhere)
        {
            string strList = "";       //存放部门列表
            string strSQL = @"select a.*,case when nvl(IS_PLAN_CHANGE,0)=1 then '是' else '否' end as isplanchange,
case when nvl(IS_BUS_EFFECT,0)=1 then '是' else '否' end as isbuseffect,
case when nvl(IS_DATA_EFFECT,0)=1 then '是' else '否' end as isdataeffect,
case when nvl(IS_STOP_SERVER,0)=1 then '是' else '否' end as isstopserver,
                            b.status,
					        case when b.status=30 then 
					        datediff('Minute',nvl(b.endtime,sysdate),nvl(b.expectendtime,sysdate))
					        else 
					        datediff('Minute',sysdate,nvl(b.expectendtime,sysdate)) end FlowDiffMinute
                            from Equ_ChangeService a,es_flow b 
                            WHERE a.FlowID = b.FlowID  ";

            if (re == null || re.CanRead == false)
            {
                //查询出空结果
                strSQL += " AND a.flowid = -1 ";
            }
            else
            {
                strSQL += pWhere;
                #region 范围条件
                if (re != null)
                {
                    switch (re.RightRange)
                    {
                        case eO_RightRange.eFull:
                            strSQL += "";
                            break;
                        case eO_RightRange.ePersonal:
                            strSQL += "AND exists (SELECT messageid FROM es_message WHERE flowid = b.flowid AND receiverid = " + lngUserID.ToString() + ")";
                            break;
                        case eO_RightRange.eDeptDirect:
                            strSQL += "AND exists (SELECT messageid FROM es_message WHERE flowid = b.flowid AND recdeptid = " + lngDeptID.ToString() + ")";
                            break;
                        case eO_RightRange.eOrgDirect:
                            strSQL += "AND exists (SELECT messageid FROM es_message WHERE flowid = b.flowid AND recorgid = " + lngOrgID.ToString() + ")";
                            break;
                        case eO_RightRange.eDept:
                            strList = DeptDP.GetDeptFullID(lngDeptID);
                            if (strList.Trim().Length > 0)
                            {
                                //不是根部门并有找到
                                strSQL += "AND exists (SELECT messageid FROM es_message WHERE flowid = b.flowid AND recdeptid in (select deptid from ts_dept where fullid like " + StringTool.SqlQ(strList + "%") + "))";
                            }
                            break;
                        case eO_RightRange.eOrg:
                            strList = DeptDP.GetDeptFullID(lngOrgID);
                            if (strList.Trim().Length > 0)
                            {
                                //不是根部门并有找到
                                strSQL += "AND exists (SELECT messageid FROM es_message WHERE flowid = b.flowid AND recorgid in (select distinct deptid from ts_dept where deptkind = 1 and fullid like " + StringTool.SqlQ(strList + "%") + "))";
                            }
                            break;
                        default:
                            strSQL += "";
                            break;
                    }
                }
                #endregion
                strSQL = strSQL + " ORDER BY a.ID DESC";
            }
            OracleConnection cn = ConfigTool.GetConnection();
            try
            {
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
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


        /// <summary>
        /// 根据条件获取变更数据表数据[根据客户信息查询的情况]
        /// </summary>
        /// <param name="lngUserID"></param>
        /// <param name="lngDeptID"></param>
        /// <param name="lngOrgID"></param>
        /// <param name="re"></param>
        /// <param name="pWhere"></param>
        /// <returns></returns>
        public static DataTable GetChangeDealDataForCustInfo(long lngUserID, long lngDeptID, long lngOrgID, RightEntity re, string pWhere)
        {
            string strList = "";       //存放部门列表
            string strSQL = @"select a.*,case when nvl(IS_PLAN_CHANGE,0)=1 then '是' else '否' end as isplanchange,
case when nvl(IS_BUS_EFFECT,0)=1 then '是' else '否' end as isbuseffect,
case when nvl(IS_DATA_EFFECT,0)=1 then '是' else '否' end as isdataeffect,
case when nvl(IS_STOP_SERVER,0)=1 then '是' else '否' end as isstopserver,
                            b.status,
					        case when b.status=30 then 
					        datediff('Minute',nvl(b.endtime,sysdate),nvl(b.expectendtime,sysdate))
					        else 
					        datediff('Minute',sysdate,nvl(b.expectendtime,sysdate)) end FlowDiffMinute,
                            d.MastCustName
                            from es_flow b,Equ_ChangeService a  left join br_ecustomer d on a.custid = d.id 
                            WHERE a.FlowID = b.FlowID  ";

            if (re == null || re.CanRead == false)
            {
                //查询出空结果
                strSQL += " AND a.flowid = -1 ";
            }
            else
            {
                strSQL += pWhere;
                #region 范围条件
                if (re != null)
                {
                    switch (re.RightRange)
                    {
                        case eO_RightRange.eFull:
                            strSQL += "";
                            break;
                        case eO_RightRange.ePersonal:
                            strSQL += "AND exists (SELECT messageid FROM es_message WHERE flowid = b.flowid AND receiverid = " + lngUserID.ToString() + ")";
                            break;
                        case eO_RightRange.eDeptDirect:
                            strSQL += "AND exists (SELECT messageid FROM es_message WHERE flowid = b.flowid AND recdeptid = " + lngDeptID.ToString() + ")";
                            break;
                        case eO_RightRange.eOrgDirect:
                            strSQL += "AND exists (SELECT messageid FROM es_message WHERE flowid = b.flowid AND recorgid = " + lngOrgID.ToString() + ")";
                            break;
                        case eO_RightRange.eDept:
                            strList = DeptDP.GetDeptFullID(lngDeptID);
                            if (strList.Trim().Length > 0)
                            {
                                //不是根部门并有找到
                                strSQL += "AND exists (SELECT messageid FROM es_message WHERE flowid = b.flowid AND recdeptid in (select deptid from ts_dept where fullid like " + StringTool.SqlQ(strList + "%") + "))";
                            }
                            break;
                        case eO_RightRange.eOrg:
                            strList = DeptDP.GetDeptFullID(lngOrgID);
                            if (strList.Trim().Length > 0)
                            {
                                //不是根部门并有找到
                                strSQL += "AND exists (SELECT messageid FROM es_message WHERE flowid = b.flowid AND recorgid in (select distinct deptid from ts_dept where deptkind = 1 and fullid like " + StringTool.SqlQ(strList + "%") + "))";
                            }
                            break;
                        default:
                            strSQL += "";
                            break;
                    }
                }
                #endregion
                strSQL = strSQL + " ORDER BY a.ID DESC";
            }
            OracleConnection cn = ConfigTool.GetConnection();
            try
            {
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
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

        public static DataTable getEquipment(string sWhere)
        {
            string SQLstr = "";
            if (sWhere == "")
            {
                SQLstr = "SELECT distinct ChangeID FROM EQU_CHANGESERVICEDETAILS a LEFT JOIN EQU_DESK b ON a.EQUID=b.id";
            }
            else
            {
                SQLstr = "SELECT distinct ChangeID FROM EQU_CHANGESERVICEDETAILS a LEFT JOIN EQU_DESK b ON a.EQUID=b.id  where " + sWhere;
            }
            OracleConnection cn = new OracleConnection(System.Configuration.ConfigurationSettings.AppSettings["SQLConnString"]);
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, SQLstr);
            if (cn.State == ConnectionState.Open)
            {
                cn.Close();
            }
            return dt;
        }

        public static DataTable getCST_ISSUE_LISTFASTQUERY(string FormId, string LoginName, string Name)
        {
            Name = Name == "==选择收藏查询条件==" ? "Temp1" : Name;
            string SQLstr = "";
            if (Name.Trim() == string.Empty)
            {
                SQLstr = @"SELECT  ID,Name,FORMID,LOGINNAME,SQLWhere,SQLText,DISPLAYCOLUMN 
                            FROM CST_ISSUE_QUERYSave
                            WHERE  nvl(SQLWhere,' ')!='Temp1' AND  FORMID=" + StringTool.SqlQ(FormId) + " AND LOGINNAME=" + StringTool.SqlQ(LoginName) + " order by SN desc";
            }
            else
            {
                if (Name == "Temp1")
                {
                    SQLstr = @"SELECT  ID,Name,FORMID,LOGINNAME,SQLWhere,SQLText ,DISPLAYCOLUMN 
                            FROM CST_ISSUE_QUERYSave
                            WHERE  SQLWhere='Temp1' AND  FORMID=" + StringTool.SqlQ(FormId) + " AND LOGINNAME=" + StringTool.SqlQ(LoginName) + "  AND Name=" + StringTool.SqlQ(Name);
                }
                else
                {
                    SQLstr = @"SELECT  ID,Name,FORMID,LOGINNAME,SQLWhere,SQLText ,DISPLAYCOLUMN 
                            FROM CST_ISSUE_QUERYSave
                            WHERE  nvl(SQLWhere,' ')!='Temp1' AND  FORMID=" + StringTool.SqlQ(FormId) + " AND LOGINNAME=" + StringTool.SqlQ(LoginName) + "  AND Name=" + StringTool.SqlQ(Name);
                }
            }

            OracleConnection cn = new OracleConnection(System.Configuration.ConfigurationSettings.AppSettings["SQLConnString"]);
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, SQLstr);
            if (cn.State == ConnectionState.Open)
            {
                cn.Close();
            }
            return dt;
        }

        public static DataTable getCST_ISSUE_Where(string FormId, string LoginName, string Name)
        {
            string SQLstr = "";
            SQLstr = @"SELECT  ID,Name,FORMID,LOGINNAME,SQLWhere,SQLText ,DISPLAYCOLUMN 
                        FROM CST_ISSUE_QUERYSave
                        WHERE FORMID=" + StringTool.SqlQ(FormId) + " AND LOGINNAME=" + StringTool.SqlQ(LoginName) + "  AND (SQLWhere='Temp1' or Name in (" + StringTool.SqlQ(Name) + "))";

            OracleConnection cn = new OracleConnection(System.Configuration.ConfigurationSettings.AppSettings["SQLConnString"]);
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, SQLstr);
            if (cn.State == ConnectionState.Open)
            {
                cn.Close();
            }
            return dt;
        }

        public static void updateCST_ISSUE_WhereNums(string sloginame, string sname)
        {
            string SQLstr = "update CST_ISSUE_QUERYSave set SN=nvl(SN,0)+1 where FORMID='frm_ChangeQuery' AND  nvl(SQLWhere,' ')!='Temp1' AND  LOGINNAME=" + StringTool.SqlQ(sloginame) + " and Name=" + StringTool.SqlQ(sname);
            OracleConnection cn = new OracleConnection(System.Configuration.ConfigurationSettings.AppSettings["SQLConnString"]);
            int i = OracleDbHelper.ExecuteNonQuery(cn, CommandType.Text, SQLstr);
            if (cn.State == ConnectionState.Open)
            {
                cn.Close();
            }
        }

        public static void deleteCST_ISSUE_Where(string sloginame, string sname)
        {
            string SQLstr = "delete from  CST_ISSUE_QUERYSave  where FORMID='frm_ChangeQuery'  and  nvl(SQLWhere,' ')!='Temp1' and  LOGINNAME=" + StringTool.SqlQ(sloginame) + " and Name=" + StringTool.SqlQ(sname);
            OracleConnection cn = new OracleConnection(System.Configuration.ConfigurationSettings.AppSettings["SQLConnString"]);
            int i = OracleDbHelper.ExecuteNonQuery(cn, CommandType.Text, SQLstr);
            if (cn.State == ConnectionState.Open)
            {
                cn.Close();
            }
        }

        public static DataTable GetChangeDealDataForCustInfo(long lngUserID, long lngDeptID, long lngOrgID, RightEntity re, string pWhere, int pagesize, int pageindex, ref int rowcount)
        {
            string strList = "";       //存放部门列表
            string strWhere = "1=1";

            if (re == null || re.CanRead == false)
            {
                //查询出空结果
                strWhere += " AND flowid = -1 ";
            }
            else
            {
                #region 范围条件
                if (re != null)
                {
                    switch (re.RightRange)
                    {
                        case eO_RightRange.eFull:
                            strWhere += "";
                            break;
                        case eO_RightRange.ePersonal:
                            strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_Equ_ChangeService.flowid AND receiverid = " + lngUserID.ToString() + ")";
                            break;
                        case eO_RightRange.eDeptDirect:
                            strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_Equ_ChangeService.flowid AND recdeptid = " + lngDeptID.ToString() + ")";
                            break;
                        case eO_RightRange.eOrgDirect:
                            strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_Equ_ChangeService.flowid AND recorgid = " + lngOrgID.ToString() + ")";
                            break;
                        case eO_RightRange.eDept:
                            strList = DeptDP.GetDeptFullID(lngDeptID);
                            if (strList.Trim().Length > 0)
                            {
                                //不是根部门并有找到
                                strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_Equ_ChangeService.flowid AND recdeptid in (select deptid from ts_dept where fullid like " + StringTool.SqlQ(strList + "%") + "))";
                            }
                            break;
                        case eO_RightRange.eOrg:
                            strList = DeptDP.GetDeptFullID(lngOrgID);
                            if (strList.Trim().Length > 0)
                            {
                                //不是根部门并有找到
                                strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_Equ_ChangeService.flowid AND recorgid in (select distinct deptid from ts_dept where deptkind = 1 and fullid like " + StringTool.SqlQ(strList + "%") + "))";
                            }
                            break;
                        default:
                            strWhere += "";
                            break;
                    }
                }
                #endregion
                strWhere += pWhere;
            }
            OracleConnection cn = ConfigTool.GetConnection();
            try
            {
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, "V_Equ_ChangeService", "*", " ORDER BY ID DESC", pagesize, pageindex, strWhere, ref rowcount);
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


        public static DataTable GetChangeDealDataForCustInfo1(long lngUserID, long lngDeptID, long lngOrgID, RightEntity re, string pWhere)
        {
            string strList = "";       //存放部门列表
            string strWhere = "1=1";
            string strSQL = "";
            if (re == null || re.CanRead == false)
            {
                //查询出空结果
                strWhere += " AND flowid = -1 ";
            }
            else
            {
                #region 范围条件
                if (re != null)
                {
                    switch (re.RightRange)
                    {
                        case eO_RightRange.eFull:
                            strWhere += "";
                            break;
                        case eO_RightRange.ePersonal:
                            strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_Equ_ChangeService.flowid AND receiverid = " + lngUserID.ToString() + ")";
                            break;
                        case eO_RightRange.eDeptDirect:
                            strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_Equ_ChangeService.flowid AND recdeptid = " + lngDeptID.ToString() + ")";
                            break;
                        case eO_RightRange.eOrgDirect:
                            strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_Equ_ChangeService.flowid AND recorgid = " + lngOrgID.ToString() + ")";
                            break;
                        case eO_RightRange.eDept:
                            strList = DeptDP.GetDeptFullID(lngDeptID);
                            if (strList.Trim().Length > 0)
                            {
                                //不是根部门并有找到
                                strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_Equ_ChangeService.flowid AND recdeptid in (select deptid from ts_dept where fullid like " + StringTool.SqlQ(strList + "%") + "))";
                            }
                            break;
                        case eO_RightRange.eOrg:
                            strList = DeptDP.GetDeptFullID(lngOrgID);
                            if (strList.Trim().Length > 0)
                            {
                                //不是根部门并有找到
                                strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_Equ_ChangeService.flowid AND recorgid in (select distinct deptid from ts_dept where deptkind = 1 and fullid like " + StringTool.SqlQ(strList + "%") + "))";
                            }
                            break;
                        default:
                            strWhere += "";
                            break;
                    }
                }
                #endregion
                strWhere += pWhere;
            }
            OracleConnection cn = ConfigTool.GetConnection();
            try
            {
                strSQL += "SELECT * FROM V_Equ_ChangeService WHERE " + strWhere;
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lngFlowID"></param>
        /// <returns></returns>
        public static string GetPersonList(long lngFlowID)
        {
            string sReturn = string.Empty;
            string strSQL = @"select Distinct TActors from es_message where FlowID=" + lngFlowID.ToString() + " and TActors is not null";

            OracleConnection cn = ConfigTool.GetConnection();
            try
            {
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
                foreach (DataRow dr in dt.Rows)
                {
                    sReturn += dr["TActors"].ToString() + ",";
                }
                return sReturn;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lngFlowID"></param>
        /// <returns></returns>
        public static bool CheckIsChangeProblem(long lngServiceFlowID)
        {
            string strSQL = "";
            strSQL = "SELECT  IssuesFlowID " +
                " FROM Equ_ChangeService " +
                " WHERE nvl(IssuesFlowID,0)=" + lngServiceFlowID.ToString();
            OracleConnection cn = ConfigTool.GetConnection();
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigTool.CloseConnection(cn);
            if (dt.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #region 判断问题是否已关联变更
        /// <summary>
        /// 判断问题是否已关联变更
        /// </summary>
        /// <param name="lngProblemFlowID"></param>
        /// <returns></returns>
        public static bool CheckIsChangeFromPro(long lngProblemFlowID)
        {
            string strSQL = "";
            strSQL = "SELECT ProblemFlowID " +
                " FROM Equ_ChangeService " +
                " WHERE nvl(ProblemFlowID,0)=" + lngProblemFlowID.ToString();
            OracleConnection cn = ConfigTool.GetConnection();
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigTool.CloseConnection(cn);
            if (dt.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        /// <summary>
        /// 获取与事件相关的变更
        /// </summary>
        /// <param name="lngFlowID"></param>
        /// <returns></returns>
        public static DataTable GetIssuesForChange(long lngFlowID)
        {
            string strSQL = "";
            strSQL = " SELECT nvl(a.BuildCode, '') || nvl(a.ServiceNo, '') AS ServiceNo,a.ServiceType,a.Subject,a.DealStatus,b.status,c.appname,a.flowid " +
                     " FROM Cst_Issues a, es_flow b,es_app c " +
                     " WHERE a.FlowID = b.flowid AND b.appID = c.appID  " +
                     " AND a.AssociateFlowID = " + lngFlowID.ToString() + " ORDER BY a.flowid DESC";

            OracleConnection cn = ConfigTool.GetConnection();
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigTool.CloseConnection(cn);
            return dt;
        }

        #region 获取与变更相关的问题
        /// <summary>
        /// 获取与变更相关的问题
        /// </summary>
        /// <param name="lngFlowID"></param>
        /// <returns></returns>
        public static DataTable GetProblemsByChange(long lngFlowID)
        {
            string strSQL = "";
            strSQL = "SELECT nvl(a.BuildCode, '') || nvl(a.ProblemNo, '') AS ServiceNo,a.Problem_TypeName,a.Problem_Title,a.StateName,a.flowid,b.status,c.appname " +
                     " FROM Pro_ProblemDeal a, es_flow b,es_app c " +
                     " WHERE a.FlowID = b.flowid AND b.appID = c.appID  " +
                     " AND a.AssociateFlowID = " + lngFlowID.ToString() + " ORDER BY a.flowid DESC";

            OracleConnection cn = ConfigTool.GetConnection();
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigTool.CloseConnection(cn);
            return dt;
        }
        #endregion

        /// <summary>
        /// 事件升级问题
        /// </summary>
        /// <param name="lngFlowID">事件流程ID</param>
        /// <returns></returns>
        public static DataTable GetChangeData(long lngFlowID)
        {
            string strSQL = @"select a.*,
                            b.status,
					        case when b.status=30 then 
					        datediff('Minute',nvl(b.endtime,sysdate),nvl(b.expectendtime,sysdate))
					        else 
					        datediff('Minute',sysdate,nvl(b.expectendtime,sysdate)) end FlowDiffMinute
                            from Equ_ChangeService a,es_flow b
                            WHERE a.FlowID = b.FlowID   ";
            strSQL += " And a.FlowID=" + lngFlowID.ToString();

            OracleConnection cn = ConfigTool.GetConnection();
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigTool.CloseConnection(cn);
            return dt;
        }

        /// <summary>
        /// 根据条件获取变更数据表数据
        /// </summary>
        /// <param name="pWhere"></param>
        /// <returns></returns>
        public static DataTable GetChangeDealDataJQuery(long lngID)
        {
            string strSQL = @"select a.*,
                            b.status
                            from Equ_ChangeService a,es_flow b 
                            WHERE a.FlowID = b.FlowID AND a.ID=" + lngID.ToString();
            OracleConnection cn = ConfigTool.GetConnection();
            try
            {
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
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


        #region 根据资产获取变更单号

        /// <summary>
        /// 根据资产获取变更单号
        /// </summary>
        /// <param name="changId">变更ID</param>
        /// <param name="equId">资产ID</param>
        /// <returns>变更单号</returns>
        public string GetChangeNo(long changId, long equId)
        {
            string changNo = string.Empty, strSQL = string.Empty;//变更单号
            strSQL = "SELECT B.CHANGENO FROM EQU_CHANGESERVICE B WHERE B.ID IN (SELECT A.CHANGEID FROM EQU_CHANGESERVICEDETAILS A WHERE A.CHANGEID<>" + changId.ToString() + " AND A.CHANGESTATUS=0 AND A.EQUID=" + equId.ToString() + ") ";
            OracleConnection cn = ConfigTool.GetConnection("SQLConnString");
            try
            {
                OracleDataReader dr = OracleDbHelper.ExecuteReader(cn, CommandType.Text, strSQL);
                if (dr.Read())
                {
                    changNo = dr["CHANGENO"].ToString();//变更单号赋值
                }
                dr.Close();
                return changNo;
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
      

        #region 根据问题单FlowID获取问题单详情
        /// <summary>
        /// 根据问题单FlowID获取问题单详情
        /// </summary>
        /// <param name="lngFlowID"></param>
        /// <returns></returns>
        public static DataTable GetProblemsByFlowID(long lngFlowID)
        {
            string strSql = @"select A.*,B.partBranchName as DEPT,B.Code as EQUCODE,b.catalogid,b.catalogname,
                                     '' as CHANGECONTENT,'' as OLDVALUE,'' as NEWVALUE,'' as Remark
                                from 
	                                (
	                                select ListID,ListName,EQUID,EQUNAME from Pro_ProblemDeal where FlowID = " + lngFlowID + @"
	                                ) A
                           left join  Equ_Desk B
                                  on A.EQUID = B.ID";

            DataTable dt = CommonDP.ExcuteSqlTable(strSql);

            return dt;
        }
        #endregion

        #region 变更明细相关

        /// <summary>
        /// 变更明细相关
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static DataTable GetCLFareItem(long id)
        {
            string sSql = @"SELECT a.*,b.PARTBRANCHNAME as DEPT FROM EQU_CHANGESERVICEDETAILS a LEFT JOIN EQU_DESK b ON a.EQUID=b.id WHERE ChangeID = " + id.ToString();

            OracleConnection cn = ConfigTool.GetConnection("SQLConnString");
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

        public static DataTable GetCLFareItem(OracleTransaction trans, long id)
        {
            string sSql = @"SELECT * FROM EQU_CHANGESERVICEDETAILS WHERE ChangeID = " + id.ToString();

            OracleConnection cn = ConfigTool.GetConnection("SQLConnString");
            try
            {
                DataTable dt = OracleDbHelper.ExecuteDataset(trans, CommandType.Text, sSql).Tables[0];
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


        /// <summary>
        /// 得到变更明细表
        /// </summary>
        /// <param name="flowid"></param>
        /// <returns></returns>
        public static DataTable GetStatusID(OracleTransaction trans, long flowid)
        {
            OracleConnection cn = ConfigTool.GetConnection("SQLConnString");
            string sSql = "SELECT * FROM EQU_CHANGESERVICEDETAILS Where ChangeID=" + flowid.ToString();
            DataTable dt = null;
            try
            {
                dt = OracleDbHelper.ExecuteDataset(trans, CommandType.Text, sSql).Tables[0];
            }
            catch (Exception e)
            {
                throw e;
            }
            return dt;
        }

        /// <summary>
        /// 保存资产XML串
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="lngID"></param>
        /// <returns></returns>
        public static void SaveCLFareDetailItem(DataTable dt, string deskChange)
        {

            try
            {
                XmlDocument xmlDoc = new XmlDocument();//创建XML对象
                XmlElement xmlEle = xmlDoc.CreateElement("XmlDesk");//创建根节点
                foreach (DataRow row in dt.Rows)
                {
                    #region 创建子节点

                    XmlElement xmlEleSub = xmlDoc.CreateElement("DeskInfo");

                    #endregion

                    #region 赋值属性值

                    xmlEleSub.SetAttribute("DETAILSID", "0");
                    xmlEleSub.SetAttribute("EQUID", row["EQUID"].ToString());//资产ID
                    xmlEleSub.SetAttribute("EQUNAME", row["EQUNAME"].ToString());//资产名称
                    xmlEleSub.SetAttribute("LISTID", row["LISTID"].ToString());//资产目录ID
                    xmlEleSub.SetAttribute("LISTNAME", row["LISTNAME"].ToString());//资产目录名称
                    xmlEleSub.SetAttribute("EQUCODE", row["EQUCODE"].ToString());//资产编号
                    xmlEleSub.SetAttribute("CHANGECONTENT", row["CHANGECONTENT"].ToString());//变更内容
                    xmlEleSub.SetAttribute("OLDVALUE", row["OLDVALUE"].ToString());//变更前值
                    xmlEleSub.SetAttribute("NEWVALUE", row["NEWVALUE"].ToString());//变更新值
                    xmlEleSub.SetAttribute("CHANGEDATE", row["CHANGEDATE"].ToString());//变更日期
                    xmlEleSub.SetAttribute("CHANGEUSERID", row["CHANGEUSERID"].ToString());//变更用户ID
                    xmlEleSub.SetAttribute("CHANGEUSERNAME", row["CHANGEUSERNAME"].ToString());//变更用户名
                    xmlEleSub.SetAttribute("CHANGEDEPTID", row["CHANGEDEPTID"].ToString());//变更用户部门ID
                    xmlEleSub.SetAttribute("CHANGEDEPTNAME", row["CHANGEDEPTNAME"].ToString());//变更用户部门名称
                    xmlEleSub.SetAttribute("CHANGESTATUS", row["CHANGESTATUS"].ToString());//变更状态
                    xmlEleSub.SetAttribute("REMARK", row["Remark"].ToString());//变更备注
                    xmlEleSub.SetAttribute("DEPT", row["CHANGEDEPTNAME"].ToString());//变更机构

                    #endregion

                    #region 添加子节点

                    xmlEle.AppendChild(xmlEleSub);//添加子节点

                    #endregion
                }
                xmlDoc.AppendChild(xmlEle);
                xmlDoc.Save(deskChange);
            }
            catch
            {
                throw;
            }
            finally
            {

            }
        }



        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="strNo"></param>
        /// <param name="tran"></param>
        public static void DeleteCLFareDetailItem(long lngID, OracleTransaction tran)
        {

            string sSql = "Delete EQU_CHANGESERVICEDETAILS Where CHANGEID=" + lngID.ToString();
            try
            {
                OracleDbHelper.ExecuteNonQuery(tran, CommandType.Text, sSql);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="costID"></param>
        /// <returns></returns>
        public static bool DeleteCLFareDetailItem(long costID)
        {
            if (costID == 0)
                return true;
            bool result = true;
            string sSql = "Delete EQU_CHANGESERVICEDETAILS Where ID=" + costID.ToString();
            try
            {
                OracleDbHelper.ExecuteNonQuery(ConfigTool.GetConnection(), CommandType.Text, sSql);
            }
            catch
            {
                result = false;
            }
            return result;
        }

        #endregion

        #region 根据FlowID获取变更临时信息

        /// <summary>
        /// GetTemporaryChangeByFlowId
        /// </summary>
        /// <param name="lngChangeId">变更ID</param>
        /// <param name="lngEquId">资产ID</param>
        /// <returns></returns>
        public DataTable GetTemporaryChangeByChangeId(long lngChangeId, long lngEquId)
        {
            DataTable dt = null;
            StringBuilder sb = new StringBuilder();

            #region 构建语句

            sb.Append("select ChangeNO from Equ_Changeservice where id in (SELECT a.change_id FROM EQU_DESK a ");
            sb.Append("WHERE a.id=" + lngEquId + " AND a.change_Id<>" + lngChangeId +")");//查询语句

            #endregion

            OracleConnection cn = ConfigTool.GetConnection("SQLConnString");//获取连接

            try
            {
                dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, sb.ToString());//获取数据
            }
            catch
            {
                //添加日志
                throw;
            }
            finally
            {
                //关闭连接
                ConfigTool.CloseConnection(cn);
            }
            return dt;
        }

        #endregion

        #region 获取变更相关服务监控图表
        /// <summary>
        /// 获取变更相关服务监控图表
        /// </summary>
        /// <param name="strMastCust"></param>
        /// <returns></returns>
        public static DataTable GetSM(string strMastCust)
        {
            string strWhere = string.Empty;

            if (strMastCust == "")
            {
                strWhere = "";
            }
            else
            {
                strWhere = " and MastCustID = " + StringTool.SqlQ(strMastCust);
            }

            string strSql = @"--需处理总数
                                select '需处理总数' as Title,count(1) as counts,'ChangeW' as Types
                                  from Equ_ChangeService A,es_Flow B,Br_ECustomer C
                                where A.FlowID = B.FlowID and A.CustID = C.ID and C.deleted = 0
	                                    and (
                                         (B.status = 30 and to_char(EndTime,'yyyy-MM') = to_char(sysdate,'yyyy-MM'))
                                         or B.status = 20)" + strWhere + @"
                                union all
                                --当月新增【变更单的登记时间为当月，除去客户为空和暂存未提交的】
                                select '当月新增' as Title,count(1) as counts,'ChangeB' as Types
                                  from Equ_ChangeService A,es_Flow B,Br_ECustomer C
                                 where A.FlowID = B.FlowID and  A.CustID = C.ID and C.deleted = 0
	                               and to_char(A.RegTime,'yyyy-MM') = to_char(sysdate,'yyyy-MM')
	                               and A.flowid NOT IN (select flowid from es_message where status=20 and senderid=0)" + strWhere + @"
                                union all
                                --当月未完成
                                select '当月未完成' as Title,count(1) as counts,'ChangeA' as Types
                                  from Equ_ChangeService A,es_Flow B,Br_ECustomer C
                                where A.FlowID = B.FlowID and A.CustID = C.ID and C.deleted = 0 and B.status = 20" + strWhere + @"
                                union all
                                --当月完成【流程结束的，且流程结束时间为当月的】
                                select '当月完成' as Title,count(1) as counts,'ChangeC' as Types
                                  from Equ_ChangeService A,es_Flow B,Br_ECustomer C
                                where A.FlowID = B.FlowID and A.CustID = C.ID and C.deleted = 0 and B.status = 30  and to_char(EndTime,'yyyy-MM') = to_char(sysdate,'yyyy-MM')" + strWhere + @"
                                union all
                                --当月紧急变更【紧急度ID固定为100，且登记时间为当月】
                                select '当月紧急变更' as Title,count(1) as counts,'ChangeD' as Types
                                  from Equ_ChangeService A,Br_ECustomer C
                                where A.CustID = C.ID and C.deleted = 0 and InstancyID = 100 and to_char(A.RegTime,'yyyy-MM') = to_char(sysdate,'yyyy-MM')" + strWhere;

            DataTable dt = CommonDP.ExcuteSqlTable(strSql);

            return dt;
        }
        #endregion

        #region 变更单，柱子点开查看详情
        /// <summary>
        /// 变更单，柱子点开查看详情
        /// </summary>
        /// <param name="strMastCust"></param>
        /// <param name="strType"></param>
        /// <param name="pagesize"></param>
        /// <param name="pageindex"></param>
        /// <param name="rowcount"></param>
        /// <returns></returns>
        public static DataTable GetSMDetail(string strMastCust, string strType, int pagesize, int pageindex, ref int rowcount)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            DataTable dt = new DataTable();
            string strWhere = " 1=1 ";
            string strSql = string.Empty;

            if (strMastCust != "")
            {
                strWhere += " and MastCustID = " + StringTool.SqlQ(strMastCust);
            }

            if (strType == "ChangeA")
            {
                //当月未完成
                dt = OracleDbHelper.ExecuteDataTable(cn, "v_ChangeA", "*", " ORDER BY ID DESC", pagesize, pageindex, strWhere, ref rowcount);
            }
            else if (strType == "ChangeB")
            {
                //当月新增
                dt = OracleDbHelper.ExecuteDataTable(cn, "v_ChangeB", "*", " ORDER BY ID DESC", pagesize, pageindex, strWhere, ref rowcount);
            }
            else if (strType == "ChangeC")
            {
                //当月完成
                dt = OracleDbHelper.ExecuteDataTable(cn, "v_ChangeC", "*", " ORDER BY ID DESC", pagesize, pageindex, strWhere, ref rowcount);
            }
            else if (strType == "ChangeD")
            {
                //当月紧急变更
                dt = OracleDbHelper.ExecuteDataTable(cn, "v_ChangeD", "*", " ORDER BY ID DESC", pagesize, pageindex, strWhere, ref rowcount);
            }
            else if (strType == "ChangeW")
            {
                //需处理总数
                dt = OracleDbHelper.ExecuteDataTable(cn, "v_ChangeW", "*", " ORDER BY ID DESC", pagesize, pageindex, strWhere, ref rowcount);
            }
            else
            {
                strSql = @"--无记录
                                select A.*
                                  from Equ_ChangeService A where 1 = 2";
            }

            return dt;
        }
        #endregion

        #region 紧急变更占比
        /// <summary>
        /// 紧急变更占比
        /// </summary>
        /// <param name="type">0表示按周 否者表示按月</param>
        /// <param name="pColum">报表公用列名</param>
        /// <param name="p_strWhere">where条件</param>
        /// <param name="p_strWhere2">计算总数的时候的where条件</param>
        /// <returns></returns>
        public static DataTable GetEqu_ChangeService(int type, string pColum, string p_strWhere, string p_strWhere2)
        {
            DataTable dt = null;

            OracleParameter[] parms = {
                      new OracleParameter("p_type",OracleType.Number,4),
                      new OracleParameter("p_Colum",OracleType.VarChar,50),
                      new OracleParameter("p_strWhere",OracleType.VarChar,2000),
                      new OracleParameter("p_strWhere2",OracleType.VarChar,2000),
                      new OracleParameter("P_GetData",OracleType.Cursor)
                };
            parms[0].Direction = ParameterDirection.Input;
            parms[1].Direction = ParameterDirection.Input;
            parms[2].Direction = ParameterDirection.Input;
            parms[3].Direction = ParameterDirection.Input;
            parms[4].Direction = ParameterDirection.Output;

            parms[0].Value = type;
            parms[1].Value = pColum;
            parms[2].Value = p_strWhere;
            parms[3].Value = p_strWhere2;

            OracleConnection cn = ConfigTool.GetConnection();
            if (cn.State != ConnectionState.Open)
            {
                cn.Open();
            }
            dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.StoredProcedure, "PROC_Equ_ChangeService", parms);
            ConfigTool.CloseConnection(cn);

            return dt;
        }
        #endregion

        #region 变更受理数量统计
        /// <summary>
        /// 变更受理数量统计
        /// </summary>
        /// <param name="GroupByType">是否按变更类别统计0表示否 否者表示是</param>
        /// <param name="type">0表示按周 否者表示按月</param>
        /// <param name="p_strWhere">where条件</param>
        /// <returns></returns>
        public static DataTable GetCHANGEACCPTED(int GroupByType, int type, string p_strWhere)
        {
            DataTable dt = null;

            OracleParameter[] parms = {
                      new OracleParameter("p_GroupByType",OracleType.Number,4),
                      new OracleParameter("p_type",OracleType.Number,4),
                      new OracleParameter("p_strWhere",OracleType.VarChar,2000),
                      new OracleParameter("P_GetData",OracleType.Cursor)
                };

            parms[0].Direction = ParameterDirection.Input;
            parms[1].Direction = ParameterDirection.Input;
            parms[2].Direction = ParameterDirection.Input;
            parms[3].Direction = ParameterDirection.Output;

            parms[0].Value = GroupByType;
            parms[1].Value = type;
            parms[2].Value = p_strWhere;

            OracleConnection cn = ConfigTool.GetConnection();
            if (cn.State != ConnectionState.Open)
            {
                cn.Open();
            }
            dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.StoredProcedure, "PROC_CHANGEACCPTED", parms);
            ConfigTool.CloseConnection(cn);

            return dt;
        }
        #endregion

        #region 变更类别占比
        /// <summary>
        /// 变更类别占比
        /// </summary>
        /// <param name="p_strWhere">where条件</param>
        /// <returns></returns>
        public static DataTable GetChangeTypePercentage(string p_strWhere)
        {
            DataTable dt = null;

            OracleParameter[] parms = {
                      new OracleParameter("p_strWhere",OracleType.NVarChar,2000),
                      new OracleParameter("P_GetData",OracleType.Cursor)
                };

            parms[0].Direction = ParameterDirection.Input;
            parms[1].Direction = ParameterDirection.Output;

            parms[0].Value = p_strWhere;

            OracleConnection cn = ConfigTool.GetConnection();
            if (cn.State != ConnectionState.Open)
            {
                cn.Open();
            }
            dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.StoredProcedure, "PROC_ChangeTypePercentage", parms);
            ConfigTool.CloseConnection(cn);

            return dt;
        }
        #endregion

        /// <summary>
        /// 判断周区间是否已经设置
        /// </summary>
        /// <returns></returns>
        public static bool WeekIsExist()
        {
            bool result = false;

            string strSQL = string.Empty;
            OracleConnection cn = ConfigTool.GetConnection();
            strSQL = "SELECT * FROM Equ_WeekSetting ";           
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigTool.CloseConnection(cn);

            if (dt != null && dt.Rows.Count > 0)
                result = true;

            return result;

        }
        /// <summary>
        /// 判断月区间是否已经设置
        /// </summary>
        /// <returns></returns>
        public static bool MonthIsExist()
        {
            bool result = false;

            string strSQL = string.Empty;
            OracleConnection cn = ConfigTool.GetConnection();
            strSQL = "SELECT * FROM Equ_MonthSetting ";
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigTool.CloseConnection(cn);

            if (dt != null && dt.Rows.Count > 0)
                result = true;

            return result;
        }


        #region 变更处理平均时间KPI
        /// <summary>
        /// 变更处理平均时间KPI
        /// </summary>
        /// <param name="type">0标示按月 1标示按年</param>
        /// <param name="p_strWhere">where条件</param>
        /// <returns></returns>
        public static DataTable GetChangeTimeAvgKPI(int type, string p_strWhere)
        {
            DataTable dt = null;

            OracleParameter[] parms = {
                      new OracleParameter("p_type",OracleType.Number,4),
                      new OracleParameter("p_strWhere",OracleType.VarChar,2000),
                      new OracleParameter("P_GetData",OracleType.Cursor)
                };

            parms[0].Direction = ParameterDirection.Input;
            parms[1].Direction = ParameterDirection.Input;
            parms[2].Direction = ParameterDirection.Output;

            parms[0].Value = type;
            parms[1].Value = p_strWhere;

            OracleConnection cn = ConfigTool.GetConnection();
            if (cn.State != ConnectionState.Open)
            {
                cn.Open();
            }
            dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.StoredProcedure, "proc_ChangeTimeAvgKPI", parms);
            ConfigTool.CloseConnection(cn);

            return dt;
        }
        #endregion

        #region 变更处理阶段分布KPI
        /// <summary>
        /// 变更处理阶段分布KPI
        /// </summary>
        /// <param name="type">0标示按月 1标示按年</param>
        /// <param name="p_strWhere">where条件</param>
        /// <returns></returns>
        public static DataTable GetChangeStatusKPI(int type, string p_strWhere)
        {
            DataTable dt = null;

            OracleParameter[] parms = {
                      new OracleParameter("p_type",OracleType.Number,4),
                      new OracleParameter("p_strWhere",OracleType.VarChar,2000),
                      new OracleParameter("P_GetData",OracleType.Cursor)
                };

            parms[0].Direction = ParameterDirection.Input;
            parms[1].Direction = ParameterDirection.Input;
            parms[2].Direction = ParameterDirection.Output;

            parms[0].Value = type;
            parms[1].Value = p_strWhere;

            OracleConnection cn = ConfigTool.GetConnection();
            if (cn.State != ConnectionState.Open)
            {
                cn.Open();
            }
            dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.StoredProcedure, "proc_ChangeStatusKPI", parms);
            ConfigTool.CloseConnection(cn);

            return dt;
        }
        #endregion

















        










    }
}
