using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using appDataProcess;
using Epower.ITSM.SqlDAL.Print;
using EpowerCom;
using Epower.ITSM.Base;

namespace Epower.ITSM.Web.Print
{
    public partial class printRule : BasePage
    {

        public long AppID = 0;
        public long FlowModelId = 0;
        public long FlowID = 0;
        public string sApplicationUrl = Constant.ApplicationPath;     //虚拟路径
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Master.TableVisible = false;

            if (!IsPostBack)
            {
                //流程实例id
                if (Request["FlowId"] != null)
                {
                    FlowID = long.Parse(Request["FlowId"].ToString());
                }
                //应用id
                if (Request["AppID"] != null)
                {
                    AppID = long.Parse(Request["AppID"].ToString());
                }
                //流程模型id
                if (Request["FlowMoldelID"] != null)
                {
                    FlowModelId = long.Parse(Request["FlowMoldelID"].ToString());
                }
                ImplDataProcess dp = new ImplDataProcess(AppID);
                DataTable dt = dp.GetFieldsDataTable(FlowID, 0);
                if (dt.Rows.Count > 0)
                {
                    string[] arrhtml = PRINTRULE.getPrintContent(FlowModelId, dt);
                    if (arrhtml[0] != "")
                    {
                        if (arrhtml[1] == "1")
                        {
                            CtrlProcess1.FlowID = FlowID;
                            CtrlProcess1.FlowModelID = FlowModelId;
                            hidProcess.Value = "1";
                            divProcess.Visible = true;
                        }
                        else
                        {
                            divProcess.Visible = false;
                        }
                        DivPrint.InnerHtml = arrhtml[0];
                    }
                    else
                    {
                        DivPrint.InnerHtml = "<br/>&nbsp;<br/>&nbsp;<br/>&nbsp;<br/>&nbsp;<br/>&nbsp;<br/><font color='red'>找不到打印内容，请先配置打印模板，再来打印</font>";
                        divBtnPrint.Visible = false;
                        divProcess.Visible = false;
                    }
                }
                else
                {
                    DivPrint.InnerHtml = "&nbsp;<br/>&nbsp;<br/>&nbsp;<br/>&nbsp;<br/>&nbsp;<br/><font color='red'>单据未保存，不能打印</font>";
                    divBtnPrint.Visible = false;
                    divProcess.Visible = false;
                }
            }
        }
    }
}
