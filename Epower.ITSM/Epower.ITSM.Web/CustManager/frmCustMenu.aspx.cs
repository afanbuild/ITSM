using System;
using System.Text;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using Epower.DevBase.BaseTools;
using Epower.ITSM.SqlDAL;
using Epower.ITSM.Base;
using Epower.DevBase.Organization.SqlDAL;

namespace Epower.ITSM.Web.CustManager
{
	/// <summary>
	/// Menu ��ժҪ˵����
	/// </summary>
    public partial class frmCustMenu : BasePage
	{
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		protected void Page_Load(object sender, System.EventArgs e)
        {
            #region Ȩ�޲���ע��    

            //�¼���
            if (CheckRight(Constant.CustomerService) == false)
            {
                ListItem value = new ListItem("�¼���", "0");

                ddlObject.Items.Remove(value);
            }
            //Ͷ�ߵ�
            if (CheckRight(Constant.BYTSQuery) == false)
            {
                ListItem value = new ListItem("Ͷ�ߵ�", "1");

                ddlObject.Items.Remove(value);
            }
            //���ⵥ
            if (CheckRight(Constant.QuestionTrace) == false)
            {
                ListItem value = new ListItem("���ⵥ", "2");

                ddlObject.Items.Remove(value);
            }

            //�����
            if (CheckRight(Constant.EquChangeQuery) == false)
            {
                ListItem value = new ListItem("�����", "3");

                ddlObject.Items.Remove(value);
            }
            #endregion 
        }


        #region ���Ȩ�� CheckRight
        /// <summary>
        /// ���Ȩ��

        /// </summary>
        /// <param name="OperatorID"></param>
        /// <returns></returns>
        private bool CheckRight(long OperatorID)
        {
            RightEntity re = (RightEntity)((Hashtable)Session["UserAllRights"])[OperatorID];
            if (re == null)
                return false;
            else
                return re.CanRead;
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdOK_Click(object sender, EventArgs e)
        {
        } 
	}
}
