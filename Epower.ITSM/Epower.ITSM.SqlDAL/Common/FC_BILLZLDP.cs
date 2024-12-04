



/*******************************************************************
 *
 * Description
 * 
 * 
 * Create By  :zhumc
 * Create Date:2011年8月16日
 * *****************************************************************/
using System;
using System.Data;
using System.Data.OracleClient;
using Epower.DevBase.BaseTools;
using Epower.ITSM.Base;
using Epower.DevBase.Organization.SqlDAL;
using Epower.DevBase.Organization.Base;

namespace Epower.ITSM.SqlDAL
{
    /// <summary>
    /// 
    /// </summary>
    public class FC_BILLZLDP
    {
        /// <summary>
        /// 
        /// </summary>
        public FC_BILLZLDP()
        { }

        #region Property
        #region djid
        private Int32 mdjid;
        /// <summary>
        ///
        /// </summary>
        public Int32 djid
        {
            get { return mdjid; }
            set { mdjid = value; }
        }
        #endregion

        #region dj_name
        private string mdj_name;
        /// <summary>
        ///
        /// </summary>
        public string dj_name
        {
            get { return mdj_name; }
            set { mdj_name = value; }
        }
        #endregion

        #region djlx
        private string mdjlx;
        /// <summary>
        ///
        /// </summary>
        public string djlx
        {
            get { return mdjlx; }
            set { mdjlx = value; }
        }
        #endregion

        #region xmltext
        private String mxmltext = string.Empty;
        /// <summary>
        ///
        /// </summary>
        public String xmltext
        {
            get { return mxmltext; }
            set { mxmltext = value; }
        }
        #endregion

        #region DjPosition
        private string mDjPosition;
        /// <summary>
        ///
        /// </summary>
        public string DjPosition
        {
            get { return mDjPosition; }
            set { mDjPosition = value; }
        }
        #endregion

        #region djsn
        private string mdjsn;
        /// <summary>
        ///
        /// </summary>
        public string djsn
        {
            get { return mdjsn; }
            set { mdjsn = value; }
        }
        #endregion

        #region designtext
        private String mdesigntext = string.Empty;
        /// <summary>
        ///
        /// </summary>
        public String designtext
        {
            get { return mdesigntext; }
            set { mdesigntext = value; }
        }
        #endregion

        #region stmptable
        private string mstmptable;
        /// <summary>
        ///
        /// </summary>
        public string stmptable
        {
            get { return mstmptable; }
            set { mstmptable = value; }
        }
        #endregion

        #region userType
        private string muserType;
        /// <summary>
        ///
        /// </summary>
        public string userType
        {
            get { return muserType; }
            set { muserType = value; }
        }
        #endregion

        #region oFlowModelID
        private Decimal moFlowModelID;
        /// <summary>
        ///
        /// </summary>
        public Decimal oFlowModelID
        {
            get { return moFlowModelID; }
            set { moFlowModelID = value; }
        }
        #endregion

        #region FlowName
        private string mFlowName;
        /// <summary>
        ///
        /// </summary>
        public string FlowName
        {
            get { return mFlowName; }
            set { mFlowName = value; }
        }
        #endregion

        #endregion

        #region GetReCorded
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lngID"></param>
        /// <returns>FC_BILLZLDP</returns>
        public FC_BILLZLDP GetReCorded(long lngID)
        {
            FC_BILLZLDP ee = new FC_BILLZLDP();
            string strSQL = string.Empty;
            strSQL = "SELECT * FROM FC_BILLZL WHERE ID = " + lngID.ToString();
            DataTable dt = CommonDP.ExcuteSqlTable(strSQL);
            foreach (DataRow dr in dt.Rows)
            {
                ee.djid = Int32.Parse(dr["djid"].ToString());
                ee.dj_name = dr["dj_name"].ToString();
                ee.djlx = dr["djlx"].ToString();
                ee.xmltext = dr["xmltext"].ToString();
                ee.DjPosition = dr["DjPosition"].ToString();
                ee.djsn = dr["djsn"].ToString();
                ee.designtext = dr["designtext"].ToString();
                ee.stmptable = dr["stmptable"].ToString();
                ee.userType = dr["userType"].ToString();
                ee.oFlowModelID = Decimal.Parse(dr["oFlowModelID"].ToString());
                ee.FlowName = dr["FlowName"].ToString();
            }
            return ee;
        }
        #endregion

        #region GetDataTable
        /// <summary>
        /// GetDataTable
        /// </summary>
        /// <param name="sWhere"></param>
        /// <param name="sOrder"></param>
        /// <param name="pagesize"></param>
        /// <param name="pageindex"></param>
        /// <param name="rowcount"></param>
        /// <returns>DataTable</returns>
        public DataTable GetDataTable(string sWhere, string sOrder, int pagesize, int pageindex, ref int rowcount)
        {
            DataTable dt = CommonDP.ExcuteSqlTablePage("FC_BILLZL", "*", sWhere, sOrder, pagesize, pageindex, ref rowcount);
            return dt;
        }
        #endregion

        #region InsertRecorded
        /// <summary>
        /// 
        /// </summary>
        /// <param name=pFC_BILLZLDP></param>
        public void InsertRecorded(FC_BILLZLDP pFC_BILLZLDP)
        {
            string strSQL = string.Empty;
            string strID = "0";
            try
            {
                strID = EpowerGlobal.EPGlobal.GetNextID("FC_BILLZLID").ToString();
                pFC_BILLZLDP.djid = int.Parse(strID);
                strSQL = @"INSERT INTO FC_BILLZL(
									djid,
									dj_name,
									djlx,
									xmltext,
									DjPosition,
									djsn,
									designtext,
									stmptable,
									userType,
									oFlowModelID,
									FlowName
					)
					VALUES( " +
                            strID.ToString() + "," +
                            StringTool.SqlQ(pFC_BILLZLDP.dj_name) + "," +
                            StringTool.SqlQ(pFC_BILLZLDP.djlx) + "," +
                            StringTool.SqlQ(pFC_BILLZLDP.xmltext) + "," +
                            StringTool.SqlQ(pFC_BILLZLDP.DjPosition) + "," +
                            StringTool.SqlQ(pFC_BILLZLDP.djsn) + "," +
                            StringTool.SqlQ(pFC_BILLZLDP.designtext) + "," +
                            StringTool.SqlQ(pFC_BILLZLDP.stmptable) + "," +
                            StringTool.SqlQ(pFC_BILLZLDP.userType) + "," +
                            pFC_BILLZLDP.oFlowModelID.ToString() + "," +
                            StringTool.SqlQ(pFC_BILLZLDP.FlowName) +
                    ")";

                CommonDP.ExcuteSql(strSQL);
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region UpdateRecorded
        /// <summary>
        /// 
        /// </summary>
        /// <param name=pFC_BILLZLDP></param>
        public void UpdateRecorded(FC_BILLZLDP pFC_BILLZLDP)
        {
            string strSQL = string.Empty;
            try
            {
                strSQL = @"UPDATE FC_BILLZL Set " +
                                                        " dj_name = " + StringTool.SqlQ(pFC_BILLZLDP.dj_name) + "," +
                            " djlx = " + StringTool.SqlQ(pFC_BILLZLDP.djlx) + "," +
                            " xmltext = " + StringTool.SqlQ(pFC_BILLZLDP.xmltext) + "," +
                            " DjPosition = " + StringTool.SqlQ(pFC_BILLZLDP.DjPosition) + "," +
                            " djsn = " + StringTool.SqlQ(pFC_BILLZLDP.djsn) + "," +
                            " designtext = " + StringTool.SqlQ(pFC_BILLZLDP.designtext) + "," +
                            " stmptable = " + StringTool.SqlQ(pFC_BILLZLDP.stmptable) + "," +
                            " userType = " + StringTool.SqlQ(pFC_BILLZLDP.userType) + "," +
                            " oFlowModelID = " + pFC_BILLZLDP.oFlowModelID.ToString() + "," +
                            " FlowName = " + StringTool.SqlQ(pFC_BILLZLDP.FlowName) +
                                " WHERE djid = " + pFC_BILLZLDP.djid.ToString();

                CommonDP.ExcuteSql(strSQL);
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region DeleteRecorded
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lngID"></param>
        public void DeleteRecorded(long lngID)
        {
            try
            {
                string strSQL = "Delete FC_BILLZL WHERE djid =" + lngID.ToString();
                CommonDP.ExcuteSql(strSQL);
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region 同步流程模型表
        /// <summary>
        /// 
        /// </summary>
        public void SyncFlowData()
        {
            try
            {
//                string strxmltext = @"<script></script><script src='../../fceform/js/fcopendj.js'></script><script defer src='../../fceform/js/fcsavedj.js'></script>
//                                        <script src='../../fceform/js/fcselfuse.js'></script><script src='../../fceform/js/fcbasecont.js'></script><script defer src='../../fceform/js/fcother.js'></script>
//                                        <script defer src='../../fceform/js/selectdate.js'></script><script src='../../fceform/../fceformext/js/userfunc.js'></script>
//                                        <link href=../../fceform/css/tabstyle.css type=text/css rel=stylesheet><link type='text/css' rel='stylesheet' href='../../fceform/css/Button.css'/>
//                                        <DIV id=SKbillsheet jslib=""fcopendj.js&#13;&#10;fcsavedj.js&#13;&#10;fcselfuse.js&#13;&#10;fcbasecont.js&#13;&#10;fcother.js&#13;&#10;selectdate.js&#13;&#10;~userfunc.js""
//                                        contxml=""<root><button><id>btnsubmit</id><id>btndelete</id></button></root>"" controlno=""SKButton:0;SKDBedit:0;checkbox:0;label:0;radio:0;listbox:0;textarea:0;combobox:0;password:0;upload:0;SKDBtext:0;chart:0;dbimg:0;img:0;SKBILLgrid:0;shape:0;tab:0;div:0;DsMain_field:0;a:0;button:3;text:0;hr:0;checkboxlist:0;radiolist:0;dropdownlist:0;grid:0;dataset:0;spin:0;excel:0;tree:0;ebshow:0;ebiao:0;layout:0;page:0""
//                                        billtaborder=""<root><taborder>btnsubmit</taborder><taborder>btndelete</taborder></root>"" isCheckPermit=""否"" alertType=""1"" 
//                                        userType BLONclose BLONopenBefore BLONopen window=""当前窗口"" toolbar=""不带工具栏"" posheight poswidth postop posleft center=""  "" isfile=""否""
//                                        type=""TT"" caption=""#caption#"" dj_sn=""#djsn#"" AutoResizeXml><BUTTON id=btnsubmit 
//                                        style=""DISPLAY: none; LEFT: 328px; WIDTH: 233px; POSITION: absolute; TOP: 1px; HEIGHT: 25px"" onmovestart=moveStart() 
//                                        onclick=""bill_onclick(&quot;$eform('提交数据')&quot;)"" controltype=""button"">流程提交按钮,请不要删除,修改</BUTTON>
//                                        <BUTTON id=btndelete style=""DISPLAY: none;LEFT: 560px; WIDTH: 229px; POSITION: absolute; TOP: 1px; HEIGHT: 25px"" onmovestart=moveStart() 
//                                        onclick=""bill_onclick(&quot;$eform('删除')&quot;)"" controltype=""button"">流程删除按钮,请不要删除,修改</BUTTON></DIV>";
//                string strDjPosition = "156,248,797,467,  ,不带工具栏,undefined,当前窗口,#caption#";
//                string strdesigntext = @"<script></script><DIV oncontrolselect=controlselect() id=SKbillsheet contentEditable=true jslib=""fcopendj.js&#13;&#10;fcsavedj.js&#13;&#10;fcselfuse.js&#13;&#10;fcbasecont.js&#13;&#10;fcother.js&#13;&#10;selectdate.js&#13;&#10;~userfunc.js""
//                                        unselectable=""on"" contxml=""<root><button><id>btnsubmit</id><id>btndelete</id></button></root>"" controlno=""SKButton:0;SKDBedit:0;checkbox:0;label:0;radio:0;listbox:0;textarea:0;combobox:0;password:0;upload:0;SKDBtext:0;chart:0;dbimg:0;img:0;SKBILLgrid:0;shape:0;tab:0;div:0;DsMain_field:0;a:0;button:3;text:0;hr:0;checkboxlist:0;radiolist:0;dropdownlist:0;grid:0;dataset:0;spin:0;excel:0;tree:0;ebshow:0;ebiao:0;layout:0;page:0""
//                                        billtaborder=""<root><taborder>btnsubmit</taborder><taborder>btndelete</taborder></root>"" isCheckPermit=""否"" alertType=""1"" 
//                                        userType BLONclose BLONopenBefore BLONopen window=""当前窗口"" toolbar=""不带工具栏"" posheight poswidth postop posleft center=""  "" isfile=""否""
//                                        type=""TT"" caption=""#caption#"" dj_sn=""#djsn#""><BUTTON onmove=move() oncontrolselect=controlselect() id=btnsubmit onresizeend=resizeEnd() onresizestart=resizeStart() onmoveend=moveEnd() onresize=resize() 
//                                        style=""DISPLAY: none; LEFT: 328px; WIDTH: 233px; POSITION: absolute; TOP: 1px; HEIGHT: 25px"" onmovestart=moveStart() fc_onclick=""bill_onclick(&quot;$eform('提交数据')&quot;)""
//                                        controltype=""button"">流程提交按钮,请不要删除,修改</BUTTON><BUTTON onmove=move() oncontrolselect=controlselect() id=btndelete onresizeend=resizeEnd() onresizestart=resizeStart() onmoveend=moveEnd() onresize=resize() 
//                                        style=""DISPLAY: none;LEFT: 560px; WIDTH: 229px; POSITION: absolute; TOP: 1px; HEIGHT: 25px"" onmovestart=moveStart() fc_onclick=""bill_onclick(&quot;$eform('删除')&quot;)""
//                                        controltype=""button"">流程删除按钮,请不要删除,修改</BUTTON></DIV>";
                string strxmltext = @"<script>function InitPage()
                                        {
                                        $id(""hidFlowID"").value = parent.Request.QueryString('ID').toString();
                                        hidFlowID.fireEvent(""onchange"");
                                        var scontrol = parent.Request.QueryString('control').toString();
                                        if(scontrol==""true"")
                                        {
                                            var arrAll=document.all;   
                                            for (i=0;i <arrAll.length;i++)   
                                            {    
                                               if(arrAll[i].id!="""")
                                                 $id(arrAll[i].id).disabled=true;
                                            }  
                                        }
                                        }</script><script src='../../fceform/js/fcopendj.js'></script><script defer src='../../fceform/js/fcsavedj.js'>
                                        </script><script src='../../fceform/js/fcselfuse.js'></script><script src='../../fceform/js/fcbasecont.js'></script>
                                        <script defer src='../../fceform/js/fcother.js'></script><script defer src='../../fceform/js/selectdate.js'></script>
                                        <script src='../../fceform/../fceformext/js/userfunc.js'></script><link href=../../fceform/css/tabstyle.css type=text/css rel=stylesheet>
                                        <link type='text/css' rel='stylesheet' href='../../fceform/css/Button.css'/><link type='text/css' rel='stylesheet' href='../../fceform/css/TextStyle.css'/>
                                        <script >document.styleSheets[0].addRule(""fc\\:dataset"",""behavior: url(../../fceform/htc/dataset.htc)"",0);</script>
                                        <script src='../../fceform/js/fcdataset.js'></script>
                                        <DIV id=SKbillsheet jslib=""fcopendj.js&#13;&#10;fcsavedj.js&#13;&#10;fcselfuse.js&#13;&#10;fcbasecont.js&#13;&#10;fcother.js&#13;&#10;selectdate.js&#13;&#10;~userfunc.js""
                                        contxml=""<root><button><id>btnsubmit</id><id>btndelete</id></button><text><id>hidFlowID</id></text><dataset><id>dataset1</id></dataset></root>""
                                        controlno=""SKButton:0;SKDBedit:0;checkbox:0;label:2;radio:0;listbox:0;textarea:0;combobox:0;password:0;upload:0;SKDBtext:0;chart:0;dbimg:0;img:0;SKBILLgrid:0;shape:0;tab:0;div:0;DsMain_field:0;a:0;button:4;text:4;hr:0;checkboxlist:0;radiolist:0;dropdownlist:0;grid:0;dataset:2;spin:0;excel:0;tree:0;ebshow:0;ebiao:0;layout:0;page:0""
                                        billtaborder=""<root><taborder>btnsubmit</taborder><taborder>btndelete</taborder><taborder>hidFlowID</taborder></root>"" isCheckPermit=""否"" alertType=""1""
                                        userType BLONclose BLONopenBefore BLONopen=""InitPage()"" window=""当前窗口"" toolbar=""不带工具栏"" posheight=""300"" poswidth postop posleft center=""  ""
                                        isfile=""否"" type=""TT"" caption=""#caption#"" dj_sn=""#djsn#"" userToolbar='<tr rowstate=""add""><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>' AutoResizeXml>
                                        <BUTTON id=btnsubmit style=""DISPLAY: none; LEFT: 328px; WIDTH: 233px; POSITION: absolute; TOP: 1px; HEIGHT: 25px""
                                        onmovestart=moveStart() onclick=""bill_onclick(&quot;$eform('提交数据')&quot;)"" controltype=""button"">流程提交按钮,请不要删除,修改</BUTTON>
                                        <BUTTON id=btndelete style=""DISPLAY: none; LEFT: 560px; WIDTH: 229px; POSITION: absolute; TOP: 1px; HEIGHT: 25px"" onmovestart=moveStart() 
                                        onclick=""bill_onclick(&quot;$eform('删除')&quot;)"" controltype=""button"">流程删除按钮,请不要删除,修改</BUTTON><?xml:namespace prefix = fc />
                                        <fc:dataset id=dataset1 controltype=""dataset"" opensortno=""1"" pubpara=""否"" isaddemptyrec=""否"" isSubGrid=""否"" submittype=""2"" submitno=""1""
                                        savetable=""E_TEST2"" idtype=""1"" idparam=""T"" issubds=""否"" onValid='bill_ondatasetvalid(""<dsid><FLOWNO></FLOWNO><ID></ID></dsid>"")' 
                                        onGetText='bill_ondatasetgettext(""<dsid><FLOWNO></FLOWNO><ID></ID></dsid>"")' onSetText='bill_ondatasetsettext(""<dsid><FLOWNO></FLOWNO><ID></ID></dsid>"")' 
                                        sqltrans=""%F4%D8%E6%D8%D4%F6XrnbXrn%DA%F2%EC%E8Xrn%98%CC%B6%D8%F4%F6rXrn%FC%DE%D8%F2%D8Xrn%9A%E6%EC%FC%AA%ACXt%96Xr%7CXt%90X%7C%92%EE%D0%F2%D8%EA%F6j%B2%D8%F0%F8%D8%F4%F6j%B0%F8%D8%F2%u0100%B4%F6%F2%E0%EA%DCXr%7EXr%7C%A0%96Xr%7CXr%80j%F6%EC%B4%F6%F2%E0%EA%DCXr%7EXr%80X%7C%96Xt%90Xr%7C""
                                        format=""<fields><field><fieldname>FLOWNO</fieldname><datatype>实数</datatype><displaylabel>流程关联编号</displaylabel><size>16</size>
                                        <precision>0</precision><fieldkind>数据项</fieldkind><defaultvalue></defaultvalue><displayformat></displayformat><nvl>否</nvl><iskey>否</iskey>
                                        <valid>是</valid><procvalid>否</procvalid><link>否</link><target></target><href></href><visible>否</visible><primarykey>否</primarykey>
                                        <fieldvalid></fieldvalid><tag></tag></field><field><fieldname>ID</fieldname><datatype>字符</datatype><displaylabel>主键编号</displaylabel><size>10</size>
                                        <precision>0</precision><fieldkind>数据项</fieldkind><defaultvalue></defaultvalue><displayformat></displayformat><nvl>否</nvl><iskey>否</iskey>
                                        <valid>是</valid><procvalid>否</procvalid><link>否</link><target></target><href></href><visible>否</visible><primarykey>是</primarykey>
                                        <fieldvalid></fieldvalid><tag></tag></field></fields>""></fc:dataset><INPUT id=hidFlowID style=""DISPLAY: none; LEFT: 213px; WIDTH: 110px; POSITION: absolute; TOP: 1px; HEIGHT: 20px""
                                        onmovestart=moveStart() controltype=""text"" china=""流程关联编号"" dataset=""dataset1"" field=""FLOWNO""></DIV>";
                string strDjPosition = "132,248,797,300,  ,不带工具栏,undefined,当前窗口,#caption#";
                string strdesigntext = @"<script>function InitPage()
                                        {
                                        $id(""hidFlowID"").value = parent.Request.QueryString('ID').toString();
                                        hidFlowID.fireEvent(""onchange"");
                                        var scontrol = parent.Request.QueryString('control').toString();
                                        if(scontrol==""true"")
                                        {
                                            var arrAll=document.all;   
                                            for (i=0;i <arrAll.length;i++)   
                                            {    
                                               if(arrAll[i].id!="""")
                                                 $id(arrAll[i].id).disabled=true;
                                            }  
                                        }
                                        }</script>
                                        <DIV oncontrolselect=controlselect() id=SKbillsheet contentEditable=true jslib=""fcopendj.js&#13;&#10;fcsavedj.js&#13;&#10;fcselfuse.js&#13;&#10;fcbasecont.js&#13;&#10;fcother.js&#13;&#10;selectdate.js&#13;&#10;~userfunc.js""
                                        unselectable=""on"" contxml=""<root><button><id>btnsubmit</id><id>btndelete</id></button><text><id>hidFlowID</id></text><dataset><id>dataset1</id>
                                        </dataset></root>"" controlno=""SKButton:0;SKDBedit:0;checkbox:0;label:2;radio:0;listbox:0;textarea:0;combobox:0;password:0;upload:0;SKDBtext:0;chart:0;dbimg:0;img:0;SKBILLgrid:0;shape:0;tab:0;div:0;DsMain_field:0;a:0;button:4;text:4;hr:0;checkboxlist:0;radiolist:0;dropdownlist:0;grid:0;dataset:2;spin:0;excel:0;tree:0;ebshow:0;ebiao:0;layout:0;page:0""
                                        billtaborder=""<root><taborder>btnsubmit</taborder><taborder>btndelete</taborder><taborder>hidFlowID</taborder></root>"" isCheckPermit=""否"" 
                                        alertType=""1"" userType BLONclose BLONopenBefore BLONopen=""InitPage()"" window=""当前窗口"" toolbar=""不带工具栏"" posheight=""300"" 
                                        poswidth postop posleft center=""  "" isfile=""否"" type=""TT"" caption=""#caption#"" dj_sn=""#djsn#"" userToolbar='<tr rowstate=""add""><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>'>
                                        <BUTTON onmove=move() oncontrolselect=controlselect() id=btnsubmit onresizeend=resizeEnd() onresizestart=resizeStart() onmoveend=moveEnd() onresize=resize()
                                        style=""DISPLAY: none; LEFT: 328px; WIDTH: 233px; POSITION: absolute; TOP: 1px; HEIGHT: 25px"" onmovestart=moveStart() fc_onclick=""bill_onclick(&quot;$eform('提交数据')&quot;)""
                                        controltype=""button"">流程提交按钮,请不要删除,修改</BUTTON><BUTTON onmove=move() oncontrolselect=controlselect() id=btndelete onresizeend=resizeEnd() onresizestart=resizeStart() onmoveend=moveEnd() onresize=resize() 
                                        style=""DISPLAY: none; LEFT: 560px; WIDTH: 229px; POSITION: absolute; TOP: 1px; HEIGHT: 25px"" onmovestart=moveStart() fc_onclick=""bill_onclick(&quot;$eform('删除')&quot;)""
                                        controltype=""button"">流程删除按钮,请不要删除,修改</BUTTON><IMG id=dataset1 onresizeend=resizeEnd() onresizestart=resizeStart() onmoveend=moveEnd() 
                                        style=""LEFT: 747px; WIDTH: 39px; POSITION: absolute; TOP: 24px; HEIGHT: 47px"" onmovestart=moveStart() src=""../../fceform/images/ef_designer_dataset.gif""
                                        controltype=""dataset"" opensortno=""1"" saveastable pubpara=""否"" isaddemptyrec=""否"" isSubGrid=""否"" 
                                        opensql=""select * from E_Test2 where FlowNO=':{parent.Request.QueryString('ID').toString()}:'"" BeforeOpen submittype=""2"" submitno=""1"" 
                                        savetable=""E_TEST2"" idtype=""1"" idparam=""T"" issubds=""否"" subdsfield masterds masterdsfield async=""否"" AfterOpen BeforePost AfterPost BeforeScroll AfterScroll formatxml='<root>
                                        <tr rowstate=""add""><td>FLOWNO</td><td>流程关联编号</td><td>实数</td><td>16</td><td>0</td><td>数据项</td><td></td><td></td><td>否</td><td>否</td><td>否</td><td>否</td><td>是</td><td>否</td><td>否</td>
                                        <td>否</td><td></td><td></td><td>right</td><td>80</td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr><tr rowstate=""add"">
                                        <td>ID</td><td>主键编号</td><td>字符</td><td>10</td><td>0</td><td>数据项</td><td></td><td></td><td>否</td><td>否</td><td>否</td><td>否</td><td>是</td>
                                        <td>否</td><td>是</td><td>否</td><td></td><td></td><td>left</td><td>80</td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr></root>'
                                        datasourceName queryTableName=""E_TEST2""><INPUT onmove=move() oncontrolselect=controlselect() id=hidFlowID onresizeend=resizeEnd() onresizestart=resizeStart() onmoveend=moveEnd() onresize=resize() 
                                        style=""DISPLAY: none; LEFT: 213px; WIDTH: 110px; POSITION: absolute; TOP: 1px; HEIGHT: 20px"" onmovestart=moveStart() value=流程关联编号 controltype=""text""
                                        china=""流程关联编号"" dataset=""dataset1"" field=""FLOWNO""></DIV>";

                //同步新增流程模型
                string strSQL = @"SELECT OFlowModelID,FlowName FROM ES_FlowModel a 
                                    WHERE deleted=0 AND status=1 AND appid =203 
                                        AND NOT EXISTS(SELECT OFlowModelID FROM FC_BILLZL b WHERE a.OFlowModelID=b.OFlowModelID)";
                DataTable dt = CommonDP.ExcuteSqlTable(strSQL);
                foreach (DataRow dr in dt.Rows)
                {
                    string strFormName = dr["FlowName"].ToString();
                    string strdjsn = dr["OFlowModelID"].ToString();
                    FC_BILLZLDP ptemp = new FC_BILLZLDP();
                    ptemp.dj_name = strFormName;
                    ptemp.djlx = "TT";
                    ptemp.xmltext = strxmltext.Replace("#caption#", strFormName).Replace("#djsn#", strdjsn);
                    ptemp.DjPosition = strDjPosition.Replace("#caption#", strFormName);
                    ptemp.djsn = strdjsn;
                    ptemp.designtext = strdesigntext.Replace("#caption#", strFormName).Replace("#djsn#", strdjsn);
                    ptemp.stmptable = "";
                    ptemp.userType = "";
                    ptemp.oFlowModelID = decimal.Parse(dr["OFlowModelID"].ToString());
                    ptemp.FlowName = strFormName;
                    ptemp.InsertRecorded(ptemp);
                }

                //同步流程名称
                strSQL = @"UPDATE FC_BILLZL SET FlowName=b.FlowName FROM ES_FlowModel b WHERE FC_BILLZL.OFlowModelID=b.OFlowModelID";
                CommonDP.ExcuteSql(strSQL);
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region 取得配置自定义表单信息 GetUserDefineTable
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lngFlowModelID"></param>
        /// <returns></returns>
        public static DataTable GetUserDefineTable(long lngFlowModelID)
        {
            string strSql = @"SELECT a.djlx,a.djsn,a.djposition  
                                FROM FC_BILLZL a,Es_FlowModel b 
                                WHERE a.oFlowModelID=b.oFlowModelID AND b.deleted=0 AND b.status=1 AND b.FlowModelID=" + lngFlowModelID.ToString();
            DataTable dt = CommonDP.ExcuteSqlTable(strSql);
            return dt;
        }
        #endregion

        #region 取得自定义流程实例的ID GetFlowUserDifineID
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetFlowUserDifineID(long lngFlowID)
        {
            string sSql = "SELECT id FROM es_flow WHERE flowid=" + lngFlowID.ToString();
            string sCnn = ConfigTool.GetConnectString();
            try
            {
                return OracleDbHelper.ExecuteScalar(sCnn, CommandType.Text, sSql).ToString();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion 
    }
}

