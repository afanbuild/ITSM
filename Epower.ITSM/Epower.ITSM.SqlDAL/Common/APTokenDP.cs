/*******************************************************************
 *
 * Description:�����¼
 * 
 * 
 * Create By  :
 * Create Date:2008��7��30��
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
    /// ������ͼ������
    /// </summary>
    public class TokenIntention
    {
        private string _Type = "0";
        private string _Url = "";

        private string _InnerText = "";

        /// <summary>
        /// ��ͼ���
        /// </summary>
        public string IntentionType
        {
            set { _Type = value; }
            get { return _Type; }
        }

        /// <summary>
        /// ת�ӵģգң�
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
        /// ����أͣ�
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
    /// ���ƴ�����
    /// </summary>
    public class APTokenDP
    {




        /// <summary>
        /// ���洦����ͼ������
        /// </summary>
        /// <param name="strIntention">xml��ʾ������ͼ</param>
        /// <param name="lngUserID">�û�ID</param>
        /// <param name="lngMNUserID">ģ����û�ID</param>
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
         /// ��ȡ֮ǰ����ĳ�����ͼ(�û���Ҫƥ��,����δ�����)
         /// ��ȡʱ����У����Ϣ
        /// </summary>
        /// <param name="lngUserID"></param>
        /// <param name="strGuid"></param>
        /// <param name="strIP"></param>
        /// <param name="lngmnUser">����ģ���û���ID</param>
         /// <returns>���ؿձ�ʾû�кϷ���Ȩ�޻򲻴���</returns>
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
                        //��ģ���û���Ϊָ��IDʱ,�������޴ν���ҳ��
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
        /// ��ȡ֮ǰ����ĳ�����ͼ(�û���Ҫƥ��,����δ�����)
        /// ��ȡʱ����У����Ϣ�������Ƶ�¼����
        /// </summary>
        /// <param name="lngUserID"></param>
        /// <param name="strGuid"></param>
        /// <param name="strIP"></param>
        /// <param name="lngmnUser">����ģ���û���ID</param>
        /// <returns>���ؿձ�ʾû�кϷ���Ȩ�޻򲻴���</returns>
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
        /// �������Ʋ�����
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
