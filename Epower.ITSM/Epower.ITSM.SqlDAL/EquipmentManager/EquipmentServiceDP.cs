/****************************************************************************
 * 
 * description:�ʲ�ͳ�Ʒ������ݴ�����
 * 
 * 
 * 
 * Create by:
 * Create Date:2007-10-09
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
    /// EquipmentServiceDP ��ժҪ˵����
	/// </summary>
    public class EquipmentServiceDP
	{
		public EquipmentServiceDP()
		{
			//
			// TODO: �ڴ˴���ӹ��캯���߼�
			//
        }

        #region ��ȡ�ʲ���� GetEquipmentType
        /// <summary>
        /// ��ȡ�ʲ����
        /// </summary>
        /// <returns></returns>
        public static DataTable GetEquipmentType()
        {
            string sSql = "select CatalogID,CatalogName,sortid from Equ_Category where ParentID=1 and Deleted=0";

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

        #region �����ʲ�����¼��������Ʒ���
        /// <summary>
        /// �����ʲ�����¼��������Ʒ���
        /// </summary>
        /// <param name="nYear"></param>
        /// <param name="lngEpuipmentTypeID"></param>
        /// <param name="lngDeptID"></param>
        /// <returns></returns>
        public static DataTable GetAnalysisEquipmentTypes(int nYear, long lngEpuipmentTypeID, long lngDeptID, long lngMastCustomer)
        {
            string sSql = "";

            OracleConnection cn = ConfigTool.GetConnection();

            sSql = @"SELECT	 e.CatalogName + '(' + d.EquTypeID + ')' as �ʲ������,e.CatalogName �ʲ����,
								'1��'=sum(case  when d.months=1 then d.qty else 0 end),
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
						       SELECT month(a.regsysdate) as months,case when FullID='' Then '1' else substring(b.FullID,2,5) end EquTypeID,count(a.smsid) as qty 
						    FROM cst_issues a,Equ_Desk b where a.Equipmentid=b.ID 
						    and  year(a.regsysdate) = " + nYear.ToString() +
                           ((lngEpuipmentTypeID == 0) ? "" : " AND substring(b.FullID,2,5) =" + lngEpuipmentTypeID + "") +
                           ((lngDeptID == 0) ? "" : " AND a.orgid =" + lngDeptID + "");
            if (lngMastCustomer != 0)
                sSql += " And nvl(a.CustID,0) In (select ID from Br_ECustomer where MastCustID=" + lngMastCustomer.ToString() + ")";

            sSql +=@" GROUP BY month(a.regsysdate),case when FullID='' Then '1' else substring(b.FullID,2,5) end) d,Equ_Category e where d.EquTypeID=to_number(e.catalogID)
                        	GROUP BY d.EquTypeID,e.CatalogName";

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

        #region ��ȡ�ʲ��ܵ��¼����� GetEquipmentAllCount
        /// <summary>
        /// ��ȡ�ʲ��ܵ��¼�����
        /// </summary>
        /// <param name="nYear"></param>
        /// <returns></returns>
        public static DataTable GetEquipmentAllCount(int nYear)
        {
            string sSql = @"SELECT month(regsysdate) as months,count(smsid) as qty" +
                " FROM cst_issues " +
                " Where Equipmentid!=0 and year(regsysdate) = " + nYear.ToString() +
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
        #endregion 
    }
}
