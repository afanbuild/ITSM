<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Report.aspx.cs" Inherits="Epower.ITSM.Web.Common.Report" %>
<%@ Register Src="../Controls/ControlPageFoot.ascx" TagName="ControlPageFoot" TagPrefix="uc3" %>
<%@ Register Src="../Controls/ControlPageFoot.ascx" TagName="ControlPageFoot" TagPrefix="uc4" %>
<%@ Register Src="../Controls/ctrtitle.ascx" TagName="ctrtitle" TagPrefix="uc1" %>
<%@ Register Src="../Controls/ctrEquCataDropList.ascx" TagName="ctrEquCataDropList"
    TagPrefix="uc2" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<head runat="server">
    <title>无标题页</title>
</head>
<body>
    <form id="form1" runat="server" style="border:0">
    <table  width="100%" border="0" cellpadding="0" cellspacing="0" id="Table1" class="Gridtable" runat="server">
    <tr id="tr2" runat="server">           
            <td style="width:50%">
                <asp:DataGrid ID="refreshPageDataGrid" runat="server" 
                    AutoGenerateColumns="False"  ShowFooter="true">
                    <HeaderStyle BorderColor="#EEEEE0" BorderStyle="Solid" BorderWidth="1px" />
                    <ItemStyle BorderColor="#EEEEE0" BorderStyle="Solid" BorderWidth="1px" />
                    <FooterStyle BorderColor="#EEEEE0" BorderStyle="Solid" BorderWidth="1px" />
                    <Columns>     
                          <asp:TemplateColumn HeaderText="服务级别" ItemStyle-HorizontalAlign="Left">
                            <HeaderStyle Width="8%" Height="40px"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center"  Height="40px"/>
                            <ItemTemplate>
                                <asp:Label ID="lblMiscellaneouse" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Miscellaneous")%>'></asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="lblLevelNameFoot" runat="server" Text='合计：'></asp:Label>
                            </FooterTemplate>
                        </asp:TemplateColumn>                                    
                       <asp:TemplateColumn HeaderText="处理中数量" ItemStyle-HorizontalAlign="Left">
                            <HeaderStyle Width="8%" Height="40px"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center"  Height="40px"/>
                            <ItemTemplate>
                                <asp:Label ID="lblProcessing" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Processing")%>'></asp:Label>
                                  <asp:Label ID="ProcessingPercentage" runat="server"  ForeColor="Blue" Text='<%# DisplayValue(Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "ProcessingPercentage")))%>'></asp:Label>
                                
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="ProcessingFoot" runat="server" Text='合计：'></asp:Label>
                            </FooterTemplate>
                        </asp:TemplateColumn>   
                                        <asp:TemplateColumn HeaderText="未解决数量" ItemStyle-HorizontalAlign="Left">
                            <HeaderStyle Width="8%" Height="40px"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center"  Height="40px"/>
                            <ItemTemplate>
                                <asp:Label ID="lblNoResponse" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.NoResponse")%>'></asp:Label>
                                <asp:Label ID="NoResponsePercentage" runat="server"  ForeColor="Blue" Text='<%# DisplayValue(Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "NoResponsePercentage")))%>'></asp:Label>

                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="NoResponseFoot" runat="server" Text='合计：'></asp:Label>
                            </FooterTemplate>
                        </asp:TemplateColumn>   
                            <asp:TemplateColumn HeaderText="已解决数量" ItemStyle-HorizontalAlign="Left">
                            <HeaderStyle Width="8%" Height="40px"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center"  Height="40px"/>
                            <ItemTemplate>
                                <asp:Label ID="lblresolved" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.resolved")%>'></asp:Label>
                                 <asp:Label ID="resolvedPercentage" runat="server" ForeColor="Blue" Text='<%# DisplayValue(Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "resolvedPercentage")))%>'></asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="resolvedFoot" runat="server" Text='合计：'></asp:Label>
                            </FooterTemplate>
                        </asp:TemplateColumn>   
                        
                        
                                     <asp:TemplateColumn HeaderText="当月新增数量" ItemStyle-HorizontalAlign="Left">
                            <HeaderStyle Width="8%" Height="40px"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center"  Height="40px"/>
                            <ItemTemplate>
                                <asp:Label ID="lblMonthNum" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.MonthNum")%>'></asp:Label>
                                <asp:Label ID="MonthNumPercentage" runat="server" ForeColor="Blue" Text='<%# DisplayValue(Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "MonthNumPercentage")))%>'></asp:Label>

                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="MonthNumFoot" runat="server" Text='合计：'></asp:Label>
                            </FooterTemplate>
                        </asp:TemplateColumn>   
                                        
                         <asp:TemplateColumn HeaderText="今日新增数量" ItemStyle-HorizontalAlign="Left">
                            <HeaderStyle Width="8%" Height="40px"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center"  Height="40px"/>
                            <ItemTemplate>
                                <asp:Label ID="lblDateNum" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.DateNum")%>'></asp:Label>
                                <asp:Label ID="DateNumPercentage" runat="server" ForeColor="Blue" Text='<%# DisplayValue(Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "DateNumPercentage")))%>'></asp:Label>

                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="DateNumFoot" runat="server" Text='合计：'></asp:Label>
                            </FooterTemplate>
                        </asp:TemplateColumn>                              
                    </Columns>
                </asp:DataGrid>
            </td>  
            <td style="width:1%">
            </td>
               <td style="width:50%">
                <asp:DataGrid ID="_YearDataGrid" runat="server"  ShowFooter="true"
                    AutoGenerateColumns="False"  
                    >
                    <HeaderStyle BorderColor="#EEEEE0" BorderStyle="Solid" BorderWidth="1px" />
                    <ItemStyle BorderColor="#EEEEE0" BorderStyle="Solid" BorderWidth="1px" />
                    <FooterStyle BorderColor="#EEEEE0" BorderStyle="Solid" BorderWidth="1px" />                    
                    <Columns>          
                    
                              <asp:TemplateColumn HeaderText="服务级别" ItemStyle-HorizontalAlign="Left">
                            <HeaderStyle Width="8%" Height="40px"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center"  Height="40px"/>
                            <ItemTemplate>
                                <asp:Label ID="lblMiscellaneouse" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Miscellaneous")%>'></asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="lblLevelNameFoot" runat="server" Text='合计：'></asp:Label>
                            </FooterTemplate>
                        </asp:TemplateColumn>             
                        
                       <asp:TemplateColumn HeaderText="全年事件量" ItemStyle-HorizontalAlign="Left">
                            <HeaderStyle Width="8%" Height="40px"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center"  Height="40px"/>
                            <ItemTemplate>
                                <asp:Label ID="lblYearNum" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.yearNum")%>'></asp:Label>
                               <asp:Label ID="lblYearNumPercentage" runat="server" ForeColor="Blue" Text='<%# DisplayValue(Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "yearNumPercentage")))%>'></asp:Label>

                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="lblYearNumFoot" runat="server" Text=''></asp:Label>
                            </FooterTemplate>
                        </asp:TemplateColumn>   
                        
                                  <asp:TemplateColumn HeaderText="全年已完成" ItemStyle-HorizontalAlign="Left">
                            <HeaderStyle Width="8%" Height="40px"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center"  Height="40px"/>
                            <ItemTemplate>
                                <asp:Label ID="lblYearAchievements" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.yearAchievements")%>'></asp:Label>
                               <asp:Label ID="lblyearPercentage" runat="server" ForeColor="Blue" Text='<%# DisplayValue(Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "yearPercentage")))%>'></asp:Label>

                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="lblYearAchievementsFoot" runat="server" Text=''></asp:Label>
                            </FooterTemplate>
                        </asp:TemplateColumn>   
                                  <asp:TemplateColumn HeaderText="当季事件量" ItemStyle-HorizontalAlign="Left">
                            <HeaderStyle Width="8%" Height="40px"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center"  Height="40px"/>
                            <ItemTemplate>
                                <asp:Label ID="lblQuarter" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.quarter")%>'></asp:Label>
                                <asp:Label ID="lblquarterPercentage" runat="server" ForeColor="Blue" Text='<%# DisplayValue(Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "quarterPercentage")))%>'></asp:Label>

                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="lblQuarterFoot" runat="server" Text=''></asp:Label>
                            </FooterTemplate>
                        </asp:TemplateColumn>                      
                        
                                  <asp:TemplateColumn HeaderText="当季已完成" ItemStyle-HorizontalAlign="Left">
                            <HeaderStyle Width="8%" Height="40px"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center"  Height="40px"/>
                            <ItemTemplate>
                                <asp:Label ID="lblQuarterAchievements" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.quarterAchievements")%>'></asp:Label>
                                <asp:Label ID="lblAchievementsPercentage" runat="server" ForeColor="Blue" Text='<%# DisplayValue(Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "AchievementsPercentage")))%>'></asp:Label>

                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="lblQuarterAchievementsFoot" runat="server" Text=''></asp:Label>
                            </FooterTemplate>
                        </asp:TemplateColumn>   
                        
                                  <asp:TemplateColumn HeaderText="当月事件量" ItemStyle-HorizontalAlign="Left">
                            <HeaderStyle Width="8%" Height="40px"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center"  Height="40px"/>
                            <ItemTemplate>
                                <asp:Label ID="lblMonthNum" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.monthNum")%>'></asp:Label>
                                <asp:Label ID="lblmonthNumPercentage" runat="server"  ForeColor="Blue" Text='<%# DisplayValue(Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "monthNumPercentage")))%>'></asp:Label>

                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="lblMonthNumFoot" runat="server" Text=''></asp:Label>
                            </FooterTemplate>
                        </asp:TemplateColumn>   
                        
                                  <asp:TemplateColumn HeaderText="当月已完成" ItemStyle-HorizontalAlign="Left">
                            <HeaderStyle Width="8%" Height="40px"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center"  Height="40px"/>
                            <ItemTemplate>
                                <asp:Label ID="lblMonthAchievements" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.monthAchievements")%>'></asp:Label>
                                <asp:Label ID="lblmonthAchievementsPercentage" runat="server" ForeColor="Blue" Text='<%# DisplayValue(Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "monthAchievementsPercentage")))%>'></asp:Label>

                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="lblMonthAchievementsFoot" runat="server" Text='合计：'></asp:Label>
                            </FooterTemplate>
                        </asp:TemplateColumn>                           
                    </Columns>
                </asp:DataGrid>         
            
            </td>
            </tr>
            </table>
    </form>
</body>
</html>
