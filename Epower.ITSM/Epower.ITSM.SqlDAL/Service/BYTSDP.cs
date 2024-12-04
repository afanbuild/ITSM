/*******************************************************************
 * ��Ȩ���У�
 * Description��Ͷ�����ݴ�����
 * 
 * 
 * Create By  ��zhumingchun
 * Create Date��2007-07-23
 * *****************************************************************/
using System;
using System.Data;
using System.Data.OracleClient;
using System.Data.OleDb;
using System.Xml;
using System.IO;

using Epower.DevBase.BaseTools;
using Epower.DevBase.Organization.Base;
using Epower.DevBase.Organization.SqlDAL;
using Epower.ITSM.Base;
using EpowerGlobal;
using EpowerCom;

namespace Epower.ITSM.SqlDAL
{
	/// <summary>
	/// EMSDP ��ժҪ˵����
	/// </summary>
    public class BYTSDP
	{
		/// <summary>
		/// EMSDP
		/// </summary>
		public BYTSDP()
		{
			//
			// TODO: �ڴ˴���ӹ��캯���߼�
			//
        }

        #region ҳ�洦��
        /// <summary>
        /// ��ȡ���еĻ���
        /// </summary>
        /// <returns></returns>
        public static DataTable GetAllManageOffices()
        {
            string sSql = "select deptid,deptname from ts_dept where deptkind=1";

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

        #region �������ҳ�����ݴ���
        /// <summary>
		/// ���ݣأ̴ͣ�������ȡ�����¼�����
		/// </summary>
		/// <param name="strXmlcond"></param>
		/// <param name="lngUserID"></param>
		/// <param name="lngDeptID"></param>
		/// <param name="lngOrgID"></param>
		/// <param name="re"></param>
        /// <param name="pWhere"></param>
		/// <returns></returns>
        public static DataTable GetIssuesForCond(string strXmlcond, long lngUserID, long lngDeptID, long lngOrgID, RightEntity re, string pWhere)
		{
			string strSQL="";        //���SQL�ַ���
			string strList="";       //��Ų����б�
			string strWhere = "";        //�������

            if (re != null && re.CanRead == true && !string.IsNullOrEmpty(strXmlcond))
			{
				#region ��ȡ��ѯ������ֵ
                FieldValues fv = new FieldValues(strXmlcond);
                //״̬
                if (fv.GetFieldValue("Status").Value != "-1" && fv.GetFieldValue("Status").Value != "")
				{
                    strWhere += " AND b.status = " + fv.GetFieldValue("Status").Value;
				}
                //�Ǽ�ʱ��
                if (fv.GetFieldValue("MessageBegin").Value.Trim() != string.Empty)
				{
                    strWhere += " AND a.RegTime >= " + StringTool.SqlQ(fv.GetFieldValue("MessageBegin").Value);
				}
                if (fv.GetFieldValue("MessageEnd").Value.Trim() != string.Empty)
				{
                    strWhere += " AND a.RegTime <= " + StringTool.SqlQ(fv.GetFieldValue("MessageEnd").Value.Trim() + " 23:59:59");
				}
                //Ͷ����
                if (fv.GetFieldValue("BYPersonName").Value.Trim() != string.Empty)
				{
                    strWhere += " AND a.BY_PersonName like " + StringTool.SqlQ("%" + fv.GetFieldValue("BYPersonName").Value.Trim() + "%");
				}
                //������Ŀ
                if (fv.GetFieldValue("Project").Value.Trim() != string.Empty && fv.GetFieldValue("Project").Value.Trim() != "0")
				{
                    strWhere += " AND a.BY_Project= " + StringTool.SqlQ(fv.GetFieldValue("Project").Value.Trim());
				}
                //��Դ����
                if (fv.GetFieldValue("Source").Value.Trim() != "-1" && fv.GetFieldValue("Source").Value.Trim()!="1009")
                {
                    strWhere += " AND a.BY_Soure= " + StringTool.SqlQ(fv.GetFieldValue("Source").Value.Trim());
                }
                //����
                if (fv.GetFieldValue("Type").Value.Trim() != "-1" && fv.GetFieldValue("Type").Value.Trim() != "1010")
                {
                    strWhere += " AND a.BY_Type= " + StringTool.SqlQ(fv.GetFieldValue("Type").Value.Trim());
                }
                //����
                if (fv.GetFieldValue("Kind").Value.Trim() != "-1" && fv.GetFieldValue("Kind").Value.Trim() != "1011")
                {
                    strWhere += " AND a.BY_Kind= " + StringTool.SqlQ(fv.GetFieldValue("Kind").Value.Trim());
                }
                //����ʱ��
                if (fv.GetFieldValue("BY_ReceiveTime").Value.Trim() != string.Empty)
                {
                    strWhere += " AND a.BY_ReceiveTime >= " + StringTool.SqlQ(fv.GetFieldValue("BY_ReceiveTime").Value);
                }
                if (fv.GetFieldValue("BY_ReceiveTimeEnd").Value.Trim() != string.Empty)
                {
                    strWhere += " AND a.BY_ReceiveTime <= " + StringTool.SqlQ(fv.GetFieldValue("BY_ReceiveTimeEnd").Value.Trim() + " 23:59:59");
                }
                #endregion
            }

            #region ����ҳ���Ѿ�����ã�ֱ�Ӵ������Ĳ�ѯ�������
            strWhere += pWhere;
            #endregion

			strSQL= @"SELECT  a.*,b.status,
					case when b.status=30 then 
					datediff('Minute',nvl(b.endtime,sysdate),nvl(b.expectendtime,sysdate))
					else 
					datediff('Minute',sysdate,nvl(b.expectendtime,sysdate)) end FlowDiffMinute
					FROM Cst_BYTS a,es_flow b
					WHERE a.FlowID = b.FlowID   ";
			
			if(re==null || re.CanRead==false)
			{
				//��ѯ���ս��
				strSQL += " AND a.flowid = -1 ";
			}
			else
			{
				strSQL = strSQL + strWhere;

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
                                strSQL += "AND exists (SELECT messageid FROM es_message WHERE flowid = b.flowid AND recdeptid in (select deptid from ts_dept where fullid like "+StringTool.SqlQ(  strList+ " % ")+"))";
                            }
                            break;
                        case eO_RightRange.eOrg:
                            strList = DeptDP.GetDeptFullID(lngOrgID);
                            if (strList.Trim().Length > 0)
                            {
                                //���Ǹ����Ų����ҵ�
                               strSQL += "AND exists (SELECT messageid FROM es_message WHERE flowid = b.flowid AND recorgid in (select distinct deptid from ts_dept where deptkind = 1 and fullid like "+StringTool.SqlQ(  strList+ " % ")+"))";
                            }
                            break;
                        default:
                            strSQL += "";
                            break;
                    }
                }
				#endregion
				strSQL = strSQL +  " ORDER BY a.BY_ID DESC";
			}
			OracleConnection cn = ConfigTool.GetConnection();
			DataTable dt = OracleDbHelper.ExecuteDataTable(cn,CommandType.Text,strSQL);
			ConfigTool.CloseConnection(cn);
			return dt;
		}
        #endregion

        /// <summary>
		/// ���ݣأ̴ͣ�������ȡ�����¼�����
		/// </summary>
		/// <param name="strXmlcond"></param>
		/// <param name="lngUserID"></param>
		/// <param name="lngDeptID"></param>
		/// <param name="lngOrgID"></param>
		/// <param name="re"></param>
        /// <param name="pWhere"></param>
		/// <returns></returns>
        public DataTable GetIssuesForCond(string strXmlcond, long lngUserID, long lngDeptID, long lngOrgID, RightEntity re, string pWhere, int pagesize, int pageindex, ref int rowcount)
		{
			string strList="";       //��Ų����б�
			string strWhere = "1=1";        //�������

            if (re != null && re.CanRead == true && !string.IsNullOrEmpty(strXmlcond))
			{
				#region ��ȡ��ѯ������ֵ
                FieldValues fv = new FieldValues(strXmlcond);
                //״̬
                if (fv.GetFieldValue("Status").Value != "-1" && fv.GetFieldValue("Status").Value != "")
				{
                    strWhere += " AND status = " + fv.GetFieldValue("Status").Value;
				}
                //�Ǽ�ʱ��
                if (fv.GetFieldValue("MessageBegin").Value.Trim() != string.Empty)
				{
                    strWhere += " AND RegTime >= " + StringTool.SqlQ(fv.GetFieldValue("MessageBegin").Value);
				}
                if (fv.GetFieldValue("MessageEnd").Value.Trim() != string.Empty)
				{
                    strWhere += " AND RegTime <= " + StringTool.SqlQ(fv.GetFieldValue("MessageEnd").Value.Trim() + " 23:59:59");
				}
                //Ͷ����
                if (fv.GetFieldValue("BYPersonName").Value.Trim() != string.Empty)
				{
                    strWhere += " AND BY_PersonName like " + StringTool.SqlQ("%" + fv.GetFieldValue("BYPersonName").Value.Trim() + "%");
				}
                //������Ŀ
                if (fv.GetFieldValue("Project").Value.Trim() != string.Empty && fv.GetFieldValue("Project").Value.Trim() != "0")
				{
                    strWhere += " AND BY_Project= " + StringTool.SqlQ(fv.GetFieldValue("Project").Value.Trim());
				}
                //��Դ����
                if (fv.GetFieldValue("Source").Value.Trim() != "-1" && fv.GetFieldValue("Source").Value.Trim()!="1009")
                {
                    strWhere += " AND BY_Soure= " + StringTool.SqlQ(fv.GetFieldValue("Source").Value.Trim());
                }
                //����
                if (fv.GetFieldValue("Type").Value.Trim() != "-1" && fv.GetFieldValue("Type").Value.Trim() != "1010")
                {
                    strWhere += " AND BY_Type= " + StringTool.SqlQ(fv.GetFieldValue("Type").Value.Trim());
                }
                //����
                if (fv.GetFieldValue("Kind").Value.Trim() != "-1" && fv.GetFieldValue("Kind").Value.Trim() != "1011")
                {
                    strWhere += " AND BY_Kind= " + StringTool.SqlQ(fv.GetFieldValue("Kind").Value.Trim());
                }
                //����ʱ��
                if (fv.GetFieldValue("BY_ReceiveTime").Value.Trim() != string.Empty)
                {
                    strWhere += " AND BY_ReceiveTime >= " + StringTool.SqlQ(fv.GetFieldValue("BY_ReceiveTime").Value);
                }
                if (fv.GetFieldValue("BY_ReceiveTimeEnd").Value.Trim() != string.Empty)
                {
                    strWhere += " AND BY_ReceiveTime <= " + StringTool.SqlQ(fv.GetFieldValue("BY_ReceiveTimeEnd").Value.Trim() + " 23:59:59");
                }
                #endregion
            }

            #region ����ҳ���Ѿ�����ã�ֱ�Ӵ������Ĳ�ѯ�������
            
            #endregion

			
			
			if(re==null || re.CanRead==false)
			{
				//��ѯ���ս��
                strWhere += " AND flowid = -1 ";
			}
			else
			{
				#region ��Χ����
                if (re != null)
                {
                    switch (re.RightRange)
                    {
                        case eO_RightRange.eFull:
                            strWhere += "";
                            break;
                        case eO_RightRange.ePersonal:
                            strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_BYTS.flowid AND receiverid = " + lngUserID.ToString() + ")";
                            break;
                        case eO_RightRange.eDeptDirect:
                            strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_BYTS.flowid AND recdeptid = " + lngDeptID.ToString() + ")";
                            break;
                        case eO_RightRange.eOrgDirect:
                            strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_BYTS.flowid AND recorgid = " + lngOrgID.ToString() + ")";
                            break;
                        case eO_RightRange.eDept:
                            strList = DeptDP.GetDeptFullID(lngDeptID);
                            if (strList.Trim().Length > 0)
                            {
                                //���Ǹ����Ų����ҵ�
                                strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_BYTS.flowid AND recdeptid in (select deptid from ts_dept where fullid like "+StringTool.SqlQ(  strList+ " % ")+"))";
                            }
                            break;
                        case eO_RightRange.eOrg:
                            strList = DeptDP.GetDeptFullID(lngOrgID);
                            if (strList.Trim().Length > 0)
                            {
                                //���Ǹ����Ų����ҵ�
                                strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_BYTS.flowid AND recorgid in (select distinct deptid from ts_dept where deptkind = 1 and fullid like "+StringTool.SqlQ(  strList+ " % ")+"))";
                            }
                            break;
                        default:
                            strWhere += "";
                            break;
                    }
                }
				#endregion
                
			}
            strWhere += pWhere;
            OracleConnection cn = ConfigTool.GetConnection();
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, "V_BYTS", "*", " ORDER BY BY_ID DESC", pagesize, pageindex,strWhere, ref rowcount);
            ConfigTool.CloseConnection(cn);
            return dt;
		}




        #region �ͻ�������Ͷ��
        /// <summary>
        /// ���ݣأ̴ͣ�������ȡ�����¼�����
        /// </summary>
        /// <param name="strXmlcond"></param>
        /// <param name="lngUserID"></param>
        /// <param name="lngDeptID"></param>
        /// <param name="lngOrgID"></param>
        /// <param name="re"></param>
        /// <param name="pWhere"></param>
        /// <returns></returns>
        public static DataTable GetIssuesForCond(string strXmlcond, long lngUserID, long lngDeptID, long lngOrgID, RightEntity re)
        {
            string strSQL = "";        //���SQL�ַ���
            string strList = "";       //��Ų����б�
            string strWhere = "";        //�������

            if (re != null && re.CanRead == true && !string.IsNullOrEmpty(strXmlcond))
            {
                #region ��ȡ��ѯ������ֵ
                FieldValues fv = new FieldValues(strXmlcond);
                if (fv.GetFieldValue("FlowID").Value != "0")
                {
                    strWhere += " And a.flowid != " + fv.GetFieldValue("FlowID").Value.ToString().Trim();
                }
                if (fv.GetFieldValue("CustID").Value != "0" && fv.GetFieldValue("CustID").Value != "-1")
                    strWhere += " And nvl(a.CustID,0) = " + fv.GetFieldValue("CustID").Value.ToString().Trim();
                #endregion
            }
            strSQL = @"SELECT  a.*,b.status,
					case when b.status=30 then 
					datediff(Minute,nvl(b.endtime,sysdate),nvl(b.expectendtime,sysdate))
					else 
					datediff('Minute',sysdate,nvl(b.expectendtime,sysdate)) end FlowDiffMinute
					FROM Cst_BYTS a,es_flow b 
					WHERE a.FlowID = b.FlowID  ";

            if (re == null || re.CanRead == false)
            {
                //��ѯ���ս��
                strSQL += " AND a.flowid = -1 ";
            }
            else
            {
                strSQL = strSQL + strWhere;

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
                                strSQL += "AND exists (SELECT messageid FROM es_message WHERE flowid = b.flowid AND recdeptid in (select deptid from ts_dept where fullid like "+StringTool.SqlQ(  strList+ " % ")+"))";
                            }
                            break;
                        case eO_RightRange.eOrg:
                            strList = DeptDP.GetDeptFullID(lngOrgID);
                            if (strList.Trim().Length > 0)
                            {
                                //���Ǹ����Ų����ҵ�
                               strSQL += "AND exists (SELECT messageid FROM es_message WHERE flowid = b.flowid AND recorgid in (select distinct deptid from ts_dept where deptkind = 1 and fullid like "+StringTool.SqlQ(  strList+ " % ")+"))";
                            }
                            break;
                        default:
                            strSQL += "";
                            break;
                    }
                }
                #endregion
                strSQL = strSQL + " ORDER BY a.BY_ID DESC";
            }
            OracleConnection cn = ConfigTool.GetConnection();
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigTool.CloseConnection(cn);
            return dt;
        }
		#endregion

        #region �������ݴ���
        /// <summary>
        /// ��ȡͶ�ߵ����
        /// </summary>
        /// <returns></returns>
        public static DataTable GetBYTSYears()
        {
            string sSql = "select distinct datepart(year,RegTime) as years from cst_byts order by datepart(year,RegTime) desc ";

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
        /// ��ȡͶ�ߵ���������
        /// </summary>
        /// <param name="nYear"></param>
        /// <returns></returns>
        public static DataTable GetAnalysisBYTSDirectionGrid(int nYear)
        {
            string sSql = @"SELECT month(RegTime) as months,count(BY_ID) as qty" +
                " FROM Cst_BYTS " +
                " WHERE year(RegTime) = " + nYear.ToString() +
                " GROUP BY month(RegTime) ";
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
        /// ��ȡ�¼��漰�Ĺ���
        /// </summary>
        /// <returns></returns>
        public static DataTable GetManageOffices()
        {
            string sSql = "SELECT deptid,deptname FROM ts_dept WHERE deptid in (select distinct BY_Project from Cst_BYTS)";

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
        /// ����Ͷ���¼��������Ʒ���
        /// </summary>
        /// <param name="nYear"></param>
        /// <param name="lngDeptID"></param>
        /// <param name="lngBYKind"></param>
        /// <returns></returns>
        public static DataTable GetAnalysisBYTSDirection(int nYear, long lngDeptID,long lngBYKind)
        {
            string sSql = "";

            OracleConnection cn = ConfigTool.GetConnection();

            sSql = @"SELECT	d.by_kindname as  Ͷ������, '1��'=sum(case  when d.months=1 then d.qty else 0 end),
								'2��'=sum(case  when d.months=2 then d.qty else 0 end),
								'3��'=sum(case  when d.months=3 then d.qty else 0 end),
								'4��'=sum(case  when d.months=4 then d.qty else 0 end),
								'5��'=sum(case  when d.months=5 then d.qty else 0 end),
								'6��'=sum(case  when d.months=6 then d.qty else 0 end),
								'7��'=sum(case  when d.months=7 then d.qty else 0 end),
								'8��'=sum(case  when d.months=8 then d.qty else 0 end),
								'9��'=sum(case  when d.months=9 then d.qty else 0 end),
								'10��'=sum(case  when d.months=10 then d.qty else 0 end),
								'11��'=sum(case  when d.months=11 then d.qty else 0 end),
								'12��'=sum(case  when d.months=12 then d.qty else 0 end)
						FROM (

						SELECT month(a.RegTime) as months , a.by_kindname + '(' + TO_CHAR(a.by_kind) + ')' as by_kindname ,count(a.BY_ID) as qty 
						FROM Cst_BYTS a

						WHERE   year(a.RegTime) = " + nYear.ToString() +
                           ((lngDeptID == 0) ? "" : " AND a.BY_Project =" + lngDeptID + "") +
                           ((lngBYKind == 0 || lngBYKind == -1) ? "" : " AND a.BY_Kind =" + lngBYKind + "") +
                @" GROUP BY month(a.RegTime) , a.by_kindname + '(' + TO_CHAR(a.by_kind) + ')' ) d  GROUP BY d.by_kindname";
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
        /// Ͷ����Դ�ֲ�ͼ
        /// </summary>
        /// <param name="strBegin"></param>
        /// <param name="strEnd"></param>
        /// <param name="lngDeptID"></param>
        /// <returns></returns>
        public static DataTable GetAnalysisBYTSBySoure(string strBegin, string strEnd, long lngDeptID)
        {
            string sSql = "";

            OracleConnection cn = ConfigTool.GetConnection();

            sSql = @"SELECT count(a.BY_ID) as ���� ,a.By_SoureName as Ͷ����Դ
						FROM Cst_BYTS a

						WHERE   a.RegTime Between " + StringTool.SqlQ(strBegin) + " AND " + StringTool.SqlQ(strEnd + " 23:59:59") +
                           ((lngDeptID == 0) ? "" : " AND a.BY_Project =" + lngDeptID + "") +
                @" GROUP BY a.By_SoureName ";

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
        /// Ͷ�����ʷֲ�ͼ
        /// </summary>
        /// <param name="strBegin"></param>
        /// <param name="strEnd"></param>
        /// <param name="lngDeptID"></param>
        /// <returns></returns>
        public static DataTable GetAnalysisBYTSByKind(string strBegin, string strEnd, long lngDeptID)
        {
            string sSql = "";

            OracleConnection cn = ConfigTool.GetConnection();

            sSql = @"SELECT count(a.BY_ID) as ���� ,a.BY_KindName as Ͷ������
						FROM Cst_BYTS a

						WHERE   a.RegTime Between " + StringTool.SqlQ(strBegin) + " AND " + StringTool.SqlQ(strEnd + " 23:59:59") +
                           ((lngDeptID == 0) ? "" : " AND a.BY_Project =" + lngDeptID + "") +
                @" GROUP BY a.BY_KindName ";

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
        /// Ͷ�߲��ŷֲ�ͼ
        /// </summary>
        /// <param name="strBegin"></param>
        /// <param name="strEnd"></param>
        /// <param name="lngDeptID"></param>
        /// <param name="lngBYKind"></param>
        /// <returns></returns>
        public static DataTable GetAnalysisBYTSByProject(string strBegin, string strEnd, long lngDeptID, long lngBYKind)
        {
            string sSql = "";

            OracleConnection cn = ConfigTool.GetConnection();

            sSql = @"SELECT count(a.BY_ID) as ���� ,a.By_ProjectName as ��Ͷ����
						FROM Cst_BYTS a

						WHERE   a.RegTime Between " + StringTool.SqlQ(strBegin) + " AND " + StringTool.SqlQ(strEnd + " 23:59:59") +
                           ((lngDeptID == 0) ? "" : " AND a.BY_Project =" + lngDeptID + "") +
                           ((lngBYKind == 0 || lngBYKind == -1) ? "" : " AND a.BY_Kind =" + lngBYKind + "") +
                @" GROUP BY a.By_ProjectName ";

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
    }
}
