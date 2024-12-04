/*******************************************************************
 * 版权所有：
 * Description：自定义桌面数据处理类
 * 
 * 
 * Create By  ：
 * Create Date：2007-11-26
 * *****************************************************************/
using System;
using System.Data;
using System.Data.OracleClient;
using Epower.DevBase.BaseTools;
using Epower.ITSM.Base;
using Epower.DevBase.Organization.SqlDAL;
using Epower.DevBase.Organization.Base;
using System.Text;
using System.Collections;

namespace Epower.ITSM.SqlDAL
{
    /// <summary>
    /// 
    /// </summary>
    public class Ea_DefineMainPageDP
    {
        /// <summary>
        /// 
        /// </summary>
        public Ea_DefineMainPageDP()
        { }

        private static string Key = "Ea_DefineMainPageDP";

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

        #region iOrder
        /// <summary>
        ///
        /// </summary>
        private Int32 miOrder;
        public Int32 iOrder
        {
            get { return miOrder; }
            set { miOrder = value; }
        }
        #endregion

        #region LeftOrRight
        /// <summary>
        ///
        /// </summary>
        private Int32 mLeftOrRight;
        public Int32 LeftOrRight
        {
            get { return mLeftOrRight; }
            set { mLeftOrRight = value; }
        }
        #endregion

        #region Title
        /// <summary>
        ///
        /// </summary>
        private String mTitle = string.Empty;
        public String Title
        {
            get { return mTitle; }
            set { mTitle = value; }
        }
        #endregion

        #region ImageUrl
        /// <summary>
        ///
        /// </summary>
        private String mImageUrl = string.Empty;
        public String ImageUrl
        {
            get { return mImageUrl; }
            set { mImageUrl = value; }
        }
        #endregion

        #region MoreUrl
        /// <summary>
        ///
        /// </summary>
        private String mMoreUrl = string.Empty;
        public String MoreUrl
        {
            get { return mMoreUrl; }
            set { mMoreUrl = value; }
        }
        #endregion

        #region Url
        /// <summary>
        ///
        /// </summary>
        private String mUrl = string.Empty;
        public String Url
        {
            get { return mUrl; }
            set { mUrl = value; }
        }
        #endregion

        #region IframeHeight
        /// <summary>
        ///
        /// </summary>
        private String mIframeHeight = string.Empty;
        public String IframeHeight
        {
            get { return mIframeHeight; }
            set { mIframeHeight = value; }
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

        #region DefaultVisible
        /// <summary>
        ///
        /// </summary>
        private Int32 mDefaultVisible;
        public Int32 DefaultVisible
        {
            get { return mDefaultVisible; }
            set { mDefaultVisible = value; }
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

        #endregion

        #region GetReCorded
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lngID"></param>
        /// <returns>Ea_DefineMainPageDP</returns>
        public Ea_DefineMainPageDP GetReCorded(long lngID)
        {
            Ea_DefineMainPageDP ee = new Ea_DefineMainPageDP();
            string strSQL = string.Empty;
            OracleConnection cn = ConfigTool.GetConnection();

            try {
                strSQL = "SELECT * FROM Ea_DefineMainPage WHERE ID = " + lngID.ToString();
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
                
                foreach (DataRow dr in dt.Rows)
                {
                    ee.ID = Decimal.Parse(dr["ID"].ToString());
                    ee.iOrder = Int32.Parse(dr["iOrder"].ToString());
                    ee.LeftOrRight = Int32.Parse(dr["LeftOrRight"].ToString());
                    ee.Title = dr["Title"].ToString();
                    ee.ImageUrl = dr["ImageUrl"].ToString();
                    ee.MoreUrl = dr["MoreUrl"].ToString();
                    ee.Url = dr["Url"].ToString();
                    ee.IframeHeight = dr["IframeHeight"].ToString();
                    ee.Deleted = Int32.Parse(dr["Deleted"].ToString());
                    ee.DefaultVisible = Int32.Parse(dr["DefaultVisible"].ToString());
                    ee.RegUserID = Decimal.Parse(dr["RegUserID"].ToString());
                    ee.RegUserName = dr["RegUserName"].ToString();
                    ee.RegTime = dr["RegTime"].ToString() == string.Empty ? DateTime.MinValue : DateTime.Parse(dr["RegTime"].ToString());
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
                strSQL = "SELECT * FROM Ea_DefineMainPage Where 1=1 And Deleted=0 ";
                strSQL += sWhere;
                strSQL += sOrder;
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
                
                return dt;
            }
            finally { ConfigTool.CloseConnection(cn); }
        }
        #endregion

        #region InsertRecorded
        /// <summary>
        /// 
        /// </summary>
        /// <param name=pEa_DefineMainPageDP></param>
        public void InsertRecorded(Ea_DefineMainPageDP pEa_DefineMainPageDP)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            string strSQL = string.Empty;
            string strID = "0";
            try
            {
                strID = EpowerGlobal.EPGlobal.GetNextID("Ea_DefineMainPageID").ToString();
                pEa_DefineMainPageDP.ID = decimal.Parse(strID);
                strSQL = @"INSERT INTO Ea_DefineMainPage(
									ID,
									iOrder,
									LeftOrRight,
									Title,
									ImageUrl,
									MoreUrl,
									Url,
									IframeHeight,
									Deleted,
									DefaultVisible,
									RegUserID,
									RegUserName,
									RegTime
					)
					VALUES( " +
                            strID.ToString() + "," +
                            pEa_DefineMainPageDP.iOrder.ToString() + "," +
                            pEa_DefineMainPageDP.LeftOrRight.ToString() + "," +
                            StringTool.SqlQ(pEa_DefineMainPageDP.Title) + "," +
                            StringTool.SqlQ(pEa_DefineMainPageDP.ImageUrl) + "," +
                            StringTool.SqlQ(pEa_DefineMainPageDP.MoreUrl) + "," +
                            StringTool.SqlQ(pEa_DefineMainPageDP.Url) + "," +
                            StringTool.SqlQ(pEa_DefineMainPageDP.IframeHeight) + "," +
                            pEa_DefineMainPageDP.Deleted.ToString() + "," +
                            pEa_DefineMainPageDP.DefaultVisible.ToString() + "," +
                            pEa_DefineMainPageDP.RegUserID.ToString() + "," +
                            StringTool.SqlQ(pEa_DefineMainPageDP.RegUserName) + "," +
                            (pEa_DefineMainPageDP.RegTime == DateTime.MinValue ? " null " : "to_date(" + StringTool.SqlQ(pEa_DefineMainPageDP.RegTime.ToString()) + ",'yyyy-MM-dd HH24:mi:ss')") +
                    ")";

                OracleDbHelper.ExecuteNonQuery(cn, CommandType.Text, strSQL);
                System.Web.HttpRuntime.Cache.Remove(Key);
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
        /// <param name=pEa_DefineMainPageDP></param>
        public void UpdateRecorded(Ea_DefineMainPageDP pEa_DefineMainPageDP)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            string strSQL = string.Empty;
            try
            {
                strSQL = @"UPDATE Ea_DefineMainPage Set " +
                                                        " iOrder = " + pEa_DefineMainPageDP.iOrder.ToString() + "," +
                            " LeftOrRight = " + pEa_DefineMainPageDP.LeftOrRight.ToString() + "," +
                            " Title = " + StringTool.SqlQ(pEa_DefineMainPageDP.Title) + "," +
                            " ImageUrl = " + StringTool.SqlQ(pEa_DefineMainPageDP.ImageUrl) + "," +
                            " MoreUrl = " + StringTool.SqlQ(pEa_DefineMainPageDP.MoreUrl) + "," +
                            " Url = " + StringTool.SqlQ(pEa_DefineMainPageDP.Url) + "," +
                            " IframeHeight = " + StringTool.SqlQ(pEa_DefineMainPageDP.IframeHeight) + "," +
                            " Deleted = " + pEa_DefineMainPageDP.Deleted.ToString() + "," +
                            " DefaultVisible = " + pEa_DefineMainPageDP.DefaultVisible.ToString() + "," +
                            " RegUserID = " + pEa_DefineMainPageDP.RegUserID.ToString() + "," +
                            " RegUserName = " + StringTool.SqlQ(pEa_DefineMainPageDP.RegUserName) + "," +
                            " RegTime = " + (pEa_DefineMainPageDP.RegTime == DateTime.MinValue ? " null " : "to_date(" + StringTool.SqlQ(pEa_DefineMainPageDP.RegTime.ToString()) + ",'yyyy-MM-dd HH24:mi:ss')") +
                                " WHERE ID = " + pEa_DefineMainPageDP.ID.ToString();

                OracleDbHelper.ExecuteNonQuery(cn, CommandType.Text, strSQL);
                System.Web.HttpRuntime.Cache.Remove(Key);
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
                string strSQL = "Update Ea_DefineMainPage Set Deleted=1  WHERE ID =" + lngID.ToString();
                OracleDbHelper.ExecuteNonQuery(cn, CommandType.Text, strSQL);
                System.Web.HttpRuntime.Cache.Remove(Key);
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


        #region 判断重复值 CheckIsRepeat
        /// <summary>
        /// 
        /// </summary>
        /// <param name="iOrder"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        public bool CheckIsRepeat(int iOrder,decimal ID)
        {
            bool sReturn = false;
            string strSQL = string.Empty;
            OracleConnection cn = ConfigTool.GetConnection();

            try
            {
                strSQL = "SELECT ID FROM Ea_DefineMainPage Where 1=1 And Deleted=0 And iOrder=" + iOrder.ToString();
                if (ID != 0)
                    strSQL += " and ID<>" + ID;
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
                if (dt.Rows.Count > 0)
                    sReturn = true;
            
                return sReturn;
            }
            finally { ConfigTool.CloseConnection(cn); }
        }
        #endregion 

        #region GetDataTable
        /// <summary>
        /// 
        /// </summary>
        /// <param name="swhere"></param>
        /// <returns></returns>
        public DataTable GetDataTable(string swhere)
        {
            DataTable dt = new DataTable();
            if (System.Web.HttpRuntime.Cache[Key] == null)
            {
                string strSQL = string.Empty;
                OracleConnection cn = ConfigTool.GetConnection();
                strSQL = "SELECT * FROM Ea_DefineMainPage Where 1=1 And Deleted=0 ";
                //strSQL += swhere;
                //strSQL += " Order by iOrder";
                try { dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL); }
                finally { ConfigTool.CloseConnection(cn); }                
                //插入缓存
                System.Web.HttpRuntime.Cache.Insert(Key, dt);
            }
            else
            {
                dt = (DataTable)System.Web.HttpRuntime.Cache[Key];
            }
            DataRow[] datarow = dt.Select(swhere, " iOrder");

            DataTable pDataTable = new DataTable("Ea_DefineMainPage1a");
            pDataTable = dt.Copy();
            pDataTable.Rows.Clear();
            Hashtable htAllRights = (Hashtable)System.Web.HttpContext.Current.Session["UserAllRights"];
            for (int i = 0; i < datarow.Length;i++ )
            {
                if (datarow[i]["operateid"].ToString() != "0" && datarow[i]["operateid"].ToString() != "")
                {
                    long OperatorID = long.Parse(datarow[i]["operateid"].ToString());
                    RightEntity re = (RightEntity)htAllRights[OperatorID];
                    if (re != null)
                    {
                        if (re.CanRead == true)
                        {
                            pDataTable.Rows.Add(datarow[i].ItemArray);
                        }
                    }

                }
                else
                {
                    pDataTable.Rows.Add(datarow[i].ItemArray);
                }
            }

            return pDataTable;
        }
        #endregion

        #region 生成主页 CreateMainPage
        /// <summary>
        /// 生成主页
        /// </summary>
        /// <param name="sMainPageSet"></param>
        /// <param name="sExpand"> 折叠了的内容，1，2</param>
        /// <param name="sleft"></param>
        /// <param name="sright"></param>
        /// <param name="re"></param>
        public void CreateMainPage(string sMainPageSet, string sExpand, ref string sleft, ref string sright, RightEntity re)
        {
            string sdivTemplete = @"<div id='module_@i'  style='position: relative;padding-bottom:10px;'>
                                    <table cellspacing='0' width='100%' cellpadding='1' border='0' class='bian' >
                                     <tr class='sy_lmbg'>
                                       <td id='module_@i_Expand'> 
                            <img class='icon' style='cursor:hand' id='ImgShowControl_@i' onclick='ShowTable(this);' height='16' src='../Images/@expandgif
                    width='16' /></td>
                                        <td id='module_@i_head' width='58%'> 
                                         @Title</td>
                                       <td id='module_@i_more' align='right' width='42%'>
                                         <div id='module_@i_op' style='float:left;display:none;'>";

            if (re != null)
            {
                if (re.CanRead == true)
                {
                    sdivTemplete += @"<a id='module_@i_url' href='#' onclick='_del(@i);'><img id='img_@i' border='0' src='../images/x.gif'></a>";
                }
            }

            sdivTemplete += @"</div>
                                         <div style='float:right;@moredivsty'><a href='@moreurl'> <img src='../Images/more-2.gif' border='0' align='absmiddle'></a>&nbsp;</div>
                                       </td>
                                     </tr>
                                     <tr>
                                      
                                        <td colspan=3 height='10' class='list'>
                                            <div id='divShowControl_@i' @hidden >
                                            <iframe id='iframe_@i' src='@url' width='100%' height='@height' frameborder='no' scrolling='no'></iframe>
                                           </div>
                                        </td>
                                    </tr>
                                    </table>
                                </div>";

            string swhere = string.Empty;
            if (sMainPageSet != string.Empty)  //设置了值
            {
                string[] arr = sMainPageSet.Split(':');
                //生成左边
                if (arr.Length > 1)
                {
                    swhere = "  iOrder In (" + arr[0].ToString().Substring(0, arr[0].Length - 1) + ")";
                    sleft = ReplaceValue(swhere, sdivTemplete, arr[0], sExpand);
                }

                //生成右边
                if (sMainPageSet.IndexOf(':') == -1)
                {
                    swhere = "  iOrder In (" + arr[0].ToString().Substring(0, arr[0].Length - 1) + ")";
                    sright = ReplaceValue(swhere, sdivTemplete, arr[0], sExpand);
                }
                else if (arr[1] != string.Empty)
                {
                    swhere = "  iOrder In (" + arr[1].ToString().Substring(0, arr[1].Length - 1) + ")";
                    sright = ReplaceValue(swhere, sdivTemplete, arr[1], sExpand);
                }
            }
            else   //没有设置值
            {
                //生成左边
                swhere = "  LeftOrRight=0 And isnull(DefaultVisible,0)=0";
                sleft = ReplaceValue(swhere, sdivTemplete, string.Empty, sExpand);

                //生成右边
                swhere = "  LeftOrRight=1 And isnull(DefaultVisible,0)=0";
                sright = ReplaceValue(swhere, sdivTemplete, string.Empty, sExpand);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="swhere"></param>
        /// <param name="sdivTemplete"></param>
        /// <param name="sCookie"></param>
        /// <param name="sExpand"></param>
        /// <returns></returns>
        private string ReplaceValue(string swhere, string sdivTemplete, string sCookie, string sExpand)
        {
            StringBuilder sb = new StringBuilder();
            DataTable dt = new DataTable();
            dt = GetDataTable(swhere);
            if (sCookie != string.Empty)
            {
                DataRow[] drarr;
                string[] arrselect = sCookie.Split(',');
                for (int i = 0; i < arrselect.Length - 1; i++)
                {
                    drarr = dt.Select(" iOrder=" + arrselect[i]);
                    if (drarr != null && drarr.Length > 0)
                    {
                        sb.Append(RepalceData(drarr[0], sdivTemplete, sExpand));
                    }
                }
            }
            else
            {
                foreach (DataRow dr in dt.Rows)
                {
                    sb.Append(RepalceData(dr, sdivTemplete, sExpand));
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="sdivTemplete"></param>
        /// <param name="sExpand"></param>
        /// <returns></returns>
        private string RepalceData(DataRow dr, string sdivTemplete, string sExpand)
        {
            string sReturn = string.Empty;
            string sTemp = string.Empty;
            sTemp = sdivTemplete.Replace("@i", dr["iOrder"].ToString()).Replace("@Title", dr["Title"].ToString()).
                    Replace("@url", dr["Url"].ToString()).Replace("@height", dr["IframeHeight"].ToString()).
                    Replace("@Timages", dr["ImageUrl"].ToString()).Replace("@moreurl", dr["MoreUrl"].ToString());
            if (sExpand.IndexOf(dr["iOrder"].ToString() + ",") >= 0)
            {
                sTemp = sTemp.Replace("@hidden", "style='display:none;'");
                sTemp = sTemp.Replace("@expandgif", "icon_expandall.gif'  title='展开' ");
            }
            else
            {
                sTemp = sTemp.Replace("@hidden", "");
                sTemp = sTemp.Replace("@expandgif", "icon_collapseall.gif'  title='隐藏' ");
            }
            if (dr["MoreUrl"].ToString() == "#")
            {
                sReturn = sTemp.Replace("@moredivsty", "display:none;");
            }
            else
            {
                sReturn = sTemp.Replace("@moredivsty", "");
            }

            return sReturn;
        }
        #endregion 
    }
}

