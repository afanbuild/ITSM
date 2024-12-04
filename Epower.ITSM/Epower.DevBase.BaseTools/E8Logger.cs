using System;
using System.Collections.Generic;
using System.Text;
using System.Data.OracleClient;

using log4net;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace Epower.DevBase.BaseTools
{
    /// <summary>
    /// 
    /// </summary>
    public class E8Logger
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        public static void Info(string msg)
        {
            log.Info(msg);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        public static void Debug(string msg)
        {
            log.Debug(msg);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        public static void Error(string msg)
        {
            log.Error(msg);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ex"></param>
        public static void Error(Exception ex)
        {
            log.Error(ex.Message + "\r\n" + ex.StackTrace);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="commandParameters"></param>
        public static void SQLogger(string cmdText, params OracleParameter[] commandParameters)
        {
            Debug("=================================");
            Debug(cmdText);
            if (commandParameters == null || commandParameters.Length == 0)
            {
                Debug("commandParameters is null.");
                return;
            }
            for (int i = 0; i < commandParameters.Length; i++)
            { 
                Debug(string.Format("参数:{0}{3}类型{1}{3}值{2}{3}",
                                        commandParameters[i].ParameterName,
                                        commandParameters[i].OracleType.ToString(),
                                        (commandParameters[i].Value == null )?"null": commandParameters[i].Value.ToString(),
                                        Environment.NewLine));
            }
            
        }
    }
}
