/*******************************************************************
 *
 * Description:习惯用语数据处理类
 * 
 * 
 * Create By  :
 * Create Date:2008年7月30日
 * *****************************************************************/
using System;
using System.Xml;
using System.Data;
using System.Data.OracleClient;
using Epower.DevBase.BaseTools;
using Epower.ITSM.Base;

namespace Epower.ITSM.SqlDAL
{
	/// <summary>
	/// SMSDp 的摘要说明。
	/// </summary>
	public class IdiomDP
	{
		public IdiomDP()
		{
			//
			// TODO: 在此处添加构造函数逻辑
			//
		}

        /// <summary>
        /// 根据当前拼音输入，获取可以选择的列表
        /// </summary>
        /// <param name="intNum"></param>
        /// <param name="id"></param>
        /// <param name="sCurr"></param>
        /// <returns></returns>
        public static string GetPinYinIdiom(int intNum, string id,string sCurr)
        {
            string sReturn = "";
            string strSQL = "";
            //暂时是在知识库的关键字
            if (StringTool.isIncludeChina(sCurr))
            {
                //包含中文仅匹配关键字
                strSQL = "SELECT  KeyWord,QueryCount " +
                    " FROM inf_tags WHERE ROWNUM<=" + intNum.ToString() + " AND KeyWord like " + StringTool.SqlQ(sCurr + "%");
            }
            else
            {
                //不包含中文
                strSQL = "SELECT KeyWord,QueryCount " +
                    " FROM inf_tags WHERE ROWNUM<=" + intNum.ToString() + " AND firstpy like " + StringTool.SqlQ(sCurr + "%") + " OR py like " + StringTool.SqlQ(sCurr + "%") + " OR KeyWord like " + StringTool.SqlQ(sCurr + "%");
            }
            strSQL += " ORDER BY querycount DESC";

            DataTable dt;
            OracleConnection cn = ConfigTool.GetConnection();

            try {
                dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);

                if (dt.Rows.Count > 0)
                {

                    XmlDocument xmlDoc = new XmlDocument();

                    xmlDoc.LoadXml(@"<AllFields></AllFields>");
                    foreach (DataRow row in dt.Rows)
                    {

                        XmlElement xmlEle = xmlDoc.CreateElement("Field");
                        xmlEle.SetAttribute("Text", row["KeyWord"].ToString().Trim().PadRight((50 - row["KeyWord"].ToString().Trim().Length > 0 ? 50 - row["KeyWord"].ToString().Trim().Length : 2), char.Parse(" ")) + row["querycount"].ToString().Trim() + "次查询");
                        xmlDoc.DocumentElement.AppendChild(xmlEle);
                    }

                    sReturn = xmlDoc.InnerXml;


                }                

                return sReturn;
            }
            finally { ConfigTool.CloseConnection(cn); }
        }

        /// <summary>
        /// 获取知识库关键快速查询列表
        /// </summary>
        /// <param name="intNum"></param>
        /// <returns></returns>
        public static string GetInfoTagsIdiom(int intNum)
        {
            string sReturn = "";
            string strSQL = "SELECT KeyWord,QueryCount " +
                " FROM inf_tags WHERE ROWNUM<=" + intNum.ToString();
            strSQL += " ORDER BY querycount DESC";
            DataTable dt;
            OracleConnection cn = ConfigTool.GetConnection();

            try
            {
                dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);

                if (dt.Rows.Count > 0)
                {

                    XmlDocument xmlDoc = new XmlDocument();

                    xmlDoc.LoadXml(@"<AllFields></AllFields>");
                    foreach (DataRow row in dt.Rows)
                    {

                        XmlElement xmlEle = xmlDoc.CreateElement("Field");
                        xmlEle.SetAttribute("Text", row["KeyWord"].ToString().Trim().PadRight((50 - row["KeyWord"].ToString().Trim().Length > 0 ? 50 - row["KeyWord"].ToString().Trim().Length : 2), char.Parse(" ")) + row["querycount"].ToString().Trim() + "次查询");
                        xmlDoc.DocumentElement.AppendChild(xmlEle);

                    }

                    sReturn = xmlDoc.InnerXml;


                }            

                return sReturn;
            }
            finally { ConfigTool.CloseConnection(cn); }
        }

		/// <summary>
        /// 获取相关习惯用语
		/// </summary>
		/// <param name="lngUserID"></param>
		/// <param name="intNum"></param>
		/// <param name="sFieldID"></param>
		/// <returns></returns>
		public static string  GetIdiom(long lngUserID,int intNum,string sFieldID)
		{
            string sReturn = "";
            string strSQL = "SELECT FieldValue " +
                " FROM ea_idiom " +
                " WHERE ROWNUM<=" + intNum.ToString() + " AND FieldID = " + StringTool.SqlQ(sFieldID);
            strSQL += " ORDER BY indate DESC";
			DataTable dt;
			OracleConnection cn = ConfigTool.GetConnection();

            try
            {
                dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);

                if (dt.Rows.Count > 0)
                {

                    XmlDocument xmlDoc = new XmlDocument();

                    xmlDoc.LoadXml(@"<AllFields></AllFields>");
                    foreach (DataRow row in dt.Rows)
                    {

                        XmlElement xmlEle = xmlDoc.CreateElement("Field");
                        xmlEle.SetAttribute("Text", row["FieldValue"].ToString().Trim());
                        xmlDoc.DocumentElement.AppendChild(xmlEle);

                    }

                    sReturn = xmlDoc.InnerXml;


                }                

                return sReturn;
            }
            finally { ConfigTool.CloseConnection(cn); }
		}
	

		
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="lngUserID"></param>
        /// <param name="strFieldID"></param>
        /// <param name="sFieldValue"></param>
        public static void AddIdiom(long lngUserID, string sFieldID, string sFieldValue)
		{
            //空值直接退出
            if (sFieldValue.Trim().Length == 0)
                return;


			string strSQL = "";
			OracleConnection cn = ConfigTool.GetConnection();
			try
			{
                strSQL = "SELECT count(*) FROM ea_idiom WHERE fieldid = " + StringTool.SqlQ(sFieldID) + " AND fieldvalue = " + StringTool.SqlQ(sFieldValue);
                int i = Convert.ToInt32(OracleDbHelper.ExecuteScalar(cn, CommandType.Text, strSQL));
                if(i == 0)
                {
                    string strID = EpowerGlobal.EPGlobal.GetNextID("ea_idiom_SEQUENCE").ToString();

                    //不存在则添加
                    strSQL = @"INSERT INTO ea_idiom (SID,FieldID,FieldValue,UsageCount,UserID,InDate) VALUES(" + strID + "," +
                        StringTool.SqlQ(sFieldID) + "," +
                        StringTool.SqlQ(sFieldValue) + ",0," +
                        lngUserID.ToString() + "," +
                       "sysdate)";

				    OracleDbHelper.ExecuteNonQuery(cn,CommandType.Text,strSQL);
                }
			}
			catch(Exception err)
			{
				throw new Exception(err.Message );
			}
			finally
			{
				ConfigTool.CloseConnection(cn);
			}

		}



	}
}

