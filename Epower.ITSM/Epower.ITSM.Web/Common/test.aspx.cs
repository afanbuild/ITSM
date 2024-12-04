using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using System.Data.Odbc;
using System.Data.OleDb;

namespace Epower.ITSM.Web.Common
{
    public partial class test : System.Web.UI.Page
    {
        /// <summary>
        /// 数据文件
        /// </summary>
        private static string table = @"e:\zh\zhwy20070904832781.dbf";

        /// <summary>
        /// Odbc链接字符串 ,其中SourceDB可以是dbf文件所在的目录也可以制定到文件,作用是一样的,如:SourceDB=d:\;或SourceDB==D:\Allart.dbf
        /// </summary>
        private static string strConn = @"Driver={Microsoft Visual FoxPro Driver};SourceType=DBF;SourceDB=E:\ZH\K3\zhwypz.dbf;Exclusive=No;NULL=NO;Collate=Machine;BACKGROUNDFETCH=NO;DELETED=NO";

        /// <summary>
        /// Oledb链接字符串 
        /// 1,到微软网站下载<Microsoft OLE DB Provider for Visual FoxPro 9.0>并安装,这样oledb才能识别provider=VFPOLEDB.1,
        /// http://www.microsoft.com/downloads/thankyou.aspx?familyId=e1a87d8f-2d58-491f-a0fa-95a3289c5fd4&displayLang=en
        /// 2,其中SourceDB可以是dbf文件所在的目录也可以制定到文件,作用是一样的,如:data source=d:\;或data source==D:\Allart.dbf
        /// </summary>
        private static string strOledbConn = @"provider=VFPOLEDB.1;data source=E:\ZH\;user id=admin;password=";

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 初始化vfp数据库，创建user表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnInti_Click(object sender, EventArgs e)
        {
            OleDbConnection conn = new OleDbConnection(strOledbConn);
            OleDbCommand cmd = new OleDbCommand();
            string sql = "CREATE TABLE zhwy11.dbf(ID c(6) ,Name C(20),BirthDay t,Salary n(6,2))";
            cmd.CommandText = sql;
            cmd.Connection = conn;
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
            showDataWithOledb();
            Response.Write("表格生成成功！");
        }

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnInsert_Click(object sender, EventArgs e)
        {
            OdbcConnection conn = new OdbcConnection(strConn);
            OdbcCommand cmd = new OdbcCommand();
            //字符c,时间t,数字n
            string sql = "insert into " + table + "(id,name,birthday,salary) values('1','haha',{^1985-09-10},3300.1)";
            cmd.CommandText = sql;
            cmd.Connection = conn;
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
            showDataWithOdbc();
            Response.Write("表格插入成功！");
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnUpdate_Click(object sender, EventArgs e)
        {

            OdbcConnection conn = new OdbcConnection(strConn);
            OdbcCommand cmd = new OdbcCommand();
            string sql = "update " + table + " set ID ='2',name='haha2'";
            cmd.CommandText = sql;
            cmd.Connection = conn;
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
            showDataWithOdbc();
            Response.Write("表格更新成功！");
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDelete_Click(object sender, EventArgs e)
        {

            OdbcConnection conn = new OdbcConnection(strConn);
            OdbcCommand cmd = new OdbcCommand();
            string sql = "delete from  " + table;
            cmd.CommandText = sql;
            cmd.Connection = conn;
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
            showDataWithOdbc();
            Response.Write("表格删除成功！");
        }

        /// <summary>
        /// 使用Oledb显示数据中数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnShowWithOledb_Click(object sender, EventArgs e)
        {
            showDataWithOledb();
            Response.Write("成功调用Oledb显示数据！");
        }
        /// <summary>
        /// 使用Odbc显示数据中数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnShowWithOdbc_Click(object sender, EventArgs e)
        {
            showDataWithOdbc();
            Response.Write("成功调用Odbc显示数据！");
        }

        /// <summary>
        /// 使用Odbc驱动链接数据库并显示数据
        /// </summary>
        private void showDataWithOdbc()
        {
            OdbcConnection conn = new OdbcConnection();
            conn.ConnectionString = strConn;
            conn.Open();
            string sql = @"select  * from " + table;
            OdbcDataAdapter da = new OdbcDataAdapter(sql, conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            Gridview1.DataSource = dt.DefaultView;
            Gridview1.DataBind();
            conn.Close();
        }

        /// <summary>
        /// 使用OleDb驱动链接数据库并显示数据
        /// </summary>
        private void showDataWithOledb()
        {
            OleDbConnection conn = new OleDbConnection();
            conn.ConnectionString = strOledbConn;
            conn.Open();
            string sql = @"select  * from " + table;
            OleDbDataAdapter da = new OleDbDataAdapter(sql, conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            Gridview1.DataSource = dt.DefaultView;
            Gridview1.DataBind();
            conn.Close();
        }

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnInsertOle_Click(object sender, EventArgs e)
        {
            OleDbConnection conn = new OleDbConnection();
            conn.ConnectionString = strOledbConn;
            OleDbCommand cmd = new OleDbCommand();
            //字符c,时间t,数字n
            string sql = "insert into " + table + "(id,name,birthday,salary) values('1','haha',{^1985-09-10},3300.1)";
            cmd.CommandText = sql;
            cmd.Connection = conn;
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
            showDataWithOledb();
            Response.Write("表格插入成功！");
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnUpdateOle_Click(object sender, EventArgs e)
        {

            OleDbConnection conn = new OleDbConnection();
            conn.ConnectionString = strOledbConn;
            OleDbCommand cmd = new OleDbCommand();
            string sql = "update " + table + " set ID ='2',name='haha2'";
            cmd.CommandText = sql;
            cmd.Connection = conn;
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
            showDataWithOledb();
            Response.Write("表格更新成功！");
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDeleteOle_Click(object sender, EventArgs e)
        {
            OleDbConnection conn = new OleDbConnection();
            conn.ConnectionString = strOledbConn;
            OleDbCommand cmd = new OleDbCommand();
            string sql = "delete from  " + table;
            cmd.CommandText = sql;
            cmd.Connection = conn;
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
            showDataWithOledb();
            Response.Write("表格删除成功！");
        }

        /// <summary>
        /// 上传表单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnUp_Click(object sender, EventArgs e)
        {
            EpowerCom.FieldValues fv = new EpowerCom.FieldValues();
            fv.Add("姓氏", txtName.Text.Trim());
            fv.Add("test", txtTest.Text.Trim());
            

            EpowerCom.FieldValues fvattach = new EpowerCom.FieldValues();
            fvattach.Add("testattach1","E:\\财务开支.txt");
            fvattach.Add("testattach2", "E:\\财务开支.txt");
            //
            SPSFormFileUP.UpFile(this, "testfile", fv.GetXmlObject().ToString(), fvattach.GetXmlObject().ToString());
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            HttpRuntime.Cache.Insert("EpCacheValidFlowModel", false);
        }

    }
}

