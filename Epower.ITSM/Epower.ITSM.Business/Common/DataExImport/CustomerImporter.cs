/*******************************************************************
 * 版权所有：深圳市非凡信息技术有限公司
 * 描述：配置管理-数据导入-客户导入
 * 
 * 
 * 创建人：孙绍棕
 * 创建日期：2013-12-18
 * *****************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.OleDb;
using Epower.ITSM.SqlDAL;
using Epower.DevBase.BaseTools;

namespace Epower.ITSM.Business.Common.DataExImport
{
    /// <summary>
    /// 客户信息导入
    /// </summary>
    public class CustomerImporter : ExcelImporter
    {
        /// <summary>
        /// 用户编号
        /// </summary>
        private long _userID;

        /// <summary>
        /// 用户名
        /// </summary>
        private String _userName;

        /// <summary>
        /// 导入结果
        /// </summary>
        private String _importResult;

        /// <summary>
        /// 导入成功条数
        /// </summary>
        public Int32 ImportedCount
        {
            get;
            private set;
        }

        /// <summary>
        /// 导入失败条数
        /// </summary>
        public Int32 ImportFaildCount
        {
            get;
            private set;
        }

        /// <summary>
        /// 设置用户信息
        /// </summary>
        /// <param name="lngUserID">用户编号</param>
        /// <param name="strUserName">用户名</param>
        public void SetupUserInfo(long lngUserID, String strUserName)
        {
            this._userID = lngUserID;
            this._userName = strUserName;
        }

        /// <summary>
        /// 取客户列表
        /// </summary>
        /// <returns></returns>
        private DataTable GetCustomerList(String strFileURL)
        {
            string sql = "SELECT * FROM [客户导入$]";
            DataSet ds = new DataSet();
            OleDbDataAdapter da = new OleDbDataAdapter(sql, this.GetConnStr(strFileURL));
            da.Fill(ds);    // 读取客户列表

            return ds.Tables[0];
        }

        /// <summary>
        /// 创建客户对象
        /// </summary>
        /// <returns></returns>
        private Br_ECustomerDP CreateCustomerObject()
        {
            Br_ECustomerDP ee = new Br_ECustomerDP();
            ee.Deleted = (int)Epower.ITSM.Base.eRecord_Status.eNormal;
            ee.RegUserID = decimal.Parse(this._userID.ToString());
            ee.RegUserName = this._userName;
            ee.RegTime = DateTime.Now;
            ee.UpdateTime = DateTime.Now;

            return ee;
        }

        /// <summary>
        /// 设置列值到用户对象
        /// </summary>
        /// <param name="strColumnName">列名</param>
        /// <param name="customerModel">用户对象</param>
        /// <param name="row">行对象</param>
        /// <returns></returns>
        private bool SetValue(String strColumnName, Br_ECustomerDP customerModel, DataRow row)
        {
            Object objVal = row[strColumnName];    // 取某列值
            if (objVal == null || String.IsNullOrEmpty(objVal.ToString()) == true)
            {
                if (strColumnName.Contains("服务单位")
                    || strColumnName.Contains("客户名称")
                    || strColumnName.Contains("客户代码"))
                    return false;

                return true;
            }

            String strVal = objVal.ToString().Trim();
            bool isOK = true;

            switch (strColumnName)
            {
                case "对应用户":

                    customerModel.UserID = decimal.Parse(CommonDP.getUserId(strVal));
                    break;
                case "服务单位":

                    try
                    {
                        if (CommonDP.AddMastCustTomer(strVal))
                        {
                            customerModel.MastCustID = decimal.Parse(CommonDP.getMastCustID(strVal));
                            customerModel.MastCustName = strVal;
                        }
                        else
                        {
                            isOK = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        isOK = false;
                        E8Logger.Error(ex);
                    }

                    break;
                case "部门":

                    customerModel.CustDeptName = strVal;

                    break;
                case "客户名称":

                    customerModel.ShortName = strVal;

                    break;
                case "客户代码":

                    customerModel.CustomCode = strVal;

                    break;
                case "英文名称":

                    customerModel.FullName = strVal;

                    break;
                case "电子邮件":

                    customerModel.Email = strVal;

                    break;

                case "客户类型":

                    try
                    {
                        if (CommonDP.IsExistCataID("1019", strVal))
                        {
                            customerModel.CustomerType = decimal.Parse(CommonDP.GetCataIDByNameAndRoot("1019", strVal));
                            customerModel.CustomerTypeName = strVal;
                        }
                    }
                    catch (Exception ex)
                    {
                        E8Logger.Error(ex);
                    }

                    break;
                case "联系人":

                    customerModel.LinkMan1 = strVal;

                    break;

                case "职位":

                    try
                    {
                        customerModel.jobID = decimal.Parse(CommonDP.GetCataIDByNameAndRoot("1014", strVal));
                        customerModel.Job = strVal;
                    }
                    catch (Exception ex)
                    {
                        E8Logger.Error(ex);
                    }

                    break;

                case "联系电话":

                    customerModel.Tel1 = strVal;

                    break;

                case "客户地址":

                    customerModel.Address = strVal;

                    break;

                case "客户权限":

                    customerModel.Rights = strVal;

                    break;

                case "备注":

                    customerModel.Remark = strVal;

                    break;
            }

            return isOK;
        }

        /// <summary>
        /// 遍历Excel文件, 导入客户信息
        /// </summary>
        /// <param name="strFileURL">Excel文件路径</param>
        /// <param name="sbResult">导入结果</param>
        /// <returns></returns>
        public override bool Exec(String strFileURL, ref StringBuilder sbResult)
        {
            DataTable dtCustomer = this.GetCustomerList(strFileURL);

            bool isImported = false;    // True,导入成功; False, 没有导入数据
            Int32 intImportedCount = 0;    // 导入客户计数
            Int32 lost = 0;    // 导入失败      
            Int32 rowIndex = 1;

            foreach (DataRow dr in dtCustomer.Rows)
            {
                rowIndex += 1;

                Br_ECustomerDP customerModel = this.CreateCustomerObject();

                try
                {

                    List<String> listColumn = new List<string>();
                    String strColumnName = String.Empty;
                    bool isOK = true;

                    foreach (DataColumn dc in dtCustomer.Columns)
                    {
                        strColumnName = dc.ColumnName;
                        isOK = this.SetValue(strColumnName, customerModel, dr);

                        if (!isOK)
                        {
                            listColumn.Add(strColumnName);
                        }
                    }


                    if (listColumn.Count > 0)    // 必填字段为空或内容不正确
                    {
                        lost += 1;

                        sbResult.AppendFormat(@"数据行【{0}】导入失败！原因: 客户【{1}】的列【{2}】的值为空或数据不正确。<br /><br/>",
                            rowIndex, customerModel.ShortName, String.Join("、", listColumn.ToArray()));

                        continue;
                    }

                    if (customerModel.ShortName == String.Empty
                        || customerModel.CustomCode == String.Empty)
                    {
                        lost += 1;

                        sbResult.AppendFormat(@"数据行【{0}】导入失败！原因: 客户名称和客户编码不能为空。<br /><br/>",
                                                        rowIndex);

                        continue;
                    }


                    customerModel.InsertRecorded(customerModel);

                    intImportedCount = intImportedCount + 1;
                    isImported = true;

                }
                catch (Exception ex)
                {
                    lost = lost + 1;

                    sbResult.AppendFormat(@"数据行【{0}】导入失败！原因: 客户【{1}】由于[{2}]导入失败。<br /><br/>",
                                rowIndex, customerModel.ShortName, ex.Message);
                }
            }

            this.ImportedCount = intImportedCount;
            this.ImportFaildCount = lost;

            return isImported;
        }


    }
}
