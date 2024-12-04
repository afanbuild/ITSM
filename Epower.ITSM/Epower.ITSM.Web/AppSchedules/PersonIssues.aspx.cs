using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Epower.ITSM.Web.AppSchedules
{
    public partial class PersonIssues : System.Web.UI.Page
    {
        public long EngineerId
        {
            set
            {
                ViewState["EngineerId"] = value;
            }
            get
            {
                if (ViewState["EngineerId"] == null)
                {
                    throw new ArgumentNullException("EngineerId is null.");
                }
                return long.Parse(ViewState["EngineerId"].ToString());
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                EngineerId = long.Parse(Request["EngineerId"].ToString());
                this.PersonIssues1.EngineerId = EngineerId;
            }

        }
    }
}
