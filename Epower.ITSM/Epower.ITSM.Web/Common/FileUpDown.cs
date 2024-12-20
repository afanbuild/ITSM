/********************************************
 * 版权所有：深圳市非凡信息技术有限公司
 * 
 * 
 * 
 * 功能描述：文件上传下载操作
 * 作者：朱明春
 * 创建日期：2006-10-19
 * ******************************************/
using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Reflection;
using System.Data.OleDb;
using System.IO;
using System.Text;

using Epower.DevBase.BaseTools;

namespace Epower.ITSM.Web.Common
{
	/// <summary>
	/// FileUpDown 的摘要说明
	/// </summary>
	public class FileUpDown
	{
		#region 临时文件存放路径
		private static string _DirPath = "../ExcelTemplate";
		/// <summary>
		/// 取得文件路径
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

		#region 文件下载
		/// <summary>
		/// 文件下载
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
				int size = 102400;//每100K同时下载数据 
				byte[] readData = new byte[size];//指定缓冲区的大小 
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
					fs.Read(readData, 0, size);//读入一个压缩块 
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
				//System.IO.File.Delete(fullPath);
			}
		}
		#endregion

		#region 上传文件
		/// <summary>
		/// 上传文件
		/// </summary>
		/// <param name="pHtmlInputFile"></param>
		/// <returns></returns>
		public static void FileUp(System.Web.UI.Page page, HtmlInputFile pHtmlInputFile, ref string pstrFullFileName)
		{
			//获取上传文件的大小
			int fFileSize = pHtmlInputFile.PostedFile.ContentLength;
			//如果没有选择上传的文件
			if (fFileSize == 0)
			{
				PageTool.MsgBox(page, "请选择要上传的文件！");
				return;
			}

			//如果上传的附件大于16兆
			if (fFileSize > 16777216)
			{
				PageTool.MsgBox(page, "上传的附件文件不能大于16兆,请分卷压缩后上传！");
				return;
			}

			//文件名
			String strFullFileName = pHtmlInputFile.PostedFile.FileName;
			FileInfo fi = new FileInfo(strFullFileName);
			//文件后缀名
			String strExt = fi.Extension;
			//实际文件名
			String strFileName = fi.Name;
			//虚文件名
			String strTmpFileName = GetFileName(DirPath + strFileName.Substring(0, strFileName.Length - strExt.Length) +
				DateTime.Now.ToString("yyyyMMdd") + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString()
				+ DateTime.Now.Millisecond.ToString(), strExt);

			//上传
			pHtmlInputFile.PostedFile.SaveAs(DirPath + strTmpFileName);

			pstrFullFileName = DirPath + strTmpFileName;
		}
		#endregion

		#region 产生文件的文件名，防止出现重复的文件
		/// <summary>
		/// 产生文件的文件名，防止出现重复的文件
		/// </summary>
		/// <param name="strFileName">文件名</param>
		/// <param name="strExt">文件后缀</param>
		/// <returns></returns>
		public static string GetFileName(String strFileName, String strExt)
		{
			int intNo = 1;
			FileInfo fi = new FileInfo(strFileName + intNo.ToString() + strExt);

			//如果文件存在
			while (fi.Exists)
			{
				intNo += 1;
				fi = new FileInfo(strFileName + intNo.ToString() + strExt);
			}
			return fi.Name;
		}
		#endregion 

		#region 删除文件
		/// <summary>
		/// 删除文件
		/// </summary>
		/// <param name="pstrFullFileName"></param>
		public static void FileDelete(string pstrFullFileName)
		{
			System.IO.File.Delete(pstrFullFileName);
		}
		#endregion 

		#region 另存文件
		/// <summary>
		/// 另存文件
		/// </summary>
		/// <param name="pstrFullFileName"></param>
		/// <param name="pstrDestFullFileName"></param>
		public static void FileCopy(string pstrFullFileName,string pstrDestFullFileName)
		{
			//读取文件
			FileInfo pFileInfo = new FileInfo(pstrFullFileName);
			//另存为
			pFileInfo.CopyTo(pstrDestFullFileName);
		}
		#endregion 
	}
}
