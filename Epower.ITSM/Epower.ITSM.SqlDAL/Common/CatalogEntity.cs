/*******************************************************************
 *
 * Description:常用分类实体
 * 
 * 
 * Create By  :
 * Create Date:2008年7月30日
 * *****************************************************************/
using System;
using System.Data;
using System.Data.OracleClient;
using Epower.DevBase.BaseTools;
using Epower.DevBase.Organization.SqlDAL;
using Epower.DevBase.Organization.Base;

namespace Epower.ITSM.SqlDAL
{
    /// <summary>
    /// CatalogEntity 的摘要说明。
    /// </summary>
    public class CatalogEntity
    {

        long _CatalogID = 0;
        string _FullID = "";
        long _OrgID = 0;
        long _ParentID = 0;
        long _OldParentID = 0;
        string _CatalogName = "";
        long _IconID = 0;
        int _SortID = 0;

        string _Remark = "";
        eO_Deleted _Deleted = eO_Deleted.eNormal;
        string _configureSchema = "";

        //long _CreatorID = 0;
        //long _UpdateID = 0;
        //long _PID = 0;


        //string _StartDate;
        //string _EndDate;
        //string _CreateDate;
        //string _UpdateDate;
        //string _CatalogCode;
        //string _DOC_Center;

        #region 属性定义
        public long CatalogID
        {
            get { return _CatalogID; }
            set { _CatalogID = value; }
        }

        public string FullID
        {
            get { return _FullID; }
            set { _FullID = value; }
        }

        public long OrgID
        {
            get { return _OrgID; }
            set { _OrgID = value; }
        }


        public long ParentID
        {
            get { return _ParentID; }
            set { _ParentID = value; }
        }

        public long OldParentID
        {
            get { return _OldParentID; }
            set { _OldParentID = value; }
        }

        public string CatalogName
        {
            get { return _CatalogName; }
            set { _CatalogName = value; }
        }

        public long IconID
        {
            get { return _IconID; }
            set { _IconID = value; }
        }


        public int SortID
        {
            get { return _SortID; }
            set { _SortID = value; }
        }

        public string Remark
        {
            get { return _Remark; }
            set { _Remark = value; }
        }

        private string mimglogo = "";
        public string imglog
        {
            get { return mimglogo; }
            set { mimglogo = value; }
        }

        public eO_Deleted Deleted
        {
            get { return _Deleted; }
            set { _Deleted = value; }
        }

        /// <summary>
        /// 配置项XML串
        /// </summary>
        public string ConfigureSchema
        {
            get { return _configureSchema; }
            set { _configureSchema = value; }
        }

        private int _IsShowSchema = 0;
        /// <summary>
        /// 是否展示扩展功能
        /// </summary>
        public int IsShowSchema
        {
            get { return _IsShowSchema; }
            set { _IsShowSchema = value; }
        }

        #endregion

        #region 构造函数
        public CatalogEntity()
        {

        }

        public CatalogEntity(long lngID)
        {
            string strSQL = "SELECT * FROM Es_Catalog WHERE Catalogid =" + lngID.ToString() + " AND deleted = " + (int)eO_Deleted.eNormal; ;
            string strConn = ConfigTool.GetConnectString();

            OracleDataReader dr = null;

            try
            {
                dr = OracleDbHelper.ExecuteReader(strConn, CommandType.Text, strSQL);

                if (dr.Read())
                {
                    this.CatalogID = (long)dr.GetDecimal(dr.GetOrdinal("CatalogID"));

                    //this.CreatorID = dr.IsDBNull(dr.GetOrdinal("CreateID")) == true? 0 : (long)dr.GetDecimal(dr.GetOrdinal("CreateID"));
                    //this.IconID = dr.IsDBNull(dr.GetOrdinal("IconID")) == true? 0 :(long)dr.GetInt32(dr.GetOrdinal("IconID"));
                    //this.IsTemp = dr.IsDBNull(dr.GetOrdinal("IsTemp")) == true? 0 :(eO_IsTrue)dr.GetInt32(dr.GetOrdinal("IsTemp"));
                    this.Deleted = dr.IsDBNull(dr.GetOrdinal("Deleted")) == true ? 0 : (eO_Deleted)dr.GetInt32(dr.GetOrdinal("Deleted"));
                    //this.Layer =  dr.IsDBNull(dr.GetOrdinal("Layer")) == true? 0 :(long)dr.GetInt32(dr.GetOrdinal("Layer"));
                    //this.LeaderID = dr.IsDBNull(dr.GetOrdinal("LeaderID")) == true? 0 :(long)dr.GetDecimal(dr.GetOrdinal("LeaderID"));
                    //this.ManagerID = dr.IsDBNull(dr.GetOrdinal("ManagerID")) == true? 0 :(long)dr.GetDecimal(dr.GetOrdinal("ManagerID"));
                    //this.OldManagerID = this.ManagerID;
                    this.OrgID = dr.IsDBNull(dr.GetOrdinal("OrgID")) == true ? 0 : (long)dr.GetDecimal(dr.GetOrdinal("OrgID"));
                    this.SortID = dr.IsDBNull(dr.GetOrdinal("SortID")) == true ? 0 : (int)dr.GetInt32(dr.GetOrdinal("SortID"));
                    //this.UpdateID = dr.IsDBNull(dr.GetOrdinal("UpdateID")) == true? 0 :(long)dr.GetDecimal(dr.GetOrdinal("UpdateID"));
                    this.ParentID = dr.IsDBNull(dr.GetOrdinal("ParentID")) == true ? 0 : (long)dr.GetDecimal(dr.GetOrdinal("ParentID"));
                    this.OldParentID = this.ParentID;
                    //this.CatalogKind =  dr.IsDBNull(dr.GetOrdinal("CatalogKind")) == true? 0 :(int)dr.GetInt32(dr.GetOrdinal("CatalogKind"));
                    //this.CatalogType =  dr.IsDBNull(dr.GetOrdinal("CatalogType")) == true? 0 :(int)dr.GetInt32(dr.GetOrdinal("CatalogType"));

                    this.CatalogName = dr.IsDBNull(dr.GetOrdinal("CatalogName")) == true ? "" : dr.GetString(dr.GetOrdinal("CatalogName"));
                    this.Remark = dr.IsDBNull(dr.GetOrdinal("Remark")) == true ? "" : dr.GetString(dr.GetOrdinal("Remark"));
                    this.FullID = dr.IsDBNull(dr.GetOrdinal("FullID")) == true ? "" : dr.GetString(dr.GetOrdinal("FullID"));
                    //this.imglog = dr.IsDBNull(dr.GetOrdinal("imglogo")) == true ? "" : dr.GetString(dr.GetOrdinal("imglogo"));
                    //this.CreateDate = dr.IsDBNull(dr.GetOrdinal("CreateDate")) == true?"":StringTool.ToMyDateFormat(dr.GetDateTime(dr.GetOrdinal("CreateDate")));
                    //this.EndDate = dr.IsDBNull(dr.GetOrdinal("EndDate")) == true?"":StringTool.ToMyDateFormat(dr.GetDateTime(dr.GetOrdinal("EndDate")));
                    //this.StartDate = dr.IsDBNull(dr.GetOrdinal("StartDate")) == true?"":StringTool.ToMyDateFormat(dr.GetDateTime(dr.GetOrdinal("StartDate")));
                    //this.UpdateDate = dr.IsDBNull(dr.GetOrdinal("UpdateDate")) == true?"":StringTool.ToMyDateFormat(dr.GetDateTime(dr.GetOrdinal("UpdateDate")));
                    //this.PID =dr.IsDBNull(dr.GetOrdinal("PID"))? 0 :(long)dr.GetDecimal(dr.GetOrdinal("PID"));
                    //this.CatalogCode = dr.IsDBNull(dr.GetOrdinal("CatalogCode")) == true ? "" :dr.GetString(dr.GetOrdinal("CatalogCode"));
                    //this.DOC_Center=dr.IsDBNull(dr.GetOrdinal("DOC_Center")) == true ? "" :dr.GetString(dr.GetOrdinal("DOC_Center"));
                    this.ConfigureSchema = dr.IsDBNull(dr.GetOrdinal("configureSchema")) == true ? "" : dr.GetString(dr.GetOrdinal("configureSchema"));
                    this.IsShowSchema = dr.IsDBNull(dr.GetOrdinal("IsShowSchema")) == true ? 0 : (int)dr.GetInt32(dr.GetOrdinal("IsShowSchema"));
                }
            }
            finally { if (dr != null) dr.Close(); }
        }


        #endregion

        #region 获取常用类别Json对象
        /// <summary>
        /// 获取常用类别Json对象
        /// </summary>
        /// <param name="lngID">标示ID</param>
        /// <returns></returns>
        public static string GetCatalogJson(long lngID)
        {
            string strSQL = string.Empty;
            OracleConnection cn = ConfigTool.GetConnection();

            try
            {
                strSQL = "SELECT * FROM Es_Catalog WHERE Catalogid =" + lngID.ToString() + " AND deleted = " + (int)eO_Deleted.eNormal;
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
                

                Json json = new Json(dt);
                return "{record:" + json.ToJson() + "}";
            }
            finally { ConfigTool.CloseConnection(cn); }
        }
        #endregion

        public CatalogEntity GetParent()
        {
            if (this.ParentID == 0)
            {
                return null;
            }
            else
            {
                return new CatalogEntity(this.ParentID);
            }
        }


        public DeptEntity GetOldParent()
        {
            if (this.OldParentID == 0)
            {
                return null;
            }
            else
            {
                return new DeptEntity(this.OldParentID);
            }
        }


        #region Public[Save,Delete]
        /// <summary>
        /// 删除
        /// </summary>
        public void Delete()
        {
            if (this.CatalogID == 0 || this.CatalogID == -1)
                return;

            string strSQL;

            //1、判断是否分类内有人员的资料,如果有则不能删除
            //２、如果可以删除，则同时删除用户组组成表中与此分类相关的信息和权限表中与此分类相关的信息。



            OracleConnection cn = ConfigTool.GetConnection();
            //
            //			strSQL = "SELECT COUNT(userid) FROM ts_userCatalog WHERE Catalogid =" + this.CatalogID.ToString() + " AND deleted = " + (int)eO_Deleted.eNormal;
            //				
            //			int count = (int)OracleDbHelper.ExecuteScalar(cn,CommandType.Text,strSQL);
            //
            //			if(count > 0)
            //			{
            //				throw new Exception("分类内存在人员信息，暂时不能删除，请先安置分类人员！");
            //			}


            //如果分类有下属分类，则不允许删除
            try
            {
                strSQL = "SELECT COUNT(CatalogID) FROM Es_Catalog WHERE ParentID =" + this.CatalogID.ToString() + " AND deleted = " + (int)eO_Deleted.eNormal;

                int count = int.Parse(OracleDbHelper.ExecuteScalar(cn, CommandType.Text, strSQL).ToString());

                if (count > 0)
                {
                    throw new Exception("分类内包含下属分类，暂时不能删除，请先删除下属分类！");
                }
            }
            catch (Exception ex) { ConfigTool.CloseConnection(cn); throw ex; }

            if (cn.State != ConnectionState.Open)
            {
                cn.Open();
            }

            OracleTransaction trans = cn.BeginTransaction();

            try
            {
                //				strSQL = "DELETE Es_Catalog " +
                //					" WHERE Catalogid = " + this.CatalogID.ToString();
                //				OracleDbHelper.ExecuteNonQuery(trans,CommandType.Text,strSQL);

                strSQL = "UPDATE Es_Catalog SET deleted = " + (int)eO_Deleted.eDeleted + ",UpdateTime = sysdate" +
                    " WHERE Catalogid = " + this.CatalogID.ToString();
                OracleDbHelper.ExecuteNonQuery(trans, CommandType.Text, strSQL);

                //				strSQL = "DELETE ts_actormembers WHERE objectid = " + this.CatalogID.ToString() + " AND actortype =" + (int)eO_ActorObject.eCatalogActor;
                //				OracleDbHelper.ExecuteNonQuery(trans,CommandType.Text,strSQL);
                //
                //				strSQL = "DELETE ts_rights WHERE objectid = " + this.CatalogID.ToString() + " AND objecttype =" + (int)eO_RightObject.eCatalogRight;
                //				OracleDbHelper.ExecuteNonQuery(trans,CommandType.Text,strSQL);

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

        public void Save()
        {
            //更新或新增（ＩＤ　＝　０，ＮＡＭＥ　不等于　空的时候新增）
            if (this.CatalogID != 0 && this.OldParentID > 0)
            {
                UpdateCatalog();
            }
            else
            {
                if (this.CatalogName.Trim() != "" && this.ParentID != 0)
                {
                    AddCatalog();
                }
                else
                {
                    throw new Exception("分类名称或上级分类不能为空");
                }
            }
        }

        #endregion

        #region Private[UpdateCatalog,AddCatalog]
        private void UpdateCatalog()
        {
            string strSQL = "";
            string strTmpFull = "";
            string strTmpOldFull = "";

            OracleConnection cn = ConfigTool.GetConnection();
            if (cn.State == ConnectionState.Closed)
            {
                cn.Open();
            }
            OracleTransaction trans = cn.BeginTransaction();
            try
            {
                strTmpFull = GetParent().FullID + (this.CatalogID == 1 ? "" : this.CatalogID.ToString().PadLeft(6, char.Parse("0")));

                if (this.OldParentID != this.ParentID)
                {
                    //更新所有下级部门的FULLID

                    strTmpOldFull = GetOldParent().FullID + (this.CatalogID == 1 ? "" : this.CatalogID.ToString().PadLeft(6, char.Parse("0")));

                    if (strTmpOldFull.Length != 0)
                    {
                        strSQL = "UPDATE es_catalog SET UpdateTime = sysdate" + "," + "fullid = replace(fullid," + StringTool.SqlQ(strTmpOldFull) + "," + StringTool.SqlQ(strTmpFull) + ")" +
                            " WHERE fullid like " + StringTool.SqlQ(strTmpOldFull + " % ") + " AND catalogid <>" + this.CatalogID.ToString();
                        OracleDbHelper.ExecuteNonQuery(trans, CommandType.Text, strSQL);
                    }
                    else
                    {
                        //不能调整根分类
                    }

                }


                this.SortID = CheckSortID(this.SortID);

                strSQL = "UPDATE Es_Catalog SET " +
                    " Catalogname =" + StringTool.SqlQ(this.CatalogName) + "," +
                    " orgid = " + this.OrgID.ToString() + "," +
                    " FullID = " + StringTool.SqlQ(strTmpFull) + "," +
                    " parentid = " + this.ParentID.ToString() + "," +
                    " sortid = " + this.SortID.ToString() + "," +
                    " deleted = " + ((int)this.Deleted).ToString() + "," +
                    " UpdateTime = sysdate" + "," +
                    " Remark =" + StringTool.SqlQ(this.Remark) + "," +
                    " configureSchema =" + StringTool.SqlQ(this.ConfigureSchema) + "," +
                    " IsShowSchema = " + this.IsShowSchema.ToString() +
                    " WHERE Catalogid = " + this.CatalogID.ToString();


                //this.UpdateDate = StringTool.ToMyDateFormat(DateTime.Now);

                OracleDbHelper.ExecuteNonQuery(trans, CommandType.Text, strSQL);

                trans.Commit();

                this.OldParentID = this.ParentID;

                //this.OldManagerID = this.ManagerID;
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

        private void AddCatalog()
        {
            string strSQL = "";
            OracleConnection cn = ConfigTool.GetConnection();
            long lngNextID = 0;
            string strParentFullID = GetParent().FullID;
            string strFullID = "";
            int iMaxSortID = 0;

            //bool blnCanManage = true;

            cn.Open();
            OracleTransaction trans = cn.BeginTransaction();

            try
            {
                //lngNextID = EpowerGlobal.EPGlobal.GetNextID("Catalog_ID");

                if (this.CatalogID != 0)
                {
                    strFullID = strParentFullID + this.CatalogID.ToString().PadLeft(6, char.Parse("0"));


                    this.OldParentID = 0;
                    //this.OldManagerID = 0;
                    this.FullID = strFullID;
                    //blnCanManage = CanManageCatalog();
                    //					if(blnCanManage == false)
                    //					{
                    //						throw new Exception("不能再担任");
                    //					}

                    //					//获取最大的ＳＯＲＴＩＤ
                    //
                    //					strSQL = "SELECT max(nvl(sortid,0)) FROM Es_Catalog WHERE parentid = " + this.ParentID.ToString() + 
                    //									" AND deleted = " + (int)eO_Deleted.eNormal;
                    //
                    //					dr = OracleDbHelper.ExecuteReader(trans,CommandType.Text,strSQL);
                    //					if(dr.Read())
                    //					{
                    //						//当前最大值加１
                    //						iMaxSortID = (dr.IsDBNull(0))?0:dr.GetInt32(0) + 1;
                    //					}
                    //					dr.Close();


                    //如果SortID为-1,则系统自动为其指定SortID
                    //					if(this.SortID==-1)
                    //					{
                    //						strSQL = "SELECT max(nvl(sortid,0)) FROM Es_Catalog WHERE parentid = " + this.ParentID.ToString() + " AND deleted = " + (int)eO_Deleted.eNormal;
                    //						this.SortID = (OracleDbHelper.ExecuteScalar(cn,CommandType.Text,strSQL)==null) ? 0: (int)OracleDbHelper.ExecuteScalar(cn,CommandType.Text,strSQL) +1;
                    //					}

                    this.SortID = CheckSortID(this.SortID);



                    strSQL = "INSERT INTO Es_Catalog (Catalogid,fullid,orgid,parentid,Catalogname,sortid,Remark,Deleted,UpdateTime,configureSchema,IsShowSchema)" +
                        " Values(" +
                        this.CatalogID.ToString() + "," +
                        StringTool.SqlQ(strFullID) + "," +
                        this.OrgID.ToString() + "," +
                        //this.CatalogKind.ToString() + "," +
                        //this.CatalogType.ToString() + "," +
                        this.ParentID.ToString() + "," +
                        StringTool.SqlQ(this.CatalogName) + "," +
                        //this.Layer.ToString() + "," +
                        //this.IconID.ToString() + "," +
                        this.SortID.ToString() + "," +
                        //((int)this.IsTemp).ToString() + "," +
                        //this.ManagerID.ToString() + "," +
                        //this.LeaderID.ToString() + "," +
                        StringTool.SqlQ(this.Remark) + "," +
                        //StringTool.EmptyToNullDate(this.StartDate) + "," +
                        //StringTool.EmptyToNullDate(this.EndDate) + "," +
                        "0" + "," +
                        "sysdate" + "," +
                        StringTool.SqlQ(this.ConfigureSchema) + "," +
                        this.IsShowSchema.ToString() +
                        //StringTool.SqlQ(this.imglog) +
                        //this.CreatorID.ToString() + ",sysdate," +
                        //this.PID +", " +
                        //StringTool.SqlQ(this.CatalogCode) + " , "+
                        //StringTool.SqlQ(this.DOC_Center) +
                        ")";

                    OracleDbHelper.ExecuteNonQuery(trans, CommandType.Text, strSQL);

                    this.OldParentID = this.ParentID;

                    trans.Commit();

                    this.SortID = iMaxSortID;

                    //this.CatalogID = lngNextID;
                    this.FullID = strFullID;
                    //this.OldManagerID = this.ManagerID;   //防止保存后马上修改会出现异常的情况
                    //this.CreateDate = StringTool.ToMyDateFormat(DateTime.Now);
                }
            }
            catch (OracleException sqle)
            {
                trans.Rollback();
                throw new EpowerException(sqle.Message.ToString(), "CatalogEntityEntity.cs/[private void add]", sqle.Source);
            }
            catch (Exception ee)
            {
                trans.Rollback();
                throw ee;
            }
            finally
            {
                ConfigTool.CloseConnection(cn);
            }
        }


        private int CheckSortID(int SortID)
        {
            //如果SortID为-1,则系统自动为其指定SortID
            if (SortID == -1)
            {
                OracleConnection cn = ConfigTool.GetConnection();
                try
                {
                    string strSQL = "SELECT max(nvl(sortid,-1)) FROM Es_Catalog WHERE parentid = " + this.ParentID.ToString() + " AND deleted = " + (int)eO_Deleted.eNormal;

                    object obj = OracleDbHelper.ExecuteScalar(cn, CommandType.Text, strSQL);
                    if (obj == System.DBNull.Value)
                    {
                        SortID = 0;
                    }
                    else
                    {
                        SortID = int.Parse(obj.ToString()) + 1;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    ConfigTool.CloseConnection(cn);
                }
            }
            return SortID;
        }
        #endregion
    }
}
