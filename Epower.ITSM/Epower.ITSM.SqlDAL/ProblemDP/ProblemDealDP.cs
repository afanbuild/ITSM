/****************************************************************************
 * 
 * description:����������ݲ�ѯ��
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

        #region ���ⵥ���
        /// <summary>
        /// �������ⵥ����ID��ȡ�����Ϣ
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
        /// ����������ȡ�������ݱ�����
        /// </summary>
        /// <param name="pWhere"></param>
        /// <returns></returns>
        public static DataTable GetProblemDealData(long lngUserID, long lngDeptID, long lngOrgID, RightEntity re, string pWhere)
        {
            string strList = "";       //��Ų����б�
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
                //��ѯ���ս��
                strSQL += " AND a.flowid = -1 ";
            }
            else
            {
                strSQL += pWhere;
                #region ��Χ����
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
                                //���Ǹ����Ų����ҵ�
                                strSQL += "AND exists (SELECT messageid FROM es_message WHERE flowid = b.flowid AND recdeptid in (select deptid from ts_dept where fullid like " + StringTool.SqlQ(strList + " % ") + "))";
                            }
                            break;
                        case eO_RightRange.eOrg:
                            strList = DeptDP.GetDeptFullID(lngOrgID);
                            if (strList.Trim().Length > 0)
                            {
                                //���Ǹ����Ų����ҵ�
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
                //��ѯ���ս��
                stWhere += " AND flowid = -1 ";
            }
            else
            {
                #region ��Χ����
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
                                //���Ǹ����Ų����ҵ�
                                stWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_Pro_ProblemDeal.flowid AND recdeptid in (select deptid from ts_dept where fullid like " + StringTool.SqlQ(strList + " % ") + "))";
                            }
                            break;
                        case eO_RightRange.eOrg:
                            strList = DeptDP.GetDeptFullID(lngOrgID);
                            if (strList.Trim().Length > 0)
                            {
                                //���Ǹ����Ų����ҵ�
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
        /// �����¼����̱��ȡ�������������
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
        /// �������������ȡ�������������
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
        /// ��������ID��ȡ�����Ϣ
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
        /// ��������¼�
        /// </summary>
        /// <param name="lngFlowID">�¼�����ID</param>
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
        /// �¼���������
        /// </summary>
        /// <param name="lngFlowID">�¼�����ID</param>
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

        #region ��ȡ������ط�����ͼ��
        /// <summary>
        /// ��ȡ������ط�����ͼ��
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

            string strSql = @"--�账������
                                select '�账������' as Title,count(1) as counts,'ProblemW' as Types
                                  from Pro_ProblemDeal A,es_Flow B,ts_UserDept C,ts_Dept D
                                 where A.FlowID = B.FlowID and A.RegUserID = C.UserID and C.DeptID = D.DeptID
                                        and D.deleted = 0
	                                    and (
                                         (B.status = 30 and to_char(EndTime,'yyyy-MM') = to_char(sysdate,'yyyy-MM'))
                                         or B.status = 20) 	
                                        and D.MastDeptID in (select E.DeptID from Br_MastCustomer E where E.Deleted = 0)
	                                    and D.MastDeptID in (select DeptID from Br_MastCustomer where 1=1" + strWhere + @")
                                union all
                                --�������������ⵥ�ĵǼ�ʱ��Ϊ���¡�
                                select '��������' as Title,count(1) as counts,'ProblemB' as Types
                                  from Pro_ProblemDeal A,es_Flow B,ts_UserDept C,ts_Dept D
                                 where A.FlowID = B.FlowID and A.RegUserID = C.UserID and C.DeptID = D.DeptID
                                            and D.deleted = 0 and to_char(A.RegTime,'yyyy-MM') = to_char(sysdate,'yyyy-MM')
		                            and A.flowid NOT IN (select flowid from es_message where status=20 and senderid=0)
	                                and D.MastDeptID in (select E.DeptID from Br_MastCustomer E where E.Deleted = 0)	  
                                    and D.MastDeptID in (select DeptID from Br_MastCustomer where 1=1" + strWhere + @")
                                union all
                                --����δ���
                                select '����δ���' as Title,count(1) as counts,'ProblemA' as Types
                                  from Pro_ProblemDeal A,es_Flow B,ts_UserDept C,ts_Dept D
                                 where A.FlowID = B.FlowID and A.RegUserID = C.UserID and C.DeptID = D.DeptID
                                        and D.deleted = 0 and B.status = 20
                                        and D.MastDeptID in (select E.DeptID from Br_MastCustomer E where E.Deleted = 0)
    	                                and D.MastDeptID in (select DeptID from Br_MastCustomer where 1=1" + strWhere + @")
                                union all
                                --������ɡ����̽����ģ������̽���ʱ��Ϊ���µġ�
                                select '���¹ر�' as Title,count(1) as counts,'ProblemC' as Types
                                  from Pro_ProblemDeal A,es_Flow B,ts_UserDept C,ts_Dept D
                                 where A.FlowID = B.FlowID and A.RegUserID = C.UserID and C.DeptID = D.DeptID
	                                and C.deleted = 0 and B.status = 30  and to_char(EndTime,'yyyy-MM') = to_char(sysdate,'yyyy-MM')
	                                and D.MastDeptID in (select E.DeptID from Br_MastCustomer E where E.Deleted = 0)	  
                                    and D.MastDeptID in (select DeptID from Br_MastCustomer where 1=1" + strWhere + @")";

            DataTable dt = CommonDP.ExcuteSqlTable(strSql);

            return dt;
        }
        #endregion

        #region ���ⵥ�����ӵ㿪�鿴����
        /// <summary>
        /// ���ⵥ�����ӵ㿪�鿴����
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
                //����δ���
                dt = OracleDbHelper.ExecuteDataTable(cn, "v_ProblemA", "*", " ORDER BY Problem_ID DESC", pagesize, pageindex, strWhere, ref rowcount);
            }
            else if (strType == "ProblemB")
            {
                //��������
                dt = OracleDbHelper.ExecuteDataTable(cn, "v_ProblemB", "*", " ORDER BY Problem_ID DESC", pagesize, pageindex, strWhere, ref rowcount);
            }
            else if (strType == "ProblemC")
            {
                //�������
                dt = OracleDbHelper.ExecuteDataTable(cn, "v_ProblemC", "*", " ORDER BY Problem_ID DESC", pagesize, pageindex, strWhere, ref rowcount);
            }
            else if (strType == "ProblemW")
            {
                //�账������
                dt = OracleDbHelper.ExecuteDataTable(cn, "v_ProblemW", "*", " ORDER BY Problem_ID DESC", pagesize, pageindex, strWhere, ref rowcount);
            }
            else
            {
                strSql = @"--�޼�¼
                                select A.*
                                  from Pro_ProblemDeal A where 1 = 2";
            }

            return dt;
        }
        #endregion

        #region ���ⰴ���ͳ��
        /// <summary>
        /// ���ⰴ���ͳ��
        /// </summary>
        /// <param name="p_strWhere">where����</param>
        /// <param name="p_strOrderby">��������</param>
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

        #region ���ⰴ�������ͳ��
        /// <summary>
        /// ���ⰴ�������ͳ��
        /// </summary>
        /// <param name="p_strWhere">where����</param>
        /// <param name="p_strOrderby">��������</param>
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

        #region �߼���ѯ���

        #region ��ȡ�߼���ѯ����
        /// <summary>
        /// ��ȡ�߼���ѯ����
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

        #region ��¼���ⵥ��ѯ������ѯ����
        /// <summary>
        /// ��¼���ⵥ��ѯ������ѯ����
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

        #region ��ѯ���ⵥ�Ѿ����ڵĸ߼���ѯ����
        /// <summary>
        /// ��ѯ���ⵥ�Ѿ����ڵĸ߼���ѯ����
        /// </summary>
        /// <param name="FormId"></param>
        /// <param name="LoginName"></param>
        /// <param name="Name"></param>
        /// <returns></returns>
        public static DataTable getCST_ISSUE_LISTFASTQUERY(string FormId, string LoginName, string Name)
        {
            Name = Name == "==ѡ���ղز�ѯ����==" ? "Temp1" : Name;

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

        #region ɾ���߼���ѯ����
        /// <summary>
        /// ɾ���߼���ѯ����
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

        #region ���ⵥ��ѯ����һ�ν���ʱ��Ĭ�ϲ�ѯ���ڴ�������ʱ��Ϊ��һ���µ�Ȩ�޷�Χ�ڵļ�¼
        /// <summary>
        /// ���ⵥ��ѯ����һ�ν���ʱ��Ĭ�ϲ�ѯ���ڴ�������ʱ��Ϊ��һ���µ�Ȩ�޷�Χ�ڵļ�¼
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

            //���ڴ�������ʱ��Ϊ��һ���µ�Ȩ�޷�Χ�ڵļ�¼
            strWhere += " and Status = 20 and RegTime >= to_date(" + StringTool.SqlQ(DateTime.Now.AddMonths(-1).ToShortDateString()) + ",'yyyy-MM-dd')";
            if (re == null || re.CanRead == false)
            {
                //��ѯ���ս��
                strWhere += " AND flowid = -1 ";
            }
            else if (re != null && re.CanRead == true)
            {
                #region ��Χ����
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
                            //���Ǹ����Ų����ҵ�
                            strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_Pro_ProblemDeal.flowid AND recdeptid in (select deptid from ts_dept where fullid like " + StringTool.SqlQ(strList + "%") + "))";
                        }
                        break;
                    case eO_RightRange.eOrg:
                        strList = DeptDP.GetDeptFullID(lngOrgID);
                        if (strList.Trim().Length > 0)
                        {
                            //���Ǹ����Ų����ҵ�
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

        #region ����xml������ȡ�߼���ѯ�����µĲ�ѯ��¼
        /// <summary>
        /// ����xml������ȡ�߼���ѯ�����µĲ�ѯ��¼
        /// </summary>
        /// <param name="strXmlcond">xml��</param>
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
            string strTmp = string.Empty;               //��ʱ��Ÿ���ѯ������ֵ
            string strWhere = " 1=1 ";                  //���where���
            string strList = string.Empty;              //Ȩ��ʹ��

            string strProblemNo = string.Empty;         //���ⵥ��
            string strEquName = string.Empty;           //�ʲ�����
            string strFlowStatus = string.Empty;        //����״̬
            string strStatus = string.Empty;            //�¼�״̬
            string strRegBeginTime = string.Empty;      //�Ǽǿ�ʼʱ��
            string strRegEndTime = string.Empty;        //�Ǽǽ���ʱ��
            string strSubject = string.Empty;           //����
            string strProbTypeID = "0";                 //�������ID
            string strProbLevelID = "0";                //���⼶��ID
            string strEffectID = "0";                   //Ӱ���ID
            string strInstancyID = "0";                 //������ID
            string strRegUser = string.Empty;         //�Ǽ���

            XmlTextReader tr = new XmlTextReader(new StringReader(strXmlcond));
            while (tr.Read())
            {
                if (tr.Name == "Field" && tr.NodeType == XmlNodeType.Element)
                {
                    strTmp = tr.GetAttribute("Value").Trim();

                    switch (tr.GetAttribute("FieldName"))
                    {
                        case "ProblemNo":           //���ⵥ��
                            strProblemNo = strTmp;
                            break;
                        case "EquName":             //�ʲ�����
                            strEquName = strTmp;
                            break;
                        case "FlowStatus":          //����״̬
                            strFlowStatus = strTmp;
                            break;
                        case "Status":              //����״̬
                            strStatus = strTmp;
                            break;
                        case "MessageBegin":        //�Ǽǿ�ʼʱ��
                            strRegBeginTime = strTmp;
                            break;
                        case "MessageEnd":          //�Ǽǽ���ʱ��
                            strRegEndTime = strTmp;
                            break;
                        case "Subject":             //����
                            strSubject = strTmp;
                            break;
                        case "CataProblemType":     //�������
                            strProbTypeID = strTmp;
                            break;
                        case "CataProblemLevel":    //���⼶��
                            strProbLevelID = strTmp;
                            break;
                        case "CtrFCDEffect":        //Ӱ���
                            strEffectID = strTmp;
                            break;
                        case "CtrFCDInstancy":      //������
                            strInstancyID = strTmp;
                            break;
                        case "txtRegUser":          //�Ǽ���
                            strRegUser = strTmp;
                            break;
                        default:
                            break;
                    }
                }
            }
            tr.Close();

            #region ������ѯ����

            //���ⵥ��
            if (strProblemNo != string.Empty)
            {
                strWhere += " AND ProblemNo like " + StringTool.SqlQ("%" + strProblemNo.Trim() + "%");
            }

            //�ʲ�����
            if (strEquName != string.Empty)
            {
                strWhere += " AND EquName like " + StringTool.SqlQ("%" + strEquName.Trim() + "%");
            }

            //����״̬
            if (strFlowStatus != "-1" && strFlowStatus != "")
            {
                strWhere += " AND status = " + strFlowStatus;
            }

            //����״̬
            if (strStatus != "0" && strStatus != string.Empty)
            {
                strWhere += " AND State = " + strStatus;
            }

            //�Ǽǿ�ʼʱ��
            if (strRegBeginTime.Length != 0)
            {
                strWhere += " AND RegTime >= to_date(" + StringTool.SqlQ(strRegBeginTime) + ",'yyyy-MM-dd HH24:mi:ss')";
            }

            //�Ǽǽ���ʱ��
            if (strRegEndTime.Length != 0)
            {
                strWhere += " AND RegTime <= to_date(" + StringTool.SqlQ(strRegEndTime) + ",'yyyy-MM-dd HH24:mi:ss')";
            }

            //����
            if (strSubject != string.Empty)
            {
                strWhere += " AND Problem_Title like " + StringTool.SqlQ("%" + strSubject.Trim() + "%");
            }

            //�������
            if (strProbTypeID != "0" && strProbTypeID != string.Empty)
            {
                strWhere += " AND Problem_Type = " + strProbTypeID;
            }

            //���⼶��
            if (strProbLevelID != "0" && strProbLevelID != string.Empty)
            {
                strWhere += " AND Problem_Level = " + strProbLevelID;
            }

            //Ӱ���
            if (strEffectID != "0" && strEffectID != string.Empty)
            {
                strWhere += " AND EffectID = " + strEffectID;
            }

            //������
            if (strInstancyID != "0" && strInstancyID != string.Empty)
            {
                strWhere += " AND InstancyID = " + strInstancyID;
            }

            //�Ǽ���
            if (strRegUser != string.Empty)
            {
                strWhere += " AND RegUserName like " + StringTool.SqlQ("%" + strRegUser.Trim() + "%");
            }

            #endregion

            #region Ȩ��
            if (re == null || re.CanRead == false)
            {
                //��ѯ���ս��
                strWhere += " AND flowid = -1 ";
            }
            else if (re != null && re.CanRead == true)
            {
                #region ��Χ����
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
                            //���Ǹ����Ų����ҵ�
                            strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_Pro_ProblemDeal.flowid AND recdeptid in (select deptid from ts_dept where fullid like " + StringTool.SqlQ(strList + "%") + "))";
                        }
                        break;
                    case eO_RightRange.eOrg:
                        strList = DeptDP.GetDeptFullID(lngOrgID);
                        if (strList.Trim().Length > 0)
                        {
                            //���Ǹ����Ų����ҵ�
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

        #region ��̬��ѯ: ʹ�����ɵĶ�̬SQL, ��ѯ��¼ - 2013-04-01 @������

        /// <summary>
        /// ʹ�����ɵĶ�̬SQL, ��ѯ��¼
        /// </summary>
        /// <param name="strWhere">��̬SQL</param>
        /// <param name="lngUserID">�û����</param>
        /// <param name="lngDeptID">���ű��</param>
        /// <param name="lngOrgID">��֯�������</param>
        /// <param name="re">Ȩ��</param>
        /// <param name="pagesize">���ݳ���</param>
        /// <param name="pageindex">�ڼ�ҳ?</param>
        /// <param name="rowcount">������</param>
        /// <returns></returns>
        public static DataTable GetProblemsWithMoreParams(string strWhere, long lngUserID, long lngDeptID, long lngOrgID, RightEntity re, int pagesize, int pageindex, ref int rowcount)
        {
            string strList = string.Empty;              //Ȩ��ʹ��

            if (String.IsNullOrEmpty(strWhere))
                strWhere = " 1 = 1 ";

            #region Ȩ��
            if (re == null || re.CanRead == false)
            {
                //��ѯ���ս��
                strWhere += " AND flowid = -1 ";
            }
            else if (re != null && re.CanRead == true)
            {
                #region ��Χ����
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
                            //���Ǹ����Ų����ҵ�
                            strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_Pro_ProblemDeal.flowid AND recdeptid in (select deptid from ts_dept where fullid like " + StringTool.SqlQ(strList + "%") + "))";
                        }
                        break;
                    case eO_RightRange.eOrg:
                        strList = DeptDP.GetDeptFullID(lngOrgID);
                        if (strList.Trim().Length > 0)
                        {
                            //���Ǹ����Ų����ҵ�
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

        #region ��̬��ѯ: ʹ�����ɵĶ�̬SQL, ��ѯ��¼, ����Ҫ��ҳ - 2013-04-08 @������

        /// <summary>
        /// ʹ�����ɵĶ�̬SQL, ��ѯ��¼, ����Ҫ��ҳ
        /// </summary>
        /// <param name="strWhere">��̬SQL</param>
        /// <param name="lngUserID">�û����</param>
        /// <param name="lngDeptID">���ű��</param>
        /// <param name="lngOrgID">��֯�������</param>
        /// <param name="re">Ȩ��</param>
        /// <returns></returns>
        public static DataTable GetProblemsWithOutPage(string strWhere, long lngUserID, long lngDeptID, long lngOrgID, RightEntity re)
        {            
            String strSqlScript = String.Empty;    // sql ��ѯ����
            string strList = string.Empty;              //Ȩ��ʹ��

            if (String.IsNullOrEmpty(strWhere))
                strWhere = " WHERE 1 = 1 ";

            #region Ȩ��
            if (re == null || re.CanRead == false)
            {
                //��ѯ���ս��
                strWhere += " AND flowid = -1 ";
            }
            else if (re != null && re.CanRead == true)
            {
                #region ��Χ����
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
                            //���Ǹ����Ų����ҵ�
                            strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_Pro_ProblemDeal.flowid AND recdeptid in (select deptid from ts_dept where fullid like " + StringTool.SqlQ(strList + "%") + "))";
                        }
                        break;
                    case eO_RightRange.eOrg:
                        strList = DeptDP.GetDeptFullID(lngOrgID);
                        if (strList.Trim().Length > 0)
                        {
                            //���Ǹ����Ų����ҵ�
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

        #region ����xml������ȡ�߼���ѯ�����µĲ�ѯ��¼������excel��
        /// <summary>
        /// ����xml������ȡ�߼���ѯ�����µĲ�ѯ��¼������excel��
        /// </summary>
        /// <param name="strXmlcond">xml��</param>
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
            string strTmp = string.Empty;               //��ʱ��Ÿ���ѯ������ֵ
            string strWhere = string.Empty;                  //���where���
            string strList = string.Empty;              //Ȩ��ʹ��

            string strProblemNo = string.Empty;         //���ⵥ��
            string strEquName = string.Empty;           //�ʲ�����
            string strFlowStatus = string.Empty;        //����״̬
            string strStatus = string.Empty;            //�¼�״̬
            string strRegBeginTime = string.Empty;      //�Ǽǿ�ʼʱ��
            string strRegEndTime = string.Empty;        //�Ǽǽ���ʱ��
            string strSubject = string.Empty;           //����
            string strProbTypeID = "0";                 //�������ID
            string strProbLevelID = "0";                //���⼶��ID
            string strEffectID = "0";                   //Ӱ���ID
            string strInstancyID = "0";                 //������ID
            string strRegUser = string.Empty;         //�Ǽ���

            string strSql = string.Empty;               //��ѯ���
            strSql = "select * from V_Pro_ProblemDeal where 1=1 ";

            XmlTextReader tr = new XmlTextReader(new StringReader(strXmlcond));
            while (tr.Read())
            {
                if (tr.Name == "Field" && tr.NodeType == XmlNodeType.Element)
                {
                    strTmp = tr.GetAttribute("Value").Trim();

                    switch (tr.GetAttribute("FieldName"))
                    {
                        case "ProblemNo":           //���ⵥ��
                            strProblemNo = strTmp;
                            break;
                        case "EquName":             //�ʲ�����
                            strEquName = strTmp;
                            break;
                        case "FlowStatus":          //����״̬
                            strFlowStatus = strTmp;
                            break;
                        case "Status":              //����״̬
                            strStatus = strTmp;
                            break;
                        case "MessageBegin":        //�Ǽǿ�ʼʱ��
                            strRegBeginTime = strTmp;
                            break;
                        case "MessageEnd":          //�Ǽǽ���ʱ��
                            strRegEndTime = strTmp;
                            break;
                        case "Subject":             //����
                            strSubject = strTmp;
                            break;
                        case "CataProblemType":     //�������
                            strProbTypeID = strTmp;
                            break;
                        case "CataProblemLevel":    //���⼶��
                            strProbLevelID = strTmp;
                            break;
                        case "CtrFCDEffect":        //Ӱ���
                            strEffectID = strTmp;
                            break;
                        case "CtrFCDInstancy":      //������
                            strInstancyID = strTmp;
                            break;
                        case "txtRegUser":          //�Ǽ���
                            strRegUser = strTmp;
                            break;
                        default:
                            break;
                    }
                }
            }
            tr.Close();

            #region ������ѯ����

            //���ⵥ��
            if (strProblemNo != string.Empty)
            {
                strWhere += " AND ProblemNo like " + StringTool.SqlQ("%" + strProblemNo.Trim() + "%");
            }

            //�ʲ�����
            if (strEquName != string.Empty)
            {
                strWhere += " AND EquName like " + StringTool.SqlQ("%" + strEquName.Trim() + "%");
            }

            //����״̬
            if (strFlowStatus != "-1" && strFlowStatus != "")
            {
                strWhere += " AND status = " + strFlowStatus;
            }

            //����״̬
            if (strStatus != "0" && strStatus != string.Empty)
            {
                strWhere += " AND State = " + strStatus;
            }

            //�Ǽǿ�ʼʱ��
            if (strRegBeginTime.Length != 0)
            {
                strWhere += " AND RegTime >= to_char(to_date(" + StringTool.SqlQ(strRegBeginTime) + ",'yyyy-MM-dd HH24:mi:ss'))";
            }

            //�Ǽǽ���ʱ��
            if (strRegEndTime.Length != 0)
            {
                strWhere += " AND RegTime <= to_char(to_date(" + StringTool.SqlQ(strRegEndTime) + ",'yyyy-MM-dd HH24:mi:ss'))";
            }

            //����
            if (strSubject != string.Empty)
            {
                strWhere += " AND Problem_Title like " + StringTool.SqlQ("%" + strSubject.Trim() + "%");
            }

            //�������
            if (strProbTypeID != "0" && strProbTypeID != string.Empty)
            {
                strWhere += " AND Problem_Type = " + strProbTypeID;
            }

            //���⼶��
            if (strProbLevelID != "0" && strProbLevelID != string.Empty)
            {
                strWhere += " AND Problem_Level = " + strProbLevelID;
            }

            //Ӱ���
            if (strEffectID != "0" && strEffectID != string.Empty)
            {
                strWhere += " AND EffectID = " + strEffectID;
            }

            //������
            if (strInstancyID != "0" && strInstancyID != string.Empty)
            {
                strWhere += " AND InstancyID = " + strInstancyID;
            }

            //�Ǽ���
            if (strRegUser != string.Empty)
            {
                strWhere += " AND RegUserName like " + StringTool.SqlQ("%" + strRegUser.Trim() + "%");
            }

            #endregion

            #region Ȩ��
            if (re == null || re.CanRead == false)
            {
                //��ѯ���ս��
                strWhere += " AND flowid = -1 ";
            }
            else if (re != null && re.CanRead == true)
            {
                #region ��Χ����
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
                            //���Ǹ����Ų����ҵ�
                            strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_Pro_ProblemDeal.flowid AND recdeptid in (select deptid from ts_dept where fullid like " + StringTool.SqlQ(strList + "%") + "))";
                        }
                        break;
                    case eO_RightRange.eOrg:
                        strList = DeptDP.GetDeptFullID(lngOrgID);
                        if (strList.Trim().Length > 0)
                        {
                            //���Ǹ����Ų����ҵ�
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

        #region ���ⵥ��ѯ����һ�ν���ʱ��Ĭ�ϲ�ѯ���ڴ�������ʱ��Ϊ��һ���µ�Ȩ�޷�Χ�ڵļ�¼������excel��
        /// <summary>
        /// ���ⵥ��ѯ����һ�ν���ʱ��Ĭ�ϲ�ѯ���ڴ�������ʱ��Ϊ��һ���µ�Ȩ�޷�Χ�ڵļ�¼������excel��
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

            string strSql = string.Empty;               //��ѯ���
            strSql = "select * from V_Pro_ProblemDeal where 1=1 ";

            //���ڴ�������ʱ��Ϊ��һ���µ�Ȩ�޷�Χ�ڵļ�¼
            strWhere += " and Status = 20 and RegTime >= to_date(to_char(DateAdd('month',-1,sysdate), 'yyyy-MM-dd'),'yyyy-MM-dd')";
            if (re == null || re.CanRead == false)
            {
                //��ѯ���ս��
                strWhere += " AND flowid = -1 ";
            }
            else if (re != null && re.CanRead == true)
            {
                #region ��Χ����
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
                            //���Ǹ����Ų����ҵ�
                            strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_Pro_ProblemDeal.flowid AND recdeptid in (select deptid from ts_dept where fullid like " + StringTool.SqlQ(strList + "%") + "))";
                        }
                        break;
                    case eO_RightRange.eOrg:
                        strList = DeptDP.GetDeptFullID(lngOrgID);
                        if (strList.Trim().Length > 0)
                        {
                            //���Ǹ����Ų����ҵ�
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

        #region ��ʱ��ͳ����������KPI
        /// <summary>
        /// ��ʱ��ͳ����������KPI
        /// </summary>
        /// <param name="type">0��ʾ���� 1��ʾ����</param>
        /// <param name="p_strWhere">where����</param>        
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


        #region ����ϲ�
        /// <summary>
        /// ��������ϲ�ʱ��ȡ�����ⵥ
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
        /// ����ϲ�ʱ��ȡ�����ⵥ
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
        /// ����ϲ���������
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



        #region ȡ���ⵥ�� - 2013-11-28 @������

        /// <summary>
        /// ȡ���ⵥ��
        /// </summary>
        /// <param name="lngFlowID">���̱��</param>
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
