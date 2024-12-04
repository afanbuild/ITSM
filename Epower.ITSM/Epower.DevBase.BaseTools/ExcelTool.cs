using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data.OleDb;

namespace  Epower.DevBase.BaseTools
{
    /// <summary>
    /// ExcelTool 的摘要说明。
    /// </summary>
    public class ExcelTool
    {
        public  OleDbConnection _oleConn;
        public  OleDbCommand _oleCmdSelect;
        public ExcelTool()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        private  string AddWithComma(string strSource,string strAdd)
        {
            if (strSource !="") strSource = strSource += ", ";
            return strSource + strAdd;
        }

        public  OleDbDataAdapter SetSheetQueryAdapter(DataTable dt)
        {
            // Deleting in Excel workbook is not possible
            //So this command is not defined
            try
            {
				    
                OleDbDataAdapter oleda = new OleDbDataAdapter(_oleCmdSelect); 
                string strInsertPar="";
                string strInsert="";				
                

                for (int iCol=0;iCol<dt.Columns.Count;iCol++)
                {
                    strInsert= AddWithComma(strInsert,dt.Columns[iCol].ColumnName);
                    strInsertPar= AddWithComma(strInsertPar,"?");
                    
                }

                string strTable = "[Sheet1$]";  
                strInsert = "INSERT INTO "+ strTable + "(" + strInsert +") Values (" + strInsertPar + ")";
                oleda.InsertCommand = new OleDbCommand(strInsert,_oleConn);                
                OleDbParameter oleParIns = null;                                
                for (int iCol=0;iCol<dt.Columns.Count;iCol++)
                {
                    oleParIns = new OleDbParameter("?",dt.Columns[iCol].DataType.ToString());
                    
                    oleParIns.SourceColumn =dt.Columns[iCol].ColumnName;
                    
                    oleda.InsertCommand.Parameters.Add(oleParIns);
                    
                    oleParIns=null;                    
                }
                return oleda;
            }			
            catch (Exception ex)
            {
                throw ex;
            }
			
        }
       
    }
}
