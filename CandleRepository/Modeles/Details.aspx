<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Details.aspx.cs" Inherits="Modeles_Details" Title="Untitled Page" %>

<%@ Register Src="PublicContractsControl.ascx" TagName="PublicContractsControl" TagPrefix="uc1" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Body" Runat="Server">
<script language="JavaScript" type="text/javascript" src="<%=ResolveUrl("~/scripts/svgcheck.js")%>"></script>
<script language="VBScript" src="<%=ResolveUrl("~/scripts/svgcheck.vbs")%>" type="text/vbscript"></script>
<script language="JavaScript"><!--
checkAndGetSVGViewer();
// -->
</script>
    <cc1:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </cc1:ToolkitScriptManager>
    <asp:SiteMapPath ID="SiteMapPath1" runat="server" Font-Names="Verdana" Font-Size="0.8em"
        PathSeparator=" : ">
        <PathSeparatorStyle Font-Bold="True" ForeColor="#507CD1" />
        <CurrentNodeStyle ForeColor="#333333" />
        <NodeStyle Font-Bold="True" ForeColor="#284E98" />
        <RootNodeStyle Font-Bold="True" ForeColor="#507CD1" />
    </asp:SiteMapPath>
    <cc1:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0"  >
        <cc1:TabPanel ID="tabDetails" runat="server" HeaderText="Details">
            <ContentTemplate>
            <div class="DetailsContainer">
                <asp:DetailsView ID="DetailsView1" runat="server" AutoGenerateRows="False" DataSourceID="ModelDataSource"
                    Height="50px" Width="100%" BackColor="White" BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px" CellPadding="3" GridLines="Horizontal">
                    <Fields>
                        <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
                        <asp:BoundField DataField="Path" HeaderText="Path" SortExpression="Path" />
                        <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" />
                        <asp:BoundField DataField="Version" HeaderText="Version" SortExpression="Version" />
                        <asp:BoundField DataField="ModelFileName" HeaderText="Model file name" SortExpression="ModelFileName" />
                        <asp:BoundField DataField="DocUrl" HeaderText="Documentation Url" SortExpression="DocUrl" />
                        <asp:BoundField DataField="ServerVersion" HeaderText="Model version number" SortExpression="ServerVersion" />
                        <asp:BoundField DataField="TestBaseAddress" HeaderText="Test base address" SortExpression="TestBaseAddress" />
                        <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description" >
                            <ItemStyle VerticalAlign="Top" />
                        </asp:BoundField>
                    </Fields>
                    <FooterStyle BackColor="#B5C7DE" ForeColor="#4A3C8C" />
                    <EditRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="#F7F7F7" />
                    <RowStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" />
                    <PagerStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" HorizontalAlign="Right" />
                    <HeaderStyle BackColor="#4A3C8C" Font-Bold="True" ForeColor="#F7F7F7" />
                    <AlternatingRowStyle BackColor="#F7F7F7" />
                </asp:DetailsView>
                <asp:ObjectDataSource ID="ModelDataSource" runat="server" SelectMethod="GetMetadata"
                    TypeName="DSLFactory.Candle.Repository.CandleRepositoryController" >
                    <SelectParameters>
                        <asp:QueryStringParameter Name="id" QueryStringField="Id" Type="String" />
                        <asp:QueryStringParameter Name="version" QueryStringField="version" Type="String" />
                    </SelectParameters>
                </asp:ObjectDataSource>
                &nbsp;
            </div>
            </ContentTemplate>
        </cc1:TabPanel>
        <cc1:TabPanel ID="tabDependencies" runat="server" HeaderText="Dependances">
        <ContentTemplate><div class="DetailsDependencies">
          <script language="JavaScript"><!--
emitSVG('src="DependenciesGraph.aspx?<%=Request.QueryString.ToString() %>" name="SVGEmbed" height="1000" width="1000" type="image/svg-xml"');
// -->
</script>
<noscript>
<embed src="DependenciesGraph.aspx?<%=Request.QueryString.ToString() %>" name="SVGEmbed" height="1000" width="1000" type="image/svg-xml"
pluginspage="http://www.adobe.com/svg/viewer/install/">
</noscript></div>
        </ContentTemplate>
        </cc1:TabPanel>
        <cc1:TabPanel ID="tabArtifacts" runat="server" HeaderText="Artifacts">
        <ContentTemplate>
                    <div class="DetailsContainer">
                        <asp:ListBox ID="lstArtifacts" runat="server" Height="100%" Width="100%"></asp:ListBox></div>
        </ContentTemplate>
        </cc1:TabPanel>
        <cc1:TabPanel ID="tabDoc" runat="server" HeaderText="Documentation">
        <ContentTemplate>
                    <asp:Label runat="server" ID="pnlDoc" />
        </ContentTemplate>
        </cc1:TabPanel>
        <cc1:TabPanel ID="TabPanel1" runat="server" HeaderText="History">
            <ContentTemplate><div class="DetailsContainer">                
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CellPadding="4"
                    DataSourceID="CandleRepositoryDataSource" ForeColor="#333333" GridLines="None">
                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <Columns>
                        <asp:BoundField DataField="UserName" HeaderText="User Name" SortExpression="UserName">
                            <ItemStyle Width="300px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Date" HeaderText="Date" SortExpression="Date">
                            <ItemStyle Width="200px" />
                        </asp:BoundField>
                    </Columns>
                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                    <EditRowStyle BackColor="#999999" />
                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                    <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                </asp:GridView>
                <asp:ObjectDataSource ID="CandleRepositoryDataSource" runat="server" SelectMethod="GetModelHistoric"
                    TypeName="DSLFactory.Candle.Repository.CandleRepositoryController">
                    <SelectParameters>
                        <asp:QueryStringParameter Name="modelId" QueryStringField="Id" Type="Object" />
                        <asp:QueryStringParameter Name="version" QueryStringField="version" Type="Object" />
                    </SelectParameters>
                </asp:ObjectDataSource></div>
            </ContentTemplate>
        </cc1:TabPanel>
        <cc1:TabPanel ID="TabPanel2" runat="server" HeaderText="Contracts">
            <ContentTemplate>
                    <div class="DetailsContainer">
                <uc1:PublicContractsControl ID="PublicContractsControl1" runat="server" />
                </div> 
            </ContentTemplate>
        </cc1:TabPanel>
    </cc1:TabContainer>
</asp:Content>

