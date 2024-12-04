/*******************************************************************
 *
 * Description:服务级别数据层
 * 
 * 
 * Create By  :zhumc
 * Create Date:2008年4月23日
 * *****************************************************************/
using System;
using System.IO;
using System.Xml;
using System.Data;
using System.Data.OracleClient;
using Epower.DevBase.BaseTools;
using Epower.ITSM.Base;
using EpowerGlobal;
using System.Collections;

namespace Epower.ITSM.SqlDAL
{
    /// <summary>
    /// 
    /// </summary>
    public class Cst_ServiceLevelDP
    {
        /// <summary>
        /// 
        /// </summary>
        public Cst_ServiceLevelDP()
        { }

        #region Property
        #region ID
        /// <summary>
        ///
        /// </summary>
        private Decimal mID;
        public Decimal ID
        {
            get { return mID; }
            set { mID = value; }
        }
        #endregion

        #region LevelName
        /// <summary>
        ///
        /// </summary>
        private String mLevelName = string.Empty;
        public String LevelName
        {
            get { return mLevelName; }
            set { mLevelName = value; }
        }
        #endregion

        #region Definition
        /// <summary>
        ///
        /// </summary>
        private String mDefinition = string.Empty;
        public String Definition
        {
            get { return mDefinition; }
            set { mDefinition = value; }
        }
        #endregion

        #region BaseLevel
        /// <summary>
        ///
        /// </summary>
        private String mBaseLevel = string.Empty;
        public String BaseLevel
        {
            get { return mBaseLevel; }
            set { mBaseLevel = value; }
        }
        #endregion

        #region NotInclude
        /// <summary>
        ///
        /// </summary>
        private String mNotInclude = string.Empty;
        public String NotInclude
        {
            get { return mNotInclude; }
            set { mNotInclude = value; }
        }
        #endregion

        #region Availability
        /// <summary>
        ///
        /// </summary>
        private String mAvailability = string.Empty;
        public String Availability
        {
            get { return mAvailability; }
            set { mAvailability = value; }
        }
        #endregion

        #region Charge
        /// <summary>
        ///
        /// </summary>
        private String mCharge = string.Empty;
        public String Charge
        {
            get { return mCharge; }
            set { mCharge = value; }
        }
        #endregion

        #region Condition
        /// <summary>
        ///
        /// </summary>
        private String mCondition;
        public String Condition
        {
            get { return mCondition; }
            set { mCondition = value; }
        }
        #endregion

        #region Deleted
        /// <summary>
        ///
        /// </summary>
        private Int32 mDeleted;
        public Int32 Deleted
        {
            get { return mDeleted; }
            set { mDeleted = value; }
        }
        #endregion

        #region IsAvail
        /// <summary>
        ///
        /// </summary>
        private Int32 mIsAvail;
        public Int32 IsAvail
        {
            get { return mIsAvail; }
            set { mIsAvail = value; }
        }
        #endregion

        #region LastUpdate
        /// <summary>
        ///
        /// </summary>
        private DateTime mLastUpdate = DateTime.MinValue;
        public DateTime LastUpdate
        {
            get { return mLastUpdate; }
            set { mLastUpdate = value; }
        }
        #endregion

        #region updateUserID
        /// <summary>
        ///
        /// </summary>
        private Decimal mupdateUserID;
        public Decimal updateUserID
        {
            get { return mupdateUserID; }
            set { mupdateUserID = value; }
        }
        #endregion

        #endregion

        #region GetReCorded
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lngID"></param>
        /// <returns>Cst_ServiceLevelDP</returns>
        public Cst_ServiceLevelDP GetReCorded(long lngID)
        {
            Cst_ServiceLevelDP ee = new Cst_ServiceLevelDP();

            DataTable dt;

               //2008-05-01 添加SQL缓存依赖的处理方式,减少数据库连接次数
            if (System.Configuration.ConfigurationSettings.AppSettings["SqlCacheModel"] == "1")
            {

                dt = CommSqlCacheHelper.GetDataTableFromCache("cstservicelevel");

                dt.DefaultView.RowFilter = " ID = " + lngID.ToString();

                foreach (DataRowView dvr in dt.DefaultView)
                {
                    ee.ID = Decimal.Parse(dvr.Row["ID"].ToString());
                    ee.LevelName = dvr.Row["LevelName"].ToString();
                    ee.Definition = dvr.Row["Definition"].ToString();
                    ee.BaseLevel = dvr.Row["BaseLevel"].ToString();
                    ee.NotInclude = dvr.Row["NotInclude"].ToString();
                    ee.Availability = dvr.Row["Availability"].ToString();
                    ee.Charge = dvr.Row["Charge"].ToString();
                    ee.Condition = dvr.Row["Condition"].ToString();
                    ee.Deleted = Int32.Parse(dvr.Row["Deleted"].ToString());
                    ee.IsAvail = Int32.Parse(dvr.Row["IsAvail"].ToString());
                    ee.LastUpdate = dvr.Row["LastUpdate"].ToString() == string.Empty ? DateTime.MinValue : DateTime.Parse(dvr.Row["LastUpdate"].ToString());
                    ee.updateUserID = Decimal.Parse(dvr.Row["updateUserID"].ToString());

                }
                return ee;


            }
            else
            {

                string strSQL = string.Empty;
                OracleConnection cn = ConfigTool.GetConnection();
                strSQL = "SELECT * FROM Cst_ServiceLevel WHERE ID = " + lngID.ToString();
                dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
                ConfigTool.CloseConnection(cn);
                foreach (DataRow dr in dt.Rows)
                {
                    ee.ID = Decimal.Parse(dr["ID"].ToString());
                    ee.LevelName = dr["LevelName"].ToString();
                    ee.Definition = dr["Definition"].ToString();
                    ee.BaseLevel = dr["BaseLevel"].ToString();
                    ee.NotInclude = dr["NotInclude"].ToString();
                    ee.Availability = dr["Availability"].ToString();
                    ee.Charge = dr["Charge"].ToString();
                    ee.Condition = dr["Condition"].ToString();
                    ee.Deleted = Int32.Parse(dr["Deleted"].ToString());
                    ee.IsAvail = Int32.Parse(dr["IsAvail"].ToString());
                    ee.LastUpdate = dr["LastUpdate"].ToString() == string.Empty ? DateTime.MinValue : DateTime.Parse(dr["LastUpdate"].ToString());
                    ee.updateUserID = Decimal.Parse(dr["updateUserID"].ToString());
                }
                return ee;
            }
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
             DataTable dt;
               //2008-05-01 添加SQL缓存依赖的处理方式,减少数据库连接次数
             if (System.Configuration.ConfigurationSettings.AppSettings["SqlCacheModel"] == "1")
             {

                 dt = CommSqlCacheHelper.GetDataTableFromCache("cstservicelevel");

                 DataTable dtTemp = dt.Clone();
                 //注意            *******      SWHERE 格式的合法性    ******
                 dt.DefaultView.RowFilter = " deleted = 0 " + sWhere;

                 dtTemp.Rows.Clear();
                 if (sOrder != string.Empty)
                     dt.DefaultView.Sort = "sortid";
                 foreach (DataRowView dvr in dt.DefaultView)
                 {
                     dtTemp.Rows.Add(dvr.Row.ItemArray);

                 }
                 return dtTemp;
             }
             else
             {
                 string strSQL = string.Empty;
                 OracleConnection cn = ConfigTool.GetConnection();
                 strSQL = "SELECT * FROM Cst_ServiceLevel Where 1=1 And Deleted=0 And isAvail =0";
                 strSQL += sWhere;
                 strSQL += sOrder;
                 dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
                 ConfigTool.CloseConnection(cn);
                 return dt;
             }
        }

        public DataTable GetDataTable(string sWhere, string sOrder, int pagesize, int pageindex, ref int rowcount)
        {           
            string strWhere = "1=1 And Deleted=0" + sWhere;
            OracleConnection cn = ConfigTool.GetConnection();
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, "Cst_ServiceLevel", "*", sOrder, pagesize, pageindex, strWhere, ref rowcount);
            ConfigTool.CloseConnection(cn);
            return dt;
        }

        #endregion


        #region GetDataTableForSelect
       /// <summary>
        /// 根据条件自动匹配吻合的服务级别
       /// </summary>
       /// <param name="lngCustID"></param>
       /// <param name="lngEquID"></param>
       /// <param name="lngTypeID"></param>
       /// <param name="lngKindID"></param>
       /// <param name="lngEffID"></param>
       /// <param name="lngInsID"></param>
       /// <returns></returns>
        public DataTable GetDataTableForSelect(long lngCustID,long lngEquID,long lngTypeID,long lngKindID,long lngEffID,long lngInsID)
        {
            DataTable dt;
               //2008-05-01 添加SQL缓存依赖的处理方式,减少数据库连接次数
            //if (System.Configuration.ConfigurationSettings.AppSettings["SqlCacheModel"] == "1")
            //{

            //    dt = CommSqlCacheHelper.GetDataTableFromCache("cstservicelevel");

            //    DataTable dtTemp = dt.Clone();
            //    //注意            *******      SWHERE 格式的合法性    ******
            //    dt.DefaultView.RowFilter = " isavail = 0 And deleted = 0 ";

            //    dtTemp.Rows.Clear();
               
            //    foreach (DataRowView dvr in dt.DefaultView)
            //    {
            //        dtTemp.Rows.Add(dvr.Row.ItemArray);

            //    }
            //    dt = dtTemp;
            //}
            //else
            //{
                string strSQL = string.Empty;
                OracleConnection cn = ConfigTool.GetConnection();
                strSQL = "SELECT * FROM Cst_ServiceLevel Where isavail = 0 And Deleted=0 ";
                dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
                ConfigTool.CloseConnection(cn);
            //}


            DataTable dtMC = null;      //服务单位资料     在算法中只获取一次
            DataTable dtEC = null;      //客户资料
            DataTable dtEqu = null;     //资产资料

            string[] sArr = new string[dt.Rows.Count];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                sArr[i] = dt.Rows[i]["Condition"].ToString();
            }

            for (int i = sArr.Length -1; i >=0; i--)
            {
                string sXml = sArr[i];
                if (sXml == "")
                {
                    //未设置规则者视为不符合条件的情况,         ???考虑展示时 特殊表示???
                    //dt.Rows[i].Delete();
                }
                else
                {
                    bool isOk = false;

                    Hashtable ht = CreateHashTable(sXml);
                    if (ht != null && ht.Keys.Count > 0)
                    {
                        foreach (DictionaryEntry de in ht)
                        {
                            bool result = CheckConditionXmlValue(de.Value.ToString(), lngCustID, lngEquID, lngTypeID, lngKindID, lngEffID, lngInsID, ref dtMC, ref dtEC, ref dtEqu);
                            if (result)
                            {
                                isOk = true;
                                break;
                            }
                        }
                    }
                    if (!isOk)
                        dt.Rows[i].Delete();

                    //if (CheckConditionXmlValue(sXml,lngCustID,lngEquID,lngTypeID,lngKindID,lngEffID,lngInsID,ref dtMC,ref dtEC,ref dtEqu) == false)
                    //{
                    //    dt.Rows[i].Delete();
                    //}
                }
            }
            dt.AcceptChanges();
            

           
            return dt;
        }
        /// <summary>
        /// 创建 datatable结构
        /// </summary>
        /// <returns></returns>
        private DataTable CreateNullTable()
        {
            DataTable dt = new DataTable("ServiceLevelRule");
            dt.Columns.Add("id");
            dt.Columns.Add("Relation");
            dt.Columns.Add("GroupValue");
            dt.Columns.Add("CondItem");
            dt.Columns.Add("CondType");
            dt.Columns.Add("Operate");
            dt.Columns.Add("Expression");
            dt.Columns.Add("Tag");

            return dt;
        }

        private Hashtable CreateHashTable(string s)
        {

            Hashtable ht = new Hashtable(); //保存分组数量
            Hashtable ht2 = new Hashtable(); //按分组保存所有的XML串            

            if (s != "")
            {
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.LoadXml(s);

                XmlNodeList ns = xmldoc.DocumentElement.SelectNodes("Condition");
                //获取所有分组
                foreach (XmlNode n in ns)
                {
                    if (n.Attributes["GroupValue"] != null)
                    {
                        if (!ht.Contains(n.Attributes["GroupValue"].Value))
                        {
                            ht.Add(n.Attributes["GroupValue"].Value, n.Attributes["GroupValue"].Value);
                        }
                    }
                }

                //根据分组信息 获取不同分组的XML串
                if (ht != null && ht.Keys.Count > 0)
                {
                    foreach (DictionaryEntry de in ht)
                    {
                        DataTable tab = CreateNullTable();
                        object[] values = new object[8];
                        foreach (XmlNode n in ns)
                        {
                            if (n.Attributes["GroupValue"].Value.Equals(de.Value.ToString()))
                            {
                                values[0] = (object)n.Attributes["ID"].Value;
                                values[1] = (object)n.Attributes["Relation"].Value;
                                values[2] = (object)n.Attributes["GroupValue"].Value;
                                values[3] = (object)n.Attributes["CondItem"].Value;
                                values[4] = (object)n.Attributes["CondType"].Value;
                                values[5] = (object)n.Attributes["Operate"].Value;
                                values[6] = (object)n.Attributes["Expression"].Value;
                                values[7] = (object)n.Attributes["Tag"].Value;
                                tab.Rows.Add(values);
                            }

                        }
                        string groupXml = GetSchemaXml(tab);
                        ht2.Add(de.Value, groupXml);
                    }
                }

            }

            return ht2;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private string GetSchemaXml(DataTable dt)
        {
            if (dt.Rows.Count == 0)
                return "";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(@"<Conditions></Conditions>");
            int i = 0;
            foreach (DataRow row in dt.Rows)
            {

                XmlElement xmlEle = xmlDoc.CreateElement("Condition");
                xmlEle.SetAttribute("ID", row["ID"].ToString().Trim());
                xmlEle.SetAttribute("Relation", row["Relation"].ToString().Trim());
                xmlEle.SetAttribute("GroupValue", row["GroupValue"].ToString().Trim());
                xmlEle.SetAttribute("CondItem", row["CondItem"].ToString().Trim());
                xmlEle.SetAttribute("CondType", row["CondType"].ToString().Trim());
                xmlEle.SetAttribute("Operate", row["Operate"].ToString().Trim());
                xmlEle.SetAttribute("Expression", row["Expression"].ToString().Trim());
                xmlEle.SetAttribute("Tag", row["Tag"].ToString().Trim());
                xmlDoc.DocumentElement.AppendChild(xmlEle);
            }
            return xmlDoc.InnerXml;

        }

        /// <summary>
        /// 判断是否符合条件
        /// </summary>
        /// <param name="sCond"></param>
        /// <param name="lngCustID"></param>
        /// <param name="lngEquID"></param>
        /// <param name="lngTypeID"></param>
        /// <param name="lngKindID"></param>
        /// <param name="lngEffID"></param>
        /// <param name="lngInsID"></param>
        /// <returns></returns>
        private bool CheckConditionXmlValue(string sCond, long lngCustID, long lngEquID, long lngTypeID, long lngKindID, long lngEffID,
                     long lngInsID, ref DataTable dtMC, ref DataTable dtEC, ref DataTable dtEqu)
        {
            /// 算法描述：
            ///    1、按顺序循环取出每一次判断所需的规则资料：字段ID、条件类别、值、比较关系、特殊TAG、下一条件的关联（可能没有了）
            ///    2、当不存在下一关联时，返回当次判断的结果
            ///    3、当存在下一关联时 4种情况
            ///			A、T 接 OR  直接返回 T
            ///			B、F 接 AND  直接返回 F
            ///			C、T 接 AND  返回下一循环判断的结果
            ///			D、F 接 OR  返回下一循环判断的结果
            ///			

            bool IsFirst = true;
            bool IsOK = false;

            bool blnCurrResult = false;

            long lngCurrItem = -1;
            long lngPreItem = 0;
            e_fm_RELATION_TYPE lngRelation = 0;
            e_ITSMSeviceLevelItem lngCondType = 0;
            e_fm_COMPARE_TYPE lngOperate = 0;
            
            string strExpress = "";
            string strTag = "";


            
            

            if (sCond.Trim() == "<Conditions></Conditions>")
            {
                //没有设置条件，直接返回TRUE
                return true;
            }


            XmlTextReader tr = new XmlTextReader(new StringReader(sCond));
            while (tr.Read())
            {

                // 注意： 此算法严格要求读取节点顺序，一定要以 ID开头、CondRelation为第二个读取的节点，否则此算法会出错！
                if (tr.NodeType == XmlNodeType.Element)
                {
                    if (tr.Name == "Condition")
                    {
                        lngPreItem = lngCurrItem;
                        if (lngPreItem != -1)
                        {
                            //第二次读到条件ID时，表示不是开始，则可以进入条件判
                            IsFirst = false;
                        }
                        lngCurrItem = long.Parse(tr.GetAttribute("ID"));

                        lngRelation = (e_fm_RELATION_TYPE)(int.Parse(tr.GetAttribute("Relation")));

                        if (IsFirst == false)
                        {
                            //第二次读到 关联关系时为一次读取信息周期，并将目前的变量作为一次条件判断！
                            blnCurrResult = ParseCondition(lngCustID, lngEquID, lngTypeID, lngKindID, lngEffID, lngInsID,
                                lngCondType, lngOperate, strExpress, strTag, ref dtMC, ref dtEC, ref dtEqu);
                            if ((blnCurrResult == false && lngRelation == e_fm_RELATION_TYPE.fmConditionAnd) ||
                                (blnCurrResult == true && lngRelation == e_fm_RELATION_TYPE.fmConditionOr))
                            {
                                //结束循环返回结果
                                IsOK = true;
                                tr.Close();
                                break;
                            }
                        }

                        lngOperate = (e_fm_COMPARE_TYPE)(int.Parse(tr.GetAttribute("Operate")));

                        strExpress = tr.GetAttribute("Expression");

                        lngCondType = (e_ITSMSeviceLevelItem)(int.Parse(tr.GetAttribute("CondType").Split(",".ToCharArray())[0]));
                        strTag = tr.GetAttribute("Tag");



                    }
                  
                }

            }

            // 最后一个条件还没有得出结果时，就取决于它了：：：
            if (IsOK == false)
                blnCurrResult = ParseCondition(lngCustID, lngEquID, lngTypeID, lngKindID, lngEffID, lngInsID,
                                lngCondType, lngOperate, strExpress, strTag, ref dtMC, ref dtEC, ref dtEqu);

            return blnCurrResult;



        }





        /// <summary>
        /// 直接获取表结果集
        /// </summary>
        /// <param name="strSQL"></param>
        /// <returns></returns>
        private DataTable GetDataTableDirect(string strSQL)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigTool.CloseConnection(cn);
            return dt;
        }


        /// <summary>
        /// 获取相关资料当前值. 整个算法中只进行一次SQL 计算
        ///  服务级别中只返回两中数据类型的可能, 一般是string  类别相关的用 number 但通过fullid进行匹配
        /// </summary>
        /// <param name="lngCondType"></param>
        /// <param name="lngCustID"></param>
        /// <param name="lngEquID"></param>
        /// <param name="strCurrValue"></param>
        /// <param name="lngDataType"></param>
        /// <param name="mc"></param>
        /// <param name="ec"></param>
        /// <param name="equ"></param>
        /// <returns></returns>
        private bool GetCurrValueInDB(e_ITSMSeviceLevelItem lngCondType,long lngCustID, long lngEquID, ref string strCurrValue, ref e_DataType lngDataType,
                              ref DataTable mc, ref DataTable ec, ref DataTable equ)
        {

            string strSQL = "";
            lngDataType = e_DataType.eString;
            strCurrValue = "";
            string sID = "0";
            bool blnHas = false;
            switch (lngCondType)
            {
                case e_ITSMSeviceLevelItem.eitsmSLIMcFullName:
                    if (mc == null)
                    {
                        strSQL = "SELECT a.* FROM Br_MastCustomer a,Br_ECustomer b WHERE a.id = b.mastcustid AND b.id = " + lngCustID.ToString();
                        mc = GetDataTableDirect(strSQL);
                    }
                    if (mc.Rows.Count > 0)
                    {
                        lngDataType = e_DataType.eString;
                        strCurrValue = mc.Rows[0]["ShortName"].ToString().Trim();
                        blnHas = true;
                    }
                    break;
                case e_ITSMSeviceLevelItem.eitsmSLIMcCustomerType:
                    if (mc == null)
                    {
                        strSQL = "SELECT a.* FROM Br_MastCustomer a,Br_ECustomer b WHERE a.id = b.mastcustid AND b.id = " + lngCustID.ToString();
                        mc = GetDataTableDirect(strSQL);
                    }
                    if (mc.Rows.Count > 0)
                    {
                        lngDataType = e_DataType.eNumber;
                        sID = mc.Rows[0]["CustomerType"].ToString().Trim();
                        if (sID != "")
                        {
                            strCurrValue = CatalogDP.GetCatalogFullID(long.Parse(sID));

                        }
                        blnHas = true;
                    }
                    break;
                case e_ITSMSeviceLevelItem.eitsmSLIMcEnterpriseType:
                    if (mc == null)
                    {
                        strSQL = "SELECT a.* FROM Br_MastCustomer a,Br_ECustomer b WHERE a.id = b.mastcustid AND b.id = " + lngCustID.ToString();
                        mc = GetDataTableDirect(strSQL);
                    }
                    if (mc.Rows.Count > 0)
                    {
                        lngDataType = e_DataType.eNumber;
                        sID = mc.Rows[0]["EnterpriseType"].ToString().Trim();
                        if (sID != "")
                        {
                            strCurrValue = CatalogDP.GetCatalogFullID(long.Parse(sID));
                        }
                        blnHas = true;
                    }
                    break;
                case e_ITSMSeviceLevelItem.eitsmSLIMcAddress:
                    if (mc == null)
                    {
                        strSQL = "SELECT a.* FROM Br_MastCustomer a,Br_ECustomer b WHERE a.id = b.mastcustid AND b.id = " + lngCustID.ToString();
                        mc = GetDataTableDirect(strSQL);
                    }
                    if (mc.Rows.Count > 0)
                    {
                        lngDataType = e_DataType.eString;
                        strCurrValue = mc.Rows[0]["Address"].ToString().Trim();
                        blnHas = true;
                    }
                    break;

                case e_ITSMSeviceLevelItem.eitsmSLIEcFullName:
                    if (ec == null)
                    {
                        strSQL = "SELECT * FROM Br_ECustomer WHERE id = " + lngCustID.ToString();
                        ec = GetDataTableDirect(strSQL);
                    }
                    if (ec.Rows.Count > 0)
                    {
                        lngDataType = e_DataType.eString;
                        strCurrValue = ec.Rows[0]["ShortName"].ToString().Trim();
                        blnHas = true;
                    }
                    break;
                case e_ITSMSeviceLevelItem.eitsmSLIEcCustCode:
                    if (ec == null)
                    {
                        strSQL = "SELECT * FROM Br_ECustomer WHERE id = " + lngCustID.ToString();
                        ec = GetDataTableDirect(strSQL);
                    }
                    if (ec.Rows.Count > 0)
                    {
                        lngDataType = e_DataType.eString;
                        strCurrValue = ec.Rows[0]["CustomCode"].ToString().Trim();
                        blnHas = true;
                    }
                    break;
                case e_ITSMSeviceLevelItem.eitsmSLIEcCustomerType:
                    if (ec == null)
                    {
                        strSQL = "SELECT * FROM Br_ECustomer WHERE id = " + lngCustID.ToString();
                        ec = GetDataTableDirect(strSQL);
                    }
                    if (ec.Rows.Count > 0)
                    {
                        lngDataType = e_DataType.eNumber;
                        sID = ec.Rows[0]["CustomerType"].ToString().Trim();
                        if (sID != "")
                        {
                            strCurrValue = CatalogDP.GetCatalogFullID(long.Parse(sID));
                        }
                        blnHas = true;
                    }
                    break;
                case e_ITSMSeviceLevelItem.eitsmSLIEcCustDeptName:
                    if (ec == null)
                    {
                        strSQL = "SELECT * FROM Br_ECustomer WHERE id = " + lngCustID.ToString();
                        ec = GetDataTableDirect(strSQL);
                    }
                    if (ec.Rows.Count > 0)
                    {
                        lngDataType = e_DataType.eString;
                        strCurrValue = ec.Rows[0]["CustDeptName"].ToString().Trim();
                        blnHas = true;
                    }
                    break;
                case e_ITSMSeviceLevelItem.eitsmSLIEcRights:
                    if (ec == null)
                    {
                        strSQL = "SELECT * FROM Br_ECustomer WHERE id = " + lngCustID.ToString();
                        ec = GetDataTableDirect(strSQL);
                    }
                    if (ec.Rows.Count > 0)
                    {
                        lngDataType = e_DataType.eString;
                        strCurrValue = ec.Rows[0]["Rights"].ToString().Trim();
                        blnHas = true;
                    }
                    break;
                case e_ITSMSeviceLevelItem.eitsmSLIEcAddress:
                    if (ec == null)
                    {
                        strSQL = "SELECT * FROM Br_ECustomer WHERE id = " + lngCustID.ToString();
                        ec = GetDataTableDirect(strSQL);
                    }
                    if (ec.Rows.Count > 0)
                    {
                        lngDataType = e_DataType.eString;
                        strCurrValue = ec.Rows[0]["Address"].ToString().Trim();
                        blnHas = true;
                    }
                    break;

                case e_ITSMSeviceLevelItem.eitsmSLIEqName:
                    if (equ == null)
                    {
                        strSQL = "SELECT * FROM Equ_Desk WHERE id = " + lngEquID.ToString();
                        equ = GetDataTableDirect(strSQL);
                    }
                    if (equ.Rows.Count > 0)
                    {
                        lngDataType = e_DataType.eString;
                        strCurrValue = equ.Rows[0]["Name"].ToString().Trim();
                        blnHas = true;
                    }
                    break;
                case e_ITSMSeviceLevelItem.eitsmSLIEqCode:
                    if (equ == null)
                    {
                        strSQL = "SELECT * FROM Equ_Desk WHERE id = " + lngEquID.ToString();
                        equ = GetDataTableDirect(strSQL);
                    }
                    if (equ.Rows.Count > 0)
                    {
                        lngDataType = e_DataType.eString;
                        strCurrValue = equ.Rows[0]["Code"].ToString().Trim();
                        blnHas = true;
                    }
                    break;
                case e_ITSMSeviceLevelItem.eitsmSLIEqCatalogID:
                    if (equ == null)
                    {
                        strSQL = "SELECT * FROM Equ_Desk WHERE id = " + lngEquID.ToString();
                        equ = GetDataTableDirect(strSQL);
                    }
                    if (equ.Rows.Count > 0)
                    {
                        lngDataType = e_DataType.eNumber;
                        sID = equ.Rows[0]["CatalogID"].ToString().Trim();
                        if (sID != "")
                        {
                            strCurrValue = Equ_SubjectDP.GetSubjectFullID(long.Parse(sID));
                        }
                        blnHas = true;
                    }
                    break;
                case e_ITSMSeviceLevelItem.eitsmSLIEqSerialNumber:
                    if (equ == null)
                    {
                        strSQL = "SELECT * FROM Equ_Desk WHERE id = " + lngEquID.ToString();
                        equ = GetDataTableDirect(strSQL);
                    }
                    if (equ.Rows.Count > 0)
                    {
                        lngDataType = e_DataType.eString;
                        strCurrValue = equ.Rows[0]["SerialNumber"].ToString().Trim();
                        blnHas = true;
                    }
                    break;
                case e_ITSMSeviceLevelItem.eitsmSLIEqPositions:
                    if (equ == null)
                    {
                        strSQL = "SELECT * FROM Equ_Desk WHERE id = " + lngEquID.ToString();
                        equ = GetDataTableDirect(strSQL);
                    }
                    if (equ.Rows.Count > 0)
                    {
                        lngDataType = e_DataType.eString;
                        strCurrValue = equ.Rows[0]["Positions"].ToString().Trim();
                        blnHas = true;
                    }
                    break;
            }

            return blnHas;


        }

        /// <summary>
        /// 解析条件
        /// </summary>
        /// <param name="strValues"></param>
        /// <param name="lngCondType"></param>
        /// <param name="lngOperate"></param>
        /// <param name="strExpress"></param>
        /// <param name="strTag"></param>
        /// <returns></returns>
        private bool ParseCondition(long lngCustID, long lngEquID, long lngTypeID, long lngKindID, long lngEffID, long lngInsID, 
                             e_ITSMSeviceLevelItem lngCondType,e_fm_COMPARE_TYPE lngOperate,  string strExpress, string strTag,
                              ref DataTable mc,ref DataTable ec,ref DataTable equ)
        {
            bool IsTrue = false;
            string strCurrValue = "";
            e_DataType lngDataType = e_DataType.eString;
            bool blnHasValue = false;
            string strDeptIDList = "";
            long lngRefID = 0;   //信息项的关联编号 ，此处用于判断是否 部门或人员关联 ，字典关联不必理会

            switch (lngCondType)
            {
                case e_ITSMSeviceLevelItem.eitsmSLIMcFullName:
                case e_ITSMSeviceLevelItem.eitsmSLIMcCustomerType:
                case e_ITSMSeviceLevelItem.eitsmSLIMcEnterpriseType:
                case e_ITSMSeviceLevelItem.eitsmSLIMcAddress:
                case e_ITSMSeviceLevelItem.eitsmSLIEcFullName:
                case e_ITSMSeviceLevelItem.eitsmSLIEcCustCode:
                case e_ITSMSeviceLevelItem.eitsmSLIEcCustomerType:
                case e_ITSMSeviceLevelItem.eitsmSLIEcCustDeptName:
                case e_ITSMSeviceLevelItem.eitsmSLIEcRights:
                case e_ITSMSeviceLevelItem.eitsmSLIEcAddress:
                    if (lngCustID == 0)
                    {
                        IsTrue = true;
                    }
                    else
                    {
                        blnHasValue = GetCurrValueInDB(lngCondType, lngCustID, lngEquID, ref strCurrValue, ref lngDataType, ref mc, ref ec, ref equ);
                        if (blnHasValue == true)
                        {
                            IsTrue = ParseExpression(strCurrValue, strExpress, lngDataType, lngOperate);
                        }
                        else
                        {
                            IsTrue = false;
                        }
                    }
                    break;
                case e_ITSMSeviceLevelItem.eitsmSLIEqName:
                case e_ITSMSeviceLevelItem.eitsmSLIEqCode:
                case e_ITSMSeviceLevelItem.eitsmSLIEqCatalogID:
                case e_ITSMSeviceLevelItem.eitsmSLIEqSerialNumber:
                case e_ITSMSeviceLevelItem.eitsmSLIEqPositions:
                    if (lngEquID == 0)
                    {
                        IsTrue = true;
                    }
                    else
                    {
                        blnHasValue = GetCurrValueInDB(lngCondType, lngCustID, lngEquID, ref strCurrValue, ref lngDataType, ref mc, ref ec, ref equ);
                        if (blnHasValue == true)
                        {
                            IsTrue = ParseExpression(strCurrValue, strExpress, lngDataType, lngOperate);
                        }
                        else
                        {
                            IsTrue = false;
                        }
                    }
                    break;
                case e_ITSMSeviceLevelItem.eitsmSLIServiceTypeID:
                case e_ITSMSeviceLevelItem.eitsmSLTServiceKindID:
                case e_ITSMSeviceLevelItem.eitsmSLTEffectID:
                case e_ITSMSeviceLevelItem.eitsmSLTInstancyID:
                    if (lngCondType == e_ITSMSeviceLevelItem.eitsmSLIServiceTypeID)
                    {
                        if (lngTypeID == 0)
                            IsTrue = true;
                        else
                            strCurrValue = CatalogDP.GetCatalogFullID(lngTypeID);
                    }
                    if (lngCondType == e_ITSMSeviceLevelItem.eitsmSLTServiceKindID)
                    {
                        if (lngKindID == 0)
                            IsTrue = true;
                        else
                            strCurrValue = CatalogDP.GetCatalogFullID(lngKindID);
                    }
                    if (lngCondType == e_ITSMSeviceLevelItem.eitsmSLTEffectID)
                    {
                        if (lngEffID == 1002)
                            IsTrue = true;
                        else
                            strCurrValue = CatalogDP.GetCatalogFullID(lngEffID);
                    }
                    if (lngCondType == e_ITSMSeviceLevelItem.eitsmSLTInstancyID)
                    {
                        if (lngInsID == 1024)
                            IsTrue = true;
                        else
                            strCurrValue = CatalogDP.GetCatalogFullID(lngInsID);
                    }

                    if (!IsTrue)    //判断如果是初值，返回True
                    {
                        strExpress = CatalogDP.GetCatalogFullID(long.Parse(strTag==""? "0" : strTag));

                        IsTrue = ParseExpression(strCurrValue, strExpress, lngDataType, lngOperate);
                    }
                    
                    break;
                   
                default:
                    break;
            }


            return IsTrue;

        }

        /// <summary>
        /// 判断表达式是否正确
        ///  在服务级别规则中只用到 "等于  0" "不等于 1" "[>=]以..开头 3" "包含[属于] 6" "不包含[不属于]7";
        /// </summary>
        /// <param name="strCurrValue"></param>
        /// <param name="strExpress"></param>
        /// <param name="lngDataType"></param>
        /// <param name="lngOperate"></param>
        /// <returns></returns>
        private bool ParseExpression(string strCurrValue, string strExpress, e_DataType lngDataType, e_fm_COMPARE_TYPE lngOperate)
        {
            bool IsTrue = false;
            switch (lngOperate)
            {
                case e_fm_COMPARE_TYPE.fmCompareEquare:
                    IsTrue = (strCurrValue.ToLower().Trim() == strExpress.ToLower().Trim());
                    break;
                case e_fm_COMPARE_TYPE.fmCompareNotEquare:
                    IsTrue = (strCurrValue.ToLower().Trim() != strExpress.ToLower().Trim());
                    break;
                case e_fm_COMPARE_TYPE.fmCompareNotLess:   //大于等于 ,在服务级别中 表示 以..开头
                    if (lngDataType == e_DataType.eString)
                    {
                        IsTrue = (strCurrValue.ToLower().IndexOf(strExpress.ToLower()) == 0);
                        
                    }
                    else
                    {
                        IsTrue = false;
                    }
                    break;
               
                case e_fm_COMPARE_TYPE.fmCompareInclude:
                    if (lngDataType == e_DataType.eString)
                    {
                        //只有字符串才进行包含不包含的比较
                        IsTrue = (strCurrValue.IndexOf(strExpress) >= 0);
                    }
                    else if (lngDataType == e_DataType.eNumber)
                    {
                        //这里只可能是 类型比较,比较fullid
                        if (strExpress == "")
                        {
                            //根分类
                            IsTrue = true;
                        }
                        else
                        {
                            IsTrue = (strCurrValue.ToLower().IndexOf(strExpress.ToLower()) == 0);
                        }
                    }
                    else
                    {

                        
                        IsTrue = false;
                    }
                    break;
                case e_fm_COMPARE_TYPE.fmCompareNotInclude:
                    if (lngDataType == e_DataType.eString)
                    {
                        //只有字符串才进行包含不包含的比较
                        IsTrue = (strCurrValue.IndexOf(strExpress) < 0);
                    }
                    else if (lngDataType == e_DataType.eNumber)
                    {
                        //这里只可能是 类型比较,比较fullid
                        if (strExpress == "")
                        {
                            //根分类
                            IsTrue = false;
                        }
                        else
                        {
                            IsTrue = (strCurrValue.ToLower().IndexOf(strExpress.ToLower()) != 0);
                        }
                    }
                    else
                    {
                        IsTrue = false;
                    }
                    break;
                default:
                    IsTrue = false;
                    break;
            }

            return IsTrue;
        }

        #endregion


        #region InsertRecorded
        /// <summary>
        /// 
        /// </summary>
        /// <param name=pCst_ServiceLevelDP></param>
        public void InsertRecorded(Cst_ServiceLevelDP pCst_ServiceLevelDP)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            string strSQL = string.Empty;
            string strID = "0";
            try
            {
                strID = EpowerGlobal.EPGlobal.GetNextID("Cst_ServiceLevelID").ToString();
                pCst_ServiceLevelDP.ID = decimal.Parse(strID);
                strSQL = @"INSERT INTO Cst_ServiceLevel(
									ID,
									LevelName,
									Definition,
									BaseLevel,
									NotInclude,
									Availability,
									Charge,
									Condition,
									Deleted,
									IsAvail,
									LastUpdate,
									updateUserID
					)
					VALUES( " +
                            strID.ToString() + "," +
                            StringTool.SqlQ(pCst_ServiceLevelDP.LevelName) + "," +
                            StringTool.SqlQ(pCst_ServiceLevelDP.Definition) + "," +
                            StringTool.SqlQ(pCst_ServiceLevelDP.BaseLevel) + "," +
                            StringTool.SqlQ(pCst_ServiceLevelDP.NotInclude) + "," +
                            StringTool.SqlQ(pCst_ServiceLevelDP.Availability) + "," +
                            StringTool.SqlQ(pCst_ServiceLevelDP.Charge) + "," +
                            StringTool.SqlQ(pCst_ServiceLevelDP.Condition) + "," +
                            pCst_ServiceLevelDP.Deleted.ToString() + "," +
                            pCst_ServiceLevelDP.IsAvail.ToString() + "," +
                            (pCst_ServiceLevelDP.LastUpdate == DateTime.MinValue ? " null " : "to_date(" + StringTool.SqlQ(pCst_ServiceLevelDP.LastUpdate.ToString()) + ",'yyyy-MM-dd HH24:mi:ss')") + "," +
                            pCst_ServiceLevelDP.updateUserID.ToString() +
                    ")";

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

        #region UpdateRecorded
        /// <summary>
        /// 
        /// </summary>
        /// <param name=pCst_ServiceLevelDP></param>
        public void UpdateRecorded(Cst_ServiceLevelDP pCst_ServiceLevelDP)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            string strSQL = string.Empty;
            try
            {
                strSQL = @"UPDATE Cst_ServiceLevel Set " +
                                                        " LevelName = " + StringTool.SqlQ(pCst_ServiceLevelDP.LevelName) + "," +
                            " Definition = " + StringTool.SqlQ(pCst_ServiceLevelDP.Definition) + "," +
                            " BaseLevel = " + StringTool.SqlQ(pCst_ServiceLevelDP.BaseLevel) + "," +
                            " NotInclude = " + StringTool.SqlQ(pCst_ServiceLevelDP.NotInclude) + "," +
                            " Availability = " + StringTool.SqlQ(pCst_ServiceLevelDP.Availability) + "," +
                            " Charge = " + StringTool.SqlQ(pCst_ServiceLevelDP.Charge) + "," +
                            " Condition = " + StringTool.SqlQ(pCst_ServiceLevelDP.Condition) + "," +
                            " Deleted = " + pCst_ServiceLevelDP.Deleted.ToString() + "," +
                            " IsAvail = " + pCst_ServiceLevelDP.IsAvail.ToString() + "," +
                            " LastUpdate = " + (pCst_ServiceLevelDP.LastUpdate == DateTime.MinValue ? " null " : "to_date(" + StringTool.SqlQ(pCst_ServiceLevelDP.LastUpdate.ToString()) + ",'yyyy-MM-dd HH24:mi:ss')") + "," +
                            " updateUserID = " + pCst_ServiceLevelDP.updateUserID.ToString() +
                                " WHERE ID = " + pCst_ServiceLevelDP.ID.ToString();

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
                string strSQL = "Update Cst_ServiceLevel Set Deleted=1  WHERE ID =" + lngID.ToString();
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


        #region 扩展代码区

        #endregion


        /// <summary>根据日期，获得星期几</summary>
        /// <param name="y">年</param> 
        /// <param name="m">月</param> 
        /// <param name="d">日</param> 
        /// <returns>星期几，1代表星期一；7代表星期日</returns>
        private static int getWeekDay(int y, int m, int d)
        {
            //if (m == 1) m = 13;
            //if (m == 2) m = 14;
            //int week = (d + 2 * m + 3 * (m + 1) / 5 + y + y / 4 - y / 100 + y / 400) % 7 + 1;
            //return week;
            int nowworknum = 0;
            DateTime dt = Convert.ToDateTime(y.ToString() + "-" + m.ToString() + "-" + d.ToString());
            switch (dt.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    nowworknum = 7;
                    break;
                case DayOfWeek.Monday:
                    nowworknum = 1;
                    break;
                case DayOfWeek.Tuesday:
                    nowworknum = 2;
                    break;
                case DayOfWeek.Wednesday:
                    nowworknum = 3;
                    break;
                case DayOfWeek.Thursday:
                    nowworknum = 4;
                    break;
                case DayOfWeek.Friday:
                    nowworknum = 5;
                    break;
                case DayOfWeek.Saturday:
                    nowworknum = 6;
                    break;
                default:
                    break;
            }
            return nowworknum;
        }

        public static void WeekSetting(DateTime begin, DateTime end, int wbegin, int wend)
        {
            OracleConnection cn = ConfigTool.GetConnection("SQLConnString");
            cn.Open();
            OracleTransaction tran = cn.BeginTransaction();
            string strSQL = string.Empty;
            int iweek = 0;
            string[] SetWeek = new string[7];
            for (int i = 0; i < 7; i++)
            {
                if ((wbegin + i) == 8)
                {
                    SetWeek[i] = "1";
                }
                else if ((wbegin + i) == 9)
                {
                    SetWeek[i] = "2";
                }
                else if ((wbegin + i) == 10)
                {
                    SetWeek[i] = "3";
                }
                else if ((wbegin + i) == 11)
                {
                    SetWeek[i] = "4";
                }
                else if ((wbegin + i) == 12)
                {
                    SetWeek[i] = "5";
                }
                else if ((wbegin + i) == 13)
                {
                    SetWeek[i] = "6";
                }
                else if ((wbegin + i) == 14)
                {
                    SetWeek[i] = "7";
                }
                else
                {
                    SetWeek[i] = (wbegin + i).ToString();
                }
            }
            DateTime dtbegin = begin;
            DateTime dtend = end;
            string time = "";
            try
            {
                strSQL = @"Delete from Equ_WeekSetting";
                OracleDbHelper.ExecuteNonQuery(tran, CommandType.Text, strSQL);
                while (dtend <= end)
                {
                    iweek = getWeekDay(dtbegin.Year, dtbegin.Month, dtbegin.Day);
                    for (int i = 0; i < 7; i++)
                    {
                        if (SetWeek[i] == iweek.ToString())
                        {
                            dtend = dtbegin.AddDays(6 - i);
                            time = dtbegin.Year.ToString() + "." + dtbegin.Month.ToString() + "." + dtbegin.Day.ToString() + "~" + dtend.Year.ToString() + "." + dtend.Month.ToString() + "." + dtend.Day.ToString();
                            strSQL = @"INSERT INTO Equ_WeekSetting(WeekName,BeginTime,EndTime,wBegin)VALUES(" + StringTool.SqlQ(time) + ",to_date(" + StringTool.SqlQ(dtbegin.ToString("yyyy-MM-dd") + " 00:00") + ",'yyyy-MM-dd HH24:mi:ss'),to_date(" + StringTool.SqlQ(dtend.ToString("yyyy-MM-dd") + " 23:59") + ",'yyyy-MM-dd HH24:mi:ss')," + wbegin + ")";
                            OracleDbHelper.ExecuteNonQuery(tran, CommandType.Text, strSQL);
                            dtbegin = dtend.AddDays(1);
                            break;
                        }
                    }
                }
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

        public static DataTable GetWeekSetting()
        {
            DataTable dt = null;
            string Sql = "select * from Equ_WeekSetting ";
            dt = CommonDP.ExcuteSqlTable(Sql);
            return dt;
        }

        public static void MonthSetting(DateTime beginDate, DateTime endDate, int bMonth, int eMonth)
        {
            OracleConnection cn = ConfigTool.GetConnection("SQLConnString");
            cn.Open();
            OracleTransaction tran = cn.BeginTransaction();
            string strSQL = string.Empty;

            DateTime dtbegin = beginDate;
            DateTime dtend = endDate;
            try
            {
                strSQL = @"Delete from equ_monthsetting";
                OracleDbHelper.ExecuteNonQuery(tran, CommandType.Text, strSQL);
                while (dtbegin < dtend)
                {
                    int maxdays = GetMaxDay(dtbegin.Year, dtbegin.Month, bMonth);

                    string start = dtbegin.Year + "-" + dtbegin.Month + "-" + maxdays;
                    dtbegin = dtbegin.AddMonths(1);
                    int maxdays2 = GetMaxDay(dtbegin.Year, dtbegin.Month, bMonth);
                    string end = DateTime.Parse(dtbegin.Year + "-" + dtbegin.Month + "-" + maxdays2).AddDays(-1).ToString("yyyy-MM-dd");
                    int index = dtbegin.Month;
                    strSQL = @"INSERT INTO equ_monthsetting(MONTHNAME,BEGINTIME,ENDTIME,MINDEX,bMonth)VALUES(" + StringTool.SqlQ(start + "~" + end) + ",to_date(" + StringTool.SqlQ(DateTime.Parse(start).ToString("yyyy-MM-dd") + " 00:00") + ",'yyyy-MM-dd HH24:mi:ss'),to_date(" + StringTool.SqlQ(DateTime.Parse(end).ToString("yyyy-MM-dd") + " 23:59:59") + ",'yyyy-MM-dd HH24:mi:ss')," + index + "," + bMonth + ")";
                    OracleDbHelper.ExecuteNonQuery(tran, CommandType.Text, strSQL);
                }
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

        /// <summary>
        /// 获取某年某月最大天数
        /// </summary>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        /// <param name="bday">用户录入的月份天数</param>
        /// <returns></returns>
        public static int GetMaxDay(int year, int month, int bday)
        {
            int result = bday;
            int maxDays = 0;
            //闰年
            if ((year % 4 == 0 && year % 100 != 0) || year % 400 == 0)
            {
                if (month == 2)
                {
                    maxDays = 29;       // 闰年2月29天                 
                }
                else if (month == 1 || month == 3 || month == 5 || month == 7 || month == 8 || month == 10 || month == 12)
                {
                    maxDays = 31;
                    // 1、3、5、7、8、10、12月都是31天                  
                }
                else
                {
                    maxDays = 30;           // 其他月份都是30天                 
                }
            }
            else
            {
                if (month == 2)
                {
                    maxDays = 28;       // 非闰年2月28天                 
                }
                else if (month == 1 || month == 3 || month == 5 || month == 7 || month == 8 || month == 10 || month == 12)
                {
                    maxDays = 31;       // 1、3、5、7、8、10、12月都是31天              
                }
                else
                {
                    maxDays = 30;           // 其他月份都是30天               
                }   
            }

            if (bday > maxDays)
                result = maxDays;

            return result;
        }

        public static DataTable Getmonthsetting()
        {
            DataTable dt = null;
            string Sql = "select * from Equ_monthsetting ";
            dt = CommonDP.ExcuteSqlTable(Sql);
            return dt;
        }
    }
}

