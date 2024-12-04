/********************************************
 * 版权所有：深圳市非凡信息技术有限公司
 * 
 * 
 * 
 * 功能描述：统一的Excel文件操作
 * 作者：朱明春
 * 创建日期：2006-10-19
 * ******************************************/
using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Xml;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Reflection;
using System.Data.OleDb;
using System.IO;
using System.Text;
using Epower.DevBase.BaseTools;
using Epower.ITSM.SqlDAL;
using EpowerCom;

using System.Collections;

namespace Epower.ITSM.Web.Common
{

    /// <summary>
    /// 输出EXCEL 字段处理实现类
    /// </summary>
    public class ImplDataFieldProcess : IDataFieldProcess
    {
        static Hashtable _fields;
        static Hashtable _fieldsRel;

        public ImplDataFieldProcess()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }



        /// <summary>
        /// 处理字段结果
        /// </summary>
        /// <param name="sFieldValue"></param>
        /// <param name="sPara"></param>
        /// <returns></returns>
        public override string GetDataFieldProcess(string sFieldValue, string sPara)
        {
            string sRet = sFieldValue;

            switch (sPara)
            {
                case "GetFlowStatus":

                    if (sFieldValue == "20")
                    {
                        sRet = "正在处理";
                    }
                    if (sFieldValue == "30")
                    {
                        sRet = "正常结束";
                    }
                    if (sFieldValue == "40")
                    {
                        sRet = "流程暂停";
                    }

                    break;
                case "GetMastCustName":
                    if (sFieldValue.Length > 0)
                    {
                        sRet = Br_MastCustomerDP.GetMastCustName(long.Parse(sFieldValue));
                    }

                    break;
                case "ParaConfigureValue":
                    if (sFieldValue.Length > 0)
                    {
                        sRet = GetConfigureValueDesc(sFieldValue);
                    }

                    break;
                default:
                    break;
            }


            return sRet;
        }

        /// <summary>
        /// 获取资产配置的描述信息，用于EXCEL 支持的格式
        /// </summary>
        /// <param name="sXml"></param>
        /// <returns></returns>
        private string GetConfigureValueDesc(string sXml)
        {
            string sDesc = "";
            string showBaseValue = "基础配置：";
            string showRelValue = "关联配置：";

            if (_fields == null)
            {
                Equ_SchemaItemsDP ee = new Equ_SchemaItemsDP();

                _fields = ee.GetAllFields(0);   //获取最新的配置项情况
            }
            if (_fieldsRel == null)
            {
                Equ_SchemaItemsDP ee = new Equ_SchemaItemsDP();

                _fieldsRel = ee.GetAllFields(1);   //获取最新的配置项情况[关联]
            }
            Hashtable _xmlHt = new Hashtable();

            _xmlHt = SetHashTableXmlValue(sXml);
            //基础配置
            foreach (System.Collections.DictionaryEntry objDE in _xmlHt)
            {
                string sCHName = "";
                string sID = objDE.Key.ToString();
                string sValue = objDE.Value.ToString();
                if (_fields[sID] != null)
                {
                    sCHName = _fields[sID].ToString();

                }

                if (sCHName.Length > 0 && sValue != string.Empty)     //朱明春修改  20090813，去掉值为空的
                {
                    showBaseValue += sCHName + ":" + sValue + ";";

                }

            }

            //关联配置
            foreach (System.Collections.DictionaryEntry objDE in _xmlHt)
            {
                string sCHName = "";
                string sID = objDE.Key.ToString();
                string sValue = objDE.Value.ToString();
                if (_fieldsRel[sID] != null)
                {
                    sCHName = _fieldsRel[sID].ToString();

                }


                if (sCHName.Length > 0 && sValue == "1")
                {
                    showRelValue += sCHName + ";";

                }

            }
            sDesc = showBaseValue + "      " + showRelValue;

            //遍历HASHTABLE
            return sDesc;

        }


        private Hashtable SetHashTableXmlValue(string strXml)
        {
            Hashtable ht = new Hashtable();

            string strTmp = "";
            try
            {
                XmlTextReader tr = new XmlTextReader(new StringReader(strXml));
                while (tr.Read())
                {
                    if (tr.Name == "Field" && tr.NodeType == XmlNodeType.Element)
                    {
                        strTmp = tr.GetAttribute("Value").Trim();
                        ht.Add(tr.GetAttribute("FieldName"), strTmp);
                    }
                }

                tr.Close();
            }
            catch
            {
            }

            return ht;
        }

    }



    /// <summary>
    /// ExcelExport 的摘要说明。
    /// </summary>
    public class ExcelExport
    {
        #region 属性区

        #region  事件导出报表
        private static string _IssuesListType = "ExcelTemplate\\Rpt_IssuesList.xls";
        /// <summary>
        /// 项目完成情况对比报表模板
        /// </summary>
        protected static string IssuesListType
        {
            get
            {
                return _IssuesListType;
            }
        }
        #endregion

        #region  问题导出报表
        private static string _ProblemListType = "ExcelTemplate\\Rpt_ProblemList.xls";
        /// <summary>
        /// 项目完成情况对比报表模板
        /// </summary>
        protected static string ProblemListType
        {
            get
            {
                return _ProblemListType;
            }
        }
        #endregion

        #region  投诉导出报表
        private static string _BYListType = "ExcelTemplate\\Rpt_BYList.xls";
        /// <summary>
        /// 项目完成情况对比报表模板
        /// </summary>
        protected static string BYListType
        {
            get
            {
                return _BYListType;
            }
        }
        #endregion

        #region  知识导出报表
        private static string _KBListType = "ExcelTemplate\\Rpt_KBList.xls";
        /// <summary>
        /// 项目完成情况对比报表模板
        /// </summary>
        protected static string KBListType
        {
            get
            {
                return _KBListType;
            }
        }
        #endregion

        #region  客户导出报表
        private static string _CustListType = "ExcelTemplate\\Rpt_CustList.xls";
        /// <summary>
        /// 项目完成情况对比报表模板
        /// </summary>
        protected static string CustListType
        {
            get
            {
                return _CustListType;
            }
        }
        #endregion

        #region  资产导出报表
        private static string _DeskListType = "ExcelTemplate\\Rpt_DeskList.xls";
        /// <summary>
        /// 项目完成情况对比报表模板
        /// </summary>
        protected static string DeskListType
        {
            get
            {
                return _DeskListType;
            }
        }
        #endregion

        #region 进入操作间报表

        private static string _QuesHouse = "ExcelTemplate\\Rpt_QuestHouse.xls";
        /// <summary>
        /// 进入操作间报表
        /// </summary>
        protected static string QuesHouse
        {
            get
            {
                return _QuesHouse;
            }
        }

        #endregion

        #region  变更单导出报表
        private static string _ChangeListType = "ExcelTemplate\\Rpt_ChangeList.xls";
        /// <summary>
        /// 项目完成情况对比报表模板
        /// </summary>
        protected static string ChangeListType
        {
            get
            {
                return _ChangeListType;
            }
        }
        #endregion

        #region  资产盘点情况报表
        private static string _CheckPlanListType = "ExcelTemplate\\Rpt_CheckPlanList.xls";
        /// <summary>
        /// 项目完成情况对比报表模板
        /// </summary>
        protected static string CheckPlanListType
        {
            get
            {
                return _CheckPlanListType;
            }
        }
        #endregion

        #region 临时文件路径
        /// <summary>
        /// 临时文件路径
        /// </summary>
        private static string TempPath
        {
            get { return "ExcelTemplate\\"; }
        }
        #endregion

        #region 短信模板导出报表
        private static string _MailMessageTemListType = "ExcelTemplate\\Rpt_MailMessageTemList.xls";
        /// <summary>
        /// 短信模板导出报表
        /// </summary>
        protected static string MailMessageTemListType
        {
            get
            {
                return _MailMessageTemListType;
            }
        }
        #endregion

        #region  日志导出
        private static string _LogListType = "ExcelTemplate\\Rpt_LogList.xls";
        /// <summary>
        /// 项目完成情况对比报表模板
        /// </summary>
        protected static string LogListType
        {
            get
            {
                return _LogListType;
            }
        }
        #endregion

        #region 资产明细列表导出
        private static string _EquDetailListType = "ExcelTemplate\\Rpt_DeskDetailLists.xls";
        /// <summary>
        /// 资产明细列表导出
        /// </summary>
        protected static string EquDetailListType
        {
            get
            {
                return _EquDetailListType;
            }
        }
        #endregion

        #endregion

        #region 方法区

        #region 导出问题单列表报表Excel
        /// <summary>
        /// 导出问题单列表报表Excel
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pDataTable"></param>
        /// <param name="pUserID"></param>
        public static void ExportProblemList(System.Web.UI.Page page, DataTable pDataTable, string pUserID)
        {
            //物理路径
            string path = page.Request.PhysicalApplicationPath;
            //文件后缀名
            string strExt = ".xls";
            //文件全路径
            string fullpath = path + ProblemListType;
            //文件名称
            string strfileName = "Rpt_ProblemList.xls";   //需要修改的地方

            //存放临时文件路径
            FileUpDown.DirPath = path + TempPath;
            string strDir = FileUpDown.DirPath + pUserID + "\\";

            //自动创建文件夹
            MyFiles.AutoCreateDirectory(strDir);

            //虚文件名
            String strTmpFileName = FileUpDown.GetFileName(strDir + strfileName.Substring(0, strfileName.Length - strExt.Length) +
                DateTime.Now.ToString("yyyyMMdd") + pUserID, strExt);
            string strFullTmpFileName = strDir + strTmpFileName;

            //插入数据                                 //需要修改的地方
            string[] arrField = { "ProblemNo", "EquName", "Problem_TypeName", "Problem_LevelName", "EffectName", "InstancyName", "Problem_Title", "Problem_Subject", "StateName", "Remark", "RegUserName" };

            IDataFieldProcess idfp = new ImplDataFieldProcess();

            ExportExcel.InsertExcel(fullpath, strFullTmpFileName, arrField, pDataTable, idfp);

            //下载文件
            FileUpDown.DownFile(page.Response, strTmpFileName, strFullTmpFileName);

            //删除临时文件
            FileUpDown.FileDelete(strFullTmpFileName);

        }
        /// <summary>
        /// 导出问题单列表报表Excel
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pDataTable"></param>
        /// <param name="pUserID"></param>
        public static void ExportProblemList(System.Web.UI.Page page, DataTable pDataTable, string[] fileName, string[] arrField, string pUserID)
        {
            //物理路径
            string path = page.Request.PhysicalApplicationPath;
            //文件后缀名
            string strExt = ".xls";
            //文件全路径
            string fullpath = path + ProblemListType;
            //文件名称
            string strfileName = "Rpt_ProblemList.xls";   //需要修改的地方

            //存放临时文件路径
            FileUpDown.DirPath = path + TempPath;
            string strDir = FileUpDown.DirPath + pUserID + "\\";

            //自动创建文件夹
            MyFiles.AutoCreateDirectory(strDir);

            //虚文件名
            String strTmpFileName = FileUpDown.GetFileName(strDir + strfileName.Substring(0, strfileName.Length - strExt.Length) +
                DateTime.Now.ToString("yyyyMMdd") + pUserID, strExt);
            string strFullTmpFileName = strDir + strTmpFileName;

            //插入数据                                 //需要修改的地方
            //string[] arrField = { "ProblemNo", "EquName", "Problem_TypeName", "Problem_LevelName", "EffectName", "InstancyName", "Problem_Title", "Problem_Subject", "StateName", "Remark", "RegUserName" };

            IDataFieldProcess idfp = new ImplDataFieldProcess();

            //ExportExcel.InsertExcel(fullpath, strFullTmpFileName, arrField, pDataTable, idfp);
            ExportExcel.InsertExcel1(fullpath, strFullTmpFileName, fileName, arrField, pDataTable, idfp);

            //下载文件
            FileUpDown.DownFile(page.Response, strTmpFileName, strFullTmpFileName);

            //删除临时文件
            FileUpDown.FileDelete(strFullTmpFileName);

        }
        #endregion


        #region 需求管理 - 2013-04-27 @孙绍棕

        /// <summary>
        /// 导出需求列表为Excel
        /// </summary>
        /// <param name="page">页面对象</param>
        /// <param name="pDataTable">数据源表</param>
        /// <param name="fileName">字段显示名的数组</param>
        /// <param name="arrField">字段名的数据</param>
        /// <param name="pUserID">用户编号</param>
        public static void ExportReqDemandList(
            System.Web.UI.Page page,
            DataTable pDataTable,
            string[] fileName,
            string[] arrField,
            string pUserID)
        {
            //物理路径
            string path = page.Request.PhysicalApplicationPath;
            //文件后缀名
            string strExt = ".xls";
            //文件全路径
            string fullpath = path + ProblemListType;
            //文件名称
            string strfileName = "Rpt_ReqDemandList.xls";   //需要修改的地方

            //存放临时文件路径
            FileUpDown.DirPath = path + TempPath;
            string strDir = FileUpDown.DirPath + pUserID + "\\";

            //自动创建文件夹
            MyFiles.AutoCreateDirectory(strDir);

            //虚文件名
            String strTmpFileName = FileUpDown.GetFileName(strDir + strfileName.Substring(0, strfileName.Length - strExt.Length) +
                DateTime.Now.ToString("yyyyMMdd") + pUserID, strExt);
            string strFullTmpFileName = strDir + strTmpFileName;

            IDataFieldProcess idfp = new ImplDataFieldProcess();

            //ExportExcel.InsertExcel(fullpath, strFullTmpFileName, arrField, pDataTable, idfp);
            ExportExcel.InsertExcel1(fullpath, strFullTmpFileName, fileName, arrField, pDataTable, idfp);

            //下载文件
            FileUpDown.DownFile(page.Response, strTmpFileName, strFullTmpFileName);

            //删除临时文件
            FileUpDown.FileDelete(strFullTmpFileName);

        }

        #endregion


        #region 导出事件列表报表Excel
        /// <summary>
        /// 导出项目完成情况对比报表Excel
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pDataTable"></param>
        /// <param name="pUserID"></param>
        public static void ExportIssuesList(System.Web.UI.Page page, DataTable pDataTable, string pUserID)
        {
            //物理路径
            string path = page.Request.PhysicalApplicationPath;
            //文件后缀名
            string strExt = ".xls";
            //文件全路径
            string fullpath = path + IssuesListType;
            //文件名称
            string strfileName = "Rpt_IssuesList.xls";   //需要修改的地方

            //存放临时文件路径
            FileUpDown.DirPath = path + TempPath;
            string strDir = FileUpDown.DirPath + pUserID + "\\";

            //自动创建文件夹
            MyFiles.AutoCreateDirectory(strDir);

            //虚文件名
            String strTmpFileName = FileUpDown.GetFileName(strDir + strfileName.Substring(0, strfileName.Length - strExt.Length) +
                DateTime.Now.ToString("yyyyMMdd") + pUserID, strExt);
            string strFullTmpFileName = strDir + strTmpFileName;

            //插入数据                                 //需要修改的地方
            string[] arrField = { "ServiceNo", "ServiceType", "MastCustName", "CustName", "contact", "CTel", "CustAddress", "EquipmentName", "CustTime", "ReportingTime", "FinishedTime", "ServiceLevel", "EffectName", "InstancyName", "CloseReasonName", "ReSouseName", "DealStatus", "subject", "Content", "status:GetFlowStatus", "Outtime", "ServiceTime", "Sjwxr", "DealContent", "RegUserName" };

            IDataFieldProcess idfp = new ImplDataFieldProcess();

            ExportExcel.InsertExcel(fullpath, strFullTmpFileName, arrField, pDataTable, idfp);

            //下载文件
            FileUpDown.DownFile(page.Response, strTmpFileName, strFullTmpFileName);

            //删除临时文件
            FileUpDown.FileDelete(strFullTmpFileName);

        }
        #endregion

        #region 导出事件列表报表Excel
        /// <summary>
        /// 导出项目完成情况对比报表Excel
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pDataTable"></param>
        /// <param name="pUserID"></param>
        public static void ExportIssuesList1(System.Web.UI.Page page, DataTable pDataTable, string[] fileName, string[] arrField, string pUserID)
        {
            //物理路径
            string path = page.Request.PhysicalApplicationPath;
            //文件后缀名
            string strExt = ".xls";
            //文件全路径
            string fullpath = path + IssuesListType;
            //文件名称
            string strfileName = "Rpt_IssuesList.xls";   //需要修改的地方

            //存放临时文件路径
            FileUpDown.DirPath = path + TempPath;
            string strDir = FileUpDown.DirPath + pUserID + "\\";

            //自动创建文件夹
            MyFiles.AutoCreateDirectory(strDir);

            //虚文件名
            String strTmpFileName = FileUpDown.GetFileName(strDir + strfileName.Substring(0, strfileName.Length - strExt.Length) +
                DateTime.Now.ToString("yyyyMMdd") + pUserID, strExt);
            string strFullTmpFileName = strDir + strTmpFileName;

            //插入数据                                 //需要修改的地方
            // string[] arrField = { "ServiceNo", "ServiceType", "MastCustName", "CustName", "contact", "CTel", "CustAddress", "EquipmentName", "CustTime", "ReportingTime", "FinishedTime", "ServiceLevel", "EffectName", "InstancyName", "CloseReasonName", "ReSouseName", "DealStatus", "subject", "Content", "status:GetFlowStatus", "Outtime", "ServiceTime", "Sjwxr", "DealContent", "RegUserName" };

            IDataFieldProcess idfp = new ImplDataFieldProcess();

            ExportExcel.InsertExcel1(fullpath, strFullTmpFileName, fileName, arrField, pDataTable, idfp);

            //下载文件
            FileUpDown.DownFile(page.Response, strTmpFileName, strFullTmpFileName);

            //删除临时文件
            FileUpDown.FileDelete(strFullTmpFileName);

        }
        #endregion

        #region 导出投诉抱怨列表报表Excel
        /// <summary>
        /// 导出项目完成情况对比报表Excel
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pDataTable"></param>
        /// <param name="pUserID"></param>
        public static void ExportBYList(System.Web.UI.Page page, DataTable pDataTable, string pUserID)
        {
            //物理路径
            string path = page.Request.PhysicalApplicationPath;
            //文件后缀名
            string strExt = ".xls";
            //文件全路径
            string fullpath = path + BYListType;
            //文件名称
            string strfileName = "Rpt_BYList.xls";   //需要修改的地方

            //存放临时文件路径
            FileUpDown.DirPath = path + TempPath;
            string strDir = FileUpDown.DirPath + pUserID + "\\";

            //自动创建文件夹
            MyFiles.AutoCreateDirectory(strDir);

            //虚文件名
            String strTmpFileName = FileUpDown.GetFileName(strDir + strfileName.Substring(0, strfileName.Length - strExt.Length) +
                DateTime.Now.ToString("yyyyMMdd") + pUserID, strExt);
            string strFullTmpFileName = strDir + strTmpFileName;

            //插入数据                                 //需要修改的地方
            string[] arrField = { "flowid", "BY_PersonName", "BY_ProjectName", "BY_SoureName", "BY_TypeName", "BY_KindName", "BY_ReceiveTime", "RegTime", "status:GetFlowStatus" };

            IDataFieldProcess idfp = new ImplDataFieldProcess();

            ExportExcel.InsertExcel(fullpath, strFullTmpFileName, arrField, pDataTable, idfp);

            //下载文件
            FileUpDown.DownFile(page.Response, strTmpFileName, strFullTmpFileName);

            //删除临时文件
            FileUpDown.FileDelete(strFullTmpFileName);

        }
        #endregion


        #region 导出知识列表报表Excel
        /// <summary>
        /// 导出项目完成情况对比报表Excel
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pDataTable"></param>
        /// <param name="pUserID"></param>
        public static void ExportKBList(System.Web.UI.Page page, DataTable pDataTable, string pUserID)
        {
            //物理路径
            string path = page.Request.PhysicalApplicationPath;
            //文件后缀名
            string strExt = ".xls";
            //文件全路径
            string fullpath = path + KBListType;
            //文件名称
            string strfileName = "Rpt_KBList.xls";   //需要修改的地方

            //存放临时文件路径
            FileUpDown.DirPath = path + TempPath;

            string strDir = FileUpDown.DirPath + pUserID + "\\";

            //自动创建文件夹
            MyFiles.AutoCreateDirectory(strDir);


            //虚文件名
            String strTmpFileName = FileUpDown.GetFileName(strDir + strfileName.Substring(0, strfileName.Length - strExt.Length) +
                DateTime.Now.ToString("yyyyMMdd") + pUserID, strExt);
            string strFullTmpFileName = strDir + strTmpFileName;

            //插入数据                                 //需要修改的地方
            string[] arrField = { "id", "Title", "PKey", "TypeName", "Tags" };

            IDataFieldProcess idfp = new ImplDataFieldProcess();

            ExportExcel.InsertExcel(fullpath, strFullTmpFileName, arrField, pDataTable, idfp);

            //下载文件
            FileUpDown.DownFile(page.Response, strTmpFileName, strFullTmpFileName);

            //删除临时文件
            FileUpDown.FileDelete(strFullTmpFileName);

        }
        #endregion

        #region 导出客户列表报表Excel
        /// <summary>
        /// 导出项目完成情况对比报表Excel
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pDataTable"></param>
        /// <param name="pUserID"></param>
        public static void ExportCustList(System.Web.UI.Page page, DataTable pDataTable, string pUserID)
        {
            //物理路径
            string path = page.Request.PhysicalApplicationPath;
            //文件后缀名
            string strExt = ".xls";
            //文件全路径
            string fullpath = path + CustListType;
            //文件名称
            string strfileName = "Rpt_CustList.xls";   //需要修改的地方

            //存放临时文件路径
            FileUpDown.DirPath = path + TempPath;

            string strDir = FileUpDown.DirPath + pUserID + "\\";

            //自动创建文件夹
            MyFiles.AutoCreateDirectory(strDir);


            //虚文件名
            String strTmpFileName = FileUpDown.GetFileName(strDir + strfileName.Substring(0, strfileName.Length - strExt.Length) +
                DateTime.Now.ToString("yyyyMMdd") + pUserID, strExt);
            string strFullTmpFileName = strDir + strTmpFileName;

            //插入数据                                 //需要修改的地方
            string[] arrField = { "id", "MastCustID:GetMastCustName", "CustDeptName", "ShortName", "FullName", "CustomCode", "CustomerTypeName", "Email", "LinkMan1", "Tel1", "Address", "Rights" };

            IDataFieldProcess idfp = new ImplDataFieldProcess();

            ExportExcel.InsertExcel(fullpath, strFullTmpFileName, arrField, pDataTable, idfp);

            //下载文件
            FileUpDown.DownFile(page.Response, strTmpFileName, strFullTmpFileName);

            //删除临时文件
            FileUpDown.FileDelete(strFullTmpFileName);

        }
        #endregion

        #region 导出资产列表报表Excel
        /// <summary>
        /// 导出项目完成情况对比报表Excel
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pDataTable"></param>
        /// <param name="pUserID"></param>
        public static void ExportDeskList(System.Web.UI.Page page, DataTable pDataTable, string pUserID)
        {
            //物理路径
            string path = page.Request.PhysicalApplicationPath;
            //文件后缀名
            string strExt = ".xls";
            //文件全路径
            string fullpath = path + DeskListType;
            //文件名称
            string strfileName = "Rpt_DeskList.xls";   //需要修改的地方

            //存放临时文件路径
            FileUpDown.DirPath = path + TempPath;

            string strDir = FileUpDown.DirPath + pUserID + "\\";

            //自动创建文件夹
            MyFiles.AutoCreateDirectory(strDir);

            //虚文件名
            String strTmpFileName = FileUpDown.GetFileName(strDir + strfileName.Substring(0, strfileName.Length - strExt.Length) +
                DateTime.Now.ToString("yyyyMMdd") + pUserID, strExt);
            string strFullTmpFileName = strDir + strTmpFileName;

            //插入数据                                 //需要修改的地方
            string[] arrField = { "Code", "Name", "CatalogName", "listName", "partBankName", "partBranchName", "CostomName", "ServiceBeginTime", "ServiceEndTime", "EquStatusName", "ConfigureValue:ParaConfigureValue" };

            IDataFieldProcess idfp = new ImplDataFieldProcess();

            ExportExcel.InsertExcel(fullpath, strFullTmpFileName, arrField, pDataTable, idfp);

            //下载文件
            FileUpDown.DownFile(page.Response, strTmpFileName, strFullTmpFileName);

            //删除临时文件
            FileUpDown.FileDelete(strFullTmpFileName);

        }
        #endregion

        #region 导出资产盘点列表报表Excel
        /// <summary>
        /// 导出资产盘点计划报表Excel
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pDataTable"></param>
        /// <param name="pUserID"></param>
        public static void ExportCheckPlanList(System.Web.UI.Page page, DataTable pDataTable, string pUserID)
        {
            //物理路径
            string path = page.Request.PhysicalApplicationPath;
            //文件后缀名
            string strExt = ".xls";
            //文件全路径
            string fullpath = path + CheckPlanListType;
            //文件名称
            string strfileName = "Rpt_CheckPlanList.xls";   //需要修改的地方

            //存放临时文件路径
            FileUpDown.DirPath = path + TempPath;

            string strDir = FileUpDown.DirPath + pUserID + "\\";

            //自动创建文件夹
            MyFiles.AutoCreateDirectory(strDir);

            //虚文件名
            String strTmpFileName = FileUpDown.GetFileName(strDir + strfileName.Substring(0, strfileName.Length - strExt.Length) +
                DateTime.Now.ToString("yyyyMMdd") + pUserID, strExt);
            string strFullTmpFileName = strDir + strTmpFileName;

            //插入数据                                 //需要修改的地方
            string[] arrField = { "Code", "Name", "SerialNumber", "Positions", "CostomName", "EquStatusName", "ConfigureValue:ParaConfigureValue" };

            IDataFieldProcess idfp = new ImplDataFieldProcess();

            ExportExcel.InsertExcel(fullpath, strFullTmpFileName, arrField, pDataTable, idfp);

            //下载文件
            FileUpDown.DownFile(page.Response, strTmpFileName, strFullTmpFileName);

            //删除临时文件
            FileUpDown.FileDelete(strFullTmpFileName);

        }
        #endregion

        #region 导出变更列表报表Excel
        /// <summary>
        /// 导出变更列表报表Excel
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pDataTable"></param>
        /// <param name="pUserID"></param>
        public static void ExportChangeList(System.Web.UI.Page page, DataTable pDataTable, string pUserID)
        {
            //物理路径
            string path = page.Request.PhysicalApplicationPath;
            //文件后缀名
            string strExt = ".xls";
            //文件全路径
            string fullpath = path + ChangeListType;  //需要修改的地方
            //文件名称
            string strfileName = "Rpt_ChangeList.xls";   //需要修改的地方

            //存放临时文件路径
            FileUpDown.DirPath = path + TempPath;
            string strDir = FileUpDown.DirPath + pUserID + "\\";

            //自动创建文件夹
            MyFiles.AutoCreateDirectory(strDir);

            //虚文件名
            String strTmpFileName = FileUpDown.GetFileName(strDir + strfileName.Substring(0, strfileName.Length - strExt.Length) +
                DateTime.Now.ToString("yyyyMMdd") + pUserID, strExt);
            string strFullTmpFileName = strDir + strTmpFileName;

            //插入数据                                 //需要修改的地方
            string[] arrField = { "ChangeNo", "ChangeTypeName", "RegTime", "MastCustName", "CustName", "CTel", "CustAddress", "Subject", "Content", "EffectName", "InstancyName", "LevelName", "DealStatus", "ChangeAnalyses", "ChangeAnalysesResult", "ChangeTime", "DealStatus", "TPersonList" };

            IDataFieldProcess idfp = new ImplDataFieldProcess();

            ExportExcel.InsertExcel(fullpath, strFullTmpFileName, arrField, pDataTable, idfp);

            //下载文件
            FileUpDown.DownFile(page.Response, strTmpFileName, strFullTmpFileName);

            //删除临时文件
            FileUpDown.FileDelete(strFullTmpFileName);

        }
        /// <summary>
        /// 导出变更列表报表Excel
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pDataTable"></param>
        /// <param name="pUserID"></param>
        public static void ExportChangeList(System.Web.UI.Page page, DataTable pDataTable, string[] fileName, string[] arrField, string pUserID)
        {
            //物理路径
            string path = page.Request.PhysicalApplicationPath;
            //文件后缀名
            string strExt = ".xls";
            //文件全路径
            string fullpath = path + ChangeListType;  //需要修改的地方
            //文件名称
            string strfileName = "Rpt_ChangeList.xls";   //需要修改的地方

            //存放临时文件路径
            FileUpDown.DirPath = path + TempPath;
            string strDir = FileUpDown.DirPath + pUserID + "\\";

            //自动创建文件夹
            MyFiles.AutoCreateDirectory(strDir);

            //虚文件名
            String strTmpFileName = FileUpDown.GetFileName(strDir + strfileName.Substring(0, strfileName.Length - strExt.Length) +
                DateTime.Now.ToString("yyyyMMdd") + pUserID, strExt);
            string strFullTmpFileName = strDir + strTmpFileName;

            //插入数据                                 //需要修改的地方
            //string[] arrField = { "ChangeNo", "ChangeTypeName", "RegTime", "MastCustName", "CustName", "CTel", "CustAddress", "Subject", "Content", "EffectName", "InstancyName", "LevelName", "DealStatus", "ChangeAnalyses", "ChangeAnalysesResult", "ChangeTime", "DealStatus", "TPersonList" };

            IDataFieldProcess idfp = new ImplDataFieldProcess();

            ExportExcel.InsertExcel1(fullpath, strFullTmpFileName, fileName, arrField, pDataTable, idfp);

            //下载文件
            FileUpDown.DownFile(page.Response, strTmpFileName, strFullTmpFileName);

            //删除临时文件
            FileUpDown.FileDelete(strFullTmpFileName);

        }
        #endregion

        #region 下载客户资料导入模板Excel
        /// <summary>
        /// 下载客户资料导入模板Excel
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pUserID"></param>
        public static void DownLoadCustExcel(System.Web.UI.Page page, string pUserID)
        {
            string CustType = "ExcelTemplate\\客户资料导入模板.xls";
            //物理路径
            string path = page.Request.PhysicalApplicationPath;
            //文件全路径
            string fullpath = path + CustType;
            //文件名称
            string strfileName = "客户资料导入模板.xls";   //需要修改的地方

            //下载文件
            FileUpDown.DownFile(page.Response, strfileName, fullpath);

        }
        #endregion

        #region 下载资产资料导入模板Excel
        /// <summary>
        /// 下载资产资料导入模板Excel
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pUserID"></param>
        public static void DownLoadEquExcel(System.Web.UI.Page page, string pUserID)
        {
            string EquType = "ExcelTemplate\\资产资料导入模板.xls";
            //物理路径
            string path = page.Request.PhysicalApplicationPath;
            //文件全路径
            string fullpath = path + EquType;
            //文件名称
            string strfileName = "资产资料导入模板.xls";   //需要修改的地方

            //下载文件
            FileUpDown.DownFile(page.Response, strfileName, fullpath);

        }
        #endregion

        #region 导出日志列表报表Excel
        /// <summary>
        /// 导出日志列表报表Excel
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pDataTable"></param>
        /// <param name="pUserID"></param>
        public static void ExportLogList(System.Web.UI.Page page, DataTable pDataTable, string pUserID)
        {
            //物理路径
            string path = page.Request.PhysicalApplicationPath;
            //文件后缀名
            string strExt = ".xls";
            //文件全路径
            string fullpath = path + LogListType;
            //文件名称
            string strfileName = "Rpt_LogList.xls";   //需要修改的地方

            //存放临时文件路径
            FileUpDown.DirPath = path + TempPath;
            string strDir = FileUpDown.DirPath + pUserID + "\\";

            //自动创建文件夹
            MyFiles.AutoCreateDirectory(strDir);

            //虚文件名
            String strTmpFileName = FileUpDown.GetFileName(strDir + strfileName.Substring(0, strfileName.Length - strExt.Length) +
                DateTime.Now.ToString("yyyyMMdd") + pUserID, strExt);
            string strFullTmpFileName = strDir + strTmpFileName;

            //插入数据                                 //需要修改的地方
            string[] arrField = { "SysName", "UserName", "Dept", "IPAddress", "OPEndTime", "ActionName", "Memo" };

            IDataFieldProcess idfp = new ImplDataFieldProcess();

            ExportExcel.InsertExcel(fullpath, strFullTmpFileName, arrField, pDataTable, idfp);

            //下载文件
            FileUpDown.DownFile(page.Response, strTmpFileName, strFullTmpFileName);

            //删除临时文件
            FileUpDown.FileDelete(strFullTmpFileName);

        }
        #endregion

        #region 导出短信模板列表报表Excel
        /// <summary>
        /// 导出短信模板列表报表Excel
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pDataTable"></param>
        /// <param name="pUserID"></param>
        public static void ExportMailMessageTemList(System.Web.UI.Page page, DataTable pDataTable, string pUserID)
        {
            //物理路径
            string path = page.Request.PhysicalApplicationPath;
            //文件后缀名
            string strExt = ".xls";
            //文件全路径
            string fullpath = path + MailMessageTemListType;
            //文件名称
            string strfileName = "Rpt_MailMessageTemList.xls";   //需要修改的地方

            //存放临时文件路径
            FileUpDown.DirPath = path + TempPath;

            string strDir = FileUpDown.DirPath + pUserID + "\\";

            //自动创建文件夹
            MyFiles.AutoCreateDirectory(strDir);


            //虚文件名
            String strTmpFileName = FileUpDown.GetFileName(strDir + strfileName.Substring(0, strfileName.Length - strExt.Length) +
                DateTime.Now.ToString("yyyyMMdd") + pUserID, strExt);
            string strFullTmpFileName = strDir + strTmpFileName;

            //插入数据                                 //需要修改的地方
            string[] arrField = { "TemplateName", "MailContent", "ModelContent", "Status", "RegTime", "RegUserName", "RegDeptName" };

            IDataFieldProcess idfp = new ImplDataFieldProcess();

            ExportExcel.InsertExcel(fullpath, strFullTmpFileName, arrField, pDataTable, idfp);

            //下载文件
            FileUpDown.DownFile(page.Response, strTmpFileName, strFullTmpFileName);

            //删除临时文件
            FileUpDown.FileDelete(strFullTmpFileName);

        }
        #endregion

        #region 导出操作间列表报表Excel
        /// <summary>
        /// 导出操作间列表报表Excel
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pDataTable"></param>
        /// <param name="pUserID"></param>
        public static void ExportQuesHouse(System.Web.UI.Page page, DataTable pDataTable, string pUserID)
        {
            //物理路径
            string path = page.Request.PhysicalApplicationPath;
            //文件后缀名
            string strExt = ".xls";
            //文件全路径
            string fullpath = path + QuesHouse;  //需要修改的地方
            //文件名称
            string strfileName = "Rpt_QuestHouse.xls";   //需要修改的地方

            //存放临时文件路径
            FileUpDown.DirPath = path + TempPath;
            string strDir = FileUpDown.DirPath + pUserID + "\\";

            //自动创建文件夹
            MyFiles.AutoCreateDirectory(strDir);

            //虚文件名
            String strTmpFileName = FileUpDown.GetFileName(strDir + strfileName.Substring(0, strfileName.Length - strExt.Length) +
                DateTime.Now.ToString("yyyyMMdd") + pUserID, strExt);
            string strFullTmpFileName = strDir + strTmpFileName;

            //插入数据                                 //需要修改的地方
            string[] arrField = { "itilno", "isbudan", "createByName", "cjrbyphone", "createbydeptname", "createdate", "comeindate", "outdate", "execbyname", "execbydeptname", "execbyno", "execbyphone", "Address", "actiontypename", "sqdescr" };

            IDataFieldProcess idfp = new ImplDataFieldProcess();

            ExportExcel.InsertExcel(fullpath, strFullTmpFileName, arrField, pDataTable, idfp);

            //下载文件
            FileUpDown.DownFile(page.Response, strTmpFileName, strFullTmpFileName);

            //删除临时文件
            FileUpDown.FileDelete(strFullTmpFileName);

        }
        #endregion

        #region 导出打印规则模板配置Excel
        /// <summary>
        /// 导出短信模板列表报表Excel
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pDataTable"></param>
        /// <param name="pUserID"></param>
        public static void ExportPrintRuleList(System.Web.UI.Page page, DataTable pDataTable, string pUserID)
        {
            //物理路径
            string path = page.Request.PhysicalApplicationPath;
            //文件后缀名
            string strExt = ".xls";
            //文件全路径
            string fullpath = path + MailMessageTemListType;
            //文件名称
            string strfileName = "Rpt_PrintRule.xls";   //需要修改的地方

            //存放临时文件路径
            FileUpDown.DirPath = path + TempPath;

            string strDir = FileUpDown.DirPath + pUserID + "\\";

            //自动创建文件夹
            MyFiles.AutoCreateDirectory(strDir);


            //虚文件名
            String strTmpFileName = FileUpDown.GetFileName(strDir + strfileName.Substring(0, strfileName.Length - strExt.Length) +
                DateTime.Now.ToString("yyyyMMdd") + pUserID, strExt);
            string strFullTmpFileName = strDir + strTmpFileName;

            //插入数据                                 //需要修改的地方
            string[] arrField = { "PrintRuleName", "AppNames", "FlowModelName", "IsOpen", "Remark" };

            IDataFieldProcess idfp = new ImplDataFieldProcess();

            ExportExcel.InsertExcel(fullpath, strFullTmpFileName, arrField, pDataTable, idfp);

            //下载文件
            FileUpDown.DownFile(page.Response, strTmpFileName, strFullTmpFileName);

            //删除临时文件
            FileUpDown.FileDelete(strFullTmpFileName);

        }
        #endregion

        #region 导出资产明细列表
        /// <summary>
        /// 导出资产明细列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pDataTable"></param>
        /// <param name="pUserID"></param>
        public static void ExportEquDetailLists(System.Web.UI.Page page, DataTable pDataTable, string pUserID)
        {
            //物理路径
            string path = page.Request.PhysicalApplicationPath;
            //文件后缀名
            string strExt = ".xls";
            //文件全路径
            string fullpath = path + EquDetailListType;       //需要修改的地方
            //文件名称
            string strfileName = "Rpt_DeskDetailLists.xls";   //需要修改的地方

            //存放临时文件路径
            FileUpDown.DirPath = path + TempPath;
            string strDir = FileUpDown.DirPath + pUserID + "\\";

            //自动创建文件夹
            MyFiles.AutoCreateDirectory(strDir);

            //虚文件名
            String strTmpFileName = FileUpDown.GetFileName(strDir + strfileName.Substring(0, strfileName.Length - strExt.Length) +
                DateTime.Now.ToString("yyyyMMdd") + pUserID, strExt);
            string strFullTmpFileName = strDir + strTmpFileName;

            //插入数据                                 //需要修改的地方
            string[] arrField = { "CustMastName", "CostomName", "CatalogName", "Name", "Code", "partBankName", "partBranchName", "ServiceBeginTimeA", "ServiceEndTimeA", "EquStatusName", "Props" };

            IDataFieldProcess idfp = new ImplDataFieldProcess();

            ExportExcel.InsertExcel(fullpath, strFullTmpFileName, arrField, pDataTable, idfp);

            //下载文件
            FileUpDown.DownFile(page.Response, strTmpFileName, strFullTmpFileName);

            //删除临时文件
            FileUpDown.FileDelete(strFullTmpFileName);

        }
        #endregion

        #endregion

    }
}
