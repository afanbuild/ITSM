/*******************************************************************
 * 版权所有：深圳市非凡信息技术有限公司
 * 描述：将附件文档转换为SWF文件
 *       服务器必须安装FlashPrinter，且必须是默认路径
 *       产生的CreateSWF.exe程序要放在Web目录下的Lib文件夹中
 * 创建人：guoch
 * 创建日期：2014-02-24
 * *****************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;
using Epower.DevBase.BaseTools;

namespace Epower.DevBase.Flash
{
    public class Program
    {
        static void Main(string[] args)
        {
            Process process = new Process();     //创建进程对象 
            string swfFile = "";
            string swfName = "";                    //产生的swf文件名
            string filePath = "";                //附件路径
            string fileName = "";                //附件名称
            string fileID = "";                 //附件ID
            string copyFile = "";           //复制的附件
            string err = "";
            try
            {
                //FlashPrinter的路径
                string paperroot = @"C:\Program Files\Macromedia\FlashPaper 2\FlashPrinter.exe";
                if (!File.Exists(paperroot))
                {
                    //没有安装FlashPrinter
                    return;
                }
                if (args.Length > 2)
                {
                    filePath = args[0];
                    fileName = args[1];
                    fileID = args[2];
                }
                else
                {
                    //filePath = "F:\\111\\";
                    //fileName = "银监检查要求整改项需求.docx";
                    //fileID = "12345";
                    return;
                }
                ProcessStartInfo startInfo = new ProcessStartInfo();
                //设置SWF路径,名称
                swfName = fileID + ".swf";
                swfFile = filePath + swfName;
                //swfFile = @"F:\111\ttt.swf";
                if (File.Exists(swfFile))
                {
                    File.Delete(swfFile);
                }
                //将附件复制一份到目录
                copyFile = filePath + fileName;
                if (File.Exists(copyFile))//判断是否有重复附件
                {
                    File.Delete(copyFile);//删除重复附件
                }
                //将附件复制一份，并且命名好，以便FlashPrinter调用
                File.Copy(filePath + fileID, copyFile, true);
                //配置参数
                startInfo.FileName = paperroot;
                startInfo.Arguments = copyFile + " -o " + swfFile;
                startInfo.CreateNoWindow = true;
                startInfo.UseShellExecute = false;     //不使用系统外壳程序启动 
                startInfo.RedirectStandardInput = false;   //不重定向输入 
                startInfo.RedirectStandardOutput = false;   //重定向输出 
                startInfo.CreateNoWindow = true;     //不创建窗口 
                process.StartInfo = startInfo;
                process.Start();
                process.WaitForExit();
            }
            catch (Exception ex)
            {
                err = ex.Message;
                throw ex;
            }
            finally
            {
                FileInfo f = new FileInfo(@"D:\Err.txt");
                StreamWriter w = f.CreateText();
                string s = "";
                foreach (string str in args)
                {
                    s += str + ",";
                }
                s = s.TrimEnd(',');
                w.WriteLine(s + " length=" + args.Length);
                if (err != "")
                {
                    w.WriteLine(err);
                }
                w.Close();
                if (swfFile.Length > 0)
                {
                    System.Threading.Thread.Sleep(8000);
                    //for (int i = 0; i < 100; i++)//等待30秒转换
                    //{
                    //    if (File.Exists(swfFile))
                    //    {
                    //        break;
                    //    }
                    //    System.Threading.Thread.Sleep(300);
                    //}
                    //释放资源
                    process.Close();
                    process.Dispose();
                    if (File.Exists(copyFile))//删除复制的附件
                    {
                        File.Delete(copyFile);
                    }
                }
               
                Console.ReadLine();
            }
        }
      
    }
}
