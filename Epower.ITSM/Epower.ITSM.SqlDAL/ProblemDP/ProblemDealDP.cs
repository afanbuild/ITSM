/****************************************************************************
 * 
 * description:问题管理数据查询类
 * 
 * 
 * 
 * Create by:zhumingchun
 * Create Date:2007-06-23
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
    /// ProblemDealDP
    /// </summary>
    public class ProblemDealDP
    {

        #region 问题单相关
        /// <summary>
        /// 根据问题单流程ID获取相关信息
        /// </summary>
        /// <param name="strFlowID"></param>
        /// <returns></returns>
        public static DataTable GetDataByFlowID(decimal strFlowID)
        {
            string sSql = "";
            OracleConnection cn = ConfigTool.GetConnection();
            sSql = @"SELECT * from Pro_ProblemDeal where FlowID=" + strFlowID.ToString();
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

        /// <summary>
        /// 根据条件获取问题数据表数据
        /// </summary>
        /// <param name="pWhere"></param>
        /// <returns></returns>
        public static DataTable GetProblemDealData(long lngUserID, long lngDeptID, long lngOrgID, RightEntity re, string pWhere)
        {
            string strList = "";       //存放部门列表
            string strSQL = @"select Problem_ID,a.FlowID,a.NodeModelID,a.FlowModelID,
                            Problem_Type,Problem_TypeName,
                            Problem_Level,Problem_LevelName,
                            State,StateName,
                            Problem_Subject,Problem_Title,
                            RegUserID,RegUserName,RegDeptID,RegDeptName,RegTime,
                            b.status,
					        case when b.status=30 then 
					        datediff('Minute',nvl(b.endtime,sysdate),nvl(b.expectendtime,sysdate))
					        else 
					        datediff('Minute',sysdate,nvl(b.expectendtime,sysdate)) end FlowDiffMinute,
                            ( select avg(scale) from Pro_ProblemAnalyse where problem_flowid = a.flowid) as scale,
                            ( select avg(effect) from Pro_ProblemAnalyse where problem_flowid = a.flowid) as effect,
                            ( select avg(stress) from Pro_ProblemAnalyse where problem_flowid = a.flowid) as stress
                            from Pro_ProblemDeal a,es_flow b
                            WHERE a.FlowID = b.FlowID   ";

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
                                strSQL += "AND exists (SELECT messageid FROM es_message WHERE flowid = b.flowid AND recdeptid in (select deptid from ts_dept where fullid like " + StringTool.SqlQ(strList + " % ") + "))";
                            }
                            break;
                        case eO_RightRange.eOrg:
                            strList = DeptDP.GetDeptFullID(lngOrgID);
                            if (strList.Trim().Length > 0)
                            {
                                //不是根部门并有找到
                                strSQL += "AND exists (SELECT messageid FROM es_message WHERE flowid = b.flowid AND recorgid in (select distinct deptid from ts_dept where deptkind = 1 and fullid like " + StringTool.SqlQ(strList + " % ") + "))";
                            }
                            break;
                        default:
                            strSQL += "";
                            break;
                    }
                }
                #endregion
                strSQL = strSQL + " ORDER BY a.Problem_ID DESC";
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
        /// 
        /// </summary>
        /// <param name="lngUserID"></param>
        /// <param name="lngDeptID"></param>
        /// <param name="lngOrgID"></param>
        /// <param name="re"></param>
        /// <param name="pWhere"></param>
        /// <param name="pagesize"></param>
        /// <param name="pageindex"></param>
        /// <param name="rowcount"></param>
        /// <returns></returns>
        public static DataTable GetProblemDealData(long lngUserID, long lngDeptID, long lngOrgID, RightEntity re, string pWhere, int pagesize, int pageindex, ref int rowcount)
        {
            string stWhere = "1=1";
            string strList = "";

            if (re == null || re.CanRead == false)
            {
                //查询出空结果
                stWhere += " AND flowid = -1 ";
            }
            else
            {
                #region 范围条件
                if (re != null)
                {
                    switch (re.RightRange)
                    {
                        case eO_RightRange.eFull:
                            stWhere += "";
                            break;
                        case eO_RightRange.ePersonal:
                            stWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_Pro_ProblemDeal.flowid AND receiverid = " + lngUserID.ToString() + ")";
                            break;
                        case eO_RightRange.eDeptDirect:
                            stWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_Pro_ProblemDeal.flowid AND recdeptid = " + lngDeptID.ToString() + ")";
                            break;
                        case eO_RightRange.eOrgDirect:
                            stWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_Pro_ProblemDeal.flowid AND recorgid = " + lngOrgID.ToString() + ")";
                            break;
                        case eO_RightRange.eDept:
                            strList = DeptDP.GetDeptFullID(lngDeptID);
                            if (strList.Trim().Length > 0)
                            {
                                //不是根部门并有找到
                                stWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_Pro_ProblemDeal.flowid AND recdeptid in (select deptid from ts_dept where fullid like " + StringTool.SqlQ(strList + " % ") + "))";
                            }
                            break;
                        case eO_RightRange.eOrg:
                            strList = DeptDP.GetDeptFullID(lngOrgID);
                            if (strList.Trim().Length > 0)
                            {
                                //不是根部门并有找到
                                stWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_Pro_ProblemDeal.flowid AND recorgid in (select distinct deptid from ts_dept where deptkind = 1 and fullid like " + StringTool.SqlQ(strList + " % ") + "))";
                            }
                            break;
                        default:
                            stWhere += "";
                            break;
                    }
                }
                #endregion
                stWhere += pWhere;
            }
            OracleConnection cn = ConfigTool.GetConnection();
            try
            {
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, "V_Pro_ProblemDeal", "*", " ORDER BY Problem_ID DESC", pagesize, pageindex, stWhere, ref rowcount);
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
        /// 根据事件流程编号取得问题分析数据
        /// </summary>
        /// <param name="sFlowID"></param>
        /// <returns></returns>
        public static DataTable GetProblemAnalsysByEvent(string sFlowID)
        {
            string strSQL = @"select Problem_FlowID,Problem_Title,Event_FlowID,Event_Title,Scale,Effect,Stress,Remark from Pro_ProblemAnalyse where Event_FlowID=" + sFlowID;
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
        /// 新增从问题表中取得问题分析数据
        /// </summary>
        /// <param name="sArr"></param>
        /// <param name="sFlowID"></param>
        /// <param name="sEventTitle"></param>
        /// <returns></returns>
        public static DataTable GetProblemAnalsys(string sArr, string sFlowID, string sEventTitle)
        {
            string sWhere = string.Empty;
            string[] arr = sArr.Split(',');
            if (arr.Length > 1)
                sWhere += " and FlowID In(";
            for (int i = 0; i < arr.Length - 1; i++)
            {
                if (i != arr.Length - 2)
                    sWhere += arr[i] + ",";
                else
                    sWhere += arr[i] + ")";
            }
            string strSQL = @"select FlowID Problem_FlowID,Problem_Title," + StringTool.SqlQ(sEventTitle)
                + " as Event_Title, to_number(" + sFlowID + ") as Event_FlowID,0.00 as Scale, 0.00 as Effect,0.00 as Stress,'' as Remark"
                + " from Pro_ProblemDeal where 1=1 ";
            strSQL += sWhere;
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
        /// 
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="ExpID"></param>
        /// <returns></returns>
        public static void SaveItem(DataTable dt, string sEventFlowID, string sEventTitle, string userid, string username, string deptid)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            try
            {
                string sql = "";
                if (dt.Rows.Count > 0)
                {
                    sql = "Delete From Pro_ProblemAnalyse Where Event_FlowID=" + sEventFlowID.ToString();
                    OracleDbHelper.ExecuteNonQuery(cn, CommandType.Text, sql.ToString());
                }
                foreach (DataRow dr in dt.Rows)
                {
                    string strID = EpowerGlobal.EPGlobal.GetNextID("Pro_ProblemAnalyse_SEQUENCE").ToString();

                    sql = " Insert Into Pro_ProblemAnalyse(ID,Problem_FlowID,Problem_Title,Event_FlowID,Event_Title,Scale,Effect,Stress,Remark,RegUserID,RegUserName,RegDeptID,RegTime) Values("+ strID +"," +
                            dr["Problem_FlowID"].ToString() + "," + StringTool.SqlQ(dr["Problem_Title"].ToString()) + "," + sEventFlowID + "," + StringTool.SqlQ(sEventTitle) + "," +
                            dr["Scale"].ToString() + "," + dr["Effect"].ToString() + "," + dr["Stress"].ToString() + "," + StringTool.SqlQ(dr["Remark"].ToString()) + "," +
                            userid + "," + StringTool.SqlQ(username) + "," + deptid + "," + "sysdate) ";

                    OracleDbHelper.ExecuteNonQuery(cn, CommandType.Text, sql.ToString());
                }
                //if (!string.IsNullOrEmpty(sql.ToString()))
                //{
                //    OracleDbHelper.ExecuteNonQuery(cn, CommandType.Text, sql.ToString());
                //}
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
        /// 根据流程ID获取相关信息
        /// </summary>
        /// <param name="strFlowID"></param>
        /// <returns></returns>
        public static DataTable GetDataByFlowID(string strFlowID)
        {
            string sSql = "";
            OracleConnection cn = ConfigTool.GetConnection();
            sSql = @"select Subject from es_flow where FlowID=" + strFlowID;
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

        /// <summary>
        /// 问题关联事件
        /// </summary>
        /// <param name="lngFlowID">事件流程ID</param>
        /// <returns></returns>
        public static DataTable GetIssuesForProblem(long lngFlowID)
        {
            string strSQL = "";
            strSQL = "SELECT nvl(d.BuildCode, '') || nvl(d.ServiceNo, '') AS ServiceNo,d.DealStatus,d.ServiceType,b.status,b.flowid,b.subject,c.appname,a.scale,a.effect,a.stress  " +
                " FROM Pro_ProblemAnalyse a, es_flow b,es_app c,cst_issues d " +
                " WHERE a.event_flowid = b.flowid AND b.appID = c.appID AND a.Event_FlowID=d.FlowID " +
                     " AND a.problem_flowid = " + lngFlowID.ToString() + "   ORDER BY b.flowid DESC";


            OracleConnection cn = ConfigTool.GetConnection();
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigTool.CloseConnection(cn);
            return dt;
        }

        /// <summary>
        /// 事件升级问题
        /// </summary>
        /// <param name="lngFlowID">事件流程ID</param>
        /// <returns></returns>
        public static DataTable GetProblemData(long lngFlowID)
        {
            string strSQL = @"select nvl(a.BuildCode, '') || nvl(a.ProblemNo, '') AS ServiceNo,Problem_ID,a.FlowID,a.NodeModelID,a.FlowModelID,
                            Problem_Type,Problem_TypeName,
                            Problem_Level,Problem_LevelName,
                            State,StateName,
                            Problem_Subject,Problem_Title,
                            RegUserID,RegUserName,RegDeptID,RegDeptName,RegTime,
                            b.status,
					        case when b.status=30 then 
					        datediff('Minute',nvl(b.endtime,sysdate),nvl(b.expectendtime,sysdate))
					        else 
					        datediff('Minute',sysdate,nvl(b.expectendtime,sysdate)) end FlowDiffMinute,
                            ( select avg(scale) from Pro_ProblemAnalyse where problem_flowid = a.flowid) as scale,
                            ( select avg(effect) from Pro_ProblemAnalyse where problem_flowid = a.flowid) as effect,
                            ( select avg(stress) from Pro_ProblemAnalyse where problem_flowid = a.flowid) as stress
                            from Pro_ProblemDeal a,es_flow b
                            WHERE a.FlowID = b.FlowID   ";
            strSQL += " And a.EquID=" + lngFlowID.ToString();

            OracleConnection cn = ConfigTool.GetConnection();
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigTool.CloseConnection(cn);
            return dt;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="lngFlowID"></param>
        /// <returns></returns>
        public static bool CheckIsChangeProblem(long lngServiceFlowID)
        {
            string strSQL = "";
            strSQL = "SELECT  Problem_ID " +
                " FROM Pro_ProblemDeal " +
                " WHERE nvl(ChangeServiceFlowID,0)=" + lngServiceFlowID.ToString();
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

        #region 获取问题相关服务监控图表
        /// <summary>
        /// 获取问题相关服务监控图表
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
                strWhere = " and ID = " + StringTool.SqlQ(strMastCust);
            }

            string strSql = @"--需处理总数
                                select '需处理总数' as Title,count(1) as counts,'ProblemW' as Types
                                  from Pro_ProblemDeal A,es_Flow B,ts_UserDept C,ts_Dept D
                                 where A.FlowID = B.FlowID and A.RegUserID = C.UserID and C.DeptID = D.DeptID
                                        and D.deleted = 0
	                                    and (
                                         (B.status = 30 and to_char(EndTime,'yyyy-MM') = to_char(sysdate,'yyyy-MM'))
                                         or B.status = 20) 	
                                        and D.MastDeptID in (select E.DeptID from Br_MastCustomer E where E.Deleted = 0)
	                                    and D.MastDeptID in (select DeptID from Br_MastCustomer where 1=1" + strWhere + @")
                                union all
                                --当月新增【问题单的登记时间为当月】
                                select '当月新增' as Title,count(1) as counts,'ProblemB' as Types
                                  from Pro_ProblemDeal A,es_Flow B,ts_UserDept C,ts_Dept D
                                 where A.FlowID = B.FlowID and A.RegUserID = C.UserID and C.DeptID = D.DeptID
                                            and D.deleted = 0 and to_char(A.RegTime,'yyyy-MM') = to_char(sysdate,'yyyy-MM')
		                            and A.flowid NOT IN (select flowid from es_message where status=20 and senderid=0)
	                                and D.MastDeptID in (select E.DeptID from Br_MastCustomer E where E.Deleted = 0)	  
                                    and D.MastDeptID in (select DeptID from Br_MastCustomer where 1=1" + strWhere + @")
                                union all
                                --当月未完成
                                select '当月未完成' as Title,count(1) as counts,'ProblemA' as Types
                                  from Pro_ProblemDeal A,es_Flow B,ts_UserDept C,ts_Dept D
                                 where A.FlowID = B.FlowID and A.RegUserID = C.UserID and C.DeptID = D.DeptID
                                        and D.deleted = 0 and B.status = 20
                                        and D.MastDeptID in (select E.DeptID from Br_MastCustomer E where E.Deleted = 0)
    	                                and D.MastDeptID in (select DeptID from Br_MastCustomer where 1=1" + strWhere + @")
                                union all
                                --当月完成【流程结束的，且流程结束时间为当月的】
                                select '当月关闭' as Title,count(1) as counts,'ProblemC' as Types
                                  from Pro_ProblemDeal A,es_Flow B,ts_UserDept C,ts_Dept D
                                 where A.FlowID = B.FlowID and A.RegUserID = C.UserID and C.DeptID = D.DeptID
	                                and C.deleted = 0 and B.status = 30  and to_char(EndTime,'yyyy-MM') = to_char(sysdate,'yyyy-MM')
	                                and D.MastDeptID in (select E.DeptID from Br_MastCustomer E where E.Deleted = 0)	  
                                    and D.MastDeptID in (select DeptID from Br_MastCustomer where 1=1" + strWhere + @")";

            DataTable dt = CommonDP.ExcuteSqlTable(strSql);

            return dt;
        }
        #endregion

        #region 问题单，柱子点开查看详情
        /// <summary>
        /// 问题单，柱子点开查看详情
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
                strWhere += " and MastDeptID in (select DeptID from Br_MastCustomer where 1=1 and ID = " + StringTool.SqlQ(strMastCust) + ") ";
            }

            if (strType == "ProblemA")
            {
                //当月未完成
                dt = OracleDbHelper.ExecuteDataTable(cn, "v_ProblemA", "*", " ORDER BY Problem_ID DESC", pagesize, pageindex, strWhere, ref rowcount);
            }
            else if (strType == "ProblemB")
            {
                //当月新增
                dt = OracleDbHelper.ExecuteDataTable(cn, "v_ProblemB", "*", " ORDER BY Problem_ID DESC", pagesize, pageindex, strWhere, ref rowcount);
            }
            else if (strType == "ProblemC")
            {
                //当月完成
                dt = OracleDbHelper.ExecuteDataTable(cn, "v_ProblemC", "*", " ORDER BY Problem_ID DESC", pagesize, pageindex, strWhere, ref rowcount);
            }
            else if (strType == "ProblemW")
            {
                //需处理总数
                dt = OracleDbHelper.ExecuteDataTable(cn, "v_ProblemW", "*", " ORDER BY Problem_ID DESC", pagesize, pageindex, strWhere, ref rowcount);
            }
            else
            {
                strSql = @"--无记录
                                select A.*
                                  from Pro_ProblemDeal A where 1 = 2";
            }

            return dt;
        }
        #endregion

        #region 问题按类别统计
        /// <summary>
        /// 问题按类别统计
        /// </summary>
        /// <param name="p_strWhere">where条件</param>
        /// <param name="p_strOrderby">排序条件</param>
        /// <returns></returns>
        public static DataTable GetProblemTypeNum(string p_strWhere, string p_strOrderby)
        {
            DataTable dt = null;

            OracleParameter[] parms = {
                      new OracleParameter("p_strWhere",OracleType.VarChar,2000),
                      new OracleParameter("p_strOrderby",OracleType.VarChar,200),
                      new OracleParameter("P_GetData",OracleType.Cursor)
                };

            parms[0].Direction = ParameterDirection.Input;
            parms[1].Direction = ParameterDirection.Input;
            parms[2].Direction = ParameterDirection.Output;

            parms[0].Value = p_strWhere;
            parms[1].Value = p_strOrderby;

            OracleConnection cn = ConfigTool.GetConnection();
            if (cn.State != ConnectionState.Open)
            {
                cn.Open();
            }
            dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.StoredProcedure, "PROC_ProblemTypeNum", parms);
            ConfigTool.CloseConnection(cn);

            return dt;
        }
        #endregion

        #region 问题按完成总数统计
        /// <summary>
        /// 问题按完成总数统计
        /// </summary>
        /// <param name="p_strWhere">where条件</param>
        /// <param name="p_strOrderby">排序条件</param>
        /// <returns></returns>
        public static DataTable GetProblemCompleteTotal(string p_strWhere, string p_strOrderby)
        {
            DataTable dt = null;

            OracleParameter[] parms = {
                      new OracleParameter("p_strWhere",OracleType.VarChar,2000),
                      new OracleParameter("p_strOrderby",OracleType.VarChar,200),
                      new OracleParameter("P_GetData",OracleType.Cursor)
                };
            parms[0].Direction = ParameterDirection.Input;
            parms[1].Direction = ParameterDirection.Input;
            parms[2].Direction = ParameterDirection.Output;

            parms[0].Value = p_strWhere;
            parms[1].Value = p_strOrderby;

            OracleConnection cn = ConfigTool.GetConnection();
            if (cn.State != ConnectionState.Open)
            {
                cn.Open();
            }
            dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.StoredProcedure, "PROC_ProblemCompleteTotal", parms);
            ConfigTool.CloseConnection(cn);

            return dt;
        }
        #endregion

        #region 高级查询相关

        #region 获取高级查询条件
        /// <summary>
        /// 获取高级查询条件
        /// </summary>
        /// <param name="FormId"></param>
        /// <param name="LoginName"></param>
        /// <param name="Name"></param>
        /// <returns></returns>
        public static DataTable getCST_ISSUE_Where(string FormId, string LoginName, string Name)
        {
            string SQLstr = "";
            SQLstr = @"SELECT  ID,Name,FORMID,LOGINNAME,SQLWhere,SQLText 
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
        #endregion

        #region 记录问题单查询条件查询次数
        /// <summary>
        /// 记录问题单查询条件查询次数
        /// </summary>
        /// <param name="sloginame"></param>
        /// <param name="sname"></param>
        public static void updateCST_ISSUE_WhereNums(string sloginame, string sname)
        {
            string SQLstr = "update CST_ISSUE_QUERYSave set SN=nvl(SN,0)+1 where FORMID='frmProblemMain' AND  nvl(SQLWhere,' ')!='Temp1' AND  LOGINNAME=" + StringTool.SqlQ(sloginame) + " and Name=" + StringTool.SqlQ(sname);
            OracleConnection cn = new OracleConnection(System.Configuration.ConfigurationSettings.AppSettings["SQLConnString"]);

            try { int i = OracleDbHelper.ExecuteNonQuery(cn, CommandType.Text, SQLstr); }
            finally
            {
                if (cn.State == ConnectionState.Open)
                {
                    cn.Close();
                }
            }

        }
        #endregion

        #region 查询问题单已经存在的高级查询条件
        /// <summary>
        /// 查询问题单已经存在的高级查询条件
        /// </summary>
        /// <param name="FormId"></param>
        /// <param name="LoginName"></param>
        /// <param name="Name"></param>
        /// <returns></returns>
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
        #endregion

        #region 删除高级查询条件
        /// <summary>
        /// 删除高级查询条件
        /// </summary>
        /// <param name="sloginame"></param>
        /// <param name="sname"></param>
        public static void deleteCST_ISSUE_Where(string sloginame, string sname)
        {
            string SQLstr = "delete from  CST_ISSUE_QUERYSave  where FORMID='frmProblemMain' and  nvl(SQLWhere,' ')!='Temp1'  and  LOGINNAME=" + StringTool.SqlQ(sloginame) + " and Name=" + StringTool.SqlQ(sname);
            OracleConnection cn = new OracleConnection(System.Configuration.ConfigurationSettings.AppSettings["SQLConnString"]);

            try { int i = OracleDbHelper.ExecuteNonQuery(cn, CommandType.Text, SQLstr); }
            finally
            {
                if (cn.State == ConnectionState.Open)
                {
                    cn.Close();
                }
            }
        }
        #endregion

        #region 问题单查询，第一次进入时，默认查询正在处理、发生时间为近一个月的权限范围内的记录
        /// <summary>
        /// 问题单查询，第一次进入时，默认查询正在处理、发生时间为近一个月的权限范围内的记录
        /// </summary>
        /// <param name="strXmlcond"></param>
        /// <param name="lngUserID"></param>
        /// <param name="lngDeptID"></param>
        /// <param name="lngOrgID"></param>
        /// <param name="re"></param>
        /// <param name="pagesize"></param>
        /// <param name="pageindex"></param>
        /// <param name="rowcount"></param>
        /// <returns></returns>
        public static DataTable GetProbsForCondNew_Init(string strXmlcond, long lngUserID, long lngDeptID, long lngOrgID, RightEntity re, int pagesize, int pageindex, ref int rowcount)
        {
            string strList = string.Empty;
            string strWhere = " 1=1 ";

            //正在处理、发生时间为近一个月的权限范围内的记录
            strWhere += " and Status = 20 and RegTime >= to_date(" + StringTool.SqlQ(DateTime.Now.AddMonths(-1).ToShortDateString()) + ",'yyyy-MM-dd')";
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
                        strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_Pro_ProblemDeal.flowid AND receiverid = " + lngUserID.ToString() + ")";
                        break;
                    case eO_RightRange.eDeptDirect:
                        strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_Pro_ProblemDeal.flowid AND recdeptid = " + lngDeptID.ToString() + ")";
                        break;
                    case eO_RightRange.eOrgDirect:
                        strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_Pro_ProblemDeal.flowid AND recorgid = " + lngOrgID.ToString() + ")";
                        break;
                    case eO_RightRange.eDept:
                        strList = DeptDP.GetDeptFullID(lngDeptID);
                        if (strList.Trim().Length > 0)
                        {
                            //不是根部门并有找到
                            strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_Pro_ProblemDeal.flowid AND recdeptid in (select deptid from ts_dept where fullid like " + StringTool.SqlQ(strList + "%") + "))";
                        }
                        break;
                    case eO_RightRange.eOrg:
                        strList = DeptDP.GetDeptFullID(lngOrgID);
                        if (strList.Trim().Length > 0)
                        {
                            //不是根部门并有找到
                            strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_Pro_ProblemDeal.flowid AND recorgid in (select distinct deptid from ts_dept where deptkind = 1 and fullid like " + StringTool.SqlQ(strList + "%") + "))";
                        }
                        break;
                    default:
                        strWhere += "";
                        break;
                }
                #endregion
            }

            OracleConnection cn = ConfigTool.GetConnection();
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, "V_Pro_ProblemDeal", "*", " ORDER BY Problem_ID DESC", pagesize, pageindex, strWhere, ref rowcount);
            ConfigTool.CloseConnection(cn);
            return dt;
        }
        #endregion

        #region 解析xml串，获取高级查询条件下的查询记录
        /// <summary>
        /// 解析xml串，获取高级查询条件下的查询记录
        /// </summary>
        /// <param name="strXmlcond">xml串</param>
        /// <param name="lngUserID"></param>
        /// <param name="lngDeptID"></param>
        /// <param name="lngOrgID"></param>
        /// <param name="re"></param>
        /// <param name="pagesize"></param>
        /// <param name="pageindex"></param>
        /// <param name="rowcount"></param>
        /// <returns></returns>
        public static DataTable GetProblemsForCond(string strXmlcond, long lngUserID, long lngDeptID, long lngOrgID, RightEntity re, int pagesize, int pageindex, ref int rowcount)
        {
            string strTmp = string.Empty;               //临时存放各查询条件的值
            string strWhere = " 1=1 ";                  //存放where语句
            string strList = string.Empty;              //权限使用

            string strProblemNo = string.Empty;         //问题单号
            string strEquName = string.Empty;           //资产名称
            string strFlowStatus = string.Empty;        //流程状态
            string strStatus = string.Empty;            //事件状态
            string strRegBeginTime = string.Empty;      //登记开始时间
            string strRegEndTime = string.Empty;        //登记结束时间
            string strSubject = string.Empty;           //标题
            string strProbTypeID = "0";                 //问题类别ID
            string strProbLevelID = "0";                //问题级别ID
            string strEffectID = "0";                   //影响度ID
            string strInstancyID = "0";                 //紧急度ID
            string strRegUser = string.Empty;         //登记人

            XmlTextReader tr = new XmlTextReader(new StringReader(strXmlcond));
            while (tr.Read())
            {
                if (tr.Name == "Field" && tr.NodeType == XmlNodeType.Element)
                {
                    strTmp = tr.GetAttribute("Value").Trim();

                    switch (tr.GetAttribute("FieldName"))
                    {
                        case "ProblemNo":           //问题单号
                            strProblemNo = strTmp;
                            break;
                        case "EquName":             //资产名称
                            strEquName = strTmp;
                            break;
                        case "FlowStatus":          //流程状态
                            strFlowStatus = strTmp;
                            break;
                        case "Status":              //问题状态
                            strStatus = strTmp;
                            break;
                        case "MessageBegin":        //登记开始时间
                            strRegBeginTime = strTmp;
                            break;
                        case "MessageEnd":          //登记结束时间
                            strRegEndTime = strTmp;
                            break;
                        case "Subject":             //标题
                            strSubject = strTmp;
                            break;
                        case "CataProblemType":     //问题类别
                            strProbTypeID = strTmp;
                            break;
                        case "CataProblemLevel":    //问题级别
                            strProbLevelID = strTmp;
                            break;
                        case "CtrFCDEffect":        //影响度
                            strEffectID = strTmp;
                            break;
                        case "CtrFCDInstancy":      //紧急度
                            strInstancyID = strTmp;
                            break;
                        case "txtRegUser":          //登记人
                            strRegUser = strTmp;
                            break;
                        default:
                            break;
                    }
                }
            }
            tr.Close();

            #region 解析查询条件

            //问题单号
            if (strProblemNo != string.Empty)
            {
                strWhere += " AND ProblemNo like " + StringTool.SqlQ("%" + strProblemNo.Trim() + "%");
            }

            //资产名称
            if (strEquName != string.Empty)
            {
                strWhere += " AND EquName like " + StringTool.SqlQ("%" + strEquName.Trim() + "%");
            }

            //流程状态
            if (strFlowStatus != "-1" && strFlowStatus != "")
            {
                strWhere += " AND status = " + strFlowStatus;
            }

            //问题状态
            if (strStatus != "0" && strStatus != string.Empty)
            {
                strWhere += " AND State = " + strStatus;
            }

            //登记开始时间
            if (strRegBeginTime.Length != 0)
            {
                strWhere += " AND RegTime >= to_date(" + StringTool.SqlQ(strRegBeginTime) + ",'yyyy-MM-dd HH24:mi:ss')";
            }

            //登记结束时间
            if (strRegEndTime.Length != 0)
            {
                strWhere += " AND RegTime <= to_date(" + StringTool.SqlQ(strRegEndTime) + ",'yyyy-MM-dd HH24:mi:ss')";
            }

            //标题
            if (strSubject != string.Empty)
            {
                strWhere += " AND Problem_Title like " + StringTool.SqlQ("%" + strSubject.Trim() + "%");
            }

            //问题类别
            if (strProbTypeID != "0" && strProbTypeID != string.Empty)
            {
                strWhere += " AND Problem_Type = " + strProbTypeID;
            }

            //问题级别
            if (strProbLevelID != "0" && strProbLevelID != string.Empty)
            {
                strWhere += " AND Problem_Level = " + strProbLevelID;
            }

            //影响度
            if (strEffectID != "0" && strEffectID != string.Empty)
            {
                strWhere += " AND EffectID = " + strEffectID;
            }

            //紧急度
            if (strInstancyID != "0" && strInstancyID != string.Empty)
            {
                strWhere += " AND InstancyID = " + strInstancyID;
            }

            //登记人
            if (strRegUser != string.Empty)
            {
                strWhere += " AND RegUserName like " + StringTool.SqlQ("%" + strRegUser.Trim() + "%");
            }

            #endregion

            #region 权限
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
                        strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_Pro_ProblemDeal.flowid AND receiverid = " + lngUserID.ToString() + ")";
                        break;
                    case eO_RightRange.eDeptDirect:
                        strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_Pro_ProblemDeal.flowid AND recdeptid = " + lngDeptID.ToString() + ")";
                        break;
                    case eO_RightRange.eOrgDirect:
                        strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_Pro_ProblemDeal.flowid AND recorgid = " + lngOrgID.ToString() + ")";
                        break;
                    case eO_RightRange.eDept:
                        strList = DeptDP.GetDeptFullID(lngDeptID);
                        if (strList.Trim().Length > 0)
                        {
                            //不是根部门并有找到
                            strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_Pro_ProblemDeal.flowid AND recdeptid in (select deptid from ts_dept where fullid like " + StringTool.SqlQ(strList + "%") + "))";
                        }
                        break;
                    case eO_RightRange.eOrg:
                        strList = DeptDP.GetDeptFullID(lngOrgID);
                        if (strList.Trim().Length > 0)
                        {
                            //不是根部门并有找到
                            strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_Pro_ProblemDeal.flowid AND recorgid in (select distinct deptid from ts_dept where deptkind = 1 and fullid like " + StringTool.SqlQ(strList + "%") + "))";
                        }
                        break;
                    default:
                        strWhere += "";
                        break;
                }
                #endregion
            }
            #endregion

            OracleConnection cn = ConfigTool.GetConnection();
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, "V_Pro_ProblemDeal", "*", " ORDER BY Problem_ID DESC", pagesize, pageindex, strWhere, ref rowcount);
            ConfigTool.CloseConnection(cn);
            return dt;
        }
        #endregion

        #region 动态查询: 使用生成的动态SQL, 查询记录 - 2013-04-01 @孙绍棕

        /// <summary>
        /// 使用生成的动态SQL, 查询记录
        /// </summary>
        /// <param name="strWhere">动态SQL</param>
        /// <param name="lngUserID">用户编号</param>
        /// <param name="lngDeptID">部门编号</param>
        /// <param name="lngOrgID">组织机构编号</param>
        /// <param name="re">权限</param>
        /// <param name="pagesize">数据长度</param>
        /// <param name="pageindex">第几页?</param>
        /// <param name="rowcount">总行数</param>
        /// <returns></returns>
        public static DataTable GetProblemsWithMoreParams(string strWhere, long lngUserID, long lngDeptID, long lngOrgID, RightEntity re, int pagesize, int pageindex, ref int rowcount)
        {
            string strList = string.Empty;              //权限使用

            if (String.IsNullOrEmpty(strWhere))
                strWhere = " 1 = 1 ";

            #region 权限
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
                        strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_Pro_ProblemDeal.flowid AND receiverid = " + lngUserID.ToString() + ")";
                        break;
                    case eO_RightRange.eDeptDirect:
                        strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_Pro_ProblemDeal.flowid AND recdeptid = " + lngDeptID.ToString() + ")";
                        break;
                    case eO_RightRange.eOrgDirect:
                        strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_Pro_ProblemDeal.flowid AND recorgid = " + lngOrgID.ToString() + ")";
                        break;
                    case eO_RightRange.eDept:
                        strList = DeptDP.GetDeptFullID(lngDeptID);
                        if (strList.Trim().Length > 0)
                        {
                            //不是根部门并有找到
                            strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_Pro_ProblemDeal.flowid AND recdeptid in (select deptid from ts_dept where fullid like " + StringTool.SqlQ(strList + "%") + "))";
                        }
                        break;
                    case eO_RightRange.eOrg:
                        strList = DeptDP.GetDeptFullID(lngOrgID);
                        if (strList.Trim().Length > 0)
                        {
                            //不是根部门并有找到
                            strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_Pro_ProblemDeal.flowid AND recorgid in (select distinct deptid from ts_dept where deptkind = 1 and fullid like " + StringTool.SqlQ(strList + "%") + "))";
                        }
                        break;
                    default:
                        strWhere += "";
                        break;
                }
                #endregion
            }
            #endregion

            OracleConnection conn = ConfigTool.GetConnection();
            DataTable dt = OracleDbHelper.ExecuteDataTable(conn, "V_Pro_ProblemDeal", "*", " ORDER BY Problem_ID DESC", pagesize, pageindex, strWhere, ref rowcount);
            ConfigTool.CloseConnection(conn);
            return dt;
        }
        #endregion

        #region 动态查询: 使用生成的动态SQL, 查询记录, 不需要分页 - 2013-04-08 @孙绍棕

        /// <summary>
        /// 使用生成的动态SQL, 查询记录, 不需要分页
        /// </summary>
        /// <param name="strWhere">动态SQL</param>
        /// <param name="lngUserID">用户编号</param>
        /// <param name="lngDeptID">部门编号</param>
        /// <param name="lngOrgID">组织机构编号</param>
        /// <param name="re">权限</param>
        /// <returns></returns>
        public static DataTable GetProblemsWithOutPage(string strWhere, long lngUserID, long lngDeptID, long lngOrgID, RightEntity re)
        {            
            String strSqlScript = String.Empty;    // sql 查询命令
            string strList = string.Empty;              //权限使用

            if (String.IsNullOrEmpty(strWhere))
                strWhere = " WHERE 1 = 1 ";

            #region 权限
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
                        strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_Pro_ProblemDeal.flowid AND receiverid = " + lngUserID.ToString() + ")";
                        break;
                    case eO_RightRange.eDeptDirect:
                        strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_Pro_ProblemDeal.flowid AND recdeptid = " + lngDeptID.ToString() + ")";
                        break;
                    case eO_RightRange.eOrgDirect:
                        strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_Pro_ProblemDeal.flowid AND recorgid = " + lngOrgID.ToString() + ")";
                        break;
                    case eO_RightRange.eDept:
                        strList = DeptDP.GetDeptFullID(lngDeptID);
                        if (strList.Trim().Length > 0)
                        {
                            //不是根部门并有找到
                            strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_Pro_ProblemDeal.flowid AND recdeptid in (select deptid from ts_dept where fullid like " + StringTool.SqlQ(strList + "%") + "))";
                        }
                        break;
                    case eO_RightRange.eOrg:
                        strList = DeptDP.GetDeptFullID(lngOrgID);
                        if (strList.Trim().Length > 0)
                        {
                            //不是根部门并有找到
                            strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_Pro_ProblemDeal.flowid AND recorgid in (select distinct deptid from ts_dept where deptkind = 1 and fullid like " + StringTool.SqlQ(strList + "%") + "))";
                        }
                        break;
                    default:
                        strWhere += "";
                        break;
                }
                #endregion
            }
            #endregion

            strSqlScript = String.Format(" SELECT * FROM  V_Pro_ProblemDeal {0} ORDER BY Problem_ID DESC",
                strWhere);

            E8Logger.Info(strSqlScript);

            OracleConnection conn = ConfigTool.GetConnection();
            DataTable dt = OracleDbHelper.ExecuteDataTable(conn, CommandType.Text, strSqlScript);
            ConfigTool.CloseConnection(conn);
            return dt;
        }
        #endregion

        #region 解析xml串，获取高级查询条件下的查询记录【导出excel】
        /// <summary>
        /// 解析xml串，获取高级查询条件下的查询记录【导出excel】
        /// </summary>
        /// <param name="strXmlcond">xml串</param>
        /// <param name="lngUserID"></param>
        /// <param name="lngDeptID"></param>
        /// <param name="lngOrgID"></param>
        /// <param name="re"></param>
        /// <param name="pagesize"></param>
        /// <param name="pageindex"></param>
        /// <param name="rowcount"></param>
        /// <returns></returns>
        public static DataTable GetProblemsForCond(string strXmlcond, long lngUserID, long lngDeptID, long lngOrgID, RightEntity re)
        {
            string strTmp = string.Empty;               //临时存放各查询条件的值
            string strWhere = string.Empty;                  //存放where语句
            string strList = string.Empty;              //权限使用

            string strProblemNo = string.Empty;         //问题单号
            string strEquName = string.Empty;           //资产名称
            string strFlowStatus = string.Empty;        //流程状态
            string strStatus = string.Empty;            //事件状态
            string strRegBeginTime = string.Empty;      //登记开始时间
            string strRegEndTime = string.Empty;        //登记结束时间
            string strSubject = string.Empty;           //标题
            string strProbTypeID = "0";                 //问题类别ID
            string strProbLevelID = "0";                //问题级别ID
            string strEffectID = "0";                   //影响度ID
            string strInstancyID = "0";                 //紧急度ID
            string strRegUser = string.Empty;         //登记人

            string strSql = string.Empty;               //查询语句
            strSql = "select * from V_Pro_ProblemDeal where 1=1 ";

            XmlTextReader tr = new XmlTextReader(new StringReader(strXmlcond));
            while (tr.Read())
            {
                if (tr.Name == "Field" && tr.NodeType == XmlNodeType.Element)
                {
                    strTmp = tr.GetAttribute("Value").Trim();

                    switch (tr.GetAttribute("FieldName"))
                    {
                        case "ProblemNo":           //问题单号
                            strProblemNo = strTmp;
                            break;
                        case "EquName":             //资产名称
                            strEquName = strTmp;
                            break;
                        case "FlowStatus":          //流程状态
                            strFlowStatus = strTmp;
                            break;
                        case "Status":              //问题状态
                            strStatus = strTmp;
                            break;
                        case "MessageBegin":        //登记开始时间
                            strRegBeginTime = strTmp;
                            break;
                        case "MessageEnd":          //登记结束时间
                            strRegEndTime = strTmp;
                            break;
                        case "Subject":             //标题
                            strSubject = strTmp;
                            break;
                        case "CataProblemType":     //问题类别
                            strProbTypeID = strTmp;
                            break;
                        case "CataProblemLevel":    //问题级别
                            strProbLevelID = strTmp;
                            break;
                        case "CtrFCDEffect":        //影响度
                            strEffectID = strTmp;
                            break;
                        case "CtrFCDInstancy":      //紧急度
                            strInstancyID = strTmp;
                            break;
                        case "txtRegUser":          //登记人
                            strRegUser = strTmp;
                            break;
                        default:
                            break;
                    }
                }
            }
            tr.Close();

            #region 解析查询条件

            //问题单号
            if (strProblemNo != string.Empty)
            {
                strWhere += " AND ProblemNo like " + StringTool.SqlQ("%" + strProblemNo.Trim() + "%");
            }

            //资产名称
            if (strEquName != string.Empty)
            {
                strWhere += " AND EquName like " + StringTool.SqlQ("%" + strEquName.Trim() + "%");
            }

            //流程状态
            if (strFlowStatus != "-1" && strFlowStatus != "")
            {
                strWhere += " AND status = " + strFlowStatus;
            }

            //问题状态
            if (strStatus != "0" && strStatus != string.Empty)
            {
                strWhere += " AND State = " + strStatus;
            }

            //登记开始时间
            if (strRegBeginTime.Length != 0)
            {
                strWhere += " AND RegTime >= to_char(to_date(" + StringTool.SqlQ(strRegBeginTime) + ",'yyyy-MM-dd HH24:mi:ss'))";
            }

            //登记结束时间
            if (strRegEndTime.Length != 0)
            {
                strWhere += " AND RegTime <= to_char(to_date(" + StringTool.SqlQ(strRegEndTime) + ",'yyyy-MM-dd HH24:mi:ss'))";
            }

            //标题
            if (strSubject != string.Empty)
            {
                strWhere += " AND Problem_Title like " + StringTool.SqlQ("%" + strSubject.Trim() + "%");
            }

            //问题类别
            if (strProbTypeID != "0" && strProbTypeID != string.Empty)
            {
                strWhere += " AND Problem_Type = " + strProbTypeID;
            }

            //问题级别
            if (strProbLevelID != "0" && strProbLevelID != string.Empty)
            {
                strWhere += " AND Problem_Level = " + strProbLevelID;
            }

            //影响度
            if (strEffectID != "0" && strEffectID != string.Empty)
            {
                strWhere += " AND EffectID = " + strEffectID;
            }

            //紧急度
            if (strInstancyID != "0" && strInstancyID != string.Empty)
            {
                strWhere += " AND InstancyID = " + strInstancyID;
            }

            //登记人
            if (strRegUser != string.Empty)
            {
                strWhere += " AND RegUserName like " + StringTool.SqlQ("%" + strRegUser.Trim() + "%");
            }

            #endregion

            #region 权限
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
                        strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_Pro_ProblemDeal.flowid AND receiverid = " + lngUserID.ToString() + ")";
                        break;
                    case eO_RightRange.eDeptDirect:
                        strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_Pro_ProblemDeal.flowid AND recdeptid = " + lngDeptID.ToString() + ")";
                        break;
                    case eO_RightRange.eOrgDirect:
                        strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_Pro_ProblemDeal.flowid AND recorgid = " + lngOrgID.ToString() + ")";
                        break;
                    case eO_RightRange.eDept:
                        strList = DeptDP.GetDeptFullID(lngDeptID);
                        if (strList.Trim().Length > 0)
                        {
                            //不是根部门并有找到
                            strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_Pro_ProblemDeal.flowid AND recdeptid in (select deptid from ts_dept where fullid like " + StringTool.SqlQ(strList + "%") + "))";
                        }
                        break;
                    case eO_RightRange.eOrg:
                        strList = DeptDP.GetDeptFullID(lngOrgID);
                        if (strList.Trim().Length > 0)
                        {
                            //不是根部门并有找到
                            strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_Pro_ProblemDeal.flowid AND recorgid in (select distinct deptid from ts_dept where deptkind = 1 and fullid like " + StringTool.SqlQ(strList + "%") + "))";
                        }
                        break;
                    default:
                        strWhere += "";
                        break;
                }
                #endregion
            }
            #endregion

            strSql += strWhere;

            OracleConnection cn = ConfigTool.GetConnection();
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSql);
            ConfigTool.CloseConnection(cn);
            return dt;
        }
        #endregion

        #region 问题单查询，第一次进入时，默认查询正在处理、发生时间为近一个月的权限范围内的记录【导出excel】
        /// <summary>
        /// 问题单查询，第一次进入时，默认查询正在处理、发生时间为近一个月的权限范围内的记录【导出excel】
        /// </summary>
        /// <param name="strXmlcond"></param>
        /// <param name="lngUserID"></param>
        /// <param name="lngDeptID"></param>
        /// <param name="lngOrgID"></param>
        /// <param name="re"></param>
        /// <param name="pagesize"></param>
        /// <param name="pageindex"></param>
        /// <param name="rowcount"></param>
        /// <returns></returns>
        public static DataTable GetProbsForCondNew_Init(string strXmlcond, long lngUserID, long lngDeptID, long lngOrgID, RightEntity re)
        {
            string strList = string.Empty;
            string strWhere = string.Empty;

            string strSql = string.Empty;               //查询语句
            strSql = "select * from V_Pro_ProblemDeal where 1=1 ";

            //正在处理、发生时间为近一个月的权限范围内的记录
            strWhere += " and Status = 20 and RegTime >= to_date(to_char(DateAdd('month',-1,sysdate), 'yyyy-MM-dd'),'yyyy-MM-dd')";
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
                        strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_Pro_ProblemDeal.flowid AND receiverid = " + lngUserID.ToString() + ")";
                        break;
                    case eO_RightRange.eDeptDirect:
                        strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_Pro_ProblemDeal.flowid AND recdeptid = " + lngDeptID.ToString() + ")";
                        break;
                    case eO_RightRange.eOrgDirect:
                        strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_Pro_ProblemDeal.flowid AND recorgid = " + lngOrgID.ToString() + ")";
                        break;
                    case eO_RightRange.eDept:
                        strList = DeptDP.GetDeptFullID(lngDeptID);
                        if (strList.Trim().Length > 0)
                        {
                            //不是根部门并有找到
                            strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_Pro_ProblemDeal.flowid AND recdeptid in (select deptid from ts_dept where fullid like " + StringTool.SqlQ(strList + "%") + "))";
                        }
                        break;
                    case eO_RightRange.eOrg:
                        strList = DeptDP.GetDeptFullID(lngOrgID);
                        if (strList.Trim().Length > 0)
                        {
                            //不是根部门并有找到
                            strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_Pro_ProblemDeal.flowid AND recorgid in (select distinct deptid from ts_dept where deptkind = 1 and fullid like " + StringTool.SqlQ(strList + "%") + "))";
                        }
                        break;
                    default:
                        strWhere += "";
                        break;
                }
                #endregion
            }

            strSql += strWhere;

            OracleConnection cn = ConfigTool.GetConnection();
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSql);
            ConfigTool.CloseConnection(cn);
            return dt;
        }
        #endregion

        #endregion

        #region 按时间统计问题数量KPI
        /// <summary>
        /// 按时间统计问题数量KPI
        /// </summary>
        /// <param name="type">0标示按月 1标示按年</param>
        /// <param name="p_strWhere">where条件</param>        
        /// <returns></returns>
        public static DataTable GetproblemCountKPI(int type, string p_strWhere)
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
            dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.StoredProcedure, "proc_problemCountKPI", parms);
            ConfigTool.CloseConnection(cn);

            return dt;
        }
        #endregion


        #region 问题合并
        /// <summary>
        /// 增加问题合并时，取出问题单
        /// </summary>
        /// <param name="strFlowList"></param>
        /// <returns></returns>
        public static DataTable GetProblemSubAdd(string strFlowList)
        {
            string strSQL = @"select nvl(a.BuildCode, '') || nvl(a.ProblemNo, '') AS ServiceNo,Problem_ID,a.FlowID,a.NodeModelID,a.FlowModelID,
                            Problem_Type,Problem_TypeName,
                            Problem_Level,Problem_LevelName,
                            State,StateName,
                            Problem_Subject,Problem_Title,
                            RegUserID,RegUserName,RegDeptID,RegDeptName,RegTime,
                            b.status,0 FlowDealState
                            from Pro_ProblemDeal a,es_flow b
                            WHERE a.FlowID = b.FlowID   ";
            strSQL += " And a.FlowID In (" + strFlowList + ")";

            OracleConnection cn = ConfigTool.GetConnection();
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigTool.CloseConnection(cn);
            return dt;
        }
        /// <summary>
        /// 问题合并时，取出问题单
        /// </summary>
        /// <param name="lngFlowID"></param>
        /// <returns></returns>
        public static DataTable GetProblemSub(long lngFlowID)
        {
            string strSQL = @"select nvl(a.BuildCode, '') || nvl(a.ProblemNo, '') AS ServiceNo,Problem_ID,a.FlowID,a.NodeModelID,a.FlowModelID,
                            Problem_Type,Problem_TypeName,
                            Problem_Level,Problem_LevelName,
                            State,StateName,
                            Problem_Subject,Problem_Title,
                            a.RegUserID,a.RegUserName,a.RegDeptID,a.RegDeptName,a.RegTime,
                            b.status,c.FlowDealState
                            from Pro_ProblemDeal a,es_flow b,Pro_ProblemRel c
                            WHERE a.FlowID = b.FlowID AND a.FlowID=c.SubFlowID  ";
            strSQL += " And c.MastFlowID =" + lngFlowID.ToString();

            OracleConnection cn = ConfigTool.GetConnection();
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigTool.CloseConnection(cn);
            return dt;
        }

        /// <summary>
        /// 问题合并，关联于
        /// </summary>
        /// <param name="lngFlowID"></param>
        /// <returns></returns>
        public static DataTable GetProblemSubRel(long lngFlowID)
        {
            string strSQL = @"select distinct nvl(a.BuildCode, '') || nvl(a.ProblemNo, '') AS ServiceNo,Problem_ID,a.FlowID,a.NodeModelID,a.FlowModelID,
                            Problem_Type,Problem_TypeName,
                            Problem_Level,Problem_LevelName,
                            State,StateName,
                            Problem_Subject,Problem_Title,
                            a.RegUserID,a.RegUserName,a.RegDeptID,a.RegDeptName,a.RegTime,
                            b.status
                            from Pro_ProblemDeal a,es_flow b,Pro_ProblemRel c
                            WHERE a.FlowID = b.FlowID AND a.FlowID=c.MastFlowID  ";
            strSQL += " And c.SubFlowID =" + lngFlowID.ToString();

            OracleConnection cn = ConfigTool.GetConnection();
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigTool.CloseConnection(cn);
            return dt;
        }
        #endregion



        #region 取问题单号 - 2013-11-28 @孙绍棕

        /// <summary>
        /// 取问题单号
        /// </summary>
        /// <param name="lngFlowID">流程编号</param>
        /// <returns></returns>
        public static String GetProblemNO(long lngFlowID)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            try
            {
                String strSQL = String.Format(@"select buildcode || problemno as problemno from pro_problemdeal where flowid ={0}", lngFlowID);
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);

                return dt.Rows[0]["problemno"].ToString().Trim();
            }
            finally
            {
                ConfigTool.CloseConnection(cn);
            }
        }

        #endregion
    }
}
