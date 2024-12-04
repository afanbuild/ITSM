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
	/// frmEditRightBatch 的摘要说明。
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

            //当从操作项进来时，则不显示操作项的选择
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
        /// 获得opearid 
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

        #region 加载数据[LoadObjectType_RightRange]
        /// <summary>
		/// 加载操作类别下拉列表
		/// </summary>
		private void LoadObjectType_RightRange()
		{
			ListItem lt;
			#region 对象类别
			//ListItem lt=new ListItem("","0");
			//dpdObjectType.Items.Add(lt);

			lt=new ListItem("部门",((int)eO_RightObject.eDeptRight).ToString());
			dpdObjectType.Items.Add(lt);
				
			lt=new ListItem("人员",((int)eO_RightObject.eUserRight).ToString());
			dpdObjectType.Items.Add(lt);

			lt=new ListItem("用户组",((int)eO_RightObject.eActorRight).ToString());
			dpdObjectType.Items.Add(lt);
			#endregion

			#region 权限范围
			//lt=new ListItem("","0");
			//dpdRightRange.Items.Add(lt);
			
			
			lt=new ListItem("个人",((int)eO_RightRange.ePersonal).ToString());
			dpdRightRange.Items.Add(lt);

			lt=new ListItem("所在部门",((int)eO_RightRange.eDeptDirect).ToString());
			dpdRightRange.Items.Add(lt);

			lt=new ListItem("所属部门",((int)eO_RightRange.eDept).ToString());
			dpdRightRange.Items.Add(lt);

			lt=new ListItem("所在机构",((int)eO_RightRange.eOrgDirect).ToString());
			dpdRightRange.Items.Add(lt);

			lt=new ListItem("所属机构",((int)eO_RightRange.eOrg).ToString());
			dpdRightRange.Items.Add(lt);

			lt=new ListItem("全局",((int)eO_RightRange.eFull).ToString());
			dpdRightRange.Items.Add(lt);


			#endregion

            #region 权限类别
            ddltRightType.Items.Clear();
            DataTable dt = RightDP.GetRightType();
            ddltRightType.DataTextField = "OpCatalog";
            ddltRightType.DataValueField = "OpCatalog";
            ddltRightType.DataSource = dt;
            ddltRightType.DataBind();

            ddltRightType.Items.Insert(0, new ListItem("--权限类别--","0"));
            #endregion 
        }
	
		#endregion


		#region Web 窗体设计器生成的代码
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: 该调用是 ASP.NET Web 窗体设计器所必需的。
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// 设计器支持所需的方法 - 不要使用代码编辑器修改
		/// 此方法的内容。
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
                    PageTool.MsgBox(this, "请选择操作项!");
                    return;
                }
            }
			if(uRight.RightValue == 0)
			{
				PageTool.MsgBox(this,"请选择权限值!");
				return;
			}

            string sObjectID = string.Empty;
            if (dpdObjectType.SelectedValue == "10")   //部门
            {
                if (CtrDeptMult.DeptID.Trim() == string.Empty)
                {
                    PageTool.MsgBox(this, "请选择授权对象!");
                    return;
                }
                sObjectID = CtrDeptMult.DeptID.Trim();
            }
            else if (dpdObjectType.SelectedValue == "20")   //用户
            {
                if (CtrUserMult.UserID.Trim() == string.Empty)
                {
                    PageTool.MsgBox(this, "请选择授权对象!");
                    return;
                }
                sObjectID = CtrUserMult.UserID.Trim();
            }
            else if (dpdObjectType.SelectedValue == "30")   //用户组
            {
                if (StringTool.String2Long(txtObjectId.Text.ToString()) == 0)
                {
                    PageTool.MsgBox(this, "请选择授权对象!");
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
					//如果更新本人的权限,同时更新HEADER
					Session["UserAllRights"] = RightDP.getUserRightTable(lngUserID);
				
					//PageTool.AddJavaScript(this,"window.opener.parent.topFrame.location.reload(); ");
				}

                PageTool.AddJavaScript(this, "window.opener.location.href=window.opener.location.href;window.close();");
				
			}
			catch(Exception ee)
			{
				PageTool.MsgBox(this,"保存权限时出现错误,错误为:\n"+ee.Message.ToString());
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
