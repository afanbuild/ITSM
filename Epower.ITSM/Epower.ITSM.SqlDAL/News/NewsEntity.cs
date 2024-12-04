using System;
using System.Data;
using System.Data.OracleClient;
using Epower.DevBase.BaseTools;
using Epower.ITSM.Base;
using System.Xml;
using EpowerGlobal;
using System.Text;
using System.IO;
using EpowerCom;
using System.Web;

namespace Epower.ITSM.SqlDAL
{
    /// <summary>
    /// NewsEntity 的摘要说明。
    /// </summary>
    public class NewsEntity
    {
        /// <summary>
        ///信息实体
        /// </summary>
        public NewsEntity()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }
        #region 属性
        private long _NewsId;
        private string _Title;
        private int _TypeId;
        private string _Writer;
        private string _Content;
        private string _InputDate;
        private string _PubDate;
        private string _OutDate;
        private long _InputUser;
        private long _CheckUser;
        eOA_DispFlag _DispFlag = eOA_DispFlag.eTrue;
        private string _Photo;
        private int _FocusNews;
        private int _Bulletin;
        private string _FileName;
        private string _SoftName;
        private long _InOrgID;
        private long _InDeptID;
        private int _IsInner;
        private decimal _IsAlert;
        #endregion

        #region 属性定义
        /// <summary>
        ///信息ID
        /// </summary>
        public long NewsId
        {
            get { return _NewsId; }
            set { _NewsId = value; }
        }
        /// <summary>
        ///信息标题
        /// </summary>
        public string Title
        {
            get { return _Title; }
            set { _Title = value; }
        }
        /// <summary>
        ///信息类型
        /// </summary>
        public int TypeId
        {
            get { return _TypeId; }
            set { _TypeId = value; }
        }
        /// <summary>
        ///作者
        /// </summary>
        public string Writer
        {
            get { return _Writer; }
            set { _Writer = value; }
        }
        /// <summary>
        ///录入时间
        /// </summary>
        public string InputDate
        {
            get { return _InputDate; }
            set { _InputDate = value; }
        }
        /// <summary>
        ///发布时间
        /// </summary>
        public string PubDate
        {
            get { return _PubDate; }
            set { _PubDate = value; }
        }
        /// <summary>
        ///信息内容
        /// </summary>
        public string Content
        {
            get { return _Content; }
            set { _Content = value; }
        }
        /// <summary>
        ///
        /// </summary>
        public string OutDate
        {
            get { return _OutDate; }
            set { _OutDate = value; }
        }
        /// <summary>
        ///录入人
        /// </summary>
        public long InputUser
        {
            get { return _InputUser; }
            set { _InputUser = value; }
        }
        /// <summary>
        ///未用属性
        /// </summary>
        public long CheckUser
        {
            get { return _CheckUser; }
            set { _CheckUser = value; }
        }
        /// <summary>
        ///是否显示
        /// </summary>
        public eOA_DispFlag DispFlag
        {
            get { return _DispFlag; }
            set { _DispFlag = value; }
        }
        /// <summary>
        ///是否弹出
        /// </summary>
        public decimal IsAlert
        {
            get { return _IsAlert; }
            set { _IsAlert = value; }
        }
        /// <summary>
        ///图片地址
        /// </summary>
        public string Photo
        {
            get { return _Photo; }
            set { _Photo = value; }
        }
        /// <summary>
        ///是否焦点新闻
        /// </summary>
        public int FocusNews
        {
            get { return _FocusNews; }
            set { _FocusNews = value; }
        }

        /// <summary>
        /// 是否最新公告
        /// </summary>
        public int Bulletin
        {
            get { return _Bulletin; }
            set { _Bulletin = value; }
        }

        /// <summary>
        ///附件随机名
        /// </summary>
        public string FileName
        {
            get { return _FileName; }
            set { _FileName = value; }
        }
        /// <summary>
        ///附件原名
        /// </summary>
        public string SoftName
        {
            get { return _SoftName; }
            set { _SoftName = value; }
        }
        /// <summary>
        ///录入人单位ID
        /// </summary>
        public long InOrgID
        {
            get { return _InOrgID; }
            set { _InOrgID = value; }
        }
        /// <summary>
        ///录入人部门ID
        /// </summary>
        public long InDeptID
        {
            get { return _InDeptID; }
            set { _InDeptID = value; }
        }

        /// <summary>
        ///显示范围
        /// </summary>
        public int IsInner
        {
            get { return _IsInner; }
            set { _IsInner = value; }
        }
        private long _TNewsID = 0;
        /// <summary>
        /// 公告ID
        /// </summary>
        public long TNewsID
        {
            get { return _TNewsID; }
            set { _TNewsID = value; }
        }
        private string _AttachXml;
        /// <summary>
        /// 附件XML串
        /// </summary>
        public String AttachXml
        {
            get { return _AttachXml; }
            set { _AttachXml = value; }
        }
        #endregion

        #region 方法
        public NewsEntity(long NewsId)
        {
            string strSQL = "SELECT * FROM OA_NEWS WHERE NewsId =" + NewsId.ToString();
            OracleConnection cn = ConfigTool.GetConnection();
            try
            {
                OracleDataReader dr = OracleDbHelper.ExecuteReader(cn, CommandType.Text, strSQL);

                if (dr.Read())
                {
                    this.NewsId = (long)dr.GetDecimal(dr.GetOrdinal("NewsId"));
                    this.Title = dr.IsDBNull(dr.GetOrdinal("Title")) == true ? "" : dr.GetString(dr.GetOrdinal("Title"));
                    this.TypeId = dr.IsDBNull(dr.GetOrdinal("TypeId")) == true ? 1 : dr.GetInt32(dr.GetOrdinal("TypeId"));
                    this.Writer = dr.IsDBNull(dr.GetOrdinal("Writer")) == true ? "" : dr.GetString(dr.GetOrdinal("Writer"));
                    this.Content = dr.IsDBNull(dr.GetOrdinal("Content")) == true ? "" : dr.GetString(dr.GetOrdinal("Content"));
                    this.InputDate = dr.IsDBNull(dr.GetOrdinal("InputDate")) == true ? "" : StringTool.ToMyDateFormat(dr.GetDateTime(dr.GetOrdinal("InputDate")));
                    this.PubDate = dr.IsDBNull(dr.GetOrdinal("PubDate")) == true ? "" : StringTool.ToMyDateFormat(dr.GetDateTime(dr.GetOrdinal("PubDate")));
                    this.OutDate = dr.IsDBNull(dr.GetOrdinal("OutDate")) == true ? "" : StringTool.ToMyDateFormat(dr.GetDateTime(dr.GetOrdinal("OutDate")));
                    this.InputUser = dr.IsDBNull(dr.GetOrdinal("InputUser")) == true ? 0 : dr.GetInt32(dr.GetOrdinal("InputUser"));
                    this.CheckUser = dr.IsDBNull(dr.GetOrdinal("CheckUser")) == true ? 0 : dr.GetInt32(dr.GetOrdinal("CheckUser")); ;
                    this.DispFlag = dr.IsDBNull(dr.GetOrdinal("DispFlag")) == true ? eOA_DispFlag.eFalse : (eOA_DispFlag)dr.GetInt32(dr.GetOrdinal("DispFlag"));
                    this.Photo = dr.IsDBNull(dr.GetOrdinal("Photo")) == true ? "" : dr.GetString(dr.GetOrdinal("Photo"));
                    this.FocusNews = dr.IsDBNull(dr.GetOrdinal("FocusNews")) == true ? 0 : (int)dr.GetInt32(dr.GetOrdinal("FocusNews"));
                    this.Bulletin = dr.IsDBNull(dr.GetOrdinal("IsBulletin")) == true ? 0 : (int)dr.GetInt32(dr.GetOrdinal("IsBulletin"));
                    this.FileName = dr.IsDBNull(dr.GetOrdinal("FileName")) == true ? "" : dr.GetString(dr.GetOrdinal("FileName"));
                    this.SoftName = dr.IsDBNull(dr.GetOrdinal("SoftName")) == true ? "" : dr.GetString(dr.GetOrdinal("SoftName"));
                    this.InOrgID = dr.IsDBNull(dr.GetOrdinal("InOrgID")) == true ? 0 : dr.GetInt32(dr.GetOrdinal("InOrgID"));
                    this.InDeptID = dr.IsDBNull(dr.GetOrdinal("InDeptID")) == true ? 0 : dr.GetInt32(dr.GetOrdinal("InDeptID"));
                    this.IsInner = dr.IsDBNull(dr.GetOrdinal("InIsInner")) == true ? 1 : dr.GetInt32(dr.GetOrdinal("InIsInner"));
                }
                dr.Close();
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
            finally
            {
                ConfigTool.CloseConnection(cn);
            }
        }

        public void Save()
        {
            //更新或新增（ＩＤ　＝　０，文章标题、类别、作者不为空的时候新增）
            if (this.NewsId != 0)
            {
                UpdateNews();
            }
            else
            {
                if (this.Title != "" && this.TypeId != 0 && this.Writer != "")
                {
                    AddNews();
                }
            }
        }

        private void AddNews()
        {
            string strSQL = "";
            string strPubDate = "";

            OracleConnection cn = ConfigTool.GetConnection();

            if (cn.State == ConnectionState.Closed)
            {
                cn.Open();
            }

            OracleTransaction trans = cn.BeginTransaction();

            try
            {
                strPubDate = this.PubDate;
                this.InputDate = DateTime.Now.ToString();

                strSQL = "INSERT INTO OA_News (NewsId,Title,TypeId,Writer,InputDate,PubDate,OutDate,Content,InputUser,DispFlag,Photo,FocusNews,IsBulletin,FileName,SoftName,InOrgID,InDeptID,IsInner,IsAlert)" +
                    " Values(" +
                    this.TNewsID.ToString() + "," +
                    StringTool.SqlQ(this.Title) + "," +
                    this.TypeId + "," +
                    StringTool.SqlQ(this.Writer) + "," +
                    "to_date(" + StringTool.EmptyToNullDate(this.InputDate) + ",'yyyy-MM-dd HH24:mi:ss')," +
                    "to_date(" + StringTool.EmptyToNullDate(strPubDate) + ",'yyyy-MM-dd HH24:mi:ss')," +
                    "to_date(" + StringTool.EmptyToNullDate(this.OutDate) + ",'yyyy-MM-dd HH24:mi:ss')," +
                    StringTool.SqlQ(this.Content) + "," +
                    this.InputUser + "," +
                    (int)this.DispFlag + "," +
                    StringTool.SqlQ(this.Photo) + "," +
                    this.FocusNews + "," +
                    this.Bulletin + "," +
                    StringTool.SqlQ(this.FileName) + "," +
                    StringTool.SqlQ(this.SoftName) + "," +
                    this.InOrgID + "," +
                    this.InDeptID + "," +
                    this.IsInner + "," +
                    this.IsAlert +
                    ")";

                OracleDbHelper.ExecuteNonQuery(trans, CommandType.Text, strSQL);

                SaveAttachments(trans, this.TNewsID, this.AttachXml);  //保存附件

                trans.Commit();
            }
            catch (Exception err)
            {
                trans.Rollback();
                throw new Exception(err.Message);
            }
            finally
            {
                ConfigTool.CloseConnection(cn);
            }

        }

        private void UpdateNews()
        {
            string strSQL = "";

            OracleConnection cn = ConfigTool.GetConnection();

            if (cn.State == ConnectionState.Closed)
            {
                cn.Open();
            }

            OracleTransaction trans = cn.BeginTransaction();

            try
            {
                this.InputDate = StringTool.ToMyDateFormat(DateTime.Now);

                strSQL = "UPDATE OA_News SET " +
                    " Title =" + StringTool.SqlQ(this.Title) + "," +
                    " TypeId = " + this.TypeId + ",";

                if (this.FileName != "" && this.SoftName != "")
                {
                    strSQL = strSQL + " FileName = " + StringTool.SqlQ(this.FileName) + "," +
                        " SoftName = " + StringTool.SqlQ(this.SoftName) + ",";
                }
                if (this.Photo != "")
                {
                    strSQL = strSQL + " Photo = " + StringTool.SqlQ(this.Photo) + ",";
                }
                strSQL = strSQL + " Writer = " + StringTool.SqlQ(this.Writer) + "," +
                    //" InputDate = " + StringTool.EmptyToNullDate(this.InputDate) + "," +
                    " PubDate = to_date(" + StringTool.EmptyToNullDate(this.PubDate) + ",'yyyy-MM-dd HH24:mi:ss')," +
                    " OutDate = to_date(" + StringTool.EmptyToNullDate(this.OutDate) + ",'yyyy-MM-dd HH24:mi:ss')," +
                    //" InputUser = " + this.InputUser + "," +
                    " DispFlag = " + (int)this.DispFlag + "," +
                    " Content= " + StringTool.SqlQ(this.Content) + "," +
                    " FocusNews = " + this.FocusNews + "," +
                    " IsBulletin = " + this.Bulletin + "," +
                    " InOrgID=" + this.InOrgID + "," +
                    " InDeptID=" + this.InDeptID + "," +
                    " IsInner=" + this.IsInner + "," +
                    " IsAlert=" + this.IsAlert +
                    " WHERE NewsId = " + this.NewsId.ToString();

                OracleDbHelper.ExecuteNonQuery(trans, CommandType.Text, strSQL);

                SaveAttachments(trans, this.NewsId, this.AttachXml);     //保存附件

                trans.Commit();

            }
            catch (Exception err)
            {
                trans.Rollback();
                throw new Exception(err.Message);
            }
            finally
            {
                ConfigTool.CloseConnection(cn);
            }
        }

        public void Delete()
        {
            OracleConnection cn = ConfigTool.GetConnection();
            if (cn.State == ConnectionState.Closed)
            {
                cn.Open();
            }

            OracleTransaction trans = cn.BeginTransaction();
            try
            {
                string strSQL = "DELETE  FROM OA_News WHERE NewsID = " + this.NewsId;
                OracleDbHelper.ExecuteNonQuery(trans, CommandType.Text, strSQL);
                trans.Commit();
            }
            catch
            {
                trans.Rollback();
                throw new Exception();
            }
            finally
            {
                ConfigTool.CloseConnection(cn);
            }
        }

        #region 静态方法区

        /// <summary>
        /// 获得公告附件的信息，以XML串表示
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
            //添加附件信息  
            strSQL = "SELECT a.FileID,a.FileName,a.SufName,a.Status,a.upTime,a.upUserID,b.Name as upUserName ,nvl(a.requstFileId,'') requstFileId " +
                " FROM OA_Attachment a,Ts_User b " +
                " WHERE a.upUserID = b.UserID AND a.Status <>" + (int)e_FileStatus.efsDeleted +
                "		AND a.KBID =" + lngKBID.ToString() + " AND nvl(a.deleted,0)=" + (int)e_Deleted.eNormal + " ORDER BY a.FileID";


            OracleConnection cn = ConfigTool.GetConnection();

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

            ConfigTool.CloseConnection(cn);


            xmlDoc.AppendChild(xmlElTmp);
            return xmlDoc.InnerXml;
        }

        /// <summary>
        /// 获得公告附件的信息，以XML串表示
        /// </summary>
        /// <param name="lngKBID"></param>
        /// <returns></returns>
        public static string GetAttachmentXml(decimal lngKBID, ref string strMonthPath)
        {
            string strSQL = "";
            OracleDataReader dr;

            XmlDocument xmlDoc = new XmlDocument();
            XmlElement xmlElTmp;
            XmlElement xmlElSub;

            xmlElTmp = xmlDoc.CreateElement("Attachments");


            //添加附件信息  
            //添加附件信息  
            strSQL = "SELECT a.FileID,a.FileName,a.SufName,a.Status,a.upTime,a.upUserID,b.Name as upUserName ,nvl(a.requstFileId,'') requstFileId ,nvl(a.MonthPath,'')  MonthPath" +
                " FROM OA_Attachment a,Ts_User b " +
                " WHERE a.upUserID = b.UserID AND a.Status <>" + (int)e_FileStatus.efsDeleted +
                "		AND a.KBID =" + lngKBID.ToString() + " AND nvl(a.deleted,0)=" + (int)e_Deleted.eNormal + " ORDER BY a.FileID";


            OracleConnection cn = ConfigTool.GetConnection();

            dr = OracleDbHelper.ExecuteReader(cn, CommandType.Text, strSQL);
            while (dr.Read())
            {
                strMonthPath = dr["MonthPath"].ToString();

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

            ConfigTool.CloseConnection(cn);


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
                strSQL = "SELECT FileName,nvl(MonthPath,'')  MonthPath  FROM OA_Attachment WHERE FileID=" + lngFileID.ToString();
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

        #region 保存附件
        /// <summary>
        /// 保存附件信息及存储附件
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="lngKBID"></param>
        /// <param name="strAttachment"></param>
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
                            strSQL = "SELECT FileID,nvl(filepath,'') FROM OA_Attachment WHERE FileID=" + lngFileID.ToString();
                            dr = OracleDbHelper.ExecuteReader(trans, CommandType.Text, strSQL);
                            while (dr.Read())
                            {
                                blnNew = false;
                                strOldFilePath = dr.GetString(1);
                            }
                            dr.Close();

                            if (blnNew)
                            {
                                strSQL = "INSERT INTO OA_Attachment (FileID,KBID,FileName,SufName,filepath,Status,upTime,upUserID,MonthPath,deleted,deleteTime,requstFileId) " +
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
                                strSQL = "UPDATE OA_Attachment SET upTime = sysdate,upUserID =" + lngupUserID.ToString() + "," +
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
                            strSQL = "SELECT nvl(filepath,'') FROM OA_Attachment WHERE FileID=" + lngFileID.ToString();
                            dr = OracleDbHelper.ExecuteReader(trans, CommandType.Text, strSQL);
                            while (dr.Read())
                            {
                                strOldFilePath = dr.GetString(0);
                            }
                            dr.Close();
                            //删除记录
                            strSQL = "update OA_Attachment set deleted=1,deletetime=sysdate,requstFileId=" + StringTool.SqlQ(reqestId.ToString()) + " WHERE FileID =" + lngFileID.ToString();
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

            //上传附件完之后 清除添加附件时增加色session 只用一次
            if (HttpContext.Current.Session["tmpsubpath"] != null)
            {
                HttpContext.Current.Session.Remove("tmpsubpath");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strTmpCatalog"></param>
        /// <param name="strFileCatalog"></param>
        /// <param name="strTmpFileN"></param>
        /// <returns></returns>
        private bool PreProcessForAttachment(string strTmpCatalog, string strFileCatalog, string strTmpFileN)
        {
            MyFiles.AutoCreateDirectory(strTmpCatalog);
            MyFiles.AutoCreateDirectory(strFileCatalog);
            FileInfo fi = new FileInfo(strTmpFileN);
            return fi.Exists;

        }


        #endregion

        #region 取得已删除的流程附件
        /// <summary>
        /// 取得已删除的流程附件
        /// </summary>
        /// <param name="strKBID"></param>
        /// <returns></returns>
        public static DataTable getDeleteAttchmentTBL(string strKBID)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            string strSQL = @"select OA.KBID as FlowId,OA.*,U.name as UpuserName,OldOA.FileName as oldFiledName from OA_Attachment OA
                left join ts_user  U on OA.upUserID=U.userid 
                left join OA_Attachment OldOA  on OA.requstFileId=TO_CHAR(OldOA.FileId)
                where nvl(OA.deleted,0)=1 and (OA.requstFileid='' or OA.requstFileid is null)  and OA.KBID=" + strKBID.ToString();
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigTool.CloseConnection(cn);
            return dt;
        }
        #endregion

        #region 查看是否在数据库中已经存在
        /// <summary>
        /// 查看是否在数据库中存在
        /// </summary>
        /// <param name="FileId"></param>
        /// <returns></returns>
        public static DataTable getFileIsTrue(long FileId)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            if (cn.State == ConnectionState.Closed)
            {
                cn.Open();
            }
            try
            {
                string strSQL = "SELECT * FROM OA_Attachment where FileID=" + FileId.ToString();
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

        #region 取得更新过的附件
        /// <summary>
        /// 取得更新过的附件
        /// </summary>
        /// <param name="lngKBID"></param>
        /// <param name="requstFileid"></param>
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
                " FROM OA_Attachment a,Ts_User b " +
                " WHERE a.upUserID = b.UserID " +
                "		AND a.KBID =" + lngKBID.ToString() + " AND a.requstFileId =" + requstFileid.ToString() + " ORDER BY a.FileID";
                }
                else
                {
                    strSQL = "SELECT a.deleteTime,a.KBID as FlowId,a.FileID,a.FileName,a.SufName,a.Status,a.upTime,a.upUserID,b.Name as upUserName ,nvl(a.requstFileId,'') requstFileId " +
                " FROM OA_Attachment a,Ts_User b " +
                " WHERE a.upUserID = b.UserID " +
                "		AND a.KBID =" + lngKBID.ToString() + " AND a.FileID =" + requstFileid.ToString() + " ORDER BY a.FileID";
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
