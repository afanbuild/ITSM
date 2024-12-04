/*******************************************************************
 * 版权所有：深圳市非凡信息技术有限公司
 * 描述：流程自定义扩展项显示方式-数据访问代码
 * 
 * 
 * 创建人：孙绍棕
 * 创建日期：2013-12-04
 * *****************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.OracleClient;
using Epower.DevBase.BaseTools;
using System.Xml;

namespace Epower.ITSM.SqlDAL.Customer
{
    /// <summary>
    /// 流程自定义扩展项显示方式-数据层代码
    /// </summary>
    public class Br_ExtensionDisplayWayDP
    {
        /// <summary>
        /// 查询 v_ex_way_nodemodel 表
        /// </summary>
        /// <param name="sWhere"></param>
        /// <param name="sOrder"></param>
        /// <returns></returns>
        public static DataTable GetDataTable(string sWhere, string sOrder)
        {

            string strSQL = string.Empty;
            OracleConnection cn = ConfigTool.GetConnection();
            strSQL = @"select appid, flowmodelid, nodemodelid from v_ex_display_nodemodel  WHERE 1 = 1  ";

            strSQL += sWhere;
            strSQL += sOrder;
            DataTable dt = new DataTable();
            dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigTool.CloseConnection(cn);
            return dt;
        }

        /// <summary>
        /// 取环节模型下的扩展项显示方式列表
        /// </summary>
        /// <param name="lngNodeModelID"></param>
        /// <returns></returns>
        public static DataTable GetExDisplayWayList(long lngNodeModelID, long lngFlowModelID)
        {
            OracleConnection conn = null;

            try
            {
                conn = ConfigTool.GetConnection();

                String strSql = String.Format(@"select t1.fieldid, t1.displaystatus, t2.chname, t2.itemtype from br_extensiondisplaystatus t1
                                            left join br_extensionsitems t2
                                            on t1.fieldid = t2.fieldid
                                            where t1.nodemodelid = {0} and t1.flowmodelid = {1}
                                            order by t1.addtime asc", lngNodeModelID, lngFlowModelID);

                return OracleDbHelper.ExecuteDataTable(conn, CommandType.Text, strSql);
            }
            finally
            {
                ConfigTool.CloseConnection(conn);
            }
        }

        /// <summary>
        /// 添加扩展项显示方式列表
        /// </summary>
        /// <param name="trans">事务对象</param>
        /// <param name="lngFlowModelID">流程模型编号</param>
        /// <param name="lngNodeModelID">环节模型编号</param>
        /// <param name="lngFieldID">扩展项编号</param>
        /// <param name="intDisplayWay">显示方式</param>
        public static void AddExDisplayWayList(OracleTransaction trans,
            long lngFlowModelID,
            long lngNodeModelID,
            long lngFieldID,
            Int32 intDisplayWay)
        {

            long lngNextID = EpowerGlobal.EPGlobal.GetNextID("br_ext_displayway");

            String strSql = String.Format(@"insert into br_extensiondisplaystatus(ID, FlowModelID, NodeModelID, FieldID, DisplayStatus, AddTime)
                                                                          values({0}, {1},         {2},         {3},     {4},           to_date('{5}','yyyy-MM-dd HH24:mi:ss'))",
                                        lngNextID, lngFlowModelID, lngNodeModelID, lngFieldID, intDisplayWay, DateTime.Now);
            OracleDbHelper.ExecuteNonQuery(trans, CommandType.Text, strSql);
        }

        /// <summary>
        /// 删除拥有扩展项显示方式的环节模型
        /// </summary>
        /// <param name="sWhere"></param>
        /// <param name="sOrder"></param>
        /// <returns></returns>
        public static void DeleteNodeModel(OracleTransaction trans, long lngFlowModelID, long lngNodeModelID)
        {
            String strSql = String.Format("delete from br_extensiondisplaystatus where flowmodelid = {1} and nodemodelid = {0}", lngNodeModelID, lngFlowModelID);
            OracleDbHelper.ExecuteNonQuery(trans, CommandType.Text, strSql);
        }


        /// <summary>
        /// 更新扩展项编号(FieldID)
        /// </summary>
        /// <param name="trans">事务对象</param>
        /// <param name="lngFlowModelID">流程模型编号</param>
        /// <param name="nfieldID">新扩展项编号</param>
        /// <param name="ofieldID">老扩展项编号</param>
        public static void UpdateFieldID(OracleTransaction trans, long lngFlowModelID, int nfieldID, int ofieldID)
        {
            String strSql = String.Format("update br_extensiondisplaystatus set fieldid = {0} where flowmodelid = {1} and fieldid = {2}", nfieldID, lngFlowModelID, ofieldID);
            OracleDbHelper.ExecuteNonQuery(trans, CommandType.Text, strSql);
        }


        /// <summary>
        /// 取应用名
        /// </summary>
        /// <param name="lngAppID"></param>
        /// <returns></returns>
        public static String GetAppName(long lngAppID)
        {
            OracleConnection conn = null;

            try
            {
                conn = ConfigTool.GetConnection();

                String strSql = String.Format(@"SELECT APPNAME FROM ES_APP WHERE APPID = {0}", lngAppID);

                DataTable dt = OracleDbHelper.ExecuteDataTable(conn, CommandType.Text, strSql);

                return dt.Rows[0]["appname"].ToString();
            }
            finally
            {
                ConfigTool.CloseConnection(conn);
            }
        }

        /// <summary>
        /// 取流程模型名
        /// </summary>
        /// <param name="lngFlowModelID"></param>
        /// <returns></returns>
        public static String GetFlowModelName(long lngFlowModelID)
        {
            OracleConnection conn = null;

            try
            {
                conn = ConfigTool.GetConnection();

                String strSql = String.Format(@"SELECT flowname as flowmodelname FROM ES_flowmodel WHERE flowmodelid = {0}", lngFlowModelID);

                DataTable dt = OracleDbHelper.ExecuteDataTable(conn, CommandType.Text, strSql);

                return dt.Rows[0]["flowmodelname"].ToString();
            }
            finally
            {
                ConfigTool.CloseConnection(conn);
            }
        }



        /// <summary>
        /// 取环节模型名
        /// </summary>
        /// <param name="lngFlowModelID"></param>
        /// <param name="lngNodeModelID"></param>
        /// <returns></returns>
        public static String GetFlowModelName(long lngFlowModelID, long lngNodeModelID)
        {
            OracleConnection conn = null;

            try
            {
                conn = ConfigTool.GetConnection();

                String strSql = String.Format(@"SELECT MAX(flowmodelid) as flowmodelid FROM es_flowmodel where oflowmodelid = {0}", lngFlowModelID);

                DataTable dt = OracleDbHelper.ExecuteDataTable(conn, CommandType.Text, strSql);

                lngFlowModelID = long.Parse(dt.Rows[0]["flowmodelid"].ToString());



                String strSql2 = String.Format(@"select nodename from es_nodemodel where flowmodelid = {0} and nodemodelid = {1}",
                    lngFlowModelID, lngNodeModelID);

                dt = OracleDbHelper.ExecuteDataTable(conn, CommandType.Text, strSql2);

                return dt.Rows[0]["nodename"].ToString();
            }
            finally
            {
                ConfigTool.CloseConnection(conn);
            }
        }


        /// <summary>
        /// 取流程模型列表
        /// </summary>
        /// <returns></returns>
        public static DataTable GetFlowModelList()
        {
            OracleConnection conn = null;

            try
            {
                conn = ConfigTool.GetConnection();

                String strSql = String.Format(@"select flowmodelid from br_extensiondisplaystatus 
group by flowmodelid ");

                return OracleDbHelper.ExecuteDataTable(conn, CommandType.Text, strSql);
            }
            finally
            {
                ConfigTool.CloseConnection(conn);
            }
        }


        /// <summary>
        /// 若环节模型表中某些环节已被删除. 则删除这些被删除的环节.
        /// </summary>
        /// <param name="lngFlowModelID">流程环节模型</param>
        public static void DeleteNodeModelDirtyData(long lngFlowModelID, long lngOFlowModelID)
        {
            OracleConnection conn = null;

            try
            {
                conn = ConfigTool.GetConnection();

                String strSql = String.Format(@"delete from br_extensiondisplaystatus
        where flowmodelid = {0} and nodemodelid not in (select nodemodelid from es_nodemodel where flowmodelid = {1})", lngOFlowModelID, lngFlowModelID);

                OracleDbHelper.ExecuteNonQuery(conn, CommandType.Text, strSql);
            }
            finally
            {
                ConfigTool.CloseConnection(conn);
            }
        }

    }
}
