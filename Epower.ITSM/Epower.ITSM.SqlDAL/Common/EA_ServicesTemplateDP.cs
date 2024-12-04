/*******************************************************************
 *
 * Description:模板
 * 
 * 
 * Create By  :zmc
 * Create Date:2008年8月27日
 * *****************************************************************/
using System;
using System.Data;
using System.Data.OracleClient;
using Epower.DevBase.BaseTools;
using Epower.ITSM.Base;
using Epower.DevBase.Organization.SqlDAL;
using Epower.DevBase.Organization.Base;

using EpowerCom;
using System.Xml;
using System.IO;
using EpowerGlobal;

namespace Epower.ITSM.SqlDAL
{
    /// <summary>
    /// 
    /// </summary>
    public class EA_ServicesTemplateDP
    {
        /// <summary>
        /// 
        /// </summary>
        public EA_ServicesTemplateDP()
        { }

        #region Property
        #region TemplateID
        /// <summary>
        ///
        /// </summary>
        private Decimal mTemplateID;
        public Decimal TemplateID
        {
            get { return mTemplateID; }
            set { mTemplateID = value; }
        }
        #endregion

        #region TemplateType
        /// <summary>
        ///
        /// </summary>
        private Int32 mTemplateType;
        public Int32 TemplateType
        {
            get { return mTemplateType; }
            set { mTemplateType = value; }
        }
        #endregion

        #region OFlowModelID
        /// <summary>
        ///
        /// </summary>
        private Decimal mOFlowModelID;
        public Decimal OFlowModelID
        {
            get { return mOFlowModelID; }
            set { mOFlowModelID = value; }
        }
        #endregion

        #region ServiceLevelID
        /// <summary>
        ///
        /// </summary>
        private Decimal mServiceLevelID;
        public Decimal ServiceLevelID
        {
            get { return mServiceLevelID; }
            set { mServiceLevelID = value; }
        }
        #endregion

        #region ServiceKindID
        /// <summary>
        ///
        /// </summary>
        private Decimal mServiceKindID;
        public Decimal ServiceKindID
        {
            get { return mServiceKindID; }
            set { mServiceKindID = value; }
        }
        #endregion

        #region ServiceLevel
        /// <summary>
        ///
        /// </summary>
        private String mServiceLevel = string.Empty;
        public String ServiceLevel
        {
            get { return mServiceLevel; }
            set { mServiceLevel = value; }
        }
        #endregion

        #region ServiceKind
        /// <summary>
        ///
        /// </summary>
        private String mServiceKind = string.Empty;
        public String ServiceKind
        {
            get { return mServiceKind; }
            set { mServiceKind = value; }
        }
        #endregion

        #region Guide

        /// <summary>
        ///
        /// </summary>
        private String mGuide = string.Empty;
        public String Guide
        {
            get { return mGuide; }
            set { mGuide = value; }
        }

        #endregion

        #region Content
        /// <summary>
        ///
        /// </summary>
        private String mContent = string.Empty;
        public String Content
        {
            get { return mContent; }
            set { mContent = value; }
        }
        #endregion

        #region TemplateName
        /// <summary>
        ///
        /// </summary>
        private String mTemplateName = string.Empty;
        public String TemplateName
        {
            get { return mTemplateName; }
            set { mTemplateName = value; }
        }
        #endregion

        #region TemplateXml
        /// <summary>
        ///
        /// </summary>
        private String mTemplateXml = string.Empty;
        public String TemplateXml
        {
            get { return mTemplateXml; }
            set { mTemplateXml = value; }
        }
        #endregion

        #region Owner
        /// <summary>
        ///
        /// </summary>
        private Decimal mOwner;
        public Decimal Owner
        {
            get { return mOwner; }
            set { mOwner = value; }
        }
        #endregion

        #region imglogo
        /// <summary>
        ///  图标LOGO
        /// </summary>
        private String mimglogo = string.Empty;
        public String imglogo
        {
            get { return mimglogo; }
            set { mimglogo = value; }
        }
        #endregion

        #region IsParent
        /// <summary>
        /// 是否一级目录
        /// </summary>
        private Int32 mIsParent;
        public Int32 IsParent
        {
            get { return mIsParent; }
            set { mIsParent = value; }
        }
        #endregion


        #region IssTempID
        /// <summary>
        /// 事件模板ID
        /// </summary>
        private Decimal mIssTempID;
        public Decimal IssTempID
        {
            get { return mIssTempID; }
            set { mIssTempID = value; }
        }
        #endregion

        #region IssTempName
        /// <summary>
        /// 事件模板名称
        /// </summary>
        private String mIssTempName = string.Empty;
        public String IssTempName
        {
            get { return mIssTempName; }
            set { mIssTempName = value; }
        }
        #endregion

        #region
        /// <summary>
        ///  附件XML串
        /// </summary>
        private String mstrAttach = "<Attachments />";
        public String AttachXml
        {
            get { return mstrAttach; }
            set { mstrAttach = value; }
        }
        #endregion

        #region AppID
        private long mAppID;
        /// <summary>
        ///  应用ID
        /// </summary>
        public long AppID
        {
            get { return mAppID; }
            set { mAppID = value; }
        }
        #endregion

        #endregion

        #region GetReCorded
        /// <summary>
        /// 根据模板Id获取数据
        /// </summary>
        /// <param name="lngID"></param>
        /// <returns>EA_ServicesTemplateDP</returns>
        public EA_ServicesTemplateDP GetReCorded(long lngID)
        {
            EA_ServicesTemplateDP ee = new EA_ServicesTemplateDP();
            string strSQL = string.Empty;
            OracleConnection cn = ConfigTool.GetConnection();

            try
            {
                strSQL = "SELECT * FROM EA_ServicesTemplate WHERE TemplateID = " + lngID.ToString();
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
                
                foreach (DataRow dr in dt.Rows)
                {
                    ee.TemplateID = Decimal.Parse(dr["TemplateID"].ToString());
                    ee.TemplateType = Int32.Parse(dr["TemplateType"].ToString());
                    ee.OFlowModelID = Decimal.Parse(dr["OFlowModelID"].ToString());
                    if (dr["ServiceKindID"].ToString() != "")
                    {
                        ee.ServiceKindID = Decimal.Parse(dr["ServiceKindID"].ToString());
                    }
                    ee.ServiceLevel = dr["ServiceLevel"].ToString();
                    ee.ServiceLevelID = Decimal.Parse(dr["ServiceLevelID"].ToString());
                    ee.ServiceKind = dr["ServiceKind"].ToString();
                    ee.Guide = dr["Guide"].ToString();
                    ee.TemplateName = dr["TemplateName"].ToString();
                    ee.TemplateXml = dr["TemplateXml"].ToString();
                    ee.Content = dr["Content"].ToString();
                    ee.imglogo = dr["imglogo"].ToString();
                    ee.IsParent = Int32.Parse(dr["IsParent"].ToString() == "" ? "0" : dr["IsParent"].ToString());

                    ee.IssTempName = dr["IssTempName"].ToString();
                    ee.IssTempID = Decimal.Parse(dr["IssTempID"].ToString() == "" ? "0" : dr["IssTempID"].ToString());
                    ee.AppID = CTools.ToInt64(dr["AppID"].ToString());
                }
                return ee;
            }
            finally { ConfigTool.CloseConnection(cn); }
        }
        /// <summary>
        /// 根据模板名称获取数据
        /// </summary>
        /// <param name="templateName"></param>
        /// <returns>EA_ServicesTemplateDP</returns>
        public EA_ServicesTemplateDP GetReCorded(string templateName)
        {
            EA_ServicesTemplateDP ee = new EA_ServicesTemplateDP();
            string strSQL = string.Empty;
            OracleConnection cn = ConfigTool.GetConnection();
            try
            {
                strSQL = "SELECT * FROM EA_ServicesTemplate WHERE TemplateName = " + StringTool.SqlQ(templateName);
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
                
                foreach (DataRow dr in dt.Rows)
                {
                    ee.TemplateID = Decimal.Parse(dr["TemplateID"].ToString());
                    ee.TemplateType = Int32.Parse(dr["TemplateType"].ToString());
                    ee.OFlowModelID = Decimal.Parse(dr["OFlowModelID"].ToString());
                    if (dr["ServiceKindID"].ToString() != "")
                    {
                        ee.ServiceKindID = Decimal.Parse(dr["ServiceKindID"].ToString());
                    }
                    ee.ServiceLevel = dr["ServiceLevel"].ToString();
                    ee.ServiceLevelID = Decimal.Parse(dr["ServiceLevelID"].ToString());
                    ee.ServiceKind = dr["ServiceKind"].ToString();
                    ee.Guide = dr["Guide"].ToString();
                    ee.TemplateName = dr["TemplateName"].ToString();
                    ee.TemplateXml = dr["TemplateXml"].ToString();
                    ee.Content = dr["Content"].ToString();
                    ee.imglogo = dr["imglogo"].ToString();
                    ee.IsParent = Int32.Parse(dr["IsParent"].ToString() == "" ? "0" : dr["IsParent"].ToString());

                    ee.IssTempName = dr["IssTempName"].ToString();
                    ee.IssTempID = Decimal.Parse(dr["IssTempID"].ToString() == "" ? "0" : dr["IssTempID"].ToString());
                }
                return ee;
            }
            finally { ConfigTool.CloseConnection(cn); }
        }
        #endregion

        #region GetDataTable
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sWhere"></param>
        /// <param name="sOrder"></param>
        /// <returns>DataTable</returns>
        public DataTable GetDataTable(string sWhere, string sOrder)
        {
            string strSQL = string.Empty;
            OracleConnection cn = ConfigTool.GetConnection();

            try
            {
                strSQL = "SELECT * FROM EA_ServicesTemplate  Where 1=1 ";
                strSQL += sWhere;
                strSQL += sOrder;
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
                
                return dt;
            }
            finally { ConfigTool.CloseConnection(cn); }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sWhere"></param>
        /// <param name="sOrder"></param>
        /// <returns>DataTable</returns>
        public DataTable GetDataTables(string sWhere, string sOrder)
        {
            string strSQL = string.Empty;
            OracleConnection cn = ConfigTool.GetConnection();

            try
            {
                strSQL = "SELECT EA_ServicesTemplate.* FROM EA_ServicesTemplate  LEFT JOIN  EA_ShortCutTemplate  ON EA_ServicesTemplate.IssTempID=EA_ShortCutTemplate.TemplateID WHERE (Owner=3 or Owner=5) ";
                strSQL += sWhere;
                strSQL += sOrder;
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
                
                return dt;
            }
            finally { ConfigTool.CloseConnection(cn); }
        }
        #endregion


        #region 获取自助附件的信息 一条
        /// <summary>
        /// 获取自助附件的信息 一条
        /// </summary>
        /// <param name="lngKBID"></param>
        /// <returns></returns>
        public static DataTable GetAttachmentDT(decimal lngKBID)
        {
            string strSQL = string.Empty;
            OracleConnection cn = ConfigTool.GetConnection();

            try
            {
                strSQL = "SELECT a.FileID,a.FileName,a.SufName,a.Status,a.upTime,a.upUserID,b.Name as upUserName,nvl(a.requstFileId, '') requstFileId " +
                  "FROM EA_Services_Attachment a, Ts_User b " +
                  "WHERE a.upUserID = b.UserID " +
                  "AND a.Status <> 1 AND a.KBID = " + lngKBID + " AND nvl(a.deleted, 0) = 0 and rownum =1 ORDER BY a.FileID ";
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            
                return dt;
            }
            finally { ConfigTool.CloseConnection(cn); }
        }
        #endregion

        #region 获得自助附件的信息
        /// <summary>
        /// 获得自助附件的信息，以XML串表示
        /// </summary>
        /// <param name="lngKBID"></param>
        /// <returns></returns>
        public static string GetAttachmentXml(decimal lngKBID)
        {
            string strSQL = "";
            OracleDataReader dr;

            XmlDocument xmlDoc = new XmlDocument();
            XmlElement xmlElTmp;
            XmlElement xmlElSub;

            xmlElTmp = xmlDoc.CreateElement("Attachments");


            //添加附件信息  
            strSQL = "SELECT a.FileID,a.FileName,a.SufName,a.Status,a.upTime,a.upUserID,b.Name as upUserName ,nvl(a.requstFileId,'') requstFileId " +
                 " FROM EA_Services_Attachment a,Ts_User b " +
                 " WHERE a.upUserID = b.UserID AND a.Status <>" + (int)e_FileStatus.efsDeleted +
                 "		AND a.KBID =" + lngKBID.ToString() + " AND nvl(a.deleted,0)=" + (int)e_Deleted.eNormal + " ORDER BY a.FileID";

            OracleConnection cn = ConfigTool.GetConnection();
            try
            {
                dr = OracleDbHelper.ExecuteReader(cn, CommandType.Text, strSQL);
                while (dr.Read())
                {
                    xmlElSub = xmlDoc.CreateElement("Attachment");
                    xmlElSub.SetAttribute("FileID", ((long)dr.GetDecimal(0)).ToString());
                    xmlElSub.SetAttribute("FileName", dr.GetString(1));
                    xmlElSub.SetAttribute("SufName", dr.GetString(2));
                    xmlElSub.SetAttribute("Status", dr.GetInt32(3).ToString());
                    xmlElSub.SetAttribute("upTime", dr.GetDateTime(4).ToString());
                    xmlElSub.SetAttribute("upUserID", ((long)dr.GetDecimal(5)).ToString());
                    xmlElSub.SetAttribute("upUserName", dr.GetString(6));
                    xmlElSub.SetAttribute("replace", dr["requstFileId"].ToString());
                    xmlElTmp.AppendChild(xmlElSub);

                }
                dr.Close();

                
            }
            finally { ConfigTool.CloseConnection(cn); }


            xmlDoc.AppendChild(xmlElTmp);
            return xmlDoc.InnerXml;
        }
        #endregion

        #region 取附件名称
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lngFileID"></param>
        /// <param name="strMonthPath"></param>
        /// <returns></returns>
        public static string GetAttachmentName(long lngFileID, ref string strMonthPath)
        {
            string strFileName = string.Empty;
            string strSQL = string.Empty;
            OracleConnection cn = ConfigTool.GetConnection();

            try
            {
                strSQL = "SELECT FileName,nvl(MonthPath,'')  MonthPath FROM EA_Services_Attachment WHERE FileID=" + lngFileID.ToString();
                OracleDataReader dr = OracleDbHelper.ExecuteReader(cn, CommandType.Text, strSQL);
                while (dr.Read())
                {
                    strFileName = dr.GetString(0);
                    strMonthPath = dr.GetString(1);
                }
                dr.Close();
                
                return strFileName;
            }
            finally { ConfigTool.CloseConnection(cn); }
        }
        #endregion

        #region 获取服务项Json对象
        /// <summary>
        /// 获取服务项Json对象
        /// </summary>
        /// <param name="lngID">标示ID</param>
        /// <returns></returns>
        public static string GetJson(long lngID)
        {
            string strSQL = string.Empty;
            OracleConnection cn = ConfigTool.GetConnection();

            try
            {
                strSQL = strSQL = "SELECT * FROM EA_ServicesTemplate WHERE TemplateID = " + lngID.ToString();
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
                

                Json json = new Json(dt);
                return "{record:" + json.ToJson() + "}";
            }
            finally { ConfigTool.CloseConnection(cn); }
        }
        #endregion

        #region InsertRecorded
        /// <summary>
        /// 
        /// </summary>
        /// <param name=pEA_ServicesTemplateDP></param>
        public void InsertRecorded(EA_ServicesTemplateDP pEA_ServicesTemplateDP)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            if (cn.State == ConnectionState.Closed)
                cn.Open();
            OracleTransaction trans = cn.BeginTransaction();

            string strSQL = string.Empty;
            try
            {
                string strID = EpowerGlobal.EPGlobal.GetNextID("EA_ServicesTemplateID").ToString();
                pEA_ServicesTemplateDP.TemplateID = decimal.Parse(strID);
                strSQL = @"INSERT INTO EA_ServicesTemplate(
                                    TemplateID,
									TemplateType,
									OFlowModelID,
									TemplateName,
									TemplateXml,
									ServiceLevelID,
                                    ServiceLevel,
                                    Content,
                                    ServiceKindID,
                                    ServiceKind,
                                    Guide,
                                    imglogo,
                                    IsParent,
                                    IssTempID,
                                    IssTempName,
                                    AppID
					)
					VALUES( " + strID + "," +
                            pEA_ServicesTemplateDP.TemplateType.ToString() + "," +
                            pEA_ServicesTemplateDP.OFlowModelID.ToString() + "," +
                            StringTool.SqlQ(pEA_ServicesTemplateDP.TemplateName) + "," +
                            StringTool.SqlQ(pEA_ServicesTemplateDP.TemplateXml) + "," +
                            pEA_ServicesTemplateDP.ServiceLevelID.ToString() + "," +
                            StringTool.SqlQ(pEA_ServicesTemplateDP.ServiceLevel.ToString()) + "," +
                            StringTool.SqlQ(pEA_ServicesTemplateDP.Content.ToString()) + "," +
                            pEA_ServicesTemplateDP.ServiceKindID.ToString() + "," +
                            StringTool.SqlQ(pEA_ServicesTemplateDP.ServiceKind.ToString()) + "," +
                            StringTool.SqlQ(pEA_ServicesTemplateDP.Guide.ToString()) + "," +
                            StringTool.SqlQ(pEA_ServicesTemplateDP.imglogo.ToString()) + "," +
                            pEA_ServicesTemplateDP.IsParent.ToString() + "," +
                            pEA_ServicesTemplateDP.IssTempID.ToString() + "," +
                            StringTool.SqlQ(pEA_ServicesTemplateDP.IssTempName.ToString()) + "," +
                            pEA_ServicesTemplateDP.AppID.ToString() + 
                    ")";

                OracleDbHelper.ExecuteNonQuery(trans, CommandType.Text, strSQL);

                //保存附件
                SaveAttachments(trans, pEA_ServicesTemplateDP.TemplateID, pEA_ServicesTemplateDP.AttachXml);

                trans.Commit();
            }
            catch
            {
                trans.Rollback();
                throw;
            }
            finally
            {
                ConfigTool.CloseConnection(cn);
            }
        }
        #endregion

        #region UpdateRecorded
        /// <summary>
        /// 
        /// </summary>
        /// <param name=pEA_ServicesTemplateDP></param>
        public void UpdateRecorded(EA_ServicesTemplateDP pEA_ServicesTemplateDP)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            if (cn.State == ConnectionState.Closed)
                cn.Open();
            OracleTransaction tran = cn.BeginTransaction();

            string strSQL = string.Empty;
            try
            {
                strSQL = @"UPDATE EA_ServicesTemplate Set " +
                            " TemplateType = " + pEA_ServicesTemplateDP.TemplateType.ToString() + "," +
                            " OFlowModelID = " + pEA_ServicesTemplateDP.OFlowModelID.ToString() + "," +
                            " TemplateName = " + StringTool.SqlQ(pEA_ServicesTemplateDP.TemplateName) + "," +
                            " TemplateXml = " + StringTool.SqlQ(pEA_ServicesTemplateDP.TemplateXml) + "," +
                            " ServiceLevelID = " + pEA_ServicesTemplateDP.ServiceLevelID.ToString() + "," +
                            " ServiceLevel = " + StringTool.SqlQ(pEA_ServicesTemplateDP.ServiceLevel.ToString()) + "," +
                            " Content = " + StringTool.SqlQ(pEA_ServicesTemplateDP.Content.ToString()) + "," +
                            " ServiceKindID = " + pEA_ServicesTemplateDP.ServiceKindID.ToString() + "," +
                            " ServiceKind = " + StringTool.SqlQ(pEA_ServicesTemplateDP.ServiceKind.ToString()) + "," +
                            " Guide=" + StringTool.SqlQ(pEA_ServicesTemplateDP.Guide.ToString()) + "," +
                            " imglogo = " + StringTool.SqlQ(pEA_ServicesTemplateDP.imglogo.ToString()) + "," +
                            " IsParent = " + pEA_ServicesTemplateDP.IsParent.ToString() + "," +
                            " IssTempID = " + pEA_ServicesTemplateDP.IssTempID.ToString() + "," +
                            " IssTempName = " + StringTool.SqlQ(pEA_ServicesTemplateDP.IssTempName.ToString()) + "," +
                            " AppID = " + pEA_ServicesTemplateDP.AppID.ToString() + 
                                " WHERE TemplateID = " + pEA_ServicesTemplateDP.TemplateID.ToString();

                OracleDbHelper.ExecuteNonQuery(tran, CommandType.Text, strSQL);

                SaveAttachments(tran, pEA_ServicesTemplateDP.TemplateID, pEA_ServicesTemplateDP.AttachXml);

                tran.Commit();
            }
            catch
            {
                tran.Rollback();
                throw;
            }
            finally
            {
                ConfigTool.CloseConnection(cn);
            }
        }
        #endregion

        #region DeleteRecorded
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lngID"></param>
        public void DeleteRecorded(long lngID)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            try
            {
                string strSQL = "Delete EA_ServicesTemplate WHERE TemplateID =" + lngID.ToString();
                OracleDbHelper.ExecuteNonQuery(cn, CommandType.Text, strSQL);
            }
            catch
            {
                throw;
            }
            finally
            {
                ConfigTool.CloseConnection(cn);
            }
        }
        #endregion

        #region 更新子类相关父类名称
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ServiceLevelID"></param>
        public static void UpdateChrildServiceLevel(long ServiceLevelID)
        {
            string strSQL = string.Empty;
            strSQL = "update EA_ServicesTemplate set ServiceLevel=(select TemplateName from EA_ServicesTemplate where TemplateID=" + ServiceLevelID + ") where ServiceLevelID=" + ServiceLevelID;
            CommonDP.ExcuteSql(strSQL);
        }
        #endregion

        #region GetMyTemplaties 获取模版列表
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lngUserID"></param>
        /// <param name="eShortType"></param>
        /// <param name="isAll"></param>
        /// <returns></returns>
        public DataTable GetMyTemplaties(long lngUserID, e_ITSMShortCutReqType eShortType, bool isAll)
        {
            string strSQL = string.Empty;
            OracleConnection cn = ConfigTool.GetConnection();

            try
            {
                strSQL = "SELECT a.*,to_char(a.TemplateID)||'|'||to_char(a.OFlowModelID) IDAndOFlowModelID FROM EA_ServicesTemplate a Where (a.owner=0 or a.owner= " + lngUserID.ToString() + ") AND a.TemplateType = " + ((int)eShortType).ToString();

                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
                

                if (isAll == false)
                {
                    //过滤暂时无效的流程模型
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        long lngOID = long.Parse(dt.Rows[i]["OFlowModelID"].ToString());
                        long lngFlowModelID = FlowModel.GetLastVersionFlowModelID(lngOID);
                        if (lngFlowModelID != 0)
                        {
                            if (FlowModel.CanUseFlowModel(lngUserID, lngFlowModelID) != 0)
                            {
                                dt.Rows[i].Delete();
                            }
                        }
                        else
                        {
                            dt.Rows[i].Delete();
                        }
                    }
                    dt.AcceptChanges();
                }

                return dt;
            }
            finally { ConfigTool.CloseConnection(cn); }
        }

        public DataTable GetMyTemplaties(long lngUserID, e_ITSMShortCutReqType eShortType, int pagesize, int pageindex, ref int rowcount, string SQlwhere)
        {
            string strWhere = "1=1 And  a.TemplateType = " + ((int)eShortType).ToString();

            if (SQlwhere != "")
            {
                strWhere += SQlwhere;
            }
            OracleConnection cn = ConfigTool.GetConnection();

            try
            {
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, "EA_ServicesTemplate a", "a.*,to_char(a.TemplateID)||'|'||to_char(a.OFlowModelID) IDAndOFlowModelID", "ORDER BY a.TemplateID DESC", pagesize, pageindex, strWhere, ref rowcount);
                
                return dt;
            }
            finally { ConfigTool.CloseConnection(cn); }
        }


        public DataTable getMytempLatiesXmlHttp(long lngId)
        {
            string strSQL = string.Empty;
            OracleConnection cn = ConfigTool.GetConnection();
            strSQL = "SELECT a.*,to_char(a.TemplateID)||'|'||to_char(a.OFlowModelID) IDAndOFlowModelID FROM EA_ServicesTemplate a Where  a.temPlateId = " + lngId.ToString();

            try
            {
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            
                return dt;
            }
            finally { ConfigTool.CloseConnection(cn); }

        }
        #endregion


        #region 保存附件
        /// <summary>
        /// 保存附件信息及存储附件
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="lngKBID"></param>
        /// <param name="strAttachment"></param>
        /// 
        private void SaveAttachments(OracleTransaction trans, decimal lngKBID, string strAttachment)
        {
            string strSQL = "";
            OracleDataReader dr;
            e_FileStatus lngFileStatus = 0;
            long lngFileID = 0;
            string strFileName = "";
            string strSufName = "";
            long lngupUserID = 0;

            bool blnNew = true;

            string strTmpCatalog = "";
            string strFileCatalog = "";
            string strTmpSubPath = "";
            string strTmpPath = "";

            string strOldFilePath = "";

            string strTmpFileN = "";
            string strFileN = "";
            string reqestId = "";
            int count = 0;

            strTmpCatalog = CommonDP.GetConfigValue("TempCataLog", "TempCataLog");
            strFileCatalog = CommonDP.GetConfigValue("TempCataLog", "FileCataLog");

            XmlTextReader tr = new XmlTextReader(new StringReader(strAttachment));
            while (tr.Read())
            {
                if (tr.NodeType == XmlNodeType.Element && tr.Name == "Attachments")
                {
                    //获取临时子路径
                    if (tr.GetAttribute("TempSubPath") != null)
                        strTmpSubPath = tr.GetAttribute("TempSubPath");

                }
                if (tr.NodeType == XmlNodeType.Element && tr.Name == "Attachment")
                {
                    lngFileStatus = (e_FileStatus)(int.Parse(tr.GetAttribute("Status")));
                    lngFileID = long.Parse(tr.GetAttribute("FileID"));
                    strFileName = tr.GetAttribute("FileName");
                    strSufName = tr.GetAttribute("SufName");
                    lngupUserID = long.Parse(tr.GetAttribute("upUserID"));
                    reqestId = tr.GetAttribute("replace");

                    if (strTmpCatalog.EndsWith(@"\") == false)
                    {
                        if (strTmpSubPath == "")
                        {
                            strTmpPath = strTmpCatalog;
                        }
                        else
                        {
                            strTmpPath = strTmpCatalog + @"\" + strTmpSubPath;
                        }
                    }
                    else
                    {
                        if (strTmpSubPath == "")
                        {
                            strTmpPath = strTmpCatalog.Substring(0, strTmpCatalog.Length - 1);
                        }
                        else
                        {
                            strTmpPath = strTmpCatalog + strTmpSubPath;
                        }

                    }

                    strTmpFileN = strTmpPath + @"\" + lngFileID.ToString();

                    string smonthfilepath = DateTime.Now.ToString("yyyyMM");
                    if (strFileCatalog.EndsWith(@"\") == false)
                    {
                        strFileN = strFileCatalog + @"\" + smonthfilepath;
                    }
                    else
                    {
                        strFileN = strFileCatalog + smonthfilepath;
                    }
                    MyFiles.AutoCreateDirectory(strFileN);
                    strFileN += @"\" + lngFileID.ToString();

                    blnNew = true;
                    switch (lngFileStatus)
                    {
                        case e_FileStatus.efsUpdate:
                        case e_FileStatus.efsNew:
                            //新增处理 ：1、添加记录、更新的情况判断是否存在记录（可能操作的同时别人在进行删除）  2、将临时目录中对应的文件编码并移到文件存储目录下
                            count++;
                            strSQL = "SELECT FileID,nvl(filepath,'') FROM EA_Services_Attachment WHERE FileID=" + lngFileID.ToString();
                            dr = OracleDbHelper.ExecuteReader(trans, CommandType.Text, strSQL);
                            while (dr.Read())
                            {
                                blnNew = false;
                                strOldFilePath = dr.GetString(1);
                            }
                            dr.Close();

                            if (blnNew)
                            {
                                strSQL = "INSERT INTO EA_Services_Attachment (FileID,KBID,FileName,SufName,filepath,Status,upTime,upUserID,MonthPath,deleted,deleteTime,requstFileId) " +
                                    " VALUES(" +
                                    lngFileID.ToString() + "," +
                                    lngKBID.ToString() + "," +
                                    StringTool.SqlQ(strFileName) + "," +
                                    StringTool.SqlQ(strSufName) + "," +
                                    StringTool.SqlQ(strFileCatalog) + "," +
                                    (int)e_FileStatus.efsNormal + "," +
                                    " sysdate " + "," +
                                    StringTool.SqlQ(lngupUserID.ToString()) + "," +
                                    StringTool.SqlQ(smonthfilepath) + "," +
                                     "0," +
                                     "null," + StringTool.SqlQ(reqestId.ToString()) +
                                    ")";
                            }
                            else
                            {
                                strSQL = "UPDATE EA_Services_Attachment SET upTime = sysdate,upUserID =" + lngupUserID.ToString() + "," +
                                             " filepath = " + StringTool.SqlQ(strFileCatalog) +
                                            " WHERE FileID=" + lngFileID.ToString();
                            }
                            OracleDbHelper.ExecuteNonQuery(trans, CommandType.Text, strSQL);

                            //2、将临时目录中对应的文件编码并移到文件存储目录下,并删除
                            if (PreProcessForAttachment(strTmpCatalog, strFileCatalog, strTmpFileN))
                            {
                                //由于没有比较好的办法处理，下载时的临时文件，所以，取消编码  2003-06-26 ***
                                //								MyComponent.MyTechLib.MyEnCoder.EnCodeFileToFile(strTmpFileN,strFileN);
                                if (File.Exists(strFileN))
                                    File.Delete(strFileN);
                                File.Move(strTmpFileN, strFileN);
                            }

                            if (strOldFilePath != "" && strOldFilePath.Trim().ToLower() != strFileCatalog.Trim().ToLower())
                            {
                                string strOldFileN = "";
                                if (strFileCatalog.EndsWith(@"\") == false)
                                {
                                    strOldFileN = strOldFilePath + @"\" + lngFileID.ToString();
                                }
                                else
                                {
                                    strOldFileN = strOldFilePath + lngFileID.ToString();
                                }
                                //删除附件
                                if (File.Exists(strOldFileN))
                                    File.Delete(strOldFileN);
                            }


                            break;

                        case e_FileStatus.efsDeleted:
                            strSQL = "SELECT nvl(filepath,'') FROM EA_Services_Attachment WHERE FileID=" + lngFileID.ToString();
                            dr = OracleDbHelper.ExecuteReader(trans, CommandType.Text, strSQL);
                            while (dr.Read())
                            {
                                strOldFilePath = dr.GetString(0);
                            }
                            dr.Close();
                            //删除记录
                            strSQL = "update EA_Services_Attachment set deleted=1,deletetime=sysdate,requstFileId=" + StringTool.SqlQ(reqestId.ToString()) + " WHERE FileID =" + lngFileID.ToString();
                            OracleDbHelper.ExecuteNonQuery(trans, CommandType.Text, strSQL);
                            break;
                        default:
                            count++;
                            break;
                    }

                    //无论如何删除临时文件  ***  保险语句  ****
                    if (File.Exists(strTmpFileN))
                        File.Delete(strTmpFileN);

                }
            }
            tr.Close();

            if (strTmpSubPath != "")
            {
                if (Directory.Exists(strTmpPath))
                    Directory.Delete(strTmpPath);
            }
            //更新流程附件状态
            if (count == 0)
            {
                strSQL = "UPDATE EA_ServicesTemplate SET Attachment =" + (int)e_IsTrue.fmFalse + " WHERE TemplateID=" + lngKBID.ToString();
            }
            else
            {
                strSQL = "UPDATE EA_ServicesTemplate SET Attachment =" + (int)e_IsTrue.fmTrue + " WHERE TemplateID=" + lngKBID.ToString();
            }
            OracleDbHelper.ExecuteNonQuery(trans, CommandType.Text, strSQL);

        }

        private bool PreProcessForAttachment(string strTmpCatalog, string strFileCatalog, string strTmpFileN)
        {
            MyFiles.AutoCreateDirectory(strTmpCatalog);
            MyFiles.AutoCreateDirectory(strFileCatalog);


            FileInfo fi = new FileInfo(strTmpFileN);
            return fi.Exists;

        }

        #region 取得已删除的流程附件
        /// <summary>
        /// 取得已删除的流程附件
        /// </summary>
        /// <param name="strKBID"></param>
        /// <returns></returns>
        public static DataTable getDeleteAttchmentTBL(string strKBID)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            if (cn.State == ConnectionState.Closed)
            {
                cn.Open();
            }
            try
            {
                string strSQL = @"select OA.KBID as FlowId,OA.*,U.name as UpuserName,OldOA.FileName as oldFiledName from EA_Services_Attachment OA
                left join ts_user  U on OA.upUserID=U.userid 
                left join EA_Services_Attachment OldOA  on OA.requstFileId=to_char(OldOA.FileId)
                where nvl(OA.deleted,0)=1 and (OA.requstFileid='' or OA.requstFileid is null) and OA.KBID=" + strKBID.ToString();
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
                return dt;
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                {
                    cn.Close();
                }
            }

        }
        #endregion

        #region 获取更新过的附件
        /// <summary>
        /// 获取更新过的附件
        /// </summary>
        /// <param name="lngKBID"></param>
        /// <param name="requstFileid"></param>
        /// <param name="IsDelete"></param>
        /// <returns></returns>
        public static DataTable getUpdateAttchmentTBL(string lngKBID, long requstFileid, bool IsDelete)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            if (cn.State == ConnectionState.Closed)
            {
                cn.Open();
            }
            try
            {

                string strSQL = string.Empty;
                if (IsDelete == false)
                {
                    strSQL = "SELECT a.deleteTime,a.KBID as FlowId,a.FileID,a.FileName,a.SufName,a.Status,a.upTime,a.upUserID,b.Name as upUserName ,nvl(a.requstFileId,'') requstFileId " +
                   " FROM EA_Services_Attachment a,Ts_User b " +
                   " WHERE a.upUserID = b.UserID " +
                   "		AND a.KBID =" + lngKBID.ToString() + " AND a.requstFileId =" + requstFileid.ToString() + "  ORDER BY a.FileID";
                }
                else
                {
                    strSQL = "SELECT a.deleteTime,a.KBID as FlowId,a.FileID,a.FileName,a.SufName,a.Status,a.upTime,a.upUserID,b.Name as upUserName ,nvl(a.requstFileId,'') requstFileId " +
                     " FROM EA_Services_Attachment a,Ts_User b " +
                     " WHERE a.upUserID = b.UserID " +
                     "		AND a.KBID =" + lngKBID.ToString() + " AND a.FileID =" + requstFileid.ToString() + "  ORDER BY a.FileID";
                }
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
                ConfigTool.CloseConnection(cn);
                return dt;
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                {
                    cn.Close();
                }
            }
        }
        #endregion

        #endregion

    }
}

