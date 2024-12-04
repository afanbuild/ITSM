
/*******************************************************************
 * 版权所有：
 * Description：传入表名称取表结datatable
 * 
 * 
 * Create By  ：yanghw
 * Create Date：2007-07-23
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
using System.Data.OracleClient;

namespace Epower.ITSM.SqlDAL.XmlHttp
{
    public class XmlHttpTable
    {

        public XmlHttpTable()
		{
			//
			// TODO: 在此处添加构造函数逻辑
			//
        }


        public static DataTable GETdatetable(string TBL,string sWhere)
        {
            string strSQL = "select * from " + TBL + " where 1=1 " + sWhere;
            OracleConnection cn = ConfigTool.GetConnection();
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigTool.CloseConnection(cn);
            return dt;
        }

        public static DataTable GETdatetableChane(string StrId)
        {
            string strSQL = @"
                    select A.*, B.ShortName,B.Address,B.LinkMan1,B.Tel1,B.Email,B.mastcustname,B.customCode,
                    b.job,b.CustDeptName,b.ShortName,b.Address,b.LinkMan1,b.Tel1,b.customCode,b.Email,b.mastcustname
                     from Equ_ChangeService A
                    left join  Br_ECustomer B
                    on A.CustId =B.Id
                     
                    where A.ID=" + StrId;
            OracleConnection cn = ConfigTool.GetConnection();
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigTool.CloseConnection(cn);
            return dt;
        }

    }
}
