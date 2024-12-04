using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.OracleClient;
using appDataProcess;
using Epower.ITSM.SqlDAL;
using Epower.DevBase.BaseTools;

namespace Epower.ITSM.Web.AppForms
{
    public partial class frm_EmailTimeSet : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string time = SeachTime();
                this.txt_Time.Text = time;
            }
        }


        public string SeachTime()
        {
             OracleConnection cn = ConfigTool.GetConnection();
             string time = "";
             try
             {
                 string seach = "select I_Time from Br_Interval_Time";
                 DataTable timedt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, seach);
                 if (timedt != null && timedt.Rows.Count > 0)
                 {
                     time = timedt.Rows[0]["I_Time"].ToString();
                 }
             }
             catch (Exception ex)
             {
                 throw ex;
             }
             finally
             {
                 ConfigTool.CloseConnection(cn);
             }

             return time;
        }

        /// <summary>
        /// 保存时间设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_SaveTime_Click(object sender, EventArgs e)
        {
            decimal time = Convert.ToDecimal(this.txt_Time.Text.ToString());
            if (time.ToString() == "")
                time = 10;
            OracleConnection cn = ConfigTool.GetConnection();

            try
            {
                int del = 0;
                if (SeachTime() != "")
                {
                    string delte = "delete from Br_Interval_Time";
                  del=  OracleDbHelper.ExecuteNonQuery(cn, CommandType.Text, delte);
                }
               
                    string inserTime = string.Format("insert into Br_Interval_Time values(Br_Interval_TimeID.nextval,{0},'{1}')", time, "0");
                    int num = OracleDbHelper.ExecuteNonQuery(cn, CommandType.Text, inserTime);
                    if (num > 0)
                    {
                        PageTool.MsgBox(this, "保存成功！");
                    }
                    else
                    { 
                        PageTool.MsgBox(this, "失败！");
                    }
               

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                ConfigTool.CloseConnection(cn);
            }
        }
    }
}
