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
        /// �����ļ�
        /// </summary>
        private static string table = @"e:\zh\zhwy20070904832781.dbf";

        /// <summary>
        /// Odbc�����ַ��� ,����SourceDB������dbf�ļ����ڵ�Ŀ¼Ҳ�����ƶ����ļ�,������һ����,��:SourceDB=d:\;��SourceDB==D:\Allart.dbf
        /// </summary>
        private static string strConn = @"Driver={Microsoft Visual FoxPro Driver};SourceType=DBF;SourceDB=E:\ZH\K3\zhwypz.dbf;Exclusive=No;NULL=NO;Collate=Machine;BACKGROUNDFETCH=NO;DELETED=NO";

        /// <summary>
        /// Oledb�����ַ��� 
        /// 1,��΢����վ����<Microsoft OLE DB Provider for Visual FoxPro 9.0>����װ,����oledb����ʶ��provider=VFPOLEDB.1,
        /// http://www.microsoft.com/downloads/thankyou.aspx?familyId=e1a87d8f-2d58-491f-a0fa-95a3289c5fd4&displayLang=en
        /// 2,����SourceDB������dbf�ļ����ڵ�Ŀ¼Ҳ�����ƶ����ļ�,������һ����,��:data source=d:\;��data source==D:\Allart.dbf
        /// </summary>
        private static string strOledbConn = @"provider=VFPOLEDB.1;data source=E:\ZH\;user id=admin;password=";

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// ��ʼ��vfp���ݿ⣬����user��
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
            Response.Write("������ɳɹ���");
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnInsert_Click(object sender, EventArgs e)
        {
            OdbcConnection conn = new OdbcConnection(strConn);
            OdbcCommand cmd = new OdbcCommand();
            //�ַ�c,ʱ��t,����n
            string sql = "insert into " + table + "(id,name,birthday,salary) values('1','haha',{^1985-09-10},3300.1)";
            cmd.CommandText = sql;
            cmd.Connection = conn;
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
            showDataWithOdbc();
            Response.Write("������ɹ���");
        }

        /// <summary>
        /// ��������
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
            Response.Write("�����³ɹ���");
        }

        /// <summary>
        /// ɾ������
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
            Response.Write("���ɾ���ɹ���");
        }

        /// <summary>
        /// ʹ��Oledb��ʾ����������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnShowWithOledb_Click(object sender, EventArgs e)
        {
            showDataWithOledb();
            Response.Write("�ɹ�����Oledb��ʾ���ݣ�");
        }
        /// <summary>
        /// ʹ��Odbc��ʾ����������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnShowWithOdbc_Click(object sender, EventArgs e)
        {
            showDataWithOdbc();
            Response.Write("�ɹ�����Odbc��ʾ���ݣ�");
        }

        /// <summary>
        /// ʹ��Odbc�����������ݿⲢ��ʾ����
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
        /// ʹ��OleDb�����������ݿⲢ��ʾ����
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
        /// ��������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnInsertOle_Click(object sender, EventArgs e)
        {
            OleDbConnection conn = new OleDbConnection();
            conn.ConnectionString = strOledbConn;
            OleDbCommand cmd = new OleDbCommand();
            //�ַ�c,ʱ��t,����n
            string sql = "insert into " + table + "(id,name,birthday,salary) values('1','haha',{^1985-09-10},3300.1)";
            cmd.CommandText = sql;
            cmd.Connection = conn;
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
            showDataWithOledb();
            Response.Write("������ɹ���");
        }

        /// <summary>
        /// ��������
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
            Response.Write("�����³ɹ���");
        }

        /// <summary>
        /// ɾ������
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
            Response.Write("���ɾ���ɹ���");
        }

        /// <summary>
        /// �ϴ���
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnUp_Click(object sender, EventArgs e)
        {
            EpowerCom.FieldValues fv = new EpowerCom.FieldValues();
            fv.Add("����", txtName.Text.Trim());
            fv.Add("test", txtTest.Text.Trim());
            

            EpowerCom.FieldValues fvattach = new EpowerCom.FieldValues();
            fvattach.Add("testattach1","E:\\����֧.txt");
            fvattach.Add("testattach2", "E:\\����֧.txt");
            //
            SPSFormFileUP.UpFile(this, "testfile", fv.GetXmlObject().ToString(), fvattach.GetXmlObject().ToString());
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            HttpRuntime.Cache.Insert("EpCacheValidFlowModel", false);
        }

    }
}

