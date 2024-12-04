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

using Epower.ITSM.SqlDAL;
using Epower.ITSM.Base;
using Epower.DevBase.Organization.SqlDAL;

namespace Epower.ITSM.Web.mydestop
{
    public partial class frmDefineDesk : BasePage
    {
        /// <summary>
        /// 
        /// </summary>
        protected bool IsLeftOrRight
        {
            get
            {
                string svalue = string.Empty;
                if (Request["POS"] != null)
                    svalue = Request["POS"].ToString();
                if (svalue == "MYTABLE_LEFT")
                    return true;
                else
                    return false;
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
                string sMainPageSet = UserDP.GetUserDeskDefineById((long)Session["UserID"]);

                LoadData(sMainPageSet);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void LoadData(string sMainPageSet)
        {
            select1.Items.Clear();
            select2.Items.Clear();

            //��ʼ����ѡ

            string sselect = string.Empty;
            DataTable dt = new DataTable();
            string swhere = string.Empty;    //��ѡ����
            string sunwhere = string.Empty;   //δѡ����
            //if (cookie != null && sMainPageSet != string.Empty)
            if (sMainPageSet != string.Empty)
            {
                string[] arr = sMainPageSet.Split(':');
                //���������ѡ
                if (arr.Length > 1 && IsLeftOrRight)
                {
                    sselect = arr[0];
                    swhere = " iOrder In (" + arr[0].ToString().Substring(0, arr[0].Length - 1) + ")";
                }
                //�����ұ�
                if (!IsLeftOrRight)
                {
                    if (sMainPageSet.IndexOf(':') == -1)
                    {
                        sselect = arr[0];
                        swhere = " iOrder In (" + arr[0].ToString().Substring(0, arr[0].Length - 1) + ")";
                    }
                    else if (arr[1] != string.Empty)
                    {
                        sselect = arr[1];
                        swhere = " iOrder In (" + arr[1].ToString().Substring(0, arr[1].Length - 1) + ")";
                    }
                }

                //����δѡ����
                if (arr[0].Length > 0)
                {
                    sunwhere = " (iOrder In (" + arr[0].ToString().Substring(0, arr[0].Length - 1) + ")";
                }
                else
                {
                    sunwhere = " (iOrder In (" + "-1" + ")";
                }
                if (arr.Length > 1 && arr[1].Length > 0)
                {
                    sunwhere += " or iOrder In (" + arr[1].ToString().Substring(0, arr[1].Length - 1) + "))";
                }
                else
                {
                    sunwhere += " or iOrder In (" + "-1" + "))";
                }      
            }
            Ea_DefineMainPageDP ee = new Ea_DefineMainPageDP();
            if (sselect != string.Empty)
            {
                dt = ee.GetDataTable(swhere);
                DataRow[] drarr;
                string[] arrselect = sselect.Split(',');
                for (int i = 0; i < arrselect.Length - 1; i++)
                {
                    drarr = dt.Select(" iOrder=" + arrselect[i]);
                    if (drarr != null && drarr.Length > 0)
                    {
                        select1.Items.Add(new ListItem(drarr[0]["Title"].ToString(), drarr[0]["iOrder"].ToString()));
                    }
                }
            }
            //else if (cookie == null || cookie.Value==string.Empty)  sMainPageSet
            else if (sMainPageSet == string.Empty)  
            {
                if (IsLeftOrRight)
                {
                    //�������
                    swhere = " LeftOrRight=0 And isnull(DefaultVisible,0)=0";
                    sunwhere = "  isnull(DefaultVisible,0)=0";
                }
                else
                {
                    //�����ұ�
                    swhere = " LeftOrRight=1 And isnull(DefaultVisible,0)=0";
                    sunwhere = "  isnull(DefaultVisible,0)=0";
                }
                dt = ee.GetDataTable(swhere);
                foreach (DataRow dr in dt.Rows)
                {
                    select1.Items.Add(new ListItem(dr["Title"].ToString(), dr["iOrder"].ToString()));
                }
            }

            //��ʼ��δѡ
            if (sunwhere != string.Empty)
            {
                dt = ee.GetDataTable(" not (" + sunwhere + ")");
            }
            else
            {
                dt = ee.GetDataTable(swhere);
            }
            foreach (DataRow dr in dt.Rows)
            {
                select2.Items.Add(new ListItem(dr["Title"].ToString(), dr["iOrder"].ToString()));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            string sMainPageSet = UserDP.GetUserDeskDefineById((long)Session["UserID"]); 
            string sleft = string.Empty;
            string sright = string.Empty;
            if (sMainPageSet != string.Empty)
            {
                //sMainPageSet = cookie.Value.ToString();
                string[] arr = sMainPageSet.Split(':');
                if (arr.Length > 1)
                {
                    sleft = arr[0] + ":";
                }
                if (sMainPageSet.IndexOf(':') == -1)
                {
                    sright = arr[0];
                }
                else if (arr[1] != string.Empty)
                {
                    sright = arr[1];
                }
            }
            else
            {
                Ea_DefineMainPageDP ee = new Ea_DefineMainPageDP();
                DataTable dt = new DataTable();
                string swhere = string.Empty;    //��ѡ����
                if (IsLeftOrRight)
                {
                        //ȡ�ұ�ȱʡֵ
                    swhere = " LeftOrRight=1 And isnull(DefaultVisible,0)=0";
                    dt = ee.GetDataTable(swhere);
                    foreach (DataRow dr in dt.Rows)
                    {
                        //select1.Items.Add(new ListItem(dr["Title"].ToString(), dr["iOrder"].ToString()));
                        sright += dr["iorder"].ToString() + ",";
                    }
                    
                }
                else
                {
                    //ȡ���ȱʡֵ
                    swhere = " LeftOrRight=0 And isnull(DefaultVisible,0)=0";
                    dt = ee.GetDataTable(swhere);
                    foreach (DataRow dr in dt.Rows)
                    {
                        //select1.Items.Add(new ListItem(dr["Title"].ToString(), dr["iOrder"].ToString()));
                        sleft += dr["iorder"].ToString() + ",";
                    }
                    if (sleft != string.Empty)
                        sleft += ":";
                }
                
                
            }

            if (IsLeftOrRight)
            {
                //�������
                sleft = this.hidValue.Value;
                if (sleft != string.Empty)
                    sleft += ":";
            }
            else
            {
                //�����ұ�
                sright = this.hidValue.Value; ;
            }
            WriteDeskDefineToProfile(sleft + sright);
            LoadData(sleft + sright);

            Epower.DevBase.BaseTools.PageTool.MsgBox(this, "���ݱ���ɹ���");
        }

        /// <summary>
        /// 
        /// </summary>
        private void WriteDeskDefineToProfile(string sParams)
        {
            UserDP.UpdateUserDeskDefine((long)Session["UserID"], sParams);
        }

        /// <summary>
        /// �ָ�����Ĭ������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnreset_Click(object sender, EventArgs e)
        {
            long lngUserID = (long)Session["UserID"];
            UserDP.DeleteUserDeskDefine(lngUserID);

            string sMainPageSet = UserDP.GetUserDeskDefineById(lngUserID);
            LoadData(sMainPageSet);
            
        }		
    }
}
