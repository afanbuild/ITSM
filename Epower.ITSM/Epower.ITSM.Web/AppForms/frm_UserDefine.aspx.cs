/***************************************************************************
 * desc���Զ�������̱�ҳ��
 * 
 * 
 * 
 * Create By:zhumingchun
 * Create Date:2011-08-17
 * ************************************************************************/

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

//����
using System.Xml;
using appDataProcess;
using Epower.DevBase.Organization.SqlDAL;
using Epower.DevBase.Organization.Base;
using EpowerCom;
using EpowerGlobal;
using Epower.DevBase.BaseTools;
using Epower.ITSM.SqlDAL;
using Epower.ITSM.Base;

namespace Epower.ITSM.Web.AppForms
{
    public partial class frm_UserDefine : BasePage
    {
        #region ��������
        /// <summary>
        /// ĸ��ҳ�Ķ������
        /// </summary>
        private FlowFormsUserDefine myFlowForms;
        #endregion 

        #region ���Զ���
        /// <summary>
        /// ������
        /// </summary>
        protected string sDjlx
        {
            get
            {
                if (ViewState["djlx"] != null)
                {
                    return ViewState["djlx"].ToString();
                }
                else
                {
                    return "TT";
                }
            }
            set
            {
                ViewState["djlx"] = value;
            }
        }
        /// <summary>
        /// ��sn
        /// </summary>
        protected string sDjsn
        {
            get
            {
                if (ViewState["djsn"] != null)
                {
                    return ViewState["djsn"].ToString();
                }
                else
                {
                    return "0";
                }
            }
            set
            {
                ViewState["djsn"] = value;
            }
        }
        /// <summary>
        /// ���߶�
        /// </summary>
        protected string sDjHeight
        {
            get
            {
                if (ViewState["djHeight"] != null)
                {
                    return ViewState["djHeight"].ToString();
                }
                else
                {
                    return "300";
                }
            }
            set
            {
                ViewState["djHeight"] = value;
            }
        }
        /// <summary>
        /// ����Ψһ���
        /// </summary>
        protected string sID
        {
            get
            {
                if (ViewState["id"] != null)
                {
                    return ViewState["id"].ToString();
                }
                else
                {
                    return "0";
                }
            }
            set
            {
                ViewState["id"] = value;
            }
        }
        /// <summary>
        /// ������Ϣ����
        /// </summary>
        protected string sInfControl
        {
            get
            {
                if (ViewState["InfControl"] != null)
                {
                    return ViewState["InfControl"].ToString();
                }
                else
                {
                    return "false";
                }
            }
            set
            {
                ViewState["InfControl"] = value;
            }
        }
        #endregion 

        #region ҳ����� Page_Load
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //ʵ������
            myFlowForms = (FlowFormsUserDefine)this.Master;

            //����ֻ��
            myFlowForms.mySetContentReadOnly += new FlowFormsUserDefine.DoContentActions(Master_mySetContentReadOnly);
            //ȡ��ҳ��ؼ���ֵ�����XML����
            myFlowForms.myGetFormsValue += new FlowFormsUserDefine.GetFormsValue(Master_myGetFormsValue);
            //����ҳ��ֵ,�������̽ڵ�����ҳ��ؼ�ֻ����
            myFlowForms.mySetFormsValue += new FlowFormsUserDefine.DoContentActions(myFlowForms_mySetFormsValue);
            //�����ύǰִ��
            myFlowForms.myPreClickCustomize += new FlowFormsUserDefine.DoContentSubmitValid(myFlowForms_myPreClickCustomize);
            //�����ݴ�ǰִ��
            myFlowForms.myPreSaveClickCustomize += new FlowFormsUserDefine.DoContentValid(Master_myPreSaveClickCustomize);
            if (Page.IsPostBack == false)
            {
                hidUser.Value = Session["UserName"].ToString();
            }
        }
        #endregion 

        #region ����ֻ�� Master_mySetContentReadOnly
        /// <summary>
        /// ����ֻ��
        /// </summary>
        void Master_mySetContentReadOnly()
        {
            sInfControl = "true";
        }
        #endregion

        #region ȡ��ҳ��ؼ�ֵ�����XML���� Master_myGetFormsValue
        /// <summary>
        /// ȡ��ҳ��ؼ�ֵ�����XML����
        /// </summary>
        /// <param name="lngActionID"></param>
        /// <param name="strActionName"></param>
        /// <returns></returns>
        private XmlDocument Master_myGetFormsValue(long lngActionID, string strActionName)
        {
            ///ע�⣺��ǰӦ��û�и��������и��������������������Ϣ��ֵ��XML�浽 Tables�ڵ���
            FieldValues fv = new FieldValues();
            fv.Add("ID", sID);
            XmlDocument xmlDoc = fv.GetXmlObject();
            return xmlDoc;
        }
        #endregion

        #region ����ҳ��ֵ,�������̽ڵ�����ҳ��ؼ�ֻ���� Master_mySetFormsValue
        /// <summary>
        /// ����ҳ��ֵ,�������̽ڵ�����ҳ��ؼ�ֻ����
        /// </summary>
        private void myFlowForms_mySetFormsValue()
        {
            objFlow oFlow = myFlowForms.oFlow;                //����������
            myFlowForms.FormTitle = oFlow.FlowName.Trim();
            #region ����ҳ��ֵ
            DataTable dt = FC_BILLZLDP.GetUserDefineTable(oFlow.FlowModelID);   //ȡ���Զ����ֵ
            foreach (DataRow dr in dt.Rows)
            {
                sDjlx = dr["djlx"].ToString();   //������
                sDjsn = dr["djsn"].ToString();   //��sn

                string sdjposition = dr["djposition"].ToString();
                string[] sarr = sdjposition.Split(',');
                sDjHeight = sarr[3].ToString();
            }

            if (oFlow.MessageID != 0)  //�����ݣ�ȡ���ID
            {
                sID = FC_BILLZLDP.GetFlowUserDifineID(oFlow.FlowID);
            }
            else   //���������ɱ��
            {
                sID = EpowerGlobal.EPGlobal.GetNextID("FlowUserDefineID").ToString(); ;
            }
            #endregion

            #region ������������ҳ��ؼ��Ƿ�ֻ��
            setFieldCollection setFields = oFlow.setFields;
            foreach (setField sf in setFields)
            {
                if (sf.Editable == true && sf.Visibled == true)
                    continue;
                switch (sf.Name.ToLower())
                {
                    case "infcontrol":
                        sInfControl = "true";
                        break;
                    default:
                        break;
                }
            }
            #endregion
        }
        #endregion

        #region ��������֮ǰִ�д���
        /// <summary>
        /// ��������֮ǰִ�д���
        /// </summary>
        /// <returns></returns>
        bool myFlowForms_myPreClickCustomize(long lngActionID, string strActionName)
        {
            return true;
        }

        /// <summary>
        /// �ݴ�����֮ǰִ�д���
        /// </summary>
        /// <returns></returns>
        bool Master_myPreSaveClickCustomize()
        {
            return true;
        }
        #endregion
    }
}
