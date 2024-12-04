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
using EpowerCom;
using EpowerGlobal;
using System.Xml;
using System.IO;
using Epower.DevBase.BaseTools;
using System.Text;
using System.Collections.Generic;

namespace Epower.ITSM.Web.Forms
{
    /// <summary>
    /// flow_sender 的摘要说明。
    /// </summary>
    public partial class flow_sender : BasePage
    {





        protected long lngMessageID = 0;
        long lngFlowModelID = 0;
        long lngActionID = -1;
        string strFormXMLValue = "";
        string strFormDefineXml = "";  //表单指定处理人XML串

        string strMessage = "";  //如果是直接发送的话，流程提交后的提示信息 ，如果为空则按提交界面的方式提示给用户
        string strAutoMsg = "";

        public string strNodeList = "";   //环节列表
        public string strLinkNodeID = "";    //下一连接环节的模型ID
        public string strLinkNodeType = "";  //下一连接环节的模型类型


        protected string strMasterNodeID = "";  //分流时必选环节模型ID

        protected string strMasterNodeName = ""; //分流时必须选择的模型名称

        protected e_SpecRightType intSpecRightType = e_SpecRightType.esrtNormal;

        protected long lngReDoUserID = 0;    //重审时要提交的的用户编号

        //特殊处理类别　：交接　终止　跳转等
        string strJumpNodeModelID = "0";                 //跳转 驳回 重审 时的　选择要跳转到的环节模型ＩＤ

        string strReDoNodeModelType = "30";            //重审时带过来的环节模型ID


        protected string strSelType = "0";     //选人页面的方式 1 时为调度时使用


        protected string strSpecRightType = "10";



        protected string sIniXml = "";

        protected string sblnDeleteMaster = "1";    //为0时可以不可以删除主办  （只有一个主办 ，并接自动添加的情况）

        //新增的，列表使用
        StringBuilder sbMastPerson = new StringBuilder();
        int iMastPerson = 0;
        StringBuilder sbReadPerson = new StringBuilder();
        int iReadPerson = 0;

        /// <summary>
        /// 按列表显示人员时的人员信息容器(主办)
        /// </summary>
        System.Collections.Generic.List<KeyValuePair<String, String>> dictMastPerson = new System.Collections.Generic.List<KeyValuePair<String, String>>();
        /// <summary>
        /// 按列表显示人员时的人员信息容器(非主办)
        /// </summary>
        System.Collections.Generic.List<KeyValuePair<String, String>> dictReadPerson = new System.Collections.Generic.List<KeyValuePair<String, String>>();


        protected string lngFlowID = "0";
        protected string lngNodeID = "0";
        private int treeNodeIndex = -1;
        StringBuilder sbPersonNodeJson = new StringBuilder();

        protected void Page_Load(object sender, System.EventArgs e)
        {
            // 在此处放置用户代码以初始化页面
            CtrTitle1.Title = "处理人员选择";

            //防止用户通过IE后退按纽重复提交
            Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);


            if (Request.QueryString["SelType"] != null)
                strSelType = Request.QueryString["SelType"];

            if (strSelType == "0")
            {
                if (!string.IsNullOrEmpty(Request.Form["MessageID"]))
                {
                    lngMessageID = long.Parse(Request.Form["MessageID"]);

                }
                else
                {
                    lngMessageID = 0;
                }
                if (!string.IsNullOrEmpty(Request.Form["FlowModelID"]))
                {
                    lngFlowModelID = long.Parse(Request.Form["FlowModelID"]);
                }
                else
                {
                    lngFlowModelID = 0;
                }
                // lngFlowModelID = long.Parse(Request.Form["FlowModelID"]);
                if (!string.IsNullOrEmpty(Request.Form["ActionID"]))
                {
                    lngActionID = long.Parse(Request.Form["ActionID"]);
                }
                else
                {
                    lngActionID = -1;
                }
                // lngActionID = long.Parse(Request.Form["ActionID"]);
                strFormXMLValue = Request.Form["FormXMLValue"];
                strFormDefineXml = Request.Form["FormDefineValue"];

                intSpecRightType = (e_SpecRightType)int.Parse(Request.Form["SpecRightType"]);

                if (!string.IsNullOrEmpty(Request.Form["ReDoUserID"]))
                {
                    lngReDoUserID = long.Parse(Request.Form["ReDoUserID"]);
                }
                else
                {
                    lngReDoUserID = 0;
                }
                // lngReDoUserID = long.Parse(Request.Form["ReDoUserID"]);

                strJumpNodeModelID = Request.Form["JumpToNodeModel"];

                strReDoNodeModelType = Request.Form["ReDoNMType"];

                lngFlowID = Request.Form["FlowID"];
                lngNodeID = Request.Form["NodeID"];

                if (lngActionID == -1)
                {
                    //Response .Write("<script.reload();</script>"); this is sz_zhengquan_2
                    Response.Write("<script>self.close();</script>");
                    // return;
                    Response.End();
                }
            }
            else
            {
                //调度时
                lngMessageID = long.Parse(Request.QueryString["AttemperID"]);
                lngFlowModelID = long.Parse(Request.QueryString["FlowModelID"]);
                lngActionID = 0;
                strFormXMLValue = Request.Form["FormXMLValue"];
                strFormDefineXml = Request.Form["FormDefineValue"];



                intSpecRightType = e_SpecRightType.esrtAttemper;
                strJumpNodeModelID = "0";
            }
            strSpecRightType = ((int)intSpecRightType).ToString();

            if (intSpecRightType == e_SpecRightType.esrtAttemper)
            {
                CtrTitle1.Title = "新处理人员选择";
            }

            if (intSpecRightType == e_SpecRightType.esrtFreeTakeOver || intSpecRightType == e_SpecRightType.esrtTakeOver)
            {
                CtrTitle1.Title = "交接人员选择";
            }

            if (intSpecRightType == e_SpecRightType.esrtTransmit)
            {
                CtrTitle1.Title = "传阅人员选择";
            }

            if (intSpecRightType == e_SpecRightType.esrtAssist)
            {
                CtrTitle1.Title = "协作人员选择";
            }

            if (intSpecRightType == e_SpecRightType.esrtCommunic)
            {
                CtrTitle1.Title = "沟通人员选择";
            }

            if (!Page.IsPostBack)
            {
                if (intSpecRightType != e_SpecRightType.esrtReDoBack)
                {
                    sbMastPerson.Append("<script language=\"javascript\">\n");
                    sbMastPerson.Append("var arrMastPerson = new Array();\n"); //ID,Name
                    sbReadPerson.Append("<script language=\"javascript\">\n");
                    sbReadPerson.Append("var arrReadPerson = new Array();\n"); //ID,Name

                    InitTree();
                }
                else
                {
                    //重审时自动发送

                    string sAutoxml = @"<Receivers><Receiver ID=""1"" NodeID=""" + strJumpNodeModelID +
                                      @""" NodeType=""" + strReDoNodeModelType +
                                      @""" UserID=""" + lngReDoUserID.ToString() +
                                      @""" Name=""" + EpowerCom.EPSystem.GetUserName(lngReDoUserID) + @""" ActorType=""Worker_"" ReceiveType="""" /></Receivers>";
                    Response.Write("<script>parent.header.flowSubmit.Receivers.value=" + StringTool.JavaScriptQ(sAutoxml) + ";</script>");
                    Response.Write("<script>parent.header.flowSubmit.LinkNodeID.value='" + EnCodeTool.EnCode(strJumpNodeModelID) + "';</script>");
                    Response.Write("<script>parent.header.flowSubmit.LinkNodeType.value='" + EnCodeTool.EnCode(strReDoNodeModelType) + "';</script>");
                    Response.Write("<script>parent.header.flowSubmit.strMessage.value=" + StringTool.JavaScriptQ("") + ";</script>");
                    Response.Write("<script>parent.header.FlowSubmit();</script>");
                    UIGlobal.SelfClose(this);

                }

            }

        }


        private void InitTree()
        {
            string strRet = "";

            long lngtmpID = 0;
            string strtmpName = "";

            string strDeptPathXml = "";

            int inttmpCount = 0;
            long lngCurrNodeID = 0; //当前环节ID
            string strCurrNodeName = ""; //当前环节的名称
            long lngFirstWorkerID = 0;  //可以自动发送的主办ID
            string sFirstWorkerName = "";  //可以自动发送的主办名称
            bool blnCanSend = true;   //是否可以直接发送， 如果只有一个环节而且，每个环节只有一个主办，并且没有阅知人员，则可以直接发送
            int nodeCnt = 0;
            int workerCnt = 0;

            long lngUserID = (long)Session["UserID"];

            bool FirstDeptExpanded = false;


            long lngMasterPath = 0;   //主送路径
            long lngCurrPath = 0;    //当前路径
            long lngLinkNodeID = 0;
            long lngLinkNodeType = 0;
            e_FMNodeType lngCurrNodeType = e_FMNodeType.fmNormal;   //生成树时当前环节的类别
            e_ReceiveActorType lngCurrReceiveType = e_ReceiveActorType.eratNone; //当前接收者类别
            //e_ReceiveActorType lngCurrReceiveType; //当前接收者类别

            string strUnionLinkEnd = "False";

            int iReaders = 0;

            //2008-02-05 表单指定人员在单纯的情况下可以直接发送不用再在界面上选择
            string blnSendToDefine = "false";
            string strSendDefineXml = "";



            CustomTreeNode RootNode = new CustomTreeNode();
            CustomTreeNode ParentNode = new CustomTreeNode();
            CustomTreeNode node;

            Message objMsg = new Message();

            // **********  表单指定处理人 范例代码在母版页上 ***********
            if (intSpecRightType == e_SpecRightType.esrtReDoBack)
            {
                //重审时直接返回 接收人
                // strRet = objMsg.GetNextReceivers(lngUserID, lngMessageID, lngFlowModelID, lngActionID, intSpecRightType, long.Parse(strJumpNodeModelID), strFormXMLValue, 1, strFormDefineXml);
            }
            else
            {
                strRet = objMsg.GetNextReceivers(lngUserID, lngMessageID, lngFlowModelID, lngActionID, intSpecRightType, long.Parse(strJumpNodeModelID), strFormXMLValue, 1, strFormDefineXml);
            }            

            treeNodeIndex = -1;
            tvReceiver.Nodes.Clear();

            XmlTextReader tr = new XmlTextReader(new StringReader(strRet));

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<Receivers></Receivers>");

            sbPersonNodeJson.Remove(0, sbPersonNodeJson.Length);
            sbPersonNodeJson.Append("<script type=\"text/javascript\"> var allNodeItems=[");

            bool blnContinue = true;
            while (tr.Read() && blnContinue)
            {
                if (tr.NodeType == XmlNodeType.Element)
                {
                    //只处理元素部分即可，因为返回的XML的内容集中在元素的属性中,并按顺序添加TreeView节点
                    switch (tr.Name)
                    {
                        case "NextReceivers":
                            //判断路径
                            if (tr.GetAttribute("HasPath") == "None")
                            {
                                blnCanSend = false;
                                tr.Close();
                                UIGlobal.MsgBox(this, "没有发现可以延续流程的路径，请与流程设置人员联系！");
                                //Response.Write("<script>window.opener.parent.close();</script>");
                                UIGlobal.SelfClose(this);
                                Response.End();
                            }
                            lngLinkNodeID = long.Parse(tr.GetAttribute("LinkNodeID"));
                            lngLinkNodeType = long.Parse(tr.GetAttribute("LinkNodeType"));
                            lngMasterPath = long.Parse(tr.GetAttribute("MasterPath"));
                            lngCurrPath = long.Parse(tr.GetAttribute("CurrPath"));
                            strUnionLinkEnd = tr.GetAttribute("UnionLinkEnd");

                            strMasterNodeID = tr.GetAttribute("MasterNodeModel");

                            blnSendToDefine = tr.GetAttribute("SendToDefine");
                            if (blnSendToDefine == "" || blnSendToDefine == null)
                                blnSendToDefine = "false";

                            if (blnSendToDefine.ToLower() == "true")
                            {
                                strSendDefineXml = tr.GetAttribute("SendToDefineXml");
                                blnContinue = false;  //退出while循环
                            }

                            ///三种情况不需选择人员，直接发送出去
                            ///		1、如果下一环节类别是合流而且主送路径不匹配，
                            ///		2、下一环节类型是结束
                            ///		3、下一环节类别为合流，但合流后面有接结束环节（不一定直接连接）
                            if ((lngLinkNodeType == (long)e_FMNodeType.fmUnite && lngCurrPath != lngMasterPath)
                                || (lngLinkNodeType == (long)e_FMNodeType.fmEnd)
                                || (lngLinkNodeType == (long)e_FMNodeType.fmUnite && lngCurrPath == lngMasterPath && strUnionLinkEnd == "True"))
                            {
                                blnCanSend = true;

                                blnContinue = false;  //退出while循环

                            }
                            else
                            {
                                //添加父节点（根节点）　２００５－０６－１７
                                ParentNode = new CustomTreeNode();
                                if (intSpecRightType == e_SpecRightType.esrtNormal)
                                {
                                    ParentNode.Text = "主办人员选择(必选)";
                                }
                                else
                                {
                                    ParentNode.Text = "人员选择(必选)";
                                }
                                ParentNode.ImageUrl = "../Images/Flow/27.bmp";
                                ParentNode.Value = "Workers_" + lngCurrNodeID.ToString();
                                ParentNode.Expanded = true;
                                treeNodeIndex++;
                                tvReceiver.Nodes.Add(ParentNode);
                            }

                            break;
                        case "Nodes":
                            //判断人员
                            if (tr.GetAttribute("ActorCount").Equals("0"))
                            {
                                blnCanSend = false;
                                UIGlobal.MsgBox(this, "没有发现可以进行后续处理的人员， 请与流程设置人员联系！");
                                //Response.Write("<script>window.opener.parent.close();</script>");
                                UIGlobal.SelfClose(this);
                                Response.End();
                            }
                            break;
                        case "Node":
                            nodeCnt++;
                            if (nodeCnt > 1)
                                blnCanSend = false;
                            lngtmpID = long.Parse(tr.GetAttribute("ID"));
                            lngCurrNodeID = lngtmpID;
                            strNodeList = strNodeList + lngCurrNodeID.ToString() + ",";
                            strtmpName = tr.GetAttribute("Name");
                            lngCurrNodeType = (e_FMNodeType)(int.Parse(tr.GetAttribute("NodeType")));
                            //如果环节没有名称，则生成缺省值
                            if (strtmpName.Trim() == "")
                                strtmpName = "环节" + lngtmpID.ToString();
                            strCurrNodeName = strtmpName;

                            if (lngCurrNodeID.ToString() == strMasterNodeID)
                            {
                                strMasterNodeName = strCurrNodeName;
                            }

                            ///判断当前环节是否有主办，如果任一个环节没有主办，均不能正常执行
                            inttmpCount = int.Parse(tr.GetAttribute("WorkerCount"));
                            if (inttmpCount == 0)
                            {
                                blnCanSend = false;
                                UIGlobal.MsgBox(this, "在环节：" + StringTool.ParseForResponse(strtmpName) + " 没有发现可以进行后续处理的人员，请与流程设置人员联系！");
                                Response.Write("<script>window.opener.parent.close();</script>");
                                UIGlobal.SelfClose(this);
                                Response.End();

                            }
                            ///添加节点
                            RootNode = new CustomTreeNode();
                            RootNode.Text = "环节：" + strtmpName;
                            RootNode.ImageUrl = "../Images/Flow/118.bmp";
                            RootNode.Value = "Node_" + lngtmpID.ToString();
                            RootNode.Expanded = true;
                            treeNodeIndex++;
                            ParentNode.ChildNodes.Add(RootNode);



                            break;
                        case "Workers":
                            ///添加节点
                            // 2005-06-17 修改: 主办/阅知/协办分开显示,这里不添加节点
                            //							ParentNode = new CustomTreeNode();
                            //							ParentNode.Text =  "所有主办人员";
                            //							ParentNode.ImageUrl = "../Images/Flow/27.bmp";
                            //							ParentNode.ID = "Workers_" + lngCurrNodeID.ToString();
                            //							ParentNode.Expanded = true;
                            //							RootNode.Nodes.Add(ParentNode);
                            break;
                        case "Worker":
                            workerCnt++;
                            if (workerCnt > 1)
                                blnCanSend = false;
                            lngtmpID = long.Parse(tr.GetAttribute("ID"));

                            strtmpName = tr.GetAttribute("Name");
                            if (strtmpName.Trim() == "")
                                strtmpName = "人员ID(" + lngtmpID.ToString() + ")";
                            if (workerCnt == 1)
                            {
                                lngFirstWorkerID = lngtmpID;
                                sFirstWorkerName = strtmpName + "(" + strCurrNodeName + ")";
                            }
                            if (lngCurrNodeType == e_FMNodeType.fmOrgNode || lngCurrNodeType == e_FMNodeType.fmDisperse)
                            {
                                //机构环节或发散环节只选择组

                                lngCurrReceiveType = (e_ReceiveActorType)(int.Parse(tr.GetAttribute("ReceiveType")));
                                strDeptPathXml = tr.GetAttribute("DeptPathXml");
                                if (strDeptPathXml == null) strDeptPathXml = "";
                                if (strDeptPathXml.Length > 0)
                                {
                                    //存在部门树节点的情况

                                    //添加节点
                                    AddActorNodeGroup(ref RootNode, strtmpName, lngtmpID, "Worker", lngCurrNodeID, strCurrNodeName, strDeptPathXml, lngCurrNodeType, lngCurrReceiveType);

                                }
                                else
                                {

                                    node = new CustomTreeNode();
                                    node.Text = strtmpName;
                                    switch (lngCurrReceiveType)
                                    {
                                        case e_ReceiveActorType.eratPerson:
                                            node.ImageUrl = "../Images/Flow/56.bmp";
                                            break;
                                        case e_ReceiveActorType.eratDept:
                                            node.ImageUrl = "../Images/Flow/27.bmp";
                                            break;
                                        case e_ReceiveActorType.eratCond:
                                            node.ImageUrl = "../Images/Flow/3.bmp";
                                            break;
                                        case e_ReceiveActorType.eratExtend:
                                            node.ImageUrl = "../Images/Flow/3.bmp";
                                            break;

                                    }
                                    node.Value = "Worker_" + lngtmpID.ToString() + "_" + ((int)lngCurrReceiveType).ToString();
                                    node.NodeBllType = lngCurrNodeID.ToString() + "_" + ((int)lngCurrNodeType).ToString();  //环节的编号_环节类别
                                    node.NodeData = strCurrNodeName;  //环节的名称
                                    if (FirstDeptExpanded == false)
                                    {
                                        node.Expanded = true;
                                        //FirstDeptExpanded=true;
                                    }
                                    treeNodeIndex++;
                                    RootNode.ChildNodes.Add(node);

                                    // sbMastPerson.Append("arrMastPerson[" + iMastPerson + "] = new Array(\"" + node.Value + "|" + node.NodeBllType + "|" + node.NodeData + "|" + strtmpName + "\", \"" + strCurrNodeName + "----" + strtmpName + "/用户组" + "\");\n");
                                    String strPersonInfo = " new Array(\"" + node.Value + "|" + node.NodeBllType + "|" + node.NodeData + "|" + strtmpName + "\", \"" + strCurrNodeName + "----" + strtmpName + "/用户组" + "\");\n";
                                    dictMastPerson.Add(new KeyValuePair<String, String>(strtmpName, strPersonInfo));

                                    //{ name: "深发展A", code: "000001",spell:"sfza",id:'1' }
                                    string UserID = node.Value;

                                    if (!sbPersonNodeJson.ToString().EndsWith("["))
                                    {
                                        sbPersonNodeJson.Append(",");
                                    }
                                    sbPersonNodeJson.Append("{");

                                    sbPersonNodeJson.AppendFormat("key:\"{0}\",", "tvReceivert" + treeNodeIndex.ToString());

                                    sbPersonNodeJson.AppendFormat("userid:\"{0}\",", UserID);
                                    sbPersonNodeJson.AppendFormat("nodeid:\"{0}\",", lngCurrNodeID.ToString());
                                    sbPersonNodeJson.AppendFormat("nodetype:\"{0}\",", ((int)lngCurrNodeType).ToString());
                                    sbPersonNodeJson.AppendFormat("name:\"{0}\",", node.Text);
                                    sbPersonNodeJson.AppendFormat("nodedata:\"{0}\",", node.NodeData);
                                    sbPersonNodeJson.AppendFormat("roleid:\"{0}\",", "Worker_");
                                    int typepoint = UserID.Substring(7).IndexOf("_");
                                    sbPersonNodeJson.AppendFormat("typepoint:\"{0}\",", typepoint);
                                    if (typepoint > 0)
                                    {
                                        sbPersonNodeJson.AppendFormat("rectype:\"{0}\",", UserID.Substring(7).Substring(typepoint + 1));
                                        sbPersonNodeJson.AppendFormat("actorid:\"{0}\"", UserID.Substring(7).Substring(0, typepoint));
                                    }
                                    else
                                    {
                                        sbPersonNodeJson.AppendFormat("rectype:\"{0}\",", "");
                                        sbPersonNodeJson.AppendFormat("actorid:\"{0}\"", UserID.Substring(7));
                                    }
                                    sbPersonNodeJson.Append("}");

                                    node.Text = node.Text
                                        + String.Format("<span style=\"display:none\">{0}</span>", node.Value);

                                    iMastPerson++;
                                }

                            }
                            else
                            {
                                strDeptPathXml = tr.GetAttribute("DeptPathXml");

                                ///添加节点
                                //添加节点
                                CustomTreeNode addNode = AddActorNode(ref RootNode, strtmpName, lngtmpID, "Worker", lngCurrNodeID, strCurrNodeName, strDeptPathXml, lngCurrNodeType);

                                //TreeNode tempNode = RootNode.ChildNodes[RootNode.ChildNodes.Count-1];
                                string UserID = addNode.Value;

                                if (!sbPersonNodeJson.ToString().EndsWith("["))
                                {
                                    sbPersonNodeJson.Append(",");
                                }
                                sbPersonNodeJson.Append("{");

                                sbPersonNodeJson.AppendFormat("key:\"{0}\",", "tvReceivert" + treeNodeIndex.ToString());

                                sbPersonNodeJson.AppendFormat("userid:\"{0}\",", UserID);
                                sbPersonNodeJson.AppendFormat("nodeid:\"{0}\",", lngCurrNodeID.ToString());
                                sbPersonNodeJson.AppendFormat("nodetype:\"{0}\",", ((int)lngCurrNodeType).ToString());
                                sbPersonNodeJson.AppendFormat("name:\"{0}\",", strtmpName);
                                sbPersonNodeJson.AppendFormat("nodedata:\"{0}\",", strCurrNodeName);
                                sbPersonNodeJson.AppendFormat("roleid:\"{0}\",", "Worker_");
                                int typepoint = UserID.Substring(7).IndexOf("_");
                                sbPersonNodeJson.AppendFormat("typepoint:\"{0}\",", typepoint);
                                if (typepoint > 0)
                                {
                                    sbPersonNodeJson.AppendFormat("rectype:\"{0}\",", UserID.Substring(7).Substring(typepoint + 1));
                                    sbPersonNodeJson.AppendFormat("actorid:\"{0}\"", UserID.Substring(7).Substring(0, typepoint));
                                }
                                else
                                {
                                    sbPersonNodeJson.AppendFormat("rectype:\"{0}\",", "");
                                    sbPersonNodeJson.AppendFormat("actorid:\"{0}\"", UserID.Substring(7));
                                }
                                sbPersonNodeJson.Append("}");

                                if (FirstDeptExpanded == false)
                                {
                                    foreach (CustomTreeNode n in RootNode.ChildNodes)
                                    {
                                        n.Expanded = true;

                                    }
                                    FirstDeptExpanded = true;
                                }
                            }

                            //如果可以直接发送，构造发送的接收人员串
                            if (blnCanSend)
                            {
                                XmlElement xmlEle = xmlDoc.CreateElement("Receiver");
                                xmlEle.SetAttribute("ID", "1");
                                xmlEle.SetAttribute("NodeID", lngCurrNodeID.ToString());
                                xmlEle.SetAttribute("NodeType", ((int)lngCurrNodeType).ToString());
                                xmlEle.SetAttribute("UserID", lngtmpID.ToString());
                                xmlEle.SetAttribute("Name", strtmpName);
                                xmlEle.SetAttribute("ActorType", "Worker_");
                                if (lngCurrNodeType == e_FMNodeType.fmOrgNode || lngCurrNodeType == e_FMNodeType.fmDisperse)
                                {
                                    //机构环节或发散环节只选择组
                                    xmlEle.SetAttribute("ReceiveType", ((int)lngCurrReceiveType).ToString());

                                }
                                else
                                {
                                    //普通环节设置接收角色为空
                                    xmlEle.SetAttribute("ReceiveType", "");
                                }
                                strAutoMsg = " " + strtmpName + " 进行 [" + strCurrNodeName + "] 环节的处理";

                                xmlDoc.DocumentElement.AppendChild(xmlEle);
                            }
                            break;
                        // 2005-06-17 修改: 主办/阅知/协办分开显示,这里不添加节点
                        case "Readers":
                            ///添加节点
                            //							ParentNode = new CustomTreeNode();
                            //							ParentNode.Text =  "所有阅知人员";
                            //							ParentNode.ImageUrl = "../Images/Flow/41.bmp";
                            //							Parentnode.Value = "Readers_" + lngCurrNodeID.ToString();
                            //							ParentNode.Expanded = true;
                            //							RootNode.Nodes.Add(ParentNode);
                            break;
                        case "Reader":
                            if (strSelType != "1")
                            {
                                // 调度时 有阅知人员也不显示出来 
                                blnCanSend = false;
                            }
                            iReaders++;

                            break;
                    }
                }

            }


            tr.Close();


            #region 默认折叠[接收人员和部门]树, 只显示第一级部门列表 - 2013-07-08 16:22 - 孙绍棕
            /*     
             * Date: 2013-07-08 16:22
             * summary: 默认折叠[接收人员和部门]树, 只显示第一级部门列表
             *              
             * modified: sunshaozong@gmail.com     
             */

            if (tvReceiver.Nodes.Count > 0)
            {
                if (tvReceiver.Nodes[0].ChildNodes.Count > 0)
                {
                    TreeNode rootDept = tvReceiver.Nodes[0].ChildNodes[0];

                    for (int i = 0; i < rootDept.ChildNodes.Count; i++)
                    {
                        TreeNode firstLevelDept = rootDept.ChildNodes[i];
                        firstLevelDept.CollapseAll();
                    }
                }
            }
            #endregion


            //添加协办/阅知树
            if (iReaders > 0 && strSelType == "0" && intSpecRightType != e_SpecRightType.esrtTakeOver
                && intSpecRightType != e_SpecRightType.esrtAttemper && intSpecRightType != e_SpecRightType.esrtBackHasDone
                && intSpecRightType != e_SpecRightType.esrtBackHasDoneRedo && intSpecRightType != e_SpecRightType.esrtBackHasDoneFlow
                && intSpecRightType != e_SpecRightType.esrtAssist && intSpecRightType != e_SpecRightType.esrtTransmit
                && intSpecRightType != e_SpecRightType.esrtFreeTakeOver && intSpecRightType != e_SpecRightType.esrtCommunic)
            {
                //只有普通发送和跳转时才可以显示 协办和阅知
                CreateTreeViewContent(tvAssistor, strRet, "Assist");

                CreateTreeViewContent(tvReader, strRet, "Reader");
            }
            else
            {
                //改变树的高度
                tvReceiver.Height = new Unit("380px");
                tvAssistor.Visible = false;
                tvReader.Visible = false;
                name1.Visible = false;
                name2.Visible = false;

                if (intSpecRightType == e_SpecRightType.esrtTransmit)  //传阅
                {
                    lblMast.Text = "传阅";
                }
                else if (intSpecRightType == e_SpecRightType.esrtAssist)   //协办
                {
                    lblMast.Text = "协办";
                }
            }
            sbPersonNodeJson.Append("]; </script>");
            //Response.Write(sbPersonNodeJson.ToString());
            Page.RegisterStartupScript(System.DateTime.Now.Ticks.ToString(), sbPersonNodeJson.ToString());

            strLinkNodeID = EnCodeTool.EnCode(lngLinkNodeID.ToString());
            strLinkNodeType = EnCodeTool.EnCode(lngLinkNodeType.ToString());
            //			strLinkNodeID = lngLinkNodeID.ToString();
            //			strLinkNodeType = lngLinkNodeType.ToString();

            if (strNodeList.EndsWith(","))
            {
                strNodeList = strNodeList.Substring(0, strNodeList.Length - 1);
            }


            dictMastPerson.Sort(new KeyValueArrangedInAlphabeticalOrderComparer());
            Int32 idx = 0;
            foreach (KeyValuePair<String, String> item in dictMastPerson)
            {
                sbMastPerson.AppendFormat("arrMastPerson[{0}] = {1} ", idx, item.Value);
                idx = idx + 1;
            }

            litMastPerson.Text = sbMastPerson.ToString() + "</script>";


            dictReadPerson.Sort(new KeyValueArrangedInAlphabeticalOrderComparer());
            idx = 0;
            foreach (KeyValuePair<String, String> item in dictReadPerson)
            {
                sbReadPerson.AppendFormat(" arrReadPerson[{0}] = {1} ", idx, item.Value);
                idx = idx + 1;
            }
            litReadPerson.Text = sbReadPerson.ToString() + "</script>";


            //Session["IniXML"] = "<Receivers></Receivers>";
            sIniXml = "<Receivers></Receivers>";
            if (workerCnt == 1)
            {
                //只有一个主办　但有协办和阅知的情况下隐藏　主办选择树
                //Session["IniXML"] = xmlDoc.InnerXml;
                sIniXml = xmlDoc.InnerXml;
                sblnDeleteMaster = "0";
                //如果只一个主办，自动添加到列表
                lstSelected.Items.Add(new ListItem("主办:" + sFirstWorkerName, lngFirstWorkerID.ToString()));
                //不为以下特殊情况时隐藏
                if (intSpecRightType != e_SpecRightType.esrtTakeOver && intSpecRightType != e_SpecRightType.esrtFreeTakeOver && intSpecRightType != e_SpecRightType.esrtAttemper
                     && intSpecRightType != e_SpecRightType.esrtCommunic && intSpecRightType != e_SpecRightType.esrtAssist && intSpecRightType != e_SpecRightType.esrtTransmit)
                {
                    tvReceiver.Visible = false;
                    name0.Visible = false;
                }
                this.tvAssistor.Height = new Unit("195px");
                this.tvReader.Height = new Unit("195px");
            }

            // # Start. 按拼音排序部门和人员树

            if (tvReceiver.Nodes.Count > 0)
                ArrangedInAlphabeticalOrder(tvReceiver.Nodes[0]);
            if (tvReader.Nodes.Count > 0)
                ArrangedInAlphabeticalOrder(tvReader.Nodes[0]);
            if (tvAssistor.Nodes.Count > 0)
                ArrangedInAlphabeticalOrder(tvAssistor.Nodes[0]);

            // # End.

            //添加客户端的隐含字段
            if (blnSendToDefine.ToLower() == "true")
            {
                //strSendDefineXml = tr.GetAttribute("SendToDefineXml");
                //blnContinue = false;  //退出while循环
                //添加客户端的隐含字段
                strMessage = "";
                Response.Write("<script>parent.header.flowSubmit.Receivers.value=" + StringTool.JavaScriptQ(strSendDefineXml) + ";</script>");
                Response.Write("<script>parent.header.flowSubmit.LinkNodeID.value='" + strLinkNodeID + "';</script>");
                Response.Write("<script>parent.header.flowSubmit.LinkNodeType.value='" + strLinkNodeType + "';</script>");
                Response.Write("<script>parent.header.flowSubmit.strMessage.value=" + StringTool.JavaScriptQ(strMessage) + ";</script>");
                Response.Write("<script>parent.header.FlowSubmit();</script>");
                UIGlobal.SelfClose(this);
            }
            else
            {
                if (intSpecRightType == e_SpecRightType.esrtTakeOver || intSpecRightType == e_SpecRightType.esrtFreeTakeOver || intSpecRightType == e_SpecRightType.esrtAttemper
                     || intSpecRightType == e_SpecRightType.esrtCommunic || intSpecRightType == e_SpecRightType.esrtAssist || intSpecRightType == e_SpecRightType.esrtTransmit)
                {
                    //交接 传阅 协作 沟通 调度时都不能自动发送
                    blnCanSend = false;
                }

                if (blnCanSend == true)
                {
                    if (strSelType == "1")
                    {
                        Response.Write("<script>alert('没有其它可以调度的对象');</script>");
                    }
                    else
                    {
                        //设置提示信息
                        if (lngLinkNodeType == (long)e_FMNodeType.fmEnd)
                        {
                            strMessage = " 流程已经完成";
                        }

                        if (lngLinkNodeType == (long)e_FMNodeType.fmUnite)
                        {
                            if (strUnionLinkEnd == "True")
                            {
                                strMessage = " 当其它到达同一合流环节的所有主办事项处理完毕时，流程将自动完成";
                            }
                            else
                            {
                                if (lngCurrPath == lngMasterPath)
                                {
                                    strMessage = " 当其它到达同一合流环节的所有主办事项处理完毕时，流程将自动提交给 " + strAutoMsg;
                                }
                                else
                                {
                                    strMessage = " 当其它到达同一合流环节的所有主办事项处理完毕时，流程将自动提交,处理人员及处理路径由其它主办事项的处理人员指定";
                                }

                            }
                        }

                        if (lngLinkNodeType == (long)e_FMNodeType.fmNormal || lngLinkNodeType == (long)e_FMNodeType.fmDraft)
                        {
                            strMessage = "流程已经自动的发送给" + strAutoMsg;
                        }


                        //直接发送
                        //Response.Write("<script>alert('可以直接发送');</script>");

                        //改为筐架内实现,不弹出窗口
                        Response.Write("<script>parent.header.flowSubmit.Receivers.value=" + StringTool.JavaScriptQ(xmlDoc.InnerXml) + ";</script>");
                        Response.Write("<script>parent.header.flowSubmit.LinkNodeID.value='" + strLinkNodeID + "';</script>");
                        Response.Write("<script>parent.header.flowSubmit.LinkNodeType.value='" + strLinkNodeType + "';</script>");
                        Response.Write("<script>parent.header.flowSubmit.strMessage.value=" + StringTool.JavaScriptQ(strMessage) + ";</script>");
                        Response.Write("<script>parent.header.FlowSubmit();</script>");
                        UIGlobal.SelfClose(this);

                    }
                }
                else
                {
                    if (lngLinkNodeType == (long)e_FMNodeType.fmUnite)
                    {
                        strMessage = "流程已经提交，当其它到达同一合流环节的所有主办事项处理完毕时，流程将自动发送给选择的处理人员";
                        //Response.Write("<script>window.opener.flowSubmit.strMessage.value=" + StringTool.JavaScriptQ(strMessage) +";</script>");
                        //显示选人页面

                        Response.Write("<script>parent.header.flowSubmit.strMessage.value=" + StringTool.JavaScriptQ(strMessage) + ";</script>");
                    }
                    Response.Write("<script>parent.fraaddNew.cols='0,0,100%';</script>");
                }

            }






        }




        private void CreateTreeViewContent(TreeView tv, string strXml, string strType)
        {

            CustomTreeNode RootNode = new CustomTreeNode();
            CustomTreeNode ParentNode = new CustomTreeNode();
            CustomTreeNode node;

            long lngtmpID = 0;
            string strtmpName = "";

            string strDeptPathXml = "";

            bool FirstDeptExpanded = false;

            int inttmpCount = 0;
            long lngCurrNodeID = 0; //当前环节ID
            string strCurrNodeName = ""; //当前环节的名称

            e_FMNodeType lngCurrNodeType = e_FMNodeType.fmNormal;   //生成树时当前环节的类别
            e_ReceiveActorType lngCurrReceiveType; //当前接收者类别


            tv.Nodes.Clear();
            XmlTextReader tr = new XmlTextReader(new StringReader(strXml));

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<Receivers></Receivers>");

            bool blnContinue = true;
            while (tr.Read() && blnContinue)
            {
                if (tr.NodeType == XmlNodeType.Element)
                {
                    //只处理元素部分即可，因为返回的XML的内容集中在元素的属性中,并按顺序添加TreeView节点
                    switch (tr.Name)
                    {
                        case "NextReceivers":
                            //添加父节点（根节点）　２００５－０６－１７
                            ParentNode = new CustomTreeNode();
                            if (strType == "Assist")
                            {
                                ParentNode.Text = "人员选择";
                                if (intSpecRightType == e_SpecRightType.esrtNormal)
                                {
                                    ParentNode.Text = "协办人员选择";
                                }
                                ParentNode.ImageUrl = "../Images/Flow/27.bmp";
                                ParentNode.Value = "Assists_" + lngCurrNodeID.ToString();
                            }
                            else
                            {
                                if (intSpecRightType == e_SpecRightType.esrtNormal)
                                {
                                    ParentNode.Text = "阅知人员选择";
                                }
                                ParentNode.ImageUrl = "../Images/Flow/27.bmp";
                                ParentNode.Value = "Readers_" + lngCurrNodeID.ToString();
                            }

                            ParentNode.Expanded = true;
                            tv.Nodes.Add(ParentNode);

                            break;
                        case "Nodes":

                            break;
                        case "Node":
                            lngtmpID = long.Parse(tr.GetAttribute("ID"));
                            lngCurrNodeID = lngtmpID;
                            strNodeList = strNodeList + lngCurrNodeID.ToString() + ",";
                            strtmpName = tr.GetAttribute("Name");
                            lngCurrNodeType = (e_FMNodeType)(int.Parse(tr.GetAttribute("NodeType")));
                            //如果环节没有名称，则生成缺省值
                            if (strtmpName.Trim() == "")
                                strtmpName = "环节" + lngtmpID.ToString();
                            strCurrNodeName = strtmpName;
                            ///判断当前环节是否有主办，如果任一个环节没有主办，均不能正常执行
                            inttmpCount = int.Parse(tr.GetAttribute("WorkerCount"));
                            if (inttmpCount == 0)
                            {
                                UIGlobal.MsgBox(this, "在环节：" + StringTool.ParseForResponse(strtmpName) + " 没有发现可以进行后续处理的人员，请与流程设置人员联系！");
                                Response.Write("<script>parent.fraaddNew.cols='0,100%,0';</script>");
                                UIGlobal.SelfClose(this);
                                Response.End();

                            }
                            ///添加节点
                            RootNode = new CustomTreeNode();
                            RootNode.Text = "环节：" + strtmpName;
                            RootNode.ImageUrl = "../Images/Flow/118.bmp";
                            RootNode.Value = "Node_" + lngtmpID.ToString();
                            RootNode.Expanded = true;
                            treeNodeIndex++;
                            ParentNode.ChildNodes.Add(RootNode);

                            if (!sbPersonNodeJson.ToString().EndsWith("["))
                            {
                                sbPersonNodeJson.Append(",");
                            }
                            sbPersonNodeJson.Append("{");

                            sbPersonNodeJson.AppendFormat("key:\"{0}\",", "tvReceivert" + treeNodeIndex.ToString());

                            sbPersonNodeJson.AppendFormat("userid:\"{0}\",", lngtmpID);
                            sbPersonNodeJson.AppendFormat("nodeid:\"{0}\",", lngtmpID.ToString());
                            sbPersonNodeJson.AppendFormat("nodetype:\"{0}\",", ((int)lngCurrNodeType).ToString());
                            sbPersonNodeJson.AppendFormat("name:\"{0}\",", strtmpName);
                            sbPersonNodeJson.AppendFormat("nodedata:\"{0}\",", strCurrNodeName);
                            sbPersonNodeJson.AppendFormat("roleid:\"{0}\",", "Worker_");
                            {
                                sbPersonNodeJson.AppendFormat("rectype:\"{0}\",", "");
                                sbPersonNodeJson.AppendFormat("actorid:\"{0}\"", lngtmpID.ToString());
                            }
                            sbPersonNodeJson.Append("}");


                            break;
                        case "Workers":

                            break;
                        case "Worker":

                            break;
                        case "Readers":
                            //添加节点

                            break;
                        case "Reader":
                            lngtmpID = long.Parse(tr.GetAttribute("ID"));
                            strtmpName = tr.GetAttribute("Name");
                            if (strtmpName.Trim() == "")
                                strtmpName = "人员ID(" + lngtmpID.ToString() + ")";


                            lngCurrReceiveType = (e_ReceiveActorType)(int.Parse(tr.GetAttribute("ReceiveType")));

                            if (lngCurrNodeType == e_FMNodeType.fmOrgNode || lngCurrNodeType == e_FMNodeType.fmDisperse)
                            {
                                //机构环节或发散环节只选择组

                                lngCurrReceiveType = (e_ReceiveActorType)(int.Parse(tr.GetAttribute("ReceiveType")));
                                strDeptPathXml = tr.GetAttribute("DeptPathXml");
                                if (strDeptPathXml == null) strDeptPathXml = "";
                                if (strDeptPathXml.Length > 0)
                                {
                                    //存在部门树节点的情况

                                    //添加节点
                                    if (strType == "Reader")
                                    {
                                        AddActorNodeGroup(ref RootNode, strtmpName, lngtmpID, "Reader", lngCurrNodeID, strCurrNodeName, strDeptPathXml, lngCurrNodeType, lngCurrReceiveType);
                                    }
                                    else
                                    {
                                        AddActorNodeGroup(ref RootNode, strtmpName, lngtmpID, "Assist", lngCurrNodeID, strCurrNodeName, strDeptPathXml, lngCurrNodeType, lngCurrReceiveType);
                                    }
                                }
                                else
                                {
                                    node = new CustomTreeNode();
                                    node.Text = strtmpName;
                                    switch (lngCurrReceiveType)
                                    {
                                        case e_ReceiveActorType.eratPerson:
                                            node.ImageUrl = "../Images/Flow/61.bmp";
                                            break;
                                        case e_ReceiveActorType.eratDept:
                                            node.ImageUrl = "../Images/Flow/27.bmp";
                                            break;
                                        case e_ReceiveActorType.eratCond:
                                            node.ImageUrl = "../Images/Flow/3.bmp";
                                            break;
                                        case e_ReceiveActorType.eratExtend:
                                            node.ImageUrl = "../Images/Flow/3.bmp";
                                            break;

                                    }
                                    if (strType == "Reader")
                                    {
                                        node.Value = "Reader_" + lngtmpID.ToString() + "_" + ((int)lngCurrReceiveType).ToString();
                                    }
                                    else
                                    {
                                        node.Value = "Assist_" + lngtmpID.ToString() + "_" + ((int)lngCurrReceiveType).ToString();
                                    }
                                    node.NodeBllType = lngCurrNodeID.ToString() + "_" + ((int)lngCurrNodeType).ToString();  //环节的编号_环节类别
                                    node.NodeData = strCurrNodeName;  //环节的名称
                                    if (FirstDeptExpanded == false)
                                    {
                                        node.Expanded = true;
                                        //FirstDeptExpanded=true;
                                    }
                                    treeNodeIndex++;
                                    RootNode.ChildNodes.Add(node);

                                    //sbMastPerson.Append("arrMastPerson[" + iMastPerson + "] = new Array(\"" + node.Value + "|" + node.NodeBllType + "|" + node.NodeData + "|" + strtmpName + "\", \"" + strCurrNodeName + "----" + strtmpName + "/用户组" + "\");\n");
                                    String strPersonInfo = " new Array(\"" + node.Value + "|" + node.NodeBllType + "|" + node.NodeData + "|" + strtmpName + "\", \"" + strCurrNodeName + "----" + strtmpName + "/用户组" + "\");\n";
                                    dictMastPerson.Add(new KeyValuePair<String, String>(strtmpName, strPersonInfo));
                                    //{ name: "深发展A", code: "000001",spell:"sfza",id:'1' }
                                    string UserID = node.Value;

                                    if (!sbPersonNodeJson.ToString().EndsWith("["))
                                    {
                                        sbPersonNodeJson.Append(",");
                                    }
                                    sbPersonNodeJson.Append("{");

                                    sbPersonNodeJson.AppendFormat("key:\"{0}\",", "tvReceivert" + treeNodeIndex.ToString());

                                    sbPersonNodeJson.AppendFormat("userid:\"{0}\",", UserID);
                                    sbPersonNodeJson.AppendFormat("nodeid:\"{0}\",", lngCurrNodeID.ToString());
                                    sbPersonNodeJson.AppendFormat("nodetype:\"{0}\",", ((int)lngCurrNodeType).ToString());
                                    sbPersonNodeJson.AppendFormat("name:\"{0}\",", node.Text);
                                    sbPersonNodeJson.AppendFormat("nodedata:\"{0}\",", node.NodeData);
                                    sbPersonNodeJson.AppendFormat("roleid:\"{0}\",", "Worker_");
                                    int typepoint = UserID.Substring(7).IndexOf("_");
                                    sbPersonNodeJson.AppendFormat("typepoint:\"{0}\",", typepoint);
                                    if (typepoint > 0)
                                    {
                                        sbPersonNodeJson.AppendFormat("rectype:\"{0}\",", UserID.Substring(7).Substring(typepoint + 1));
                                        sbPersonNodeJson.AppendFormat("actorid:\"{0}\"", UserID.Substring(7).Substring(0, typepoint));
                                    }
                                    else
                                    {
                                        sbPersonNodeJson.AppendFormat("rectype:\"{0}\",", "");
                                        sbPersonNodeJson.AppendFormat("actorid:\"{0}\"", UserID.Substring(7));
                                    }
                                    sbPersonNodeJson.Append("}");


                                    iMastPerson++;
                                }

                            }
                            else
                            {
                                strDeptPathXml = tr.GetAttribute("DeptPathXml");
                                if (strDeptPathXml == null) strDeptPathXml = "";
                                //添加节点

                                if (lngCurrReceiveType == e_ReceiveActorType.eratPerson)
                                {
                                    AddActorNode(ref RootNode, strtmpName, lngtmpID, strType, lngCurrNodeID, strCurrNodeName, strDeptPathXml, lngCurrNodeType);
                                }
                                else
                                {
                                    if (strDeptPathXml.Length > 0)
                                    {
                                        AddActorNodeGroup(ref RootNode, strtmpName, lngtmpID, strType, lngCurrNodeID, strCurrNodeName, strDeptPathXml, lngCurrNodeType, lngCurrReceiveType);
                                    }
                                    else
                                    {
                                        node = new CustomTreeNode();
                                        node.Text = strtmpName;
                                        switch (lngCurrReceiveType)
                                        {
                                            case e_ReceiveActorType.eratPerson:
                                                node.ImageUrl = "../Images/Flow/56.bmp";
                                                break;
                                            case e_ReceiveActorType.eratDept:
                                                node.ImageUrl = "../Images/Flow/27.bmp";
                                                break;
                                            case e_ReceiveActorType.eratCond:
                                                node.ImageUrl = "../Images/Flow/3.bmp";
                                                break;
                                            case e_ReceiveActorType.eratExtend:
                                                node.ImageUrl = "../Images/Flow/3.bmp";
                                                break;

                                        }
                                        node.Value = strType + "_" + lngtmpID.ToString() + "_" + ((int)lngCurrReceiveType).ToString();
                                        node.NodeBllType = lngCurrNodeID.ToString() + "_" + ((int)lngCurrNodeType).ToString();  //环节的编号_环节类别
                                        node.NodeData = strCurrNodeName;  //环节的名称
                                        if (FirstDeptExpanded == false)
                                        {
                                            node.Expanded = true;
                                            //FirstDeptExpanded=true;
                                        }
                                        treeNodeIndex++;
                                        RootNode.ChildNodes.Add(node);
                                    }
                                }


                            }
                            if (FirstDeptExpanded == false)
                            {
                                foreach (CustomTreeNode n in RootNode.ChildNodes)
                                {
                                    n.Expanded = true;
                                }
                                FirstDeptExpanded = true;
                            }
                            break;
                    }
                }

            }


            tr.Close();
        }
        private void AddActorNodeGroup(ref CustomTreeNode ParentNode, string strName,
            long lngID, string strType, long lngNodeID, string strNodeName, string strDeptPath, e_FMNodeType lngCurrNodeType, e_ReceiveActorType lngCurrReceiveType)
        {

            //*  根据 deptpath描述创建部门节点，如果存在则不创建

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(strDeptPath);
            XmlNodeList xmlNodes = xmlDoc.DocumentElement.SelectNodes("Dept");


            long lngDeptID = 0;
            string strDeptName = "";

            CustomTreeNode deptnode = new CustomTreeNode();
            CustomTreeNode pnode = ParentNode;
            CustomTreeNode node;
            //bool blnHasExpanded = false;

            string strDeptNodeID = "";


            //foreach(XmlNode xmlNode in xmlNodes)
            if (xmlNodes.Count > 1)
            {
                //最后一个部门路径的节点是自己，所以不需要添加路径
                for (int i = 0; i < xmlNodes.Count - 1; i++)
                {
                    //判断是否存在
                    //node = (CustomTreeNode)tvReceiver.FindControl("Dept_" + lngNodeID.ToString() + "_" + strType + "_" + lngDeptID.ToString());


                    XmlNode xmlNode = xmlNodes.Item(i);
                    lngDeptID = long.Parse(((XmlElement)xmlNode).GetAttribute("ID"));
                    strDeptName = ((XmlElement)xmlNode).GetAttribute("DeptName");
                    strDeptNodeID = "Dept_" + lngNodeID.ToString() + "_" + strType + "_" + lngDeptID.ToString();

                    deptnode = FindNode(pnode, strDeptNodeID);

                    if (deptnode.Value == "-2")
                    {
                        deptnode = new CustomTreeNode();
                        deptnode.Text = strDeptName;

                        deptnode.Value = strDeptNodeID;
                        deptnode.ImageUrl = "../Images/Flow/2.bmp";

                        //node = tvReceiver.GetNodeFromIndex("1");
                        treeNodeIndex++;
                        pnode.ChildNodes.Add(deptnode);
                        //0000017

                        if (!sbPersonNodeJson.ToString().EndsWith("["))
                        {
                            sbPersonNodeJson.Append(",");
                        }
                        sbPersonNodeJson.Append("{");

                        sbPersonNodeJson.AppendFormat("key:\"{0}\",", "tvReceivert" + treeNodeIndex.ToString());

                        sbPersonNodeJson.AppendFormat("userid:\"{0}\",", strDeptNodeID);
                        sbPersonNodeJson.AppendFormat("nodeid:\"{0}\",", strDeptNodeID.ToString());
                        sbPersonNodeJson.AppendFormat("nodetype:\"{0}\",", ((int)lngCurrNodeType).ToString());
                        sbPersonNodeJson.AppendFormat("name:\"{0}\",", strDeptName);
                        sbPersonNodeJson.AppendFormat("nodedata:\"{0}\",", strNodeName);
                        sbPersonNodeJson.AppendFormat("roleid:\"{0}\",", "Worker_");
                        int typepoint = strDeptNodeID.Substring(7).IndexOf("_");
                        sbPersonNodeJson.AppendFormat("typepoint:\"{0}\",", typepoint);
                        if (typepoint > 0)
                        {
                            sbPersonNodeJson.AppendFormat("rectype:\"{0}\",", strDeptNodeID.Substring(7).Substring(typepoint + 1));
                            sbPersonNodeJson.AppendFormat("actorid:\"{0}\"", strDeptNodeID.Substring(7).Substring(0, typepoint));
                        }
                        else
                        {
                            sbPersonNodeJson.AppendFormat("rectype:\"{0}\",", "");
                            sbPersonNodeJson.AppendFormat("actorid:\"{0}\"", strDeptNodeID.Substring(7));
                        }
                        sbPersonNodeJson.Append("}");

                    }


                    pnode = deptnode;
                }

            }

            node = new CustomTreeNode();
            node.Text = strName;
            node.ImageUrl = "../Images/Flow/27.bmp";
            node.NodeBllType = lngNodeID.ToString() + "_" + ((int)lngCurrNodeType).ToString();  //环节的编号_环节类别
            node.NodeData = strNodeName;      //环节的名称
            string tmpKey = "";
            if (strType == "Reader")
            {
                node.Value = "Reader_" + lngID.ToString() + "_" + ((int)lngCurrReceiveType).ToString();

                //sbReadPerson.Append("arrReadPerson[" + iReadPerson + "] = new Array(\"" + node.Value + "|" + node.NodeBllType + "|" + node.NodeData + "|" + strName + "\", \"" + strNodeName + "----" + strName + "/部门" + "\");\n");
                String strPersonInfo = "  new Array(\"" + node.Value + "|" + node.NodeBllType + "|" + node.NodeData + "|" + strName + "\", \"" + strNodeName + "----" + strName + "/部门" + "\");\n";
                dictReadPerson.Add(new KeyValuePair<String, String>(strName, strPersonInfo));

                iReadPerson++;
                tmpKey = "Reader_";
            }
            else if (strType == "Worker")
            {
                node.Value = "Worker_" + lngID.ToString() + "_" + ((int)lngCurrReceiveType).ToString();

                //sbMastPerson.Append("arrMastPerson[" + iMastPerson + "] = new Array(\"" + node.Value + "|" + node.NodeBllType + "|" + node.NodeData + "|" + strName + "\", \"" + strNodeName + "----" + strName + "/部门" + "\");\n");
                String strPersonInfo = " new Array(\"" + node.Value + "|" + node.NodeBllType + "|" + node.NodeData + "|" + strName + "\", \"" + strNodeName + "----" + strName + "/部门" + "\");\n";
                dictMastPerson.Add(new KeyValuePair<String, String>(strName, strPersonInfo));
                iMastPerson++;
                tmpKey = "Worker";
            }
            else
            {
                //协办
                node.Value = "Assist_" + lngID.ToString() + "_" + ((int)lngCurrReceiveType).ToString();
                tmpKey = "tvAssistort";
            }

            string UserID = node.Value;
            int lngCurrNodeID = treeNodeIndex;
            if (!sbPersonNodeJson.ToString().EndsWith("["))
            {
                sbPersonNodeJson.Append(",");
            }
            sbPersonNodeJson.Append("{");

            sbPersonNodeJson.AppendFormat("key:\"{0}\",", tmpKey + treeNodeIndex.ToString());

            sbPersonNodeJson.AppendFormat("userid:\"{0}\",", UserID);
            sbPersonNodeJson.AppendFormat("nodeid:\"{0}\",", lngCurrNodeID.ToString());
            sbPersonNodeJson.AppendFormat("nodetype:\"{0}\",", ((int)lngCurrNodeType).ToString());
            sbPersonNodeJson.AppendFormat("name:\"{0}\",", node.Text);
            sbPersonNodeJson.AppendFormat("nodedata:\"{0}\",", strNodeName);
            sbPersonNodeJson.AppendFormat("roleid:\"{0}\",", strType + "_");
            int typepoint2 = UserID.Substring(7).IndexOf("_");
            sbPersonNodeJson.AppendFormat("typepoint:\"{0}\",", typepoint2);
            if (typepoint2 > 0)
            {
                sbPersonNodeJson.AppendFormat("rectype:\"{0}\",", UserID.Substring(7).Substring(typepoint2 + 1));
                sbPersonNodeJson.AppendFormat("actorid:\"{0}\"", UserID.Substring(7).Substring(0, typepoint2));
            }
            else
            {
                sbPersonNodeJson.AppendFormat("rectype:\"{0}\",", "");
                sbPersonNodeJson.AppendFormat("actorid:\"{0}\"", UserID.Substring(7));
            }
            sbPersonNodeJson.Append("}");

            node.Text = node.Text
                + String.Format("<span style=\"display:none\">{0}</span>", node.Value);

            //node.Expanded = true;
            //ParentNode.Nodes.Add(node);
            treeNodeIndex++;
            pnode.ChildNodes.Add(node);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ParentNode"></param>
        /// <param name="strName"></param>
        /// <param name="lngID"></param>
        /// <param name="strType"></param>
        /// <param name="lngNodeID"></param>
        /// <param name="strNodeName"></param>
        /// <param name="strDeptPath"></param>
        /// <param name="lngCurrNodeType"></param>
        /// <param name="inum"></param>
        private CustomTreeNode AddActorNode(ref CustomTreeNode ParentNode, string strName,
            long lngID, string strType, long lngNodeID, string strNodeName, string strDeptPath, e_FMNodeType lngCurrNodeType)
        {

            #region 如果获取人员时没有进行排序，则strDeptPath此时为null 余向前 2014-03-05
            if (string.IsNullOrEmpty(strDeptPath))
            {
                strDeptPath = "<Depts></Depts>";
            }
            #endregion

            //*  根据 deptpath描述创建部门节点，如果存在则不创建

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(strDeptPath);
            XmlNodeList xmlNodes = xmlDoc.DocumentElement.SelectNodes("Dept");


            long lngDeptID = 0;
            string strDeptName = "";

            CustomTreeNode deptnode = new CustomTreeNode();
            CustomTreeNode pnode = ParentNode;
            CustomTreeNode node;
            //bool blnHasExpanded = false;

            string strDeptNodeID = "";


            foreach (XmlNode xmlNode in xmlNodes)
            {
                //判断是否存在
                //node = (CustomTreeNode)tvReceiver.FindControl("Dept_" + lngNodeID.ToString() + "_" + strType + "_" + lngDeptID.ToString());


                lngDeptID = long.Parse(((XmlElement)xmlNode).GetAttribute("ID"));
                strDeptName = ((XmlElement)xmlNode).GetAttribute("DeptName");
                strDeptNodeID = "Dept_" + lngNodeID.ToString() + "_" + strType + "_" + lngDeptID.ToString();

                deptnode = FindNode(pnode, strDeptNodeID);

                if (deptnode.Value == "-2")
                {
                    deptnode = new CustomTreeNode();
                    deptnode.Text = strDeptName;

                    deptnode.Value = strDeptNodeID;
                    deptnode.ImageUrl = "../Images/Flow/2.bmp";

                    //node = tvReceiver.GetNodeFromIndex("1");

                    treeNodeIndex++;
                    pnode.ChildNodes.Add(deptnode);

                    if (!sbPersonNodeJson.ToString().EndsWith("["))
                    {
                        sbPersonNodeJson.Append(",");
                    }
                    sbPersonNodeJson.Append("{");

                    sbPersonNodeJson.AppendFormat("key:\"{0}\",", "tvReceivert" + treeNodeIndex.ToString());

                    sbPersonNodeJson.AppendFormat("userid:\"{0}\",", strDeptNodeID);
                    sbPersonNodeJson.AppendFormat("nodeid:\"{0}\",", strDeptNodeID.ToString());
                    sbPersonNodeJson.AppendFormat("nodetype:\"{0}\",", ((int)lngCurrNodeType).ToString());
                    sbPersonNodeJson.AppendFormat("name:\"{0}\",", strDeptName);
                    sbPersonNodeJson.AppendFormat("nodedata:\"{0}\",", strNodeName);
                    sbPersonNodeJson.AppendFormat("roleid:\"{0}\",", strType);
                    int typepoint = strDeptNodeID.Substring(7).IndexOf("_");
                    sbPersonNodeJson.AppendFormat("typepoint:\"{0}\",", typepoint);
                    if (typepoint > 0)
                    {
                        sbPersonNodeJson.AppendFormat("rectype:\"{0}\",", strDeptNodeID.Substring(7).Substring(typepoint + 1));
                        sbPersonNodeJson.AppendFormat("actorid:\"{0}\"", strDeptNodeID.Substring(7).Substring(0, typepoint));
                    }
                    else
                    {
                        sbPersonNodeJson.AppendFormat("rectype:\"{0}\",", "");
                        sbPersonNodeJson.AppendFormat("actorid:\"{0}\"", strDeptNodeID.Substring(7));
                    }
                    sbPersonNodeJson.Append("}");

                }

                pnode = deptnode;

            }

            node = new CustomTreeNode();
            node.Text = strName;
            node.NodeBllType = lngNodeID.ToString() + "_" + ((int)lngCurrNodeType).ToString();  //环节的编号_环节类别
            node.NodeData = strNodeName;      //环节的名称
            string tmpKey = "";

            if (strType == "Reader")
            {
                node.ImageUrl = "../Images/Flow/61.bmp";
                node.Value = "Reader_" + lngID.ToString();

                //sbReadPerson.Append("arrReadPerson[" + iReadPerson + "] = new Array(\"" + node.Value + "|" + node.NodeBllType + "|" + node.NodeData + "|" + strName + "\", \"" + strNodeName + "----" + strName + "/" + strDeptName + "\");\n");
                String strPersonInfo = " new Array(\"" + node.Value + "|" + node.NodeBllType + "|" + node.NodeData + "|" + strName + "\", \"" + strNodeName + "----" + strName + "/" + strDeptName + "\");\n";
                dictReadPerson.Add(new KeyValuePair<String, String>(strName, strPersonInfo));
                iReadPerson++;

                tmpKey = "Reader_";
            }
            else if (strType == "Worker")
            {
                node.ImageUrl = "../Images/Flow/56.bmp";
                node.Value = "Worker_" + lngID.ToString();

                //sbMastPerson.Append("arrMastPerson[" + iMastPerson + "] = new Array(\"" + node.Value + "|" + node.NodeBllType + "|" + node.NodeData + "|" + strName + "\", \"" + strNodeName + "----" + strName + "/" + strDeptName + "\");\n");
                String strPersonInfo = " new Array(\"" + node.Value + "|" + node.NodeBllType + "|" + node.NodeData + "|" + strName + "\", \"" + strNodeName + "----" + strName + "/部门" + "\");\n";
                dictMastPerson.Add(new KeyValuePair<String, String>(strName, strPersonInfo));


                iMastPerson++;

                tmpKey = "Worker";
            }
            else
            {
                //协办
                node.ImageUrl = "../Images/Flow/61.bmp";
                node.Value = "Assist_" + lngID.ToString();

                tmpKey = "tvAssistort";
            }

            //node.Expanded = true;
            //ParentNode.Nodes.Add(node);

            string UserID = node.Value;
            //int lngCurrNodeID = lngNodeID;
            if (!sbPersonNodeJson.ToString().EndsWith("["))
            {
                sbPersonNodeJson.Append(",");
            }
            sbPersonNodeJson.Append("{");

            sbPersonNodeJson.AppendFormat("key:\"{0}\",", tmpKey + treeNodeIndex.ToString());
            sbPersonNodeJson.AppendFormat("userid:\"{0}\",", UserID);
            sbPersonNodeJson.AppendFormat("nodeid:\"{0}\",", lngNodeID);
            sbPersonNodeJson.AppendFormat("nodetype:\"{0}\",", ((int)lngCurrNodeType).ToString());
            sbPersonNodeJson.AppendFormat("name:\"{0}\",", node.Text);
            sbPersonNodeJson.AppendFormat("nodedata:\"{0}\",", strNodeName);
            sbPersonNodeJson.AppendFormat("roleid:\"{0}\",", strType + "_");
            int typepoint2 = UserID.Substring(7).IndexOf("_");
            sbPersonNodeJson.AppendFormat("typepoint:\"{0}\",", typepoint2);
            if (typepoint2 > 0)
            {
                sbPersonNodeJson.AppendFormat("rectype:\"{0}\",", UserID.Substring(7).Substring(typepoint2 + 1));
                sbPersonNodeJson.AppendFormat("actorid:\"{0}\"", UserID.Substring(7).Substring(0, typepoint2));
            }
            else
            {
                sbPersonNodeJson.AppendFormat("rectype:\"{0}\",", "");
                sbPersonNodeJson.AppendFormat("actorid:\"{0}\"", UserID.Substring(7));
            }
            sbPersonNodeJson.Append("}");


            node.Text = node.Text
                + String.Format("<span style=\"display:none\">{0}</span>", node.Value);

            treeNodeIndex++;

            pnode.ChildNodes.Add(node);
            return node;
        }

        private CustomTreeNode FindNode(CustomTreeNode ParentNode, string strID)
        {
            CustomTreeNode node = new CustomTreeNode();
            node.Value = "-2";
            if (ParentNode.ChildNodes.Count > 0)
            {
                foreach (CustomTreeNode n in ParentNode.ChildNodes)
                {
                    if (n.Value == strID)
                    {
                        return n;
                    }
                    else
                    {
                        if (n.Value.Substring(0, 5) != "Worker_")
                        {
                            node = FindNode(n, strID);
                            if (node.Value != "-2")
                            {
                                //表示找到
                                return node;
                            }
                        }
                    }
                }
            }
            return node;
        }



        /// <summary>
        /// 按拼音排序部门和人员
        /// </summary>
        /// <param name="treeNode"></param>
        private void ArrangedInAlphabeticalOrder(TreeNode treeNode)
        {
            System.Collections.Generic.List<TreeNode> listTreeNode = new System.Collections.Generic.List<TreeNode>();
            foreach (TreeNode item in treeNode.ChildNodes)
            {
                listTreeNode.Add(item);
            }

            listTreeNode.Sort(new TreeNodeArrangedInAlphabeticalOrderComparer());

            treeNode.ChildNodes.Clear();

            for (int idx = 0; idx < listTreeNode.Count; idx++)
            {
                TreeNode _childNode = listTreeNode[idx];
                if (_childNode.ChildNodes.Count > 0)
                    ArrangedInAlphabeticalOrder(_childNode);

                treeNode.ChildNodes.Add(_childNode);
            }

        }


        #region Web Form Designer generated code
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN：该调用是 ASP.NET Web 窗体设计器所必需的。
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




    }



    /// <summary>
    /// 按字母顺序排序部门和人员
    /// </summary>
    public class TreeNodeArrangedInAlphabeticalOrderComparer : System.Collections.Generic.IComparer<TreeNode>
    {
        public TreeNodeArrangedInAlphabeticalOrderComparer() { }

        #region IComparer<TreeNode> 成员

        public int Compare(TreeNode x, TreeNode y)
        {
            return x.Text.Trim().CompareTo(y.Text.Trim());
        }

        #endregion
    }


    /// <summary>
    /// 按字母顺序排序部门和人员
    /// </summary>
    public class KeyValueArrangedInAlphabeticalOrderComparer : System.Collections.Generic.IComparer<KeyValuePair<String, String>>
    {
        public KeyValueArrangedInAlphabeticalOrderComparer() { }

        #region IComparer<TreeNode> 成员

        public int Compare(KeyValuePair<String, String> x, KeyValuePair<String, String> y)
        {
            return x.Key.Trim().CompareTo(y.Key.Trim());
        }

        #endregion
    }


}
