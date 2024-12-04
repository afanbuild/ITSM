/*******************************************************************
 * ��Ȩ���У�
 * Description���ļ�������
 * 
 * 
 * Create By  ��
 * Create Date��2007-08-28
 * *****************************************************************/
using System;
using System.Web;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Web.UI.HtmlControls;

namespace Epower.DevBase.BaseTools
{
    /// <summary>
    /// 
    /// </summary>
    public class FileTools
    {
        #region ��ʱ�ļ����·��
        private static string _DirPath = "../TempDir";
        /// <summary>
        /// ȡ���ļ�·��
        /// </summary>
        public static String DirPath
        {
            get
            {
                System.IO.DirectoryInfo dirInfo = new DirectoryInfo(_DirPath);
                if (!dirInfo.Exists)
                {
                    dirInfo.Create();
                }
                return _DirPath;
            }
            set
            {
                _DirPath = value;
            }
        }
        #endregion

        #region �ļ�����
        /// <summary>
        /// �ļ�����
        /// </summary>
        /// <param name="Response"></param>
        /// <param name="fileName"></param>
        /// <param name="fullPath"></param>
        /// <returns></returns>
        public static bool DownFile(System.Web.HttpResponse Response, string fileName, string fullPath)
        {
            System.IO.FileStream fs = System.IO.File.OpenRead(fullPath);
            try
            {
                Response.ContentType = "application/octet-stream";

                Response.AppendHeader("Content-Disposition", "attachment;filename=" +
                    HttpUtility.UrlEncode(fileName, System.Text.Encoding.UTF8) + ";charset=GB2312");
                long fLen = fs.Length;
                int size = 102400;//ÿ100Kͬʱ�������� 
                byte[] readData = new byte[size];//ָ���������Ĵ�С 
                if (size > fLen) size = Convert.ToInt32(fLen);
                long fPos = 0;
                bool isEnd = false;
                while (!isEnd)
                {
                    if ((fPos + size) > fLen)
                    {
                        size = Convert.ToInt32(fLen - fPos);
                        readData = new byte[size];
                        isEnd = true;
                    }
                    fs.Read(readData, 0, size);//����һ��ѹ���� 
                    if (readData.Length > 0)
                        Response.BinaryWrite(readData);
                    fPos += size;
                }
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                fs.Close();
                System.IO.File.Delete(fullPath);
            }
        }

        /// <summary>
        /// �ļ�����
        /// </summary>
        /// <param name="Response"></param>
        /// <param name="fileName"></param>
        /// <param name="fullPath"></param>
        /// <returns></returns>
        public static void DownFileWrite(System.Web.HttpResponse Response, string fileName, string fullPath)
        {
            if (File.Exists(fullPath))
            {
                string filename = fileName;

                System.IO.FileInfo file = new FileInfo(fullPath);

                filename = HttpUtility.UrlEncode(fileName);

                Response.Clear();
                Response.ClearHeaders();

                Response.ContentType = "application/octet-stream";
                Response.AddHeader("Content-Disposition", "attachment; filename=\"" + fileName + "\"");

                Response.AddHeader("Content-Length", file.Length.ToString());

                Response.Flush();
                Response.WriteFile(file.FullName);
                Response.End();
            }
        }
        #endregion

        #region �ϴ��ļ�
        /// <summary>
        /// �ϴ��ļ�
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pHtmlInputFile"></param>
        /// <param name="pstrFullFileName"></param>
        public static void FileUp(System.Web.UI.Page page, HtmlInputFile pHtmlInputFile, ref string pstrFullFileName)
        {
            //��ȡ�ϴ��ļ��Ĵ�С
            int fFileSize = pHtmlInputFile.PostedFile.ContentLength;
            //���û��ѡ���ϴ����ļ�
            if (fFileSize == 0)
            {
                PageTool.MsgBox(page, "��ѡ��Ҫ�ϴ����ļ���");
                return;
            }

            //����ϴ��ĸ�������16��
            if (fFileSize > 16777216)
            {
                PageTool.MsgBox(page, "�ϴ��ĸ����ļ����ܴ���16��,��־�ѹ�����ϴ���");
                return;
            }

            //�ļ���
            String strFullFileName = pHtmlInputFile.PostedFile.FileName;
            FileInfo fi = new FileInfo(strFullFileName);
            //�ļ���׺��
            String strExt = fi.Extension;
            //ʵ���ļ���
            String strFileName = fi.Name;
            //���ļ���
            String strTmpFileName = GetFileName(DirPath + strFileName.Substring(0, strFileName.Length - strExt.Length) +
                DateTime.Now.ToString("yyyyMMdd") + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString()
                + DateTime.Now.Millisecond.ToString(), strExt);

            //�ϴ�
            pHtmlInputFile.PostedFile.SaveAs(DirPath + strTmpFileName);

            pstrFullFileName = DirPath + strTmpFileName;
        }
        #endregion

        #region �����ļ����ļ�������ֹ�����ظ����ļ�
        /// <summary>
        /// �����ļ����ļ�������ֹ�����ظ����ļ�
        /// </summary>
        /// <param name="strFileName">�ļ���</param>
        /// <param name="strExt">�ļ���׺</param>
        /// <returns></returns>
        public static string GetFileName(String strFileName, String strExt)
        {
            int intNo = 1;
            FileInfo fi = new FileInfo(strFileName + intNo.ToString() + strExt);

            //����ļ�����
            while (fi.Exists)
            {
                intNo += 1;
                fi = new FileInfo(strFileName + intNo.ToString() + strExt);
            }
            return fi.Name;
        }
        #endregion

        #region ɾ���ļ�
        /// <summary>
        /// ɾ���ļ�
        /// </summary>
        /// <param name="pstrFullFileName"></param>
        public static void FileDelete(string pstrFullFileName)
        {
            System.IO.File.Delete(pstrFullFileName);
        }
        #endregion

        #region ����ļ�
        /// <summary>
        /// ����ļ�
        /// </summary>
        /// <param name="pstrFullFileName"></param>
        /// <param name="pstrDestFullFileName"></param>
        public static void FileCopy(string pstrFullFileName, string pstrDestFullFileName)
        {
            //��ȡ�ļ�
            FileInfo pFileInfo = new FileInfo(pstrFullFileName);
            //���Ϊ
            pFileInfo.CopyTo(pstrDestFullFileName);
        }
        #endregion 
    }
}
