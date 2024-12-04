/****************************************************************************
 * 
 * description:�첽������
 * 
 * 
 * 
 * Create by:yxq
 * Create Date:2011-08-15
 * *************************************************************************/
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
using Epower.DevBase.BaseTools;

using Epower.ITSM.SqlDAL;
using Epower.ITSM.Base;
using Epower.DevBase.Organization.SqlDAL;

namespace Epower.ITSM.Web.mydestop
{
    /// <summary>
    /// frmXmlHttp ��ժҪ˵����
    /// </summary>
    public partial class frmXmlHttpAjax : System.Web.UI.Page
    {
        #region ҳ����� Page_Load
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request["Cust"] != null)    //���ͻ�����
            {
                string sCust = Request["Cust"].ToString();
                CheckCust(sCust);
            }
            else if (Request["Equ"] != null)    //����豸
            {
                string sEqu = Request["Equ"].ToString();
                CheckEqu(sEqu);
            }
            else if (Request["EquIDs"] != null)    //�ʲ�ID��
            {
                string strEqus = Request["EquIDs"].ToString();
                string strEquName = string.Empty;      //����ģ����ѯ���ʲ�����

                if (Request["EquName"] != null)
                {
                    strEquName = Request["EquName"].ToString();
                }
                CheckEqus(strEqus, strEquName);
            }
            
        }
        #endregion

        #region CheckEqus
        /// <summary>
        /// CheckEqus
        /// </summary>
        /// <param name="strEqus">��Ӧ�ó�����ѡ���б��е��ʲ�ID��</param>
        /// /// <param name="strEquName">���������ģ����ѯ���ʲ�����</param>
        private void CheckEqus(string strEqus, string strEquName)
        {
            string strWhere = string.Empty;

            if (strEqus != "")
                strWhere = " and id not in (" + strEqus + ")";

            if (strEquName != "")
            {
                strWhere += " and Name like " + StringTool.SqlQ("%" + strEquName + "%");
            }

            DataTable dt = new DataTable();
            dt = new Equ_DeskDP().GetDataTable(strWhere, string.Empty);

            string sReturnValue = "-1";
            int icount = dt.Rows.Count;
            if (icount == 1)  //�ҵ�Ψһ
            {
                Json json = new Json(dt);
                sReturnValue = "{record:" + json.ToJson() + "}";

            }
            else if (icount > 1)  //�ҵ����
            {
                sReturnValue = "0";
            }
            Response.Clear();
            //�����Ƿ������� 0 ��, 1 ��
            Response.Write(sReturnValue);
            Response.Flush();
            Response.End();
        }
        #endregion

        #region ���ͻ�����
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private void CheckCust(string sCust)
        {
            string sSql = " and (ShortName like " + StringTool.SqlQ("%" + sCust + "%")
                       + " or FullName like " + StringTool.SqlQ("%" + sCust + "%")
                       + " or CustomCode like " + StringTool.SqlQ("%" + sCust + "%")
                       + " or LinkMan1 like " + StringTool.SqlQ("%" + sCust + "%")
                       + " or Email like " + StringTool.SqlQ("%" + sCust + "%")
                       + " or Tel1 like " + StringTool.SqlQ("%" + sCust + "%") + ")";
            Br_ECustomerDP mBr_ECustomerDP = new Br_ECustomerDP();
            DataTable dt = mBr_ECustomerDP.GetDataTableAjax(sSql, "");

            string sReturnValue = "-1";
            int icount = dt.Rows.Count;
            if (icount == 1)  //�ҵ�Ψһ
            {
                Json json = new Json(dt);
                sReturnValue = "{record:" + json.ToJson() + "}";

            }
            else if (icount > 1)  //�ҵ����
            {
                sReturnValue = "0";
            }
            Response.Clear();
            //�����Ƿ������� 0 ��, 1 ��
            Response.Write(sReturnValue);
            Response.Flush();
            Response.End();
        }
        #endregion

        #region ����豸����
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private void CheckEqu(string sEqu)
        {
            string sSql = " and (Name like " + StringTool.SqlQ("%" + sEqu + "%")
                        +" or Code like"+StringTool.SqlQ("%" + sEqu + "%")+ ")";
            if (Request["Equ"] != null && Request["Equ"].ToString().Trim() != "")
                sSql += "And nvl(CostomName,'')  like " + StringTool.SqlQ("%" + Request["Equ"].ToString().Trim() + "%");

            if (Request["EquipmentCatalogID"] != null && decimal.Parse(Request["EquipmentCatalogID"].ToString() == "" ? "0" : Request["EquipmentCatalogID"].ToString()) > 0)
                sSql += " And listId=" + Request["EquipmentCatalogID"].ToString();
            //else
            //    sSql += " And 1=2 "; //û���ʲ�����ID ��ѯ��

            Epower.ITSM.SqlDAL.Equ_DeskDP mEqu_DeskDP = new Epower.ITSM.SqlDAL.Equ_DeskDP();
            DataTable dt = mEqu_DeskDP.GetDataTableAjax(sSql, string.Empty);

            string sReturnValue = "-1";
            int icount = dt.Rows.Count;
            if (icount == 1)  //�ҵ�Ψһ
            {
                Json json = new Json(dt);
                sReturnValue = "{record:" + json.ToJson() + "}";
            }
            else if (icount > 1)  //�ҵ����
            {
                sReturnValue = "0";
            }
            Response.Clear();
            //�����Ƿ������� 0 ��, 1 ��
            Response.Write(sReturnValue);
            Response.Flush();
            Response.End();
        }
        #endregion
    }
}
