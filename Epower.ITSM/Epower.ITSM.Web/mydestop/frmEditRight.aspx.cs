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
using Epower.ITSM.SqlDAL;

namespace Epower.ITSM.Web.mydestop
{
	/// <summary>
	/// frmEditRight 的摘要说明。
	/// </summary>
	public partial class frmEditRight : BasePage
	{
        /// <summary>
        /// 
        /// </summary>
        public string RightID
        {
            get { return ViewState["RightID"].ToString(); }
            set { ViewState["RightID"] = value; }
        }

        public string Opener_ClientId
        {
            get {
                if (Request["Opener_ClientId"] != null)
                {
                    return Request["Opener_ClientId"].ToString();
                }
                else
                {
                    return "";
                }
            }
        }

        #region OperateType
        /// <summary>
        /// 业务对象类别
        /// </summary>
        public int OperateType
        {
            get { return int.Parse(ViewState["OperateType"].ToString()); }
            set { ViewState["OperateType"] = value; }
        }
        #endregion 
        #region OperateID
        /// <summary>
        /// 业务对象ID
        /// </summary>
        public int OperateID
        {
            get { return int.Parse(ViewState["OperateID"].ToString()); }
            set { ViewState["OperateID"] = value; }
        }
        #endregion 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if(!IsPostBack)
			{
				if(this.Request.QueryString["rightid"]!=null)
				{
                    RightID = this.Request.QueryString["rightid"].ToString();
				}
                if (this.Request.QueryString["OperateID"] != null)
                {
                    OperateID = int.Parse(this.Request.QueryString["OperateID"].ToString());
                }
                if (this.Request.QueryString["OperateType"] != null)
                {
                    OperateType = int.Parse(this.Request.QueryString["OperateType"].ToString());
                }
                
                LoadObjectType_RightRange();
                LoadData(StringTool.String2Long(RightID));
			}
		}

		#region 加载数据[LoadObjectType_RightRange,LoadData]
		/// <summary>
		/// 加载操作类别下拉列表
		/// </summary>
		private void LoadObjectType_RightRange()
		{
			ListItem lt;
			#region 对象类别
			lt=new ListItem("部门",((int)eO_RightObject.eDeptRight).ToString());
			dpdObjectType.Items.Add(lt);
				
			lt=new ListItem("人员",((int)eO_RightObject.eUserRight).ToString());
			dpdObjectType.Items.Add(lt);

			lt=new ListItem("用户组",((int)eO_RightObject.eActorRight).ToString());
			dpdObjectType.Items.Add(lt);
			#endregion
		}
		/// <summary>
		/// 加载一个操作的详细资料
		/// </summary>
		/// <param name="lngId"></param>
		private void LoadData(long lngId)
		{
            EA_ExtendRightsDP ee = new EA_ExtendRightsDP();
			if(lngId!=0)
			{
                ee = ee.GetReCorded(lngId);
			}
			labID.Text=ee.RightID.ToString();
			txtObjectId.Text=ee.ObjectID.ToString();
			txtObjectName.Text=ee.ObjectName ;

			if((int)ee.ObjectType==0)
				dpdObjectType.SelectedIndex  =0;
			else
				dpdObjectType.SelectedValue=((int)ee.ObjectType).ToString();			

		}
		#endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		protected void cmdSave_Click(object sender, System.EventArgs e)
		{
            if (StringTool.String2Long(txtObjectId.Text.ToString()) == 0)
            {
                PageTool.MsgBox(this, "请选择授权对象!");
                return;
            }

            EA_ExtendRightsDP ee = new EA_ExtendRightsDP();
            ee.RightID = decimal.Parse(RightID);
            ee.ObjectID = StringTool.String2Long(txtObjectId.Text.ToString());
            ee.ObjectType = int.Parse(dpdObjectType.SelectedValue.ToString());
            ee.RightValue = 10;
            ee.OperateType = OperateType;
            ee.OperateID = OperateID;
			try
			{
                string OtherRightID = ee.Get_RightID(OperateType, ee.OperateID.ToString(), ee.ObjectID.ToString(), dpdObjectType.SelectedValue.ToString());
				if(!OtherRightID.Equals("0") && !OtherRightID.Equals(labID.Text.ToString()))
					PageTool.MsgBox(this,"对象已对该操作项设定权限,请从新输入!");
				else
				{
                    if (ee.RightID == 0)
                    {
                        ee.InsertRecorded(ee);
                    }
                    else
                    {
                        ee.UpdateRecorded(ee);
                    }
                    PageTool.AddJavaScript(this, "window.opener.document.getElementById('" + Opener_ClientId + "').click();window.close();");
				}
			}
			catch(Exception eep)
			{
                PageTool.MsgBox(this, "保存权限时出现错误,错误为:\n" + eep.Message.ToString());
			}
		}
	}
}
