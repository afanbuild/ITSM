/*******************************************************************
 *
 * Description:单点登录
 * 
 * 
 * Create By  :
 * Create Date:2008年7月30日
 * *****************************************************************/
using System;
using System.Data;
using System.Xml;
using System.Data.OracleClient;
using Epower.DevBase.BaseTools;
using Epower.ITSM.Base;
using Epower.DevBase.Organization.Base;

namespace Epower.ITSM.SqlDAL
{

    /// <summary>
    /// 令牌意图解析类
    /// </summary>
    public class TokenIntention
    {
        private string _Type = "0";
        private string _Url = "";

        private string _InnerText = "";

        /// <summary>
        /// 意图类别
        /// </summary>
        public string IntentionType
        {
            set { _Type = value; }
            get { return _Type; }
        }

        /// <summary>
        /// 转接的ＵＲＬ
        /// </summary>
        public string IntentionUrl
        {
            set { _Url = value; }
            get { return _Url; }
        }

        public TokenIntention()
        {
        }


        public TokenIntention(string strXMl)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(strXMl);


            _Type = xmlDoc.SelectSingleNode("/Intention/IntentionType").InnerText;
            _Url = xmlDoc.SelectSingleNode("/Intention/IntentionUrl").InnerText;

            xmlDoc = null;

        }

        /// <summary>
        /// 输出ＸＭＬ
        /// </summary>
        /// <returns></returns>
        public string ToXml()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement xmlRoot = xmlDoc.CreateElement("Intention");
            XmlElement xmlEle;


            xmlEle = xmlDoc.CreateElement("IntentionType");
            xmlEle.InnerText = _Type;
            xmlRoot.AppendChild(xmlEle);

            xmlEle = xmlDoc.CreateElement("IntentionUrl");
            xmlEle.InnerText = _Url;
            xmlRoot.AppendChild(xmlEle);



            xmlDoc.AppendChild(xmlRoot);
            return xmlDoc.InnerXml;

        }




    }

    /// <summary>
    /// 令牌处理类
    /// </summary>
    public class APTokenDP
    {




        /// <summary>
        /// 保存处理意图的令牌
        /// </summary>
        /// <param name="strIntention">xml表示程序意图</param>
        /// <param name="lngUserID">用户ID</param>
        /// <param name="lngMNUserID">模拟的用户ID</param>
        /// <returns></returns>
         public static string SaveTokenInfo(string strIntention,long lngUserID,long lngMNUserID)
         {
              
                  OracleConnection con = ConfigTool.GetConnection();
                  try
                  {
                      string strGuid = System.Guid.NewGuid().ToString();
                      string strSql = "Insert Into Ts_APToken(TokenID,UserID,mnuserid,intention,CreatTime,TokenStatus)  Values(" +
                                      StringTool.SqlQ(strGuid) +"," +
                                      lngUserID.ToString() + "," +
                                      lngMNUserID.ToString() + "," +
                                      StringTool.SqlQ(strIntention) + ",sysdate,0)";
                      OracleDbHelper.ExecuteNonQuery(con, CommandType.Text, strSql);

                      return strGuid;
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
         /// 获取之前保存的程序意图(用户需要匹配,并且未处理过)
         /// 获取时保存校验信息
        /// </summary>
        /// <param name="lngUserID"></param>
        /// <param name="strGuid"></param>
        /// <param name="strIP"></param>
        /// <param name="lngmnUser">返回模拟用户的ID</param>
         /// <returns>返回空表示没有合法的权限或不存在</returns>
        public static string GetTokenInfo(long lngUserID, string strGuid,string strIP,ref long lngmnUser)
        {
            OracleConnection con = ConfigTool.GetConnection();
            string strIntention = "";
            if (string.IsNullOrEmpty(strGuid))
            {
                return "";
            }
            try
            {
                string strSql = "Select intention,mnuserid From Ts_APToken Where Tokenid = " + StringTool.SqlQ(strGuid) + "  And userID = " +
                                 lngUserID.ToString() + "  And tokenstatus = 0" ;
                DataTable dt = OracleDbHelper.ExecuteDataTable(con, CommandType.Text, strSql);
                if (dt.Rows.Count > 0)
                {
                    strIntention = dt.Rows[0][0].ToString();
                    lngmnUser = long.Parse(dt.Rows[0][1].ToString());
                    if (lngmnUser == lngUserID)
                    {
                        //当模拟用户不为指定ID时,可以无限次进入页面
                        strSql = "update Ts_APToken set " +
                                 " tokenstatus = 1,validatetime = sysdate, " +
                                 "matchip = " + StringTool.SqlQ(strIP) +
                                "Where Tokenid = " + StringTool.SqlQ(strGuid);
                        OracleDbHelper.ExecuteNonQuery(con, CommandType.Text, strSql);
                    }
                    
                    return strIntention;
                }
                else
                {
                    return "";
                }
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
        /// 获取之前保存的程序意图(用户需要匹配,并且未处理过)
        /// 获取时保存校验信息，不限制登录次数
        /// </summary>
        /// <param name="lngUserID"></param>
        /// <param name="strGuid"></param>
        /// <param name="strIP"></param>
        /// <param name="lngmnUser">返回模拟用户的ID</param>
        /// <returns>返回空表示没有合法的权限或不存在</returns>
        public static string GetTokenInfo20111104(long lngUserID, string strGuid, string strIP, ref long lngmnUser)
        {
            OracleConnection con = ConfigTool.GetConnection();
            string strIntention = "";
            if (string.IsNullOrEmpty(strGuid))
            {
                return "";
            }
            try
            {
                string strSql = "Select intention,mnuserid From Ts_APToken Where Tokenid = " + StringTool.SqlQ(strGuid) + "  And userID = " +
                                 lngUserID.ToString() + "  And tokenstatus = 0";
                DataTable dt = OracleDbHelper.ExecuteDataTable(con, CommandType.Text, strSql);
                if (dt.Rows.Count > 0)
                {
                    strIntention = dt.Rows[0][0].ToString();
                    lngmnUser = long.Parse(dt.Rows[0][1].ToString());
                    return strIntention;
                }
                else
                {
                    return "";
                }
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
        /// 设置令牌不可用
        /// </summary>
        /// <param name="strGuid"></param>
        /// <param name="strIP"></param>
        public static void SetAPTokenSatus(string strGuid, string strIP)
        {
            string strSql = "update Ts_APToken set " +
                                 " tokenstatus = 1,validatetime = sysdate, " +
                                 " matchip = " + StringTool.SqlQ(strIP) +
                                " Where Tokenid = " + StringTool.SqlQ(strGuid);
            CommonDP.ExcuteSql(strSql);
        }


    }

  
}
