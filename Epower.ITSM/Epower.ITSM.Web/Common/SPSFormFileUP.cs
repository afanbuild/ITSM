/****************************************************************************
 * 
 * description:SharePoint �����࣬�����ݹ鵵���ĵ��鵵
 * 
 * 
 * 
 * Create by:
 * Create Date:2007-09-07
 * *************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.SharePoint;
using System.IO;
using System.Xml;
using System.Data;

namespace Epower.ITSM.Web.Common
{
    /// <summary>
    /// SPSFormFileUP
    /// </summary>
    public class SPSFormFileUP
    {
        /// <summary>
        /// �ϴ��ļ�
        /// </summary>
        /// <param name="pNodeID">�ڵ�ID</param>
        /// <param name="pXmlContent">������</param>
        /// <param name="pAttach">������byte[]������</param>
        /// <returns></returns>
        public static bool UpFile(System.Web.UI.Page pPage,string pNodeID, string pXmlContent, string pXmlAttach)
        {
            bool breturn = true;

            XmlNode pXmlNode = GetXmlNode(pPage,pNodeID);
            int pType = int.Parse(pXmlNode.Attributes["Type"].Value.ToString());
            //�ϴ�������
            if (pType == 0)
            {
                breturn = UpFormFile(pXmlContent, pXmlAttach, pXmlNode);
            }
            //�ϴ��ĵ�
            else   
            {
                breturn = UpDocumentFile(pXmlContent, pXmlAttach, pXmlNode);
            }
            return breturn;
        }

        /// <summary>
        /// �ϴ������ݣ������ݿ����и���
        /// </summary>
        /// <param name="pXmlContent"></param>
        /// <param name="pAttach"></param>
        /// <returns></returns>
        private static bool UpFormFile(string pXmlContent, string pXmlAttach, XmlNode pXmlNode)
        {
            bool breturn = true;
            SPWeb pSPWeb = GetSpWeb(pXmlNode);
            SPListItemCollection pitem = pSPWeb.Lists[pXmlNode.Attributes["SpsForm"].Value.ToString()].Items;
            SPListItem item = pitem.Add();
            SPListItem list = item.ListItems.Add();
            try
            {
                XmlTextReader tr = new XmlTextReader(new StringReader(pXmlContent));
                while (tr.Read())
                {
                    if (tr.Name == "Field" && tr.NodeType == XmlNodeType.Element)
                    {
                        try
                        {
                            list[tr.GetAttribute("FieldName").ToString()] = tr.GetAttribute("Value").Trim();
                        }
                        catch
                        {
                           
                        }
                    }
                }
                //�ϴ�����
                if (!string.IsNullOrEmpty(pXmlAttach))
                {
                    XmlTextReader trattach = new XmlTextReader(new StringReader(pXmlAttach));
                    while (trattach.Read())
                    {
                        if (trattach.Name == "Field" && trattach.NodeType == XmlNodeType.Element)
                        {
                            byte[] pbyte = GetFileByte(trattach.GetAttribute("Value").Trim());
                            list.Attachments.Add(trattach.GetAttribute("FieldName").ToString(), pbyte);
                        }
                    }
                }
            }
            catch { breturn = false; }
            list.Update();
            return breturn;
        }

        /// <summary>
        /// �ϴ��ĵ�
        /// </summary>
        /// <param name="pXmlContent"></param>
        /// <param name="pAttach"></param>
        /// <returns></returns>
        private static bool UpDocumentFile(string pXmlContent, string pXmlAttach, XmlNode pXmlNode)
        {
            bool breturn = true;
            SPWeb pSPWeb = GetSpWeb(pXmlNode);
            SPFolder folder = pSPWeb.GetFolder(pXmlNode.Attributes["SpsForm"].Value);

            //�ϴ�����
            if (!string.IsNullOrEmpty(pXmlAttach))
            {
                XmlTextReader trattach = new XmlTextReader(new StringReader(pXmlAttach));
                while (trattach.Read())
                {
                    if (trattach.Name == "Field" && trattach.NodeType == XmlNodeType.Element)
                    {
                        byte[] pbyte = GetFileByte(trattach.GetAttribute("Value").Trim());
                        //�ϴ��ļ�
                        SPFile spfile = folder.Files.Add(trattach.GetAttribute("FieldName").ToString(), pbyte);

                        SPListItem item = spfile.Item;
                        SPList list = item.ParentList;
                        SPFieldCollection fields = list.Fields;

                        //TITLE    �ļ�����
                        string TITLE = "����";
                        try
                        {
                            // ��һЩ�Զ�������� 
                            spfile.Item[TITLE] = "test";
                        }
                        catch
                        {
                            //�������
                            fields.Add(TITLE, SPFieldType.Text, false);
                            SPField newField_TITLE = fields[TITLE];
                            SPView view = list.DefaultView;
                            SPViewFieldCollection viewFields = view.ViewFields;
                            viewFields.Add(newField_TITLE);
                            view.Update();

                            item[TITLE] = "testdf";
                        }

                        spfile.Item.Update();
                    }
                }
            }

            return breturn;
        }

        /// <summary>
        /// ȡ����Ӧ�Ľڵ�
        /// </summary>
        /// <param name="pID"></param>
        /// <returns></returns>
        private static XmlNode GetXmlNode(System.Web.UI.Page pPage, string pNodeID)
        {
            XmlDocument xmlDoc = new XmlDocument();
            string sPath = pPage.Server.MapPath("SPSUPSet.xml");
            if (!File.Exists(sPath))
                throw new Exception("δ�ҵ���Ӧ�����ļ���");
            xmlDoc.Load(sPath);
            XmlNode root = xmlDoc.DocumentElement.SelectSingleNode("Setting[@id='" + pNodeID + "' ]");
            return root;
        }

        /// <summary>
        /// ȡ����Ӧ����վ
        /// </summary>
        /// <returns></returns>
        private static SPWeb GetSpWeb(XmlNode pXmlNode)
        {
            string sSpsWebSite = pXmlNode.Attributes["SpsWebSite"].Value.ToString();
            string sSpsWeb = pXmlNode.Attributes["SpsWeb"].Value.ToString();

            //ȡ����վ
            SPSite st = new SPSite(sSpsWebSite);
            SPWeb site = st.OpenWeb(sSpsWeb);
            site.AllowUnsafeUpdates = true;
            return site;
        }

        /// <summary>
        /// �����ļ�·����ȡ���ļ�����
        /// </summary>
        /// <param name="fileSourcePath"></param>
        /// <returns></returns>
        private static byte[] GetFileByte(string fileSourcePath)
        {
            byte[] bute = null;
            FileStream fStream = null;
            //ȡ���ļ�����
            try
            {
                fStream = new FileStream(fileSourcePath, FileMode.Open, FileAccess.Read, FileShare.Read);

                bute = new byte[fStream.Length];
                fStream.Read(bute, 0, bute.Length);
            }
            finally
            {
                if (fStream != null)
                {
                    fStream.Close();
                }
            }
            return bute;
        }
    }
}
