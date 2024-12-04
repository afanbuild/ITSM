using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data.OleDb;

namespace Epower.DevBase.BaseTools
{

    /// <summary>
    /// 
    /// </summary>
    public class IDataFieldProcess
    {
        /// <summary>
        /// 
        /// </summary>
        public IDataFieldProcess()
        {
            //
            // TODO: �ڴ˴���ӹ��캯���߼�
            //
        }
        /// <summary>
        ///  �����ֶν��
        /// </summary>
        /// <param name="lngID"></param>
        /// <param name="lngAppID"></param>
        /// <param name="lngOpID"></param>
        /// <returns></returns>
        public virtual string GetDataFieldProcess(string sFieldValue, string sPara)
        {
            return sFieldValue;
        }
    }

	/// <summary>
	/// ExcelProc ��ժҪ˵����
	/// </summary>
	public class ExportExcel
	{
		private static OleDbConnection _oleConn;
		private static OleDbCommand _oleCmdSelect;
		public ExportExcel()
		{
			//
			// TODO: �ڴ˴���ӹ��캯���߼�
			//
		}

		private static string AddWithComma(string strSource,string strAdd)
		{
			if (strSource !="") strSource = strSource += ", ";
			return strSource + strAdd;
		}

		private static OleDbDataAdapter SetSheetQueryAdapter(DataTable dt)
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tmpFileName">Դ�ļ�</param>
        /// <param name="toFileName">Ŀ���ļ�</param>
        /// <param name="arrFields">�ֶ�����</param>
        /// <param name="dtSource">Դ����</param>
        public static void InsertExcel1(string tmpFileName, string toFileName, string[] fileName, string[] arrFields, System.Data.DataTable dtSource, IDataFieldProcess idfp)
        {
            string Tablename = "[Sheet1$]";
            System.IO.File.Copy(tmpFileName, toFileName, true);

            System.IO.FileInfo fi = new System.IO.FileInfo(toFileName);
            fi.Attributes = System.IO.FileAttributes.Normal;

            string strCon = @" Provider = Microsoft.Jet.OLEDB.4.0;Data Source= " + toFileName + ";Extended Properties=Excel 8.0";
            if (System.Configuration.ConfigurationSettings.AppSettings["Is64MachineExcel"] != null
                && System.Configuration.ConfigurationSettings.AppSettings["Is64MachineExcel"].ToLower() == "true")
            {
                strCon = @" Provider = Microsoft.ACE.OLEDB.12.0;Data Source= " + toFileName + ";Extended Properties=Excel 12.0";
            }

            _oleConn = new OleDbConnection(strCon);
            string strCom = " SELECT * FROM  " + Tablename + " WHERE 1=2";
            try
            {
                _oleConn.Open();
                //���������ӣ��õ�һ�����ݼ�
                _oleCmdSelect = new OleDbCommand(strCom, _oleConn);
                OleDbDataAdapter myCommand = new OleDbDataAdapter(strCom, _oleConn);

                OleDbCommandBuilder cb = new OleDbCommandBuilder(myCommand);
                //����һ�� DataTable����
                DataTable myDT = new DataTable(Tablename);
                //�õ��Լ���DataSet����
                myCommand.Fill(myDT);

                foreach (DataRow drSource in dtSource.Rows)
                {
                    if (arrFields != null)
                    {
                        DataRow dr = myDT.NewRow();

                        for (int n = 0; n < arrFields.Length; n++)
                        {
                            if (!string.IsNullOrEmpty(arrFields[n]))
                            {
                                string sFileName = arrFields[n].ToString();
                                string sValue = "";

                                if (sFileName.IndexOf(":") > 0)
                                {
                                    //����ֶ�������: �ֿ�,�򾭹� һ�δ�������
                                    string[] sArr = sFileName.Split(":".ToCharArray());
                                    sValue = idfp.GetDataFieldProcess(drSource[sArr[0]].ToString(), sArr[1]);
                                }
                                else
                                {

                                    sValue = drSource[sFileName].ToString();
                                }
                                if (sFileName == "ConfigureValue:ParaConfigureValue")
                                {
                                    if (sValue == "�������ã�      �������ã�")
                                        sValue = " ";
                                }
                                if (sValue == "")
                                {
                                    sValue = " ";
                                }
                                if (sValue.Length > 255)
                                    sValue = sValue.Substring(0, 252) + "...";

                                dr[n] = (object)sValue;
                            }
                        }

                        myDT.Rows.InsertAt(dr, 0);
                    }
                }

                DataRow drow = myDT.NewRow();
                for (int i = 0; i < fileName.Length; i++)
                {
                    if (!string.IsNullOrEmpty(fileName[i]))
                    {
                        drow[i] = (object)fileName[i];
                    }
                }
                myDT.Rows.InsertAt(drow, 0);
                OleDbDataAdapter oleAdapter = SetSheetQueryAdapter(myDT);

                oleAdapter.Update(myDT);

            }
            catch (Exception er)
            {
                throw new Exception(er.Message);
            }
            finally
            {
                _oleConn.Close();
            }

        }
       /// <summary>
       /// 
       /// </summary>
       /// <param name="tmpFileName">Դ�ļ�</param>
       /// <param name="toFileName">Ŀ���ļ�</param>
       /// <param name="arrFields">�ֶ�����</param>
       /// <param name="dtSource">Դ����</param>
		public static void InsertExcel(string tmpFileName,string toFileName,string[] arrFields,System.Data.DataTable dtSource,IDataFieldProcess idfp)
		{            
			string Tablename = "[Sheet1$]";
			System.IO.File.Copy(tmpFileName,toFileName,true);

			System.IO.FileInfo fi=new System.IO.FileInfo(toFileName);
			fi.Attributes=System.IO.FileAttributes.Normal;

			string strCon = @" Provider = Microsoft.Jet.OLEDB.4.0;Data Source= "+toFileName+";Extended Properties=Excel 8.0" ;
            if (System.Configuration.ConfigurationSettings.AppSettings["Is64MachineExcel"] != null
                && System.Configuration.ConfigurationSettings.AppSettings["Is64MachineExcel"].ToLower() == "true")
            {
                strCon = @" Provider = Microsoft.ACE.OLEDB.12.0;Data Source= " + toFileName + ";Extended Properties=Excel 12.0";
            }

			_oleConn = new OleDbConnection ( strCon ) ;
			string strCom = " SELECT * FROM  "+ Tablename+" WHERE 1=2";
			try
			{
				_oleConn.Open () ;
				//���������ӣ��õ�һ�����ݼ�
				_oleCmdSelect =new OleDbCommand(strCom, _oleConn); 
				OleDbDataAdapter myCommand = new OleDbDataAdapter ( strCom , _oleConn);

				OleDbCommandBuilder cb=new OleDbCommandBuilder(myCommand);
				//����һ�� DataTable����
				DataTable myDT = new DataTable (Tablename) ;
				//�õ��Լ���DataSet����
				myCommand.Fill(myDT) ; 
				foreach(DataRow drSource in dtSource.Rows)    
				{
					DataRow dr = myDT.NewRow();
					for(int n=0;n<arrFields.Length;n++)
					{
						string sFileName=arrFields[n].ToString();
                        string sValue = "";

                        if (sFileName.IndexOf(":") > 0)
                        {
                            //����ֶ�������: �ֿ�,�򾭹� һ�δ�������
                            string[] sArr = sFileName.Split(":".ToCharArray());
                            sValue = idfp.GetDataFieldProcess(drSource[sArr[0]].ToString(), sArr[1]);
                        }
                        else
                        {
                            sValue = drSource[sFileName].ToString();
                        }
                        if (sFileName == "ConfigureValue:ParaConfigureValue")
                        {
                            if (sValue == "�������ã�      �������ã�")
                                sValue = " ";
                        }
                        if (sValue == "")
                        {
                            sValue = " ";
                        }
                        if (sValue.Length > 255)
                            sValue = sValue.Substring(0, 252) + "...";
                        dr[n] = (object)sValue;
					}
					myDT.Rows.InsertAt(dr,0);
				}
				OleDbDataAdapter oleAdapter = SetSheetQueryAdapter(myDT);		
				
				oleAdapter.Update(myDT); 
                                  
			}
			catch(Exception er)
			{
				throw new Exception(er.Message);
			}
			finally
			{
				_oleConn.Close () ; 				
			}
            
		}
	}
}
