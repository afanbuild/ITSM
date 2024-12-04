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

namespace Epower.ITSM.SqlDAL
{
    /// <summary>
    /// FareDP 的摘要说明。

    /// </summary>
    public class ReportDP
    {
        public ReportDP()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

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
        /// 根据事件类别编号，查询事件名
        /// </summary>
        /// <param name="serverstypeid"></param>
        /// <returns></returns>
        public static DataTable Getiss(long serverstypeid)
        {
            OracleConnection con = ConfigTool.GetConnection();
            DataTable dt = new DataTable();
            string strsql = string.Empty;
            try
            {
                strsql = @"select distinct c.servicetype from cst_issues c where c.servicetypeid = '" + serverstypeid + "'";
                dt = OracleDbHelper.ExecuteDataTable(con, CommandType.Text, strsql);
                return dt;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                ConfigTool.CloseConnection(con);
            }
        }


        /// <summary>
        /// 获取各个事件的客户满意度
        /// </summary>
        /// <param name="nYear"></param>
        /// <param name="lngServiceTypeID"></param>
        /// <param name="lngWTTypeID"></param>
        /// <param name="lngDeptID"></param>
        /// <param name="lngMastCustomer"></param>
        /// <returns></returns>
        public static DataTable GetAnalysisStatisfied2(int nYear, long lngServiceTypeID, long lngWTTypeID, long lngDeptID, long lngMastCustomer)
        {
            #region 之前的

            //long lngCatalogID = lngServiceTypeID;
            //string sFullID = string.Empty;
            //if (lngServiceTypeID == -1 || lngServiceTypeID == 0)
            //    lngCatalogID = 1001;

            //OracleConnection cn = ConfigTool.GetConnection("SQLConnString");
            //OracleParameter[] para = {
            //     new OracleParameter("nYear", OracleType.VarChar , 50 ),
            //     new OracleParameter("lngServiceTypeID", OracleType.VarChar , 50 ),
            //     new OracleParameter("lngWTTypeID", OracleType.VarChar , 50 ),
            //     new OracleParameter("lngDeptID", OracleType.VarChar , 50 ),
            //     new OracleParameter("lngMastCustomer", OracleType.VarChar , 50 ),
            //     new OracleParameter("p_cursor",OracleType.Cursor)
            //};
            //para[0].Direction = ParameterDirection.Input;
            //para[1].Direction = ParameterDirection.Input;
            //para[2].Direction = ParameterDirection.Input;
            //para[3].Direction = ParameterDirection.Input;
            //para[4].Direction = ParameterDirection.Input;
            //para[5].Direction = ParameterDirection.Output;

            //para[0].Value = nYear.ToString();
            //para[1].Value = lngCatalogID.ToString();
            //para[2].Value = lngWTTypeID.ToString();
            //para[3].Value = lngDeptID.ToString();
            //para[4].Value = lngMastCustomer.ToString();
            //try
            //{
            //    DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.StoredProcedure, "PROC_Satisfaction2", para);
            //    return dt;
            //}
            //catch (Exception e)
            //{
            //    throw e;
            //}
            //finally
            //{
            //    ConfigTool.CloseConnection(cn);
            //}
            #endregion

            #region 后面修改的

            OracleConnection cn = ConfigTool.GetConnection();
            string sSql = string.Empty;
            if (lngServiceTypeID == 0 && lngMastCustomer == 0)
            {
                sSql = @"
                select b.months,sum(b.fqty) huisum,sum(b.summanyi) mancis,b.ser,b.sername sername from 
                (select datepart('month',ciss.regsysdate) as months,
                ciss.ServiceTypeID as ser,ciss.servicetype as sername,
                (select count(flowid) from EA_Issues_FeedBack where flowid = ciss.flowid) as fqty,
                (select sum(case when feedback=3 then 0 else 1 end) from EA_Issues_FeedBack where flowid = ciss.flowid) as summanyi
                from cst_issues ciss,Es_Catalog c WHERE ciss.servicetypeid=c.CatalogID and
                datepart('year',ciss.regsysdate)  = " + nYear;

                sSql += ")b group by b.months,b.ser,b.sername order by months";
            }
            else if (lngServiceTypeID != 0 && lngServiceTypeID != 1001 && lngMastCustomer != 0)
            {
                sSql = string.Format("select b.months,sum(b.fqty) huisum,sum(b.summanyi) mancis,b.ser,b.sername sername from " +
               "(select datepart('month',ciss.regsysdate) as months, " +
               "ciss.ServiceTypeID as ser,ciss.servicetype as sername, " +
               "(select count(flowid) from EA_Issues_FeedBack where flowid = ciss.flowid) as fqty," +
               "(select sum(case when feedback=3 then 0 else 1 end) from EA_Issues_FeedBack where flowid = ciss.flowid) as summanyi " +
               " from cst_issues ciss,Es_Catalog c WHERE ciss.servicetypeid=c.CatalogID and " +
               " datepart('year',ciss.regsysdate)  = '" + nYear + "'" +
               " and c.catalogid='" + lngServiceTypeID + "'" +
               " and nvl(CustID,0) In (select ID from Br_ECustomer where MastCustID='" + lngMastCustomer + "' ))b group by b.months,b.ser,b.sername order by months");
            }
            else if (lngServiceTypeID != 0 && lngServiceTypeID != 1001 && lngMastCustomer == 0)
            {
                sSql = @"
                select b.months,sum(b.fqty) huisum,sum(b.summanyi) mancis,b.ser,b.sername sername from 
                (select datepart('month',ciss.regsysdate) as months,
                ciss.ServiceTypeID as ser,ciss.servicetype as sername,
                (select count(flowid) from EA_Issues_FeedBack where flowid = ciss.flowid) as fqty,
                (select sum(case when feedback=3 then 0 else 1 end) from EA_Issues_FeedBack where flowid = ciss.flowid) as summanyi
                from cst_issues ciss,Es_Catalog c WHERE ciss.servicetypeid=c.CatalogID and
                datepart('year',ciss.regsysdate)  = " + nYear;

                sSql += " and  c.catalogid='" + lngServiceTypeID + "' )b group by b.months,b.ser,b.sername order by months";
            }
            else if ((lngServiceTypeID == 0 || lngServiceTypeID == 1001) && lngMastCustomer != 0)
            {
                sSql = @"
                select b.months,sum(b.fqty) huisum,sum(b.summanyi) mancis,b.ser,b.sername sername from 
                (select datepart('month',ciss.regsysdate) as months,
                ciss.ServiceTypeID as ser,ciss.servicetype as sername,
                (select count(flowid) from EA_Issues_FeedBack where flowid = ciss.flowid) as fqty,
                (select sum(case when feedback=3 then 0 else 1 end) from EA_Issues_FeedBack where flowid = ciss.flowid) as summanyi
                from cst_issues ciss,Es_Catalog c WHERE ciss.servicetypeid=c.CatalogID and
                datepart('year',ciss.regsysdate)  = " + nYear;

                sSql += "  and nvl(CustID,0) In (select ID from Br_ECustomer where MastCustID='" + lngMastCustomer + "') )b group by b.months,b.ser,b.sername order by months";
            }
            try
            {
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, sSql);

                if (dt != null && dt.Rows.Count > 0)
                {
                    string sqldele = @"delete from br_man ";
                    int numdel = OracleDbHelper.ExecuteNonQuery(cn, CommandType.Text, sqldele);



                    foreach (DataRow dr in dt.Rows)
                    {
                        string sqlinsert = string.Format("insert into br_man values('" + dr["months"] + "','" + dr["huisum"] + "','" + dr["mancis"] + "','" + dr["ser"] + "','" + dr["sername"] + "')");
                        int numinsert = OracleDbHelper.ExecuteNonQuery(cn, CommandType.Text, sqlinsert);

                    }
                }


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
            #endregion
        }

        /// <summary>
        /// 查询每个月总的回访次数
        /// </summary>
        /// <returns></returns>
        public static DataTable GetMan()
        {
            OracleConnection cn = ConfigTool.GetConnection();
            string sSql = string.Empty;
            sSql = @"select m.months,sum(m.huisum) huisum from br_man m group by m.months order by months";
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
        /// 获取事件类别
        /// </summary>
        /// <returns></returns>
        public static DataTable GetSerName()
        {
            OracleConnection cn = ConfigTool.GetConnection();
            string sql = @"select distinct m.ser,m.sername from br_man m order by m.ser,m.sername";
            try
            {
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, sql);
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
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.StoredProcedure, "PROC_Satisfaction", para);
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

                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.StoredProcedure, "PROC_CustomerSatisfaction", para);
                //int num = dt.Columns.Count;
                //foreach (DataRow dr in dt.Rows)
                //{
                //    if (dr[num - 1].ToString() == "100")
                //        dr[num - 1] = "1";
                //}
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
        public static DataTable GetAnalysisCSMTypeDirectionGrid(int nYear)
        {
            string sSql = @"SELECT datepart('month',regsysdate) as months,count(smsid) as qty" +
                " FROM cst_issues " +
                " WHERE   datepart('year',regsysdate) = " + nYear.ToString();
            sSql += " GROUP BY datepart('month',regsysdate) ";

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
            //            sSql = @" declare @sFullID nvarchar(200)
            //                      select @sFullID=FullID from Es_Catalog where CatalogID=" + lngCatalogID.ToString();

            //            sSql += @" SELECT	d.servicetype as 事件类别,
            //								'1月'=sum(case  when d.months=1 then d.qty else 0 end),
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
            //						SELECT month(a.regsysdate) as months,c.CatalogName  + '(' + convert(varchar,c.CataID) + ')' as servicetype ,count(a.smsid) as qty 
            //						FROM cst_issues a,(select substring(a.FullID,0,len(@sFullID)+1) 
            //                            FullID,a.CatalogID,b.CatalogID as CataID,b.CatalogName
            //                            from Es_Catalog a left outer join Es_Catalog b On substring(a.FullID,0,len(@sFullID)+7)=b.FullID) c
            //                            where a.ServiceTypeID=c.CatalogID and c.FullID=@sFullID and a.ServiceTypeID<>1001";

            //            sSql += " and year(a.regsysdate) = " + nYear.ToString() +
            //                           ((lngDeptID == 0 || lngDeptID == -1) ? "" : " AND a.orgid =" + lngDeptID + "");
            //            if (lngMastCustomer != 0)
            //                sSql += " And nvl(CustID,0) In (select ID from Br_ECustomer where MastCustID=" + lngMastCustomer.ToString() + ")";
            //            sSql += @" GROUP BY month(a.regsysdate),c.CatalogName  + '(' + convert(varchar,c.CataID) + ')' ) d
            //                        	GROUP BY d.servicetype";

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

                dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.StoredProcedure, "PROC_TrendAnalysis", parms);

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

                dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.StoredProcedure, "PROC_AnnualDataAnalysis", parms);

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
        /// 重大事件故障次数
        /// </summary>
        /// <returns></returns>
        public static DataTable GetAnalysisImportant(string type, string year, string beginTime, string endTime, string equipmentName)
        {
            string strSQL = "";
            DataTable dt = null;
            switch (type)
            {
                case "year":
                    strSQL = @"SELECT datepart('year',a.realitytime) as 年份,count(distinct(a.flowid)) as 故障次数 FROM cst_issues a
                                        inner join es_message m on a.flowid=m.flowid  where a.servicetype='重大事件' and not(m.senderid=0 and m.status=20)";
                    if (equipmentName != "")
                    {
                        strSQL += " and a.equipmentname='" + equipmentName + "'";
                    }
                    strSQL += " GROUP BY datepart('year',a.realitytime) order by 年份 asc";
                    break;
                case "month":
                    strSQL = "SELECT datepart('month',c.realitytime) as 月份,count(distinct(c.flowid)) as 故障次数 FROM cst_issues c inner join es_message m on c.flowid=m.flowid where datepart('year',c.realitytime) =" + year + "  and c.servicetype='重大事件' and not(m.senderid=0 and m.status=20)";
                    if (equipmentName != "")
                    {
                        strSQL += " and c.equipmentname='" + equipmentName + "'";
                    }
                    strSQL += " GROUP BY datepart('month',c.realitytime) order by 月份 asc";
                    break;
                case "day":
                    strSQL = "select to_char(a.realitytime,'yyyy-MM-dd') as 日期,count(distinct(a.flowid)) as 故障次数 from cst_issues a inner join es_message m on a.flowid=m.flowid  where a.servicetype='重大事件' and not(m.senderid=0 and m.status=20) and a.realitytime between to_date('" + beginTime + "','yyyy-mm-dd HH24:mi:ss') and to_date('" + endTime + "','yyyy-mm-dd HH24:mi:ss')";
                    if (equipmentName != "")
                    {
                        strSQL += " and a.equipmentname='" + equipmentName + "'";
                    }
                    strSQL += " GROUP BY to_char(a.realitytime,'yyyy-MM-dd') order by 日期 asc";
                    break;
            }
            OracleConnection cn = ConfigTool.GetConnection();
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
        ///重大事件故障次数（按资产计算）

        /// </summary>
        /// <param name="equipmentname"></param>
        /// <returns></returns>
        public static DataTable GetAnalysisImportantDirection(string type, int nYear, string beginTime, string endTime, string equipmentname)
        {
            DataTable dt = null;
            OracleConnection cn = ConfigTool.GetConnection();
            if (cn.State != ConnectionState.Open)
            {
                cn.Open();
            }
            try
            {
                switch (type)
                {
                    case "year":
                        OracleParameter[] parms1 = {
                                                  new OracleParameter("equipmentname",OracleType.VarChar,200),
                                                  new OracleParameter("p_cursor",OracleType.Cursor)};
                        parms1[0].Direction = ParameterDirection.Input;
                        parms1[1].Direction = ParameterDirection.Output;

                        parms1[0].Value = equipmentname;
                        dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.StoredProcedure, "PROC_ImportantYearDirection", parms1);
                        break;
                    case "month":
                        OracleParameter[] parms2 ={
                                                new OracleParameter("nYear",OracleType.Int32,5),
                                                new OracleParameter("equipmentname",OracleType.VarChar,200),
                                                new OracleParameter("p_cursor",OracleType.Cursor)};
                        parms2[0].Direction = ParameterDirection.Input;
                        parms2[1].Direction = ParameterDirection.Input;
                        parms2[2].Direction = ParameterDirection.Output;

                        parms2[0].Value = nYear;
                        parms2[1].Value = equipmentname;

                        dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.StoredProcedure, "PROC_ImportantMonthDirection", parms2);
                        break;
                    case "day":
                        OracleParameter[] parms3 = { 
                                                    new OracleParameter("beginTime",OracleType.VarChar,200),
                                                    new OracleParameter("endTime",OracleType.VarChar,200),
                                                    new OracleParameter("equipmentname",OracleType.VarChar,200),
                                                    new OracleParameter("p_cursor",OracleType.Cursor)};
                        parms3[0].Direction = ParameterDirection.Input;
                        parms3[1].Direction = ParameterDirection.Input;
                        parms3[2].Direction = ParameterDirection.Input;
                        parms3[3].Direction = ParameterDirection.Output;

                        parms3[0].Value = beginTime;
                        parms3[1].Value = endTime;
                        parms3[2].Value = equipmentname;

                        dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.StoredProcedure, "PROC_IMPORTANTDAYDIRECTION", parms3);
                        break;
                }
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
        /// 重大事件30分钟恢复率

        /// </summary>
        /// <param name="type"></param>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <param name="equipmentname"></param>
        /// <param name="issuerootname"></param>
        /// <returns></returns>
        public static DataTable GetImportantRecoveryRate(string type, int nYear, string beginTime, string endTime, string equipmentname, string timespan)
        {
            DataTable dt = null;
            OracleConnection cn = ConfigTool.GetConnection();
            if (cn.State != ConnectionState.Open)
            {
                cn.Open();
            }
            try
            {
                switch (type)
                {
                    case "year":
                        OracleParameter[] parms1 = {
                      new OracleParameter("equipmentname",OracleType.VarChar,200),
                      new OracleParameter("p_cursor",OracleType.Cursor)};
                        parms1[0].Direction = ParameterDirection.Input;
                        parms1[1].Direction = ParameterDirection.Output;

                        parms1[0].Value = equipmentname;
                        dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.StoredProcedure, "PROC_IMPORTANTRATEYEAR", parms1);
                        break;
                    case "month":
                        OracleParameter[] parms2 ={
                                                new OracleParameter("nYear",OracleType.Int32,5),
                        new OracleParameter("equipmentname",OracleType.VarChar,200),
                      new OracleParameter("p_cursor",OracleType.Cursor)};
                        parms2[0].Direction = ParameterDirection.Input;
                        parms2[1].Direction = ParameterDirection.Input;
                        parms2[2].Direction = ParameterDirection.Output;

                        parms2[0].Value = nYear;
                        parms2[1].Value = equipmentname;

                        dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.StoredProcedure, "PROC_IMPORTANTRATEMONTH", parms2);
                        break;
                    case "day":
                        OracleParameter[] parms3 = { 
                                                    new OracleParameter("beginTime",OracleType.VarChar,200),
                                                    new OracleParameter("endTime",OracleType.VarChar,200),
                                                    new OracleParameter("equipmentname",OracleType.VarChar,200),
                                                    new OracleParameter("timeDate",OracleType.VarChar,200),
                                                    new OracleParameter("p_cursor",OracleType.Cursor)};
                        parms3[0].Direction = ParameterDirection.Input;
                        parms3[1].Direction = ParameterDirection.Input;
                        parms3[2].Direction = ParameterDirection.Input;
                        parms3[3].Direction = ParameterDirection.Input;
                        parms3[4].Direction = ParameterDirection.Output;
                        parms3[0].Value = beginTime;
                        parms3[1].Value = endTime;
                        parms3[2].Value = equipmentname;
                        parms3[3].Value = timespan;

                        dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.StoredProcedure, "PROC_IMPORTANTRATEDAY", parms3);
                        break;
                }
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
        /// 重大事件60分钟恢复率

        /// </summary>
        /// <param name="type"></param>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <param name="equipmentname"></param>
        /// <param name="issuerootname"></param>
        /// <returns></returns>
        public static DataTable GetImportantRecoveryRateForHour(string type, int nYear, string beginTime, string endTime, string equipmentname, string timespan)
        {
            DataTable dt = null;
            OracleConnection cn = ConfigTool.GetConnection();
            if (cn.State != ConnectionState.Open)
            {
                cn.Open();
            }
            try
            {
                switch (type)
                {
                    case "year":
                        OracleParameter[] parms1 = {
                      new OracleParameter("equipmentname",OracleType.VarChar,200),
                      new OracleParameter("p_cursor",OracleType.Cursor)};
                        parms1[0].Direction = ParameterDirection.Input;
                        parms1[1].Direction = ParameterDirection.Output;

                        parms1[0].Value = equipmentname;

                        dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.StoredProcedure, "PROC_IMPORTANTRATEHOUR_YEAR", parms1);
                        break;
                    case "month":
                        OracleParameter[] parms2 ={
                                                new OracleParameter("nYear",OracleType.Int32,5),
                        new OracleParameter("equipmentname",OracleType.VarChar,200),
                      new OracleParameter("p_cursor",OracleType.Cursor)};
                        parms2[0].Direction = ParameterDirection.Input;
                        parms2[1].Direction = ParameterDirection.Input;
                        parms2[2].Direction = ParameterDirection.Output;

                        parms2[0].Value = nYear;
                        parms2[1].Value = equipmentname;

                        dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.StoredProcedure, "PROC_IMPORTANTRATEHOUR_MONTH", parms2);
                        break;
                    case "day":
                        OracleParameter[] parms3 = { 
                                                    new OracleParameter("beginTime",OracleType.VarChar,200),
                                                    new OracleParameter("endTime",OracleType.VarChar,200),
                                                    new OracleParameter("equipmentname",OracleType.VarChar,200),
                                                    new OracleParameter("timeDate",OracleType.VarChar,200),
                                                    new OracleParameter("p_cursor",OracleType.Cursor)};
                        parms3[0].Direction = ParameterDirection.Input;
                        parms3[1].Direction = ParameterDirection.Input;
                        parms3[2].Direction = ParameterDirection.Input;
                        parms3[3].Direction = ParameterDirection.Input;
                        parms3[4].Direction = ParameterDirection.Output;
                        parms3[0].Value = beginTime;
                        parms3[1].Value = endTime;
                        parms3[2].Value = equipmentname;
                        parms3[3].Value = timespan;
                        dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.StoredProcedure, "PROC_IMPORTANTHOUR_DAY", parms3);
                        break;
                }
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
        /// 重大事件平均恢复时间统计
        /// </summary>
        /// <param name="type"></param>
        /// <param name="year"></param>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <param name="equipmentname"></param>
        /// <returns></returns>
        public static DataTable GetImportantRecovery(string type, string year, string beginTime, string endTime, string equipmentname)
        {
            DataTable dt = null;
            string strSQL = "";
            OracleConnection cn = ConfigTool.GetConnection();
            if (cn.State != ConnectionState.Open)
            {
                cn.Open();
            }
            switch (type)
            {
                case "year":
                    strSQL = @"SELECT datepart('year',c.realitytime) as 年份,
                                          decode((select count(flowid)
                                                        from cst_issues
                                                       where flowid not in
                                                             (select c.flowid
                                                                from cst_issues c
                                                               inner join (select e.flowid
                                                                            from es_message e
                                                                           right join (select flowid
                                                                                        from es_message m
                                                                                      having count(Flowid) = 1
                                                                                       group by flowid) m on e.flowid =
                                                                                                             m.flowid
                                                                           where e.status = 20
                                                                             and senderid = 0) m on c.flowid =
                                                                                                    m.flowid
                                                               where c.servicetype = '重大事件')
                                                         and servicetype = '重大事件'),0,0,
                                          round((sum(c.endtime-c.realitytime)*24*60)/
                                                  (select count(flowid)
                                                        from cst_issues
                                                       where flowid not in
                                                             (select c.flowid
                                                                from cst_issues c
                                                               inner join (select e.flowid
                                                                            from es_message e
                                                                           right join (select flowid
                                                                                        from es_message m
                                                                                      having count(Flowid) = 1
                                                                                       group by flowid) m on e.flowid =
                                                                                                             m.flowid
                                                                           where e.status = 20
                                                                             and senderid = 0) m on c.flowid =
                                                                                                    m.flowid
                                                               where c.servicetype = '重大事件')
                                                         and servicetype = '重大事件')
                                          ,2)) as 恢复时间
                                        FROM cst_issues c  where c.servicetype = '重大事件'
                               and flowid not in
                                   (select c.flowid
                                      from cst_issues c
                                     inner join (select e.flowid
                                                  from es_message e
                                                 right join (select flowid
                                                              from es_message m
                                                            having count(Flowid) = 1
                                                             group by flowid) m on e.flowid = m.flowid
                                                 where e.status = 20
                                                   and senderid = 0) m on c.flowid = m.flowid
                                     where c.servicetype = '重大事件')";
                    if (equipmentname != "")
                    {
                        strSQL = @"SELECT datepart('year',c.realitytime) as 年份,
                                              decode((select count(flowid)
                                                            from cst_issues
                                                           where flowid not in
                                                                 (select c.flowid
                                                                    from cst_issues c
                                                                   inner join (select e.flowid
                                                                                from es_message e
                                                                               right join (select flowid
                                                                                            from es_message m
                                                                                          having count(Flowid) = 1
                                                                                           group by flowid) m on e.flowid =
                                                                                                                 m.flowid
                                                                               where e.status = 20
                                                                                 and senderid = 0) m on c.flowid =
                                                                                                        m.flowid
                                                                   where c.servicetype = '重大事件')
                                                             and servicetype = '重大事件' and equipmentname='" + equipmentname + "'),0,0,";
                        strSQL += @" round((sum(c.endtime-c.realitytime)*24*60)/
                                                  (select count(flowid)
                                                        from cst_issues
                                                       where flowid not in
                                                             (select c.flowid
                                                                from cst_issues c
                                                               inner join (select e.flowid
                                                                            from es_message e
                                                                           right join (select flowid
                                                                                        from es_message m
                                                                                      having count(Flowid) = 1
                                                                                       group by flowid) m on e.flowid =
                                                                                                             m.flowid
                                                                           where e.status = 20
                                                                             and senderid = 0) m on c.flowid =
                                                                                                    m.flowid
                                                               where c.servicetype = '重大事件')
                                                         and servicetype = '重大事件'  and  equipmentname='" + equipmentname + "'),2)) as 恢复时间 FROM cst_issues c  where c.servicetype = '重大事件' and c.equipmentname='" + equipmentname + "'";
                        strSQL += @" and flowid not in
                                   (select c.flowid
                                      from cst_issues c
                                     inner join (select e.flowid
                                                  from es_message e
                                                 right join (select flowid
                                                              from es_message m
                                                            having count(Flowid) = 1
                                                             group by flowid) m on e.flowid = m.flowid
                                                 where e.status = 20
                                                   and senderid = 0) m on c.flowid = m.flowid
                                     where c.servicetype = '重大事件') ";
                    }
                    strSQL += "  GROUP BY datepart('year',c.realitytime) order by 年份 asc";
                    break;
                case "month":
                    strSQL = @"SELECT c.months as 月份,
       decode(count(months),0,0,round((sum(c.endtime - c.realitytime) * 24 * 60) / count(months), 2)) as 恢复时间
  from (select equipmentname,
               datepart('month', realitytime) as months,
               realitytime,
               endtime
          from cst_issues
         where flowid not in
               (select c.flowid
                  from cst_issues c
                 inner join (select e.flowid
                              from es_message e
                             right join (select flowid
                                          from es_message m where status = 20
                               and senderid = 0
                                        having count(Flowid) = 1
                                         group by flowid) m on e.flowid =
                                                               m.flowid
                             where e.status = 20
                               and senderid = 0) m on c.flowid = m.flowid
                 where c.servicetype = '重大事件')
           and servicetype = '重大事件'
           and datepart('year', realitytime) =" + year + ") c ";
                    if (equipmentname != "")
                    {
                        strSQL = @"SELECT c.months as 月份,
       decode(count(months),0,0,round((sum(c.endtime - c.realitytime) * 24 * 60) / count(months), 2)) as 恢复时间
  from (select equipmentname,
               datepart('month', realitytime) as months,
               realitytime,
               endtime
          from cst_issues
         where flowid not in
               (select c.flowid
                  from cst_issues c
                 inner join (select e.flowid
                              from es_message e
                             right join (select flowid
                                          from es_message m where status = 20
                               and senderid = 0
                                        having count(Flowid) = 1
                                         group by flowid) m on e.flowid =
                                                               m.flowid
                             where e.status = 20
                               and senderid = 0) m on c.flowid = m.flowid
                 where c.servicetype = '重大事件')
           and servicetype = '重大事件' and equipmentname='" + equipmentname + "' and datepart('year', realitytime) =" + year + ") c";
                    }
                    strSQL += "  GROUP BY c.months order by c.months";
                    break;
                case "day":
                    strSQL = @"select to_date(to_char(realitytime, 'yyyy-mm-dd'), 'yyyy-mm-dd') as 日期,
       decode(count(to_date(to_char(realitytime, 'yyyy-mm-dd'),
                            'yyyy-mm-dd')),
              0,
              0,
              round((sum((endtime - realitytime) * 24 * 60) /
                    count(to_date(to_char(realitytime, 'yyyy-mm-dd'),
                                   'yyyy-mm-dd'))),
                    2)) as 恢复时间
  from cst_issues
 where flowid not in
       (select c.flowid
          from cst_issues c
         inner join (select e.flowid
                      from es_message e
                     right join (select flowid
                                  from es_message m
                                having count(Flowid) = 1
                                 group by flowid) m on e.flowid = m.flowid
                     where e.status = 20
                       and senderid = 0) m on c.flowid = m.flowid
         where servicetype = '重大事件')
          and servicetype='重大事件' 
            and  realitytime between to_date('" + beginTime + "','yyyy-mm-dd HH24:mi:ss') and to_date('" + endTime + "','yyyy-mm-dd HH24:mi:ss')";
                    if (equipmentname != "")
                    {
                        strSQL = @"select to_date(to_char(realitytime, 'yyyy-mm-dd'), 'yyyy-mm-dd') as 日期,
       decode(count(to_date(to_char(realitytime, 'yyyy-mm-dd'),
                            'yyyy-mm-dd')),
              0,
              0,
              round((sum((endtime - realitytime) * 24 * 60) /
                    count(to_date(to_char(realitytime, 'yyyy-mm-dd'),
                                   'yyyy-mm-dd'))),
                    2)) as 恢复时间
  from cst_issues
 where flowid not in
       (select c.flowid
          from cst_issues c
         inner join (select e.flowid
                      from es_message e
                     right join (select flowid
                                  from es_message m
                                having count(Flowid) = 1
                                 group by flowid) m on e.flowid = m.flowid
                     where e.status = 20
                       and senderid = 0) m on c.flowid = m.flowid
         where servicetype = '重大事件')
   and servicetype = '重大事件' 
and equipmentname='" + equipmentname + "'  and realitytime between to_date('" + beginTime + "','yyyy-mm-dd HH24:mi:ss') and to_date('" + endTime + "','yyyy-mm-dd HH24:mi:ss')";
                    }
                    strSQL += " group by to_date(to_char(realitytime, 'yyyy-mm-dd'), 'yyyy-mm-dd') order by 日期 asc";
                    break;
            }
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
        /// 重大事件平均恢复时间
        /// </summary>
        /// <param name="type"></param>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <param name="equipmentname"></param>
        /// <param name="issuerootname"></param>
        /// <returns></returns>
        public static DataTable GetImportantRecoveryTime(string type, int nYear, string beginTime, string endTime, string equipmentname)
        {
            DataTable dt = null;
            OracleConnection cn = ConfigTool.GetConnection();
            if (cn.State != ConnectionState.Open)
            {
                cn.Open();
            }
            try
            {
                switch (type)
                {
                    case "year":
                        OracleParameter[] parms1 = {
                                                  new OracleParameter("equipmentname",OracleType.VarChar,200),
                                                  new OracleParameter("p_cursor",OracleType.Cursor)};
                        parms1[0].Direction = ParameterDirection.Input;
                        parms1[1].Direction = ParameterDirection.Output;

                        parms1[0].Value = equipmentname;

                        dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.StoredProcedure, "PROC_IMPORTANTRecoveryYEAR", parms1);
                        break;
                    case "month":
                        OracleParameter[] parms2 ={
                                                new OracleParameter("nYear",OracleType.Int32,5),
                                                new OracleParameter("equipmentname",OracleType.VarChar,200),
                                                new OracleParameter("p_cursor",OracleType.Cursor)};
                        parms2[0].Direction = ParameterDirection.Input;
                        parms2[1].Direction = ParameterDirection.Input;
                        parms2[2].Direction = ParameterDirection.Output;

                        parms2[0].Value = nYear;
                        parms2[1].Value = equipmentname;

                        dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.StoredProcedure, "PROC_IMPORTANTRecoveryMONTH", parms2);
                        break;
                    case "day":
                        OracleParameter[] parms3 = { 
                                                    new OracleParameter("beginTime",OracleType.VarChar,200),
                                                    new OracleParameter("endTime",OracleType.VarChar,200),
                                                    new OracleParameter("equipmentname",OracleType.VarChar,200),
                                                    new OracleParameter("p_cursor",OracleType.Cursor)};
                        parms3[0].Direction = ParameterDirection.Input;
                        parms3[1].Direction = ParameterDirection.Input;
                        parms3[2].Direction = ParameterDirection.Input;
                        parms3[3].Direction = ParameterDirection.Output;
                        parms3[0].Value = beginTime;
                        parms3[1].Value = endTime;
                        parms3[2].Value = equipmentname;

                        dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.StoredProcedure, "PROC_IMPORTANTRecoveryDAY", parms3);
                        break;
                }
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
        /// 关键系统可用率

        /// </summary>
        /// <param name="p_MorningR"></param>
        /// <param name="p_MorningE"></param>
        /// <param name="p_afternoonR"></param>
        /// <param name="p_afternoonE"></param>
        /// <param name="equipmentName"></param>
        /// <param name="realityTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public static DataTable GetSchemeRatio(string p_MorningR, string p_MorningE, string p_afternoonR, string p_afternoonE, string equipmentName, string realityTime, string endTime)
        {
            DataTable dt = null;
            OracleConnection cn = ConfigTool.GetConnection();
            if (cn.State != ConnectionState.Open)
            {
                cn.Open();
            }
            try
            {
                string sqlWhere = "";
                OracleParameter[] parms = {
                    new OracleParameter("p_MorningR",OracleType.VarChar,200),
                    new OracleParameter("p_MorningE",OracleType.VarChar,200),
                    new OracleParameter("p_afternoonR",OracleType.VarChar,200),
                    new OracleParameter("p_afternoonE",OracleType.VarChar,200),
                    new OracleParameter("equipmentName",OracleType.VarChar,200),
                    new OracleParameter("beginTime",OracleType.VarChar,200),
                    new OracleParameter("endTime",OracleType.VarChar,200),
                    new OracleParameter("p_cursor",OracleType.Cursor)
                };
                parms[0].Direction = ParameterDirection.Input;
                parms[1].Direction = ParameterDirection.Input;
                parms[2].Direction = ParameterDirection.Input;
                parms[3].Direction = ParameterDirection.Input;
                parms[4].Direction = ParameterDirection.Input;
                parms[5].Direction = ParameterDirection.Input;
                parms[6].Direction = ParameterDirection.Input;
                parms[7].Direction = ParameterDirection.Output;
                parms[0].Value = p_MorningR;
                parms[1].Value = p_MorningE;
                parms[2].Value = p_afternoonR;
                parms[3].Value = p_afternoonE;
                parms[4].Value = equipmentName;
                parms[5].Value = realityTime;
                parms[6].Value = endTime;
                dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.StoredProcedure, "PROC_GetSchemeRatio", parms);
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
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.StoredProcedure, "PROC_ServiceVolume", para);
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

        public static DataTable SeachStaffWorks(string strBeginDate, string strEndDate, long lngStatusID, long lngMastCustomer)
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
        public static DataTable EngineerAnalysis(string strBeginDate, string strEndDate, long lngStatusID, long lngMastCustomer, int pagesize, int pageindex, ref int rowcount)
        {
            string sWhere = string.Empty;
            if (strBeginDate != "")
                sWhere += " and  c.regsysdate >= to_date(" + StringTool.SqlQ(strBeginDate) + ",'yyyy-MM-dd   hh24:mi:ss')";
            if (strEndDate != "")
                sWhere += " and  c.regsysdate <= to_date(" + StringTool.SqlQ(strEndDate) + ",'yyyy-MM-dd   hh24:mi:ss')";

            sWhere += ((lngStatusID == 0) || (lngStatusID == -1) ? "" : " AND dealstatusid =" + lngStatusID.ToString());
            //if (lngMastCustomer != 0)
            //    sWhere += " And nvl(CustID,0) In (select ID from Br_ECustomer where MastCustID=" + lngMastCustomer.ToString() + ")";
            int startindex = 0;
            int endindex = 0;

            if (pageindex == 1)
            {
                startindex = 1;
                endindex = pagesize;
            }
            else
            {
                startindex = (pageindex - 1) * pagesize + 1;
                endindex = pageindex * pagesize;
            }
            string sql = "";

            string strs = @"SELECT * FROM(select b.*,ROWNUM RN from ( ";
            string stre = @")b WHERE ROWNUM <= " + endindex + ")WHERE RN >= " + startindex;

            //这里 修改了 left outer join 为 inner join
            string sSql = @"select a.ID,a.Name,a.BlongDeptName,a.Remark,a.UserName,Faculty,JoinDate,
                            nvl(b.Num,0) Num,Case when nvl(b.Num,0)=0 then '是' else '否' end IsnullWork
                            from Cst_ServiceStaff a
                            inner join (
                            select count(a.ID) Num,a.ID,Name,BlongDeptID,BlongDeptName 
                            from Cst_ServiceStaff a,Cst_ServiceStaffList b,Cst_Issues c
                            where b.ServiceStaffID=a.ID and b.FlowID=c.FlowID and b.NewFlag=1 ";
            sSql += sWhere + @" Group By a.ID,Name,BlongDeptID,BlongDeptName
                                ) b ON a.ID=b.ID Where 1=1  and a.deleted=0 ";
            if (lngMastCustomer != 0)
            {
                sSql += " And a.BlongDeptId=" + lngMastCustomer;
            }
            //sSql += " and rownum <=10";
            sql = sSql;
            sSql = strs + sSql + stre;

            OracleConnection cn = ConfigTool.GetConnection();
            try
            {
                DataTable dtsql = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, sql);
                rowcount = dtsql.Rows.Count;

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
                strWhere += " And a.ServiceTypeID=" + lngMastID;
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
                //DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, sSql);

                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.StoredProcedure, prcName, para);
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
            #region 之前的

            ////string sSql = string.Empty;
            //OracleConnection cn = ConfigTool.GetConnection();

            ////            sSql += @" declare @num1 decimal(18,0)
            ////                        SELECT @num1=Count(EquipMentID) FROM (SELECT distinct EquipMentID FROM cst_issues a where EquipMentID<>0 and EquipMentID<>-1";

            ////            if (strBegin != "")
            ////                sSql += " and  a.regsysdate >= " + StringTool.SqlQ(strBegin);
            ////            if (strEnd != "")
            ////                sSql += " and  a.regsysdate <= " + StringTool.SqlQ(strEnd);

            ////            //sSql += @" and a.regsysdate Between " + StringTool.SqlQ(strBegin) + " AND " + StringTool.SqlQ(strEnd);
            ////            if (lngMastCustomer != 0)
            ////                sSql += " And nvl(CustID,0) In (select ID from Br_ECustomer where MastCustID=" + lngMastCustomer.ToString() + ")";
            ////            sSql += @") a select @num1 '数量','已维修' 资产故障
            ////                        union all
            ////                        select  Count(ID)-@num1,'未维修' from Equ_Desk where Deleted=0 ";
            ////            if (lngMastCustomer != 0)
            ////                sSql += " And nvl(Costom,0) In (select ID from Br_ECustomer where MastCustID=" + lngMastCustomer.ToString() + ")";
            //OracleParameter[] para = {
            //        new OracleParameter("startDate", OracleType.VarChar ,50),
            //        new OracleParameter("endDate", OracleType.VarChar , 50),
            //        new OracleParameter("mastCustomerID", OracleType.VarChar , 50 ),
            //                                new OracleParameter("p_cursor",OracleType.Cursor)};
            //para[0].Direction = ParameterDirection.Input;
            //para[1].Direction = ParameterDirection.Input;
            //para[2].Direction = ParameterDirection.Input;
            //para[3].Direction = ParameterDirection.Output;

            //para[0].Value = StringTool.SqlQ(strBegin);
            //para[1].Value = StringTool.SqlQ(strEnd);
            //para[2].Value = lngMastCustomer.ToString();

            //try
            //{
            //    //DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.StoredProcedure , sSql);
            //    DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.StoredProcedure, "PROC_FaultRate", para);
            //    return dt;
            //}
            //catch (Exception e)
            //{
            //    throw e;
            //}
            //finally
            //{
            //    ConfigTool.CloseConnection(cn);
            //}
            #endregion
            string strsql = string.Empty;
            string strsqlsum = string.Empty;
            OracleConnection cn = ConfigTool.GetConnection();
            DataTable dts = new DataTable();
            dts.Columns.Add(new DataColumn().ColumnName = "数量");
            dts.Columns.Add(new DataColumn().ColumnName = "资产故障");

            try
            {
                if (lngMastCustomer <= 0)
                {
                    strsql = @"select count(e.id) num from Equ_Desk e  where e.id in( select distinct c.EquipmentID from cst_issues c  where c.equipmentid >0
                    and c.FlowModelID in
                     (
                     SELECT a.FlowModelID FROM ES_FlowModel a
                    INNER JOIN 
                    (SELECT Es_NodeModel.FlowModelID, ES_AppOp.OpID FROM Es_NodeModel
                     LEFT JOIN ES_AppOp ON Es_NodeModel.OpID = ES_AppOp.OpID
                     WHERE NodeModelID = 2 AND FlowModelID IN 
                     (SELECT FlowModelID FROM es_flowmodel WHERE DELETED = 0 AND Status = 1 AND AppID = 1026)
                    ) b ON a.FlowModelID =b.FlowModelID WHERE b.OpID =9351 or b.OpID = 9353)
                    )
                    and e.regtime BETWEEN TO_DATE( '" + strBegin + "', 'yyyy-MM-dd HH24:mi:ss') AND TO_DATE('" + strEnd + "','yyyy-MM-dd HH24:mi:ss') and e.mastcustid >0";

                    strsqlsum = @"select count(e.id) enum from equ_desk e where e.deleted = 0 and e.RegTime BETWEEN TO_DATE( '" + strBegin + "', 'yyyy-MM-dd HH24:mi:ss') AND TO_DATE('" + strEnd + "','yyyy-MM-dd HH24:mi:ss') ";
                }
                else
                {
                    strsql = @"select count(e.id) num from Equ_Desk e  where e.id in( select distinct c.EquipmentID from cst_issues c  where c.equipmentid >0
                    and c.FlowModelID in
                     (
                     SELECT a.FlowModelID FROM ES_FlowModel a
                    INNER JOIN 
                    (SELECT Es_NodeModel.FlowModelID, ES_AppOp.OpID FROM Es_NodeModel
                     LEFT JOIN ES_AppOp ON Es_NodeModel.OpID = ES_AppOp.OpID
                     WHERE NodeModelID = 2 AND FlowModelID IN 
                     (SELECT FlowModelID FROM es_flowmodel WHERE DELETED = 0 AND Status = 1 AND AppID = 1026)
                    ) b ON a.FlowModelID =b.FlowModelID WHERE b.OpID =9351 or b.OpID = 9353)
                    )
                    and e.regtime BETWEEN TO_DATE( '" + strBegin + "', 'yyyy-MM-dd HH24:mi:ss') AND TO_DATE('" + strEnd + "','yyyy-MM-dd HH24:mi:ss') and e.mastcustid='" + lngMastCustomer + "'";
                    strsqlsum = @"select count(e.id) enum from equ_desk e where e.deleted = 0 and e.RegTime BETWEEN TO_DATE( '" + strBegin + "', 'yyyy-MM-dd HH24:mi:ss') AND TO_DATE('" + strEnd + "','yyyy-MM-dd HH24:mi:ss')  and e.Mastcustid='" + lngMastCustomer + "'";
                }


                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strsql);//得到根据时间和服务单位查询上报的资产个数
                DataTable dtsum = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strsqlsum);//得到总资产个数


                decimal sbnum = 0;//资产上报个数
                decimal descksum = 0;//资产总个数

                if (dt != null && dt.Rows.Count > 0)
                {
                    if (!string.IsNullOrEmpty(dt.Rows[0][0].ToString()))
                    {
                        sbnum = Convert.ToDecimal(dt.Rows[0][0].ToString());//上报个数
                    }
                }
                else
                {
                    sbnum = 0;
                }

                if (dtsum != null && dtsum.Rows.Count > 0)
                {
                    if (!string.IsNullOrEmpty(dtsum.Rows[0][0].ToString()))
                    {
                        descksum = Convert.ToDecimal(dtsum.Rows[0][0].ToString());//资产总个数

                    }
                }
                else
                {
                    descksum = 0;
                }

                DataRow dr = dts.NewRow();
                dr["数量"] = sbnum;
                dr["资产故障"] = "故障数:" + sbnum;

                DataRow dr2 = dts.NewRow();
                dr2["数量"] = descksum - sbnum;
                dr2["资产故障"] = "无故障数:" + (descksum - sbnum);
                dts.Rows.Add(dr);
                dts.Rows.Add(dr2);
                return dts;
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
        public static DataTable GetFirstRatio(string strBegin, string strEnd, long lngFCDServiceType)
        {
            string sSql = string.Empty;
            OracleConnection cn = ConfigTool.GetConnection();

            sSql += @" select Count(SmsID) 数量,'首次解决' 首次解决,nvl(a.servicetype,'其他类别') 事件类别 FROM cst_issues a  where nvl(IsFirstSolve,0)=1";

            if (strBegin != "")
                sSql += " and  a.regsysdate >= to_date(" + StringTool.SqlQ(strBegin) + ",'yyyy-MM-dd HH24:mi:ss')";
            if (strEnd != "")
                sSql += " and  a.regsysdate <=to_date(" + StringTool.SqlQ(strEnd) + ",'yyyy-MM-dd HH24:mi:ss')";
            if (!string.IsNullOrEmpty(lngFCDServiceType.ToString()))
            {
                if (lngFCDServiceType.ToString() != "1001")
                {
                    sSql += " and a.servicetypeid='" + lngFCDServiceType + "'";
                }
            }
            sSql += " group by a.servicetype";

            sSql += @" union all
                        select Count(SmsID),'首次未解决',nvl(a.servicetype,'其他类别') 事件类别 FROM cst_issues a  where nvl(IsFirstSolve,0)=0";

            if (strBegin != "")
                sSql += " and  a.regsysdate >= to_date(" + StringTool.SqlQ(strBegin) + ",'yyyy-MM-dd HH24:mi:ss')";
            if (strEnd != "")
                sSql += " and  a.regsysdate <=to_date(" + StringTool.SqlQ(strEnd) + ",'yyyy-MM-dd HH24:mi:ss')";
            if (!string.IsNullOrEmpty(lngFCDServiceType.ToString()))
            {
                if (lngFCDServiceType.ToString() != "1001")
                {
                    sSql += " and a.servicetypeid='" + lngFCDServiceType + "'";
                }
            }
            sSql += " group by a.servicetype";

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
                              " datediff('Minute',sysdate,nvl(a.expected,sysdate)) as DiffMinute,d.Name as FlowName,A.actortype ,E.FinishedTime,E.Content" +
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
                              " AND d.ReceiveID = " + UserId.ToString() + " AND a.Deleted =" + (int)e_Deleted.eNormal + " AND a.ReceiverID = 0 " +
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
                              " AND d.ReceiveID = " + UserId.ToString() + " AND a.Deleted =" + (int)e_Deleted.eNormal + " AND a.ReceiverID = 0 " +
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
                              " AND d.ReceiveID = " + UserId.ToString() + " AND a.Deleted =" + (int)e_Deleted.eNormal + " AND a.ReceiverID = 0 " +
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
                              " AND d.ReceiveID = " + UserId.ToString() + " AND a.Deleted =" + (int)e_Deleted.eNormal + " AND a.ReceiverID = 0 " +
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
                        + " AND D.AppId <> 1028 AND D.AppID <> 1026 AND  D.AppID <> 420 AND D.AppID <> 201 AND D.AppID <> 210 "
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
                              " AND d.ReceiveID = " + UserId.ToString() + " AND a.Deleted =" + (int)e_Deleted.eNormal + " AND a.ReceiverID = 0 AND b.AppId <> 1028 AND b.AppID <> 1026 AND  b.AppID <> 420 AND b.AppID <> 201 AND b.AppID <> 210 " +
                              " ORDER BY a.MessageID DESC";
                }
            }

            OracleConnection cn = ConfigTool.GetConnection();
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigTool.CloseConnection(cn);
            return dt;
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

        #region 按区间统计事件类别

        /// <summary>
        /// 按区间统计事件类别

        /// </summary>
        /// <param name="type">0表示按周 否者表示按月</param>
        /// <param name="p_strWhere">where条件</param>
        /// <returns></returns>
        public static DataTable GetCst_IssuesIntervalCount(int type, string p_strWhere)
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
            dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.StoredProcedure, "PROC_Cst_IssuesIntervalCount", parms);
            ConfigTool.CloseConnection(cn);

            return dt;
        }
        #endregion

        #region 按照问题类别统计

        public static DataTable GetQuetByType(DateTime startTime, DateTime endTime)
        {
            DataTable dt = null;
            OracleParameter[] parms = {
                      new OracleParameter("P_STARTTIME",OracleType.DateTime),
                      new OracleParameter("P_ENDTIME",OracleType.DateTime),    
                      new OracleParameter("P_OUTTABLE",OracleType.Cursor)
                };

            parms[0].Direction = ParameterDirection.Input;
            parms[1].Direction = ParameterDirection.Input;
            parms[2].Direction = ParameterDirection.Output;

            parms[0].Value = startTime;
            parms[1].Value = endTime;


            OracleConnection cn = ConfigTool.GetConnection();
            if (cn.State != ConnectionState.Open)
            {
                cn.Open();
            }
            try
            {
                dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.StoredProcedure, "PROC_RPT_QUST_TYPE", parms);
            }
            catch (Exception ex)
            {
                E8Logger.Error(ex);
                throw;
            }
            finally
            {
                ConfigTool.CloseConnection(cn);
            }

            return dt;
        }

        #endregion

        #region 按照问题类别统计

        public static DataTable GetQuetByPriority(DateTime startTime, DateTime endTime)
        {
            DataTable dt = null;
            OracleParameter[] parms = {
                      new OracleParameter("P_STARTTIME",OracleType.DateTime),
                      new OracleParameter("P_ENDTIME",OracleType.DateTime),    
                      new OracleParameter("P_OUTTABLE",OracleType.Cursor)
                };

            parms[0].Direction = ParameterDirection.Input;
            parms[1].Direction = ParameterDirection.Input;
            parms[2].Direction = ParameterDirection.Output;

            parms[0].Value = startTime;
            parms[1].Value = endTime;


            OracleConnection cn = ConfigTool.GetConnection();
            if (cn.State != ConnectionState.Open)
            {
                cn.Open();
            }
            try
            {
                dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.StoredProcedure, "PROC_RPT_QUST_Priority", parms);
            }
            catch (Exception ex)
            {
                E8Logger.Error(ex);
                throw;
            }
            finally
            {
                ConfigTool.CloseConnection(cn);
            }

            return dt;
        }

        #endregion

        #region 按天统计工程师工作量
        public static DataTable GetCst_IssuesDayCount(DateTime startTime, DateTime endTime, string engineerIds)
        {
            DataTable dt = null;
            OracleParameter[] parms = {
                      new OracleParameter("P_STARTTIME",OracleType.DateTime),
                      new OracleParameter("P_ENDTIME",OracleType.DateTime),
                      new OracleParameter("P_ENGINEER",OracleType.VarChar,200),
                      new OracleParameter("P_OUTTABLE",OracleType.Cursor)
                };

            parms[0].Direction = ParameterDirection.Input;
            parms[1].Direction = ParameterDirection.Input;
            parms[2].Direction = ParameterDirection.Input;
            parms[3].Direction = ParameterDirection.Output;

            parms[0].Value = startTime;
            parms[1].Value = endTime;
            parms[2].Value = engineerIds;

            OracleConnection cn = ConfigTool.GetConnection();
            if (cn.State != ConnectionState.Open)
            {
                cn.Open();
            }
            try
            {
                dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.StoredProcedure, "PROC_RPT_ENGINERCOUNT", parms);
            }
            catch (Exception ex)
            {
                E8Logger.Error(ex);
                throw;
            }
            finally
            {
                ConfigTool.CloseConnection(cn);
            }

            return dt;
        }

        /// <summary>
        /// 按天统计工程师工作量
        /// </summary>
        /// <param name="p_strWhere">where条件</param>
        /// <param name="p_strWhere2">where2条件</param>
        /// <returns></returns>
        public static DataTable GetCst_IssuesDayCount(string p_strWhere, string p_strWhere2)
        {
            DataTable dt = null;

            OracleParameter[] parms = {
                      new OracleParameter("p_strWhere",OracleType.VarChar,2000),
                      new OracleParameter("p_strWhere2",OracleType.VarChar,2000),
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
            dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.StoredProcedure, "PROC_Cst_IssuesDayCount", parms);
            ConfigTool.CloseConnection(cn);

            return dt;
        }
        #endregion

        #region 计算满意度

        public string Getfeedback(object strID, string strBeginDate, string strEndDate, long lngStatusID, long lngMastCustomer)
        {
            decimal Id = 0;
            if (strID == null)
                return "";
            Id = Convert.ToDecimal(strID);
            string feedback = "";
            Double feedbacksum = 0.0f;
            Double flownum = 0.0f;
            Double jsnum = 0.0f;
            OracleConnection cn = ConfigTool.GetConnection();

            try
            {
                string sWhere = string.Empty;

                if (strBeginDate != "")
                    sWhere += " and  regsysdate >= to_date(" + StringTool.SqlQ(strBeginDate) + ",'yyyy-MM-dd   hh24:mi:ss')";
                if (strEndDate != "")
                    sWhere += " and  regsysdate <= to_date(" + StringTool.SqlQ(strEndDate) + ",'yyyy-MM-dd   hh24:mi:ss')";

                sWhere += ((lngStatusID == 0) || (lngStatusID == -1) ? "" : " AND dealstatusid =" + lngStatusID.ToString());

                //回访个数
                string sSql = "";
                if (lngMastCustomer != 0)
                {
                    sSql = string.Format("select count(e.feedperson) n from EA_Issues_FeedBack e " +
                   "where e.flowid in( select iss.flowid flowid from cst_issues iss where iss.flowid in " +
                   "( select cl.flowid  from Cst_ServiceStaff a inner join cst_servicestafflist cl on a.id = cl.servicestaffid " +
                   "where a.ID='" + strID + "' And BlongDeptId=" + lngMastCustomer + ")" + sWhere + ")");
                }
                else
                {
                    sSql = string.Format("select count(e.feedperson) n from EA_Issues_FeedBack e " +
                   "where e.flowid in( select iss.flowid flowid from cst_issues iss where iss.flowid in " +
                   "( select cl.flowid  from Cst_ServiceStaff a inner join cst_servicestafflist cl on a.id = cl.servicestaffid " +
                   "where a.ID='" + strID + "')" + sWhere + ")");
                }



                OracleDataReader dr = OracleDbHelper.ExecuteReader(cn, CommandType.Text, sSql);

                //满意个数
                string sql2 = "";
                if (lngMastCustomer != 0)
                {
                    sql2 = string.Format("select count(e.FeedBack) sumfeedback from EA_Issues_FeedBack e where e.flowid in(" +
                                             " select iss.flowid flowid from cst_issues iss where iss.flowid in(" +
                                             " select cl.flowid  from Cst_ServiceStaff a inner join cst_servicestafflist cl" +
                                             " on a.id = cl.servicestaffid where a.ID='" + strID + "' And BlongDeptId=" + lngMastCustomer + ")" + sWhere + ") and e.feedback != 3");
                }
                else
                {
                    sql2 = string.Format("select count(e.FeedBack) sumfeedback from EA_Issues_FeedBack e where e.flowid in(" +
                                                 " select iss.flowid flowid from cst_issues iss where iss.flowid in(" +
                                                 " select cl.flowid  from Cst_ServiceStaff a inner join cst_servicestafflist cl" +
                                                 " on a.id = cl.servicestaffid where a.ID='" + strID + "')" + sWhere + ") and e.feedback != 3");
                }


                OracleDataReader dr2 = OracleDbHelper.ExecuteReader(cn, CommandType.Text, sql2);
                if (dr != null)
                {
                    while (dr.Read())
                    {
                        if (dr[0].ToString() != "")
                        {
                            feedbacksum = Convert.ToDouble(dr[0].ToString());
                        }
                    }
                }

                while (dr2.Read())
                {
                    if (!string.IsNullOrEmpty(dr2[0].ToString()))
                    {
                        flownum = Convert.ToDouble(dr2[0].ToString());
                    }
                }

                int N = 4;
                if (flownum > 0)
                {
                    jsnum = flownum / feedbacksum;//满意个数/回复个数
                    jsnum = Math.Round(jsnum, 2);
                    feedback = (jsnum * 100).ToString("N" + N.ToString()) + "%";
                }
                else
                    feedback = "0.0000%";
                dr2.Close();
                dr.Close();

                return feedback;
            }
            finally { ConfigTool.CloseConnection(cn); }
        }
        #endregion

        #region 计算回复率

        public string GetGetFeedPerson(object strID, string strBeginDate, string strEndDate, long lngStatusID, long lngMastCustomer)
        {
            decimal Id = 0;
            if (strID == null)
                return "";
            Id = Convert.ToDecimal(strID);
            Double feedsum = 0.0f;
            Double flownum = 0.0f;
            Double jsum = 0.0f;
            string FeedPerson = "";
            OracleConnection cn = ConfigTool.GetConnection();

            try
            {
                string sWhere = string.Empty;
                if (strBeginDate != "")
                    sWhere += " and  regsysdate >= to_date(" + StringTool.SqlQ(strBeginDate) + ",'yyyy-MM-dd   hh24:mi:ss')";
                if (strEndDate != "")
                    sWhere += " and  regsysdate <= to_date(" + StringTool.SqlQ(strEndDate) + ",'yyyy-MM-dd   hh24:mi:ss')";

                sWhere += ((lngStatusID == 0) || (lngStatusID == -1) ? "" : " AND dealstatusid =" + lngStatusID.ToString());

                //回访个数
                string sSql = "";
                if (lngMastCustomer != 0)
                {
                    sSql = string.Format("select count(e.feedperson) n from EA_Issues_FeedBack e " +
                   "where e.flowid in( select iss.flowid flowid from cst_issues iss where iss.flowid in " +
                   "( select cl.flowid  from Cst_ServiceStaff a inner join cst_servicestafflist cl on a.id = cl.servicestaffid " +
                   "where a.ID='" + strID + "' And BlongDeptId=" + lngMastCustomer + ")" + sWhere + ")");
                }
                else
                {
                    sSql = string.Format("select count(e.feedperson) n from EA_Issues_FeedBack e " +
                   "where e.flowid in( select iss.flowid flowid from cst_issues iss where iss.flowid in " +
                   "( select cl.flowid  from Cst_ServiceStaff a inner join cst_servicestafflist cl on a.id = cl.servicestaffid " +
                   "where a.ID='" + strID + "')" + sWhere + ")");
                }

                OracleDataReader dr = OracleDbHelper.ExecuteReader(cn, CommandType.Text, sSql);

                //事件个数
                string sql2 = "";
                if (lngMastCustomer != 0)
                {
                    sql2 = string.Format("select count(*) num from Cst_Issues cis where cis.flowid in ( select cl.flowid from Cst_ServiceStaff a inner join cst_servicestafflist cl" +
                       " on a.id = cl.servicestaffid where a.id='" + strID + "' And BlongDeptId=" + lngMastCustomer + ") " + sWhere + " ");
                }
                else
                {
                    sql2 = string.Format("select count(*) num from Cst_Issues cis where cis.flowid in ( select cl.flowid from Cst_ServiceStaff a inner join cst_servicestafflist cl" +
                          " on a.id = cl.servicestaffid where a.id='" + strID + "')" + sWhere + "");
                }


                OracleDataReader dr2 = OracleDbHelper.ExecuteReader(cn, CommandType.Text, sql2);

                while (dr.Read())
                {
                    if (dr[0].ToString() != "0")
                    {
                        feedsum = Convert.ToDouble(dr[0].ToString());
                    }
                }

                while (dr2.Read())
                {
                    if (!string.IsNullOrEmpty(dr2[0].ToString()))
                    {
                        flownum = Convert.ToDouble(dr2[0].ToString());
                    }
                }

                int N = 4;
                if (flownum > 0)
                {
                    jsum = feedsum / flownum;
                    //jsum = Math.Round(jsum, 2);
                    FeedPerson = (jsum * 100).ToString("N" + N.ToString()) + "%";
                }
                else
                    FeedPerson = "0.0000%";

                dr2.Close();
                dr.Close();

                return FeedPerson;
            }
            finally { ConfigTool.CloseConnection(cn); }
        }
        #endregion

        #region  计算实际达标率

        public string GetTarget(object strID, string strBeginDate, string strEndDate, long lngStatusID, long lngMastCustomer)
        {
            decimal Id = 0;
            if (strID == null)
                return "";
            Id = Convert.ToDecimal(strID);
            string feedback = "";
            Double feedbacksum = 0.0f;
            Double flownum = 0.0f;
            Double jsnum = 0.0f;
            OracleConnection cn = ConfigTool.GetConnection();

            try
            {
                string sWhere = string.Empty;
                if (strBeginDate != "")
                    sWhere += " and  regsysdate >= to_date(" + StringTool.SqlQ(strBeginDate) + ",'yyyy-MM-dd   hh24:mi:ss')";
                if (strEndDate != "")
                    sWhere += " and  regsysdate <= to_date(" + StringTool.SqlQ(strEndDate) + ",'yyyy-MM-dd   hh24:mi:ss')";

                sWhere += ((lngStatusID == 0) || (lngStatusID == -1) ? "" : " AND dealstatusid =" + lngStatusID.ToString());

                //满意个数
                string sSql = "";
                if (lngMastCustomer != 0)
                {
                    sSql = string.Format("select count(e.FeedBack) sumfeedback from EA_Issues_FeedBack e where e.flowid in(" +
                                               " select iss.flowid flowid from cst_issues iss where iss.flowid in(" +
                                               " select cl.flowid  from Cst_ServiceStaff a inner join cst_servicestafflist cl" +
                                               " on a.id = cl.servicestaffid where a.ID='" + strID + "' And BlongDeptId=" + lngMastCustomer + ")" + sWhere + ") and e.feedback != 3");
                }
                else
                {
                    sSql = string.Format("select count(e.FeedBack) sumfeedback from EA_Issues_FeedBack e where e.flowid in(" +
                                                 " select iss.flowid flowid from cst_issues iss where iss.flowid in(" +
                                                 " select cl.flowid  from Cst_ServiceStaff a inner join cst_servicestafflist cl" +
                                                 " on a.id = cl.servicestaffid where a.ID='" + strID + "')" + sWhere + ") and e.feedback != 3");
                }

                OracleDataReader dr = OracleDbHelper.ExecuteReader(cn, CommandType.Text, sSql);
                //总事件数
                string sql2 = "";
                if (lngMastCustomer != 0)
                {
                    sql2 = string.Format("select count(*) num from Cst_Issues cis where cis.flowid in ( select cl.flowid from Cst_ServiceStaff a inner join cst_servicestafflist cl" +
                       " on a.id = cl.servicestaffid where a.id='" + strID + "' And BlongDeptId=" + lngMastCustomer + ") " + sWhere + " ");
                }
                else
                {
                    sql2 = string.Format("select count(*) num from Cst_Issues cis where cis.flowid in ( select cl.flowid from Cst_ServiceStaff a inner join cst_servicestafflist cl" +
                       " on a.id = cl.servicestaffid where a.id='" + strID + "')" + sWhere + "");
                }

                string sql3 = "";
                if (lngMastCustomer != 0)
                {
                    sql3 = string.Format("select  cs.levelid,cs.target from  Cst_SLGuid cs " +
                                        "where cs.levelid in " +
                           "(select  max(cls.servicelevelid) servicelevelid   from  Cst_Issues cls where cls.flowid in ( " +
                           "select cl.flowid from Cst_ServiceStaff a inner join cst_servicestafflist cl " +
                           "on a.id = cl.servicestaffid where a.id = '" + strID + "' And BlongDeptId=" + lngMastCustomer + " )" + sWhere + " )group by cs.levelid,cs.target");
                }
                else
                {
                    sql3 = string.Format("select  cs.levelid,cs.target from  Cst_SLGuid cs " +
                                        "where cs.levelid in " +
                           "(select  max(cls.servicelevelid) servicelevelid   from  Cst_Issues cls where cls.flowid in ( " +
                           "select cl.flowid from Cst_ServiceStaff a inner join cst_servicestafflist cl " +
                           "on a.id = cl.servicestaffid where a.id = '" + strID + "')" + sWhere + " )group by cs.levelid,cs.target");
                }



                OracleDataReader dr2 = OracleDbHelper.ExecuteReader(cn, CommandType.Text, sql2);
                OracleDataReader dr3 = OracleDbHelper.ExecuteReader(cn, CommandType.Text, sql3);

                if (dr != null)
                {
                    while (dr.Read())
                    {
                        if (dr[0].ToString() != "")
                        {
                            feedbacksum = Convert.ToDouble(dr[0].ToString());
                        }

                    }

                }

                while (dr2.Read())
                {
                    flownum = Convert.ToDouble(dr2[0].ToString());
                }

                double target = 0.0f;
                int num = 0;
                if (dr3 != null)
                {
                    while (dr3.Read())
                    {
                        if (dr3[1] != null && dr3[1].ToString() != "")
                        {
                            if (!string.IsNullOrEmpty(dr3[1].ToString()))
                            {
                                target += Convert.ToDouble(dr3[1].ToString());//获取达标率


                                num++;
                            }
                        }

                    }

                    if (num > 0)
                        target = target / num;
                }

                if (flownum > 0)
                {
                    jsnum = feedbacksum / flownum;
                    //jsnum = Math.Round(jsnum, 2);
                    jsnum = jsnum * 100; //
                    int N = 4;

                    if (target > 0)
                    {
                        if (jsnum >= target)
                            feedback = "100.0000%";
                        else
                            feedback = jsnum.ToString("N" + N.ToString()) + "%";
                    }
                    else
                    {
                        feedback = "100.0000%";
                    }
                }
                else
                    feedback = "0.0000%";

                dr2.Close();
                dr.Close();
                dr3.Close();

                return feedback;
            }
            finally { ConfigTool.CloseConnection(cn); }
        }
        #endregion

        #region  封装的日期方法

        public static string SqlDate(DateTime dt)
        {
            return string.Format("to_date('{0}','yyyy-mm-dd hh24:mi:ss')", dt.ToString("yyyy-MM-dd hh:mm:ss"));
        }

        public static string SqlDate(DateTime? dt)
        {
            if (!dt.HasValue)
            {
                return "null";
            }
            return SqlDate(dt.Value);
        }

        #endregion

        #region 获取节假日

        /// <summary>
        /// 根据两个日期，返回之间的节假日

        /// </summary>
        /// <param name="sartime">开始时间</param>
        /// <param name="endtime">结束时间</param>
        /// <returns>节假日</returns>
        public DataTable GetCalender(string sartime, string endtime)
        {
            string strsql = string.Empty;
            OracleConnection cn = ConfigTool.GetConnection();
            try
            {
                strsql = @"select * from ts_calender tc where tc.caldate between  '" + sartime.ToString() + "' and '" + endtime.ToString() + "'";
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strsql);
                return dt;
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

        #region 获取事件总数
        /// <summary>
        /// 根据不同条件查询事件1总数量

        /// </summary>
        /// <param name="strID"></param>
        /// <param name="strBeginDate"></param>
        /// <param name="strEndDate"></param>
        /// <param name="lngStatusID"></param>
        /// <param name="lngMastCustomer"></param>
        /// <returns></returns>
        public DataTable Get_Issuse(object strID, string strBeginDate, string strEndDate, long lngStatusID, long lngMastCustomer)
        {
            decimal Id = 0;
            if (strID == null)
                return null;
            Id = Convert.ToDecimal(strID);
            string feedback = "";
            Double feedbacksum = 0.0f;
            Double flownum = 0.0f;
            Double jsnum = 0.0f;
            OracleConnection cn = ConfigTool.GetConnection();

            string sWhere = string.Empty;
            if (strBeginDate != "")
                sWhere += " and  regsysdate >= to_date(" + StringTool.SqlQ(strBeginDate) + ",'yyyy-MM-dd   hh24:mi:ss')";
            if (strEndDate != "")
                sWhere += " and  regsysdate <= to_date(" + StringTool.SqlQ(strEndDate) + ",'yyyy-MM-dd   hh24:mi:ss')";

            sWhere += ((lngStatusID == 0) || (lngStatusID == -1) ? "" : " AND dealstatusid =" + lngStatusID.ToString());

            //总事件数
            string sql2 = "";
            if (lngMastCustomer != 0)
            {
                sql2 = string.Format("select count(*) num from Cst_Issues cis where cis.flowid in ( select cl.flowid from Cst_ServiceStaff a inner join cst_servicestafflist cl" +
                   " on a.id = cl.servicestaffid where a.id='" + strID + "' And BlongDeptId=" + lngMastCustomer + ") " + sWhere + " ");
            }
            else
            {
                sql2 = string.Format("select count(*) num from Cst_Issues cis where cis.flowid in ( select cl.flowid from Cst_ServiceStaff a inner join cst_servicestafflist cl" +
                   " on a.id = cl.servicestaffid where a.id='" + strID + "')" + sWhere + "");
            }

            try
            {
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, sql2);
                return dt;
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

        #region 计算处理达标率的事件
        /// <summary>
        /// 获取在规定处理时限内完成的事件数
        /// </summary>
        /// <returns></returns>
        public int Get_Limit_OKnum(object strID, string strBeginDate, string strEndDate, long lngStatusID, long lngMastCustomer)
        {
            decimal Id = 0;

            Id = Convert.ToDecimal(strID);
            string feedback = "";
            Double feedbacksum = 0.0f;
            Double flownum = 0.0f;
            Double jsnum = 0.0f;
            OracleConnection cn = ConfigTool.GetConnection();

            string sWhere = string.Empty;
            if (strBeginDate != "")
                sWhere += " and  regsysdate >= to_date(" + StringTool.SqlQ(strBeginDate) + ",'yyyy-MM-dd   hh24:mi:ss')";
            if (strEndDate != "")
                sWhere += " and  regsysdate <= to_date(" + StringTool.SqlQ(strEndDate) + ",'yyyy-MM-dd   hh24:mi:ss')";

            sWhere += ((lngStatusID == 0) || (lngStatusID == -1) ? "" : " AND dealstatusid =" + lngStatusID.ToString());

            //总事件数
            string sql2 = "";
            if (lngMastCustomer != 0)
            {
                sql2 = string.Format("select * from Cst_Issues cis where cis.flowid in ( select cl.flowid from Cst_ServiceStaff a inner join cst_servicestafflist cl" +
                   " on a.id = cl.servicestaffid where a.id='" + strID + "' And BlongDeptId=" + lngMastCustomer + ") " + sWhere + " ");
            }
            else
            {
                sql2 = string.Format("select *  from Cst_Issues cis where cis.flowid in ( select cl.flowid from Cst_ServiceStaff a inner join cst_servicestafflist cl" +
                   " on a.id = cl.servicestaffid where a.id='" + strID + "')" + sWhere + "");
            }

            try
            {
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, sql2);


                int oknum = 0;//合格的事件数
                string flowids = string.Empty;//流程ID
                foreach (DataRow dr in dt.Rows)
                {
                    if (!string.IsNullOrEmpty(dr["flowid"].ToString()))
                    {
                        flowids = flowids + dr["flowid"].ToString() + ",";
                    }
                }

                flowids = flowids.Trim().Substring(0, flowids.Length - 1);//

                string strsql = string.Format("select efl.* from ea_flowbuslimit efl where efl.flowid in (" + flowids + ") and efl.guidid = 10001");
                DataTable dtflow = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strsql);//每个事件对应的处理时限集合


                DateTime FinishedTime = DateTime.Now;//完成时间——初始化当前系统时间
                DateTime limitime = DateTime.Now;//设置的处理时限

                foreach (DataRow dr in dt.Rows)
                {
                    if (!string.IsNullOrEmpty(dr["FinishedTime"].ToString()))
                    {
                        FinishedTime = Convert.ToDateTime(dr["FinishedTime"].ToString());//读取完成时间
                    }
                    else
                    {
                        FinishedTime = DateTime.Now;//如果没有完成时间，默认当前系统时间

                    }

                    DataRow[] dtflowdr = dtflow.Select("flowid = '" + dr["flowid"].ToString() + "'");//根据事件流程编号，查询处理时限

                    if (dtflowdr != null && dtflowdr.Length > 0)
                        limitime = Convert.ToDateTime(dtflowdr[0]["limittime"].ToString());//获取处理时限
                    else
                        limitime = DateTime.Now;

                    //派出时间与设置处理时限比较

                    int zy = limitime.CompareTo(FinishedTime);
                    if (zy >= 0)
                        oknum++;
                }

                return oknum;

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

        #region 计算响应达标率的事件
        /// <summary>
        /// 获取在规定的响应时间内完成的事件数

        /// </summary>
        /// <param name="strID">工程师ID</param>
        /// <param name="strBeginDate"></param>
        /// <param name="strEndDate"></param>
        /// <param name="lngStatusID"></param>
        /// <param name="lngMastCustomer"></param>
        /// <returns></returns>
        public int Get_TimeLimit_OKnum(object strID, string strBeginDate, string strEndDate, long lngStatusID, long lngMastCustomer)
        {
            decimal Id = 0;

            Id = Convert.ToDecimal(strID);
            string feedback = "";
            Double feedbacksum = 0.0f;
            Double flownum = 0.0f;
            Double jsnum = 0.0f;
            OracleConnection cn = ConfigTool.GetConnection();

            string sWhere = string.Empty;
            if (strBeginDate != "")
                sWhere += " and  regsysdate >= to_date(" + StringTool.SqlQ(strBeginDate) + ",'yyyy-MM-dd   hh24:mi:ss')";
            if (strEndDate != "")
                sWhere += " and  regsysdate <= to_date(" + StringTool.SqlQ(strEndDate) + ",'yyyy-MM-dd   hh24:mi:ss')";

            sWhere += ((lngStatusID == 0) || (lngStatusID == -1) ? "" : " AND dealstatusid =" + lngStatusID.ToString());


            string sql2 = "";
            if (lngMastCustomer != 0)
            {
                sql2 = string.Format("select cis.*  from Cst_Issues cis where cis.flowid in ( select cl.flowid from Cst_ServiceStaff a inner join cst_servicestafflist cl" +
                   " on a.id = cl.servicestaffid where a.id='" + strID + "' And BlongDeptId=" + lngMastCustomer + ") " + sWhere + " ");
            }
            else
            {
                sql2 = string.Format("select cis.*  from Cst_Issues cis where cis.flowid in ( select cl.flowid from Cst_ServiceStaff a inner join cst_servicestafflist cl" +
                   " on a.id = cl.servicestaffid where a.id='" + strID + "')" + sWhere + "");
            }

            try
            {
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, sql2);//某个工程师在某个时间段内的所有事件


                int oknum = 0;//合格的事件数
                string flowids = string.Empty;//流程ID
                foreach (DataRow dr in dt.Rows)
                {
                    if (!string.IsNullOrEmpty(dr["flowid"].ToString()))
                    {
                        flowids = flowids + dr["flowid"].ToString() + ",";
                    }
                }

                flowids = flowids.Trim().Substring(0, flowids.Length - 1);//

                string strsql = string.Format("select efl.* from ea_flowbuslimit efl where efl.flowid in (" + flowids + ") and efl.guidid = 10002");
                DataTable dtflow = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strsql);//每个事件对应的响应时间集合


                DateTime outputdate = DateTime.Now;//派出时间——初始化当前系统时间
                DateTime limitime = DateTime.Now;//设置的响应时间

                foreach (DataRow dr in dt.Rows)
                {
                    if (!string.IsNullOrEmpty(dr["Outtime"].ToString()))
                    {
                        outputdate = Convert.ToDateTime(dr["Outtime"].ToString());//读取派出时间
                    }
                    else
                    {
                        outputdate = DateTime.Now;//如果没有派出时间，默认为当前系统时间
                    }

                    DataRow[] dtflowdr = dtflow.Select("flowid = '" + dr["flowid"].ToString() + "'");//根据事件流程编号，查询响应时间

                    if (dtflowdr != null && dtflowdr.Length > 0)
                        limitime = Convert.ToDateTime(dtflowdr[0]["limittime"].ToString());//得到设置的响应时间

                    else
                        limitime = DateTime.Now;

                    //派出时间与设置响应时间比较

                    int zy = limitime.CompareTo(outputdate);
                    if (zy >= 0)
                        oknum++;
                }

                return oknum;

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

        #region 事件首次解决率


        public static DataTable GetFirstSolve(DateTime startTime, DateTime endTime, int deptId)
        {
            DataTable dt = null;
            OracleParameter[] parms = {
                      new OracleParameter("P_STARTTIME",OracleType.DateTime),
                      new OracleParameter("P_ENDTIME",OracleType.DateTime),    
                       new OracleParameter("p_DeptID",OracleType.Int32),    
                      new OracleParameter("P_OUTTABLE",OracleType.Cursor)
                };

            parms[0].Direction = ParameterDirection.Input;
            parms[1].Direction = ParameterDirection.Input;
            parms[2].Direction = ParameterDirection.Input;
            parms[3].Direction = ParameterDirection.Output;

            parms[0].Value = startTime;
            parms[1].Value = endTime;
            parms[2].Value = deptId;

            OracleConnection cn = ConfigTool.GetConnection();
            if (cn.State != ConnectionState.Open)
            {
                cn.Open();
            }
            try
            {
                dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.StoredProcedure, "PROC_RPT_FIRST_SOLVE", parms);
            }
            catch (Exception ex)
            {
                E8Logger.Error(ex);
                throw;
            }
            finally
            {
                ConfigTool.CloseConnection(cn);
            }

            return dt;
        }


        #endregion

        #region MyRegion

        public static DataTable GetDailyCount(DateTime startTime, DateTime endTime, int deptId, int custId)
        {
            DataTable dt = null;
            OracleParameter[] parms = {
                      new OracleParameter("P_STARTTIME",OracleType.DateTime),
                      new OracleParameter("P_ENDTIME",OracleType.DateTime),    
                      new OracleParameter("P_ORGID",OracleType.Int32),    
                      new OracleParameter("P_CUSTID",OracleType.Int32),    
                      new OracleParameter("P_OUTTABLE",OracleType.Cursor)
                };

            parms[0].Direction = ParameterDirection.Input;
            parms[1].Direction = ParameterDirection.Input;
            parms[2].Direction = ParameterDirection.Input;
            parms[3].Direction = ParameterDirection.Input;
            parms[4].Direction = ParameterDirection.Output;

            parms[0].Value = startTime;
            parms[1].Value = endTime;
            parms[2].Value = deptId;
            parms[3].Value = custId;

            OracleConnection cn = ConfigTool.GetConnection();
            if (cn.State != ConnectionState.Open)
            {
                cn.Open();
            }
            try
            {
                dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.StoredProcedure, "PROC_RPT_DIALYCOUNT", parms);
            }
            catch (Exception ex)
            {
                E8Logger.Error(ex);
                throw;
            }
            finally
            {
                ConfigTool.CloseConnection(cn);
            }

            return dt;
        }

        #endregion


        #region 需求管理 - 服务量日报表 - 2013-05-03 @孙绍棕

        /// <summary>
        /// 服务量日报表
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="deptId">类别编号</param>
        /// <param name="custId">客户编号</param>
        /// <returns></returns>
        public static DataTable GetDailySummaryForReqDemand(
            DateTime startTime, 
            DateTime endTime, 
            int deptId, 
            int custId)
        {
            DataTable dt = null;
            OracleParameter[] parms = {
                      new OracleParameter("P_STARTTIME",OracleType.DateTime),
                      new OracleParameter("P_ENDTIME",OracleType.DateTime),    
                      new OracleParameter("P_ORGID",OracleType.Int32),    
                      new OracleParameter("P_CUSTID",OracleType.Int32),    
                      new OracleParameter("P_OUTTABLE",OracleType.Cursor)
                };

            parms[0].Direction = ParameterDirection.Input;
            parms[1].Direction = ParameterDirection.Input;
            parms[2].Direction = ParameterDirection.Input;
            parms[3].Direction = ParameterDirection.Input;
            parms[4].Direction = ParameterDirection.Output;

            parms[0].Value = startTime;
            parms[1].Value = endTime;
            parms[2].Value = deptId;
            parms[3].Value = custId;

            OracleConnection cn = ConfigTool.GetConnection();
            if (cn.State != ConnectionState.Open)
            {
                cn.Open();
            }
            try
            {
                dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.StoredProcedure, "PROC_RPT_ReqD_DIALYCOUNT", parms);
            }
            catch (Exception ex)
            {
                E8Logger.Error(ex);
                throw;
            }
            finally
            {
                ConfigTool.CloseConnection(cn);
            }

            return dt;
        }

        #endregion
    }
}
