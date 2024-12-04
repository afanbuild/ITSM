/****************************************************************************
 * 
 * description:服务单数据处理层
 * 
 * 
 * 
 * Create by:
 * Create Date:2007-08-01
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
using System.Collections.Generic;
using System.Collections;
using System.Data.SqlClient;
using Epower.ITSM.SqlDAL.ES_TBLCS;
using EpowerCom;

namespace Epower.ITSM.SqlDAL
{
    /// <summary>
    /// FareDP 的摘要说明。
    /// </summary>
    public class ZHServiceDP
    {
        public ZHServiceDP()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        #region 服务管理相关
        /// <summary>
        /// 根据流程ID获取相关信息
        /// </summary>
        /// <param name="strFlowID"></param>
        /// <returns></returns>
        public static DataTable GetDataByFlowID(decimal strFlowID)
        {
            string sSql = "";
            OracleConnection cn = ConfigTool.GetConnection();
            sSql = @"SELECT * from cst_issues where FlowID=" + strFlowID.ToString();
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

        #region 根据流程ID获取相关信息
        /// <summary>
        /// 根据流程ID获取相关信息
        /// </summary>
        /// <param name="strFlowID"></param>
        /// <returns></returns>
        public static DataTable GetIssuesByFlowID(decimal strFlowID)
        {
            string sSql = "";
            OracleConnection cn = ConfigTool.GetConnection();
            sSql = @"select A.*,B.partBranchName as DEPT,B.Code as EQUCODE,b.catalogid,b.catalogname,
                     '' as CHANGECONTENT,'' as OLDVALUE,'' as NEWVALUE,'' as Remark
                    from 
                            (
                            select EquipmentCatalogID as ListID,EquipmentCatalogName as ListName,EquipmentID as EQUID,EquipmentName as EQUNAME from cst_issues where FlowID =" + strFlowID.ToString() + @"
                            ) A
                    left join  Equ_Desk B
                      on A.EQUID = B.ID";
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

        #region  根据FlowID查询出对应的当前处理人
        public string GetCurrName(object strFlowID)
        {
            decimal flowId = 0;
            if (strFlowID == null)
                return "";
            flowId = Convert.ToDecimal(strFlowID);
            string Name = "";
            OracleConnection cn = ConfigTool.GetConnection();

            try
            {
                string sSql = string.Format("SELECT c.Name,c.UserID FROM ES_MESSAGE a LEFT JOIN TS_USER c ON a.ReceiverID=c.UserID WHERE a.MessageID = "
                    + "( " + "SELECT max(b.MessageID)MessageID FROM CST_ISSUES a LEFT JOIN ES_MESSAGE b ON a.FlowID=b.FlowID WHERE a.FlowID=" + strFlowID + " and b.messagetype =10)");
                OracleDataReader dr = OracleDbHelper.ExecuteReader(cn, CommandType.Text, sSql);
                while (dr.Read())
                {
                    Name = dr["Name"].ToString();
                }
                dr.Close();

                return Name;
            }
            finally { ConfigTool.CloseConnection(cn); }
        }
        #endregion

        #region 服务分析相关
        /// <summary>
        /// 获取客户服务响应及时率分析数据
        /// </summary>
        /// <param name="nYear"></param>
        /// <param name="lngServiceTypeID"></param>
        /// <param name="lngDeptID"></param>
        /// <returns></returns>
        public static DataTable GetAnalysisCSMOnTimeRate(int nYear, long lngServiceTypeID, long lngDeptID)
        {

            string sSql = "";

            OracleConnection cn = ConfigTool.GetConnection();

            //GUIDID 固定为10002

            //流程正常结束的才纳入统计
            //10002 为响应及时率
            sSql = @"SELECT   '1月'=sum(case  when c.months=1 then c.OnTimeRate else 0 end),
									'2月'= sum(case  when c.months=2 then c.OnTimeRate else 0 end),
									'3月'=sum(case  when c.months=3 then c.OnTimeRate else 0 end),
									'4月'=sum(case  when c.months=4 then c.OnTimeRate else 0 end),
									'5月'=sum(case  when c.months=5 then c.OnTimeRate else 0 end),
									'6月'=sum(case  when c.months=6 then c.OnTimeRate else 0 end),
									'7月'=sum(case  when c.months=7 then c.OnTimeRate else 0 end),
									'8月'=sum(case  when c.months=8 then c.OnTimeRate else 0 end),
									'9月'=sum(case  when c.months=9 then c.OnTimeRate else 0 end),
									'10月'=sum(case  when c.months=10 then c.OnTimeRate else 0 end),
									'11月'=sum(case  when c.months=11 then c.OnTimeRate else 0 end),
									'12月'=sum(case  when c.months=12 then c.OnTimeRate else 0 end)
								FROM
								( SELECT month(a.regsysdate) as months,case when count(a.smsid) = 0 then 1 
																		else sum(case when datediff('Minute',Outtime,c.LimitTime) <= 0 then 0 
																				else 1 end) / to_number(count(smsid))  end as OnTimeRate FROM cst_issues a ,es_flow b,ea_flowbuslimit c
								WHERE a.flowid = b.flowid  AND a.flowid = c.flowid AND c.guidid = 10002 AND b.status = 30 AND  year(a.regsysdate) = " + nYear.ToString() +
                                ((lngServiceTypeID == 0) || (lngServiceTypeID == -1) ? "" : "AND a.servicetypeid =" + lngServiceTypeID.ToString()) +
                                ((lngDeptID == 0 || lngDeptID == -1) ? "" : " AND a.orgid =" + lngDeptID + "") +
                                @" AND not a.regsysdate is null AND  not a.servicetime is null

								GROUP BY  month(a.regsysdate) ) c";





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
        /// 获取客户服务处理及时率分析数据
        /// </summary>
        /// <param name="nYear"></param>
        /// <param name="lngServiceTypeID"></param>
        /// <param name="lngDeptID"></param>
        /// <returns></returns>
        public static DataTable GetAnalysisCSMDealTimeRate(int nYear, long lngServiceTypeID, long lngDeptID)
        {

            string sSql = "";

            OracleConnection cn = ConfigTool.GetConnection();

            //GUIDID 固定为10001

            //流程正常结束的才纳入统计
            //10001 为处理及时率
            sSql = @"SELECT   '1月'=sum(case  when c.months=1 then c.OnTimeRate else 0 end),
									'2月'= sum(case  when c.months=2 then c.OnTimeRate else 0 end),
									'3月'=sum(case  when c.months=3 then c.OnTimeRate else 0 end),
									'4月'=sum(case  when c.months=4 then c.OnTimeRate else 0 end),
									'5月'=sum(case  when c.months=5 then c.OnTimeRate else 0 end),
									'6月'=sum(case  when c.months=6 then c.OnTimeRate else 0 end),
									'7月'=sum(case  when c.months=7 then c.OnTimeRate else 0 end),
									'8月'=sum(case  when c.months=8 then c.OnTimeRate else 0 end),
									'9月'=sum(case  when c.months=9 then c.OnTimeRate else 0 end),
									'10月'=sum(case  when c.months=10 then c.OnTimeRate else 0 end),
									'11月'=sum(case  when c.months=11 then c.OnTimeRate else 0 end),
									'12月'=sum(case  when c.months=12 then c.OnTimeRate else 0 end)
								FROM
								( SELECT month(a.regsysdate) as months,case when count(a.smsid) = 0 then 1 
																		else sum(case when datediff('Minute',FinishedTime,c.LimitTime) <= 0 then 0 
																				else 1 end) / to_number(count(smsid))  end as OnTimeRate FROM cst_issues a ,es_flow b,ea_flowbuslimit c
								WHERE a.flowid = b.flowid  AND a.flowid = c.flowid AND c.guidid = 10001 AND b.status = 30 AND  year(a.regsysdate) = " + nYear.ToString() +
                                ((lngServiceTypeID == 0) || (lngServiceTypeID == -1) ? "" : "AND a.servicetypeid =" + lngServiceTypeID.ToString()) +
                                ((lngDeptID == 0 || lngDeptID == -1) ? "" : " AND a.orgid =" + lngDeptID + "") +
                                @" AND not a.regsysdate is null AND  not a.servicetime is null

								GROUP BY  month(a.regsysdate) ) c";





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
        /// 获取客户满意度分析
        /// </summary>
        /// <param name="nYear"></param>
        /// <param name="lngServiceTypeID"></param>
        /// <param name="lngWTTypeID"></param>
        /// <param name="lngDeptID"></param>
        /// <returns></returns>
        public static DataTable GetAnalysisStatisfied(int nYear, long lngServiceTypeID, long lngWTTypeID, long lngDeptID, long lngMastCustomer)
        {
            long lngCatalogID = lngServiceTypeID;
            string sFullID = string.Empty;
            if (lngServiceTypeID == -1 || lngServiceTypeID == 0)
                lngCatalogID = 1001;
            //            string sSql = @" declare @sFullID nvarchar(200)
            //                      select @sFullID=FullID from Es_Catalog where CatalogID=" + lngCatalogID.ToString();
            //            sSql += @" SELECT   '年度'+'" + nYear + @"' as nYear, '1月'=sum(case  when c.months=1 then c.sqtyRate else 0 end),
            //									'2月'= sum(case  when c.months=2 then c.sqtyRate else 0 end),
            //									'3月'=sum(case  when c.months=3 then c.sqtyRate else 0 end),
            //									'4月'=sum(case  when c.months=4 then c.sqtyRate else 0 end),
            //									'5月'=sum(case  when c.months=5 then c.sqtyRate else 0 end),
            //									'6月'=sum(case  when c.months=6 then c.sqtyRate else 0 end),
            //									'7月'=sum(case  when c.months=7 then c.sqtyRate else 0 end),
            //									'8月'=sum(case  when c.months=8 then c.sqtyRate else 0 end),
            //									'9月'=sum(case  when c.months=9 then c.sqtyRate else 0 end),
            //									'10月'=sum(case  when c.months=10 then c.sqtyRate else 0 end),
            //									'11月'=sum(case  when c.months=11 then c.sqtyRate else 0 end),
            //									'12月'=sum(case  when c.months=12 then c.sqtyRate else 0 end)
            //								FROM (
            //									SELECT month(b.RegSysDate) as months,convert(decimal,sum(case when a.feedback = 1 then 1 else 0 end))
            //											/ (sum(case when a.feedback = 1 then 1 else 0 end) + sum(case when a.feedback = 1 then 0 else 1 end))*100 as sqtyRate 
            //									FROM EA_Issues_FeedBack a,cst_issues b,Es_Catalog c 
            //									WHERE a.flowid = b.flowid And servicetypeid=CatalogID AND a.appid=1026 AND year(b.regsysdate) = " + nYear.ToString() +
            //                                    ((lngWTTypeID == 0) || (lngWTTypeID == -1) ? "" : "AND b.ServiceLevelID =" + lngWTTypeID.ToString()) +
            //                                    ((lngDeptID == 0 || lngDeptID == -1) ? "" : " AND b.orgid =" + lngDeptID + "");
            //            if (lngMastCustomer != 0)
            //                sSql += " And nvl(CustID,0) In (select ID from Br_ECustomer where MastCustID=" + lngMastCustomer.ToString() + ")";
            //            if (lngCatalogID != 1001)
            //            {
            //                sSql += " AND substring(c.FullID,0,len(@sFullID)+1)=@sFullID";
            //            }
            //            sSql += @" GROUP BY month(b.RegSysDate) ) c";

            OracleConnection cn = ConfigTool.GetConnection("SQLConnString");
            OracleParameter[] para = {
                 new OracleParameter("nYear", OracleType.VarChar , 50 ),
                 new OracleParameter("lngServiceTypeID", OracleType.VarChar , 50 ),
                 new OracleParameter("lngWTTypeID", OracleType.VarChar , 50 ),
                 new OracleParameter("lngDeptID", OracleType.VarChar , 50 ),
                 new OracleParameter("lngMastCustomer", OracleType.VarChar , 50 ),
                 new OracleParameter("p_cursor",OracleType.Cursor)
            };
            para[0].Direction = ParameterDirection.Input;
            para[1].Direction = ParameterDirection.Input;
            para[2].Direction = ParameterDirection.Input;
            para[3].Direction = ParameterDirection.Input;
            para[4].Direction = ParameterDirection.Input;
            para[5].Direction = ParameterDirection.Output;

            para[0].Value = nYear.ToString();
            para[1].Value = lngCatalogID.ToString();
            para[2].Value = lngWTTypeID.ToString();
            para[3].Value = lngDeptID.ToString();
            para[4].Value = lngMastCustomer.ToString();
            try
            {
                //DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, sSql);
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.StoredProcedure, "PROC_RPT_Satisfaction", para);
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
        /// 获取客户满意度统计数据
        /// </summary>
        /// <param name="nYear"></param>
        /// <param name="lngServiceTypeID"></param>
        /// <param name="lngWTTypeID"></param>
        /// <param name="lngDeptID"></param>
        /// <param name="lngMastCustomer"></param>
        /// <returns></returns>
        public static DataTable GetAnalysisStatisfiedGrid(int nYear, long lngServiceTypeID, long lngWTTypeID, long lngDeptID, long lngMastCustomer)
        {
            long lngCatalogID = lngServiceTypeID;
            string sFullID = string.Empty;
            if (lngServiceTypeID == -1 || lngServiceTypeID == 0)
                lngCatalogID = 1001;
            //            string sSql = @" declare @sFullID nvarchar(200)
            //                      select @sFullID=FullID from Es_Catalog where CatalogID=" + lngCatalogID.ToString();
            //            sSql += @" SELECT b.months as 月份,sum(b.fsqty) as 满意次数,sum(b.fqty) as 回访次数,
            //							count(b.flowid) as 事件数量,convert(decimal,sum(b.yhfnum)) * 100 / count(b.flowid) as 回访率,
            //							convert(decimal,sum(b.fsqty)) * 100 / sum(b.fqty) as 满意度
            //						FROM
            //						(
            //						SELECT month(a.RegSysDate) as months,flowid,
            //							(select count(flowid) from EA_Issues_FeedBack where flowid = a.flowid) as fqty,
            //								(select sum(case when feedback = 3 then 0 else 1 end) from EA_Issues_FeedBack where flowid = a.flowid) as fsqty,
            //                                (select count(flowid) from (select distinct flowid from EA_Issues_FeedBack) feeback where flowid = a.flowid) as yhfnum
            //							FROM cst_issues a,Es_Catalog c 
            //							WHERE servicetypeid=CatalogID and year(a.regsysdate) = " + nYear.ToString() +
            //                            ((lngWTTypeID == 0) || (lngWTTypeID == -1) ? "" : "AND a.ServiceLevelID =" + lngWTTypeID.ToString()) +
            //                            ((lngDeptID == 0 || lngDeptID == -1) ? "" : " AND a.orgid =" + lngDeptID + "");
            //            if (lngMastCustomer != 0)
            //                sSql += " And nvl(CustID,0) In (select ID from Br_ECustomer where MastCustID=" + lngMastCustomer.ToString() + ")";
            //            if (lngCatalogID != 1001)
            //            {
            //                sSql += " AND substring(c.FullID,0,len(@sFullID)+1)=@sFullID";
            //            }
            //            sSql += @" ) b
            //							GROUP BY b.months";

            OracleConnection cn = ConfigTool.GetConnection("SQLConnString");
            OracleParameter[] para = {
                 new OracleParameter("nYear", OracleType.VarChar , 50 ),
                 new OracleParameter("lngServiceTypeID", OracleType.VarChar , 50 ),
                 new OracleParameter("lngWTTypeID", OracleType.VarChar , 50 ),
                 new OracleParameter("lngDeptID", OracleType.VarChar , 50 ),
                 new OracleParameter("lngMastCustomer", OracleType.VarChar , 50 ),
                 new OracleParameter("p_cursor",OracleType.Cursor)
            };
            para[0].Direction = ParameterDirection.Input;
            para[1].Direction = ParameterDirection.Input;
            para[2].Direction = ParameterDirection.Input;
            para[3].Direction = ParameterDirection.Input;
            para[4].Direction = ParameterDirection.Input;
            para[5].Direction = ParameterDirection.Output;

            para[0].Value = nYear.ToString();
            para[1].Value = lngCatalogID.ToString();
            para[2].Value = lngWTTypeID.ToString();
            para[3].Value = lngDeptID.ToString();
            para[4].Value = lngMastCustomer.ToString();

            try
            {
                //DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, sSql);

                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.StoredProcedure, "PROC_RPT_CustomerSatisfaction", para);
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
        /// 获取客户服务事件趋势数据
        /// </summary>
        /// <param name="nYear"></param>
        /// <returns></returns>
        public static DataTable GetAnalysisCSMTypeDirectionGrid(int nYear, long lngServiceTypeID, long lngDeptID, long lngMastCustomer)
        {
            //string sSql = "";

            //OracleConnection cn = ConfigTool.GetConnection();

            long lngCatalogID = lngServiceTypeID;
            string sFullID = string.Empty;
            if (lngServiceTypeID == -1 || lngServiceTypeID == 0)
                lngCatalogID = 1001;

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

                dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.StoredProcedure, "PROC_RPT_SERVER_TREND_MONTH", parms);

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
        /// 获取事件的年份
        /// </summary>
        /// <returns></returns>
        public static DataTable GetIssuesYears()
        {
            string sSql = "select distinct datepart('year',regsysdate) as years from cst_issues order by datepart('year',regsysdate) desc ";

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




        /// <summary>
        /// 获取事件涉及的管理处
        /// </summary>
        /// <returns></returns>
        public static DataTable GetManageOffices()
        {
            string sSql = "SELECT deptid,deptname FROM ts_dept WHERE deptid in (select distinct orgid from cst_issues)";

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


        /// <summary>
        /// 获取服务单位
        /// </summary>
        /// <returns></returns>
        public static DataTable GetMastCustomer()
        {
            string sSql = "SELECT ShortName,ID FROM Br_MastCustomer where Deleted=0";
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

        /// <summary>
        /// 各类服务类别事件发生趋势分析
        /// </summary>
        /// <param name="nYear"></param>
        /// <param name="lngServiceTypeID"></param>
        /// <param name="lngDeptID"></param>
        /// <param name="lngMastCustomer"></param>
        /// <returns></returns>
        public static DataTable GetAnalysisServiceTypeDirectionTypes(int nYear, long lngServiceTypeID, long lngDeptID, long lngMastCustomer)
        {
            //string sSql = "";

            //OracleConnection cn = ConfigTool.GetConnection();

            long lngCatalogID = lngServiceTypeID;
            string sFullID = string.Empty;
            if (lngServiceTypeID == -1 || lngServiceTypeID == 0)
                lngCatalogID = 1001;

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

                dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.StoredProcedure, "PROC_RPT_SERVER_TREND", parms);

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
        /// 全年度事件发生趋势分析
        /// </summary>
        /// <param name="nYear"></param>
        /// <param name="lngServiceTypeID"></param>
        /// <param name="lngDeptID"></param>
        /// <returns></returns>
        public static DataTable GetAnalysisDirection(int nYear, long lngServiceTypeID, long lngDeptID, long lngMastCustomer)
        {
            //string sSql = "";

            //OracleConnection cn = ConfigTool.GetConnection();

            long lngCatalogID = lngServiceTypeID;
            string sFullID = string.Empty;
            if (lngServiceTypeID == -1 || lngServiceTypeID == 0)
                lngCatalogID = 1001;
            //            sSql = @" declare @sFullID nvarchar(200)
            //                      select @sFullID=FullID from Es_Catalog where CatalogID=" + lngCatalogID.ToString();

            //            sSql += @"SELECT	 '事件单数'	月份,
            //                                '1月'=sum(case  when d.months=1 then d.qty else 0 end),
            //								'2月'=sum(case  when d.months=2 then d.qty else 0 end),
            //								'3月'=sum(case  when d.months=3 then d.qty else 0 end),
            //								'4月'=sum(case  when d.months=4 then d.qty else 0 end),
            //								'5月'=sum(case  when d.months=5 then d.qty else 0 end),
            //								'6月'=sum(case  when d.months=6 then d.qty else 0 end),
            //								'7月'=sum(case  when d.months=7 then d.qty else 0 end),
            //								'8月'=sum(case  when d.months=8 then d.qty else 0 end),
            //								'9月'=sum(case  when d.months=9 then d.qty else 0 end),
            //								'10月'=sum(case  when d.months=10 then d.qty else 0 end),
            //								'11月'=sum(case  when d.months=11 then d.qty else 0 end),
            //								'12月'=sum(case  when d.months=12 then d.qty else 0 end)
            //						FROM (
            //
            //						SELECT month(a.regsysdate) as months,count(a.smsid) as qty 
            //						FROM cst_issues a,Es_Catalog b
            //
            //						WHERE servicetypeid=CatalogID and year(a.regsysdate) = " + nYear.ToString() +
            //                           ((lngDeptID == 0 || lngDeptID == -1) ? "" : " AND a.orgid =" + lngDeptID + "");
            //            if (lngMastCustomer != 0)
            //                sSql += " And nvl(CustID,0) In (select ID from Br_ECustomer where MastCustID=" + lngMastCustomer.ToString() + ")";
            //            if (lngCatalogID != 1001)
            //            {
            //                sSql += " AND substring(b.FullID,0,len(@sFullID)+1)=@sFullID";
            //            }
            //            sSql += @" GROUP BY month(a.regsysdate) ) d
            //                        	";

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

                dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.StoredProcedure, "PROC_RPT_AnnualDataAnalysis", parms);

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
        /// 各类服务类别事件工时统计趋势分析
        /// </summary>
        /// <param name="nYear"></param>
        /// <param name="lngServiceTypeID"></param>
        /// <param name="lngDeptID"></param>
        /// <returns></returns>
        public static DataTable GetAnalysisServiceTypeManHourDirection(int nYear, long lngServiceTypeID, long lngDeptID)
        {
            string sSql = "";

            OracleConnection cn = ConfigTool.GetConnection();

            sSql = @"SELECT	d.servicetype as 事件类别,
								'1月'=sum(case  when d.months=1 then d.qty else 0 end),
								'2月'=sum(case  when d.months=2 then d.qty else 0 end),
								'3月'=sum(case  when d.months=3 then d.qty else 0 end),
								'4月'=sum(case  when d.months=4 then d.qty else 0 end),
								'5月'=sum(case  when d.months=5 then d.qty else 0 end),
								'6月'=sum(case  when d.months=6 then d.qty else 0 end),
								'7月'=sum(case  when d.months=7 then d.qty else 0 end),
								'8月'=sum(case  when d.months=8 then d.qty else 0 end),
								'9月'=sum(case  when d.months=9 then d.qty else 0 end),
								'10月'=sum(case  when d.months=10 then d.qty else 0 end),
								'11月'=sum(case  when d.months=11 then d.qty else 0 end),
								'12月'=sum(case  when d.months=12 then d.qty else 0 end)
						FROM (

						SELECT month(a.regsysdate) as months,a.servicetype  + '(' + TO_CHAR(a.servicetypeid) + ')' as servicetype ,sum(a.TotalHours) as qty 
						FROM cst_issues a

						WHERE   year(a.regsysdate) = " + nYear.ToString() +
                           ((lngServiceTypeID == 0) || (lngServiceTypeID == -1) ? "" : " AND a.servicetypeid =" + lngServiceTypeID + "") +
                           ((lngDeptID == 0 || lngDeptID == -1) ? "" : " AND a.orgid =" + lngDeptID + "") +
                @" GROUP BY month(a.regsysdate),a.servicetype  + '(' + TO_CHAR(a.servicetypeid) + ')' ) d
                        	GROUP BY d.servicetype";

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
        /// 各类委托类别事件发生趋势分析
        /// </summary>
        /// <param name="nYear"></param>
        /// <param name="lngWTTypeID"></param>
        /// <param name="lngDeptID"></param>
        /// <returns></returns>
        public static DataTable GetAnalysisWTTypeDirectionTypes(int nYear, long lngWTTypeID, long lngDeptID)
        {
            string sSql = "";

            OracleConnection cn = ConfigTool.GetConnection();

            sSql = @"SELECT	d.ServiceLevel as 服务级别,
								'1月'=sum(case  when d.months=1 then d.qty else 0 end),
								'2月'=sum(case  when d.months=2 then d.qty else 0 end),
								'3月'=sum(case  when d.months=3 then d.qty else 0 end),
								'4月'=sum(case  when d.months=4 then d.qty else 0 end),
								'5月'=sum(case  when d.months=5 then d.qty else 0 end),
								'6月'=sum(case  when d.months=6 then d.qty else 0 end),
								'7月'=sum(case  when d.months=7 then d.qty else 0 end),
								'8月'=sum(case  when d.months=8 then d.qty else 0 end),
								'9月'=sum(case  when d.months=9 then d.qty else 0 end),
								'10月'=sum(case  when d.months=10 then d.qty else 0 end),
								'11月'=sum(case  when d.months=11 then d.qty else 0 end),
								'12月'=sum(case  when d.months=12 then d.qty else 0 end)
						FROM (

						SELECT month(a.regsysdate) as months,a.ServiceLevel  + '(' + TO_CHAR(a.ServiceLevelID) + ')' as ServiceLevel ,count(a.smsid) as qty 
						FROM cst_issues a

						WHERE   year(a.regsysdate) = " + nYear.ToString() +
                                ((lngWTTypeID == 0) || (lngWTTypeID == -1) ? "" : " AND a.ServiceLevelID =" + lngWTTypeID + "") +
                                ((lngDeptID == 0 || lngDeptID == -1) ? "" : " AND a.orgid =" + lngDeptID + "") +
                @" GROUP BY month(a.regsysdate),a.ServiceLevel  + '(' + TO_CHAR(a.ServiceLevelID) + ')' ) d
                        	GROUP BY d.ServiceLevel";

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
        /// 服务量分布分析（事件类别）
        /// </summary>
        /// <param name="strBegin"></param>
        /// <param name="strEnd"></param>
        /// <param name="lngDeptID"></param>
        /// <param name="lngMastCustomer"></param>
        /// <param name="lngServiceTypeID"></param>
        /// <returns></returns>
        public static DataTable GetAnalysisServiceTypeAnalysis(string strBegin, string strEnd, long lngDeptID, long lngMastCustomer, long lngServiceTypeID)
        {
            OracleConnection cn = ConfigTool.GetConnection("SQLConnString");

            long lngCatalogID = lngServiceTypeID;
            string sFullID = string.Empty;
            if (lngServiceTypeID == -1)
                lngCatalogID = 1001;
            //            sSql = @" declare @sFullID nvarchar(200)
            //                      select @sFullID=FullID from Es_Catalog where CatalogID=" + lngCatalogID.ToString();
            //            sSql += @" SELECT count(a.smsid) as 数量 ,c.CatalogName  || '(' || to_char(c.CataID) || ')' as 事件类别
            //						FROM cst_issues a,(select substr(a.FullID,0,lenght(@sFullID)+1) FullID,a.CatalogID,b.CatalogID as CataID,b.CatalogName
            //                            from Es_Catalog a left outer join Es_Catalog b On substr(a.FullID,0,length(@sFullID)+7)=b.FullID) c
            //                            where a.ServiceTypeID=c.CatalogID and c.FullID=@sFullID and a.ServiceTypeID<>1001";

            //            sSql += @" and a.regsysdate Between " + StringTool.SqlQ(strBegin) + " AND " + StringTool.SqlQ(strEnd + " 23:59:59") +
            //                           ((lngDeptID == 0 || lngDeptID == -1) ? "" : " AND a.orgid =" + lngDeptID.ToString() + "");
            //            if (lngMastCustomer != 0)
            //                sSql += " And nvl(CustID,0) In (select ID from Br_ECustomer where MastCustID=" + lngMastCustomer.ToString() + ")";
            //            //if (lngServiceTypeID != 0)
            //            sSql += " GROUP BY c.CatalogName  || '(' || to_char(c.CataID) || ')' ";


            OracleParameter[] para = {
                 new OracleParameter("startDate", OracleType.VarChar , 50 ),
                 new OracleParameter("endDate", OracleType.VarChar , 50 ),
                 new OracleParameter("lngDeptID", OracleType.VarChar , 50 ),
                 new OracleParameter("lngMastCustomer", OracleType.VarChar , 50 ),
                 new OracleParameter("lngServiceTypeID", OracleType.VarChar , 50 ),
                 new OracleParameter("p_cursor",OracleType.Cursor)
            };
            para[0].Direction = ParameterDirection.Input;
            para[1].Direction = ParameterDirection.Input;
            para[2].Direction = ParameterDirection.Input;
            para[3].Direction = ParameterDirection.Input;
            para[4].Direction = ParameterDirection.Input;
            para[5].Direction = ParameterDirection.Output;

            para[0].Value = StringTool.SqlQ(strBegin);
            para[1].Value = StringTool.SqlQ(strEnd);
            para[2].Value = lngDeptID.ToString();
            para[3].Value = lngMastCustomer.ToString();
            para[4].Value = lngCatalogID.ToString();
            try
            {
                //DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, sSql);
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.StoredProcedure, "PROC_RPT_ServiceVolume", para);
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
        /// 各类服务级别事件分布分析
        /// </summary>
        /// <param name="strBegin"></param>
        /// <param name="strEnd"></param>
        /// <param name="lngDeptID"></param>
        /// <returns></returns>
        public static DataTable GetAnalysisWTTypeAnalysis(string strBegin, string strEnd, long lngDeptID)
        {
            string sSql = "";

            OracleConnection cn = ConfigTool.GetConnection();

            sSql = @"SELECT count(a.smsid) as 数量 ,a.ServiceLevel  + '(' + TO_CHAR(a.ServiceLevelID) + ')' as 服务级别
						FROM cst_issues a

						WHERE   a.regsysdate Between " + StringTool.SqlQ(strBegin) + " AND " + StringTool.SqlQ(strEnd + " 23:59:59") +
                           ((lngDeptID == 0 || lngDeptID == -1) ? "" : " AND a.orgid =" + lngDeptID + "") +
                @" GROUP BY a.ServiceLevel  + '(' + TO_CHAR(.ServiceLevelID) + ')' ";

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
        /// 获取费用情况
        /// </summary>
        /// <param name="strBeginDate"></param>
        /// <param name="strEndDate"></param>
        /// <param name="lngDeptID"></param>
        /// <param name="lngServiceTypeID"></param>
        /// <returns></returns>
        public static DataTable GetMaterialStat(string strBeginDate, string strEndDate, long lngDeptID, long lngServiceTypeID, long lngStatusID, long lngMastCustomer)
        {
            string sWhere = string.Empty;
            sWhere += " and  regsysdate Between " + StringTool.SqlQ(strBeginDate) + " AND " + StringTool.SqlQ(strEndDate + " 23:59:59") +
                           ((lngDeptID == 0 || lngDeptID == -1) ? "" : " AND orgid =" + lngDeptID) +
                           ((lngServiceTypeID == 0) || (lngServiceTypeID == -1) ? "" : "AND servicetypeid =" + lngServiceTypeID.ToString()) +
                           ((lngStatusID == 0) || (lngStatusID == -1) ? "" : "AND dealstatusid =" + lngStatusID.ToString());
            if (lngMastCustomer != 0)
                sWhere += " And nvl(MastID,0)=" + lngMastCustomer.ToString();

            string sSql = @"select nvl(a.MastID,0) '编号',nvl(a.MastName,'其它') '服务单位名称', case when Sum(nvl(Quantity,0))=0 then 0 else  Sum(nvl(FareAmount,0))/Sum(nvl(Quantity,0)) end  '单价',
 	                            Sum(nvl(Quantity,0)) '数量',Sum(nvl(FareAmount,0)) '小计费用',
	                            Sum(nvl(HumanAmount,0)) '其它费',Sum(nvl(Cst_Cost.TotalAmount,0)) '合计费用'
                         from Cst_Cost,Cst_Issues 
                         left outer join (select Br_ECustomer.ID CustomerID,Br_MastCustomer.ID MastID,Br_MastCustomer.ShortName MastName from Br_ECustomer,Br_MastCustomer where Br_ECustomer.MastCustID=Br_MastCustomer.ID) a  ON Cst_Issues.CustID=CustomerID where Cst_Issues.SMSID=Cst_Cost.SmsID " + sWhere +
                         " group by MastID,MastName ";
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



        /// <summary>
        /// 客户趋势图
        /// </summary>
        /// <param name="nYear"></param>
        /// <param name="lngServiceTypeID"></param>
        /// <param name="lngDeptID"></param>
        /// <returns></returns>
        public static DataTable GetAnalysisCustom(int nYear, long lngCustID, long lngDeptID, string sCust)
        {
            string sSql = "";

            OracleConnection cn = ConfigTool.GetConnection();

            sSql = @"SELECT	" + StringTool.SqlQ(sCust) + @"as  客户名称,
								'1月'=sum(case  when d.months=1 then d.qty else 0 end),
								'2月'=sum(case  when d.months=2 then d.qty else 0 end),
								'3月'=sum(case  when d.months=3 then d.qty else 0 end),
								'4月'=sum(case  when d.months=4 then d.qty else 0 end),
								'5月'=sum(case  when d.months=5 then d.qty else 0 end),
								'6月'=sum(case  when d.months=6 then d.qty else 0 end),
								'7月'=sum(case  when d.months=7 then d.qty else 0 end),
								'8月'=sum(case  when d.months=8 then d.qty else 0 end),
								'9月'=sum(case  when d.months=9 then d.qty else 0 end),
								'10月'=sum(case  when d.months=10 then d.qty else 0 end),
								'11月'=sum(case  when d.months=11 then d.qty else 0 end),
								'12月'=sum(case  when d.months=12 then d.qty else 0 end)
						FROM (

						SELECT month(a.regsysdate) as months,a.CustID,count(a.smsid) as qty 
						FROM cst_issues a

						WHERE  a.CustID is not null and a.CustID!=0 and year(a.regsysdate) = " + nYear.ToString() +
                           ((lngCustID == 0) ? "" : " AND a.CustID =" + lngCustID + "") +
                           ((lngDeptID == 0 || lngDeptID == -1) ? "" : " AND a.orgid =" + lngDeptID + "") +
                @" GROUP BY month(a.regsysdate),a.CustID ) d";

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

        #region 工程师工作量
        /// <summary>
        /// 工程师工作量
        /// </summary>
        /// <param name="strBeginDate"></param>
        /// <param name="strEndDate"></param>
        /// <param name="lngStatusID"></param>
        /// <param name="lngMastCustomer"></param>
        /// <returns></returns>
        public static DataTable StatStaffWorks(string strBeginDate, string strEndDate, long lngStatusID, long lngMastCustomer)
        {
            string sWhere = string.Empty;

            if (strBeginDate != "")
                sWhere += " and  regsysdate >= to_date(" + StringTool.SqlQ(strBeginDate) + ",'yyyy-MM-dd   hh24:mi:ss')";
            if (strEndDate != "")
                sWhere += " and  regsysdate <= to_date(" + StringTool.SqlQ(strEndDate) + ",'yyyy-MM-dd   hh24:mi:ss')";

            sWhere += ((lngStatusID == 0) || (lngStatusID == -1) ? "" : " AND dealstatusid =" + lngStatusID.ToString());
            string sSql = @"select a.ID,a.Name,a.BlongDeptName,a.Remark,a.UserName,Faculty,JoinDate,
                            nvl(b.Num,0) Num,nvl(b.TotalHours,0) TotalHours,nvl(b.TotalAmount,0) TotalAmount
                            from Cst_ServiceStaff a
                            left outer join 
                            (select Count(ServiceStaffID) Num,Sum(b.TotalHours) TotalHours,
                            Sum(TotalAmount) TotalAmount,ServiceStaffID,ServiceStaffName 
                            from Cst_ServiceStaffList a,Cst_Issues b
                            where a.FlowID=b.FlowID";
            sSql += sWhere + @" Group By ServiceStaffID,ServiceStaffName ) b
                             ON  a.ID=b.ServiceStaffID Where 1=1  and a.deleted=0 ";
            if (lngMastCustomer != 0)
            {
                sSql += " And BlongDeptId=" + lngMastCustomer;
            }

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

        #region 工程师工作情况分析
        /// <summary>
        /// 工程师工作情况分析
        /// </summary>
        /// <param name="strBeginDate"></param>
        /// <param name="strEndDate"></param>
        /// <param name="lngStatusID"></param>
        /// <param name="lngMastCustomer"></param>
        /// <returns></returns>
        public static DataTable EngineerAnalysis(string strBeginDate, string strEndDate, long lngStatusID, long lngMastCustomer)
        {
            string sWhere = string.Empty;
            if (strBeginDate != "")
                sWhere += " and  c.regsysdate >= to_date(" + StringTool.SqlQ(strBeginDate) + ",'yyyy-MM-dd   hh24:mi:ss')";
            if (strEndDate != "")
                sWhere += " and  c.regsysdate <= to_date(" + StringTool.SqlQ(strEndDate) + ",'yyyy-MM-dd   hh24:mi:ss')";

            sWhere += ((lngStatusID == 0) || (lngStatusID == -1) ? "" : " AND dealstatusid =" + lngStatusID.ToString());
            //if (lngMastCustomer != 0)
            //    sWhere += " And nvl(CustID,0) In (select ID from Br_ECustomer where MastCustID=" + lngMastCustomer.ToString() + ")";
            string sSql = @"select a.ID,a.Name,a.BlongDeptName,a.Remark,a.UserName,Faculty,JoinDate,
                            nvl(b.Num,0) Num,Case when nvl(b.Num,0)=0 then '是' else '否' end IsnullWork
                            from Cst_ServiceStaff a
                            Left outer join (
                            select count(a.ID) Num,a.ID,Name,BlongDeptID,BlongDeptName 
                            from Cst_ServiceStaff a,Cst_ServiceStaffList b,Cst_Issues c
                            where b.ServiceStaffID=a.ID and b.FlowID=c.FlowID and b.NewFlag=1 ";
            sSql += sWhere + @" Group By a.ID,Name,BlongDeptID,BlongDeptName
                                ) b ON a.ID=b.ID Where 1=1  and a.deleted=0 ";
            if (lngMastCustomer != 0)
            {
                sSql += " And a.BlongDeptId=" + lngMastCustomer;
            }

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

        /// <summary>
        /// 事件超期（LSA）列表
        /// </summary>
        /// <param name="strBeginDate"></param>
        /// <param name="strEndDate"></param>
        /// <param name="lngStatusID"></param>
        /// <param name="lngMastID"></param>
        /// <returns></returns>
        public static DataTable LSAOverTimeReport(string strBeginDate, string strEndDate, long lngStatusID, long lngMastID)
        {
            string sWhere = string.Empty;
            //            if (strBeginDate != "")
            //                sWhere += " and  a.CustTime >= " + StringTool.SqlQ(strBeginDate);
            //            if (strEndDate != "")
            //                sWhere += " and  a.CustTime <= " + StringTool.SqlQ(strEndDate);

            //            sWhere += ((lngStatusID == 0) || (lngStatusID == -1) ? "" : " AND a.dealstatusid =" + lngStatusID.ToString()) +
            //                           ((lngMastID == 0) || (lngMastID == -1) ? "" : " AND h.MastID =" + lngMastID.ToString());
            //            string sSql = @"SELECT 
            //                      b.ID, b.LevelName, a.SMSID, a.FlowID, a.NodeModelID, a.FlowModelID, a.BuildCode + a.ServiceNo AS ServiceNo, a.Subject, a.Content, a.RegUserID, 
            //                      a.RegUserName, a.RegDeptID, a.RegDeptName, a.ServiceLevelID, a.ServiceLevel, a.ServiceTypeID, a.ServiceType, a.ServiceKindID, a.ServiceKind, 
            //                      a.EffectID, a.EffectName, a.InstancyID, a.InstancyName, a.DealStatusID, a.DealStatus, a.CustTime, a.CustID, a.CustName, a.CustAddress, a.Contact, 
            //                      a.CTel, a.EquipmentID, a.EquipmentName, a.EquipmentCatalogName, a.EquipmentItemCode, a.EquipmentSN, a.DealContent, a.Outtime, a.ServiceTime, 
            //                      a.FinishedTime, a.SjwxrID, a.Sjwxr, a.BuildCode, a.TotalHours, a.TotalAmount, a.OrgID, a.ChangeProblemFlowID, a.ChangeProblem, a.RegSysDate, 
            //                      a.RegSysUserID, a.RegSysUser, h.CustomCode
            //                        FROM         (SELECT     Br_ECustomer.ID AS CustomerID, Br_MastCustomer.ID AS MastID, Br_MastCustomer.ShortName AS MastName, 
            //                                                                      Br_ECustomer.CustomCode
            //                                               FROM          Br_ECustomer INNER JOIN
            //                                                                      Br_MastCustomer ON Br_ECustomer.MastCustID = Br_MastCustomer.ID) AS h RIGHT OUTER JOIN
            //                                              Cst_Issues AS a INNER JOIN
            //                                              Cst_ServiceLevel AS b ON a.ServiceLevelID = b.ID 
            //                                              INNER JOIN
            //                                              ( select distinct(d.FlowID) from Cst_Issues as d
            //                                                 inner join EA_FlowBusLimit AS c 
            //                                                 on d.FlowID = c.FlowID
            //	                                         WHERE (CASE WHEN c.GuidID = 10002 THEN datediff('Minute', LimitTime, nvl(Outtime, sysdate)) 
            //	    		                                     WHEN c.GuidID = 10001 THEN datediff('Minute', LimitTime, nvl(FinishedTime, sysdate)) END > 0
            //                                               ) )e        
            //                        ON a.FlowID = e.FlowID 
            //                        ON h.CustomerID = a.CustID
            //                        WHERE 1=1 ";
            //            sSql += sWhere;
            //            sSql += "ORDER BY a.BuildCode + a.ServiceNo DESC ";

            OracleConnection cn = ConfigTool.GetConnection("SQLConnString");
            OracleParameter[] para = {
                 new OracleParameter("p_strWhere", OracleType.VarChar , 2000 ),               
                 new OracleParameter("p_cursor",OracleType.Cursor)
            };
            para[0].Direction = ParameterDirection.Input;
            para[1].Direction = ParameterDirection.Output;

            string strWhere = "";
            if (strBeginDate.Trim() != "")
                strWhere += " AND a.CustTime >= to_date(" + StringTool.SqlQ(strBeginDate.ToString()) + ",'yyyy-MM-dd HH24:mi:ss')";
            if (strEndDate.Trim() != "")
            {
                strEndDate = strEndDate;
                strWhere += " AND a.CustTime <= to_date(" + StringTool.SqlQ(strEndDate.ToString()) + ",'yyyy-MM-dd HH24:mi:ss')";
            }
            if (lngStatusID > 0)
            {
                strWhere += " and a.dealstatusid = " + lngStatusID;
            }

            if (lngMastID > 0)
            {
                strWhere += " And h.MastID=" + lngMastID;
            }

            para[0].Value = strWhere;

            try
            {
                //DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, sSql);

                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.StoredProcedure, "PROC_EventLimitTime", para);
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
        /// SLA达标率
        /// </summary>
        /// <param name="strBeginDate"></param>
        /// <param name="strEndDate"></param>
        /// <param name="lngStatusID"></param>
        /// <param name="lngMastID"></param>
        /// <returns></returns>
        public static DataTable SLAMark(string strBeginDate, string strEndDate, long lngStatusID, long lngMastID, string prcName)
        {
            string strWhere = string.Empty;
            //            if (strBeginDate != "")
            //                sWhere += " and  a.CustTime >= " + StringTool.SqlQ(strBeginDate);
            //            if (strEndDate != "")
            //                sWhere += " and  a.CustTime <= " + StringTool.SqlQ(strEndDate);

            //            sWhere += ((lngStatusID == 0) || (lngStatusID == -1) ? "" : " AND a.dealstatusid =" + lngStatusID.ToString()) +
            //                           ((lngMastID == 0) || (lngMastID == -1) ? "" : " AND h.MastID =" + lngMastID.ToString());
            //            string sSql = @"SELECT     COUNT(a_1.ID) AS icount, COUNT(a_1.ID) - SUM(a_1.超过响应时间) AS iRespond, SUM(a_1.超过响应时间) AS iRespondOver, a_1.ID, a_1.LevelName, 
            //                      a_1.GuidName, a_1.GuidID, Cast((COUNT(a_1.ID) - SUM(a_1.超过响应时间)) / CAST(COUNT(a_1.ID) AS decimal(18, 2)) AS decimal(18, 2)) * 100 AS irate, b.Target, 
            //                      CASE b.TimeUnit WHEN 0 THEN b.TimeLiMit / 60.00 WHEN 1 THEN b.TimeLiMit WHEN 2 THEN b.TimeLiMit * 24 WHEN 3 THEN b.TimeLiMit / 60.00 ELSE
            //                       b.TimeLiMit END AS TimeLimit
            //                        FROM         (SELECT     CASE WHEN datediff('Minute', e.LimitTime, nvl(Outtime, sysdate)) > 0 AND e.GuidID = 10002 THEN 1 WHEN datediff(Minute, e.LimitTime, 
            //                                              nvl(FinishedTime, sysdate)) > 0 AND e.GuidID = 10001 THEN 1 ELSE 0 END AS 超过响应时间, 
            //                                              CASE WHEN e.GuidID = 10002 THEN datediff('Minute', e.LimitTime, nvl(Outtime, sysdate)) WHEN e.GuidID = 10001 THEN datediff(Minute, 
            //                                              e.LimitTime, nvl(FinishedTime, sysdate)) END AS TimeLimit, d.GuidName, e.GuidID, b.ID, b.LevelName
            //                       FROM          Cst_ServiceLevel AS b INNER JOIN
            //                                              Cst_Issues AS a LEFT OUTER JOIN
            //                                                  (SELECT     Br_ECustomer.ID AS CustomerID, Br_MastCustomer.ID AS MastID, Br_MastCustomer.ShortName AS MastName, 
            //                                                                           Br_ECustomer.CustomCode
            //                                                    FROM          Br_ECustomer INNER JOIN
            //                                                                           Br_MastCustomer ON Br_ECustomer.MastCustID = Br_MastCustomer.ID) AS h ON a.CustID = h.CustomerID ON 
            //                                              b.ID = a.ServiceLevelID INNER JOIN
            //                                              EA_FlowBusLimit AS e ON a.FlowID = e.FlowID INNER JOIN
            //                                              Cst_GuidDefinition AS d ON e.GuidID = d.GuidID
            //                       WHERE 1=1 ";
            //            sSql += sWhere;
            //            sSql += @") AS a_1 INNER JOIN
            //                      Cst_SLGuid AS b ON a_1.GuidID = b.GuidID AND a_1.ID = b.LevelID
            //                        GROUP BY a_1.ID, a_1.LevelName, a_1.GuidID, a_1.GuidName, b.Target, b.TimeLimit, b.TimeUnit
            //                        ORDER BY a_1.ID";

            OracleConnection cn = ConfigTool.GetConnection("SQLConnString");
            OracleParameter[] para = {
                 new OracleParameter("p_strWhere", OracleType.VarChar , 2000 ),               
                 new OracleParameter("p_cursor",OracleType.Cursor)
            };
            para[0].Direction = ParameterDirection.Input;
            para[1].Direction = ParameterDirection.Output;

            if (strBeginDate.Trim() != "")
                strWhere += " AND a.regsysdate >= to_date(" + StringTool.SqlQ(strBeginDate.ToString()) + ",'yyyy-MM-dd HH24:mi:ss')";
            if (strEndDate.Trim() != "")
            {
                strEndDate = strEndDate;
                strWhere += " AND a.regsysdate <= to_date(" + StringTool.SqlQ(strEndDate.ToString()) + ",'yyyy-MM-dd HH24:mi:ss')";
            }
            if (lngStatusID > 0)
            {
                strWhere += " and a.dealstatusid = " + lngStatusID;
            }

            if (lngMastID > 0)
            {
                strWhere += " And h.ServiceTypeID=" + lngMastID;
            }

            para[0].Value = strWhere;

            try
            {
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.StoredProcedure, prcName, para);
                return dt;
            }
            catch (Exception e)
            {
                E8Logger.Error(prcName);
                E8Logger.Error(strWhere);
                throw e;
            }
            finally
            {
                ConfigTool.CloseConnection(cn);
            }
        }

        #region 资产故障率分析
        /// <summary>
        /// 资产故障率分析
        /// </summary>
        /// <param name="strBegin"></param>
        /// <param name="strEnd"></param>
        /// <param name="lngMastCustomer"></param>
        /// <returns></returns>
        public static DataTable GetEquFalseRatio(string strBegin, string strEnd, long lngMastCustomer)
        {
            //string sSql = string.Empty;
            OracleConnection cn = ConfigTool.GetConnection();

            //            sSql += @" declare @num1 decimal(18,0)
            //                        SELECT @num1=Count(EquipMentID) FROM (SELECT distinct EquipMentID FROM cst_issues a where EquipMentID<>0 and EquipMentID<>-1";

            //            if (strBegin != "")
            //                sSql += " and  a.regsysdate >= " + StringTool.SqlQ(strBegin);
            //            if (strEnd != "")
            //                sSql += " and  a.regsysdate <= " + StringTool.SqlQ(strEnd);

            //            //sSql += @" and a.regsysdate Between " + StringTool.SqlQ(strBegin) + " AND " + StringTool.SqlQ(strEnd);
            //            if (lngMastCustomer != 0)
            //                sSql += " And nvl(CustID,0) In (select ID from Br_ECustomer where MastCustID=" + lngMastCustomer.ToString() + ")";
            //            sSql += @") a select @num1 '数量','已维修' 资产故障
            //                        union all
            //                        select  Count(ID)-@num1,'未维修' from Equ_Desk where Deleted=0 ";
            //            if (lngMastCustomer != 0)
            //                sSql += " And nvl(Costom,0) In (select ID from Br_ECustomer where MastCustID=" + lngMastCustomer.ToString() + ")";
            OracleParameter[] para = {
                    new OracleParameter("startDate", OracleType.VarChar ,50),
					new OracleParameter("endDate", OracleType.VarChar , 50),
                    new OracleParameter("mastCustomerID", OracleType.VarChar , 50 ),
                                            new OracleParameter("p_cursor",OracleType.Cursor)};
            para[0].Direction = ParameterDirection.Input;
            para[1].Direction = ParameterDirection.Input;
            para[2].Direction = ParameterDirection.Input;
            para[3].Direction = ParameterDirection.Output;

            para[0].Value = StringTool.SqlQ(strBegin);
            para[1].Value = StringTool.SqlQ(strEnd);
            para[2].Value = lngMastCustomer.ToString();

            try
            {
                //DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.StoredProcedure , sSql);
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.StoredProcedure, "PROC_FaultRate", para);
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

        #region 首次解次率分析
        /// <summary>
        /// 首次解次率分析
        /// </summary>
        /// <param name="strBegin"></param>
        /// <param name="strEnd"></param>
        /// <param name="lngMastCustomer"></param>
        /// <returns></returns>
        public static DataTable GetFirstRatio(string strBegin, string strEnd, long lngMastCustomer)
        {
            string sSql = string.Empty;
            OracleConnection cn = ConfigTool.GetConnection();

            sSql += @" select Count(SmsID) 数量,'首次解决' 首次解决 FROM cst_issues a  where nvl(IsFirstSolve,0)=1";

            if (strBegin != "")
                sSql += " and  a.regsysdate >= to_date(" + StringTool.SqlQ(strBegin) + ",'yyyy-MM-dd HH24:mi:ss')";
            if (strEnd != "")
                sSql += " and  a.regsysdate <=to_date(" + StringTool.SqlQ(strEnd) + ",'yyyy-MM-dd HH24:mi:ss')";

            //sSql += @" and a.regsysdate Between " + StringTool.SqlQ(strBegin) + " AND " + StringTool.SqlQ(strEnd);
            if (lngMastCustomer != 0)
                sSql += " And nvl(CustID,0) In (select ID from Br_ECustomer where MastCustID=" + lngMastCustomer.ToString() + ")";
            sSql += @" union all
                        select Count(SmsID),'首次未解决' FROM cst_issues a  where nvl(IsFirstSolve,0)=0";

            if (strBegin != "")
                sSql += " and  a.regsysdate >= to_date(" + StringTool.SqlQ(strBegin) + ",'yyyy-MM-dd HH24:mi:ss')";
            if (strEnd != "")
                sSql += " and  a.regsysdate <=to_date(" + StringTool.SqlQ(strEnd) + ",'yyyy-MM-dd HH24:mi:ss')";

            //sSql += @" and a.regsysdate Between " + StringTool.SqlQ(strBegin) + " AND " + StringTool.SqlQ(strEnd);
            if (lngMastCustomer != 0)
                sSql += " And nvl(CustID,0) In (select ID from Br_ECustomer where MastCustID=" + lngMastCustomer.ToString() + ")";
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

        #region 工程师工作分析
        /// <summary>
        /// 工程师工作分析
        /// </summary>
        /// <param name="strBegin"></param>
        /// <param name="strEnd"></param>
        /// <param name="lngMastCustomer"></param>
        /// <returns></returns>
        public static DataTable GetEngIneerWork(string strBegin, string strEnd, long lngMastCustomer)
        {
            string sSql = string.Empty;
            OracleConnection cn = ConfigTool.GetConnection();


            //            sSql += @" declare @unNum decimal(18,0)
            //                        select @unNum=Count(SmsID)
            //                        from Cst_Issues a where (SjwxrID='0' or SjwxrID='')";
            //            if (strBegin != "")
            //                sSql += " and  a.regsysdate >= " + StringTool.SqlQ(strBegin);
            //            if (strEnd != "")
            //                sSql += " and  a.regsysdate <= " + StringTool.SqlQ(strEnd);

            //            //sSql += @" and a.regsysdate Between " + StringTool.SqlQ(strBegin) + " AND " + StringTool.SqlQ(strEnd);
            //            if (lngMastCustomer != 0)
            //                sSql += " And nvl(CustID,0) In (select ID from Br_ECustomer where MastCustID=" + lngMastCustomer.ToString() + ")";
            //            sSql += @" declare @DealNum decimal(18,0)
            //                        select @DealNum=Count(FlowID) from
            //                        (select distinct a.FlowID
            //                        from Cst_ServiceStaffList a,Cst_ServiceStaff b,Cst_Issues c
            //                        where a.ServiceStaffID=b.ID and a.FlowID=c.FlowID and NewFlag=1";
            //            //sSql += @" and c.regsysdate Between " + StringTool.SqlQ(strBegin) + " AND " + StringTool.SqlQ(strEnd);
            //            if (strBegin != "")
            //                sSql += " and  c.regsysdate >= " + StringTool.SqlQ(strBegin);
            //            if (strEnd != "")
            //                sSql += " and  c.regsysdate <= " + StringTool.SqlQ(strEnd);

            //            if (lngMastCustomer != 0)
            //                sSql += " And nvl(CustID,0) In (select ID from Br_ECustomer where MastCustID=" + lngMastCustomer.ToString() + ")";
            //            sSql += ") d";

            //            sSql += @" select Count(a.SmsID)-@unNum-@DealNum '数量',
            //                        '工程师已处理' as '处理情况'
            //                        from Cst_Issues a where 1=1";
            //            //sSql += @" and a.regsysdate Between " + StringTool.SqlQ(strBegin) + " AND " + StringTool.SqlQ(strEnd);
            //            if (strBegin != "")
            //                sSql += " and  a.regsysdate >= " + StringTool.SqlQ(strBegin);
            //            if (strEnd != "")
            //                sSql += " and  a.regsysdate <= " + StringTool.SqlQ(strEnd);

            //            if (lngMastCustomer != 0)
            //                sSql += " And nvl(CustID,0) In (select ID from Br_ECustomer where MastCustID=" + lngMastCustomer.ToString() + ")";
            //            sSql += @" union all
            //                        select @DealNum,'工程师处理中' 
            //                       union all
            //                        select @unNum,'工程师未处理' ";

            OracleParameter[] para = {
                    new OracleParameter("startDate", OracleType.VarChar ,50),
					new OracleParameter("endDate", OracleType.VarChar , 50),
                    new OracleParameter("lngMastCustomer", OracleType.VarChar , 50 ),
                    new OracleParameter("p_cursor",OracleType.Cursor)};
            para[0].Direction = ParameterDirection.Input;
            para[1].Direction = ParameterDirection.Input;
            para[2].Direction = ParameterDirection.Input;
            para[3].Direction = ParameterDirection.Output;

            para[0].Value = StringTool.SqlQ(strBegin);
            para[1].Value = StringTool.SqlQ(strEnd);
            para[2].Value = lngMastCustomer.ToString();

            try
            {
                //DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.StoredProcedure , sSql);
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.StoredProcedure, "PROC_AnalysisWork", para);
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
        #endregion

        #region 费用明细相关
        /// <summary>
        /// 获得差旅费用明细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static DataTable GetCLFareItem(long id)
        {
            string sSql = @"SELECT * FROM cst_cost WHERE smsid = " + id.ToString();


            DataTable dt = CommonDP.ExcuteSqlTable(sSql);

            return dt;
        }



        /// <summary>
        /// 保存费用明细
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="strNo"></param>
        /// <returns></returns>
        public static long SaveCLFareDetailItem(DataTable dt, long lngID)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            if (cn.State != ConnectionState.Open) { cn.Open(); }
            OracleTransaction tran = cn.BeginTransaction();
            if (lngID != 0)
            {
                DeleteCLFareDetailItem(lngID, tran); //先删除再新增
            }
            else
            {
                lngID = EPGlobal.GetNextID("ZHService_ID");
            }
            if (dt.Rows.Count < 1)
            {
                return lngID;
            }

            for (int n = 0; n < dt.Rows.Count; n++)
            {
                dt.Rows[n]["smsid"] = lngID;
            }
            //save
            try
            {
                // string scn = ConfigTool.GetConnectString();
                string sSql = "SELECT * FROM cst_cost Where smsid=" + lngID;
                OracleDataAdapter da = new OracleDataAdapter(sSql, cn);
                OracleCommandBuilder cb = new OracleCommandBuilder(da);
                da.SelectCommand.Transaction = tran;
                cb.GetInsertCommand().Transaction = tran;
                da.Update(dt);

                tran.Commit();

                return lngID;
            }
            catch (Exception e)
            {
                tran.Rollback();
                throw e;
            }
            finally
            {
                tran.Dispose();
                ConfigTool.CloseConnection(cn);
            }
        }

        /// <summary>
        /// 删除费用明细
        /// </summary>
        /// <param name="strNo"></param>
        /// <param name="tran"></param>
        public static void DeleteCLFareDetailItem(long lngID, OracleTransaction tran)
        {

            string sSql = "Delete cst_cost Where smsid=" + lngID.ToString();
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
        /// 删除费用明细
        /// </summary>
        /// <param name="costID"></param>
        /// <returns></returns>
        public static bool DeleteCLFareDetailItem(long costID)
        {
            if (costID == 0)
                return true;
            bool result = true;
            string sSql = "Delete cst_cost Where costid=" + costID.ToString();
            try
            {
                OracleDbHelper.ExecuteNonQuery(ConfigTool.GetConnectString(), CommandType.Text, sSql);
            }
            catch
            {
                result = false;
            }
            return result;
        }
        #endregion

        #region 事件跟踪相关查询
        /// <summary>
        /// 超时事件
        /// </summary>
        /// <param name="lngUserID"></param>
        /// <param name="lngDeptID"></param>
        /// <param name="lngOrgID"></param>
        /// <param name="tp"></param>
        /// <param name="iOverHours">超时时间(小时)</param>
        /// <param name="re">权限</param>
        /// <returns></returns>
        public static DataTable GetIssuesForOverTime(long lngUserID, long lngDeptID, long lngOrgID, eOA_TracePeriod tp, int iOverHours, RightEntity re)
        {

            string strSQL = "";
            string strList = "";


            strSQL = @"SELECT  nvl(buildCode,'')||nvl(a.ServiceNo,'') ServiceNo,a.smsid,a.flowid,a.regsysdate,a.subject,a.reguserid,a.Content,a.DealContent,a.CustAddress,a.ServiceTypeID,a.ServiceType,a.Sjwxr,
                      a.regusername,a.RegDeptID,a.RegDeptName,a.CustTime,a.CustName,a.contact,a.ctel,b.status,b.endtime,a.EquipmentCatalogName,a.EquipmentName,a.ReportingTime,a.FinishedTime,a.ServiceKind,
                      a.ServiceLevel,a.EffectName,a.InstancyName,a.CloseReasonName,a.ReSouseName,a.Outtime,a.ServiceTime,
                      datediff('Minute',sysdate,nvl(b.expectendtime,sysdate)) as FlowDiffMinute,a.dealstatus,a.ChangeProblemFlowID,d.customcode,(case a.MastCust when '' then d.MastCustName
				      when null then d.MastCustName
				      else a.mastCust end ) as MastCustName   " +
                      " FROM es_flow b,Cst_Issues a left join br_ecustomer d on a.custid = d.id " +
                      " WHERE a.FlowID = b.FlowID ";

            if (re.CanRead == false)
            {
                //查询出空结果
                strSQL += " AND a.flowid = -1 ";
            }
            else
            {
                #region 范围条件
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


                #endregion

                switch (tp)
                {
                    case eOA_TracePeriod.eMonth:
                        strSQL += " AND a.RegSysDate >= to_date(to_char(DateAdd('month',-1,sysdate),'yyyy-MM-dd'),'yyyy-MM-dd') ";
                        break;
                    case eOA_TracePeriod.eSeason:
                        strSQL += " AND a.RegSysDate >= dateadd('month',-3,sysdate) ";
                        break;
                    case eOA_TracePeriod.eHalfYear:
                        strSQL += " AND a.RegSysDate >= dateadd('month',-6,sysdate)";
                        break;
                    case eOA_TracePeriod.eYear:
                        strSQL += " AND a.RegSysDate >= dateadd('year',-1,sysdate)";
                        break;
                    default:
                        break;
                }
                strSQL += " AND ((datediff('Minute',sysdate,nvl(b.expectendtime,sysdate)) < (0- " + (iOverHours * 60).ToString() + ") AND " +
                          " b.status <> " + ((int)e_FlowStatus.efsEnd).ToString() + ")  OR " +
                          "( datediff('Minute',nvl(b.endtime,sysdate),nvl(b.expectendtime,sysdate)) < (0- " + (iOverHours * 60).ToString() + ")" +
                          " AND b.status = " + ((int)e_FlowStatus.efsEnd).ToString() + ") ) ORDER BY a.smsid DESC";
            }
            OracleConnection cn = ConfigTool.GetConnection();
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigTool.CloseConnection(cn);
            return dt;
        }

        /// <summary>
        /// 超时事件
        /// </summary>
        /// <param name="lngUserID"></param>
        /// <param name="lngDeptID"></param>
        /// <param name="lngOrgID"></param>
        /// <param name="tp"></param>
        /// <param name="iOverHours"></param>
        /// <param name="re"></param>
        /// <param name="pagesize"></param>
        /// <param name="pageindex"></param>
        /// <param name="rowcount"></param>
        /// <returns></returns>
        public DataTable GetIssuesForOverTime(long lngUserID, long lngDeptID, long lngOrgID, eOA_TracePeriod tp, int iOverHours, RightEntity re, int pagesize, int pageindex, ref int rowcount)
        {

            string strWhere = " 1=1";
            string strList = "";
            if (re.CanRead == false)
            {
                //查询出空结果
                strWhere += " AND flowid = -1 ";
            }
            else
            {
                #region 范围条件
                switch (re.RightRange)
                {
                    case eO_RightRange.eFull:
                        strWhere += "";
                        break;
                    case eO_RightRange.ePersonal:
                        strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_CST_Issue.flowid AND receiverid = " + lngUserID.ToString() + ")";
                        break;
                    case eO_RightRange.eDeptDirect:
                        strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_CST_Issue.flowid AND recdeptid = " + lngDeptID.ToString() + ")";
                        break;
                    case eO_RightRange.eOrgDirect:
                        strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_CST_Issue.flowid AND recorgid = " + lngOrgID.ToString() + ")";
                        break;
                    case eO_RightRange.eDept:
                        strList = DeptDP.GetDeptFullID(lngDeptID);
                        if (strList.Trim().Length > 0)
                        {
                            //不是根部门并有找到
                            strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_CST_Issue.flowid AND recdeptid in (select deptid from ts_dept where fullid like " + StringTool.SqlQ(strList + "%") + "))";
                        }
                        break;
                    case eO_RightRange.eOrg:
                        strList = DeptDP.GetDeptFullID(lngOrgID);
                        if (strList.Trim().Length > 0)
                        {
                            //不是根部门并有找到
                            strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_CST_Issue.flowid AND recorgid in (select distinct deptid from ts_dept where deptkind = 1 and fullid like " + StringTool.SqlQ(strList + "%") + "))";
                        }
                        break;
                    default:
                        strWhere += "";
                        break;
                }


                #endregion

                switch (tp)
                {
                    case eOA_TracePeriod.eMonth:
                        strWhere += " AND RegSysDate >= to_date(to_char(DateAdd('month',-1,sysdate),'yyyy-MM-dd'),'yyyy-MM-dd') ";
                        break;
                    case eOA_TracePeriod.eSeason:
                        strWhere += " AND RegSysDate >= dateadd('month',-3,sysdate)";
                        break;
                    case eOA_TracePeriod.eHalfYear:
                        strWhere += " AND RegSysDate >= dateadd('month',-6,sysdate)";
                        break;
                    case eOA_TracePeriod.eYear:
                        strWhere += " AND RegSysDate >=dateadd('year',-1,sysdate)";
                        break;
                    default:
                        break;
                }
                strWhere += " AND ((datediff('Minute',sysdate,nvl(expectendtime,sysdate)) < (0- " + (iOverHours * 60).ToString() + ") AND " +
                          " status <> " + ((int)e_FlowStatus.efsEnd).ToString() + ")  OR " +
                          "( datediff('Minute',nvl(endtime,sysdate),nvl(expectendtime,sysdate)) < (0- " + (iOverHours * 60).ToString() + ")" +
                          " AND status = " + ((int)e_FlowStatus.efsEnd).ToString() + ") ) ";
            }
            OracleConnection cn = ConfigTool.GetConnection();
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, "V_CST_Issue", "*", " ORDER BY smsid DESC", pagesize, pageindex, strWhere, ref rowcount);
            ConfigTool.CloseConnection(cn);
            return dt;
        }


        /// <summary>
        /// 超时事件
        /// </summary>
        /// <param name="lngUserID"></param>
        /// <param name="lngDeptID"></param>
        /// <param name="lngOrgID"></param>
        /// <param name="tp"></param>
        /// <param name="iOverHours"></param>
        /// <param name="re"></param>
        /// <param name="pagesize"></param>
        /// <param name="pageindex"></param>
        /// <param name="rowcount"></param>
        /// <returns></returns>
        public DataTable GetIssuesForOverTime(long lngUserID, eOA_TracePeriod tp, int iOverHours, int pagesize, int pageindex, ref int rowcount)
        {

            string strWhere = " 1=1 AND RegUserID= " + lngUserID.ToString();

            switch (tp)
            {
                case eOA_TracePeriod.eMonth:
                    strWhere += " AND RegSysDate >= to_date(to_char(DateAdd('month',-1,sysdate),'yyyy-MM-dd'),'yyyy-MM-dd') ";
                    break;
                case eOA_TracePeriod.eSeason:
                    strWhere += " AND RegSysDate >= dateadd('month',-3,sysdate) ";
                    break;
                case eOA_TracePeriod.eHalfYear:
                    strWhere += " AND RegSysDate >= dateadd('month',-6,sysdate)";
                    break;
                case eOA_TracePeriod.eYear:
                    strWhere += " AND RegSysDate >= dateadd('year',-1,sysdate)";
                    break;
                default:
                    break;
            }

            strWhere += " AND ((datediff('Minute',sysdate,nvl(expectendtime,sysdate)) < (0- " + (iOverHours * 60).ToString() + ") AND " +
                      " status <> " + ((int)e_FlowStatus.efsEnd).ToString() + ")  OR " +
                      "( datediff('Minute',nvl(endtime,sysdate),nvl(expectendtime,sysdate)) < (0- " + (iOverHours * 60).ToString() + ")" +
                      " AND status = " + ((int)e_FlowStatus.efsEnd).ToString() + ") ) ";
            OracleConnection cn = ConfigTool.GetConnection();
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, "V_CST_Issue", "*", " ORDER BY smsid DESC", pagesize, pageindex, strWhere, ref rowcount);
            ConfigTool.CloseConnection(cn);
            return dt;
        }



        /// <summary>
        /// 未回访
        /// </summary>
        /// <param name="lngUserID"></param>
        /// <param name="lngDeptID"></param>
        /// <param name="lngOrgID"></param>
        /// <param name="tp"></param>
        /// <param name="re"></param>
        /// <param name="iPageSize"></param>
        /// <returns></returns>
        public static DataTable GetIssuesForUnFeedBack(long lngUserID, long lngDeptID, long lngOrgID, eOA_TracePeriod tp, RightEntity re, int iPageSize)
        {
            string strSQL = "";
            string strList = "";
            strSQL = @"SELECT  nvl(buildCode,'')||nvl(a.ServiceNo,'') ServiceNo,a.smsid,a.flowid,a.regsysdate,a.subject,a.reguserid,a.Content,a.DealContent,a.CustAddress,a.ServiceTypeID,a.ServiceType,a.Sjwxr,
                      a.regusername,a.RegDeptID,a.RegDeptName,a.CustTime,a.CustName,a.contact,a.ctel,b.status,b.endtime,a.EquipmentCatalogName,a.EquipmentName,a.ReportingTime,a.FinishedTime,a.ServiceKind,
                    a.ServiceLevel,a.EffectName,a.InstancyName,a.CloseReasonName,a.ReSouseName,a.Outtime,a.ServiceTime,
                   datediff('Minute',sysdate,nvl(b.expectendtime,sysdate)) as FlowDiffMinute,a.dealstatus,a.ChangeProblemFlowID,d.customcode,(case a.MastCust when '' then d.MastCustName
				when null then d.MastCustName
				else a.mastCust end ) as MastCustName   " +
              " FROM es_flow b,Cst_Issues a left join br_ecustomer d on a.custid = d.id " +
              " WHERE a.FlowID = b.FlowID " + (iPageSize == 0 ? "" : " and rownum<=" + iPageSize.ToString());
            if (re.CanRead == false)
            {
                //查询出空结果
                strSQL += " AND a.flowid = -1 ";
            }
            else
            {
                #region 范围条件
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
                #endregion

                switch (tp)
                {
                    case eOA_TracePeriod.eMonth:
                        strSQL += " AND a.RegSysDate >= to_date(to_char(DateAdd('month',-1,sysdate),'yyyy-MM-dd'),'yyyy-MM-dd') ";
                        break;
                    case eOA_TracePeriod.eSeason:
                        strSQL += " AND a.RegSysDate >= dateadd('month',-3,sysdate)";
                        break;
                    case eOA_TracePeriod.eHalfYear:
                        strSQL += " AND a.RegSysDate >= dateadd('month',-6,sysdate)";
                        break;
                    case eOA_TracePeriod.eYear:
                        strSQL += " AND a.RegSysDate >= dateadd('year',-1,sysdate) ";
                        break;
                    default:
                        break;
                }
                strSQL += " AND not exists(select feedbackid from  ea_issues_feedback where flowid = a.flowid )  AND b.status= " + ((int)e_FlowStatus.efsEnd).ToString() +
                        " ORDER BY a.smsid DESC";
            }
            OracleConnection cn = ConfigTool.GetConnection();
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigTool.CloseConnection(cn);
            return dt;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lngUserID"></param>
        /// <param name="lngDeptID"></param>
        /// <param name="lngOrgID"></param>
        /// <param name="tp"></param>
        /// <param name="re"></param>
        /// <param name="pagesize"></param>
        /// <param name="pageindex"></param>
        /// <param name="rowcount"></param>
        /// <param name="iEmail"></param>
        /// <returns></returns>
        public DataTable GetIssuesForUnFeedBack(long lngUserID, long lngDeptID, long lngOrgID, eOA_TracePeriod tp, RightEntity re, int pagesize, int pageindex, ref int rowcount, int iEmail)
        {

            string strWhere = " 1=1 ";
            string strList = "";


            if (re.CanRead == false)
            {
                //查询出空结果
                strWhere += " AND flowid = -1 ";
            }
            else
            {
                #region 范围条件
                switch (re.RightRange)
                {
                    case eO_RightRange.eFull:
                        strWhere += "";
                        break;
                    case eO_RightRange.ePersonal:
                        strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_CST_Issue.flowid AND receiverid = " + lngUserID.ToString() + ")";
                        break;
                    case eO_RightRange.eDeptDirect:
                        strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_CST_Issue.flowid AND recdeptid = " + lngDeptID.ToString() + ")";
                        break;
                    case eO_RightRange.eOrgDirect:
                        strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_CST_Issue.flowid AND recorgid = " + lngOrgID.ToString() + ")";
                        break;
                    case eO_RightRange.eDept:
                        strList = DeptDP.GetDeptFullID(lngDeptID);
                        if (strList.Trim().Length > 0)
                        {
                            //不是根部门并有找到
                            strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_CST_Issue.flowid AND recdeptid in (select deptid from ts_dept where fullid like " + StringTool.SqlQ(strList + "%") + "))";
                        }
                        break;
                    case eO_RightRange.eOrg:
                        strList = DeptDP.GetDeptFullID(lngOrgID);
                        if (strList.Trim().Length > 0)
                        {
                            //不是根部门并有找到
                            strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_CST_Issue.flowid AND recorgid in (select distinct deptid from ts_dept where deptkind = 1 and fullid like " + StringTool.SqlQ(strList + "%") + "))";
                        }
                        break;
                    default:
                        strWhere += "";
                        break;
                }
                #endregion

                switch (tp)
                {
                    case eOA_TracePeriod.eMonth:
                        strWhere += " AND RegSysDate >= to_date(" + StringTool.SqlQ(DateTime.Now.AddMonths(-1).ToShortDateString()) + ",'yyyy-MM-dd')";
                        break;
                    case eOA_TracePeriod.eSeason:
                        strWhere += " AND RegSysDate >= to_date(" + StringTool.SqlQ(DateTime.Now.AddMonths(-3).ToShortDateString()) + ",'yyyy-MM-dd')";
                        break;
                    case eOA_TracePeriod.eHalfYear:
                        strWhere += " AND RegSysDate >= dateadd('month',-6,sysdate)";
                        break;
                    case eOA_TracePeriod.eYear:
                        strWhere += " AND RegSysDate >= dateadd('year',-1,sysdate)";
                        break;
                    default:
                        break;
                }
                //strWhere += " AND not exists(select feedbackid from  ea_issues_feedback where flowid = V_CST_Issue.flowid )  AND nvl(V_CST_Issue.EmailState,0) = 0 AND V_CST_Issue.status= " + ((int)e_FlowStatus.efsEnd).ToString();
                strWhere += " AND not exists(select feedbackid from  ea_issues_feedback where flowid = V_CST_Issue.flowid )  AND nvl(V_CST_Issue.EmailState,0) = 0 ";
                if (iEmail == 1)   //有邮件
                {
                    strWhere += " AND exists(select id from  Br_ECustomer where V_CST_Issue.CustID = Br_ECustomer.ID AND nvl(Email,' ')!=' ')  ";
                }
                else if (iEmail == 2)  //无邮件
                {
                    strWhere += " AND exists(select id from  Br_ECustomer where V_CST_Issue.CustID = Br_ECustomer.ID AND nvl(Email,' ')=' ')  ";
                }
            }

            OracleConnection cn = ConfigTool.GetConnection();
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, "V_CST_Issue", "*", " ORDER BY smsid DESC", pagesize, pageindex, strWhere, ref rowcount);
            ConfigTool.CloseConnection(cn);
            return dt;
        }

        /// <summary>
        /// 未回访
        /// </summary>
        /// <param name="lngUserID"></param>
        /// <param name="lngDeptID"></param>
        /// <param name="lngOrgID"></param>
        /// <param name="tp"></param>
        /// <param name="re"></param>
        /// <param name="iPageSize"></param>
        /// <returns></returns>
        public DataTable GetIssuesForUnFeedBack(long lngUserID, eOA_TracePeriod tp, int pagesize, int pageindex, ref int rowcount)
        {
            string strWhere = " 1=1 AND RegUserID= " + lngUserID.ToString();
            switch (tp)
            {
                case eOA_TracePeriod.eMonth:
                    strWhere += " AND RegSysDate >= to_date(to_char(DateAdd('month',-1,sysdate),'yyyy-MM-dd'),'yyyy-MM-dd') ";
                    break;
                case eOA_TracePeriod.eSeason:
                    strWhere += " AND RegSysDate >=dateadd('month',-3,sysdate)";
                    break;
                case eOA_TracePeriod.eHalfYear:
                    strWhere += " AND RegSysDate >=dateadd('month',-6,sysdate)";
                    break;
                case eOA_TracePeriod.eYear:
                    strWhere += " AND RegSysDate >= dateadd('year',-1,sysdate)";
                    break;
                default:
                    break;
            }

            strWhere += " AND not exists(select feedbackid from  ea_issues_feedback where flowid = V_CST_Issue.flowid )  AND nvl(V_CST_Issue.EmailState,0) = 0 AND V_CST_Issue.status= " + ((int)e_FlowStatus.efsEnd).ToString();

            OracleConnection cn = ConfigTool.GetConnection();
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, "V_CST_Issue", "*", " ORDER BY smsid DESC", pagesize, pageindex, strWhere, ref rowcount);
            ConfigTool.CloseConnection(cn);
            return dt;
        }


        /// <summary>
        /// 超时事件(区别是否完成)
        /// </summary>
        /// <param name="lngUserID"></param>
        /// <param name="lngDeptID"></param>
        /// <param name="lngOrgID"></param>
        /// <param name="tp"></param>
        /// <param name="hasFinished"></param>
        /// <param name="re"></param>
        /// <returns></returns>
        public static DataTable GetIssuesForOverTime(long lngUserID, long lngDeptID, long lngOrgID, eOA_TracePeriod tp, bool hasFinished, RightEntity re)
        {
            string strSQL = "";
            string strList = "";
            strSQL = @"SELECT  nvl(buildCode,'')||nvl(a.ServiceNo,'') ServiceNo,a.smsid,a.flowid,a.regsysdate,a.subject,a.reguserid,a.Content,a.DealContent,a.CustAddress,a.ServiceTypeID,a.ServiceType,a.Sjwxr,
                      a.regusername,a.RegDeptID,a.RegDeptName,a.CustTime,a.CustName,a.contact,a.ctel,b.status,b.endtime,a.EquipmentCatalogName,a.EquipmentName,a.ReportingTime,a.FinishedTime,a.ServiceKind,
                      a.ServiceLevel,a.EffectName,a.InstancyName,a.CloseReasonName,a.ReSouseName,a.Outtime,a.ServiceTime,
                      datediff('Minute',sysdate,nvl(b.expectendtime,sysdate)) as FlowDiffMinute,a.dealstatus,a.ChangeProblemFlowID,d.customcode,(case a.MastCust 
				    when null then d.MastCustName
				    else a.mastCust end ) as MastCustName   " +
                    " FROM es_flow b,Cst_Issues a left join br_ecustomer d on a.custid = d.id " +
                    " WHERE a.FlowID = b.FlowID ";
            if (re.CanRead == false)
            {
                //查询出空结果
                strSQL += " AND a.flowid = -1 ";
            }
            else
            {
                #region 范围条件
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


                #endregion

                switch (tp)
                {
                    case eOA_TracePeriod.eMonth:
                        strSQL += " AND a.RegSysDate >= to_date(to_char(DateAdd('month',-1,sysdate),'yyyy-MM-dd'),'yyyy-MM-dd') ";
                        break;
                    case eOA_TracePeriod.eSeason:
                        strSQL += " AND a.RegSysDate >=dateadd('month',-3,sysdate)";
                        break;
                    case eOA_TracePeriod.eHalfYear:
                        strSQL += " AND a.RegSysDate >=dateadd('month',-6,sysdate)";
                        break;
                    case eOA_TracePeriod.eYear:
                        strSQL += " AND a.RegSysDate >= dateadd('year',-1,sysdate)";
                        break;
                    default:
                        break;
                }

                if (hasFinished == true)
                {
                    strSQL += " AND datediff('Minute',nvl(b.endtime,sysdate),nvl(b.expectendtime,sysdate)) < 0 AND b.status= " + ((int)e_FlowStatus.efsEnd).ToString() +
                        " ORDER BY a.smsid DESC";
                }
                else
                {
                    strSQL += " AND datediff('Minute',sysdate,nvl(b.expectendtime,sysdate)) < 0 AND b.status= " + ((int)e_FlowStatus.efsHandle).ToString() +
                        " ORDER BY a.smsid DESC";
                }
            }



            OracleConnection cn = ConfigTool.GetConnection();
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigTool.CloseConnection(cn);
            return dt;
        }

        /// <summary>
        /// 超时事件(区别是否完成)
        /// </summary>
        /// <param name="lngUserID"></param>
        /// <param name="lngDeptID"></param>
        /// <param name="lngOrgID"></param>
        /// <param name="tp"></param>
        /// <param name="hasFinished"></param>
        /// <param name="re"></param>
        /// <returns></returns>
        public DataTable GetIssuesForOverTime(long lngUserID, long lngDeptID, long lngOrgID, eOA_TracePeriod tp, bool hasFinished, RightEntity re, int pagesize, int pageindex, ref int rowcount)
        {

            string strWhere = "1=1";
            string strList = "";


            if (re.CanRead == false)
            {
                //查询出空结果
                strWhere += " AND flowid = -1 ";
            }
            else
            {
                #region 范围条件
                switch (re.RightRange)
                {
                    case eO_RightRange.eFull:
                        strWhere += "";
                        break;
                    case eO_RightRange.ePersonal:
                        strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_CST_Issue.flowid AND receiverid = " + lngUserID.ToString() + ")";
                        break;
                    case eO_RightRange.eDeptDirect:
                        strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_CST_Issue.flowid AND recdeptid = " + lngDeptID.ToString() + ")";
                        break;
                    case eO_RightRange.eOrgDirect:
                        strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_CST_Issue.flowid AND recorgid = " + lngOrgID.ToString() + ")";
                        break;
                    case eO_RightRange.eDept:
                        strList = DeptDP.GetDeptFullID(lngDeptID);
                        if (strList.Trim().Length > 0)
                        {
                            //不是根部门并有找到
                            strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_CST_Issue.flowid AND recdeptid in (select deptid from ts_dept where fullid like " + StringTool.SqlQ(strList + "%") + "))";
                        }
                        break;
                    case eO_RightRange.eOrg:
                        strList = DeptDP.GetDeptFullID(lngOrgID);
                        if (strList.Trim().Length > 0)
                        {
                            //不是根部门并有找到
                            strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_CST_Issue.flowid AND recorgid in (select distinct deptid from ts_dept where deptkind = 1 and fullid like " + StringTool.SqlQ(strList + "%") + "))";
                        }
                        break;
                    default:
                        strWhere += "";
                        break;
                }


                #endregion

                switch (tp)
                {
                    case eOA_TracePeriod.eMonth:
                        strWhere += " AND RegSysDate >= to_date(" + StringTool.SqlQ(DateTime.Now.AddMonths(-1).ToShortDateString()) + ",'yyyy-MM-dd')";
                        break;
                    case eOA_TracePeriod.eSeason:
                        strWhere += " AND RegSysDate >= to_date(" + StringTool.SqlQ(DateTime.Now.AddMonths(-3).ToShortDateString()) + ",'yyyy-MM-dd')";
                        break;
                    case eOA_TracePeriod.eHalfYear:
                        strWhere += " AND RegSysDate >= dateadd('month',-6,sysdate)";
                        break;
                    case eOA_TracePeriod.eYear:
                        strWhere += " AND RegSysDate >= dateadd('year',-1,sysdate)";
                        break;
                    default:
                        break;
                }

                if (hasFinished == true)
                {
                    //strWhere += " AND datediff('Minute',nvl(endtime,sysdate),nvl(expectendtime,sysdate)) < 0 AND V_CST_Issue.status= " + ((int)e_FlowStatus.efsEnd).ToString();
                    strWhere += " AND V_CST_Issue.flowid in (select flowid from Ea_FlowBusLimit l where l.guidid=10001 and datediff('Minute',nvl(V_CST_Issue.finishedtime,sysdate),nvl(l.limittime,sysdate))<0) and (V_CST_Issue.DealStatus = '已解决' or V_CST_Issue.DealStatus = '已完成'or V_CST_Issue.DealStatus = '已关闭'or (  V_CST_Issue.DealStatus is null AND V_CST_Issue.status= " + ((int)e_FlowStatus.efsEnd).ToString() + "))";
                }
                else
                {
                    //strWhere += " AND datediff('Minute',sysdate,nvl(expectendtime,sysdate)) < 0 AND V_CST_Issue.status= " + ((int)e_FlowStatus.efsHandle).ToString();
                    strWhere += " AND V_CST_Issue.flowid in (select flowid from Ea_FlowBusLimit l where l.guidid=10001 and datediff('Minute',nvl(V_CST_Issue.finishedtime,sysdate),nvl(l.limittime,sysdate))<0) and (V_CST_Issue.DealStatus = '处理中' or V_CST_Issue.DealStatus = 'IT审批中'or V_CST_Issue.DealStatus = '部门审批中'or V_CST_Issue.DealStatus = '业务审批中' or (  V_CST_Issue.DealStatus is null AND V_CST_Issue.status= " + ((int)e_FlowStatus.efsHandle).ToString() + "))";
                }
            }

            OracleConnection cn = ConfigTool.GetConnection();
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, "V_CST_Issue", "*", " ORDER BY smsid DESC", pagesize, pageindex, strWhere, ref rowcount);
            ConfigTool.CloseConnection(cn);
            return dt;
        }

        /// <summary>
        /// 超时事件(区别是否完成)
        /// </summary>
        /// <param name="lngUserID"></param>
        /// <param name="lngDeptID"></param>
        /// <param name="lngOrgID"></param>
        /// <param name="tp"></param>
        /// <param name="hasFinished"></param>
        /// <param name="re"></param>
        /// <returns></returns>
        public DataTable GetIssuesForOverTime(long lngUserID, eOA_TracePeriod tp, bool hasFinished, int pagesize, int pageindex, ref int rowcount)
        {

            string strWhere = "1=1 AND RegUserID= " + lngUserID.ToString();

            switch (tp)
            {
                case eOA_TracePeriod.eMonth:
                    strWhere += " AND RegSysDate >= to_date(to_char(DateAdd('month',-1,sysdate),'yyyy-MM-dd'),'yyyy-MM-dd') ";
                    break;
                case eOA_TracePeriod.eSeason:
                    strWhere += " AND RegSysDate >=dateadd('month',-3,sysdate)";
                    break;
                case eOA_TracePeriod.eHalfYear:
                    strWhere += " AND RegSysDate >=dateadd('month',-6,sysdate)";
                    break;
                case eOA_TracePeriod.eYear:
                    strWhere += " AND RegSysDate >= dateadd('year',-1,sysdate)";
                    break;
                default:
                    break;
            }

            if (hasFinished == true)
            {
                strWhere += " AND datediff('Minute',nvl(endtime,sysdate),nvl(expectendtime,sysdate)) < 0 AND V_CST_Issue.status= " + ((int)e_FlowStatus.efsEnd).ToString();
            }
            else
            {
                strWhere += " AND datediff('Minute',sysdate,nvl(expectendtime,sysdate)) < 0 AND V_CST_Issue.status= " + ((int)e_FlowStatus.efsHandle).ToString();
            }

            DataTable dt = null;
            OracleConnection cn = null;
            try
            {
                cn = ConfigTool.GetConnection();
                dt = OracleDbHelper.ExecuteDataTable(cn, "V_CST_Issue", "*", " ORDER BY smsid DESC", pagesize, pageindex, strWhere, ref rowcount);
                return dt;
            }
            catch
            {
                throw;
            }
            finally
            {
                ConfigTool.CloseConnection(cn);

            }

            //OracleConnection cn = ConfigTool.GetConnection();
            //DataTable dt = OracleDbHelper.ExecuteDataTable(cn, "V_CST_Issue", "*", " ORDER BY smsid DESC", pagesize, pageindex, strWhere, ref rowcount);
            //ConfigTool.CloseConnection(cn);
            //return dt;
        }

        /// <summary>
        /// 查询我登记的事件
        /// </summary>
        /// <param name="lngUserID"></param>
        /// <param name="lngDeptID"></param>
        /// <param name="lngOrgID"></param>
        /// <param name="tp"></param>
        /// <param name="re"></param>
        /// <returns></returns>
        public static DataTable GetIssuesForMy(long lngUserID, long lngDeptID, long lngOrgID, eOA_TracePeriod tp, RightEntity re)
        {
            string strSQL = "";
            strSQL = @"SELECT  nvl(buildCode,'')||nvl(a.ServiceNo,'') ServiceNo,a.smsid,a.flowid,a.regsysdate,a.subject,a.reguserid,a.regusername,a.RegDeptID,a.RegDeptName,a.CustTime,a.CustName,a.Content,a.DealContent,a.CustAddress,a.ServiceTypeID,a.ServiceType,a.Sjwxr,
                        a.contact,a.ctel,b.status,b.endtime,a.EquipmentCatalogName,a.EquipmentName,a.ReportingTime,a.FinishedTime,a.ServiceKind,
                        a.ServiceLevel,a.EffectName,a.InstancyName,a.CloseReasonName,a.ReSouseName,a.Outtime,a.ServiceTime,
                        datediff('Minute',sysdate,nvl(b.expectendtime,sysdate)) as FlowDiffMinute,a.dealstatus,a.ChangeProblemFlowID,d.customcode,(case a.MastCust 
				when null then d.MastCustName
				else a.mastCust end ) as MastCustName   " +
                " FROM es_flow b,Cst_Issues a left join br_ecustomer d on a.custid = d.id" +
                " WHERE a.FlowID = b.FlowID ";

            if (re.CanRead == false)
            {
                //查询出空结果
                strSQL += " AND a.flowid = -1 ";
            }
            else
            {
                switch (tp)
                {
                    case eOA_TracePeriod.eMonth:
                        strSQL += " AND a.RegSysDate >= to_date(to_char(DateAdd('month',-1,sysdate),'yyyy-MM-dd'),'yyyy-MM-dd') ";
                        break;
                    case eOA_TracePeriod.eSeason:
                        strSQL += " AND a.RegSysDate >=dateadd('month',-3,sysdate)";
                        break;
                    case eOA_TracePeriod.eHalfYear:
                        strSQL += " AND a.RegSysDate >=dateadd('month',-6,sysdate)";
                        break;
                    case eOA_TracePeriod.eYear:
                        strSQL += " AND a.RegSysDate >=dateadd('year',-1,sysdate)";
                        break;
                    default:
                        break;

                }
                strSQL += " AND a.RegUserID= " + lngUserID.ToString() + " ORDER BY a.smsid DESC";
            }
            OracleConnection cn = ConfigTool.GetConnection();
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigTool.CloseConnection(cn);
            return dt;
        }

        /// <summary>
        /// 查询我登记的事件
        /// </summary>
        /// <param name="lngUserID"></param>
        /// <param name="lngDeptID"></param>
        /// <param name="lngOrgID"></param>
        /// <param name="tp"></param>
        /// <param name="re"></param>
        /// <param name="pagesize"></param>
        /// <param name="pageindex"></param>
        /// <param name="rowcount"></param>
        /// <returns></returns>
        public DataTable GetIssuesForMy(long lngUserID, long lngDeptID, long lngOrgID, eOA_TracePeriod tp, RightEntity re, int pagesize, int pageindex, ref int rowcount)
        {

            string strWhere = " 1=1";

            if (re == null || re.CanRead == false)
            {
                //查询出空结果
                strWhere += " AND flowid = -1 ";
            }
            else
            {
                switch (tp)
                {
                    case eOA_TracePeriod.eMonth:
                        strWhere += " AND RegSysDate >= to_date(" + StringTool.SqlQ(DateTime.Now.AddMonths(-1).ToShortDateString()) + ",'yyyy-MM-dd')";
                        break;
                    case eOA_TracePeriod.eSeason:
                        strWhere += " AND RegSysDate >= to_date(" + StringTool.SqlQ(DateTime.Now.AddMonths(-3).ToShortDateString()) + ",'yyyy-MM-dd')";
                        break;
                    case eOA_TracePeriod.eHalfYear:
                        strWhere += " AND RegSysDate >=dateadd('month',-6,sysdate)";
                        break;
                    case eOA_TracePeriod.eYear:
                        strWhere += " AND RegSysDate >=dateadd('year',-1,sysdate)";
                        break;
                    default:
                        break;
                }
                strWhere += " AND RegUserID= " + lngUserID.ToString();
            }


            OracleConnection cn = ConfigTool.GetConnection();
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, "V_CST_Issue", "*", " ORDER BY smsid DESC", pagesize, pageindex, strWhere, ref rowcount);
            ConfigTool.CloseConnection(cn);
            return dt;
        }

        /// <summary>
        /// 查询我登记的事件
        /// </summary>
        /// <param name="lngUserID"></param>
        /// <param name="lngDeptID"></param>
        /// <param name="lngOrgID"></param>
        /// <param name="tp"></param>
        /// <param name="re"></param>
        /// <param name="pagesize"></param>
        /// <param name="pageindex"></param>
        /// <param name="rowcount"></param>
        /// <returns></returns>
        public DataTable GetIssuesForMy(long lngUserID, eOA_TracePeriod tp, int pagesize, int pageindex, ref int rowcount)
        {

            string strWhere = " 1=1";
            switch (tp)
            {
                case eOA_TracePeriod.eMonth:
                    strWhere += " AND RegSysDate >= to_date(to_char(DateAdd('month',-1,sysdate),'yyyy-MM-dd'),'yyyy-MM-dd') ";
                    break;
                case eOA_TracePeriod.eSeason:
                    strWhere += " AND RegSysDate >=dateadd('month',-3,sysdate)";
                    break;
                case eOA_TracePeriod.eHalfYear:
                    strWhere += " AND RegSysDate >= dateadd('month',-6,sysdate)";
                    break;
                case eOA_TracePeriod.eYear:
                    strWhere += " AND RegSysDate >=dateadd('year',-1,sysdate)";
                    break;
                default:
                    break;
            }

            strWhere += " AND RegUserID= " + lngUserID.ToString();

            OracleConnection cn = ConfigTool.GetConnection();
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, "V_CST_Issue", "*", " ORDER BY smsid DESC", pagesize, pageindex, strWhere, ref rowcount);
            ConfigTool.CloseConnection(cn);
            return dt;
        }

        /// <summary>
        /// 有我参与处理的事件
        /// </summary>
        /// <param name="lngUserID"></param>
        /// <param name="lngDeptID"></param>
        /// <param name="lngOrgID"></param>
        /// <param name="tp"></param>
        /// <param name="re"></param>
        /// <param name="pagesize"></param>
        /// <param name="pageindex"></param>
        /// <param name="rowcount"></param>
        /// <returns></returns>
        public DataTable GetIssuesForMyProccess(long lngUserID, long lngDeptID, long lngOrgID, eOA_TracePeriod tp, RightEntity re, int pagesize, int pageindex, ref int rowcount)
        {

            string strWhere = "1=1";
            if (re.CanRead == false)
            {
                //查询出空结果
                strWhere += " AND flowid = -1 ";
            }
            else
            {
                switch (tp)
                {
                    case eOA_TracePeriod.eMonth:
                        strWhere += " AND RegSysDate >= to_date(to_char(DateAdd('month',-1,sysdate),'yyyy-MM-dd'),'yyyy-MM-dd') ";
                        break;
                    case eOA_TracePeriod.eSeason:
                        strWhere += " AND RegSysDate >=dateadd('month',-3,sysdate)";
                        break;
                    case eOA_TracePeriod.eHalfYear:
                        strWhere += " AND RegSysDate >=dateadd('month',-6,sysdate) ";
                        break;
                    case eOA_TracePeriod.eYear:
                        strWhere += " AND RegSysDate >=dateadd('year',-1,sysdate)";
                        break;
                    default:
                        break;
                }
                strWhere += " AND flowid in (SELECT distinct flowid FROM es_message WHERE receiverid = " + lngUserID.ToString() +
                    " AND actortype =" + ((int)e_ActorClass.fmMasterActor).ToString() + ")";
            }

            OracleConnection cn = ConfigTool.GetConnection();
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, "V_CST_Issue", "*", " ORDER BY smsid DESC", pagesize, pageindex, strWhere, ref rowcount);
            ConfigTool.CloseConnection(cn);
            return dt;
        }


        #region 正在处理
        /// <summary>
        /// 正在处理
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
        public DataTable GetIssuesForHandle(long lngUserID, long lngDeptID, long lngOrgID, eOA_TracePeriod tp, RightEntity re, int pagesize, int pageindex, ref int rowcount)
        {
            string strList = string.Empty;
            string strWhere = " 1=1 ";

            //正在处理
            strWhere += " and Status =20 ";
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
                        strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_CST_Issue.flowid AND receiverid = " + lngUserID.ToString() + ")";
                        break;
                    case eO_RightRange.eDeptDirect:
                        strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_CST_Issue.flowid AND recdeptid = " + lngDeptID.ToString() + ")";
                        break;
                    case eO_RightRange.eOrgDirect:
                        strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_CST_Issue.flowid AND recorgid = " + lngOrgID.ToString() + ")";
                        break;
                    case eO_RightRange.eDept:
                        strList = DeptDP.GetDeptFullID(lngDeptID);
                        if (strList.Trim().Length > 0)
                        {
                            //不是根部门并有找到
                            strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_CST_Issue.flowid AND recdeptid in (select deptid from ts_dept where fullid like " + StringTool.SqlQ(strList + "%") + "))";
                        }
                        break;
                    case eO_RightRange.eOrg:
                        strList = DeptDP.GetDeptFullID(lngOrgID);
                        if (strList.Trim().Length > 0)
                        {
                            //不是根部门并有找到
                            strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_CST_Issue.flowid AND recorgid in (select distinct deptid from ts_dept where deptkind = 1 and fullid like " + StringTool.SqlQ(strList + "%") + "))";
                        }
                        break;
                    default:
                        strWhere += "";
                        break;
                }
                #endregion

                #region 查询范围

                switch (tp)
                {
                    case eOA_TracePeriod.eMonth:
                        strWhere += " AND RegSysDate >= to_date(" + StringTool.SqlQ(DateTime.Now.AddMonths(-1).ToShortDateString()) + ",'yyyy-MM-dd')";
                        break;
                    case eOA_TracePeriod.eSeason:
                        strWhere += " AND RegSysDate >= to_date(" + StringTool.SqlQ(DateTime.Now.AddMonths(-3).ToShortDateString()) + ",'yyyy-MM-dd')";
                        break;
                    case eOA_TracePeriod.eHalfYear:
                        strWhere += " AND RegSysDate >=dateadd('month',-6,sysdate)";
                        break;
                    case eOA_TracePeriod.eYear:
                        strWhere += " AND RegSysDate >=dateadd('year',-1,sysdate)";
                        break;
                    default:
                        break;
                }
                #endregion
            }

            OracleConnection cn = ConfigTool.GetConnection();
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, "V_CST_Issue", "*", " ORDER BY smsid DESC", pagesize, pageindex, strWhere, ref rowcount);
            ConfigTool.CloseConnection(cn);
            return dt;
        }


        /// <summary>
        /// 正在处理
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
        public DataTable GetIssuesForHandle(long lngUserID, eOA_TracePeriod tp, int pagesize, int pageindex, ref int rowcount)
        {
            string strList = string.Empty;
            string strWhere = " 1=1 ";

            //正在处理
            strWhere += " AND Status =20 AND RegUserID= " + lngUserID.ToString();

            #region 查询范围

            switch (tp)
            {
                case eOA_TracePeriod.eMonth:
                    strWhere += " AND RegSysDate >= to_date(to_char(DateAdd('month',-1,sysdate),'yyyy-MM-dd'),'yyyy-MM-dd') ";
                    break;
                case eOA_TracePeriod.eSeason:
                    strWhere += " AND RegSysDate >=dateadd('month',-3,sysdate)";
                    break;
                case eOA_TracePeriod.eHalfYear:
                    strWhere += " AND RegSysDate >= dateadd('month',-6,sysdate)";
                    break;
                case eOA_TracePeriod.eYear:
                    strWhere += " AND RegSysDate >=dateadd('year',-1,sysdate)";
                    break;
                default:
                    break;
            }

            #endregion

            OracleConnection cn = ConfigTool.GetConnection();
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, "V_CST_Issue", "*", " ORDER BY smsid DESC", pagesize, pageindex, strWhere, ref rowcount);
            ConfigTool.CloseConnection(cn);
            return dt;
        }
        #endregion

        #region 正在处理 导出excel
        /// <summary>
        /// 正在处理 导出excel
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
        public static DataTable GetIssuesForHandle(long lngUserID, long lngDeptID, long lngOrgID, eOA_TracePeriod tp, RightEntity re)
        {
            string strSQL = string.Empty;
            string strList = string.Empty;

            strSQL = @"SELECT  nvl(buildCode,'')||nvl(a.ServiceNo,'') ServiceNo,a.smsid,a.flowid,a.regsysdate,a.subject,a.reguserid,a.Content,a.DealContent,a.CustAddress,a.ServiceTypeID,a.ServiceType,a.Sjwxr,
                      a.regusername,a.RegDeptID,a.RegDeptName,a.CustTime,a.CustName,a.contact,a.ctel,b.status,b.endtime,a.EquipmentCatalogName,a.EquipmentName,a.ReportingTime,a.FinishedTime,a.ServiceKind,
                      a.ServiceLevel,a.EffectName,a.InstancyName,a.CloseReasonName,a.ReSouseName,a.Outtime,a.ServiceTime,
                      datediff('Minute',sysdate,nvl(b.expectendtime,sysdate)) as FlowDiffMinute,a.dealstatus,a.ChangeProblemFlowID,d.customcode,(case a.MastCust 
				    when null then d.MastCustName
				    else a.mastCust end ) as MastCustName   " +
                    " FROM es_flow b,Cst_Issues a left join br_ecustomer d on a.custid = d.id " +
                    " WHERE a.FlowID = b.FlowID ";

            string strWhere = string.Empty;

            //正在处理
            strWhere += " and Status =20 ";
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
                        strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = a.flowid AND receiverid = " + lngUserID.ToString() + ")";
                        break;
                    case eO_RightRange.eDeptDirect:
                        strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = a.flowid AND recdeptid = " + lngDeptID.ToString() + ")";
                        break;
                    case eO_RightRange.eOrgDirect:
                        strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = a.flowid AND recorgid = " + lngOrgID.ToString() + ")";
                        break;
                    case eO_RightRange.eDept:
                        strList = DeptDP.GetDeptFullID(lngDeptID);
                        if (strList.Trim().Length > 0)
                        {
                            //不是根部门并有找到
                            strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = a.flowid AND recdeptid in (select deptid from ts_dept where fullid like " + StringTool.SqlQ(strList + "%") + "))";
                        }
                        break;
                    case eO_RightRange.eOrg:
                        strList = DeptDP.GetDeptFullID(lngOrgID);
                        if (strList.Trim().Length > 0)
                        {
                            //不是根部门并有找到
                            strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = a.flowid AND recorgid in (select distinct deptid from ts_dept where deptkind = 1 and fullid like " + StringTool.SqlQ(strList + "%") + "))";
                        }
                        break;
                    default:
                        strWhere += "";
                        break;
                }
                #endregion

                #region 查询范围

                switch (tp)
                {
                    case eOA_TracePeriod.eMonth:
                        strWhere += " AND RegSysDate >= to_date(to_char(DateAdd('month',-1,sysdate),'yyyy-MM-dd'),'yyyy-MM-dd') ";
                        break;
                    case eOA_TracePeriod.eSeason:
                        strWhere += " AND RegSysDate >= dateadd('month',-3,sysdate)";
                        break;
                    case eOA_TracePeriod.eHalfYear:
                        strWhere += " AND RegSysDate >= dateadd('month',-6,sysdate)";
                        break;
                    case eOA_TracePeriod.eYear:
                        strWhere += " AND RegSysDate >= dateadd('year',-1,sysdate)";
                        break;
                    default:
                        break;
                }

                #endregion

            }

            strSQL = strSQL + strWhere;

            OracleConnection cn = ConfigTool.GetConnection();
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigTool.CloseConnection(cn);
            return dt;
        }
        #endregion

        #region 正常结束
        /// <summary>
        /// 正常结束
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
        public DataTable GetIssuesForEnd(long lngUserID, long lngDeptID, long lngOrgID, eOA_TracePeriod tp, RightEntity re, int pagesize, int pageindex, ref int rowcount)
        {
            string strList = string.Empty;
            string strWhere = " 1=1 ";

            //正常结束
            strWhere += " and Status =30 ";
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
                        strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_CST_Issue.flowid AND receiverid = " + lngUserID.ToString() + ")";
                        break;
                    case eO_RightRange.eDeptDirect:
                        strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_CST_Issue.flowid AND recdeptid = " + lngDeptID.ToString() + ")";
                        break;
                    case eO_RightRange.eOrgDirect:
                        strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_CST_Issue.flowid AND recorgid = " + lngOrgID.ToString() + ")";
                        break;
                    case eO_RightRange.eDept:
                        strList = DeptDP.GetDeptFullID(lngDeptID);
                        if (strList.Trim().Length > 0)
                        {
                            //不是根部门并有找到
                            strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_CST_Issue.flowid AND recdeptid in (select deptid from ts_dept where fullid like " + StringTool.SqlQ(strList + "%") + "))";
                        }
                        break;
                    case eO_RightRange.eOrg:
                        strList = DeptDP.GetDeptFullID(lngOrgID);
                        if (strList.Trim().Length > 0)
                        {
                            //不是根部门并有找到
                            strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_CST_Issue.flowid AND recorgid in (select distinct deptid from ts_dept where deptkind = 1 and fullid like " + StringTool.SqlQ(strList + "%") + "))";
                        }
                        break;
                    default:
                        strWhere += "";
                        break;
                }
                #endregion

                #region 查询范围

                switch (tp)
                {
                    case eOA_TracePeriod.eMonth:
                        strWhere += " AND RegSysDate >= to_date(" + StringTool.SqlQ(DateTime.Now.AddMonths(-1).ToShortDateString()) + ",'yyyy-MM-dd')";
                        break;
                    case eOA_TracePeriod.eSeason:
                        strWhere += " AND RegSysDate >= to_date(" + StringTool.SqlQ(DateTime.Now.AddMonths(-3).ToShortDateString()) + ",'yyyy-MM-dd')";
                        break;
                    case eOA_TracePeriod.eHalfYear:
                        strWhere += " AND RegSysDate >= dateadd('month',-6,sysdate)";
                        break;
                    case eOA_TracePeriod.eYear:
                        strWhere += " AND RegSysDate >= dateadd('year',-1,sysdate)";
                        break;
                    default:
                        break;
                }
                #endregion
            }
            OracleConnection cn = ConfigTool.GetConnection();
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, "V_CST_Issue", "*", " ORDER BY smsid DESC", pagesize, pageindex, strWhere, ref rowcount);
            ConfigTool.CloseConnection(cn);
            return dt;
        }
        #endregion

        #region 正常结束
        /// <summary>
        /// 正常结束
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
        public DataTable GetIssuesForEnd(long lngUserID, eOA_TracePeriod tp, int pagesize, int pageindex, ref int rowcount)
        {
            string strList = string.Empty;
            string strWhere = " 1=1 AND RegUserID= " + lngUserID.ToString();

            //正常结束
            strWhere += " and Status =30 ";

            #region 查询范围

            switch (tp)
            {
                case eOA_TracePeriod.eMonth:
                    strWhere += " AND RegSysDate >= to_date(to_char(DateAdd('month',-1,sysdate),'yyyy-MM-dd'),'yyyy-MM-dd') ";
                    break;
                case eOA_TracePeriod.eSeason:
                    strWhere += " AND RegSysDate >= dateadd('month',-3,sysdate)";
                    break;
                case eOA_TracePeriod.eHalfYear:
                    strWhere += " AND RegSysDate >= dateadd('month',-6,sysdate)";
                    break;
                case eOA_TracePeriod.eYear:
                    strWhere += " AND RegSysDate >=dateadd('year',-1,sysdate) ";
                    break;
                default:
                    break;
            }

            #endregion

            OracleConnection cn = ConfigTool.GetConnection();
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, "V_CST_Issue", "*", " ORDER BY smsid DESC", pagesize, pageindex, strWhere, ref rowcount);
            ConfigTool.CloseConnection(cn);
            return dt;
        }
        #endregion

        #region 正常结束 导出excel
        /// <summary>
        /// 正常结束 导出excel
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
        public static DataTable GetIssuesForEnd(long lngUserID, long lngDeptID, long lngOrgID, eOA_TracePeriod tp, RightEntity re)
        {
            string strSQL = string.Empty;
            string strList = string.Empty;
            strSQL = @"SELECT  nvl(buildCode,'')||nvl(a.ServiceNo,'') ServiceNo,a.smsid,a.flowid,a.regsysdate,a.subject,a.reguserid,a.Content,a.DealContent,a.CustAddress,a.ServiceTypeID,a.ServiceType,a.Sjwxr,
                      a.regusername,a.RegDeptID,a.RegDeptName,a.CustTime,a.CustName,a.contact,a.ctel,b.status,b.endtime,a.EquipmentCatalogName,a.EquipmentName,a.ReportingTime,a.FinishedTime,a.ServiceKind,
                      a.ServiceLevel,a.EffectName,a.InstancyName,a.CloseReasonName,a.ReSouseName,a.Outtime,a.ServiceTime,
                      datediff('Minute',sysdate,nvl(b.expectendtime,sysdate)) as FlowDiffMinute,a.dealstatus,a.ChangeProblemFlowID,d.customcode,(case a.MastCust 
				    when null then d.MastCustName
				    else a.mastCust end ) as MastCustName   " +
                    " FROM es_flow b,Cst_Issues a left join br_ecustomer d on a.custid = d.id " +
                    " WHERE a.FlowID = b.FlowID ";

            string strWhere = string.Empty;

            //正常结束
            strWhere += " and Status =30 ";

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
                        strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = a.flowid AND receiverid = " + lngUserID.ToString() + ")";
                        break;
                    case eO_RightRange.eDeptDirect:
                        strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = a.flowid AND recdeptid = " + lngDeptID.ToString() + ")";
                        break;
                    case eO_RightRange.eOrgDirect:
                        strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = a.flowid AND recorgid = " + lngOrgID.ToString() + ")";
                        break;
                    case eO_RightRange.eDept:
                        strList = DeptDP.GetDeptFullID(lngDeptID);
                        if (strList.Trim().Length > 0)
                        {
                            //不是根部门并有找到
                            strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = a.flowid AND recdeptid in (select deptid from ts_dept where fullid like " + StringTool.SqlQ(strList + "%") + "))";
                        }
                        break;
                    case eO_RightRange.eOrg:
                        strList = DeptDP.GetDeptFullID(lngOrgID);
                        if (strList.Trim().Length > 0)
                        {
                            //不是根部门并有找到
                            strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = a.flowid AND recorgid in (select distinct deptid from ts_dept where deptkind = 1 and fullid like " + StringTool.SqlQ(strList + "%") + "))";
                        }
                        break;
                    default:
                        strWhere += "";
                        break;
                }
                #endregion

                #region 查询范围

                switch (tp)
                {
                    case eOA_TracePeriod.eMonth:
                        strWhere += " AND RegSysDate >= to_date(to_char(DateAdd('month',-1,sysdate),'yyyy-MM-dd'),'yyyy-MM-dd') ";
                        break;
                    case eOA_TracePeriod.eSeason:
                        strWhere += " AND RegSysDate >= dateadd('month',-3,sysdate)";
                        break;
                    case eOA_TracePeriod.eHalfYear:
                        strWhere += " AND RegSysDate >= dateadd('month',-6,sysdate)";
                        break;
                    case eOA_TracePeriod.eYear:
                        strWhere += " AND RegSysDate >= dateadd('year',-1,sysdate)";
                        break;
                    default:
                        break;
                }
                #endregion
            }
            strSQL = strSQL + strWhere;

            OracleConnection cn = ConfigTool.GetConnection();
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigTool.CloseConnection(cn);
            return dt;
        }
        #endregion


        #region 正在处理,正常结束
        /// <summary>
        /// 事件单查询，正在处理,正常结束
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
        public DataTable GetIssuesForStatus(int Status, long lngUserID, long lngDeptID, long lngOrgID, eOA_TracePeriod tp, RightEntity re, int pagesize, int pageindex, ref int rowcount)
        {
            string strList = string.Empty;
            string strWhere = " 1=1 ";

            //正在处理、正常结束
            strWhere += " and Status = " + Status;
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
                        strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_CST_Issue.flowid AND receiverid = " + lngUserID.ToString() + ")";
                        break;
                    case eO_RightRange.eDeptDirect:
                        strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_CST_Issue.flowid AND recdeptid = " + lngDeptID.ToString() + ")";
                        break;
                    case eO_RightRange.eOrgDirect:
                        strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_CST_Issue.flowid AND recorgid = " + lngOrgID.ToString() + ")";
                        break;
                    case eO_RightRange.eDept:
                        strList = DeptDP.GetDeptFullID(lngDeptID);
                        if (strList.Trim().Length > 0)
                        {
                            //不是根部门并有找到
                            strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_CST_Issue.flowid AND recdeptid in (select deptid from ts_dept where fullid like " + StringTool.SqlQ(strList + "%") + "))";
                        }
                        break;
                    case eO_RightRange.eOrg:
                        strList = DeptDP.GetDeptFullID(lngOrgID);
                        if (strList.Trim().Length > 0)
                        {
                            //不是根部门并有找到
                            strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_CST_Issue.flowid AND recorgid in (select distinct deptid from ts_dept where deptkind = 1 and fullid like " + StringTool.SqlQ(strList + "%") + "))";
                        }
                        break;
                    default:
                        strWhere += "";
                        break;
                }
                #endregion

                #region 查询范围

                switch (tp)
                {
                    case eOA_TracePeriod.eMonth:
                        strWhere += " AND RegSysDate >= to_date(to_char(DateAdd('month',-1,sysdate),'yyyy-MM-dd'),'yyyy-MM-dd') ";
                        break;
                    case eOA_TracePeriod.eSeason:
                        strWhere += " AND RegSysDate >= dateadd('month',-3,sysdate)";
                        break;
                    case eOA_TracePeriod.eHalfYear:
                        strWhere += " AND RegSysDate >= dateadd('month',-6,sysdate)";
                        break;
                    case eOA_TracePeriod.eYear:
                        strWhere += " AND RegSysDate >= dateadd('year',-1,sysdate)";
                        break;
                    default:
                        break;
                }

                #endregion

            }

            OracleConnection cn = ConfigTool.GetConnection();
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, "V_CST_Issue", "*", " ORDER BY smsid DESC", pagesize, pageindex, strWhere, ref rowcount);
            ConfigTool.CloseConnection(cn);
            return dt;
        }
        #endregion


        /// <summary>
        /// 事件跟踪快速查询
        /// </summary>
        /// <param name="tp"></param>
        /// <param name="lngUserID"></param>
        /// <param name="strCustInfo"></param>
        /// <param name="strFlowStatus"></param>
        /// <param name="strStatus"></param>
        /// <param name="strMessageBegin"></param>
        /// <param name="strMessageEnd"></param>
        /// <returns></returns>
        public static DataTable getProccess(eOA_TocdProccess tp, long lngUserID, string strCustInfo, string strFlowStatus, string strStatus, string strMessageBegin, string strMessageEnd, long lngDeptID, long lngOrgID, RightEntity re, int pagesize, int pageindex, ref int rowcount)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            string strWhere = "";
            string strList = "";
            //用户信息
            if (strCustInfo != string.Empty)
            {
                strWhere += " And (";
                strWhere += " CustName like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                strWhere += " OR Contact like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                strWhere += " OR CustCode like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                strWhere += " OR CustAddress like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                strWhere += " OR MastCust like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                strWhere += " OR Email like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                strWhere += " OR CTel like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                strWhere += " ) ";

            }

            if (strFlowStatus != "-1" && strFlowStatus != "")
            {
                if (tp != eOA_TocdProccess.notReturnVisit && tp != eOA_TocdProccess.overtimeEventfulfill && tp != eOA_TocdProccess.overtimeEventNOfulfill && tp != eOA_TocdProccess.overtime48Event && tp != eOA_TocdProccess.overtimeEvent)
                {
                    strWhere += " AND status = " + strFlowStatus;
                }
            }

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
                        strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_CST_Issue.flowid AND receiverid = " + lngUserID.ToString() + ")";
                        break;
                    case eO_RightRange.eDeptDirect:
                        strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_CST_Issue.flowid AND recdeptid = " + lngDeptID.ToString() + ")";
                        break;
                    case eO_RightRange.eOrgDirect:
                        strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_CST_Issue.flowid AND recorgid = " + lngOrgID.ToString() + ")";
                        break;
                    case eO_RightRange.eDept:
                        strList = DeptDP.GetDeptFullID(lngDeptID);
                        if (strList.Trim().Length > 0)
                        {
                            //不是根部门并有找到
                            strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_CST_Issue.flowid AND recdeptid in (select deptid from ts_dept where fullid like " + StringTool.SqlQ(strList + "%") + "))";
                        }
                        break;
                    case eO_RightRange.eOrg:
                        strList = DeptDP.GetDeptFullID(lngOrgID);
                        if (strList.Trim().Length > 0)
                        {
                            //不是根部门并有找到
                            strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_CST_Issue.flowid AND recorgid in (select distinct deptid from ts_dept where deptkind = 1 and fullid like " + StringTool.SqlQ(strList + "%") + "))";
                        }
                        break;
                    default:
                        strWhere += "";
                        break;
                }
                #endregion
            }


            if (strStatus != "0" && strStatus != "" && strStatus != "-1" && strStatus != "1017")
            {
                strWhere += " AND dealstatusid = " + strStatus;
            }

            if (strMessageBegin != "" && strMessageBegin.Length != 0)
            {
                strWhere += " AND CustTime >= to_date(" + StringTool.SqlQ(strMessageBegin) + ",'yyyy-MM-dd HH24:mi:ss')";
            }

            if (strMessageEnd != "" && strMessageEnd.Length != 0)
            {
                strWhere += " AND CustTime <= to_date(" + StringTool.SqlQ(strMessageEnd + " 23:59:59") + ",'yyyy-MM-dd HH24:mi:ss')";
            }
            string strSql = "";
            switch (tp)
            {
                case eOA_TocdProccess.isMeCheck:
                    //由我登记
                    strSql = @" 1=1 AND flowid in (SELECT distinct flowid FROM es_message WHERE receiverid = " + lngUserID.ToString() +
                    " AND actortype =" + ((int)e_ActorClass.fmMasterActor).ToString() + ") " + strWhere;
                    break;
                case eOA_TocdProccess.isMePartakeCheck:
                    //我参与处理
                    strSql = @"  1=1 AND flowid in (SELECT distinct flowid FROM es_message WHERE receiverid = " + lngUserID.ToString() +
                    " AND (actortype =" + ((int)e_ActorClass.fmMasterActor).ToString() + " or actortype =" + ((int)e_ActorClass.fmAssistActor).ToString() + " )) " + strWhere;
                    break;
                case eOA_TocdProccess.notReturnVisit:
                    //未回访
                    strSql = @"  1=1 AND not exists(select feedbackid from  ea_issues_feedback where flowid = V_CST_Issue.flowid )  AND status= " + ((int)e_FlowStatus.efsEnd).ToString() + "  " + strWhere;
                    break;
                case eOA_TocdProccess.overtime48Event:
                    //超时48小时
                    int iOverHours2 = 48;
                    strSql = @"  1=1 AND ((datediff('Minute',sysdate,nvl(expectendtime,sysdate)) < (0- " + (iOverHours2 * 60).ToString() + ") AND " +
                          " status <> " + ((int)e_FlowStatus.efsEnd).ToString() + ")  OR " +
                          "( datediff('Minute',nvl(endtime,sysdate),nvl(expectendtime,sysdate)) < (0- " + (iOverHours2 * 60).ToString() + ")" +
                          " AND status = " + ((int)e_FlowStatus.efsEnd).ToString() + ") )   " + strWhere;
                    break;
                case eOA_TocdProccess.overtimeEvent:
                    //超时
                    int iOverHours = 0;
                    strSql = @"  1=1 AND ((datediff('Minute',sysdate,nvl(expectendtime,sysdate)) < (0- " + (iOverHours * 60).ToString() + ") AND " +
                          " status <> " + ((int)e_FlowStatus.efsEnd).ToString() + ")  OR " +
                          "( datediff('Minute',nvl(endtime,sysdate),nvl(expectendtime,sysdate)) < (0- " + (iOverHours * 60).ToString() + ")" +
                          " AND status = " + ((int)e_FlowStatus.efsEnd).ToString() + ") )   " + strWhere;
                    break;
                case eOA_TocdProccess.overtimeEventfulfill:
                    //超时完成
                    strSql = @"  1=1 AND datediff('Minute',nvl(endtime,sysdate),nvl(expectendtime,sysdate)) < 0 AND status= " + ((int)e_FlowStatus.efsEnd).ToString() + " " + strWhere;
                    break;
                case eOA_TocdProccess.overtimeEventNOfulfill:
                    //超时未完成
                    strSql = @"  1=1 AND datediff('Minute',nvl(endtime,sysdate),nvl(expectendtime,sysdate)) < 0 AND status= " + ((int)e_FlowStatus.efsHandle).ToString() + " " + strWhere;
                    break;
                default:
                    break;
            }
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, "V_CST_Issue", "*", " ORDER BY smsid DESC", pagesize, pageindex, strSql, ref rowcount);
            ConfigTool.CloseConnection(cn);
            return dt;
        }


        /// <summary>
        /// 有我参与处理的事件
        /// </summary>
        /// <param name="lngUserID"></param>
        /// <param name="lngDeptID"></param>
        /// <param name="lngOrgID"></param>
        /// <param name="tp"></param>
        /// <param name="re"></param>
        /// <returns></returns>
        public static DataTable GetIssuesForMyProccess(long lngUserID, long lngDeptID, long lngOrgID, eOA_TracePeriod tp, RightEntity re)
        {
            string strSQL = "";
            strSQL = @"SELECT  nvl(buildCode,'')||nvl(a.ServiceNo,'') ServiceNo,a.smsid,a.flowid,a.regsysdate,a.subject,a.reguserid,a.regusername,a.Content,a.DealContent,a.CustAddress,a.ServiceTypeID,a.ServiceType,a.Sjwxr,
                    a.RegDeptID,a.RegDeptName,a.CustTime,a.CustName,a.contact,a.ctel,b.status,b.endtime,a.EquipmentCatalogName,a.EquipmentName,a.ReportingTime,a.FinishedTime,a.ServiceKind,
                    a.ServiceLevel,a.EffectName,a.InstancyName,a.CloseReasonName,a.ReSouseName,a.Outtime,a.ServiceTime,
                   datediff('Minute',sysdate,nvl(b.expectendtime,sysdate)) as FlowDiffMinute,a.dealstatus,a.ChangeProblemFlowID,d.customcode,(case a.MastCust when '' then d.MastCustName
				when null then d.MastCustName
				else a.mastCust end ) as MastCustName   " +
                " FROM es_flow b,Cst_Issues a left join br_ecustomer d on a.custid = d.id" +
                " WHERE a.FlowID = b.FlowID  ";
            if (re.CanRead == false)
            {
                //查询出空结果
                strSQL += " AND a.flowid = -1 ";
            }
            else
            {
                switch (tp)
                {
                    case eOA_TracePeriod.eMonth:
                        strSQL += " AND a.RegSysDate >= to_date(to_char(DateAdd('month',-1,sysdate),'yyyy-MM-dd'),'yyyy-MM-dd') ";
                        break;
                    case eOA_TracePeriod.eSeason:
                        strSQL += " AND a.RegSysDate >= dateadd('month',-3,sysdate)";
                        break;
                    case eOA_TracePeriod.eHalfYear:
                        strSQL += " AND a.RegSysDate >= dateadd('month',-6,sysdate)";
                        break;
                    case eOA_TracePeriod.eYear:
                        strSQL += " AND a.RegSysDate >= dateadd('year',-1,sysdate)";
                        break;
                    default:
                        break;
                }
                strSQL += " AND a.flowid in (SELECT distinct flowid FROM es_message WHERE receiverid = " + lngUserID.ToString() +
                    " AND actortype =" + ((int)e_ActorClass.fmMasterActor).ToString() + ") ORDER BY a.smsid DESC";
            }
            OracleConnection cn = ConfigTool.GetConnection();
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigTool.CloseConnection(cn);
            return dt;
        }


        /// <summary>
        /// 根据ＸＭＬ串条件获取事件事件集合
        /// </summary>
        /// <param name="strXmlcond"></param>
        /// <param name="lngUserID"></param>
        /// <param name="lngDeptID"></param>
        /// <param name="lngOrgID"></param>
        /// <param name="re"></param>
        /// <returns></returns>
        public static DataTable GetIssuesForCond(string strXmlcond, long lngUserID, long lngDeptID, long lngOrgID, RightEntity re)
        {

            string strSQL = "";
            string strTmp = "";
            string strList = "";

            string strWhere = strXmlcond;

            strSQL = @"SELECT * FROM V_CST_ISSUE_For_Excel WHERE 1 = 1 ";


            if (re == null || re.CanRead == false)
            {
                //查询出空结果
                strSQL += " AND flowid = -1 ";
            }
            else if (re != null && re.CanRead == true)
            {
                #region 范围条件
                switch (re.RightRange)
                {
                    case eO_RightRange.eFull:
                        strSQL += "";
                        break;
                    case eO_RightRange.ePersonal:
                        strSQL += "AND exists (SELECT messageid FROM es_message WHERE flowid = flowid AND receiverid = " + lngUserID.ToString() + ")";
                        break;
                    case eO_RightRange.eDeptDirect:
                        strSQL += "AND exists (SELECT messageid FROM es_message WHERE flowid = flowid AND recdeptid = " + lngDeptID.ToString() + ")";
                        break;
                    case eO_RightRange.eOrgDirect:
                        strSQL += "AND exists (SELECT messageid FROM es_message WHERE flowid = flowid AND recorgid = " + lngOrgID.ToString() + ")";
                        break;
                    case eO_RightRange.eDept:
                        strList = DeptDP.GetDeptFullID(lngDeptID);
                        if (strList.Trim().Length > 0)
                        {
                            //不是根部门并有找到
                            strSQL += "AND exists (SELECT messageid FROM es_message WHERE flowid = flowid AND recdeptid in (select deptid from ts_dept where fullid like " + StringTool.SqlQ(strList + "%") + "))";
                        }
                        break;
                    case eO_RightRange.eOrg:
                        strList = DeptDP.GetDeptFullID(lngOrgID);
                        if (strList.Trim().Length > 0)
                        {
                            //不是根部门并有找到
                            strSQL += "AND exists (SELECT messageid FROM es_message WHERE flowid = flowid AND recorgid in (select distinct deptid from ts_dept where deptkind = 1 and fullid like " + StringTool.SqlQ(strList + "%") + "))";
                        }
                        break;
                    default:
                        strSQL += "";
                        break;
                }
                #endregion

                if (!String.IsNullOrEmpty(strWhere))
                {
                    string strCustID = string.Empty;
                    string strFlowID = string.Empty;

                    XmlTextReader tr = new XmlTextReader(new StringReader(strXmlcond));
                    while (tr.Read())
                    {
                        if (tr.Name == "Field" && tr.NodeType == XmlNodeType.Element)
                        {
                            strTmp = tr.GetAttribute("Value").Trim();
                            switch (tr.GetAttribute("FieldName"))
                            {
                                case "CustID":
                                    strCustID = strTmp;
                                    break;
                                case "FlowID":
                                    strFlowID = strTmp;
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                    tr.Close();

                    if (strCustID != string.Empty && CTools.ToInt64(strCustID) > 0)
                        strSQL += " and CustID=" + strCustID;


                    if (strFlowID != string.Empty && CTools.ToInt64(strFlowID) > 0)
                        strSQL += " and FlowID=" + strFlowID;

                }
            }

            strSQL = strSQL + " ORDER BY smsid DESC";

            OracleConnection cn = ConfigTool.GetConnection();
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigTool.CloseConnection(cn);
            return dt;
        }



        /// <summary>
        /// 根据ＸＭＬ串条件获取事件事件集合
        /// </summary>
        /// <param name="strXmlcond"></param>
        /// <param name="lngUserID"></param>
        /// <param name="lngDeptID"></param>
        /// <param name="lngOrgID"></param>
        /// <param name="re"></param>
        /// <returns></returns>
        public static DataTable GetIssuesForCondForClientManage(string strXmlcond, long lngUserID, long lngDeptID, long lngOrgID, RightEntity re)
        {

            string strSQL = "";
            string strTmp = "";
            string strList = "";

            string strStatus = "";
            string strSubject = "";
            string strPerson = "";
            string strCustInfo = "";
            string strMessageEnd = string.Empty;
            string strMessageBegin = string.Empty;
            string strEquipmentid = "0";
            string strFlowID = "0";
            string strCustID = "0";
            string strservicetypeid = "0";
            string strEquID = "0";
            string strSericeNo = string.Empty;
            string strFlowStatus = "-1";
            string strSjwxrID = "0";

            string strtype = "0";
            string strEffect = "0";
            string strInstancy = "0";
            string strServiceKind = "0";
            string sRefFlowID = "-1";
            string sArrFlows = "";
            string ctrFCDWTType = "0";

            string strchkIncludeSub = "true";  //事件类别是否包含子类
            string strServiceLevel = "0";      //服务级别
            string strEqu = string.Empty;     //资产信息
            string strEmailState = string.Empty;   //邮件回访状态

            string strHistory = string.Empty;           //是否从查看历史参照传过来
            string strWhere = "";


            #region 获取查询参数的值
            XmlTextReader tr = new XmlTextReader(new StringReader(strXmlcond));
            while (tr.Read())
            {
                if (tr.Name == "Field" && tr.NodeType == XmlNodeType.Element)
                {
                    strTmp = tr.GetAttribute("Value").Trim();
                    switch (tr.GetAttribute("FieldName"))
                    {
                        case "Status":
                            strStatus = strTmp;
                            break;
                        case "Subject":
                            strSubject = strTmp;
                            break;
                        case "Person":
                            strPerson = strTmp;
                            break;
                        case "CustInfo":
                            strCustInfo = strTmp;
                            break;
                        case "MessageBegin":
                            strMessageBegin = strTmp;
                            break;
                        case "MessageEnd":
                            strMessageEnd = strTmp;
                            break;
                        case "Equipmentid":
                            strEquipmentid = strTmp;
                            break;
                        case "FlowID":
                            strFlowID = strTmp;
                            break;
                        case "CustID":
                            strCustID = strTmp;
                            break;
                        case "servicetypeid":
                            strservicetypeid = strTmp;
                            break;
                        case "EquID":
                            strEquID = strTmp;
                            break;
                        case "SericeNo":
                            strSericeNo = strTmp;
                            break;
                        case "FlowStatus":
                            strFlowStatus = strTmp;
                            break;
                        case "SjwxrID":
                            strSjwxrID = strTmp;
                            break;
                        case "ctrFCDServiceType":
                            strtype = strTmp;
                            break;
                        case "strServiceKind":
                            strServiceKind = strTmp;
                            break;
                        case "CtrFCDEffect":
                            strEffect = strTmp;
                            break;
                        case "CtrFCDInstancy":
                            strInstancy = strTmp;
                            break;
                        case "RefFlowID":
                            sRefFlowID = strTmp;
                            break;
                        case "ArrFlows":
                            sArrFlows = strTmp;
                            break;
                        case "ctrFCDWTType":
                            ctrFCDWTType = strTmp;
                            break;
                        case "chkIncludeSub":
                            strchkIncludeSub = strTmp;
                            break;
                        case "ServiceLevel":
                            strServiceLevel = strTmp;
                            break;
                        case "Equ":
                            strEqu = strTmp;
                            break;
                        case "EmailState":
                            strEmailState = strTmp;
                            break;
                        case "IsHistory":
                            strHistory = strTmp;
                            break;
                        default:
                            break;
                    }
                }
            }
            tr.Close();
            #endregion

            if (strFlowStatus != "-1" && strFlowStatus != "")
            {
                strWhere += " AND b.status = " + strFlowStatus;
            }
            if (strStatus != "0" && strStatus != "" && strStatus != "-1" && strStatus != "1017")
                strWhere += " AND a.dealstatusid = " + strStatus;

            if (strSubject.Length != 0)
                strWhere += " AND a.subject like " + StringTool.SqlQ("%" + strSubject.Trim() + "%");

            if (strPerson.Length != 0)
                strWhere += " AND a.RegUserName like " + StringTool.SqlQ("%" + strPerson.Trim() + "%");

            if (strMessageBegin.Length != 0)
                strWhere += " AND a.CustTime >= to_date(" + StringTool.SqlQ(strMessageBegin) + ",'yyyy-MM-dd HH24:mi:ss')";

            //用户信息
            if (strCustInfo.Trim() != string.Empty)
            {
                strWhere += " And (";
                strWhere += " a.CustName like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                strWhere += " OR a.Contact like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                strWhere += " OR d.ShortName like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                strWhere += " OR d.FullName like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                strWhere += " OR d.Address like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                strWhere += " OR d.CustomCode like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                strWhere += " OR d.Tel1 like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                strWhere += " OR a.CTel like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                strWhere += " ) ";
            }

            if (strMessageEnd != "")
                strWhere += " AND a.CustTime <= to_date(" + StringTool.SqlQ(strMessageEnd) + ",'yyyy-MM-dd HH24:mi:ss')";

            if (strEquipmentid.Trim() != "0")
                strWhere += " And nvl(a.Equipmentid,0) = " + strEquipmentid.Trim();

            if (strFlowID.Trim() != "0")
                strWhere += " And a.flowid != " + strFlowID.Trim();

            if (strCustID.Trim() != "0")
                strWhere += " And nvl(a.CustID,0) = " + strCustID.Trim();

            //事件类别
            if ((strservicetypeid == "0" || strservicetypeid == "-1" || strservicetypeid == "1001") && strchkIncludeSub.ToLower() == "true")
            {
            }
            else if (strchkIncludeSub.ToLower() == "true")
            {
                string strFullID = CatalogDP.GetCatalogFullID(long.Parse(strservicetypeid));
                strWhere += @" And Exists(select CatalogID,FullID from Es_Catalog Where CatalogID=servicetypeid And SUBSTR(FullID,1,"
                    + strFullID.Length.ToString() + ") = " + StringTool.SqlQ(strFullID) + ")";
            }
            else
            {
                strWhere += " And a.servicetypeid=" + strservicetypeid.Trim();
            }

            if (ctrFCDWTType.Trim() != "0" && ctrFCDWTType.Trim() != "1002" && ctrFCDWTType.Trim() != "-1")
            {
                strWhere += " And a.ServiceKindID=" + ctrFCDWTType.Trim();
            }


            if ((strHistory == "true" && decimal.Parse(strEquID) > 0) || (strHistory == "false" && decimal.Parse(strEquID) > 0))
            {
                strWhere += " And a.Equipmentid=" + strEquID.Trim();
            }
            else if (strHistory == "true" && (strEquID.Trim() == "0" || strEquID.Trim() == "-1"))
            {
                //查看历史参照；当资产ID为0时，说明未选择任何资产，此时历史参照记录应为空
                strWhere += " And 1=2 ";
            }
            if (strSericeNo.Trim() != string.Empty)
            {
                strWhere += " And a.buildCode||a.ServiceNo like " + StringTool.SqlQ("%" + strSericeNo + "%");
            }
            //工程师ID
            if (strSjwxrID != "0" && strSjwxrID != "")
            {
                string[] sjwxrID = strSjwxrID.Split(',');
                int[] sjwxrId = new int[sjwxrID.Length];
                for (int i = 0; i < sjwxrID.Length; i++)
                {
                    sjwxrId[i] = Convert.ToInt32(sjwxrID[i]);
                }

                ArrayList array = new ArrayList();
                for (int i = 1; i <= sjwxrId.Length; i++)
                {
                    List<int[]> lst_Combination = PermutationAndCombination<int>.GetCombination(sjwxrId, i);
                    for (int j = 0; j < lst_Combination.Count; j++)
                    {
                        int[] arr = lst_Combination[j];
                        string rowString = "";
                        for (int r = 0; r < arr.Length; r++)
                        {
                            rowString += arr[r] + ",";
                        }
                        array.Add(rowString.Substring(0, rowString.LastIndexOf(",")));
                    }
                }
                strWhere += " and sjwxrID in (";
                string str = "";
                for (int i = 0; i < array.Count; i++)
                {
                    str += StringTool.SqlQ(array[i].ToString()) + ",";
                }

                strWhere += str.Substring(0, str.LastIndexOf(",")) + ")";
            }

            if (strEffect != "0" && strEffect != "" && strEffect != "-1" && strEffect != "1023")
                strWhere += " AND a.EffectID = " + strEffect;
            if (strInstancy != "0" && strInstancy != "" && strInstancy != "-1" && strInstancy != "1024")
                strWhere += " AND a.InstancyID = " + strInstancy;

            if (sRefFlowID != "-1")  //投诉单查找相关联的事件单
            {
                strWhere += " And  a.FlowID In (Select RelFlowID from Cst_BytsRel where Cst_BytsRel.FlowID=" + sRefFlowID + ")";
            }
            if (sArrFlows != string.Empty)
            {
                string[] arr = sArrFlows.Split(',');
                if (arr.Length > 1)
                    strWhere += " and a.FlowID In(";
                for (int i = 0; i < arr.Length - 1; i++)
                {
                    if (i != arr.Length - 2)
                        strWhere += arr[i] + ",";
                    else
                        strWhere += arr[i] + ")";
                }
            }

            //服务级别 
            if (strServiceLevel.Trim() != "0")
            {
                strWhere += " And a.ServiceLevelID=" + strServiceLevel.Trim();
            }
            //资产信息
            if (strEqu.Trim() != string.Empty)
            {
                strWhere += " And Exists(select ID from Equ_Desk where EquipmentID=ID and (";
                strWhere += " Name like " + StringTool.SqlQ("%" + strEqu.Trim() + "%");
                strWhere += " OR Code like " + StringTool.SqlQ("%" + strEqu.Trim() + "%");
                strWhere += " OR Positions like " + StringTool.SqlQ("%" + strEqu.Trim() + "%");
                strWhere += " OR SerialNumber like " + StringTool.SqlQ("%" + strEqu.Trim() + "%");
                strWhere += " OR ProvideName like " + StringTool.SqlQ("%" + strEqu.Trim() + "%");
                strWhere += " OR Model like " + StringTool.SqlQ("%" + strEqu.Trim() + "%");
                strWhere += " OR ItemCode like " + StringTool.SqlQ("%" + strEqu.Trim() + "%");
                strWhere += " OR CostomName like " + StringTool.SqlQ("%" + strEqu.Trim() + "%");
                strWhere += " OR Breed like " + StringTool.SqlQ("%" + strEqu.Trim() + "%");
                strWhere += " OR CatalogName like " + StringTool.SqlQ("%" + strEqu.Trim() + "%");
                strWhere += " )) ";
            }
            //邮件回访状态
            if (strEmailState != string.Empty)
            {
                strWhere += " And nvl(EmailState,0)=" + strEmailState;
            }
            strSQL = @"SELECT nvl(buildCode,'')||nvl(a.ServiceNo,'') ServiceNo,a.smsid,a.flowid,a.regsysdate,a.subject,a.email,a.reguserid,a.Content,a.DealContent,a.CustAddress,a.ServiceTypeID,a.ServiceType,a.Sjwxr,
                      a.regusername,a.RegDeptID,a.RegDeptName,a.CustTime,a.CustName,a.contact,a.ctel,b.status,b.endtime,a.EquipmentCatalogName,a.EquipmentName,a.ReportingTime,a.FinishedTime,a.ServiceKind,
                      a.ServiceLevel,a.EffectName,a.InstancyName,a.CloseReasonName,a.ReSouseName,a.Outtime,a.ServiceTime,
                      datediff('Minute',sysdate,nvl(b.expectendtime,sysdate)) as FlowDiffMinute,a.dealstatus,a.ChangeProblemFlowID,
                d.customcode,(case when nvl(a.MastCust,'')='' then d.MastCustName
				    else a.mastCust end ) as MastCustName   " +
                    " FROM es_flow b,Cst_Issues a left join br_ecustomer d on a.custid = d.id " +
                    " WHERE a.FlowID = b.FlowID ";

            if (sRefFlowID == "-1")
            {
                if (re == null || re.CanRead == false)
                {
                    //查询出空结果
                    strSQL += " AND a.flowid = -1 ";
                }
                else if (re != null && re.CanRead == true)
                {
                    #region 范围条件
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
                    #endregion
                }
            }
            strSQL = strSQL + strWhere;
            strSQL = strSQL + " ORDER BY a.smsid DESC";

            OracleConnection cn = ConfigTool.GetConnection();
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigTool.CloseConnection(cn);
            return dt;
        }



        /// <summary>
        /// 根据ＸＭＬ串条件获取事件事件集合
        /// </summary>
        /// <param name="strXmlcond"></param>
        /// <param name="lngUserID"></param>
        /// <param name="lngDeptID"></param>
        /// <param name="lngOrgID"></param>
        /// <param name="re"></param>
        /// <returns></returns>
        public static DataTable GetIssuesForCond3333333333333333333333(string strXmlcond, long lngUserID, long lngDeptID, long lngOrgID, RightEntity re)
        {

            string strSQL = "";
            string strTmp = "";
            string strList = "";

            string strStatus = "";
            string strSubject = "";
            string strPerson = "";
            string strCustInfo = "";
            string strMessageEnd = string.Empty;
            string strMessageBegin = string.Empty;
            string strEquipmentid = "0";
            string strFlowID = "0";
            string strCustID = "0";
            string strservicetypeid = "0";
            string strEquID = "0";
            string strSericeNo = string.Empty;
            string strFlowStatus = "-1";
            string strSjwxrID = "0";

            string strtype = "0";
            string strEffect = "0";
            string strInstancy = "0";
            string strServiceKind = "0";
            string sRefFlowID = "-1";
            string sArrFlows = "";
            string ctrFCDWTType = "0";

            string strchkIncludeSub = "true";  //事件类别是否包含子类
            string strServiceLevel = "0";      //服务级别
            string strEqu = string.Empty;     //资产信息
            string strEmailState = string.Empty;   //邮件回访状态

            string strHistory = string.Empty;           //是否从查看历史参照传过来
            string strWhere = "";


            #region 获取查询参数的值
            XmlTextReader tr = new XmlTextReader(new StringReader(strXmlcond));
            while (tr.Read())
            {
                if (tr.Name == "Field" && tr.NodeType == XmlNodeType.Element)
                {
                    strTmp = tr.GetAttribute("Value").Trim();
                    switch (tr.GetAttribute("FieldName"))
                    {
                        case "Status":
                            strStatus = strTmp;
                            break;
                        case "Subject":
                            strSubject = strTmp;
                            break;
                        case "Person":
                            strPerson = strTmp;
                            break;
                        case "CustInfo":
                            strCustInfo = strTmp;
                            break;
                        case "MessageBegin":
                            strMessageBegin = strTmp;
                            break;
                        case "MessageEnd":
                            strMessageEnd = strTmp;
                            break;
                        case "Equipmentid":
                            strEquipmentid = strTmp;
                            break;
                        case "FlowID":
                            strFlowID = strTmp;
                            break;
                        case "CustID":
                            strCustID = strTmp;
                            break;
                        case "servicetypeid":
                            strservicetypeid = strTmp;
                            break;
                        case "EquID":
                            strEquID = strTmp;
                            break;
                        case "SericeNo":
                            strSericeNo = strTmp;
                            break;
                        case "FlowStatus":
                            strFlowStatus = strTmp;
                            break;
                        case "SjwxrID":
                            strSjwxrID = strTmp;
                            break;
                        case "ctrFCDServiceType":
                            strtype = strTmp;
                            break;
                        case "strServiceKind":
                            strServiceKind = strTmp;
                            break;
                        case "CtrFCDEffect":
                            strEffect = strTmp;
                            break;
                        case "CtrFCDInstancy":
                            strInstancy = strTmp;
                            break;
                        case "RefFlowID":
                            sRefFlowID = strTmp;
                            break;
                        case "ArrFlows":
                            sArrFlows = strTmp;
                            break;
                        case "ctrFCDWTType":
                            ctrFCDWTType = strTmp;
                            break;
                        case "chkIncludeSub":
                            strchkIncludeSub = strTmp;
                            break;
                        case "ServiceLevel":
                            strServiceLevel = strTmp;
                            break;
                        case "Equ":
                            strEqu = strTmp;
                            break;
                        case "EmailState":
                            strEmailState = strTmp;
                            break;
                        case "IsHistory":
                            strHistory = strTmp;
                            break;
                        default:
                            break;
                    }
                }
            }
            tr.Close();
            #endregion

            if (strFlowStatus != "-1" && strFlowStatus != "")
            {
                strWhere += " AND b.status = " + strFlowStatus;
            }
            if (strStatus != "0" && strStatus != "" && strStatus != "-1" && strStatus != "1017")
                strWhere += " AND a.dealstatusid = " + strStatus;

            if (strSubject.Length != 0)
                strWhere += " AND a.subject like " + StringTool.SqlQ("%" + strSubject.Trim() + "%");

            if (strPerson.Length != 0)
                strWhere += " AND a.RegUserName like " + StringTool.SqlQ("%" + strPerson.Trim() + "%");

            if (strMessageBegin.Length != 0)
                strWhere += " AND a.CustTime >= to_date(" + StringTool.SqlQ(strMessageBegin) + ",'yyyy-MM-dd HH24:mi:ss')";

            //用户信息
            if (strCustInfo.Trim() != string.Empty)
            {
                strWhere += " And (";
                strWhere += " a.CustName like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                strWhere += " OR a.Contact like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                strWhere += " OR d.ShortName like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                strWhere += " OR d.FullName like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                strWhere += " OR d.Address like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                strWhere += " OR d.CustomCode like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                strWhere += " OR d.Tel1 like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                strWhere += " OR a.CTel like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                strWhere += " ) ";
            }

            if (strMessageEnd != "")
                strWhere += " AND a.CustTime <= to_date(" + StringTool.SqlQ(strMessageEnd) + ",'yyyy-MM-dd HH24:mi:ss')";

            if (strEquipmentid.Trim() != "0")
                strWhere += " And nvl(a.Equipmentid,0) = " + strEquipmentid.Trim();

            if (strFlowID.Trim() != "0")
                strWhere += " And a.flowid != " + strFlowID.Trim();

            if (strCustID.Trim() != "0")
                strWhere += " And nvl(a.CustID,0) = " + strCustID.Trim();

            //事件类别
            if ((strservicetypeid == "0" || strservicetypeid == "-1" || strservicetypeid == "1001") && strchkIncludeSub.ToLower() == "true")
            {
            }
            else if (strchkIncludeSub.ToLower() == "true")
            {
                string strFullID = CatalogDP.GetCatalogFullID(long.Parse(strservicetypeid));
                strWhere += @" And Exists(select CatalogID,FullID from Es_Catalog Where CatalogID=servicetypeid And SUBSTR(FullID,1,"
                    + strFullID.Length.ToString() + ") = " + StringTool.SqlQ(strFullID) + ")";
            }
            else
            {
                strWhere += " And a.servicetypeid=" + strservicetypeid.Trim();
            }

            if (ctrFCDWTType.Trim() != "0" && ctrFCDWTType.Trim() != "1002" && ctrFCDWTType.Trim() != "-1")
            {
                strWhere += " And a.ServiceKindID=" + ctrFCDWTType.Trim();
            }


            if ((strHistory == "true" && decimal.Parse(strEquID) > 0) || (strHistory == "false" && decimal.Parse(strEquID) > 0))
            {
                strWhere += " And a.Equipmentid=" + strEquID.Trim();
            }
            else if (strHistory == "true" && (strEquID.Trim() == "0" || strEquID.Trim() == "-1"))
            {
                //查看历史参照；当资产ID为0时，说明未选择任何资产，此时历史参照记录应为空
                strWhere += " And 1=2 ";
            }
            if (strSericeNo.Trim() != string.Empty)
            {
                strWhere += " And a.buildCode||a.ServiceNo like " + StringTool.SqlQ("%" + strSericeNo + "%");
            }
            //工程师ID
            if (strSjwxrID != "0" && strSjwxrID != "")
            {
                string[] sjwxrID = strSjwxrID.Split(',');
                int[] sjwxrId = new int[sjwxrID.Length];
                for (int i = 0; i < sjwxrID.Length; i++)
                {
                    sjwxrId[i] = Convert.ToInt32(sjwxrID[i]);
                }

                ArrayList array = new ArrayList();
                for (int i = 1; i <= sjwxrId.Length; i++)
                {
                    List<int[]> lst_Combination = PermutationAndCombination<int>.GetCombination(sjwxrId, i);
                    for (int j = 0; j < lst_Combination.Count; j++)
                    {
                        int[] arr = lst_Combination[j];
                        string rowString = "";
                        for (int r = 0; r < arr.Length; r++)
                        {
                            rowString += arr[r] + ",";
                        }
                        array.Add(rowString.Substring(0, rowString.LastIndexOf(",")));
                    }
                }
                strWhere += " and sjwxrID in (";
                string str = "";
                for (int i = 0; i < array.Count; i++)
                {
                    str += StringTool.SqlQ(array[i].ToString()) + ",";
                }

                strWhere += str.Substring(0, str.LastIndexOf(",")) + ")";
            }

            if (strEffect != "0" && strEffect != "" && strEffect != "-1" && strEffect != "1023")
                strWhere += " AND a.EffectID = " + strEffect;
            if (strInstancy != "0" && strInstancy != "" && strInstancy != "-1" && strInstancy != "1024")
                strWhere += " AND a.InstancyID = " + strInstancy;

            if (sRefFlowID != "-1")  //投诉单查找相关联的事件单
            {
                strWhere += " And  a.FlowID In (Select RelFlowID from Cst_BytsRel where Cst_BytsRel.FlowID=" + sRefFlowID + ")";
            }
            if (sArrFlows != string.Empty)
            {
                string[] arr = sArrFlows.Split(',');
                if (arr.Length > 1)
                    strWhere += " and a.FlowID In(";
                for (int i = 0; i < arr.Length - 1; i++)
                {
                    if (i != arr.Length - 2)
                        strWhere += arr[i] + ",";
                    else
                        strWhere += arr[i] + ")";
                }
            }

            //服务级别 
            if (strServiceLevel.Trim() != "0")
            {
                strWhere += " And a.ServiceLevelID=" + strServiceLevel.Trim();
            }
            //资产信息
            if (strEqu.Trim() != string.Empty)
            {
                strWhere += " And Exists(select ID from Equ_Desk where EquipmentID=ID and (";
                strWhere += " Name like " + StringTool.SqlQ("%" + strEqu.Trim() + "%");
                strWhere += " OR Code like " + StringTool.SqlQ("%" + strEqu.Trim() + "%");
                strWhere += " OR Positions like " + StringTool.SqlQ("%" + strEqu.Trim() + "%");
                strWhere += " OR SerialNumber like " + StringTool.SqlQ("%" + strEqu.Trim() + "%");
                strWhere += " OR ProvideName like " + StringTool.SqlQ("%" + strEqu.Trim() + "%");
                strWhere += " OR Model like " + StringTool.SqlQ("%" + strEqu.Trim() + "%");
                strWhere += " OR ItemCode like " + StringTool.SqlQ("%" + strEqu.Trim() + "%");
                strWhere += " OR CostomName like " + StringTool.SqlQ("%" + strEqu.Trim() + "%");
                strWhere += " OR Breed like " + StringTool.SqlQ("%" + strEqu.Trim() + "%");
                strWhere += " OR CatalogName like " + StringTool.SqlQ("%" + strEqu.Trim() + "%");
                strWhere += " )) ";
            }
            //邮件回访状态
            if (strEmailState != string.Empty)
            {
                strWhere += " And nvl(EmailState,0)=" + strEmailState;
            }
            strSQL = @"SELECT nvl(buildCode,'')||nvl(a.ServiceNo,'') ServiceNo,a.smsid,a.flowid,a.regsysdate,a.subject,a.email,a.reguserid,a.Content,a.DealContent,a.CustAddress,a.ServiceTypeID,a.ServiceType,a.Sjwxr,
                      a.regusername,a.RegDeptID,a.RegDeptName,a.CustTime,a.CustName,a.contact,a.ctel,b.status,b.endtime,a.EquipmentCatalogName,a.EquipmentName,a.ReportingTime,a.FinishedTime,a.ServiceKind,
                      a.ServiceLevel,a.EffectName,a.InstancyName,a.CloseReasonName,a.ReSouseName,a.Outtime,a.ServiceTime,
                      datediff('Minute',sysdate,nvl(b.expectendtime,sysdate)) as FlowDiffMinute,a.dealstatus,a.ChangeProblemFlowID,
                d.customcode,(case when nvl(a.MastCust,'')='' then d.MastCustName
				    else a.mastCust end ) as MastCustName   " +
                    " FROM es_flow b,Cst_Issues a left join br_ecustomer d on a.custid = d.id " +
                    " WHERE a.FlowID = b.FlowID ";

            if (sRefFlowID == "-1")
            {
                if (re == null || re.CanRead == false)
                {
                    //查询出空结果
                    strSQL += " AND a.flowid = -1 ";
                }
                else if (re != null && re.CanRead == true)
                {
                    #region 范围条件
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
                    #endregion
                }
            }
            strSQL = strSQL + strWhere;
            strSQL = strSQL + " ORDER BY a.smsid DESC";

            OracleConnection cn = ConfigTool.GetConnection();
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigTool.CloseConnection(cn);
            return dt;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static DataTable getIssues(long id)
        {
            string SQLstr = @"SELECT  a.flowModelid,nvl(buildCode,'')||nvl(a.ServiceNo,'') ServiceNo,a.smsid,a.flowid,a.regsysdate,a.subject,a.reguserid,a.regusername,
                            a.Content,a.DealContent,a.Sjwxr,a.CustAddress,a.Content,a.ServiceTypeID,a.ServiceType,a.job,a.CustDeptName,
                            a.RegDeptID,a.RegDeptName,a.CustTime,a.CustName,a.contact,a.ctel,a.CustCode,a.Email,a.MastCust,a.EquipmentName,
		                    a.EquPositions,a.EquCode,a.EquSN,a.EquModel,a.EquBreed,b.status,b.endtime,
                            datediff('Minute',sysdate,nvl(b.expectendtime,sysdate)) as FlowDiffMinute,a.dealstatus,a.ChangeProblemFlowID  
                            FROM es_flow b,Cst_Issues a
                            WHERE a.FlowID = b.FlowID  and a.SMSID=" + id.ToString();

            OracleConnection cn = ConfigTool.GetConnection();
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, SQLstr);
            ConfigTool.CloseConnection(cn);
            return dt;
        }

        #region 得到流程当前处理人 GetCurrDealName
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lngFlowID"></param>
        /// <returns></returns>
        public static DataTable GetCurrDealName(long lngFlowID)
        {
            String SQLStr = @"SELECT B.Name,d.NodeName FROM ES_MESSAGE A,Ts_User B,Es_Node C,Es_NodeModel D
                             WHERE A.ReceiverID=B.UserID AND A.NodeID=C.NodeID AND C.NodeModelID=D.NodeModelID 
	                            AND C.FlowModelID=D.FlowModelID AND (A.Status=20 OR A.Status=10 OR  A.Status=40) AND A.FlowID=" + lngFlowID.ToString();
            OracleConnection cn = ConfigTool.GetConnection();
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, SQLStr);
            ConfigTool.CloseConnection(cn);
            return dt;
        }
        #endregion







        /// <summary>
        /// 根据ＸＭＬ串条件获取事件事件集合
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
        public DataTable GetIssuesForCond(string strXmlcond, long lngUserID, long lngDeptID, long lngOrgID, RightEntity re, int pagesize, int pageindex, ref int rowcount)
        {
            string strTmp = "";
            string strList = "";

            string strStatus = "";
            string strSubject = "";
            string strPerson = "";
            string strCustInfo = "";
            string strMessageEnd = string.Empty;
            string strMessageBegin = string.Empty;
            string strEquipmentid = "0";
            string strFlowID = "0";
            string strCustID = "0";
            string strservicetypeid = "0";
            string strEquID = "0";
            string strSericeNo = string.Empty;
            string strFlowStatus = "-1";

            string strtype = "0";
            string strEffect = "0";
            string strInstancy = "0";
            string strServiceKind = "0";
            string sRefFlowID = "-1";
            string sArrFlows = "";
            string ctrFCDWTType = "0";
            string strSjwxrID = "0";                 //工程师
            string strCustName = "";               //客户名称


            string strchkIncludeSub = "true";  //事件类别是否包含子类
            string strServiceLevel = "0";      //服务级别
            string strEqu = string.Empty;     //资产信息
            string strEmailState = string.Empty;   //邮件回访状态

            string strWhere = " 1=1 ";

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
                        strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_CST_Issue.flowid AND receiverid = " + lngUserID.ToString() + ")";
                        break;
                    case eO_RightRange.eDeptDirect:
                        strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_CST_Issue.flowid AND recdeptid = " + lngDeptID.ToString() + ")";
                        break;
                    case eO_RightRange.eOrgDirect:
                        strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_CST_Issue.flowid AND recorgid = " + lngOrgID.ToString() + ")";
                        break;
                    case eO_RightRange.eDept:
                        strList = DeptDP.GetDeptFullID(lngDeptID);
                        if (strList.Trim().Length > 0)
                        {
                            //不是根部门并有找到
                            strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_CST_Issue.flowid AND recdeptid in (select deptid from ts_dept where fullid like " + StringTool.SqlQ(strList + "%") + "))";
                        }
                        break;
                    case eO_RightRange.eOrg:
                        strList = DeptDP.GetDeptFullID(lngOrgID);
                        if (strList.Trim().Length > 0)
                        {
                            //不是根部门并有找到
                            strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_CST_Issue.flowid AND recorgid in (select distinct deptid from ts_dept where deptkind = 1 and fullid like " + StringTool.SqlQ(strList + "%") + "))";
                        }
                        break;
                    default:
                        strWhere += "";
                        break;
                }

                if (!String.IsNullOrEmpty(strXmlcond))
                    strWhere = String.Format("{0} AND {1}", strWhere, strXmlcond);


                #endregion
            }

            OracleConnection cn = ConfigTool.GetConnection();
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, "V_CST_Issue", "*", " ORDER BY smsid DESC", pagesize, pageindex, strWhere, ref rowcount);
            ConfigTool.CloseConnection(cn);
            return dt;
        }









        /// <summary>
        /// 根据ＸＭＬ串条件获取事件事件集合
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
        public DataTable GetIssuesForCond22222222222222222222222(string strXmlcond, long lngUserID, long lngDeptID, long lngOrgID, RightEntity re, int pagesize, int pageindex, ref int rowcount)
        {
            string strTmp = "";
            string strList = "";

            string strStatus = "";
            string strSubject = "";
            string strPerson = "";
            string strCustInfo = "";
            string strMessageEnd = string.Empty;
            string strMessageBegin = string.Empty;
            string strEquipmentid = "0";
            string strFlowID = "0";
            string strCustID = "0";
            string strservicetypeid = "0";
            string strEquID = "0";
            string strSericeNo = string.Empty;
            string strFlowStatus = "-1";

            string strtype = "0";
            string strEffect = "0";
            string strInstancy = "0";
            string strServiceKind = "0";
            string sRefFlowID = "-1";
            string sArrFlows = "";
            string ctrFCDWTType = "0";
            string strSjwxrID = "0";                 //工程师
            string strCustName = "";               //客户名称


            string strchkIncludeSub = "true";  //事件类别是否包含子类
            string strServiceLevel = "0";      //服务级别
            string strEqu = string.Empty;     //资产信息
            string strEmailState = string.Empty;   //邮件回访状态

            string strWhere = " 1=1 ";


            #region 获取查询参数的值
            XmlTextReader tr = new XmlTextReader(new StringReader(strXmlcond));
            while (tr.Read())
            {
                if (tr.Name == "Field" && tr.NodeType == XmlNodeType.Element)
                {
                    strTmp = tr.GetAttribute("Value").Trim();
                    switch (tr.GetAttribute("FieldName"))
                    {
                        case "CustName"://客户名称
                            strCustName = strTmp;
                            break;
                        case "IssueNo"://事件单号
                            strSericeNo = strTmp;
                            break;
                        case "Status"://事件状态
                            strStatus = strTmp;
                            break;
                        case "Subject"://标题
                            strSubject = strTmp;
                            break;
                        case "Person"://登单人
                            strPerson = strTmp;
                            break;
                        case "UserPSjzxr"://工程师ID【新增】
                            strSjwxrID = strTmp;
                            break;
                        case "MessageBegin"://发生时间开始
                            strMessageBegin = strTmp;
                            break;
                        case "MessageEnd"://发生时间结束
                            strMessageEnd = strTmp;
                            break;
                        case "servicetypeid"://事件类别
                            strservicetypeid = strTmp;
                            break;
                        case "FlowStatus"://流程状态
                            strFlowStatus = strTmp;
                            break;
                        case "CtrFCDEffect"://影响度
                            strEffect = strTmp;
                            break;
                        case "CtrFCDInstancy": //紧急度
                            strInstancy = strTmp;
                            break;
                        case "ctrFCDWTType"://事件性质
                            ctrFCDWTType = strTmp;
                            break;
                        case "ServiceLevel"://服务级别
                            strServiceLevel = strTmp;
                            break;
                        case "Equ"://资产信息
                            strEqu = strTmp;
                            break;
                        case "EmailState"://邮件回访状态
                            strEmailState = strTmp;
                            break;
                        default:
                            break;
                    }
                }
            }
            tr.Close();
            #endregion

            //客户名称
            if (strCustName.Trim() != string.Empty)
            {
                strWhere += " And CustName like " + StringTool.SqlQ("%" + strCustName + "%");
            }

            //事件单号
            if (strSericeNo.Trim() != string.Empty)
            {
                strWhere += " And ServiceNo like " + StringTool.SqlQ("%" + strSericeNo + "%");
            }

            //事件状态
            if (strStatus != "0" && strStatus != "" && strStatus != "-1" && strStatus != "1017")
                strWhere += " AND dealstatusid = " + strStatus;

            //标题
            if (strSubject.Length != 0)
                strWhere += " AND subject like " + StringTool.SqlQ("%" + strSubject.Trim() + "%");

            //登单人
            if (strPerson.Length != 0)
                strWhere += " AND RegUserName like " + StringTool.SqlQ("%" + strPerson.Trim() + "%");

            //工程师ID
            if (strSjwxrID != "0" && strSjwxrID != "")
            {
                string[] sjwxrID = strSjwxrID.Split(',');
                int[] sjwxrId = new int[sjwxrID.Length];
                strWhere += " AND (";
                for (int i = 0; i < sjwxrID.Length; i++)
                {
                    strWhere += "  INSTR(sjwxrID,'" + sjwxrID[i].ToString() + "',1,1)>0 ";
                    if (i < sjwxrID.Length - 1)
                    {
                        strWhere += " OR ";
                    }
                }
                strWhere += ") ";
                //ArrayList array = new ArrayList();
                //for (int i = 1; i <= sjwxrId.Length; i++)
                //{
                //    List<int[]> lst_Combination = PermutationAndCombination<int>.GetCombination(sjwxrId, i);
                //    for (int j = 0; j < lst_Combination.Count; j++)
                //    {
                //        int[] arr=lst_Combination[j];
                //        string rowString = "";
                //        for (int r = 0; r < arr.Length; r++)
                //        {
                //            rowString += arr[r] + ",";
                //        }
                //        array.Add(rowString.Substring(0,rowString.LastIndexOf(",")));
                //    }
                //}
                //strWhere += " and sjwxrID in (";
                //string str = "";
                //for (int i = 0; i < array.Count; i++)
                //{
                //    str +=StringTool.SqlQ(array[i].ToString())+",";
                //}

                //strWhere +=str.Substring(0,str.LastIndexOf(","))+")";
            }

            //发生开始时间
            if (strMessageBegin.Length != 0)
                strWhere += " AND CustTime >= to_date(" + StringTool.SqlQ(strMessageBegin) + ",'yyyy-MM-dd HH24:mi:ss')";

            //发生结束时间
            if (strMessageEnd != "")
                strWhere += " AND CustTime <= to_date(" + StringTool.SqlQ(strMessageEnd) + ",'yyyy-MM-dd HH24:mi:ss')";

            //事件类别
            if ((strservicetypeid == "0" || strservicetypeid == "-1" || strservicetypeid == "1001") && strchkIncludeSub.ToLower() == "true")
            {
            }
            else if (strchkIncludeSub.ToLower() == "true")
            {
                string strFullID = CatalogDP.GetCatalogFullID(long.Parse(strservicetypeid));
                strWhere += @" And Exists(select CatalogID,FullID from Es_Catalog Where CatalogID=servicetypeid And SUBSTR(FullID,1,"
                    + strFullID.Length.ToString() + ") = " + StringTool.SqlQ(strFullID) + ")";
            }
            else
            {
                strWhere += " And servicetypeid=" + strservicetypeid.Trim();
            }

            //流程状态
            if (strFlowStatus != "-1" && strFlowStatus != "")
            {
                strWhere += " AND status = " + strFlowStatus;
            }

            //影响度
            if (strEffect != "0" && strEffect != "" && strEffect != "-1" && strEffect != "1023")
                strWhere += " AND EffectID = " + strEffect;

            //紧急度
            if (strInstancy != "0" && strInstancy != "" && strInstancy != "-1" && strInstancy != "1024")
                strWhere += " AND InstancyID = " + strInstancy;

            //事件性质
            if (ctrFCDWTType.Trim() != "0" && ctrFCDWTType.Trim() != "1002" && ctrFCDWTType.Trim() != "-1")
            {
                strWhere += " And ServiceKindID=" + ctrFCDWTType.Trim();
            }

            //服务级别 
            if (strServiceLevel.Trim() != "0")
            {
                strWhere += " And ServiceLevelID=" + strServiceLevel.Trim();
            }
            //资产信息
            if (strEqu.Trim() != string.Empty)
            {
                strWhere += " And (";
                strWhere += " EquipmentName like " + StringTool.SqlQ("%" + strEqu.Trim() + "%");
                strWhere += " OR EquPositions like " + StringTool.SqlQ("%" + strEqu.Trim() + "%");
                strWhere += " OR EquCode like " + StringTool.SqlQ("%" + strEqu.Trim() + "%");
                strWhere += " OR EquSN like " + StringTool.SqlQ("%" + strEqu.Trim() + "%");
                strWhere += " OR EquModel like " + StringTool.SqlQ("%" + strEqu.Trim() + "%");
                strWhere += " OR EquBreed like " + StringTool.SqlQ("%" + strEqu.Trim() + "%");
                strWhere += " ) ";
            }

            //邮件回访状态
            //if (strEmailState != string.Empty)
            //{
            //    strWhere += " And nvl(EmailState,0)=" + strEmailState;
            //}

            //用户信息
            if (strCustInfo.Trim() != string.Empty)
            {
                strWhere += " And (";
                strWhere += " CustName like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                strWhere += " OR Contact like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                strWhere += " OR CustCode like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                strWhere += " OR CustAddress like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                strWhere += " OR MastCust like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                strWhere += " OR Email like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                strWhere += " OR CTel like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                strWhere += " ) ";
            }

            if (sRefFlowID != "-1")  //投诉单查找相关联的事件单
            {
                strWhere += " And  FlowID In (Select RelFlowID from Cst_BytsRel where Cst_BytsRel.FlowID=" + sRefFlowID + ")";
            }
            if (sArrFlows != string.Empty)
            {
                string[] arr = sArrFlows.Split(',');
                if (arr.Length > 1)
                    strWhere += " and FlowID In(";
                for (int i = 0; i < arr.Length - 1; i++)
                {
                    if (i != arr.Length - 2)
                        strWhere += arr[i] + ",";
                    else
                        strWhere += arr[i] + ")";
                }
            }

            if (sRefFlowID == "-1")
            {
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
                            strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_CST_Issue.flowid AND receiverid = " + lngUserID.ToString() + ")";
                            break;
                        case eO_RightRange.eDeptDirect:
                            strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_CST_Issue.flowid AND recdeptid = " + lngDeptID.ToString() + ")";
                            break;
                        case eO_RightRange.eOrgDirect:
                            strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_CST_Issue.flowid AND recorgid = " + lngOrgID.ToString() + ")";
                            break;
                        case eO_RightRange.eDept:
                            strList = DeptDP.GetDeptFullID(lngDeptID);
                            if (strList.Trim().Length > 0)
                            {
                                //不是根部门并有找到
                                strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_CST_Issue.flowid AND recdeptid in (select deptid from ts_dept where fullid like " + StringTool.SqlQ(strList + "%") + "))";
                            }
                            break;
                        case eO_RightRange.eOrg:
                            strList = DeptDP.GetDeptFullID(lngOrgID);
                            if (strList.Trim().Length > 0)
                            {
                                //不是根部门并有找到
                                strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_CST_Issue.flowid AND recorgid in (select distinct deptid from ts_dept where deptkind = 1 and fullid like " + StringTool.SqlQ(strList + "%") + "))";
                            }
                            break;
                        default:
                            strWhere += "";
                            break;
                    }


                    #endregion
                }
            }
            OracleConnection cn = ConfigTool.GetConnection();
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, "V_CST_Issue", "*", " ORDER BY smsid DESC", pagesize, pageindex, strWhere, ref rowcount);
            ConfigTool.CloseConnection(cn);
            return dt;
        }

        public static DataTable getCST_ISSUE_LISTFASTQUERY(string FormId, string LoginName, string Name)
        {
            Name = Name == "==选择收藏查询条件==" ? "Temp1" : Name;
            string SQLstr = "";
            if (Name.Trim() == string.Empty)
            {
                SQLstr = @"SELECT  ID,Name,FORMID,LOGINNAME,SQLWhere,SQLText ,DISPLAYCOLUMN
                            FROM CST_ISSUE_QUERYSave
                            WHERE  nvl(SQLWhere,' ')!='Temp1' AND  FORMID=" + StringTool.SqlQ(FormId) + " AND LOGINNAME=" + StringTool.SqlQ(LoginName) + " order by SN desc";
            }
            else
            {
                if (Name == "Temp1")
                {
                    SQLstr = @"SELECT  ID,Name,FORMID,LOGINNAME,SQLWhere,SQLText ,DISPLAYCOLUMN
                            FROM CST_ISSUE_QUERYSave
                            WHERE  nvl(SQLWhere,' ')='Temp1' AND  FORMID=" + StringTool.SqlQ(FormId) + " AND LOGINNAME=" + StringTool.SqlQ(LoginName) + "  AND Name=" + StringTool.SqlQ(Name);
                }
                else
                {
                    SQLstr = @"SELECT  ID,Name,FORMID,LOGINNAME,SQLWhere,SQLText,DISPLAYCOLUMN
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

        public static void updateCST_ISSUE_WhereNums(string sloginame, string sname)
        {
            string SQLstr = "update CST_ISSUE_QUERYSave set SN=nvl(SN,0)+1 where FORMID='CST_Issue_List' AND  nvl(SQLWhere,' ')!='Temp1' and  LOGINNAME=" + StringTool.SqlQ(sloginame) + " and Name=" + StringTool.SqlQ(sname);
            OracleConnection cn = new OracleConnection(System.Configuration.ConfigurationSettings.AppSettings["SQLConnString"]);

            try
            {
                int i = OracleDbHelper.ExecuteNonQuery(cn, CommandType.Text, SQLstr);
            }
            finally { ConfigTool.CloseConnection(cn); }
        }

        public static void deleteCST_ISSUE_Where(string sloginame, string sname)
        {
            string SQLstr = "delete from  CST_ISSUE_QUERYSave  where FORMID='CST_Issue_List' and  nvl(SQLWhere,' ')!='Temp1'  and  LOGINNAME=" + StringTool.SqlQ(sloginame) + " and Name=" + StringTool.SqlQ(sname);
            OracleConnection cn = new OracleConnection(System.Configuration.ConfigurationSettings.AppSettings["SQLConnString"]);

            try
            {
                int i = OracleDbHelper.ExecuteNonQuery(cn, CommandType.Text, SQLstr);
            }
            finally { ConfigTool.CloseConnection(cn); }
        }

        #region 事件单点击查询时，查询的记录
        /// <summary>
        /// 事件单点击查询时，查询的记录
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
        public DataTable GetIssuesForCondNew1(string strXmlcond, long lngUserID, long lngDeptID, long lngOrgID, RightEntity re, int pagesize, int pageindex, ref int rowcount)
        {
            string strTmp = "";
            string strList = "";

            string strCustInfo = "";                        //客户信息
            string strSericeNo = string.Empty;                  //事件单号
            string BeginTime = "";
            string EndTime = "";
            string strContent = "";                      //详细描述

            string strWhere = " 1=1 ";

            #region 获取查询参数的值
            XmlTextReader tr = new XmlTextReader(new StringReader(strXmlcond));
            while (tr.Read())
            {
                if (tr.Name == "Field" && tr.NodeType == XmlNodeType.Element)
                {
                    strTmp = tr.GetAttribute("Value").Trim();
                    switch (tr.GetAttribute("FieldName"))
                    {
                        case "CustInfo":
                            strCustInfo = strTmp;
                            break;
                        case "SericeNo":
                            strSericeNo = strTmp;
                            break;
                        case "Content":
                            strContent = strTmp;
                            break;
                        case "RegSysDateBegin":
                            BeginTime = strTmp;
                            break;
                        case "RegSysDateEnd":
                            EndTime = strTmp;
                            break;
                        default:
                            break;
                    }
                }
            }
            tr.Close();
            #endregion


            //用户信息
            if (strCustInfo.Trim() != string.Empty)
            {
                strWhere += " And (";
                strWhere += " CustName like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                strWhere += " OR Contact like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                strWhere += " OR CustCode like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                strWhere += " OR CustAddress like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                //strWhere += " OR MastCust like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                strWhere += " OR Email like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                strWhere += " OR CTel like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                strWhere += " OR Subject like" + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
            }

            if (strSericeNo.Trim() != string.Empty)
            {
                strWhere += " OR ServiceNo like " + StringTool.SqlQ("%" + strSericeNo + "%") + ")";
            }
            if (strContent.Trim() != string.Empty)
            {
                strWhere += "OR Content like " + StringTool.SqlQ("%" + strContent + "%") + ")";
            }
            if (BeginTime.Trim() != string.Empty)
            {
                strWhere += " And RegSysDate>= to_date(" + StringTool.SqlQ(BeginTime) + ",'yyyy-MM-dd HH24:mi:ss')";
            }
            if (EndTime.Trim() != string.Empty)
            {
                strWhere += " And RegSysDate<= to_date(" + StringTool.SqlQ(EndTime) + ",'yyyy-MM-dd HH24:mi:ss')";
            }


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
                        strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_CST_Issue.flowid AND receiverid = " + lngUserID.ToString() + ")";
                        break;
                    case eO_RightRange.eDeptDirect:
                        strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_CST_Issue.flowid AND recdeptid = " + lngDeptID.ToString() + ")";
                        break;
                    case eO_RightRange.eOrgDirect:
                        strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_CST_Issue.flowid AND recorgid = " + lngOrgID.ToString() + ")";
                        break;
                    case eO_RightRange.eDept:
                        strList = DeptDP.GetDeptFullID(lngDeptID);
                        if (strList.Trim().Length > 0)
                        {
                            //不是根部门并有找到
                            strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_CST_Issue.flowid AND recdeptid in (select deptid from ts_dept where fullid like " + StringTool.SqlQ(strList + "%") + "))";
                        }
                        break;
                    case eO_RightRange.eOrg:
                        strList = DeptDP.GetDeptFullID(lngOrgID);
                        if (strList.Trim().Length > 0)
                        {
                            //不是根部门并有找到
                            strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_CST_Issue.flowid AND recorgid in (select distinct deptid from ts_dept where deptkind = 1 and fullid like " + StringTool.SqlQ(strList + "%") + "))";
                        }
                        break;
                    default:
                        strWhere += "";
                        break;
                }


                #endregion
            }

            OracleConnection cn = ConfigTool.GetConnection();
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, "V_CST_Issue", "*", " ORDER BY smsid DESC", pagesize, pageindex, strWhere, ref rowcount);
            ConfigTool.CloseConnection(cn);
            return dt;
        }
        #endregion

        #region 事件单点击查询时，查询的记录【导出excel】
        /// <summary>
        /// 事件单点击查询时，查询的记录【导出excel】
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
        public static DataTable GetIssuesForCondNew1(string strXmlcond, long lngUserID, long lngDeptID, long lngOrgID, RightEntity re)
        {
            string strTmp = string.Empty;

            string strSQL = string.Empty;
            string strList = string.Empty;


            //            strSQL = @"SELECT  nvl(buildCode,'')||nvl(a.ServiceNo,'') ServiceNo,a.smsid,a.flowid,a.regsysdate,a.subject,a.reguserid,a.Content,a.DealContent,a.CustAddress,a.ServiceTypeID,a.ServiceType,a.Sjwxr,
            //                      a.regusername,a.RegDeptID,a.RegDeptName,a.CustTime,a.CustName,a.contact,a.ctel,b.status,b.endtime,a.EquipmentCatalogName,a.EquipmentName,a.ReportingTime,a.FinishedTime,a.ServiceKind,
            //                      a.ServiceLevel,a.EffectName,a.InstancyName,a.CloseReasonName,a.ReSouseName,a.Outtime,a.ServiceTime,
            //                      datediff('Minute',sysdate,nvl(b.expectendtime,sysdate)) as FlowDiffMinute,a.dealstatus,a.ChangeProblemFlowID,d.customcode,(case a.MastCust when '' then d.MastCustName
            //				    when null then d.MastCustName
            //				    else a.mastCust end ) as MastCustName   " +
            //                    " FROM es_flow b,Cst_Issues a left join br_ecustomer d on a.custid = d.id " +
            //                    " WHERE a.FlowID = b.FlowID ";
            strSQL = @"SELECT  * from V_CST_Issue_Excel where 1=1";

            string strCustInfo = string.Empty;                  //客户信息
            string strSericeNo = string.Empty;                  //事件单号
            string strContent = string.Empty;                  //详细内容
            string BeginTime = string.Empty;
            string EndTime = string.Empty;
            string strWhere = string.Empty;                     //where条件

            #region 获取查询参数的值
            XmlTextReader tr = new XmlTextReader(new StringReader(strXmlcond));
            while (tr.Read())
            {
                if (tr.Name == "Field" && tr.NodeType == XmlNodeType.Element)
                {
                    strTmp = tr.GetAttribute("Value").Trim();
                    switch (tr.GetAttribute("FieldName"))
                    {
                        case "CustInfo":
                            strCustInfo = strTmp;
                            break;
                        case "SericeNo":
                            strSericeNo = strTmp;
                            break;
                        case "Content":
                            strContent = strTmp;
                            break;
                        case "RegSysDateBegin":
                            BeginTime = strTmp;
                            break;
                        case "RegSysDateEnd":
                            EndTime = strTmp;
                            break;
                        default:
                            break;
                    }
                }
            }
            tr.Close();
            #endregion

            //用户信息
            if (strCustInfo.Trim() != string.Empty)
            {
                strWhere += " And (";
                strWhere += " CustName like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                strWhere += " OR Contact like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                strWhere += " OR CustCode like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                strWhere += " OR CustAddress like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                //strWhere += " OR CustDeptName like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                strWhere += " OR Email like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                strWhere += " OR CTel like " + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");
                strWhere += " OR Subject like" + StringTool.SqlQ("%" + strCustInfo.Trim() + "%");

            }

            if (strSericeNo.Trim() != string.Empty)
            {
                strWhere += " OR ServiceNo like " + StringTool.SqlQ("%" + strSericeNo + "%") + ")";
            }
            if (strContent.Trim() != string.Empty)
            {
                strWhere += "OR Content like " + StringTool.SqlQ("%" + strContent + "%") + ")";
            }
            if (BeginTime.Trim() != string.Empty)
            {
                strWhere += " And RegSysDate>= to_date(" + StringTool.SqlQ(BeginTime) + ",'yyyy-MM-dd HH24:mi:ss')";
            }
            if (EndTime.Trim() != string.Empty)
            {
                strWhere += " And RegSysDate<= to_date(" + StringTool.SqlQ(EndTime) + ",'yyyy-MM-dd HH24:mi:ss')";
            }


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
                        strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = a.flowid AND receiverid = " + lngUserID.ToString() + ")";
                        break;
                    case eO_RightRange.eDeptDirect:
                        strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = a.flowid AND recdeptid = " + lngDeptID.ToString() + ")";
                        break;
                    case eO_RightRange.eOrgDirect:
                        strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = a.flowid AND recorgid = " + lngOrgID.ToString() + ")";
                        break;
                    case eO_RightRange.eDept:
                        strList = DeptDP.GetDeptFullID(lngDeptID);
                        if (strList.Trim().Length > 0)
                        {
                            //不是根部门并有找到
                            strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = a.flowid AND recdeptid in (select deptid from ts_dept where fullid like " + StringTool.SqlQ(strList + "%") + "))";
                        }
                        break;
                    case eO_RightRange.eOrg:
                        strList = DeptDP.GetDeptFullID(lngOrgID);
                        if (strList.Trim().Length > 0)
                        {
                            //不是根部门并有找到
                            strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = a.flowid AND recorgid in (select distinct deptid from ts_dept where deptkind = 1 and fullid like " + StringTool.SqlQ(strList + "%") + "))";
                        }
                        break;
                    default:
                        strWhere += "";
                        break;
                }


                #endregion
            }
            strSQL += strWhere;
            OracleConnection cn = ConfigTool.GetConnection();
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigTool.CloseConnection(cn);
            return dt;
        }
        #endregion

        #region 事件单查询，第一次进入时，默认查询正在处理、发生时间为近一个月的权限范围内的记录
        /// <summary>
        /// 事件单查询，第一次进入时，默认查询正在处理、发生时间为近一个月的权限范围内的记录
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
        public DataTable GetIssuesForCondNew_Init(string strXmlcond, long lngUserID, long lngDeptID, long lngOrgID, RightEntity re, int pagesize, int pageindex, ref int rowcount)
        {
            string strList = string.Empty;
            string strWhere = " 1=1 ";

            //正在处理、发生时间为近一个月的权限范围内的记录
            strWhere += " and Status = 20 and RegSysDate >= to_date(" + StringTool.SqlQ(DateTime.Now.AddMonths(-1).ToShortDateString()) + ",'yyyy-MM-dd')";
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
                        strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_CST_Issue.flowid AND receiverid = " + lngUserID.ToString() + ")";
                        break;
                    case eO_RightRange.eDeptDirect:
                        strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_CST_Issue.flowid AND recdeptid = " + lngDeptID.ToString() + ")";
                        break;
                    case eO_RightRange.eOrgDirect:
                        strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_CST_Issue.flowid AND recorgid = " + lngOrgID.ToString() + ")";
                        break;
                    case eO_RightRange.eDept:
                        strList = DeptDP.GetDeptFullID(lngDeptID);
                        if (strList.Trim().Length > 0)
                        {
                            //不是根部门并有找到
                            strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_CST_Issue.flowid AND recdeptid in (select deptid from ts_dept where fullid like " + StringTool.SqlQ(strList + "%") + "))";
                        }
                        break;
                    case eO_RightRange.eOrg:
                        strList = DeptDP.GetDeptFullID(lngOrgID);
                        if (strList.Trim().Length > 0)
                        {
                            //不是根部门并有找到
                            strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_CST_Issue.flowid AND recorgid in (select distinct deptid from ts_dept where deptkind = 1 and fullid like " + StringTool.SqlQ(strList + "%") + "))";
                        }
                        break;
                    default:
                        strWhere += "";
                        break;
                }
                #endregion
            }

            OracleConnection cn = ConfigTool.GetConnection();
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, "V_CST_Issue", "*", " ORDER BY smsid DESC", pagesize, pageindex, strWhere, ref rowcount);
            ConfigTool.CloseConnection(cn);
            return dt;
        }
        #endregion

        #region 事件单查询，第一次进入时，默认查询正在处理、发生时间为近一个月的权限范围内的记录
        /// <summary>
        /// 事件单查询，第一次进入时，默认查询正在处理、发生时间为近一个月的权限范围内的记录
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
        public DataTable GetIssuesForCondNew_Init(string strXmlcond, long lngUserID, int pagesize, int pageindex, ref int rowcount)
        {
            string strList = string.Empty;
            string strWhere = " 1=1 ";

            //正在处理、发生时间为近一个月的权限范围内的记录
            strWhere += " AND Status = 20 and RegSysDate >= to_date(to_char(DateAdd('month',-1,sysdate),'yyyy-MM-dd'),'yyyy-MM-dd') AND  RegUserID= " + lngUserID.ToString();
            OracleConnection cn = ConfigTool.GetConnection();
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, "V_CST_Issue", "*", " ORDER BY smsid DESC", pagesize, pageindex, strWhere, ref rowcount);
            ConfigTool.CloseConnection(cn);
            return dt;
        }
        #endregion

        #region 事件单查询，第一次进入时，默认查询正在处理、发生时间为近一个月的权限范围内的记录【导出excel】
        /// <summary>
        /// 事件单查询，第一次进入时，默认查询正在处理、发生时间为近一个月的权限范围内的记录【导出excel】
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
        public static DataTable GetIssuesForCondNew_Init(string strXmlcond, long lngUserID, long lngDeptID, long lngOrgID, RightEntity re)
        {
            string strSQL = string.Empty;
            string strList = string.Empty;
            strSQL = @"SELECT  nvl(buildCode,'')||nvl(a.ServiceNo,'') ServiceNo,a.smsid,a.flowid,a.regsysdate,a.subject,a.reguserid,a.Content,a.DealContent,a.CustAddress,a.ServiceTypeID,a.ServiceType,a.Sjwxr,
                      a.regusername,a.RegDeptID,a.RegDeptName,a.CustTime,a.CustName,a.contact,a.ctel,b.status,b.endtime,a.EquipmentCatalogName,a.EquipmentName,a.ReportingTime,a.FinishedTime,a.ServiceKind,
                      a.ServiceLevel,a.EffectName,a.InstancyName,a.CloseReasonName,a.ReSouseName,a.Outtime,a.ServiceTime,
                      datediff('Minute',sysdate,nvl(b.expectendtime,sysdate)) as FlowDiffMinute,a.dealstatus,a.ChangeProblemFlowID,d.customcode,(case a.MastCust 
				    when null then d.MastCustName
				    else a.mastCust end ) as MastCustName   " +
                    " FROM es_flow b,Cst_Issues a left join br_ecustomer d on a.custid = d.id " +
                    " WHERE a.FlowID = b.FlowID ";
            string strWhere = string.Empty;

            //正在处理、发生时间为近一个月的权限范围内的记录
            strWhere += " and Status = 20 and RegSysDate >= to_date(" + StringTool.SqlQ(DateTime.Now.AddMonths(-1).ToShortDateString()) + ",'yyyy-MM-dd')";
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
                        strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = a.flowid AND receiverid = " + lngUserID.ToString() + ")";
                        break;
                    case eO_RightRange.eDeptDirect:
                        strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = a.flowid AND recdeptid = " + lngDeptID.ToString() + ")";
                        break;
                    case eO_RightRange.eOrgDirect:
                        strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = a.flowid AND recorgid = " + lngOrgID.ToString() + ")";
                        break;
                    case eO_RightRange.eDept:
                        strList = DeptDP.GetDeptFullID(lngDeptID);
                        if (strList.Trim().Length > 0)
                        {
                            //不是根部门并有找到
                            strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = a.flowid AND recdeptid in (select deptid from ts_dept where fullid like " + StringTool.SqlQ(strList + "%") + "))";
                        }
                        break;
                    case eO_RightRange.eOrg:
                        strList = DeptDP.GetDeptFullID(lngOrgID);
                        if (strList.Trim().Length > 0)
                        {
                            //不是根部门并有找到
                            strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = a.flowid AND recorgid in (select distinct deptid from ts_dept where deptkind = 1 and fullid like " + StringTool.SqlQ(strList + "%") + "))";
                        }
                        break;
                    default:
                        strWhere += "";
                        break;
                }
                #endregion
            }

            strSQL = strSQL + strWhere;
            OracleConnection cn = ConfigTool.GetConnection();
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigTool.CloseConnection(cn);
            return dt;
        }
        #endregion

        /// <summary>
        /// 事件量统计
        /// </summary>
        /// <param name="strBeginDate"></param>
        /// <param name="strEndDate"></param>
        /// <param name="lngBuildID"></param>
        /// <returns></returns>
        public static DataTable GetCstSomeStat(string strBeginDate, string strEndDate, long lngBuildID)
        {
            string sWhere = string.Empty;
            sWhere += " and  regdate Between " + StringTool.SqlQ(strBeginDate) + " AND " + StringTool.SqlQ(strEndDate + " 23:59:59") +
                           ((lngBuildID == 0) ? "" : " AND buildID =" + lngBuildID);
            string sSql = @"select CName '统计项',nvl(FinishCount,0) '已完成',nvl(NeedCount,0) '应回访',nvl(FeedBackCount,0) '已回访',
                            nvl(AllCount,0) '总单数',nvl(LevelCount,0) '满意数',
	                        nvl(FareAmount,0) '材料费',nvl(HumanAmount,0) '人工费',nvl(TotalHours,0) '工时'
                                from Es_DictItem a 
                                left outer join 
                                --已完成,服务工时
                                (select wttypeid,wtType,Count(SMSID) as FinishCount,Sum(TotalHours) as TotalHours from Cst_Issues where dealstatusid=8 " + sWhere + @" Group By wttypeid,wtType) b
	                                ON a.ItemID=b.wttypeid
                                left outer join 
                                --总单数
                                (select wttypeid,wtType,Count(SMSID) as AllCount from Cst_Issues where 1=1 " + sWhere + @"  Group By wttypeid,wtType) c
	                                ON a.ItemID=c.wttypeid
                                left outer join 
                                --已回访
                                (select wttypeid,wtType,Count(SMSID) as FeedBackCount from Cst_Issues where dealstatusid=8 and (wttypeid=1 or wttypeid=2)
                                and Exists(select a.FlowID from EA_Issues_FeedBack a where a.FlowID=Cst_Issues.FlowID " + sWhere + @")
                                Group By wttypeid,wtType) d
	                                ON a.ItemID=d.wttypeid
                                left outer join
                                --满意数
                                (select wttypeid,wtType,Count(SMSID) as LevelCount from Cst_Issues where statislevelid=2 " + sWhere + @"  Group By wttypeid,wtType) e
	                                ON a.ItemID=e.wttypeid
                                left outer join
                                --人工费，材料费
                                (select wttypeid,wtType,Sum(FareAmount) FareAmount,Sum(HumanAmount) HumanAmount from Cst_Issues,Cst_Cost
                                 where Cst_Cost.SMSID=Cst_Issues.SMSID " + sWhere + @"  Group By wttypeid,wtType) f
	                                ON a.ItemID=f.wttypeid
                                --应回访
                                left outer join
                                (select wttypeid,wtType,Count(SMSID) as NeedCount from Cst_Issues where dealstatusid=8 and (wttypeid=1 or wttypeid=2) " + sWhere + @" Group By wttypeid,wtType) g
	                                ON a.ItemID=g.wttypeid
                                where DictID=1850103";

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
        /// <summary>
        /// 统计工时
        /// </summary>
        /// <param name="strBeginDate"></param>
        /// <param name="strEndDate"></param>
        /// <param name="lngDeptID"></param>
        /// <returns></returns>
        public static DataTable GetCstBuildStaffStat(string strBeginDate, string strEndDate, long lngDeptID)
        {
            string sWhere = string.Empty;
            sWhere += " and  regdate Between " + StringTool.SqlQ(strBeginDate) + " AND " + StringTool.SqlQ(strEndDate + " 23:59:59") +
                           ((lngDeptID == 0) ? "" : " and  sjwxrid in (select ID from  Cst_ServiceStaff where Deleted=0 and  BlongDeptID=" + lngDeptID + ")");
            string sSql = @"select sjwxrid ID,sjwxr 姓名,Count(SMSID) as 单数,Sum(TotalHours) as 合计时间 
                            from Cst_Issues 
                            where dealstatusid=8 " + sWhere + " Group By sjwxrid,sjwxr";

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

        #region 通过FLOWID取得事件资料
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lngFlowID"></param>
        /// <returns></returns>
        public static DataTable GetEventData(long lngFlowID)
        {
            string strSQL = @"SELECT  nvl(buildCode,'')||nvl(a.ServiceNo,'') ServiceNo,a.smsid,a.flowid,a.regsysdate,a.subject,a.reguserid,a.regusername,
                   a.RegDeptID,a.RegDeptName,a.CustTime,a.CustName,a.contact,a.ctel,b.status,b.endtime,
                  datediff('Minute',sysdate,nvl(b.expectendtime,sysdate)) as FlowDiffMinute,a.dealstatus,a.ChangeProblemFlowID,d.customcode,a.FinishedTime " +
                " FROM es_flow b,Cst_Issues a left join br_ecustomer d on a.custid = d.id" +
                " WHERE a.FlowID = b.FlowID ";
            strSQL += " And a.FlowID=" + lngFlowID.ToString();
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
        #endregion

        #region 更新服务是否邮件通知
        /// <summary>
        /// 更新状态为已通知邮件回访
        /// </summary>
        /// <param name="lngFlowID"></param>
        public static void UpdateEmailState(long lngFlowID)
        {
            string strSQL = " Update Cst_Issues Set EmailState=1 where nvl(EmailState,0)<>2 and FlowID=" + lngFlowID.ToString();
            OracleConnection cn = ConfigTool.GetConnection();
            try
            {
                OracleDbHelper.ExecuteNonQuery(cn, CommandType.Text, strSQL);
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
        public static DataTable GetDate(long lngFlowID)
        {
            string strSQL = " Select * FROM  Cst_Issues where FlowID=" + lngFlowID.ToString();
            OracleConnection cn = ConfigTool.GetConnection();
            DataTable dt = null;
            try
            {
                dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                ConfigTool.CloseConnection(cn);
            }
            return dt;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="lngFlowID"></param>
        public static void UpdateEmailState(OracleTransaction trans, long lngFlowID)
        {
            string strSQL = " Update Cst_Issues Set EmailState=1 where nvl(EmailState,0)<>2 and FlowID=" + lngFlowID.ToString();
            try
            {
                OracleDbHelper.ExecuteNonQuery(trans, CommandType.Text, strSQL);
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
            }
        }

        /// <summary>
        /// 更新状态为已邮件回访
        /// </summary>
        /// <param name="lngFlowID"></param>
        public static void UpdateEmailStateTwo(long lngFlowID)
        {
            string strSQL = " Update Cst_Issues Set EmailState=2 where FlowID=" + lngFlowID.ToString();
            OracleConnection cn = ConfigTool.GetConnection();
            try
            {
                OracleDbHelper.ExecuteNonQuery(cn, CommandType.Text, strSQL);
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

        #region 资产统计 GetEquStts
        /// <summary>
        /// 资产统计 GetEquStts
        /// </summary>
        /// <param name="sWhere"></param>
        /// <param name="sOrder"></param>
        /// <returns></returns>
        public DataTable GetEquStts(String sWhere, String sOrder)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            cn.Open();
            String strSQL = @"select a.MastCust,b.CatalogName,count(a.EquipmentID) as Equstts
                                from Cst_Issues a,Equ_Desk b,es_Flow c
                               where a.EquipmentID=b.ID and a.FlowID = c.FlowID and c.status = 20 and MastCust is not null";
            if (CommonDP.GetConfigValue("Other", "DataLimit") != null && CommonDP.GetConfigValue("Other", "DataLimit") == "1")  //是否限制
            {
                if (System.Web.HttpContext.Current.Session["MastLimitList"] != null)
                {
                    sWhere = sWhere + " and CustID<>0 and Exists(select ID from Br_ECustomer b where b.ID=CustID and b.MastCustID in (" + System.Web.HttpContext.Current.Session["MastLimitList"].ToString() + "))";
                }
            }
            try
            {
                strSQL += sWhere;
                strSQL += " group by MastCust,CatalogName ";
                strSQL += sOrder;
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
                return dt;
            }
            catch
            {
                throw;
            }
            finally
            {
                ConfigTool.CloseConnection(cn);
            }

        }
        #endregion

        #region GetBatchDealData 取得总行服务台批量处理数据
        /// <summary>
        /// 取得总行服务台批量处理数据
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="swhere"></param>
        /// <returns></returns>
        public static DataTable GetBatchDealData(long UserId, string swhere)
        {
            string strSQL = string.Empty;        //存放SQL字符串
            //审批批量分派环节的业务数据全部抓出来
            string strWhere = string.Empty;
            if (swhere != string.Empty)
            {
                strWhere += swhere;
            }
            strSQL = @"SELECT nvl(E.buildCode||E.ServiceNo,'') ServiceNo,E.*,A.MessageID,A.ActionID,c.nodemodelID as nodemodelidnew,h.MastCustName,
                              datediff('Minute',sysdate,nvl(a.expected,sysdate)) as flowdiffminute,
	                          D.endtime,D.status,D.AppID,E.ServiceKind,a.senderusername,a.sendernodename 
                        FROM Es_Flow D, V_ES_MESSAGE  A
	                    ,Es_Node  B,Es_NodeModel C,Cst_Issues E 
                        left join br_ecustomer h on E.custid = h.id
                        WHERE A.NodeID=B.NodeID AND B.nodemodelid=C.nodemodelid 
	                    AND B.flowmodelid=C.flowmodelid AND E.FlowID=A.FlowID 
	                    AND D.FlowID=A.FlowID AND E.FlowID=A.FlowID "
                + " AND A.status=" + ((int)e_MessageStatus.emsHandle).ToString()
                + " AND A.Deleted =" + (int)e_Deleted.eNormal
                + " AND A.actortype =" + ((int)e_ActorClass.fmMasterActor).ToString()
                + " AND D.AppId =1026"
                + " AND A.ReceiverId=" + UserId
                + " AND EXISTS (SELECT BusId FROM Es_BusDef WHERE C.nodeBusId=BusID AND busId=6083 AND AppId=1026)";
            OracleConnection cn = ConfigTool.GetConnection("SQLConnString");
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigTool.CloseConnection(cn);
            return dt;
        }
        #endregion

        #region 待办事项，按应用分
        /// <summary>
        /// 待办事项，按应用分
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="appid"></param>
        /// <param name="eac"></param>
        /// <param name="ems"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static DataTable getFlowWorkNow(long UserId, long appid, int type)
        {

            string strSQL = string.Empty;        //存放SQL字符串

            if (appid == 1026)//事件
            {
                if (type == 1)
                {
                    strSQL = @"SELECT (E.buildcode || E.Serviceno) as NumberNo,E.ServiceKind,E.ServiceType,E.regusername,D.subject,A.FlowID,A.FActors,E.ServiceLevel," +
                              " A.MessageID,A.ReceiveTime,A.senderusername,A.sendernodename," +
                              " A.IsRead,A.Important,D.Attachment,c.appid,c.AppName," +
                              " datediff('Minute',sysdate,nvl(a.expected,sysdate)) as DiffMinute,d.Name as FlowName,A.actortype " +
                              " FROM cst_issues E,Es_Flow D, V_Es_Message A,Es_App c" +
                              " WHERE E.FlowID=A.FlowID AND D.FlowID=A.FlowID AND E.FlowID=A.FlowID AND D.Appid=c.AppID " +
                              " AND A.status<>" + ((int)e_MessageStatus.emsFinished).ToString() + " AND A.Deleted =" + (int)e_Deleted.eNormal +
                              " AND D.AppId = 1026 AND A.ReceiverId=" + UserId +
                              " order by E.FlowId desc";
                }
                else
                {
                    strSQL = @"SELECT (nvl(f.buildCode,'') || nvl(f.ServiceNo,'')) NumberNo,f.*,d.ID,a.MessageID,a.flowid,b.Flowmodelid,e.nodemodelid,b.appid," +
                              " a.IsRead,a.Important,a.FActors,a.ReceiveTime,a.actortype,b.Subject as zhaiyao,b.Attachment,b.Name as FlowName,c.AppID,c.AppName as AppName," +
                              " datediff('Minute',sysdate,nvl(a.expected,sysdate)) as DiffMinute,b.FlowID,b.status,b.endtime,f.CustName,f.ServiceKind,a.senderusername,a.sendernodename  " +
                              " FROM V_ES_MESSAGE_NotReceiver a,Es_Flow b,Es_App c,Es_ReceiveList d,Es_Node e,Cst_Issues f" +
                              " WHERE d.MessageID = a.MessageID AND a.FlowID = b.FlowID AND b.AppID = c.AppID And a.nodeid=e.nodeid And b.Flowid=f.flowid " +
                              " AND d.ReceiveID = " + UserId.ToString() + " AND a.Deleted =" + (int)e_Deleted.eNormal + " AND a.ReceiverID = 0 AND b.AppId = 1026 " +
                              " ORDER BY a.MessageID DESC";
                }
            }
            else if (appid == 420)//变更
            {
                if (type == 1)
                {
                    strSQL = @"SELECT E.changeno NumberNo,D.subject,A.FlowID,A.FActors,E.LevelName,
                        A.MessageID,A.ReceiveTime,A.senderusername,A.sendernodename,
                        A.IsRead,A.Important,D.Attachment,c.appid,c.AppName,
                        datediff('Minute',sysdate,nvl(a.expected,sysdate)) as DiffMinute,d.Name as FlowName,A.actortype
                        FROM equ_changeservice E,Es_Flow D, V_Es_Message A,Es_App c
   	                    WHERE E.FlowID=A.FlowID AND D.FlowID=A.FlowID AND E.FlowID=A.FlowID AND D.Appid=c.AppID "
                        + " AND A.status<>" + ((int)e_MessageStatus.emsFinished).ToString()
                        + " AND A.Deleted =" + (int)e_Deleted.eNormal
                        + " AND D.AppId = 420 "
                        + " AND A.ReceiverId=" + UserId
                        + " order by E.FlowId desc";
                }
                else
                {
                    strSQL = @"SELECT f.changeno NumberNo,d.ID,a.MessageID,a.flowid,b.Flowmodelid,e.nodemodelid,b.appid,f.LevelName," +
                              " a.IsRead,a.Important,a.FActors,a.ReceiveTime,a.actortype,b.Subject,b.Attachment,b.Name as FlowName,c.AppID,c.AppName as AppName," +
                              " datediff('Minute',sysdate,nvl(a.expected,sysdate)) as DiffMinute,b.FlowID,b.status,b.endtime,f.CustName,a.senderusername,a.sendernodename  " +
                              " FROM V_ES_MESSAGE_NotReceiver a,Es_Flow b,Es_App c,Es_ReceiveList d,Es_Node e,equ_changeservice f" +
                              " WHERE d.MessageID = a.MessageID AND a.FlowID = b.FlowID AND b.AppID = c.AppID And a.nodeid=e.nodeid And b.Flowid=f.flowid " +
                              " AND d.ReceiveID = " + UserId.ToString() + " AND a.Deleted =" + (int)e_Deleted.eNormal + " AND a.ReceiverID = 0 AND b.AppId = 420 " +
                              " ORDER BY a.MessageID DESC";
                }
            }
            else if (appid == 210)//问题
            {
                if (type == 1)
                {
                    strSQL = @"SELECT (nvl(E.buildCode,'') || nvl(E.ProblemNo,'')) NumberNo,E.Problem_LevelName,E.*,A.FlowID,A.FActors,D.subject,
                        A.MessageID,A.ReceiveTime,A.senderusername,A.sendernodename,
                        A.IsRead,A.Important,D.Attachment,c.appid,c.AppName,
                        datediff('Minute',sysdate,nvl(a.expected,sysdate)) as DiffMinute,d.Name as FlowName,A.actortype
                        FROM Pro_ProblemDeal E,Es_Flow D, V_Es_Message A,Es_App c
   	                    WHERE E.FlowID=A.FlowID AND D.FlowID=A.FlowID AND E.FlowID=A.FlowID AND D.Appid=c.AppID "
                        + " AND A.status<>" + ((int)e_MessageStatus.emsFinished).ToString()
                        + " AND A.Deleted =" + (int)e_Deleted.eNormal
                        + " AND D.AppId = 210 "
                        + " AND A.ReceiverId=" + UserId
                        + " order by E.FlowId desc";
                }
                else
                {
                    strSQL = @"SELECT (nvl(f.buildCode,'') || nvl(f.ProblemNo,'')) NumberNo,f.*,d.ID,a.MessageID,a.flowid,b.Flowmodelid,e.nodemodelid,b.appid," +
                              " a.IsRead,a.Important,a.FActors,a.ReceiveTime,a.actortype,b.Subject,b.Attachment,b.Name as FlowName,c.AppID,c.AppName as AppName," +
                              " datediff('Minute',sysdate,nvl(a.expected,sysdate)) as DiffMinute,b.FlowID,b.status,b.endtime,a.senderusername,a.sendernodename  " +
                              " FROM V_ES_MESSAGE_NotReceiver a,Es_Flow b,Es_App c,Es_ReceiveList d,Es_Node e,Pro_ProblemDeal f" +
                              " WHERE d.MessageID = a.MessageID AND a.FlowID = b.FlowID AND b.AppID = c.AppID And a.nodeid=e.nodeid And b.Flowid=f.flowid " +
                              " AND d.ReceiveID = " + UserId.ToString() + " AND a.Deleted =" + (int)e_Deleted.eNormal + " AND a.ReceiverID = 0 AND b.AppId = 210 " +
                              " ORDER BY a.MessageID DESC";
                }

            }
            else if (appid == 1028)//发布
            {
                if (type == 1)
                {
                    strSQL = @"SELECT E.*,A.FlowID,A.FActors,d.Subject,
                        A.MessageID,A.ReceiveTime,A.senderusername,A.sendernodename,
                        A.IsRead,A.Important,D.Attachment,c.appid,c.AppName,
                        datediff('Minute',sysdate,nvl(a.expected,sysdate)) as DiffMinute,d.Name as FlowName,A.actortype
                        FROM OA_RELEASEMANAGEMENT E,Es_Flow D, V_Es_Message A,Es_App c
   	                    WHERE E.FlowID=A.FlowID AND D.FlowID=A.FlowID AND E.FlowID=A.FlowID AND D.Appid=c.AppID "
                        + " AND A.status<>" + ((int)e_MessageStatus.emsFinished).ToString()
                        + " AND A.Deleted =" + (int)e_Deleted.eNormal
                        + " AND D.AppId = 1028 "
                        + " AND A.ReceiverId=" + UserId
                        + " order by E.FlowId desc";
                }
                else
                {
                    strSQL = @"select f.*,d.ID,a.MessageID,a.flowid,b.Flowmodelid,e.nodemodelid,b.appid," +
                              " a.IsRead,a.Important,a.FActors,a.ReceiveTime,a.actortype,b.Subject,b.Attachment,b.Name as FlowName,c.AppID,c.AppName as AppName," +
                              " datediff('Minute',sysdate,nvl(a.expected,sysdate)) as DiffMinute,b.FlowID,b.status,b.endtime ,a.senderusername,a.sendernodename " +
                              " FROM V_ES_MESSAGE_NotReceiver a,Es_Flow b,Es_App c,Es_ReceiveList d,Es_Node e,OA_RELEASEMANAGEMENT f" +
                              " WHERE d.MessageID = a.MessageID AND a.FlowID = b.FlowID AND b.AppID = c.AppID And a.nodeid=e.nodeid And b.Flowid=f.flowid " +
                              " AND d.ReceiveID = " + UserId.ToString() + " AND a.Deleted =" + (int)e_Deleted.eNormal + " AND a.ReceiverID = 0 AND b.AppId = 1028 " +
                              " ORDER BY a.MessageID DESC";
                }
            }
            else//其它
            {
                if (type == 1)
                {
                    strSQL = @"SELECT A.FlowID,D.Name,A.FActors,D.Subject,
                        A.MessageID,A.ReceiveTime,A.senderusername,A.sendernodename,
                        A.IsRead,A.Important,D.Attachment,c.appid,c.AppName,
                        datediff('Minute',sysdate,nvl(a.expected,sysdate)) as DiffMinute,d.Name as FlowName,A.actortype
                        FROM Es_Flow D, V_Es_Message A,Es_App c
   	                    WHERE D.FlowID=A.FlowID AND D.Appid=c.AppID "
                        + " AND A.status<>" + ((int)e_MessageStatus.emsFinished).ToString()
                        + " AND A.Deleted =" + (int)e_Deleted.eNormal
                        + " AND D.AppID <> 1026 AND  D.AppID <> 420 AND D.AppID <> 201 AND D.AppID <> 210 "
                        + " AND A.ReceiverId=" + UserId
                        + " order by a.FlowId desc";
                }
                else
                {
                    strSQL = @"SELECT d.ID,a.MessageID,a.flowid,b.Flowmodelid,e.nodemodelid,b.appid,b.Name," +
                              " a.IsRead,a.Important,a.FActors,a.ReceiveTime,a.actortype,b.Subject,b.Attachment,b.Name as FlowName,c.AppID,c.AppName as AppName," +
                              " datediff('Minute',sysdate,nvl(a.expected,sysdate)) as DiffMinute,b.FlowID,b.status,b.endtime,a.senderusername,a.sendernodename  " +
                              " FROM V_ES_MESSAGE_NotReceiver a,Es_Flow b,Es_App c,Es_ReceiveList d,Es_Node e" +
                              " WHERE d.MessageID = a.MessageID AND a.FlowID = b.FlowID AND b.AppID = c.AppID And a.nodeid=e.nodeid " +
                              " AND d.ReceiveID = " + UserId.ToString() + " AND a.Deleted =" + (int)e_Deleted.eNormal + " AND a.ReceiverID = 0 AND b.AppID <> 1026 AND  b.AppID <> 420 AND b.AppID <> 201 AND b.AppID <> 210 " +
                              " ORDER BY a.MessageID DESC";
                }
            }

            OracleConnection cn = ConfigTool.GetConnection();
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigTool.CloseConnection(cn);
            return dt;
        }
        #endregion

        #region 得到记录数
        /// <summary>
        /// 得到记录数
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="appid"></param>
        /// <param name="eac"></param>
        /// <param name="ems"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static int getFlowWorkNowCount(long UserId, long appid, int type)
        {

            string strSQL = string.Empty;        //存放SQL字符串

            if (appid == 1026)//事件
            {
                if (type == 1)
                {
                    strSQL = @"SELECT Count(*) AS rowcounts" +
                              " FROM cst_issues E,Es_Flow D, V_Es_Message A,Es_App c" +
                              " WHERE E.FlowID=A.FlowID AND D.FlowID=A.FlowID AND E.FlowID=A.FlowID AND D.Appid=c.AppID " +
                              " AND A.status<>" + ((int)e_MessageStatus.emsFinished).ToString() + " AND A.Deleted =" + (int)e_Deleted.eNormal +
                              " AND D.AppId = 1026 AND A.ReceiverId=" + UserId;
                }
                else
                {
                    strSQL = @"SELECT Count(*) AS rowcounts" +
                              " FROM V_ES_MESSAGE_NotReceiver a,Es_Flow b,Es_App c,Es_ReceiveList d,Es_Node e,Cst_Issues f" +
                              " WHERE d.MessageID = a.MessageID AND a.FlowID = b.FlowID AND b.AppID = c.AppID And a.nodeid=e.nodeid And b.Flowid=f.flowid " +
                              " AND d.ReceiveID = " + UserId.ToString() + " AND a.Deleted =" + (int)e_Deleted.eNormal + " AND a.ReceiverID = 0 AND b.AppId = 1026 ";
                }
            }
            else if (appid == 420)//变更
            {
                if (type == 1)
                {
                    strSQL = @"SELECT COUNT(*) as rowcounts
                        FROM equ_changeservice E,Es_Flow D, V_Es_Message A,Es_App c
   	                    WHERE E.FlowID=A.FlowID AND D.FlowID=A.FlowID AND E.FlowID=A.FlowID AND D.Appid=c.AppID "
                        + " AND A.status<>" + ((int)e_MessageStatus.emsFinished).ToString()
                        + " AND A.Deleted =" + (int)e_Deleted.eNormal
                        + " AND D.AppId = 420 "
                        + " AND A.ReceiverId=" + UserId;
                }
                else
                {
                    strSQL = @"SELECT Count(*) AS rowcounts" +
                              " FROM V_ES_MESSAGE_NotReceiver a,Es_Flow b,Es_App c,Es_ReceiveList d,Es_Node e,equ_changeservice f" +
                              " WHERE d.MessageID = a.MessageID AND a.FlowID = b.FlowID AND b.AppID = c.AppID And a.nodeid=e.nodeid And b.Flowid=f.flowid " +
                              " AND d.ReceiveID = " + UserId.ToString() + " AND a.Deleted =" + (int)e_Deleted.eNormal + " AND a.ReceiverID = 0 AND b.AppId = 420 ";
                }
            }
            else if (appid == 210)//问题
            {
                if (type == 1)
                {
                    strSQL = @"SELECT Count(*) AS rowcounts
                        FROM Pro_ProblemDeal E,Es_Flow D, V_Es_Message A,Es_App c
   	                    WHERE E.FlowID=A.FlowID AND D.FlowID=A.FlowID AND E.FlowID=A.FlowID AND D.Appid=c.AppID "
                        + " AND A.status<>" + ((int)e_MessageStatus.emsFinished).ToString()
                        + " AND A.Deleted =" + (int)e_Deleted.eNormal
                        + " AND D.AppId = 210 "
                        + " AND A.ReceiverId=" + UserId;
                }
                else
                {
                    strSQL = @"SELECT Count(*) AS rowcounts" +
                              " FROM V_ES_MESSAGE_NotReceiver a,Es_Flow b,Es_App c,Es_ReceiveList d,Es_Node e,Pro_ProblemDeal f" +
                              " WHERE d.MessageID = a.MessageID AND a.FlowID = b.FlowID AND b.AppID = c.AppID And a.nodeid=e.nodeid And b.Flowid=f.flowid " +
                              " AND d.ReceiveID = " + UserId.ToString() + " AND a.Deleted =" + (int)e_Deleted.eNormal + " AND a.ReceiverID = 0 AND b.AppId = 210 ";
                }
            }
            else if (appid == 1028)//发布
            {
                if (type == 1)
                {
                    strSQL = @"SELECT Count(*) AS rowcounts
                        FROM OA_RELEASEMANAGEMENT E,Es_Flow D, V_Es_Message A,Es_App c
   	                    WHERE E.FlowID=A.FlowID AND D.FlowID=A.FlowID AND E.FlowID=A.FlowID AND D.Appid=c.AppID "
                        + " AND A.status<>" + ((int)e_MessageStatus.emsFinished).ToString()
                        + " AND A.Deleted =" + (int)e_Deleted.eNormal
                        + " AND D.AppId = 1028 "
                        + " AND A.ReceiverId=" + UserId;
                }
                else
                {
                    strSQL = @"SELECT Count(*) AS rowcounts" +
                              " FROM V_ES_MESSAGE_NotReceiver a,Es_Flow b,Es_App c,Es_ReceiveList d,Es_Node e,OA_RELEASEMANAGEMENT f" +
                              " WHERE d.MessageID = a.MessageID AND a.FlowID = b.FlowID AND b.AppID = c.AppID And a.nodeid=e.nodeid And b.Flowid=f.flowid " +
                              " AND d.ReceiveID = " + UserId.ToString() + " AND a.Deleted =" + (int)e_Deleted.eNormal + " AND a.ReceiverID = 0 AND b.AppId = 1028 ";
                }
            }
            else//其它
            {
                if (type == 1)
                {
                    strSQL = @"SELECT Count(*) AS rowcounts
                        FROM Es_Flow D, V_Es_Message A,Es_App c
   	                    WHERE D.FlowID=A.FlowID AND D.Appid=c.AppID "
                        + " AND A.status<>" + ((int)e_MessageStatus.emsFinished).ToString()
                        + " AND A.Deleted =" + (int)e_Deleted.eNormal
                        + " AND c.AppID <> 1026 AND  c.AppID <> 420 AND c.AppID <> 201 AND c.AppID<>210 "
                        + " AND A.ReceiverId=" + UserId;
                }
                else
                {
                    strSQL = @"SELECT Count(*) AS rowcounts" +
                              " FROM V_ES_MESSAGE_NotReceiver a,Es_Flow b,Es_App c,Es_ReceiveList d,Es_Node e" +
                              " WHERE d.MessageID = a.MessageID AND a.FlowID = b.FlowID AND b.AppID = c.AppID And a.nodeid=e.nodeid  AND c.AppID <> 1026 AND  c.AppID <> 420 AND c.AppID <> 201 AND c.AppID<>210 " +
                              " AND d.ReceiveID = " + UserId.ToString() + " AND a.Deleted =" + (int)e_Deleted.eNormal + " AND a.ReceiverID = 0";
                }
            }

            OracleConnection cn = ConfigTool.GetConnection();
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigTool.CloseConnection(cn);
            int row = 0;
            if (dt != null)
                row = int.Parse(dt.Rows[0]["rowcounts"].ToString());
            return row;
        }
        #endregion

        #region 根据流程模型ID得到业务类别ID
        /// <summary>
        /// 根据流程模型ID得到业务类别ID
        /// </summary>
        /// <param name="flowmodelid"></param>
        /// <returns></returns>
        public static int GetBID(long flowmodelid)
        {
            int iCount = 0;
            string strSQL = string.Empty;
            OracleConnection cn = ConfigTool.GetConnection("SQLConnString");

            try
            {
                strSQL = "select flowbusid from es_flowmodel where flowmodelid = " + flowmodelid.ToString();
                OracleDataReader dr = OracleDbHelper.ExecuteReader(cn, CommandType.Text, strSQL);
                while (dr.Read())
                {
                    iCount = dr.IsDBNull(0) == true ? 0 : dr.GetInt32(0);
                    break;
                }
                dr.Close();

                return iCount;
            }
            finally { ConfigTool.CloseConnection(cn); }
        }

        #endregion

        #region 获取事件相关服务监控图表
        /// <summary>
        /// 获取事件相关服务监控图表
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

            string strSql = @"--需处理总数【当月未完成+当月完成】
                                select '需处理总数' as Title,count(1) as counts,'IssueW' as Types
                                  from Cst_Issues A,es_Flow B,Br_ECustomer C
                                 where A.FlowID = B.FlowID and A.CustID = C.ID and C.deleted = 0
                                         and (
                                         (B.status = 30 and to_char(FinishedTime,'yyyy-MM') = to_char(sysdate,'yyyy-MM'))
                                         or B.status = 20)" + strWhere + @"
                                union all
                                --当月新增【事件单的登记时间为当月，除去客户为空和暂存未提交的】
                                select '当月新增' as Title,count(1) as counts,'IssueB' as Types
                                  from Cst_Issues A,es_Flow B,Br_ECustomer C
                                 where A.FlowID = B.FlowID and A.CustID = C.ID and C.deleted = 0
	                               and to_char(RegSysDate,'yyyy-MM') = to_char(sysdate,'yyyy-MM')
	                               and A.flowid NOT IN (select flowid from es_message where status=20 and senderid=0)" + strWhere + @"
                                union all
                                --当月未完成【到本月为止，未完成的】
                                select '当月未完成' as Title,count(1) as counts,'IssueA' as Types
                                  from Cst_Issues A,es_Flow B,Br_ECustomer C
                                where A.FlowID = B.FlowID and A.CustID = C.ID and C.deleted = 0 and B.status = 20" + strWhere + @"
                                union all
                                --当月完成【事件单的完成时间为当月】
                                select '当月完成' as Title,count(1) as counts,'IssueC' as Types
                                  from Cst_Issues A,es_Flow B,Br_ECustomer C
                                where A.FlowID = B.FlowID and A.CustID = C.ID and C.deleted = 0 and B.status = 30 and to_char(FinishedTime,'yyyy-MM') = to_char(sysdate,'yyyy-MM')" + strWhere + @"
                                union all
                                --当月已超时【事件单的登记时间为当月，且根据服务级别判断已超时的】
                                select '当月已超时' as Title,count(1) as counts,'IssueD' as Types
                                from 
                                (
                                select AA.* from 
                                (
                                select A.* from Cst_Issues A,Es_Flow B
                                where  A.FlowID=B.FlowID and datediff('Minute',nvl(endtime,sysdate),nvl(expectendtime,sysdate)) < 0
                                and to_char(RegSysDate,'yyyy-MM') = to_char(sysdate,'yyyy-MM')
                                ) AA,Br_ECustomer C
                                where  AA.CustID = C.ID" + strWhere + @"
                                ) AAA";

            DataTable dt = CommonDP.ExcuteSqlTable(strSql);

            return dt;
        }
        #endregion

        #region 事件单，柱子点开查看详情
        /// <summary>
        /// 事件单，柱子点开查看详情
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

            if (strType == "IssueA")
            {
                //当月未完成
                dt = OracleDbHelper.ExecuteDataTable(cn, "v_IssusA", "*", " ORDER BY smsID DESC", pagesize, pageindex, strWhere, ref rowcount);
            }
            else if (strType == "IssueB")
            {
                //当月新增
                dt = OracleDbHelper.ExecuteDataTable(cn, "v_IssusB", "*", " ORDER BY smsID DESC", pagesize, pageindex, strWhere, ref rowcount);
            }
            else if (strType == "IssueC")
            {
                //当月完成
                dt = OracleDbHelper.ExecuteDataTable(cn, "v_IssusC", "*", " ORDER BY smsID DESC", pagesize, pageindex, strWhere, ref rowcount);
            }
            else if (strType == "IssueD")
            {
                //当月已超时
                dt = OracleDbHelper.ExecuteDataTable(cn, "v_IssusD", "*", " ORDER BY smsID DESC", pagesize, pageindex, strWhere, ref rowcount);
            }
            else if (strType == "IssueW")
            {
                //需处理总数
                dt = OracleDbHelper.ExecuteDataTable(cn, "v_IssusW", "*", " ORDER BY smsID DESC", pagesize, pageindex, strWhere, ref rowcount);
            }
            else
            {
                strSql = @"--无记录
                                select A.*
                                  from Cst_Issues A where 1 = 2";
            }

            return dt;
        }
        #endregion

        #region 得到各应用业务动作
        /// <summary>
        /// 得到各应用业务动作
        /// </summary>
        /// <returns></returns>
        public static DataTable GetBusActions(long AppID)
        {
            string strSql = "select * from es_BusAction where AppID = " + AppID;

            DataTable dt = CommonDP.ExcuteSqlTable(strSql);

            return dt;
        }
        #endregion

        #region 根据工程师ID串获取工程师名称串
        /// <summary>
        /// 根据工程师ID串获取工程师名称串
        /// </summary>
        /// <param name="strUserIDs"></param>
        /// <returns></returns>
        public static string GetStaffNamesByStaffIDs(string strUserIDs)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            string strUserNames = string.Empty;         //用户名称串
            string strSql = "SELECT * FROM Cst_ServiceStaff where deleted = 0 and ID in (" + strUserIDs + ")";

            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSql);
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    strUserNames += row["Name"].ToString() + ",";
                }
            }
            if (strUserNames.Length > 0)
                strUserNames = strUserNames.Substring(0, strUserNames.Length - 1);      //工程师名称串

            return strUserNames;
        }
        #endregion

        #region 事件平均超时时长KPI
        /// <summary>
        /// 事件平均超时时长KPI
        /// </summary>
        /// <param name="type">0标示按月 1标示按年</param>
        /// <param name="p_strWhere">where条件</param>
        /// <returns></returns>
        public static DataTable GetCstOutTimeKPI(int type, string p_strWhere)
        {
            DataTable dt = null;

            OracleParameter[] parms = {
                      new OracleParameter("p_type",OracleType.Number,4),
                      new OracleParameter("p_strWhere",OracleType.NVarChar,2000),
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
            dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.StoredProcedure, "proc_CstOutTimeKPI", parms);
            ConfigTool.CloseConnection(cn);

            return dt;
        }
        #endregion

        #region 事件日均产率KPI
        /// <summary>
        /// 事件日均产率KPI
        /// </summary>
        /// <param name="type">0标示按月 1标示按年</param>
        /// <param name="p_strWhere">where条件</param>
        /// <returns></returns>
        public static DataTable GetCstDayAvgManageKPI(int type, string p_strWhere)
        {
            DataTable dt = null;

            OracleParameter[] parms = {
                      new OracleParameter("p_type",OracleType.Number,4),
                      new OracleParameter("p_strWhere",OracleType.NVarChar,2000),
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
            dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.StoredProcedure, "proc_CstDayAvgManageKPI", parms);
            ConfigTool.CloseConnection(cn);

            return dt;
        }
        #endregion

        #region 满意度异常率KPI
        /// <summary>
        /// 满意度异常率KPI
        /// </summary>
        /// <param name="type">0标示按月 1标示按年</param>
        /// <param name="p_strWhere">where条件</param>
        /// <returns></returns>
        public static DataTable GetCstDegreeRateKPI(int type, string p_strWhere)
        {
            DataTable dt = null;

            OracleParameter[] parms = {
                      new OracleParameter("p_type",OracleType.Number,4),
                      new OracleParameter("p_strWhere",OracleType.NVarChar,2000),
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
            dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.StoredProcedure, "proc_CstDegreeRateKPI", parms);
            ConfigTool.CloseConnection(cn);

            return dt;
        }
        #endregion

        #region 重大事件（故障）次数KPI
        /// <summary>
        /// 重大事件（故障）次数KPI
        /// </summary>
        /// <param name="type">0标示按月 1标示按年</param>
        /// <param name="p_strWhere">where条件</param>
        /// <returns></returns>
        public static DataTable GetCstGreatCountKPI(int type, string p_strWhere)
        {
            DataTable dt = null;

            OracleParameter[] parms = {
                      new OracleParameter("p_type",OracleType.Number,4),
                      new OracleParameter("p_strWhere",OracleType.NVarChar,2000),
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
            dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.StoredProcedure, "proc_CstGreatCountKPI", parms);
            ConfigTool.CloseConnection(cn);

            return dt;
        }
        #endregion

        #region 重大事件平均恢复时间KPI
        /// <summary>
        /// 重大事件平均恢复时间KPI
        /// </summary>
        /// <param name="type">0标示按月 1标示按年</param>
        /// <param name="p_strWhere">where条件</param>
        /// <returns></returns>
        public static DataTable GetCstGreatRecoverAvgTimeKPI(int type, string p_strWhere)
        {
            DataTable dt = null;

            OracleParameter[] parms = {
                      new OracleParameter("p_type",OracleType.Number,4),
                      new OracleParameter("p_strWhere",OracleType.NVarChar,2000),
                      new OracleParameter("P_GetData",OracleType.Cursor)
                };
            parms[0].Value = type;
            parms[1].Value = p_strWhere;

            parms[0].Direction = ParameterDirection.Input;
            parms[1].Direction = ParameterDirection.Input;
            parms[2].Direction = ParameterDirection.Output;

            OracleConnection cn = ConfigTool.GetConnection();
            if (cn.State != ConnectionState.Open)
            {
                cn.Open();
            }
            dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.StoredProcedure, "proc_CstGreatRecoverAvgTimeKPI", parms);
            ConfigTool.CloseConnection(cn);

            return dt;
        }
        #endregion

        #region 重大事件30/60分钟恢复率
        /// <summary>
        /// 重大事件30/60分钟恢复率
        /// </summary>
        /// <param name="type">0标示按月 1标示按年</param>
        /// <param name="p_strWhere">where条件(包括30/60分钟的的条件)</param>
        /// <param name="p_strWhere2">where条件(不包括30/60分钟的的条件)</param>
        /// <returns></returns>
        public static DataTable GetCstGreatRecoverRateKPI(int type, string p_strWhere, string p_strWhere2)
        {
            DataTable dt = null;

            OracleParameter[] parms = {
                      new OracleParameter("@type",OracleType.Number,4),
                      new OracleParameter("@p_strWhere",OracleType.NVarChar,2000),
                      new OracleParameter("@p_strWhere2",OracleType.NVarChar,2000)
                };
            parms[0].Value = type;
            parms[1].Value = p_strWhere;
            parms[2].Value = p_strWhere2;

            OracleConnection cn = ConfigTool.GetConnection();
            if (cn.State != ConnectionState.Open)
            {
                cn.Open();
            }
            dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.StoredProcedure, "proc_CstGreatRecoverRateKPI", parms);
            ConfigTool.CloseConnection(cn);

            return dt;
        }
        #endregion

        #region 事件降低率KPI
        /// <summary>
        /// 事件降低率KPI
        /// </summary>
        /// <param name="p_strWhere">where条件(包含固定年份条件如(datepart(year,RegSysDate)=2011))</param>
        /// <param name="p_strWhere2">where条件(不包含固定年份条件)</param>
        /// <returns></returns>
        public static DataTable GetCstDownRateKPI(string p_strWhere, string p_strWhere2)
        {
            DataTable dt = null;

            OracleParameter[] parms = {
                      new OracleParameter("p_strWhere",OracleType.NVarChar,2000),
                      new OracleParameter("p_strWhere2",OracleType.NVarChar,2000),
                      new OracleParameter("P_GetData",OracleType.Cursor)
                };
            parms[0].Direction = ParameterDirection.Input;
            parms[1].Direction = ParameterDirection.Input;
            parms[2].Direction = ParameterDirection.Output;

            parms[0].Value = p_strWhere;
            parms[1].Value = p_strWhere2;

            OracleConnection cn = ConfigTool.GetConnection();
            if (cn.State != ConnectionState.Open)
            {
                cn.Open();
            }
            dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.StoredProcedure, "proc_CstDownRateKPI", parms);
            ConfigTool.CloseConnection(cn);

            return dt;
        }
        #endregion

        #region 根据流程ID 查询 服务级别时限设置表记录
        /// <summary>
        /// 根据流程ID 查询 服务级别时限设置表记录
        /// </summary>
        /// <param name="strFlowID"></param>
        /// <returns></returns>
        public static DataTable GetFlowBusLimitByFlowID(decimal strFlowID)
        {
            string sSql = "";
            OracleConnection cn = ConfigTool.GetConnection();
            sSql = @"SELECT * from EA_FlowBusLimit where FlowID=" + strFlowID.ToString();
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
        public static DataTable GetFlowBusLimitDetailByFlowID(decimal strFlowID, decimal strGuidID)
        {
            string sSql = "";
            OracleConnection cn = ConfigTool.GetConnection();
            sSql = @"SELECT * from EA_FlowBusLimit where FlowID=" + strFlowID.ToString() + " and GuidID=" + strGuidID;
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

        #region 重复事件

        /// <summary>
        /// 添加重复事件合并时，取出事件单
        /// </summary>
        /// <param name="strFlowList"></param>
        /// <returns></returns>
        public static DataTable GetIssuesSubAdd(string strFlowList)
        {
            string strSQL = @"select nvl(a.BuildCode,'') || nvl(a.ServiceNo,'') AS ServiceNo,SMSID,a.FlowID,a.NodeModelID,a.FlowModelID,
                            ServiceTypeID,ServiceType,ServiceLevelID,ServiceLevel,
                            DealStatusID,DealStatus,a.Subject,Content,
                            RegSysUserID,RegSysUser,RegDeptID,RegDeptName,RegSysDate,
                            b.status,0 FlowDealState  from    Cst_Issues  a,es_flow b    
                            where a.FlowID=b.FlowID   ";
            strSQL += " And a.FlowID In (" + strFlowList + ")";

            OracleConnection cn = ConfigTool.GetConnection();
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigTool.CloseConnection(cn);
            return dt;
        }

        /// <summary>
        /// 重复事件合并时，取出事件单
        /// </summary>
        /// <param name="lngFlowID"></param>
        /// <returns></returns>
        /// 
        public static DataTable GetIssuesSub(long lngFlowID)
        {
            string strSQL = @"select nvl(a.BuildCode,'') || nvl(a.ServiceNo,'') as ServiceNo,SMSID,a.FlowID,a.NodeModelID,a.FlowModelID,
                            ServiceTypeID,ServiceType,ServiceLevelID,ServiceLevel,
                            DealStatusID,DealStatus,a.Subject,Content,
                            RegSysUserID,RegSysUser,RegDeptID,RegDeptName,RegSysDate,
                            b.status,c.FlowDealState
                            from Cst_Issues a,es_flow b,Pro_ProblemRel c
                            where a.FlowID=b.FlowID AND a.FlowID=c.SubFlowID";
            strSQL += " And c.MastFlowID =" + lngFlowID.ToString();

            OracleConnection cn = ConfigTool.GetConnection();
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigTool.CloseConnection(cn);
            return dt;
        }

        /// <summary>
        /// 重复事件合并，关联于
        /// </summary>
        /// <param name="lngFlowID"></param>
        /// <returns></returns>
        public static DataTable GetIssuesSubRel(long lngFlowID)
        {
            string strSQL = @"select distinct nvl(a.BuildCode,'') || nvl(a.ServiceNo,'') as ServiceNo,
                            SMSID,a.FlowID,a.NodeModelID,a.FlowModelID,
                            ServiceTypeID,ServiceType,ServiceLevelID,ServiceLevel,
                            DealStatusID,DealStatus,a.Subject,Content,
                            RegSysUserID,RegSysUser,RegDeptID,RegDeptName,RegSysDate,
                            b.status
                            from Cst_Issues a,es_flow b,Pro_ProblemRel c
                            where a.FlowID=b.FlowID AND a.FlowID=c.MastFlowID ";
            strSQL += " And c.SubFlowID =" + lngFlowID.ToString();

            OracleConnection cn = ConfigTool.GetConnection();
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigTool.CloseConnection(cn);
            return dt;
        }

        #endregion

        /// <summary>
        /// 获得一个list的 对象 数组
        /// </summary>
        /// <param name="EquID"></param>
        /// <returns></returns>
        public static List<EQU_deploy> getEQU_deployList(long EquID)
        {
            List<EQU_deploy> list = new List<EQU_deploy>();
            string strSQL = @"select * from EQU_deploy where EquId=" + EquID.ToString();
            DataTable dt = CommonDP.ExcuteSqlTable(strSQL);
            if (dt.Rows.Count > 0)
            {

                foreach (DataRow dr in dt.Rows)
                {
                    EQU_deploy EQU = new EQU_deploy();
                    EQU.ID = long.Parse(dr["ID"].ToString());
                    EQU.EquID = long.Parse(dr["EquID"].ToString());
                    EQU.FieldID = long.Parse(dr["FieldID"].ToString());
                    EQU.CHName = dr["CHName"].ToString();
                    EQU.Value = dr["value"].ToString();
                    list.Add(EQU);
                }

            }
            return list;
        }

        /// <summary>
        /// 查询 督办信息
        /// </summary>
        /// <param name="flowid">事件单 flowid</param>
        /// <returns>督办信息</returns>
        public DataTable getMonitor(string flowid)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            DataTable dt_Monitor = new DataTable();
            try
            {
                string str_SeachMonitor = string.Format("select * from EA_Monitor where flowid={0}", flowid);
                dt_Monitor = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, str_SeachMonitor);
                return dt_Monitor;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                ConfigTool.CloseConnection(cn);
            }
        }

        #region 陈志文 2012-09-03
        /// <summary>
        /// 根据Flowid得到事件单号
        /// </summary>
        /// <param name="flowid"></param>
        /// <returns></returns>
        public string GetCstIessNo(string flowid)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            string cstno = "";
            try
            {
                string seachno = string.Format("select serviceno,buildcode from cst_issues where flowid ='{0}'", flowid);
                DataTable dtno = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, seachno);
                if (dtno != null && dtno.Rows.Count > 0)
                {
                    cstno = dtno.Rows[0]["buildcode"].ToString() + dtno.Rows[0]["serviceno"].ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                ConfigTool.CloseConnection(cn);
            }

            return cstno;
        }

        /// <summary>
        /// 根据事件单号，删除已发邮件
        /// </summary>
        /// <param name="cstno"></param>
        public void DeleteEmail(string cstno)
        {
            OracleConnection cn = ConfigTool.GetConnection();

            try
            {
                string deletEmail = string.Format("delete from Br_Send_Email where cstno ='{0}'", cstno);
                OracleDbHelper.ExecuteNonQuery(cn, CommandType.Text, deletEmail);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                ConfigTool.CloseConnection(cn);
            }

        }
        #endregion

        /// <summary>
        /// 获取资产故障事件
        /// </summary>
        /// <returns></returns>
        public DataTable Get_AssetsAvailability(string begindate, string enddate)
        {
            string str_seach = "";
            str_seach = @"select flowid,buildcode,serviceno,CustTime,FinishedTime,equipmentid,equipmentname 
                               from cst_issues where equipmentid >0 ";

            if (begindate != "" && enddate != "")
            {
                begindate = Convert.ToDateTime(begindate).Date.ToString();
                str_seach = str_seach + string.Format(" and custtime between to_date('{0}','yyyy-MM-dd HH24:mi:ss') and to_date('{1}','yyyy-MM-dd HH24:mi:ss')", begindate, enddate);
            }

            OracleConnection cn = ConfigTool.GetConnection();
            DataTable dt_iss = new DataTable();
            try
            {

                dt_iss = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, str_seach);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                ConfigTool.CloseConnection(cn);
            }

            return dt_iss;
        }

        /// <summary>
        /// 获取资产信息
        /// </summary>
        /// <returns></returns>
        public DataTable Get_Desk()
        {
            string seach_Desk = @"select * from Equ_Desk where deleted = 0";
            OracleConnection cn = ConfigTool.GetConnection();
            DataTable dt_Desk = new DataTable();
            try
            {

                dt_Desk = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, seach_Desk);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                ConfigTool.CloseConnection(cn);
            }

            return dt_Desk;
        }

        #region 我已办事项
        /// <summary>
        /// 我已办事项
        /// </summary>
        /// <param name="UserId">用户ID</param>
        /// <param name="appid">应用ID</param>
        /// <param name="type">类型ID</param>
        /// <returns></returns>
        public static DataTable getHastodoitem_FlowWorkNownum(long UserId, long appid, int type)
        {
            string strSQL = string.Empty;        //存放SQL字符串

            if (appid == 1026)//事件
            {
                if (type == 1)
                {
                    strSQL = @"SELECT (E.buildcode || E.Serviceno) as NumberNo,E.ServiceKind,E.ServiceType,E.regusername,D.subject,A.FlowID,A.FActors,E.ServiceLevel," +
                              " A.MessageID,A.ReceiveTime,A.senderusername,A.sendernodename," +
                              " A.IsRead,A.Important,D.Attachment,c.appid,c.AppName," +
                              " datediff('Minute',sysdate,nvl(a.expected,sysdate)) as DiffMinute,d.Name as FlowName,A.actortype ,E.FinishedTime,E.Content" +
                              " FROM cst_issues E,Es_Flow D, V_Es_Message A,Es_App c" +
                              " WHERE E.FlowID=A.FlowID AND D.FlowID=A.FlowID AND E.FlowID=A.FlowID AND D.Appid=c.AppID " +
                              " AND A.status=" + ((int)e_MessageStatus.emsFinished).ToString() + " AND A.Deleted =" + (int)e_Deleted.eNormal +
                              " AND D.AppId = 1026 AND A.ReceiverId=" + UserId +
                              " order by E.FlowId desc ";
                }
                else
                {
                    strSQL = @"SELECT (nvl(f.buildCode,'') || nvl(f.ServiceNo,'')) NumberNo,f.*,d.ID,a.MessageID,a.flowid,b.Flowmodelid,e.nodemodelid,b.appid," +
                              " a.IsRead,a.Important,a.FActors,a.ReceiveTime,a.actortype,b.Subject as zhaiyao,b.Attachment,b.Name as FlowName,c.AppID,c.AppName as AppName," +
                              " datediff('Minute',sysdate,nvl(a.expected,sysdate)) as DiffMinute,b.FlowID,b.status,b.endtime,f.CustName,f.ServiceKind,a.senderusername,a.sendernodename  " +
                              " FROM V_ES_MESSAGE_NotReceiver a,Es_Flow b,Es_App c,Es_ReceiveList d,Es_Node e,Cst_Issues f" +
                              " WHERE d.MessageID = a.MessageID AND a.FlowID = b.FlowID AND b.AppID = c.AppID And a.nodeid=e.nodeid And b.Flowid=f.flowid " +
                              " AND d.ReceiveID = " + UserId.ToString() + " AND a.Deleted =" + (int)e_Deleted.eNormal + " AND a.ReceiverID = 0 " +
                              " ORDER BY a.MessageID DESC ";
                }
            }
            else if (appid == 420)//变更
            {
                if (type == 1)
                {
                    strSQL = @"SELECT E.changeno NumberNo,D.subject,A.FlowID,A.FActors,E.LevelName,
                        A.MessageID,A.ReceiveTime,A.senderusername,A.sendernodename,
                        A.IsRead,A.Important,D.Attachment,c.appid,c.AppName,
                        datediff('Minute',sysdate,nvl(a.expected,sysdate)) as DiffMinute,d.Name as FlowName,A.actortype
                        FROM equ_changeservice E,Es_Flow D, V_Es_Message A,Es_App c
   	                    WHERE E.FlowID=A.FlowID AND D.FlowID=A.FlowID AND E.FlowID=A.FlowID AND D.Appid=c.AppID "
                        + " AND A.status=" + ((int)e_MessageStatus.emsFinished).ToString()
                        + " AND A.Deleted =" + (int)e_Deleted.eNormal
                        + " AND D.AppId = 420 "
                        + " AND A.ReceiverId=" + UserId
                        + " order by E.FlowId desc  ";
                }
                else
                {
                    strSQL = @"SELECT f.changeno NumberNo,d.ID,a.MessageID,a.flowid,b.Flowmodelid,e.nodemodelid,b.appid,f.LevelName," +
                              " a.IsRead,a.Important,a.FActors,a.ReceiveTime,a.actortype,b.Subject,b.Attachment,b.Name as FlowName,c.AppID,c.AppName as AppName," +
                              " datediff('Minute',sysdate,nvl(a.expected,sysdate)) as DiffMinute,b.FlowID,b.status,b.endtime,f.CustName,a.senderusername,a.sendernodename  " +
                              " FROM V_ES_MESSAGE_NotReceiver a,Es_Flow b,Es_App c,Es_ReceiveList d,Es_Node e,equ_changeservice f" +
                              " WHERE d.MessageID = a.MessageID AND a.FlowID = b.FlowID AND b.AppID = c.AppID And a.nodeid=e.nodeid And b.Flowid=f.flowid " +
                              " AND d.ReceiveID = " + UserId.ToString() + " AND a.Deleted =" + (int)e_Deleted.eNormal + " AND a.ReceiverID = 0 " +
                              " ORDER BY a.MessageID DESC ";
                }
            }
            else if (appid == 210)//问题
            {
                if (type == 1)
                {
                    strSQL = @"SELECT (nvl(E.buildCode,'') || nvl(E.ProblemNo,'')) NumberNo,E.Problem_LevelName,E.*,A.FlowID,A.FActors,D.subject,
                        A.MessageID,A.ReceiveTime,A.senderusername,A.sendernodename,
                        A.IsRead,A.Important,D.Attachment,c.appid,c.AppName,
                        datediff('Minute',sysdate,nvl(a.expected,sysdate)) as DiffMinute,d.Name as FlowName,A.actortype
                        FROM Pro_ProblemDeal E,Es_Flow D, V_Es_Message A,Es_App c
   	                    WHERE E.FlowID=A.FlowID AND D.FlowID=A.FlowID AND E.FlowID=A.FlowID AND D.Appid=c.AppID "
                        + " AND A.status=" + ((int)e_MessageStatus.emsFinished).ToString()
                        + " AND A.Deleted =" + (int)e_Deleted.eNormal
                        + " AND D.AppId = 210 "
                        + " AND A.ReceiverId=" + UserId
                        + " order by E.FlowId desc ";
                }
                else
                {
                    strSQL = @"SELECT (nvl(f.buildCode,'') || nvl(f.ProblemNo,'')) NumberNo,f.*,d.ID,a.MessageID,a.flowid,b.Flowmodelid,e.nodemodelid,b.appid," +
                              " a.IsRead,a.Important,a.FActors,a.ReceiveTime,a.actortype,b.Subject,b.Attachment,b.Name as FlowName,c.AppID,c.AppName as AppName," +
                              " datediff('Minute',sysdate,nvl(a.expected,sysdate)) as DiffMinute,b.FlowID,b.status,b.endtime,a.senderusername,a.sendernodename  " +
                              " FROM V_ES_MESSAGE_NotReceiver a,Es_Flow b,Es_App c,Es_ReceiveList d,Es_Node e,Pro_ProblemDeal f" +
                              " WHERE d.MessageID = a.MessageID AND a.FlowID = b.FlowID AND b.AppID = c.AppID And a.nodeid=e.nodeid And b.Flowid=f.flowid " +
                              " AND d.ReceiveID = " + UserId.ToString() + " AND a.Deleted =" + (int)e_Deleted.eNormal + " AND a.ReceiverID = 0 " +
                              " ORDER BY a.MessageID DESC ";
                }

            }
            else if (appid == 400)//知识
            {

                //strSQL =string.Format("select  * from (select E.flowid ,E.Title ,E.PKey ,E.TypeName ,E.RegTime ,A.IsRead"+
                //" FROM Inf_KMBase E,Es_Flow D, V_Es_Message A,Es_App c WHERE E.FlowID=A.FlowID AND D.FlowID=A.FlowID AND E.FlowID=A.FlowID AND "+
                //" D.Appid=c.AppID  AND A.status = " + ((int)e_MessageStatus.emsFinished).ToString()+" AND A.Deleted ="+(int)e_Deleted.eNormal+" AND D.AppId = 400 AND A.ReceiverId="+UserId+""+
                //" group by E.flowid ,E.Title ,E.PKey ,E.TypeName ,E.RegTime ,A.IsRead"+
                //" order by E.FlowId desc) where rownum <=50");

                strSQL = string.Format("SELECT E.flowid ,E.Title ,E.PKey ,E.TypeName ,E.RegTime,D.subject,A.FlowID,A.FActors, A.MessageID,A.ReceiveTime,A.senderusername,A.sendernodename,"
                + " A.IsRead,A.Important,D.Attachment,c.appid,c.AppName, datediff('Minute',sysdate,nvl(a.expected,sysdate))"
                + " as DiffMinute,d.Name as FlowName,A.actortype"
                + " FROM Inf_KMBase  E,Es_Flow D, V_Es_Message A,Es_App c WHERE E.FlowID=A.FlowID AND D.FlowID=A.FlowID AND E.FlowID=A.FlowID "
                + " AND D.Appid=c.AppID  AND A.status=" + ((int)e_MessageStatus.emsFinished).ToString() + " AND A.Deleted =" + (int)e_Deleted.eNormal + " AND D.AppId = 400 AND A.ReceiverId=" + UserId + ""
                + "  order by E.FlowId desc");


            }
            else if (appid == 1028)//发布
            {
                if (type == 1)
                {
                    strSQL = @"SELECT E.*,A.FlowID,A.FActors,d.Subject,
                        A.MessageID,A.ReceiveTime,A.senderusername,A.sendernodename,
                        A.IsRead,A.Important,D.Attachment,c.appid,c.AppName,
                        datediff('Minute',sysdate,nvl(a.expected,sysdate)) as DiffMinute,d.Name as FlowName,A.actortype
                        FROM OA_RELEASEMANAGEMENT E,Es_Flow D, V_Es_Message A,Es_App c
   	                    WHERE E.FlowID=A.FlowID AND D.FlowID=A.FlowID AND E.FlowID=A.FlowID AND D.Appid=c.AppID "
                        + " AND A.status=" + ((int)e_MessageStatus.emsFinished).ToString()
                        + " AND A.Deleted =" + (int)e_Deleted.eNormal
                        + " AND D.AppId = 1028 "
                        + " AND A.ReceiverId=" + UserId
                        + " order by E.FlowId desc ";
                }
                else
                {
                    strSQL = @"select f.*,d.ID,a.MessageID,a.flowid,b.Flowmodelid,e.nodemodelid,b.appid," +
                              " a.IsRead,a.Important,a.FActors,a.ReceiveTime,a.actortype,b.Subject,b.Attachment,b.Name as FlowName,c.AppID,c.AppName as AppName," +
                              " datediff('Minute',sysdate,nvl(a.expected,sysdate)) as DiffMinute,b.FlowID,b.status,b.endtime ,a.senderusername,a.sendernodename " +
                              " FROM V_ES_MESSAGE_NotReceiver a,Es_Flow b,Es_App c,Es_ReceiveList d,Es_Node e,OA_RELEASEMANAGEMENT f" +
                              " WHERE d.MessageID = a.MessageID AND a.FlowID = b.FlowID AND b.AppID = c.AppID And a.nodeid=e.nodeid And b.Flowid=f.flowid " +
                              " AND d.ReceiveID = " + UserId.ToString() + " AND a.Deleted =" + (int)e_Deleted.eNormal + " AND a.ReceiverID = 0 " +
                              " ORDER BY a.MessageID DESC  ";
                }
            }
            else//其它
            {
                if (type == 1)
                {
                    strSQL = @"SELECT A.FlowID,D.Name,A.FActors,D.Subject,
                        A.MessageID,A.ReceiveTime,A.senderusername,A.sendernodename,
                        A.IsRead,A.Important,D.Attachment,c.appid,c.AppName,
                        datediff('Minute',sysdate,nvl(a.expected,sysdate)) as DiffMinute,d.Name as FlowName,A.actortype
                        FROM Es_Flow D, V_Es_Message A,Es_App c
   	                    WHERE D.FlowID=A.FlowID AND D.Appid=c.AppID "
                        + " AND A.status=" + ((int)e_MessageStatus.emsFinished).ToString()
                        + " AND A.Deleted =" + (int)e_Deleted.eNormal
                        + " AND D.AppId <> 1028 AND D.AppID <> 1026 AND  D.AppID <> 420 AND D.AppID <> 201 AND D.AppID <> 210 and D.AppID <> 400"
                        + " AND A.ReceiverId=" + UserId
                        + " order by a.FlowId desc ";
                }
                else
                {
                    strSQL = @" SELECT d.ID,a.MessageID,a.flowid,b.Flowmodelid,e.nodemodelid,b.appid,b.Name," +
                              " a.IsRead,a.Important,a.FActors,a.ReceiveTime,a.actortype,b.Subject,b.Attachment,b.Name as FlowName,c.AppID,c.AppName as AppName," +
                              " datediff('Minute',sysdate,nvl(a.expected,sysdate)) as DiffMinute,b.FlowID,b.status,b.endtime,a.senderusername,a.sendernodename  " +
                              " FROM V_ES_MESSAGE_NotReceiver a,Es_Flow b,Es_App c,Es_ReceiveList d,Es_Node e" +
                              " WHERE d.MessageID = a.MessageID AND a.FlowID = b.FlowID AND b.AppID = c.AppID And a.nodeid=e.nodeid " +
                              " AND d.ReceiveID = " + UserId.ToString() + " AND a.Deleted =" + (int)e_Deleted.eNormal + " AND a.ReceiverID = 0 AND b.AppId <> 1028 AND b.AppID <> 1026 AND  b.AppID <> 420 AND b.AppID <> 201 AND b.AppID <> 210 and D.AppID <> 400" +
                              " ORDER BY a.MessageID DESC ";
                }
            }

            OracleConnection cn = ConfigTool.GetConnection();
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigTool.CloseConnection(cn);
            return dt;
        }

        /// <summary>
        /// 已办事项，按应用分
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="appid"></param>
        /// <param name="eac"></param>
        /// <param name="ems"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static DataTable getHastodoitem_FlowWorkNow(long UserId, long appid, int type)
        {

            string strSQL = string.Empty;        //存放SQL字符串

            if (appid == 1026)//事件
            {
                if (type == 1)
                {
                    strSQL = @"select * from (SELECT (E.buildcode || E.Serviceno) as NumberNo,E.ServiceKind,E.ServiceType,E.regusername,D.subject,A.FlowID,A.FActors,E.ServiceLevel," +
                              " A.MessageID,A.ReceiveTime,A.senderusername,A.sendernodename," +
                              " A.IsRead,A.Important,D.Attachment,c.appid,c.AppName," +
                              " datediff('Minute',sysdate,nvl(a.expected,sysdate)) as DiffMinute,d.Name as FlowName,A.actortype ,E.FinishedTime,E.Content" +
                              " FROM cst_issues E,Es_Flow D, V_Es_Message A,Es_App c" +
                              " WHERE E.FlowID=A.FlowID AND D.FlowID=A.FlowID AND E.FlowID=A.FlowID AND D.Appid=c.AppID " +
                              " AND A.status=" + ((int)e_MessageStatus.emsFinished).ToString() + " AND A.Deleted =" + (int)e_Deleted.eNormal +
                              " AND D.AppId = 1026 AND A.ReceiverId=" + UserId +
                              " order by E.FlowId desc) where  rownum <=50";
                }
                else
                {
                    strSQL = @"select * from (SELECT (nvl(f.buildCode,'') || nvl(f.ServiceNo,'')) NumberNo,f.*,d.ID,a.MessageID,a.flowid,b.Flowmodelid,e.nodemodelid,b.appid," +
                              " a.IsRead,a.Important,a.FActors,a.ReceiveTime,a.actortype,b.Subject as zhaiyao,b.Attachment,b.Name as FlowName,c.AppID,c.AppName as AppName," +
                              " datediff('Minute',sysdate,nvl(a.expected,sysdate)) as DiffMinute,b.FlowID,b.status,b.endtime,f.CustName,f.ServiceKind,a.senderusername,a.sendernodename  " +
                              " FROM V_ES_MESSAGE_NotReceiver a,Es_Flow b,Es_App c,Es_ReceiveList d,Es_Node e,Cst_Issues f" +
                              " WHERE d.MessageID = a.MessageID AND a.FlowID = b.FlowID AND b.AppID = c.AppID And a.nodeid=e.nodeid And b.Flowid=f.flowid " +
                              " AND d.ReceiveID = " + UserId.ToString() + " AND a.Deleted =" + (int)e_Deleted.eNormal + " AND a.ReceiverID = 0 " +
                              " ORDER BY a.MessageID DESC) where  rownum <=50";
                }
            }
            else if (appid == 420)//变更
            {
                if (type == 1)
                {
                    strSQL = @"select * from ( SELECT E.changeno NumberNo,D.subject,A.FlowID,A.FActors,E.LevelName,
                        A.MessageID,A.ReceiveTime,A.senderusername,A.sendernodename,
                        A.IsRead,A.Important,D.Attachment,c.appid,c.AppName,
                        datediff('Minute',sysdate,nvl(a.expected,sysdate)) as DiffMinute,d.Name as FlowName,A.actortype
                        FROM equ_changeservice E,Es_Flow D, V_Es_Message A,Es_App c
   	                    WHERE E.FlowID=A.FlowID AND D.FlowID=A.FlowID AND E.FlowID=A.FlowID AND D.Appid=c.AppID "
                        + " AND A.status=" + ((int)e_MessageStatus.emsFinished).ToString()
                        + " AND A.Deleted =" + (int)e_Deleted.eNormal
                        + " AND D.AppId = 420 "
                        + " AND A.ReceiverId=" + UserId
                        + " order by E.FlowId desc ) where  rownum <=50";
                }
                else
                {
                    strSQL = @"select * from ( SELECT f.changeno NumberNo,d.ID,a.MessageID,a.flowid,b.Flowmodelid,e.nodemodelid,b.appid,f.LevelName," +
                              " a.IsRead,a.Important,a.FActors,a.ReceiveTime,a.actortype,b.Subject,b.Attachment,b.Name as FlowName,c.AppID,c.AppName as AppName," +
                              " datediff('Minute',sysdate,nvl(a.expected,sysdate)) as DiffMinute,b.FlowID,b.status,b.endtime,f.CustName,a.senderusername,a.sendernodename  " +
                              " FROM V_ES_MESSAGE_NotReceiver a,Es_Flow b,Es_App c,Es_ReceiveList d,Es_Node e,equ_changeservice f" +
                              " WHERE d.MessageID = a.MessageID AND a.FlowID = b.FlowID AND b.AppID = c.AppID And a.nodeid=e.nodeid And b.Flowid=f.flowid " +
                              " AND d.ReceiveID = " + UserId.ToString() + " AND a.Deleted =" + (int)e_Deleted.eNormal + " AND a.ReceiverID = 0 " +
                              " ORDER BY a.MessageID DESC) where rownum <=50 ";
                }
            }
            else if (appid == 210)//问题
            {
                if (type == 1)
                {
                    strSQL = @"SELECT (nvl(E.buildCode,'') || nvl(E.ProblemNo,'')) NumberNo,E.Problem_LevelName,E.*,A.FlowID,A.FActors,D.subject,
                        A.MessageID,A.ReceiveTime,A.senderusername,A.sendernodename,
                        A.IsRead,A.Important,D.Attachment,c.appid,c.AppName,
                        datediff('Minute',sysdate,nvl(a.expected,sysdate)) as DiffMinute,d.Name as FlowName,A.actortype
                        FROM Pro_ProblemDeal E,Es_Flow D, V_Es_Message A,Es_App c
   	                    WHERE E.FlowID=A.FlowID AND D.FlowID=A.FlowID AND E.FlowID=A.FlowID AND D.Appid=c.AppID "
                        + " AND A.status=" + ((int)e_MessageStatus.emsFinished).ToString()
                        + " AND A.Deleted =" + (int)e_Deleted.eNormal
                        + " AND D.AppId = 210 "
                        + " AND A.ReceiverId=" + UserId
                        + " order by E.FlowId desc ";
                }
                else
                {
                    strSQL = @"SELECT (nvl(f.buildCode,'') || nvl(f.ProblemNo,'')) NumberNo,f.*,d.ID,a.MessageID,a.flowid,b.Flowmodelid,e.nodemodelid,b.appid," +
                              " a.IsRead,a.Important,a.FActors,a.ReceiveTime,a.actortype,b.Subject,b.Attachment,b.Name as FlowName,c.AppID,c.AppName as AppName," +
                              " datediff('Minute',sysdate,nvl(a.expected,sysdate)) as DiffMinute,b.FlowID,b.status,b.endtime,a.senderusername,a.sendernodename  " +
                              " FROM V_ES_MESSAGE_NotReceiver a,Es_Flow b,Es_App c,Es_ReceiveList d,Es_Node e,Pro_ProblemDeal f" +
                              " WHERE d.MessageID = a.MessageID AND a.FlowID = b.FlowID AND b.AppID = c.AppID And a.nodeid=e.nodeid And b.Flowid=f.flowid " +
                              " AND d.ReceiveID = " + UserId.ToString() + " AND a.Deleted =" + (int)e_Deleted.eNormal + " AND a.ReceiverID = 0 " +
                              " ORDER BY a.MessageID DESC ";
                }

            }
            else if (appid == 400)//知识
            {

                //strSQL =string.Format("select  * from (select E.flowid ,E.Title ,E.PKey ,E.TypeName ,E.RegTime ,A.IsRead"+
                //" FROM Inf_KMBase E,Es_Flow D, V_Es_Message A,Es_App c WHERE E.FlowID=A.FlowID AND D.FlowID=A.FlowID AND E.FlowID=A.FlowID AND "+
                //" D.Appid=c.AppID  AND A.status = " + ((int)e_MessageStatus.emsFinished).ToString()+" AND A.Deleted ="+(int)e_Deleted.eNormal+" AND D.AppId = 400 AND A.ReceiverId="+UserId+""+
                //" group by E.flowid ,E.Title ,E.PKey ,E.TypeName ,E.RegTime ,A.IsRead"+
                //" order by E.FlowId desc) where rownum <=50");

                strSQL = string.Format("SELECT E.flowid ,E.Title ,E.PKey ,E.TypeName ,E.RegTime,D.subject,A.FlowID,A.FActors, A.MessageID,A.ReceiveTime,A.senderusername,A.sendernodename,"
                + " A.IsRead,A.Important,D.Attachment,c.appid,c.AppName, datediff('Minute',sysdate,nvl(a.expected,sysdate))"
                + " as DiffMinute,d.Name as FlowName,A.actortype"
                + " FROM Inf_KMBase  E,Es_Flow D, V_Es_Message A,Es_App c WHERE E.FlowID=A.FlowID AND D.FlowID=A.FlowID AND E.FlowID=A.FlowID "
                + " AND D.Appid=c.AppID  AND A.status=" + ((int)e_MessageStatus.emsFinished).ToString() + " AND A.Deleted =" + (int)e_Deleted.eNormal + " AND D.AppId = 400 AND A.ReceiverId=" + UserId + ""
                + " order by E.FlowId desc");


            }
            else if (appid == 1028)//发布
            {
                if (type == 1)
                {
                    strSQL = @"SELECT E.*,A.FlowID,A.FActors,d.Subject,
                        A.MessageID,A.ReceiveTime,A.senderusername,A.sendernodename,
                        A.IsRead,A.Important,D.Attachment,c.appid,c.AppName,
                        datediff('Minute',sysdate,nvl(a.expected,sysdate)) as DiffMinute,d.Name as FlowName,A.actortype
                        FROM OA_RELEASEMANAGEMENT E,Es_Flow D, V_Es_Message A,Es_App c
   	                    WHERE E.FlowID=A.FlowID AND D.FlowID=A.FlowID AND E.FlowID=A.FlowID AND D.Appid=c.AppID "
                        + " AND A.status=" + ((int)e_MessageStatus.emsFinished).ToString()
                        + " AND A.Deleted =" + (int)e_Deleted.eNormal
                        + " AND D.AppId = 1028 "
                        + " AND A.ReceiverId=" + UserId
                        + " and rownum <=50"
                        + " order by E.FlowId desc ";
                }
                else
                {
                    strSQL = @"select f.*,d.ID,a.MessageID,a.flowid,b.Flowmodelid,e.nodemodelid,b.appid," +
                              " a.IsRead,a.Important,a.FActors,a.ReceiveTime,a.actortype,b.Subject,b.Attachment,b.Name as FlowName,c.AppID,c.AppName as AppName," +
                              " datediff('Minute',sysdate,nvl(a.expected,sysdate)) as DiffMinute,b.FlowID,b.status,b.endtime ,a.senderusername,a.sendernodename " +
                              " FROM V_ES_MESSAGE_NotReceiver a,Es_Flow b,Es_App c,Es_ReceiveList d,Es_Node e,OA_RELEASEMANAGEMENT f" +
                              " WHERE d.MessageID = a.MessageID AND a.FlowID = b.FlowID AND b.AppID = c.AppID And a.nodeid=e.nodeid And b.Flowid=f.flowid " +
                              " AND d.ReceiveID = " + UserId.ToString() + " AND a.Deleted =" + (int)e_Deleted.eNormal + " AND a.ReceiverID = 0 " +
                              " and rownum <=50" +
                              " ORDER BY a.MessageID DESC  ";
                }
            }
            else//其它
            {
                if (type == 1)
                {
                    strSQL = @"select * from ( SELECT A.FlowID,D.Name,A.FActors,D.Subject,
                        A.MessageID,A.ReceiveTime,A.senderusername,A.sendernodename,
                        A.IsRead,A.Important,D.Attachment,c.appid,c.AppName,
                        datediff('Minute',sysdate,nvl(a.expected,sysdate)) as DiffMinute,d.Name as FlowName,A.actortype
                        FROM Es_Flow D, V_Es_Message A,Es_App c
   	                    WHERE D.FlowID=A.FlowID AND D.Appid=c.AppID "
                        + " AND A.status=" + ((int)e_MessageStatus.emsFinished).ToString()
                        + " AND A.Deleted =" + (int)e_Deleted.eNormal
                        + " AND D.AppId <> 1028 AND D.AppID <> 1026 AND  D.AppID <> 420 AND D.AppID <> 201 AND D.AppID <> 210 and D.AppID <> 400"
                        + " AND A.ReceiverId=" + UserId
                        + " order by a.FlowId desc ) where  rownum <=50 ";
                }
                else
                {
                    strSQL = @"select * from (  SELECT d.ID,a.MessageID,a.flowid,b.Flowmodelid,e.nodemodelid,b.appid,b.Name," +
                              " a.IsRead,a.Important,a.FActors,a.ReceiveTime,a.actortype,b.Subject,b.Attachment,b.Name as FlowName,c.AppID,c.AppName as AppName," +
                              " datediff('Minute',sysdate,nvl(a.expected,sysdate)) as DiffMinute,b.FlowID,b.status,b.endtime,a.senderusername,a.sendernodename  " +
                              " FROM V_ES_MESSAGE_NotReceiver a,Es_Flow b,Es_App c,Es_ReceiveList d,Es_Node e" +
                              " WHERE d.MessageID = a.MessageID AND a.FlowID = b.FlowID AND b.AppID = c.AppID And a.nodeid=e.nodeid " +
                              " AND d.ReceiveID = " + UserId.ToString() + " AND a.Deleted =" + (int)e_Deleted.eNormal + " AND a.ReceiverID = 0 AND b.AppId <> 1028 AND b.AppID <> 1026 AND  b.AppID <> 420 AND b.AppID <> 201 AND b.AppID <> 210 and D.AppID <> 400" +
                              " ORDER BY a.MessageID DESC  ) where  rownum <=50";
                }
            }

            OracleConnection cn = ConfigTool.GetConnection();
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigTool.CloseConnection(cn);
            return dt;
        }
        #endregion


        #region 查询所有非服务请求并且具有启动权限的事件流程 余向前 2013-04-02
        /// <summary>
        /// 查询所有非服务请求并且具有启动权限的事件流程
        /// </summary>
        /// <param name="lngUserID">用户ID</param>
        /// <returns></returns>
        public static DataTable GetIssuesFlowModelManage(long lngUserID)
        {
            string strSQL = " select a.oflowmodelid,a.flowmodelid,a.flowname from es_flowmodel a inner join" +
                            "(select Es_Nodemodel.Flowmodelid,es_appop.Opid from es_nodemodel left join es_appop" +
                            " on es_nodemodel.opid=es_appop.opid where nodemodelid=2 and flowmodelid in" +
                            "(select flowmodelid from es_flowmodel where deleted=0 and status=1 and appid=1026))" +
                            " b on a.flowmodelid=b.flowmodelid  where b.Opid=  9351";
            DataTable dt = CommonDP.ExcuteSqlTable(strSQL);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow row = dt.Rows[i];
                int intCanStart = CanUseFlowModel(long.Parse(row["flowmodelid"].ToString()), lngUserID);
                if (intCanStart != 0)
                {
                    row.BeginEdit();
                    dt.Rows[i].Delete();
                    row.EndEdit();
                    dt.AcceptChanges();
                }

            }
            return dt;
        }

        /// <summary>
        /// 查询所有非服务请求并且具有启动权限的事件流程
        /// </summary>
        /// <param name="lngUserID">用户ID</param>
        /// <param name="AppID">应用ID</param>
        /// <param name="OpID">相关视图ID</param>
        /// <returns></returns>
        public static DataTable GetIssuesFlowModelManage(long lngUserID, long AppID, long OpID)
        {
            string strSQL = " select a.oflowmodelid,a.flowmodelid,a.flowname from es_flowmodel a inner join" +
                            "(select Es_Nodemodel.Flowmodelid,es_appop.Opid from es_nodemodel left join es_appop" +
                            " on es_nodemodel.opid=es_appop.opid where nodemodelid=2 and flowmodelid in" +
                            "(select flowmodelid from es_flowmodel where deleted=0 and status=1 and appid=" + AppID + "))" +
                            " b on a.flowmodelid=b.flowmodelid  where b.Opid= " + OpID;
            DataTable dt = CommonDP.ExcuteSqlTable(strSQL);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow row = dt.Rows[i];
                int intCanStart = CanUseFlowModel(long.Parse(row["flowmodelid"].ToString()), lngUserID);
                if (intCanStart != 0)
                {
                    row.BeginEdit();
                    dt.Rows[i].Delete();
                    row.EndEdit();
                    dt.AcceptChanges();
                }

            }
            return dt;
        }

        /// <summary>
        /// 判断用户是否可以启动对应的流程模型 两种情况不能启动流程模型： 1、流程模型不是启动状态,返回 -1 2、用户不是流程模型的启动人员 返回 -2
        /// </summary>
        /// <param name="lngFlowModelID">流程模型编号</param>
        /// <returns>启动状态</returns>
        public static int CanUseFlowModel(long lngFlowModelID, long lngUserID)
        {
            long lngOFlowModelID = 0;
            long lngNewFlowModelID = 0;



            lngOFlowModelID = FlowDP.GetOFlowModelID(lngFlowModelID);//原FlowModelID
            lngNewFlowModelID = FlowModel.GetLastVersionFlowModelID(lngOFlowModelID);//获取最新FlowModelID;

            int intCanStart = FlowModel.CanUseFlowModel(lngUserID, lngNewFlowModelID);

            return intCanStart;
        }

        #endregion





        #region 取事件单号 - 2013-11-28 @孙绍棕

        /// <summary>
        /// 取事件单号
        /// </summary>
        /// <param name="lngFlowID">流程编号</param>
        /// <returns></returns>
        public static String GetIssueNO(long lngFlowID)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            try
            {
                String strSQL = String.Format(@"select buildcode || serviceno as issueno from cst_issues where flowid ={0}", lngFlowID);
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);

                return dt.Rows[0]["issueno"].ToString().Trim();
            }
            finally
            {
                ConfigTool.CloseConnection(cn);
            }

        }

        #endregion
    }
}
