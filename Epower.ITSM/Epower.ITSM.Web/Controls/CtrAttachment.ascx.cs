/*******************************************************************
 * ��Ȩ���У�
 * Description�������������ؼ�
 * 
 * 
 * Create By  ��
 * Create Date��
 * *****************************************************************/
using System;
using System.IO;
using System.Data;
using System.Drawing;
using System.Web;
using System.Xml;
using System.Net;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Epower.DevBase.BaseTools;
using Epower.ITSM.SqlDAL;
using Epower.ITSM.Base;
using EpowerGlobal;
using EpowerCom;
using System.Collections;

namespace Epower.ITSM.Web.Controls
{
    /// <summary>
    ///		CtrAttachEdit ��ժҪ˵����
    /// </summary>
    public partial class CtrAttachment : System.Web.UI.UserControl
    {
        private string strTmpCatalog = "";
        private string strFileCatalog = "";
        private eOA_FlowControlState cState = eOA_FlowControlState.eNormal;
        private bool _loadStateFlag = false;

        #region ����

        /// <summary>
        /// ������� ȱʡΪ���̸���
        /// </summary>
        public eOA_AttachmentType AttachmentType
        {
            get
            {
                if (ViewState[this.ID + "AttachType"] == null)
                    return eOA_AttachmentType.eNormal;
                else
                    return (eOA_AttachmentType)int.Parse(ViewState[this.ID + "AttachType"].ToString());
            }
            set { ViewState[this.ID + "AttachType"] = (int)value; }
        }
        /// <summary>
        /// ������ʱ��� ȱʡΪ���̸���
        /// </summary>
        public eOA_AttachmentType TempAttachmentType
        {
            get
            {
                if (ViewState[this.ID + "TempAttachType"] == null)
                    return eOA_AttachmentType.eNormal;
                else
                    return (eOA_AttachmentType)int.Parse(ViewState[this.ID + "TempAttachType"].ToString());
            }
            set { ViewState[this.ID + "TempAttachType"] = (int)value; }
        }

        /// <summary>
        /// �����Ĺؼ��ֱ�� 
        /// ȱʡʱ������ID, ����������ݸ������
        /// </summary>
        public long TempFlowID
        {
            get
            {
                if (ViewState[this.ID + "TempFlowID"] == null)
                    return 0;
                else
                    return StringTool.String2Long(ViewState[this.ID + "TempFlowID"].ToString());
            }
            set { ViewState[this.ID + "TempFlowID"] = value; }
        }

        /// <summary>
        /// Ӧ��ID
        /// </summary>
        public long AppID
        {
            get
            {
                if (ViewState[this.ID + "AppID"] == null)
                    return 0;
                else
                    return StringTool.String2Long(ViewState[this.ID + "AppID"].ToString());
            }
            set { ViewState[this.ID + "AppID"] = value; }
        }

        /// <summary>
        /// ����ģ��ID
        /// </summary>
        public long FlowModelID
        {
            get
            {
                if (ViewState[this.ID + "FlowModelID"] == null)
                    return 0;
                else
                    return StringTool.String2Long(ViewState[this.ID + "FlowModelID"].ToString());
            }
            set { ViewState[this.ID + "FlowModelID"] = value; }
        }

        /// <summary>
        /// �����Ĺؼ��ֱ�� 
        /// ȱʡʱ������ID, ����������ݸ������
        /// </summary>
        public long FlowID
        {
            get
            {
                if (ViewState[this.ID + "FlowID"] == null)
                    return 0;
                else
                    return StringTool.String2Long(ViewState[this.ID + "FlowID"].ToString());
            }
            set { ViewState[this.ID + "FlowID"] = value; }
        }

        /// <summary>
        /// ���̻���ģ�ͱ�� 
        /// </summary>
        public long NodeModelID
        {
            get
            {
                if (ViewState[this.ID + "NodeModelID"] == null)
                    return 0;
                else
                    return StringTool.String2Long(ViewState[this.ID + "NodeModelID"].ToString());
            }
            set { ViewState[this.ID + "NodeModelID"] = value; }
        }

        public string AttachXML
        {
            get { return ViewState[this.ID + "pAttachment"].ToString(); }
        }

        public bool ReadOnly
        {
            set
            {
                ViewState[this.ID + "ReadOnly"] = value;
                SetReadOnly();
            }
            get
            {
                if (ViewState[this.ID + "ReadOnly"] == null)
                    return false;
                else
                    return (bool)ViewState[this.ID + "ReadOnly"];
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string AttachRight
        {
            set
            {
                ViewState[this.ID + "AttachRight"] = value;
                SetAttRight();
            }
            get
            {
                if (ViewState[this.ID + "AttachRight"] == null)
                    return "0";
                else
                    return ViewState[this.ID + "AttachRight"].ToString();
            }
        }

        /// <summary>
        /// �Ƿ����¼���
        /// </summary>
        public bool LoadStateFlag
        {
            get { return _loadStateFlag; }
            set { _loadStateFlag = value; }
        }

        /// <summary>
        /// Share Point �ĵ���վ��
        /// </summary>
        public string SpsWebSite
        {
            set
            {
                ViewState[this.ID + "SpsWebSite"] = value;
            }
            get
            {
                if (ViewState[this.ID + "SpsWebSite"] == null)
                    return "";
                else
                    return ViewState[this.ID + "SpsWebSite"].ToString();
            }
        }

        /// <summary>
        /// Share Point �ĵ����е�Ŀ¼
        /// </summary>
        public string SpsFolder
        {
            set
            {
                ViewState[this.ID + "SpsFolder"] = value;
            }
            get
            {
                if (ViewState[this.ID + "SpsFolder"] == null)
                    return "";
                else
                    return ViewState[this.ID + "SpsFolder"].ToString();
            }
        }

        /// <summary>
        /// �ļ���ǰ׺
        /// </summary>
        public string FilePrefix
        {
            set
            {
                ViewState[this.ID + "FilePrefix"] = value;
            }
            get
            {
                if (ViewState[this.ID + "FilePrefix"] == null)
                    return "";
                else
                    return ViewState[this.ID + "FilePrefix"].ToString();
            }
        }

        /// <summary>
        /// �ϴ���SPS�ϵ��ļ���С����,��λ(M)
        /// 0������
        /// </summary>
        public int SizeLimitUpSPS
        {
            set
            {
                ViewState[this.ID + "SizeLimitUpSPS"] = value;
            }
            get
            {
                if (ViewState[this.ID + "SizeLimitUpSPS"] == null)
                    return 0;
                else
                    return StringTool.String2Int(ViewState[this.ID + "SizeLimitUpSPS"].ToString());
            }
        }

        /// <summary>
        /// ����ϴ��ļ���С
        /// </summary>
        public string strMaxFileSize
        {
            get
            {
                if (hidMaxFileSize.Value.Trim() == string.Empty)
                    return "4";
                else
                    return hidMaxFileSize.Value;
            }
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, System.EventArgs e)
        {
            strTmpCatalog = CommonDP.GetConfigValue("TempCataLog", "TempCataLog");
            strFileCatalog = CommonDP.GetConfigValue("TempCataLog", "FileCataLog");
            if (CommonDP.GetConfigValue("TempCataLog", "MaxFileSize") != null)
            {
                hidMaxFileSize.Value = CommonDP.GetConfigValue("TempCataLog", "MaxFileSize");
            }
            this.Page.ClientScript.RegisterStartupScript(this.GetType(), "uploadinit", "try{UploadInit();}catch(e){}", true);

            if (!IsPostBack)
            {

                if (TempFlowID != 0)   //�����ʱ��������FLOWID��Ϊ0����������
                {
                    LoadTempFile(this.TempAttachmentType, this.TempFlowID);
                }
                else
                {
                    LoadAttachment(this.AttachmentType, this.FlowID);
                }

                #region ��ӵĴ����
                //******************************************************************************* 
                //* �� �� ���� DW��Ѷ����
                //* �� �� �ˣ� ��ӱ��(chenyingpeng@d-wolves.com)
                //* ������ڣ� 2010��06��25��
                //******************************************************************************* 
                if (cState != eOA_FlowControlState.eNormal)
                {
                    ReadOnly = true;
                    SetReadOnly();
                    if (cState == eOA_FlowControlState.eHidden)
                    {
                        ReadOnly = false;
                        SetReadOnly();
                    }
                }
                #endregion

                #region ��ӵĴ����
                //******************************************************************************* 
                //* �� �� ���� DW��Ѷ����
                //* �� �� �ˣ� ��ӱ��(chenyingpeng@d-wolves.com)
                //* ������ڣ� 2010��06��25��
                //******************************************************************************* 
                if (IsPostBack)
                {
                    if (LoadStateFlag)
                    {
                        if (cState != eOA_FlowControlState.eNormal)
                        {
                            ReadOnly = true;
                            SetReadOnly();
                            if (cState == eOA_FlowControlState.eHidden)
                            {
                                ReadOnly = false;
                                SetReadOnly();
                            }
                        }
                        else
                        {
                            SetReadOnly();
                        }
                    }
                }
                #endregion

                GridBind(this.FlowID.ToString());
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private void SetReadOnly()
        {
            if (this.ReadOnly)
            {
                dgAttachment.Columns[3].Visible = false;
                trAdd.Visible = false;
                trRemark.Visible = false;
            }
            else
            {
                dgAttachment.Columns[3].Visible = true;
                trAdd.Visible = true;
                trRemark.Visible = true;
            }

        }

        #region ����Ȩ�޿���
        /// <summary>
        /// ����Ȩ�޿���
        /// </summary>
        private void SetAttRight()
        {
            if (AttachRight == "0" || AttachRight == "1")   //û������Ȩ��
            {
                return;
            }
            string strAttRight = DecToBin(int.Parse(AttachRight));

            if (strAttRight.Length < 5)   //Ȩ�޳����쳣���˳�
                return;


            dgAttachment.Columns[3].Visible = false;
            trAdd.Visible = false;
            trRemark.Visible = false;
            ButYC.Visible = false;
            ButXS.Visible = false;
            if (strAttRight[0].ToString() == "1")   //�鿴��Ȩ��  
            {
                dgAttachment.Columns[3].Visible = false;
                ButYC.Visible = true;
                ButXS.Visible = true;
            }
            if (strAttRight[1].ToString() == "1")   //������Ȩ��  
            {
                trAdd.Visible = true;
                trRemark.Visible = true;
            }
            if (strAttRight[2].ToString() == "1")   //�޸���Ȩ��  
            {
                dgAttachment.Columns[3].Visible = true;
            }
            if (strAttRight[3].ToString() == "1")   //ɾ����Ȩ��  
            {
                dgAttachment.Columns[3].Visible = true;
            }
        }

        /// <summary>
        /// ȡ��Ȩ��
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        public e_TrueOrFalse GetAttRight(e_AttachmentRight eAttRight)
        {
            string strAttRight = DecToBin(int.Parse(AttachRight));
            if (strAttRight.Length < 5)   //Ȩ�޳����쳣���˳�
                return e_TrueOrFalse.eFalse;
            if (eAttRight == e_AttachmentRight.eRead)
            {
                return (e_TrueOrFalse)int.Parse(strAttRight[0].ToString());
            }
            else if (eAttRight == e_AttachmentRight.eNew)
            {
                return (e_TrueOrFalse)int.Parse(strAttRight[1].ToString());
            }
            else if (eAttRight == e_AttachmentRight.eEdit)
            {
                return (e_TrueOrFalse)int.Parse(strAttRight[2].ToString());
            }
            else if (eAttRight == e_AttachmentRight.eDelete)
            {
                return (e_TrueOrFalse)int.Parse(strAttRight[3].ToString());
            }
            else if (eAttRight == e_AttachmentRight.eMustInput)
            {
                return (e_TrueOrFalse)int.Parse(strAttRight[4].ToString());
            }
            else
            {
                return e_TrueOrFalse.eFalse;
            }
        }
        /// <summary>
        /// ʮ����ת������
        /// </summary>
        /// <returns></returns>
        private string DecToBin(int dNumber)
        {
            string strNumber = string.Empty;
            int iTemp;
            int i = 0;
            while (dNumber > 0)
            {
                iTemp = dNumber % 2;
                dNumber = dNumber / 2;
                strNumber = iTemp + strNumber;
                i++;
            }
            return strNumber;
        }
        #endregion

        #region ���ظ���
        /// <summary>
        /// ���ظ���
        /// </summary>
        /// <param name="FlowID"></param>
        private void LoadAttachment(eOA_AttachmentType eAttachmentType, long FlowID)
        {
            string sAttXML = "";
            if (eAttachmentType == eOA_AttachmentType.eNormal)
            {
                sAttXML = Message.GetAttachmentXml(FlowID);
            }
            else if (eAttachmentType == eOA_AttachmentType.eNews)
            {
                sAttXML = NewsEntity.GetAttachmentXml(FlowID);
            }
            else if (eAttachmentType == eOA_AttachmentType.eZZ)
            {
                sAttXML = EA_ServicesTemplateDP.GetAttachmentXml(FlowID);
            }
            else if (eAttachmentType == eOA_AttachmentType.eZC)
            {
                //�ʲ�
                sAttXML = Equ_DeskDP.GetAttachmentXml(FlowID);
            }
            else
            { 
                sAttXML = Inf_InformationDP.GetAttachmentXml(FlowID);
            }

            ViewState[this.ID + "pAttachment"] = sAttXML;

            DoDataBind(sAttXML);

            if (sAttXML == "<Attachments />" && this.ReadOnly)
            {
                tabMain.Visible = false;
                dgAttachment.Visible = false;
                labMsg.Text = "�޸���";
                labMsg.Visible = true;
            }
            else
            {
                labMsg.Visible = false;
            }
        }
        #endregion

        #region ��Ӹ���
        /// <summary>
        /// ��Ӹ���
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdAttach_Click(object sender, System.EventArgs e)
        {
            long lngNextFileID = 0;

            string strFileName = "";
            string strFName = "";   //����·�����ļ���
            string strAttachXml = "";
            long lngUserID = (long)Session["UserID"];
            string strPersonName = Session["PersonName"].ToString();

            e_FileStatus lngFileStatus = 0;
            e_FileStatus lngOriStatus = 0;

            string strTmpSubPath = "";    //������ʱ��·��,��ֹ���û���ͻ
            string strTmpPath = "";

            #region yxq �޸ĳɶ฽���ϴ���ʽ 2013-2-6

            hidbackData.Value = hidbackData.Value.Trim('*');

            if (hidbackData.Value.Trim() != "")
            {
                strAttachXml = ViewState[this.ID + "pAttachment"].ToString();
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(strAttachXml);


                if (hidbackData.Value.Contains("*"))
                {
                    string[] arrData = hidbackData.Value.Split('*');
                    for (int i = 0; i < arrData.Length; i++)
                    {
                        string[] arr = arrData[i].Split('|');

                        strFName = StringTool.sfReplaceFileNameSymol(GetFileName(arr[0]));
                        strTmpSubPath = arr[1];
                        lngNextFileID = long.Parse(arr[2]);

                        xmlDoc.DocumentElement.SetAttribute("TempSubPath", strTmpSubPath);

                        lngFileStatus = e_FileStatus.efsNew;

                        XmlElement xmlEl = xmlDoc.CreateElement("Attachment");
                        xmlEl.SetAttribute("FileID", lngNextFileID.ToString());
                        xmlEl.SetAttribute("FileName", strFName);
                        xmlEl.SetAttribute("SufName", GetSufName(GetFileName(arr[0])));
                        xmlEl.SetAttribute("Status", ((int)lngFileStatus).ToString());
                        xmlEl.SetAttribute("upTime", System.DateTime.Now.ToString());
                        xmlEl.SetAttribute("upUserID", lngUserID.ToString());
                        xmlEl.SetAttribute("upUserName", strPersonName);
                        xmlEl.SetAttribute("replace", "");
                        xmlEl.SetAttribute("OriginID", NodeModelID.ToString());
                        xmlDoc.DocumentElement.AppendChild(xmlEl);
                    }
                }
                else
                {
                    string[] arr = hidbackData.Value.Split('|');

                    strFName = StringTool.sfReplaceFileNameSymol(GetFileName(arr[0]));
                    strTmpSubPath = arr[1];
                    lngNextFileID = long.Parse(arr[2]);

                    xmlDoc.DocumentElement.SetAttribute("TempSubPath", strTmpSubPath);

                    lngFileStatus = e_FileStatus.efsNew;

                    XmlElement xmlEl = xmlDoc.CreateElement("Attachment");
                    xmlEl.SetAttribute("FileID", lngNextFileID.ToString());
                    xmlEl.SetAttribute("FileName", strFName);
                    xmlEl.SetAttribute("SufName", GetSufName(GetFileName(arr[0])));
                    xmlEl.SetAttribute("Status", ((int)lngFileStatus).ToString());
                    xmlEl.SetAttribute("upTime", System.DateTime.Now.ToString());
                    xmlEl.SetAttribute("upUserID", lngUserID.ToString());
                    xmlEl.SetAttribute("upUserName", strPersonName);
                    xmlEl.SetAttribute("replace", "");
                    xmlEl.SetAttribute("OriginID", NodeModelID.ToString());
                    xmlDoc.DocumentElement.AppendChild(xmlEl);
                }


                //����XML������ˢ��GRID

                strAttachXml = xmlDoc.InnerXml;
                ViewState[this.ID + "pAttachment"] = strAttachXml;
                DoDataBind(strAttachXml);

                txtFile.Value = string.Empty;
                hidbackData.Value = string.Empty;

            }
            #endregion
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eAttachmentType"></param>
        /// <param name="FlowID"></param>
        protected void LoadTempFile(eOA_AttachmentType eAttachmentType, long FlowID)
        {
            string sAttXML = string.Empty;

            string strMonthPath = ""; //�����ļ�������ļ���

            if (eAttachmentType == eOA_AttachmentType.eNormal)
            {
                sAttXML = Message.GetAttachmentXml(FlowID, ref strMonthPath);
            }
            else if (eAttachmentType == eOA_AttachmentType.eNews)
            {
                sAttXML = NewsEntity.GetAttachmentXml(FlowID, ref strMonthPath);
            }
            else if (eAttachmentType == eOA_AttachmentType.eZC)
            {
                //�ʲ�
                sAttXML = Equ_DeskDP.GetAttachmentXml(FlowID, ref strMonthPath);
            }
            else if (eAttachmentType == eOA_AttachmentType.eKB)   //֪ʶ��
            {
                sAttXML = Inf_InformationDP.GetAttachmentXml(FlowID, ref strMonthPath);
            }
            else
            {
                sAttXML = Inf_InformationDP.GetAttachmentXml(FlowID, ref strMonthPath);
            }

            long lngNextFileID = 0;

            string strFileName = "";
            string strFName = "";   //����·�����ļ���
            string strAttachXml = "";
            long lngUserID = (long)Session["UserID"];
            string strPersonName = Session["PersonName"].ToString();

            string strTmpSubPath = "";    //������ʱ��·��,��ֹ���û���ͻ
            string strTmpPath = "";

            if (sAttXML.Length > 0)
            {
                try
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    XmlElement xmlElTmp;
                    xmlElTmp = xmlDoc.CreateElement("Attachments");

                    //������ʱ·��
                    if (strTmpSubPath == "")
                    {
                        Random rnd = new Random();
                        strTmpSubPath = rnd.Next(100000000).ToString();
                    }

                    if (strTmpCatalog.EndsWith(@"\") == false)
                    {
                        strTmpPath = strTmpCatalog + @"\" + strTmpSubPath;
                    }
                    else
                    {
                        strTmpPath = strTmpCatalog + strTmpSubPath;
                    }
                    MyFiles.AutoCreateDirectory(strTmpPath);

                    string pFileName = string.Empty;
                    string pSufName = string.Empty;
                    string pFileID = string.Empty;
                    XmlDocument tempxmldoc = new XmlDocument();
                    tempxmldoc.LoadXml(sAttXML);
                    foreach (XmlNode node in tempxmldoc.ChildNodes[0].ChildNodes)
                    {
                        pFileName = ((XmlElement)node).GetAttribute("FileName").ToString();
                        pSufName = ((XmlElement)node).GetAttribute("SufName").ToString();
                        pFileID = ((XmlElement)node).GetAttribute("FileID").ToString();
                        lngNextFileID = EPGlobal.GetNextID("FILE_ID");
                        XmlElement xmlEl = xmlDoc.CreateElement("Attachment");
                        xmlEl.SetAttribute("FileID", lngNextFileID.ToString());
                        xmlEl.SetAttribute("FileName", pFileName);
                        xmlEl.SetAttribute("SufName", pSufName);
                        xmlEl.SetAttribute("Status", ((int)e_FileStatus.efsNew).ToString());
                        xmlEl.SetAttribute("upTime", System.DateTime.Now.ToString());
                        xmlEl.SetAttribute("upUserID", lngUserID.ToString());
                        xmlEl.SetAttribute("upUserName", strPersonName);
                        xmlEl.SetAttribute("replace", "");
                        xmlEl.SetAttribute("OriginID", NodeModelID.ToString());
                        xmlElTmp.AppendChild(xmlEl);


                        string strUrl = "";
                        if (strFileCatalog.EndsWith(@"\") == false)
                        {
                            if (strMonthPath != string.Empty)
                                strUrl = strFileCatalog + @"\" + strMonthPath + @"\" + pFileID.ToString();
                            else
                                strUrl = strFileCatalog + @"\" + pFileID.ToString();
                        }
                        else
                        {
                            if (strMonthPath != string.Empty)
                                strUrl = strFileCatalog + strMonthPath + @"\" + pFileID.ToString();
                            else
                                strUrl = strFileCatalog + pFileID.ToString();
                        }

                        //��ȡ�ļ�
                        FileInfo pFileInfo = new FileInfo(strUrl);
                        //���Ϊ
                        pFileInfo.CopyTo(strTmpPath + @"\" + lngNextFileID.ToString());

                    }
                    xmlDoc.AppendChild(xmlElTmp);
                    xmlDoc.DocumentElement.SetAttribute("TempSubPath", strTmpSubPath);

                    //����XML������ˢ��GRID
                    strAttachXml = xmlDoc.InnerXml;
                    ViewState[this.ID + "pAttachment"] = xmlDoc.InnerXml;
                    DoDataBind(strAttachXml);
                    txtFile.Value = string.Empty;
                }
                catch
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strPath"></param>
        /// <returns></returns>
        private string GetFileName(string strPath)
        {
            return strPath.Substring(strPath.LastIndexOf(@"\") + 1);
        }
        private string GetSufName(string strFileName)
        {
            return strFileName.Substring(strFileName.LastIndexOf(".") + 1);
        }

        private void DoDataBind(string strAttachXml)
        {
            //			DataSet ds = new DataSet();
            DataTable dt = new DataTable("Attachments");

            dt.Columns.Add("FileID");
            dt.Columns.Add("FileName");
            dt.Columns.Add("SufName");
            dt.Columns.Add("Status");
            dt.Columns.Add("upTime");
            dt.Columns.Add("upUserID");
            dt.Columns.Add("upUserName");
            dt.Columns.Add("IDAndName");
            dt.Columns.Add("requestFileId");
            dt.Columns.Add("IsUpdate");
            dt.Columns.Add("OriginID");

            string strDelete = string.Empty;

            #region �ж�ɾ���� ��ɾ���� ���뵽�̶�λ��
            XmlTextReader trUpdate = new XmlTextReader(new StringReader(strAttachXml));
            while (trUpdate.Read())
            {
                if (trUpdate.Name == "Attachment" && trUpdate.NodeType == XmlNodeType.Element)
                {
                    if ((e_FileStatus)(int.Parse(trUpdate.GetAttribute("Status"))) == e_FileStatus.efsDeleted)
                    {
                        if (!string.IsNullOrEmpty(trUpdate.GetAttribute("replace")) && trUpdate.GetAttribute("replace").Trim() != "")
                        {
                            strDelete += trUpdate.GetAttribute("FileID").Trim() + "|" + trUpdate.GetAttribute("replace").Trim() + ",";
                        }
                    }
                }
            }
            #endregion


            XmlTextReader tr = new XmlTextReader(new StringReader(strAttachXml));
            object[] values = new object[11];
            while (tr.Read())
            {
                if (tr.Name == "Attachment" && tr.NodeType == XmlNodeType.Element)
                {
                    string isUpdate = "0";
                    values[0] = (object)tr.GetAttribute("FileID");
                    values[1] = (object)tr.GetAttribute("FileName");
                    values[2] = (object)tr.GetAttribute("SufName");
                    values[3] = (object)tr.GetAttribute("Status");
                    values[4] = (object)tr.GetAttribute("upTime");
                    values[5] = (object)tr.GetAttribute("upUserID");
                    values[6] = (object)tr.GetAttribute("upUserName");
                    values[7] = (object)(values[0].ToString() + "                                 " + values[1].ToString());
                    values[8] = (object)tr.GetAttribute("replace");
                    values[9] = (object)isUpdate;
                    values[10] = (object)tr.GetAttribute("OriginID");

                    if ((e_FileStatus)(int.Parse(values[3].ToString())) != e_FileStatus.efsDeleted)
                    {
                        dt.Rows.Add(values);
                        if (tryinchang.Visible == true)
                        {
                            #region ����δ����Ĵ������
                            string[] str = strDelete.Split(',');
                            bool isbool = false;
                            foreach (string strValue in str)
                            {
                                if (strValue.Trim() != "")
                                {
                                    if (strValue.Split('|')[1].Trim() == values[0].ToString().Trim())
                                    {
                                        getUpdate(long.Parse(strValue.Split('|')[0].Trim()), ref dt, true);
                                        isbool = true;
                                    }
                                }
                            }
                            #endregion
                            if (isbool == false)
                            {
                                //����������ɾ�������
                                getUpdate(long.Parse(values[0].ToString()), ref dt, false);
                            }
                        }
                    }

                }

            }
            tr.Close();
            dgAttachment.DataSource = dt.DefaultView;
            dgAttachment.DataBind();


        }

        #region  getUpdate
        /// <summary>
        /// getUpdate
        /// </summary>
        /// <param name="Filed"></param>
        /// <param name="dt"></param>
        /// <param name="IsDelete"></param>
        public void getUpdate(long Filed, ref DataTable dt, bool IsDelete)
        {
            DataTable upDT = new DataTable();
            if (this.AttachmentType == eOA_AttachmentType.eNormal)
            {
                upDT = FlowDP.getUpdateAttchmentTBL(FlowID.ToString(), Filed, IsDelete);
            }
            else if (this.AttachmentType == eOA_AttachmentType.eNews)
            {
                upDT = NewsEntity.getUpdateAttchmentTBL(FlowID.ToString(), Filed, IsDelete);
            }
            else if (this.AttachmentType == eOA_AttachmentType.eZC)
            {
                upDT = Equ_DeskDP.getUpdateAttchmentTBL(FlowID.ToString(), Filed, IsDelete);
            }            
            else if (this.AttachmentType == eOA_AttachmentType.eKB)   //֪ʶ��
            {
                upDT = Inf_InformationDP.getUpdateAttchmentTBL(FlowID.ToString(), Filed, IsDelete);
            }            
            else
            {
                upDT = FlowDP.getUpdateAttchmentTBL(FlowID.ToString(), Filed, IsDelete);
            }

            if (upDT.Rows.Count > 0)
            {
                string isUpdate = "1";
                object[] values = new object[10];
                values[0] = (object)upDT.Rows[0]["FileID"];
                values[1] = (object)upDT.Rows[0]["FileName"];
                values[2] = (object)upDT.Rows[0]["SufName"];
                values[3] = (object)upDT.Rows[0]["Status"];
                values[4] = (object)upDT.Rows[0]["upTime"];
                values[5] = (object)upDT.Rows[0]["upUserID"];
                values[6] = (object)upDT.Rows[0]["upUserName"];
                values[7] = (object)(values[0].ToString() + "                                 " + values[1].ToString());
                values[8] = (object)upDT.Rows[0]["requstFileId"];
                values[9] = (object)isUpdate;
                dt.Rows.Add(values);
                getUpdate(long.Parse(values[0].ToString()), ref dt, false);
            }
        }
        #endregion

        #region dgAttachment_ItemDataBound
        /// <summary>
        /// dgAttachment_ItemDataBound
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dgAttachment_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                switch ((e_FileStatus)(int.Parse(e.Item.Cells[4].Text.Trim())))
                {
                    case e_FileStatus.efsNew:
                        for (int i = 0; i < e.Item.Cells.Count; i++)
                        {
                            e.Item.Cells[i].ForeColor = Color.Blue;
                        }
                        break;
                    case e_FileStatus.efsUpdate:
                        for (int i = 0; i < e.Item.Cells.Count; i++)
                        {
                            e.Item.Cells[i].ForeColor = Color.Red;
                        }
                        break;
                    default:
                        for (int i = 0; i < e.Item.Cells.Count; i++)
                        {
                            e.Item.Cells[i].ForeColor = Color.Black;
                        }
                        break;
                }

                //����ɾ���͸��°�ť�Ƿ�ɼ�
                Button cmdDel = (Button)e.Item.FindControl("cmdDel");
                Button cmdEdit = (Button)e.Item.FindControl("cmdEdit");
                cmdDel.Visible = false;
                cmdEdit.Visible = false;
                if (CheckRight(Constant.AttachmentManager))  //����Ȩ��
                {
                    cmdDel.Visible = true;
                    cmdEdit.Visible = true;
                }
                else if (e.Item.Cells[8].Text.Trim().Equals(Session["UserID"].ToString()))    //���˵ģ����и��£�ɾ��Ȩ��
                {
                    if (GetAttRight(e_AttachmentRight.eEdit) == e_TrueOrFalse.eTrue)
                    {
                        cmdEdit.Visible = true;
                    }
                    if (GetAttRight(e_AttachmentRight.eDelete) == e_TrueOrFalse.eTrue)
                    {
                        cmdDel.Visible = true;
                    }
                }
                else
                {
                    cmdDel.Visible = false;
                    cmdEdit.Visible = false;
                }

                if (e.Item.Cells[9].Text.ToString() == "1")
                {
                    //�ѱ����µ� ��ť����ʾ   
                    cmdDel.Visible = false;
                    cmdEdit.Visible = false;
                    ((Label)e.Item.FindControl("lblFileName")).Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + ((Label)e.Item.FindControl("lblFileName")).Text;
                }

                e.Item.Cells[0].Attributes.Add("align", "Left");

                #region �жϰ�ť�Ƿ� ��ԭ
                long lngFileID = long.Parse(e.Item.Cells[5].Text);  //FileID

                string strAttachXml = ViewState[this.ID + "pAttachment"].ToString();
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(strAttachXml);
                XmlNodeList nl = xmlDoc.DocumentElement.SelectNodes("Attachment[@FileID=" + lngFileID.ToString() + "]");

                if (nl.Count > 0)
                {
                    //����״̬
                    foreach (XmlNode node in nl)
                    {

                        if (((XmlElement)node).GetAttribute("Status").ToString() == ((int)e_FileStatus.efsNew).ToString())
                        {
                            XmlNodeList nl2 = xmlDoc.DocumentElement.SelectNodes("Attachment[@replace=" + lngFileID.ToString() + "]");
                            if (nl2.Count > 0)
                            {
                                ((Button)e.Item.FindControl("cmdDel")).Text = "ȡ��";
                            }
                        }

                    }

                }
                #endregion



            }
        }
        #endregion

        #region dgAttachment_ItemCommand
        /// <summary>
        /// dgAttachment_ItemCommand
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void dgAttachment_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "delete")
            {
                #region ���Ǳ�����û��Ȩ��ʱ������ɾ������
                if (e.Item.Cells[8].Text.Trim().Equals(Session["UserID"].ToString()) || CheckRight(Constant.AttachmentManager))  //���Ǳ��˲���û��Ȩ��
                {
                }
                else
                {
                    Response.Write("<script language='javascript'>alert('����ɾ�������ϴ��ĸ���');</script>");
                    return;
                }
                #endregion

                #region ɾ��
                long lngFileID = 0;

                string strAttachXml = "";
                e_FileStatus lngFileStatus = 0;

                lngFileID = long.Parse(e.Item.Cells[5].Text);  //FileID

                try
                {
                    strAttachXml = ViewState[this.ID + "pAttachment"].ToString();
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(strAttachXml);

                    XmlNodeList nl = xmlDoc.DocumentElement.SelectNodes("Attachment[@FileID=" + lngFileID.ToString() + "]");

                    if (nl.Count > 0)
                    {

                        //����״̬
                        foreach (XmlNode node in nl)
                        {
                            //ֻ������һ��
                            lngFileStatus = e_FileStatus.efsDeleted;

                            ((XmlElement)node).SetAttribute("Status", ((int)lngFileStatus).ToString());
                        }

                    }

                    #region �������º�δ����ֱ��ɾ���Ĵ����߼�
                    XmlNodeList nl2 = xmlDoc.DocumentElement.SelectNodes("Attachment[@replace=" + lngFileID.ToString() + "]");
                    if (nl2.Count > 0)
                    {
                        //����״̬
                        foreach (XmlNode node in nl2)
                        {
                            e_FileStatus FileStatus = isTrueDeleteFile(long.Parse(((XmlElement)node).GetAttribute("FileID").ToString()));
                            if (FileStatus != e_FileStatus.efsDeleted)
                            {
                                ((XmlElement)node).SetAttribute("Status", ((int)FileStatus).ToString());
                                ((XmlElement)node).SetAttribute("replace", "");
                            }
                        }
                    }
                    #endregion

                    //����XML������ˢ��GRID

                    strAttachXml = xmlDoc.InnerXml;
                    ViewState[this.ID + "pAttachment"] = strAttachXml;
                    DoDataBind(strAttachXml);
                    GridBind(this.FlowID.ToString());

                    updateFileDiv.Visible = false;
                }
                catch
                {
                    throw;
                }
                #endregion
            }
            else if (e.CommandName == "Edit")
            {
                if (e.Item.Cells[8].Text.Trim().Equals(Session["UserID"].ToString()) || CheckRight(Constant.AttachmentManager))  //���Ǳ��˲���û��Ȩ��
                {
                }
                else
                {
                    Response.Write("<script language='javascript'>alert('���ܸ��������ϴ��ĸ���');</script>");
                    return;
                }
                deleteFileId.Value = e.Item.Cells[5].Text;  //FileID
                updateFileDiv.Visible = true;
            }
            else if (e.CommandName == "Preview")//guoch 2014-02-24��������Ԥ������
            {
                //long FileID = Convert .ToInt64(e.CommandArgument);
                string strIDAndName = e.CommandArgument.ToString();
                long lngFileID = long.Parse(strIDAndName.Substring(0, strIDAndName.IndexOf(" ")));


                string strFile = Epower.ITSM.SqlDAL.Inf_InformationDP.GetSWFFile(lngFileID.ToString());

                if (strFile != "")
                {
                    string url = "../Forms/flow_attachment_preview.aspx?Flash=" + lngFileID ;
                    Response.Write("<script>window.open('" + url + "')</script>");
                    //OnlinePreview preview = new OnlinePreview(Request.PhysicalApplicationPath +  OnlinePreview.FlashFile, filepath);
                    //preview.GetSWF(strFileName);
                }
            }
        }
        #endregion

        #region �ж��Ƿ��Ѿ�ɾ�����ļ�
        /// <summary>
        /// �жϸø��������ݿ��е�״̬
        /// </summary>
        /// <param name="FileId"></param>
        /// <returns></returns>
        public e_FileStatus isTrueDeleteFile(long FileId)
        {
            e_FileStatus lngFileStatus = 0;
            DataTable dt = new DataTable();
            if (this.AttachmentType == eOA_AttachmentType.eNormal)
            {
                dt = FlowDP.getFileIsTrue(FileId);
            }
            else if (this.AttachmentType == eOA_AttachmentType.eNews)
            {
                dt = NewsEntity.getFileIsTrue(FileId);
            }
            else if (this.AttachmentType == eOA_AttachmentType.eZC)
            {
                //�ʲ�
                dt = Equ_DeskDP.getFileIsTrue(FileId);
            }
            else if (this.AttachmentType == eOA_AttachmentType.eKB)   //֪ʶ��
            {
                dt = Inf_InformationDP.getFileIsTrue(FileId);
            }
            else
            {
                dt = FlowDP.getFileIsTrue(FileId);
            }

            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["deleted"].ToString() == "1")
                {
                    lngFileStatus = e_FileStatus.efsDeleted;
                }
                else
                {
                    lngFileStatus = e_FileStatus.efsNormal;
                }
            }
            else
            {
                lngFileStatus = e_FileStatus.efsNew;
            }

            return lngFileStatus;
        }
        #endregion

        #region �ж�Ȩ��
        /// <summary>
        /// �ж�Ȩ��
        /// </summary>
        /// <param name="ResourceKey"></param>
        /// <returns></returns>
        public bool CheckRight(long ResourceKey)
        {
            bool breturn = false;
            Hashtable htAllRights = (Hashtable)System.Web.HttpContext.Current.Session["UserAllRights"];
            Epower.DevBase.Organization.SqlDAL.RightEntity re = (Epower.DevBase.Organization.SqlDAL.RightEntity)htAllRights[ResourceKey];
            if (re == null)
            {
                breturn = false;
            }
            else
            {
                if (re.CanRead == false)
                {
                    breturn = false;
                }
                else
                {
                    breturn = true;
                }
            }
            return breturn;
        }
        #endregion

        #region GetUrl��3��������
        /// <summary>
        /// GetUrl��3��������
        /// </summary>
        /// <param name="lngFlowID"></param>
        /// <returns></returns>
        protected string GetUrl(string FileName, string Status, string IDAndName)
        {
            string sUrl = "";
            e_FileStatus fs = (e_FileStatus)(int.Parse(Status));
            if (fs == e_FileStatus.efsNew || fs == e_FileStatus.efsUpdate)  //����
            {
                sUrl = FileName.Trim();
            }
            else
            {
                sUrl = "<A href='../Forms/flow_Attachment_download.aspx?FileID=" + IDAndName.Trim() + "&Type=" + AttachmentType.ToString() + "' target='_blank'" + ">" + FileName.Trim() + "</A>";
            }
            return sUrl;
        }
        #endregion

        #region GetUrl��2��������
        /// <summary>
        /// GetUrl��2��������
        /// </summary>
        /// <param name="lngFlowID"></param>
        /// <returns></returns>
        protected string GetUrl(string FileName, string IDAndName)
        {
            string sUrl = "";
            sUrl = "<A href='../Forms/flow_Attachment_download.aspx?FileID=" + IDAndName.Trim() + "&Type=" + AttachmentType.ToString() + "' target='_blank'" + ">" + FileName.Trim() + "</A>";
            return sUrl;
        }
        #endregion

        #region �����ݱ�
        /// <summary>
        /// �����ݱ�
        /// </summary>
        /// <param name="flowId"></param>
        public void GridBind(string flowId)
        {
            DataTable dt = null;
            if (this.AttachmentType == eOA_AttachmentType.eNormal)
            {
                dt = FlowDP.getDeleteAttchmentTBL(flowId);
            }
            else if (this.AttachmentType == eOA_AttachmentType.eNews)
            {
                dt = NewsEntity.getDeleteAttchmentTBL(flowId);
            }
            else if (this.AttachmentType == eOA_AttachmentType.eZC)
            {
                dt = Equ_DeskDP.getDeleteAttchmentTBL(flowId);
            }            
            else if (this.AttachmentType == eOA_AttachmentType.eKB)   //֪ʶ��
            {
                dt = Inf_InformationDP.getDeleteAttchmentTBL(flowId);
            }            
            else
            {
                dt = FlowDP.getDeleteAttchmentTBL(flowId);
            }

            DataTable deleteDt = new DataTable();
            deleteDt.Columns.Add("FileName");
            deleteDt.Columns.Add("IDAndName");
            deleteDt.Columns.Add("upTime");
            deleteDt.Columns.Add("upUserName");
            deleteDt.Columns.Add("deleteTime");
            deleteDt.Columns.Add("IsUpdate");

            foreach (DataRow dr in dt.Rows)
            {
                string isUpdate = "0";
                object[] values = new object[6];
                values[0] = (object)dr["FileName"];
                values[1] = (object)(dr["FileID"].ToString() + "                                 " + dr["FileName"].ToString());
                values[2] = (object)dr["upTime"];
                values[3] = (object)dr["upUserName"];
                values[4] = (object)dr["deleteTime"];
                values[5] = (object)isUpdate;
                deleteDt.Rows.Add(values);
                getDeleteUpdate(long.Parse(dr["FileID"].ToString()), ref deleteDt);
            }

            AttachmentDelete.DataSource = deleteDt;
            AttachmentDelete.DataBind();
        }
        #endregion

        #region getDeleteUpdate
        /// <summary>
        /// getDeleteUpdate
        /// </summary>
        /// <param name="Filed"></param>
        /// <param name="dt"></param>
        public void getDeleteUpdate(long Filed, ref DataTable dt)
        {
            DataTable upDT = new DataTable();
            if (this.AttachmentType == eOA_AttachmentType.eNormal)
            {
                upDT = FlowDP.getUpdateAttchmentTBL(FlowID.ToString(), Filed, false);
            }
            else if (this.AttachmentType == eOA_AttachmentType.eNews)
            {
                upDT = NewsEntity.getUpdateAttchmentTBL(FlowID.ToString(), Filed, false);
            }
            else if (this.AttachmentType == eOA_AttachmentType.eZC)
            {
                //�ʲ�
                upDT = Equ_DeskDP.getUpdateAttchmentTBL(FlowID.ToString(), Filed, false);
            }
            else if (this.AttachmentType == eOA_AttachmentType.eKB)   //֪ʶ��
            {
                upDT = Inf_InformationDP.getUpdateAttchmentTBL(FlowID.ToString(), Filed, false);
            }
            else
            {
                upDT = FlowDP.getUpdateAttchmentTBL(FlowID.ToString(), Filed, false);
            }

            if (upDT.Rows.Count > 0)
            {
                string isUpdate = "1";
                object[] values = new object[6];
                values[0] = (object)upDT.Rows[0]["FileName"];
                values[1] = (object)(upDT.Rows[0]["FileID"].ToString() + "                                 " + upDT.Rows[0]["FileName"].ToString());
                values[2] = (object)upDT.Rows[0]["upTime"].ToString();
                values[3] = (object)upDT.Rows[0]["upUserName"];
                values[4] = (object)upDT.Rows[0]["deleteTime"];
                values[5] = (object)isUpdate;
                dt.Rows.Add(values);
                getDeleteUpdate(long.Parse(upDT.Rows[0]["FileID"].ToString()), ref dt);
            }
        }
        #endregion

        #region AttachmentDelete_ItemDataBound
        /// <summary>
        /// AttachmentDelete_ItemDataBound
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void AttachmentDelete_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                if (e.Item.Cells[4].Text.ToString() == "1")
                {
                    //�ѱ����µ� ��ť����ʾ                     
                    ((Label)e.Item.FindControl("lblFileName")).Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + ((Label)e.Item.FindControl("lblFileName")).Text;
                }
            }
        }
        #endregion

        #region �������¡�ȷ����
        /// <summary>
        /// �������¡�ȷ����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btncmdAttach2_Click(object sender, EventArgs e)
        {


            long lngNextFileID = 0;

            string strFileName = "";
            string strFName = "";   //����·�����ļ���
            string strAttachXml = "";
            long lngUserID = (long)Session["UserID"];
            string strPersonName = Session["PersonName"].ToString();

            e_FileStatus lngFileStatus = 0;
            e_FileStatus lngOriStatus = 0;

            string strTmpSubPath = "";    //������ʱ��·��,��ֹ���û���ͻ
            string strTmpPath = "";

            if (File2.PostedFile != null)
            {
                if (File2.PostedFile.FileName.Trim() == "")
                {

                    strAttachXml = ViewState[this.ID + "pAttachment"].ToString();
                    return;
                }

                int MaxFileSize = 4;
                if (CommonDP.GetConfigValue("TempCataLog", "MaxFileSize") != null)
                {
                    MaxFileSize = int.Parse(CommonDP.GetConfigValue("TempCataLog", "MaxFileSize"));
                }
                if (File2.PostedFile.ContentLength > MaxFileSize * 1024 * 1024)  //�ж��ϴ��ļ���С
                {

                    PageTool.MsgBox(this.Page, "�ϴ��ļ�����" + MaxFileSize.ToString() + "M�������ϴ��ļ���");
                    return;
                }

                try
                {
                    strAttachXml = ViewState[this.ID + "pAttachment"].ToString();
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(strAttachXml);

                    //������ʱ·��
                    strTmpSubPath = xmlDoc.DocumentElement.GetAttribute("TempSubPath");
                    if (strTmpSubPath == "")
                    {
                        Random rnd = new Random();
                        strTmpSubPath = DateTime.Now.ToString("yyyyMMdd") + rnd.Next(100000000).ToString();
                        xmlDoc.DocumentElement.SetAttribute("TempSubPath", strTmpSubPath);
                    }

                    if (strTmpCatalog.EndsWith(@"\") == false)
                    {
                        strTmpPath = strTmpCatalog + @"\" + strTmpSubPath;
                    }
                    else
                    {
                        strTmpPath = strTmpCatalog + strTmpSubPath;
                    }
                    MyFiles.AutoCreateDirectory(strTmpPath);


                    //�ж��Ƿ��ظ�
                    strFName = StringTool.sfReplaceFileNameSymol(GetFileName(File2.PostedFile.FileName));

                    lngNextFileID = EPGlobal.GetNextID("FILE_ID");
                    lngFileStatus = e_FileStatus.efsNew;
                    XmlElement xmlEl = xmlDoc.CreateElement("Attachment");
                    xmlEl.SetAttribute("FileID", lngNextFileID.ToString());
                    xmlEl.SetAttribute("FileName", strFName);
                    xmlEl.SetAttribute("SufName", GetSufName(GetFileName(File2.PostedFile.FileName)));
                    xmlEl.SetAttribute("Status", ((int)lngFileStatus).ToString());
                    xmlEl.SetAttribute("upTime", System.DateTime.Now.ToString());
                    xmlEl.SetAttribute("upUserID", lngUserID.ToString());
                    xmlEl.SetAttribute("upUserName", strPersonName);
                    xmlEl.SetAttribute("replace", "");
                    xmlEl.SetAttribute("OriginID", NodeModelID.ToString());
                    xmlDoc.DocumentElement.AppendChild(xmlEl);

                    ////����ļ�
                    strFileName = strTmpPath + @"\" + lngNextFileID.ToString();
                    ////��������ʱ·��
                    File2.PostedFile.SaveAs(strFileName);

                    strAttachXml = xmlDoc.InnerXml;
                    ViewState[this.ID + "pAttachment"] = strAttachXml;
                    DoDataBind(strAttachXml);
                    txtFile.Value = string.Empty;


                    #region ��ɾ��
                    long lngFileID = 0;

                    strAttachXml = "";
                    lngFileStatus = 0;
                    lngFileID = long.Parse(deleteFileId.Value);  //FileID
                    try
                    {
                        strAttachXml = ViewState[this.ID + "pAttachment"].ToString();
                        xmlDoc = new XmlDocument();
                        xmlDoc.LoadXml(strAttachXml);


                        if (isTrueFile(lngFileID))
                        {
                            XmlNodeList nl = xmlDoc.DocumentElement.SelectNodes("Attachment[@FileID=" + lngFileID.ToString() + "]");
                            if (nl.Count > 0)
                            {
                                //����״̬
                                foreach (XmlNode node in nl)
                                {
                                    //ֻ������һ��
                                    lngFileStatus = e_FileStatus.efsDeleted;
                                    ((XmlElement)node).SetAttribute("Status", ((int)lngFileStatus).ToString());
                                    ((XmlElement)node).SetAttribute("replace", ((int)lngNextFileID).ToString());
                                }
                            }
                        }
                        else
                        {
                            XmlNodeList nl = xmlDoc.DocumentElement.SelectNodes("Attachment[@FileID=" + lngFileID.ToString() + "]");
                            string replasec = "";
                            if (nl.Count > 0)
                            {
                                //ɾ�������µ�
                                //����״̬
                                foreach (XmlNode node in nl)
                                {
                                    //ֻ������һ��
                                    lngFileStatus = e_FileStatus.efsDeleted;
                                    ((XmlElement)node).SetAttribute("Status", ((int)lngFileStatus).ToString());
                                    replasec = ((XmlElement)node).GetAttribute("FileID").ToString();
                                }
                            }
                            //���¸��������ı�ɾ���Ĺ�ϵ
                            //�жϲ�Ϊ�յ����
                            if (replasec != "")
                            {
                                XmlNodeList nl2 = xmlDoc.DocumentElement.SelectNodes("Attachment[@replace=" + replasec.ToString() + "]");
                                if (nl2.Count > 0)
                                {
                                    //����״̬
                                    foreach (XmlNode node in nl2)
                                    {
                                        //ֻ������һ��
                                        lngFileStatus = e_FileStatus.efsDeleted;
                                        ((XmlElement)node).SetAttribute("Status", ((int)lngFileStatus).ToString());
                                        ((XmlElement)node).SetAttribute("replace", ((int)lngNextFileID).ToString());
                                    }
                                }
                            }

                        }


                        //����XML������ˢ��GRID
                        strAttachXml = xmlDoc.InnerXml;
                        ViewState[this.ID + "pAttachment"] = strAttachXml;
                        DoDataBind(strAttachXml);

                        GridBind(this.FlowID.ToString());
                        // Response.Write("<script>window.parent.header.flowInfo.Attachment.value='" + strAttachXml + "'</script>");

                    }
                    catch
                    {
                        throw;
                    }
                    #endregion

                    deleteFileId.Value = "";
                    updateFileDiv.Visible = false;
                    //Response.Write("<script>window.parent.header.flowInfo.Attachment.value=" + StringTool.JavaScriptQ(HttpUtility.UrlEncode(strAttachXml)) +"</script>");

                }
                catch
                {
                    throw;
                }
            }

        }
        #endregion

        #region �������¡�ȡ����
        /// <summary>
        /// �������¡�ȡ����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button1_Click(object sender, EventArgs e)
        {
            deleteFileId.Value = "";
            updateFileDiv.Visible = false;
        }
        #endregion

        #region �ж��ļ������ݿ����Ƿ����
        /// <summary>
        /// �ж��ļ������ݿ����Ƿ����
        /// </summary>
        /// <param name="FileId"></param>
        /// <returns></returns>
        public bool isTrueFile(long FileId)
        {
            DataTable dt = new DataTable();
            if (this.AttachmentType == eOA_AttachmentType.eNormal)
            {
                dt = FlowDP.getFileIsTrue(FileId);
            }
            else if (this.AttachmentType == eOA_AttachmentType.eNews)
            {
                dt = NewsEntity.getFileIsTrue(FileId);
            }
            else
            {
                dt = FlowDP.getFileIsTrue(FileId);
            }

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

        #region ��ʾ��ʷ����
        /// <summary>
        /// ��ʾ��ʷ����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ButXS_Click(object sender, EventArgs e)
        {
            tryinchang.Visible = true;
            ButYC.Visible = true;
            ButXS.Visible = false;
            string Attachment = ViewState[this.ID + "pAttachment"].ToString();
            DoDataBind(Attachment);
        }
        #endregion

        #region ������ʷ����
        /// <summary>
        /// ������ʷ����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ButYC_Click(object sender, EventArgs e)
        {
            tryinchang.Visible = false;
            ButYC.Visible = false;
            ButXS.Visible = true;
            string Attachment = ViewState[this.ID + "pAttachment"].ToString();
            DoDataBind(Attachment);
        }
        #endregion
    }
}
