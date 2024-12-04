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
using Epower.DevBase.Organization.Base;
using Epower.DevBase.Organization.SqlDAL;

namespace Epower.ITSM.Web.Forms
{
	/// <summary>
	/// frmEditRightBatch ��ժҪ˵����
	/// </summary>
	public partial class frmEditRightBatch : BasePage
	{

	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			CtrOperates1.SystemID = (long)Session["SystemID"];
			if(!IsPostBack)
			{
				LoadObjectType_RightRange();
			}

            //���Ӳ��������ʱ������ʾ�������ѡ��
            if (OpearId != "")
            {
                tr_RightType.Visible = false;
                tr_Operates.Visible = false;
            }

            if (hidObjectName.Value.Trim() != txtObjectName.Text.Trim())
            {
                txtObjectName.Text = hidObjectName.Value.Trim();
            }
        }

        #region  yanghw 2011-09-05
        /// <summary>
        /// ���opearid 
        /// </summary>
        public string OpearId
        {
            get {
                if (Request["OpearId"] != null)
                {
                    return Request["OpearId"].ToString();
                }
                else
                {
                    return "";
                }
            }

        }
        #endregion 

        #region ��������[LoadObjectType_RightRange]
        /// <summary>
		/// ���ز�����������б�
		/// </summary>
		private void LoadObjectType_RightRange()
		{
			ListItem lt;
			#region �������
			//ListItem lt=new ListItem("","0");
			//dpdObjectType.Items.Add(lt);

			lt=new ListItem("����",((int)eO_RightObject.eDeptRight).ToString());
			dpdObjectType.Items.Add(lt);
				
			lt=new ListItem("��Ա",((int)eO_RightObject.eUserRight).ToString());
			dpdObjectType.Items.Add(lt);

			lt=new ListItem("�û���",((int)eO_RightObject.eActorRight).ToString());
			dpdObjectType.Items.Add(lt);
			#endregion

			#region Ȩ�޷�Χ
			//lt=new ListItem("","0");
			//dpdRightRange.Items.Add(lt);
			
			
			lt=new ListItem("����",((int)eO_RightRange.ePersonal).ToString());
			dpdRightRange.Items.Add(lt);

			lt=new ListItem("���ڲ���",((int)eO_RightRange.eDeptDirect).ToString());
			dpdRightRange.Items.Add(lt);

			lt=new ListItem("��������",((int)eO_RightRange.eDept).ToString());
			dpdRightRange.Items.Add(lt);

			lt=new ListItem("���ڻ���",((int)eO_RightRange.eOrgDirect).ToString());
			dpdRightRange.Items.Add(lt);

			lt=new ListItem("��������",((int)eO_RightRange.eOrg).ToString());
			dpdRightRange.Items.Add(lt);

			lt=new ListItem("ȫ��",((int)eO_RightRange.eFull).ToString());
			dpdRightRange.Items.Add(lt);


			#endregion

            #region Ȩ�����
            ddltRightType.Items.Clear();
            DataTable dt = RightDP.GetRightType();
            ddltRightType.DataTextField = "OpCatalog";
            ddltRightType.DataValueField = "OpCatalog";
            ddltRightType.DataSource = dt;
            ddltRightType.DataBind();

            ddltRightType.Items.Insert(0, new ListItem("--Ȩ�����--","0"));
            #endregion 
        }
	
		#endregion


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
            string strRangeID = Session["RangeID"].ToString();

			string strOpIDs = CtrOperates1.GetSelectOpIDs();
            if (OpearId == "")
            {
                if (strOpIDs.Length == 0)
                {
                    PageTool.MsgBox(this, "��ѡ�������!");
                    return;
                }
            }
			if(uRight.RightValue == 0)
			{
				PageTool.MsgBox(this,"��ѡ��Ȩ��ֵ!");
				return;
			}

            string sObjectID = string.Empty;
            if (dpdObjectType.SelectedValue == "10")   //����
            {
                if (CtrDeptMult.DeptID.Trim() == string.Empty)
                {
                    PageTool.MsgBox(this, "��ѡ����Ȩ����!");
                    return;
                }
                sObjectID = CtrDeptMult.DeptID.Trim();
            }
            else if (dpdObjectType.SelectedValue == "20")   //�û�
            {
                if (CtrUserMult.UserID.Trim() == string.Empty)
                {
                    PageTool.MsgBox(this, "��ѡ����Ȩ����!");
                    return;
                }
                sObjectID = CtrUserMult.UserID.Trim();
            }
            else if (dpdObjectType.SelectedValue == "30")   //�û���
            {
                if (StringTool.String2Long(txtObjectId.Text.ToString()) == 0)
                {
                    PageTool.MsgBox(this, "��ѡ����Ȩ����!");
                    return;
                }
                sObjectID = txtObjectId.Text.Trim() + ",";
            }
               
			try
			{
                if (OpearId == "")
                {
                    RightDP.SetRightsBatch(sObjectID, (eO_RightObject)(int.Parse(dpdObjectType.SelectedValue.ToString())),
                    strOpIDs, uRight.RightValue, (eO_RightRange)(int.Parse(dpdRightRange.SelectedValue.ToString())), hidDeptList.Value, strRangeID);
                }
                else
                {
                    RightDP.SetRightsBatchOpear(sObjectID, (eO_RightObject)(int.Parse(dpdObjectType.SelectedValue.ToString())),
                    OpearId, uRight.RightValue, (eO_RightRange)(int.Parse(dpdRightRange.SelectedValue.ToString())), hidDeptList.Value, strRangeID);
                }
                
			
				long lngUserID = (long)Session["UserID"];
				if(int.Parse(dpdObjectType.SelectedValue.ToString()) == (int)eO_RightObject.eUserRight 
					&& StringTool.String2Long(txtObjectId.Text.ToString()) == lngUserID)
				{
					//������±��˵�Ȩ��,ͬʱ����HEADER
					Session["UserAllRights"] = RightDP.getUserRightTable(lngUserID);
				
					//PageTool.AddJavaScript(this,"window.opener.parent.topFrame.location.reload(); ");
				}

                PageTool.AddJavaScript(this, "window.opener.location.href=window.opener.location.href;window.close();");
				
			}
			catch(Exception ee)
			{
				PageTool.MsgBox(this,"����Ȩ��ʱ���ִ���,����Ϊ:\n"+ee.Message.ToString());
			}
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddltRightType_SelectedIndexChanged(object sender, EventArgs e)
        {
            CtrOperates1.OpCatalog = ddltRightType.SelectedValue;
            CtrOperates1.LoadData();
        }
	}
}
