<%@ Control Language="C#" AutoEventWireup="true" CodeFile="LastModelsPublication.ascx.cs" Inherits="LastModelsPublication" %>
<asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CellPadding="4"
    DataSourceID="CandleRepositoryDataSource" ForeColor="#333333" GridLines="None">
    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
    <Columns>
        <asp:BoundField DataField="UserName" HeaderText="UserName" SortExpression="UserName" />
        <asp:BoundField DataField="ModelName" HeaderText="Model" SortExpression="ModelName" />
        <asp:BoundField DataField="Date" HeaderText="Date" SortExpression="Date" />
    </Columns>
    <RowStyle BackColor="#EFF3FB" />
    <EditRowStyle BackColor="#2461BF" />
    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
    <AlternatingRowStyle BackColor="White" />
    <EmptyDataTemplate>No models published recently.</EmptyDataTemplate>
</asp:GridView>
<asp:ObjectDataSource ID="CandleRepositoryDataSource" runat="server" SelectMethod="GetLastUpload"
    TypeName="DSLFactory.Candle.Repository.CandleRepositoryController"></asp:ObjectDataSource>
