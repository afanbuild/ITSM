/*******************************************************************
 *
 * Description:通用流程配置
 * 
 * 
 * Create By  :
 * Create Date:2008年7月30日
 * *****************************************************************/
using System;
using System.Data;
using System.Data.OracleClient;
using Epower.DevBase.BaseTools;
using EpowerCom;
using System.Collections.Generic;

namespace Epower.ITSM.SqlDAL
{
    /// <summary>
    /// AppFieldConfigDP 的摘要说明。
    /// </summary>
    public class AppFieldConfigDP
    {
        public AppFieldConfigDP()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static DataTable GetFlowModelList()
        {
            string sSql = "SELECT FlowModelID,oFlowModelID,FlowName FROM ES_FlowModel " +
                        "WHERE Deleted=0 and status=1 AND appid = 199";
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
        ///  获取流程模型列表,ID为OFlowModelID
        /// </summary>
        /// <param name="lngAppID"></param>
        /// <param name="lngUserID"></param>
        /// <returns></returns>
        public static DataTable GetFlowModelListForOID(long lngAppID, long lngUserID)
        {
            string sSql = "SELECT OFlowModelID,FlowModelID,FlowName FROM ES_FlowModel " +
                        "WHERE Deleted=0 and status=1 AND appid = " + lngAppID.ToString();

            OracleConnection cn = ConfigTool.GetConnection();
            try
            {
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, sSql);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    long lngFlowModelID = long.Parse(dt.Rows[i]["FlowModelID"].ToString());
                    if (FlowModel.CanUseFlowModel(lngUserID, lngFlowModelID) != 0)
                    {
                        dt.Rows[i].Delete();
                    }
                }
                dt.AcceptChanges();
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
        ///  获取流程模型列表,ID为OFlowModelID
        /// </summary>
        /// <param name="lngAppID"></param>
        /// <param name="lngUserID"></param>
        /// <returns></returns>
        public static DataTable GetFlowModelList(long lngAppID, long lngUserID, long lngOwner)
        {
            string strSQL = string.Empty;

            long lngViewID = 0;

            #region 根据lngAppID和lngOwner 查询起草环节绑定对应操作视图的ID 余向前 2013-04-18
            switch (lngAppID)
            {
                case 1026: //事件管理
                    if (lngOwner == 0)
                        lngViewID = 9351;
                    else if (lngOwner == 3)
                        lngViewID = 9353;
                    break;
                case 1062: //需求管理
                    if (lngOwner == 0)
                        lngViewID = 9711;
                    else if (lngOwner == 3)
                        lngViewID = 9712;
                    break;
                default:
                    break;
            }
            #endregion


            strSQL = @"SELECT a.OFlowModelID,a.FlowModelID,a.FlowName
                            FROM ES_FlowModel a
                           INNER JOIN (SELECT Es_NodeModel.FlowModelID, ES_AppOp.OpID
                           FROM Es_NodeModel
                           LEFT JOIN ES_AppOp ON Es_NodeModel.OpID = ES_AppOp.OpID
                           WHERE NodeModelID = 2
                           AND FlowModelID IN (SELECT FlowModelID
                                      FROM es_flowmodel
                                     WHERE DELETED = 0
                                       AND Status = 1
                                       AND AppID = " + lngAppID + @")) b ON a.FlowModelID =
                                                               b.FlowModelID
                           WHERE b.OpID =" + lngViewID;

            OracleConnection cn = ConfigTool.GetConnection();
            try
            {
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    long lngFlowModelID = long.Parse(dt.Rows[i]["FlowModelID"].ToString());
                    if (FlowModel.CanUseFlowModel(lngUserID, lngFlowModelID) != 0)
                    {
                        dt.Rows[i].Delete();
                    }
                }
                dt.AcceptChanges();
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
        /// 检查流程模型是否改变
        /// </summary>
        /// <param name="lngOFlowModelId"></param>
        public static bool CheckFlowModel(long lngOFlowModelId, long lngOpID)
        {
            string strSQL = string.Empty;
            strSQL = @"SELECT COUNT(b.OpID) aCount
                        FROM ES_FlowModel a
                        INNER JOIN (SELECT Es_NodeModel.FlowModelID, ES_AppOp.OpID
                                      FROM Es_NodeModel
                                      LEFT JOIN ES_AppOp ON Es_NodeModel.OpID = ES_AppOp.OpID
                                     WHERE NodeModelID = 2
                                       AND FlowModelID IN (SELECT FlowModelID
                                                             FROM es_flowmodel
                                                            WHERE DELETED = 0
                                                              AND Status = 1
                                                              AND OFlowModelID = " + lngOFlowModelId + @")) b ON a.FlowModelID =
                                                                                       b.FlowModelID
                        AND b.OpID=" + lngOpID;
            OracleConnection cn = ConfigTool.GetConnection();
            try
            {
                OracleDataReader reader = OracleDbHelper.ExecuteReader(cn, CommandType.Text, strSQL);
                if (reader.Read())
                {
                    string aCount = reader["aCount"].ToString();
                    if (aCount == "1")
                        return true;
                    return false;
                }
                reader.Close();
                return false;
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
        /// <returns></returns>
        public static long GetOFlowModelID(long lngFlowModelID)
        {
            long lngOFlowModelID = 0;
            string sSql = "SELECT oFlowModelID FROM ES_FlowModel " +
                        "WHERE Deleted=0 and status=1 AND appid = 199 And FlowModelID=" + lngFlowModelID.ToString();
            OracleConnection cn = ConfigTool.GetConnection();
            try
            {
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, sSql);
                if (dt.Rows.Count > 0)
                    lngOFlowModelID = long.Parse(dt.Rows[0]["oFlowModelID"].ToString());
                return lngOFlowModelID;
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
        /// <param name="sFlowModelID"></param>
        /// <returns></returns>
        public static DataTable GetFieldsConfig(string sFlowModelID)
        {
            if (sFlowModelID == "0" || sFlowModelID == "")
                return null;
            //string sSql="Select * from Es_FMFields Where FlowModelID="+sFlowModelID;
            string sSql = "SELECT * FROM es_fmfields WHERE flowmodelid IN " +
                "(SELECT flowmodelid FROM es_flowmodel WHERE oflowmodelid = " +
                " (SELECT oflowmodelid FROM es_flowmodel WHERE flowmodelid = " + sFlowModelID + ")) " +
                " ORDER BY flowmodelid DESC ";
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

        public static void DeleteFiledConfig(string sFlowModelID)
        {
            string sSql = "delete Es_FMFields Where FlowModelID=" + sFlowModelID;
            string scn = ConfigTool.GetConnectString();
            try
            {
                OracleDbHelper.ExecuteNonQuery(scn, CommandType.Text, sSql);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sFlowModelID"></param>
        /// <param name="sDate1"></param>
        /// <param name="sDate2"></param>
        /// <param name="sDate3"></param>
        /// <param name="sDate4"></param>
        /// <param name="string1"></param>
        /// <param name="string2"></param>
        /// <param name="string3"></param>
        /// <param name="string4"></param>
        /// <param name="string5"></param>
        /// <param name="string6"></param>
        /// <param name="string7"></param>
        /// <param name="string8"></param>
        /// <param name="snum1"></param>
        /// <param name="snum2"></param>
        /// <param name="snum3"></param>
        /// <param name="snum4"></param>
        /// <param name="snum5"></param>
        /// <param name="scata1"></param>
        /// <param name="scata2"></param>
        /// <param name="scata3"></param>
        /// <param name="scata4"></param>
        /// <param name="scata5"></param>
        /// <param name="sbool1"></param>
        /// <param name="sbool2"></param>
        /// <param name="sbool3"></param>
        /// <param name="sbool4"></param>
        /// <param name="sremark1"></param>
        /// <param name="sFBox"></param>
        /// <param name="sdatev1"></param>
        /// <param name="sdate2v"></param>
        /// <param name="sdatev3"></param>
        /// <param name="sdate4v"></param>
        /// <param name="sstr1v"></param>
        /// <param name="sstr2v"></param>
        /// <param name="sstr3v"></param>
        /// <param name="sstr4v"></param>
        /// <param name="sstr5v"></param>
        /// <param name="sstr6v"></param>
        /// <param name="sstr7v"></param>
        /// <param name="sstr8v"></param>
        /// <param name="snum1v"></param>
        /// <param name="snum2v"></param>
        /// <param name="snum3v"></param>
        /// <param name="snum4v"></param>
        /// <param name="snum5v"></param>
        /// <param name="scata1v"></param>
        /// <param name="scata2v"></param>
        /// <param name="scata3v"></param>
        /// <param name="scata4v"></param>
        /// <param name="scata5v"></param>
        /// <param name="scata1rid"></param>
        /// <param name="scata2rid"></param>
        /// <param name="scata3rid"></param>
        /// <param name="scata4rid"></param>
        /// <param name="scata5rid"></param>
        /// <param name="sb1v"></param>
        /// <param name="sb2v"></param>
        /// <param name="sb3v"></param>
        /// <param name="sb4v"></param>
        /// <param name="sremark1v"></param>
        /// <param name="strQueryXml"></param>
        /// <param name="sDesc"></param>
        /// <param name="sPrintTitle"></param>
        /// <param name="sPrintBottom"></param>
        /// <param name="sDescv"></param>
        /// <param name="sDescControl"></param>
        /// <param name="sPrintTitlev"></param>
        /// <param name="sPrintBottomv"></param>
        /// <param name="sdate1m"></param>
        /// <param name="sdate2m"></param>
        /// <param name="sdate3m"></param>
        /// <param name="sdate4m"></param>
        /// <param name="sstr1m"></param>
        /// <param name="sstr2m"></param>
        /// <param name="sstr3m"></param>
        /// <param name="sstr4m"></param>
        /// <param name="sstr5m"></param>
        /// <param name="sstr6m"></param>
        /// <param name="sstr7m"></param>
        /// <param name="sstr8m"></param>
        /// <param name="snum1m"></param>
        /// <param name="snum2m"></param>
        /// <param name="snum3m"></param>
        /// <param name="snum4m"></param>
        /// <param name="snum5m"></param>
        /// <param name="scata1m"></param>
        /// <param name="scata2m"></param>
        /// <param name="scata3m"></param>
        /// <param name="scata4m"></param>
        /// <param name="scata5m"></param>
        /// <param name="remarkm"></param>
        /// <param name="sdate1s"></param>
        /// <param name="sdate2s"></param>
        /// <param name="sdate3s"></param>
        /// <param name="sdate4s"></param>
        public static void SaveFiledsConfig(string sFlowModelID, string sDate1, string sDate2, string sDate3, string sDate4,
            string sDate5, string sDate6, string sDate7, string sDate8,
            string string1, string string2, string string3, string string4, string string5, string string6, string string7, string string8,
            string snum1, string snum2, string snum3, string snum4, string snum5,
            string scata1, string scata2, string scata3, string scata4, string scata5,
            string sbool1, string sbool2, string sbool3, string sbool4,
            string sremark1, string sremark2, string sremark3, string sremark4, string sFBox,
            string sdatev1, string sdate2v, string sdatev3, string sdate4v,
            string sdatev5, string sdate6v, string sdatev7, string sdate8v,
            string sstr1v, string sstr2v, string sstr3v, string sstr4v, string sstr5v, string sstr6v, string sstr7v, string sstr8v,
            string snum1v, string snum2v, string snum3v, string snum4v, string snum5v,
            string scata1v, string scata2v, string scata3v, string scata4v, string scata5v,
            string scata1rid, string scata2rid, string scata3rid, string scata4rid, string scata5rid,
            string sb1v, string sb2v, string sb3v, string sb4v,
            string sremark1v, string sremark2v, string sremark3v, string sremark4v, string strQueryXml,
            string sDesc, string sPrintTitle, string sPrintBottom,
            string sDescv, string sDescControl, string sPrintTitlev, string sPrintBottomv,
            string sdate1m, string sdate2m, string sdate3m, string sdate4m,
            string sdate5m, string sdate6m, string sdate7m, string sdate8m,
            string sstr1m, string sstr2m, string sstr3m, string sstr4m, string sstr5m, string sstr6m, string sstr7m, string sstr8m,
            string snum1m, string snum2m, string snum3m, string snum4m, string snum5m,
            string scata1m, string scata2m, string scata3m, string scata4m, string scata5m,
            string remarkm, string remark2m, string remark3m, string remark4m,
            string sdate1s, string sdate2s, string sdate3s, string sdate4s,
            string sdate5s, string sdate6s, string sdate7s, string sdate8s
            )
        {
            OracleConnection cn = ConfigTool.GetConnection();
            cn.Open();
            OracleTransaction tran = cn.BeginTransaction();
            try
            {
                string sSql = "delete Es_FMFields Where FlowModelID=" + sFlowModelID;
                OracleDbHelper.ExecuteNonQuery(tran, CommandType.Text, sSql);

                sSql = @"Insert Into Es_FMFields(FlowModelID," +
                    "Date1,Date2,Date3,Date4," +
                    "Date5,Date6,Date7,Date8," +
                    "String1,String2,String3,String4,string5,string6,string7,string8," +
                    "Number1,Number2,Number3,Number4,Number5," +
                    "Cate1,Cate2,Cate3,Cate4,Cate5," +
                    "Bool1,Bool2,Bool3,Bool4," +
                    "Remark1,Remark2,Remark3,Remark4,FBox," +

                    "Date1Validate,Date2Validate,Date3Validate,Date4Validate," +
                    "Date5Validate,Date6Validate,Date7Validate,Date8Validate," +

                    "String1Validate,String2Validate,String3Validate,String4Validate,String5Validate,String6Validate,String7Validate,String8Validate," +
                    "Number1Validate,Number2Validate,Number3Validate,Number4Validate,Number5Validate," +
                    "Cate1Validate,Cate2Validate,Cate3Validate,Cate4Validate,Cate5Validate," +
                    "Cate1RootID,Cate2RootID,Cate3RootID,Cate4RootID,Cate5RootID," +
                    "Bool1Validate,Bool2Validate,Bool3Validate,Bool4Validate," +
                    "Remark1Validate,Remark2Validate,Remark3Validate,Remark4Validate,Queryxml,UpdateDate,DescValidate,DescControl,TitleValidate,BottomValidate," +

                    "Date1Must,Date2Must,Date3Must,Date4Must,Date5Must,Date6Must,Date7Must,Date8Must,String1Must,String2Must,String3Must,String4Must,String5Must,String6Must,String7Must,String8Must," +
                    "Number1Must,Number2Must,Number3Must,Number4Must,Number5Must,Cate1Must,Cate2Must,Cate3Must,Cate4Must,Cate5Must," +
                    "RemarkMust,Remark2Must,Remark3Must,Remark4Must," +
                    "Date1Show,Date2Show,Date3Show,Date4Show," +
                    "Date5Show,Date6Show,Date7Show,Date8Show" +
                    " ) values(" +
                    sFlowModelID + "," + StringTool.SqlQ(sDate1) + "," + StringTool.SqlQ(sDate2) + "," + StringTool.SqlQ(sDate3) + "," + StringTool.SqlQ(sDate4) + "," +
                    StringTool.SqlQ(sDate5) + "," + StringTool.SqlQ(sDate6) + "," + StringTool.SqlQ(sDate7) + "," + StringTool.SqlQ(sDate8) + "," +

                    StringTool.SqlQ(string1) + "," + StringTool.SqlQ(string2) + "," + StringTool.SqlQ(string3) + "," + StringTool.SqlQ(string4) + "," +
                    StringTool.SqlQ(string5) + "," + StringTool.SqlQ(string6) + "," + StringTool.SqlQ(string7) + "," + StringTool.SqlQ(string8) + "," +
                    StringTool.SqlQ(snum1) + "," + StringTool.SqlQ(snum2) + "," + StringTool.SqlQ(snum3) + "," + StringTool.SqlQ(snum4) + "," + StringTool.SqlQ(snum5) + "," +
                    StringTool.SqlQ(scata1) + "," + StringTool.SqlQ(scata2) + "," + StringTool.SqlQ(scata3) + "," + StringTool.SqlQ(scata4) + "," + StringTool.SqlQ(scata5) + "," +
                    StringTool.SqlQ(sbool1) + "," + StringTool.SqlQ(sbool2) + "," + StringTool.SqlQ(sbool3) + "," + StringTool.SqlQ(sbool4) + "," +

                    StringTool.SqlQ(sremark1) + "," + StringTool.SqlQ(sremark2) + "," + StringTool.SqlQ(sremark3) + "," + StringTool.SqlQ(sremark4) + "," + StringTool.SqlQ(sFBox) + "," +

                    sdatev1 + "," + sdate2v + "," + sdatev3 + "," + sdate4v + "," +
                    sdatev5 + "," + sdate6v + "," + sdatev7 + "," + sdate8v + "," +

                    sstr1v + "," + sstr2v + "," + sstr3v + "," + sstr4v + "," + sstr5v + "," + sstr6v + "," + sstr7v + "," + sstr8v + "," +
                    snum1v + "," + snum2v + "," + snum3v + "," + snum4v + "," + snum5v + "," +
                    scata1v + "," + scata2v + "," + scata3v + "," + scata4v + "," + scata5v + "," +
                    scata1rid + "," + scata2rid + "," + scata3rid + "," + scata4rid + "," + scata5rid + "," +

                    sb1v + "," + sb2v + "," + sb3v + "," + sb4v + "," + sremark1v + "," + sremark2v + "," + sremark3v + "," + sremark4v + "," + StringTool.SqlQ(strQueryXml) + ",sysdate" + "," +                    
                    sDescv + "," + sDescControl + "," + sPrintTitlev + "," + sPrintBottomv + "," +

                    sdate1m + "," + sdate2m + "," + sdate3m + "," + sdate4m + "," +
                    sdate5m + "," + sdate6m + "," + sdate7m + "," + sdate8m + "," +

                    sstr1m + "," + sstr2m + "," + sstr3m + "," + sstr4m + "," + sstr5m + "," +
                    sstr6m + "," + sstr7m + "," + sstr8m + "," + snum1m + "," + snum2m + "," + snum3m + "," + snum4m + "," + snum5m + "," +
                    scata1m + "," + scata2m + "," + scata3m + "," + scata4m + "," + scata5m + "," +
                    remarkm + "," + remark2m + "," + remark3m + "," + remark4m + "," +
                    sdate1s + "," + sdate2s + "," + sdate3s + "," + sdate4s + "," +
                    sdate5s + "," + sdate6s + "," + sdate7s + "," + sdate8s +
                    ")";

                OracleDbHelper.ExecuteNonQuery(tran, CommandType.Text, sSql);

                #region 解决字符超长出错 yxq 2014-05-19
                sSql = "update Es_FMFields set Description=:a,PrintTitle=:b,PrintBottom=:c where FlowModelID = " + sFlowModelID;
                OracleCommand cmdCST = new OracleCommand(sSql, tran.Connection, tran);
                cmdCST.Parameters.Add("a", OracleType.Clob).Value = sDesc;
                cmdCST.Parameters.Add("b", OracleType.Clob).Value = sPrintTitle;
                cmdCST.Parameters.Add("c", OracleType.Clob).Value = sPrintBottom;
                cmdCST.ExecuteNonQuery();
                #endregion
                
                tran.Commit();
            }
            catch (Exception e)
            {
                tran.Rollback();
                throw e;
            }
            finally
            {
                cn.Dispose();
                ConfigTool.CloseConnection(cn);
            }
        }


        #region 保存表单分组和字段排序信息 - 2013-11-22 @孙绍棕
        /// <summary>
        /// 保存表单分组和字段排序信息
        /// </summary>
        /// <param name="lngFlowModelID">流程模型编号</param>
        /// <param name="listField">分组和字段排序信息</param>
        public static void SaveFieldMenuAndOrderby(long lngFlowModelID, List<List<String>> listField)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            cn.Open();
            OracleTransaction trans = cn.BeginTransaction();
            try
            {
                String strSql = String.Format("DELETE FROM APP_PUB_NORMAL_FIELD WHERE FlowModelID = {0}", lngFlowModelID);
                OracleDbHelper.ExecuteNonQuery(trans, CommandType.Text, strSql);    // 移除旧的配置数据

                DateTime now = DateTime.Now;
                foreach (List<String> listValue in listField)
                {
                    strSql = String.Format(@"INSERT INTO APP_PUB_NORMAL_FIELD (FLOWMODELID, GroupID, FieldName, Orderby, AddTime)
                                                                               VALUES({0},{1},{2},{3},to_date('{4}','yyyy-MM-dd HH24:mi:ss'))",
                                                                               lngFlowModelID,
                                                                               listValue[1],    // 组编号
                                                                               StringTool.SqlQ(listValue[0]),    // 字段名
                                                                               listValue[2],    // 排序号
                                                                               now);

                    OracleDbHelper.ExecuteNonQuery(trans, CommandType.Text, strSql);
                }

                trans.Commit();
            }
            catch (Exception e)
            {
                trans.Rollback();
                throw e;
            }
            finally
            {
                ConfigTool.CloseConnection(cn);
            }
        }
        #endregion

        #region 取表单字段菜单的配置信息 - 2013-11-22 @孙绍棕
        /// <summary>
        /// 取表单字段菜单的配置信息
        /// </summary>
        /// <param name="lngFlowModelID">流程模型编号</param>
        /// <returns></returns>
        public static DataTable GetFieldMenuInfoByFlowModelID(long lngFlowModelID)
        {
            OracleConnection conn = ConfigTool.GetConnection();
            try
            {
                String strSql = String.Format("SELECT FLOWMODELID,GROUPID,FIELDNAME,ORDERBY FROM APP_PUB_NORMAL_FIELD WHERE FlowModelID = {0}", lngFlowModelID);
                DataTable dt = OracleDbHelper.ExecuteDataTable(conn, CommandType.Text, strSql);
                return dt;
            }
            finally
            {
                ConfigTool.CloseConnection(conn);
            }
        }
        #endregion



        #region 取已排序过的表单字段和菜单的配置信息 - 2013-11-25 @孙绍棕
        /// <summary>
        /// 取已排序过的表单字段和菜单的配置信息
        /// </summary>
        /// <param name="lngFlowModelID">流程模型编号</param>
        /// <returns></returns>
        public static DataTable GetOrderbyFieldAndMenu(long lngFlowModelID)
        {
            OracleConnection conn = ConfigTool.GetConnection();
            try
            {
                String strSql = String.Format(@"SELECT t1.groupid, t1.fieldname, t2.catalogname FROM APP_PUB_NORMAL_FIELD t1
                                                left join es_catalog t2
                                                on t1.groupid = t2.catalogid
                                                WHERE t1.flowmodelid = {0}
                                                order by t2.sortid asc, t1.orderby asc
                                                ", lngFlowModelID);

                DataTable dt = OracleDbHelper.ExecuteDataTable(conn, CommandType.Text, strSql);
                return dt;
            }
            finally
            {
                ConfigTool.CloseConnection(conn);
            }
        }
        #endregion


        #region 取字段名和显示名的集合 - 2013-11-27 @孙绍棕
        /// <summary>
        /// 取字段名和显示名的集合
        /// </summary>
        /// <param name="lngFlowModelID">流程模型编号</param>
        /// <returns></returns>
        public static List<KeyValuePair<String, String>> GetFieldNameAndDisplayName(long lngFlowModelID)
        {
            OracleConnection conn = ConfigTool.GetConnection();
            try
            {
                // # 取流程模型可用的字段名
                String strSql = String.Format(@"select fieldname from 
                                                app_pub_normal_field where flowmodelid = {0}
                                                ", lngFlowModelID);

                DataTable dtFieldName = OracleDbHelper.ExecuteDataTable(conn, CommandType.Text, strSql);

                strSql = String.Format(@"select * from es_fmfields where flowmodelid = {0}
                                                ", lngFlowModelID);

                // # 取流程模型可用字段的显示名
                DataTable dtDisplayName = OracleDbHelper.ExecuteDataTable(conn, CommandType.Text, strSql);

                // # 组装字段名和显示名, 并返回
                List<KeyValuePair<String, String>> listField = new List<KeyValuePair<string, string>>();
                foreach (DataRow drField in dtFieldName.Rows)
                {

                    //             if (fieldname.indexOf('cata') > -1) {
                    //    fieldname = fieldname.replace('cata','cate');
                    //} else if (fieldname.indexOf('num') > -1) {
                    //    fieldname = fieldname.replace('num','number');
                    //} else if (fieldname == 'remark') {
                    //    fieldname = 'remark1';
                    //}  
                    String strFieldName = drField["fieldname"].ToString();

                    if (strFieldName.Contains("num"))
                        strFieldName = strFieldName.Replace("num", "number");
                    else if (strFieldName.Contains("cata"))
                        strFieldName = strFieldName.Replace("cata", "cate");
                    else if (strFieldName == "remark")
                        strFieldName = strFieldName.Replace("remark", "remark1");


                    String strDisplayName = dtDisplayName.Rows[0][strFieldName].ToString();

                    listField.Add(new KeyValuePair<string, string>(strFieldName, strDisplayName));
                }

                return listField;
            }
            finally
            {
                ConfigTool.CloseConnection(conn);
            }
        }
        #endregion



    }


}
