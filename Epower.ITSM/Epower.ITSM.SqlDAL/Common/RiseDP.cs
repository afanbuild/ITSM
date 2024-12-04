/****************************************************************************
 * 
 * description:回访
 * 
 * 
 * 
 * Create by:
 * Create Date:
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
    /// RiseDP 的摘要说明。
    /// </summary>
    public class RiseDP
    {
        /// <summary>
        /// 服务事件处理类
        /// </summary>
        public RiseDP()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }



        /// <summary>
        /// 获取事件的年份
        /// </summary>
        /// <returns></returns>
        public static DataTable GetIssuesYears()
        {
            string sSql = "select distinct datepart(year,regsysdate) as years from ris_issues ";

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
        ///  获取客户满意度分析
        /// </summary>
        /// <param name="nYear"></param>
        /// <param name="lngCustID"></param>
        /// <param name="lngProductID"></param>
        /// <returns></returns>
        public static DataTable GetAnalysisStatisfied(int nYear, long lngCustID, long lngProductID)
        {
            string sSql = @"SELECT   '1月'=sum(case  when c.months=1 then c.sqtyRate else 0 end),
									'2月'= sum(case  when c.months=2 then c.sqtyRate else 0 end),
									'3月'=sum(case  when c.months=3 then c.sqtyRate else 0 end),
									'4月'=sum(case  when c.months=4 then c.sqtyRate else 0 end),
									'5月'=sum(case  when c.months=5 then c.sqtyRate else 0 end),
									'6月'=sum(case  when c.months=6 then c.sqtyRate else 0 end),
									'7月'=sum(case  when c.months=7 then c.sqtyRate else 0 end),
									'8月'=sum(case  when c.months=8 then c.sqtyRate else 0 end),
									'9月'=sum(case  when c.months=9 then c.sqtyRate else 0 end),
									'10月'=sum(case  when c.months=10 then c.sqtyRate else 0 end),
									'11月'=sum(case  when c.months=11 then c.sqtyRate else 0 end),
									'12月'=sum(case  when c.months=12 then c.sqtyRate else 0 end)
								FROM (
									SELECT month(b.RegSysDate) as months,to_number(sum(case when a.feedback = 1 then 1 else 0 end))
											/ (sum(case when a.feedback = 1 then 1 else 0 end) + sum(case when a.feedback = 1 then 0 else 1 end)) as sqtyRate 
									FROM EA_Issues_FeedBack a,ris_issues b
									WHERE a.flowid = b.flowid AND a.appid=300 AND year(b.regsysdate) = " + nYear.ToString() +
                                    ((lngCustID == 0) ? "" : "AND b.ecustomerid =" + lngCustID.ToString()) +
                                    ((lngProductID == 0) ? "" : "AND b.productid =" + lngProductID.ToString()) +
                                    @" GROUP BY month(b.RegSysDate) ) c";




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
        /// 获取客户满意度统计数据
        /// </summary>
        /// <param name="nYear"></param>
        /// <param name="lngCustID"></param>
        /// <param name="lngProductID"></param>
        /// <returns></returns>
        public static DataTable GetAnalysisStatisfiedGrid(int nYear, long lngCustID, long lngProductID)
        {
            string sSql = @"SELECT b.months as 月份,sum(b.fsqty) as 满意次数,sum(b.fqty) as 回访次数,
							count(b.flowid) as 事件数量,to_number(sum(b.fqty)) * 100 / count(b.flowid) as 回访率,
							to_number(sum(b.fsqty)) * 100 / sum(b.fqty) as 满意度
						FROM
						(
						SELECT month(a.RegSysDate) as months,flowid,
							(select count(flowid) from EA_Issues_FeedBack where flowid = a.flowid) as fqty,
								(select sum(case when feedback = 1 then 1 else 0 end) from EA_Issues_FeedBack where flowid = a.flowid) as fsqty
							FROM ris_issues a
							WHERE year(a.regsysdate) = " + nYear.ToString() +
                            ((lngCustID == 0) ? "" : "AND a.ecustomerid =" + lngCustID.ToString()) +
                            ((lngProductID == 0) ? "" : "AND a.productid =" + lngProductID.ToString()) +
                            @" ) b
							GROUP BY b.months";




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
        /// 获取客户服务处理及时率分析数据
        /// </summary>
        /// <param name="nYear"></param>
        /// <param name="lngCustID"></param>
        /// <param name="lngProductID"></param>
        /// <param name="lngRootID">事件类别的根节点</param>
        /// <returns></returns>
        public static DataTable GetAnalysisCSMOnTimeRate(int nYear, long lngCustID, long lngProductID, long lngRootID)
        {
            string strFullID = "";
            string sSql = "select fullid from es_catalog where catalogid = " + lngRootID.ToString();

            OracleConnection cn = ConfigTool.GetConnection();

            try
            {
                OracleDataReader dr = OracleDbHelper.ExecuteReader(cn, CommandType.Text, sSql);
                while (dr.Read())
                {
                    strFullID = dr.GetString(0);
                    break;
                }
                dr.Close();
            }
            catch (Exception ex) { ConfigTool.CloseConnection(cn); throw ex; }

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
								( SELECT month(a.regsysdate) as months,case when count(smsid) = 0 then 1 
																		else sum(case when b.endtime > nvl(b.expectendtime,b.endtime) then 0 
																				else 1 end) / to_number(count(smsid))  end as OnTimeRate FROM ris_issues a,es_flow b,es_catalog c 
								WHERE a.flowid = b.flowid AND a.smstype = c.catalogid AND year(a.regsysdate) = " + nYear.ToString() + ((strFullID.Length == 0) ? "" : " AND c.fullid like " + StringTool.SqlQ(strFullID + " % ")) +
                                ((lngCustID == 0) ? "" : "AND a.ecustomerid =" + lngCustID.ToString()) +
                                ((lngProductID == 0) ? "" : "AND a.productid =" + lngProductID.ToString()) +
                                @" AND not b.endtime is null

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
        /// 获取客户服务事件趋势数据
        /// </summary>
        /// <param name="nYear"></param>
        /// <returns></returns>
        public static DataTable GetAnalysisCSMTypeDirection(int nYear)
        {
            string sSql = @"select '1月'=sum(case  when a.months=1 then qty else 0 end),
							'2月'=sum(case  when a.months=2 then qty else 0 end),
							'3月'=sum(case  when a.months=3 then qty else 0 end),
							'4月'=sum(case  when a.months=4 then qty else 0 end),
							'5月'=sum(case  when a.months=5 then qty else 0 end),
							'6月'=sum(case  when a.months=6 then qty else 0 end),
							'7月'=sum(case  when a.months=7 then qty else 0 end),
							'8月'=sum(case  when a.months=8 then qty else 0 end),
							'9月'=sum(case  when a.months=9 then qty else 0 end),
							'10月'=sum(case  when a.months=10 then qty else 0 end),
							'11月'=sum(case  when a.months=11 then qty else 0 end),
							'12月'=sum(case  when a.months=12 then qty else 0 end)
					from ( SELECT month(regsysdate) as months,count(smsid) as qty" +
                        " FROM ris_issues " +
                        " WHERE year(regsysdate) = " + nYear.ToString() +
                        " GROUP BY month(regsysdate) ) a ";




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
        /// 获取客户服务事件趋势数据
        /// </summary>
        /// <param name="nYear"></param>
        /// <returns></returns>
        public static DataTable GetAnalysisCSMTypeDirectionGrid(int nYear)
        {
            string sSql = @"SELECT month(regsysdate) as months,count(smsid) as qty" +
                " FROM ris_issues " +
                " WHERE year(regsysdate) = " + nYear.ToString() +
                " GROUP BY month(regsysdate) ";




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
        /// 获取客户服务事件趋势数据(按类别)
        /// </summary>
        /// <param name="nYear"></param>
        /// <param name="lngRootID">分析的起始节点</param>
        /// <param name="lngCustID"></param>
        /// <param name="lngProductID"></param>
        /// <returns></returns>
        public static DataTable GetAnalysisCSMTypeDirectionTypes(int nYear, long lngRootID, long lngCustID, long lngProductID)
        {
            int iLen = 12;
            string strFullID = "";
            string sSql = "select fullid,len(fullid) from es_catalog where catalogid = " + lngRootID.ToString();

            OracleConnection cn = ConfigTool.GetConnection();
            try
            {
                OracleDataReader dr = OracleDbHelper.ExecuteReader(cn, CommandType.Text, sSql);
                while (dr.Read())
                {
                    strFullID = dr.GetString(0);
                    iLen = dr.GetInt32(1) + 6;
                    break;
                }
                dr.Close();
            }
            catch (Exception ex) { ConfigTool.CloseConnection(cn); throw ex; }


            sSql = @"SELECT	d.catalogname as 事件类别,
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

						SELECT b.catalogname,c.months,c.qty FROM 

						(SELECT month(a.regsysdate) as months,rtrim(substring(nvl(b.fullid,'') + replicate(' '," + iLen.ToString() + "),1," + iLen.ToString() + @")) as fullid,count(a.smsid) as qty 
						FROM ris_issues a,es_catalog b

						WHERE a.smstype = b.catalogid AND  year(a.regsysdate) = " + nYear.ToString() + ((strFullID.Length == 0) ? "" : " AND b.fullid like " + StringTool.SqlQ(strFullID + " % ")) +
                        ((lngCustID == 0) ? "" : "AND a.ecustomerid =" + lngCustID.ToString()) +
                        ((lngProductID == 0) ? "" : "AND a.productid =" + lngProductID.ToString()) +
                        @" GROUP BY month(a.regsysdate),rtrim(substring(nvl(b.fullid,'') + replicate(' '," + iLen.ToString() + "),1," + iLen.ToString() + @")) ) c,
                       es_catalog b 	WHERE c.fullid = b.fullid 	) d 	GROUP BY d.catalogname";

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
        /// 各客户事件发生趋势分析
        /// </summary>
        /// <param name="nYear"></param>
        /// <param name="lngRootID"></param>
        /// <param name="lngCustID"></param>
        /// <param name="lngProductID"></param>
        /// <returns></returns>
        public static DataTable GetAnalysisCSMCustDirectionTypes(int nYear, long lngRootID, long lngCustID, long lngProductID)
        {
            string strFullID = "";
            string sSql = "select fullid from es_catalog where catalogid = " + lngRootID.ToString();

            OracleConnection cn = ConfigTool.GetConnection();
            try
            {
                OracleDataReader dr = OracleDbHelper.ExecuteReader(cn, CommandType.Text, sSql);
                while (dr.Read())
                {
                    strFullID = dr.GetString(0);
                    break;
                }
                dr.Close();
            }
            catch (Exception ex) { ConfigTool.CloseConnection(cn); throw ex; }

            sSql = @"SELECT	d.fullname as 客户名称,
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

						SELECT nvl(b.fullname,'--') as fullname,c.months,c.qty FROM 

						(SELECT month(a.regsysdate) as months,a.ecustomerid ,count(a.smsid) as qty 
						FROM ris_issues a,es_catalog b

						WHERE a.smstype = b.catalogid AND  year(a.regsysdate) = " + nYear.ToString() + ((strFullID.Length == 0) ? "" : " AND b.fullid like " + StringTool.SqlQ(strFullID + " % ")) +
                ((lngCustID == 0) ? "" : "AND a.ecustomerid =" + lngCustID.ToString()) +
                ((lngProductID == 0) ? "" : "AND a.productid =" + lngProductID.ToString()) +
                @" GROUP BY month(a.regsysdate),a.ecustomerid ) c,
                       br_ecustomer b 	WHERE c.ecustomerid *= b.ecustomerid 	) d 	GROUP BY d.fullname";

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
        /// 各类产品事件发生趋势分析
        /// </summary>
        /// <param name="nYear"></param>
        /// <param name="lngRootID"></param>
        /// <param name="lngCustID"></param>
        /// <param name="lngProductID"></param>
        /// <returns></returns>
        public static DataTable GetAnalysisCSMProductDirectionTypes(int nYear, long lngRootID, long lngCustID, long lngProductID)
        {
            string strFullID = "";
            string sSql = "select fullid from es_catalog where catalogid = " + lngRootID.ToString();

            OracleConnection cn = ConfigTool.GetConnection();
            try
            {
                OracleDataReader dr = OracleDbHelper.ExecuteReader(cn, CommandType.Text, sSql);
                while (dr.Read())
                {
                    strFullID = dr.GetString(0);
                    break;
                }
                dr.Close();
            }
            catch (Exception ex) { ConfigTool.CloseConnection(cn); throw ex; }

            sSql = @"SELECT	d.productname as 产品名称,
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

						SELECT nvl(b.productname,'--') as productname,c.months,c.qty FROM 

						(SELECT month(a.regsysdate) as months,a.productid ,count(a.smsid) as qty 
						FROM ris_issues a,es_catalog b

						WHERE a.smstype = b.catalogid AND  year(a.regsysdate) = " + nYear.ToString() + ((strFullID.Length == 0) ? "" : " AND b.fullid like " + StringTool.SqlQ(strFullID + " % ")) +
                ((lngCustID == 0) ? "" : "AND a.ecustomerid =" + lngCustID.ToString()) +
                ((lngProductID == 0) ? "" : "AND a.productid =" + lngProductID.ToString()) +
                @" GROUP BY month(a.regsysdate),a.productid ) c,
                       br_product b 	WHERE c.productid *= b.productid 	) d 	GROUP BY d.productname";

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
        /// 获取客户服务事件趋势数据(按类别)
        /// </summary>
        /// <param name="nYear"></param>
        /// <param name="lngRootID">分析的起始节点</param>
        /// <returns></returns>
        public static DataTable GetAnalysisCSMTypeDirectionTypes(int nYear, long lngRootID)
        {
            return GetAnalysisCSMTypeDirectionTypes(nYear, lngRootID, 0, 0);
        }





        /// <summary>
        /// 保存督办意见资料
        /// </summary>
        /// <param name="lngFlowID"></param>
        /// <param name="lngUserID"></param>
        /// <param name="strSuggest"></param>
        /// <param name="strUserName"></param>
        public static void AddMonitorSuggest(long lngFlowID, long lngUserID, string strSuggest, string strUserName)
        {

            string strSQL = "";

            long lngSMSID = 0;

            //流程ID为0 退出
            if (lngFlowID == 0)
                return;

            OracleConnection cn = ConfigTool.GetConnection();

            try
            {
                strSQL = "SELECT smsid FROM ris_issues WHERE flowid =" + lngFlowID.ToString();

                OracleDataReader dr = OracleDbHelper.ExecuteReader(cn, CommandType.Text, strSQL);
                while (dr.Read())
                {
                    lngSMSID = (long)dr.GetDecimal(0);
                    break;
                }
                dr.Close();
            }
            catch (Exception ex) { ConfigTool.CloseConnection(cn); throw ex; }

            try
            {
                if (lngSMSID != 0)
                {

                    strSQL = "INSERT INTO ris_issues_Mon (smsID,processsysdate,suggest,douser,dousername)" +
                        " VALUES( " +
                        lngSMSID.ToString() + ",sysdate," +
                        StringTool.SqlQ(strSuggest) + "," +
                        lngUserID.ToString() + "," +
                        StringTool.SqlQ(strUserName) +
                        ")";
                    OracleDbHelper.ExecuteNonQuery(cn, CommandType.Text, strSQL);
                }                
            }
            finally { ConfigTool.CloseConnection(cn); }
        }

        /// <summary>
        ///  添加回访信息
        /// </summary>
        /// <param name="lngFlowID"></param>
        /// <param name="lngUserID"></param>
        /// <param name="strSuggest"></param>
        /// <param name="strUserName"></param>
        /// <param name="iFeedBack">满意程度选项</param>
        public static void AddFeedBack(long lngFlowID, long lngUserID, string strSuggest, string strUserName, int iFeedBack, long lngAppID, string strFeedPerson, string strCustName, int iFeedType, string strFBTime)
        {

            string strSQL = "";

            long lngSMSID = 0;

            //流程ID为0 退出
            if (lngFlowID == 0)
                return;

            OracleConnection cn = ConfigTool.GetConnection();
            try
            {
                string strID = EpowerGlobal.EPGlobal.GetNextID("EA_Issues_FeedBack_SEQUENCE").ToString();

                strSQL = "INSERT INTO EA_Issues_FeedBack (FeedBackID,FlowID,processsysdate,suggest,douser,dousername,appid,feedperson,custname,feedtype,fbtime,feedback)" +
                  " VALUES(" + strID + ", " +
                  lngFlowID.ToString() + ",sysdate," +
                  StringTool.SqlQ(strSuggest) + "," +
                  lngUserID.ToString() + "," +
                  StringTool.SqlQ(strUserName) + "," +
                  lngAppID.ToString() + "," +
                  StringTool.SqlQ(strFeedPerson) + "," +
                  StringTool.SqlQ(strCustName) + "," +
                  iFeedType.ToString() + "," +
                  "to_date(" + StringTool.SqlQ(strFBTime) + ",'yyyy-MM-dd HH24:mi:ss')," +
                  iFeedBack.ToString() +
                  ")";
                OracleDbHelper.ExecuteNonQuery(cn, CommandType.Text, strSQL);
            }
            finally { ConfigTool.CloseConnection(cn); }            
        }


        /// <summary>
        /// 删除回访情况
        /// </summary>
        /// <param name="lngID"></param>
        public static void DeleteFeedBack(long lngID)
        {

            string strSQL = "";



            OracleConnection cn = ConfigTool.GetConnection();


            try
            {
                strSQL = "DELETE EA_Issues_FeedBack WHERE feedbackid =" + lngID.ToString();

                OracleDbHelper.ExecuteNonQuery(cn, CommandType.Text, strSQL);                
            }
            finally { ConfigTool.CloseConnection(cn); }
        }



        /// <summary>
        /// 删除督办意见资料
        /// </summary>
        /// <param name="lngMonID"></param>
        public static void DeleteMonitorSuggest(long lngMonID)
        {

            string strSQL = "";



            OracleConnection cn = ConfigTool.GetConnection();


            try
            {
                strSQL = "DELETE ris_issues_Mon WHERE monid =" + lngMonID.ToString();

                OracleDbHelper.ExecuteNonQuery(cn, CommandType.Text, strSQL);                
            }
            finally { ConfigTool.CloseConnection(cn); }
        }


        /// <summary>
        /// 获得会议通知处理的全过程意见
        /// </summary>
        /// <param name="LngFlowID">流程ID</param>
        /// <returns></returns>
        public static DataTable GetAllRespInfo(long LngFlowID)
        {
            string sSql = " SELECT a.*,c.nodename" +
                " FROM Ris_Issues_Resp a,Ris_Issues b,es_nodemodel c" +
                " WHERE a.SMSID = b.SMSID AND b.flowmodelid = c.flowmodelid AND b.nodemodelid = c.nodemodelid and b.FlowID=" + LngFlowID.ToString() +
                " ORDER BY processsysdate DESC";

            OracleConnection cn = ConfigTool.GetConnection();

            try
            {
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, sSql);
                
                return dt;
            }
            finally { ConfigTool.CloseConnection(cn); }
        }


        /// <summary>
        /// 获取所有督办意见的信息
        /// </summary>
        /// <param name="LngFlowID"></param>
        /// <returns></returns>
        public static DataTable GetAllMonInfo(long LngFlowID)
        {
            string sSql = " SELECT a.*" +
                " FROM Ris_Issues_Mon a,Ris_Issues b" +
                " WHERE a.SMSID = b.SMSID and b.FlowID=" + LngFlowID.ToString() +
                " ORDER BY processsysdate DESC";

            OracleConnection cn = ConfigTool.GetConnection();
            try
            {
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, sSql);
            
                return dt;
            }
            finally { ConfigTool.CloseConnection(cn); }
        }


        /// <summary>
        /// 获取所有回访的信息
        /// </summary>
        /// <param name="LngFlowID"></param>
        /// <returns></returns>
        public static DataTable GetAllFeedBack(long LngFlowID)
        {
            string sSql = " SELECT a.*" +
                " FROM EA_Issues_FeedBack a" +
                " WHERE a.FlowID=" + LngFlowID.ToString() +
                " ORDER BY processsysdate DESC";

            OracleConnection cn = ConfigTool.GetConnection();

            try
            {
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, sSql);
                
                return dt;
            }
            finally { ConfigTool.CloseConnection(cn); }
        }



        /// <summary>
        /// 获取待办的事件列表
        /// </summary>
        /// <param name="lngUserID"></param>
        /// <returns></returns>
        public static DataTable GetUnDoList(long lngUserID)
        {
            string sSql = " SELECT a.messageid,a.actortype,b.subject,b.regusername,b.regsysdate,b.fullsmstypename,b.fullcustname, c.nodename,datediff('Minute',sysdate,nvl(a.expected,sysdate)) as DiffMinute,datediff(Minute,sysdate,nvl(d.expectendtime,sysdate)) as FlowDiffMinute " +
                " FROM es_message a,ris_issues b,es_nodemodel c,es_flow d " +
                " WHERE a.flowid = d.flowid and a.flowid = b.flowid and b.flowmodelid = c.flowmodelid  " +
                " and b.nodemodelid = c.nodemodelid and a.receiverid = " + lngUserID.ToString() +
                " and a.status = " + ((int)e_MessageStatus.emsHandle).ToString() + " ORDER BY a.receivetime DESC";

            OracleConnection cn = ConfigTool.GetConnection();

            try
            {
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, sSql);
            
                return dt;
            }
            finally { ConfigTool.CloseConnection(cn); }
        }

        /// <summary>
        /// 获取用户未接收列表
        /// </summary>
        /// <param name="lngUserID"></param>
        /// <returns></returns>
        public static DataTable GetReceiveMessageList(long lngUserID)
        {

            string sSql = " SELECT d.ID,a.messageid,a.actortype,b.subject,b.regusername,b.regsysdate,b.fullsmstypename,b.fullcustname, c.nodename,datediff('Minute',sysdate,nvl(a.expected,sysdate)) as DiffMinute,datediff(Minute,sysdate,nvl(e.expectendtime,sysdate)) as FlowDiffMinute " +
                " FROM es_message a,ris_issues b,es_nodemodel c,Es_ReceiveList d,es_flow e " +
                " WHERE d.MessageID = a.MessageID AND a.flowid = e.flowid AND a.flowid = b.flowid and b.flowmodelid = c.flowmodelid  " +
                " and b.nodemodelid = c.nodemodelid and d.receiveid = " + lngUserID.ToString() +
                " AND a.Deleted =" + (int)e_Deleted.eNormal +
                " AND a.ReceiverID = 0 " +
                " ORDER BY a.MessageID DESC";

            OracleConnection cn = ConfigTool.GetConnection();

            try
            {
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, sSql);
                
                return dt;
            }
            finally { ConfigTool.CloseConnection(cn); }

        }

        #region 通用查询数据访问方法
        /// <summary>
        /// 获取流程数据
        /// </summary>
        /// <param name="strCondi"></param>
        /// <returns></returns>
        public static DataTable GetFlowModelInfo(string strCondi)
        {
            string strSql = "select * from Es_FlowModel where deleted = 0 " + strCondi + " order by FlowName ";
            OracleConnection con = ConfigTool.GetConnection();
            try
            {
                return OracleDbHelper.ExecuteDataTable(con, CommandType.Text, strSql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                ConfigTool.CloseConnection(con);
            }
        }


        /// <summary>
        /// 获取流程自定义字段信息
        /// </summary>
        /// <param name="strCondi"></param>
        /// <returns></returns>
        public static DataTable GetFieldsInfo(string strCondi)
        {
            string strSql = "select * from Es_FMFields  Where 1=1 " + strCondi;
            OracleConnection con = ConfigTool.GetConnection();
            try
            {
                return OracleDbHelper.ExecuteDataTable(con, CommandType.Text, strSql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                ConfigTool.CloseConnection(con);
            }
        }


        /// <summary>
        /// 获取流程信息
        /// </summary>
        /// <param name="fmID"></param>
        /// <param name="strCondi"></param>
        /// <returns></returns>
        public static DataTable GetAppInfo(long fmID, string strCondi)
        {
            string strSql = "select distinct a.FlowName,a.DeptName,a.ApplyName,a.StartDate,a.EndDate," +
                    "(select NodeName From Es_NodeModel where a.FlowModelID = Es_NodeModel.FlowModelID " +
                    " and a.FlowStatus = Es_NodeModel.NodeModelID) NodeName " +
                    " from App_pub_Normal_Head a where a.FlowModelID = " + fmID.ToString().Trim() +
                    strCondi.Trim();
            OracleConnection con = ConfigTool.GetConnection();
            try
            {
                return OracleDbHelper.ExecuteDataTable(con, CommandType.Text, strSql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                ConfigTool.CloseConnection(con);
            }
        }
        #endregion

        #region 获取满意度服务监控图表
        /// <summary>
        /// 获取满意度服务监控图表
        /// </summary>
        /// <param name="strMastCust"></param>
        /// <returns></returns>
        public static DataTable GetSM()
        {
            string strSql = @"--满意
                            select  '满意' as Title,count(1) as counts,'FeedBackA' as Types
                              from EA_Issues_FeedBack A,Cst_Issues B
                             where	A.FlowID = B.FlowID and 
	                             feedback = 1 and to_char(FBTime,'yyyy') = to_char(sysdate,'yyyy')
                            union all
                            --基本满意
                            select  '基本满意' as Title,count(1) as counts,'FeedBackB' as Types
                              from EA_Issues_FeedBack A,Cst_Issues B
                             where	A.FlowID = B.FlowID and 
	                             feedback = 2 and to_char(FBTime,'yyyy') = to_char(sysdate,'yyyy')
                            union all
                            --不满意
                            select  '不满意' as Title,count(1) as counts,'FeedBackC' as Types
                              from EA_Issues_FeedBack A,Cst_Issues B
                             where	A.FlowID = B.FlowID and 
	                             feedback = 3 and to_char(FBTime,'yyyy') = to_char(sysdate,'yyyy')";

            DataTable dt = CommonDP.ExcuteSqlTable(strSql);

            return dt;
        }
        #endregion

        #region 满意度，柱子点开查看详情
        /// <summary>
        /// 满意度，柱子点开查看详情
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

            try
            {
                DataTable dt = new DataTable();
                string strWhere = " 1=1 ";
                string strSql = string.Empty;

                if (strType == "FeedBackA")
                {
                    //当月新增
                    dt = OracleDbHelper.ExecuteDataTable(cn, "v_FeedBackA", "*", " ORDER BY smsID DESC", pagesize, pageindex, strWhere, ref rowcount);
                }
                else if (strType == "FeedBackB")
                {
                    //当月变更中
                    dt = OracleDbHelper.ExecuteDataTable(cn, "v_FeedBackB", "*", " ORDER BY smsID DESC", pagesize, pageindex, strWhere, ref rowcount);
                }
                else if (strType == "FeedBackC")
                {
                    //当月已变更
                    dt = OracleDbHelper.ExecuteDataTable(cn, "v_FeedBackC", "*", " ORDER BY smsID DESC", pagesize, pageindex, strWhere, ref rowcount);
                }
                else
                {
                    strSql = @"--无记录
                                select A.*
                                  from Cst_Issues A where 1 = 2";
                }

                return dt;
            }
            finally { ConfigTool.CloseConnection(cn); }
        }
        #endregion
    }
}
