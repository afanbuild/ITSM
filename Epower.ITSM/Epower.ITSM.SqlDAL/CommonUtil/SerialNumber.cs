using System;
using System.Collections.Generic;
using System.Text;
using Epower.DevBase.BaseTools;
using System.Data.OracleClient;
using System.Data;

namespace Epower.ITSM.SqlDAL.CommonUtil
{
    public class SerialNumber
    {
        private SerialNumber()
        {

        }
        #region 查询系列号
        public static int GetNextval(string seqName)
        {
            int nextval = 0;
            string sSql = string.Format("select {0}.Nextval from dual", seqName);
            OracleConnection cn = ConfigTool.GetConnection();
            try
            {
                OracleDataReader dr = OracleDbHelper.ExecuteReader(cn, CommandType.Text, sSql);
                while (dr.Read())
                {
                    nextval = Int32.Parse(dr["NextVal"].ToString());

                }
                dr.Close();
                return nextval;
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
