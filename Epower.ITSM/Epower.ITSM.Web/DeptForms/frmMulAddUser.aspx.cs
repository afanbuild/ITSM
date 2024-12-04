/*******************************************************************
 * ��Ȩ���У��Ƿ���Ϣ����
 * Description���û������������Ա��Ա
 * 
 * 
 * Create By  ��zhumc
 * Create Date��2010-12-22
 * *****************************************************************/
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

using Epower.DevBase.Organization.SqlDAL;
using Epower.DevBase.BaseTools;
using Epower.DevBase.Organization.Base;

namespace Epower.ITSM.Web.DeptForms
{
    /// <summary>
    /// 
    /// </summary>
    public partial class frmMulAddUser : BasePage
    {
        /// <summary>
        /// �û�����
        /// </summary>
        protected string sActorID
        {
            get
            {
                if (Request["actorid"] != null)
                {
                    return Request["actorid"].ToString();
                }
                else
                {
                    return "0";
                }
            }
        }

        /// <summary>
        /// �û����߲���
        /// </summary>
        protected string sType
        {
            get
            {
                if (Request["type"] != null)
                {
                    return Request["type"].ToString();
                }
                else
                {
                    return "0";
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (sType == "0") //��������û�
                {
                    truser.Visible = true;
                    trdept.Visible = false;
                }
                else
                {
                    truser.Visible = false;
                    trdept.Visible = true;
                }
            }
        }

        /// <summary>
        /// ���������û�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            int iactortype = 20;
            string[] sUserID;
            string[] sUserName;

            if (sType == "0") //��������û�
            {
                iactortype = 20;
                if (UserPickerMult1.UserID.Trim() == string.Empty)
                {
                    PageTool.MsgBox(this, "��ѡ����Ҫ��ӵ��û���");
                    return;
                }
                sUserID = UserPickerMult1.UserID.Split(',');
                sUserName = UserPickerMult1.UserName.Split(',');
            }
            else
            {
                iactortype = 10;
                if (DeptPickerMult1.DeptID.Trim() == string.Empty)
                {
                    PageTool.MsgBox(this, "��ѡ����Ҫ��ӵĲ��ţ�");
                    return;
                }
                sUserID = DeptPickerMult1.DeptID.Split(',');
                sUserName = DeptPickerMult1.DeptName.Split(',');
            }

            
            if (sActorID == "0")
            {
                PageTool.MsgBox(this, "δָ����Ӧ�û���,����ʧ�ܣ�");
                return;
            }

            string sCatchName = string.Empty;
            string sRepeatName = string.Empty;
            

            try
            {
                for (int i = 0; i < sUserID.Length-1; i++)
                {
                    ActorMemberEntity ame = new ActorMemberEntity();
                    ame.ActorID = StringTool.String2Long(sActorID);
                    ame.ID = 0;
                    ame.ObjectID = long.Parse(sUserID[i]);
                    ame.ActorType = (eO_ActorObject)iactortype;
                    // ע��ֻ������ʱ�ű�����rangeid
                    ame.RangeID = Session["RangeID"].ToString();

                    if (ame.ActorType == 0 || ame.ObjectID == 0)   //�����쳣
                    {
                        sCatchName += sUserName[i] + ",";
                    }
                    if (ActorMemberEntity.Is_ActorMemeber_Exist(ame.ActorID.ToString(), ((int)ame.ActorType).ToString(), ame.ObjectID.ToString()))
                    {
                        sRepeatName += sUserName[i] + ",";
                    }
                    else
                    {
                        ame.Save();
                    }
                }

                if (sCatchName == string.Empty && sRepeatName == string.Empty)
                {
                    PageTool.MsgBox(this, "������ӳɹ���");
                }
                else
                {
                    string sMsg = "������ӳɹ���";
                    if (sCatchName != string.Empty)
                    {
                        sMsg += "�쳣�����У�" + sCatchName + "��";
                    }
                    if (sRepeatName != string.Empty)
                    {
                        sMsg += "�Ѿ������û���ĳ�Ա�У�" + sRepeatName + "��";
                    }
                    PageTool.MsgBox(this, sMsg);
                }
                PageTool.AddJavaScript(this, "window.opener.location.href=window.opener.location.href;window.close();");
            }
            catch (Exception ex)
            {
                PageTool.MsgBox(this, "�����û����Աʱ���ִ���,����Ϊ:\n" + ex.Message.ToString());
            }
        }
    }
}
