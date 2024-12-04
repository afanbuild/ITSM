/********************************************
 * ��Ȩ���У������зǷ���Ϣ�������޹�˾
 * 
 * 
 * 
 * ����������ͳһ��Excel�ļ�����
 * ���ߣ�������
 * �������ڣ�2006-10-19
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
    /// ���EXCEL �ֶδ���ʵ����
    /// </summary>
    public class ImplDataFieldProcess : IDataFieldProcess
    {
        static Hashtable _fields;
        static Hashtable _fieldsRel;

        public ImplDataFieldProcess()
        {
            //
            // TODO: �ڴ˴���ӹ��캯���߼�
            //
        }



        /// <summary>
        /// �����ֶν��
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
                        sRet = "���ڴ���";
                    }
                    if (sFieldValue == "30")
                    {
                        sRet = "��������";
                    }
                    if (sFieldValue == "40")
                    {
                        sRet = "������ͣ";
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
        /// ��ȡ�ʲ����õ�������Ϣ������EXCEL ֧�ֵĸ�ʽ
        /// </summary>
        /// <param name="sXml"></param>
        /// <returns></returns>
        private string GetConfigureValueDesc(string sXml)
        {
            string sDesc = "";
            string showBaseValue = "�������ã�";
            string showRelValue = "�������ã�";

            if (_fields == null)
            {
                Equ_SchemaItemsDP ee = new Equ_SchemaItemsDP();

                _fields = ee.GetAllFields(0);   //��ȡ���µ����������
            }
            if (_fieldsRel == null)
            {
                Equ_SchemaItemsDP ee = new Equ_SchemaItemsDP();

                _fieldsRel = ee.GetAllFields(1);   //��ȡ���µ����������[����]
            }
            Hashtable _xmlHt = new Hashtable();

            _xmlHt = SetHashTableXmlValue(sXml);
            //��������
            foreach (System.Collections.DictionaryEntry objDE in _xmlHt)
            {
                string sCHName = "";
                string sID = objDE.Key.ToString();
                string sValue = objDE.Value.ToString();
                if (_fields[sID] != null)
                {
                    sCHName = _fields[sID].ToString();

                }

                if (sCHName.Length > 0 && sValue != string.Empty)     //�������޸�  20090813��ȥ��ֵΪ�յ�
                {
                    showBaseValue += sCHName + ":" + sValue + ";";

                }

            }

            //��������
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

            //����HASHTABLE
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
    /// ExcelExport ��ժҪ˵����
    /// </summary>
    public class ExcelExport
    {
        #region ������

        #region  �¼���������
        private static string _IssuesListType = "ExcelTemplate\\Rpt_IssuesList.xls";
        /// <summary>
        /// ��Ŀ�������Աȱ���ģ��
        /// </summary>
        protected static string IssuesListType
        {
            get
            {
                return _IssuesListType;
            }
        }
        #endregion

        #region  ���⵼������
        private static string _ProblemListType = "ExcelTemplate\\Rpt_ProblemList.xls";
        /// <summary>
        /// ��Ŀ�������Աȱ���ģ��
        /// </summary>
        protected static string ProblemListType
        {
            get
            {
                return _ProblemListType;
            }
        }
        #endregion

        #region  Ͷ�ߵ�������
        private static string _BYListType = "ExcelTemplate\\Rpt_BYList.xls";
        /// <summary>
        /// ��Ŀ�������Աȱ���ģ��
        /// </summary>
        protected static string BYListType
        {
            get
            {
                return _BYListType;
            }
        }
        #endregion

        #region  ֪ʶ��������
        private static string _KBListType = "ExcelTemplate\\Rpt_KBList.xls";
        /// <summary>
        /// ��Ŀ�������Աȱ���ģ��
        /// </summary>
        protected static string KBListType
        {
            get
            {
                return _KBListType;
            }
        }
        #endregion

        #region  �ͻ���������
        private static string _CustListType = "ExcelTemplate\\Rpt_CustList.xls";
        /// <summary>
        /// ��Ŀ�������Աȱ���ģ��
        /// </summary>
        protected static string CustListType
        {
            get
            {
                return _CustListType;
            }
        }
        #endregion

        #region  �ʲ���������
        private static string _DeskListType = "ExcelTemplate\\Rpt_DeskList.xls";
        /// <summary>
        /// ��Ŀ�������Աȱ���ģ��
        /// </summary>
        protected static string DeskListType
        {
            get
            {
                return _DeskListType;
            }
        }
        #endregion

        #region ��������䱨��

        private static string _QuesHouse = "ExcelTemplate\\Rpt_QuestHouse.xls";
        /// <summary>
        /// ��������䱨��
        /// </summary>
        protected static string QuesHouse
        {
            get
            {
                return _QuesHouse;
            }
        }

        #endregion

        #region  �������������
        private static string _ChangeListType = "ExcelTemplate\\Rpt_ChangeList.xls";
        /// <summary>
        /// ��Ŀ�������Աȱ���ģ��
        /// </summary>
        protected static string ChangeListType
        {
            get
            {
                return _ChangeListType;
            }
        }
        #endregion

        #region  �ʲ��̵��������
        private static string _CheckPlanListType = "ExcelTemplate\\Rpt_CheckPlanList.xls";
        /// <summary>
        /// ��Ŀ�������Աȱ���ģ��
        /// </summary>
        protected static string CheckPlanListType
        {
            get
            {
                return _CheckPlanListType;
            }
        }
        #endregion

        #region ��ʱ�ļ�·��
        /// <summary>
        /// ��ʱ�ļ�·��
        /// </summary>
        private static string TempPath
        {
            get { return "ExcelTemplate\\"; }
        }
        #endregion

        #region ����ģ�嵼������
        private static string _MailMessageTemListType = "ExcelTemplate\\Rpt_MailMessageTemList.xls";
        /// <summary>
        /// ����ģ�嵼������
        /// </summary>
        protected static string MailMessageTemListType
        {
            get
            {
                return _MailMessageTemListType;
            }
        }
        #endregion

        #region  ��־����
        private static string _LogListType = "ExcelTemplate\\Rpt_LogList.xls";
        /// <summary>
        /// ��Ŀ�������Աȱ���ģ��
        /// </summary>
        protected static string LogListType
        {
            get
            {
                return _LogListType;
            }
        }
        #endregion

        #region �ʲ���ϸ�б���
        private static string _EquDetailListType = "ExcelTemplate\\Rpt_DeskDetailLists.xls";
        /// <summary>
        /// �ʲ���ϸ�б���
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

        #region ������

        #region �������ⵥ�б���Excel
        /// <summary>
        /// �������ⵥ�б���Excel
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pDataTable"></param>
        /// <param name="pUserID"></param>
        public static void ExportProblemList(System.Web.UI.Page page, DataTable pDataTable, string pUserID)
        {
            //����·��
            string path = page.Request.PhysicalApplicationPath;
            //�ļ���׺��
            string strExt = ".xls";
            //�ļ�ȫ·��
            string fullpath = path + ProblemListType;
            //�ļ�����
            string strfileName = "Rpt_ProblemList.xls";   //��Ҫ�޸ĵĵط�

            //�����ʱ�ļ�·��
            FileUpDown.DirPath = path + TempPath;
            string strDir = FileUpDown.DirPath + pUserID + "\\";

            //�Զ������ļ���
            MyFiles.AutoCreateDirectory(strDir);

            //���ļ���
            String strTmpFileName = FileUpDown.GetFileName(strDir + strfileName.Substring(0, strfileName.Length - strExt.Length) +
                DateTime.Now.ToString("yyyyMMdd") + pUserID, strExt);
            string strFullTmpFileName = strDir + strTmpFileName;

            //��������                                 //��Ҫ�޸ĵĵط�
            string[] arrField = { "ProblemNo", "EquName", "Problem_TypeName", "Problem_LevelName", "EffectName", "InstancyName", "Problem_Title", "Problem_Subject", "StateName", "Remark", "RegUserName" };

            IDataFieldProcess idfp = new ImplDataFieldProcess();

            ExportExcel.InsertExcel(fullpath, strFullTmpFileName, arrField, pDataTable, idfp);

            //�����ļ�
            FileUpDown.DownFile(page.Response, strTmpFileName, strFullTmpFileName);

            //ɾ����ʱ�ļ�
            FileUpDown.FileDelete(strFullTmpFileName);

        }
        /// <summary>
        /// �������ⵥ�б���Excel
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pDataTable"></param>
        /// <param name="pUserID"></param>
        public static void ExportProblemList(System.Web.UI.Page page, DataTable pDataTable, string[] fileName, string[] arrField, string pUserID)
        {
            //����·��
            string path = page.Request.PhysicalApplicationPath;
            //�ļ���׺��
            string strExt = ".xls";
            //�ļ�ȫ·��
            string fullpath = path + ProblemListType;
            //�ļ�����
            string strfileName = "Rpt_ProblemList.xls";   //��Ҫ�޸ĵĵط�

            //�����ʱ�ļ�·��
            FileUpDown.DirPath = path + TempPath;
            string strDir = FileUpDown.DirPath + pUserID + "\\";

            //�Զ������ļ���
            MyFiles.AutoCreateDirectory(strDir);

            //���ļ���
            String strTmpFileName = FileUpDown.GetFileName(strDir + strfileName.Substring(0, strfileName.Length - strExt.Length) +
                DateTime.Now.ToString("yyyyMMdd") + pUserID, strExt);
            string strFullTmpFileName = strDir + strTmpFileName;

            //��������                                 //��Ҫ�޸ĵĵط�
            //string[] arrField = { "ProblemNo", "EquName", "Problem_TypeName", "Problem_LevelName", "EffectName", "InstancyName", "Problem_Title", "Problem_Subject", "StateName", "Remark", "RegUserName" };

            IDataFieldProcess idfp = new ImplDataFieldProcess();

            //ExportExcel.InsertExcel(fullpath, strFullTmpFileName, arrField, pDataTable, idfp);
            ExportExcel.InsertExcel1(fullpath, strFullTmpFileName, fileName, arrField, pDataTable, idfp);

            //�����ļ�
            FileUpDown.DownFile(page.Response, strTmpFileName, strFullTmpFileName);

            //ɾ����ʱ�ļ�
            FileUpDown.FileDelete(strFullTmpFileName);

        }
        #endregion


        #region ������� - 2013-04-27 @������

        /// <summary>
        /// ���������б�ΪExcel
        /// </summary>
        /// <param name="page">ҳ�����</param>
        /// <param name="pDataTable">����Դ��</param>
        /// <param name="fileName">�ֶ���ʾ��������</param>
        /// <param name="arrField">�ֶ���������</param>
        /// <param name="pUserID">�û����</param>
        public static void ExportReqDemandList(
            System.Web.UI.Page page,
            DataTable pDataTable,
            string[] fileName,
            string[] arrField,
            string pUserID)
        {
            //����·��
            string path = page.Request.PhysicalApplicationPath;
            //�ļ���׺��
            string strExt = ".xls";
            //�ļ�ȫ·��
            string fullpath = path + ProblemListType;
            //�ļ�����
            string strfileName = "Rpt_ReqDemandList.xls";   //��Ҫ�޸ĵĵط�

            //�����ʱ�ļ�·��
            FileUpDown.DirPath = path + TempPath;
            string strDir = FileUpDown.DirPath + pUserID + "\\";

            //�Զ������ļ���
            MyFiles.AutoCreateDirectory(strDir);

            //���ļ���
            String strTmpFileName = FileUpDown.GetFileName(strDir + strfileName.Substring(0, strfileName.Length - strExt.Length) +
                DateTime.Now.ToString("yyyyMMdd") + pUserID, strExt);
            string strFullTmpFileName = strDir + strTmpFileName;

            IDataFieldProcess idfp = new ImplDataFieldProcess();

            //ExportExcel.InsertExcel(fullpath, strFullTmpFileName, arrField, pDataTable, idfp);
            ExportExcel.InsertExcel1(fullpath, strFullTmpFileName, fileName, arrField, pDataTable, idfp);

            //�����ļ�
            FileUpDown.DownFile(page.Response, strTmpFileName, strFullTmpFileName);

            //ɾ����ʱ�ļ�
            FileUpDown.FileDelete(strFullTmpFileName);

        }

        #endregion


        #region �����¼��б���Excel
        /// <summary>
        /// ������Ŀ�������Աȱ���Excel
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pDataTable"></param>
        /// <param name="pUserID"></param>
        public static void ExportIssuesList(System.Web.UI.Page page, DataTable pDataTable, string pUserID)
        {
            //����·��
            string path = page.Request.PhysicalApplicationPath;
            //�ļ���׺��
            string strExt = ".xls";
            //�ļ�ȫ·��
            string fullpath = path + IssuesListType;
            //�ļ�����
            string strfileName = "Rpt_IssuesList.xls";   //��Ҫ�޸ĵĵط�

            //�����ʱ�ļ�·��
            FileUpDown.DirPath = path + TempPath;
            string strDir = FileUpDown.DirPath + pUserID + "\\";

            //�Զ������ļ���
            MyFiles.AutoCreateDirectory(strDir);

            //���ļ���
            String strTmpFileName = FileUpDown.GetFileName(strDir + strfileName.Substring(0, strfileName.Length - strExt.Length) +
                DateTime.Now.ToString("yyyyMMdd") + pUserID, strExt);
            string strFullTmpFileName = strDir + strTmpFileName;

            //��������                                 //��Ҫ�޸ĵĵط�
            string[] arrField = { "ServiceNo", "ServiceType", "MastCustName", "CustName", "contact", "CTel", "CustAddress", "EquipmentName", "CustTime", "ReportingTime", "FinishedTime", "ServiceLevel", "EffectName", "InstancyName", "CloseReasonName", "ReSouseName", "DealStatus", "subject", "Content", "status:GetFlowStatus", "Outtime", "ServiceTime", "Sjwxr", "DealContent", "RegUserName" };

            IDataFieldProcess idfp = new ImplDataFieldProcess();

            ExportExcel.InsertExcel(fullpath, strFullTmpFileName, arrField, pDataTable, idfp);

            //�����ļ�
            FileUpDown.DownFile(page.Response, strTmpFileName, strFullTmpFileName);

            //ɾ����ʱ�ļ�
            FileUpDown.FileDelete(strFullTmpFileName);

        }
        #endregion

        #region �����¼��б���Excel
        /// <summary>
        /// ������Ŀ�������Աȱ���Excel
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pDataTable"></param>
        /// <param name="pUserID"></param>
        public static void ExportIssuesList1(System.Web.UI.Page page, DataTable pDataTable, string[] fileName, string[] arrField, string pUserID)
        {
            //����·��
            string path = page.Request.PhysicalApplicationPath;
            //�ļ���׺��
            string strExt = ".xls";
            //�ļ�ȫ·��
            string fullpath = path + IssuesListType;
            //�ļ�����
            string strfileName = "Rpt_IssuesList.xls";   //��Ҫ�޸ĵĵط�

            //�����ʱ�ļ�·��
            FileUpDown.DirPath = path + TempPath;
            string strDir = FileUpDown.DirPath + pUserID + "\\";

            //�Զ������ļ���
            MyFiles.AutoCreateDirectory(strDir);

            //���ļ���
            String strTmpFileName = FileUpDown.GetFileName(strDir + strfileName.Substring(0, strfileName.Length - strExt.Length) +
                DateTime.Now.ToString("yyyyMMdd") + pUserID, strExt);
            string strFullTmpFileName = strDir + strTmpFileName;

            //��������                                 //��Ҫ�޸ĵĵط�
            // string[] arrField = { "ServiceNo", "ServiceType", "MastCustName", "CustName", "contact", "CTel", "CustAddress", "EquipmentName", "CustTime", "ReportingTime", "FinishedTime", "ServiceLevel", "EffectName", "InstancyName", "CloseReasonName", "ReSouseName", "DealStatus", "subject", "Content", "status:GetFlowStatus", "Outtime", "ServiceTime", "Sjwxr", "DealContent", "RegUserName" };

            IDataFieldProcess idfp = new ImplDataFieldProcess();

            ExportExcel.InsertExcel1(fullpath, strFullTmpFileName, fileName, arrField, pDataTable, idfp);

            //�����ļ�
            FileUpDown.DownFile(page.Response, strTmpFileName, strFullTmpFileName);

            //ɾ����ʱ�ļ�
            FileUpDown.FileDelete(strFullTmpFileName);

        }
        #endregion

        #region ����Ͷ�߱�Թ�б���Excel
        /// <summary>
        /// ������Ŀ�������Աȱ���Excel
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pDataTable"></param>
        /// <param name="pUserID"></param>
        public static void ExportBYList(System.Web.UI.Page page, DataTable pDataTable, string pUserID)
        {
            //����·��
            string path = page.Request.PhysicalApplicationPath;
            //�ļ���׺��
            string strExt = ".xls";
            //�ļ�ȫ·��
            string fullpath = path + BYListType;
            //�ļ�����
            string strfileName = "Rpt_BYList.xls";   //��Ҫ�޸ĵĵط�

            //�����ʱ�ļ�·��
            FileUpDown.DirPath = path + TempPath;
            string strDir = FileUpDown.DirPath + pUserID + "\\";

            //�Զ������ļ���
            MyFiles.AutoCreateDirectory(strDir);

            //���ļ���
            String strTmpFileName = FileUpDown.GetFileName(strDir + strfileName.Substring(0, strfileName.Length - strExt.Length) +
                DateTime.Now.ToString("yyyyMMdd") + pUserID, strExt);
            string strFullTmpFileName = strDir + strTmpFileName;

            //��������                                 //��Ҫ�޸ĵĵط�
            string[] arrField = { "flowid", "BY_PersonName", "BY_ProjectName", "BY_SoureName", "BY_TypeName", "BY_KindName", "BY_ReceiveTime", "RegTime", "status:GetFlowStatus" };

            IDataFieldProcess idfp = new ImplDataFieldProcess();

            ExportExcel.InsertExcel(fullpath, strFullTmpFileName, arrField, pDataTable, idfp);

            //�����ļ�
            FileUpDown.DownFile(page.Response, strTmpFileName, strFullTmpFileName);

            //ɾ����ʱ�ļ�
            FileUpDown.FileDelete(strFullTmpFileName);

        }
        #endregion


        #region ����֪ʶ�б���Excel
        /// <summary>
        /// ������Ŀ�������Աȱ���Excel
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pDataTable"></param>
        /// <param name="pUserID"></param>
        public static void ExportKBList(System.Web.UI.Page page, DataTable pDataTable, string pUserID)
        {
            //����·��
            string path = page.Request.PhysicalApplicationPath;
            //�ļ���׺��
            string strExt = ".xls";
            //�ļ�ȫ·��
            string fullpath = path + KBListType;
            //�ļ�����
            string strfileName = "Rpt_KBList.xls";   //��Ҫ�޸ĵĵط�

            //�����ʱ�ļ�·��
            FileUpDown.DirPath = path + TempPath;

            string strDir = FileUpDown.DirPath + pUserID + "\\";

            //�Զ������ļ���
            MyFiles.AutoCreateDirectory(strDir);


            //���ļ���
            String strTmpFileName = FileUpDown.GetFileName(strDir + strfileName.Substring(0, strfileName.Length - strExt.Length) +
                DateTime.Now.ToString("yyyyMMdd") + pUserID, strExt);
            string strFullTmpFileName = strDir + strTmpFileName;

            //��������                                 //��Ҫ�޸ĵĵط�
            string[] arrField = { "id", "Title", "PKey", "TypeName", "Tags" };

            IDataFieldProcess idfp = new ImplDataFieldProcess();

            ExportExcel.InsertExcel(fullpath, strFullTmpFileName, arrField, pDataTable, idfp);

            //�����ļ�
            FileUpDown.DownFile(page.Response, strTmpFileName, strFullTmpFileName);

            //ɾ����ʱ�ļ�
            FileUpDown.FileDelete(strFullTmpFileName);

        }
        #endregion

        #region �����ͻ��б���Excel
        /// <summary>
        /// ������Ŀ�������Աȱ���Excel
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pDataTable"></param>
        /// <param name="pUserID"></param>
        public static void ExportCustList(System.Web.UI.Page page, DataTable pDataTable, string pUserID)
        {
            //����·��
            string path = page.Request.PhysicalApplicationPath;
            //�ļ���׺��
            string strExt = ".xls";
            //�ļ�ȫ·��
            string fullpath = path + CustListType;
            //�ļ�����
            string strfileName = "Rpt_CustList.xls";   //��Ҫ�޸ĵĵط�

            //�����ʱ�ļ�·��
            FileUpDown.DirPath = path + TempPath;

            string strDir = FileUpDown.DirPath + pUserID + "\\";

            //�Զ������ļ���
            MyFiles.AutoCreateDirectory(strDir);


            //���ļ���
            String strTmpFileName = FileUpDown.GetFileName(strDir + strfileName.Substring(0, strfileName.Length - strExt.Length) +
                DateTime.Now.ToString("yyyyMMdd") + pUserID, strExt);
            string strFullTmpFileName = strDir + strTmpFileName;

            //��������                                 //��Ҫ�޸ĵĵط�
            string[] arrField = { "id", "MastCustID:GetMastCustName", "CustDeptName", "ShortName", "FullName", "CustomCode", "CustomerTypeName", "Email", "LinkMan1", "Tel1", "Address", "Rights" };

            IDataFieldProcess idfp = new ImplDataFieldProcess();

            ExportExcel.InsertExcel(fullpath, strFullTmpFileName, arrField, pDataTable, idfp);

            //�����ļ�
            FileUpDown.DownFile(page.Response, strTmpFileName, strFullTmpFileName);

            //ɾ����ʱ�ļ�
            FileUpDown.FileDelete(strFullTmpFileName);

        }
        #endregion

        #region �����ʲ��б���Excel
        /// <summary>
        /// ������Ŀ�������Աȱ���Excel
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pDataTable"></param>
        /// <param name="pUserID"></param>
        public static void ExportDeskList(System.Web.UI.Page page, DataTable pDataTable, string pUserID)
        {
            //����·��
            string path = page.Request.PhysicalApplicationPath;
            //�ļ���׺��
            string strExt = ".xls";
            //�ļ�ȫ·��
            string fullpath = path + DeskListType;
            //�ļ�����
            string strfileName = "Rpt_DeskList.xls";   //��Ҫ�޸ĵĵط�

            //�����ʱ�ļ�·��
            FileUpDown.DirPath = path + TempPath;

            string strDir = FileUpDown.DirPath + pUserID + "\\";

            //�Զ������ļ���
            MyFiles.AutoCreateDirectory(strDir);

            //���ļ���
            String strTmpFileName = FileUpDown.GetFileName(strDir + strfileName.Substring(0, strfileName.Length - strExt.Length) +
                DateTime.Now.ToString("yyyyMMdd") + pUserID, strExt);
            string strFullTmpFileName = strDir + strTmpFileName;

            //��������                                 //��Ҫ�޸ĵĵط�
            string[] arrField = { "Code", "Name", "CatalogName", "listName", "partBankName", "partBranchName", "CostomName", "ServiceBeginTime", "ServiceEndTime", "EquStatusName", "ConfigureValue:ParaConfigureValue" };

            IDataFieldProcess idfp = new ImplDataFieldProcess();

            ExportExcel.InsertExcel(fullpath, strFullTmpFileName, arrField, pDataTable, idfp);

            //�����ļ�
            FileUpDown.DownFile(page.Response, strTmpFileName, strFullTmpFileName);

            //ɾ����ʱ�ļ�
            FileUpDown.FileDelete(strFullTmpFileName);

        }
        #endregion

        #region �����ʲ��̵��б���Excel
        /// <summary>
        /// �����ʲ��̵�ƻ�����Excel
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pDataTable"></param>
        /// <param name="pUserID"></param>
        public static void ExportCheckPlanList(System.Web.UI.Page page, DataTable pDataTable, string pUserID)
        {
            //����·��
            string path = page.Request.PhysicalApplicationPath;
            //�ļ���׺��
            string strExt = ".xls";
            //�ļ�ȫ·��
            string fullpath = path + CheckPlanListType;
            //�ļ�����
            string strfileName = "Rpt_CheckPlanList.xls";   //��Ҫ�޸ĵĵط�

            //�����ʱ�ļ�·��
            FileUpDown.DirPath = path + TempPath;

            string strDir = FileUpDown.DirPath + pUserID + "\\";

            //�Զ������ļ���
            MyFiles.AutoCreateDirectory(strDir);

            //���ļ���
            String strTmpFileName = FileUpDown.GetFileName(strDir + strfileName.Substring(0, strfileName.Length - strExt.Length) +
                DateTime.Now.ToString("yyyyMMdd") + pUserID, strExt);
            string strFullTmpFileName = strDir + strTmpFileName;

            //��������                                 //��Ҫ�޸ĵĵط�
            string[] arrField = { "Code", "Name", "SerialNumber", "Positions", "CostomName", "EquStatusName", "ConfigureValue:ParaConfigureValue" };

            IDataFieldProcess idfp = new ImplDataFieldProcess();

            ExportExcel.InsertExcel(fullpath, strFullTmpFileName, arrField, pDataTable, idfp);

            //�����ļ�
            FileUpDown.DownFile(page.Response, strTmpFileName, strFullTmpFileName);

            //ɾ����ʱ�ļ�
            FileUpDown.FileDelete(strFullTmpFileName);

        }
        #endregion

        #region ��������б���Excel
        /// <summary>
        /// ��������б���Excel
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pDataTable"></param>
        /// <param name="pUserID"></param>
        public static void ExportChangeList(System.Web.UI.Page page, DataTable pDataTable, string pUserID)
        {
            //����·��
            string path = page.Request.PhysicalApplicationPath;
            //�ļ���׺��
            string strExt = ".xls";
            //�ļ�ȫ·��
            string fullpath = path + ChangeListType;  //��Ҫ�޸ĵĵط�
            //�ļ�����
            string strfileName = "Rpt_ChangeList.xls";   //��Ҫ�޸ĵĵط�

            //�����ʱ�ļ�·��
            FileUpDown.DirPath = path + TempPath;
            string strDir = FileUpDown.DirPath + pUserID + "\\";

            //�Զ������ļ���
            MyFiles.AutoCreateDirectory(strDir);

            //���ļ���
            String strTmpFileName = FileUpDown.GetFileName(strDir + strfileName.Substring(0, strfileName.Length - strExt.Length) +
                DateTime.Now.ToString("yyyyMMdd") + pUserID, strExt);
            string strFullTmpFileName = strDir + strTmpFileName;

            //��������                                 //��Ҫ�޸ĵĵط�
            string[] arrField = { "ChangeNo", "ChangeTypeName", "RegTime", "MastCustName", "CustName", "CTel", "CustAddress", "Subject", "Content", "EffectName", "InstancyName", "LevelName", "DealStatus", "ChangeAnalyses", "ChangeAnalysesResult", "ChangeTime", "DealStatus", "TPersonList" };

            IDataFieldProcess idfp = new ImplDataFieldProcess();

            ExportExcel.InsertExcel(fullpath, strFullTmpFileName, arrField, pDataTable, idfp);

            //�����ļ�
            FileUpDown.DownFile(page.Response, strTmpFileName, strFullTmpFileName);

            //ɾ����ʱ�ļ�
            FileUpDown.FileDelete(strFullTmpFileName);

        }
        /// <summary>
        /// ��������б���Excel
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pDataTable"></param>
        /// <param name="pUserID"></param>
        public static void ExportChangeList(System.Web.UI.Page page, DataTable pDataTable, string[] fileName, string[] arrField, string pUserID)
        {
            //����·��
            string path = page.Request.PhysicalApplicationPath;
            //�ļ���׺��
            string strExt = ".xls";
            //�ļ�ȫ·��
            string fullpath = path + ChangeListType;  //��Ҫ�޸ĵĵط�
            //�ļ�����
            string strfileName = "Rpt_ChangeList.xls";   //��Ҫ�޸ĵĵط�

            //�����ʱ�ļ�·��
            FileUpDown.DirPath = path + TempPath;
            string strDir = FileUpDown.DirPath + pUserID + "\\";

            //�Զ������ļ���
            MyFiles.AutoCreateDirectory(strDir);

            //���ļ���
            String strTmpFileName = FileUpDown.GetFileName(strDir + strfileName.Substring(0, strfileName.Length - strExt.Length) +
                DateTime.Now.ToString("yyyyMMdd") + pUserID, strExt);
            string strFullTmpFileName = strDir + strTmpFileName;

            //��������                                 //��Ҫ�޸ĵĵط�
            //string[] arrField = { "ChangeNo", "ChangeTypeName", "RegTime", "MastCustName", "CustName", "CTel", "CustAddress", "Subject", "Content", "EffectName", "InstancyName", "LevelName", "DealStatus", "ChangeAnalyses", "ChangeAnalysesResult", "ChangeTime", "DealStatus", "TPersonList" };

            IDataFieldProcess idfp = new ImplDataFieldProcess();

            ExportExcel.InsertExcel1(fullpath, strFullTmpFileName, fileName, arrField, pDataTable, idfp);

            //�����ļ�
            FileUpDown.DownFile(page.Response, strTmpFileName, strFullTmpFileName);

            //ɾ����ʱ�ļ�
            FileUpDown.FileDelete(strFullTmpFileName);

        }
        #endregion

        #region ���ؿͻ����ϵ���ģ��Excel
        /// <summary>
        /// ���ؿͻ����ϵ���ģ��Excel
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pUserID"></param>
        public static void DownLoadCustExcel(System.Web.UI.Page page, string pUserID)
        {
            string CustType = "ExcelTemplate\\�ͻ����ϵ���ģ��.xls";
            //����·��
            string path = page.Request.PhysicalApplicationPath;
            //�ļ�ȫ·��
            string fullpath = path + CustType;
            //�ļ�����
            string strfileName = "�ͻ����ϵ���ģ��.xls";   //��Ҫ�޸ĵĵط�

            //�����ļ�
            FileUpDown.DownFile(page.Response, strfileName, fullpath);

        }
        #endregion

        #region �����ʲ����ϵ���ģ��Excel
        /// <summary>
        /// �����ʲ����ϵ���ģ��Excel
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pUserID"></param>
        public static void DownLoadEquExcel(System.Web.UI.Page page, string pUserID)
        {
            string EquType = "ExcelTemplate\\�ʲ����ϵ���ģ��.xls";
            //����·��
            string path = page.Request.PhysicalApplicationPath;
            //�ļ�ȫ·��
            string fullpath = path + EquType;
            //�ļ�����
            string strfileName = "�ʲ����ϵ���ģ��.xls";   //��Ҫ�޸ĵĵط�

            //�����ļ�
            FileUpDown.DownFile(page.Response, strfileName, fullpath);

        }
        #endregion

        #region ������־�б���Excel
        /// <summary>
        /// ������־�б���Excel
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pDataTable"></param>
        /// <param name="pUserID"></param>
        public static void ExportLogList(System.Web.UI.Page page, DataTable pDataTable, string pUserID)
        {
            //����·��
            string path = page.Request.PhysicalApplicationPath;
            //�ļ���׺��
            string strExt = ".xls";
            //�ļ�ȫ·��
            string fullpath = path + LogListType;
            //�ļ�����
            string strfileName = "Rpt_LogList.xls";   //��Ҫ�޸ĵĵط�

            //�����ʱ�ļ�·��
            FileUpDown.DirPath = path + TempPath;
            string strDir = FileUpDown.DirPath + pUserID + "\\";

            //�Զ������ļ���
            MyFiles.AutoCreateDirectory(strDir);

            //���ļ���
            String strTmpFileName = FileUpDown.GetFileName(strDir + strfileName.Substring(0, strfileName.Length - strExt.Length) +
                DateTime.Now.ToString("yyyyMMdd") + pUserID, strExt);
            string strFullTmpFileName = strDir + strTmpFileName;

            //��������                                 //��Ҫ�޸ĵĵط�
            string[] arrField = { "SysName", "UserName", "Dept", "IPAddress", "OPEndTime", "ActionName", "Memo" };

            IDataFieldProcess idfp = new ImplDataFieldProcess();

            ExportExcel.InsertExcel(fullpath, strFullTmpFileName, arrField, pDataTable, idfp);

            //�����ļ�
            FileUpDown.DownFile(page.Response, strTmpFileName, strFullTmpFileName);

            //ɾ����ʱ�ļ�
            FileUpDown.FileDelete(strFullTmpFileName);

        }
        #endregion

        #region ��������ģ���б���Excel
        /// <summary>
        /// ��������ģ���б���Excel
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pDataTable"></param>
        /// <param name="pUserID"></param>
        public static void ExportMailMessageTemList(System.Web.UI.Page page, DataTable pDataTable, string pUserID)
        {
            //����·��
            string path = page.Request.PhysicalApplicationPath;
            //�ļ���׺��
            string strExt = ".xls";
            //�ļ�ȫ·��
            string fullpath = path + MailMessageTemListType;
            //�ļ�����
            string strfileName = "Rpt_MailMessageTemList.xls";   //��Ҫ�޸ĵĵط�

            //�����ʱ�ļ�·��
            FileUpDown.DirPath = path + TempPath;

            string strDir = FileUpDown.DirPath + pUserID + "\\";

            //�Զ������ļ���
            MyFiles.AutoCreateDirectory(strDir);


            //���ļ���
            String strTmpFileName = FileUpDown.GetFileName(strDir + strfileName.Substring(0, strfileName.Length - strExt.Length) +
                DateTime.Now.ToString("yyyyMMdd") + pUserID, strExt);
            string strFullTmpFileName = strDir + strTmpFileName;

            //��������                                 //��Ҫ�޸ĵĵط�
            string[] arrField = { "TemplateName", "MailContent", "ModelContent", "Status", "RegTime", "RegUserName", "RegDeptName" };

            IDataFieldProcess idfp = new ImplDataFieldProcess();

            ExportExcel.InsertExcel(fullpath, strFullTmpFileName, arrField, pDataTable, idfp);

            //�����ļ�
            FileUpDown.DownFile(page.Response, strTmpFileName, strFullTmpFileName);

            //ɾ����ʱ�ļ�
            FileUpDown.FileDelete(strFullTmpFileName);

        }
        #endregion

        #region �����������б���Excel
        /// <summary>
        /// �����������б���Excel
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pDataTable"></param>
        /// <param name="pUserID"></param>
        public static void ExportQuesHouse(System.Web.UI.Page page, DataTable pDataTable, string pUserID)
        {
            //����·��
            string path = page.Request.PhysicalApplicationPath;
            //�ļ���׺��
            string strExt = ".xls";
            //�ļ�ȫ·��
            string fullpath = path + QuesHouse;  //��Ҫ�޸ĵĵط�
            //�ļ�����
            string strfileName = "Rpt_QuestHouse.xls";   //��Ҫ�޸ĵĵط�

            //�����ʱ�ļ�·��
            FileUpDown.DirPath = path + TempPath;
            string strDir = FileUpDown.DirPath + pUserID + "\\";

            //�Զ������ļ���
            MyFiles.AutoCreateDirectory(strDir);

            //���ļ���
            String strTmpFileName = FileUpDown.GetFileName(strDir + strfileName.Substring(0, strfileName.Length - strExt.Length) +
                DateTime.Now.ToString("yyyyMMdd") + pUserID, strExt);
            string strFullTmpFileName = strDir + strTmpFileName;

            //��������                                 //��Ҫ�޸ĵĵط�
            string[] arrField = { "itilno", "isbudan", "createByName", "cjrbyphone", "createbydeptname", "createdate", "comeindate", "outdate", "execbyname", "execbydeptname", "execbyno", "execbyphone", "Address", "actiontypename", "sqdescr" };

            IDataFieldProcess idfp = new ImplDataFieldProcess();

            ExportExcel.InsertExcel(fullpath, strFullTmpFileName, arrField, pDataTable, idfp);

            //�����ļ�
            FileUpDown.DownFile(page.Response, strTmpFileName, strFullTmpFileName);

            //ɾ����ʱ�ļ�
            FileUpDown.FileDelete(strFullTmpFileName);

        }
        #endregion

        #region ������ӡ����ģ������Excel
        /// <summary>
        /// ��������ģ���б���Excel
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pDataTable"></param>
        /// <param name="pUserID"></param>
        public static void ExportPrintRuleList(System.Web.UI.Page page, DataTable pDataTable, string pUserID)
        {
            //����·��
            string path = page.Request.PhysicalApplicationPath;
            //�ļ���׺��
            string strExt = ".xls";
            //�ļ�ȫ·��
            string fullpath = path + MailMessageTemListType;
            //�ļ�����
            string strfileName = "Rpt_PrintRule.xls";   //��Ҫ�޸ĵĵط�

            //�����ʱ�ļ�·��
            FileUpDown.DirPath = path + TempPath;

            string strDir = FileUpDown.DirPath + pUserID + "\\";

            //�Զ������ļ���
            MyFiles.AutoCreateDirectory(strDir);


            //���ļ���
            String strTmpFileName = FileUpDown.GetFileName(strDir + strfileName.Substring(0, strfileName.Length - strExt.Length) +
                DateTime.Now.ToString("yyyyMMdd") + pUserID, strExt);
            string strFullTmpFileName = strDir + strTmpFileName;

            //��������                                 //��Ҫ�޸ĵĵط�
            string[] arrField = { "PrintRuleName", "AppNames", "FlowModelName", "IsOpen", "Remark" };

            IDataFieldProcess idfp = new ImplDataFieldProcess();

            ExportExcel.InsertExcel(fullpath, strFullTmpFileName, arrField, pDataTable, idfp);

            //�����ļ�
            FileUpDown.DownFile(page.Response, strTmpFileName, strFullTmpFileName);

            //ɾ����ʱ�ļ�
            FileUpDown.FileDelete(strFullTmpFileName);

        }
        #endregion

        #region �����ʲ���ϸ�б�
        /// <summary>
        /// �����ʲ���ϸ�б�
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pDataTable"></param>
        /// <param name="pUserID"></param>
        public static void ExportEquDetailLists(System.Web.UI.Page page, DataTable pDataTable, string pUserID)
        {
            //����·��
            string path = page.Request.PhysicalApplicationPath;
            //�ļ���׺��
            string strExt = ".xls";
            //�ļ�ȫ·��
            string fullpath = path + EquDetailListType;       //��Ҫ�޸ĵĵط�
            //�ļ�����
            string strfileName = "Rpt_DeskDetailLists.xls";   //��Ҫ�޸ĵĵط�

            //�����ʱ�ļ�·��
            FileUpDown.DirPath = path + TempPath;
            string strDir = FileUpDown.DirPath + pUserID + "\\";

            //�Զ������ļ���
            MyFiles.AutoCreateDirectory(strDir);

            //���ļ���
            String strTmpFileName = FileUpDown.GetFileName(strDir + strfileName.Substring(0, strfileName.Length - strExt.Length) +
                DateTime.Now.ToString("yyyyMMdd") + pUserID, strExt);
            string strFullTmpFileName = strDir + strTmpFileName;

            //��������                                 //��Ҫ�޸ĵĵط�
            string[] arrField = { "CustMastName", "CostomName", "CatalogName", "Name", "Code", "partBankName", "partBranchName", "ServiceBeginTimeA", "ServiceEndTimeA", "EquStatusName", "Props" };

            IDataFieldProcess idfp = new ImplDataFieldProcess();

            ExportExcel.InsertExcel(fullpath, strFullTmpFileName, arrField, pDataTable, idfp);

            //�����ļ�
            FileUpDown.DownFile(page.Response, strTmpFileName, strFullTmpFileName);

            //ɾ����ʱ�ļ�
            FileUpDown.FileDelete(strFullTmpFileName);

        }
        #endregion

        #endregion

    }
}
