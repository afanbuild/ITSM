/*******************************************************************
 *
 * Description
 * 
 * 附件必填配置表操作类
 * 
 * Create By  :余向前
 * Create Date:2013年3月20日
 * *****************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Epower.DevBase.BaseTools;
using EpowerCom;
using System.Xml;
using System.IO;

namespace Epower.ITSM.SqlDAL
{
    public class BR_Attachment_ConfigDP
    {
        public BR_Attachment_ConfigDP()
        {

        }

        #region 属性
        private long mID;
        /// <summary>
        /// 标示ID
        /// </summary>
        public long ID
        {
            get { return mID; }
            set { mID = value; }
        }

        private long mAppID;
        /// <summary>
        /// 应用ID
        /// </summary>
        public long AppID
        {
            get { return mAppID; }
            set { mAppID = value; }
        }

        private long mOFlowModelID;
        /// <summary>
        /// 流程模型ID
        /// </summary>
        public long OFlowModelID
        {
            get { return mOFlowModelID; }
            set { mOFlowModelID = value; }
        }

        private long mNodeModelID;
        /// <summary>
        /// 环节模型ID
        /// </summary>
        public long NodeModelID
        {
            get { return mNodeModelID; }
            set { mNodeModelID = value; }
        }

        private string mNodeName;
        /// <summary>
        /// 环节名称
        /// </summary>
        public string NodeName
        {
            get { return mNodeName; }
            set { mNodeName = value; }
        }

        private int mOperators;
        /// <summary>
        /// 比较符 0表示等于、1表示以..开头、2表示包含
        /// </summary>
        public int Operators
        {
            get { return mOperators; }
            set { mOperators = value; }
        }

        private string mAttachmentName;
        /// <summary>
        /// 必填附件名称
        /// </summary>
        public string AttachmentName
        {
            get { return mAttachmentName; }
            set { mAttachmentName = value; }
        }

        private string mAttachmentType;
        /// <summary>
        /// 必填附件类型
        /// </summary>
        public string AttachmentType
        {
            get { return mAttachmentType; }
            set { mAttachmentType = value; }
        }

        #endregion

        #region GetReCorded
        /// <summary>
        /// 获取某条记录
        /// </summary>
        /// <param name="lngID">标示ID</param>
        /// <returns></returns>
        public BR_Attachment_ConfigDP GetReCorded(long lngID)
        {
            BR_Attachment_ConfigDP ee = new BR_Attachment_ConfigDP();

            string strSQL = string.Empty;
            strSQL = "select * from BR_Attachment_Config where ID = " + lngID.ToString();
            DataTable dt = CommonDP.ExcuteSqlTable(strSQL);
            foreach (DataRow dr in dt.Rows)
            {
                ee.ID = CTools.ToInt64(dr["ID"].ToString(), 0);
                ee.AppID = CTools.ToInt64(dr["AppID"].ToString(), 0);
                ee.OFlowModelID = CTools.ToInt64(dr["OFLOWMODELID"].ToString(), 0);
                ee.NodeModelID = CTools.ToInt64(dr["NodeModelID"].ToString(), 0);
                ee.NodeName = dr["NODENAME"].ToString();
                ee.Operators = CTools.ToInt(dr["Operators"].ToString(), 0);
                ee.AttachmentName = dr["AttachmentName"].ToString();
                ee.AttachmentType = dr["AttachmentType"].ToString();
            }
            return ee;
        }       
        #endregion

        #region GetDataTable
        /// <summary>
        /// 分页获取数据
        /// </summary>
        /// <param name="sWhere">条件语句 如( 1=1 and deleted=0 )</param>
        /// <param name="sOrder">排序条件 如 ( order by ID )</param>
        /// <param name="pagesize">每页显示条数</param>
        /// <param name="pageindex">第几页</param>
        /// <param name="rowcount">总记录数</param>
        /// <returns>DataTable</returns>
        public DataTable GetDataTable(string sWhere, string sOrder, int pagesize, int pageindex, ref int rowcount)
        {
            DataTable dt = CommonDP.ExcuteSqlTablePage("BR_Attachment_Config", "*", sWhere, sOrder, pagesize, pageindex, ref rowcount);
            return dt;
        }
        #endregion

        #region GetDataTable
        /// <summary>
        /// 分页获取数据
        /// </summary>
        /// <param name="sWhere">条件语句 如( 1=1 and deleted=0 )</param>
        /// <param name="sOrder">排序条件 如 ( order by ID )</param>
        /// <param name="pagesize">每页显示条数</param>
        /// <param name="pageindex">第几页</param>
        /// <param name="rowcount">总记录数</param>
        /// <returns>DataTable</returns>
        public DataTable GetDataTableView(string sWhere, string sOrder, int pagesize, int pageindex, ref int rowcount)
        {
            DataTable dt = CommonDP.ExcuteSqlTablePage("v_BR_Attachment_Config", "*", sWhere, sOrder, pagesize, pageindex, ref rowcount);
            return dt;
        }
        #endregion

        #region GetDataTable
        /// <summary>
        /// 按where条件查询数据
        /// </summary>
        /// <param name="sWhere">where 条件 如 ( and 1=1 )</param>
        /// <param name="sOrder">排序条件 如(order by ID)</param>
        /// <returns>DataTable</returns>
        public DataTable GetDataTable(string sWhere, string sOrder)
        {
            string strSQL = string.Empty;
            strSQL = "SELECT * FROM BR_Attachment_Config Where 1=1 ";
            strSQL += sWhere;
            strSQL += sOrder;
            DataTable dt = CommonDP.ExcuteSqlTable(strSQL);
            return dt;
        }

        #endregion

        #region InsertRecorded
        /// <summary>
        /// 添加记录
        /// </summary>
        /// <param name="pBR_Attachment_ConfigDP"></param>
        public void InsertRecorded(BR_Attachment_ConfigDP pBR_Attachment_ConfigDP)
        {
            string strSQL = string.Empty;
            string strID = "0";
            try
            {
                strID = EpowerGlobal.EPGlobal.GetNextID("BR_Attachment_ConfigID").ToString();
                pBR_Attachment_ConfigDP.ID = CTools.ToInt64(strID, 0);
                strSQL = @"insert into BR_Attachment_Config(
                                        ID,
                                        AppID,
                                        OFLOWMODELID,
                                        NodeModelID,
                                        NODENAME,
                                        Operators,
                                        AttachmentName,
                                        AttachmentType
                         )
                         values (" +
                                  strID.ToString() + "," +
                                  pBR_Attachment_ConfigDP.AppID.ToString() + "," +
                                  pBR_Attachment_ConfigDP.OFlowModelID.ToString() + "," +
                                  pBR_Attachment_ConfigDP.NodeModelID.ToString() + "," +
                                  StringTool.SqlQ(pBR_Attachment_ConfigDP.NodeName) + "," +
                                  pBR_Attachment_ConfigDP.Operators.ToString() + "," +
                                  StringTool.SqlQ(pBR_Attachment_ConfigDP.AttachmentName) + "," +
                                  StringTool.SqlQ(pBR_Attachment_ConfigDP.AttachmentType) +
                       ")";

                CommonDP.ExcuteSql(strSQL);
            }
            catch { }
        }
        #endregion

        #region DeleteAll
        /// <summary>
        /// 删除某个流程模型下所有配置信息
        /// <param name="lngAppID">应用ID</param>
        /// <param name="lngOFlowModelID">流程模型ID</param>
        /// </summary>
        public void DeleteAll(long lngAppID, long lngOFlowModelID)
        {
            try
            {
                string strSQL = " delete from BR_Attachment_Config where AppID = " + lngAppID.ToString() + "and OFLOWMODELID = " + lngOFlowModelID.ToString();
                CommonDP.ExcuteSql(strSQL);
            }
            catch { }
        }
        #endregion

        #region 验证必填附件是否已经上传
        /// <summary>
        /// 验证必填附件是否已经上传
        /// </summary>
        /// <param name="lngAppID">应用ID</param>
        /// <param name="lngFlowModelID">流程模型ID</param>
        /// <param name="lngNodeModelID">环节模型ID</param>
        /// <param name="strAttachXml">附件字符串</param>
        /// <param name="strmsg">返回的错误信息</param>
        /// <returns></returns>
        public static bool CheckAttachment(long lngAppID, long lngFlowModelID, long lngNodeModelID, string strAttachXml, ref string strmsg)
        {
            bool result = true;
            long lngOFlowModelID = FlowDP.GetOFlowModelID(lngFlowModelID); //获取OFlowModelID

            //获取必填附件配置信息
            BR_Attachment_ConfigDP ee = new BR_Attachment_ConfigDP();
            string strWhere = " and AppID=" + lngAppID + " and OFlowModelID=" + lngOFlowModelID + " and NodeModelID=" + lngNodeModelID;
            DataTable dt = ee.GetDataTable(strWhere, "");

            //没有设置必填配置信息时 直接返回true
            if (dt == null || dt.Rows.Count <= 0)
                return result;

            #region 没有上传附件时判断
            if (strAttachXml.Trim() == "<Attachments />")
            {
                result = false;
                foreach (DataRow dr in dt.Rows)
                {
                    //比较符
                    int Operators = CTools.ToInt(dr["Operators"].ToString(), 0);
                    //必填附件名称
                    string strAttachmentName = dr["AttachmentName"].ToString().Trim().ToLower();
                    //必填附件类型
                    string strAttachmentType = dr["AttachmentType"].ToString().Trim().ToLower();

                    switch (Operators)
                    {
                        case 0: //等于                            
                            strmsg += "必须上传名称为" + strAttachmentName;
                            break;
                        case 1: //以..开头
                            strmsg += "必须上传名称以" + strAttachmentName + "开头";
                            break;
                        case 2: //包含
                            strmsg += "必须上传名称包含" + strAttachmentName;
                            break;
                    }
                    if (strAttachmentType != string.Empty)
                    {
                        strmsg += ",格式为" + strAttachmentType;
                    }
                    strmsg += "的附件! ";
                
                }
                return result;
            }
            #endregion            

            #region 有上传附件时判断
            //循环判断
            foreach (DataRow dr in dt.Rows)
            {
                bool isOK = false; //是否匹配

                //比较符
                int Operators = CTools.ToInt(dr["Operators"].ToString(), 0);
                //必填附件名称
                string strAttachmentName = dr["AttachmentName"].ToString().Trim().ToLower();
                //必填附件类型
                string strAttachmentType = dr["AttachmentType"].ToString().Trim().ToLower();

                string strFileName = "";
                string strSufName = "";

                XmlTextReader tr = new XmlTextReader(new StringReader(strAttachXml));
                while (tr.Read())
                {
                    if (tr.NodeType == XmlNodeType.Element && tr.Name == "Attachment")
                    {
                        strFileName = tr.GetAttribute("FileName").Substring(0, tr.GetAttribute("FileName").LastIndexOf('.')).Trim().ToLower(); //文件名 如 XXXX
                        strSufName = tr.GetAttribute("SufName").Trim().ToLower();   //文件扩展名 如 jpg

                        //匹配名称
                        switch (Operators)
                        { 
                            case 0: //等于
                                if (strAttachmentName == strFileName)
                                {
                                    isOK = true;
                                }
                                break;
                            case 1: //以..开头
                                if (strFileName.StartsWith(strAttachmentName))
                                {
                                    isOK = true;
                                }
                                break;
                            case 2: //包含
                                if (strFileName.Contains(strAttachmentName))
                                {
                                    isOK = true;
                                }
                                break;
                        }
                        //匹配类型
                        if (strAttachmentType != string.Empty && strAttachmentType != strSufName)
                        {
                            isOK = false;
                        }


                        if (isOK)
                        {
                            break;
                        }                        
                    }                   
                }
                tr.Close();

                if (!isOK)
                {
                    switch (Operators)
                    {
                        case 0: //等于                           
                            strmsg += "必须上传名称等于" + strAttachmentName;                            
                            break;
                        case 1: //以..开头
                            strmsg += "必须上传名称以" + strAttachmentName + "开头";
                            break;
                        case 2: //包含
                            strmsg += "必须上传名称包含" + strAttachmentName;                            
                            break;
                    }

                    if (strAttachmentType != string.Empty)
                    {
                        strmsg += ",格式为" + strAttachmentType;
                    }
                    strmsg += "的附件! ";
                }

            }
            #endregion

            if (strmsg.Trim() != string.Empty)
                result = false;

            return result;

        }
        #endregion
    }
}
