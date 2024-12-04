using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using Epower.ITSM.SqlDAL;
using System.Timers;
using System.Data.OracleClient;
using Epower.DevBase.BaseTools;

namespace Epower.ITSM.Web
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            Application["MAXFLOWMODELID"] = -1;
            Timer objTimer = new Timer();
            objTimer.Interval = 1000 *60 ; //这个时间单位毫秒,比如10秒，就写10000 
            objTimer.Enabled = true;
            objTimer.Elapsed += new ElapsedEventHandler(objTimer_Elapsed);
        }

        void objTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
           int maxFlowModelId=GetMaxFlowModelId();
            if (maxFlowModelId >  int.Parse(Application["MAXFLOWMODELID"].ToString()))
            {
                Application["MAXFLOWMODELID"] = maxFlowModelId;              
                HttpRuntime.Cache["EpCacheValidFlowModel"] = false;
                E8Logger.Info("EpCacheValidFlowModel==>false");
            }
        } 

        private int GetMaxFlowModelId()
        {
            int result = 0;
            OracleConnection cn =null ;
            OracleDataReader dr = null;
            try
            {
                string strSQL = "SELECT NVL(MAX(FLOWMODELID),0) AS FLOWMODELID FROM ES_FLOWMODEL ";
                cn = ConfigTool.GetConnection();
                dr = OracleDbHelper.ExecuteReader(cn, CommandType.Text, strSQL);
        
                while (dr.Read())
                {
                    result = dr.GetInt32(0);
                }
                dr.Close();
                return result;
            }
            catch
            {
                return -1;
            }
            finally
            {                                
                ConfigTool.CloseConnection(cn);
            }
           
        }

        protected void Application_End(object sender, EventArgs e)
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Session_Start(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Session_End(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_Error(Object sender, EventArgs e)
        {
            Exception exception = HttpContext.Current.Server.GetLastError().GetBaseException();

            string errorInfo = "实例：" + exception.InnerException + "；错误信息："
                + exception.Message + "；对象：" + exception.Source + "；方法："
                + exception.TargetSite + "；其它：" + exception.ToString();
            //异常日志
            Epower.ITSM.Log.ExceptionLog.PostException(errorInfo);

            //Response.Write(@"<script language='javascript'>alert(" + Epower.DevBase.BaseTools.StringTool.JavaScriptQ("系统出现异常，请与系统管理员联系，谢谢！") + "); </script> ");
            //Response.Write("<scipt>alert('系统出现异常，请与系统管理员联系，谢谢！');</scipt>");
            //Response.End();
        }
    }
}