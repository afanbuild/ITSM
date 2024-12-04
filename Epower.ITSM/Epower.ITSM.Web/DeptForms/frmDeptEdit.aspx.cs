/*
 *	by danqs
 */
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
using Epower.DevBase.BaseTools;
using Epower.DevBase.Organization;
using Epower.DevBase.Organization.Base;
using Epower.DevBase.Organization.SqlDAL;
using System.Text;

namespace Epower.ITSM.Web.Forms
{
    /// <summary>
    /// frmDeptEdit ��ժҪ˵����
    /// </summary>
    public partial class frmDeptEdit : BasePage
    {
        //********************  ע������  ********************
        //  2006-01-26 �� ɾ�� ��֯�ṹ�Բ�������֧��,Ŀǰ�Ĳ��������ԭ���Ĳ���רҵ
        //         ���׳������ʹ��� 
        // **************************************************

        int iSystemModel = 0;
        protected void Page_Load(object sender, System.EventArgs e)
        {
            CtrTitle.Title = "��������ά��";



            iSystemModel = int.Parse(System.Configuration.ConfigurationSettings.AppSettings["SystemModel"]);

            if (!IsPostBack)
            {
                LoadDeptKindType();
                if (this.Request.QueryString["DeptID"] != null)
                {
                    string sDeptId = this.Request.QueryString["DeptID"].ToString();
                    // ��¼��ǰ����
                    Session["OldDeptID"] = long.Parse(sDeptId);

                    hidDeptID.Value = sDeptId;
                    LoadData(StringTool.String2Long(sDeptId));


                }

            }
        }

        private void LoadDeptKindType()
        {
            dpdDeptKind.Items.Add(new ListItem("����", "0"));
            dpdDeptKind.Items.Add(new ListItem("����", "1"));

            //���ز���רҵ
            DataTable dt = DeptEntity.Get_Dept_Professional();
            dpdDeptProfessional.DataSource = dt.DefaultView;
            dpdDeptProfessional.DataValueField = "PID";
            dpdDeptProfessional.DataTextField = "PName";
            dpdDeptProfessional.DataBind();

        }

        private void LoadData(long lngDeptID)
        {
            DeptEntity de = new DeptEntity(lngDeptID);

            string strKind = "";
            string strType = "";
            string strPID = "";


            txtDeptName.Text = de.DeptName;
            txtDeptCode.Text = de.DeptCode;
            txtDesc.Text = de.Description;
            txtEndDate.Text = de.EndDate;
            txtStartDate.Text = de.StartDate;

            strKind = de.DeptKind.ToString().Trim();
            strType = de.DeptType.ToString().Trim();
            strPID = de.PID.ToString().Trim();
            //txtDOC_Center.Text =de.DOC_Center;

            //dpdIsTmp.SelectedValue=((int)de.IsTemp).ToString();
            chkbIsTempDept.Checked = (int)de.IsTemp == 0 ? false : true;

            hidLeaderID.Value = de.LeaderID.ToString();
            hidManagerID.Value = de.ManagerID.ToString();
            hidPDeptID.Value = de.ParentID.ToString();
            txtSortID.Text = de.SortID.ToString();
            lblDeptID.Text = de.DeptID.ToString();


            if (lngDeptID == 1)//������
            {
                txtPDeptName.Text = "��";

            }

            txtLeaderName.Text = UserControls.GetUserName(StringTool.String2Long(hidLeaderID.Value));
            txtManagerName.Text = UserControls.GetUserName(StringTool.String2Long(hidManagerID.Value));
            hidMangerName.Value = txtManagerName.Text.Trim();
            txtPDeptName.Text = DeptDP.GetDeptName(StringTool.String2Long(hidPDeptID.Value));




            if (iSystemModel == 0 || (long)Session["UserID"] == 1)
            {
                //����Ա�� �����÷�ʽ
                switch (strKind)
                {
                    case "0":
                        dpdDeptKind.SelectedIndex = 0;

                        break;

                    case "1":
                        dpdDeptKind.SelectedIndex = 1;

                        break;
                }
            }
            else
            {
                //���÷�ʽʱ,�������ȫ�ֹ���Ա,���� ����������Ŀ
                trDeptKind.Visible = false;
            }


            for (int i = 0; i < dpdDeptProfessional.Items.Count; i++)
            {
                if (dpdDeptProfessional.Items[i].Value == strPID)
                {
                    dpdDeptProfessional.SelectedIndex = i;
                }
            }



        }


        #region Web ������������ɵĴ���
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: �õ����� ASP.NET Web ���������������ġ�
            //
            InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        /// �����֧������ķ��� - ��Ҫʹ�ô���༭���޸�
        /// �˷��������ݡ�
        /// </summary>
        private void InitializeComponent()
        {

        }
        #endregion

        protected void cmdSave_Click(object sender, System.EventArgs e)
        {
            DeptEntity de = new DeptEntity(StringTool.String2Long(hidDeptID.Value));

            de.DeptName = txtDeptName.Text;
            de.DeptCode = txtDeptCode.Text.Trim();
            de.Description = txtDesc.Text;
            de.EndDate = txtEndDate.Text;
            de.StartDate = txtStartDate.Text;

            if (iSystemModel == 0 || (long)Session["UserID"] == 1)
            {
                de.DeptKind = int.Parse(dpdDeptKind.Items[dpdDeptKind.SelectedIndex].Value);
            }
            else
            {
                //���÷�ʽʱ,�������ȫ�ֹ���Ա,��������ȱʡΪ����,�������޸ĸ��ڵ��ֵ
                if ((long)Session["RootDeptID"] != de.DeptID)
                {
                    de.DeptKind = 0;
                }
            }

            de.DeptType = 0;
            de.PID = long.Parse(dpdDeptProfessional.Items[dpdDeptProfessional.SelectedIndex].Value);

            de.IsTemp = chkbIsTempDept.Checked ? eO_IsTrue.eTrue : eO_IsTrue.eFalse;

            //����༭ʱ�����ű��
            if ((StringTool.String2Long(hidDeptID.Value) != 0) && (de.ParentID != StringTool.String2Long(hidPDeptID.Value)))
            {
                de.HasChangeParent = true;
            }

            de.ManagerID = StringTool.String2Long(hidManagerID.Value);

            de.LeaderID = StringTool.String2Long(hidLeaderID.Value);
            de.ParentID = StringTool.String2Long(hidPDeptID.Value);
            de.DeptID = StringTool.String2Long(hidDeptID.Value);
            de.SortID = StringTool.String2Int(txtSortID.Text);
            de.OrgID = DeptDP.GetDirectOrg(StringTool.String2Long(hidPDeptID.Value));
            //de.DOC_Center =this.txtDOC_Center.Text.Trim();
            de.DOC_Center = "";
            de.UpdateID = StringTool.String2Long(Session["UserID"].ToString());
            de.CreatorID = StringTool.String2Long(Session["UserID"].ToString());
            if (de.DeptName.Trim() == "")
            {
                labMsg.Text = "�������Ʋ���Ϊ��!";
                return;
            }
            else if (de.ParentID == 0)
            {
                labMsg.Text = "��ѡ���ϼ�����!";
                return;
            }
            else
            {
                labMsg.Text = "";
            }

            try
            {


                long lngDeptId = de.Save();

                HttpRuntime.Cache.Insert("EpCacheValidDept", false);

                hidDeptID.Value = de.DeptID.ToString();

                if (hidDeptID.Value.Trim() == "1")
                {
                    Session["UserDeptName"] = txtDeptName.Text.Trim();
                }

                //����Ӳ�ѯ���Ž���򿪣������رղ�ˢ�¸�����
                if (Request["CloseAction"] != null && Request["CloseAction"].ToString().Equals("1"))
                {
                    PageTool.AddJavaScript(this, "window.parent.returnValue = 'refresh';self.close();");
                }
                else
                {
                    PageTool.AddJavaScript(this,
                        String.Format("window.parent.contents.location='frmDeptContent.aspx?deptid={0}'", lngDeptId));
                    PageTool.AddJavaScript(this, "window.parent.deptinfo.location='frmdeptedit.aspx?deptid=" + lngDeptId.ToString() + "&DeptText=" + txtDeptName.Text + "'");
                    Session["OldDeptID"] = lngDeptId;
                }
            }
            catch (Exception ee)
            {
                if (ee.Message.CompareTo("�����ٵ���") == 0)
                {
                    PageTool.MsgBox(this, hidMangerName.Value + ee.Message + "���ţ�" + txtDeptName.Text + " �Ĳ��Ź���Ա");
                }
                else if (ee.Message.CompareTo("���ܵ���") == 0)
                {
                    PageTool.MsgBox(this, ee.Message + "���ţ�" + hidMangerName.Value + " �Ĳ��Ź���Ա,��Ϊ����ʹ�¼����ŵĹ���Ա���������Ľ��й���");
                }
                else
                {

                    PageTool.MsgBox(this, ee.Message.ToString());
                }
            }

        }

        protected void cmdAdd_Click(object sender, System.EventArgs e)
        {
            string sDeptId = this.Request.QueryString["DeptID"].ToString();
            //string sDeptText=this.Request.QueryString["DeptText"].ToString();	



            txtDeptName.Text = "";
            txtDeptCode.Text = "";
            txtDesc.Text = "";
            txtEndDate.Text = "";
            txtStartDate.Text = "";
            //txtDOC_Center.Text ="";

            chkbIsTempDept.Checked = false;

            if (iSystemModel == 0 || (long)Session["UserID"] == 1)
            {
                dpdDeptKind.SelectedIndex = 0;
            }

            hidLeaderID.Value = "";
            hidManagerID.Value = "";
            hidPDeptID.Value = "";
            hidDeptID.Value = "";//��ղ��ű�ʶ


            //����Ĭ���ϼ����ţ�Ĭ�����ܣ�Ĭ���쵼
            DeptEntity de = new DeptEntity(StringTool.String2Long(sDeptId));
            hidLeaderID.Value = de.LeaderID.ToString();
            hidManagerID.Value = "1"; // de.ManagerID.ToString();
            hidPDeptID.Value = sDeptId == null ? "0" : sDeptId.Trim();
            txtLeaderName.Text = UserControls.GetUserName(StringTool.String2Long(hidLeaderID.Value));
            txtManagerName.Text = UserControls.GetUserName(StringTool.String2Long(hidManagerID.Value));
            txtPDeptName.Text = de.DeptName;
            txtSortID.Text = "-1";
            //txtDOC_Center.Text =de.DOC_Center;

        }

        protected void cmdDelete_Click(object sender, System.EventArgs e)
        {
            DeptEntity de = new DeptEntity();
            de.DeptID = StringTool.String2Long(hidDeptID.Value);
            de.UpdateID = StringTool.String2Long(Session["UserID"].ToString());
            try
            {
                de.Delete();

                HttpRuntime.Cache.Insert("EpCacheValidDept", false);

                //����Ӳ�ѯ���Ž���򿪣������رղ�ˢ�¸�����
                if (Request["CloseAction"] != null && Request["CloseAction"].ToString().Equals("1"))
                    PageTool.AddJavaScript(this, "window.parent.returnValue = 'refresh';self.close();");
                else
                {
                    long lngParentId = Convert.ToInt64(hidPDeptID.Value);

                    PageTool.AddJavaScript(this,
                        String.Format("window.parent.contents.location='frmDeptContent.aspx?deptid={0}';window.location='about:blank'", lngParentId));

                    Session["OldDeptID"] = lngParentId;
                }
            }
            catch (Exception ee)
            {
                PageTool.MsgBox(this, "ɾ������ʱ���ִ��󣬴���Ϊ��" + ee.Message.ToString());
            }
        }
    }
}
