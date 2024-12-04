/*******************************************************************
 * Description:�ʲ�����
 * Create By  :zhumc
 * Create Date:2008��3��7��
 * *****************************************************************/
using System;
using System.Xml;
using System.Data;
using System.Collections;
using System.Data.OracleClient;
using Epower.DevBase.BaseTools;
using Epower.ITSM.Base;
using Epower.DevBase.Organization.SqlDAL;
using Epower.DevBase.Organization.Base;
using Epower.ITSM.SqlDAL.ES_TBLCS;
using System.Collections.Generic;
using System.Text;
using EpowerGlobal;
using System.IO;
using EpowerCom;

namespace Epower.ITSM.SqlDAL
{
    /// <summary>
    /// �ʲ�����
    /// </summary>
    public class Equ_DeskDP
    {
        #region ���췽��

        /// <summary>
        /// ���췽��
        /// </summary>
        public Equ_DeskDP()
        { }

        #endregion

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

        #region Name
        /// <summary>
        ///
        /// </summary>
        private String mName = string.Empty;
        public String Name
        {
            get { return mName; }
            set { mName = value; }
        }
        #endregion

        #region Code
        /// <summary>
        ///
        /// </summary>
        private String mCode = string.Empty;
        public String Code
        {
            get { return mCode; }
            set { mCode = value; }
        }
        #endregion

        #region Positions
        /// <summary>
        ///
        /// </summary>
        private String mPositions = string.Empty;
        public String Positions
        {
            get { return mPositions; }
            set { mPositions = value; }
        }
        #endregion

        #region Breed
        /// <summary>
        ///
        /// </summary>
        private String mBreed = string.Empty;
        public String Breed
        {
            get { return mBreed; }
            set { mBreed = value; }
        }
        #endregion

        #region Model
        /// <summary>
        ///
        /// </summary>
        private String mModel = string.Empty;
        public String Model
        {
            get { return mModel; }
            set { mModel = value; }
        }
        #endregion

        #region SerialNumber
        /// <summary>
        ///
        /// </summary>
        private String mSerialNumber = string.Empty;
        public String SerialNumber
        {
            get { return mSerialNumber; }
            set { mSerialNumber = value; }
        }
        #endregion

        #region Provide
        /// <summary>
        ///
        /// </summary>
        private Decimal mProvide;
        public Decimal Provide
        {
            get { return mProvide; }
            set { mProvide = value; }
        }
        #endregion

        #region ProvideName
        /// <summary>
        ///
        /// </summary>
        private String mProvideName = string.Empty;
        public String ProvideName
        {
            get { return mProvideName; }
            set { mProvideName = value; }
        }
        #endregion

        #region CatalogID
        /// <summary>
        ///
        /// </summary>
        private Decimal mCatalogID;
        public Decimal CatalogID
        {
            get { return mCatalogID; }
            set { mCatalogID = value; }
        }
        #endregion

        #region CatalogName
        /// <summary>
        ///
        /// </summary>
        private String mCatalogName = string.Empty;
        public String CatalogName
        {
            get { return mCatalogName; }
            set { mCatalogName = value; }
        }
        #endregion

        #region FullID
        /// <summary>
        ///
        /// </summary>
        private String mFullID = string.Empty;
        public String FullID
        {
            get { return mFullID; }
            set { mFullID = value; }
        }
        #endregion

        #region EquStatusID
        /// <summary>
        ///
        /// </summary>
        private Decimal mEquStatusID;
        public Decimal EquStatusID
        {
            get { return mEquStatusID; }
            set { mEquStatusID = value; }
        }
        #endregion

        #region EquStatusName
        /// <summary>
        ///
        /// </summary>
        private String mEquStatusName = string.Empty;
        public String EquStatusName
        {
            get { return mEquStatusName; }
            set { mEquStatusName = value; }
        }
        #endregion

        #region ConfigureValue
        /// <summary>
        ///
        /// </summary>
        private String mConfigureValue = string.Empty;
        public String ConfigureValue
        {
            get { return mConfigureValue; }
            set { mConfigureValue = value; }
        }
        #endregion

        #region Deleted
        /// <summary>
        ///
        /// </summary>
        private Decimal mDeleted;
        public Decimal Deleted
        {
            get { return mDeleted; }
            set { mDeleted = value; }
        }
        #endregion

        #region RegUserID
        /// <summary>
        ///
        /// </summary>
        private Decimal mRegUserID;
        public Decimal RegUserID
        {
            get { return mRegUserID; }
            set { mRegUserID = value; }
        }
        #endregion

        #region UpdateUserID
        /// <summary>
        ///
        /// </summary>
        private Decimal mUpdateUserID;
        public Decimal UpdateUserID
        {
            get { return mUpdateUserID; }
            set { mUpdateUserID = value; }
        }
        #endregion

        #region RegUserName
        /// <summary>
        ///
        /// </summary>
        private String mRegUserName = string.Empty;
        public String RegUserName
        {
            get { return mRegUserName; }
            set { mRegUserName = value; }
        }
        #endregion

        #region RegDeptID
        /// <summary>
        ///
        /// </summary>
        private Decimal mRegDeptID;
        public Decimal RegDeptID
        {
            get { return mRegDeptID; }
            set { mRegDeptID = value; }
        }
        #endregion

        #region RegDeptName
        /// <summary>
        ///
        /// </summary>
        private String mRegDeptName = string.Empty;
        public String RegDeptName
        {
            get { return mRegDeptName; }
            set { mRegDeptName = value; }
        }
        #endregion

        #region RegTime
        /// <summary>
        ///
        /// </summary>
        private DateTime mRegTime = DateTime.MinValue;
        public DateTime RegTime
        {
            get { return mRegTime; }
            set { mRegTime = value; }
        }
        #endregion

        #region partBankId
        private Decimal mpartBankId;
        /// <summary>
        /// ����id
        /// </summary>
        public Decimal partBankId
        {
            get { return mpartBankId; }
            set { mpartBankId = value; }
        }
        #endregion

        #region partBranchId
        private Decimal mpartBranchId;
        /// <summary>
        /// ����id
        /// </summary>
        public Decimal partBranchId
        {
            get { return mpartBranchId; }
            set { mpartBranchId = value; }
        }
        #endregion

        #region partBankName
        private string mpartBankName = string.Empty;
        /// <summary>
        /// ����name
        /// </summary>
        public string partBankName
        {
            get { return mpartBankName; }
            set { mpartBankName = value; }
        }
        #endregion

        #region partBranchName
        private string mpartBranchName = string.Empty;
        /// <summary>
        /// ����name
        /// </summary>
        public string partBranchName
        {
            get { return mpartBranchName; }
            set { mpartBranchName = value; }
        }
        #endregion

        #region ConfigureInfo
        /// <summary>
        ///
        /// </summary>
        private String mConfigureInfo;
        public String ConfigureInfo
        {
            get { return mConfigureInfo; }
            set { mConfigureInfo = value; }
        }
        #endregion

        #region Costom
        /// <summary>
        ///
        /// </summary>
        private Decimal mCostom;
        public Decimal Costom
        {
            get { return mCostom; }
            set { mCostom = value; }
        }
        #endregion

        #region CostomName
        /// <summary>
        ///
        /// </summary>
        private String mCostomName = string.Empty;
        public String CostomName
        {
            get { return mCostomName; }
            set { mCostomName = value; }
        }
        #endregion

        #region ServiceBeginTime
        /// <summary>
        ///
        /// </summary>
        private DateTime mServiceBeginTime = DateTime.MinValue;
        public DateTime ServiceBeginTime
        {
            get { return mServiceBeginTime; }
            set { mServiceBeginTime = value; }
        }
        #endregion

        #region ServiceEndTime
        /// <summary>
        ///
        /// </summary>
        private DateTime mServiceEndTime = DateTime.MinValue;
        public DateTime ServiceEndTime
        {
            get { return mServiceEndTime; }
            set { mServiceEndTime = value; }
        }
        #endregion

        #region ItemCode
        /// <summary>
        ///
        /// </summary>
        private String mItemCode = string.Empty;
        public String ItemCode
        {
            get { return mItemCode; }
            set { mItemCode = value; }
        }
        #endregion

        #region Version
        /// <summary>
        ///
        /// </summary>
        private decimal mVersion = 0;
        public decimal Version
        {
            get { return mVersion; }
            set { mVersion = value; }
        }
        #endregion

        #region FlowID
        /// <summary>
        ///
        /// </summary>
        private decimal mFlowID = 0;
        public decimal FlowID
        {
            get { return mFlowID; }
            set { mFlowID = value; }
        }
        #endregion

        #region UpdateTime
        /// <summary>
        ///
        /// </summary>
        private DateTime mUpdateTime = DateTime.MinValue;
        public DateTime UpdateTime
        {
            get { return mUpdateTime; }
            set { mUpdateTime = value; }
        }
        #endregion

        #region ListID
        private decimal mListID = 0;
        /// <summary>
        /// �ʲ�Ŀ¼ID
        /// </summary>
        public decimal ListID
        {
            get { return mListID; }
            set { mListID = value; }
        }
        #endregion

        #region ListName
        private String mListName = string.Empty;
        /// <summary>
        /// �ʲ�Ŀ¼����
        /// </summary>
        public String ListName
        {
            get { return mListName; }
            set { mListName = value; }
        }
        #endregion

        #region EquId
        /// <summary>
        /// 
        /// </summary>
        private decimal mEquId = 0;
        public decimal EquId
        {
            get { return mEquId; }
            set { mEquId = value; }
        }
        #endregion

        #region Changedate
        /// <summary>
        /// ���ʱ��
        /// </summary>
        private string sChangedate = System.DateTime.Now.ToString();
        public string Changedate
        {
            get { return sChangedate; }
            set { sChangedate = value; }
        }
        #endregion

        #region ChangeBy
        /// <summary>
        /// ChangeBy �����
        /// </summary>
        /// 
        private string sChangeBy = string.Empty;
        public string ChangeBy
        {
            get { return sChangeBy; }
            set { sChangeBy = value; }
        }

        #endregion

        #region HistorySchema

        private String mHistorySchema = string.Empty;
        /// <summary>
        /// ��ʷ�汾��������� ����SCHEMA,��ʷ�汾չʾʱ�õ�
        /// </summary>
        public String HistorySchema
        {
            get { return mHistorySchema; }
            set { mHistorySchema = value; }
        }
        #endregion

        #region List<EQU_deploy>

        private List<EQU_deploy> mEquDeploy = new List<EQU_deploy>();
        /// <summary>
        /// �ʲ���չ��
        /// </summary>
        public List<EQU_deploy> EquDeploy
        {
            get { return mEquDeploy; }
            set { mEquDeploy = value; }
        }

        #region
        /// <summary>
        ///  ����XML��
        /// </summary>
        private String mstrAttach = "<Attachments />";
        public String AttachXml
        {
            get { return mstrAttach; }
            set { mstrAttach = value; }
        }
        #endregion

        #endregion

        #region Mastcustid
        /// <summary>
        /// ����λ
        /// </summary>
        private Decimal mMastcustid;
        public Decimal Mastcustid
        {
            get { return mMastcustid; }
            set { mMastcustid = value; }
        }
        #endregion

        #endregion

        #region GetReCorded
        /// <summary>
        /// ��ȡ�ʲ���Ϣ
        /// </summary>
        /// <param name="lngID"></param>
        /// <returns>Equ_DeskDP</returns>
        public Equ_DeskDP GetReCorded(long lngID)
        {
            Equ_DeskDP ee = new Equ_DeskDP();//�ʲ�����
            string strSQL = string.Empty;//SQL���
            OracleConnection cn = ConfigTool.GetConnection();//��ȡ����
            try
            {
                strSQL = "SELECT * FROM Equ_Desk WHERE ID = " + lngID.ToString();//��ѯ���
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);//ִ�в�ѯ

                #region ��ֵ�ʲ���Ϣ

                foreach (DataRow dr in dt.Rows)
                {
                    ee.ID = Decimal.Parse(dr["ID"].ToString());
                    ee.Name = dr["Name"].ToString();
                    ee.Code = dr["Code"].ToString();
                    ee.Positions = dr["Positions"].ToString();
                    ee.SerialNumber = dr["SerialNumber"].ToString();
                    ee.Provide = Decimal.Parse(dr["Provide"].ToString());
                    ee.ProvideName = dr["ProvideName"].ToString();
                    ee.Breed = dr["Breed"].ToString();
                    ee.Model = dr["Model"].ToString();
                    ee.CatalogID = Decimal.Parse(dr["CatalogID"].ToString());
                    ee.CatalogName = dr["CatalogName"].ToString();
                    ee.EquStatusID = (dr["EquStatusID"].ToString() == string.Empty ? 0 : Decimal.Parse(dr["EquStatusID"].ToString()));
                    ee.EquStatusName = dr["EquStatusName"].ToString();
                    ee.FullID = dr["FullID"].ToString();
                    ee.Deleted = Decimal.Parse(dr["Deleted"].ToString());
                    ee.UpdateUserID = Decimal.Parse(dr["UpdateUserID"].ToString());
                    ee.RegUserID = Decimal.Parse(dr["RegUserID"].ToString());
                    ee.RegUserName = dr["RegUserName"].ToString();
                    ee.RegDeptID = Decimal.Parse(dr["RegDeptID"].ToString());
                    ee.RegDeptName = dr["RegDeptName"].ToString();
                    ee.RegTime = dr["RegTime"].ToString() == string.Empty ? DateTime.MinValue : DateTime.Parse(dr["RegTime"].ToString());
                    ee.partBankId = decimal.Parse(dr["partBankId"].ToString() == "" ? "0" : dr["partBankId"].ToString());
                    ee.partBranchId = decimal.Parse(dr["partBranchId"].ToString() == "" ? "0" : dr["partBranchId"].ToString());
                    ee.partBankName = dr["partBankName"].ToString();
                    ee.partBranchName = dr["partBranchName"].ToString();
                    ee.ConfigureInfo = dr["ConfigureInfo"].ToString();
                    ee.ConfigureValue = dr["ConfigureValue"].ToString();
                    ee.Costom = Decimal.Parse(dr["Costom"].ToString());
                    ee.CostomName = dr["CostomName"].ToString();
                    ee.ServiceBeginTime = dr["ServiceBeginTime"].ToString() == string.Empty ? DateTime.MinValue : DateTime.Parse(dr["ServiceBeginTime"].ToString());
                    ee.ServiceEndTime = dr["ServiceEndTime"].ToString() == string.Empty ? DateTime.MinValue : DateTime.Parse(dr["ServiceEndTime"].ToString());
                    ee.ItemCode = dr["ItemCode"].ToString();
                    ee.Version = dr["Version"].ToString() == string.Empty ? 0 : Decimal.Parse(dr["Version"].ToString());
                    ee.UpdateTime = dr["UpdateTime"].ToString() == string.Empty ? DateTime.MinValue : DateTime.Parse(dr["UpdateTime"].ToString());
                    ee.ListID = Decimal.Parse(dr["ListID"].ToString() == "" ? "0" : dr["ListID"].ToString()); //�ʲ�����
                    ee.ListName = dr["ListName"].ToString();
                    ee.EquDeploy = EQU_deploy.getEQU_deployList(long.Parse(ee.ID.ToString()));
                    ee.Mastcustid = (dr["Mastcustid"].ToString() == string.Empty ? 0 : Decimal.Parse(dr["Mastcustid"].ToString()));
                }
                #endregion

            }
            catch
            {
                throw;
            }
            finally
            {
                ConfigTool.CloseConnection(cn);//�ر�����
            }
            return ee;
        }
        #endregion

        #region GetReCorded
        /// <summary>
        /// ��ȡ�ʲ���Ϣ
        /// </summary>
        /// <param name="lngID"></param>
        /// <returns>Equ_DeskDP</returns>
        public Equ_DeskDP GetReCorded(OracleTransaction trans, long lngID)
        {
            Equ_DeskDP ee = new Equ_DeskDP();//�ʲ�����
            string strSQL = string.Empty;//SQL���
            try
            {
                strSQL = "SELECT * FROM Equ_Desk WHERE ID = " + lngID.ToString();//��ѯ���
                DataTable dt = OracleDbHelper.ExecuteDataset(trans, CommandType.Text, strSQL).Tables[0];

                #region ��ֵ�ʲ���Ϣ

                foreach (DataRow dr in dt.Rows)
                {
                    ee.ID = Decimal.Parse(dr["ID"].ToString());
                    ee.Name = dr["Name"].ToString();
                    ee.Code = dr["Code"].ToString();
                    ee.Positions = dr["Positions"].ToString();
                    ee.SerialNumber = dr["SerialNumber"].ToString();
                    ee.Provide = Decimal.Parse(dr["Provide"].ToString());
                    ee.ProvideName = dr["ProvideName"].ToString();
                    ee.Breed = dr["Breed"].ToString();
                    ee.Model = dr["Model"].ToString();
                    ee.CatalogID = Decimal.Parse(dr["CatalogID"].ToString());
                    ee.CatalogName = dr["CatalogName"].ToString();
                    ee.EquStatusID = (dr["EquStatusID"].ToString() == string.Empty ? 0 : Decimal.Parse(dr["EquStatusID"].ToString()));
                    ee.EquStatusName = dr["EquStatusName"].ToString();
                    ee.FullID = dr["FullID"].ToString();
                    ee.Deleted = Decimal.Parse(dr["Deleted"].ToString());
                    ee.UpdateUserID = Decimal.Parse(dr["UpdateUserID"].ToString());
                    ee.RegUserID = Decimal.Parse(dr["RegUserID"].ToString());
                    ee.RegUserName = dr["RegUserName"].ToString();
                    ee.RegDeptID = Decimal.Parse(dr["RegDeptID"].ToString());
                    ee.RegDeptName = dr["RegDeptName"].ToString();
                    ee.RegTime = dr["RegTime"].ToString() == string.Empty ? DateTime.MinValue : DateTime.Parse(dr["RegTime"].ToString());
                    ee.partBankId = decimal.Parse(dr["partBankId"].ToString() == "" ? "0" : dr["partBankId"].ToString());
                    ee.partBranchId = decimal.Parse(dr["partBranchId"].ToString() == "" ? "0" : dr["partBranchId"].ToString());
                    ee.partBankName = dr["partBankName"].ToString();
                    ee.partBranchName = dr["partBranchName"].ToString();
                    ee.ConfigureInfo = dr["ConfigureInfo"].ToString();
                    ee.ConfigureValue = dr["ConfigureValue"].ToString();
                    ee.Costom = Decimal.Parse(dr["Costom"].ToString());
                    ee.CostomName = dr["CostomName"].ToString();
                    ee.ServiceBeginTime = dr["ServiceBeginTime"].ToString() == string.Empty ? DateTime.MinValue : DateTime.Parse(dr["ServiceBeginTime"].ToString());
                    ee.ServiceEndTime = dr["ServiceEndTime"].ToString() == string.Empty ? DateTime.MinValue : DateTime.Parse(dr["ServiceEndTime"].ToString());
                    ee.ItemCode = dr["ItemCode"].ToString();
                    ee.Version = dr["Version"].ToString() == string.Empty ? 0 : Decimal.Parse(dr["Version"].ToString());
                    ee.UpdateTime = dr["UpdateTime"].ToString() == string.Empty ? DateTime.MinValue : DateTime.Parse(dr["UpdateTime"].ToString());
                    ee.Mastcustid = (dr["Mastcustid"].ToString() == string.Empty ? 0 : Decimal.Parse(dr["Mastcustid"].ToString()));//������λ
                    ee.ListID = Decimal.Parse(dr["ListID"].ToString() == "" ? "0" : dr["ListID"].ToString()); //�ʲ�����
                    ee.ListName = dr["ListName"].ToString();
                    ee.EquDeploy = EQU_deploy.getEQU_deployList(trans, long.Parse(ee.ID.ToString()));
                }
                #endregion

            }
            catch
            {
                throw;
            }
            finally
            {
            }
            return ee;
        }
        #endregion

        #region GetDataTable
        /// <summary>
        /// ����������ȡ����
        /// </summary>
        /// <param name="sWhere"></param>
        /// <param name="sOrder"></param>
        /// <returns>DataTable</returns>
        public DataTable GetDataTable(string sWhere, string sOrder)
        {
            string strSQL = string.Empty;
            OracleConnection cn = ConfigTool.GetConnection();
            strSQL = "SELECT * FROM Equ_Desk Where 1=1 And Deleted=0 ";
            strSQL += sWhere;
            if (CommonDP.GetConfigValue("Other", "DataLimit") != null && CommonDP.GetConfigValue("Other", "DataLimit") == "1")  //�Ƿ�����
            {
                if (System.Web.HttpContext.Current.Session["MastLimitList"] != null)
                {
                    strSQL = strSQL + " and Costom<>0 and Exists(select ID from Br_ECustomer b where b.ID=Costom and MastCustID in (" + System.Web.HttpContext.Current.Session["MastLimitList"].ToString() + "))";
                }
            }
            strSQL += sOrder;
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigTool.CloseConnection(cn);
            return dt;
        }

        #region GetDataTableAjax
        /// <summary>
        /// GetDataTableAjax
        /// </summary>
        /// <param name="sWhere"></param>
        /// <param name="sOrder"></param>
        /// <returns>DataTable</returns>
        public DataTable GetDataTableAjax(string sWhere, string sOrder)
        {
            string strSQL = string.Empty;
            OracleConnection cn = ConfigTool.GetConnection();
            strSQL = "SELECT * FROM V_Equ_Desk Where 1=1 And Deleted=0 ";
            strSQL += sWhere;
            strSQL += sOrder;
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigTool.CloseConnection(cn);
            return dt;
        }
        #endregion

        /// <summary>
        /// ��������λ
        /// </summary>
        /// <param name="sWhere"></param>
        /// <param name="sOrder"></param>
        /// <param name="pagesize"></param>
        ///  <param name="pageindex"></param>
        ///  <param name="rowcount"></param>
        /// <returns>DataTable</returns>
        public DataTable GetDataTableCust(string sWhere, string sOrder, int pagesize, int pageindex, ref int rowcount)
        {
            string strSQLWhere = string.Empty;
            OracleConnection cn = ConfigTool.GetConnection();
            strSQLWhere = @" Equ_Desk.Deleted=0 and Br_ECustomer.Deleted=0";
            strSQLWhere += sWhere;
            if (CommonDP.GetConfigValue("Other", "DataLimit") != null && CommonDP.GetConfigValue("Other", "DataLimit") == "1")  //�Ƿ�����
            {
                if (System.Web.HttpContext.Current.Session["MastLimitList"] != null)
                {
                    strSQLWhere = strSQLWhere + " and Equ_Desk.Costom<>0 and Exists(select ID from Br_ECustomer b where b.ID=Costom and MastCustID in (" + System.Web.HttpContext.Current.Session["MastLimitList"].ToString() + "))";
                }
            }

            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, "Equ_Desk Left outer join Br_ECustomer ON nvl(Equ_Desk.Costom,0)=Br_ECustomer.ID", "Equ_Desk.*,nvl(Br_ECustomer.MastCustName,'') as CustMastName ", sOrder, pagesize, pageindex, strSQLWhere, ref rowcount);
            ConfigTool.CloseConnection(cn);
            return dt;
        }

        #region �ʲ���ϸ�б�����excel�����ݷ���
        /// <summary>
        /// �ʲ���ϸ�б�����excel�����ݷ���
        /// </summary>
        /// <param name="sWhere"></param>
        /// <param name="sOrder"></param>
        /// <param name="pagesize"></param>
        ///  <param name="pageindex"></param>
        ///  <param name="rowcount"></param>
        /// <returns>DataTable</returns>
        public DataTable GetDataTableEquDetails(string sWhere, string sOrder, int pagesize, int pageindex, ref int rowcount)
        {
            string strSQLWhere = string.Empty;
            OracleConnection cn = ConfigTool.GetConnection();
            strSQLWhere = @" A.Deleted=0 and Br_ECustomer.Deleted=0";
            strSQLWhere += sWhere;
            if (CommonDP.GetConfigValue("Other", "DataLimit") != null && CommonDP.GetConfigValue("Other", "DataLimit") == "1")  //�Ƿ�����
            {
                if (System.Web.HttpContext.Current.Session["MastLimitList"] != null)
                {
                    strSQLWhere = strSQLWhere + " and A.Costom<>0 and Exists(select ID from Br_ECustomer b where b.ID=Costom and MastCustID in (" + System.Web.HttpContext.Current.Session["MastLimitList"].ToString() + "))";
                }
            }

            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, "Equ_Desk A Left outer join Br_ECustomer ON nvl(A.Costom,0)=Br_ECustomer.ID",
                            @"A.ID,A.CostomName,CatalogName,A.ListName,A.Name,A.Code,A.partBankName,A.partBranchName,
                           to_char(A.ServiceBeginTime,'yyyy-MM-dd') as ServiceBeginTimeA,to_char(A.ServiceEndTime,'yyyy-MM-dd') as ServiceEndTimeA,
                           EquStatusName,nvl(Br_ECustomer.MastCustName,'') as CustMastName", sOrder, pagesize, pageindex, strSQLWhere, ref rowcount);
            ConfigTool.CloseConnection(cn);
            return dt;
        }
        #endregion

        /// <summary>
        /// �ʲ�����Ԥ��
        /// </summary>
        /// <param name="sWhere"></param>
        /// <param name="sOrder"></param>
        /// <param name="iOverDay"></param>
        /// <returns></returns>
        public DataTable GetEquOverTime(string sWhere, string sOrder, int iOverDay, int pagesize, int pageindex, ref int rowcount)
        {
            string strSQLWhere = string.Empty;
            OracleConnection cn = ConfigTool.GetConnection();
            strSQLWhere = @"Equ_Desk.Deleted=0";
            strSQLWhere += sWhere;
            if (CommonDP.GetConfigValue("Other", "DataLimit") != null && CommonDP.GetConfigValue("Other", "DataLimit") == "1")  //�Ƿ�����
            {
                if (System.Web.HttpContext.Current.Session["MastLimitList"] != null)
                {
                    strSQLWhere = strSQLWhere + " and Equ_Desk.Costom<>0 and Exists(select ID from Br_ECustomer b where b.ID=Costom and MastCustID in (" + System.Web.HttpContext.Current.Session["MastLimitList"].ToString() + "))";
                }
            }
            //@" Equ_Desk.*,nvl(Br_ECustomer.MastCustName,'') as CustMastName,
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, " Equ_Desk  Left outer join Br_ECustomer ON nvl(Equ_Desk.Costom,0)=Br_ECustomer.ID",
                @" Equ_Desk.*,nvl(Br_ECustomer.MastCustName,'') as CustMastName,
                    to_date(to_char( sysdate,'yyyy/mm/dd'),'yyyy/mm/dd') - to_date(to_char( ServiceEndTime,'yyyy/mm/dd'),'yyyy/mm/dd')  AS OverDays  ",
                sOrder, pagesize, pageindex, strSQLWhere, ref rowcount);
            ConfigTool.CloseConnection(cn);
            return dt;
        }


        /// <summary>
        /// ����������ȡ����
        /// </summary>
        /// <param name="sWhere"></param>
        /// <param name="sOrder"></param>
        /// <param name="ht"></param>
        /// <param name="pagesize"></param>
        /// <param name="pageindex"></param>
        /// <param name="rowcount"></param>
        /// <returns></returns>
        public DataTable GetDataTable(string sWhere, string sOrder, Hashtable ht, int pagesize, int pageindex, ref int rowcount)
        {
            string sSchemaKey = string.Empty;
            string sSchemaValue = string.Empty;
            string sXml = string.Empty;
            OracleConnection cn = ConfigTool.GetConnection();

            if (ht.Count > 0)
            {
                IDictionaryEnumerator myEnum = ht.GetEnumerator();
                while (myEnum.MoveNext())
                {
                    sSchemaKey = myEnum.Key.ToString();
                    sSchemaValue = myEnum.Value.ToString();

                    string strItemType = GetItemTypeByFieldID(sSchemaKey);

                    if (strItemType == "6")
                    {
                        //���Ϊ�������ͣ����ַ���ת����������
                        sWhere += " and ID in (select EquID from EQU_deploy where FieldID = " + StringTool.SqlQ(sSchemaKey) + " and Value = TO_DATE(" + StringTool.SqlQ(sSchemaValue) + ",'yyyy-mm-dd   hh24:mi:ss '))";
                    }
                    else if (strItemType == "7" || strItemType == "8" || strItemType == "9")
                    {
                        //���������š��û�
                        sWhere += " and ID in (select EquID from EQU_deploy where FieldID = " + StringTool.SqlQ(sSchemaKey) + " and Value = " + StringTool.SqlQ(sSchemaValue) + ")";
                    }
                    else
                    {
                        //����
                        sWhere += " and ID in (select EquID from EQU_deploy where FieldID = " + StringTool.SqlQ(sSchemaKey) + " and Value like " + StringTool.SqlQ("%" + sSchemaValue + "%") + ")";
                    }
                }

            }

            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, "Equ_Desk", "*", sOrder, pagesize, pageindex, sWhere, ref rowcount);
            ConfigTool.CloseConnection(cn);

            return dt;
        }

        #endregion

        #region �����ͻ���Ϣ����µĲ�ѯ
        /// <summary>
        /// �����ͻ���Ϣ����µĲ�ѯ
        /// </summary>
        /// <param name="sWhere"></param>
        /// <param name="sOrder"></param>
        /// <param name="ht"></param>
        /// <param name="pagesize"></param>
        /// <param name="pageindex"></param>
        /// <param name="rowcount"></param>
        /// <returns></returns>
        public DataTable GetDataTableSpec(string sWhere, string sOrder, Hashtable ht, int pagesize, int pageindex, ref int rowcount)
        {
            string sSchemaKey = string.Empty;
            string sSchemaValue = string.Empty;
            string sXml = string.Empty;

            if (CommonDP.GetConfigValue("Other", "DataLimit") != null && CommonDP.GetConfigValue("Other", "DataLimit") == "1")  //�Ƿ�����
            {
                if (System.Web.HttpContext.Current.Session["MastLimitList"] != null)
                {
                    sWhere = sWhere + " and Costom<>0 and Exists(select ID from Br_ECustomer b where b.ID=Costom and MastCustID in (" + System.Web.HttpContext.Current.Session["MastLimitList"].ToString() + "))";
                }
            }
            if (ht.Count > 0)
            {
                IDictionaryEnumerator myEnum = ht.GetEnumerator();
                while (myEnum.MoveNext())
                {
                    sSchemaKey = myEnum.Key.ToString();
                    sSchemaValue = myEnum.Value.ToString();

                    string strItemType = GetItemTypeByFieldID(sSchemaKey);

                    if (strItemType == "6")
                    {
                        //���Ϊ�������ͣ����ַ���ת����������
                        sWhere += " and ID in (select EquID from EQU_deploy where FieldID = " + StringTool.SqlQ(sSchemaKey) + " and Value = TO_DATE(" + StringTool.SqlQ(sSchemaValue) + ",'yyyy-mm-dd   hh24:mi:ss '))";
                    }
                    else if (strItemType == "7" || strItemType == "8" || strItemType == "9")
                    {
                        //���������š��û�
                        sWhere += " and ID in (select EquID from EQU_deploy where FieldID = " + StringTool.SqlQ(sSchemaKey) + " and Value = " + StringTool.SqlQ(sSchemaValue) + ")";
                    }
                    else
                    {
                        //����
                        sWhere += " and ID in (select EquID from EQU_deploy where FieldID = " + StringTool.SqlQ(sSchemaKey) + " and Value like " + StringTool.SqlQ("%" + sSchemaValue + "%") + ")";
                    }
                }

            }

            OracleConnection cn = ConfigTool.GetConnection();
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, "V_Equ_Desk", "*", sOrder, pagesize, pageindex, sWhere, ref rowcount);
            ConfigTool.CloseConnection(cn);
            return dt;
        }
        #endregion

        #region ����������ID��ȡ����������
        /// <summary>
        /// ����������ID��ȡ����������
        /// </summary>
        /// <param name="strFieldID">Equ_SchemaItems���FieldID�ֶ�</param>
        /// <returns></returns>
        private string GetItemTypeByFieldID(string strFieldID)
        {
            string strRet = string.Empty;
            string strSql = "select * from Equ_SchemaItems where FieldID = " + strFieldID;

            DataTable dt = CommonDP.ExcuteSqlTable(strSql);
            if (dt != null && dt.Rows.Count > 0)
            {
                strRet = dt.Rows[0]["ItemType"].ToString();
            }
            return strRet;
        }
        #endregion

        #region ��ȡ��ͬ���õ��豸�嵥
        /// <summary>
        /// ��ȡ��ͬ���õ��豸�嵥
        /// </summary>
        /// <param name="sID"></param>
        /// <param name="sValue"></param>
        /// <returns></returns>
        public DataTable GetSameSchemasEqus(string sID, string sValue)
        {
            string strSql = @"select A.*,nvl(B.Value,'') as ""Default"",nvl(c.ShortName,'') MastCustName from Equ_Desk A 
                              inner join
	                            (
	                            select * from EQU_deploy
	                              where EquID in 
		                            (select EquID from EQU_deploy where FieldID = " + sID + @" and Value = " + StringTool.SqlQ(sValue) + @")
                                    and FieldID = " + sID + @" and Value = " + StringTool.SqlQ(sValue) + @"
	                            ) B 
                               on A.ID = B.EquID
                               left  JOIN Br_MastCustomer c ON c.ID=A.MastCustId 
                               where A.Deleted=0 order by A.ID Desc ";

            DataTable dt = CommonDP.ExcuteSqlTable(strSql);

            return dt;
        }
        #endregion

        #region GetSameSchemaItems
        /// <summary>
        /// ��ȡ��ͬ���õ��豸�嵥
        /// </summary>
        /// <param name="sID"></param>
        /// <param name="sValue"></param>
        /// <param name="blnExact">�Ƿ�ȷƥ��</param>
        /// <returns></returns>
        public DataTable GetSameSchemaItems(string sID, string sValue, bool blnExact)
        {
            // SQL 2005  ����ֱ��ͨ�� SQL ������ XML  ���ּ����ԣ�������ʵ�� 
            string strSQL = string.Empty;
            string sXml = string.Empty;
            OracleConnection cn = ConfigTool.GetConnection();
            strSQL = "SELECT *," + StringTool.SqlQ(sValue) + " as [Default] FROM Equ_Desk Where 1=1 And Deleted=0 ";
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                sXml = dt.Rows[i]["ConfigureValue"].ToString();
                if (sXml == "")
                {
                    dt.Rows[i].Delete();
                }
                else
                {
                    if (CheckConfigureXmlValue(sXml, sID, sValue, blnExact) == false)
                    {
                        dt.Rows[i].Delete();
                    }
                }
            }
            dt.AcceptChanges();

            ConfigTool.CloseConnection(cn);
            return dt;
        }


        /// <summary>
        /// ����������ϰ������洢
        /// </summary>
        /// <param name="sXml"></param>
        /// <param name="lngUserID"></param>
        public void UpdateIdiomForSchemaValues(string sXml, long lngUserID)
        {
            string sFieldID = "";
            string sFieldValue = "";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(sXml);

            XmlNodeList nodes = xmlDoc.SelectNodes("Fields/Field");



            foreach (XmlNode n in nodes)
            {

                sFieldValue = n.Attributes["Value"].Value.Trim();
                if (sFieldValue.Length > 0 && sFieldValue != "0" && sFieldValue != "1")
                {
                    sFieldID = "SchemaItem_" + n.Attributes["FieldName"].Value.Trim();
                    IdiomDP.AddIdiom(lngUserID, sFieldID, sFieldValue);
                }
            }

        }

        /// <summary>
        /// �������XMLֵ��ĳ�ֶε�ֵ�Ƿ� == ָ��ֵ
        /// </summary>
        /// <param name="sXml"></param>
        /// <param name="sID"></param>
        /// <param name="sValue"></param>
        /// <param name="blnExact">�Ƿ�ȷƥ��</param>
        /// <returns></returns>
        public bool CheckConfigureXmlValue(string sXml, string sID, string sValue, bool blnExact)
        {
            bool ret = false;
            string strTemp = string.Empty;

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(sXml);
            if (blnExact == true)
            {
                XmlNode node = xmlDoc.SelectSingleNode("Fields/Field[@FieldName=" + StringTool.SqlQ(sID.Trim()) + " and @Value=" + StringTool.SqlQ(sValue.Trim()) + "]");
                if (node != null)
                    ret = true;
            }
            else
            {
                XmlNode node = xmlDoc.SelectSingleNode("Fields/Field[@FieldName=" + StringTool.SqlQ(sID.Trim()) + "]");
                if (node != null)
                {
                    strTemp = node.Attributes["Value"].Value.ToLower();
                    if (sValue != "")
                    {
                        ret = (strTemp.IndexOf(sValue.ToLower()) >= 0);
                    }
                    else
                    {
                        ret = (strTemp.Trim() == sValue ? true : false);
                    }
                }
                else
                {
                    ret = false;
                }
            }
            return ret;
        }

        public bool CheckConfigureXmlValue(string sXml, string sID, string sValue)
        {
            return CheckConfigureXmlValue(sXml, sID, sValue, true);
        }

        #endregion

        #region �жϹ�Ӧ���Ƿ����ʲ����õ�

        /// <summary>
        /// �жϹ�Ӧ���Ƿ����ʲ����õ�
        /// </summary>
        /// <param name="ProvidId"></param>
        /// <returns></returns>
        public static bool getProvideName(string ProvidId)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, " select * From Equ_Desk where provide =" + ProvidId + "  and deleted ='0';");
            ConfigTool.CloseConnection(cn);
            if (dt.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region �ж��ʲ������Ƿ����

        /// <summary>
        /// �ж��ʲ������Ƿ����
        /// </summary>
        /// <param name="deskName"></param>
        /// <returns></returns>
        public static bool getEquDeskName(string deskName)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, " select * From Equ_Desk where name =" + StringTool.SqlQ(deskName) + " and deleted ='0';");
            ConfigTool.CloseConnection(cn);
            if (dt.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion


        /// <summary>
        /// �ж��ʲ�����Ƿ��ظ�
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public bool CodeIsTow(string code)
        {
            string strSQL = "select 1 from Equ_Desk where code =" + StringTool.SqlQ(code) + " and deleted=0";
            DataTable dt = CommonDP.ExcuteSqlTable(strSQL);
            if (dt.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        /// <summary>
        /// �ж��ʲ�����Ƿ��ظ�
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static bool CodeIsTow(string code, long EquID)
        {
            string strSQL = "select 1 from Equ_Desk where code =" + StringTool.SqlQ(code) + " and deleted=0 and ID !=" + EquID;
            DataTable dt = CommonDP.ExcuteSqlTable(strSQL);
            if (dt.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        #region InsertRecorded
        /// <summary>
        /// �������
        /// </summary>
        /// <param name=pEqu_DeskDP></param>
        public void InsertRecorded(Equ_DeskDP pEqu_DeskDP, List<EQU_deploy> list)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            cn.Open();
            OracleTransaction trans = cn.BeginTransaction();
            string strSQL = string.Empty;
            string strID = "0";
            try
            {
                // ����ʱ��β���Ϊͬһ��, ���ʲ�����ʱ���б���ʱ���Ϊͬһ�죬��Ĭ�ϸ����޽���ʱ��
                // ����һ��.
                // ������ - 2014 - 03 - 17
                if (pEqu_DeskDP.ServiceBeginTime != null && pEqu_DeskDP.ServiceEndTime != null)
                {
                    if (pEqu_DeskDP.ServiceBeginTime.Date.Equals(pEqu_DeskDP.ServiceEndTime.Date))
                    {
                        pEqu_DeskDP.ServiceEndTime.AddDays(1);
                    }
                }

                strID = EpowerGlobal.EPGlobal.GetNextID("Equ_DeskID").ToString();
                pEqu_DeskDP.ID = decimal.Parse(strID);
                strSQL = @"INSERT INTO Equ_Desk(
									ID,
									Name,
									Code,
									Positions,
									SerialNumber,
                                    Breed,
                                    Model,
									Provide,
									ProvideName,
									EquStatusID,
									EquStatusName,
                                    CatalogID,
									CatalogName,
									FullID,
									Deleted,
                                    UpdateUserID,
									RegUserID,
									RegUserName,
									RegDeptID,
									RegDeptName,
									RegTime,
                                    partBankId,
                                    partBankName,
                                    partBranchId,
                                    partBranchName,
									ConfigureInfo,
                                    ConfigureValue,
									Costom,
									CostomName,
									ServiceBeginTime,
									ServiceEndTime,
                                    Version,
									ItemCode,
                                    UpdateTime,
                                    ListID,
                                    ListName,
                                    Mastcustid
					)
					VALUES( " +
                            strID.ToString() + "," +
                            StringTool.SqlQ(pEqu_DeskDP.Name) + "," +
                            StringTool.SqlQ(pEqu_DeskDP.Code) + "," +
                            StringTool.SqlQ(pEqu_DeskDP.Positions) + "," +
                            StringTool.SqlQ(pEqu_DeskDP.SerialNumber) + "," +
                            StringTool.SqlQ(pEqu_DeskDP.Breed) + "," +
                            StringTool.SqlQ(pEqu_DeskDP.Model) + "," +
                            pEqu_DeskDP.Provide.ToString() + "," +
                            StringTool.SqlQ(pEqu_DeskDP.ProvideName) + "," +
                            pEqu_DeskDP.EquStatusID.ToString() + "," +
                            StringTool.SqlQ(pEqu_DeskDP.EquStatusName) + "," +
                            pEqu_DeskDP.CatalogID.ToString() + "," +
                            StringTool.SqlQ(pEqu_DeskDP.CatalogName) + "," +
                            StringTool.SqlQ(pEqu_DeskDP.FullID) + "," +
                            pEqu_DeskDP.Deleted.ToString() + "," +
                            pEqu_DeskDP.RegUserID.ToString() + "," +
                            pEqu_DeskDP.RegUserID.ToString() + "," +
                            StringTool.SqlQ(pEqu_DeskDP.RegUserName) + "," +
                            pEqu_DeskDP.RegDeptID.ToString() + "," +
                            StringTool.SqlQ(pEqu_DeskDP.RegDeptName) + "," +
                            (pEqu_DeskDP.RegTime == DateTime.MinValue ? " null " : "to_date(" + StringTool.SqlQ(pEqu_DeskDP.RegTime.ToString()) + ",'yyyy-mm-dd   hh24:mi:ss')") + "," +
                            pEqu_DeskDP.partBankId.ToString() + "," +
                            StringTool.SqlQ(pEqu_DeskDP.partBankName.ToString()) + "," +
                            pEqu_DeskDP.partBranchId.ToString() + "," +
                            StringTool.SqlQ(pEqu_DeskDP.partBranchName.ToString()) + "," +
                            StringTool.SqlQ(pEqu_DeskDP.ConfigureInfo) + "," +
                            StringTool.SqlQ(pEqu_DeskDP.ConfigureValue) + "," +
                            pEqu_DeskDP.Costom.ToString() + "," +
                            StringTool.SqlQ(pEqu_DeskDP.CostomName) + "," +
                            (pEqu_DeskDP.ServiceBeginTime == DateTime.MinValue ? " null " : "to_date(" + StringTool.SqlQ(pEqu_DeskDP.ServiceBeginTime.ToString()) + ",'yyyy-mm-dd   hh24:mi:ss')") + "," +
                            (pEqu_DeskDP.ServiceEndTime == DateTime.MinValue ? " null " : "to_date(" + StringTool.SqlQ(pEqu_DeskDP.ServiceEndTime.ToString()) + ",'yyyy-mm-dd   hh24:mi:ss')") + "," +
                            "0" + "," +
                            StringTool.SqlQ(pEqu_DeskDP.ItemCode) + "," +
                            "sysdate" + "," +
                            pEqu_DeskDP.ListID.ToString() + "," +
                            StringTool.SqlQ(pEqu_DeskDP.ListName) + "," +
                            (pEqu_DeskDP.Mastcustid.ToString() == "" ? "0" : pEqu_DeskDP.Mastcustid.ToString()) +
                    ")";

                OracleDbHelper.ExecuteNonQuery(trans, CommandType.Text, strSQL);

                //����������Ϣ
                foreach (EQU_deploy deploy in list)
                {
                    deploy.EquID = long.Parse(pEqu_DeskDP.ID.ToString());
                    deploy.save(trans, deploy);
                }

                //���渽��
                SaveAttachments(trans, pEqu_DeskDP.ID, pEqu_DeskDP.AttachXml);

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
        /// ��������
        /// </summary>
        /// <param name="pEqu_DeskDP"></param>
        /// <param name="lngUserID"></param>
        public void UpdateRecorded(Equ_DeskDP pEqu_DeskDP, long lngUserID, List<EQU_deploy> list)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            cn.Open();
            OracleTransaction tran = cn.BeginTransaction();
            string strSQL = string.Empty;
            try
            {
                //����ǰ�û�ֱ�ӱ༭�����������ʷ�汾,�򲻲����µ���ʷ�汾

                Equ_DeskDP eLast = new Equ_DeskDP();
                eLast = eLast.GetReCordedForLastHistory((long)pEqu_DeskDP.ID);

                if (eLast.ID == pEqu_DeskDP.ID && pEqu_DeskDP.UpdateUserID.ToString() == lngUserID.ToString() && eLast.FlowID == 0)
                {
                    //�������ʷ�汾
                    strSQL = @"UPDATE Equ_DeskHistory Set " +
                                     " VersionTime = sysdate " +
                                           " WHERE ID = " + eLast.ID.ToString() + " AND version =" + eLast.Version.ToString();
                    OracleDbHelper.ExecuteNonQuery(tran, CommandType.Text, strSQL);
                    List<EQU_deployHistory> list2 = EQU_deployHistory.getEQU_deployList((long)pEqu_DeskDP.ID, long.Parse(eLast.Version.ToString()));
                    //�����ʲ���ʷ������Ϣ
                    foreach (EQU_deployHistory deployHtory in list2)
                    {
                        deployHtory.versionTime = System.DateTime.Now;//�޸���ʷʱ��                    
                        deployHtory.saveUpdate(tran, deployHtory);
                    }

                    strSQL = @"UPDATE Equ_Desk Set " +
                                " Name = " + StringTool.SqlQ(pEqu_DeskDP.Name) + "," +
                                " Code = " + StringTool.SqlQ(pEqu_DeskDP.Code) + "," +
                                " Positions = " + StringTool.SqlQ(pEqu_DeskDP.Positions) + "," +
                                " SerialNumber = " + StringTool.SqlQ(pEqu_DeskDP.SerialNumber) + "," +
                                " Breed = " + StringTool.SqlQ(pEqu_DeskDP.Breed) + "," +
                                " Model = " + StringTool.SqlQ(pEqu_DeskDP.Model) + "," +
                                " Provide = " + pEqu_DeskDP.Provide.ToString() + "," +
                                " ProvideName = " + StringTool.SqlQ(pEqu_DeskDP.ProvideName) + "," +
                                 "EquStatusID = " + pEqu_DeskDP.EquStatusID.ToString() + "," +
                                " EquStatusName = " + StringTool.SqlQ(pEqu_DeskDP.EquStatusName) + "," +
                                " CatalogID = " + pEqu_DeskDP.CatalogID.ToString() + "," +
                                " CatalogName = " + StringTool.SqlQ(pEqu_DeskDP.CatalogName) + "," +
                                " FullID = " + StringTool.SqlQ(pEqu_DeskDP.FullID) + "," +
                                " Deleted = " + pEqu_DeskDP.Deleted.ToString() + "," +
                                " UpdateUserID = " + lngUserID.ToString() + "," +
                                " RegUserID = " + pEqu_DeskDP.RegUserID.ToString() + "," +
                                " RegUserName = " + StringTool.SqlQ(pEqu_DeskDP.RegUserName) + "," +
                                " RegDeptID = " + pEqu_DeskDP.RegDeptID.ToString() + "," +
                                " RegDeptName = " + StringTool.SqlQ(pEqu_DeskDP.RegDeptName) + "," +
                                " RegTime = " + (pEqu_DeskDP.RegTime == DateTime.MinValue ? " null " : "to_date(" + StringTool.SqlQ(pEqu_DeskDP.RegTime.ToString())) + ",'yyyy-MM-dd HH24:mi:ss')," +
                                " partBankId=" + pEqu_DeskDP.partBankId.ToString() + "," +
                                " partBankName=" + StringTool.SqlQ(pEqu_DeskDP.partBankName.ToString()) + "," +
                                " partBranchId=" + pEqu_DeskDP.partBranchId.ToString() + "," +
                                " partBranchName=" + StringTool.SqlQ(pEqu_DeskDP.partBranchName.ToString()) + "," +
                                " ConfigureInfo = " + StringTool.SqlQ(pEqu_DeskDP.ConfigureInfo) + "," +
                                " ConfigureValue = " + StringTool.SqlQ(pEqu_DeskDP.ConfigureValue) + "," +
                                " Costom = " + pEqu_DeskDP.Costom.ToString() + "," +
                                " CostomName = " + StringTool.SqlQ(pEqu_DeskDP.CostomName) + "," +
                                " ServiceBeginTime = " + (pEqu_DeskDP.ServiceBeginTime == DateTime.MinValue ? " null " : "to_date(" + StringTool.SqlQ(pEqu_DeskDP.ServiceBeginTime.ToString())) + ",'yyyy-MM-dd HH24:mi:ss')," +
                                " ServiceEndTime = " + (pEqu_DeskDP.ServiceEndTime == DateTime.MinValue ? " null " : "to_date(" + StringTool.SqlQ(pEqu_DeskDP.ServiceEndTime.ToString())) + ",'yyyy-MM-dd HH24:mi:ss')," +
                                " ItemCode = " + StringTool.SqlQ(pEqu_DeskDP.ItemCode) + "," +
                                " UpdateTime= sysdate," +
                                " ListID=" + pEqu_DeskDP.ListID.ToString() + "," +
                                " ListName=" + StringTool.SqlQ(pEqu_DeskDP.ListName) + "," +
                                " Mastcustid=" + (pEqu_DeskDP.Mastcustid.ToString() == "" ? "0" : pEqu_DeskDP.Mastcustid.ToString()) +
                                " WHERE ID = " + pEqu_DeskDP.ID.ToString();


                    foreach (EQU_deploy deploy in list)
                    {
                        deploy.EquID = long.Parse(pEqu_DeskDP.ID.ToString());
                        deploy.save(tran, deploy);
                    }

                    OracleDbHelper.ExecuteNonQuery(tran, CommandType.Text, strSQL);
                }
                else
                {
                    //������ʷ�汾
                    Equ_DeskDP eCurr = new Equ_DeskDP();
                    eCurr = eCurr.GetReCorded((long)pEqu_DeskDP.ID);    //Ŀǰ���ݿ��ȡֵ
                    bool bIsChange = CompareVerEqu(pEqu_DeskDP, eCurr);    //�Ƿ��������޸���,True��ʾ�޸Ĺ���
                    List<EQU_deploy> list2 = EQU_deploy.getEQU_deployList((long)pEqu_DeskDP.ID);
                    if (list != list2)
                    {
                        bIsChange = true;
                    }
                    if (bIsChange)//����������޸Ĺ��ˣ���ִ��
                    {
                        strSQL = @"INSERT INTO Equ_DeskHistory(
									                ID,
                                                    FlowID,
									                Name,
									                Code,
									                Positions,
									                SerialNumber,
                                                    Breed,
                                                    Model,
									                Provide,
									                ProvideName,
									                EquStatusID,
									                EquStatusName,
                                                    CatalogID,
									                CatalogName,
									                FullID,
									                Deleted,
                                                    UpdateUserID,
									                RegUserID,
									                RegUserName,
									                RegDeptID,
									                RegDeptName,
									                RegTime,
                                                    partBankId,
                                                    partBankName,
                                                    partBranchId,
                                                    partBranchName,
									                ConfigureInfo,
                                                    ConfigureValue,
									                Costom,
									                CostomName,
									                ServiceBeginTime,
									                ServiceEndTime,
                                                    Version,
                                                    VersionTime,
                                                    ConfigureSchema,
									                ItemCode,
                                                    ListID,
                                                    ListName,
                                                    Mastcustid
					                )
					                VALUES( " +
                                    eCurr.ID.ToString() + "," +
                                    "0" + "," +
                                    StringTool.SqlQ(eCurr.Name) + "," +
                                    StringTool.SqlQ(eCurr.Code) + "," +
                                    StringTool.SqlQ(eCurr.Positions) + "," +
                                    StringTool.SqlQ(eCurr.SerialNumber) + "," +
                                    StringTool.SqlQ(eCurr.Breed) + "," +
                                    StringTool.SqlQ(eCurr.Model) + "," +
                                    eCurr.Provide.ToString() + "," +
                                    StringTool.SqlQ(eCurr.ProvideName) + "," +
                                    eCurr.EquStatusID.ToString() + "," +
                                    StringTool.SqlQ(eCurr.EquStatusName) + "," +
                                    eCurr.CatalogID.ToString() + "," +
                                    StringTool.SqlQ(eCurr.CatalogName) + "," +
                                    StringTool.SqlQ(eCurr.FullID) + "," +
                                    eCurr.Deleted.ToString() + "," +
                                    lngUserID.ToString() + "," +
                                    eCurr.RegUserID.ToString() + "," +
                                    StringTool.SqlQ(eCurr.RegUserName) + "," +
                                    eCurr.RegDeptID.ToString() + "," +
                                    StringTool.SqlQ(eCurr.RegDeptName) + "," +
                                    (eCurr.RegTime == DateTime.MinValue ? " null " : "to_date(" + StringTool.SqlQ(eCurr.RegTime.ToString())) + ",'yyyy-MM-dd HH24:mi:ss')," +
                                    eCurr.partBankId.ToString() + "," +
                                    StringTool.SqlQ(eCurr.partBankName.ToString()) + "," +
                                    eCurr.partBranchId.ToString() + "," +
                                    StringTool.SqlQ(eCurr.partBranchName.ToString()) + "," +
                                    StringTool.SqlQ(eCurr.ConfigureInfo) + "," +
                                    StringTool.SqlQ(eCurr.ConfigureValue) + "," +
                                    eCurr.Costom.ToString() + "," +
                                    StringTool.SqlQ(eCurr.CostomName) + "," +
                                    (eCurr.ServiceBeginTime == DateTime.MinValue ? " null " : "to_date(" + StringTool.SqlQ(eCurr.ServiceBeginTime.ToString())) + ",'yyyy-MM-dd HH24:mi:ss')," +
                                    (eCurr.ServiceEndTime == DateTime.MinValue ? " null " : "to_date(" + StringTool.SqlQ(eCurr.ServiceEndTime.ToString())) + ",'yyyy-MM-dd HH24:mi:ss')," +
                                    eCurr.Version.ToString() + "," +
                                    "sysdate" + "," +
                                    StringTool.SqlQ(Equ_SubjectDP.GetCatalogSchema((long)eCurr.CatalogID)) + "," +
                                    StringTool.SqlQ(eCurr.ItemCode) + "," +
                                    eCurr.ListID.ToString() + "," +
                                    StringTool.SqlQ(eCurr.ListName.ToString()) + "," +
                                    (eCurr.Mastcustid.ToString() == "" ? "0" : eCurr.Mastcustid.ToString()) +

                            ")";
                        OracleDbHelper.ExecuteNonQuery(tran, CommandType.Text, strSQL);
                        //�����ʲ���ʷ������Ϣ
                        foreach (EQU_deploy deployHtory in list2)
                        {
                            EQU_deployHistory deployHistory = new EQU_deployHistory();
                            deployHistory.ID = deployHtory.ID;
                            deployHistory.EquID = deployHtory.EquID;
                            deployHistory.FieldID = deployHtory.FieldID;
                            deployHistory.CHName = deployHtory.CHName;
                            deployHistory.Value = deployHtory.Value;
                            deployHistory.version = int.Parse(eCurr.Version.ToString());
                            deployHistory.versionTime = System.DateTime.Now;
                            deployHistory.saveInsert(tran, deployHistory);

                        }

                        strSQL = @"UPDATE Equ_Desk Set " +
                                   " Name = " + StringTool.SqlQ(pEqu_DeskDP.Name) + "," +
                                   " Code = " + StringTool.SqlQ(pEqu_DeskDP.Code) + "," +
                                   " Positions = " + StringTool.SqlQ(pEqu_DeskDP.Positions) + "," +
                                   " SerialNumber = " + StringTool.SqlQ(pEqu_DeskDP.SerialNumber) + "," +
                                   " Breed = " + StringTool.SqlQ(pEqu_DeskDP.Breed) + "," +
                                   " Model = " + StringTool.SqlQ(pEqu_DeskDP.Model) + "," +
                                   " Provide = " + pEqu_DeskDP.Provide.ToString() + "," +
                                   " ProvideName = " + StringTool.SqlQ(pEqu_DeskDP.ProvideName) + "," +
                                    "EquStatusID = " + pEqu_DeskDP.EquStatusID.ToString() + "," +
                                   " EquStatusName = " + StringTool.SqlQ(pEqu_DeskDP.EquStatusName) + "," +
                                   " CatalogID = " + pEqu_DeskDP.CatalogID.ToString() + "," +
                                   " CatalogName = " + StringTool.SqlQ(pEqu_DeskDP.CatalogName) + "," +
                                   " FullID = " + StringTool.SqlQ(pEqu_DeskDP.FullID) + "," +
                                   " Deleted = " + pEqu_DeskDP.Deleted.ToString() + "," +
                                   " UpdateUserID = " + lngUserID.ToString() + "," +
                                   " RegUserID = " + pEqu_DeskDP.RegUserID.ToString() + "," +
                                   " RegUserName = " + StringTool.SqlQ(pEqu_DeskDP.RegUserName) + "," +
                                   " RegDeptID = " + pEqu_DeskDP.RegDeptID.ToString() + "," +
                                   " RegDeptName = " + StringTool.SqlQ(pEqu_DeskDP.RegDeptName) + "," +
                                   " RegTime = " + (pEqu_DeskDP.RegTime == DateTime.MinValue ? " null " : "to_date(" + StringTool.SqlQ(pEqu_DeskDP.RegTime.ToString())) + ",'yyyy-MM-dd HH24:mi:ss')," +
                                   " partBankId=" + pEqu_DeskDP.partBankId.ToString() + "," +
                                   " partBankName=" + StringTool.SqlQ(pEqu_DeskDP.partBankName.ToString()) + "," +
                                   " partBranchId=" + pEqu_DeskDP.partBranchId.ToString() + "," +
                                   " partBranchName=" + StringTool.SqlQ(pEqu_DeskDP.partBranchName.ToString()) + "," +
                                   " ConfigureInfo = " + StringTool.SqlQ(pEqu_DeskDP.ConfigureInfo) + "," +
                                   " ConfigureValue = " + StringTool.SqlQ(pEqu_DeskDP.ConfigureValue) + "," +
                                   " version = nvl(version,0) + 1" + "," +
                                   " Costom = " + pEqu_DeskDP.Costom.ToString() + "," +
                                   " CostomName = " + StringTool.SqlQ(pEqu_DeskDP.CostomName) + "," +
                                   " ServiceBeginTime = " + (pEqu_DeskDP.ServiceBeginTime == DateTime.MinValue ? " null " : "to_date(" + StringTool.SqlQ(pEqu_DeskDP.ServiceBeginTime.ToString())) + ",'yyyy-MM-dd HH24:mi:ss')," +
                                   " ServiceEndTime = " + (pEqu_DeskDP.ServiceEndTime == DateTime.MinValue ? " null " : "to_date(" + StringTool.SqlQ(pEqu_DeskDP.ServiceEndTime.ToString())) + ",'yyyy-MM-dd HH24:mi:ss')," +
                                   " ItemCode = " + StringTool.SqlQ(pEqu_DeskDP.ItemCode) + "," +
                                   " UpdateTime= sysdate," +
                                   " ListID=" + pEqu_DeskDP.ListID.ToString() + "," +
                                   " ListName=" + StringTool.SqlQ(pEqu_DeskDP.ListName) + "," +
                                   " Mastcustid=" + (pEqu_DeskDP.Mastcustid.ToString() == "" ? "0" : pEqu_DeskDP.Mastcustid.ToString()) +
                                       " WHERE ID = " + pEqu_DeskDP.ID.ToString();

                        OracleDbHelper.ExecuteNonQuery(tran, CommandType.Text, strSQL);
                        // �����ʲ�������Ϣ
                        foreach (EQU_deploy deploy in list)
                        {
                            deploy.EquID = long.Parse(pEqu_DeskDP.ID.ToString());
                            deploy.save(tran, deploy);
                        }

                    }
                }

                //���渽��
                SaveAttachments(tran, pEqu_DeskDP.ID, pEqu_DeskDP.AttachXml);

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
        /// ɾ������
        /// </summary>
        /// <param name="lngID"></param>
        public void DeleteRecorded(long lngID)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            try
            {
                string strSQL = "Update Equ_Desk Set Deleted=1,UpdateTime=sysdate  WHERE ID =" + lngID.ToString();

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
        /// ɾ����������
        /// </summary>
        /// <param name="lngID"></param>
        public void DeleteRecorded(string strID)
        {
            if (strID == "") strID = "0";

            OracleConnection cn = ConfigTool.GetConnection();
            try
            {
                string strSQL = "Update Equ_Desk Set Deleted=1,UpdateTime=sysdate  WHERE ID in (" + strID + ")";

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

        #region GetReCordedForChange

        /// <summary>
        /// ������ϻ�ȡ��ʱ��¼
        /// </summary>
        /// <param name="lngID"></param>
        /// <param name="lngFlowID"></param>
        /// <returns></returns>
        public Equ_DeskDP GetReCordedForChange(long lngID, long lngFlowID)
        {
            Equ_DeskDP ee = new Equ_DeskDP();
            string strSQL = string.Empty;
            OracleConnection cn = ConfigTool.GetConnection();
            strSQL = "SELECT * FROM Equ_DeskChange WHERE FlowID = " + lngFlowID.ToString() + " and ID=" + lngID;
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigTool.CloseConnection(cn);
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                ee.ID = Decimal.Parse(dr["ID"].ToString());
                ee.Name = dr["Name"].ToString();
                ee.Code = dr["Code"].ToString();
                ee.Positions = dr["Positions"].ToString();
                ee.SerialNumber = dr["SerialNumber"].ToString();
                ee.Provide = Decimal.Parse(dr["Provide"].ToString());
                ee.ProvideName = dr["ProvideName"].ToString();
                ee.Breed = dr["Breed"].ToString();
                ee.Model = dr["Model"].ToString();
                ee.CatalogID = Decimal.Parse(dr["CatalogID"].ToString());
                ee.CatalogName = dr["CatalogName"].ToString();
                ee.EquStatusID = (dr["EquStatusID"].ToString() == string.Empty ? 0 : Decimal.Parse(dr["EquStatusID"].ToString()));
                ee.EquStatusName = dr["EquStatusName"].ToString();
                ee.FullID = dr["FullID"].ToString();
                ee.Deleted = Decimal.Parse(dr["Deleted"].ToString());
                ee.RegUserID = Decimal.Parse(dr["RegUserID"].ToString());
                ee.RegUserName = dr["RegUserName"].ToString();
                ee.RegDeptID = Decimal.Parse(dr["RegDeptID"].ToString());
                ee.RegDeptName = dr["RegDeptName"].ToString();
                ee.RegTime = dr["RegTime"].ToString() == string.Empty ? DateTime.MinValue : DateTime.Parse(dr["RegTime"].ToString());
                ee.partBankId = decimal.Parse(dr["partBankId"].ToString() == "" ? "0" : dr["partBankId"].ToString());
                ee.partBranchId = decimal.Parse(dr["partBranchId"].ToString() == "" ? "0" : dr["partBranchId"].ToString());
                ee.partBankName = dr["partBankName"].ToString();
                ee.partBranchName = dr["partBranchName"].ToString();
                ee.ConfigureInfo = dr["ConfigureInfo"].ToString();
                ee.ConfigureValue = dr["ConfigureValue"].ToString();
                ee.Costom = Decimal.Parse(dr["Costom"].ToString());
                ee.CostomName = dr["CostomName"].ToString();
                ee.ServiceBeginTime = dr["ServiceBeginTime"].ToString() == string.Empty ? DateTime.MinValue : DateTime.Parse(dr["ServiceBeginTime"].ToString());
                ee.ServiceEndTime = dr["ServiceEndTime"].ToString() == string.Empty ? DateTime.MinValue : DateTime.Parse(dr["ServiceEndTime"].ToString());
                ee.ItemCode = dr["ItemCode"].ToString();
                ee.Version = dr["Version"].ToString() == string.Empty ? 0 : Decimal.Parse(dr["Version"].ToString());
                ee.ListID = Decimal.Parse(dr["ListID"].ToString() == "" ? "0" : dr["ListID"].ToString()); //�ʲ�����
                ee.ListName = dr["ListName"].ToString();
                ee.FlowID = lngFlowID;
                ee.ChangeBy = dr["ChangeBy"].ToString();
                ee.Changedate = dr["Changedate"].ToString() == string.Empty ? DateTime.MinValue.ToString() : dr["Changedate"].ToString();
                ee.FlowID = lngFlowID;
                ee.EquDeploy = Equ_DeskChangeDeploy.getEQU_ChangeDeployList(lngFlowID, long.Parse(ee.ID.ToString()));//�ʲ���չ��

                ee.Mastcustid = Decimal.Parse(dr["Mastcustid"].ToString() == "" ? "0" : dr["Mastcustid"].ToString()); //����λ

                //��ʱ��û�е�ǰ�޸�������
            }
            else
            {
                //û���򷵻ص�ǰ�豸����
                ee = this.GetReCorded(lngID);
            }
            return ee;
        }
        #endregion

        #region GetReCordedForChange

        /// <summary>
        /// ������ϻ�ȡ��ʱ��¼
        /// </summary>
        /// <param name="lngID"></param>
        /// <param name="lngFlowID"></param>
        /// <returns></returns>
        public Equ_DeskDP GetReCordedForChange(OracleTransaction trans, long lngID, long lngFlowID)
        {
            Equ_DeskDP ee = new Equ_DeskDP();
            string strSQL = string.Empty;
            OracleConnection cn = ConfigTool.GetConnection();
            strSQL = "SELECT * FROM Equ_DeskChange WHERE FlowID = " + lngFlowID.ToString() + " and ID=" + lngID;
            DataTable dt = OracleDbHelper.ExecuteDataset(trans, CommandType.Text, strSQL).Tables[0];
            ConfigTool.CloseConnection(cn);
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                ee.ID = Decimal.Parse(dr["ID"].ToString());
                ee.Name = dr["Name"].ToString();
                ee.Code = dr["Code"].ToString();
                ee.Positions = dr["Positions"].ToString();
                ee.SerialNumber = dr["SerialNumber"].ToString();
                ee.Provide = Decimal.Parse(dr["Provide"].ToString());
                ee.ProvideName = dr["ProvideName"].ToString();
                ee.Breed = dr["Breed"].ToString();
                ee.Model = dr["Model"].ToString();
                ee.CatalogID = Decimal.Parse(dr["CatalogID"].ToString());
                ee.CatalogName = dr["CatalogName"].ToString();
                ee.EquStatusID = (dr["EquStatusID"].ToString() == string.Empty ? 0 : Decimal.Parse(dr["EquStatusID"].ToString()));
                ee.EquStatusName = dr["EquStatusName"].ToString();
                ee.FullID = dr["FullID"].ToString();
                ee.Deleted = Decimal.Parse(dr["Deleted"].ToString());
                ee.RegUserID = Decimal.Parse(dr["RegUserID"].ToString());
                ee.RegUserName = dr["RegUserName"].ToString();
                ee.RegDeptID = Decimal.Parse(dr["RegDeptID"].ToString());
                ee.RegDeptName = dr["RegDeptName"].ToString();
                ee.RegTime = dr["RegTime"].ToString() == string.Empty ? DateTime.MinValue : DateTime.Parse(dr["RegTime"].ToString());
                ee.partBankId = decimal.Parse(dr["partBankId"].ToString() == "" ? "0" : dr["partBankId"].ToString());
                ee.partBranchId = decimal.Parse(dr["partBranchId"].ToString() == "" ? "0" : dr["partBranchId"].ToString());
                ee.partBankName = dr["partBankName"].ToString();
                ee.partBranchName = dr["partBranchName"].ToString();
                ee.ConfigureInfo = dr["ConfigureInfo"].ToString();
                ee.ConfigureValue = dr["ConfigureValue"].ToString();
                ee.Costom = Decimal.Parse(dr["Costom"].ToString());
                ee.CostomName = dr["CostomName"].ToString();
                ee.ServiceBeginTime = dr["ServiceBeginTime"].ToString() == string.Empty ? DateTime.MinValue : DateTime.Parse(dr["ServiceBeginTime"].ToString());
                ee.ServiceEndTime = dr["ServiceEndTime"].ToString() == string.Empty ? DateTime.MinValue : DateTime.Parse(dr["ServiceEndTime"].ToString());
                ee.ItemCode = dr["ItemCode"].ToString();
                ee.Version = dr["Version"].ToString() == string.Empty ? 0 : Decimal.Parse(dr["Version"].ToString());
                ee.ListID = Decimal.Parse(dr["ListID"].ToString() == "" ? "0" : dr["ListID"].ToString()); //�ʲ�����
                ee.ListName = dr["ListName"].ToString();
                ee.FlowID = lngFlowID;
                ee.ChangeBy = dr["ChangeBy"].ToString();
                ee.Changedate = dr["Changedate"].ToString() == string.Empty ? DateTime.MinValue.ToString() : dr["Changedate"].ToString();
                ee.FlowID = lngFlowID;
                ee.EquDeploy = Equ_DeskChangeDeploy.getEQU_ChangeDeployList(trans, lngFlowID, long.Parse(ee.ID.ToString()));//�ʲ���չ��

                ee.Mastcustid = Decimal.Parse(dr["Mastcustid"].ToString() == "" ? "0" : dr["Mastcustid"].ToString()); //����λ
                //��ʱ��û�е�ǰ�޸�������
            }
            else
            {
                //û���򷵻ص�ǰ�豸����
                ee = this.GetReCorded(trans, lngID);
            }
            return ee;
        }
        #endregion

        #region GetDeskRequestStatus

        /// <summary>
        /// ��ȡ�ʲ�����״̬,��ǰFLOWID����
        /// </summary>
        /// <param name="lngID"></param>
        /// <param name="lngFlowID"></param>
        /// <returns></returns>
        public int GetDeskRequestStatus(long lngID, long lngFlowID)
        {
            int iRequest = 0;
            OracleConnection cn = ConfigTool.GetConnection();
            string strSQL = "";
            if (lngFlowID == 0)
            {
                strSQL = "SELECT nvl(request,0) FROM Equ_Desk WHERE ID=" + lngID.ToString();
            }
            else
            {
                strSQL = "SELECT nvl(request,0) FROM Equ_Desk WHERE ID=" + lngID.ToString() +
                                " AND id <> (select equipmentid FROM equ_changeservice WHERE ROWNUM<=1 AND flowid = " + lngFlowID.ToString() + " )";
            }

            try
            {
                OracleDataReader dr = OracleDbHelper.ExecuteReader(cn, CommandType.Text, strSQL);
                while (dr.Read())
                {
                    iRequest = dr.GetInt32(0);

                    break;
                }
                dr.Close();
            }
            finally { ConfigTool.CloseConnection(cn); }

            return iRequest;

        }

        #endregion

        #region GetTempChangeExist
        /// <summary>
        /// �ж��Ƿ���ڱ���������ʱ�����Ϣ
        /// </summary>
        /// <param name="lngFlowID"></param>
        /// <returns></returns>
        public bool GetTempChangeExist(long lngFlowID)
        {
            bool iRequest = false;


            if (lngFlowID != 0)
            {
                OracleConnection cn = ConfigTool.GetConnection();

                try
                {
                    string strSQL = "";
                    strSQL = "SELECT count(*) FROM Equ_DeskChange WHERE FlowID=" + lngFlowID.ToString();

                    OracleDataReader dr = OracleDbHelper.ExecuteReader(cn, CommandType.Text, strSQL);
                    while (dr.Read())
                    {
                        if (dr.GetInt32(0) > 0)
                            iRequest = true;

                        break;
                    }
                    dr.Close();


                }
                finally { ConfigTool.CloseConnection(cn); }
            }
            return iRequest;

        }
        #endregion

        #region GetReCordedForVersion

        /// <summary>
        /// ��ȡ��Ӧ�汾����Ϣ
        /// </summary>
        /// <param name="lngID"></param>
        /// <param name="lngVersion"></param>
        /// <returns></returns>
        public Equ_DeskDP GetReCordedForVersion(long lngID, long lngVersion)
        {
            Equ_DeskDP ee = new Equ_DeskDP();
            if (Equ_DeskDP.isversion(lngVersion, lngID))
            {
                ee = ee.GetReCorded(lngID);
            }
            else
            {
                string strSQL = string.Empty;
                OracleConnection cn = ConfigTool.GetConnection();
                strSQL = "SELECT * FROM Equ_DeskHistory WHERE ID = " + lngID.ToString() + " AND version = " + lngVersion.ToString(); ;
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
                ConfigTool.CloseConnection(cn);
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];

                    ee.ID = Decimal.Parse(dr["ID"].ToString());
                    ee.Name = dr["Name"].ToString();
                    ee.Code = dr["Code"].ToString();
                    ee.Positions = dr["Positions"].ToString();
                    ee.SerialNumber = dr["SerialNumber"].ToString();
                    ee.Provide = Decimal.Parse(dr["Provide"].ToString());
                    ee.ProvideName = dr["ProvideName"].ToString();
                    ee.Breed = dr["Breed"].ToString();
                    ee.Model = dr["Model"].ToString();
                    ee.CatalogID = Decimal.Parse(dr["CatalogID"].ToString());
                    ee.CatalogName = dr["CatalogName"].ToString();
                    ee.EquStatusID = (dr["EquStatusID"].ToString() == string.Empty ? 0 : Decimal.Parse(dr["EquStatusID"].ToString()));
                    ee.EquStatusName = dr["EquStatusName"].ToString();
                    ee.FullID = dr["FullID"].ToString();
                    ee.Deleted = Decimal.Parse(dr["Deleted"].ToString());
                    ee.UpdateUserID = Decimal.Parse(dr["UpdateUserID"].ToString());
                    ee.RegUserID = Decimal.Parse(dr["RegUserID"].ToString());
                    ee.RegUserName = dr["RegUserName"].ToString();
                    ee.RegDeptID = Decimal.Parse(dr["RegDeptID"].ToString());
                    ee.RegDeptName = dr["RegDeptName"].ToString();
                    ee.RegTime = dr["RegTime"].ToString() == string.Empty ? DateTime.MinValue : DateTime.Parse(dr["RegTime"].ToString());
                    ee.partBankId = decimal.Parse(dr["partBankId"].ToString() == "" ? "0" : dr["partBankId"].ToString());
                    ee.partBranchId = decimal.Parse(dr["partBranchId"].ToString() == "" ? "0" : dr["partBranchId"].ToString());
                    ee.partBankName = dr["partBankName"].ToString();
                    ee.partBranchName = dr["partBranchName"].ToString();
                    ee.ConfigureInfo = dr["ConfigureInfo"].ToString();
                    ee.ConfigureValue = dr["ConfigureValue"].ToString();
                    ee.Costom = Decimal.Parse(dr["Costom"].ToString());
                    ee.CostomName = dr["CostomName"].ToString();
                    ee.ServiceBeginTime = dr["ServiceBeginTime"].ToString() == string.Empty ? DateTime.MinValue : DateTime.Parse(dr["ServiceBeginTime"].ToString());
                    ee.ServiceEndTime = dr["ServiceEndTime"].ToString() == string.Empty ? DateTime.MinValue : DateTime.Parse(dr["ServiceEndTime"].ToString());
                    ee.ItemCode = dr["ItemCode"].ToString();
                    ee.Version = dr["Version"].ToString() == string.Empty ? 0 : Decimal.Parse(dr["Version"].ToString());
                    ee.FlowID = dr["FlowID"].ToString() == string.Empty ? 0 : Decimal.Parse(dr["FlowID"].ToString());
                    ee.ListID = Decimal.Parse(dr["ListID"].ToString() == "" ? "0" : dr["ListID"].ToString()); //�ʲ�����
                    ee.ListName = dr["ListName"].ToString();
                    //��ʷ�汾�����������
                    ee.HistorySchema = dr["ConfigureSchema"].ToString();

                    ee.Mastcustid = Decimal.Parse(dr["Mastcustid"].ToString() == "" ? "0" : dr["Mastcustid"].ToString()); //����λ
                }

            }
            return ee;
        }

        #endregion

        #region GetReCordedForNextVersion
        /// <summary>
        /// ��ȡ��һ���汾���ʲ�,û�����ȡ���°汾
        /// </summary>
        /// <param name="lngID"></param>
        /// <param name="lngVersion"></param>
        /// <returns></returns>
        public Equ_DeskDP GetReCordedForNextVersion(long lngID, long lngVersion)
        {
            Equ_DeskDP ee = new Equ_DeskDP();
            string strSQL = string.Empty;
            OracleConnection cn = ConfigTool.GetConnection();
            strSQL = "SELECT * FROM Equ_DeskHistory WHERE ROWNUM<=1 AND ID = " + lngID.ToString() + " AND version > " + lngVersion.ToString() + " ORDER BY version";
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigTool.CloseConnection(cn);
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];

                ee.ID = Decimal.Parse(dr["ID"].ToString());
                ee.Name = dr["Name"].ToString();
                ee.Code = dr["Code"].ToString();
                ee.Positions = dr["Positions"].ToString();
                ee.SerialNumber = dr["SerialNumber"].ToString();
                ee.Provide = Decimal.Parse(dr["Provide"].ToString());
                ee.ProvideName = dr["ProvideName"].ToString();
                ee.Breed = dr["Breed"].ToString();
                ee.Model = dr["Model"].ToString();
                ee.CatalogID = Decimal.Parse(dr["CatalogID"].ToString());
                ee.CatalogName = dr["CatalogName"].ToString();
                ee.EquStatusID = (dr["EquStatusID"].ToString() == string.Empty ? 0 : Decimal.Parse(dr["EquStatusID"].ToString()));
                ee.EquStatusName = dr["EquStatusName"].ToString();
                ee.FullID = dr["FullID"].ToString();
                ee.Deleted = Decimal.Parse(dr["Deleted"].ToString());
                ee.UpdateUserID = Decimal.Parse(dr["UpdateUserID"].ToString());
                ee.RegUserID = Decimal.Parse(dr["RegUserID"].ToString());
                ee.RegUserName = dr["RegUserName"].ToString();
                ee.RegDeptID = Decimal.Parse(dr["RegDeptID"].ToString());
                ee.RegDeptName = dr["RegDeptName"].ToString();
                ee.RegTime = dr["RegTime"].ToString() == string.Empty ? DateTime.MinValue : DateTime.Parse(dr["RegTime"].ToString());
                ee.partBankId = decimal.Parse(dr["partBankId"].ToString() == "" ? "0" : dr["partBankId"].ToString());
                ee.partBranchId = decimal.Parse(dr["partBranchId"].ToString() == "" ? "0" : dr["partBranchId"].ToString());
                ee.partBankName = dr["partBankName"].ToString();
                ee.partBranchName = dr["partBranchName"].ToString();
                ee.ConfigureInfo = dr["ConfigureInfo"].ToString();
                ee.ConfigureValue = dr["ConfigureValue"].ToString();
                ee.Costom = Decimal.Parse(dr["Costom"].ToString());
                ee.CostomName = dr["CostomName"].ToString();
                ee.ServiceBeginTime = dr["ServiceBeginTime"].ToString() == string.Empty ? DateTime.MinValue : DateTime.Parse(dr["ServiceBeginTime"].ToString());
                ee.ServiceEndTime = dr["ServiceEndTime"].ToString() == string.Empty ? DateTime.MinValue : DateTime.Parse(dr["ServiceEndTime"].ToString());
                ee.ItemCode = dr["ItemCode"].ToString();
                ee.Version = dr["Version"].ToString() == string.Empty ? 0 : Decimal.Parse(dr["Version"].ToString());
                ee.FlowID = dr["FlowID"].ToString() == string.Empty ? 0 : Decimal.Parse(dr["FlowID"].ToString());
                ee.ListID = Decimal.Parse(dr["ListID"].ToString() == "" ? "0" : dr["ListID"].ToString()); //�ʲ�����
                ee.ListName = dr["ListName"].ToString();

                ee.Mastcustid = Decimal.Parse(dr["Mastcustid"].ToString() == "" ? "0" : dr["Mastcustid"].ToString()); //����λ
            }
            else
            {
                //û���򷵻ص�ǰ�豸����
                ee = this.GetReCorded(lngID);
            }
            return ee;
        }

        #endregion

        #region GetReCordedForLastHistory
        /// <summary>
        /// ��ȡ����һ����ʷ�汾���ʲ�
        /// </summary>
        /// <param name="lngID"></param>
        /// <returns></returns>
        public Equ_DeskDP GetReCordedForLastHistory(long lngID)
        {
            Equ_DeskDP ee = new Equ_DeskDP();
            string strSQL = string.Empty;
            OracleConnection cn = ConfigTool.GetConnection();
            strSQL = "SELECT * FROM Equ_DeskHistory WHERE ROWNUM<=1 AND ID = " + lngID.ToString() + " ORDER BY version DESC";
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigTool.CloseConnection(cn);
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];

                ee.ID = Decimal.Parse(dr["ID"].ToString());
                ee.Name = dr["Name"].ToString();
                ee.Code = dr["Code"].ToString();
                ee.Positions = dr["Positions"].ToString();
                ee.SerialNumber = dr["SerialNumber"].ToString();
                ee.Provide = Decimal.Parse(dr["Provide"].ToString());
                ee.ProvideName = dr["ProvideName"].ToString();
                ee.Breed = dr["Breed"].ToString();
                ee.Model = dr["Model"].ToString();
                ee.CatalogID = Decimal.Parse(dr["CatalogID"].ToString());
                ee.CatalogName = dr["CatalogName"].ToString();
                ee.EquStatusID = (dr["EquStatusID"].ToString() == string.Empty ? 0 : Decimal.Parse(dr["EquStatusID"].ToString()));
                ee.EquStatusName = dr["EquStatusName"].ToString();
                ee.FullID = dr["FullID"].ToString();
                ee.Deleted = Decimal.Parse(dr["Deleted"].ToString());
                ee.UpdateUserID = Decimal.Parse(dr["UpdateUserID"].ToString());
                ee.RegUserID = Decimal.Parse(dr["RegUserID"].ToString());
                ee.RegUserName = dr["RegUserName"].ToString();
                ee.RegDeptID = Decimal.Parse(dr["RegDeptID"].ToString());
                ee.RegDeptName = dr["RegDeptName"].ToString();
                ee.RegTime = dr["RegTime"].ToString() == string.Empty ? DateTime.MinValue : DateTime.Parse(dr["RegTime"].ToString());
                ee.partBankId = decimal.Parse(dr["partBankId"].ToString() == "" ? "0" : dr["partBankId"].ToString());
                ee.partBranchId = decimal.Parse(dr["partBranchId"].ToString() == "" ? "0" : dr["partBranchId"].ToString());
                ee.partBankName = dr["partBankName"].ToString();
                ee.partBranchName = dr["partBranchName"].ToString();
                ee.ConfigureInfo = dr["ConfigureInfo"].ToString();
                ee.ConfigureValue = dr["ConfigureValue"].ToString();
                ee.Costom = Decimal.Parse(dr["Costom"].ToString());
                ee.CostomName = dr["CostomName"].ToString();
                ee.ServiceBeginTime = dr["ServiceBeginTime"].ToString() == string.Empty ? DateTime.MinValue : DateTime.Parse(dr["ServiceBeginTime"].ToString());
                ee.ServiceEndTime = dr["ServiceEndTime"].ToString() == string.Empty ? DateTime.MinValue : DateTime.Parse(dr["ServiceEndTime"].ToString());
                ee.ItemCode = dr["ItemCode"].ToString();
                ee.Version = dr["Version"].ToString() == string.Empty ? 0 : Decimal.Parse(dr["Version"].ToString());
                ee.FlowID = dr["FlowID"].ToString() == string.Empty ? 0 : Decimal.Parse(dr["FlowID"].ToString());
                ee.ListID = Decimal.Parse(dr["ListID"].ToString() == "" ? "0" : dr["ListID"].ToString()); //�ʲ�����
                ee.ListName = dr["ListName"].ToString();
                ee.EquDeploy = EQU_deployHistory.getEQU_deployVersionList(long.Parse(ee.ID.ToString()), long.Parse(ee.Version.ToString()));
                ee.Mastcustid = Decimal.Parse(dr["Mastcustid"].ToString() == "" ? "0" : dr["Mastcustid"].ToString()); //����λ
            }

            return ee;
        }

        #endregion

        #region GetReCordedForPreVersion
        /// <summary>
        /// ��ȡǰһ���汾���ʲ�
        /// </summary>
        /// <param name="lngID"></param>
        /// <param name="lngVersion"></param>
        /// <returns></returns>
        public Equ_DeskDP GetReCordedForPreVersion(long lngID, long lngVersion)
        {
            Equ_DeskDP ee = new Equ_DeskDP();
            string strSQL = string.Empty;
            OracleConnection cn = ConfigTool.GetConnection();
            strSQL = "SELECT * FROM Equ_DeskHistory WHERE ROWNUM<=1 AND ID = " + lngID.ToString() + " AND version < " + lngVersion.ToString() + " ORDER BY version DESC";
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigTool.CloseConnection(cn);
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];

                ee.ID = Decimal.Parse(dr["ID"].ToString());
                ee.Name = dr["Name"].ToString();
                ee.Code = dr["Code"].ToString();
                ee.Positions = dr["Positions"].ToString();
                ee.SerialNumber = dr["SerialNumber"].ToString();
                ee.Provide = Decimal.Parse(dr["Provide"].ToString());
                ee.ProvideName = dr["ProvideName"].ToString();
                ee.Breed = dr["Breed"].ToString();
                ee.Model = dr["Model"].ToString();
                ee.CatalogID = Decimal.Parse(dr["CatalogID"].ToString());
                ee.CatalogName = dr["CatalogName"].ToString();
                ee.EquStatusID = (dr["EquStatusID"].ToString() == string.Empty ? 0 : Decimal.Parse(dr["EquStatusID"].ToString()));
                ee.EquStatusName = dr["EquStatusName"].ToString();
                ee.FullID = dr["FullID"].ToString();
                ee.Deleted = Decimal.Parse(dr["Deleted"].ToString());
                ee.UpdateUserID = Decimal.Parse(dr["UpdateUserID"].ToString());
                ee.RegUserID = Decimal.Parse(dr["RegUserID"].ToString());
                ee.RegUserName = dr["RegUserName"].ToString();
                ee.RegDeptID = Decimal.Parse(dr["RegDeptID"].ToString());
                ee.RegDeptName = dr["RegDeptName"].ToString();
                ee.RegTime = dr["RegTime"].ToString() == string.Empty ? DateTime.MinValue : DateTime.Parse(dr["RegTime"].ToString());
                ee.partBankId = decimal.Parse(dr["partBankId"].ToString() == "" ? "0" : dr["partBankId"].ToString());
                ee.partBranchId = decimal.Parse(dr["partBranchId"].ToString() == "" ? "0" : dr["partBranchId"].ToString());
                ee.partBankName = dr["partBankName"].ToString();
                ee.partBranchName = dr["partBranchName"].ToString();
                ee.ConfigureInfo = dr["ConfigureInfo"].ToString();
                ee.ConfigureValue = dr["ConfigureValue"].ToString();
                ee.Costom = Decimal.Parse(dr["Costom"].ToString());
                ee.CostomName = dr["CostomName"].ToString();
                ee.ServiceBeginTime = dr["ServiceBeginTime"].ToString() == string.Empty ? DateTime.MinValue : DateTime.Parse(dr["ServiceBeginTime"].ToString());
                ee.ServiceEndTime = dr["ServiceEndTime"].ToString() == string.Empty ? DateTime.MinValue : DateTime.Parse(dr["ServiceEndTime"].ToString());
                ee.ItemCode = dr["ItemCode"].ToString();
                ee.Version = dr["Version"].ToString() == string.Empty ? 0 : Decimal.Parse(dr["Version"].ToString());

                ee.FlowID = dr["FlowID"].ToString() == string.Empty ? 0 : Decimal.Parse(dr["FlowID"].ToString());
                ee.ListID = Decimal.Parse(dr["ListID"].ToString() == "" ? "0" : dr["ListID"].ToString()); //�ʲ�����
                ee.ListName = dr["ListName"].ToString();

                ee.Mastcustid = Decimal.Parse(dr["Mastcustid"].ToString() == "" ? "0" : dr["Mastcustid"].ToString()); //����λ
            }
            else
            {

            }
            return ee;
        }
        #endregion

        #region GetReCordedForHistory

        /// <summary>
        /// ������ϻ�ȡ��Ӧ�汾����ʷ��¼
        /// </summary>
        /// <param name="lngFlowID"></param>
        /// <returns></returns>
        public Equ_DeskDP GetReCordedForHistory(long lngFlowID)
        {
            Equ_DeskDP ee = new Equ_DeskDP();
            string strSQL = string.Empty;
            OracleConnection cn = ConfigTool.GetConnection();
            strSQL = "SELECT * FROM Equ_DeskHistory WHERE FlowID = " + lngFlowID.ToString();
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigTool.CloseConnection(cn);
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];

                ee.ID = Decimal.Parse(dr["ID"].ToString());
                ee.Name = dr["Name"].ToString();
                ee.Code = dr["Code"].ToString();
                ee.Positions = dr["Positions"].ToString();
                ee.SerialNumber = dr["SerialNumber"].ToString();
                ee.Provide = Decimal.Parse(dr["Provide"].ToString());
                ee.ProvideName = dr["ProvideName"].ToString();
                ee.Breed = dr["Breed"].ToString();
                ee.Model = dr["Model"].ToString();
                ee.CatalogID = Decimal.Parse(dr["CatalogID"].ToString());
                ee.CatalogName = dr["CatalogName"].ToString();
                ee.EquStatusID = (dr["EquStatusID"].ToString() == string.Empty ? 0 : Decimal.Parse(dr["EquStatusID"].ToString()));
                ee.EquStatusName = dr["EquStatusName"].ToString();
                ee.FullID = dr["FullID"].ToString();
                ee.Deleted = Decimal.Parse(dr["Deleted"].ToString());
                ee.UpdateUserID = Decimal.Parse(dr["UpdateUserID"].ToString());
                ee.RegUserID = Decimal.Parse(dr["RegUserID"].ToString());
                ee.RegUserName = dr["RegUserName"].ToString();
                ee.RegDeptID = Decimal.Parse(dr["RegDeptID"].ToString());
                ee.RegDeptName = dr["RegDeptName"].ToString();
                ee.RegTime = dr["RegTime"].ToString() == string.Empty ? DateTime.MinValue : DateTime.Parse(dr["RegTime"].ToString());
                ee.partBankId = decimal.Parse(dr["partBankId"].ToString() == "" ? "0" : dr["partBankId"].ToString());
                ee.partBranchId = decimal.Parse(dr["partBranchId"].ToString() == "" ? "0" : dr["partBranchId"].ToString());
                ee.partBankName = dr["partBankName"].ToString();
                ee.partBranchName = dr["partBranchName"].ToString();
                ee.ConfigureInfo = dr["ConfigureInfo"].ToString();
                ee.ConfigureValue = dr["ConfigureValue"].ToString();
                ee.Costom = Decimal.Parse(dr["Costom"].ToString());
                ee.CostomName = dr["CostomName"].ToString();
                ee.ServiceBeginTime = dr["ServiceBeginTime"].ToString() == string.Empty ? DateTime.MinValue : DateTime.Parse(dr["ServiceBeginTime"].ToString());
                ee.ServiceEndTime = dr["ServiceEndTime"].ToString() == string.Empty ? DateTime.MinValue : DateTime.Parse(dr["ServiceEndTime"].ToString());
                ee.ItemCode = dr["ItemCode"].ToString();
                ee.Version = dr["Version"].ToString() == string.Empty ? 0 : Decimal.Parse(dr["Version"].ToString());
                ee.ListID = Decimal.Parse(dr["ListID"].ToString() == "" ? "0" : dr["ListID"].ToString()); //�ʲ�����
                ee.ListName = dr["ListName"].ToString();
                ee.EquDeploy = EQU_deployHistory.getEQU_deployVersionList(long.Parse(ee.ID.ToString()), long.Parse(ee.Version.ToString()));

                ee.Mastcustid = Decimal.Parse(dr["Mastcustid"].ToString() == "" ? "0" : dr["Mastcustid"].ToString()); //����λ
            }

            return ee;
        }
        #endregion

        #region HasChangeTempToHistory

        /// <summary>
        /// �ж϶�Ӧ�ı���Ƿ��Ѿ�Ӧ��
        /// </summary>
        /// <param name="lngID"></param>
        /// <param name="lngVersion"></param>
        /// <returns></returns>
        public bool HasChangeTempToHistory(long lngID, long lngVersion)
        {
            int iCount = 0;
            string strSQL = string.Empty;
            OracleConnection cn = ConfigTool.GetConnection();

            try
            {
                strSQL = "SELECT count(*) FROM Equ_DeskHistory WHERE  ID = " + lngID.ToString() + " AND version = " + lngVersion.ToString();
                OracleDataReader dr = OracleDbHelper.ExecuteReader(cn, CommandType.Text, strSQL);
                while (dr.Read())
                {
                    iCount = dr.GetInt32(0);
                    break;
                }
                dr.Close();
            }
            finally { ConfigTool.CloseConnection(cn); }

            if (iCount > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region SaveToChangeTemp

        /// <summary>
        /// ���������ʱ�����¼
        /// </summary>
        /// <param name="pEqu_DeskDP">�ʲ���Ϣ</param>
        /// <param name="lngFlowID">����ID</param>
        /// <param name="lngUserID">�����û�</param>
        /// <param name="deployList">��չ��</param>
        public void SaveToChangeTemp(Equ_DeskDP pEqu_DeskDP, long lngFlowID, long lngUserID, List<EQU_deploy> deployList)
        {

            OracleConnection cn = ConfigTool.GetConnection();
            cn.Open();
            OracleTransaction trans = cn.BeginTransaction();

            try
            {
                #region ����IDΪ�ؼ���ɾ�������ʱ����

                string strSQL = "Delete Equ_DeskChange WHERE ID =" + pEqu_DeskDP.ID + " And FlowID=" + lngFlowID;
                OracleDbHelper.ExecuteNonQuery(trans, CommandType.Text, strSQL);

                #endregion

                #region ����IDΪ�ؼ���ɾ����չ����

                strSQL = "Delete Equ_DeskChangeDeploy WHERE EquID =" + pEqu_DeskDP.ID + " And FlowID=" + lngFlowID;
                OracleDbHelper.ExecuteNonQuery(trans, CommandType.Text, strSQL);

                #endregion

                #region ������

                strSQL = @"INSERT INTO Equ_DeskChange(
									ID,
                                    FlowID,
									Name,
									Code,
									Positions,
									SerialNumber,
                                    Breed,
                                    Model,
									Provide,
									ProvideName,
									EquStatusID,
									EquStatusName,
                                    CatalogID,
									CatalogName,
									FullID,
									Deleted,
                                    UpdateUserID,
									RegUserID,
									RegUserName,
									RegDeptID,
									RegDeptName,
									RegTime,
                                    partBankId,
                                    partBankName,
                                    partBranchId,
                                    partBranchName,
                                    ListID,
                                    ListName,
									ConfigureInfo,
                                    ConfigureValue,
									Costom,
									CostomName,
									ServiceBeginTime,
									ServiceEndTime,
                                    Version,
									ItemCode,
                                    Mastcustid
					)
					VALUES( " +
                            pEqu_DeskDP.ID.ToString() + "," +
                            lngFlowID.ToString() + "," +
                            StringTool.SqlQ(pEqu_DeskDP.Name) + "," +
                            StringTool.SqlQ(pEqu_DeskDP.Code) + "," +
                            StringTool.SqlQ(pEqu_DeskDP.Positions) + "," +
                            StringTool.SqlQ(pEqu_DeskDP.SerialNumber) + "," +
                            StringTool.SqlQ(pEqu_DeskDP.Breed) + "," +
                            StringTool.SqlQ(pEqu_DeskDP.Model) + "," +
                            pEqu_DeskDP.Provide.ToString() + "," +
                            StringTool.SqlQ(pEqu_DeskDP.ProvideName) + "," +
                            pEqu_DeskDP.EquStatusID.ToString() + "," +
                            StringTool.SqlQ(pEqu_DeskDP.EquStatusName) + "," +
                            pEqu_DeskDP.CatalogID.ToString() + "," +
                            StringTool.SqlQ(pEqu_DeskDP.CatalogName) + "," +
                            StringTool.SqlQ(pEqu_DeskDP.FullID) + "," +
                            pEqu_DeskDP.Deleted.ToString() + "," +
                            lngUserID.ToString() + "," +
                            pEqu_DeskDP.RegUserID.ToString() + "," +
                            StringTool.SqlQ(pEqu_DeskDP.RegUserName) + "," +
                            pEqu_DeskDP.RegDeptID.ToString() + "," +
                            StringTool.SqlQ(pEqu_DeskDP.RegDeptName) + "," +
                            (pEqu_DeskDP.RegTime == DateTime.MinValue ? " null " : "to_date(" + StringTool.SqlQ(pEqu_DeskDP.RegTime.ToString()) + ",'yyyy-MM-dd HH24:mi:ss')") + "," +
                             pEqu_DeskDP.partBankId.ToString() + "," +
                             StringTool.SqlQ(pEqu_DeskDP.partBankName.ToString()) + "," +
                            pEqu_DeskDP.partBranchId.ToString() + "," +
                            StringTool.SqlQ(pEqu_DeskDP.partBranchName.ToString()) + "," +
                            pEqu_DeskDP.ListID.ToString() + "," +
                            StringTool.SqlQ(pEqu_DeskDP.ListName.ToString()) + "," +
                            StringTool.SqlQ(pEqu_DeskDP.ConfigureInfo) + "," +
                            StringTool.SqlQ(pEqu_DeskDP.ConfigureValue) + "," +
                            pEqu_DeskDP.Costom.ToString() + "," +
                            StringTool.SqlQ(pEqu_DeskDP.CostomName) + "," +
                            (pEqu_DeskDP.ServiceBeginTime == DateTime.MinValue ? " null " : "to_date(" + StringTool.SqlQ(pEqu_DeskDP.ServiceBeginTime.ToString()) + ",'yyyy-MM-dd HH24:mi:ss')") + "," +
                            (pEqu_DeskDP.ServiceEndTime == DateTime.MinValue ? " null " : "to_date(" + StringTool.SqlQ(pEqu_DeskDP.ServiceEndTime.ToString()) + ",'yyyy-MM-dd HH24:mi:ss')") + "," +
                            pEqu_DeskDP.Version.ToString() + "," +
                            StringTool.SqlQ(pEqu_DeskDP.ItemCode) + "," +
                            (pEqu_DeskDP.Mastcustid.ToString() == "" ? "0" : pEqu_DeskDP.Mastcustid.ToString()) +
                    ")";
                #endregion

                #region ִ�����

                OracleDbHelper.ExecuteNonQuery(trans, CommandType.Text, strSQL);

                #endregion

                #region ��������ʱ����չ����Ϣ

                //��������ʱ����չ����Ϣ
                foreach (EQU_deploy deploy in deployList)
                {
                    #region ��������

                    Equ_DeskChangeDeploy DeskChangeDeploy = new Equ_DeskChangeDeploy();

                    #endregion

                    #region �����ֵ
                    DeskChangeDeploy.ID = deploy.ID;//ID
                    DeskChangeDeploy.FlowId = lngFlowID;//����ID
                    DeskChangeDeploy.EquID = deploy.EquID;//�ʲ�ID
                    DeskChangeDeploy.FieldID = deploy.FieldID;//������ID
                    DeskChangeDeploy.CHName = deploy.CHName;//������������
                    DeskChangeDeploy.Value = deploy.Value;//������ֵ

                    #endregion

                    #region ִ�����

                    DeskChangeDeploy.InsertRecorded(trans, DeskChangeDeploy);//�������

                    #endregion

                }

                #endregion

                #region �����ύ

                trans.Commit();

                #endregion
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

        #region BatchUpdateSchemaItem
        /// <summary>
        /// ��������ֵ
        /// </summary>
        /// <param name="lngID"></param>
        /// <param name="sID"></param>
        /// <param name="sValue"></param>
        /// <param name="lngUserID"></param>
        public void BatchUpdateSchemaItem(long lngID, string sID, string sValue, long lngUserID)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            cn.Open();
            OracleTransaction tran = cn.BeginTransaction();
            try
            {
                string strSQL = string.Empty;
                Equ_DeskDP eLast = new Equ_DeskDP();
                eLast = eLast.GetReCordedForLastHistory(lngID);

                if (eLast.ID == lngID && eLast.UpdateUserID.ToString() == lngUserID.ToString() && eLast.FlowID == 0)
                {
                    //�������ʷ�汾
                    strSQL = @"UPDATE Equ_DeskHistory Set " +
                                     " VersionTime = sysdate " +
                                           " WHERE ID = " + eLast.ID.ToString() + " AND version =" + eLast.Version.ToString();

                    OracleDbHelper.ExecuteNonQuery(tran, CommandType.Text, strSQL);


                    strSQL = "Update Equ_Desk Set UpdateUserID = " + lngUserID.ToString() + "," + "UpdateTime=sysdate" +
                                       "  WHERE deleted = 0 AND ID =" + lngID.ToString();

                    OracleDbHelper.ExecuteNonQuery(tran, CommandType.Text, strSQL);

                    strSQL = "Update EQU_deploy SET [Value]=" + StringTool.SqlQ(sValue) + " WHERE EquID=" + lngID + " AND FieldID=" + sID;

                    OracleDbHelper.ExecuteNonQuery(tran, CommandType.Text, strSQL);
                }
                else
                {
                    //������ʷ�汾
                    Equ_DeskDP eCurr = new Equ_DeskDP();

                    eCurr = eCurr.GetReCorded(lngID);    //Ŀǰ���ݿ��ȡֵ
                    List<EQU_deploy> list2 = EQU_deploy.getEQU_deployList(lngID);
                    strSQL = @"INSERT INTO Equ_DeskHistory(
									                ID,
                                                    FlowID,
									                Name,
									                Code,
									                Positions,
									                SerialNumber,
                                                    Breed,
                                                    Model,
									                Provide,
									                ProvideName,
									                EquStatusID,
									                EquStatusName,
                                                    CatalogID,
									                CatalogName,
									                FullID,
									                Deleted,
                                                    UpdateUserID,
									                RegUserID,
									                RegUserName,
									                RegDeptID,
									                RegDeptName,
									                RegTime,
                                                    partBankId,
                                                    partBankName,
                                                    partBranchId,
                                                    partBranchName,
									                ConfigureInfo,
                                                    ConfigureValue,
									                Costom,
									                CostomName,
									                ServiceBeginTime,
									                ServiceEndTime,
                                                    Version,
                                                    VersionTime,
                                                    ConfigureSchema,
									                ItemCode,
                                                    Mastcustid
					                )
					                VALUES( " +
                                eCurr.ID.ToString() + "," +
                                "0" + "," +
                                StringTool.SqlQ(eCurr.Name) + "," +
                                StringTool.SqlQ(eCurr.Code) + "," +
                                StringTool.SqlQ(eCurr.Positions) + "," +
                                StringTool.SqlQ(eCurr.SerialNumber) + "," +
                                StringTool.SqlQ(eCurr.Breed) + "," +
                                StringTool.SqlQ(eCurr.Model) + "," +
                                eCurr.Provide.ToString() + "," +
                                StringTool.SqlQ(eCurr.ProvideName) + "," +
                                eCurr.EquStatusID.ToString() + "," +
                                StringTool.SqlQ(eCurr.EquStatusName) + "," +
                                eCurr.CatalogID.ToString() + "," +
                                StringTool.SqlQ(eCurr.CatalogName) + "," +
                                StringTool.SqlQ(eCurr.FullID) + "," +
                                eCurr.Deleted.ToString() + "," +
                                lngUserID.ToString() + "," +
                                eCurr.RegUserID.ToString() + "," +
                                StringTool.SqlQ(eCurr.RegUserName) + "," +
                                eCurr.RegDeptID.ToString() + "," +
                                StringTool.SqlQ(eCurr.RegDeptName) + "," +
                                (eCurr.RegTime == DateTime.MinValue ? " null " : StringTool.SqlQ(eCurr.RegTime.ToString())) + "," +
                                eCurr.partBankId.ToString() + "," +
                                StringTool.SqlQ(eCurr.partBankName.ToString()) + "," +
                                eCurr.partBranchId.ToString() + "," +
                                StringTool.SqlQ(eCurr.partBranchName.ToString()) + "," +
                                StringTool.SqlQ(eCurr.ConfigureInfo) + "," +
                                StringTool.SqlQ(eCurr.ConfigureValue) + "," +
                                eCurr.Costom.ToString() + "," +
                                StringTool.SqlQ(eCurr.CostomName) + "," +
                                (eCurr.ServiceBeginTime == DateTime.MinValue ? " null " : StringTool.SqlQ(eCurr.ServiceBeginTime.ToString())) + "," +
                                (eCurr.ServiceEndTime == DateTime.MinValue ? " null " : StringTool.SqlQ(eCurr.ServiceEndTime.ToString())) + "," +
                                eCurr.Version.ToString() + "," +
                                "sysdate" + "," +
                                StringTool.SqlQ(Equ_SubjectDP.GetCatalogSchema((long)eCurr.CatalogID)) + "," +
                                StringTool.SqlQ(eCurr.ItemCode) + "," +
                                (eCurr.Mastcustid.ToString() == "" ? "0" : eCurr.Mastcustid.ToString()) +
                        ")";
                    OracleDbHelper.ExecuteNonQuery(tran, CommandType.Text, strSQL);

                    //�����ʲ���ʷ������Ϣ
                    foreach (EQU_deploy deployHtory in list2)
                    {
                        EQU_deployHistory deployHistory = new EQU_deployHistory();
                        deployHistory.ID = deployHtory.ID;
                        deployHistory.EquID = deployHtory.EquID;
                        deployHistory.FieldID = deployHtory.FieldID;
                        deployHistory.CHName = deployHtory.CHName;
                        deployHistory.Value = deployHtory.Value;
                        deployHistory.version = int.Parse(eCurr.Version.ToString());
                        deployHistory.versionTime = System.DateTime.Now;
                        deployHistory.saveInsert(tran, deployHistory);

                    }

                    strSQL = "Update Equ_Desk Set version = nvl(version,0) + 1,UpdateUserID = " + lngUserID.ToString() + "," + "UpdateTime=sysdate" +
                                       "  WHERE deleted = 0 AND ID =" + lngID.ToString();

                    OracleDbHelper.ExecuteNonQuery(tran, CommandType.Text, strSQL);

                    strSQL = "Update EQU_deploy SET [Value]=" + StringTool.SqlQ(sValue) + " WHERE EquID=" + lngID + " AND FieldID=" + sID;

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

        #endregion

        #region ���ݿͻ�IDȡ���ʲ�
        /// <summary>
        /// ���ݿͻ�IDȡ���ʲ�
        /// </summary>
        /// <param name="CustID"></param>
        /// <returns></returns>
        public Equ_DeskDP GetEquByCustID(long CustID)
        {
            Equ_DeskDP ee = new Equ_DeskDP();
            string strSQL = string.Empty;
            OracleConnection cn = ConfigTool.GetConnection();
            strSQL = "SELECT * FROM Equ_Desk WHERE rownum<=1 and Deleted=0 And Costom = " + CustID.ToString();
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigTool.CloseConnection(cn);
            foreach (DataRow dr in dt.Rows)
            {
                ee.ID = Decimal.Parse(dr["ID"].ToString());
                ee.Name = dr["Name"].ToString();
                ee.Code = dr["Code"].ToString();
                ee.Breed = dr["Breed"].ToString();
                ee.Model = dr["Model"].ToString();
                ee.Positions = dr["Positions"].ToString();
                ee.SerialNumber = dr["SerialNumber"].ToString();
                ee.Provide = Decimal.Parse(dr["Provide"].ToString());
                ee.ProvideName = dr["ProvideName"].ToString();
                ee.CatalogID = Decimal.Parse(dr["CatalogID"].ToString());
                ee.CatalogName = dr["CatalogName"].ToString();
                ee.EquStatusID = (dr["EquStatusID"].ToString() == string.Empty ? 0 : Decimal.Parse(dr["EquStatusID"].ToString()));
                ee.EquStatusName = dr["EquStatusName"].ToString();
                ee.FullID = dr["FullID"].ToString();
                ee.Deleted = Decimal.Parse(dr["Deleted"].ToString());
                ee.UpdateUserID = Decimal.Parse(dr["UpdateUserID"].ToString());
                ee.RegUserID = Decimal.Parse(dr["RegUserID"].ToString());
                ee.RegUserName = dr["RegUserName"].ToString();
                ee.RegDeptID = Decimal.Parse(dr["RegDeptID"].ToString());
                ee.RegDeptName = dr["RegDeptName"].ToString();
                ee.RegTime = dr["RegTime"].ToString() == string.Empty ? DateTime.MinValue : DateTime.Parse(dr["RegTime"].ToString());
                ee.partBankId = decimal.Parse(dr["partBankId"].ToString() == "" ? "0" : dr["partBankId"].ToString());
                ee.partBranchId = decimal.Parse(dr["partBranchId"].ToString() == "" ? "0" : dr["partBranchId"].ToString());
                ee.partBankName = dr["partBankName"].ToString();
                ee.partBranchName = dr["partBranchName"].ToString();
                ee.ConfigureInfo = dr["ConfigureInfo"].ToString();
                ee.ConfigureValue = dr["ConfigureValue"].ToString();
                ee.Costom = Decimal.Parse(dr["Costom"].ToString());
                ee.CostomName = dr["CostomName"].ToString();
                ee.ServiceBeginTime = dr["ServiceBeginTime"].ToString() == string.Empty ? DateTime.MinValue : DateTime.Parse(dr["ServiceBeginTime"].ToString());
                ee.ServiceEndTime = dr["ServiceEndTime"].ToString() == string.Empty ? DateTime.MinValue : DateTime.Parse(dr["ServiceEndTime"].ToString());
                ee.ItemCode = dr["ItemCode"].ToString();
                ee.Version = dr["Version"].ToString() == string.Empty ? 0 : Decimal.Parse(dr["Version"].ToString());
                ee.ListID = Decimal.Parse(dr["ListID"].ToString() == "" ? "0" : dr["ListID"].ToString()); //�ʲ�����
                ee.ListName = dr["ListName"].ToString();
            }
            return ee;
        }
        #endregion

        #region �ж��Ƿ��ظ�

        /// <summary>
        /// �ж��Ƿ��ظ�
        /// </summary>
        /// <param name="version"></param>
        /// <param name="EquId"></param>
        /// <returns></returns>
        public static bool isversion(long version, long EquId)
        {
            string strSQl = "select * from equ_desk  where id=" + EquId.ToString() + " and version=" + version.ToString();
            DataTable dt = CommonDP.ExcuteSqlTable(strSQl);
            if (dt.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }


        }

        /// <summary>
        /// �ж��Ƿ��ظ�
        /// </summary>
        /// <param name="version"></param>
        /// <param name="EquId"></param>
        /// <returns></returns>
        public static string isversionRtnXml(long version, long EquId)
        {
            string strSQl = "select configureSchema from Equ_DeskHistory  where id=" + EquId.ToString() + " and version=" + version.ToString();
            DataTable dt = CommonDP.ExcuteSqlTable(strSQl);
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0]["configureSchema"].ToString();
            }
            else
            {
                return "";
            }


        }

        #endregion

        #region GetEquAllRelXml ��ȡȫ����ʷ�汾��XML��

        /// <summary>
        /// ��ȡȫ����ʷ�汾��XML��
        /// </summary>
        /// <param name="lngID"></param>
        /// <param name="iBeginX"></param>
        /// <param name="iBeginY"></param>
        /// <param name="iWidth"></param>
        /// <param name="iHeight"></param>
        /// <returns></returns>
        public string GetEquAllHistoryXml(long lngID, int iBeginX, int iBeginY, int iWidth, int iHeight)
        {
            string strSQL = string.Empty;
            int iIndex = 0;  //��ǰѭ����λ��
            int iTotlIndex = 0; //��λ��

            int iWStep = 4000;
            int iHStep = 2200;

            int iTotalH = 3;   //�����������
            int iFX = 0;       // 0 ����  1 �ҵ���


            int iTmpBeginX = iBeginX;
            int iTmpBeginY = iBeginY;

            string sRootImage = "";
            long lngCategory = 0;
            string strName = "";
            int iTmp = 0;





            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<EQURELATION></EQURELATION>");

            OracleConnection cn = ConfigTool.GetConnection();
            strSQL = "SELECT * FROM Equ_Desk WHERE ID = " + lngID.ToString();
            DataTable dtEqu = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);

            string strVersionDesc = "";
            string strEditDesc = "";

            if (dtEqu.Rows.Count > 0)
            {
                lngCategory = long.Parse(dtEqu.Rows[0]["CatalogID"].ToString());
                strName = dtEqu.Rows[0]["Name"].ToString() + " �����û�:" + dtEqu.Rows[0]["CostomName"].ToString();

                #region
                //��ӵ�һ���豸

                XmlElement xmlEle = xmlDoc.CreateElement("EQU");

                XmlElement xmlRoot = xmlDoc.DocumentElement;

                xmlEle.SetAttribute("INDEX", iTotlIndex.ToString());
                iTotlIndex++;
                iIndex++;
                xmlEle.SetAttribute("LEFT", iBeginX.ToString());
                xmlEle.SetAttribute("TOP", iBeginY.ToString());
                xmlEle.SetAttribute("EQUID", lngID.ToString());
                xmlEle.SetAttribute("VERSION", dtEqu.Rows[0]["version"].ToString().Trim());
                xmlEle.SetAttribute("WIDTH", iWidth.ToString());
                xmlEle.SetAttribute("HEIGHT", iHeight.ToString());
                if (GetEquStatus(lngID.ToString()))
                {
                    xmlEle.SetAttribute("DISPLAYERRORICO", "1");
                }
                else
                {
                    xmlEle.SetAttribute("DISPLAYERRORICO", "0");//visible
                }

                XmlElement xmlTmp1 = xmlDoc.CreateElement("IMAGESRC");


                sRootImage = Equ_SubjectDP.GetSubjectImageUrl(lngCategory);

                sRootImage = (sRootImage == "" ? "../Images/P_desk.jpg" : sRootImage);

                xmlTmp1.InnerText = sRootImage;

                xmlEle.AppendChild(xmlTmp1);

                xmlTmp1 = xmlDoc.CreateElement("TEXT");
                xmlTmp1.InnerText = strName;
                xmlEle.AppendChild(xmlTmp1);

                xmlRoot.AppendChild(xmlEle);

                strSQL = @"SELECT nvl(flowid,0) as flowid,version,versiontime,costomname,updateuserid FROM equ_DeskHistory
                            WHERE  ID = " + lngID.ToString() + " ORDER BY version desc ";
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
                foreach (DataRow row in dt.Rows)
                {
                    strVersionDesc = "�汾:" + row["version"].ToString() + " �����û�:" + row["costomname"].ToString() + " " + row["versiontime"].ToString().Substring(0, row["versiontime"].ToString().IndexOf(" ") == -1 ? 0 : row["versiontime"].ToString().IndexOf(" "));
                    //���һ���豸��һ����
                    xmlEle = xmlDoc.CreateElement("EQU");

                    xmlRoot = xmlDoc.DocumentElement;

                    xmlEle.SetAttribute("INDEX", iTotlIndex.ToString());
                    iTotlIndex++;
                    iIndex++;
                    if (iFX == 0)
                    {      //����
                        if (iIndex <= iTotalH)
                        {
                            //����ת��
                            iTmpBeginY = iTmpBeginY;
                            iTmpBeginX = iTmpBeginX + iWStep;
                        }
                        else
                        {
                            iIndex = 1;

                            iTmpBeginX = iTmpBeginX;
                            iTmpBeginY = iTmpBeginY + iHStep;
                        }
                    }
                    else
                    {
                        //�ҵ���
                        if (iIndex <= iTotalH)
                        {
                            //����ת��
                            iTmpBeginY = iTmpBeginY;
                            iTmpBeginX = iTmpBeginX - iWStep;
                        }
                        else
                        {
                            iIndex = 1;

                            iTmpBeginX = iTmpBeginX;
                            iTmpBeginY = iTmpBeginY + iHStep;
                        }
                    }
                    xmlEle.SetAttribute("LEFT", iTmpBeginX.ToString());
                    xmlEle.SetAttribute("TOP", iTmpBeginY.ToString());
                    xmlEle.SetAttribute("EQUID", lngID.ToString());
                    xmlEle.SetAttribute("VERSION", row["version"].ToString().Trim());
                    xmlEle.SetAttribute("WIDTH", iWidth.ToString());
                    xmlEle.SetAttribute("HEIGHT", iHeight.ToString());
                    if (GetEquStatus(lngID.ToString()))
                    {
                        xmlEle.SetAttribute("DISPLAYERRORICO", "1");
                    }
                    else
                    {
                        xmlEle.SetAttribute("DISPLAYERRORICO", "0");//visible
                    }
                    xmlTmp1 = xmlDoc.CreateElement("IMAGESRC");


                    xmlTmp1.InnerText = sRootImage;

                    xmlEle.AppendChild(xmlTmp1);

                    xmlTmp1 = xmlDoc.CreateElement("TEXT");
                    xmlTmp1.InnerText = strVersionDesc;
                    xmlEle.AppendChild(xmlTmp1);

                    xmlRoot.AppendChild(xmlEle);

                    //�����

                    xmlEle = xmlDoc.CreateElement("LINK");


                    xmlTmp1 = xmlDoc.CreateElement("TEXT");


                    if (iFX == 0)
                    {
                        if (iIndex != 1)
                        {
                            iTmp = iTmpBeginX - iWStep + iWidth + 400;
                        }
                        else
                        {
                            iTmp = iTmpBeginX + iWidth / 2 + 200;
                        }
                    }
                    else
                    {
                        if (iIndex != 1)
                        {
                            iTmp = iTmpBeginX + iWidth + 400;
                        }
                        else
                        {
                            iTmp = iTmpBeginX + iWidth / 2 + 200;
                        }
                    }
                    xmlTmp1.SetAttribute("X", iTmp.ToString());


                    if (iIndex != 1)
                    {
                        iTmp = iTmpBeginY + iHeight / 2 - 300;
                    }
                    else
                    {
                        iTmp = iTmpBeginY - iHeight / 2;
                    }
                    xmlTmp1.SetAttribute("Y", iTmp.ToString());

                    if (row["flowid"].ToString() != "0")
                    {
                        xmlTmp1.InnerText = "�����";
                        xmlEle.SetAttribute("TEXTSAVE", "");
                    }
                    else
                    {
                        strEditDesc = Epower.DevBase.Organization.SqlDAL.UserDP.GetUserName(long.Parse(row["updateuserid"].ToString())) + " �޸�";
                        xmlEle.SetAttribute("TEXTSAVE", strEditDesc);
                    }
                    xmlEle.SetAttribute("FLOWID", row["flowid"].ToString().Trim());

                    xmlEle.AppendChild(xmlTmp1);

                    xmlTmp1 = xmlDoc.CreateElement("_DRAWSTYLE");
                    xmlTmp1.InnerText = "Solid";
                    xmlEle.AppendChild(xmlTmp1);

                    xmlTmp1 = xmlDoc.CreateElement("_ARROWDST");
                    xmlTmp1.InnerText = "Classic";
                    xmlEle.AppendChild(xmlTmp1);

                    xmlTmp1 = xmlDoc.CreateElement("EXTRAPOINTS");


                    if (iFX == 0)
                    {
                        //�� ����
                        XmlElement xmlTmp2 = xmlDoc.CreateElement("POINT");
                        if (iIndex != 1)
                        {
                            iTmp = iTmpBeginX - iWStep + iWidth;
                        }
                        else
                        {
                            iTmp = iTmpBeginX + iWidth / 2;
                        }
                        xmlTmp2.SetAttribute("X", iTmp.ToString());
                        if (iIndex != 1)
                        {
                            iTmp = iTmpBeginY + iHeight / 2;
                        }
                        else
                        {
                            iTmp = iTmpBeginY - iHStep + iHeight;
                        }
                        xmlTmp2.SetAttribute("Y", iTmp.ToString());

                        xmlTmp1.AppendChild(xmlTmp2);


                        //β�ڵ�
                        xmlTmp2 = xmlDoc.CreateElement("POINT");

                        if (iIndex != 1)
                        {
                            iTmp = iTmpBeginX;
                        }
                        else
                        {
                            iTmp = iTmpBeginX + iWidth / 2;
                        }
                        xmlTmp2.SetAttribute("X", iTmp.ToString());
                        if (iIndex != 1)
                        {
                            iTmp = iTmpBeginY + iHeight / 2;
                        }
                        else
                        {
                            iTmp = iTmpBeginY - 100;
                        }
                        xmlTmp2.SetAttribute("Y", iTmp.ToString());

                        xmlTmp1.AppendChild(xmlTmp2);

                    }
                    else
                    {
                        // �ҵ���  

                        XmlElement xmlTmp2 = xmlDoc.CreateElement("POINT");

                        if (iIndex != 1)
                        {
                            iTmp = iTmpBeginX + iWStep;
                        }
                        else
                        {
                            iTmp = iTmpBeginX + iWidth / 2;
                        }
                        xmlTmp2.SetAttribute("X", iTmp.ToString());
                        if (iIndex != 1)
                        {
                            iTmp = iTmpBeginY + iHeight / 2;
                        }
                        else
                        {
                            iTmp = iTmpBeginY - iHStep + iHeight;

                        }
                        xmlTmp2.SetAttribute("Y", iTmp.ToString());

                        xmlTmp1.AppendChild(xmlTmp2);


                        xmlTmp2 = xmlDoc.CreateElement("POINT");
                        if (iIndex != 1)
                        {
                            iTmp = iTmpBeginX + iWidth;
                        }
                        else
                        {
                            iTmp = iTmpBeginX + iWidth / 2;
                        }
                        xmlTmp2.SetAttribute("X", iTmp.ToString());
                        if (iIndex != 1)
                        {
                            iTmp = iTmpBeginY + iHeight / 2;
                        }
                        else
                        {
                            iTmp = iTmpBeginY - 100;
                        }
                        xmlTmp2.SetAttribute("Y", iTmp.ToString());

                        xmlTmp1.AppendChild(xmlTmp2);


                        xmlTmp1.AppendChild(xmlTmp2);

                    }




                    xmlEle.AppendChild(xmlTmp1);

                    xmlRoot.AppendChild(xmlEle);

                    if (iIndex == 1)
                    {
                        //�ı䷽��
                        iFX = (iFX == 0 ? 1 : 0);
                    }

                }

                #endregion

            }


            ConfigTool.CloseConnection(cn);


            return xmlDoc.InnerXml;

        }

        #endregion

        public bool GetEquStatus(string equId)
        {
            if (string.IsNullOrEmpty(equId) || (equId == "0"))
            {
                throw new ArgumentNullException("equId is null or zero.(" + equId + ")");
            }
            string sql = string.Format(@"SELECT EquipmentID  FROM CST_ISSUES I WHERE EquipmentID={0} AND 
                            EXISTS (SELECT * FROM ES_FLOW F WHERE F.FLOWID =I.FLOWID AND F.STATUS IN (20,30))", equId);

            OracleConnection cn = ConfigTool.GetConnection("SQLConnString");//��ȡ����
            try
            {

                int result = Convert.ToInt32(OracleDbHelper.ExecuteScalar(cn, CommandType.Text, sql));//ִ�в�ѯ
                return (result == 0) ? true : false;
            }
            catch
            {
                throw;
            }
            finally
            {

                ConfigTool.CloseConnection(cn);//�ر�����
            }

        }

        #region �Ƚ����°汾��Ŀǰ�����ֵ�Ƿ��б仯
        /// <summary>
        /// �Ƚ����°汾��Ŀǰ�����ֵ�Ƿ��б仯
        /// </summary>
        /// <param name="pEqu_DeskDP"></param>
        /// <param name="pOldEqu_DeskDP"></param>
        /// <returns></returns>
        private bool CompareVerEqu(Equ_DeskDP pEqu_DeskDP, Equ_DeskDP pOldEqu_DeskDP)
        {
            if (pEqu_DeskDP.ID.ToString() != pOldEqu_DeskDP.ID.ToString())
                return true;
            if (pEqu_DeskDP.Name.Trim() != pOldEqu_DeskDP.Name.Trim())
                return true;
            if (pEqu_DeskDP.Code.Trim() != pOldEqu_DeskDP.Code.Trim())
                return true;
            if (pEqu_DeskDP.Positions.Trim() != pOldEqu_DeskDP.Positions.Trim())
                return true;
            if (pEqu_DeskDP.SerialNumber.Trim() != pOldEqu_DeskDP.SerialNumber.Trim())
                return true;
            if (pEqu_DeskDP.Provide != pOldEqu_DeskDP.Provide)
                return true;
            if (pEqu_DeskDP.ProvideName.Trim() != pOldEqu_DeskDP.ProvideName.Trim())
                return true;
            if (pEqu_DeskDP.Breed.Trim() != pOldEqu_DeskDP.Breed.Trim())
                return true;
            if (pEqu_DeskDP.Model.Trim() != pOldEqu_DeskDP.Model.Trim())
                return true;
            if (pEqu_DeskDP.CatalogID != pOldEqu_DeskDP.CatalogID)
                return true;
            if (pEqu_DeskDP.CatalogName.Trim() != pOldEqu_DeskDP.CatalogName.Trim())
                return true;
            if (pEqu_DeskDP.EquStatusID != pOldEqu_DeskDP.EquStatusID)
                return true;
            if (pEqu_DeskDP.EquStatusName.Trim() != pOldEqu_DeskDP.EquStatusName.Trim())
                return true;
            if (pEqu_DeskDP.FullID.Trim() != pOldEqu_DeskDP.FullID.Trim())
                return true;
            if (pEqu_DeskDP.ConfigureInfo.Trim() != pOldEqu_DeskDP.ConfigureInfo.Trim())
                return true;
            if (pEqu_DeskDP.ConfigureValue.Trim() != pOldEqu_DeskDP.ConfigureValue.Trim())
                return true;
            if (pEqu_DeskDP.Costom != pOldEqu_DeskDP.Costom)
                return true;
            if (pEqu_DeskDP.ServiceBeginTime != pOldEqu_DeskDP.ServiceBeginTime)
                return true;
            if (pEqu_DeskDP.ServiceEndTime != pOldEqu_DeskDP.ServiceEndTime)
                return true;
            if (pEqu_DeskDP.ItemCode.Trim() != pOldEqu_DeskDP.ItemCode.Trim())
                return true;
            return false;
        }
        #endregion

        #region �����ʲ�ID��ȡ�ʲ������б�
        /// <summary>
        ///  �����ʲ�ID��ȡ�ʲ������б�
        /// </summary>
        /// <param name="strEquID"></param>
        /// <returns></returns>
        public static DataTable GetDeskPropsByDeskID(string strEquID)
        {
            string strSql = @"SELECT A.ID,A.FieldID,B.EquID,B.CHName 
                                FROM Equ_SchemaItems A
                          INNER JOIN EQU_deploy  B
                                  ON A.FieldID = B.FieldID 
                               WHERE B.EquID =" + strEquID;

            DataTable dt = CommonDP.ExcuteSqlTable(strSql);

            return dt;
        }
        #endregion

        #region �����ʲ�ID��ȡ��Ӧ�ʲ�����������xml
        /// <summary>
        /// �����ʲ�ID��ȡ��Ӧ�ʲ�����������xml
        /// </summary>
        /// <param name="strEquID"></param>
        /// <returns></returns>
        public static string GetSchemaXmlByEquID(string strEquID)
        {
            string strRet = string.Empty;
            string strSql = @"select ConfigureSchema from Equ_Category where CatalogID = (select CatalogID from Equ_desk where ID = " + strEquID + " )";

            DataTable dt = CommonDP.ExcuteSqlTable(strSql);

            if (dt != null && dt.Rows.Count > 0)
            {
                strRet = dt.Rows[0]["ConfigureSchema"].ToString();
            }

            return strRet;
        }
        #endregion

        #region �����ʲ�ID�õ�����Ӱ����ʲ��������
        /// <summary>
        ///  �����ʲ�ID�õ�����Ӱ����ʲ��������
        /// </summary>
        /// <param name="strEquID"></param>
        /// <returns></returns>
        public static string GetEquNamesByEquID(string strEquID)
        {
            string strRet = string.Empty;

            string strSql = @"select Name from Equ_Desk 
	                                 where ID in (
		                                select Equ_ID from Equ_Rel where RelID = " + strEquID + @"
		                                )";
            DataTable dt = CommonDP.ExcuteSqlTable(strSql);

            foreach (DataRow dr in dt.Rows)
            {
                strRet += dr["Name"].ToString() + ",";
            }
            return strRet;
        }
        #endregion

        #region �����ɾ��ʱ�Ĳ���

        /// <summary>
        /// �����ɾ��ʱ�Ĳ���
        /// ���ɾ��ʱ����ɾ������ĵ���ʱ�������
        /// </summary>
        /// <param name="lngFlowID"></param>
        /// <param name="EquId"></param>
        /// <param name="chanceId">sss </param>
        public static void DeleteEquChange(long lngFlowID, long EquId, long chanceId)
        {
            OracleConnection cn = ConfigTool.GetConnection("SQLConnString");
            cn.Open();
            OracleTransaction trans = cn.BeginTransaction();
            try
            {

                string strSQL = "Delete Equ_DeskChange WHERE FlowID =" + lngFlowID.ToString() + " and id=" + EquId.ToString();
                OracleDbHelper.ExecuteNonQuery(trans, CommandType.Text, strSQL);

                strSQL = "select * from equ_changeservicedetails  WHERE changeid = " + chanceId.ToString();
                OracleDataReader dr = OracleDbHelper.ExecuteReader(trans, CommandType.Text, strSQL);
                if (dr.Read() == true)
                {
                    dr.Close();

                    #region �����ʲ�״̬
                    strSQL = " UPDATE equ_desk SET request = 0 WHERE change_id=" + chanceId.ToString() + " AND ID in(SELECT equid FROM equ_changeservicedetails WHERE changeid = " + chanceId.ToString() + " AND equid=" + EquId.ToString() + ")";
                    OracleDbHelper.ExecuteNonQuery(trans, CommandType.Text, strSQL);
                    #endregion

                    #region ɾ�������ϸ

                    strSQL = " delete equ_changeservicedetails  WHERE changeid = " + chanceId.ToString() + " AND equid=" + EquId.ToString();
                    OracleDbHelper.ExecuteNonQuery(trans, CommandType.Text, strSQL);

                    #endregion

                }
                if (dr.IsClosed == false)
                {
                    dr.Close();
                }
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

        #region ��ȡ�����ʱ����Ϣ

        /// <summary>
        /// ��ʱ����Ϣ
        /// </summary>
        /// <param name="lngFlowID"></param>
        /// <returns></returns>
        public static DataTable getEquChange(long lngFlowID)
        {
            DataTable dt = null;
            OracleConnection cn = ConfigTool.GetConnection("SQLConnString");
            cn.Open();
            try
            {
                string strSQL = "select * from  Equ_DeskChange where flowid =" + lngFlowID;
                dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            }
            catch
            {
                throw;
            }
            finally
            {
                ConfigTool.CloseConnection(cn);
            }
            return dt;
        }

        #endregion

        #region �ʲ�����

        /// <summary>
        /// �ʲ�����
        /// </summary>
        /// <param name="equId">����ID</param>
        /// <param name="changeId">���ID</param>
        public string AssetLock(long equId, long changeId)
        {
            string strSQL = string.Empty;//SQL���
            int result = 0;
            ChangeDealDP cdd = new ChangeDealDP();//ʵ����

            OracleConnection cn = ConfigTool.GetConnection("SQLConnString");//��ȡ����
            try
            {
                strSQL = "SELECT COUNT(1) FROM equ_desk WHERE request = 1 AND ID=" + equId;//�ж��Ƿ��Ѿ�����
                result = Convert.ToInt32(OracleDbHelper.ExecuteScalar(cn, CommandType.Text, strSQL));//ִ�в�ѯ
                if (result == 0)
                {
                    strSQL = "UPDATE equ_desk SET request = 1,change_Id=" + changeId + " WHERE ID=" + equId;//�������
                    result = OracleDbHelper.ExecuteNonQuery(cn, CommandType.Text, strSQL);//ִ������
                    return "1";
                }
                else
                {
                    DataTable dt = cdd.GetTemporaryChangeByChangeId(changeId, equId);//�ʲ�����
                    if (dt.Rows.Count > 0)
                    {
                        string ChangeNO = dt.Rows[0]["ChangeNO"].ToString();
                        //string strMessage = "������������޸Ĵ��ʲ�!����[" + cdd.GetChangeNo(changeId, equId) + "]!";
                        string strMessage = "������������޸Ĵ��ʲ�!����[" + ChangeNO + "]!";
                        return strMessage;//�ʲ�������
                    }
                    return "1";
                }
            }
            catch
            {
                //д����־
                return "���ݿ��������";
            }
            finally
            {

                ConfigTool.CloseConnection(cn);//�ر�����
            }

        }

        #endregion

        #region �ʲ��Ƿ�����

        /// <summary>
        /// �ʲ��Ƿ�����
        /// </summary>
        /// <param name="equId">�ʲ�ID</param>
        /// <param name="changeId">���ID</param>
        public string AssetIsLock(long equId, long changeId)
        {
            string strSQL = string.Empty;//SQL���
            int result = 0;
            ChangeDealDP cdd = new ChangeDealDP();//ʵ����

            OracleConnection cn = ConfigTool.GetConnection("SQLConnString");//��ȡ����
            try
            {
                strSQL = "SELECT COUNT(1) FROM equ_desk WHERE request = 1 AND ID=" + equId;//�ж��Ƿ��Ѿ�����
                result = Convert.ToInt32(OracleDbHelper.ExecuteScalar(cn, CommandType.Text, strSQL));//ִ�в�ѯ
                if (result != 0)
                {
                    DataTable dt = cdd.GetTemporaryChangeByChangeId(changeId, equId);//�ʲ�����
                    if (dt.Rows.Count > 0)
                    {
                        string ChangeNO = dt.Rows[0]["ChangeNO"].ToString();
                        //string strMessage = "������������޸Ĵ��ʲ�!����[" + cdd.GetChangeNo(changeId, equId) + "]!";
                        string strMessage = "������������޸Ĵ��ʲ�!����[" + ChangeNO + "]!";
                        return strMessage;//�ʲ�������
                    }
                    return "��ǰ�������";
                }
                return "û������";
            }
            catch
            {
                //д����־
                return "���ݿ��������";
            }
            finally
            {

                ConfigTool.CloseConnection(cn);//�ر�����
            }

        }

        #endregion

        #region ��ȡ�ʲ���ط�����ͼ��
        /// <summary>
        /// ��ȡ�ʲ���ط�����ͼ��
        /// </summary>
        /// <param name="strMastCust"></param>
        /// <returns></returns>
        public static DataTable GetSM(string strMastCust)
        {
            string strWhere = string.Empty;

            if (strMastCust == "")
            {
                strWhere = "";
            }
            else
            {
                strWhere = " and C.MastCustID = " + StringTool.SqlQ(strMastCust);
            }

            string strSql = @"--��������
                            select '��������' as Title,count(1) as counts,'EquA' as Types
                            from Equ_Desk A,Br_ECustomer C
                            where A.Costom = C.ID and C.deleted = 0 and to_char(A.RegTime,'yyyy-MM') = to_char(sysdate,'yyyy-MM')" + strWhere + @"
                            union all
                            --���±���С���ʼ���ʱ��Ϊ���£������ϸ�����ӱ����ʼʱ��BeginChangeTime�ֶΡ�
                            select '���±����'as Title,count(1) as counts,'EquB' as Types
                              from Equ_Desk A,Br_ECustomer C
                             where A.Costom = C.ID
                               and A.Deleted = 0 and C.deleted = 0 
                               and A.Request = 1 
                               and A.ID in (
	                            select EquID from Equ_Changeservicedetails
		                                where to_char(BeginChangeTime,'yyyy-MM') = to_char(sysdate,'yyyy-MM')
	                                          and ChangeStatus = 0		
	                            )" + strWhere + @"
                            union all
                            --�����ѱ���������ϸ���б��ʱ��Ϊ���¡�
                            select '�����ѱ��'as Title,count(1) as counts,'EquC' as Types
                              from Equ_Desk A,Br_ECustomer C
                             where A.Costom = C.ID
                               and A.Deleted = 0 and C.deleted = 0 
                               and A.Request = 1 
                               and A.ID in (
	                            select EquID from Equ_Changeservicedetails
		                                where to_char(BeginChangeTime,'yyyy-MM') = to_char(sysdate,'yyyy-MM')
	                                          and ChangeStatus = 1	
	                            )" + strWhere + @"
                            union all
                            --���·������ϡ��������ϵ�ʱ��Ϊ���£����¼����ĵǵ�ʱ��Ϊ���¡�
                            select '���·�������' as Title,count(1) as counts,'EquD' as Types
                            from Equ_Desk AA,Br_ECustomer C,
                            (select A.* from Cst_Issues A,es_Flow B
	                            where A.FlowID = B.FlowID 
		                            and B.FlowModelID in (select FlowModelID from es_FlowModel where FlowBusID = 888)
		                            and to_char(A.RegSysDate,'yyyy-MM') = to_char(sysdate,'yyyy-MM')
                            ) BB
                            where AA.Costom = C.ID and C.ID = BB.CustID and C.deleted = 0" + strWhere + @"
                            union all
                            --����δ������[δ�߱�������޸ĵģ�ȡ�ʲ���ʷ��İ汾ʱ���жϵ���]
                            select '����δ����������'as Title,count(1) as counts,'EquE' as Types
                              from Equ_Desk A,Equ_DeskHistory B,Br_ECustomer C
                             where A.ID = B.ID and A.Costom = C.ID  
                                   and A.Deleted = 0 and C.deleted = 0
                                   and to_char(B.VersionTime,'yyyy-MM') = to_char(sysdate,'yyyy-MM') 
                                   and B.FlowID = 0" + strWhere;

            DataTable dt = CommonDP.ExcuteSqlTable(strSql);

            return dt;
        }
        #endregion

        #region �ʲ������ӵ㿪�鿴����
        /// <summary>
        /// �ʲ������ӵ㿪�鿴����
        /// </summary>
        /// <param name="strMastCust"></param>
        /// <param name="strType"></param>
        /// <param name="pagesize"></param>
        /// <param name="pageindex"></param>
        /// <param name="rowcount"></param>
        /// <returns></returns>
        public static DataTable GetSMDetail(string strMastCust, string strType, int pagesize, int pageindex, ref int rowcount)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            DataTable dt = new DataTable();
            string strWhere = " 1=1 ";
            string strSql = string.Empty;

            if (strMastCust != "")
            {
                strWhere += " and MastCustID = " + StringTool.SqlQ(strMastCust);
            }

            if (strType == "EquA")
            {
                //��������
                dt = OracleDbHelper.ExecuteDataTable(cn, "v_EquA", "*", " ORDER BY ID DESC", pagesize, pageindex, strWhere, ref rowcount);
            }
            else if (strType == "EquB")
            {
                //���±����
                dt = OracleDbHelper.ExecuteDataTable(cn, "v_EquB", "*", " ORDER BY ID DESC", pagesize, pageindex, strWhere, ref rowcount);
            }
            else if (strType == "EquC")
            {
                //�����ѱ��
                dt = OracleDbHelper.ExecuteDataTable(cn, "v_EquC", "*", " ORDER BY ID DESC", pagesize, pageindex, strWhere, ref rowcount);
            }
            else if (strType == "EquD")
            {
                //���·�������
                dt = OracleDbHelper.ExecuteDataTable(cn, "v_EquD", "*", " ORDER BY ID DESC", pagesize, pageindex, strWhere, ref rowcount);
            }
            else if (strType == "EquE")
            {
                //����δ������
                dt = OracleDbHelper.ExecuteDataTable(cn, "v_EquE", "*", " ORDER BY ID DESC", pagesize, pageindex, strWhere, ref rowcount);
            }
            else
            {
                strSql = @"--�޼�¼
                                select A.*
                                  from Equ_Desk A where 1 = 2";
            }

            return dt;
        }
        #endregion

        #region ��ȡ�����ʲ��������б�����
        /// <summary>
        /// ��ȡ�����ʲ��������б�����
        /// </summary>
        public static DataTable GetInitEqus()
        {
            string strSql = "select * from Equ_Desk where deleted = 0";

            DataTable dt = CommonDP.ExcuteSqlTable(strSql);
            return dt;
        }
        #endregion

        #region �����ʲ�ID�õ��ʲ�����
        /// <summary>
        /// �����ʲ�ID�õ��ʲ�����
        /// </summary>
        public static string GetEquNameByID(string strEquID)
        {
            string strRet = string.Empty;
            string strSql = "select * from Equ_Desk where deleted = 0 and ID = " + strEquID;

            DataTable dt = CommonDP.ExcuteSqlTable(strSql);
            if (dt != null && dt.Rows.Count > 0)
            {
                strRet = dt.Rows[0]["Name"].ToString();
            }
            return strRet;
        }
        #endregion

        #region �����ʲ�ID�õ��ʲ����
        /// <summary>
        /// �����ʲ�ID�õ��ʲ����
        /// </summary>
        public static string GetEquCodeByID(string strEquID)
        {
            string strRet = string.Empty;
            string strSql = "select * from Equ_Desk where deleted = 0 and ID = " + strEquID;

            DataTable dt = CommonDP.ExcuteSqlTable(strSql);
            if (dt != null && dt.Rows.Count > 0)
            {
                strRet = dt.Rows[0]["code"].ToString();
            }
            return strRet;
        }
        #endregion

        #region �����ʲ�ID�õ��ʲ���չ����
        /// <summary>
        /// �����ʲ�ID�õ��ʲ���չ����
        /// </summary>
        /// <param name="strEquID"></param>
        /// <returns></returns>
        public static string GetPropsByEquID(string strEquID)
        {
            string strRet = string.Empty;           //����sql��

            string strSql = "select * From EQU_deploy where EquID = " + strEquID;

            DataTable dt = CommonDP.ExcuteSqlTable(strSql);

            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        strRet += row["CHName"].ToString() + ":" + row["Value"].ToString() + ";";
                    }
                }
            }

            return strRet;
        }
        #endregion

        #region ����ʲ���������Ϣ����XML����ʾ
        /// <summary>
        /// ����ʲ���������Ϣ����XML����ʾ
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


            //��Ӹ�����Ϣ  
            strSQL = "SELECT a.FileID,a.FileName,a.SufName,a.Status,a.upTime,a.upUserID,b.Name as upUserName ,nvl(a.requstFileId,'') requstFileId " +
                 " FROM Equ_Attachment a,Ts_User b " +
                 " WHERE a.upUserID = b.UserID AND a.Status <>" + (int)e_FileStatus.efsDeleted +
                 "		AND a.KBID =" + lngKBID.ToString() + " AND nvl(a.deleted,0)=" + (int)e_Deleted.eNormal + " ORDER BY a.FileID";

            OracleConnection cn = ConfigTool.GetConnection("SQLConnString");

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
                    xmlElSub.SetAttribute("upTime", dr.GetDateTime(4).ToLongDateString());
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
        /// <summary>
        /// ����ʲ���������Ϣ����XML����ʾ
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


            //��Ӹ�����Ϣ  
            strSQL = "SELECT a.FileID,a.FileName,a.SufName,a.Status,a.upTime,a.upUserID,b.Name as upUserName ,nvl(a.requstFileId,'') requstFileId,nvl(a.MonthPath,'')  MonthPath " +
                 " FROM Equ_Attachment a,Ts_User b " +
                 " WHERE a.upUserID = b.UserID AND a.Status <>" + (int)e_FileStatus.efsDeleted +
                 "		AND a.KBID =" + lngKBID.ToString() + " AND nvl(a.deleted,0)=" + (int)e_Deleted.eNormal + " ORDER BY a.FileID";

            OracleConnection cn = ConfigTool.GetConnection("SQLConnString");

            try
            {
                dr = OracleDbHelper.ExecuteReader(cn, CommandType.Text, strSQL);
                while (dr.Read())
                {
                    strMonthPath = dr["MonthPath"].ToString();

                    xmlElSub = xmlDoc.CreateElement("Attachment");
                    xmlElSub.SetAttribute("FileID", ((long)dr.GetDecimal(0)).ToString());
                    xmlElSub.SetAttribute("FileName", dr.GetString(1));
                    xmlElSub.SetAttribute("SufName", dr.GetString(2));
                    xmlElSub.SetAttribute("Status", dr.GetInt32(3).ToString());
                    xmlElSub.SetAttribute("upTime", dr.GetDateTime(4).ToLongDateString());
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

        #region ȡ��������
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lngFileID"></param>
        /// <returns></returns>
        public static string GetAttachmentName(long lngFileID)
        {
            string strFileName = string.Empty;
            string strSQL = string.Empty;
            OracleConnection cn = ConfigTool.GetConnection("SQLConnString");

            try
            {
                strSQL = "SELECT FileName FROM Equ_Attachment WHERE FileID=" + lngFileID.ToString();
                OracleDataReader dr = OracleDbHelper.ExecuteReader(cn, CommandType.Text, strSQL);
                while (dr.Read())
                {
                    strFileName = dr.GetString(0);
                }
                dr.Close();
            }
            finally { ConfigTool.CloseConnection(cn); }

            return strFileName;
        }
        /// <summary>
        /// ȡ��������
        /// </summary>
        /// <param name="lngFileID"></param>
        /// <param name="strMonthPath"></param>
        /// <returns></returns>
        public static string GetAttachmentName(long lngFileID, ref string strMonthPath)
        {
            string strFileName = string.Empty;
            string strSQL = string.Empty;
            OracleConnection cn = ConfigTool.GetConnection();
            strSQL = "SELECT FileName,nvl(MonthPath,'')  MonthPath FROM Equ_Attachment WHERE FileID=" + lngFileID.ToString();
            OracleDataReader dr = OracleDbHelper.ExecuteReader(cn, CommandType.Text, strSQL);
            while (dr.Read())
            {
                strFileName = dr.GetString(0);
                strMonthPath = dr.GetString(1);
            }
            dr.Close();
            ConfigTool.CloseConnection(cn);
            return strFileName;
        }
        #endregion

        #region ���渽��
        /// <summary>
        /// ���渽����Ϣ���洢����
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
                    //��ȡ��ʱ��·��
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
                            //�������� ��1����Ӽ�¼�����µ�����ж��Ƿ���ڼ�¼�����ܲ�����ͬʱ�����ڽ���ɾ����  2������ʱĿ¼�ж�Ӧ���ļ����벢�Ƶ��ļ��洢Ŀ¼��
                            count++;
                            strSQL = "SELECT FileID,nvl(filepath,'') FROM Equ_Attachment WHERE FileID=" + lngFileID.ToString();
                            dr = OracleDbHelper.ExecuteReader(trans, CommandType.Text, strSQL);
                            while (dr.Read())
                            {
                                blnNew = false;
                                strOldFilePath = dr.GetString(1);
                            }
                            dr.Close();

                            if (blnNew)
                            {
                                strSQL = "INSERT INTO Equ_Attachment (FileID,KBID,FileName,SufName,filepath,Status,upTime,upUserID,MonthPath,deleted,deleteTime,requstFileId) " +
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
                                strSQL = "UPDATE Equ_Attachment SET upTime = sysdate,upUserID =" + lngupUserID.ToString() + "," +
                                             " filepath = " + StringTool.SqlQ(strFileCatalog) +
                                            " WHERE FileID=" + lngFileID.ToString();
                            }
                            OracleDbHelper.ExecuteNonQuery(trans, CommandType.Text, strSQL);

                            //2������ʱĿ¼�ж�Ӧ���ļ����벢�Ƶ��ļ��洢Ŀ¼��,��ɾ��
                            if (PreProcessForAttachment(strTmpCatalog, strFileCatalog, strTmpFileN))
                            {
                                //����û�бȽϺõİ취��������ʱ����ʱ�ļ������ԣ�ȡ������  2003-06-26 ***
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
                                //ɾ������
                                if (File.Exists(strOldFileN))
                                    File.Delete(strOldFileN);
                            }


                            break;

                        case e_FileStatus.efsDeleted:
                            strSQL = "SELECT nvl(filepath,'') FROM Equ_Attachment WHERE FileID=" + lngFileID.ToString();
                            dr = OracleDbHelper.ExecuteReader(trans, CommandType.Text, strSQL);
                            while (dr.Read())
                            {
                                strOldFilePath = dr.GetString(0);
                            }
                            dr.Close();
                            //ɾ����¼
                            strSQL = "update Equ_Attachment set deleted=1,deletetime=sysdate,requstFileId=" + StringTool.SqlQ(reqestId.ToString()) + " WHERE FileID =" + lngFileID.ToString();
                            OracleDbHelper.ExecuteNonQuery(trans, CommandType.Text, strSQL);
                            break;
                        default:
                            count++;
                            break;
                    }

                    //�������ɾ����ʱ�ļ�  ***  �������  ****
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

            //�������̸���״̬
            if (count == 0)
            {
                strSQL = "UPDATE Equ_desk SET Attachment =" + (int)e_IsTrue.fmFalse + " WHERE ID=" + lngKBID.ToString();
            }
            else
            {
                strSQL = "UPDATE Equ_desk SET Attachment =" + (int)e_IsTrue.fmTrue + " WHERE ID=" + lngKBID.ToString();
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

        #endregion

        #region ���ݱ�ID�鿴�����Ƿ����
        /// <summary>
        /// ���ݱ�ID�鿴�����Ƿ����
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
                string strSQL = "SELECT * FROM Equ_Attachment where FileID=" + FileId.ToString();
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

        #region ȡ����ɾ�������̸���
        /// <summary>
        /// ȡ����ɾ�������̸���
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
                string strSQL = @"select OA.KBID as FlowId,OA.*,U.name as UpuserName,OldOA.FileName as oldFiledName from Equ_Attachment OA
                left join ts_user  U on OA.upUserID=U.userid 
                left join Equ_Attachment OldOA  on OA.requstFileId=to_char(OldOA.FileId)
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

        #region ��ȡ���¹��ĸ���
        /// <summary>
        /// ��ȡ���¹��ĸ���
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
                   " FROM Equ_Attachment a,Ts_User b " +
                   " WHERE a.upUserID = b.UserID " +
                   "		AND a.KBID =" + lngKBID.ToString() + " AND a.requstFileId =" + requstFileid.ToString() + "  ORDER BY a.FileID";
                }
                else
                {
                    strSQL = "SELECT a.deleteTime,a.KBID as FlowId,a.FileID,a.FileName,a.SufName,a.Status,a.upTime,a.upUserID,b.Name as upUserName ,nvl(a.requstFileId,'') requstFileId " +
                     " FROM Equ_Attachment a,Ts_User b " +
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


        #region ������չ���FieldID
        /// <summary>
        /// ������չ���FieldID
        /// </summary>
        /// <param name="trans">�������</param>
        /// <param name="lngFieldID">��չ����</param>
        /// <param name="lngOFieldID">����չ����</param>
        /// <returns></returns>
        public static void UpdateExFieldID(OracleTransaction trans, long lngFieldID, long lngOFieldID)
        {
            string strSQL = String.Format(@" UPDATE equ_deploy SET fieldid = {0} WHERE fieldid = {1} ", lngFieldID, lngOFieldID);
            OracleDbHelper.ExecuteNonQuery(trans, CommandType.Text, strSQL);
        }
        #endregion
    }
}

