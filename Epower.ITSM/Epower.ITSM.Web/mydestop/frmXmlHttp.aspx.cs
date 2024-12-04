/****************************************************************************
 * 
 * description:�첽������
 * 
 * 
 * 
 * Create by:zhumingchun
 * Create Date:2007-11-26
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
    public partial class frmXmlHttp : System.Web.UI.Page
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

            if (Request["MYTABLE"] != null)   //�Զ�������
            {
                string sParams = Request["MYTABLE"].ToString();
                WriteDeskDefineToProfile(sParams);
            }
            if (Request["MYTABLEEXPAND"] != null)   //�Զ��������۵�״̬
            {
                string sParams = Request["MYTABLEEXPAND"].ToString();
                WriteDeskDefineExpandToProfile(sParams);
            }
            if (Request["Tabs"] != null)   //jquery��Tabs����ҳ
            {
                string sUserID = "TabsFlowFormsTab";
                if (Request["Tabs"] != "-1")
                {
                    Response.Cookies[sUserID].Value = Request["Tabs"].ToString();
                    Response.Cookies[sUserID].Path = "/";
                    Response.Cookies[sUserID].Expires = DateTime.Now.AddMonths(1);
                }
                else
                {
                    HttpCookie cookie = Request.Cookies[sUserID];
                    string svalue = "0";
                    if (cookie != null)
                    {
                        svalue = cookie.Value.ToString();
                    }
                    Response.Clear();
                    Response.Write(svalue);
                    Response.Flush();
                    Response.End();
                }
            }

            if (Request["PersonOp"] != null)   //jquery���Զ�����ٴ������
            {
                string svalue = string.Empty;
                Response.Clear();
                Response.Write(svalue);
                Response.Flush();
                Response.End();
            }

            if (Request["EquId"] != null)//�ʲ�����
            {
                string equId = Request["EquId"].ToString();
                string changeId = Request["ChangeId"].ToString();
                AssetLock(equId, changeId);
            }
            if (Request["AssetLockId"] != null)//�ʲ��Ƿ�����
            {
                string equId = Request["AssetLockId"].ToString();
                string changeId = Request["ChangeId"].ToString();
                AssetIsLock(equId, changeId);
            }

            if (Request["Owner"] != null && Request["AppID"] != null)//ģ������
            {
                GetFlowModelList(CTools.ToInt64(Request["AppID"].ToString()), Request["Owner"].ToString());
            }
        }
        #endregion

        #region �Զ������� WriteCookie
        /// <summary>
        /// 
        /// </summary>
        private void WriteDeskDefineToProfile(string sParams)
        {
            UserDP.UpdateUserDeskDefine((long)Session["UserID"], sParams);
        }
        #endregion

        #region �Զ��������۵�״̬ WriteCookie
        /// <summary>
        /// 
        /// </summary>
        private void WriteDeskDefineExpandToProfile(string sParams)
        {
            string sUserID = Constant.UserDeskTopCookiesKey + Session["UserID"].ToString() + "_Expand";
            Response.Cookies[sUserID].Value = sParams;
            Response.Cookies[sUserID].Path = "/";
            //Response.Cookies["FeiMainPage"].Domain = "oa.cpm.gd.cn";
            Response.Cookies[sUserID].Expires = DateTime.Now.AddYears(1);
            //UserDP.UpdateUserDeskDefine((long)Session["UserID"], sParams);
        }
        #endregion

        #region ���ͻ�����
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private void CheckCust(string sCust)
        {
            string sSql = " and (E.ShortName like " + StringTool.SqlQ("%" + sCust + "%")
                       + " or E.FullName like " + StringTool.SqlQ("%" + sCust + "%")
                       + " or E.CustomCode like " + StringTool.SqlQ("%" + sCust + "%")
                       + " or E.LinkMan1 like " + StringTool.SqlQ("%" + sCust + "%")
                       + " or E.Tel1 like " + StringTool.SqlQ("%" + sCust + "%") + ")";
            Br_ECustomerDP mBr_ECustomerDP = new Br_ECustomerDP();
            DataTable dt = mBr_ECustomerDP.GetCustomerServic(sSql, string.Empty);

            string sReturnValue = "-1";
            int icount = dt.Rows.Count;
            if (icount == 1)  //�ҵ�Ψһ
            {
                sReturnValue = dt.Rows[0]["Id"].ToString() + "," + dt.Rows[0]["ShortName"].ToString() + ","
                    + dt.Rows[0]["Address"].ToString() + "," + dt.Rows[0]["LinkMan1"].ToString() + ","
                    + dt.Rows[0]["Tel1"].ToString();

                sReturnValue += "," + dt.Rows[0]["CustomCode"].ToString() + "," + dt.Rows[0]["Email"].ToString() + "," + dt.Rows[0]["MastCust"].ToString();

                //���ݿͻ�IDȡ���豸��Ϣ
                Epower.ITSM.SqlDAL.Equ_DeskDP ee = new Epower.ITSM.SqlDAL.Equ_DeskDP();
                ee = ee.GetEquByCustID(long.Parse(dt.Rows[0]["Id"].ToString()));
                sReturnValue += "," + ee.ID.ToString() + "," + ee.Name.ToString() + ","
                    + ee.Positions + "," + ee.Code.ToString() + ","
                    + ee.SerialNumber.ToString() + "," + ee.Breed.Trim() + "," + ee.Model.Trim() + "," + ee.ListID.ToString().Trim() + "," + ee.ListName.Trim();

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
                        + " or Code like " + StringTool.SqlQ("%" + sEqu + "%") + ")";
            if (Request["EquCust"] != null)
                sSql += "And nvl(CostomName,'')  like " + StringTool.SqlQ("%" + Request["EquCust"].ToString().Trim() + "%");
            Epower.ITSM.SqlDAL.Equ_DeskDP mEqu_DeskDP = new Epower.ITSM.SqlDAL.Equ_DeskDP();
            DataTable dt = mEqu_DeskDP.GetDataTable(sSql, string.Empty);

            string sReturnValue = "-1";
            int icount = dt.Rows.Count;
            if (icount == 1)  //�ҵ�Ψһ
            {
                sReturnValue = dt.Rows[0]["Id"].ToString() + "," + dt.Rows[0]["Name"].ToString() + "," + dt.Rows[0]["Positions"].ToString() +
                    "," + dt.Rows[0]["Code"].ToString() + "," + dt.Rows[0]["SerialNumber"].ToString() + "," + dt.Rows[0]["Breed"].ToString() + "," + dt.Rows[0]["Model"].ToString();
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

        #region �ʲ�����

        /// <summary>
        /// �ʲ�����
        /// </summary>
        /// <param name="strEquId"></param>
        private void AssetLock(string strEquId, string strChangeId)
        {
            #region �������

            long equId = (strEquId == string.Empty ? 0 : long.Parse(strEquId));//�ʲ�ID
            long changeId = (strChangeId == string.Empty ? 0 : long.Parse(strChangeId));//���ID
            string sReturnValue = string.Empty;//����ֵ
            Equ_DeskDP ed = new Equ_DeskDP();//�ʲ�

            #endregion

            #region ��������

            sReturnValue = ed.AssetLock(equId, changeId);//��������

            #endregion

            #region ���

            Response.Clear();
            //�����Ƿ������� 0 ��, 1 ��
            Response.Write(sReturnValue);
            Response.Flush();
            Response.End();

            #endregion
        }

        #endregion

        #region �ʲ��Ƿ�����

        /// <summary>
        /// �ʲ��Ƿ�����
        /// </summary>
        /// <param name="strEquId"></param>
        /// <param name="strChangeId"></param>
        private void AssetIsLock(string strEquId, string strChangeId)
        {
            #region �������

            long equId = (strEquId == string.Empty ? 0 : long.Parse(strEquId));//�ʲ�ID
            long changeId = (strChangeId == string.Empty ? 0 : long.Parse(strChangeId));//���ID
            string sReturnValue = string.Empty;//����ֵ
            Equ_DeskDP ed = new Equ_DeskDP();//�ʲ�

            #endregion

            #region ��������

            sReturnValue = ed.AssetIsLock(equId, changeId);//��������

            #endregion

            #region ���

            Response.Clear();
            //�����Ƿ������� 0 ��, 1 ��
            Response.Write(sReturnValue);
            Response.Flush();
            Response.End();

            #endregion
        }

        #endregion

        #region ��ȡ����ģ��

        /// <summary>
        /// ��ȡ����ģ��
        /// </summary>
        /// <param name="strOwner"></param>
        private void GetFlowModelList(long lngAppID, string strOwner)
        {
            long lngOwner = long.Parse(strOwner);
            DataTable dt = AppFieldConfigDP.GetFlowModelList(lngAppID, (long)Session["UserID"], lngOwner);
            string sReturnValue = string.Empty;
            Json json = new Json(dt);
            sReturnValue = "{record:" + json.ToJson() + "}";
            Response.Clear();
            //�����Ƿ������� 0 ��, 1 ��
            Response.Write(sReturnValue);
            Response.Flush();
            Response.End();
        }

        #endregion
    }
}
