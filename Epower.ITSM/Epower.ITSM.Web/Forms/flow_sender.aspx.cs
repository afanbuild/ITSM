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
    /// flow_sender ��ժҪ˵����
    /// </summary>
    public partial class flow_sender : BasePage
    {





        protected long lngMessageID = 0;
        long lngFlowModelID = 0;
        long lngActionID = -1;
        string strFormXMLValue = "";
        string strFormDefineXml = "";  //��ָ��������XML��

        string strMessage = "";  //�����ֱ�ӷ��͵Ļ��������ύ�����ʾ��Ϣ �����Ϊ�����ύ����ķ�ʽ��ʾ���û�
        string strAutoMsg = "";

        public string strNodeList = "";   //�����б�
        public string strLinkNodeID = "";    //��һ���ӻ��ڵ�ģ��ID
        public string strLinkNodeType = "";  //��һ���ӻ��ڵ�ģ������


        protected string strMasterNodeID = "";  //����ʱ��ѡ����ģ��ID

        protected string strMasterNodeName = ""; //����ʱ����ѡ���ģ������

        protected e_SpecRightType intSpecRightType = e_SpecRightType.esrtNormal;

        protected long lngReDoUserID = 0;    //����ʱҪ�ύ�ĵ��û����

        //���⴦����𡡣����ӡ���ֹ����ת��
        string strJumpNodeModelID = "0";                 //��ת ���� ���� ʱ�ġ�ѡ��Ҫ��ת���Ļ���ģ�ͣɣ�

        string strReDoNodeModelType = "30";            //����ʱ�������Ļ���ģ��ID


        protected string strSelType = "0";     //ѡ��ҳ��ķ�ʽ 1 ʱΪ����ʱʹ��


        protected string strSpecRightType = "10";



        protected string sIniXml = "";

        protected string sblnDeleteMaster = "1";    //Ϊ0ʱ���Բ�����ɾ������  ��ֻ��һ������ �������Զ���ӵ������

        //�����ģ��б�ʹ��
        StringBuilder sbMastPerson = new StringBuilder();
        int iMastPerson = 0;
        StringBuilder sbReadPerson = new StringBuilder();
        int iReadPerson = 0;

        /// <summary>
        /// ���б���ʾ��Աʱ����Ա��Ϣ����(����)
        /// </summary>
        System.Collections.Generic.List<KeyValuePair<String, String>> dictMastPerson = new System.Collections.Generic.List<KeyValuePair<String, String>>();
        /// <summary>
        /// ���б���ʾ��Աʱ����Ա��Ϣ����(������)
        /// </summary>
        System.Collections.Generic.List<KeyValuePair<String, String>> dictReadPerson = new System.Collections.Generic.List<KeyValuePair<String, String>>();


        protected string lngFlowID = "0";
        protected string lngNodeID = "0";
        private int treeNodeIndex = -1;
        StringBuilder sbPersonNodeJson = new StringBuilder();

        protected void Page_Load(object sender, System.EventArgs e)
        {
            // �ڴ˴������û������Գ�ʼ��ҳ��
            CtrTitle1.Title = "������Աѡ��";

            //��ֹ�û�ͨ��IE���˰�Ŧ�ظ��ύ
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
                //����ʱ
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
                CtrTitle1.Title = "�´�����Աѡ��";
            }

            if (intSpecRightType == e_SpecRightType.esrtFreeTakeOver || intSpecRightType == e_SpecRightType.esrtTakeOver)
            {
                CtrTitle1.Title = "������Աѡ��";
            }

            if (intSpecRightType == e_SpecRightType.esrtTransmit)
            {
                CtrTitle1.Title = "������Աѡ��";
            }

            if (intSpecRightType == e_SpecRightType.esrtAssist)
            {
                CtrTitle1.Title = "Э����Աѡ��";
            }

            if (intSpecRightType == e_SpecRightType.esrtCommunic)
            {
                CtrTitle1.Title = "��ͨ��Աѡ��";
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
                    //����ʱ�Զ�����

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
            long lngCurrNodeID = 0; //��ǰ����ID
            string strCurrNodeName = ""; //��ǰ���ڵ�����
            long lngFirstWorkerID = 0;  //�����Զ����͵�����ID
            string sFirstWorkerName = "";  //�����Զ����͵���������
            bool blnCanSend = true;   //�Ƿ����ֱ�ӷ��ͣ� ���ֻ��һ�����ڶ��ң�ÿ������ֻ��һ�����죬����û����֪��Ա�������ֱ�ӷ���
            int nodeCnt = 0;
            int workerCnt = 0;

            long lngUserID = (long)Session["UserID"];

            bool FirstDeptExpanded = false;


            long lngMasterPath = 0;   //����·��
            long lngCurrPath = 0;    //��ǰ·��
            long lngLinkNodeID = 0;
            long lngLinkNodeType = 0;
            e_FMNodeType lngCurrNodeType = e_FMNodeType.fmNormal;   //������ʱ��ǰ���ڵ����
            e_ReceiveActorType lngCurrReceiveType = e_ReceiveActorType.eratNone; //��ǰ���������
            //e_ReceiveActorType lngCurrReceiveType; //��ǰ���������

            string strUnionLinkEnd = "False";

            int iReaders = 0;

            //2008-02-05 ��ָ����Ա�ڵ���������¿���ֱ�ӷ��Ͳ������ڽ�����ѡ��
            string blnSendToDefine = "false";
            string strSendDefineXml = "";



            CustomTreeNode RootNode = new CustomTreeNode();
            CustomTreeNode ParentNode = new CustomTreeNode();
            CustomTreeNode node;

            Message objMsg = new Message();

            // **********  ��ָ�������� ����������ĸ��ҳ�� ***********
            if (intSpecRightType == e_SpecRightType.esrtReDoBack)
            {
                //����ʱֱ�ӷ��� ������
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
                    //ֻ����Ԫ�ز��ּ��ɣ���Ϊ���ص�XML�����ݼ�����Ԫ�ص�������,����˳�����TreeView�ڵ�
                    switch (tr.Name)
                    {
                        case "NextReceivers":
                            //�ж�·��
                            if (tr.GetAttribute("HasPath") == "None")
                            {
                                blnCanSend = false;
                                tr.Close();
                                UIGlobal.MsgBox(this, "û�з��ֿ����������̵�·������������������Ա��ϵ��");
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
                                blnContinue = false;  //�˳�whileѭ��
                            }

                            ///�����������ѡ����Ա��ֱ�ӷ��ͳ�ȥ
                            ///		1�������һ��������Ǻ�����������·����ƥ�䣬
                            ///		2����һ���������ǽ���
                            ///		3����һ�������Ϊ�����������������нӽ������ڣ���һ��ֱ�����ӣ�
                            if ((lngLinkNodeType == (long)e_FMNodeType.fmUnite && lngCurrPath != lngMasterPath)
                                || (lngLinkNodeType == (long)e_FMNodeType.fmEnd)
                                || (lngLinkNodeType == (long)e_FMNodeType.fmUnite && lngCurrPath == lngMasterPath && strUnionLinkEnd == "True"))
                            {
                                blnCanSend = true;

                                blnContinue = false;  //�˳�whileѭ��

                            }
                            else
                            {
                                //��Ӹ��ڵ㣨���ڵ㣩����������������������
                                ParentNode = new CustomTreeNode();
                                if (intSpecRightType == e_SpecRightType.esrtNormal)
                                {
                                    ParentNode.Text = "������Աѡ��(��ѡ)";
                                }
                                else
                                {
                                    ParentNode.Text = "��Աѡ��(��ѡ)";
                                }
                                ParentNode.ImageUrl = "../Images/Flow/27.bmp";
                                ParentNode.Value = "Workers_" + lngCurrNodeID.ToString();
                                ParentNode.Expanded = true;
                                treeNodeIndex++;
                                tvReceiver.Nodes.Add(ParentNode);
                            }

                            break;
                        case "Nodes":
                            //�ж���Ա
                            if (tr.GetAttribute("ActorCount").Equals("0"))
                            {
                                blnCanSend = false;
                                UIGlobal.MsgBox(this, "û�з��ֿ��Խ��к����������Ա�� ��������������Ա��ϵ��");
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
                            //�������û�����ƣ�������ȱʡֵ
                            if (strtmpName.Trim() == "")
                                strtmpName = "����" + lngtmpID.ToString();
                            strCurrNodeName = strtmpName;

                            if (lngCurrNodeID.ToString() == strMasterNodeID)
                            {
                                strMasterNodeName = strCurrNodeName;
                            }

                            ///�жϵ�ǰ�����Ƿ������죬�����һ������û�����죬����������ִ��
                            inttmpCount = int.Parse(tr.GetAttribute("WorkerCount"));
                            if (inttmpCount == 0)
                            {
                                blnCanSend = false;
                                UIGlobal.MsgBox(this, "�ڻ��ڣ�" + StringTool.ParseForResponse(strtmpName) + " û�з��ֿ��Խ��к����������Ա����������������Ա��ϵ��");
                                Response.Write("<script>window.opener.parent.close();</script>");
                                UIGlobal.SelfClose(this);
                                Response.End();

                            }
                            ///��ӽڵ�
                            RootNode = new CustomTreeNode();
                            RootNode.Text = "���ڣ�" + strtmpName;
                            RootNode.ImageUrl = "../Images/Flow/118.bmp";
                            RootNode.Value = "Node_" + lngtmpID.ToString();
                            RootNode.Expanded = true;
                            treeNodeIndex++;
                            ParentNode.ChildNodes.Add(RootNode);



                            break;
                        case "Workers":
                            ///��ӽڵ�
                            // 2005-06-17 �޸�: ����/��֪/Э��ֿ���ʾ,���ﲻ��ӽڵ�
                            //							ParentNode = new CustomTreeNode();
                            //							ParentNode.Text =  "����������Ա";
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
                                strtmpName = "��ԱID(" + lngtmpID.ToString() + ")";
                            if (workerCnt == 1)
                            {
                                lngFirstWorkerID = lngtmpID;
                                sFirstWorkerName = strtmpName + "(" + strCurrNodeName + ")";
                            }
                            if (lngCurrNodeType == e_FMNodeType.fmOrgNode || lngCurrNodeType == e_FMNodeType.fmDisperse)
                            {
                                //�������ڻ�ɢ����ֻѡ����

                                lngCurrReceiveType = (e_ReceiveActorType)(int.Parse(tr.GetAttribute("ReceiveType")));
                                strDeptPathXml = tr.GetAttribute("DeptPathXml");
                                if (strDeptPathXml == null) strDeptPathXml = "";
                                if (strDeptPathXml.Length > 0)
                                {
                                    //���ڲ������ڵ�����

                                    //��ӽڵ�
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
                                    node.NodeBllType = lngCurrNodeID.ToString() + "_" + ((int)lngCurrNodeType).ToString();  //���ڵı��_�������
                                    node.NodeData = strCurrNodeName;  //���ڵ�����
                                    if (FirstDeptExpanded == false)
                                    {
                                        node.Expanded = true;
                                        //FirstDeptExpanded=true;
                                    }
                                    treeNodeIndex++;
                                    RootNode.ChildNodes.Add(node);

                                    // sbMastPerson.Append("arrMastPerson[" + iMastPerson + "] = new Array(\"" + node.Value + "|" + node.NodeBllType + "|" + node.NodeData + "|" + strtmpName + "\", \"" + strCurrNodeName + "----" + strtmpName + "/�û���" + "\");\n");
                                    String strPersonInfo = " new Array(\"" + node.Value + "|" + node.NodeBllType + "|" + node.NodeData + "|" + strtmpName + "\", \"" + strCurrNodeName + "----" + strtmpName + "/�û���" + "\");\n";
                                    dictMastPerson.Add(new KeyValuePair<String, String>(strtmpName, strPersonInfo));

                                    //{ name: "�չA", code: "000001",spell:"sfza",id:'1' }
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

                                ///��ӽڵ�
                                //��ӽڵ�
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

                            //�������ֱ�ӷ��ͣ����췢�͵Ľ�����Ա��
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
                                    //�������ڻ�ɢ����ֻѡ����
                                    xmlEle.SetAttribute("ReceiveType", ((int)lngCurrReceiveType).ToString());

                                }
                                else
                                {
                                    //��ͨ�������ý��ս�ɫΪ��
                                    xmlEle.SetAttribute("ReceiveType", "");
                                }
                                strAutoMsg = " " + strtmpName + " ���� [" + strCurrNodeName + "] ���ڵĴ���";

                                xmlDoc.DocumentElement.AppendChild(xmlEle);
                            }
                            break;
                        // 2005-06-17 �޸�: ����/��֪/Э��ֿ���ʾ,���ﲻ��ӽڵ�
                        case "Readers":
                            ///��ӽڵ�
                            //							ParentNode = new CustomTreeNode();
                            //							ParentNode.Text =  "������֪��Ա";
                            //							ParentNode.ImageUrl = "../Images/Flow/41.bmp";
                            //							Parentnode.Value = "Readers_" + lngCurrNodeID.ToString();
                            //							ParentNode.Expanded = true;
                            //							RootNode.Nodes.Add(ParentNode);
                            break;
                        case "Reader":
                            if (strSelType != "1")
                            {
                                // ����ʱ ����֪��ԱҲ����ʾ���� 
                                blnCanSend = false;
                            }
                            iReaders++;

                            break;
                    }
                }

            }


            tr.Close();


            #region Ĭ���۵�[������Ա�Ͳ���]��, ֻ��ʾ��һ�������б� - 2013-07-08 16:22 - ������
            /*     
             * Date: 2013-07-08 16:22
             * summary: Ĭ���۵�[������Ա�Ͳ���]��, ֻ��ʾ��һ�������б�
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


            //���Э��/��֪��
            if (iReaders > 0 && strSelType == "0" && intSpecRightType != e_SpecRightType.esrtTakeOver
                && intSpecRightType != e_SpecRightType.esrtAttemper && intSpecRightType != e_SpecRightType.esrtBackHasDone
                && intSpecRightType != e_SpecRightType.esrtBackHasDoneRedo && intSpecRightType != e_SpecRightType.esrtBackHasDoneFlow
                && intSpecRightType != e_SpecRightType.esrtAssist && intSpecRightType != e_SpecRightType.esrtTransmit
                && intSpecRightType != e_SpecRightType.esrtFreeTakeOver && intSpecRightType != e_SpecRightType.esrtCommunic)
            {
                //ֻ����ͨ���ͺ���תʱ�ſ�����ʾ Э�����֪
                CreateTreeViewContent(tvAssistor, strRet, "Assist");

                CreateTreeViewContent(tvReader, strRet, "Reader");
            }
            else
            {
                //�ı����ĸ߶�
                tvReceiver.Height = new Unit("380px");
                tvAssistor.Visible = false;
                tvReader.Visible = false;
                name1.Visible = false;
                name2.Visible = false;

                if (intSpecRightType == e_SpecRightType.esrtTransmit)  //����
                {
                    lblMast.Text = "����";
                }
                else if (intSpecRightType == e_SpecRightType.esrtAssist)   //Э��
                {
                    lblMast.Text = "Э��";
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
                //ֻ��һ�����졡����Э�����֪����������ء�����ѡ����
                //Session["IniXML"] = xmlDoc.InnerXml;
                sIniXml = xmlDoc.InnerXml;
                sblnDeleteMaster = "0";
                //���ֻһ�����죬�Զ���ӵ��б�
                lstSelected.Items.Add(new ListItem("����:" + sFirstWorkerName, lngFirstWorkerID.ToString()));
                //��Ϊ�����������ʱ����
                if (intSpecRightType != e_SpecRightType.esrtTakeOver && intSpecRightType != e_SpecRightType.esrtFreeTakeOver && intSpecRightType != e_SpecRightType.esrtAttemper
                     && intSpecRightType != e_SpecRightType.esrtCommunic && intSpecRightType != e_SpecRightType.esrtAssist && intSpecRightType != e_SpecRightType.esrtTransmit)
                {
                    tvReceiver.Visible = false;
                    name0.Visible = false;
                }
                this.tvAssistor.Height = new Unit("195px");
                this.tvReader.Height = new Unit("195px");
            }

            // # Start. ��ƴ�������ź���Ա��

            if (tvReceiver.Nodes.Count > 0)
                ArrangedInAlphabeticalOrder(tvReceiver.Nodes[0]);
            if (tvReader.Nodes.Count > 0)
                ArrangedInAlphabeticalOrder(tvReader.Nodes[0]);
            if (tvAssistor.Nodes.Count > 0)
                ArrangedInAlphabeticalOrder(tvAssistor.Nodes[0]);

            // # End.

            //��ӿͻ��˵������ֶ�
            if (blnSendToDefine.ToLower() == "true")
            {
                //strSendDefineXml = tr.GetAttribute("SendToDefineXml");
                //blnContinue = false;  //�˳�whileѭ��
                //��ӿͻ��˵������ֶ�
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
                    //���� ���� Э�� ��ͨ ����ʱ�������Զ�����
                    blnCanSend = false;
                }

                if (blnCanSend == true)
                {
                    if (strSelType == "1")
                    {
                        Response.Write("<script>alert('û���������Ե��ȵĶ���');</script>");
                    }
                    else
                    {
                        //������ʾ��Ϣ
                        if (lngLinkNodeType == (long)e_FMNodeType.fmEnd)
                        {
                            strMessage = " �����Ѿ����";
                        }

                        if (lngLinkNodeType == (long)e_FMNodeType.fmUnite)
                        {
                            if (strUnionLinkEnd == "True")
                            {
                                strMessage = " ����������ͬһ�������ڵ�����������������ʱ�����̽��Զ����";
                            }
                            else
                            {
                                if (lngCurrPath == lngMasterPath)
                                {
                                    strMessage = " ����������ͬһ�������ڵ�����������������ʱ�����̽��Զ��ύ�� " + strAutoMsg;
                                }
                                else
                                {
                                    strMessage = " ����������ͬһ�������ڵ�����������������ʱ�����̽��Զ��ύ,������Ա������·����������������Ĵ�����Աָ��";
                                }

                            }
                        }

                        if (lngLinkNodeType == (long)e_FMNodeType.fmNormal || lngLinkNodeType == (long)e_FMNodeType.fmDraft)
                        {
                            strMessage = "�����Ѿ��Զ��ķ��͸�" + strAutoMsg;
                        }


                        //ֱ�ӷ���
                        //Response.Write("<script>alert('����ֱ�ӷ���');</script>");

                        //��Ϊ�����ʵ��,����������
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
                        strMessage = "�����Ѿ��ύ������������ͬһ�������ڵ�����������������ʱ�����̽��Զ����͸�ѡ��Ĵ�����Ա";
                        //Response.Write("<script>window.opener.flowSubmit.strMessage.value=" + StringTool.JavaScriptQ(strMessage) +";</script>");
                        //��ʾѡ��ҳ��

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
            long lngCurrNodeID = 0; //��ǰ����ID
            string strCurrNodeName = ""; //��ǰ���ڵ�����

            e_FMNodeType lngCurrNodeType = e_FMNodeType.fmNormal;   //������ʱ��ǰ���ڵ����
            e_ReceiveActorType lngCurrReceiveType; //��ǰ���������


            tv.Nodes.Clear();
            XmlTextReader tr = new XmlTextReader(new StringReader(strXml));

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<Receivers></Receivers>");

            bool blnContinue = true;
            while (tr.Read() && blnContinue)
            {
                if (tr.NodeType == XmlNodeType.Element)
                {
                    //ֻ����Ԫ�ز��ּ��ɣ���Ϊ���ص�XML�����ݼ�����Ԫ�ص�������,����˳�����TreeView�ڵ�
                    switch (tr.Name)
                    {
                        case "NextReceivers":
                            //��Ӹ��ڵ㣨���ڵ㣩����������������������
                            ParentNode = new CustomTreeNode();
                            if (strType == "Assist")
                            {
                                ParentNode.Text = "��Աѡ��";
                                if (intSpecRightType == e_SpecRightType.esrtNormal)
                                {
                                    ParentNode.Text = "Э����Աѡ��";
                                }
                                ParentNode.ImageUrl = "../Images/Flow/27.bmp";
                                ParentNode.Value = "Assists_" + lngCurrNodeID.ToString();
                            }
                            else
                            {
                                if (intSpecRightType == e_SpecRightType.esrtNormal)
                                {
                                    ParentNode.Text = "��֪��Աѡ��";
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
                            //�������û�����ƣ�������ȱʡֵ
                            if (strtmpName.Trim() == "")
                                strtmpName = "����" + lngtmpID.ToString();
                            strCurrNodeName = strtmpName;
                            ///�жϵ�ǰ�����Ƿ������죬�����һ������û�����죬����������ִ��
                            inttmpCount = int.Parse(tr.GetAttribute("WorkerCount"));
                            if (inttmpCount == 0)
                            {
                                UIGlobal.MsgBox(this, "�ڻ��ڣ�" + StringTool.ParseForResponse(strtmpName) + " û�з��ֿ��Խ��к����������Ա����������������Ա��ϵ��");
                                Response.Write("<script>parent.fraaddNew.cols='0,100%,0';</script>");
                                UIGlobal.SelfClose(this);
                                Response.End();

                            }
                            ///��ӽڵ�
                            RootNode = new CustomTreeNode();
                            RootNode.Text = "���ڣ�" + strtmpName;
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
                            //��ӽڵ�

                            break;
                        case "Reader":
                            lngtmpID = long.Parse(tr.GetAttribute("ID"));
                            strtmpName = tr.GetAttribute("Name");
                            if (strtmpName.Trim() == "")
                                strtmpName = "��ԱID(" + lngtmpID.ToString() + ")";


                            lngCurrReceiveType = (e_ReceiveActorType)(int.Parse(tr.GetAttribute("ReceiveType")));

                            if (lngCurrNodeType == e_FMNodeType.fmOrgNode || lngCurrNodeType == e_FMNodeType.fmDisperse)
                            {
                                //�������ڻ�ɢ����ֻѡ����

                                lngCurrReceiveType = (e_ReceiveActorType)(int.Parse(tr.GetAttribute("ReceiveType")));
                                strDeptPathXml = tr.GetAttribute("DeptPathXml");
                                if (strDeptPathXml == null) strDeptPathXml = "";
                                if (strDeptPathXml.Length > 0)
                                {
                                    //���ڲ������ڵ�����

                                    //��ӽڵ�
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
                                    node.NodeBllType = lngCurrNodeID.ToString() + "_" + ((int)lngCurrNodeType).ToString();  //���ڵı��_�������
                                    node.NodeData = strCurrNodeName;  //���ڵ�����
                                    if (FirstDeptExpanded == false)
                                    {
                                        node.Expanded = true;
                                        //FirstDeptExpanded=true;
                                    }
                                    treeNodeIndex++;
                                    RootNode.ChildNodes.Add(node);

                                    //sbMastPerson.Append("arrMastPerson[" + iMastPerson + "] = new Array(\"" + node.Value + "|" + node.NodeBllType + "|" + node.NodeData + "|" + strtmpName + "\", \"" + strCurrNodeName + "----" + strtmpName + "/�û���" + "\");\n");
                                    String strPersonInfo = " new Array(\"" + node.Value + "|" + node.NodeBllType + "|" + node.NodeData + "|" + strtmpName + "\", \"" + strCurrNodeName + "----" + strtmpName + "/�û���" + "\");\n";
                                    dictMastPerson.Add(new KeyValuePair<String, String>(strtmpName, strPersonInfo));
                                    //{ name: "�չA", code: "000001",spell:"sfza",id:'1' }
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
                                //��ӽڵ�

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
                                        node.NodeBllType = lngCurrNodeID.ToString() + "_" + ((int)lngCurrNodeType).ToString();  //���ڵı��_�������
                                        node.NodeData = strCurrNodeName;  //���ڵ�����
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

            //*  ���� deptpath�����������Žڵ㣬��������򲻴���

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
                //���һ������·���Ľڵ����Լ������Բ���Ҫ���·��
                for (int i = 0; i < xmlNodes.Count - 1; i++)
                {
                    //�ж��Ƿ����
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
            node.NodeBllType = lngNodeID.ToString() + "_" + ((int)lngCurrNodeType).ToString();  //���ڵı��_�������
            node.NodeData = strNodeName;      //���ڵ�����
            string tmpKey = "";
            if (strType == "Reader")
            {
                node.Value = "Reader_" + lngID.ToString() + "_" + ((int)lngCurrReceiveType).ToString();

                //sbReadPerson.Append("arrReadPerson[" + iReadPerson + "] = new Array(\"" + node.Value + "|" + node.NodeBllType + "|" + node.NodeData + "|" + strName + "\", \"" + strNodeName + "----" + strName + "/����" + "\");\n");
                String strPersonInfo = "  new Array(\"" + node.Value + "|" + node.NodeBllType + "|" + node.NodeData + "|" + strName + "\", \"" + strNodeName + "----" + strName + "/����" + "\");\n";
                dictReadPerson.Add(new KeyValuePair<String, String>(strName, strPersonInfo));

                iReadPerson++;
                tmpKey = "Reader_";
            }
            else if (strType == "Worker")
            {
                node.Value = "Worker_" + lngID.ToString() + "_" + ((int)lngCurrReceiveType).ToString();

                //sbMastPerson.Append("arrMastPerson[" + iMastPerson + "] = new Array(\"" + node.Value + "|" + node.NodeBllType + "|" + node.NodeData + "|" + strName + "\", \"" + strNodeName + "----" + strName + "/����" + "\");\n");
                String strPersonInfo = " new Array(\"" + node.Value + "|" + node.NodeBllType + "|" + node.NodeData + "|" + strName + "\", \"" + strNodeName + "----" + strName + "/����" + "\");\n";
                dictMastPerson.Add(new KeyValuePair<String, String>(strName, strPersonInfo));
                iMastPerson++;
                tmpKey = "Worker";
            }
            else
            {
                //Э��
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

            #region �����ȡ��Աʱû�н���������strDeptPath��ʱΪnull ����ǰ 2014-03-05
            if (string.IsNullOrEmpty(strDeptPath))
            {
                strDeptPath = "<Depts></Depts>";
            }
            #endregion

            //*  ���� deptpath�����������Žڵ㣬��������򲻴���

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
                //�ж��Ƿ����
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
            node.NodeBllType = lngNodeID.ToString() + "_" + ((int)lngCurrNodeType).ToString();  //���ڵı��_�������
            node.NodeData = strNodeName;      //���ڵ�����
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
                String strPersonInfo = " new Array(\"" + node.Value + "|" + node.NodeBllType + "|" + node.NodeData + "|" + strName + "\", \"" + strNodeName + "----" + strName + "/����" + "\");\n";
                dictMastPerson.Add(new KeyValuePair<String, String>(strName, strPersonInfo));


                iMastPerson++;

                tmpKey = "Worker";
            }
            else
            {
                //Э��
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
                                //��ʾ�ҵ�
                                return node;
                            }
                        }
                    }
                }
            }
            return node;
        }



        /// <summary>
        /// ��ƴ�������ź���Ա
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
            // CODEGEN���õ����� ASP.NET Web ���������������ġ�
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




    }



    /// <summary>
    /// ����ĸ˳�������ź���Ա
    /// </summary>
    public class TreeNodeArrangedInAlphabeticalOrderComparer : System.Collections.Generic.IComparer<TreeNode>
    {
        public TreeNodeArrangedInAlphabeticalOrderComparer() { }

        #region IComparer<TreeNode> ��Ա

        public int Compare(TreeNode x, TreeNode y)
        {
            return x.Text.Trim().CompareTo(y.Text.Trim());
        }

        #endregion
    }


    /// <summary>
    /// ����ĸ˳�������ź���Ա
    /// </summary>
    public class KeyValueArrangedInAlphabeticalOrderComparer : System.Collections.Generic.IComparer<KeyValuePair<String, String>>
    {
        public KeyValueArrangedInAlphabeticalOrderComparer() { }

        #region IComparer<TreeNode> ��Ա

        public int Compare(KeyValuePair<String, String> x, KeyValuePair<String, String> y)
        {
            return x.Key.Trim().CompareTo(y.Key.Trim());
        }

        #endregion
    }


}
