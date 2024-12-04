/*******************************************************************
 * 版权所有：深圳市非凡信息技术有限公司
 * 描述：配置管理-数据导入-操作界面
 * 
 * 
 * 创建人：孙绍棕
 * 创建日期：2013-12-19
 * *****************************************************************/

using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Epower.DevBase.BaseTools;
using System.Data;
using System.Data.OleDb;
using Epower.ITSM.SqlDAL;
using Epower.ITSM.Base;
using System.Xml;
using Epower.DevBase.Organization.SqlDAL;
using Epower.ITSM.SqlDAL.ES_TBLCS;
using System.IO;
using Epower.ITSM.Business.Common.DataExImport;
using System.Text;

namespace Epower.ITSM.Web.EquipmentManager
{
    public partial class frmEqu_Insert : BasePage
    {

        /// <summary>
        /// 客户导入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnImportCust_Click(object sender, EventArgs e)
        {
            trCustomerErrorList.Visible = false;

            try
            {
                string url = Server.MapPath("~/Files/") + FileUpload2.FileName;
                //上传文件并指定上传目录的路径   
                FileUpload2.PostedFile.SaveAs(url);


                StringBuilder sbResult = new StringBuilder();
                CustomerImporter customerImporter = new CustomerImporter();

                long lngUserID = long.Parse(Session["UserID"].ToString());
                String strPersonName = Session["PersonName"].ToString();

                customerImporter.SetupUserInfo(lngUserID, strPersonName);

                customerImporter.Exec(url, ref sbResult);




                String strTipMsg = String.Format("导入成功{0}条，失败{1}条! ",
                        customerImporter.ImportedCount, customerImporter.ImportFaildCount);


                if (customerImporter.ImportFaildCount > 0)
                {
                    sbResult.Insert(0, strTipMsg + "<br/><br/>");
                    literalCustomerErrorList.Text = "<br/>" + sbResult.ToString();

                    trCustomerErrorList.Visible = true;
                }


                PageTool.MsgBox(this, strTipMsg);



            }
            catch (Exception ex)
            {
                PageTool.MsgBox(this, "导入文件类型不正确，" + ex.Message);
                E8Logger.Error(ex);
            }

        }

        /// <summary>
        /// 资产导入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnImportEqu_Click(object sender, EventArgs e)
        {
            tdEquErrorList.Visible = false;

            try
            {
                string url = Server.MapPath("~/Files/") + FileUpload1.FileName;
                //上传文件并指定上传目录的路径   
                FileUpload1.PostedFile.SaveAs(url);

                long lngUserID = long.Parse(Session["UserID"].ToString());
                String strPersonName = Session["PersonName"].ToString();

                long lngUserDeptID = long.Parse(Session["UserDeptID"].ToString());
                String strUserDeptName = Session["UserDeptName"].ToString();

                StringBuilder sbResult = new StringBuilder();


                EquImporter equImporter = new EquImporter();
                equImporter.SetupUserInfo(lngUserID, strPersonName, lngUserDeptID, strUserDeptName);

                equImporter.Exec(url, ref sbResult);


                String strTipMsg = "导入成功" + equImporter.ImportedCount + "条，" + "失败" + equImporter.ImportFaildCount + "条! ";



                if (equImporter.ImportFaildCount > 0)
                {
                    sbResult.Insert(0, strTipMsg + "<br/><br/>");

                    labEqu.Visible = true;
                    labEqu.Text = "<br /> " + sbResult.ToString();
                    labEqu.ForeColor = System.Drawing.Color.Red;

                    tdEquErrorList.Visible = true;
                }

                PageTool.MsgBox(this, strTipMsg);

            }
            catch (Exception ex)
            {
                String strErrMsg = "导入文件类型不正确，" + ex.Message;
                PageTool.MsgBox(this, strErrMsg);

                E8Logger.Error(ex);
            }


        }

        /// <summary>
        /// 客户导入模板下载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkCust_Click(object sender, EventArgs e)
        {
            Epower.ITSM.Web.Common.ExcelExport.DownLoadCustExcel(this, Session["UserID"].ToString());
        }

        /// <summary>
        /// 资产导入模板下载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkEqu_Click(object sender, EventArgs e)
        {
            Epower.ITSM.Web.Common.ExcelExport.DownLoadEquExcel(this, Session["UserID"].ToString());
        }

    }
}
